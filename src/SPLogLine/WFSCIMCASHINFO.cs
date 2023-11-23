using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCIMCASHINFO : SPLine
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

      public WFSCIMCASHINFO(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CIM_CASH_UNIT_INFO) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         lUnitCount = 1;
         int indexOfTable = logLine.IndexOf("lppCashIn->");
         int indexOfList = logLine.IndexOf("lppCashIn =");

         if (indexOfTable > 0)
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromTable(logLine, lUnitCount);
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
         else if (indexOfTable > 0 || logLine.Contains("usNumber = ["))
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromList(logLine, lUnitCount);
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

      protected static string[] fwTypesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=fwType)(([ \\t]+\\d+)+)");
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

      protected static string[] ulCashInCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulCashInCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulInitialCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return Util.MatchTable(subLogLine, "(?<=ulInitialCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulDispensedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return Util.MatchTable(subLogLine, "(?<=ulDispensedCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulPresentedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return Util.MatchTable(subLogLine, "(?<=ulPresentedCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulRetractedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return Util.MatchTable(subLogLine, "(?<=ulRetractedCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulRejectCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return Util.MatchTable(subLogLine, "(?<=ulRejectCount)(([ \\t]+\\d+)+)");
      }

      protected static string[] ulMinimumsFromTable(string logLine)
      {
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return Util.MatchTable(subLogLine, "(?<=ulMinimum)(([ \\t]+\\d+)+)");
      }

      protected static string[,] noteNumberListFromTable(string logLine, int lUnitCount = 1)
      {
         return WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(logLine, lUnitCount);
      }

      protected static string[] ulMaximumsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=ulMaximum)(([ \\t]+\\d+)+)");
      }

      protected static string[] usStatusFromTable(string logLine)
      {
         return Util.MatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
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

      protected static string[] fwTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(fwType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulCashInCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCashInCount(logicalUnits.thisUnit).xfsMatch.Trim());
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

      protected static string[,] noteNumberListFromList(string logLine, int lUnitCount = 1)
      {
         return WFSCIMNOTENUMBERLIST.NoteNumberListFromList(logLine, lUnitCount);
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

      // fwType
      protected static (bool success, string xfsMatch, string subLogLine) fwType(string logLine)
      {
         return Util.MatchList(logLine, "(?<=fwType = \\[)(\\d+)");
      }

      // ulInitialCount
      protected static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return Util.MatchList(subLogLine, "(?<=ulInitialCount = \\[)(\\d+)");
      }

      // ulDispensedCount
      protected static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return Util.MatchList(subLogLine, "(?<=ulDispensedCount = \\[)(\\d+)");
      }

      // ulPresentedCount
      protected static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return Util.MatchList(subLogLine, "(?<=ulPresentedCount = \\[)(\\d+)");
      }

      // ulRetractedCount
      protected static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return Util.MatchList(subLogLine, "(?<=ulRetractedCount = \\[)(\\d+)");
      }

      // ulRejectCount
      protected static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return Util.MatchList(subLogLine, "(?<=ulRejectCount = \\[)(\\d+)");
      }

      // ulCashInCount
      protected static (bool success, string xfsMatch, string subLogLine) ulCashInCount(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulCashInCount = \\[)(\\d+)");
      }

      // ulMinimum
      protected static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return Util.MatchList(subLogLine, "(?<=ulMinimum = \\[)(\\d+)");
      }

      // lppPhysical  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) lppPhysical(string logLine)
      {
         return Util.MatchList(logLine, "(?<=lppPhysical = {.*})");
      }
   }
}
