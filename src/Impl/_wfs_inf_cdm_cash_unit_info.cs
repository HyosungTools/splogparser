
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
      // G E N E R I C    R O U T I N E S

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
      private static string[] GenericMatchTable(string logLine, string regEx)
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
      private static (bool success, string xfsMatch, string subLogLine) GenericMatchList(string logLine, string regStr, string def = "0")
      {
         Regex timeRegex = new Regex(regStr);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }

      /// <summary>
      /// Generic pull out usCount. There are two forms 'usCount=5' and 'usCount = [5]'
      /// </summary>
      /// <param name="logLine">xfs log line </param>
      /// <param name="regStr">regular expression identifying usCount</param>
      /// <param name="def">default return value</param>
      /// <returns></returns>
      public static int GenericMatchusCount(string logLine, string regStr, int def = 0)
      {
         Regex countRegex = new Regex(regStr);
         Match m = countRegex.Match(logLine);
         if (m.Success)
         {
            return int.Parse(m.Groups[1].Value);
         }

         return def;
      }

      // T A B L E   A C C E S S   F U N C T I O N S

      public static int usCountFromTable(string logLine)
      {
         // e.g. usCount=5
         return GenericMatchusCount(logLine, "(?<=usCount=)(\\d+)");
      }

      public static string[] usNumbersFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=usNumber)(([ \\t]+\\d+)+)");
      }

      public static string[] usTypesFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=usType)(([ \\t]+\\d+)+)");
      }

      public static string[] cUnitIDsFromTable(string logLine)
      {
         // note it's \w+
         return GenericMatchTable(logLine, "(?<=cUnitID)(([ \\t]+\\w+)+)");
      }

      public static string[] cCurrencyIDsFromTable(string logLine)
      {
         // note it's \w+
         return GenericMatchTable(logLine, "(?<=cCurrencyID)(([ \\t]+\\w+)+)");
      }

      public static string[] ulValuesFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulValues)(([ \\t]+\\d+)+)");
      }

      public static string[] ulInitialCountsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulInitialCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulCountsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulCount)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusesFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      public static string[] ulMinimumsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulMinimum)(([ \\t]+\\d+)+)");
      }

      public static string[] ulMaximumsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulMaximum)(([ \\t]+\\d+)+)");
      }

      public static string[] usStatusFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRejectCountsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulRejectCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulDispensedCountsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulDispensedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulPresentedCountsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulPresentedCount)(([ \\t]+\\d+)+)");
      }

      public static string[] ulRetractedCountsFromTable(string logLine)
      {
         return GenericMatchTable(logLine, "(?<=ulRetractedCount)(([ \\t]+\\d+)+)");
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

         return (0,logLine); 
      }

      public static string[] usNumbersFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(usNumber(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] usTypesFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(usType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[]  cUnitIDsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(cUnitID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] cCurrencyIDsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(cCurrencyID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulValuesFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulValue(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulInitialCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulInitialCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulMinimumsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulMinimum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulMaximumsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulMaximum(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulRejectCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulRejectCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulDispensedCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulDispensedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulPresentedCountsFromLists(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulPresentedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulRetractedCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulRetractedCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] usStatusesFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usCountFromList(logLine);
         (string thisUnit, string nextUnits) logicalUnits = NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      // I N D I V I D U A L    A C C E S S O R S

      // usNumber  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return GenericMatchList(logLine, "(?<=usNumber = \\[)(\\d+)");
      }

      // usType
      public static (bool success, string xfsMatch, string subLogLine) usType(string logLine)
      {
         return GenericMatchList(logLine, "(?<=usType = \\[)(\\d+)");
      }

      // cUnitID
      public static (bool success, string xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         return GenericMatchList(logLine, "(?<=cUnitID = \\[)([a-zA-Z0-9]*)\\]");
      }

      // cCurrencyID
      public static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return GenericMatchList(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]");
      }

      // ulValues
      public static (bool success, string xfsMatch, string subLogLine) ulValue(string logLine)
      {
         return GenericMatchList(logLine, "(?<=ulValues = \\[)(\\d+)");
      }

      // usStatus
      public static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return GenericMatchList(logLine, "(?<=usStatus = \\[)(\\d+)");
      }

      // ulInitialCount
      public static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         return GenericMatchList(logLine, "(?<=ulInitialCount = \\[)(\\d+)");
      }

      // ulMinimum
      public static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         return GenericMatchList(logLine, "(?<=ulMinimum = \\[)(\\d+)");
      }

      // ulMaximum
      public static (bool success, string xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return GenericMatchList(logLine, "(?<=ulMaximum = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return GenericMatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // ulRejectCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         return GenericMatchList(logLine, "(?<=ulRejectCount = \\[)(\\d+)");
      }

      // ulDispensedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulDispensedCount = \\[)(\\d+))|(More Data)"
         return GenericMatchList(logLine, "(?<=ulDispensedCount = \\[)(\\d+)");
      }

      // ulPresentedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulPresentedCount = \\[)(\\d+))|(More Data)"
         return GenericMatchList(logLine, "(?<=ulPresentedCount = \\[)(\\d+)");
      }

      // ulRetractedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         // we are going to read the physical values because sometimes the logical values are missing (More Data)
         // "((?<=}\\r\\n[ \\t]+ulRetractedCount = \\[)(\\d+))|(More Data)"
         return GenericMatchList(logLine, "(?<=ulRetractedCount = \\[)(\\d+)");
      }

      // lppPhysical  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) lppPhysical(string logLine)
      {
         return GenericMatchList(logLine, "(?<=lppPhysical = {.*})");
      }
   }
}
