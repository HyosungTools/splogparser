using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RegEx
{
    public class Util
    {
      public static string RemovePhysicalUnit(string logicalUnitString)
      {
         // Find usNumPhysicalCUs = [
         string numKey = "usNumPhysicalCUs = [";
         int numStart = logicalUnitString.IndexOf(numKey);
         if (numStart == -1)
         {
            return logicalUnitString;
         }
         numStart += numKey.Length;
         int numEnd = logicalUnitString.IndexOf("]", numStart);
         if (numEnd == -1)
         {
            return logicalUnitString;
         }
         string numStr = logicalUnitString.Substring(numStart, numEnd - numStart);
         if (!int.TryParse(numStr, out int numUnits) || numUnits <= 0)
         {
            return logicalUnitString;
         }

         // Find lppPhysical =
         string physKey = "lppPhysical =";
         int start = logicalUnitString.IndexOf(physKey, numEnd);
         if (start == -1)
         {
            return logicalUnitString;
         }

         int i = start + physKey.Length;

         // Process each physical unit block
         for (int unit = 0; unit < numUnits; unit++)
         {
            // Skip whitespace to find next {
            while (i < logicalUnitString.Length && char.IsWhiteSpace(logicalUnitString[i]))
            {
               i++;
            }
            if (i >= logicalUnitString.Length || logicalUnitString[i] != '{')
            {
               // Malformed, return original
               return logicalUnitString;
            }

            int braceCount = 1;
            i++; // Skip {
            while (i < logicalUnitString.Length && braceCount > 0)
            {
               if (logicalUnitString[i] == '{')
               {
                  braceCount++;
               }
               else if (logicalUnitString[i] == '}')
               {
                  braceCount--;
               }
               i++;
            }

            if (braceCount != 0)
            {
               // Unbalanced, return original
               return logicalUnitString;
            }
         }

         // i is now after the last }
         int end = i;

         // Remove the section
         return logicalUnitString.Substring(0, start) + logicalUnitString.Substring(end);
      }

      public static List<string> ExtractPhysicalUnits(string logicalUnitString)
      {
         List<string> physicalUnits = new List<string>();

         // Find usNumPhysicalCUs = [
         string numKey = "usNumPhysicalCUs = [";
         int numStart = logicalUnitString.IndexOf(numKey);
         if (numStart == -1)
         {
            return physicalUnits; // Empty list if not found
         }
         numStart += numKey.Length;
         int numEnd = logicalUnitString.IndexOf("]", numStart);
         if (numEnd == -1)
         {
            return physicalUnits;
         }
         string numStr = logicalUnitString.Substring(numStart, numEnd - numStart);
         if (!int.TryParse(numStr, out int numUnits) || numUnits <= 0)
         {
            return physicalUnits;
         }

         // Find lppPhysical =
         string physKey = "lppPhysical =";
         int start = logicalUnitString.IndexOf(physKey, numEnd);
         if (start == -1)
         {
            return physicalUnits;
         }

         int i = start + physKey.Length;

         // Process each physical unit block
         for (int unit = 0; unit < numUnits; unit++)
         {
            // Skip whitespace to find next {
            while (i < logicalUnitString.Length && char.IsWhiteSpace(logicalUnitString[i]))
            {
               i++;
            }
            if (i >= logicalUnitString.Length || logicalUnitString[i] != '{')
            {
               // Malformed, stop and return what we have
               break;
            }

            int blockStart = i;
            int braceCount = 1;
            i++; // Skip {
            while (i < logicalUnitString.Length && braceCount > 0)
            {
               if (logicalUnitString[i] == '{')
               {
                  braceCount++;
               }
               else if (logicalUnitString[i] == '}')
               {
                  braceCount--;
               }
               i++;
            }

            if (braceCount != 0)
            {
               // Unbalanced, skip this one
               continue;
            }

            // Extract the block from blockStart to i-1 (including braces)
            string block = logicalUnitString.Substring(blockStart, i - blockStart);
            physicalUnits.Add(block);
         }

         return physicalUnits;
      }

      public static (bool success, string xfsMatch, string subLogLine) Match(string logLine, string regStr, string def = "0")
      {
         Regex timeRegex = new Regex(regStr);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }

      public static string[] MatchTable(string logLine, string regStr)
      {
         List<string> values = new List<string>();
         Regex regEx = new Regex(regStr);
         Match m = regEx.Match(logLine);
         if (m.Success)
         {
            values = m.Groups[1].Value.Split('\t').ToList();
            values.RemoveAll(s => string.IsNullOrEmpty(s));

            for (int i = 0; i < values.Count; i++)
            {
               if (!values[i].Contains("   "))
               {
                  values[i] = values[i].Trim(); 
               }
            }
         }

         Console.WriteLine($"MatchTable - size: {values.Count} elements : {string.Join("|", values)}");
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
      public static (bool success, string xfsMatch, string subLogLine) MatchList(string logLine, string regStr, string def = "0")
      {
         Regex regEx = new Regex(regStr);
         Match m = regEx.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }


      // e.g. lpulValues = [0, 0, 0, 4, 4, 0],
      public static (bool success, string[] xfsMatch, string subLogLine) MatchListToArray(string logLine, string regStr)
      {
         Regex typeRegex = new Regex(regStr);
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
      /// Generic pull out usCount. There are two forms 'usCount=5' and 'usCount = [5]'
      /// </summary>
      /// <param name="logLine">xfs log line </param>
      /// <param name="regStr">regular expression identifying usCount</param>
      /// <param name="def">default return value</param>
      /// <returns></returns>
      public static int MatchInt(string logLine, string regStr, int def = 0)
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
//         Console.WriteLine($"Resize: {string.Join("|", values)}");
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
      /// 
      public static (string thisLogicalUnit, string nextLogicalUnits) NextUnit(string logLine)
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
