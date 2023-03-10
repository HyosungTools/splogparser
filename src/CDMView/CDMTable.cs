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

               // for the table format log line, build the Summary Table once
               if (!seenFirstFullCashUnitInfo)
               {
                  seenFirstFullCashUnitInfo = true; 
                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

                  // for each row, set the tracefile, timestamp and hresult
                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["file"] = _traceFile;
                     dataRows[i + 1]["time"] = lpResult.tsTimestamp(xfsLine);
                     dataRows[i + 1]["error"] = lpResult.hResult(xfsLine);
                  }

                  results = _wfs_inf_cdm_cash_unit_info.usTypesFromTable(xfsLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["type"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.cUnitIDsFromTable(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["name"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.cCurrencyIDsFromTable(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["currency"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulValuesFromTable(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["denom"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulInitialCountsFromTable(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["initial"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulMinimumsFromTable(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["min"] = results.xfsMatch[i];
                  }

                  results = _wfs_inf_cdm_cash_unit_info.ulMaximumsFromTable(results.subLogLine);

                  for (int i = 0; i < lUnitCount; i++)
                  {
                     dataRows[i + 1]["max"] = results.xfsMatch[i];
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
               results = _wfs_inf_cdm_cash_unit_info.usStatusFromTable(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["status"] = results.xfsMatch[i];
               }

               // ulCount
               results = _wfs_inf_cdm_cash_unit_info.ulCountsFromTable(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["count"] = results.xfsMatch[i];
               }

               // ulRejectCount
               results = _wfs_inf_cdm_cash_unit_info.ulRejectCountsFromTable(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["reject"] = results.xfsMatch[i];
               }

               // ulDispensedCount
               results = _wfs_inf_cdm_cash_unit_info.ulDispensedCountsFromTable(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["dispensed"] = results.xfsMatch[i];
               }

               // ulPresentedCount
               results = _wfs_inf_cdm_cash_unit_info.ulPresentedCountsFromTable(xfsLine);
               for (int i = 0; i < lUnitCount; i++)
               {
                  dataRowCashUnit[i]["presented"] = results.xfsMatch[i];
               }

               // ulRetractedCount
               results = _wfs_inf_cdm_cash_unit_info.ulRetractedCountsFromTable(xfsLine);
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


               // how many logical units in the list. 
               int lUnitCount = int.Parse(result.xfsMatch.Trim());

               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

               string thisNumberStartsHere = string.Empty; 
               string nextNumberStartsHere = xfsLine; 

               for (int i = 0; i < lUnitCount; i++)
               {
                  // isolate usNumber
                  result = _wfs_inf_cdm_cash_unit_info.usNumber(nextNumberStartsHere);
                  int usNumber = int.Parse(result.xfsMatch.Trim());

                  // save off a marker to the start of this number
                  thisNumberStartsHere = result.subLogLine;

                  ctx.ConsoleWriteLogLine(String.Format("lUnitCount: {0} usNumber:{1}", lUnitCount, usNumber));

                  dataRows[usNumber]["file"] = _traceFile;
                  dataRows[usNumber]["time"] = lpResult.tsTimestamp(xfsLine);
                  dataRows[usNumber]["error"] = lpResult.hResult(xfsLine);

                  // isolate usType
                  result = _wfs_inf_cdm_cash_unit_info.usType(thisNumberStartsHere);
                  dataRows[usNumber]["type"] = result.xfsMatch.Trim();

                  // isolate cUnitID
                  result = _wfs_inf_cdm_cash_unit_info.cUnitID(thisNumberStartsHere);
                  dataRows[usNumber]["name"] = result.xfsMatch.Trim();

                  // isolate cCurrencyID
                  result = _wfs_inf_cdm_cash_unit_info.cCurrencyID(thisNumberStartsHere);
                  dataRows[usNumber]["currency"] = result.xfsMatch.Trim();

                  // isolate ulValues
                  result = _wfs_inf_cdm_cash_unit_info.ulValue(thisNumberStartsHere);
                  dataRows[usNumber]["denom"] = result.xfsMatch.Trim();

                  // ulInitialCount
                  result = _wfs_inf_cdm_cash_unit_info.ulInitialCount(thisNumberStartsHere);
                  dataRows[usNumber]["initial"] = result.xfsMatch.Trim();

                  // ulMinimum
                  result = _wfs_inf_cdm_cash_unit_info.ulMinimum(thisNumberStartsHere);
                  dataRows[usNumber]["min"] = result.xfsMatch.Trim();

                  // ulMaximum
                  result = _wfs_inf_cdm_cash_unit_info.ulMaximum(thisNumberStartsHere);
                  dataRows[usNumber]["min"] = result.xfsMatch.Trim();

                  dTableSet.Tables["Summary"].AcceptChanges();

                  // Now use the usNumber to create and populate a row in the CashUnit- table
                  DataRow cashUnitRow = dTableSet.Tables["CashUnit-" + usNumber.ToString()].NewRow();

                  cashUnitRow["file"] = _traceFile;
                  cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
                  cashUnitRow["error"] = lpResult.hResult(xfsLine);

                  // isolate ulCount
                  result = _wfs_inf_cdm_cash_unit_info.ulCount(thisNumberStartsHere);
                  cashUnitRow["count"] = result.xfsMatch.Trim();

                  // isolate ulRejectedCount
                  result = _wfs_inf_cdm_cash_unit_info.ulRejectCount(thisNumberStartsHere);
                  cashUnitRow["reject"] = result.xfsMatch.Trim();

                  // isolate usStatus
                  result = _wfs_inf_cdm_cash_unit_info.usStatus(thisNumberStartsHere);
                  cashUnitRow["status"] = result.xfsMatch.Trim();

                  // isolate ulDispensedCount
                  result = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(thisNumberStartsHere);
                  cashUnitRow["dispensed"] = result.xfsMatch.Trim();

                  // isolate ulPresentedCount
                  result = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(thisNumberStartsHere);
                  cashUnitRow["presented"] = result.xfsMatch.Trim();

                  // isolate ulRetractedCount
                  result = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(thisNumberStartsHere);
                  cashUnitRow["retracted"] = result.xfsMatch.Trim();

                  dTableSet.Tables["CashUnit-" + usNumber.ToString()].Rows.Add(cashUnitRow);
                  dTableSet.Tables["CashUnit-" + usNumber.ToString()].AcceptChanges();

                  // save this point as where to start looking for the next number
                  nextNumberStartsHere = result.subLogLine;
                  thisNumberStartsHere = string.Empty;
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
