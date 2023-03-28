
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Impl
{
   /// <summary>
   /// 
   /// </summary>
   public class _wfs_cim_cash_info
   {
      // T A B L E   A C C E S S   F U N C T I O N S

      public static (bool success, string xfsMatch, string subLogLine) usCountFromTable(string logLine, int lUnitCount = 1)
      {
         return _wfs_base.GenericMatch(logLine, "(?<=usCount=)(\\d+)", lUnitCount.ToString());
      }

      public static string[] usNumbersFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=usNumber)(([ \\t]+\\d+)+)");
      }

      public static string[] fwTypesFromTable(string logLine)
      {
         return _wfs_base.GenericMatchTable(logLine, "(?<=fwType)(([ \\t]+\\d+)+)");
      }

      public static string[] cUnitIDsFromTable(string logLine, int lUnitCount = 1)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = _wfs_base.GenericMatchTable(logLine, "(?<=cUnitID)(([ \\t]+\\w+)+)");
         return _wfs_base.TrimAll(_wfs_base.Resize(values, lUnitCount, ""));
      }

      public static string[] cCurrencyIDsFromTable(string logLine, int lUnitCount = 1)
      {
         // there are cases where the last LU is not defined so be prepared to resize.
         // note it's \w+
         string[] values = _wfs_base.GenericMatchTable(logLine, "(?<=cCurrencyID)(([ \\t]+\\w+)+)");
         return _wfs_base.TrimAll(_wfs_base.Resize(values, lUnitCount, ""));
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

      public static string[,] noteNumberListFromTable(string logLine, int lUnitCount = 1)
      {
         return _wfs_note_numbers.NoteNumberListFromTable(logLine, lUnitCount);
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

      public static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine, int lUnitCount = 1)
      {
         return _wfs_base.GenericMatch(logLine, "(?<=usCount = )\\[(\\d+)\\]", lUnitCount.ToString());
      }

      public static string[] usNumbersFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usNumber(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));

      }

      public static string[] fwTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(fwType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] cUnitIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(cUnitID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount, ""));
      }

      public static string[] cCurrencyIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(cCurrencyID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount, ""));
      }

      public static string[] ulValuesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulValue(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulCashInCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCashInCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulMaximumsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMaximum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] usStatusesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[,] noteNumberListFromList(string logLine, int lUnitCount = 1)
      {
         return _wfs_note_numbers.NoteNumberListFromList(logLine, lUnitCount);
      }

      public static string[] ulInitialCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulInitialCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulDispensedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulDispensedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulPresentedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulPresentedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulRetractedCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRetractedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulRejectCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRejectCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulMinimumsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMinimum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), lUnitCount));
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
