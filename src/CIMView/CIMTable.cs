using Contract;
using Impl;
using System;
using System.Data;
using LogLineHandler;

namespace CIMView
{
   internal class CIMTable : BaseTable
   {
      private readonly bool have_seen_WFS_INF_CIM_CASH_UNIT_INFO = false;

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
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is SPLine spLogLine)
            {
               switch (spLogLine.xfsType)
               {
                  case LogLineHandler.XFSType.WFS_INF_CIM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CIM_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CIM_CASH_UNIT_INFO:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CIM_CASH_UNIT_INFO(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CIM_CASH_IN_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CIM_CASH_IN_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN_START:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN_START(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN_END:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN_END(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN_ROLLBACK(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_RETRACT:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_RETRACT(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_RESET:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_RESET(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_STARTEX:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_STARTEX(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_ENDEX:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_ENDEX(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_USRE_CIM_CASHUNITTHRESHOLD(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_CIM_CASHUNITINFOCHANGED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_CIM_ITEMSTAKEN(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_CIM_INPUTREFUSE(spLogLine);
                        break;
                     }
                  default:
                     break;
               }
            }
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
         string tableName = string.Empty;

         try
         {
            // S T A T U S   T A B L E

            tableName = "Status";

            // COMPRESS
            string[] columns = new string[] { "error", "status", "safedoor", "acceptor", "intstack", "stackitems" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            string[,] colKeyMap = new string[5, 2]
            {
               {"status", "fwDevice" },
               {"safedoor", "fwSafeDoor;"},
               {"acceptor", "fwAcceptor" },
               {"intstack", "fwIntermediateStacker" },
               {"stackitems", "fwStackerItems" }
            };
            AddEnglishToTable(tableName, colKeyMap);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         try
         {
            // S U M M A R Y   T A B L E 

            tableName = "Summary";

            // COMPRESS
            DeleteRedundantRows(tableName);

            // ADD ENGLISH
            string[,] colKeyMap = new string[1, 2]
            {
               {"type", "fwType" }
            };
            AddEnglishToTable(tableName, colKeyMap);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         try
         {
            // C A S H  I N   T A B L E

            ctx.ConsoleWriteLogLine("Compress the CashIn Tables: sort by time, visit every row and delete rows that are unchanged from their predecessor");

            // the list of columns to compare
            string[] cashUnitCols = new string[] { "error", "status", "cashin", "count" };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashIn-"))
               {
                  // COMPRESS
                  tableName = dTable.TableName;
                  CompressTable(tableName, cashUnitCols);
               }
            }

            // ADD ENGLISH
            string[,] colKeyMap = new string[1, 2]
            {
               {"status", "usStatus" }
            };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashIn-"))
               {
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         try
         {
            // D E P O S I T   T A B L E

            // ADD ENGLISH
            string[,] colKeyMap = new string[2, 2]
            {
               {"position", "position" },
               {"status", "wStatus" }
            };


            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.Equals("Deposit"))
               {
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
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

                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = {0}", cashUnitNumber));
                  if (dataRows.Length == 1)
                  {
                     if (dataRows[0]["denom"].ToString().Trim() == "0")
                     {
                        ctx.ConsoleWriteLogLine("denom == 0");
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

            // TODO - delete any CashUnit column named LUx
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception : " + e.Message);
         }

         return base.WriteExcelFile();
      }

      protected void WFS_INF_CIM_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMSTATUS cimStatus)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               dataRow["status"] = cimStatus.fwDevice;
               dataRow["safedoor"] = cimStatus.fwSafeDoor;
               dataRow["acceptor"] = cimStatus.fwAcceptor;
               dataRow["intstack"] = cimStatus.fwIntStacker;
               dataRow["stackitems"] = cimStatus.fwStackerItems;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CIM_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_INF_CIM_CASH_UNIT_INFO(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               if (!have_seen_WFS_INF_CIM_CASH_UNIT_INFO)
               {
                  try
                  {
                     // First time seeing CASH_UNIT_INFO, populate the Summary Table
                     // have_seen_WFS_INF_CIM_CASH_UNIT_INFO = true;

                     DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

                     // for each row, set the tracefile, timestamp and hresult
                     for (int i = 0; i < cashInfo.lUnitCount; i++)
                     {
                        try
                        {
                           // Now use the usNumbers to create and populate a row in the CashUnit-x table
                           int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                           if (usNumber < 1)
                           {
                              // We have to check because some log lines are truncated (i.e. "more data")
                              // and produce bad results
                              ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                              continue;
                           }

                           dataRows[usNumber]["file"] = spLogLine.LogFile;
                           dataRows[usNumber]["time"] = spLogLine.Timestamp;
                           dataRows[usNumber]["error"] = spLogLine.HResult;

                           dataRows[usNumber]["type"] = cashInfo.fwTypes[i];
                           dataRows[usNumber]["name"] = cashInfo.cUnitIDs[i];
                           dataRows[usNumber]["currency"] = cashInfo.cCurrencyIDs[i];
                           dataRows[usNumber]["denom"] = cashInfo.ulValues[i];
                           dataRows[usNumber]["max"] = cashInfo.ulMaximums[i];
                        }
                        catch (Exception e)
                        {
                           ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO Summary Table Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                        }
                     }

                     dTableSet.Tables["Summary"].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO First Time Assignment Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                  }
               }

               for (int i = 0; i < cashInfo.lUnitCount; i++)
               {
                  try
                  {
                     // Now use the usNumbers to create and populate a row in the CashUnit-x table
                     int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                     if (usNumber < 1)
                     {
                        // We have to check because some log lines are truncated (i.e. "more data")
                        // and produce bad results
                        ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                        continue;
                     }

                     string tableName = "CashIn-" + usNumber.ToString();
                     DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                     cashUnitRow["file"] = spLogLine.LogFile;
                     cashUnitRow["time"] = spLogLine.Timestamp;
                     cashUnitRow["error"] = spLogLine.HResult;

                     cashUnitRow["status"] = cashInfo.usStatuses[i];
                     cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                     cashUnitRow["count"] = cashInfo.ulCounts[i];

                     try
                     {
                        for (int j = 0; j < 20; j++)
                        {
                           if (!string.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                           {
                              string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                              cashUnitRow["N" + noteNum[0]] = noteNum[1];
                           }
                        }
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO CashIn Setting Notes Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                     }

                     dTableSet.Tables[tableName].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO CashIn Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
         }
      }

      protected void WFS_INF_CIM_CASH_IN_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINSTATUS cashIn)
            {

               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["status"] = cashIn.wStatus;
               dataRow["refused"] = cashIn.usNumOfRefused;

               try
               {
                  for (int j = 0; j < 20; j++)
                  {
                     if (!string.IsNullOrEmpty(cashIn.noteNumbers[0, j]) && cashIn.noteNumbers[0, j].Contains(":"))
                     {
                        string[] noteNum = cashIn.noteNumbers[0, j].Split(':');
                        dataRow["N" + noteNum[0]] = noteNum[1];
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_IN_STATUS CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CIM_CASH_IN_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_START(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "start";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_START Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN(SPLine spLogLine)
      {
         try
         {
            // access the values
            (bool success, string xfsMatch, string subLogLine) result = LogLineHandler.WFSCIMCASHINSTATUS.usNumOfRefusedFromList(spLogLine.logLine);
            string[,] noteNumberList = LogLineHandler.WFSCIMNOTENUMBERLIST.NoteNumberListFromList(spLogLine.logLine);

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            try
            {
               for (int j = 0; j < 20; j++)
               {
                  if (!string.IsNullOrEmpty(noteNumberList[0, j]) && noteNumberList[0, j].Contains(":"))
                  {
                     string[] noteNum = noteNumberList[0, j].Split(':');
                     dataRow["N" + noteNum[0]] = noteNum[1];
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
            }

            // position
            dataRow["position"] = "cash in";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_END(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSCIMCASHINFO cashInfo)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["position"] = "end";
               dataRow["refused"] = "";

               try
               {
                  if (cashInfo.lUnitCount > 0)
                  {

                     for (int j = 0; j < 20; j++)
                     {
                        if (!string.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                        {
                           string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                           dataRow["N" + noteNum[0]] = noteNum[1];
                        }
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }

               dTableSet.Tables["Deposit"].AcceptChanges();

               for (int i = 0; i < cashInfo.lUnitCount; i++)
               {
                  try
                  {
                     // Now use the usNumbers to create and populate a row in the CashUnit-x table
                     int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                     if (usNumber < 1)
                     {
                        // We have to check because some log lines are truncated (i.e. "more data")
                        // and produce bad results
                        ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                        continue;
                     }

                     string tableName = "CashIn-" + usNumber.ToString();
                     DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                     cashUnitRow["file"] = spLogLine.LogFile;
                     cashUnitRow["time"] = spLogLine.Timestamp;
                     cashUnitRow["error"] = spLogLine.HResult;

                     cashUnitRow["status"] = cashInfo.usStatuses[i];
                     cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                     cashUnitRow["count"] = cashInfo.ulCounts[i];

                     try
                     {
                        for (int j = 0; j < 20; j++)
                        {
                           if (!string.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                           {
                              string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                              cashUnitRow["N" + noteNum[0]] = noteNum[1];
                           }
                        }
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Setting Notes Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                     }

                     dTableSet.Tables[tableName].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_ROLLBACK(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "rollback";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_ROLLBACK Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_RETRACT(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["position"] = "retract";
               dataRow["refused"] = "";

               try
               {
                  for (int j = 0; j < 20; j++)
                  {
                     if (!string.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                     {
                        string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                        dataRow["N" + noteNum[0]] = noteNum[1];
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
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

                     cashUnitRow["file"] = spLogLine.LogFile;
                     cashUnitRow["time"] = spLogLine.Timestamp;
                     cashUnitRow["error"] = spLogLine.HResult;

                     cashUnitRow["status"] = cashInfo.usStatuses[i];
                     cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                     cashUnitRow["count"] = cashInfo.ulCounts[i];

                     try
                     {
                        for (int j = 0; j < 20; j++)
                        {
                           if (!string.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                           {
                              string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                              cashUnitRow["N" + noteNum[0]] = noteNum[1];
                           }
                        }
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Setting Notes Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                     }

                     dTableSet.Tables["CashIn-" + cashInfo.usNumbers[i]].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_RESET(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "reset";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_RESET Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_STARTEX(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "startex";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_STARTEX Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CIM_ENDEX(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "endex";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_ENDEX Exception : " + e.Message);
         }
      }
      protected void WFS_USRE_CIM_CASHUNITTHRESHOLD(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
               int i = int.Parse(cashInfo.usNumbers[0]);

               dataRows[i]["file"] = spLogLine.LogFile;
               dataRows[i]["time"] = spLogLine.Timestamp;
               dataRows[i]["error"] = spLogLine.HResult;

               dataRows[i]["type"] = cashInfo.fwTypes[0];
               dataRows[i]["name"] = cashInfo.cUnitIDs[0];
               dataRows[i]["currency"] = cashInfo.cCurrencyIDs[0];
               dataRows[i]["denom"] = cashInfo.ulValues[0];
               dataRows[i]["max"] = cashInfo.ulMaximums[0];

            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_CIM_CASHUNITTHRESHOLD Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CIM_CASHUNITINFOCHANGED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               try
               {
                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
                  int i = int.Parse(cashInfo.usNumbers[0]);

                  dataRows[i]["file"] = spLogLine.LogFile;
                  dataRows[i]["time"] = spLogLine.Timestamp;
                  dataRows[i]["error"] = spLogLine.HResult;

                  dataRows[i]["type"] = cashInfo.fwTypes[0];
                  dataRows[i]["name"] = cashInfo.cUnitIDs[0];
                  dataRows[i]["currency"] = cashInfo.cCurrencyIDs[0];
                  dataRows[i]["denom"] = cashInfo.ulValues[0];
                  dataRows[i]["max"] = cashInfo.ulMaximums[0];

                  dTableSet.Tables["Summary"].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED Updating Summary Table Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }

               try
               {
                  string tableName = "CashIn-" + cashInfo.usNumbers[0];
                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = spLogLine.LogFile;
                  cashUnitRow["time"] = spLogLine.Timestamp;
                  cashUnitRow["error"] = spLogLine.HResult;

                  cashUnitRow["status"] = cashInfo.usStatuses[0];
                  cashUnitRow["cashin"] = cashInfo.ulCashInCounts[0];
                  cashUnitRow["count"] = cashInfo.ulCounts[0];

                  try
                  {
                     for (int j = 0; j < 20; j++)
                     {
                        if (!string.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                        {
                           string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                           cashUnitRow["N" + noteNum[0]] = noteNum[1];
                        }
                     }
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                  }


                  dTableSet.Tables["CashIn-" + cashInfo.usNumbers[0]].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED Updating Summary Table Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CIM_CASHUNITINFOCHANGED Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CIM_ITEMSTAKEN(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "items taken";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CIM_ITEMSTAKEN Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_CIM_INPUTREFUSE(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMINPUTREFUSE cimRefused)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["position"] = String.Format("input refused-{0}", cimRefused.usReason);
               dataRow["refused"] = "";

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_CIM_INPUTREFUSE Exception : " + e.Message);
         }
      }
   }
}
