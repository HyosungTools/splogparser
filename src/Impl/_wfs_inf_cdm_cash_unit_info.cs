
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Impl
{
   /// <summary>
   /// 
   /// </summary>
   public static class _wfs_inf_cdm_cash_unit_info
   {
      public static (bool success, string xfsMatch, string subLogLine) usCount(string logLine, string def = "6")
      {
         // usCount=6 or 
         Regex countRegex = new Regex("(?<=usCount=)(\\d+)");
         Match m = countRegex.Match(logLine);
         if (m.Success)
         {

            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }
         // usCount = [2]
         countRegex = new Regex("(?<=usCount = )\\[(\\d+)\\]");
         m = countRegex.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }

      /// <summary>
      /// GenericMatch. for example
      /// The usType line looks like this - and we want to isolate the 6 digits
      /// usType			6		2		12		12		12		12
      /// 1. We pass in "(?<=usType)((\\t+\\d+)+)", parse, and throw out the 'usType'
      /// 4. Split the remaining string, throw out any '\t' and we are left with then numbers 
      /// </summary>
      /// <param name="regEx"></param>
      /// <param name="logLine"></param>
      /// <returns></returns>
      private static string[] GenericMatch(string logLine, string regEx)
      {
         List<string> values = new List<string>();
         Regex typeRegex = new Regex(regEx);
         Match m = typeRegex.Match(logLine);
         if (m.Success)
         {
            values = m.Groups[0].Value.Split('\t').ToList();
            values.RemoveAll(s => s == "");
         }

         return values.ToArray();
      }

      /// <summary>
      /// Similar to the above except here we are dealing with a list type log line, not a table, 
      /// so we are returning distinct single values not a list. 
      /// This is identical to the GenericMatch in _wft_inf_cdm_status so there's a case to abstract into a
      /// separate class all the different regex functions
      /// </summary>
      /// <param name="regEx"></param>
      /// <param name="logLine"></param>
      /// <returns></returns>
      private static (bool success, string xfsMatch, string subLogLine) GenericMatch2(string logLine, string regStr, string def = "0")
      {
         Regex timeRegex = new Regex(regStr);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }


      public static (string thisLogicalUnit, string nextLogicalUnits)  NextLogicalUnit(string logLine)
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

      public static string[] usNumbersFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=usNumber)(([ \\t]+\\d+)+)");
      }

      public static string[] usTypesFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=usType)(([ \\t]+\\d+)+)");
      }

      public static string[] cUnitIDsFromTable(string logLine)
      {
         // note it's \w+
         return GenericMatch(logLine, "(?<=cUnitID)(([ \\t]+\\w+)+)");
      }

      public static string[] cCurrencyIDsFromTable(string logLine)
      {
         // note it's \w+
         return GenericMatch(logLine, "(?<=cCurrencyID)(([ \\t]+\\w+)+)");
      }

      public static string[] ulValuesFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulValues)(([ \\t]+\\d+)+)");
      }

      public static string[] ulInitialCountsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulInitialCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulCountsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulCount)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusesFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      public static string[] ulMinimumsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulMinimum)(([ \\t]+\\d+)+)");
      }

      public static string[] ulMaximumsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulMaximum)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRejectCountsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulRejectCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulDispensedCountsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulDispensedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulPresentedCountsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulPresentedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRetractedCountsFromTable(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulRetractedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] usNumbersFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim()); 
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(usNumber(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] usTypesFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(usType(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[]  cUnitIDsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(cUnitID(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] cCurrencyIDsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(cCurrencyID(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulValuesFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulValue(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulInitialCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulInitialCount(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulMinimumsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulMinimum(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulMaximumsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulMaximum(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulRejectCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulRejectCount(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulDispensedCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulDispensedCount(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulPresentedCountsFromLists(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulPresentedCount(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] ulRetractedCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               values.Add(ulRetractedCount(logicalUnits.thisUnit).xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      public static string[] usStatusesFromList(string logLine)
      {
         List<string> values = new List<string>();

         // isolate count
         (bool success, string xfsMatch, string subLogLine) result = usCount(logLine);
         if (result.success)
         {
            int usCount = int.Parse(result.xfsMatch.Trim());
            (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);
            for (int i = 0; i < usCount; i++)
            {
               result = usStatus(logicalUnits.thisUnit);
               values.Add(result.xfsMatch.Trim());
               logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
            }
         }
         return values.ToArray();
      }

      // usNumber  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return GenericMatch2(logLine, "(?<=usNumber = \\[)(\\d+)");
      }

      // usType
      public static (bool success, string xfsMatch, string subLogLine) usType(string logLine)
      {
         return GenericMatch2(logLine, "(?<=usType = \\[)(\\d+)");
      }

      // cUnitID
      public static (bool success, string xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         return GenericMatch2(logLine, "(?<=cUnitID = \\[)([a-zA-Z0-9]*)\\]");
      }

      // cCurrencyID
      public static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return GenericMatch2(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]");
      }

      // ulValues
      public static (bool success, string xfsMatch, string subLogLine) ulValue(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulValues = \\[)(\\d+)");
      }

      // usStatus
      public static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return GenericMatch2(logLine, "(?<=usStatus = \\[)(\\d+)");
      }

      // ulInitialCount
      public static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulInitialCount = \\[)(\\d+)");
      }

      // ulMinimum
      public static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulMinimum = \\[)(\\d+)");
      }

      // ulMaximum
      public static (bool success, string xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulMaximum = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // ulRejectCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulRejectCount = \\[)(\\d+)");
      }

      // ulDispensedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulDispensedCount = \\[)(\\d+))|(More Data)"
         return GenericMatch2(logLine, "(?<=ulDispensedCount = \\[)(\\d+)");
      }

      // ulPresentedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulPresentedCount = \\[)(\\d+))|(More Data)"
         return GenericMatch2(logLine, "(?<=ulPresentedCount = \\[)(\\d+)");
      }

      // ulRetractedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulRetractedCount = \\[)(\\d+))|(More Data)"
         return GenericMatch2(logLine, "(?<=ulRetractedCount = \\[)(\\d+)");
      }

      // lppPhysical  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) lppPhysical(string logLine)
      {
         return GenericMatch2(logLine, "(?<=lppPhysical = {.*})");
      }
   }
}
