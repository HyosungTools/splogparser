
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Impl
{
   /// <summary>
   /// 
   /// </summary>
   public static class _wfs_inf_cim_cash_in_status
   {
      // wStatus  - 
      public static (bool success, string xfsMatch, string subLogLine) wStatus(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=wStatus = \\[)(\\d+)");
      }

      // usNumOfRefused  - 
      public static (bool success, string xfsMatch, string subLogLine) usNumOfRefused(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=usNumOfRefused = \\[)(\\d+)");
      }

      // usNumOfNoteNumbers  - 
      public static (bool success, string xfsMatch, string subLogLine) usNumOfNoteNumbers(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=usNumOfNoteNumbers = \\[)(\\d+)");
      }

      public static string[] usNoteIDsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = usNumOfNoteNumbers(logLine);
         int usCount = int.Parse(result.xfsMatch.Trim()); 
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(usNoteID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return values.ToArray();
      }

      public static string[] ulCountsFromList(string logLine)
      {
         List<string> values = new List<string>();
         (bool success, string xfsMatch, string subLogLine) result = usNumOfNoteNumbers(logLine);
         int usCount = int.Parse(result.xfsMatch.Trim());
         (string thisUnit, string nextUnits) logicalUnits = _wfs_base.NextLogicalUnit(result.subLogLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = _wfs_base.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return _wfs_base.TrimAll(_wfs_base.Resize(values.ToArray(), usCount));
      }

      // usNoteID
      public static (bool success, string xfsMatch, string subLogLine) usNoteID(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=usNoteID = \\[)(\\d+)");
      }

      // ulCount
      public static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

   }
}
