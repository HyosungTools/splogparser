using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;
using Impl;

namespace CashTrailView
{
   #region model

   /// <summary>Where the notes in a reject/retract bin actually came from.</summary>
   internal enum BinOrigin
   {
      DISPENSER,   // notes picked from a dispense cassette that never reached the customer
      DEPOSIT,     // notes the customer put in that never reached the cash-in cassette
      MIXED,       // both, in the same bin delta
      UNKNOWN      // no corroborating cassette movement in the match window
   }

   /// <summary>
   /// One logical cash unit as the device reports it. Note that the SAME PHYSICAL UNIT can be
   /// reported by two devices (a combined reject/retract unit is visible to both CDM and CIM),
   /// and the SAME LCU NAME can mean two DIFFERENT units on two devices (CDM.LCU02 is a $50
   /// dispense cassette; CIM.LCU02 is the cash-in cassette). Both cases are handled explicitly.
   /// </summary>
   internal class CashUnit
   {
      public string Device;        // CDM | CIM
      public int Number;           // logical unit number within that device
      public string Name;          // LCU00, LCU01, ...
      public string Type;          // cash | cashin | reject | retract | retain
      public string Currency;      // USD
      public int Denom;            // 20, 50, ... 0 for non-cash units
      public int Max;              // capacity
      public string TableName;     // CashUnit-3 / CashIn-2 (pre-rename name in the view XML)
      public DataTable Table;

      public bool IsShared;        // same physical unit is reported by another device too
      public string SharedWith;    // that device

      public string Qualified { get { return Device + "." + Name; } }
      public bool IsCash { get { return Type == "cash" && Denom > 0; } }
      public bool IsBin { get { return Type == "reject" || Type == "retract" || Type == "retain"; } }
      public bool IsCashIn { get { return Type == "cashin"; } }
   }

   /// <summary>Change in a cash unit's cumulative counters between two consecutive reports.</summary>
   internal class UnitDelta
   {
      public DateTime Time;
      public CashUnit Unit;
      public string File;
      public string Status;

      public int dCount;        // current contents (bins report this; cassettes often do not)
      public int dReject;
      public int dDispensed;
      public int dPresented;
      public int dRetracted;
      public int dCashIn;

      public int[] dDenoms;     // per-denomination change
      public int[] Denoms;      // per-denomination running total after this report

      /// <summary>
      /// Notes that LEFT this cassette and did NOT reach the customer. This single number is the
      /// whole ballgame: it is the only direct evidence that dispenser cash went into a bin.
      /// </summary>
      public int Unpresented { get { return dDispensed - dPresented - dRetracted; } }
   }

   /// <summary>One row of OverView's Transaction table.</summary>
   internal class ApTxn
   {
      public DateTime Time;
      public string Session;
      public string Type;
      public string Account;
      public double Requested;
      public double Dispensed;
      public double Cash;
      public double Check;
      public bool Success;

      public bool IsWithdrawal { get { return Type == "Withdrawal"; } }
      public bool IsCashDeposit { get { return Type == "CashDeposit"; } }
      public bool IsReversal { get { return Type != null && Type.EndsWith("Reversal"); } }
   }

   /// <summary>A resolved reject/retract bin event with provenance.</summary>
   internal class BinEvent
   {
      public DateTime Time;
      public CashUnit Bin;
      public string File;
      public int Notes;
      public int[] Denoms;
      public int Amount;
      public BinOrigin Origin;
      public int OriginPct;
      public int DispenserNotes;
      public int DispenserAmount;
      public List<string> Evidence = new List<string>();
      public ApTxn Txn;
      public bool Reversed;
      public List<string> Flags = new List<string>();
      public string Explain;
   }

   /// <summary>A service visit (a cluster of start/end exchange events).</summary>
   internal class ExchangeVisit
   {
      public DateTime First;
      public DateTime Last;
   }

   #endregion

   internal class CashTrailTable : BaseTable
   {
      // ---------------------------------------------------------------- constants

      public static readonly string[] DenomColumns = { "USD0", "USD1", "USD2", "USD5", "USD10", "USD20", "USD50", "USD100" };
      public static readonly int[] DenomValues = { 0, 1, 2, 5, 10, 20, 50, 100 };

      private const string T_BALANCE = "CashBalance";
      private const string T_TRAIL = "CashTrail";
      private const string T_BINS = "CashBins";

      /// <summary>Cassette movement this close to a bin update is treated as the same physical event.</summary>
      private static readonly TimeSpan MATCH_WINDOW = TimeSpan.FromSeconds(5);
      /// <summary>How far from an SP event to look for the AP transaction that caused it.</summary>
      private static readonly TimeSpan TXN_WINDOW = TimeSpan.FromSeconds(120);
      /// <summary>How long to wait for a reversal before calling a failed dispense a customer loss.</summary>
      private static readonly TimeSpan REVERSAL_WINDOW = TimeSpan.FromSeconds(180);
      /// <summary>Exchange events closer than this belong to one service visit.</summary>
      private static readonly TimeSpan VISIT_GAP = TimeSpan.FromSeconds(60);

      // ---------------------------------------------------------------- state

      private readonly List<CashUnit> units = new List<CashUnit>();
      private readonly Dictionary<CashUnit, List<UnitDelta>> deltas = new Dictionary<CashUnit, List<UnitDelta>>();
      private readonly List<ApTxn> txns = new List<ApTxn>();
      private readonly List<BinEvent> binEvents = new List<BinEvent>();
      private readonly List<ExchangeVisit> visits = new List<ExchangeVisit>();

      /// <summary>Who the customer was, by time: masked PAN from OverSummary, or the identification
      /// they used when there was no card. Ordered by time.</summary>
      private readonly List<Tuple<DateTime, string>> customerIndex = new List<Tuple<DateTime, string>>();
      /// <summary>Resolved once per AP session so every row in a session names the same customer.</summary>
      private readonly Dictionary<string, string> sessionCustomer = new Dictionary<string, string>();

      /// <summary>Masked PAN, e.g. 479281XXXXXX3639. The OverSummary card column also carries
      /// lifecycle keywords (inserted / read / ejected / on-us), which this excludes.</summary>
      private static readonly Regex PanPattern = new Regex(@"^[0-9]{4,8}[Xx\*]{4,}[0-9]{2,6}$", RegexOptions.Compiled);
      private static readonly TimeSpan CUSTOMER_LOOKBACK = TimeSpan.FromSeconds(300);
      private static readonly TimeSpan CUSTOMER_LOOKAHEAD = TimeSpan.FromSeconds(60);

