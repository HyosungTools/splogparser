using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TxnView
{
   /// <summary>
   /// TxnTable reconstructs transactions from the AP log and emits a cash-in/cash-out ledger.
   ///
   /// Reconstruction logic (verified against real teller-assisted withdrawals):
   ///   - a transaction is bounded by its flow-point arc, keyed off the "fp" prefix
   ///     (Withdrawal / Deposit / FastCash / BalanceInquiry / Transfer);
   ///   - requested amount comes from CurrentTransaction.Amount (cents);
   ///   - cash OUT is the authoritative "Last Dispensed Count A/B/C/D = n" per cassette,
   ///     multiplied by the denomination read from "Dispenser Unit Value" (generic cassettes,
   ///     denominations filled from the log at run time - never hardcoded);
   ///   - "PresentCashAndWaitTaken result is ItemsTaken" / "CASH TAKEN" confirms the customer took it;
   ///   - cash IN (deposits) comes from BillMixTotalAmount + TotalCheckAmount.
   /// State is accumulated across ProcessRow() and flushed in PostProcess().
   /// </summary>
   class TxnTable : BaseTable
   {
      private class Txn
      {
         public string Type = "";
         public string File = "";
         public DateTime Start;
         public DateTime End;
         public bool Teller = false;
         public long RequestedCents = 0;
         public long CashInCents = 0;
         public string Account = "";
         public readonly Dictionary<string, int> Counts = new Dictionary<string, int>();  // cassette letter -> notes dispensed
         public bool DispenseOk = false;
         public bool Taken = false;
         public bool Retract = false;
         public bool Closed = false;
         public readonly List<string> Faults = new List<string>();
      }

      private readonly List<Txn> _txns = new List<Txn>();
      private Txn _active = null;
      private readonly List<int> _unitValues = new List<int>();   // cassette denominations, in A,B,C,D order (from the log)
      private string _file = "";
      private DateTime _lastTs = DateTime.MinValue;
      private bool _haveTs = false;

      private static readonly string[] TYPES = { "Withdrawal", "Deposit", "FastCash", "BalanceInquiry", "Transfer" };
      private static readonly string[] CASSETTES = { "A", "B", "C", "D" };

      // APLine.tsTimestamp() only replaces the LAST '-' with '.', so the date keeps its dashes:
      // "2026-07-10 23:07:08.782"  ->  format uses dashes for the date, dot for milliseconds.
      private static readonly string[] TS_FORMATS = { "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss" };

      private static readonly Regex reFp        = new Regex("\"fp\": \"(?<fp>[^\"]+)\"", RegexOptions.Compiled);
      private static readonly Regex reAmount    = new Regex("\"Amount\": (?<a>\\d+)", RegexOptions.Compiled);
      private static readonly Regex reAccount   = new Regex("\"AccountNumber\": \"(?<acct>[^\"]*)\"", RegexOptions.Compiled);
      private static readonly Regex reBillMix   = new Regex("\"BillMixTotalAmount\": (?<a>\\d+)", RegexOptions.Compiled);
      private static readonly Regex reCheckAmt  = new Regex("\"TotalCheckAmount\": (?<a>\\d+)", RegexOptions.Compiled);
      private static readonly Regex reUnitValue = new Regex("Dispenser Unit Value:\\s*(?<v>\\d+)", RegexOptions.Compiled);
      private static readonly Regex reDispCount = new Regex("Last Dispensed Count (?<c>[A-D]) = (?<n>\\d+)", RegexOptions.Compiled);

      public TxnTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      public override bool WriteExcelFile()
      {
         return base.WriteExcelFile();
      }

      public override void ProcessRow(ILogLine logLine)
      {
         APLine ap = logLine as APLine;
         if (ap == null) return;

         string text = ap.logLine ?? string.Empty;
         if (!string.IsNullOrEmpty(ap.LogFile)) _file = ap.LogFile;

         DateTime parsed;
         if (ap.IsValidTimestamp &&
             DateTime.TryParseExact(ap.Timestamp, TS_FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
         {
            _lastTs = parsed;
            _haveTs = true;
         }
         DateTime now = _haveTs ? _lastTs : DateTime.MinValue;

         // cassette denominations - first full set of "Dispenser Unit Value" lines, in order = A,B,C,D
         Match mv = reUnitValue.Match(text);
         if (mv.Success && _unitValues.Count < CASSETTES.Length)
         {
            _unitValues.Add(int.Parse(mv.Groups["v"].Value, CultureInfo.InvariantCulture));
         }

         // flow point - starts / switches the active transaction and carries requested amount + account
         Match mfp = reFp.Match(text);
         if (mfp.Success)
         {
            string prefix = mfp.Groups["fp"].Value.Split('-')[0];
            if (Array.IndexOf(TYPES, prefix) >= 0)
            {
               if (_active == null || _active.Type != prefix || _active.Closed)
               {
                  _active = new Txn { Type = prefix, Start = now, End = now, File = _file };
                  _txns.Add(_active);
               }
            }
            if (_active != null && !_active.Closed)
            {
               Match ma = reAmount.Match(text);
               if (ma.Success)
               {
                  long a = long.Parse(ma.Groups["a"].Value, CultureInfo.InvariantCulture);
                  if (a > _active.RequestedCents) _active.RequestedCents = a;
               }
               Match mac = reAccount.Match(text);
               if (mac.Success && !string.IsNullOrEmpty(mac.Groups["acct"].Value)) _active.Account = mac.Groups["acct"].Value;
               _active.End = now;
            }
         }

         if (_active == null || _active.Closed) return;
         Txn t = _active;

         if (text.IndexOf("ActiveTeller", StringComparison.Ordinal) >= 0) t.Teller = true;

         // deposit cash-in
         Match mb = reBillMix.Match(text);
         if (mb.Success) { long v = long.Parse(mb.Groups["a"].Value, CultureInfo.InvariantCulture); if (v > t.CashInCents) t.CashInCents = v; }
         Match mck = reCheckAmt.Match(text);
         if (mck.Success) { long v = long.Parse(mck.Groups["a"].Value, CultureInfo.InvariantCulture); if (v > t.CashInCents) t.CashInCents = v; }

         // cash-out: authoritative per-cassette dispensed note counts
         if (text.IndexOf("ResetLastDispensedCount", StringComparison.Ordinal) >= 0) t.Counts.Clear();
         Match mdc = reDispCount.Match(text);
         if (mdc.Success) t.Counts[mdc.Groups["c"].Value] = int.Parse(mdc.Groups["n"].Value, CultureInfo.InvariantCulture);

         if (text.IndexOf("OnDispenseComplete", StringComparison.Ordinal) >= 0 && text.IndexOf("event received", StringComparison.Ordinal) >= 0) t.DispenseOk = true;
         if (text.IndexOf("result is ItemsTaken", StringComparison.Ordinal) >= 0 || text.IndexOf("CASH TAKEN", StringComparison.Ordinal) >= 0) t.Taken = true;
         if (text.IndexOf("Retract", StringComparison.OrdinalIgnoreCase) >= 0) t.Retract = true;

         if ((text.IndexOf("TimeOut", StringComparison.Ordinal) >= 0 || text.IndexOf("Timeout", StringComparison.Ordinal) >= 0)
             && (text.IndexOf("TransactionRequest", StringComparison.Ordinal) >= 0 || text.IndexOf("Host", StringComparison.Ordinal) >= 0))
         {
            if (!t.Faults.Contains("Host timeout")) t.Faults.Add("Host timeout");
         }

         if (text.IndexOf("DetermineCloseState", StringComparison.Ordinal) >= 0) { t.End = now; t.Closed = true; }
      }

      public override void PostProcess()
      {
         int clean = 0, flagged = 0;
         string denomLegend = DenomLegend();

         foreach (Txn t in _txns)
         {
            long cashOut = CashOut(t);
            string outcome = Outcome(t);
            string fault = string.Join("; ", t.Faults);

            bool isCashOut = t.Type == "Withdrawal" || t.Type == "FastCash";
            // "where's my money" flag: dispensed but the amounts don't agree, or dispensed and not taken
            if (isCashOut && t.DispenseOk && cashOut != t.RequestedCents)
               fault = Append(fault, "REQUESTED != DISPENSED");
            if (isCashOut && t.DispenseOk && !t.Taken)
               fault = Append(fault, "cash not taken");

            if (string.IsNullOrEmpty(fault)) clean++; else flagged++;

            DataRow r = dTableSet.Tables["TransactionLedger"].Rows.Add();
            r["file"] = string.IsNullOrEmpty(t.File) ? _file : t.File;
            r["time"] = t.Start.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            r["type"] = t.Type;
            r["requested"] = Dollars(t.RequestedCents);
            r["cashout"] = isCashOut ? Dollars(cashOut) : "";
            r["cashin"] = (t.Type == "Deposit") ? Dollars(t.CashInCents) : "";
            r["notes"] = NoteBreakdown(t);
            r["taken"] = isCashOut ? (t.Taken ? "Yes" : "No") : "";
            r["teller"] = t.Teller ? "Yes" : "";
            r["outcome"] = outcome;
            r["fault"] = fault;
            r["account"] = t.Account;
         }

         dTableSet.Tables["TransactionLedger"].AcceptChanges();

         ctx.ConsoleWriteLogLine(string.Format(
            "TxnView: {0} transactions ({1} clean, {2} flagged). Cassette denominations {3}.",
            _txns.Count, clean, flagged, denomLegend));
      }

      // ---- helpers ----

      private long CashOut(Txn t)
      {
         long sum = 0;
         foreach (KeyValuePair<string, int> kv in t.Counts)
         {
            int idx = Array.IndexOf(CASSETTES, kv.Key);
            if (idx >= 0 && idx < _unitValues.Count) sum += (long)kv.Value * _unitValues[idx] * 100L;
         }
         return sum;
      }

      private string NoteBreakdown(Txn t)
      {
         List<string> parts = new List<string>();
         for (int i = 0; i < CASSETTES.Length; i++)
         {
            int n;
            if (t.Counts.TryGetValue(CASSETTES[i], out n) && n > 0)
            {
               string denom = (i < _unitValues.Count) ? "$" + _unitValues[i].ToString(CultureInfo.InvariantCulture) : CASSETTES[i];
               parts.Add(n.ToString(CultureInfo.InvariantCulture) + "x" + denom);
            }
         }
         return string.Join(" + ", parts);
      }

      private string Outcome(Txn t)
      {
         if (t.Type == "Withdrawal" || t.Type == "FastCash")
         {
            if (t.DispenseOk && t.Taken) return "Completed - taken";
            if (t.DispenseOk && t.Retract) return "RETRACTED";
            if (t.DispenseOk) return "DISPENSED - not taken";
            return "No dispense";
         }
         return "Completed";
      }

      private string DenomLegend()
      {
         List<string> parts = new List<string>();
         for (int i = 0; i < _unitValues.Count; i++)
            parts.Add(CASSETTES[i] + "=$" + _unitValues[i].ToString(CultureInfo.InvariantCulture));
         return "[" + string.Join(", ", parts) + "]";
      }

      private static string Dollars(long cents)
      {
         return (cents / 100m).ToString("N2", CultureInfo.InvariantCulture);
      }

      private static string Append(string s, string add)
      {
         return string.IsNullOrEmpty(s) ? add : s + "; " + add;
      }
   }
}
