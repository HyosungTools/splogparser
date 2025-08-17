using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CDMUnitList : SPFlatLine
   {
      public string[] unitList; 

      public CDMUnitList(ILogFileHandler handler, string line, SPFlatType flatType) : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         try
         {
            base.Initialize();

            if (flatType == SPFlatType.CDM_PhysicalCounts)
            {
               //Console.WriteLine("Physical Counts...so the log line is getting created");
            }

            // GetUnitID0043UnitID[(51060)(51060)(51063)(51066)(51171)]
            // GetUnitType0085UnitType[(RETRACTCASSETTE)(REJECTCASSETTE)(BILLCASSETTE)(BILLCASSETTE)(BILLCASSETTE)]
            // UnitCurrencyID[(   )(   )(USD)(USD)(USD)
            // UnitValue[(0)(0)(5)(20)(50)]

            // Regular expression to match content inside parentheses
            string pattern = @"\(([^)]*)\)";
            MatchCollection matches = Regex.Matches(logLine, pattern);

            // Extract content and convert whitespace-only to empty string
            unitList = matches
                .Cast<Match>()
                .Select(m => string.IsNullOrWhiteSpace(m.Groups[1].Value) ? "" : m.Groups[1].Value)
                .ToArray();
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception in Initialize()");
         }
      }
   }
}