      public CashTrailTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      // ================================================================ entry point

      /// <summary>
      /// Read CDMView / CIMView / OverView XML, reconcile, and build the three output tables.
      /// </summary>
      public void BuildSummary()
      {
         ctx.ConsoleWriteLogLine("CashTrailTable.BuildSummary");

         DataSet cdm = LoadView("CDMView");
         DataSet cim = LoadView("CIMView");
         DataSet over = LoadView("OverView");

         if (cdm == null && cim == null)
         {
            ctx.ConsoleWriteLogLine("ERROR: neither CDMView nor CIMView XML is available. "
                                  + "CashTrailView needs at least one of -s CDM / -s CIM. Aborting.");
            return;
         }

         BuildUnits("CDM", cdm);
         BuildUnits("CIM", cim);
         DetectSharedUnits();

         foreach (CashUnit u in units)
         {
            deltas[u] = ComputeDeltas(u);
         }

         LoadTransactions(over);
         BuildCustomerIndex(over);
         LoadExchangeVisits(cdm, cim);
         BuildBinEvents();

         InitOutputTables();
         BuildBalanceTable();
         BuildTrailTable();
         BuildBinsTable();

         ctx.ConsoleWriteLogLine(string.Format(
            "CashTrail built: {0} units ({1} shared), {2} bin events, {3} transactions, {4} service visits",
            units.Count, units.Count(u => u.IsShared), binEvents.Count, txns.Count, visits.Count));
      }

      // ================================================================ loading

      /// <summary>Load a sibling view's XML from the work folder. Returns null if it was not run.</summary>
      private DataSet LoadView(string name)
      {
         try
         {
            string xsd = ctx.WorkFolder + "\\" + name + ".xsd";
            string xml = ctx.WorkFolder + "\\" + name + ".xml";

            if (!ctx.ioProvider.Exists(xsd) || !ctx.ioProvider.Exists(xml))
            {
               ctx.ConsoleWriteLogLine(name + " XML not found - that view was not run.");
               return null;
            }

            DataSet ds = new DataSet();
            ds.ReadXmlSchema(xsd);
            ds.ReadXml(xml);

            foreach (DataTable t in ds.Tables)
            {
               ctx.ConsoleWriteLogLine(string.Format("{0} contains table: {1} ({2} rows)", name, t.TableName, t.Rows.Count));
            }
            return ds;
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine(string.Format("Exception loading {0}: {1}", name, ex.Message));
            return null;
         }
      }

      /// <summary>
      /// Build the cash unit topology from a view's Summary table. The per-unit tables are still
      /// named CashUnit-N / CashIn-N at this point - the rename to USD50 / reject / retract happens
      /// in WriteExcelFile, which runs AFTER Analyze. Resolve by number rather than by name so this
      /// works on any machine configuration.
      /// </summary>
      private void BuildUnits(string device, DataSet ds)
      {
         if (ds == null || !ds.Tables.Contains("Summary")) return;

         foreach (DataRow row in ds.Tables["Summary"].Rows)
         {
            int number = GetInt(row, "number");
            if (number <= 0) continue;

            CashUnit u = new CashUnit
            {
               Device = device,
               Number = number,
               Name = GetStr(row, "name"),
               Type = GetStr(row, "type").ToLowerInvariant(),
               Currency = GetStr(row, "currency"),
               Denom = GetInt(row, "denom"),
               Max = GetInt(row, "max")
            };

            // the Summary table repeats a row per report - keep one per (device, number)
            if (units.Any(x => x.Device == device && x.Number == number)) continue;

            foreach (string prefix in new[] { "CashUnit-", "CashIn-", "CashUnit", "CashIn" })
            {
               string candidate = prefix + number;
               if (ds.Tables.Contains(candidate))
               {
                  u.TableName = candidate;
                  u.Table = ds.Tables[candidate];
                  break;
               }
            }

            if (u.Table == null)
            {
               ctx.ConsoleWriteLogLine(string.Format("No unit table found for {0} unit {1} ({2})", device, number, u.Name));
               continue;
            }

            units.Add(u);
            ctx.ConsoleWriteLogLine(string.Format("Cash unit {0}.{1} #{2} type={3} denom={4} max={5} table={6}",
               device, u.Name, u.Number, u.Type, u.Denom, u.Max, u.TableName));
         }
      }

      /// <summary>
      /// Decide which units are the SAME PHYSICAL unit reported twice.
      ///
      /// The test is name AND type AND capacity - all three. Matching on name alone is wrong and
      /// produces exactly the confusion this view exists to remove: CDM.LCU02 (a $50 dispense
      /// cassette) and CIM.LCU02 (the cash-in cassette) share a name and are unrelated.
      /// </summary>
      private void DetectSharedUnits()
      {
         foreach (var group in units.GroupBy(u => u.Name))
         {
            List<CashUnit> g = group.ToList();
            if (g.Count < 2) continue;

            for (int i = 0; i < g.Count; i++)
            {
               for (int j = i + 1; j < g.Count; j++)
               {
                  bool same = g[i].Type == g[j].Type && g[i].Max == g[j].Max && g[i].Device != g[j].Device;
                  if (!same)
                  {
                     ctx.ConsoleWriteLogLine(string.Format(
                        "NAME COLLISION: {0} and {1} share a logical name but are different units "
                      + "({2}/{3} vs {4}/{5}). They are reported separately.",
                        g[i].Qualified, g[j].Qualified, g[i].Type, g[i].Max, g[j].Type, g[j].Max));
                     continue;
                  }

                  g[i].IsShared = true; g[i].SharedWith = g[j].Device;
                  g[j].IsShared = true; g[j].SharedWith = g[i].Device;

                  ctx.ConsoleWriteLogLine(string.Format(
                     "SHARED UNIT: {0} and {1} are the same physical {2} unit. Its contents will be "
                   + "reported once, with an origin.", g[i].Qualified, g[j].Qualified, g[i].Type));
               }
            }
         }
      }

