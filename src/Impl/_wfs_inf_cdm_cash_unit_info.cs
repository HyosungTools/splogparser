using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
      private static (bool success, string[] xfsMatch, string subLogLine) GenericMatch(string logLine, string regEx)
      {
         Regex typeRegex = new Regex(regEx);
         Match m = typeRegex.Match(logLine);
         if (m.Success)
         {
            List<string> usTypes = m.Groups[0].Value.Split('\t').ToList();
            usTypes.RemoveAll(s => s == "");
            return (true, usTypes.ToArray(), logLine.Substring(m.Index));
         }

         return (false, null, logLine);
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

      public static (bool success, string[] xfsMatch, string subLogLine) usType(string logLine)
      {
         return GenericMatch(logLine, "(?<=usType)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         // note it's \w+ 
         return GenericMatch(logLine, "(?<=cUnitID)(([ \\t]+\\w+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         // note it's \w+
         return GenericMatch(logLine, "(?<=cCurrencyID)(([ \\t]+\\w+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulValues(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulValues)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulInitialCount)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulCount)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulMinimum)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulMaximum)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return GenericMatch(logLine, "(?<=usStatus)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulRejectCount)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulDispensedCount)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulPresentedCount)(([ \\t]+\\d+)+)");
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         return GenericMatch(logLine, "(?<=ulRetractedCount)(([ \\t]+\\d+)+)");
      }

      // usNumber  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return GenericMatch2(logLine, "(?<=usNumber = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulCount2(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // ulRejectCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulRejectCount2(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulRejectCount = \\[)(\\d+)");
      }

      // ulDispensedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulDispensedCount2(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulDispensedCount = \\[)(\\d+)");
      }

      // ulPresentedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulPresentedCount2(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulPresentedCount = \\[)(\\d+)");
      }

      // ulRetractedCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulRetractedCount2(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulRetractedCount = \\[)(\\d+)");
      }
   }
}
