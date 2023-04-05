using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Impl;

namespace DeviceView
{
   internal class DEVTable : BaseTable
   {
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
         //STATUS TABLE

         // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
         ctx.ConsoleWriteLogLine("Compress the Status Table: sort by time, visit every row and delete rows that are unchanged from their predecessor");
         ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table start: rows before: {0}", dTableSet.Tables["Status"].Rows.Count));

         // the list of columns to compare
         string[] columns = new string[] { "error", "PTR", "IDC", "CDM", "PIN", "CHK", "DEP", "TTU", "SIU", "VDM", "CAM", "ALM", "CIM", "BCR", "IPM" };
         (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables["Status"], "time ASC", columns);
         if (!result.success)
         {
            ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
         }
         ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table complete: rows after: {0}", dTableSet.Tables["Status"].Rows.Count));

         // add English
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

         for (int i = 0; i < 15; i++)
         {
            result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Status"], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
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
   }
}