      /// <summary>
      /// Walk a unit's report rows chronologically and difference the cumulative counters.
      /// Rows where the unit is 'missing' (physically out of the machine) are skipped - the device
      /// zeroes or freezes its counters there and differencing across them invents note movement.
      /// </summary>
      private List<UnitDelta> ComputeDeltas(CashUnit unit)
      {
         List<UnitDelta> result = new List<UnitDelta>();
         if (unit.Table == null) return result;

         var rows = unit.Table.Rows.Cast<DataRow>()
                        .Select(r => new { Row = r, Time = ParseTime(r) })
                        .Where(x => x.Time != DateTime.MinValue)
                        .OrderBy(x => x.Time)
                        .ToList();

         int[] prev = null;
         int[] prevDenoms = null;

         foreach (var x in rows)
         {
            string status = GetStr(x.Row, "status").ToLowerInvariant();
            if (status == "missing") continue;

            // Rows with no device status are not device reports - they are summary rows other
            // analyses append to the same table ("Exchange 1", "$ per denomination", "Total $400").
            // Differencing across them invents note movement, so skip them.
            if (string.IsNullOrEmpty(status)) continue;

            int[] cur =
            {
               GetInt(x.Row, "count"),
               GetInt(x.Row, "reject"),
               GetInt(x.Row, "dispensed"),
               GetInt(x.Row, "presented"),
               GetInt(x.Row, "retracted"),
               GetInt(x.Row, "cashin")
            };
            int[] curDenoms = GetDenoms(x.Row);

            if (prev != null)
            {
               UnitDelta d = new UnitDelta
               {
                  Time = x.Time,
                  Unit = unit,
                  File = GetStr(x.Row, "file"),
                  Status = status,
                  dCount = cur[0] - prev[0],
                  dReject = cur[1] - prev[1],
                  dDispensed = cur[2] - prev[2],
                  dPresented = cur[3] - prev[3],
                  dRetracted = cur[4] - prev[4],
                  dCashIn = cur[5] - prev[5],
                  dDenoms = new int[DenomColumns.Length],
                  Denoms = curDenoms
               };
               for (int i = 0; i < DenomColumns.Length; i++)
               {
                  d.dDenoms[i] = curDenoms[i] - prevDenoms[i];
               }

               bool changed = d.dCount != 0 || d.dReject != 0 || d.dDispensed != 0 || d.dPresented != 0
                           || d.dRetracted != 0 || d.dCashIn != 0 || d.dDenoms.Any(v => v != 0);
               if (changed) result.Add(d);
            }

            prev = cur;
            prevDenoms = curDenoms;
         }

         return result;
      }

      private void LoadTransactions(DataSet over)
      {
         if (over == null || !over.Tables.Contains("Transaction")) return;

         foreach (DataRow row in over.Tables["Transaction"].Rows)
         {
            DateTime t = ParseTime(row);
            if (t == DateTime.MinValue) continue;

            txns.Add(new ApTxn
            {
               Time = t,
               Session = GetStr(row, "SessionId"),
               Type = GetStr(row, "TransactionType"),
               Account = GetStr(row, "AccountNumberMasked"),
               Requested = GetDouble(row, "AmountRequested"),
               Dispensed = GetDouble(row, "AmountDispensed"),
               Cash = GetDouble(row, "TotalCashAmount"),
               Check = GetDouble(row, "TotalCheckAmount"),
               Success = GetStr(row, "Success").Equals("True", StringComparison.OrdinalIgnoreCase)
            });
         }
         txns.Sort((a, b) => a.Time.CompareTo(b.Time));
      }

      /// <summary>
      /// Build the "who was standing at the machine" index.
      ///
      /// Preferred answer is the masked PAN read off the card, from OverSummary's card column.
      /// But not every customer uses a card - on this fleet a customer can identify with an account
      /// number and an SSN, and then no PAN is ever read (NHSWS-18867 session 3500 is exactly that
      /// case). So the Session table is indexed as a fallback and the column degrades to
      /// "AccountNumber x6999" rather than going blank.
      /// </summary>
      private void BuildCustomerIndex(DataSet over)
      {
         if (over == null) return;

         if (over.Tables.Contains("Summary"))
         {
            foreach (DataRow row in over.Tables["Summary"].Rows)
            {
               string card = GetStr(row, "card");
               if (string.IsNullOrEmpty(card) || !PanPattern.IsMatch(card)) continue;
               DateTime t = ParseTime(row);
               if (t != DateTime.MinValue) customerIndex.Add(Tuple.Create(t, card));
            }
         }

         if (over.Tables.Contains("Session"))
         {
            foreach (DataRow row in over.Tables["Session"].Rows)
            {
               DateTime t = ParseTime(row);
               if (t == DateTime.MinValue) continue;

               string type = GetStr(row, "IdentificationType");
               string id = GetStr(row, "IdentificationNumberMasked");
               if (string.IsNullOrEmpty(id)) continue;

               customerIndex.Add(Tuple.Create(t, string.IsNullOrEmpty(type) ? "x" + id : type + " x" + id));
            }
         }

         customerIndex.Sort((a, b) => a.Item1.CompareTo(b.Item1));
         ctx.ConsoleWriteLogLine(string.Format("Customer index: {0} card reads / session identifications", customerIndex.Count));
      }

      /// <summary>
      /// The customer for an AP session. Resolved once per session from the session's earliest
      /// transaction, so every row of a multi-transaction session names the same person.
      /// A full PAN beats a session identification when both are in the window.
      /// </summary>
      private string CustomerForSession(string session, DateTime fallbackTime)
      {
         if (string.IsNullOrEmpty(session)) return ResolveCustomer(fallbackTime);

         string cached;
         if (sessionCustomer.TryGetValue(session, out cached)) return cached;

         ApTxn first = txns.Where(t => t.Session == session).OrderBy(t => t.Time).FirstOrDefault();
         string who = ResolveCustomer(first != null ? first.Time : fallbackTime);
         sessionCustomer[session] = who;
         return who;
      }

      /// <summary>
      /// Nearest identification in [at - 5 min, at + 1 min], preferring a PAN.
      ///
      /// The card column is never left blank. A blank cell reads as "the tool didn't look";
      /// UNK reads as "there is no card number here", which is the truth and is itself a finding -
      /// a cardless session means the customer identified by account number, so a card-based
      /// dispute needs a different line of enquiry. Where a cardless identification is known it is
      /// named alongside UNK rather than thrown away.
      /// </summary>
      private const string NO_CARD = "UNK";

      private string ResolveCustomer(DateTime at)
      {
         string bestPan = null, bestAny = null;
         double bestPanGap = double.MaxValue, bestAnyGap = double.MaxValue;

         foreach (var entry in customerIndex)
         {
            if (entry.Item1 < at - CUSTOMER_LOOKBACK) continue;
            if (entry.Item1 > at + CUSTOMER_LOOKAHEAD) break;      // index is time-ordered

            double gap = Math.Abs((entry.Item1 - at).TotalSeconds);
            bool isPan = PanPattern.IsMatch(entry.Item2);

            if (isPan && gap < bestPanGap) { bestPanGap = gap; bestPan = entry.Item2; }
            if (gap < bestAnyGap) { bestAnyGap = gap; bestAny = entry.Item2; }
         }

         if (bestPan != null) return bestPan;
         if (bestAny != null) return NO_CARD + " (" + bestAny + ")";
         return NO_CARD;
      }

