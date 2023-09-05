using System;
using System.Data;
using Contract;
using Impl;

namespace DeviceView
{
   internal class DEVTable : BaseTable
   {
      string[] hServiceArray = new string[100];

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
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);

            switch (result.xfsType)
            {
               case XFSType.WFS_INF_PTR_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("PTR", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_IDC_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("IDC", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CDM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("CDM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_PIN_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("PIN", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CHK_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("CHK", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_DEP_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("DEP", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_TTU_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("TTU", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_SIU_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("SIU", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_VDM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("VDM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CAM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("CAM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_ALM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("ALM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CIM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("CIM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_BCR_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("BCR", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_IPM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_STATUS("IPM", result.xfsLine);
                     break;
                  }
               case XFSType.WFPOPEN:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFSOPEN(LogTime.GetTimeFromLogLine2(logLine), result.xfsLine);
                     break;
                  }
               case XFSType.WFPCLOSE:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFSCLOSE(LogTime.GetTimeFromLogLine2(logLine), result.xfsLine);
                     break;
                  }
               case XFSType.WFS_SYSEVENT:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFSSYSEVENT(LogTime.GetTimeFromLogLine2(logLine), result.xfsLine);
                     break;
                  }
               default:
                  break;
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

      protected void WFS_INF_STATUS(string device, string xfsLine)
      {
         try
         {
            //ctx.ConsoleWriteLogLine(String.Format("WFS_INF_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSDEVSTATUS devStatus = new WFSDEVSTATUS(ctx);

            try
            {
               devStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            dataRow[device] = devStatus.fwDevice;

            dTableSet.Tables["Status"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_STATUS Exception : " + e.Message);
         }

         return;
      }

      protected void WFSOPEN(string logTime, string xfsLine)
      {
         try
         {
            // ctx.ConsoleWriteLogLine(String.Format("WFPOPEN tracefile '{0}' xfsLine '{1}'", _traceFile, xfsLine.Substring(0, 20)));

            WFPOPEN wfpOpen = new WFPOPEN(ctx);

            try
            {
               wfpOpen.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFSOPEN Assignment Exception {0}. {1}, {2}", _traceFile, logTime, e.Message));
            }

            string xfsDevice = WFPOPEN.device(xfsLine);
            ctx.ConsoleWriteLogLine(String.Format("xfsDevice = {0}", xfsDevice));
            if (!String.IsNullOrEmpty(xfsDevice))
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = _traceFile;
               dataRow["time"] = logTime;
               dataRow["error"] = wfpOpen.lpszAppID;

               dataRow[xfsDevice] = "open (" + wfpOpen.hService + ")";

               // store xfs device
               hServiceArray[int.Parse(wfpOpen.hService)] = xfsDevice;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFPOPEN Exception : " + e.Message);
         }

         return;
      }
      protected void WFSCLOSE(string logTime, string xfsLine)
      {
         try
         {
            // ctx.ConsoleWriteLogLine(String.Format("WFPOPEN tracefile '{0}' xfsLine '{1}'", _traceFile, xfsLine.Substring(0, 20)));

            WFPCLOSE wfpClose = new WFPCLOSE(ctx);

            try
            {
               wfpClose.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFSCLOSE Assignment Exception {0}. {1}, {2}", _traceFile, logTime, e.Message));
            }

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            // recover the xfs device
            string xfsDevice = hServiceArray[int.Parse(wfpClose.hService)];

            dataRow["file"] = _traceFile;
            dataRow["time"] = logTime;
            dataRow[xfsDevice] = "close (" + wfpClose.hService + ")";

            dTableSet.Tables["Status"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFSCLOSE Exception : " + e.Message);
         }
      }

      protected void WFSSYSEVENT(string logTime, string xfsLine)
      {
         try
         {
            // ctx.ConsoleWriteLogLine(String.Format("WFSSYSEVENT tracefile '{0}' xfsLine '{1}'", _traceFile, xfsLine.Substring(0, 20)));

            WFSSYSEVENT wfsSysEvent = new WFSSYSEVENT(ctx);

            try
            {
               wfsSysEvent.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFSSYSEVENT Assignment Exception {0}. {1}, {2}", _traceFile, logTime, e.Message));
            }

            DataRow dataRow = dTableSet.Tables["SysEvent"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            dataRow["logical"] = wfsSysEvent.logicalName;
            dataRow["physical"] = wfsSysEvent.physicalName;
            dataRow["description"] = wfsSysEvent.lpbDescription;

            dTableSet.Tables["SysEvent"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFSSYSEVENT Exception : " + e.Message);
         }
      }
   }
}
