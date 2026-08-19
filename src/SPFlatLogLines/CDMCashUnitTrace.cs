using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Parses a consolidated Ctrl::TraceCDMCashUnitInfo line (Diebold-Nixdorf flat logs).
   ///
   /// Unlike the CIM trace (CLogicalUnit::TraceCIMCashUnitInfo - one logical unit per line, fields
   /// as LogicalUnit[n].Field[v]), the CDM trace carries EVERY logical unit in ONE line as parallel
   /// paren-arrays, one entry per unit:
   ///
   ///   PROPERTY Ctrl::TraceCDMCashUnitInfo
   ///     NumberOfLogicalUnits[3]
   ///     UnitNumber[(1)(2)(3)]
   ///     UnitType[(RETRACTCASSETTE)(REJECTCASSETTE)(BILLCASSETTE)]
   ///     UnitID[(05612)(05612)(05613)]
   ///     UnitCurrencyID[(   )(   )(USD)]
   ///     UnitValue[(0)(0)(20)]        <-- per-cassette DENOMINATION
   ///     UnitCount[(0)(0)(12000)]
   ///     UnitStatus[(OK)(OK)(EMPTY)]
   ///     UnitInitialCount[(0)(0)(12000)] ...
   ///
   /// The i-th entry of each array describes logical unit i. UnitValue is the denomination the CIM
   /// cash-in trace leaves blank for dispense-side cassettes, so this line is where a DN CDM cash-unit
   /// worksheet gets its denominations.
   /// </summary>
   public class CDMCashUnitTrace : SPFlatLine
   {
      /// <summary>Number of logical units (NumberOfLogicalUnits, or the UnitNumber array length).</summary>
      public int Count { get; private set; }

      private Dictionary<string, string[]> _arrays;

      public CDMCashUnitTrace(ILogFileHandler handler, string line, SPFlatType flatType = SPFlatType.CDM_CashUnitTrace)
         : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         _arrays = new Dictionary<string, string[]>();

         // Named paren-arrays: UnitType[(A)(B)(C)] -> _arrays["UnitType"] = { A, B, C }.
         // Whitespace-only entries (e.g. UnitCurrencyID's (   )) become "".
         foreach (Match m in Regex.Matches(logLine, @"(\w+)\[((?:\([^)]*\))+)\]"))
         {
            string name = m.Groups[1].Value;
            if (_arrays.ContainsKey(name))
            {
               continue;
            }
            string[] vals = Regex.Matches(m.Groups[2].Value, @"\(([^)]*)\)")
               .Cast<Match>()
               .Select(x => string.IsNullOrWhiteSpace(x.Groups[1].Value) ? "" : x.Groups[1].Value.Trim())
               .ToArray();
            _arrays.Add(name, vals);
         }

         Match n = Regex.Match(logLine, @"NumberOfLogicalUnits\[(\d+)\]");
         int parsed;
         if (n.Success && int.TryParse(n.Groups[1].Value, out parsed))
         {
            Count = parsed;
         }
         if (Count == 0 && _arrays.ContainsKey("UnitNumber"))
         {
            Count = _arrays["UnitNumber"].Length;
         }
      }

      /// <summary>The i-th entry of the named array (e.g. "UnitValue", "UnitType"), or "" if absent.</summary>
      public string At(string name, int i)
      {
         string[] a;
         if (_arrays != null && _arrays.TryGetValue(name, out a) && i >= 0 && i < a.Length)
         {
            return a[i];
         }
         return "";
      }
   }
}
