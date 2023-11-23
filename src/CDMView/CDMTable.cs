using Contract;
using Impl;
using System;
using System.Data;
using LogLineHandler;

namespace CDMView
{

   internal class CDMTable : BaseTable
   {
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
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is SPLine spLogLine)
            {
               switch (spLogLine.xfsType)
               {
                  case LogLineHandler.XFSType.WFS_INF_CDM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CDM_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CDM_CASH_UNIT_INFO:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CDM_CASH_UNIT_INFO(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CDM_PRESENT_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CDM_PRESENT_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CDM_DISPENSE:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CDM_DISPENSE(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CDM_PRESENT:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CDM_PRESENT(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CDM_REJECT:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CDM_REJECT(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CDM_RETRACT:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CDM_RETRACT(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CDM_RESET:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CDM_RESET(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CDM_STARTEX:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CDM_STARTEX(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CDM_ENDEX:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CDM_ENDEX(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_CDM_CASHUNITINFOCHANGED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CDM_ITEMSTAKEN:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_CDM_ITEMSTAKEN(spLogLine);
                        break;
                     }
                  default:
                     break;

               }
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
         string tableName = string.Empty;

         try
         {
            // S T A T U S  T A B L E

            tableName = "Status";

            // COMPRESS
            string[] columns = new string[] { "error", "status", "dispenser", "intstack", "shutter", "posstatus", "transport", "transstat", "position" };
            CompressTable(tableName, columns);

            // ADD ENGLISH 
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
            AddEnglishToTable(tableName, colKeyMap);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // S U M M A R Y  T A B L E

            tableName = "Summary";

            // COMPRESS
            DeleteRedundantRows(tableName);

            // ADD ENGLISH 
            string[,] colKeyMap = new string[1, 2]
            {
               {"type", "usType" }
            };
            AddEnglishToTable(tableName, colKeyMap);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // C A S H  U N I T   T A B L E S

            string[] columns = new string[] { "error", "status", "initial", "count", "reject", "dispensed", "presented", "retracted" };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashUnit-"))
               {
                  // COMPRESS
                  tableName = dTable.TableName;
                  CompressTable(tableName, columns);
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
               if (dTable.TableName.StartsWith("CashUnit-"))
               {
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // D I S P E N S E  T A B L E 

            tableName = "Dispense";

            // ADD ENGLISH 
            string[,] colKeyMap = new string[1, 2]
            {
               {"position", "wPresentState" }
            };
            AddEnglishToTable(tableName, colKeyMap);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // R E N A M E  C A S H  U N I T S

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Rename the Cash Units : Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashUnit-"))
               {
                  // Isolate the Number (e.g. from CashUnit-1, isolate the '1')
                  string cashUnitNumber = dTable.TableName.Replace("CashUnit-", string.Empty);

                  // From the Summary Table, find the row where number matches the cashUnitNumber (should only be one)
                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = {0}", cashUnitNumber));
                  if (dataRows.Length == 1)
                  {
                     if (dataRows[0]["denom"].ToString().Trim() == "0")
                     {
                        // if currency is "" use the type (e.g. RETRACT, REJECT, RETAIN)
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

      protected void WFS_INF_CDM_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCDMSTATUS)
            {
               WFSCDMSTATUS cdmStatus = (LogLineHandler.WFSCDMSTATUS)spLogLine;

               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

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
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CDM_STATUS Exception : " + e.Message);
         }

         return;
      }
      protected void WFS_INF_CDM_CASH_UNIT_INFO(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSCDMCUINFO cashInfo)
            {
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
                     dataRows[usNumber]["file"] = spLogLine.LogFile;
                     dataRows[usNumber]["time"] = spLogLine.Timestamp;
                     dataRows[usNumber]["error"] = spLogLine.HResult;

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
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Summary Table Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                  }

               }

               dTableSet.Tables["Summary"].AcceptChanges();

               for (int i = 0; i < cashInfo.lUnitCount; i++)
               {
                  try
                  {
                     // Now use the usNumber to create and populate a row in the CashUnit- table
                     int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                     string tableName = "CashUnit-" + usNumber.ToString();
                     //ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO cashInfo.usNumbers[{0}] = '{1}' tableName = '{2}': ", i, usNumber, tableName));

                     if (usNumber < 1)
                     {
                        // We have to check because some log lines are truncated (i.e. "more data")
                        // and produce bad results
                        continue;
                     }

                     DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                     cashUnitRow["file"] = spLogLine.LogFile;
                     cashUnitRow["time"] = spLogLine.Timestamp;
                     cashUnitRow["error"] = spLogLine.HResult;

                     cashUnitRow["initial"] = cashInfo.ulInitialCounts[i];
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
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Cash Unit Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO  End of Function Exception : " + e.Message);
         }
      }
      protected void WFS_INF_CDM_PRESENT_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCDMPRESENTSTATUS cdmPresentStatus)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();
               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

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
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO  End of Function Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_DISPENSE(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCDMDENOMINATION)
            {
               WFSCDMDENOMINATION cdmDenom = (LogLineHandler.WFSCDMDENOMINATION)spLogLine;

               // add new row
               DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();
               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

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
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_DISPENSE Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_PRESENT(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "present";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_PRESENT Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_REJECT(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "reject";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_REJECT Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_RETRACT(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "retract";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_RETRACT Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_RESET(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "reset";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_RESET Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_STARTEX(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "startex";

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_STARTEX Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CDM_ENDEX(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "endex";

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CDM_ENDEX Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CDM_CASHUNITINFOCHANGED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCDMCUINFO cashInfo)
            {
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
                  //ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CDM_CASHUNITINFOCHANGED usNumber = '{0}' tableName = '{1}': ", usNumber, tableName));

                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = spLogLine.LogFile;
                  cashUnitRow["time"] = spLogLine.Timestamp;
                  cashUnitRow["error"] = spLogLine.HResult;

                  cashUnitRow["initial"] = cashInfo.ulInitialCounts[0];
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
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_CASHUNITINFOCHANGED Exception : " + e.Message);
         }
      }
      protected void WFS_USRE_CDM_CASHUNITTHRESHOLD(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCDMCUINFO cashInfo)
            {
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
                  //ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_CIM_CASHUNITTHRESHOLD usNumber = '{0}' tableName = '{1}': ", usNumber, tableName));

                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = spLogLine.LogFile;
                  cashUnitRow["time"] = spLogLine.Timestamp;
                  cashUnitRow["error"] = spLogLine.HResult;

                  cashUnitRow["initial"] = cashInfo.ulInitialCounts[0];
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
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_CIM_CASHUNITTHRESHOLD Exception : " + e.Message);
         }
      }
      protected void WFS_SRVE_CDM_ITEMSTAKEN(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

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
