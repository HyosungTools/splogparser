using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;

namespace CIMView
{
   internal class CIMTable : BaseTable
   {
      private bool have_seen_WFS_INF_CIM_CASH_UNIT_INFO = false;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CIMTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;

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
               case XFSType.WFS_INF_CIM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_CIM_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CIM_CASH_UNIT_INFO:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_CIM_CASH_UNIT_INFO(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CIM_CASH_IN_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_CIM_CASH_IN_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN_START:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CIM_CASH_IN_START(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CIM_CASH_IN(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN_END:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CIM_CASH_IN_END(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CIM_CASH_IN_ROLLBACK(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CIM_RETRACT:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CIM_RETRACT(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_CIM_RESET:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_CIM_RESET(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_USRE_CIM_CASHUNITTHRESHOLD(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_SRVE_CIM_CASHUNITINFOCHANGED(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_SRVE_CIM_ITEMSTAKEN(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_EXEE_CIM_INPUTREFUSE(result.xfsLine);
                     break;
                  }
               default:
                  break;
            };
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CIMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         try
         {
            // S T A T U S   T A B L E

            // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
            ctx.ConsoleWriteLogLine("Compress the Status Table: sort by time, visit every row and delete rows that are unchanged from their predecessor");
            ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table start: rows before: {0}", dTableSet.Tables["Status"].Rows.Count));

            // the list of columns to compare
            string[] columns = new string[] { "error", "status", "safedoor", "acceptor", "intstack", "stackitems" };
            (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables["Status"], "time ASC", columns);
            if (!result.success)
            {
               ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
            }
            ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table complete: rows after: {0}", dTableSet.Tables["Status"].Rows.Count));

            // add English to the Status Table
            string[,] colKeyMap = new string[5, 2]
            {
               {"status", "fwDevice" },
               {"safedoor", "fwSafeDoor;"},
               {"acceptor", "fwAcceptor" },
               {"intstack", "fwIntermediateStacker" },
               {"stackitems", "fwStackerItems" }
            };

            for (int i = 0; i < 5; i++)
            {
               result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Status"], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
            }

            // S U M M A R Y   T A B L E 

            // delete redundant lines from the Summary Table
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

            // Add English to Summary Table
            string[,] summaryColMap = new string[1, 2]
            {
            {"type", "fwType" }
            };

            for (int i = 0; i < 1; i++)
            {
               result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Summary"], dTableSet.Tables["Messages"], summaryColMap[i, 0], summaryColMap[i, 1]);
            }


            // C A S H  I N   T A B L E
            ctx.ConsoleWriteLogLine("Compress the CashIn Tables: sort by time, visit every row and delete rows that are unchanged from their predecessor");

            // the list of columns to compare
            string[] cashUnitCols = new string[] { "error", "status", "cashin", "count" };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashIn-"))
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

            // add English to CashIn Tables
            string[,] cashUnitColMap = new string[1, 2]
            {
            {"status", "usStatus" }
            };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashIn-"))
               {
                  for (int i = 0; i < 1; i++)
                  {
                     result = _datatable_ops.AddEnglishToTable(ctx, dTable, dTableSet.Tables["Messages"], cashUnitColMap[i, 0], cashUnitColMap[i, 1]);
                  }
               }
            }


            // D E P O S I T   T A B L E

            // add English to Deposit
            string[,] depositColMap = new string[1, 2]
            {
               {"status", "wStatus" }
            };

            try
            {
               foreach (DataTable dTable in dTableSet.Tables)
               {
                  if (dTable.TableName.Equals("Deposit"))
                  {
                     ctx.ConsoleWriteLogLine(String.Format("Adding English to table '{0}'", dTable.TableName));
                     for (int i = 0; i < 1; i++)
                     {
                        result = _datatable_ops.AddEnglishToTable(ctx, dTable, dTableSet.Tables["Messages"], depositColMap[i, 0], depositColMap[i, 1]);
                     }
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION adding English to to table:'{0}'", e.Message));
            }


            // A D D   M O N E Y   T O   T A B L E S

            try
            {
               foreach (DataTable dTable in dTableSet.Tables)
               {
                  if (dTable.TableName.StartsWith("CashIn-") || dTable.TableName.Equals("Deposit"))
                  {
                     ctx.ConsoleWriteLogLine("Adding money to table :" + dTable.TableName);
                     _datatable_ops.AddMoneyToTable(ctx, dTable, dTableSet.Tables["Messages"]);
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("WriteExcelFile Exception ADD MONEY TO TABLES: " + e.Message);

            }


            // A D D   A M O U N T   T O   D E P O S I T   T A B L E

            try
            {
               foreach (DataTable dTable in dTableSet.Tables)
               {
                  if (dTable.TableName.Equals("Deposit"))
                  {
                     ctx.ConsoleWriteLogLine("Adding money to table :" + dTable.TableName);
                     _datatable_ops.AddAmountToTable(ctx, dTable);
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("WriteExcelFile Exception ADD MONEY TO TABLES: " + e.Message);

            }

            // RENAME CASHIN TABLES - DO THIS LAST

            try
            {
               // rename the cash units
               foreach (DataTable dTable in dTableSet.Tables)
               {
                  ctx.ConsoleWriteLogLine("Rename the Cash Units : Looking at table :" + dTable.TableName);
                  if (dTable.TableName.StartsWith("CashIn-"))
                  {
                     string cashUnitNumber = dTable.TableName.Replace("CashIn-", string.Empty);
                     ctx.ConsoleWriteLogLine("cashUnitNumber :" + cashUnitNumber);

                     dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = {0}", cashUnitNumber));
                     if (dataRows.Length == 1)
                     {
                        if (dataRows[0]["denom"].ToString().Trim() == "0")
                        {
                           ctx.ConsoleWriteLogLine("denom == 0");
                           // if currency is "" use the type (e.g. RETRACT, REJECT
                           ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["type"].ToString()));
                           dTable.TableName = dataRows[0]["type"].ToString();
                           dTable.AcceptChanges();

                           // Rename the Dispense column name to match the Logical Unit Name
                           //dTableSet.Tables["Deposit"].Columns["LU" + cashUnitNumber].ColumnName = dTable.TableName;
                        }
                        else
                        {
                           // otherwise combine the currency and denomination
                           ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString()));
                           dTable.TableName = dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString();
                           dTable.AcceptChanges();

                           // Rename the Dispense column name to match the Logical Unit Name
                           //dTableSet.Tables["Deposit"].Columns["LU" + cashUnitNumber].ColumnName = dTable.TableName;
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

      protected void WFS_INF_CIM_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            (bool success, string xfsMatch, string subLogLine) result;

            // fwDevice
            result = _wfs_inf_cim_status.fwDevice(xfsLine);
            if (result.success) dataRow["status"] = result.xfsMatch.Trim();

            // fwSafeDoor
            result = _wfs_inf_cim_status.fwSafeDoor(result.subLogLine);
            if (result.success) dataRow["safedoor"] = result.xfsMatch.Trim();

            // fwAcceptor
            result = _wfs_inf_cim_status.fwAcceptor(result.subLogLine);
            if (result.success) dataRow["acceptor"] = result.xfsMatch.Trim();

            // fwIntermediateStacker
            result = _wfs_inf_cim_status.fwIntermediateStacker(result.subLogLine);
            if (result.success) dataRow["intstack"] = result.xfsMatch.Trim();

            // fwStackerItems
            result = _wfs_inf_cim_status.fwStackerItems(result.subLogLine);
            if (result.success) dataRow["stackitems"] = result.xfsMatch.Trim();

            dTableSet.Tables["Status"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CIM_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_INF_CIM_CASH_UNIT_INFO(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSCIMCASHINFO cashInfo = new WFSCIMCASHINFO(ctx);
            try
            {
               cashInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            if (!have_seen_WFS_INF_CIM_CASH_UNIT_INFO)
            {
               try
               {
                  // First time seeing CASH_UNIT_INFO, populate the Summary Table
                  ctx.ConsoleWriteLogLine("WFS_INF_CIM_CASH_UNIT_INFO First time seeing CASH_UNIT_INFO, populate the Summary Table");
                  have_seen_WFS_INF_CIM_CASH_UNIT_INFO = true;

                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

                  // for each row, set the tracefile, timestamp and hresult
                  for (int i = 0; i < cashInfo.lUnitCount; i++)
                  {
                     try
                     {
                        // Now use the usNumbers to create and populate a row in the CashUnit-x table
                        if (int.Parse(cashInfo.usNumbers[i].Trim()) < 1)
                        {
                           // We have to check because some log lines are truncated (i.e. "more data")
                           // and produce bad results
                           ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                           continue;
                        }

                        if (cashInfo.ulCounts[i] == "0" && cashInfo.ulCashInCounts[i] == "0" && cashInfo.usStatuses[i] == "0")
                        {
                           // again truncated log lines result in garbage output
                           ctx.ConsoleWriteLogLine("usNumbers[i] == 0, continue");
                           continue;
                        }

                        dataRows[i + 1]["file"] = _traceFile;
                        dataRows[i + 1]["time"] = lpResult.tsTimestamp(xfsLine);
                        dataRows[i + 1]["error"] = lpResult.hResult(xfsLine);

                        dataRows[i + 1]["type"] = cashInfo.fwTypes[i];
                        dataRows[i + 1]["name"] = cashInfo.cUnitIDs[i];
                        dataRows[i + 1]["currency"] = cashInfo.cCurrencyIDs[i];
                        dataRows[i + 1]["denom"] = cashInfo.ulValues[i];
                        dataRows[i + 1]["max"] = cashInfo.ulMaximums[i];
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO Summary Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
                     }
                  }

                  dTableSet.Tables["Summary"].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO First Time Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
               }
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
                     ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                     continue;
                  }

                  if (cashInfo.ulCounts[i] == "0" && cashInfo.ulCashInCounts[i] == "0" && cashInfo.usStatuses[i] == "0")
                  {
                     // again truncated log lines result in garbage output
                     ctx.ConsoleWriteLogLine("usNumbers[i] == 0, continue");
                     continue;
                  }

                  string tableName = "CashIn-" + cashInfo.usNumbers[i].Trim();
                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = _traceFile;
                  cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
                  cashUnitRow["error"] = lpResult.hResult(xfsLine);

                  cashUnitRow["status"] = cashInfo.usStatuses[i];
                  cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                  cashUnitRow["count"] = cashInfo.ulCounts[i];

                  try
                  {
                     for (int j = 0; j < 20; j++)
                     {
                        if (!String.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                        {
                           ctx.ConsoleWriteLogLine(String.Format("Setting Notes: i: {0} j: {1}, NoteNum: {2}", i, j, cashInfo.noteNumbers[i, j]));
                           string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                           cashUnitRow["N" + noteNum[0]] = noteNum[1];
                        }
                     }
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO CashIn Setting Notes Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
                  }

                  dTableSet.Tables["CashIn-" + cashInfo.usNumbers[i]].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO CashIn Table Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Exception {0}, {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
         }
      }

      protected void WFS_INF_CIM_CASH_IN_STATUS(string xfsLine)
      {
         try
         {
            // access the values
            (bool success, string xfsMatch, string subLogLine) result;

            result = _wfs_inf_cim_cash_in_status.wStatus(xfsLine);
            string wStatus = result.xfsMatch.Trim();

            result = _wfs_inf_cim_cash_in_status.usNumOfRefused(xfsLine);
            string usNumOfRefused = result.xfsMatch.Trim();

            string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(result.subLogLine);

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["status"] = wStatus;
            dataRow["refused"] = usNumOfRefused;

            try
            {
               for (int j = 0; j < 20; j++)
               {
                  if (!String.IsNullOrEmpty(noteNumberList[0, j]) && noteNumberList[0, j].Contains(":"))
                  {
                     string[] noteNum = noteNumberList[0, j].Split(':');
                     dataRow["N" + noteNum[0]] = noteNum[1];
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_IN_STATUS CashIn Setting Notes Exception {0}, {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CIM_CASH_IN_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_START(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_IN_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Start";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_START Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));
            ctx.ConsoleWriteLogLine(xfsLine);

            // access the values
            (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_cash_in_status.usNumOfRefused(xfsLine);
            string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(result.subLogLine);

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            try
            {
               for (int j = 0; j < 20; j++)
               {
                  if (!String.IsNullOrEmpty(noteNumberList[0, j]) && noteNumberList[0, j].Contains(":"))
                  {
                     string[] noteNum = noteNumberList[0, j].Split(':');
                     dataRow["N" + noteNum[0]] = noteNum[1];
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN CashIn Setting Notes Exception {0}, {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            // position
            dataRow["position"] = "CashIn";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_END(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSCIMCASHINFO cashInfo = new WFSCIMCASHINFO(ctx);
            try
            {
               cashInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "End";
            dataRow["refused"] = "";

            try
            {
               for (int j = 0; j < 20; j++)
               {
                  if (!String.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                  {
                     string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                     dataRow["N" + noteNum[0]] = noteNum[1];
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Setting Notes Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            dTableSet.Tables["Deposit"].AcceptChanges();

            for (int i = 0; i < cashInfo.lUnitCount; i++)
            {
               try
               {
                  // Now use the usNumbers to create and populate a row in the CashUnit-x table
                  if (int.Parse(cashInfo.usNumbers[i].Trim()) < 1)
                  {
                     // We have to check because some log lines are truncated (i.e. "more data")
                     // and produce bad results
                     ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                     continue;
                  }

                  if (cashInfo.ulCounts[i] == "0" && cashInfo.ulCashInCounts[i] == "0" && cashInfo.usStatuses[i] == "0")
                  {
                     // again truncated log lines result in garbage output
                     ctx.ConsoleWriteLogLine("usNumbers[i] == 0, continue");
                     continue;
                  }

                  string tableName = "CashIn-" + cashInfo.usNumbers[i].Trim();
                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = _traceFile;
                  cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
                  cashUnitRow["error"] = lpResult.hResult(xfsLine);

                  cashUnitRow["status"] = cashInfo.usStatuses[i];
                  cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                  cashUnitRow["count"] = cashInfo.ulCounts[i];

                  try
                  {
                     for (int j = 0; j < 20; j++)
                     {
                        if (!String.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                        {
                           string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                           cashUnitRow["N" + noteNum[0]] = noteNum[1];
                        }
                     }
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Setting Notes Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
                  }

                  dTableSet.Tables["CashIn-" + cashInfo.usNumbers[i]].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Table Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_ROLLBACK(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_ROLLBACK tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Rollback";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CAWFS_CMD_CIM_CASH_IN_ROLLBACKSH_IN_ROLLBACK Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_RETRACT(string xfsLine)
      {
         try
         {
            WFSCIMCASHINFO cashInfo = new WFSCIMCASHINFO(ctx);
            try
            {
               cashInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Retract";
            dataRow["refused"] = "";

            try
            {
               for (int j = 0; j < 20; j++)
               {
                  if (!String.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                  {
                     string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                     dataRow["N" + noteNum[0]] = noteNum[1];
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Setting Notes Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            dTableSet.Tables["Deposit"].AcceptChanges();

            for (int i = 0; i < cashInfo.lUnitCount; i++)
            {
               try
               {
                  // Now use the usNumbers to create and populate a row in the CashUnit-x table
                  if (int.Parse(cashInfo.usNumbers[i].Trim()) < 1)
                  {
                     // We have to check because some log lines are truncated (i.e. "more data")
                     // and produce bad results
                     ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                     continue;
                  }

                  if (cashInfo.ulCounts[i] == "0" && cashInfo.ulCashInCounts[i] == "0" && cashInfo.usStatuses[i] == "0")
                  {
                     // again truncated log lines result in garbage output
                     ctx.ConsoleWriteLogLine("usNumbers[i] == 0, continue");
                     continue;
                  }

                  string tableName = "CashIn-" + cashInfo.usNumbers[i].Trim();
                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = _traceFile;
                  cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
                  cashUnitRow["error"] = lpResult.hResult(xfsLine);

                  cashUnitRow["status"] = cashInfo.usStatuses[i];
                  cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                  cashUnitRow["count"] = cashInfo.ulCounts[i];

                  try
                  {
                     for (int j = 0; j < 20; j++)
                     {
                        if (!String.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                        {
                           string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                           cashUnitRow["N" + noteNum[0]] = noteNum[1];
                        }
                     }
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Setting Notes Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
                  }

                  dTableSet.Tables["CashIn-" + cashInfo.usNumbers[i]].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Table Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_RESET(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RESET tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Reset";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_RESET Exception : " + e.Message);
         }
      }

      protected void WFS_USRE_CIM_CASHUNITTHRESHOLD(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_CIM_CASHUNITTHRESHOLD tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSCIMCASHINFO cashInfo = new WFSCIMCASHINFO(ctx);
            try
            {
               cashInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
            int i = int.Parse(cashInfo.usNumbers[0]);

            dataRows[i]["file"] = _traceFile;
            dataRows[i]["time"] = lpResult.tsTimestamp(xfsLine);
            dataRows[i]["error"] = lpResult.hResult(xfsLine);

            dataRows[i]["type"] = cashInfo.fwTypes[0];
            dataRows[i]["name"] = cashInfo.cUnitIDs[0];
            dataRows[i]["currency"] = cashInfo.cCurrencyIDs[0];
            dataRows[i]["denom"] = cashInfo.ulValues[0];
            dataRows[i]["max"] = cashInfo.ulMaximums[0];

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_CIM_CASHUNITTHRESHOLD Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CIM_CASHUNITINFOCHANGED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSCIMCASHINFO cashInfo = new WFSCIMCASHINFO(ctx);
            try
            {
               cashInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
               int i = int.Parse(cashInfo.usNumbers[0]);

               dataRows[i]["file"] = _traceFile;
               dataRows[i]["time"] = lpResult.tsTimestamp(xfsLine);
               dataRows[i]["error"] = lpResult.hResult(xfsLine);

               dataRows[i]["type"] = cashInfo.fwTypes[0];
               dataRows[i]["name"] = cashInfo.cUnitIDs[0];
               dataRows[i]["currency"] = cashInfo.cCurrencyIDs[0];
               dataRows[i]["denom"] = cashInfo.ulValues[0];
               dataRows[i]["max"] = cashInfo.ulMaximums[0];

               dTableSet.Tables["Summary"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED Updating Summary Table Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               string tableName = "CashIn-" + cashInfo.usNumbers[0];
               DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

               cashUnitRow["file"] = _traceFile;
               cashUnitRow["time"] = lpResult.tsTimestamp(xfsLine);
               cashUnitRow["error"] = lpResult.hResult(xfsLine);

               cashUnitRow["status"] = cashInfo.usStatuses[0];
               cashUnitRow["cashin"] = cashInfo.ulCashInCounts[0];
               cashUnitRow["count"] = cashInfo.ulCounts[0];

               try
               {
                  for (int j = 0; j < 20; j++)
                  {
                     if (!String.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                     {
                        string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                        cashUnitRow["N" + noteNum[0]] = noteNum[1];
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED CashIn Setting Notes Exception {0}, {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
               }


               dTableSet.Tables["CashIn-" + cashInfo.usNumbers[0]].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED Updating Summary Table Exception {0}, {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CIM_CASHUNITINFOCHANGED Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CIM_ITEMSTAKEN(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_ITEMSTAKEN tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Items Taken";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CIM_ITEMSTAKEN Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_CIM_INPUTREFUSE(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_CIM_INPUTREFUSE tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            // position
            dataRow["position"] = "Input Refused";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_CIM_INPUTREFUSE Exception : " + e.Message);
         }
      }
   }
}
