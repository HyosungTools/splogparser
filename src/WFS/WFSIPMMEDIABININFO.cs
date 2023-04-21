using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_INF_IPM_MEDIA_BIN_INFO


   public class WFSIPMMEDIABININFO : WFS
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

      public WFSIPMMEDIABININFO(IContext ctx) : base(ctx)
      {
         lUnitCount = 1;
      }

      public string Initialize(string nwLogLine)
      {
         int indexOfTable = nwLogLine.IndexOf("lppMediaBin->");
         int indexOfList = nwLogLine.IndexOf("lppMediaBin ="); 

         if (indexOfTable > 0)
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromTable(nwLogLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

         }
         else if (indexOfTable > 0 || nwLogLine.Contains("usBinNumber = ["))
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromList(nwLogLine, lUnitCount);
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
         else
         {
            ctx.ConsoleWriteLogLine("EXCEPTION in WFSCIMCASHINFO Initialize, unexpected log line: " + nwLogLine);
         }

         return nwLogLine;
      }

      // T A B L E   A C C E S S   F U N C T I O N S

      public static (bool success, string xfsMatch, string subLogLine) usCountFromTable(string logLine, int lUnitCount = 1)
      {
         return WFS.WFSMatch(logLine, "(?<=usCount=)(\\d+)", lUnitCount.ToString());
      }


      // L I S T    A C C E S S    F U N C T I O N S 

      public static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine, int lUnitCount = 1)
      {
         return WFS.WFSMatch(logLine, "(?<=usCount = )\\[(\\d+)\\]", lUnitCount.ToString());
      }

      public static string[] usBinNumbersFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usBinNumber(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));

      }

      public static string[] fwTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(fwType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] wMediaTypesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(wMediaType(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] lpstrBinIDsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(lpstrBinID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount, ""));
      }

      public static string[] ulMediaInCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMediaInCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount, ""));
      }

      public static string[] ulCountsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulRetractOperationsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulRetractOperation(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulMaximumItemsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMaximumItem(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] ulMaximumRetractOperationsFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(ulMaximumRetractOperation(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }

      public static string[] usStatusesFromList(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = WFS.NextLogicalUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = WFS.NextLogicalUnit(logicalUnits.nextUnits);
         }
         return WFS.TrimAll(WFS.Resize(values.ToArray(), lUnitCount));
      }


      // I N D I V I D U A L    A C C E S S O R S

      // usBinNumber  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) usBinNumber(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usBinNumber = \\[)(\\d+)");
      }

      // fwType
      public static (bool success, string xfsMatch, string subLogLine) fwType(string logLine)
      {
         // in logs these are hex values
         return WFS.WFSMatchList(logLine, "(?<=fwType = \\[)([a-zA-Z0-9]*)\\]", "");
      }

      // wMediaType
      public static (bool success, string xfsMatch, string subLogLine) wMediaType(string logLine)
      {
         // in logs these are hex values
         return WFS.WFSMatchList(logLine, "(?<=wMediaType = \\[)([a-zA-Z0-9]*)\\]", "");
      }

      // lpstrBinID
      public static (bool success, string xfsMatch, string subLogLine) lpstrBinID(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=lpstrBinID = \\[)([a-zA-Z0-9 ]*)\\]", "");
      }

      // ulMediaInCount
      public static (bool success, string xfsMatch, string subLogLine) ulMediaInCount(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulMediaInCount = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }

      // ulRetractOperations
      public static (bool success, string xfsMatch, string subLogLine) ulRetractOperation(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulRetractOperations = \\[)(\\d+)");
      }

      // ulMaximumItems
      public static (bool success, string xfsMatch, string subLogLine) ulMaximumItem(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulMaximumItems = \\[)(\\d+)");
      }

      // ulMaximumRetractOperations
      public static (bool success, string xfsMatch, string subLogLine) ulMaximumRetractOperation(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulMaximumRetractOperations = \\[)(\\d+)");
      }

      // usStatus
      public static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usStatus = \\[)(\\d+)");
      }

   }
}
