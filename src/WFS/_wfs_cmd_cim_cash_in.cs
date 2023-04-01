using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Impl
{
   public static class _wfs_cmd_cim_cash_in
   {
      // usNumOfNoteNumbers
      public static (int usCount, string subLogLine) usNumOfNoteNumbers(string logLine)
      {
         // e.g. usCount = [2]
         Regex countRegex = new Regex("(?<=usNumOfNoteNumbers = )\\[(\\d+)\\]");
         Match m = countRegex.Match(logLine);
         if (m.Success)
         {
            return (int.Parse(m.Groups[1].Value.Trim()), logLine.Substring(m.Index));
         }

         return (0, logLine);
      }

      public static string[] usNoteIDsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usNumOfNoteNumbers(logLine);
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(usNoteID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (int usCount, string subLogLine) result = usNumOfNoteNumbers(logLine);
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < result.usCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      // usNoteID
      public static (bool success, string xfsMatch, string subLogLine) usNoteID(string logLine)
      {
         return WFS.WFSMatch(logLine, "(?<=usNoteID = \\[)(\\d+)");
      }

      // ulCount
      public static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return WFS.WFSMatch(logLine, "(?<=ulCount = \\[)(\\d+)");
      }
   }


}
