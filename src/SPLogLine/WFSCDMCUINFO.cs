using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class PhysicalCDMCU
   {
      public string usNumPhysicalCU { get; set; } = "";
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

   public class WFSCDMCUINFO : WFSCUINFO
   {
      public int lUnitCount { get; set; }

      // LOGICAL

      public string[] usNumbers { get; set; }
      public string[] usTypes { get; set; }
      public string[] cUnitIDs { get; set; }
      public string[] cCurrencyIDs { get; set; }
      public string[] ulValues { get; set; }
      public string[] ulInitialCounts { get; set; }
      public string[] ulCounts { get; set; }
      public string[] bAppLocks { get; set; }
      public string[] ulRejectCounts { get; set; }
      public string[] ulMinimums { get; set; }
      public string[] ulMaximums { get; set; }
      public string[] usStatuses { get; set; }
      public string[] ulDispensedCounts { get; set; }
      public string[] ulPresentedCounts { get; set; }
      public string[] ulRetractedCounts { get; set; }
      public string[] usNumPhysicalCUss { get; set; }


      // PHYSICAL

      public List<PhysicalCDMCU> listPhysical { get; set; }

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
         bAppLocks = new string[0];
         ulRejectCounts = new string[0];
         ulMinimums = new string[0];
         ulMaximums = new string[0];
         usStatuses = new string[0];
         ulDispensedCounts = new string[0];
         ulPresentedCounts = new string[0];
         ulRetractedCounts = new string[0];
         usNumPhysicalCUss = new string[0];

         listPhysical = new List<PhysicalCDMCU>();

         // set some indecies - in part to help determine what type of log line we have
         // there are two forms - Table Form (e.g. WFS_INF_CDM_CASH_UNIT_INFO_1)
         // and List Form (e.g. WFS_INF_CDM_CASH_UNIT_INFO_3 and WFS_SRVE_CDM_CASHUNITINFOCHANGED_1)

         int indexOfTable = logLine.IndexOf("lppList->");
         int indexOfList = logLine.IndexOf("lppList =");
         int indexOfPhysical = logLine.IndexOf("lppPhysical");

         // WFS_SRVE_CDM_CASHUNITINFOCHANGED doesnt have an 'lppList' so we'll use 'lpBuffer'
         // this works as long as we test indexOfTable first
         if (indexOfList == -1)
            indexOfList = logLine.IndexOf("lpBuffer =");

         // Parsing for Table Form 
         if (indexOfTable > 0)
         {
            // Isolate the logical cash unit part of the table
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
            cUnitIDs = cUnitIDsFromTable(logicalSubLogLine);
            cCurrencyIDs = cCurrencyIDsFromTable(logicalSubLogLine);
            ulValues = ulValuesFromTable(logicalSubLogLine);
            ulInitialCounts = ulInitialCountsFromTable(logicalSubLogLine);
            ulCounts = ulCountsFromTable(logicalSubLogLine);
            bAppLocks = bAppLocksFromTable(logicalSubLogLine);
            ulRejectCounts = ulRejectCountsFromTable(logicalSubLogLine);
            ulMinimums = ulMinimumsFromTable(logicalSubLogLine);
            ulMaximums = ulMaximumsFromTable(logicalSubLogLine);
            usStatuses = usStatusesFromTable(logicalSubLogLine);
            ulDispensedCounts = ulDispensedCountsFromTable(logicalSubLogLine);
            ulPresentedCounts = ulPresentedCountsFromTable(logicalSubLogLine);
            ulRetractedCounts = ulRetractedCountsFromTable(logicalSubLogLine);

            // Physical parsing for table form (only if lppPhysical-> exists)
            //int indexOfPhysical = result.subLogLine.IndexOf("lppPhysical->");
            if (indexOfPhysical >= 0)
            {
               string physicalSubLogLine = logLine.Substring(indexOfPhysical);

               string[] p_numPhysicalCUs = usNumPhysicalCUsFromTable(physicalSubLogLine);
               string[] p_lpPhysicalPositionNames = lpPhysicalPositionNamesFromTable(physicalSubLogLine);
               string[] p_cUnitIDs = cUnitIDsFromTable(physicalSubLogLine);
               string[] p_ulInitialCounts = ulInitialCountsFromTable(physicalSubLogLine);
               string[] p_ulCounts = ulCountsFromTable(physicalSubLogLine);
               string[] p_ulRejectCounts = ulRejectCountsFromTable(physicalSubLogLine);
               string[] p_ulMaximums = ulMaximumsFromTable(physicalSubLogLine);
               string[] p_usPStatuses = usPStatusesFromTable(physicalSubLogLine);
               string[] p_bHardwareSensors = bHardwareSensorsFromTable(physicalSubLogLine);
               string[] p_ulDispensedCounts = ulDispensedCountsFromTable(physicalSubLogLine);
               string[] p_ulPresentedCounts = ulPresentedCountsFromTable(physicalSubLogLine);
               string[] p_ulRetractedCounts = ulRetractedCountsFromTable(physicalSubLogLine);

               for (int i = 0; i < p_numPhysicalCUs.Length; i++)
               {
                  var pcu = new PhysicalCDMCU
                  {
                     usNumPhysicalCU = i < p_numPhysicalCUs?.Length ? p_numPhysicalCUs[i].Trim() : "",
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
            usNumPhysicalCUss = usNumPhysicalCUsFromList(logicalUnitParts, lUnitCount);

            // each logical part reports how many physical parts it has, so sum those up. 
            int totalNumPhysical = 0;
            int value = 0;
            foreach (string numPhysical in usNumPhysicalCUss)
            {
               if (int.TryParse(numPhysical, out value))
                  totalNumPhysical = totalNumPhysical + value;
            }

            listPhysical = new List<PhysicalCDMCU>();

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
                  PhysicalCDMCU pcu = new PhysicalCDMCU();

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
   }
}
