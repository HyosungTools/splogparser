using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace MVErrorsView
{
   /// <summary>
   /// One row per WARN/ERROR line from the MoniView server logs. Non-INFO lines only.
   /// Each row carries the raw level/category, the machine (IP / terminal) when it can be
   /// resolved from the line, the classified Kind, the plain-English Comment, and the raw
   /// continuation tail (SQL text / stack trace) in Detail for drill-down.
   /// </summary>
   class MVErrorsTable : BaseTable
   {
      private const string TABLE_NAME = "Errors";

      public MVErrorsTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process one line from the MoniView server log. Only WARN and ERROR lines produce a row.
      /// </summary>
      /// <param name="logLine">the parsed log line</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is MVServerLine mvLine)
            {
               if (mvLine.Level == "WARN" || mvLine.Level == "ERROR")
               {
                  base.ProcessRow(mvLine);
                  AddErrorRow(mvLine);
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("MVErrorsTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void AddErrorRow(MVServerLine mvLine)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables[TABLE_NAME].Rows.Add();

            dataRow["file"] = mvLine.LogFile;
            dataRow["time"] = mvLine.Timestamp;
            dataRow["level"] = mvLine.Level;
            dataRow["category"] = mvLine.Category;
            dataRow["ip"] = mvLine.IP;
            dataRow["terminal"] = mvLine.TerminalId;
            dataRow["kind"] = mvLine.Kind;
            dataRow["comment"] = mvLine.Comment;
            dataRow["detail"] = Truncate(mvLine.Detail, SENSIBLE_EXCEL_CELL_DATA_LENGTH);

            dTableSet.Tables[TABLE_NAME].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("MVErrorsTable.AddErrorRow Exception : " + e.Message);
         }
      }

      private string Truncate(string value, int max)
      {
         if (string.IsNullOrEmpty(value))
         {
            return "";
         }

         if (value.Length <= max)
         {
            return value;
         }

         return value.Substring(0, max);
      }
   }
}
