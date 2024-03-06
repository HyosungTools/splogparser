using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;

namespace PINView
{
   internal class PINTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public PINTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;

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
            string[] columns = new string[] { "error", "device", "encstat", "autobeepmode", "certificatestate", "deviceposition", "powersaverecoverytime", "antifraudmodule", "completion" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            ctx.ConsoleWriteLogLine(String.Format("Add English to {0} Table", tableName));
            string[,] colKeyMap = new string[8, 2]
            {
                {"device", "fwDevice"},
                {"encstat", "fwEncStat"},
                {"autobeepmode", "fwAutoBeepMode"},
                {"certificatestate", "dwCertificateState"},
                {"deviceposition", "wDevicePosition"},
                {"powersaverecoverytime", "usPowerSaveRecoveryTime"},
                {"antifraudmodule", "wAntiFraudModule"},
                {"completion", "wCompletion"},
            };
            AddEnglishToTable(tableName, colKeyMap);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
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
            if (logLine is SPLine spLogLine)
            {
               switch (spLogLine.xfsType)
               {
                  case LogLineHandler.XFSType.WFS_INF_PIN_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_PIN_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_PIN_GET_PIN:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateColumn(spLogLine, "status", "storepin");
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_PIN_GET_PINBLOCK:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateColumn(spLogLine, "status", "getpinblock");
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_PIN_GET_DATA:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateColumn(spLogLine, "status", "getkeypress");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_CMD_PIN_RESET:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateColumn(spLogLine, "status", "reset");
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_PIN_KEY:
                     {
                        base.ProcessRow(spLogLine);
                        ActiveKey(spLogLine);
                        break;
                     }
                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("PINTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void WFS_INF_PIN_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSPINSTATUS pinStatus)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;
               dataRow["device"] = pinStatus.fwDevice;
               dataRow["encstat"] = pinStatus.fwEncStat;
               dataRow["autobeepmode"] = pinStatus.fwAutoBeepMode;
               dataRow["certificatestate"] = pinStatus.dwCertificateState;
               dataRow["deviceposition"] = pinStatus.wDevicePosition;
               dataRow["powersaverecoverytime"] = pinStatus.usPowerSaveRecoveryTime;
               dataRow["antifraudmodule"] = pinStatus.wAntiFraudModule;
               dataRow["status"] = string.Empty;
               dataRow["completion"] = string.Empty;
               dataRow["fkfdk"] = string.Empty;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_PIN_STATUS Exception : " + e.Message);
         }
      }
      protected void UpdateColumn(SPLine spLogLine, string column, string value)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;
            dataRow["device"] = string.Empty;
            dataRow["encstat"] = string.Empty;
            dataRow["autobeepmode"] = string.Empty;
            dataRow["certificatestate"] = string.Empty;
            dataRow["deviceposition"] = string.Empty;
            dataRow["powersaverecoverytime"] = string.Empty;
            dataRow["antifraudmodule"] = string.Empty;
            dataRow["status"] = string.Empty;
            dataRow["completion"] = string.Empty;
            dataRow["fkfdk"] = string.Empty;

            dataRow[column] = value;

            dTableSet.Tables["Status"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UpdateStatus Exception : " + e.Message);
         }
      }
      protected void ActiveKey(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSPINKEY pinKey)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;
               dataRow["device"] = string.Empty;
               dataRow["encstat"] = string.Empty;
               dataRow["autobeepmode"] = string.Empty;
               dataRow["certificatestate"] = string.Empty;
               dataRow["deviceposition"] = string.Empty;
               dataRow["powersaverecoverytime"] = string.Empty;
               dataRow["antifraudmodule"] = string.Empty;
               dataRow["status"] = string.Empty;
               dataRow["completion"] = string.Empty;

               dataRow["completion"] = pinKey.wCompletion;
               dataRow["fkfdk"] = pinKey.ulDigit;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UpdateStatus Exception : " + e.Message);
         }
      }
   }
}
