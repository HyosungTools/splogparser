using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;

namespace SIUView
{
   internal class SIUTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public SIUTable(IContext ctx, string viewName) : base(ctx, viewName)
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
            string[] columns = new string[] { "error", "safe", "device", "opswitch", "tamper", "inttamper", "cabinet", "ups", "audio", "errorcode", "description" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            ctx.ConsoleWriteLogLine(String.Format("Add English to {0} Table", tableName));
            string[,] colKeyMap = new string[8, 2]
            {
               {"safe", "fwSafeDoor" },
               {"device", "fwDevice"},
               {"opswitch", "opSwitch" },
               {"tamper", "tamper"},
               {"inttamper", "intTamper"},
               {"cabinet", "cabinet"},
               {"ups", "ups" },
               {"audio", "audio" }
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
                  case LogLineHandler.XFSType.WFS_INF_SIU_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_SIU_STATUS(spLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("SIUTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void WFS_INF_SIU_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSSIUSTATUS siuStatus)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               dataRow["safe"] = siuStatus.fwSafeDoor;
               dataRow["device"] = siuStatus.fwDevice;
               dataRow["opswitch"] = siuStatus.opSwitch;
               dataRow["tamper"] = siuStatus.tamper;
               dataRow["inttamper"] = siuStatus.intTamper;
               dataRow["cabinet"] = siuStatus.cabinet;
               dataRow["ups"] = siuStatus.ups;
               dataRow["audio"] = siuStatus.audio;
               dataRow["description"] = siuStatus.description;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_SIU_STATUS Exception : " + e.Message);
         }
      }
   }
}
