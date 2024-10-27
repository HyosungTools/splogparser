using System;
using System.Collections.Generic;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace CashDispView
{
   internal class DispTable : BaseTable
   {
      DataRow m_lcuRow;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public DispTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;
      }

      public override void PostProcess()
      {
         string tableName = string.Empty;

         try
         {
            // S T A T U S  T A B L E

            tableName = "Status";

            // COMPRESS
            string[] columns = new string[] { "error", "device", "position", "dispenser", "shutter", "stacker", "posstatus", "transport" };
            CompressTable(tableName, columns);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            tableName = "Dispense";

            // COMPRESS
            string[] columns = new string[] { "error", "position", "hostamount", "coreaccount", "billmix", "dispensedamount", "LU0", "LU1", "LU2", "LU3", "LU4", "LU5", "LU6", "LU7", "LU8", "LU9" };
            CompressTable(tableName, columns);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // S U M M A R Y  T A B L E

            tableName = "Summary";

            // delete rows where the denom is -1, meaning not set
            DeleteRedundantRows(tableName, "denom", "-1");
            dTableSet.Tables[tableName].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            DataColumnCollection dataColumns;

            // rename the dispense columns to match the note type columns
            DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
            foreach (DataRow dataRow in dataRows)
            {
               dataColumns = dTableSet.Tables["Dispense"].Columns;
               foreach (DataColumn dataColumn in dataColumns)
               {
                  if (dataColumn.ColumnName == "LU" + dataRow["splcu"].ToString())
                  {
                     // the column name is named after a logical unit (e.g. LU1) - rename the column
                     dataColumn.ColumnName = dataRow["currency"].ToString() + dataRow["denom"].ToString();
                  }
               }
            }

            // delete redundant LUx columns from the dispense table
            dataColumns = dTableSet.Tables["Dispense"].Columns;
            List<string> deleteColNames = new List<string>();
            foreach (DataColumn dataColumn in dataColumns)
            {
               if (dataColumn.ColumnName.StartsWith("LU"))
               {
                  deleteColNames.Add(dataColumn.ColumnName);
               }
            }

            foreach (string colName in deleteColNames)
            {
               dTableSet.Tables["Dispense"].Columns.Remove(colName);
            }

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // C S T L I S T  T A B L E

            tableName = "CSTList";

            try
            {
               foreach (DataRow cstListRow in dTableSet.Tables[tableName].Rows)
               {
                  foreach (DataRow summaryRow in dTableSet.Tables["Summary"].Rows)
                  {
                     if (summaryRow["name"].ToString() == cstListRow["notetype"].ToString())
                     {
                        summaryRow["cindex"] = cstListRow["cstindex"].ToString();
                     }
                  }
                  ctx.ConsoleWriteLogLine(message: String.Format("notetype : {0} CSTIndex {1}", cstListRow["notetype"].ToString(), cstListRow["cstindex"].ToString()));
               }

               // delete the table
               dTableSet.Tables.Remove(tableName);
               dTableSet.AcceptChanges();

               foreach (DataTable dataTable in dTableSet.Tables)
               {
                  ctx.ConsoleWriteLogLine(message: String.Format("Post Delete CSTList Table, table name is : {0}", dataTable.TableName));
               }

            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(message: String.Format("PostProcess Exception : {0}",e.Message));

               // delete the table
               dTableSet.Tables.Remove(tableName);
               dTableSet.AcceptChanges();
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         base.PostProcess();
         return;
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         foreach (DataTable dataTable in dTableSet.Tables)
         {
            ctx.ConsoleWriteLogLine(String.Format("WriteExcelFile, table name is : {0}", dataTable.TableName));
         }
         return base.WriteExcelFile();
      }


      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is APLine apLogLine)
            {
               switch (apLogLine.apType)
               {
                  /* UPDATE_STATUS */

                  case APLogType.APLOG_CDM_ONLINE:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "device", "online");
                        break;
                     }
                  case APLogType.APLOG_CDM_OFFLINE:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "device", "offline");
                        break;
                     }
                  case APLogType.APLOG_CDM_ONHWERROR:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "device", "hwerror");
                        break;
                     }
                  case APLogType.APLOG_CDM_DEVERROR:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "device", "deverror");
                        break;
                     }
                  case APLogType.APLOG_CDM_ONOK:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "device", "devok");
                        break;
                     }
                  case APLogType.CashDispenser_NotInPosition:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "position", "notinposition");
                        break;
                     }
                  case APLogType.CashDispenser_InPosition:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "position", "inposition");
                        break;
                     }
                  case APLogType.CashDispenser_OnNoDispense:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "dispenser", "nodispense");
                        break;
                     }
                  case APLogType.CashDispenser_OnDispenserOK:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "dispenser", "ok");
                        break;
                     }
                  case APLogType.CashDispenser_OnShutterOpen:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "shutter", "open");
                        break;
                     }
                  case APLogType.CashDispenser_OnShutterClosed:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "shutter", "closed");
                        break;
                     }
                  case APLogType.CashDispenser_OnStackerNotEmpty:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "stacker", "notempty");
                        break;
                     }
                  case APLogType.CashDispenser_OnStackerEmpty:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "stacker", "empty");
                        break;
                     }
                  case APLogType.CashDispenser_OnPositionNotEmpty:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "posstatus", "notempty");
                        break;
                     }
                  case APLogType.CashDispenser_OnPositionEmpty:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "posstatus", "empty");
                        break;
                     }
                  case APLogType.CashDispenser_OnTransportNotEmpty:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "transport", "notempty");
                        break;
                     }
                  case APLogType.CashDispenser_OnTransportEmpty:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_STATUS(apLogLine, "transport", "empty");
                        break;
                     }

                  /* UPDATE DISPENSE */

                  case APLogType.CashDispenser_OnDispenseComplete:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_DISPENSE(apLogLine, "position", "dispensed");
                        break;
                     }
                  case APLogType.CashDispenser_OnPresentComplete:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_DISPENSE(apLogLine, "position", "presented");
                        break;
                     }
                  case APLogType.CashDispenser_OnRetractComplete:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_DISPENSE(apLogLine, "position", "retracted");
                        break;
                     }
                  case APLogType.CashDispenser_OnItemsTaken:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_DISPENSE(apLogLine, "position", "itemstaken");
                        break;
                     }

                  case APLogType.CashDispenser_OnDenominateComplete:
                     {
                        base.ProcessRow(apLogLine);
                        UPDATE_DISPENSE(apLogLine, "position", "denominated");
                        break;
                     }
                  case APLogType.CashDispenser_ExecDispense:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is CashDispenser_ExecDispense execDispense)
                        {
                           UPDATE_DISPENSE(apLogLine, "hostamount", execDispense.hostAmount);
                        }
                        break;
                     }
                  case APLogType.CashDispenser_UpdateTypeInfoToDispense:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is CashDispenser_UpdateTypeInfoToDispense typeInfoToDispense)
                        {
                           UPDATE_DISPENSE(apLogLine, "dispensedamount", typeInfoToDispense.dispenseAmount);
                        }
                        break;
                     }
                  case APLogType.Core_ProcessWithdrawalTransaction_Account:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is Core_ProcessWithdrawalTransaction_Account wdAccount)
                        {
                           UPDATE_DISPENSE(apLogLine, "coreaccount", wdAccount.account);
                        }
                        break;
                     }
                  case APLogType.Core_DispensedAmount:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is Core_DispensedAmount dispensedAmount)
                        {
                           UPDATE_DISPENSE(apLogLine, "dispensedamount", dispensedAmount.amount);
                        }
                        break;
                     }
                  case APLogType.Core_RequiredBillMixList:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is Core_RequiredBillMixList requiredBillMixList)
                        {
                           UPDATE_DISPENSE_BILLMIX(apLogLine, "required", requiredBillMixList.requiredbillmixlist);
                        }
                        break;
                     }
                  case APLogType.CashDispenser_DispenseSyncAsync:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is CashDispenser_DispenseSyncAsync dispSync)
                        {
                           UPDATE_DISPENSE(dispSync, "", "");
                        }
                        break;
                     }

                  case APLogType.HelperFunctions_GetConfiguredBillMixList:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is APLineField billMixList)
                        {
                           UPDATE_DISPENSE_BILLMIX(billMixList, "configured", billMixList.field);
                        }
                        break;
                     }
                  case APLogType.HelperFunctions_GetFewestBillMixList:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is APLineField billMixList)
                        {
                           UPDATE_DISPENSE_BILLMIX(billMixList, "fewest", billMixList.field);
                        }
                        break;
                     }

                  /* UPDATE SUMMARY */

                  case APLogType.CashDispenser_SetupCSTList:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is CashDispenser_SetupCSTList setupCSTList)
                        {
                           SETUP_CSTLIST(setupCSTList);
                        }
                        break;
                     }

                  case APLogType.CashDispenser_SetupNoteType:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is CashDispenser_SetupNoteType setupNoteType)
                        {
                           SETUP_NOTETYPE(setupNoteType);
                        }
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


      protected void UPDATE_STATUS(APLine apLogLine, string colName, string newStatus)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = apLogLine.LogFile;
            dataRow["time"] = apLogLine.Timestamp;
            dataRow["error"] = apLogLine.HResult;

            dataRow[colName] = newStatus;

            dTableSet.Tables["Status"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("STATUS_DEVICE Exception : " + e.Message);
         }

         return;
      }

      protected void UPDATE_DISPENSE(APLine apLogLine, string colName, string newStatus)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = apLogLine.LogFile;
            dataRow["time"] = apLogLine.Timestamp;
            dataRow["error"] = apLogLine.HResult;

            if (!string.IsNullOrEmpty(colName))
            {
               dataRow[colName] = newStatus;
            }

            if (apLogLine is CashDispenser_DispenseSyncAsync dispSync)
            {
               for (int i = 0; i < dispSync.dispense.Length; i++)
               {
                  dataRow["LU" + i.ToString()] = dispSync.dispense[i];
               }
            }

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UPDATE_DISPENSE Exception : " + e.Message);
         }

         return;
      }

      protected void UPDATE_DISPENSE_BILLMIX(APLine apLogLine, string posValue, string billMixValue)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = apLogLine.LogFile;
            dataRow["time"] = apLogLine.Timestamp;
            dataRow["error"] = apLogLine.HResult;

            dataRow["position"] = posValue;
            dataRow["billmix"] = billMixValue;

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UPDATE_DISPENSE Exception : " + e.Message);
         }

         return;
      }

      protected void UPDATE_DISPSYNCASYNC(CashDispenser_DispenseSyncAsync dispSync)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("UPDATE_DISPSYNCASYNC dispSync.dispense.Length = {0}", dispSync.dispense.Length));

            DataRow dataRow = dTableSet.Tables["Dispense"].Rows.Add();

            dataRow["file"] = dispSync.LogFile;
            dataRow["time"] = dispSync.Timestamp;
            dataRow["error"] = dispSync.HResult;

            for (int i = 0; i < dispSync.dispense.Length; i++)
            {
               dataRow["LU" + i.ToString()] = dispSync.dispense[i];
            }

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UPDATE_DISPENSE Exception : " + e.Message);
         }

         return;
      }

      protected void UPDATE_LDC(CashDispenser_GetLCULastDispensedCount lastDispensedCount)
      {
         try
         {
            if (lastDispensedCount.noteType == "A")
            {
               m_lcuRow = dTableSet.Tables["Dispense"].Rows.Add();
            }

            m_lcuRow["file"] = lastDispensedCount.LogFile;
            m_lcuRow["time"] = lastDispensedCount.Timestamp;
            m_lcuRow["error"] = lastDispensedCount.HResult;

            m_lcuRow["position"] = "lastdispense";

            m_lcuRow[lastDispensedCount.noteType] = lastDispensedCount.amount;

            dTableSet.Tables["Dispense"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UPDATE_DISPENSE Exception : " + e.Message);
         }

         return;
      }

      protected void SETUP_CSTLIST(CashDispenser_SetupCSTList setupCSTList)
      {
         try
         {
            foreach (DataRow dataRow in dTableSet.Tables["CSTList"].Rows)
            {
               if (dataRow["notetype"].ToString() == setupCSTList.parent)
               {
                  if (dataRow["cstindex"].ToString().IndexOf(setupCSTList.child) == -1)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("Before Update - dataRow[notetype] = {0}, dataRow[cstindex] = {1}", dataRow["notetype"].ToString(), dataRow["cstindex"].ToString()));
                     dataRow["cstindex"] = dataRow["cstindex"].ToString() + setupCSTList.child + " ";
                     ctx.ConsoleWriteLogLine(String.Format("After  Update - dataRow[notetype] = {0}, dataRow[cstindex] = {1}", dataRow["notetype"].ToString(), dataRow["cstindex"].ToString()));
                     dTableSet.Tables["CSTList"].AcceptChanges();
                  }
                  break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("SETUP_CSTLIST Exception : " + e.Message);
         }
      }

      protected void SETUP_NOTETYPE(CashDispenser_SetupNoteType setupNoteType)
      {
         try
         {
            foreach (DataRow dataRow in dTableSet.Tables["Summary"].Rows)
            {
               if (dataRow["name"].ToString() == setupNoteType.noteType)
               {
                  dataRow["file"] = setupNoteType.LogFile;
                  dataRow["time"] = setupNoteType.Timestamp;
                  dataRow["error"] = setupNoteType.HResult;

                  dataRow["name"] = setupNoteType.noteType;
                  dataRow["currency"] = setupNoteType.currency;
                  dataRow["denom"] = setupNoteType.value;
                  dataRow["splcu"] = setupNoteType.splcu;
                  dataRow["sppcu"] = setupNoteType.sppcu;

                  dTableSet.Tables["Summary"].AcceptChanges();
                  break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("SETUP_NOTETYPE Exception : " + e.Message);
         }
      }
   }
}

