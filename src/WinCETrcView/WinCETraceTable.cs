using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace WinCETraceView
{
   /// <summary>
   /// Table for WinCE trace log data.
   /// Stores parsed trace entries for Excel output.
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
      /// Add a trace line to the Summary table
      /// </summary>
      /// <param name="traceLine">Parsed trace line</param>
      protected void AddTraceRow(WinCETraceLine traceLine)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = traceLine.LogFile;
            dataRow["time"] = traceLine.Timestamp;
            dataRow["type"] = traceLine.LogType.ToString() + " - " + traceLine.traceType.ToString();
            dataRow["code"] = traceLine.Code;
            dataRow["message"] = traceLine.Message;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WinCETraceTable.AddTraceRow Exception: " + e.Message);
         }
      }
   }
}
