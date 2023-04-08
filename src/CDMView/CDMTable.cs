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
               case XFSType.WFS_INF_CDM_PRESENT_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_CDM_PRESENT_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CDM_DISPENSE:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CDM_DISPENSE(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CDM_PRESENT:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CDM_PRESENT(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CDM_REJECT:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CDM_REJECT(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RETRACT:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CDM_RETRACT(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RESET:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CDM_RESET(result.xfsLine);
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
                     base.ProcessRow(traceFile, logLine);
                     WFS_USRE_CIM_CASHUNITTHRESHOLD(result.xfsLine);
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

         // SUMMARY TABLE - Delete redundant lines from the Summary Table
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

         // add English to the Dispense Table
         string[,] dispenseColMap = new string[1, 2]
         {
            {"position", "wPresentState" }
         };

         for (int i = 0; i < 1; i++)
         {
            result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Dispense"], dTableSet.Tables["Messages"], dispenseColMap[i, 0], dispenseColMap[i, 1]);
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

            WFSCDMSTATUS cdmStatus = new WFSCDMSTATUS(ctx);

            try
            {
               cdmStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            dataRow["status"] = cdmStatus.fwDevice;
            dataRow["dispenser"] = cdmStatus.fwDispenser;
            dataRow["intstack"] = cdmStatus.fwIntStacker;
            dataRow["shutter"] = cdmStatus.fwShutter;
            dataRow["posstatus"] = cdmStatus.fwPositionStatus;
            dataRow["transport"] = cdmStatus.fwTransport;
            dataRow["transstat"] = cdmStatus.fwTransportStatus;
            dataRow["position"] = cdmStatus.wDevicePosition;

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
               // have_seen_WFS_INF_CDM_CASH_UNIT_INFO = true;

               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

               // for each row, set the tracefile, timestamp and hresult
               for (int i = 0; i < cashInfo.lUnitCount; i++)
               {
                  // Now use the usNumbers to create and populate a row in the CashUnit-x table
                  int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                  if (usNumber < 1)
                  {
                     // We have to check because some log lines are truncated (i.e. "more data")
                     // and produce bad results
                     continue;
                  }

                  try
                     {
                     dataRows[usNumber]["file"] = _traceFile;
                     dataRows[usNumber]["time"] = lpResult.tsTimestamp(xfsLine);
                     dataRows[usNumber]["error"] = lpResult.hResult(xfsLine);

                     dataRows[usNumber]["type"] = cashInfo.usTypes[i];
                     dataRows[usNumber]["name"] = cashInfo.cUnitIDs[i];
                     dataRows[usNumber]["currency"] = cashInfo.cCurrencyIDs[i];
                     dataRows[usNumber]["denom"] = cashInfo.ulValues[i];
                     dataRows[usNumber]["initial"] = cashInfo.ulInitialCounts[i];
                     dataRows[usNumber]["min"] = cashInfo.ulMinimums[i];
                     dataRows[usNumber]["max"] = cashInfo.ulMaximums[i];
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Summary Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
                  }
               }

               dTableSet.Tables["Summary"].AcceptChanges();
            }

            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO cashInfo.lUnitCount '{0}' ", cashInfo.lUnitCount));

            for (int i = 0; i < cashInfo.lUnitCount; i++)
            {
               try
               {
                  // Now use the usNumber to create and populate a row in the CashUnit- table
                  int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                  string tableName = "CashUnit-" + usNumber.ToString();
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO cashInfo.usNumbers[{0}] = '{1}' tableName = '{2}': ", i, usNumber, tableName));

                  if (usNumber < 1)
                  {
                     // We have to check because some log lines are truncated (i.e. "more data")
                     // and produce bad results
                     continue;
                  }

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

                  dTableSet.Tables[tableName].AcceptChanges();
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
      protected void WFS_INF_CDM_PRESENT_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CMD_PRESENT_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));


            WFSCDMPRESENTSTATUS cdmPresentStatus = new WFSCDMPRESENTSTATUS(ctx);

            try
            {
               cdmPresentStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CMD_PRESENT_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();
            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = cdmPresentStatus.wPresentState;

            // amount
            dataRow["amount"] = cdmPresentStatus.CDMDENOM.ulAmount;

            int usCount = int.Parse(cdmPresentStatus.CDMDENOM.usCount);

            // populate the columns of the DataRow using lpulValues values
            for (int i = 0; i < usCount; i++)
            {
               string columnName = "LU" + (i + 1).ToString();
               dataRow[columnName] = cdmPresentStatus.CDMDENOM.lpulValues[i].Trim() == "0" ? "" : cdmPresentStatus.CDMDENOM.lpulValues[i].Trim();
            }

            dTableSet.Tables["Dispense"].AcceptChanges();
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


            WFSCDMDENOMINATION cdmDenom = new WFSCDMDENOMINATION(ctx);

            try
            {
               cdmDenom.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_DISPENSE Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();
            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "dispense";

            // amount
            dataRow["amount"] = cdmDenom.ulAmount;

            int usCount = int.Parse(cdmDenom.usCount);

            // populate the columns of the DataRow using lpulValues values
            for (int i = 0; i < usCount; i++)
            {
               string columnName = "LU" + (i + 1).ToString();
               dataRow[columnName] = cdmDenom.lpulValues[i].Trim() == "0" ? "" : cdmDenom.lpulValues[i].Trim();
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
            dataRow["position"] = "present";

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
            dataRow["position"] = "reject";

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
            dataRow["position"] = "retract";

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
            dataRow["position"] = "reset";

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
               int usNumber = int.Parse(cashInfo.usNumbers[0].Trim());
               string tableName = "CashUnit-" + usNumber.ToString();
               ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CDM_CASHUNITINFOCHANGED usNumber = '{0}' tableName = '{1}': ", usNumber, tableName));

               DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

               cashUnitRow["file"] = _traceFile;
               cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
               cashUnitRow["error"] = lpResult.hResult(xfsLine);

               cashUnitRow["count"] = cashInfo.ulCounts[0];
               cashUnitRow["reject"] = cashInfo.ulRejectCounts[0];
               cashUnitRow["status"] = cashInfo.usStatuses[0];
               cashUnitRow["dispensed"] = cashInfo.ulDispensedCounts[0];
               cashUnitRow["presented"] = cashInfo.ulPresentedCounts[0];
               cashUnitRow["retracted"] = cashInfo.ulRetractedCounts[0];

               dTableSet.Tables[tableName].AcceptChanges();
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
      protected void WFS_USRE_CIM_CASHUNITTHRESHOLD(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_CIM_CASHUNITTHRESHOLD tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

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
               ctx.ConsoleWriteLogLine("WFS_USRE_CIM_CASHUNITTHRESHOLD Update Summary Table Exception : " + e.Message);
            }

            try
            {
               // Now use the usNumber to create and populate a row in the CashUnit- table
               int usNumber = int.Parse(cashInfo.usNumbers[0].Trim());
               string tableName = "CashUnit-" + usNumber.ToString();
               ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_CIM_CASHUNITTHRESHOLD usNumber = '{0}' tableName = '{1}': ", usNumber, tableName));

               DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

               cashUnitRow["file"] = _traceFile;
               cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
               cashUnitRow["error"] = lpResult.hResult(xfsLine);

               cashUnitRow["count"] = cashInfo.ulCounts[0];
               cashUnitRow["reject"] = cashInfo.ulRejectCounts[0];
               cashUnitRow["status"] = cashInfo.usStatuses[0];
               cashUnitRow["dispensed"] = cashInfo.ulDispensedCounts[0];
               cashUnitRow["presented"] = cashInfo.ulPresentedCounts[0];
               cashUnitRow["retracted"] = cashInfo.ulRetractedCounts[0];

               dTableSet.Tables[tableName].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("WFS_USRE_CIM_CASHUNITTHRESHOLD Update CashUnit Table Exception : " + e.Message);
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_CIM_CASHUNITTHRESHOLD Exception : " + e.Message);
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
            dataRow["position"] = "items taken";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_ITEMSTAKEN Exception : " + e.Message);
         }
      }
   }
}
