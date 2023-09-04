using Contract;
using Impl;
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
            string[] columns = new string[] { "error", "device", "encstat", "autobeepmode", "certificatestate", "deviceposition", "powersaverecoverytime", "antifraudmodule"};
            CompressTable(tableName, columns);

            // ADD ENGLISH
            ctx.ConsoleWriteLogLine(String.Format("Add English to {0} Table", tableName));
            string[,] colKeyMap = new string[7, 2]
            {               
                {"device", "fwDevice"},
                {"encstat", "fwEncStat"},
                {"autobeepmode", "fwAutoBeepMode"},
                {"certificatestate", "dwCertificateState"},                
                {"deviceposition", "wDevicePosition"},
                {"powersaverecoverytime", "usPowerSaveRecoveryTime"},
                {"antifraudmodule", "wAntiFraudModule"},
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
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
            switch (result.xfsType)
            {
               case XFSType.WFS_INF_PIN_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_PIN_STATUS(result.xfsLine);
                     break;
                  }

               default:
                  break;
            };
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("PINTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void WFS_INF_PIN_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_PIN_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSPINSTATUS pinStatus = new WFSPINSTATUS(ctx);

            try
            {
               pinStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_PIN_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = _traceFile;
               dataRow["time"] = lpResult.tsTimestamp(xfsLine);
               dataRow["error"] = lpResult.hResult(xfsLine);
               dataRow["device"] = pinStatus.fwDevice;
               dataRow["encstat"] = pinStatus.fwEncStat;
               dataRow["autobeepmode"] = pinStatus.fwAutoBeepMode;
               dataRow["certificatestate"] = pinStatus.dwCertificateState;
               dataRow["deviceposition"] = pinStatus.wDevicePosition;               
               dataRow["powersaverecoverytime"] = pinStatus.usPowerSaveRecoveryTime;
               dataRow["antifraudmodule"] = pinStatus.wAntiFraudModule;

               dTableSet.Tables["Status"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_PIN_STATUS Status Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_PIN_STATUS Exception : " + e.Message);
         }
      }
   }
}
