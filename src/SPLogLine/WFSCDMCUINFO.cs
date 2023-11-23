using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCDMCUINFO : SPLine
   {
      public int lUnitCount { get; set; }
      public string[] usNumbers { get; set; }
      public string[] usTypes { get; set; }
      public string[] cUnitIDs { get; set; }
      public string[] cCurrencyIDs { get; set; }
      public string[] ulValues { get; set; }
      public string[] ulInitialCounts { get; set; }
      public string[] ulCounts { get; set; }
      public string[] ulRejectCounts { get; set; }
      public string[] ulMinimums { get; set; }
      public string[] ulMaximums { get; set; }
      public string[] usStatuses { get; set; }
      public string[] ulDispensedCounts { get; set; }
      public string[] ulPresentedCounts { get; set; }
      public string[] ulRetractedCounts { get; set; }

      public WFSCDMCUINFO(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CDM_CASH_UNIT_INFO) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         lUnitCount = 1;
         int indexOfTable = logLine.IndexOf("lppList->");
         int indexOfList = logLine.IndexOf("lppList =");

         if (indexOfTable > 0)
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromTable(logLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = usNumbersFromTable(result.subLogLine);
            usTypes = usTypesFromTable(result.subLogLine);
            cUnitIDs = cUnitIDsFromTable(result.subLogLine, lUnitCount);
            cCurrencyIDs = cCurrencyIDsFromTable(result.subLogLine, lUnitCount);
            ulValues = ulValuesFromTable(result.subLogLine);
            ulInitialCounts = ulInitialCountsFromTable(result.subLogLine);
            ulCounts = ulCountsFromTable(result.subLogLine);
            ulMaximums = ulMaximumsFromTable(result.subLogLine);
            usStatuses = usStatusesFromTable(result.subLogLine);
            ulInitialCounts = ulInitialCountsFromTable(result.subLogLine);
            ulDispensedCounts = ulDispensedCountsFromTable(result.subLogLine);
            ulPresentedCounts = ulPresentedCountsFromTable(result.subLogLine);
            ulRetractedCounts = ulRetractedCountsFromTable(result.subLogLine);
            ulRejectCounts = ulRejectCountsFromTable(result.subLogLine);
            ulMinimums = ulMinimumsFromTable(result.subLogLine);
         }
         else if (indexOfTable > 0 || logLine.Contains("usNumber = ["))
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromList(logLine, lUnitCount);
            if (result.success)
            {
               lUnitCount = int.Parse(result.xfsMatch.Trim());
            }

            usNumbers = usNumbersFromList(result.subLogLine, lUnitCount);
            usTypes = usTypesFromList(result.subLogLine, lUnitCount);
            cUnitIDs = cUnitIDsFromList(result.subLogLine, lUnitCount);
            cCurrencyIDs = cCurrencyIDsFromList(result.subLogLine, lUnitCount);
            ulValues = ulValuesFromList(result.subLogLine, lUnitCount);
            ulInitialCounts = ulInitialCountsFromList(result.subLogLine, lUnitCount);
            ulCounts = ulCountsFromList(result.subLogLine, lUnitCount);
            ulMaximums = ulMaximumsFromList(result.subLogLine, lUnitCount);
            usStatuses = usStatusesFromList(result.subLogLine, lUnitCount);
            ulInitialCounts = ulInitialCountsFromList(result.subLogLine, lUnitCount);
            ulDispensedCounts = ulDispensedCountsFromList(result.subLogLine, lUnitCount);
            ulPresentedCounts = ulPresentedCountsFromList(result.subLogLine, lUnitCount);
            ulRetractedCounts = ulRetractedCountsFromList(result.subLogLine, lUnitCount);
            ulRejectCounts = ulRejectCountsFromList(result.subLogLine, lUnitCount);
            ulMinimums = ulMinimumsFromList(result.subLogLine, lUnitCount);
         }
      }

      // T A B L E   A C C E S S   F U N C T I O N S

      protected static (bool success, string xfsMatch, string subLogLine) usCountFromTable(string logLine, int lUnitCount = 1)
      {
         return Util.Match(logLine, "(?<=usCount=)(\\d+)", lUnitCount.ToString());
      }

      protected static string[] usNumbersFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=usNumber)(([ \\t]+\\d+)+)");
      }

      protected static string[] usTypesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=usType)(([ \\t]+\\d+)+)");
      }

      protected static string[] cUnitIDsFromTable(string logLine, int lUnitCount = 1)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = Util.MatchTable(logLine, "(?<=cUnitID)(([ \\t]+\\w+)+)");
         return Util.TrimAll(Util.Resize(values, lUnitCount, ""));
      }

      protected static string[] cCurrencyIDsFromTable(string logLine, int lUnitCount = 1)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = Util.MatchTable(logLine, "(?<=cCurrencyID)(([ \\t]+\\w+)+)");
         return Util.TrimAll(Util.Resize(values, lUnitCount, ""));
      }

      protected static string[] ulValuesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulValues)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] usStatusesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulInitialCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulInitialCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulMinimumsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulMinimum)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulMaximumsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulMaximum)(([ \\t]+\\d+)+)");
      }

      protected static string[] usStatusFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulRejectCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulRejectCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulDispensedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulDispensedCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulPresentedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulPresentedCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulRetractedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulRetractedCount)(([ \\t]+\\d+)+)");
      }

      // L I S T    A C C E S S    F U N C T I O N S 

      protected static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine, int lUnitCount = 1)
      {
         return Util.Match(logLine, "(?<=usCount = )\\[(\\d+)\\]", lUnitCount.ToString());
      }

      protected static string[] usNumbersFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usNumber(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));

      }

      protected static string[] usTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulInitialCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulInitialCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] cUnitIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(cUnitID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount, ""));
      }

      protected static string[] cCurrencyIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(cCurrencyID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount, ""));
      }

      protected static string[] ulValuesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulValue(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulMaximumsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMaximum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulMinimumsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMinimum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulRejectCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRejectCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulDispensedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulDispensedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulPresentedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulPresentedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulRetractedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRetractedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] usStatusesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // I N D I V I D U A L    A C C E S S O R S


      // usNumber  - we dont need to search for this in the table log line, only in the list log line
      protected static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usNumber = \\[)(\\d+)");
      }

      // cUnitID
      protected static (bool success, string xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         return Util.MatchList(logLine, "(?<=cUnitID = \\[)([a-zA-Z0-9]*)\\]", "");
      }

      // cCurrencyID
      protected static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return Util.MatchList(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]", "");
      }

      // ulValues
      protected static (bool success, string xfsMatch, string subLogLine) ulValue(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulValues = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // ulMaximum
      protected static (bool success, string xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulMaximum = \\[)(\\d+)");
      }

      // usStatus
      protected static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usStatus = \\[)(\\d+)");
      }

      // usType
      protected static (bool success, string xfsMatch, string subLogLine) usType(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usType = \\[)(\\d+)");
      }

      // ulInitialCount
      protected static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulInitialCount = \\[)(\\d+)");
      }

      // ulMinimum
      protected static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulMinimum = \\[)(\\d+)");
      }

      // ulRejectCount  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulRejectCount = \\[)(\\d+)");
      }

      // ulDispensedCount  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulDispensedCount = \\[)(\\d+))|(More Data)"
         return Util.MatchList(logLine, "(?<=ulDispensedCount = \\[)(\\d+)");
      }

      // ulPresentedCount  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulPresentedCount = \\[)(\\d+))|(More Data)"
         return Util.MatchList(logLine, "(?<=ulPresentedCount = \\[)(\\d+)");
      }

      // ulRetractedCount  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulRetractedCount = \\[)(\\d+))|(More Data)"
         return Util.MatchList(logLine, "(?<=ulRetractedCount = \\[)(\\d+)");
      }

      // lppPhysical  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) lppPhysical(string logLine)
      {
         return Util.MatchList(logLine, "(?<=lppPhysical = {.*})");
      }
   }
}
