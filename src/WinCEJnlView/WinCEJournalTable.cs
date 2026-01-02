using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace WinCEJournalView
{
   /// <summary>
   /// Table for WinCE electronic journal data.
   /// Stores parsed journal entries with KindCode to description lookup.
   /// </summary>
   class WinCEJournalTable : BaseTable
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public WinCEJournalTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process one line from the journal file.
      /// </summary>
      /// <param name="logLine">Parsed log line</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is WinCEJournalLine journalLine)
            {
               base.ProcessRow(logLine);
               AddJournalRow(journalLine);
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("WinCEJournalTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Add a journal line to the Summary table
      /// </summary>
      /// <param name="journalLine">Parsed journal line</param>
      protected void AddJournalRow(WinCEJournalLine journalLine)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = journalLine.LogFile;
            dataRow["time"] = journalLine.Timestamp;
            dataRow["sequence"] = journalLine.Sequence;
            dataRow["kindcode"] = journalLine.KindCode;
            dataRow["description"] = LookupKindCodeDescription(journalLine.KindCode);
            dataRow["data"] = journalLine.Data;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WinCEJournalTable.AddJournalRow Exception: " + e.Message);
         }
      }

      /// <summary>
      /// Look up the human-readable description for a KindCode from the Messages table.
      /// </summary>
      /// <param name="kindCode">2-character kind code (e.g., "SF", "OA")</param>
      /// <returns>Description string or "Unknown" if not found</returns>
      protected string LookupKindCodeDescription(string kindCode)
      {
         try
         {
            if (dTableSet.Tables.Contains("Messages"))
            {
               DataTable messages = dTableSet.Tables["Messages"];
               
               // Search for matching KindCode in Messages table
               foreach (DataRow row in messages.Rows)
               {
                  string type = row["type"]?.ToString() ?? string.Empty;
                  string code = row["code"]?.ToString() ?? string.Empty;

                  if (type == "KindCode" && code == kindCode)
                  {
                     return row["brief"]?.ToString() ?? "Unknown";
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WinCEJournalTable.LookupKindCodeDescription Exception: " + e.Message);
         }

         return "Unknown";
      }
   }
}
