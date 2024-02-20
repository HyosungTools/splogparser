using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;

namespace IDCView
{
   internal class IDCTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public IDCTable(IContext ctx, string viewName) : base(ctx, viewName)
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
            string[] columns = new string[] { "error", "device", "media", "retainbin", "security", "uscards", "chippower", "chipmodule", "magreadmodule", "errorcode" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            ctx.ConsoleWriteLogLine(String.Format("Add English to {0} Table", tableName));
            string[,] colKeyMap = new string[7, 2]
            {
                {"device", "fwDevice"},
                {"media", "fwMedia"},
                {"retainbin", "fwRetainBin"},
                {"security", "fwSecurity"},
                {"chippower", "fwChipPower"},
                {"chipmodule", "fwChipModule"},
                {"magreadmodule", "fwMagReadModule"}
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
                  case LogLineHandler.XFSType.WFS_INF_IDC_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_IDC_STATUS(spLogLine);
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_CMD_IDC_READ_RAW_DATA:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "device", "readraw");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_CMD_IDC_CHIP_IO:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "device", "readchip");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_CMD_IDC_CHIP_POWER:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "device", "powerchip");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_EXEE_IDC_INVALIDTRACKDATA:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "trackdata", "invaliddata");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_EXEE_IDC_MEDIAINSERTED:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "media", "inserted");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_SRVE_IDC_MEDIAREMOVED:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "media", "removed");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_USRE_IDC_RETAINBINTHRESHOLD:
                     {
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_EXEE_IDC_INVALIDMEDIA:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "media", "invalid");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_EXEE_IDC_MEDIARETAINED:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "media", "retained");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_SRVE_IDC_MEDIADETECTED:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "media", "detected");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_SRVE_IDC_RETAINBININSERTED:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "retainbin", "inserted");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_SRVE_IDC_RETAINBINREMOVED:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "retainbin", "removed");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_EXEE_IDC_INSERTCARD:
                     {
                        base.ProcessRow(spLogLine);
                        UpdateStatus(spLogLine, "device", "ready for card");
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_SRVE_IDC_DEVICEPOSITION:
                     {
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_SRVE_IDC_POWER_SAVE_CHANGE:
                     {
                        break;
                     }

                  case LogLineHandler.XFSType.WFS_EXEE_IDC_TRACKDETECTED:
                     {
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("IDCTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void WFS_INF_IDC_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSIDCSTATUS idcStatus)
            {
               try
               {
                  DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

                  dataRow["file"] = spLogLine.LogFile;
                  dataRow["time"] = spLogLine.Timestamp;
                  dataRow["error"] = spLogLine.HResult;
                  dataRow["device"] = idcStatus.fwDevice;
                  dataRow["media"] = idcStatus.fwMedia;
                  dataRow["trackdata"] = string.Empty;
                  dataRow["retainbin"] = idcStatus.fwRetainBin;
                  dataRow["security"] = idcStatus.fwSecurity;
                  dataRow["uscards"] = idcStatus.usCards;
                  dataRow["chippower"] = idcStatus.fwChipPower;
                  dataRow["chipmodule"] = idcStatus.fwChipModule;
                  dataRow["magreadmodule"] = idcStatus.fwMagReadModule;

                  dTableSet.Tables["Status"].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IDC_STATUS Status Table Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IDC_STATUS Exception : " + e.Message);
         }
      }

      protected void UpdateStatus(SPLine spLogLine, string column, string newStatus)
      {
         try
         {
            if (spLogLine is WFSDEVSTATUS idcStatus)
            {
               try
               {
                  DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

                  dataRow["file"] = spLogLine.LogFile;
                  dataRow["time"] = spLogLine.Timestamp;
                  dataRow["error"] = spLogLine.HResult;
                  dataRow["device"] = string.Empty;
                  dataRow["media"] = string.Empty;
                  dataRow["trackdata"] = string.Empty;
                  dataRow["retainbin"] = string.Empty;
                  dataRow["security"] = string.Empty;
                  dataRow["uscards"] = string.Empty;
                  dataRow["chippower"] = string.Empty;
                  dataRow["chipmodule"] = string.Empty;
                  dataRow["magreadmodule"] = string.Empty;

                  dataRow[column] = newStatus;

                  dTableSet.Tables["Status"].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("UpdateStatus: Status Table Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IDC_STATUS Exception : " + e.Message);
         }
      }
   }
}