      /// <summary>Collect start/end exchange markers and cluster them into service visits.</summary>
      private void LoadExchangeVisits(DataSet cdm, DataSet cim)
      {
         List<DateTime> marks = new List<DateTime>();
         foreach (var pair in new[] { new { Ds = cdm, Tbl = "Dispense" }, new { Ds = cim, Tbl = "Deposit" } })
         {
            if (pair.Ds == null || !pair.Ds.Tables.Contains(pair.Tbl)) continue;
            foreach (DataRow row in pair.Ds.Tables[pair.Tbl].Rows)
            {
               if (!GetStr(row, "position").ToLowerInvariant().Contains("exchange")) continue;
               DateTime t = ParseTime(row);
               if (t != DateTime.MinValue) marks.Add(t);
            }
         }
         marks.Sort();

         foreach (DateTime t in marks)
         {
            if (visits.Count > 0 && t - visits[visits.Count - 1].Last <= VISIT_GAP)
            {
               visits[visits.Count - 1].Last = t;
            }
            else
            {
               visits.Add(new ExchangeVisit { First = t, Last = t });
            }
         }
      }

      // ================================================================ bin provenance

      /// <summary>
      /// For every increase in a reject/retract bin, decide where the notes came from.
      ///
      /// The rule: notes that left a dispense cassette at the same instant and were never presented
      /// are dispenser cash. Everything else is deposit cash. If a bin is shared, only the instance
      /// that carries a usable note count is used, and denominations are taken from whichever
      /// instance reports them.
      /// </summary>
      private void BuildBinEvents()
      {
         foreach (CashUnit bin in units.Where(u => u.IsBin))
         {
            // a shared bin is reported twice; take the instance that reports contents,
            // preferring the one whose count column actually moves
            if (bin.IsShared)
            {
               CashUnit peer = units.FirstOrDefault(u => u != bin && u.Name == bin.Name && u.Type == bin.Type);
               if (peer != null && !deltas[bin].Any(d => d.dCount > 0) && deltas[peer].Any(d => d.dCount > 0))
                  continue;   // the peer is the better instance; it will be processed on its own pass
               if (peer != null && deltas[bin].Any(d => d.dCount > 0) && deltas[peer].Any(d => d.dCount > 0)
                   && string.CompareOrdinal(bin.Device, peer.Device) > 0)
                  continue;   // both usable - keep one deterministically
            }

            foreach (UnitDelta ev in deltas[bin])
            {
               int notes = ev.dCount > 0 ? ev.dCount : ev.dDenoms.Sum();
               if (notes <= 0) continue;

               BinEvent be = new BinEvent
               {
                  Time = ev.Time,
                  Bin = bin,
                  File = ev.File,
                  Notes = notes,
                  Denoms = ResolveBinDenoms(bin, ev)
               };

               // dispenser-origin evidence
               foreach (CashUnit cass in units.Where(u => u.IsCash))
               {
                  foreach (UnitDelta cd in deltas[cass])
                  {
                     if (Math.Abs((cd.Time - ev.Time).TotalMilliseconds) > MATCH_WINDOW.TotalMilliseconds) continue;
                     if (cd.Unpresented <= 0) continue;

                     be.DispenserNotes += cd.Unpresented;
                     be.DispenserAmount += cd.Unpresented * cass.Denom;
                     be.Evidence.Add(string.Format("{0} (${1}) dispensed +{2}, presented +{3} -> {4} note(s) never reached the customer",
                        cass.Qualified, cass.Denom, cd.dDispensed, cd.dPresented, cd.Unpresented));
                  }
               }

               if (be.DispenserNotes >= be.Notes && be.Notes > 0)
               {
                  be.Origin = BinOrigin.DISPENSER; be.OriginPct = 100;
               }
               else if (be.DispenserNotes == 0)
               {
                  be.Origin = be.Evidence.Count == 0 && !AnyCashUnitReporting(ev.Time) ? BinOrigin.UNKNOWN : BinOrigin.DEPOSIT;
                  be.OriginPct = 0;
               }
               else
               {
                  be.Origin = BinOrigin.MIXED;
                  be.OriginPct = (int)Math.Round(100.0 * be.DispenserNotes / be.Notes);
               }

               // value the bin delta from its denominations when the device reported them,
               // otherwise fall back to the value of the cassette notes we matched
               int byDenom = ComputeAmount(be.Denoms);
               be.Amount = byDenom > 0 ? byDenom : be.DispenserAmount;

               be.Txn = NearestTxn(ev.Time, t => t.IsWithdrawal || t.IsCashDeposit);
               if (be.Txn != null) be.Reversed = FindReversal(be.Txn) != null;

               BuildBinFlagsAndExplanation(be);
               binEvents.Add(be);
            }
         }

         binEvents.Sort((a, b) => a.Time.CompareTo(b.Time));
      }

      /// <summary>
      /// A shared bin's denominations may be reported by only one of the two devices. Look at this
      /// delta first, then at the peer instance inside the match window.
      /// </summary>
      private int[] ResolveBinDenoms(CashUnit bin, UnitDelta ev)
      {
         if (ev.dDenoms.Any(v => v != 0)) return (int[])ev.dDenoms.Clone();

         CashUnit peer = units.FirstOrDefault(u => u != bin && u.Name == bin.Name && u.Type == bin.Type && u.IsShared);
         if (peer != null)
         {
            foreach (UnitDelta pd in deltas[peer])
            {
               if (Math.Abs((pd.Time - ev.Time).TotalMilliseconds) <= MATCH_WINDOW.TotalMilliseconds
                   && pd.dDenoms.Any(v => v != 0))
               {
                  return (int[])pd.dDenoms.Clone();
               }
            }
         }
         return new int[DenomColumns.Length];
      }

      private bool AnyCashUnitReporting(DateTime at)
      {
         return units.Where(u => u.IsCash)
                     .Any(u => deltas[u].Any(d => Math.Abs((d.Time - at).TotalMilliseconds) <= MATCH_WINDOW.TotalMilliseconds));
      }

