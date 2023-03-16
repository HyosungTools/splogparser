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
      private bool have_seen_WFS_CMD_CDM_DISPENSE = false;

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

         foreach (DataTable dTable in dTableSet.Tables)
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
         try
         {
            // rename the cash units
            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Rename the Cash Units : Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashUnit-"))
               {
                  string cashUnitNumber = dTable.TableName.Replace("CashUnit-", string.Empty);
                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = {0}", cashUnitNumber));
                  if (dataRows.Length == 1)
                  {
                     if (dataRows[0]["denom"].ToString().Trim() == "0")
                     {
                        // if currency is "" use the type (e.g. RETRACT, REJECT
                        ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["type"].ToString()));
                        dTable.TableName = dataRows[0]["type"].ToString();
                        dTable.AcceptChanges();
                     }
                     else
                     {
                        // otherwise combine the currency and denomination
                        ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString()));
                        dTable.TableName = dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString();
                        dTable.AcceptChanges();
                     }
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("Exception : " + e.Message);
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
            ctx.ConsoleWriteLogLine(String.Format("WFS_IN_CDM_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

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
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // sometimes we get a single value back, sometimes we get a list
            (bool success, string xfsMatch, string subLogLine) result;

            // there are two styles of log lines. If the log line contains 'lppList->' expect
            // a table of data. If the log lines contains 'lppList =' expect a list
            if (xfsLine.Contains("lppList->"))
            {
               ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO contains 'lppList->'");

               // isolate count
               result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
               if (!result.success)
               {
                  ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO Failed to isolate count from WFS_INF_CDM_CASH_UNIT_INFO message");
                  return;
               }

               // how many logical units in the table. 
               int lUnitCount = int.Parse(result.xfsMatch.Trim());

               // isolate usNumber, usType, cUnitIDs, cCueencyID, ulValue, ulInitialCount, ulMinimum, ulMaximum 
               string[] usNumbers = _wfs_inf_cdm_cash_unit_info.usNumbersFromTable(xfsLine);
               string[] usTypes = _wfs_inf_cdm_cash_unit_info.usTypesFromTable(xfsLine);
               string[] cInitIDs = _wfs_inf_cdm_cash_unit_info.cUnitIDsFromTable(xfsLine);
               string[] cCurrencyIDs = _wfs_inf_cdm_cash_unit_info.cCurrencyIDsFromTable(xfsLine);
               string[] ulValues = _wfs_inf_cdm_cash_unit_info.ulValuesFromTable(xfsLine);
               string[] ulInitialCounts = _wfs_inf_cdm_cash_unit_info.ulInitialCountsFromTable(xfsLine);
               string[] ulMinimums = _wfs_inf_cdm_cash_unit_info.ulMinimumsFromTable(xfsLine);
               string[] ulMaximums = _wfs_inf_cdm_cash_unit_info.ulMaximumsFromTable(xfsLine);
               string[] ulCounts = _wfs_inf_cdm_cash_unit_info.ulCountsFromTable(xfsLine);
               string[] ulRejectCounts = _wfs_inf_cdm_cash_unit_info.ulRejectCountsFromTable(xfsLine);
               string[] usStatuses = _wfs_inf_cdm_cash_unit_info.usStatusesFromTable(xfsLine);
               string[] ulDispensedCounts = _wfs_inf_cdm_cash_unit_info.ulDispensedCountsFromTable(xfsLine);
               string[] ulPresentedCounts = _wfs_inf_cdm_cash_unit_info.ulPresentedCountsFromTable(xfsLine);
               string[] ulRetractedCounts = _wfs_inf_cdm_cash_unit_info.ulRetractedCountsFromTable(xfsLine);

               // for the table format log line, build the Summary Table once
               if (!have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
               {
                  have_seen_WFS_INF_CDM_CASH_UNIT_INFO = true;
                  ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO Setting have_seen_WFS_INF_CDM_CASH_UNIT_INFO to true");

                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

                  // for each row, set the tracefile, timestamp and hresult
                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["file"] = _traceFile;
                     dataRows[i + 1]["time"] = lpResult.tsTimestamp(xfsLine);
                     dataRows[i + 1]["error"] = lpResult.hResult(xfsLine);

                     dataRows[i + 1]["type"] = usTypes[i];
                     dataRows[i + 1]["name"] = cInitIDs[i];
                     dataRows[i + 1]["currency"] = cCurrencyIDs[i];
                     dataRows[i + 1]["denom"] = ulValues[i];
                     dataRows[i + 1]["initial"] = ulInitialCounts[i];
                     dataRows[i + 1]["min"] = ulMinimums[i];
                     dataRows[i + 1]["max"] = ulMaximums[i];
                  }

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

                  dTableSet.Tables["Summary"].AcceptChanges();
                  ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO we've built the Summary table, now on to the CashUnit table");
               }

               for (int i = 0; i < lUnitCount; i++)
               {
                  // Now use the usNumbers to create and populate a row in the CashUnit- table
                  DataRow cashUnitRow = dTableSet.Tables["CashUnit-" + usNumbers[i]].NewRow();

                  cashUnitRow["file"] = _traceFile;
                  cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
                  cashUnitRow["error"] = lpResult.hResult(xfsLine);

                  cashUnitRow["count"] = ulCounts[i];
                  cashUnitRow["reject"] = ulRejectCounts[i];
                  cashUnitRow["status"] = usStatuses[i];
                  cashUnitRow["dispensed"] = ulDispensedCounts[i];
                  cashUnitRow["presented"] = ulPresentedCounts[i];
                  cashUnitRow["retracted"] = ulRetractedCounts[i];

                  dTableSet.Tables["CashUnit-" + usNumbers[i]].Rows.Add(cashUnitRow);
                  dTableSet.Tables["CashUnit-" + usNumbers[i]].AcceptChanges();
               }
            }
            else if (xfsLine.Contains("lppList ="))
            {

               // isolate count
               result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
               if (!result.success)
               {
                  ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO Failed to isolate count from WFS_INF_CDM_CASH_UNIT_INFO message");
                  return;
               }

               // how many logical units in the table. 
               int lUnitCount = int.Parse(result.xfsMatch.Trim());

               // isolate usNumber, usType, cUnitIDs, cCueencyID, ulValue, ulInitialCount, ulMinimum, ulMaximum 
               string[] usNumbers = _wfs_inf_cdm_cash_unit_info.usNumbersFromList(xfsLine);
               string[] usTypes = _wfs_inf_cdm_cash_unit_info.usTypesFromList(xfsLine);
               string[] cInitIDs = _wfs_inf_cdm_cash_unit_info.cUnitIDsFromList(xfsLine);
               string[] cCurrencyIDs = _wfs_inf_cdm_cash_unit_info.cCurrencyIDsFromList(xfsLine);
               string[] ulValues = _wfs_inf_cdm_cash_unit_info.ulValuesFromList(xfsLine);
               string[] ulInitialCounts = _wfs_inf_cdm_cash_unit_info.ulInitialCountsFromList(xfsLine);
               string[] ulMinimums = _wfs_inf_cdm_cash_unit_info.ulMinimumsFromList(xfsLine);
               string[] ulMaximums = _wfs_inf_cdm_cash_unit_info.ulMaximumsFromList(xfsLine);
               string[] ulCounts = _wfs_inf_cdm_cash_unit_info.ulCountsFromList(xfsLine);
               string[] ulRejectCounts = _wfs_inf_cdm_cash_unit_info.ulRejectCountsFromList(xfsLine);
               string[] usStatuses = _wfs_inf_cdm_cash_unit_info.usStatusesFromList(xfsLine);
               string[] ulDispensedCounts = _wfs_inf_cdm_cash_unit_info.ulDispensedCountsFromList(xfsLine);
               string[] ulPresentedCounts = _wfs_inf_cdm_cash_unit_info.ulPresentedCountsFromLists(xfsLine);
               string[] ulRetractedCounts = _wfs_inf_cdm_cash_unit_info.ulRetractedCountsFromList(xfsLine);

               // for the list format log line, build the Summary Table once
               if (!have_seen_WFS_INF_CDM_CASH_UNIT_INFO)
               {
                  have_seen_WFS_INF_CDM_CASH_UNIT_INFO = true;

                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

                  // for each row, set the tracefile, timestamp and hresult
                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["file"] = _traceFile;
                     dataRows[i + 1]["time"] = lpResult.tsTimestamp(xfsLine);
                     dataRows[i + 1]["error"] = lpResult.hResult(xfsLine);

                     dataRows[i + 1]["type"] = usTypes[i];
                     dataRows[i + 1]["name"] = cInitIDs[i];
                     dataRows[i + 1]["currency"] = cCurrencyIDs[i];
                     dataRows[i + 1]["denom"] = ulValues[i];
                     dataRows[i + 1]["initial"] = ulInitialCounts[i];
                     dataRows[i + 1]["min"] = ulMinimums[i];
                     dataRows[i + 1]["max"] = ulMaximums[i];

                     dTableSet.Tables["Summary"].AcceptChanges();
                     ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO we've built the Summary table, now on to the CashUnit table");
                  }

                  // clean up empty rows of the Summary table
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
               }

               for (int i = 0; i < lUnitCount; i++)
               {
                  // Now use the usNumbers to create and populate a row in the CashUnit- table
                  DataRow cashUnitRow = dTableSet.Tables["CashUnit-" + usNumbers[i]].NewRow();

                  cashUnitRow["file"] = _traceFile;
                  cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
                  cashUnitRow["error"] = lpResult.hResult(xfsLine);

                  cashUnitRow["count"] = ulCounts[i];
                  cashUnitRow["reject"] = ulRejectCounts[i];
                  cashUnitRow["status"] = usStatuses[i];
                  cashUnitRow["dispensed"] = ulDispensedCounts[i];
                  cashUnitRow["presented"] = ulPresentedCounts[i]; 
                  cashUnitRow["retracted"] = ulRetractedCounts[i];

                  dTableSet.Tables["CashUnit-" + usNumbers[i]].Rows.Add(cashUnitRow);
                  dTableSet.Tables["CashUnit-" + usNumbers[i]].AcceptChanges();
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("Exception : " + e.Message);
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

            if (dTableSet.Tables["Summary"].Rows.Count == 0)
            {
               ctx.ConsoleWriteLogLine("WFS_CMD_CDM_DISPENSE no rows in the Summary table");
               return;
            }

            int luCount = dTableSet.Tables["Summary"].Rows.Count;

            if (!have_seen_WFS_CMD_CDM_DISPENSE)
            {
               // flag that we have see at least 1 _WFS_CMD_CDM_DISPENSE
               have_seen_WFS_CMD_CDM_DISPENSE = true;
               ctx.ConsoleWriteLogLine("WFS_CMD_CDM_DISPENSE first time in");

               // a list of the logical units in number order
               DataView view = dTableSet.Tables["Summary"].DefaultView;
               view.Sort = "number ASC";

               // create a column in the Dispense table for each logical unit (row) in the Summary table
               for (int i = 0; i < dTableSet.Tables["Summary"].Rows.Count; i++)
               {
                  string columnName = string.Empty;
                  if (view[i]["currency"].ToString().Trim() == "")
                  {
                     columnName = view[i]["name"].ToString();
                  }
                  else
                  {
                     columnName = view[i]["currency"].ToString() + view[i]["denom"].ToString();
                  }
                  ctx.ConsoleWriteLogLine("WFS_CMD_CDM_DISPENSE adding column name :" + columnName);
                  dTableSet.Tables["Dispense"].Columns.Add(columnName, typeof(string));
               }

               dTableSet.Tables["Dispense"].Columns.Add("comment", typeof(string));
               dTableSet.Tables["Dispense"].AcceptChanges();
            }

            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_DISPENSE create row of '{0}' columns ", dTableSet.Tables["Dispense"].Columns.Count));
            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Dispense";

            // amount
            result = _wfs_cmd_cdm_dispense.ulAmount(xfsLine);
            dataRow["amount"] = result.xfsMatch.Trim();

            // lpulValues
            DataView dataView = dTableSet.Tables["Summary"].DefaultView;
            dataView.Sort = "number ASC";

            // usCount (should be equal to or less than luCount)
            result = _wfs_cmd_cdm_dispense.usCount(xfsLine);
            int usCount = int.Parse(result.xfsMatch.Trim());

            // now use 'results' (not 'result'!) to access the lpulValues values
            results = _wfs_cmd_cdm_dispense.lpulValues(result.subLogLine);

            // populate the columns of the DataRow using lpulValues values
            for (int i = 0; i < usCount; i++)
            {
               string columnName = string.Empty;
               if (dataView[i]["currency"].ToString().Trim() == "")
               {
                  columnName = dataView[i]["name"].ToString();
               }
               else
               {
                  columnName = dataView[i]["currency"].ToString() + dataView[i]["denom"].ToString();
               }
               // only write non zero values for readability
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
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CDM_DISPENSE create row of '{0}' columns ", dTableSet.Tables["Dispense"].Columns.Count));
            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Present";
            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_DISPENSE Exception : " + e.Message);
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

            // sometimes we get a single value back, sometimes we get a list
            (bool success, string xfsMatch, string subLogLine) result;

            // isolate usNumber
            result = _wfs_inf_cdm_cash_unit_info.usNumber(xfsLine);
            int usNumber = int.Parse(result.xfsMatch.Trim());

            // Now use the usNumber to create and populate a row in the CashUnit- table
            DataRow cashUnitRow = dTableSet.Tables["CashUnit-" + usNumber.ToString()].NewRow();

            cashUnitRow["file"] = _traceFile;
            cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
            cashUnitRow["error"] = lpResult.hResult(xfsLine);

            // isolate ulCount
            result = _wfs_inf_cdm_cash_unit_info.ulCount(result.subLogLine);
            cashUnitRow["count"] = result.xfsMatch.Trim();

            // isolate ulRejectedCount
            result = _wfs_inf_cdm_cash_unit_info.ulRejectCount(result.subLogLine);
            cashUnitRow["reject"] = result.xfsMatch.Trim();

            // isolate usStatus
            result = _wfs_inf_cdm_cash_unit_info.usStatus(result.subLogLine);
            cashUnitRow["status"] = result.xfsMatch.Trim();

            // isolate ulDispensedCount
            result = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(result.subLogLine);
            cashUnitRow["dispensed"] = result.xfsMatch.Trim();

            // isolate ulPresentedCount
            result = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(result.subLogLine);
            cashUnitRow["presented"] = result.xfsMatch.Trim();

            // isolate ulRetractedCount
            result = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(result.subLogLine);
            cashUnitRow["retracted"] = result.xfsMatch.Trim();

            dTableSet.Tables["CashUnit-" + usNumber.ToString()].Rows.Add(cashUnitRow);
            dTableSet.Tables["CashUnit-" + usNumber.ToString()].AcceptChanges();
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
