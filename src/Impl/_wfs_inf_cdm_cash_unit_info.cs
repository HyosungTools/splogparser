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
      private static (bool success, string[] xfsMatch, string subLogLine) GenericMatch(string regEx, string logLine)
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

      public static (bool success, string[] xfsMatch, string subLogLine) usType(string logLine)
      {
         return GenericMatch("(?<=usType)(([ \\t]+\\d+)+)", logLine);
      }

      public static (bool success, string[] xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         // note it's \w+ 
         return GenericMatch("(?<=cUnitID)(([ \\t]+\\w+)+)", logLine);
      }

      public static (bool success, string[] xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         // note it's \w+
         return GenericMatch("(?<=cCurrencyID)(([ \\t]+\\w+)+)", logLine);
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulValues(string logLine)
      {
         return GenericMatch("(?<=ulValues)(([ \\t]+\\d+)+)", logLine);
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         return GenericMatch("(?<=ulInitialCount)(([ \\t]+\\d+)+)", logLine);
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         return GenericMatch("(?<=ulMinimum)(([ \\t]+\\d+)+)", logLine);
      }

      public static (bool success, string[] xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return GenericMatch("(?<=ulMaximum)(([ \\t]+\\d+)+)", logLine);
      }
   }
}
