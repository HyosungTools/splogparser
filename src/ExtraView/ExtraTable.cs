using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;

namespace ExtraView
{
   internal class ExtraTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public ExtraTable(IContext ctx, string viewName) : base(ctx, viewName)
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
                  case LogLineHandler.XFSType.WFS_INF_PTR_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("PTR", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_IDC_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("IDC", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CDM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("CDM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_PIN_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("PIN", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CHK_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("CHK", spLogLine);
                        break;
                     }
                  //case XFSType.WF_INF_DEP_STATUS:
                  //   {
                  //      base.ProcessRow(traceFile, logLine);
                  //      WF_INF_DEP_STATUS(result.xfsLine);
                  //      break;
                  //   }
                  case LogLineHandler.XFSType.WFS_INF_TTU_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("TTU", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_SIU_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("SIU", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_VDM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("VDM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CAM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("CAM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_ALM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("ALM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CIM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("CIM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_BCR_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("BCR", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_IPM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_Extra_STATUS("IPM", spLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("ExtraTable.ProcessRow EXCEPTION:" + e.Message);
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
            string[] columns = new string[] { "error", "device", "spver", "epver", "comment" };
            RemoveDuplicateRows(tableName, columns);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         return base.WriteExcelFile();
      }

      protected void WFS_INF_Extra_STATUS(string deviceType, SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSSTATUS wfsStatus)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               dataRow["device"] = deviceType;
               dataRow["spver"] = wfsStatus.SPVersion;
               dataRow["epver"] = wfsStatus.EPVersion;
               dataRow["comment"] = wfsStatus.lpszExtra;

               dTableSet.Tables["Status"].AcceptChanges();
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_Extra_STATUS Exception : " + e.Message);
         }
      }
   }
}

