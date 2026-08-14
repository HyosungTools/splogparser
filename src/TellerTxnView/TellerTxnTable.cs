using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace TellerTxnView
{
   /// <summary>
   /// Reconstructs teller-assisted transactions in the Analyze phase by reading the data tables
   /// other views already wrote to WorkFolder\&lt;View&gt;.xml. Reads DataRows only (no log lines),
   /// so it depends on no line classes. Populates two of its own tables:
   ///   TellerTransactions - one row per teller transaction (any type: withdrawal, cash/check
   ///                        deposit, check cashing, transfer, ...).
   ///   TellerDevice       - one row per Nextware Open/Close session event (good AND fault), so the
   ///                        support tech sees the device sessions that succeeded as well as failed.
   /// </summary>
   class TellerTxnTable : BaseTable
   {
      public TellerTxnTable(IContext ctx, string viewName) : base(ctx, viewName) { }

      /// <summary>Analyze-only view: no per-line processing.</summary>
      public override void ProcessRow(ILogLine logLine) { }

      private class Txn
      {
         public string Start = "";
         public string End = "";
         public string Asset = "";
         public string TellerId = "";
         public string TellerName = "";
         public string Customer = "";
         public string Sess = "";

         // type resolution: TransactionType (authoritative but often blank on teller rows) with a
         // fallback to the dominant RemoteControl task name.
         public string TxnType = "";                        // e.g. Withdrawal, CheckDeposit, CashDeposit
         public readonly Dictionary<string, int> Tasks = new Dictionary<string, int>();

         // money (raw, as captured - comma-joined lists for multi-item transactions)
         public string CheckAmount = "";     // Checks_Amount
         public string CheckScore = "";      // Checks_AmountScore
         public string CashAmount = "";      // CashDetails_Amount (cash in for a deposit)
         public string CurrencyItems = "";   // CurrencyItems_Summary (denomination breakdown)
         public string AcctType = "";        // Accounts_AccountType (Checking/Savings)
         public string AcctAmount = "";      // Accounts_Amount (posted to account)
         public string ReviewAmount = "";    // Review_TellerAmount
         public string TellerApproval = "";  // Review_TellerApproval (ApprovalType enum)
         public string ReviewReason = "";    // Review_ReasonForReview (ReviewReason [Flags] enum)

         // enriched from sibling views
         public string CashDispensed = "";
         public string CheckDisposition = "";

         public readonly List<string> Faults = new List<string>();
      }

      // ---- small helpers ----
      private static string Col(DataRow r, string name)
      {
         if (!r.Table.Columns.Contains(name)) return "";
         object v = r[name];
         return v == null || v == DBNull.Value ? "" : v.ToString().Trim();
      }

      private static DataTable Get(Dictionary<string, DataTable> tables, string name)
      {
         return tables != null && tables.TryGetValue(name, out DataTable t) ? t : null;
      }

      private static DataRow[] SortedByTime(DataTable t)
      {
         if (t == null) return new DataRow[0];
         try { return t.Select("", "time ASC"); }
         catch { return t.Select(); }
      }

      /// <summary>
      /// Reconstruct from the source tables the VIEW loaded and handed us (keyed by table name).
      /// This class does not read files or know which views to pull from - that is the view's job.
      /// </summary>
      public void Build(IContext ctx, Dictionary<string, DataTable> tables)
      {
         DataTable moni = Get(tables, "MoniPlus2sEvents");
         DataTable nextware = Get(tables, "NextwareEvents");
         DataTable over = Get(tables, "OverSummary");   // guarded (optional)
         DataTable ipm = Get(tables, "IPMDeposit");     // guarded (optional)

         if (moni == null)
         {
            ctx.ConsoleWriteLogLine("TellerTxn: MoniPlus2sEvents not loaded - run with -e * so AEView is parsed.");
            return;
         }

         List<Txn> txns = ReconstructTransactions(moni);
         ProcessDeviceSessions(nextware, txns);
         EnrichDispense(over, txns);
         EnrichCheckDisposition(ipm, txns);
         WriteRows(ctx, txns);
      }

      private List<Txn> ReconstructTransactions(DataTable moni)
      {
         List<Txn> txns = new List<Txn>();
         Txn cur = null;

         foreach (DataRow r in SortedByTime(moni))
         {
            string rest = Col(r, "RestResource");
            string ts = Col(r, "time");
            string sess = Col(r, "TellerSession_Id");

            // start of a teller-assisted transaction (teller assigned).
            // Each Txn is added to the list at creation, so closing is just cur = null.
            if (rest == "TellerSession" && sess != "" && sess != "0")
            {
               if (cur != null && cur.Sess != "" && cur.Sess != sess) cur = null;   // close previous (already in list)
               if (cur == null) { cur = new Txn { Start = ts, End = ts }; txns.Add(cur); }
               cur.Sess = sess;
               if (Col(r, "TellerId") != "") cur.TellerId = Col(r, "TellerId");
               if (Col(r, "TellerName") != "") cur.TellerName = Col(r, "TellerName");
            }

            if (cur == null) continue;

            if (Col(r, "AssetName") != "") cur.Asset = Col(r, "AssetName");
            if (Col(r, "CustomerId") != "") cur.Customer = Col(r, "CustomerId");
            if (ts != "") { cur.End = ts; if (cur.Start == "") cur.Start = ts; }

            // ---- type: TransactionType is authoritative when present ----
            string tt = Col(r, "TransactionType");
            if (tt != "" && cur.TxnType == "") cur.TxnType = tt;

            // ---- type fallback: tally the meaningful RemoteControl tasks in this session ----
            string task = BusinessTask(Col(r, "RemoteControl_TaskName"));
            if (task != "")
            {
               cur.Tasks.TryGetValue(task, out int n);
               cur.Tasks[task] = n + 1;
            }

            // ---- money: capture the first non-empty of each on any row in the window ----
            CaptureFirst(r, "Checks_Amount",      ref cur.CheckAmount);
            CaptureFirst(r, "Checks_AmountScore", ref cur.CheckScore);
            CaptureFirst(r, "CashDetails_Amount", ref cur.CashAmount);
            CaptureFirst(r, "CurrencyItems_Summary", ref cur.CurrencyItems);
            CaptureFirst(r, "Accounts_AccountType", ref cur.AcctType);
            CaptureFirst(r, "Accounts_Amount",    ref cur.AcctAmount);

            if (rest == "TransactionReviewMessage")
            {
               CaptureFirst(r, "Review_TellerAmount",    ref cur.ReviewAmount);
               CaptureFirst(r, "Review_TellerApproval",  ref cur.TellerApproval);
               CaptureFirst(r, "Review_ReasonForReview", ref cur.ReviewReason);
            }

            if (rest == "TellerSessionRequest" && Col(r, "HttpRequest") == "DELETE")
            {
               cur = null;   // transaction complete
            }
         }
         return txns;
      }

      private static void CaptureFirst(DataRow r, string col, ref string dest)
      {
         if (dest != "") return;
         string v = Col(r, col);
         if (v != "") dest = v;
      }

      /// <summary>
      /// Emit every Nextware Open/Close session event - good and bad - to the TellerDevice table so
      /// the tech can see which device sessions succeeded as well as which failed. Only FAILED/ERROR
      /// events are pinned to a transaction's summary line.
      /// </summary>
      private void ProcessDeviceSessions(DataTable nextware, List<Txn> txns)
      {
         if (nextware == null) return;
         foreach (DataRow r in SortedByTime(nextware))
         {
            string chg = Col(r, "MonitoringDeviceChanges");
            if (chg.IndexOf("SESSION", StringComparison.OrdinalIgnoreCase) < 0) continue;   // session events only

            string ts = Col(r, "time");
            string dev = Col(r, "MonitoringDeviceName");
            if (dev == "") dev = "?";
            string elapsed = Col(r, "MonitoringElapsed");

            bool fault = chg.IndexOf("FAILED", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         chg.IndexOf("ERROR", StringComparison.OrdinalIgnoreCase) >= 0;
            string status = fault ? "FAULT" : "OK";

            AddDeviceRow(ts, dev, chg, status, elapsed);

            if (!fault) continue;
            foreach (Txn t in txns)
               if (t.Start != "" && string.CompareOrdinal(ts, t.Start) >= 0 &&
                   string.CompareOrdinal(ts, t.End) <= 0 && t.Faults.Count < 20)
               { t.Faults.Add($"{dev}: {chg}"); break; }
         }
      }

      private void EnrichDispense(DataTable over, List<Txn> txns)
      {
         if (over == null) return;
         foreach (Txn t in txns)
         {
            bool dispensed = false, denominated = false;
            foreach (DataRow r in over.Select())
            {
               string ts = Col(r, "time");
               if (t.Start == "" || string.CompareOrdinal(ts, t.Start) < 0 || string.CompareOrdinal(ts, t.End) > 0) continue;
               string d = Col(r, "dispensed").ToLowerInvariant();
               if (d.Contains("dispensed") || d.Contains("presented") || d.Contains("taken")) dispensed = true;
               else if (d.Contains("denominated")) denominated = true;
            }
            t.CashDispensed = dispensed ? "dispensed" : (denominated ? "denominated only (no dispense)" : "");
         }
      }

      private void EnrichCheckDisposition(DataTable ipm, List<Txn> txns)
      {
         if (ipm == null) return;
         foreach (Txn t in txns)
         {
            bool rollback = false, taken = false, accepted = false;
            foreach (DataRow r in ipm.Select())
            {
               string ts = Col(r, "time");
               if (t.Start == "" || string.CompareOrdinal(ts, t.Start) < 0 || string.CompareOrdinal(ts, t.End) > 0) continue;
               string trans = Col(r, "trans").ToLowerInvariant();
               if (trans.Contains("rollback")) rollback = true;
               if (trans.Contains("taken")) taken = true;
               if (trans.Contains("accepted") || trans.Contains("active")) accepted = true;
            }
            if (rollback || taken) t.CheckDisposition = "returned (rollback)";
            else if (accepted) t.CheckDisposition = "accepted";
         }
      }

      private void WriteRows(IContext ctx, List<Txn> txns)
      {
         foreach (Txn t in txns)
         {
            try
            {
               DataRow r = dTableSet.Tables["TellerTransactions"].Rows.Add();
               r["file"] = "AEView";
               r["time"] = t.Start;
               r["endtime"] = t.End;
               r["duration"] = DurationSecs(t.Start, t.End);
               r["asset"] = t.Asset;
               r["teller"] = string.IsNullOrEmpty(t.TellerName) ? t.TellerId : $"{t.TellerName} ({t.TellerId})";
               r["customer"] = t.Customer;
               r["type"] = ResolveType(t);
               r["amount"] = HeadlineAmount(t);
               r["detail"] = Detail(t);
               r["tellerApproval"] = ApprovalLabel(t.TellerApproval);
               r["reviewReason"] = ReviewReasonLabel(t.ReviewReason);
               r["cashDispensed"] = t.CashDispensed;
               r["checkDisposition"] = t.CheckDisposition;
               r["deviceFault"] = string.Join(" | ", t.Faults);
               r["outcome"] = Outcome(t);
               dTableSet.Tables["TellerTransactions"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("TellerTxn add row exception: " + e.Message);
            }
         }
         ctx.ConsoleWriteLogLine($"TellerTxnView: {txns.Count} teller transactions, "
            + $"{dTableSet.Tables["TellerDevice"].Rows.Count} device session events.");
      }

      private void AddDeviceRow(string ts, string device, string action, string status, string elapsed)
      {
         try
         {
            DataRow r = dTableSet.Tables["TellerDevice"].Rows.Add();
            r["file"] = "AEView";
            r["time"] = ts;
            r["device"] = device;
            r["action"] = action;
            r["status"] = status;
            r["elapsed"] = elapsed;
            dTableSet.Tables["TellerDevice"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("TellerTxn add device exception: " + e.Message);
         }
      }

      // ---- type ----

      /// <summary>Keep only the meaningful teller tasks; drop the plumbing tasks that every
      /// session has (configuration query, customer input prompts, receipt printing, id scan).</summary>
      private static string BusinessTask(string taskName)
      {
         if (string.IsNullOrEmpty(taskName)) return "";
         if (taskName.IndexOf("Configuration", StringComparison.OrdinalIgnoreCase) >= 0) return "";
         if (taskName.IndexOf("CustomerInput", StringComparison.OrdinalIgnoreCase) >= 0) return "";
         if (taskName.IndexOf("PrintReceipt", StringComparison.OrdinalIgnoreCase) >= 0) return "";
         if (taskName.IndexOf("ScanId", StringComparison.OrdinalIgnoreCase) >= 0) return "";
         return taskName.EndsWith("Task") ? taskName.Substring(0, taskName.Length - 4) : taskName;
      }

      /// <summary>Friendly transaction type. Prefer the authoritative TransactionType; if it is blank
      /// (common on teller rows) fall back to the dominant business task in the session.</summary>
      private static string ResolveType(Txn t)
      {
         string raw = t.TxnType != "" ? t.TxnType : DominantTask(t);
         if (raw == "") return "(teller session)";

         switch (raw)
         {
            case "Withdrawal":       return "Cash Withdrawal";
            case "FastCash":         return "Fast Cash (withdrawal)";
            case "Dispense":         return "Cash Withdrawal";
            case "CashDeposit":      return "Cash Deposit";
            case "CheckDeposit":     return "Check Deposit";
            case "CheckCashing":     return "Check Cashing";
            case "Deposit":          return "Deposit";
            case "DepositTBD":       return "Deposit";
            case "Transfer":         return "Transfer";
            case "BalanceInquiry":   return "Balance Inquiry";
            case "Settlement":       return "Settlement";
            default:                 return raw;
         }
      }

      private static string DominantTask(Txn t)
      {
         string best = ""; int bestN = 0;
         foreach (var kv in t.Tasks)
            if (kv.Value > bestN) { bestN = kv.Value; best = kv.Key; }
         return best;
      }

      private static bool IsPayout(string type)
      {
         return type == "Cash Withdrawal" || type == "Fast Cash (withdrawal)" || type == "Check Cashing";
      }

      private static bool IsDeposit(string type)
      {
         return type == "Cash Deposit" || type == "Check Deposit" || type == "Deposit";
      }

      // ---- money ----

      /// <summary>Headline amount for the row: pick the money field that fits the type, formatted as
      /// currency. Amount fields are comma-joined lists (one entry per item), so each is summed.</summary>
      private static string HeadlineAmount(Txn t)
      {
         string type = ResolveType(t);
         long check = SumCents(t.CheckAmount);
         long cash = SumCents(t.CashAmount);
         long acct = SumCents(t.AcctAmount);
         long review = SumCents(t.ReviewAmount);

         long chosen;
         if (IsPayout(type))            chosen = FirstNonZero(check, review, acct);
         else if (IsDeposit(type))      chosen = FirstNonZero(check + cash, acct, review);
         else                           chosen = FirstNonZero(acct, check, cash, review);

         return chosen > 0 ? FormatCentsLong(chosen) : "";
      }

      /// <summary>Human breakdown of the items involved, so nothing is hidden behind the headline.</summary>
      private static string Detail(Txn t)
      {
         List<string> parts = new List<string>();

         string checks = FormatCentsList(t.CheckAmount);
         if (checks != "")
         {
            string score = t.CheckScore != "" ? $" (score {t.CheckScore})" : "";
            parts.Add($"check: {checks}{score}");
         }

         string cash = FormatCentsList(t.CashAmount);
         if (cash != "") parts.Add($"cash in: {cash}");
         if (t.CurrencyItems != "") parts.Add(t.CurrencyItems);

         if (t.AcctType != "" || t.AcctAmount != "")
         {
            string amt = FormatCentsList(t.AcctAmount);
            parts.Add($"{t.AcctType} {amt}".Trim());
         }

         string review = FormatCentsList(t.ReviewAmount);
         if (review != "" && review != checks) parts.Add($"teller amt: {review}");

         return string.Join(" | ", parts);
      }

      private static long FirstNonZero(params long[] vals)
      {
         foreach (long v in vals) if (v > 0) return v;
         return 0;
      }

      /// <summary>Sum the numeric (cent) entries of a comma-joined amount list. Empty/non-numeric
      /// entries (e.g. a leading empty from ",400000,262500") are skipped.</summary>
      private static long SumCents(string commaList)
      {
         if (string.IsNullOrEmpty(commaList)) return 0;
         long sum = 0;
         foreach (string part in commaList.Split(','))
         {
            string p = part.Trim();
            if (p != "" && long.TryParse(p, out long c)) sum += c;
         }
         return sum;
      }

      /// <summary>Format each entry of a comma-joined cent list as currency, e.g.
      /// ",400000,262500" -> "$4,000.00, $2,625.00".</summary>
      private static string FormatCentsList(string commaList)
      {
         if (string.IsNullOrEmpty(commaList)) return "";
         List<string> outp = new List<string>();
         foreach (string part in commaList.Split(','))
         {
            string p = part.Trim();
            if (p == "") continue;
            outp.Add(FormatCents(p));
         }
         return string.Join(", ", outp);
      }

      private static string FormatCents(string cents)
      {
         if (string.IsNullOrEmpty(cents)) return "";
         if (long.TryParse(cents, out long c)) return FormatCentsLong(c);
         return cents;
      }

      private static string FormatCentsLong(long cents)
      {
         return "$" + (cents / 100.0).ToString("N2");
      }

      // ---- outcome / approval ----

      private static string Outcome(Txn t)
      {
         if (t.Faults.Count > 0) return "FAULT: " + t.Faults[0];

         string type = ResolveType(t);

         if (IsPayout(type))
         {
            if (t.CashDispensed == "dispensed") return "Completed (cash dispensed)";
            if (t.CashDispensed.StartsWith("denominated"))
               return t.CheckDisposition.StartsWith("returned")
                  ? "No dispense - check returned"
                  : "No dispense - denominated only";
            if (t.CheckDisposition.StartsWith("returned")) return "Check returned (no dispense)";
            return "(confirm dispense in Over)";
         }

         if (IsDeposit(type))
         {
            if (t.CheckDisposition == "accepted") return "Completed (deposit accepted)";
            if (t.CheckDisposition.StartsWith("returned")) return "Items returned (rollback)";
            return "(confirm deposit in IPM/Over)";
         }

         return "Completed";
      }

      // Review_TellerApproval -> NH.ActiveTeller.Models ApprovalType (0=None,1=Approved,2=Modified,3=Denied).
      // Modified = a successful approval WITH teller-provided changes (e.g. a custom bill mix).
      private static string ApprovalLabel(string raw)
      {
         if (string.IsNullOrEmpty(raw)) return "";
         if (raw.Length == 1) return ApprovalName(raw[0]);
         // multi-item session: single-digit per-item codes concatenated, e.g. "12"
         List<string> parts = new List<string>();
         foreach (char c in raw)
         {
            if (c < '0' || c > '3') return raw;   // not a clean concatenation - show raw
            parts.Add(ApprovalName(c));
         }
         return string.Join(", ", parts);
      }

      private static string ApprovalName(char c)
      {
         switch (c)
         {
            case '0': return "None";
            case '1': return "Approved";
            case '2': return "Modified";
            case '3': return "Denied";
            default:  return c.ToString();
         }
      }

      // Review_ReasonForReview -> ReviewReason [Flags] bitmask (why the item needed teller review).
      private static readonly (long bit, string name)[] ReviewReasons =
      {
         (1,   "AmountEdited"),
         (2,   "AmountOverLimit"),
         (4,   "TotalAmountOverLimit"),
         (8,   "StopCheck"),
         (16,  "CurrencyTransactionReportRequired"),
         (32,  "HasWarning"),
         (64,  "HostOverrideRequested"),
         (128, "InvalidCheck"),
         (256, "Fraud"),
         (512, "SuspectedFraud"),
      };

      private static string ReviewReasonLabel(string raw)
      {
         if (string.IsNullOrEmpty(raw)) return "";
         if (!long.TryParse(raw, out long v)) return raw;   // multi-item concatenation - show raw
         if (v == 0) return "None";
         List<string> parts = new List<string>();
         long known = 0;
         foreach (var rr in ReviewReasons)
         {
            known |= rr.bit;
            if ((v & rr.bit) == rr.bit) parts.Add(rr.name);
         }
         long leftover = v & ~known;
         if (leftover != 0) parts.Add("0x" + leftover.ToString("X"));
         return parts.Count > 0 ? string.Join(" | ", parts) : v.ToString();
      }

      private static string DurationSecs(string start, string end)
      {
         string[] fmts = { "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss" };
         if (DateTime.TryParseExact(start, fmts, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime s) &&
             DateTime.TryParseExact(end, fmts, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime e))
            return ((int)(e - s).TotalSeconds).ToString();
         return "";
      }
   }
}
