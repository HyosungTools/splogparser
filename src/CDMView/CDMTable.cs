using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;

namespace CDMView
{

   internal class CDMTable : BaseTable
   {
      private bool seenFirstFullCashUnitInfo = false;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CDMTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = false;
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);

            switch (result.xfsType)
            {
               case XFSType.WFS_INF_CDM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_IN_CDM_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CDM_CASH_UNIT_INFO:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_CDM_CASH_UNIT_INFO(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CDM_DISPENSE:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_DISPENSE");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_PRESENT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_PRESENT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_REJECT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_REJECT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RETRACT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RETRACT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RESET:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RESET");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_ITEMSTAKEN:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_EXEE_CIM_INPUTREFUSE");
                     break;
                  }
               default:
                  break;

            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CDMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         //STATUS TABLE

         // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
         ctx.ConsoleWriteLogLine("Compress the Status Table: sort by time, visit every row and delete rows that are unchanged from their predecessor");
         ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table start: rows before: {0}", dTableSet.Tables["Status"].Rows.Count));

         // the list of columns to compare
         string[] columns = new string[] { "error", "status", "dispenser", "intstack", "shutter", "posstatus", "transport", "transstat", "position" };
         (bool success, string message) result = DataTableOps.DeleteUnchangedRowsInTable(dTableSet.Tables["Status"], "time ASC", columns);
         if (!result.success)
         {
            ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
         }
         ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table complete: rows after: {0}", dTableSet.Tables["Status"].Rows.Count));

         // add English
         string[,] colKeyMap = new string[8, 2]
         {
            {"status", "fwDevice" },
            {"dispenser", "fwDispenser"},
            {"intstack", "fwIntermediateStacker"},
            {"shutter", "fwShutter"},
            {"posstatus", "fwPositionStatus"},
            {"transport", "fwTransport"},
            {"transstat", "fwTransportStatus"},
            {"position", "wDevicePosition"}
         };

         for (int i = 0; i < 8; i++)
         {
            result = DataTableOps.AddEnglishToTable(dTableSet.Tables["Status"], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
         }


         // CashUnit Table
         ctx.ConsoleWriteLogLine("Compress the CashUnit Tables: sort by time, visit every row and delete rows that are unchanged from their predecessor");
 
         // the list of columns to compare
         string[] cashUnitCols = new string[] { "error", "status", "count", "reject", "dispensed", "presented", "retracted" };

         foreach(DataTable dTable in dTableSet.Tables)
         {
            ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
            if (dTable.TableName.StartsWith("CashUnit-"))
            {
               ctx.ConsoleWriteLogLine(String.Format("Compress the Table '{0}' rows before: {1}", dTable.TableName, dTable.Rows.Count));
               (bool success, string message) cashUnitResult = DataTableOps.DeleteUnchangedRowsInTable(dTable, "time ASC", cashUnitCols);
               if (!cashUnitResult.success)
               {
                  ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + cashUnitResult.message);
               }
               ctx.ConsoleWriteLogLine(String.Format("Compress the Table '{0}' rows after: {1}", dTable.TableName, dTable.Rows.Count));
            }
         }

         // add English 
         string[,] summaryColMap = new string[1, 2]
         {
            {"type", "usType" }
         };

         for (int i = 0; i < 1; i++)
         {
            result = DataTableOps.AddEnglishToTable(dTableSet.Tables["Summary"], dTableSet.Tables["Messages"], summaryColMap[i, 0], summaryColMap[i, 1]);
         }

         // add English
         string[,] cashUnitColMap = new string[1, 2]
         {
            {"status", "usStatus" }
         };

         foreach (DataTable dTable in dTableSet.Tables)
         {
            ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
            if (dTable.TableName.StartsWith("CashUnit-"))
            {
               for (int i = 0; i < 1; i++)
               {
                  result = DataTableOps.AddEnglishToTable(dTable, dTableSet.Tables["Messages"], cashUnitColMap[i, 0], cashUnitColMap[i, 1]);
               }
            }
         }

