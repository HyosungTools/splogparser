using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Impl
{
   public static class _wfs_base
   {
      public static (bool success, string xfsMatch, string subLogLine) GenericMatch(string logLine, string regStr, string def = "0")
      {
         Regex timeRegex = new Regex(regStr);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }

      public static string[] GenericMatchTable(string logLine, string regStr)
      {
         List<string> values = new List<string>();
         Regex regEx = new Regex(regStr);
         Match m = regEx.Match(logLine);
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
      public static (bool success, string xfsMatch, string subLogLine) GenericMatchList(string logLine, string regStr, string def = "0")
      {
         Regex regEx = new Regex(regStr);
         Match m = regEx.Match(logLine);
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
      public static string[] Resize(string[] values, int ulCount, string defValue = "0")
      {
         int oldSize = values.Length;
         Array.Resize(ref values, ulCount);
         for (int i = oldSize; i < ulCount; i++)
         {
            values[i] = defValue;
         }
         return values;
      }
      public static string[] TrimAll(string[] values)
      {
         return values.Select(value => value.Trim()).ToArray();
      }

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

   }
}
