using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Commands where this structure is used: 
   //
   // WFS_INF_IPM_MEDIA_BIN_INFO


   public class WFSIPMMEDIABININFO : SPLine
   {
      public int lUnitCount { get; set; }
      public string[] usBinNumbers { get; set; }
      public string[] fwTypes { get; set; }
      public string[] wMediaTypes { get; set; }
      public string[] lpstrBinIDs { get; set; }
      public string[] ulMediaInCounts { get; set; }
      public string[] ulCounts { get; set; }
      public string[] ulRetractOperations { get; set; }
      public string[] ulMaximumItems { get; set; }
      public string[] ulMaximumRetractOperations { get; set; }
      public string[] usStatuses { get; set; }

      public WFSIPMMEDIABININFO(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_IPM_MEDIA_BIN_INFO) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         lUnitCount = 1;
         int indexOfTable = logLine.IndexOf("lppMediaBin->");
         int indexOfList = logLine.IndexOf("lppMediaBin ="); 

         if (indexOfTable > 0)
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromTable(logLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

         }
         else if (indexOfTable > 0 || logLine.Contains("usBinNumber = ["))
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromList(logLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usBinNumbers = usBinNumbersFromList(result.subLogLine, lUnitCount);
            fwTypes = fwTypesFromList(result.subLogLine, lUnitCount);
            wMediaTypes = wMediaTypesFromList(result.subLogLine, lUnitCount);
            lpstrBinIDs = lpstrBinIDsFromList(result.subLogLine, lUnitCount);
            ulMediaInCounts = ulMediaInCountsFromList(result.subLogLine, lUnitCount);
            ulCounts = ulCountsFromList(result.subLogLine, lUnitCount);
            ulRetractOperations = ulRetractOperationsFromList(result.subLogLine, lUnitCount);
            ulMaximumItems = ulMaximumItemsFromList(result.subLogLine, lUnitCount);
            ulMaximumRetractOperations = ulMaximumRetractOperationsFromList(result.subLogLine, lUnitCount);
            usStatuses = usStatusesFromList(result.subLogLine, lUnitCount);
         }
      }

      // T A B L E   A C C E S S   F U N C T I O N S

      protected static (bool success, string xfsMatch, string subLogLine) usCountFromTable(string logLine, int lUnitCount = 1)
      {
         return Util.Match(logLine, "(?<=usCount=)(\\d+)", lUnitCount.ToString());
      }


      // L I S T    A C C E S S    F U N C T I O N S 

      protected static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine, int lUnitCount = 1)
      {
         return Util.Match(logLine, "(?<=usCount = )\\[(\\d+)\\]", lUnitCount.ToString());
      }

      protected static string[] usBinNumbersFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usBinNumber(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));

      }

      protected static string[] fwTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(fwType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] wMediaTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(wMediaType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] lpstrBinIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(lpstrBinID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount, ""));
      }

      protected static string[] ulMediaInCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMediaInCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount, ""));
      }

      protected static string[] ulCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulRetractOperationsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRetractOperation(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulMaximumItemsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMaximumItem(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] ulMaximumRetractOperationsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMaximumRetractOperation(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] usStatusesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }


      // I N D I V I D U A L    A C C E S S O R S

      // usBinNumber  - we dont need to search for this in the table log line, only in the list log line
      protected static (bool success, string xfsMatch, string subLogLine) usBinNumber(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usBinNumber = \\[)(\\d+)");
      }

      // fwType
      protected static (bool success, string xfsMatch, string subLogLine) fwType(string logLine)
      {
         // in logs these are hex values
         return Util.MatchList(logLine, "(?<=fwType = \\[)([a-zA-Z0-9]*)\\]", "");
      }

      // wMediaType
      protected static (bool success, string xfsMatch, string subLogLine) wMediaType(string logLine)
      {
         // in logs these are hex values
         return Util.MatchList(logLine, "(?<=wMediaType = \\[)([a-zA-Z0-9]*)\\]", "");
      }

      // lpstrBinID
      protected static (bool success, string xfsMatch, string subLogLine) lpstrBinID(string logLine)
      {
         return Util.MatchList(logLine, "(?<=lpstrBinID = \\[)([a-zA-Z0-9 ]*)\\]", "");
      }

      // ulMediaInCount
      protected static (bool success, string xfsMatch, string subLogLine) ulMediaInCount(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulMediaInCount = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // ulRetractOperations
      protected static (bool success, string xfsMatch, string subLogLine) ulRetractOperation(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulRetractOperations = \\[)(\\d+)");
      }

      // ulMaximumItems
      protected static (bool success, string xfsMatch, string subLogLine) ulMaximumItem(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulMaximumItems = \\[)(\\d+)");
      }

      // ulMaximumRetractOperations
      protected static (bool success, string xfsMatch, string subLogLine) ulMaximumRetractOperation(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulMaximumRetractOperations = \\[)(\\d+)");
      }

      // usStatus
      protected static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usStatus = \\[)(\\d+)");
      }

   }
}
