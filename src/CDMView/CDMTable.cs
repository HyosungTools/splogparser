using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;

namespace CDMView
{

   internal class CDMTable : BaseTable
   {
      private bool have_seen_WFS_INF_CDM_CASH_UNIT_INFO = false;

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
                     WFS_INF_CDM_STATUS(result.xfsLine);
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
                     if (have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
                     {
                        base.ProcessRow(traceFile, logLine);
                        WFS_CMD_CDM_DISPENSE(result.xfsLine);
                     }
                     else
                     {
                        ctx.ConsoleWriteLogLine("have_not seen WFS_INF_CDM_CASH_UNIT_INFO");
                     }
                     break;
                  }
               case XFSType.WFS_CMD_CDM_PRESENT:
                  {
                     if (have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
                     {
                        base.ProcessRow(traceFile, logLine);
                        WFS_CMD_CDM_PRESENT(result.xfsLine);
                     }
                     else
                     {
                        ctx.ConsoleWriteLogLine("have_not seen WFS_INF_CDM_CASH_UNIT_INFO");
                     }
                     break;
                  }
               case XFSType.WFS_CMD_CDM_REJECT:
                  {
                     if (have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
                     {
                        base.ProcessRow(traceFile, logLine);
                        WFS_CMD_CDM_REJECT(result.xfsLine);
                     }
                     else
                     {
                        ctx.ConsoleWriteLogLine("have_not seen WFS_INF_CDM_CASH_UNIT_INFO");
                     }
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RETRACT:
                  {
                     if (have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
                     {
                        base.ProcessRow(traceFile, logLine);
                        WFS_CMD_CDM_RETRACT(result.xfsLine);
                     }
                     else
                     {
                        ctx.ConsoleWriteLogLine("have_not seen WFS_INF_CDM_CASH_UNIT_INFO");
                     }
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RESET:
                  {
                     if (have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
                     {
                        base.ProcessRow(traceFile, logLine);
                        WFS_CMD_CDM_RESET(result.xfsLine);
                     }
                     else
                     {
                        ctx.ConsoleWriteLogLine("have_not seen WFS_INF_CDM_CASH_UNIT_INFO");
                     }
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_SRVE_CDM_CASHUNITINFOCHANGED(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_ITEMSTAKEN:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_SRVE_CDM_ITEMSTAKEN(result.xfsLine);
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
         (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables["Status"], "time ASC", columns);
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
            result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Status"], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
         }

         // SMMARY TABLE - Delete redundant lines from the Summary Table
         DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
         List<DataRow> deleteRows = new List<DataRow>();
         foreach (DataRow dataRow in dataRows)
         {
            if (dataRow["file"].ToString().Trim() == string.Empty)
            {
               deleteRows.Add(dataRow);
            }
         }

         foreach (DataRow dataRow in deleteRows)
         {
            dataRow.Delete();
         }

         // CASH UNIT TABLE
         ctx.ConsoleWriteLogLine("Compress the CashUnit Tables: sort by time, visit every row and delete rows that are unchanged from their predecessor");

         // the list of columns to compare
         string[] cashUnitCols = new string[] { "error", "status", "count", "reject", "dispensed", "presented", "retracted" };

         foreach (DataTable dTable in dTableSet.Tables)
         {
            ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
            if (dTable.TableName.StartsWith("CashUnit-"))
            {
               ctx.ConsoleWriteLogLine(String.Format("Compress the Table '{0}' rows before: {1}", dTable.TableName, dTable.Rows.Count));
               (bool success, string message) cashUnitResult = _datatable_ops.DeleteUnchangedRowsInTable(dTable, "time ASC", cashUnitCols);
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
            result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Summary"], dTableSet.Tables["Messages"], summaryColMap[i, 0], summaryColMap[i, 1]);
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
                  result = _datatable_ops.AddEnglishToTable(ctx, dTable, dTableSet.Tables["Messages"], cashUnitColMap[i, 0], cashUnitColMap[i, 1]);
               }
            }
         }
         try
         {
            // rename the cash units
            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Rename the Cash Units : Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashUnit-"))
               {
                  string cashUnitNumber = dTable.TableName.Replace("CashUnit-", string.Empty);
                  dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = {0}", cashUnitNumber));
                  if (dataRows.Length == 1)
                  {
                     if (dataRows[0]["denom"].ToString().Trim() == "0")
                     {
                        // if currency is "" use the type (e.g. RETRACT, REJECT
                        ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["type"].ToString()));
                        dTable.TableName = dataRows[0]["type"].ToString();
                        dTable.AcceptChanges();

                        // Rename the Dispense column name to match the Logical Unit Name
                        dTableSet.Tables["Dispense"].Columns["LU" + cashUnitNumber].ColumnName = dTable.TableName;
                     }
                     else
                     {
                        // otherwise combine the currency and denomination
                        ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString()));
                        dTable.TableName = dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString();
                        dTable.AcceptChanges();

                        // Rename the Dispense column name to match the Logical Unit Name
                        dTableSet.Tables["Dispense"].Columns["LU" + cashUnitNumber].ColumnName = dTable.TableName;
                     }
                  }
               }
            }

            // TODO - delete any CashUnit column named LUx
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception : " + e.Message);
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
      protected void WFS_INF_CDM_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            (bool success, string xfsMatch, string subLogLine) result;

            // fwDevice
            result = _wfs_inf_cdm_status.fwDevice(xfsLine);
            if (result.success) dataRow["status"] = result.xfsMatch.Trim();

            // fwDispenser
            result = _wfs_inf_cdm_status.fwDispenser(result.subLogLine);
            if (result.success) dataRow["dispenser"] = result.xfsMatch.Trim();

            // fwIntermediateStacker
            result = _wfs_inf_cdm_status.fwIntermediateStacker(result.subLogLine);
            if (result.success) dataRow["intstack"] = result.xfsMatch.Trim();

            // fwShutter
            result = _wfs_inf_cdm_status.fwShutter(result.subLogLine);
            if (result.success) dataRow["shutter"] = result.xfsMatch.Trim();

            // fwPositionStatus
            result = _wfs_inf_cdm_status.fwPositionStatus(result.subLogLine);
            if (result.success) dataRow["posstatus"] = result.xfsMatch.Trim();

            // fwTransport
            result = _wfs_inf_cdm_status.fwTransport(result.subLogLine);
            if (result.success) dataRow["transport"] = result.xfsMatch.Trim();

            // fwTransportStatus
            result = _wfs_inf_cdm_status.fwTransportStatus(result.subLogLine);
            if (result.success) dataRow["transstat"] = result.xfsMatch.Trim();

            // wDevicePosition
            result = _wfs_inf_cdm_status.wDevicePosition(result.subLogLine);
            if (result.success) dataRow["position"] = result.xfsMatch.Trim();

            dTableSet.Tables["Status"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CDM_STATUS Exception : " + e.Message);
         }

         return;
      }
      protected void WFS_INF_CDM_CASH_UNIT_INFO(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));


