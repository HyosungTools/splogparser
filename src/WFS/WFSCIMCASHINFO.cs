using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_INF_CIM_CASH_UNIT_INFO
   // WFS_CMD_CIM_CASH_IN_END
   // WFS_CMD_CIM_RETRACT
   // WFS_USRE_CIM_CASHUNITTHRESHOLD
   // WFS_SRVE_CIM_CASHUNITINFOCHANGED

   public class WFSCIMCASHINFO : WFS
   {
      public int lUnitCount { get; set; }
      public string[] usNumbers { get; set; }
      public string[] fwTypes { get; set; }
      public string[] cUnitIDs { get; set; }
      public string[] cCurrencyIDs { get; set; }
      public string[] ulValues { get; set; }
      public string[] ulCashInCounts { get; set; }
      public string[] ulCounts { get; set; }
      public string[] ulMaximums { get; set; }
      public string[] usStatuses { get; set; }
      public string[,] noteNumbers { get; set; }
      public string[] ulInitialCounts { get; set; }
      public string[] ulDispensedCounts { get; set; }
      public string[] ulPresentedCounts { get; set; }
      public string[] ulRetractedCounts { get; set; }
      public string[] ulRejectCounts { get; set; }
      public string[] ulMinimums { get; set; }

      public WFSCIMCASHINFO(IContext ctx) : base(ctx)
      {
         lUnitCount = 1;
      }

      public string Initialize(string nwLogLine)
      {
         int indexOfTable = nwLogLine.IndexOf("lppCashIn->");
         int indexOfList = nwLogLine.IndexOf("lppCashIn ="); 

         if (indexOfTable > 0)
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromTable(nwLogLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = usNumbersFromTable(result.subLogLine);
            fwTypes = fwTypesFromTable(result.subLogLine);
            cUnitIDs = cUnitIDsFromTable(result.subLogLine, lUnitCount);
            cCurrencyIDs = cCurrencyIDsFromTable(result.subLogLine, lUnitCount);
            ulValues = ulValuesFromTable(result.subLogLine);
            ulCashInCounts = ulCashInCountsFromTable(result.subLogLine);
            ulCounts = ulCountsFromTable(result.subLogLine);
            ulMaximums = ulMaximumsFromTable(result.subLogLine);
            usStatuses = usStatusesFromTable(result.subLogLine);
            noteNumbers = noteNumberListFromTable(result.subLogLine, lUnitCount);
            ulInitialCounts = ulInitialCountsFromTable(result.subLogLine);
            ulDispensedCounts = ulDispensedCountsFromTable(result.subLogLine);
            ulPresentedCounts = ulPresentedCountsFromTable(result.subLogLine);
            ulRetractedCounts = ulRetractedCountsFromTable(result.subLogLine);
            ulRejectCounts = ulRejectCountsFromTable(result.subLogLine);
            ulMinimums = ulMinimumsFromTable(result.subLogLine);
         }
         else if (indexOfTable > 0 || nwLogLine.Contains("usNumber = ["))
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromList(nwLogLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = usNumbersFromList(result.subLogLine, lUnitCount);
            fwTypes = fwTypesFromList(result.subLogLine, lUnitCount);
            cUnitIDs = cUnitIDsFromList(result.subLogLine, lUnitCount);
            cCurrencyIDs = cCurrencyIDsFromList(result.subLogLine, lUnitCount);
            ulValues = ulValuesFromList(result.subLogLine, lUnitCount);
            ulCashInCounts = ulCashInCountsFromList(result.subLogLine, lUnitCount);
            ulCounts = ulCountsFromList(result.subLogLine, lUnitCount);
            ulMaximums = ulMaximumsFromList(result.subLogLine, lUnitCount);
            usStatuses = usStatusesFromList(result.subLogLine, lUnitCount);
            noteNumbers = noteNumberListFromList(result.subLogLine, lUnitCount);
            ulInitialCounts = ulInitialCountsFromList(result.subLogLine, lUnitCount);
            ulDispensedCounts = ulDispensedCountsFromList(result.subLogLine, lUnitCount);
            ulPresentedCounts = ulPresentedCountsFromList(result.subLogLine, lUnitCount);
            ulRetractedCounts = ulRetractedCountsFromList(result.subLogLine, lUnitCount);
            ulRejectCounts = ulRejectCountsFromList(result.subLogLine, lUnitCount);
            ulMinimums = ulMinimumsFromList(result.subLogLine, lUnitCount);
         }
         else
         {
            ctx.ConsoleWriteLogLine("EXCEPTION in WFSCIMCASHINFO Initialize, unexpected log line: " + nwLogLine);
         }

         return nwLogLine;
      }

      // T A B L E   A C C E S S   F U N C T I O N S

      public static (bool success, string xfsMatch, string subLogLine) usCountFromTable(string logLine, int lUnitCount = 1)
      {
         return WFS.WFSMatch(logLine, "(?<=usCount=)(\\d+)", lUnitCount.ToString());
      }

      public static string[] usNumbersFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=usNumber)(([ \\t]+\\d+)+)");
      }

      public static string[] fwTypesFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=fwType)(([ \\t]+\\d+)+)");
      }

      public static string[] cUnitIDsFromTable(string logLine, int lUnitCount = 1)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = WFS.WFSMatchTable(logLine, "(?<=cUnitID)(([ \\t]+\\w+)+)");
         return WFS.TrimAll(WFS.Resize(values, lUnitCount, ""));
      }

      public static string[] cCurrencyIDsFromTable(string logLine, int lUnitCount = 1)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = WFS.WFSMatchTable(logLine, "(?<=cCurrencyID)(([ \\t]+\\w+)+)");
         return WFS.TrimAll(WFS.Resize(values, lUnitCount, ""));
      }

      public static string[] ulValuesFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=ulValues)(([ \\t]+\\d+)+)");
      }

      public static string[] ulCountsFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=ulCount)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusesFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      public static string[] ulCashInCountsFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=ulCashInCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulInitialCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return WFS.WFSMatchTable(subLogLine, "(?<=ulInitialCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulDispensedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return WFS.WFSMatchTable(subLogLine, "(?<=ulDispensedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulPresentedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return WFS.WFSMatchTable(subLogLine, "(?<=ulPresentedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRetractedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return WFS.WFSMatchTable(subLogLine, "(?<=ulRetractedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRejectCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return WFS.WFSMatchTable(subLogLine, "(?<=ulRejectCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulMinimumsFromTable(string logLine)
      {
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return WFS.WFSMatchTable(subLogLine, "(?<=ulMinimum)(([ \\t]+\\d+)+)");
      }

      public static string[,] noteNumberListFromTable(string logLine, int lUnitCount = 1)
      {
         return WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(logLine, lUnitCount);
      }

      public static string[] ulMaximumsFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=ulMaximum)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusFromTable(string logLine)
      {
         return WFS.WFSMatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      // L I S T    A C C E S S    F U N C T I O N S 

      public static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine, int lUnitCount = 1)
      {
         return WFS.WFSMatch(logLine, "(?<=usCount = )\\[(\\d+)\\]", lUnitCount.ToString());
      }

      public static string[] usNumbersFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usNumber(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));

      }

      public static string[] fwTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(fwType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulCashInCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCashInCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] cUnitIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(cUnitID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount, ""));
      }

      public static string[] cCurrencyIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(cCurrencyID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount, ""));
      }

      public static string[] ulValuesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulValue(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulMaximumsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMaximum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] usStatusesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[,] noteNumberListFromList(string logLine, int lUnitCount = 1)
      {
         return WFSCIMNOTENUMBERLIST.NoteNumberListFromList(logLine, lUnitCount);
      }

      public static string[] ulInitialCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulInitialCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulDispensedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulDispensedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulPresentedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulPresentedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulRetractedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRetractedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulRejectCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRejectCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulMinimumsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMinimum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      // I N D I V I D U A L    A C C E S S O R S


      // usNumber  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usNumber = \\[)(\\d+)");
      }

      // cUnitID
      public static (bool success, string xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=cUnitID = \\[)([a-zA-Z0-9]*)\\]", "");
      }

      // cCurrencyID
      public static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]", "");
      }

      // ulValues
      public static (bool success, string xfsMatch, string subLogLine) ulValue(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulValues = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // ulMaximum
      public static (bool success, string xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulMaximum = \\[)(\\d+)");
      }

      // usStatus
      public static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usStatus = \\[)(\\d+)");
      }

      // fwType
      public static (bool success, string xfsMatch, string subLogLine) fwType(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=fwType = \\[)(\\d+)");
      }

      // ulInitialCount
      public static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return WFS.WFSMatchList(subLogLine, "(?<=ulInitialCount = \\[)(\\d+)");
      }

      // ulDispensedCount
      public static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return WFS.WFSMatchList(subLogLine, "(?<=ulDispensedCount = \\[)(\\d+)");
      }

      // ulPresentedCount
      public static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return WFS.WFSMatchList(subLogLine, "(?<=ulPresentedCount = \\[)(\\d+)");
      }

      // ulRetractedCount
      public static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return WFS.WFSMatchList(subLogLine, "(?<=ulRetractedCount = \\[)(\\d+)");
      }

      // ulRejectCount
      public static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return WFS.WFSMatchList(subLogLine, "(?<=ulRejectCount = \\[)(\\d+)");
      }

      // ulCashInCount
      public static (bool success, string xfsMatch, string subLogLine) ulCashInCount(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulCashInCount = \\[)(\\d+)");
      }

      // ulMinimum
      public static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return WFS.WFSMatchList(subLogLine, "(?<=ulMinimum = \\[)(\\d+)");
      }

      // lppPhysical  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) lppPhysical(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=lppPhysical = {.*})");
      }
   }
}
