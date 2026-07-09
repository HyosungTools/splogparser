using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Parses a consolidated CLogicalUnit::TraceCIMCashUnitInfo line (DieboldNixdorf flat logs).
   /// One line carries every field for one logical unit, e.g.:
   ///
   ///    PROPERTY CLogicalUnit::TraceCIMCashUnitInfo
   ///       LogicalUnit[2].Number[3]  LogicalUnit[2].Type[RECYCLING]  LogicalUnit[2].UnitID[72253]
   ///       LogicalUnit[2].CurrencyID[USD]  LogicalUnit[2].CashInCount[8]  LogicalUnit[2].TotalCount[2929]
   ///       LogicalUnit[2].ItemID[(39943)]  LogicalUnit[2].ItemCount[(2929)]  LogicalUnit[2].ItemValue[(5)]
   ///       LogicalUnit[2].InitialCount[3000]  LogicalUnit[2].DispensedCount[79] ...
   ///       LogicalUnit[2].PCUPositionName[(SLOT1)]  LogicalUnit[2].PCUTotalCount[(2929)] ...
   ///
   /// All Key[Value] pairs are captured into Fields. Scalars read via Field(),
   /// paren-list values (ItemID, ItemCount, PCU*) read via FieldList().
   /// </summary>
   public class CIMCashUnitTrace : SPFlatLine
   {
      /// <summary>The logical unit index n from LogicalUnit[n].* (zero-based), or -1 if not found.</summary>
      public int UnitIndex { get; private set; }

      /// <summary>All Key -> raw Value pairs found on the line.</summary>
      public Dictionary<string, string> Fields { get; private set; }

      // Convenience accessors for the fields a cassette-count table needs
      public string Number { get { return Field("Number"); } }
      public string UnitType { get { return Field("Type"); } }
      public string UnitID { get { return Field("UnitID"); } }
      public string CurrencyID { get { return Field("CurrencyID"); } }
      public string Status { get { return Field("Status"); } }
      public string InitialCount { get { return Field("InitialCount"); } }
      public string TotalCount { get { return Field("TotalCount"); } }
      public string CashInCount { get { return Field("CashInCount"); } }
      public string DispensedCount { get { return Field("DispensedCount"); } }
      public string PresentedCount { get { return Field("PresentedCount"); } }
      public string RetractedCount { get { return Field("RetractedCount"); } }
      public string RejectCount { get { return Field("RejectCount"); } }

      public CIMCashUnitTrace(ILogFileHandler handler, string line, SPFlatType flatType = SPFlatType.CIM_CashUnitTrace) : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         UnitIndex = -1;
         Fields = new Dictionary<string, string>();

         // LogicalUnit[2].TotalCount[2929]  ->  key = TotalCount, value = 2929
         foreach (Match match in Regex.Matches(logLine, @"LogicalUnit\[(\d+)\]\.(\w+)\[([^\]]*)\]"))
         {
            if (UnitIndex < 0)
            {
               int parsed;
               if (int.TryParse(match.Groups[1].Value, out parsed))
               {
                  UnitIndex = parsed;
               }
            }

            string key = match.Groups[2].Value;
            string value = match.Groups[3].Value.Trim();

            if (!Fields.ContainsKey(key))
            {
               Fields.Add(key, value);
            }
         }
      }

      /// <summary>Scalar field accessor; returns empty string if the field is absent.</summary>
      public string Field(string name)
      {
         string value;
         return Fields != null && Fields.TryGetValue(name, out value) ? value : string.Empty;
      }

      /// <summary>
      /// Paren-list field accessor, e.g. ItemValue[(5)] or PCUStatus[(OK)(OK)] -> string array.
      /// Whitespace-only entries become empty strings. Returns an empty array if absent.
      /// </summary>
      public string[] FieldList(string name)
      {
         string raw = Field(name);

         return Regex.Matches(raw, @"\(([^)]*)\)")
             .Cast<Match>()
             .Select(m => string.IsNullOrWhiteSpace(m.Groups[1].Value) ? "" : m.Groups[1].Value)
             .ToArray();
      }
   }
}
