using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_INF_CDM_CASH_UNIT_INFO
   // WFS_USRE_CDM_CASHUNITTHRESHOLD
   // WFS_SRVE_CDM_CASHUNITINFOCHANGED
   // WFS_EXEE_CDM_CASHUNITERROR

   public class WFSCDMCUINFO : WFSCDMCIM
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

      public WFSCDMCUINFO(IContext ctx) : base(ctx)
      {
         lUnitCount = 1;
      }

      public string Initialize(string nwLogLine)
      {
         int indexOfTable = nwLogLine.IndexOf("lppList->");
         int indexOfList = nwLogLine.IndexOf("lppList =");

         if (indexOfTable > 0)
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = _wfs_cim_cash_info.usCountFromTable(nwLogLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = _wfs_cim_cash_info.usNumbersFromTable(result.subLogLine);
            usTypes = _wfs_cdm_cu_info.usTypesFromTable(result.subLogLine);
            cUnitIDs = _wfs_cdm_cu_info.cUnitIDsFromTable(result.subLogLine, lUnitCount);
            cCurrencyIDs = _wfs_cdm_cu_info.cCurrencyIDsFromTable(result.subLogLine, lUnitCount);
            ulValues = _wfs_cdm_cu_info.ulValuesFromTable(result.subLogLine);
            ulInitialCounts = _wfs_cdm_cu_info.ulInitialCountsFromTable(result.subLogLine);
            ulCounts = _wfs_cdm_cu_info.ulCountsFromTable(result.subLogLine);
            ulMaximums = _wfs_cdm_cu_info.ulMaximumsFromTable(result.subLogLine);
            usStatuses = _wfs_cdm_cu_info.usStatusesFromTable(result.subLogLine);
            ulInitialCounts = _wfs_cdm_cu_info.ulInitialCountsFromTable(result.subLogLine);
            ulDispensedCounts = _wfs_cdm_cu_info.ulDispensedCountsFromTable(result.subLogLine);
            ulPresentedCounts = _wfs_cdm_cu_info.ulPresentedCountsFromTable(result.subLogLine);
            ulRetractedCounts = _wfs_cdm_cu_info.ulRetractedCountsFromTable(result.subLogLine);
            ulRejectCounts = _wfs_cdm_cu_info.ulRejectCountsFromTable(result.subLogLine);
            ulMinimums = _wfs_cdm_cu_info.ulMinimumsFromTable(result.subLogLine);
         }
         else if (indexOfTable > 0 || nwLogLine.Contains("usNumber = ["))
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = _wfs_cim_cash_info.usCountFromList(nwLogLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = _wfs_cdm_cu_info.usNumbersFromList(result.subLogLine, lUnitCount);
            usTypes = _wfs_cdm_cu_info.usTypesFromList(result.subLogLine, lUnitCount);
            cUnitIDs = _wfs_cdm_cu_info.cUnitIDsFromList(result.subLogLine, lUnitCount);
            cCurrencyIDs = _wfs_cdm_cu_info.cCurrencyIDsFromList(result.subLogLine, lUnitCount);
            ulValues = _wfs_cdm_cu_info.ulValuesFromList(result.subLogLine, lUnitCount);
            ulInitialCounts = _wfs_cdm_cu_info.ulInitialCountsFromList(result.subLogLine, lUnitCount);
            ulCounts = _wfs_cdm_cu_info.ulCountsFromList(result.subLogLine, lUnitCount);
            ulMaximums = _wfs_cdm_cu_info.ulMaximumsFromList(result.subLogLine, lUnitCount);
            usStatuses = _wfs_cdm_cu_info.usStatusesFromList(result.subLogLine, lUnitCount);
            ulInitialCounts = _wfs_cdm_cu_info.ulInitialCountsFromList(result.subLogLine, lUnitCount);
            ulDispensedCounts = _wfs_cdm_cu_info.ulDispensedCountsFromList(result.subLogLine, lUnitCount);
            ulPresentedCounts = _wfs_cdm_cu_info.ulPresentedCountsFromList(result.subLogLine, lUnitCount);
            ulRetractedCounts = _wfs_cdm_cu_info.ulRetractedCountsFromList(result.subLogLine, lUnitCount);
            ulRejectCounts = _wfs_cdm_cu_info.ulRejectCountsFromList(result.subLogLine, lUnitCount);
            ulMinimums = _wfs_cdm_cu_info.ulMinimumsFromList(result.subLogLine, lUnitCount);
         }
         else
         {
            ctx.ConsoleWriteLogLine("EXCEPTION in WFSCIMCASHINFO Initialize, unexpected log line: " + nwLogLine);
         }

         return nwLogLine;
      }
   }
}
