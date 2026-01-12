using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace WinCETraceView
{
   /// <summary>
   /// Table class for WinCE trace log data.
   /// Stores parsed trace entries for Excel output, matching TraceViewer column layout.
   /// </summary>
   class WinCETraceTable : BaseTable
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public WinCETraceTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process one line from the trace log file.
      /// </summary>
      /// <param name="logLine">Parsed log line</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is WinCETraceLine traceLine)
            {
               base.ProcessRow(logLine);
               AddTraceRow(traceLine);
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("WinCETraceTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Add a trace line to the Summary table.
      /// Columns match TraceViewer layout: Date/Time, UNIT, Func, Error, Data, Module Name, Class Name, Func. Name
      /// </summary>
      /// <param name="traceLine">Parsed trace line</param>
      protected void AddTraceRow(WinCETraceLine traceLine)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].NewRow();

            dataRow["file"] = traceLine.LogFile;
            dataRow["time"] = traceLine.Timestamp;                    // Now includes seconds: "2025-04-09 09:26:32.000"
            dataRow["type"] = traceLine.LogType.ToString();           // 'o', 'O', 'f', 'F', 's', 't'
            dataRow["unit"] = traceLine.Unit;                         // First 2 chars of code: "1E", "54", "1A", etc.
            dataRow["func"] = traceLine.Func;                         // Chars 2-3 of code: "12", "30", "01", etc.
            dataRow["error"] = traceLine.Error;                       // Chars 4-10 of code: "0000000", "9999999", etc.
            dataRow["data"] = traceLine.Message;                      // The trace message text

            // Lookup columns from Messages table using Unit+Func as the key
            string lookupKey = traceLine.Unit + traceLine.Func;       // e.g., "1E12", "1A01", "5430"
            dataRow["modulename"] = LookupMessage("modulename", lookupKey);
            dataRow["classname"] = LookupMessage("classname", lookupKey);
            dataRow["funcname"] = LookupMessage("funcname", lookupKey);

            dTableSet.Tables["Summary"].Rows.Add(dataRow);
            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WinCETraceTable.AddTraceRow Exception: " + e.Message);
         }
      }

      /// <summary>
      /// Generic lookup in Messages table by type and code.
      /// The Messages table has columns: type, code, description
      /// - type: "modulename", "classname", or "funcname"
      /// - code: Unit+Func concatenated (e.g., "1E12", "1A01")
      /// - description: The human-readable value
      /// </summary>
      /// <param name="messageType">Type of lookup: "modulename", "classname", or "funcname"</param>
      /// <param name="code">The Unit+Func code to look up (e.g., "1E12")</param>
      /// <returns>The description from the Messages table, or empty string if not found</returns>
      private string LookupMessage(string messageType, string code)
      {
         if (string.IsNullOrEmpty(code))
            return string.Empty;

         try
         {
            if (dTableSet.Tables.Contains("Messages"))
            {
               DataTable messagesTable = dTableSet.Tables["Messages"];
               foreach (DataRow row in messagesTable.Rows)
               {
                  string rowType = row["type"]?.ToString();
                  string rowCode = row["code"]?.ToString();
                  if (rowType == messageType && rowCode == code)
                  {
                     return row["description"]?.ToString() ?? string.Empty;
                  }
               }
            }
         }
         catch
         {
            // Return empty on lookup failure
         }

         return string.Empty;
      }
   }
}
