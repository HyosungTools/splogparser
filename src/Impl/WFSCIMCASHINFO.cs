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
   // WFS_INF_CIM_CASH_UNIT_INFO
   // WFS_CMD_CIM_CASH_IN_END
   // WFS_CMD_CIM_RETRACT
   // WFS_USRE_CIM_CASHUNITTHRESHOLD
   // WFS_SRVE_CIM_CASHUNITINFOCHANGED

   public class WFSCIMCASHINFO : WFSCDMCIM
   {
      public int lUnitCount { get; set; }
      public string[] usNumbers { get; set; }
      public string[] fwTypes { get; set; }
      public string[] cUnitIDs { get; set; }
      public string[] cCurrencyIDs { get; set; }
      public string[] ulValues { get; set; }
      public string[] ulCashInCounts { get; set; }
      public string[] ulCounts { get; set; }
      public string[] ulMaximums { get; set; }
      public string[] usStatuses { get; set; }
      public string[,] noteNumbers { get; set; }
      public string[] ulInitialCounts { get; set; }
      public string[] ulDispensedCounts { get; set; }
      public string[] ulPresentedCounts { get; set; }
      public string[] ulRetractedCounts { get; set; }
      public string[] ulRejectCounts { get; set; }
      public string[] ulMinimums { get; set; }

      public WFSCIMCASHINFO(IContext ctx) : base(ctx)
      {
         lUnitCount = 1;
      }

      public string Initialize(string nwLogLine)
      {
         int indexOfTable = nwLogLine.IndexOf("lppCashIn->");
         int indexOfList = nwLogLine.IndexOf("lppCashIn ="); 

         if (indexOfTable > 0)
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = _wfs_cim_cash_info.usCountFromTable(nwLogLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = _wfs_cim_cash_info.usNumbersFromTable(result.subLogLine);
            fwTypes = _wfs_cim_cash_info.fwTypesFromTable(result.subLogLine);
            cUnitIDs = _wfs_cim_cash_info.cUnitIDsFromTable(result.subLogLine, lUnitCount);
            cCurrencyIDs = _wfs_cim_cash_info.cCurrencyIDsFromTable(result.subLogLine, lUnitCount);
            ulValues = _wfs_cim_cash_info.ulValuesFromTable(result.subLogLine);
            ulCashInCounts = _wfs_cim_cash_info.ulCashInCountsFromTable(result.subLogLine);
            ulCounts = _wfs_cim_cash_info.ulCountsFromTable(result.subLogLine);
            ulMaximums = _wfs_cim_cash_info.ulMaximumsFromTable(result.subLogLine);
            usStatuses = _wfs_cim_cash_info.usStatusesFromTable(result.subLogLine);
            noteNumbers = _wfs_cim_cash_info.noteNumberListFromTable(result.subLogLine, lUnitCount);
            ulInitialCounts = _wfs_cim_cash_info.ulInitialCountsFromTable(result.subLogLine);
            ulDispensedCounts = _wfs_cim_cash_info.ulDispensedCountsFromTable(result.subLogLine);
            ulPresentedCounts = _wfs_cim_cash_info.ulPresentedCountsFromTable(result.subLogLine);
            ulRetractedCounts = _wfs_cim_cash_info.ulRetractedCountsFromTable(result.subLogLine);
            ulRejectCounts = _wfs_cim_cash_info.ulRejectCountsFromTable(result.subLogLine);
            ulMinimums = _wfs_cim_cash_info.ulMinimumsFromTable(result.subLogLine);
         }
         else if (indexOfTable > 0 || nwLogLine.Contains("usNumber = ["))
         {
            // isolate count (e.g usCount=7) default to 1. 
            (bool success, string xfsMatch, string subLogLine) result = _wfs_cim_cash_info.usCountFromList(nwLogLine, lUnitCount);
            lUnitCount = int.Parse(result.xfsMatch.Trim());

            usNumbers = _wfs_cim_cash_info.usNumbersFromList(result.subLogLine, lUnitCount);
            fwTypes = _wfs_cim_cash_info.fwTypesFromList(result.subLogLine, lUnitCount);
            cUnitIDs = _wfs_cim_cash_info.cUnitIDsFromList(result.subLogLine, lUnitCount);
            cCurrencyIDs = _wfs_cim_cash_info.cCurrencyIDsFromList(result.subLogLine, lUnitCount);
            ulValues = _wfs_cim_cash_info.ulValuesFromList(result.subLogLine, lUnitCount);
            ulCashInCounts = _wfs_cim_cash_info.ulCashInCountsFromList(result.subLogLine, lUnitCount);
            ulCounts = _wfs_cim_cash_info.ulCountsFromList(result.subLogLine, lUnitCount);
            ulMaximums = _wfs_cim_cash_info.ulMaximumsFromList(result.subLogLine, lUnitCount);
            usStatuses = _wfs_cim_cash_info.usStatusesFromList(result.subLogLine, lUnitCount);
            noteNumbers = _wfs_cim_cash_info.noteNumberListFromList(result.subLogLine, lUnitCount);
            ulInitialCounts = _wfs_cim_cash_info.ulInitialCountsFromList(result.subLogLine, lUnitCount);
            ulDispensedCounts = _wfs_cim_cash_info.ulDispensedCountsFromList(result.subLogLine, lUnitCount);
            ulPresentedCounts = _wfs_cim_cash_info.ulPresentedCountsFromList(result.subLogLine, lUnitCount);
            ulRetractedCounts = _wfs_cim_cash_info.ulRetractedCountsFromList(result.subLogLine, lUnitCount);
            ulRejectCounts = _wfs_cim_cash_info.ulRejectCountsFromList(result.subLogLine, lUnitCount);
            ulMinimums = _wfs_cim_cash_info.ulMinimumsFromList(result.subLogLine, lUnitCount);
         }
         else
         {
            ctx.ConsoleWriteLogLine("EXCEPTION in WFSCIMCASHINFO Initialize, unexpected log line: " + nwLogLine);
         }

         return nwLogLine;
      }
   }
}
