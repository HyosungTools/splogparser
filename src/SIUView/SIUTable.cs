using Contract;
using Impl;
using System;
using System.Collections.Generic;
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
            string[] columns = new string[] { "error", "safe", "device", "opswitch", "tamper", "inttamper", "cabinet", "errorcode", "description" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            ctx.ConsoleWriteLogLine(String.Format("Add English to {0} Table", tableName));
            string[,] colKeyMap = new string[6, 2]
            {
               {"safe", "fwSafeDoor" },
               {"device", "fwDevice"},
               {"opswitch", "opSwitch" },
               {"tamper", "tamper"},
               {"inttamper", "intTamper"},
               {"cabinet", "cabinet"}
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
            string[] columns = new string[] { "error", "sp_version", "ep_version" };
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
               case XFSType.WFS_INF_SIU_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_SIU_STATUS(result.xfsLine);
                     break;
                  }

               default:
                  break;
            };
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("SIUTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void WFS_INF_SIU_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_SIU_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSSIUSTATUS siuStatus = new WFSSIUSTATUS(ctx);

            try
            {
               siuStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_SIU_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               DataRow dataRowSummary = dTableSet.Tables["Summary"].Rows.Add();

               dataRowSummary["file"] = _traceFile;
               dataRowSummary["time"] = lpResult.tsTimestamp(xfsLine);
               dataRowSummary["error"] = lpResult.hResult(xfsLine);
               dataRowSummary["sp_version"] = siuStatus.SP_Version;
               dataRowSummary["ep_version"] = siuStatus.EP_Version;

               dTableSet.Tables["Summary"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_SIU_STATUS Summary Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            try
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = _traceFile;
               dataRow["time"] = lpResult.tsTimestamp(xfsLine);
               dataRow["error"] = lpResult.hResult(xfsLine);
               dataRow["safe"] = siuStatus.fwSafeDoor;
               dataRow["device"] = siuStatus.fwDevice;
               dataRow["opswitch"] = siuStatus.opSwitch;
               dataRow["tamper"] = siuStatus.tamper;
               dataRow["inttamper"] = siuStatus.intTamper;
               dataRow["cabinet"] = siuStatus.cabinet;
               dataRow["description"] = siuStatus.description;

               dTableSet.Tables["Status"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_SIU_STATUS Status Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_SIU_STATUS Exception : " + e.Message);
         }
      }
   }
}