         return base.WriteExcelFile();
      }

      protected (bool success, DataRow dataRow) FindMessages(string type, string code)
      {
         // Create an array for the key values to find.
         object[] findByKeys = new object[2];

         // Set the values of the keys to find.
         findByKeys[0] = type;
         findByKeys[1] = code;

         DataRow foundRow = dTableSet.Tables["Messages"].Rows.Find(findByKeys);
         if (foundRow != null)
         {
            return (true, foundRow);
         }
         else
         {
            return (false, null);
         }
      }
      protected void WFS_IN_CDM_STATUS(string xfsLine)
      {
         try
         {
            DataRow newRow = dTableSet.Tables["Status"].NewRow();

            newRow["file"] = _traceFile;
            newRow["time"] = lpResult.tsTimestamp(xfsLine);
            newRow["error"] = lpResult.hResult(xfsLine);

            (bool success, string xfsMatch, string subLogLine) result;

            // fwDevice
            result = _wfs_inf_cdm_status.fwDevice(xfsLine);
            if (result.success) newRow["status"] = result.xfsMatch.Trim();

            // fwDispenser
            result = _wfs_inf_cdm_status.fwDispenser(result.subLogLine);
            if (result.success) newRow["dispenser"] = result.xfsMatch.Trim();

            // fwIntermediateStacker
            result = _wfs_inf_cdm_status.fwIntermediateStacker(result.subLogLine);
            if (result.success) newRow["intstack"] = result.xfsMatch.Trim();

            // fwShutter
            result = _wfs_inf_cdm_status.fwShutter(result.subLogLine);
            if (result.success) newRow["shutter"] = result.xfsMatch.Trim();

            // fwPositionStatus
            result = _wfs_inf_cdm_status.fwPositionStatus(result.subLogLine);
            if (result.success) newRow["posstatus"] = result.xfsMatch.Trim();

            // fwTransport
            result = _wfs_inf_cdm_status.fwTransport(result.subLogLine);
            if (result.success) newRow["transport"] = result.xfsMatch.Trim();

            // fwTransportStatus
            result = _wfs_inf_cdm_status.fwTransportStatus(result.subLogLine);
            if (result.success) newRow["transstat"] = result.xfsMatch.Trim();

            // wDevicePosition
            result = _wfs_inf_cdm_status.wDevicePosition(result.subLogLine);
            if (result.success) newRow["position"] = result.xfsMatch.Trim();

            dTableSet.Tables["Status"].Rows.Add(newRow);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("Exception : " + e.Message);
         }

         return;
      }

      protected void WFS_INF_CDM_CASH_UNIT_INFO(string xfsLine)
      {
         try
         {
            // sometimes we get a single value back, sometimes we get a list
            (bool success, string xfsMatch, string subLogLine) result;
            (bool success, string[] xfsMatch, string subLogLine) results;

            // there are two styles of log lines. If the log line contains 'lppList->' expect
            // a table of data. If the log lines contains 'lppList =' expect a list
            if (xfsLine.Contains("lppList->"))
            {
               // isolate count
               result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
               if (!result.success)
               {
                  ctx.ConsoleWriteLogLine("Failed to isolate count from WFS_INF_CDM_CASH_UNIT_INFO message");
                  return;
               }

               // how many logical units in the table. 
               int lUnitCount = int.Parse(result.xfsMatch.Trim());

               if (!seenFirstFullCashUnitInfo)
               {
                  seenFirstFullCashUnitInfo = true;

                  // create the new rows to hold the Summary of each Logical Cash Unit
                  DataRow[] dataRowSummary = new DataRow[lUnitCount];
                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i] = dTableSet.Tables["Summary"].NewRow();
                  }

                  // for each new row, set the tracefile, timestamp and hresult
                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["file"] = _traceFile;
                     dataRowSummary[i]["time"] = lpResult.tsTimestamp(xfsLine);
                     dataRowSummary[i]["error"] = lpResult.hResult(xfsLine);
                     dataRowSummary[i]["number"] = (i + 1).ToString();
                  }

                  results = _wfs_inf_cdm_cash_unit_info.usType(xfsLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["type"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.cUnitID(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["name"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.cCurrencyID(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["currency"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulValues(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["denom"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulInitialCount(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["initial"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulMinimum(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["min"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulMaximum(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRowSummary[i]["max"] = results.xfsMatch[i];
                  }

                  // add the new rows to the Summary table
                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dTableSet.Tables["Summary"].Rows.Add(dataRowSummary[i]);
                  }

                  ctx.ConsoleWriteLogLine("CashUnit - we've built the Summary table, now on to the CashUnit table");
               }

               // create the new rows to hold the Summary of each Logical Cash Unit
               DataRow[] dataRowCashUnit = new DataRow[lUnitCount];

               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i] = dTableSet.Tables["CashUnit-" + (i + 1).ToString()].NewRow();
               }

               // for each new row, set the tracefile, timestamp and hresult
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["file"] = _traceFile;
                  dataRowCashUnit[i]["time"] = lpResult.tsTimestamp(xfsLine);
                  dataRowCashUnit[i]["error"] = lpResult.hResult(xfsLine);
               }

               // usStatus
               results = _wfs_inf_cdm_cash_unit_info.usStatus(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["status"] = results.xfsMatch[i];
               }

               // ulCount
               results = _wfs_inf_cdm_cash_unit_info.ulCount(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["count"] = results.xfsMatch[i];
               }

               // ulRejectCount
               results = _wfs_inf_cdm_cash_unit_info.ulRejectCount(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["reject"] = results.xfsMatch[i];
               }

               // ulDispensedCount
               results = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["dispensed"] = results.xfsMatch[i];
               }

               // ulPresentedCount
               results = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["presented"] = results.xfsMatch[i];
               }

               // ulRetractedCount
               results = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["retracted"] = results.xfsMatch[i];
               }

               for (int i = 0; i < lUnitCount; i++)
               {
                  dTableSet.Tables["CashUnit-" + (i + 1).ToString()].Rows.Add(dataRowCashUnit[i]);
               }

               for (int i = 0; i < lUnitCount; i++)
               {
                  dTableSet.Tables["CashUnit-" + (i + 1).ToString()].AcceptChanges();
               }
            }
            else if (xfsLine.Contains("lppList ="))
            {
               // isolate count
               result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
               if (!result.success)
               {
                  ctx.ConsoleWriteLogLine("Failed to isolate count from WFS_INF_CDM_CASH_UNIT_INFO message");
                  return;
               }

               ctx.ConsoleWriteLogLine("Update CashUnit Table");

               // how many logical units in the list. 
               int lUnitCount = int.Parse(result.xfsMatch.Trim());

               // we are going to create 1 CashUnit Row but populate a number of columns. 
               DataRow dataRow = dTableSet.Tables["CashUnit"].NewRow();
               foreach (DataColumn col in dataRow.Table.Columns)
               {
                  ctx.ConsoleWriteLogLine("DataColumn in Row : " + col.ColumnName);
               }

               dataRow["file"] = _traceFile;
               dataRow["time"] = lpResult.tsTimestamp(xfsLine);
               dataRow["error"] = lpResult.hResult(xfsLine);

               for (int i = 0; i < lUnitCount; i++)
               {
                  result = _wfs_inf_cdm_cash_unit_info.usNumber(xfsLine);

                  // we have the Logical Unit Number, now fill in the rest
                  string usNumber = result.xfsMatch.Trim();

                  result = _wfs_inf_cdm_cash_unit_info.ulCount2(result.subLogLine);
                  dataRow[usNumber + "-cnt"] = result.xfsMatch.Trim();

                  result = _wfs_inf_cdm_cash_unit_info.ulRejectCount2(result.subLogLine);
                  dataRow[usNumber + "-rjt"] = result.xfsMatch.Trim();

                  result = _wfs_inf_cdm_cash_unit_info.ulDispensedCount2(result.subLogLine);
                  dataRow[usNumber + "-disp"] = result.xfsMatch.Trim();

                  result = _wfs_inf_cdm_cash_unit_info.ulPresentedCount2(result.subLogLine);
                  dataRow[usNumber + "-pres"] = result.xfsMatch.Trim();

                  result = _wfs_inf_cdm_cash_unit_info.ulRetractedCount2(result.subLogLine);
                  dataRow[usNumber + "-ret"] = result.xfsMatch.Trim();
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("Exception : " + e.Message);
         }
      }
   }
}
