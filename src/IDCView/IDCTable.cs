using Contract;
using Impl;
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

         try
         {
            // S U M M A R Y  T A B L E

            tableName = "Summary";

            // COMPRESS
            string[] columns = new string[] { "error", "spversion", "epversion" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            AddEnglishToTable(tableName, null);
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
