
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Impl
{
   /// <summary>
   /// 
   /// </summary>
   public class _wfs_inf_cim_cash_unit_info
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

      public string Initialize(string nwLogLine)
      {

         if (nwLogLine.Contains("lppCashIn->"))
         {
            // isolate count (e.g usCount=7)
            (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(nwLogLine, "(?<=usCount=)(\\d+)", "1");
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = usNumbersFromTable(result.subLogLine);
            fwTypes = fwTypesFromTable(result.subLogLine);
            cUnitIDs = cUnitIDsFromTable(lUnitCount, result.subLogLine);
            cCurrencyIDs = cCurrencyIDsFromTable(lUnitCount, result.subLogLine);
            ulValues = ulValuesFromTable(result.subLogLine);
            ulCashInCounts = ulCashInCountsFromTable(result.subLogLine);
            ulCounts = ulCountsFromTable(result.subLogLine);
            ulMaximums = ulMaximumsFromTable(result.subLogLine);
            usStatuses = usStatusesFromTable(result.subLogLine);
            noteNumbers = noteNumberListFromTable(lUnitCount, result.subLogLine);
            ulInitialCounts = ulInitialCountsFromTable(result.subLogLine);
            ulDispensedCounts = ulDispensedCountsFromTable(result.subLogLine);
            ulPresentedCounts = ulPresentedCountsFromTable(result.subLogLine);
            ulRetractedCounts = ulRetractedCountsFromTable(result.subLogLine);
            ulRejectCounts = ulRejectCountsFromTable(result.subLogLine);
            ulMinimums = ulMinimumsFromTable(result.subLogLine);
         }
         else
         {
            // isolate count (e.g usCount=7)
            int ulCount = 1;
            (int usCount, string subLogLine) result = _wfs_inf_cim_cash_unit_info.usCountFromList(nwLogLine);
            if (result.usCount > 0)
            {
               ulCount = result.usCount; 
            }
             
            usNumbers = usNumbersFromList(ulCount, result.subLogLine);
            fwTypes = fwTypesFromList(ulCount, result.subLogLine);
            cUnitIDs = cUnitIDsFromList(ulCount, result.subLogLine);
            cCurrencyIDs = cCurrencyIDsFromList(ulCount, result.subLogLine);
            ulValues = ulValuesFromList(ulCount, result.subLogLine);
            ulCashInCounts = ulCashInCountsFromList(ulCount, result.subLogLine);
            ulCounts = ulCountsFromList(ulCount, result.subLogLine);
            ulMaximums = ulMaximumsFromList(ulCount, result.subLogLine);
            usStatuses = usStatusesFromList(ulCount, result.subLogLine);
            noteNumbers = noteNumberListFromList(ulCount, result.subLogLine);
            ulInitialCounts = ulInitialCountsFromList(ulCount, result.subLogLine);
            ulDispensedCounts = ulDispensedCountsFromList(ulCount, result.subLogLine);
            ulPresentedCounts = ulPresentedCountsFromList(ulCount, result.subLogLine);
            ulRetractedCounts = ulRetractedCountsFromList(ulCount, result.subLogLine);
            ulRejectCounts = ulRejectCountsFromList(ulCount, result.subLogLine);
            ulMinimums = ulMinimumsFromList(ulCount, result.subLogLine);
         }

         return nwLogLine;
      }

      // T A B L E   A C C E S S   F U N C T I O N S

      public static int usCountFromTable(string logLine)
      {
         // e.g. usCount=5
         return _wfs_base.GenericMatchusCount(logLine, "(?<=usCount=)(\\d+)");
      }

      public static string[] usNumbersFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=usNumber)(([ \\t]+\\d+)+)");
      }

      public static string[] fwTypesFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=fwType)(([ \\t]+\\d+)+)");
      }

      public static string[] cUnitIDsFromTable(int usCount, string logLine)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = _wfs_base.GenericMatchTable(logLine, "(?<=cUnitID)(([ \\t]+\\w+)+)");
         return _wfs_base.TrimAll(_wfs_base.Resize(values, usCount, ""));
      }

      public static string[] cCurrencyIDsFromTable(int usCount, string logLine)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = _wfs_base.GenericMatchTable(logLine, "(?<=cCurrencyID)(([ \\t]+\\w+)+)");
         return _wfs_base.TrimAll(_wfs_base.Resize(values, usCount, ""));
      }

      public static string[] ulValuesFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=ulValues)(([ \\t]+\\d+)+)");
      }

      public static string[] ulCashInCountsFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=ulCashInCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulInitialCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return _wfs_base.GenericMatchTable(subLogLine, "(?<=ulInitialCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulDispensedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return _wfs_base.GenericMatchTable(subLogLine, "(?<=ulDispensedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulPresentedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return _wfs_base.GenericMatchTable(subLogLine, "(?<=ulPresentedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRetractedCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return _wfs_base.GenericMatchTable(subLogLine, "(?<=ulRetractedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRejectCountsFromTable(string logLine)
      {
         // we have to look atfter 'lpNoteNumberList->'
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return _wfs_base.GenericMatchTable(subLogLine, "(?<=ulRejectCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulMinimumsFromTable(string logLine)
      {
         string subLogLine = logLine.Substring(logLine.IndexOf("lpNoteNumberList->"));
         return _wfs_base.GenericMatchTable(subLogLine, "(?<=ulMinimum)(([ \\t]+\\d+)+)");
      }

      public static string[] ulCountsFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=ulCount)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusesFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      public static string[,] noteNumberListFromTable(int lUnitCount, string logLine)
      {
         return _wfs_note_numbers.NoteNumberListFromTable(lUnitCount, logLine);
      }

      public static string[] ulMaximumsFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=ulMaximum)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      // L I S T    A C C E S S    F U N C T I O N S 

      /// <summary>
      /// Given a list format pull off in strings the next logical unit for processing
      /// </summary>
      /// <param name="logLine"></param>
      /// <returns></returns>
      public static (string thisLogicalUnit, string nextLogicalUnits) NextLogicalUnit(string logLine)
      {
         int indexOfOpenBracket = logLine.IndexOf('{');
         if (indexOfOpenBracket < 0)
         {
            return (string.Empty, logLine);
         }

         string subLogLine = logLine.Substring(indexOfOpenBracket);
         int endPos = -1;
         int bracketCount = 0;

         foreach (char c in subLogLine)
         {
            // endPos is the index of 'c'
            endPos++;
            if (c.Equals('{'))
            {
               bracketCount++;
            }
            else if (c.Equals('}'))
            {
               bracketCount--;
            }
            if (bracketCount == 0)
            {
               break;
            }
         }

         return (subLogLine.Substring(0, endPos), subLogLine.Substring(endPos + 1));
      }

      public static (int usCount, string subLogLine) usCountFromList(string logLine)
      {
         // e.g. usCount = [2]
         Regex countRegex = new Regex("(?<=usCount = )\\[(\\d+)\\]");
         Match m = countRegex.Match(logLine);
         if (m.Success)
         {
            return (int.Parse(m.Groups[1].Value.Trim()), logLine.Substring(m.Index));
         }

         return (0, logLine);
      }

      public static string[] usNumbersFromList(int ulCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < ulCount; i++)
         {
            values.Add(usNumber(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), ulCount));

      }

      public static string[] fwTypesFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount ; i++)
         {
            values.Add(fwType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] cUnitIDsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(cUnitID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount, ""));
      }

      public static string[] cCurrencyIDsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(cCurrencyID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount, ""));
      }

      public static string[] ulValuesFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulValue(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulCashInCountsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulCashInCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulCountsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulMaximumsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulMaximum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] usStatusesFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[,] noteNumberListFromList(int usCount, string logLine)
      {
         return _wfs_note_numbers.NoteNumberListFromList(usCount, logLine);
      }

      public static string[] ulInitialCountsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulInitialCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulDispensedCountsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulDispensedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulPresentedCountsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulPresentedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulRetractedCountsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulRetractedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulRejectCountsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulRejectCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      public static string[] ulMinimumsFromList(int usCount, string logLine)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(logLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulMinimum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      // I N D I V I D U A L    A C C E S S O R S

      // usNumber  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=usNumber = \\[)(\\d+)");
      }

      // usType
      public static (bool success, string xfsMatch, string subLogLine) fwType(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=fwType = \\[)(\\d+)");
      }

      // cUnitID
      public static (bool success, string xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=cUnitID = \\[)([a-zA-Z0-9]*)\\]", "");
      }

      // cCurrencyID
      public static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]", "");
      }

      // ulValues
      public static (bool success, string xfsMatch, string subLogLine) ulValue(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=ulValues = \\[)(\\d+)");
      }

      // usStatus
      public static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=usStatus = \\[)(\\d+)");
      }

      // ulInitialCount
      public static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return _wfs_base.GenericMatchList(subLogLine, "(?<=ulInitialCount = \\[)(\\d+)");
      }

      // ulDispensedCount
      public static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return _wfs_base.GenericMatchList(subLogLine, "(?<=ulDispensedCount = \\[)(\\d+)");
      }

      // ulPresentedCount
      public static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return _wfs_base.GenericMatchList(subLogLine, "(?<=ulPresentedCount = \\[)(\\d+)");
      }

      // ulRetractedCount
      public static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return _wfs_base.GenericMatchList(subLogLine, "(?<=ulRetractedCount = \\[)(\\d+)");
      }

      // ulRejectCount
      public static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return _wfs_base.GenericMatchList(subLogLine, "(?<=ulRejectCount = \\[)(\\d+)");
      }

      // ulCashInCount
      public static (bool success, string xfsMatch, string subLogLine) ulCashInCount(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=ulCashInCount = \\[)(\\d+)");
      }

      // ulMaximum
      public static (bool success, string xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=ulMaximum = \\[)(\\d+)");
      }

      // ulMinimum
      public static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         // we have to look after lpszCashUnitName
         int index = logLine.IndexOf("lpszCashUnitName");
         string subLogLine = (index >= 0) ? logLine.Substring(logLine.IndexOf("lpszCashUnitName")) : logLine;
         return _wfs_base.GenericMatchList(subLogLine, "(?<=ulMinimum = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // lppPhysical  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) lppPhysical(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=lppPhysical = {.*})");
      }
   }
}