      private void BuildBinFlagsAndExplanation(BinEvent be)
      {
         string who = be.Txn == null ? "no matching transaction"
                    : string.Format("session {0}, account x{1}", be.Txn.Session, be.Txn.Account);

         if (be.Origin == BinOrigin.DISPENSER)
         {
            if (be.Bin.IsShared) be.Flags.Add("DISPENSER_CASH_IN_SHARED_BIN");
            be.Explain = string.Format(
               "${0} ({1}) is DISPENSER cash, not deposit money. {2} note(s) were picked from the "
             + "cassettes for a dispense that failed and were diverted to the {3} bin. {4}. "
             + "Do NOT count this with deposits - credit the dispense cassettes.",
               be.Amount, DenomText(be.Denoms), be.Notes, be.Bin.Type, who);
         }
         else if (be.Origin == BinOrigin.DEPOSIT)
         {
            be.Flags.Add("DEPOSIT_CASH_IN_BIN");
            be.Explain = string.Format(
               "${0} ({1}) is DEPOSIT cash the customer inserted that never reached the cash-in "
             + "cassette. {2}. Check whether it was returned to the customer or is still in the bin.",
               be.Amount, DenomText(be.Denoms), who);
         }
         else if (be.Origin == BinOrigin.MIXED)
         {
            be.Flags.Add("MIXED_ORIGIN_BIN");
            be.Explain = string.Format(
               "${0} in the {1} bin is MIXED: {2} note(s) (${3}) came from the dispense cassettes, "
             + "the remaining {4} note(s) came from the deposit path. {5}. Split the count before balancing.",
               be.Amount, be.Bin.Type, be.DispenserNotes, be.DispenserAmount, be.Notes - be.DispenserNotes, who);
         }
         else
         {
            be.Flags.Add("ORIGIN_UNKNOWN");
            be.Explain = string.Format(
               "{0} note(s) entered the {1} bin with no corroborating cassette movement in the log. "
             + "Origin could not be determined - check whether the matching device was parsed (-s CDM / -s CIM).",
               be.Notes, be.Bin.Type);
         }

         if (be.Txn != null && be.Txn.IsWithdrawal && be.Txn.Dispensed < be.Txn.Requested)
         {
            be.Flags.Add(be.Reversed ? "REVERSED_NO_CUSTOMER_LOSS" : "NO_REVERSAL_CHECK_CUSTOMER");
            be.Explain += be.Reversed
               ? string.Format(" The customer was reversed (${0} requested, ${1} dispensed) - no claim owed.",
                               be.Txn.Requested, be.Txn.Dispensed)
               : string.Format(" *** No reversal found for the failed ${0} withdrawal - the customer may have been debited. ***",
                               be.Txn.Requested);
         }
      }

      // ================================================================ output: CashBalance

      private void InitOutputTables()
      {
         InitDataTable(T_BALANCE);
         AddColumn(T_BALANCE, "period");
         AddColumn(T_BALANCE, "item");
         AddColumn(T_BALANCE, "notes");
         AddColumn(T_BALANCE, "amount");
         AddColumn(T_BALANCE, "variance");
         AddColumn(T_BALANCE, "flags");
         AddColumn(T_BALANCE, "explain");

         InitDataTable(T_TRAIL);
         AddColumn(T_TRAIL, "session");
         AddColumn(T_TRAIL, "card");
         AddColumn(T_TRAIL, "account");
         AddColumn(T_TRAIL, "event");
         AddColumn(T_TRAIL, "device");
         AddColumn(T_TRAIL, "requested");
         AddColumn(T_TRAIL, "toCustomer");
         AddColumn(T_TRAIL, "fromCustomer");
         AddColumn(T_TRAIL, "toBin");
         AddColumn(T_TRAIL, "hostPosted");
         AddColumn(T_TRAIL, "variance");
         AddColumn(T_TRAIL, "outcome");
         AddColumn(T_TRAIL, "flags");
         AddColumn(T_TRAIL, "explain");

         InitDataTable(T_BINS);
         AddColumn(T_BINS, "bin");
         AddColumn(T_BINS, "sharedWith");
         AddColumn(T_BINS, "notes");
         foreach (string d in DenomColumns) AddColumn(T_BINS, d);
         AddColumn(T_BINS, "amount");
         AddColumn(T_BINS, "origin");
         AddColumn(T_BINS, "confidence");
         AddColumn(T_BINS, "session");
         AddColumn(T_BINS, "card");
         AddColumn(T_BINS, "account");
         AddColumn(T_BINS, "evidence");
         AddColumn(T_BINS, "flags");
         AddColumn(T_BINS, "explain");
      }

