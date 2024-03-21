using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Contract;

namespace Impl
{
   public abstract class BaseView
   {
      IContext ctx;

      // static constants
      /// <summary>
      /// Type for this View. Views can be grouped together by type. 
      /// </summary>
      public ParseType parseType { get; }

      /// <summary>
      /// Unique name for this View. 
      /// </summary>
      protected readonly string viewName;

      /// <summary>
      /// My base table(s)
      /// </summary>
      protected BaseTable bTable;

      /// <summary>
      /// Constructor. 
      /// </summary>
      /// <param name="parseType">View type used to group views. Not unique. </param>
      /// <param name="viewName">View name. Must be unique.</param>
      protected BaseView(ParseType parseType, string viewName)
      {
         this.parseType = parseType;
         this.viewName = viewName;
      }

      /// <summary>(get) name of View for display/logging purposes.</summary>
      public virtual string Name { get { return viewName; } }

      /// <summary>
      /// Abstract - Every View must implement CreateTableInstance
      /// </summary>
      /// <param name="ctx">The current context</param>
      /// <returns>BaseTable</returns>
      protected abstract BaseTable CreateTableInstance(IContext ctx);

      /// <summary>
      /// Initialize this View
      /// </summary>
      /// <param name="ctx"></param>
      public virtual void Initialize(IContext ctx)
      {
         this.ctx = ctx;

         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Initialize: " + viewName);

         // create a table instance
         bTable = CreateTableInstance(ctx);
         ctx.LogWriteLine("Table created: " + viewName);
      }

      public virtual void PreProcess(IContext ctx)
      {
         return;
      }

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      public virtual void Process(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Process: " + viewName);

         // For each file found by this log handler...
         foreach (string fileName in ctx.activeHandler.FilesFound)
         {
            if (string.IsNullOrEmpty(fileName))
            {
               continue;
            }

            ctx.ConsoleWriteLogLine(String.Format("Processing file: {0}", fileName));

            if (fileName.EndsWith(".zip"))
            {
               ctx.ConsoleWriteLogLine(".zip file skipped");
               continue;
            }

            ctx.activeHandler.OpenLogFile(fileName);

            ctx.activeHandler.LineNumber = 1;

            long rowsProcessed = 0;
            long rowsTimeAccepted = 0;
            DateTime firstTimestamp = DateTime.MinValue;
            DateTime lastTimestamp = DateTime.MinValue;

            // For each log line in this file...
            while (!ctx.activeHandler.EOF())
            {
               try
               {
                  ILogLine logLine = ctx.activeHandler.IdentifyLine(ctx.activeHandler.ReadLine());

                  if (fileName.Contains("Workstation20231103"))
                  {
                     // debugging breakpoint
                  }

                  // TIME RANGE
                  if (logLine.IsValidTimestamp)
                  {
                     DateTime logTimestamp = DateTime.Parse(logLine.Timestamp);

                     lastTimestamp = logTimestamp;
                     firstTimestamp = (firstTimestamp == DateTime.MinValue) ? lastTimestamp : firstTimestamp;

                     if (DateTime.Compare(logTimestamp, ctx.opts.StartTime) < 0 || DateTime.Compare(logTimestamp, ctx.opts.EndTime) > 0 )
                     {
                        continue;
                     }

                     rowsTimeAccepted++;
                  }

                  if (!logLine.IgnoreThisLine)
                  {
                     // store the result in the output XML

                     bTable.ProcessRow(logLine);
                     rowsProcessed++;
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Processing file {0} : {1} {2}", fileName, e.Message, e));
               }
               finally
               {
                  ctx.activeHandler.LineNumber += 1;
               }
            }

            ctx.ConsoleWriteLogLine($"{ctx.activeHandler.LineNumber - 1} lines {firstTimestamp} to {lastTimestamp} ({rowsTimeAccepted} in time range, {rowsProcessed} processed) ");
         }
      }

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      /// <remarks>The view's tables are added to the combined dataset</remarks>
      public virtual void PostProcess(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Post Process: " + viewName);

         bTable.PostProcess();
         bTable.WriteXmlFile();
      }

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      public virtual void PreAnalyze(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Pre Analyze: " + viewName);

         return;
      }

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      public virtual void Analyze(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Analyze: " + viewName);

         return;
      }

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      public virtual void PostAnalyze(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Post Analyze: " + viewName);

         return;
      }

      public virtual void WriteExcel(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("------------------------------------------------");
         ctx.ConsoleWriteLogLine("WriteExcel: " + viewName);

         // create a Table Instance and load up the xml file
         BaseTable bTable = CreateTableInstance(ctx);

         // write to Excel
         bTable.WriteExcelFile();

         // update logInfo and return
         ctx.ConsoleWriteLogLine("End WriteExcel: " + viewName);

      }

      public virtual void Cleanup(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("------------------------------------------------");
         ctx.ConsoleWriteLogLine("Cleanup: " + viewName);

         // Cleanup to Xml
         BaseTable bTable = CreateTableInstance(ctx);
         bTable.CleanupXMLFile();

         // update logInfo and return
         ctx.ConsoleWriteLogLine("End Cleanup: " + viewName);
      }
   }
}



