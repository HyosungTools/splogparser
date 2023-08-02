using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;

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

            // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table: sort by time, visit every row and delete rows that are unchanged from their predecessor", tableName));
            ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table start: rows before: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));

            // the list of columns to compare
            string[] columns = new string[] { "error", "device", "media", "retainbin", "security", "uscards", "chippower","chipmodule","magreadmodule", "errorcode"};
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
                {"media", "fwMedia"},
                {"retainbin", "fwRetainBin"},
                {"security", "fwSecurity"},                
                {"chippower", "fwChipPower"},
                {"chipmodule", "fwChipModule"},
                {"magreadmodule", "fwMagReadModule"},
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
            string[] columns = new string[] { "error", "spversion", "epversion" };
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
               case XFSType.WFS_INF_IDC_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_IDC_STATUS(result.xfsLine);
                     break;
                  }

               default:
                  break;
            };
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("IDCTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void WFS_INF_IDC_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IDC_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIDCSTATUS idcStatus = new WFSIDCSTATUS(ctx);

            try
            {
               idcStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IDC_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               DataRow dataRowSummary = dTableSet.Tables["Summary"].Rows.Add();

               dataRowSummary["file"] = _traceFile;
               dataRowSummary["time"] = lpResult.tsTimestamp(xfsLine);
               dataRowSummary["error"] = lpResult.hResult(xfsLine);
               dataRowSummary["spversion"] = idcStatus.SPVersion;
               dataRowSummary["epversion"] = idcStatus.EPVersion;
               dTableSet.Tables["Summary"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IDC_STATUS Summary Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = _traceFile;
               dataRow["time"] = lpResult.tsTimestamp(xfsLine);
               dataRow["error"] = lpResult.hResult(xfsLine);
               dataRow["device"] = idcStatus.fwDevice;
               dataRow["media"] = idcStatus.fwMedia;
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
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IDC_STATUS Status Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IDC_STATUS Exception : " + e.Message);
         }
      }
   }
}