      /// <summary>
      /// The answer sheet. One block per balance period (service visit to service visit): what the
      /// host says, what the hardware says, and the difference.
      /// </summary>
      private void BuildBalanceTable()
      {
         List<Tuple<DateTime, DateTime, string>> periods = new List<Tuple<DateTime, DateTime, string>>();
         for (int i = 0; i + 1 < visits.Count; i++)
         {
            periods.Add(Tuple.Create(visits[i].Last, visits[i + 1].First, "Period " + (i + 1)));
         }
         if (periods.Count == 0 && txns.Count > 0)
         {
            periods.Add(Tuple.Create(txns.First().Time, txns.Last().Time, "Whole log (no service visit found)"));
         }
         if (periods.Count == 0)
         {
            // no service visit and no host data - fall back to the span of the device reports so the
            // hardware-side figures and the bin findings still get a period to sit in
            var allDeltas = units.SelectMany(u => deltas[u]).ToList();
            if (allDeltas.Count > 0)
            {
               periods.Add(Tuple.Create(allDeltas.Min(d => d.Time), allDeltas.Max(d => d.Time),
                                        "Whole log (no service visit found)"));
            }
         }

         // Is there any host data to reconcile against? With no OverView the posted totals are all
         // zero, and comparing a real cassette count against zero manufactures an enormous variance
         // that looks like missing money. Report the missing input instead.
         bool hasHostData = txns.Any(t => t.IsCashDeposit || t.IsWithdrawal);

         foreach (var p in periods)
         {
            DateTime from = p.Item1, to = p.Item2;
            string label = p.Item3;

            double depPosted = txns.Where(t => t.IsCashDeposit && t.Time >= from && t.Time <= to).Sum(t => t.Cash);
            int depUnit = CashInSnapshot(from, to);

            double wdPosted = txns.Where(t => t.IsWithdrawal && t.Time >= from && t.Time <= to).Sum(t => t.Dispensed);
            int wdPresented = units.Where(u => u.IsCash)
                                   .Sum(u => deltas[u].Where(d => d.Time >= from && d.Time <= to)
                                                      .Sum(d => d.dPresented) * u.Denom);

            int binDispenser = binEvents.Where(b => b.Time >= from && b.Time <= to
                                              && (b.Origin == BinOrigin.DISPENSER || b.Origin == BinOrigin.MIXED))
                                        .Sum(b => b.DispenserAmount);
            int binDeposit = binEvents.Where(b => b.Time >= from && b.Time <= to && b.Origin != BinOrigin.DISPENSER)
                                      .Sum(b => b.Amount - b.DispenserAmount);

            double depVar = depUnit - depPosted;
            double wdVar = wdPresented - wdPosted;

            EmitBalance(from, label, "PERIOD", "", "", "",
               string.Format("Balance period {0:yyyy-MM-dd HH:mm} to {1:yyyy-MM-dd HH:mm}", from, to));

            if (!hasHostData)
            {
               EmitBalance(from, label, "HOST DATA UNAVAILABLE", "", "", "",
                  "OverView reported no cash transactions, so there is nothing to reconcile the hardware "
                + "against. Re-run with -a Over and a zip that contains APLog*.log. Deposit and dispense "
                + "variances are SKIPPED below - absence of host data is not a shortage. The cassette and "
                + "bin figures are read straight from the device and stand on their own.",
                  "NO_HOST_DATA");

               EmitBalance(from, label, "DEPOSITS counted in cash-in unit", "", Money(depUnit), "",
                  "Value the cash-in cassette reports holding at the close of the period.");
               EmitBalance(from, label, "WITHDRAWALS presented to customer", "", Money(wdPresented), "",
                  "Notes the cassettes report actually presenting, valued at cassette denomination.");
               EmitBalance(from, label, "REJECT/RETRACT - dispenser origin", "", Money(binDispenser), "",
                  binDispenser > 0
                     ? string.Format("${0} of DISPENSER cash is in the reject/retract bin.", binDispenser)
                     : "No dispenser cash in the bins.",
                  binDispenser > 0 ? "DISPENSER_CASH_IN_BIN" : "");
               EmitBalance(from, label, "REJECT/RETRACT - deposit origin", "", Money(binDeposit), "",
                  binDeposit > 0
                     ? string.Format("${0} of DEPOSIT cash is in the bins.", binDeposit)
                     : "No deposit cash in the bins.",
                  binDeposit > 0 ? "DEPOSIT_CASH_IN_BIN" : "");

               EmitBalance(from, label, "VERDICT", "", "", "",
                  string.Format("Cannot confirm balance - no host transactions were parsed. {0}",
                     binDispenser > 0
                        ? string.Format("Separately: ${0} of dispenser cash is sitting in the reject/retract "
                                      + "bin, which would read as a cash-deposit overage of the same amount "
                                      + "if that bin were counted with deposits.", binDispenser)
                        : "No dispenser cash is sitting in the bins."),
                  "VERDICT;NO_HOST_DATA");
               continue;
            }

            EmitBalance(from, label, "DEPOSITS posted to host", "", Money(depPosted), "",
               "Sum of CashDeposit transactions the core accepted.");
            EmitBalance(from, label, "DEPOSITS counted in cash-in unit", "", Money(depUnit), "",
               "Value the cash-in cassette reports holding at the close of the period.");
            EmitBalance(from, label, "DEPOSIT VARIANCE", "", Money(depVar), Money(depVar),
               Math.Abs(depVar) < 0.005
                  ? "Deposits balance. Every dollar the customers put in was posted."
                  : "Deposits DO NOT balance - investigate before looking at the bins.",
               Math.Abs(depVar) < 0.005 ? "" : "DEPOSIT_VARIANCE");

            EmitBalance(from, label, "WITHDRAWALS posted to host", "", Money(wdPosted), "",
               "Sum of AmountDispensed on Withdrawal transactions.");
            EmitBalance(from, label, "WITHDRAWALS presented to customer", "", Money(wdPresented), "",
               "Notes the cassettes report actually presenting, valued at cassette denomination.");
            EmitBalance(from, label, "DISPENSE VARIANCE", "", Money(wdVar), Money(wdVar),
               Math.Abs(wdVar) < 0.005
                  ? "Dispense balances."
                  : "Dispense DOES NOT balance - the host and the hardware disagree.",
               Math.Abs(wdVar) < 0.005 ? "" : "DISPENSE_VARIANCE");

            EmitBalance(from, label, "REJECT/RETRACT - dispenser origin", "", Money(binDispenser), "",
               binDispenser > 0
                  ? string.Format("${0} of DISPENSER cash is in the reject/retract bin. If that bin was "
                                + "emptied into the deposit count, physical deposits read ${0} OVER the host.", binDispenser)
                  : "No dispenser cash in the bins.",
               binDispenser > 0 ? "DISPENSER_CASH_IN_BIN" : "");
            EmitBalance(from, label, "REJECT/RETRACT - deposit origin", "", Money(binDeposit), "",
               binDeposit > 0
                  ? string.Format("${0} of DEPOSIT cash is in the bins - customer notes that never made "
                                + "the cash-in cassette.", binDeposit)
                  : "No deposit cash in the bins.",
               binDeposit > 0 ? "DEPOSIT_CASH_IN_BIN" : "");

            EmitBalance(from, label, "VERDICT", "", "", "", BuildVerdict(depVar, wdVar, binDispenser, binDeposit),
               "VERDICT");
         }

         // customer exposure - the actual "where's my money"
         foreach (ApTxn t in txns.Where(t => t.IsWithdrawal && t.Dispensed < t.Requested))
         {
            ApTxn rev = FindReversal(t);
            EmitBalance(t.Time, "EXPOSURE",
               string.Format("Failed dispense session {0} x{1}", t.Session, t.Account),
               "", Money(t.Requested), Money(t.Requested - t.Dispensed),
               rev != null
                  ? string.Format("${0} requested, ${1} dispensed, REVERSED at {2:HH:mm:ss}. "
                                + "Customer was not debited - no claim owed.", t.Requested, t.Dispensed, rev.Time)
                  : string.Format("${0} requested, ${1} dispensed, NO REVERSAL FOUND within {2} seconds. "
                                + "The customer may have been debited without receiving cash - check the core.",
                                t.Requested, t.Dispensed, REVERSAL_WINDOW.TotalSeconds),
               rev != null ? "REVERSED_NO_CUSTOMER_LOSS" : "CUSTOMER_DEBITED_NO_CASH");
         }

         // cassettes that ran out and stayed out
         foreach (CashUnit u in units.Where(x => x.IsCash))
         {
            UnitDelta empty = deltas[u].FirstOrDefault(d => d.Status == "empty");
            if (empty == null) continue;
            UnitDelta refill = deltas[u].FirstOrDefault(d => d.Time > empty.Time && d.Status != "empty" && d.Status != "low");
            // how long it stayed empty: until a refill, else to the end of the observed log. Prefer
            // host transactions for that end point, but fall back to the device reports so the
            // duration is still right when OverView was not parsed.
            DateTime observedEnd = txns.Count > 0 ? txns.Last().Time : empty.Time;
            var allDeltas = units.SelectMany(x => deltas[x]).ToList();
            if (allDeltas.Count > 0 && allDeltas.Max(d => d.Time) > observedEnd)
               observedEnd = allDeltas.Max(d => d.Time);
            DateTime end = refill != null ? refill.Time : observedEnd;
            EmitBalance(empty.Time, "SERVICE", string.Format("{0} (${1}) ran empty", u.Qualified, u.Denom),
               "", "", "",
               string.Format("Empty from {0:yyyy-MM-dd HH:mm} for {1:0.0} hours{2}. Every dispense needing "
                           + "${3} notes in that window was at risk.",
                  empty.Time, (end - empty.Time).TotalHours, refill == null ? " (never replenished in this log)" : "", u.Denom),
               "CASSETTE_EMPTY");
         }
      }

