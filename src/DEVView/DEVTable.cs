using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace DeviceView
{
   internal class DEVTable : BaseTable
   {
      readonly string[] hServiceArray = new string[100];

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public DEVTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
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
                        WFS_INF_STATUS("PTR", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_IDC_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("IDC", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CDM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("CDM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_PIN_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("PIN", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CHK_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("CHK", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_DEP_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("DEP", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_TTU_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("TTU", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_SIU_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("SIU", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_VDM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("VDM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CAM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("CAM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_ALM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("ALM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CIM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("CIM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_BCR_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("BCR", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_IPM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_STATUS("IPM", spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFPOPEN:
                     {
                        base.ProcessRow(spLogLine);
                        WFSOPEN(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFPCLOSE:
                     {
                        base.ProcessRow(spLogLine);
                        WFSCLOSE(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SYSEVENT:
                     {
                        base.ProcessRow(spLogLine);
                        WFSSYSEVENT(spLogLine);
                        break;
                     }
                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("DEVTable.ProcessRow EXCEPTION:" + e.Message);
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
            //STATUS TABLE

            tableName = "Status";

            // COMPRESS
            string[] columns = new string[] { "error", "PTR", "IDC", "CDM", "PIN", "CHK", "DEP", "TTU", "SIU", "VDM", "CAM", "ALM", "CIM", "BCR", "IPM" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            string[,] colKeyMap = new string[15, 2]
            {
               {"PTR", "fwDevice" },
               {"IDC", "fwDevice" },
               {"CDM", "fwDevice" },
               {"PIN", "fwDevice" },
               {"CHK", "fwDevice" },
               {"DEP", "fwDevice" },
               {"TTU", "fwDevice" },
               {"SIU", "fwDevice" },
               {"VDM", "fwDevice" },
               {"CAM", "fwDevice" },
               {"ALM", "fwDevice" },
               {"CAM", "fwDevice" },
               {"CIM", "fwDevice" },
               {"BCR", "fwDevice" },
               {"IPM", "fwDevice" }
            };
            AddEnglishToTable(tableName, colKeyMap);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // S Y S E V E N T

            tableName = "SysEvent";

            // COMPRESS
            string[] columns = new string[] { "error", "logical", "physical", "description" };
            CompressTable(tableName, columns);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         return base.WriteExcelFile();
      }

      protected void WFS_INF_STATUS(string device, SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSDEVSTATUS devStatus)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               dataRow[device] = devStatus.fwDevice;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_STATUS Exception : " + e.Message);
         }

         return;
      }

      protected void WFSOPEN(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFPOPEN wfpOpen)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = wfpOpen.Timestamp;
               dataRow["error"] = wfpOpen.lpszAppID;

               //dataRow[xfsDevice] = "open (" + wfpOpen.hService + ")";

               //// store xfs device
               //hServiceArray[int.Parse(wfpOpen.hService)] = xfsDevice;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFPOPEN Exception : " + e.Message);
         }

         return;
      }
      protected void WFSCLOSE(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFPCLOSE wfpOpen)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               // recover the xfs device
               // string xfsDevice = hServiceArray[int.Parse(wfpClose.hService)];

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               // dataRow[xfsDevice] = "close (" + wfpClose.hService + ")";

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFSCLOSE Exception : " + e.Message);
         }
      }

      protected void WFSSYSEVENT(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSSYSEVENT wfsSysEvent)
            {
               DataRow dataRow = dTableSet.Tables["SysEvent"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               dataRow["logical"] = wfsSysEvent.logicalName;
               dataRow["physical"] = wfsSysEvent.physicalName;
               dataRow["description"] = wfsSysEvent.lpbDescription;

               dTableSet.Tables["SysEvent"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFSSYSEVENT Exception : " + e.Message);
         }
      }
   }
}