            WFSCDMCUINFO cashInfo = new WFSCDMCUINFO(ctx);
            try
            {
               cashInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            if (!have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
            {
               // First time seeing CASH_UNIT_INFO, populate the Summary Table
               have_seen_WFS_INF_CDM_CASH_UNIT_INFO = true;

               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

               // for each row, set the tracefile, timestamp and hresult
               for (int i = 0; i < cashInfo.lUnitCount; i++)
               {
                  try
                  {
                     dataRows[i + 1]["file"] = _traceFile;
                     dataRows[i + 1]["time"] = lpResult.tsTimestamp(xfsLine);
                     dataRows[i + 1]["error"] = lpResult.hResult(xfsLine);

                     dataRows[i + 1]["type"] = cashInfo.usTypes[i];
                     dataRows[i + 1]["name"] = cashInfo.cUnitIDs[i];
                     dataRows[i + 1]["currency"] = cashInfo.cCurrencyIDs[i];
                     dataRows[i + 1]["denom"] = cashInfo.ulValues[i];
                     dataRows[i + 1]["initial"] = cashInfo.ulInitialCounts[i];
                     dataRows[i + 1]["min"] = cashInfo.ulMinimums[i];
                     dataRows[i + 1]["max"] = cashInfo.ulMaximums[i];
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Summary Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
                  }
               }

               dTableSet.Tables["Summary"].AcceptChanges();
            }

            for (int i = 0; i < cashInfo.lUnitCount; i++)
            {
               try
               {
                  // Now use the usNumbers to create and populate a row in the CashUnit-x table
                  if (int.Parse(cashInfo.usNumbers[i].Trim()) < 1)
                  {
                     // We have to check because some log lines are truncated (i.e. "more data")
                     // and produce bad results
                     continue;
                  }

                  if (cashInfo.ulCounts[i] == "0" && cashInfo.ulRejectCounts[i] == "0" && cashInfo.ulDispensedCounts[i] == "0" && cashInfo.ulPresentedCounts[i] == "0")
                  {
                     // again truncated log lines result in garbage output
                     continue;
                  }

                  string tableName = "CashUnit-" + cashInfo.usNumbers[i].Trim();
                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = _traceFile;
                  cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
                  cashUnitRow["error"] = lpResult.hResult(xfsLine);

                  cashUnitRow["count"] = cashInfo.ulCounts[i];
                  cashUnitRow["reject"] = cashInfo.ulRejectCounts[i];
                  cashUnitRow["status"] = cashInfo.usStatuses[i];
                  cashUnitRow["dispensed"] = cashInfo.ulDispensedCounts[i];
                  cashUnitRow["presented"] = cashInfo.ulPresentedCounts[i];
                  cashUnitRow["retracted"] = cashInfo.ulRetractedCounts[i];

                  dTableSet.Tables["CashUnit-" + cashInfo.usNumbers[i]].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Cash Unit Table Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO  End of Function Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_DISPENSE(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_DISPENSE tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // sometimes we get a single value back, sometimes we get a list
            (bool success, string xfsMatch, string subLogLine) result;
            (bool success, string[] xfsMatch, string subLogLine) results;

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();
            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Dispense";

            // amount
            result = _wfs_cmd_cdm_dispense.ulAmount(xfsLine);
            dataRow["amount"] = result.xfsMatch.Trim();

            // usCount (should be equal to or less than luCount)
            result = _wfs_cmd_cdm_dispense.usCount(xfsLine);
            int usCount = int.Parse(result.xfsMatch.Trim());

            // now use 'results' (not 'result'!) to access the lpulValues values
            results = _wfs_cmd_cdm_dispense.lpulValues(result.subLogLine);

            // populate the columns of the DataRow using lpulValues values
            for (int i = 0; i < usCount; i++)
            {
               string columnName = "LU" + (i + 1).ToString();
               dataRow[columnName] = results.xfsMatch[i].Trim() == "0" ? "" : results.xfsMatch[i].Trim();
            }

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_DISPENSE Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_PRESENT(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_PRESENT tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Present";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_PRESENT Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_REJECT(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_REJECT tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Reject";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_REJECT Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_RETRACT(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_RETRACT tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Retract";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_RETRACT Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_RESET(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_RESET tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Reset";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_RESET Exception : " + e.Message);
         }
      }
      protected void WFS_SRVE_CDM_CASHUNITINFOCHANGED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CDM_CASHUNITINFOCHANGED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSCDMCUINFO cashInfo = new WFSCDMCUINFO(ctx);
            try
            {
               cashInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
               int i = int.Parse(cashInfo.usNumbers[0]);

               dataRows[i]["type"] = cashInfo.usTypes[0];
               dataRows[i]["name"] = cashInfo.cUnitIDs[0];
               dataRows[i]["currency"] = cashInfo.cCurrencyIDs[0];
               dataRows[i]["denom"] = cashInfo.ulValues[0];
               dataRows[i]["initial"] = cashInfo.ulInitialCounts[0];
               dataRows[i]["min"] = cashInfo.ulMinimums[0];
               dataRows[i]["max"] = cashInfo.ulMaximums[0];

               dTableSet.Tables["Summary"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_CASHUNITINFOCHANGED Update Summary Table Exception : " + e.Message);
            }

            try
            {

               // Now use the usNumber to create and populate a row in the CashUnit- table
               DataRow cashUnitRow = dTableSet.Tables["CashUnit-" + cashInfo.usNumbers[0]].Rows.Add();

               cashUnitRow["file"] = _traceFile;
               cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
               cashUnitRow["error"] = lpResult.hResult(xfsLine);

               cashUnitRow["count"] = cashInfo.ulCounts[0];
               cashUnitRow["reject"] = cashInfo.ulRejectCounts[0];
               cashUnitRow["status"] = cashInfo.usStatuses[0];
               cashUnitRow["dispensed"] = cashInfo.ulDispensedCounts[0];
               cashUnitRow["presented"] = cashInfo.ulPresentedCounts[0];
               cashUnitRow["retracted"] = cashInfo.ulRetractedCounts[0];

               dTableSet.Tables["CashUnit-" + cashInfo.usNumbers[0]].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_CASHUNITINFOCHANGED Update CashUnit Table Exception : " + e.Message);
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_CASHUNITINFOCHANGED Exception : " + e.Message);
         }
      }
      protected void WFS_SRVE_CDM_ITEMSTAKEN(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CDM_ITEMSTAKEN tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Customer";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_ITEMSTAKEN Exception : " + e.Message);
         }
      }
   }
}
