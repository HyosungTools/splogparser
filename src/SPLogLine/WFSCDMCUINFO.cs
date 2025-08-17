using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class PhysicalCU
   {
      public string lpPhysicalPositionName { get; set; } = "";
      public string cUnitID { get; set; } = "";
      public string ulInitialCount { get; set; } = "";
      public string ulCount { get; set; } = "";
      public string ulRejectCount { get; set; } = "";
      public string ulMaximum { get; set; } = "";
      public string usPStatus { get; set; } = "";
      public string bHardwareSensor { get; set; } = "";
      public string ulDispensedCount { get; set; } = "";
      public string ulPresentedCount { get; set; } = "";
      public string ulRetractedCount { get; set; } = "";
   }

   public class WFSCDMCUINFO : SPLine
   {
      public int lUnitCount { get; set; }
      public string[] usNumbers { get; set; }
      public string[] usTypes { get; set; }
      public string[] cUnitIDs { get; set; }
      public string[] cCurrencyIDs { get; set; }
      public string[] ulValues { get; set; }
      public string[] ulInitialCounts { get; set; }
      public string[] ulCounts { get; set; }
      public string[] ulRejectCounts { get; set; }
      public string[] ulMinimums { get; set; }
      public string[] ulMaximums { get; set; }
      public string[] usStatuses { get; set; }
      public string[] ulDispensedCounts { get; set; }
      public string[] ulPresentedCounts { get; set; }
      public string[] ulRetractedCounts { get; set; }
      public string[] numPhysicalCUs { get; set; }
      public List<PhysicalCU> listPhysical { get; set; }
      public bool IsTruncated { get; private set; }

      public WFSCDMCUINFO(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CDM_CASH_UNIT_INFO) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         lUnitCount = 1;
         IsTruncated = false;

         // Initialize all array properties to empty arrays to avoid null references
         usNumbers = new string[0];
         usTypes = new string[0];
         cUnitIDs = new string[0];
         cCurrencyIDs = new string[0];
         ulValues = new string[0];
         ulInitialCounts = new string[0];
         ulCounts = new string[0];
         ulRejectCounts = new string[0];
         ulMinimums = new string[0];
         ulMaximums = new string[0];
         usStatuses = new string[0];
         ulDispensedCounts = new string[0];
         ulPresentedCounts = new string[0];
         ulRetractedCounts = new string[0];
         numPhysicalCUs = new string[0];
         listPhysical = new List<PhysicalCU>();

         // set some indecies - in part to help determine what type of log line we have
         // there are two forms - Table Form (e.g. WFS_INF_CDM_CASH_UNIT_INFO_1)
         // and List Form (e.g. WFS_INF_CDM_CASH_UNIT_INFO_3 and WFS_SRVE_CDM_CASHUNITINFOCHANGED_1)

         int indexOfTable = logLine.IndexOf("lppList->");
         int indexOfList = logLine.IndexOf("lppList =");

         // WFS_SRVE_CDM_CASHUNITINFOCHANGED doesnt have an 'lppList' so we'll use 'lpBuffer'
         // this works as long as we test indexOfTable first
         if (indexOfList == -1)
            indexOfList = logLine.IndexOf("lpBuffer =");

         int indexOfPhysical = logLine.IndexOf("lppPhysical");

         // Parsing for Table Form 
         if (indexOfTable > 0)
         {
            string logicalSubLogLine = logLine.Substring(indexOfTable);
            if (indexOfPhysical > 0)
            {
               logicalSubLogLine = logLine.Substring(indexOfTable, indexOfPhysical - indexOfTable);
            }

            // Table form parsing
            (bool success, string xfsMatch, string subLogLine) result = usCountFromTable(logLine, lUnitCount);
            lUnitCount = result.success ? int.Parse(result.xfsMatch.Trim()) : lUnitCount;

            usNumbers = usNumbersFromTable(logicalSubLogLine);
            usTypes = usTypesFromTable(logicalSubLogLine);
            cUnitIDs = cUnitIDsFromTable(logicalSubLogLine, lUnitCount);
            cCurrencyIDs = cCurrencyIDsFromTable(logicalSubLogLine, lUnitCount);
            ulValues = ulValuesFromTable(logicalSubLogLine);
            ulInitialCounts = ulInitialCountsFromTable(logicalSubLogLine);
            ulCounts = ulCountsFromTable(logicalSubLogLine);
            ulRejectCounts = ulRejectCountsFromTable(logicalSubLogLine);
            ulMinimums = ulMinimumsFromTable(logicalSubLogLine);
            ulMaximums = ulMaximumsFromTable(logicalSubLogLine);
            usStatuses = usStatusesFromTable(logicalSubLogLine);
            ulDispensedCounts = ulDispensedCountsFromTable(logicalSubLogLine);
            ulPresentedCounts = ulPresentedCountsFromTable(logicalSubLogLine);
            ulRetractedCounts = ulRetractedCountsFromTable(logicalSubLogLine);



            // Initialize listPhysical with defaults
            listPhysical = new List<PhysicalCU>();

            // Physical parsing for table form (only if lppPhysical-> exists)
            //int indexOfPhysical = result.subLogLine.IndexOf("lppPhysical->");
            if (indexOfPhysical >= 0)
            {
               string physicalSubLogLine = logLine.Substring(indexOfPhysical);
               numPhysicalCUs = usNumPhysicalCUsFromTable(physicalSubLogLine);

               string[] p_lpPhysicalPositionNames = lpPhysicalPositionNamesFromTable(physicalSubLogLine);
               string[] p_cUnitIDs = p_cUnitIDsFromTable(physicalSubLogLine);
               string[] p_ulInitialCounts = p_ulInitialCountsFromTable(physicalSubLogLine);
               string[] p_ulCounts = p_ulCountsFromTable(physicalSubLogLine);
               string[] p_ulRejectCounts = p_ulRejectCountsFromTable(physicalSubLogLine);
               string[] p_ulMaximums = p_ulMaximumsFromTable(physicalSubLogLine);
               string[] p_usPStatuses = p_usPStatusesFromTable(physicalSubLogLine);
               string[] p_bHardwareSensors = p_bHardwareSensorsFromTable(physicalSubLogLine);
               string[] p_ulDispensedCounts = p_ulDispensedCountsFromTable(physicalSubLogLine);
               string[] p_ulPresentedCounts = p_ulPresentedCountsFromTable(physicalSubLogLine);
               string[] p_ulRetractedCounts = p_ulRetractedCountsFromTable(physicalSubLogLine);

               for (int i = 0; i < lUnitCount; i++)
               {
                  int numPhysical = numPhysicalCUs[i] != null ? int.Parse(numPhysicalCUs[i]) : 0;
                  for (int j = 0; j < numPhysical; j++)
                  {
                     var pcu = new PhysicalCU
                     {
                        lpPhysicalPositionName = i < p_lpPhysicalPositionNames?.Length ? p_lpPhysicalPositionNames[i].Trim() : "",
                        cUnitID = i < p_cUnitIDs?.Length ? p_cUnitIDs[i].Trim() : "",
                        ulInitialCount = i < p_ulInitialCounts?.Length ? p_ulInitialCounts[i].Trim() : "",
                        ulCount = i < p_ulCounts?.Length ? p_ulCounts[i].Trim() : "",
                        ulRejectCount = i < p_ulRejectCounts?.Length ? p_ulRejectCounts[i].Trim() : "",
                        ulMaximum = i < p_ulMaximums?.Length ? p_ulMaximums[i].Trim() : "",
                        usPStatus = i < p_usPStatuses?.Length ? p_usPStatuses[i].Trim() : "",
                        bHardwareSensor = i < p_bHardwareSensors?.Length ? p_bHardwareSensors[i].Trim() : "",
                        ulDispensedCount = i < p_ulDispensedCounts?.Length ? p_ulDispensedCounts[i].Trim() : "",
                        ulPresentedCount = i < p_ulPresentedCounts?.Length ? p_ulPresentedCounts[i].Trim() : "",
                        ulRetractedCount = i < p_ulRetractedCounts?.Length ? p_ulRetractedCounts[i].Trim() : ""
                     };
                     listPhysical.Add(pcu);
                  }
               }
            }
         }

         // Parsing for List Form 
         else if (indexOfList >= 0 || logLine.Contains("usNumber = ["))
         {
            IsTruncated = logLine.Contains("...(More Data)..."); 

            // List form parsing with improved truncation handling
            (bool success, string xfsMatch, string subLogLine) result = usCountFromList(logLine, lUnitCount);
            lUnitCount = result.success ? int.Parse(result.xfsMatch.Trim()) : lUnitCount;

            // Pull the Logical Unit parts out of the log line - take out the Physical Unit parts so 
            // you only have logical unit settings, then iterate over the array pulling out individual settings
            string logicalSubLogLine = logLine.Substring(indexOfList);
            string[] logicalUnitParts = GetLogicalUnits(logicalSubLogLine, lUnitCount);

            // adjust lUnitCount because of log line truncation the last logical unit maybe garbage
            lUnitCount = logicalUnitParts.Length; 

            usNumbers = usNumbersFromList(logicalUnitParts, lUnitCount);
            usTypes = usTypesFromList(logicalUnitParts, lUnitCount);
            cUnitIDs = cUnitIDsFromList(logicalUnitParts, lUnitCount);
            cCurrencyIDs = cCurrencyIDsFromList(logicalUnitParts, lUnitCount);
            ulValues = ulValuesFromList(logicalUnitParts, lUnitCount);
            ulInitialCounts = ulInitialCountsFromList(logicalUnitParts, lUnitCount);
            ulCounts = ulCountsFromList(logicalUnitParts, lUnitCount);
            ulMaximums = ulMaximumsFromList(logicalUnitParts, lUnitCount);
            usStatuses = usStatusesFromList(logicalUnitParts, lUnitCount);
            ulDispensedCounts = ulDispensedCountsFromList(logicalUnitParts, lUnitCount);
            ulPresentedCounts = ulPresentedCountsFromList(logicalUnitParts, lUnitCount);
            ulRetractedCounts = ulRetractedCountsFromList(logicalUnitParts, lUnitCount);
            ulRejectCounts = ulRejectCountsFromList(logicalUnitParts, lUnitCount);
            ulMinimums = ulMinimumsFromList(logicalUnitParts, lUnitCount);
            numPhysicalCUs = usNumPhysicalCUsFromList(logicalUnitParts, lUnitCount);

            // each logical part reports how many physical parts it has, so sum those up. 
            int totalNumPhysical = 0;
            int value = 0;
            foreach (string numPhysical in numPhysicalCUs)
            {
               if (int.TryParse(numPhysical, out value))
                  totalNumPhysical = totalNumPhysical + value;
            }

            listPhysical = new List<PhysicalCU>();

            // pull the Physical Unit parts out of the log line - take out all the logical part settings, then build one long string
            // we have to treat physical parts slightly different from logical parts because the relationship could be 1:M

            string[] physicalUnitParts = GetPhysicalUnits(logicalSubLogLine, totalNumPhysical);
            string[] p_lpPhysicalPositionNames = lpPhysicalPositionNamesFromList(physicalUnitParts, totalNumPhysical);
            string[] p_cUnitIDs = p_cUnitIDsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulInitialCounts = p_ulInitialCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulCounts = p_ulCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulRejectCounts = p_ulRejectCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulMaximums = p_ulMaximumsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_usPStatuses = p_usPStatusesFromList(physicalUnitParts, totalNumPhysical);
            string[] p_bHardwareSensors = p_bHardwareSensorsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulDispensedCounts = p_ulDispensedCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulPresentedCounts = p_ulPresentedCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulRetractedCounts = p_ulRetractedCountsFromList(physicalUnitParts, totalNumPhysical);

            for (int i = 0; i < totalNumPhysical; i++)
            {
               try
               {
                  PhysicalCU pcu = new PhysicalCU();

                  pcu.lpPhysicalPositionName = string.IsNullOrEmpty(p_lpPhysicalPositionNames[i]) ? "" : p_lpPhysicalPositionNames[i].Trim();
                  pcu.cUnitID = string.IsNullOrEmpty(p_cUnitIDs[i]) ? "" : p_cUnitIDs[i].Trim();
                  pcu.ulInitialCount = string.IsNullOrEmpty(p_ulInitialCounts[i]) ? "" : p_ulInitialCounts[i].Trim();
                  pcu.ulCount = string.IsNullOrEmpty(p_ulCounts[i]) ? "" : p_ulCounts[i].Trim();
                  pcu.ulRejectCount = string.IsNullOrEmpty(p_ulRejectCounts[i]) ? "" : p_ulRejectCounts[i].Trim();
                  pcu.ulMaximum = string.IsNullOrEmpty(p_ulMaximums[i]) ? "" : p_ulMaximums[i].Trim();
                  pcu.usPStatus = string.IsNullOrEmpty(p_usPStatuses[i]) ? "" : p_usPStatuses[i].Trim();
                  pcu.bHardwareSensor = string.IsNullOrEmpty(p_bHardwareSensors[i]) ? "" : p_bHardwareSensors[i].Trim();
                  pcu.ulDispensedCount = string.IsNullOrEmpty(p_ulDispensedCounts[i]) ? "" : p_ulDispensedCounts[i].Trim();
                  pcu.ulPresentedCount = string.IsNullOrEmpty(p_ulPresentedCounts[i]) ? "" : p_ulPresentedCounts[i].Trim();
                  pcu.ulRetractedCount = string.IsNullOrEmpty(p_ulRetractedCounts[i]) ? "" : p_ulRetractedCounts[i].Trim();

                  listPhysical.Add(pcu);
               }
               catch (Exception e)
               {
                  Console.WriteLine("DEM Failed to add new PhysicalCU");
               }
            }
         }
      }


      private T[] ResizeArray<T>(T[] array, int newSize, T defaultValue = default)
      {
         if (array == null)
            return new T[newSize];

         var temp = array;
         Array.Resize(ref temp, newSize);
         for (int i = array.Length; i < newSize; i++)
         {
            temp[i] = defaultValue;
         }
         return temp;
      }

      // Table Access Methods
      protected static (bool success, string xfsMatch, string subLogLine) usCountFromTable(string logLine, int lUnitCount = 1) 
      { 
         return Util.Match(logLine, @"(?<=usCount=)(\d+)(?=\s*(?:\n|$))", lUnitCount.ToString()); 
      }

      protected static string[] usNumbersFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=usNumber\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] usTypesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=usType\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] cUnitIDsFromTable(string logLine, int lUnitCount = 1)
      {
         return Util.MatchTable(logLine, @"(?<=cUnitID\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] cCurrencyIDsFromTable(string logLine, int lUnitCount = 1)
      {
         return Util.MatchTable(logLine, @"(?<=cCurrencyID\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulValuesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulValues\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulCount\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] usStatusesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=usStatus\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulInitialCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulInitialCount\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulMinimumsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulMinimum\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulMaximumsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulMaximum\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulRejectCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulRejectCount\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulDispensedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulDispensedCount\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulPresentedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulPresentedCount\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] ulRetractedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulRetractedCount\s+)([^\n]*)(?=\s*(?:\n|$))");
      }

      protected static string[] usNumPhysicalCUsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=usNumPhysicalCUs\s+)((?:[0-9]+\s*)+)");
      }

      protected static string[] lpPhysicalPositionNamesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=lpPhysicalPositionName\s+)([^\n]+)");
      }

      protected static string[] p_cUnitIDsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=cUnitID\s+)([^\n]+)");
      }
      protected static string[] p_ulInitialCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulInitialCount\s+)([^\n]+)");
      }

      protected static string[] p_ulCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulCount\s+)([^\n]+)");
      }

      protected static string[] p_ulRejectCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulRejectCount\s+)([^\n]+)");
      }

      protected static string[] p_ulMaximumsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulMaximum\s+)([^\n]+)");
      }

      protected static string[] p_usPStatusesFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=usPStatus\s+)([^\n]+)");
      }

      protected static string[] p_bHardwareSensorsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=bHardwareSensor\s+)([^\n]+)");
      }

      protected static string[] p_ulDispensedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulDispensedCount\s+)([^\n]+)");
      }

      protected static string[] p_ulPresentedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulPresentedCount\s+)([^\n]+)");
      }

      protected static string[] p_ulRetractedCountsFromTable(string logLine)
      {
         return Util.MatchTable(logLine, @"(?<=ulRetractedCount\s+)([^\n]+)");
      }

      // List Access Methods
      protected static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine, int lUnitCount = 1)
      {
         return Util.Match(logLine, @"(?<=usCount\s*=\s*\[)(\d+)(?=\])", lUnitCount.ToString());
      }

      protected static string[] GetLogicalUnits(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);
         logicalUnits.thisUnit = Util.RemovePhysicalUnit(logicalUnits.thisUnit);

         for (int i = 0; i < lUnitCount; i++)
         {
            if (!logicalUnits.thisUnit.Contains("...(More Data)..."))
            {
               values.Add(logicalUnits.thisUnit);
            }
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
            logicalUnits.thisUnit = Util.RemovePhysicalUnit(logicalUnits.thisUnit);
         }
         return values.ToArray();
      }

      protected static string[] GetPhysicalUnits(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);
         List<string> physicalUnits = Util.ExtractPhysicalUnits(logicalUnits.thisUnit);
         foreach (string unit in physicalUnits)
         {
            //Console.WriteLine("DEM GetPhysicalUnits : " + unit);
            values.Add(unit);
            //Console.WriteLine("DEM values length : " + values.Count.ToString());
         }

         for (int i = 1; i < lUnitCount; i++)
         {
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
            physicalUnits = Util.ExtractPhysicalUnits(logicalUnits.thisUnit);
            foreach (string unit in physicalUnits)
            {
               //Console.WriteLine("DEM GetPhysicalUnits : " + unit);
               values.Add(unit);
               //Console.WriteLine("DEM values length : " + values.Count.ToString());
            }
         }
         return values.ToArray();
      }

      // iterate over the logical units and pull out - usNumber
      protected static string[] usNumbersFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.usNumber(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usType
      protected static string[] usTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.usType(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulInitialCounts
      protected static string[] ulInitialCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulInitialCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - cUnitIDs
      protected static string[] cUnitIDsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.cUnitID(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - cCurrencyIDs
      protected static string[] cCurrencyIDsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.cCurrencyID(part).xfsMatch/*.Trim()*/);  // preserve 3 spaces for no currency
         }
         return /*Util.TrimAll*/(Util.Resize(values.ToArray(), lUnitCount)); // preserve 3 spaces for no currency
      }

      // iterate over the logical units and pull out - ulValue
      protected static string[] ulValuesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulValue(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulCount
      protected static string[] ulCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulMaximum
      protected static string[] ulMaximumsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulMaximum(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulMinimum
      protected static string[] ulMinimumsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulMinimum(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulRejectCount
      protected static string[] ulRejectCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulRejectCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulDispensedCount
      protected static string[] ulDispensedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulDispensedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulPresentedCount
      protected static string[] ulPresentedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulPresentedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulRetractedCount
      protected static string[] ulRetractedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.ulRetractedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usStatus
      protected static string[] usStatusesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.usStatus(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usNumPhysicalCUs
      protected static string[] usNumPhysicalCUsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCDMCUINFO.usNumPhysicalCUs(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] lpPhysicalPositionNamesFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.lpPhysicalPositionName(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_cUnitIDsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.cUnitID(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulInitialCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.ulInitialCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.ulCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulRejectCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.ulRejectCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulMaximumsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.ulMaximum(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_usPStatusesFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.usPStatus(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_bHardwareSensorsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.bHardwareSensor(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulDispensedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.ulDispensedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulPresentedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.ulPresentedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulRetractedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCDMCUINFO.ulRetractedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }


      // Individual Accessors
      protected static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usNumber\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=cUnitID\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=cCurrencyID\s*=\s*\[)([a-zA-Z0-9 ]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulValue(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulValues\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulMaximum\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usStatus\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usType(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usType\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulInitialCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulMinimum\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulRejectCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulDispensedCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulPresentedCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulRetractedCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usNumPhysicalCUs(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usNumPhysicalCUs\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) lppPhysicalBlock(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=lppPhysical\s*=\s*\{)(.*?(?=}\s*(?:ulDispensedCount|}|{)))", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) physical_lpPhysicalPositionName(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=lpPhysicalPositionName\s*=\s*\[)([^]]*?)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) physical_cUnitID(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=cUnitID\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) physical_usPStatus(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usPStatus\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) physical_bHardwareSensor(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=bHardwareSensor\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) lpPhysicalPositionName(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=lpPhysicalPositionName\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usPStatus(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usPStatus\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bHardwareSensor(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=bHardwareSensor\s*=\s*\[)(\d+)(?=\])", "");
      }
   }
}
