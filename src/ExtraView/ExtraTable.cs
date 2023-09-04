using Contract;
using Impl;
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
                     WFS_INF_Extra_STATUS("PTR", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_IDC_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("IDC", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CDM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("CDM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_PIN_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("PIN", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CHK_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("CHK", result.xfsLine);
                     break;
                  }
               //case XFSType.WF_INF_DEP_STATUS:
               //   {
               //      base.ProcessRow(traceFile, logLine);
               //      WF_INF_DEP_STATUS(result.xfsLine);
               //      break;
               //   }
               case XFSType.WFS_INF_TTU_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("TTU", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_SIU_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("SIU", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_VDM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("VDM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CAM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("CAM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_ALM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("ALM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CIM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("CIM", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_BCR_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("BCR", result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_IPM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_Extra_STATUS("IPM", result.xfsLine);
                     break;
                  }

               default:
                  break;
            };
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
            string[] columns = new string[] { "error", "device", "spver", "epver" };
            RemoveDuplicateRows(tableName, columns);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         return base.WriteExcelFile();
      }

      protected void WFS_INF_Extra_STATUS(string deviceType, string xfsLine)
      {
         try
         {
            // ctx.ConsoleWriteLogLine(String.Format("WFS_INF_Extra_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));


            WFSSTATUS wfsStatus = new WFSSTATUS(ctx);
            wfsStatus.Initialize(xfsLine);

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            dataRow["device"] = deviceType;
            dataRow["spver"] = wfsStatus.SPVersion;
            dataRow["epver"] = wfsStatus.EPVersion;
            dataRow["comment"] = wfsStatus.lpszExtra;

            dTableSet.Tables["Status"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_Extra_STATUS Exception : " + e.Message);
         }
      }
   }
}