      private string BuildVerdict(double depVar, double wdVar, int binDispenser, int binDeposit)
      {
         List<string> parts = new List<string>();

         if (Math.Abs(depVar) < 0.005) parts.Add("Deposits balance to $0.00.");
         else parts.Add(string.Format("Deposits are out by {0}.", Money(depVar)));

         if (Math.Abs(wdVar) < 0.005) parts.Add("Dispense balances to $0.00.");
         else parts.Add(string.Format("Dispense is out by {0}.", Money(wdVar)));

         if (binDispenser > 0)
            parts.Add(string.Format("${0} of dispenser cash is sitting in the reject/retract bin - "
                                  + "that is the most likely source of a cash-deposit overage of the same amount.", binDispenser));
         if (binDeposit > 0)
            parts.Add(string.Format("${0} of deposit cash is in the bins.", binDeposit));

         if (Math.Abs(depVar) < 0.005 && Math.Abs(wdVar) < 0.005 && binDispenser == 0 && binDeposit == 0)
            parts.Add("Nothing is missing in this period.");

         return string.Join(" ", parts);
      }

      /// <summary>
      /// Value in the cash-in cassette at the close of the period. Read as a running-total snapshot,
      /// not a sum of deltas - the device sometimes drops an update, and summing deltas silently
      /// loses those notes.
      /// </summary>
      private int CashInSnapshot(DateTime from, DateTime to)
      {
         int best = 0;
         foreach (CashUnit u in units.Where(x => x.IsCashIn))
         {
            foreach (DataRow row in u.Table.Rows)
            {
               DateTime t = ParseTime(row);
               if (t < from || t > to) continue;
               string st = GetStr(row, "status").ToLowerInvariant();
               if (st == "missing" || string.IsNullOrEmpty(st)) continue;   // see ComputeDeltas
               int v = ComputeAmount(GetDenoms(row));
               if (v > best) best = v;
            }
         }
         return best;
      }

      // ================================================================ output: CashTrail

      /// <summary>One row per money event, in time order, with host-vs-hardware on the same line.</summary>
      private void BuildTrailTable()
      {
         var events = new List<Tuple<DateTime, Action>>();

         foreach (ApTxn t in txns.Where(x => x.IsWithdrawal || x.IsCashDeposit || x.IsReversal))
         {
            ApTxn tx = t;
            events.Add(Tuple.Create(tx.Time, (Action)(() => EmitTrailForTxn(tx))));
         }
         foreach (BinEvent b in binEvents)
         {
            BinEvent be = b;
            events.Add(Tuple.Create(be.Time, (Action)(() =>
               EmitTrail(be.Time, be.Txn == null ? "" : be.Txn.Session, be.Txn == null ? "" : be.Txn.Account,
                         "TO " + be.Bin.Type.ToUpperInvariant() + " BIN", be.Bin.Device,
                         "", "", "", Money(be.Amount), "", "",
                         be.Origin.ToString(), string.Join(";", be.Flags), be.Explain))));
         }
         foreach (ExchangeVisit v in visits)
         {
            ExchangeVisit vi = v;
            events.Add(Tuple.Create(vi.First, (Action)(() =>
               EmitTrail(vi.First, "", "", "", "SERVICE VISIT", "", "", "", "", "", "", "", "EXCHANGE", "",
                         string.Format("Cash units opened for exchange {0:HH:mm:ss} to {1:HH:mm:ss}. "
                                     + "Everything before this line belongs to the previous settlement.", vi.First, vi.Last)))));
         }

         foreach (var e in events.OrderBy(x => x.Item1)) e.Item2();
      }

      private void EmitTrailForTxn(ApTxn t)
      {
         string outcome, flags = "", explain;

         if (t.IsWithdrawal)
         {
            double variance = t.Dispensed - t.Requested;
            ApTxn rev = FindReversal(t);
            if (Math.Abs(variance) < 0.005)
            {
               outcome = "OK";
               explain = string.Format("${0} requested, ${0} dispensed and taken.", t.Requested);
            }
            else if (rev != null)
            {
               outcome = "REVERSED";
               flags = "REVERSED_NO_CUSTOMER_LOSS";
               explain = string.Format("${0} requested, ${1} dispensed. Reversal posted at {2:HH:mm:ss} - "
                                     + "customer not debited, no claim owed.", t.Requested, t.Dispensed, rev.Time);
            }
            else
            {
               outcome = "SHORT";
               flags = "CUSTOMER_DEBITED_NO_CASH";
               explain = string.Format("${0} requested, only ${1} dispensed, and NO reversal was found. "
                                     + "Check the core - the customer may be out ${2}.",
                                     t.Requested, t.Dispensed, t.Requested - t.Dispensed);
            }
            EmitTrail(t.Time, t.Session, t.Account, "WITHDRAWAL", "CDM",
                      Money(t.Requested), Money(t.Dispensed), "", "", Money(t.Dispensed),
                      Money(variance), outcome, flags, explain);
         }
         else if (t.IsCashDeposit)
         {
            EmitTrail(t.Time, t.Session, t.Account, "CASH DEPOSIT", "CIM",
                      "", "", Money(t.Cash), "", Money(t.Cash), Money(0), "OK", "",
                      string.Format("${0} in notes accepted and posted to the core.", t.Cash));
         }
         else
         {
            EmitTrail(t.Time, t.Session, t.Account, t.Type.ToUpperInvariant(), "",
                      Money(t.Requested), "", "", "", Money(0), "", "REVERSAL", "",
                      "Host entry backed out. Whatever preceded it did not complete.");
         }
      }

