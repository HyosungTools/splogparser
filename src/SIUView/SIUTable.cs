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
         try
         {
            // S T A T U S   T A B L E

            // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
            ctx.ConsoleWriteLogLine("Compress the Status Table: sort by time, visit every row and delete rows that are unchanged from their predecessor");
            ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table start: rows before: {0}", dTableSet.Tables["Status"].Rows.Count));

            // the list of columns to compare
            string[] columns = new string[] { "error", "safe" };
            (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables["Status"], "time ASC", columns);
            if (!result.success)
            {
               ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
            }
            ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table complete: rows after: {0}", dTableSet.Tables["Status"].Rows.Count));

            // add English to the Status Table
            string[,] colKeyMap = new string[1, 2]
            {
               {"safe", "fwSafeDoor" }
            };

            for (int i = 0; i < 1; i++)
            {
               result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Status"], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception : " + e.Message);
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

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            dataRow["safe"] = siuStatus.fwSafeDoor;

            dTableSet.Tables["Status"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_SIU_STATUS Exception : " + e.Message);
         }
      }
   }
}
