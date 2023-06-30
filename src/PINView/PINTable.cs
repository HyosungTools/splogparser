using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;

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

            // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table: sort by time, visit every row and delete rows that are unchanged from their predecessor", tableName));
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table start: rows before: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));

            // the list of columns to compare
            string[] columns = new string[] { "error", "device", "encstat", "autobeepmode", "certificatestate", "deviceposition", "powersaverecoverytime", "antifraudmodule"};
            (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables[tableName], "time ASC", columns);
            if (!result.success)
            {
               ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
            }
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table complete: rows after: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));

            // add English to the Status Table
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

            for (int i = 0; i < 7; i++)
            {
               result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables[tableName], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception processing the {0} table - {1}", tableName, e.Message));           
         }

         try
         {
            // S U M M A R Y  T A B L E

            tableName = "Summary";

            // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table: sort by time, visit every row and delete rows that are unchanged from their predecessor", tableName));
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table start: rows before: {1}", tableName, dTableSet.Tables["Summary"].Rows.Count));

            // the list of columns to compare
            string[] columns = new string[] { "error", "sp_version", "ep_version" };
            (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables[tableName], "time ASC", columns);
            if (!result.success)
            {
               ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
            }
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table complete: rows after: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         return base.WriteExcelFile();
      }

      protected (bool success, DataRow dataRow) FindMessages(string type, string code)
      {
         // Create an array for the key values to find.
         object[] findByKeys = new object[2];

         // Set the values of the keys to find.
         findByKeys[0] = type;
         findByKeys[1] = code;

         DataRow foundRow = dTableSet.Tables["Messages"].Rows.Find(findByKeys);
         if (foundRow != null)
         {
            return (true, foundRow);
         }
         else
         {
            return (false, null);
         }
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
               DataRow dataRowSummary = dTableSet.Tables["Summary"].Rows.Add();

               dataRowSummary["file"] = _traceFile;
               dataRowSummary["time"] = lpResult.tsTimestamp(xfsLine);
               dataRowSummary["error"] = lpResult.hResult(xfsLine);
               dataRowSummary["sp_version"] = pinStatus.SP_Version;
               dataRowSummary["ep_version"] = pinStatus.EP_Version;
               dTableSet.Tables["Summary"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_PIN_STATUS Summary Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
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