      // ================================================================ output: CashBins

      private void BuildBinsTable()
      {
         foreach (BinEvent b in binEvents)
         {
            (bool ok, DataRow row) = NewRow(T_BINS);
            if (!ok) continue;

            row["file"] = b.File ?? "";
            row["time"] = b.Time.ToString("yyyy-MM-dd HH:mm:ss.fff");
            row["bin"] = b.Bin.Qualified + " (" + b.Bin.Type + ")";
            row["sharedWith"] = b.Bin.IsShared ? b.Bin.SharedWith : "";
            row["notes"] = b.Notes.ToString();
            for (int i = 0; i < DenomColumns.Length; i++)
               row[DenomColumns[i]] = b.Denoms[i] > 0 ? b.Denoms[i].ToString() : "";
            row["amount"] = Money(b.Amount);
            row["origin"] = b.Origin.ToString();
            row["confidence"] = b.Origin == BinOrigin.MIXED ? b.OriginPct + "% dispenser"
                              : b.Origin == BinOrigin.UNKNOWN ? "no evidence" : "exact match";
            row["session"] = b.Txn == null ? "" : b.Txn.Session;
            row["card"] = b.Txn == null ? ResolveCustomer(b.Time) : CustomerForSession(b.Txn.Session, b.Time);
            row["account"] = b.Txn == null ? "" : "x" + b.Txn.Account;
            row["evidence"] = string.Join("; ", b.Evidence);
            row["flags"] = string.Join(";", b.Flags);
            row["explain"] = b.Explain;

            AddRow(T_BINS, row);
         }
      }

      // ================================================================ emit helpers

      private void EmitBalance(DateTime time, string period, string item, string notes, string amount,
                               string variance, string explain, string flags = "")
      {
         (bool ok, DataRow row) = NewRow(T_BALANCE);
         if (!ok) return;

         row["file"] = "";
         row["time"] = time == DateTime.MinValue ? "" : time.ToString("yyyy-MM-dd HH:mm:ss");
         row["period"] = period;
         row["item"] = item;
         row["notes"] = notes;
         row["amount"] = amount;
         row["variance"] = variance;
         row["flags"] = flags;
         row["explain"] = explain;

         AddRow(T_BALANCE, row);
      }

      private void EmitTrail(DateTime time, string session, string account, string evt, string device,
                             string requested, string toCustomer, string fromCustomer, string toBin,
                             string hostPosted, string variance, string outcome, string flags, string explain)
      {
         EmitTrail(time, session, CustomerForSession(session, time), account, evt, device, requested,
                   toCustomer, fromCustomer, toBin, hostPosted, variance, outcome, flags, explain);
      }

      private void EmitTrail(DateTime time, string session, string card, string account, string evt, string device,
                             string requested, string toCustomer, string fromCustomer, string toBin,
                             string hostPosted, string variance, string outcome, string flags, string explain)
      {
         (bool ok, DataRow row) = NewRow(T_TRAIL);
         if (!ok) return;

         row["file"] = "";
         row["time"] = time.ToString("yyyy-MM-dd HH:mm:ss.fff");
         row["session"] = session;
         row["card"] = card;
         row["account"] = string.IsNullOrEmpty(account) ? "" : (account.StartsWith("x") ? account : "x" + account);
         row["event"] = evt;
         row["device"] = device;
         row["requested"] = requested;
         row["toCustomer"] = toCustomer;
         row["fromCustomer"] = fromCustomer;
         row["toBin"] = toBin;
         row["hostPosted"] = hostPosted;
         row["variance"] = variance;
         row["outcome"] = outcome;
         row["flags"] = flags;
         row["explain"] = explain;

         AddRow(T_TRAIL, row);
      }

      // ================================================================ small helpers

      private ApTxn NearestTxn(DateTime at, Func<ApTxn, bool> filter)
      {
         ApTxn best = null;
         foreach (ApTxn t in txns)
         {
            if (!filter(t)) continue;
            TimeSpan gap = t.Time - at;
            if (gap.Duration() > TXN_WINDOW) continue;
            if (best == null || (t.Time - at).Duration() < (best.Time - at).Duration()) best = t;
         }
         return best;
      }

      private ApTxn FindReversal(ApTxn t)
      {
         return txns.FirstOrDefault(x => x.IsReversal
                                      && x.Session == t.Session
                                      && (x.Time - t.Time).Duration() <= REVERSAL_WINDOW);
      }

      private static string Money(double v)
      {
         return v.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
      }

      private string DenomText(int[] denoms)
      {
         List<string> parts = new List<string>();
         for (int i = 0; i < DenomColumns.Length; i++)
            if (denoms[i] > 0) parts.Add(string.Format("{0}x${1}", denoms[i], DenomValues[i]));
         return parts.Count == 0 ? "denominations not reported" : string.Join(" + ", parts);
      }

      private int ComputeAmount(int[] denoms)
      {
         int total = 0;
         for (int i = 0; i < denoms.Length && i < DenomValues.Length; i++) total += denoms[i] * DenomValues[i];
         return total;
      }

      private int[] GetDenoms(DataRow row)
      {
         int[] d = new int[DenomColumns.Length];
         for (int i = 0; i < DenomColumns.Length; i++) d[i] = GetInt(row, DenomColumns[i]);
         return d;
      }

      private DateTime ParseTime(DataRow row)
      {
         string s = GetStr(row, "time");
         if (string.IsNullOrEmpty(s)) return DateTime.MinValue;
         DateTime dt;
         return DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) ? dt : DateTime.MinValue;
      }

      private string GetStr(DataRow row, string col)
      {
         if (row == null || !row.Table.Columns.Contains(col)) return string.Empty;
         object v = row[col];
         return v == null || v == DBNull.Value ? string.Empty : v.ToString().Trim();
      }

      private int GetInt(DataRow row, string col)
      {
         string s = GetStr(row, col);
         if (string.IsNullOrEmpty(s)) return 0;
         int i; if (int.TryParse(s, out i)) return i;
         double d; if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d)) return (int)d;
         return 0;
      }

      private double GetDouble(DataRow row, string col)
      {
         string s = GetStr(row, col);
         if (string.IsNullOrEmpty(s)) return 0;
         double d;
         return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0;
      }
   }
}
