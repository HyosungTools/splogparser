using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Impl
{
   public static class _wfs_cmd_cdm_dispense
   {
      /// <summary>
      /// GenericMatch. for example
      /// The lpulValues line looks like this - and we want to isolate the 6 digits
      /// 		lpulValues = [0, 0, 0, 4, 4, 0],
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
            List<string> usTypes = m.Groups[0].Value.Split(',').ToList();
            usTypes.RemoveAll(s => s == "");
            string[] usTypesArray = usTypes.ToArray();
            for (int i = 0; i < usTypesArray.Length; i++)
               usTypesArray[i] = usTypesArray[i].Trim();
            return (true, usTypesArray, logLine.Substring(m.Index));
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

      // cCurrencyID
      public static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return GenericMatch2(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]");
      }
      // ulAmount
      public static (bool success, string xfsMatch, string subLogLine) ulAmount(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulAmount = \\[)(\\d+)\\]");
      }
      // usCount
      public static (bool success, string xfsMatch, string subLogLine) usCount(string logLine)
      {
         return GenericMatch2(logLine, "(?<=usCount = \\[)(\\d+)\\]");
      }

      // lpulValues
      public static (bool success, string[] xfsMatch, string subLogLine) lpulValues(string logLine)
      {
         // lpulValues = [0, 0, 0, 4, 4, 0],
         return GenericMatch(logLine, "(?<=lpulValues = \\[)([0-9 ,]*)");
      }

      // ulCashBox 
      public static (bool success, string xfsMatch, string subLogLine) ulCashBox(string logLine)
      {
         return GenericMatch2(logLine, "(?<=ulCashBox = \\[)(\\d+)\\]");
      }
   }
}
