using System;
using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class PhysicalCIMCU
   {
      public string usNumPhysicalCU { get; set; } = "";
      public string lpPhysicalPositionName { get; set; } = "";
      public string cUnitID { get; set; } = "";
      public string ulCashInCount { get; set; } = "";
      public string ulCount { get; set; } = "";
      public string ulMaximum { get; set; } = "";
      public string usPStatus { get; set; } = "";
      public string bHardwareSensor { get; set; } = "";
      public string ulInitialCount { get; set; } = "";
      public string ulDispensedCount { get; set; } = "";
      public string ulPresentedCount { get; set; } = "";
      public string ulRetractedCount { get; set; } = "";
      public string ulRejectCount { get; set; } = "";
      public string lpszExtra { get; set; } = "";
   }

   public class WFSCIMCASHINFO : WFSCUINFO
   {
      public int lUnitCount { get; set; }

      // LOGICAL

      public string[] usNumbers { get; set; }
      public string[] fwTypes { get; set; }
      public string[] fwItemTypes { get; set; }
      public string[] cUnitIDs { get; set; }
      public string[] cCurrencyIDs { get; set; }
      public string[] ulValues { get; set; }
      public string[] ulCashInCounts { get; set; }
      public string[] ulCounts { get; set; }
      public string[] ulMaximums { get; set; }
      public string[] usStatuses { get; set; }
      public string[] bAppLocks { get; set; }
      public string[] usNumPhysicalCUss { get; set; }


      // PHYSICAL

      public List<PhysicalCIMCU> listPhysical { get; set; }

      // NOTE NUMBERS

      public string[,] noteNumbers { get; set; }

      // LCU ETC

      public string[] usCDMTypes { get; set; }
      public string[] lpszCashUnitNames { get; set; }
      public string[] ulInitialCounts { get; set; }
      public string[] ulDispensedCounts { get; set; }
      public string[] ulPresentedCounts { get; set; }
      public string[] ulRetractedCounts { get; set; }
      public string[] ulRejectCounts { get; set; }
      public string[] ulMinimums { get; set; }
      public string[] lpszExtraLCUs { get; set; }

      public bool IsTruncated { get; private set; }


      public WFSCIMCASHINFO(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CIM_CASH_UNIT_INFO) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         lUnitCount = 1;
         IsTruncated = false;

         // Initialize all array properties to empty arrays to avoid null references
         usNumbers = new string[0];
         fwTypes = new string[0];
         fwItemTypes = new string[0];
         cUnitIDs = new string[0];
         cCurrencyIDs = new string[0];
         ulValues = new string[0];
         ulCashInCounts = new string[0];
         ulCounts = new string[0];
         ulMaximums = new string[0];
         usStatuses = new string[0];
         bAppLocks = new string[0];
         usNumPhysicalCUss = new string[0];

         usCDMTypes = new string[0];
         lpszCashUnitNames = new string[0];
         ulInitialCounts = new string[0];
         ulDispensedCounts = new string[0];
         ulPresentedCounts = new string[0];
         ulRetractedCounts = new string[0];
         ulRejectCounts = new string[0];
         ulMinimums = new string[0];
         lpszExtraLCUs = new string[0]; 

         listPhysical = new List<PhysicalCIMCU>();

         // set some indecies - in part to help determine what type of log line we have
         // there are two forms - Table Form and List Form 

         int indexOfTable = logLine.IndexOf("lppCashIn->");
         int indexOfList = logLine.IndexOf("lppCashIn =");
         int indexOfNull = logLine.IndexOf("lppCashIn=NULL");
         int indexOfPhysical = logLine.IndexOf("lppPhysical");
         int indexOfNoteNumber = logLine.IndexOf("lpNoteNumberList->");
         int indexOfLCUETC = logLine.IndexOf("LCU ETC");

         // T A B L E  F O R M

         if (indexOfTable > 0)
         {
            // L O G I C A L

            string logicalSubLogLine = logLine.Substring(indexOfTable);
            if (indexOfPhysical > 0)
            {
               logicalSubLogLine = logLine.Substring(indexOfTable, indexOfPhysical - indexOfTable);
            }

            // Table form parsing
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = usCountFromTable(logLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = usNumbersFromTable(logicalSubLogLine);
            fwTypes = fwTypesFromTable(logicalSubLogLine);
            fwItemTypes = fwItemTypesFromTable(logicalSubLogLine);
            cUnitIDs = cUnitIDsFromTable(logicalSubLogLine);
            cCurrencyIDs = cCurrencyIDsFromTable(logicalSubLogLine);
            ulValues = ulValuesFromTable(logicalSubLogLine);
            ulCashInCounts = ulCashInCountsFromTable(logicalSubLogLine);
            ulCounts = ulCountsFromTable(logicalSubLogLine);
            ulMaximums = ulMaximumsFromTable(logicalSubLogLine);
            usStatuses = usStatusesFromTable(logicalSubLogLine);
            bAppLocks = bAppLocksFromTable(logicalSubLogLine);

            // Initialize listPhysical with defaults
            listPhysical = new List<PhysicalCIMCU>();

            // P H Y S I C A L

            if (indexOfPhysical >= 0)
            {
               string physicalSubLogLine = logLine.Substring(indexOfPhysical);
               if (indexOfNoteNumber > 0)
               {
                  physicalSubLogLine = logLine.Substring(indexOfPhysical, indexOfNoteNumber - indexOfPhysical);
               }

               string[] p_numPhysicalCUs = usNumPhysicalCUsFromTable(physicalSubLogLine);
               string[] p_lpPhysicalPositionNames = lpPhysicalPositionNamesFromTable(physicalSubLogLine);
               string[] p_cUnitIDs = cUnitIDsFromTable(physicalSubLogLine);
               string[] p_ulCashInCount = ulCashInCountFromTable(physicalSubLogLine);
               string[] p_ulMaximums = ulMaximumsFromTable(physicalSubLogLine);
               string[] p_ulCounts = ulCountsFromTable(physicalSubLogLine);
               string[] p_usPStatuses = usPStatusesFromTable(physicalSubLogLine);
               string[] p_bHardwareSensors = bHardwareSensorsFromTable(physicalSubLogLine);
               string[] p_ulInitialCounts = ulInitialCountsFromTable(physicalSubLogLine);
               string[] p_ulDispensedCounts = ulDispensedCountsFromTable(physicalSubLogLine);
               string[] p_ulPresentedCounts = ulPresentedCountsFromTable(physicalSubLogLine);
               string[] p_ulRetractedCounts = ulRetractedCountsFromTable(physicalSubLogLine);
               string[] p_ulRejectCounts = ulRejectCountsFromTable(physicalSubLogLine);
               string[] p_lpszExtras = lpszExtrasFromTable(physicalSubLogLine);

               for (int i = 0; i < p_numPhysicalCUs.Length; i++)
               {
                  var pcu = new PhysicalCIMCU
                  {
                     usNumPhysicalCU = i < p_numPhysicalCUs?.Length ? p_numPhysicalCUs[i].Trim() : "",
                     lpPhysicalPositionName = i < p_lpPhysicalPositionNames?.Length ? p_lpPhysicalPositionNames[i].Trim() : "",
                     cUnitID = i < p_cUnitIDs?.Length ? p_cUnitIDs[i].Trim() : "",
                     ulCashInCount = i < p_ulCashInCount?.Length ? p_ulCashInCount[i].Trim() : "",
                     ulCount = i < p_ulCounts?.Length ? p_ulCounts[i].Trim() : "",
                     ulMaximum = i < p_ulMaximums?.Length ? p_ulMaximums[i].Trim() : "",
                     usPStatus = i < p_usPStatuses?.Length ? p_usPStatuses[i].Trim() : "",
                     bHardwareSensor = i < p_bHardwareSensors?.Length ? p_bHardwareSensors[i].Trim() : "",
                     ulInitialCount = i < p_ulInitialCounts?.Length ? p_ulInitialCounts[i].Trim() : "",
                     ulDispensedCount = i < p_ulDispensedCounts?.Length ? p_ulDispensedCounts[i].Trim() : "",
                     ulPresentedCount = i < p_ulPresentedCounts?.Length ? p_ulPresentedCounts[i].Trim() : "",
                     ulRetractedCount = i < p_ulRetractedCounts?.Length ? p_ulRetractedCounts[i].Trim() : "",
                     ulRejectCount = i < p_ulRejectCounts?.Length ? p_ulRejectCounts[i].Trim() : "",
                     lpszExtra = i < p_lpszExtras?.Length ? p_lpszExtras[i].Trim() : ""
                  };

                  listPhysical.Add(pcu);
               }
            }

            // N O T E   N U M B E R S

            if (indexOfNoteNumber > 0)
            {
               noteNumbers = noteNumberListFromTable(logLine, lUnitCount);
            }

            // L C U   E T C

            if (indexOfLCUETC >= 0)
            {
               string lcuEtcLogLine = logLine.Substring(indexOfLCUETC);

               usCDMTypes = usCDMTypesFromTable(lcuEtcLogLine);
               lpszCashUnitNames = lpszCashUnitNamesFromTable(lcuEtcLogLine);
               ulInitialCounts = ulInitialCountsFromTable(lcuEtcLogLine);
               ulDispensedCounts = ulDispensedCountsFromTable(lcuEtcLogLine);
               ulPresentedCounts = ulPresentedCountsFromTable(lcuEtcLogLine);
               ulRetractedCounts = ulRetractedCountsFromTable(lcuEtcLogLine);
               ulRejectCounts = ulRejectCountsFromTable(lcuEtcLogLine);
               ulMinimums = ulMinimumsFromTable(lcuEtcLogLine);
               lpszExtraLCUs = lpszExtrasFromTable(lcuEtcLogLine); 
            }
         }

         // L I S T  F O R M

         // Parsing for List Form 
         else if (indexOfList >= 0 || logLine.Contains("usNumber = ["))
         {
            // L O G I C A L

            IsTruncated = logLine.Contains("...(More Data)...");

            // List form parsing with improved truncation handling
            (bool success, string xfsMatch, string subLogLine) result = usCountFromList(logLine, lUnitCount);
            lUnitCount = result.success ? int.Parse(result.xfsMatch.Trim()) : lUnitCount;

            // If the log is truncated reduce the number of LCU by 1
            if (IsTruncated)
            {
               lUnitCount--; 
            }

            // Pull the Logical Unit parts out of the log line - take out the Physical and Note Number parts so 
            // you only have logical unit settings, then iterate over the array pulling out individual settings
            string logicalSubLogLine = logLine.Substring(indexOfList);
            string[] logicalUnitParts = GetLogicalUnits(logicalSubLogLine, lUnitCount);

            usNumbers = usNumbersFromList(logicalUnitParts, lUnitCount);
            fwTypes = fwTypesFromList(logicalUnitParts, lUnitCount); 
            fwItemTypes = fwItemTypesFromList(logicalUnitParts, lUnitCount); 
            cUnitIDs = cUnitIDsFromList(logicalUnitParts, lUnitCount); 
            cCurrencyIDs = cCurrencyIDsFromList(logicalUnitParts, lUnitCount); 
            ulValues = ulValuesFromList(logicalUnitParts, lUnitCount);
            ulCashInCounts = ulCashInCountsFromList(logicalUnitParts, lUnitCount); 
            ulCounts = ulCountsFromList(logicalUnitParts, lUnitCount); 
            ulMaximums = ulMaximumsFromList(logicalUnitParts, lUnitCount); 
            usStatuses = usStatusesFromList(logicalUnitParts, lUnitCount); 
            bAppLocks = bAppLocksFromList(logicalUnitParts, lUnitCount); 
            usNumPhysicalCUss = usNumPhysicalCUsFromList(logicalUnitParts, lUnitCount);

            usCDMTypes = usCDMTypesFromList(logicalUnitParts, lUnitCount); 
            lpszCashUnitNames = lpszCashUnitNamesFromList(logicalUnitParts, lUnitCount);
            ulInitialCounts = ulInitialCountsFromList(logicalUnitParts, lUnitCount); 
            ulDispensedCounts = ulDispensedCountsFromList(logicalUnitParts, lUnitCount); 
            ulPresentedCounts = ulPresentedCountsFromList(logicalUnitParts, lUnitCount); 
            ulRetractedCounts = ulRetractedCountsFromList(logicalUnitParts, lUnitCount); 
            ulRejectCounts = ulRejectCountsFromList(logicalUnitParts, lUnitCount); 
            ulMinimums = ulMinimumsFromList(logicalUnitParts, lUnitCount);
            lpszExtraLCUs = lpszExtrasFromList(logicalUnitParts, lUnitCount);

            noteNumbers = noteNumberListFromList(logicalUnitParts, lUnitCount);

            // each logical part reports how many physical parts it has, so sum those up. 
            int totalNumPhysical = 0;
            int value = 0;
            foreach (string numPhysical in usNumPhysicalCUss)
            {
               if (int.TryParse(numPhysical, out value))
                  totalNumPhysical = totalNumPhysical + value;
            }

            listPhysical = new List<PhysicalCIMCU>();

            // pull the Physical Unit parts out of the log line - take out all the logical part settings, then build one long string
            // we have to treat physical parts slightly different from logical parts because the relationship could be 1:M

            string[] physicalUnitParts = GetPhysicalUnits(logicalSubLogLine, totalNumPhysical);
            totalNumPhysical = physicalUnitParts.Length; 

            string[] p_lpPhysicalPositionNames = lpPhysicalPositionNamesFromList(physicalUnitParts, totalNumPhysical);
            string[] p_cUnitIDs = p_cUnitIDsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulCashInCounts = p_ulCashInCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulCounts = p_ulCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulMaximums = p_ulMaximumsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_usPStatuses = p_usPStatusesFromList(physicalUnitParts, totalNumPhysical);
            string[] p_bHardwareSensors = p_bHardwareSensorsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_lpszExtras = p_lpszExtrasFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulInitialCounts = p_ulInitialCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulDispensedCounts = p_ulDispensedCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulPresentedCounts = p_ulPresentedCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulRetractedCounts = p_ulRetractedCountsFromList(physicalUnitParts, totalNumPhysical);
            string[] p_ulRejectCounts = p_ulRejectCountsFromList(physicalUnitParts, totalNumPhysical);

            for (int i = 0; i < totalNumPhysical; i++)
            {
               try
               {
                  PhysicalCIMCU pcu = new PhysicalCIMCU();

                  pcu.lpPhysicalPositionName = string.IsNullOrEmpty(p_lpPhysicalPositionNames[i]) ? "" : p_lpPhysicalPositionNames[i].Trim();
                  pcu.cUnitID = string.IsNullOrEmpty(p_cUnitIDs[i]) ? "" : p_cUnitIDs[i].Trim();
                  pcu.ulCashInCount = string.IsNullOrEmpty(p_ulCashInCounts[i]) ? "" : p_ulCashInCounts[i].Trim();
                  pcu.ulCount = string.IsNullOrEmpty(p_ulCounts[i]) ? "" : p_ulCounts[i].Trim();
                  pcu.ulMaximum = string.IsNullOrEmpty(p_ulMaximums[i]) ? "" : p_ulMaximums[i].Trim();
                  pcu.usPStatus = string.IsNullOrEmpty(p_usPStatuses[i]) ? "" : p_usPStatuses[i].Trim();
                  pcu.bHardwareSensor = string.IsNullOrEmpty(p_bHardwareSensors[i]) ? "" : p_bHardwareSensors[i].Trim();
                  pcu.lpszExtra = string.IsNullOrEmpty(p_lpszExtras[i]) ? "" : p_lpszExtras[i].Trim();
                  pcu.ulInitialCount = string.IsNullOrEmpty(p_ulInitialCounts[i]) ? "" : p_ulInitialCounts[i].Trim();
                  pcu.ulDispensedCount = string.IsNullOrEmpty(p_ulDispensedCounts[i]) ? "" : p_ulDispensedCounts[i].Trim();
                  pcu.ulPresentedCount = string.IsNullOrEmpty(p_ulPresentedCounts[i]) ? "" : p_ulPresentedCounts[i].Trim();
                  pcu.ulRetractedCount = string.IsNullOrEmpty(p_ulRetractedCounts[i]) ? "" : p_ulRetractedCounts[i].Trim();
                  pcu.ulRejectCount = string.IsNullOrEmpty(p_ulRejectCounts[i]) ? "" : p_ulRejectCounts[i].Trim();

                  listPhysical.Add(pcu);
               }
               catch (Exception e)
               {
                  Console.WriteLine("Failed to add new PhysicalCU");
               }
            }
         }
         else if (indexOfNull > 0)
         {
            lUnitCount = 0; 
         }
      }
   }
}
