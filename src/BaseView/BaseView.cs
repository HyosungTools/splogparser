using System;
using Contract;
using LogFileHandler;

namespace Impl
{
   public abstract class BaseView
   {
      IContext ctx; 

      // static constants
      /// <summary>
      /// Type for this View. Views can be grouped together by type. 
      /// </summary>
      protected readonly string viewType;

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
      /// <param name="viewType">View type used to group views. Not unique. </param>
      /// <param name="viewName">View name. Must be unique.</param>
      protected BaseView(string viewType, string viewName)
      {
         this.viewType = viewType;
         this.viewName = viewName;
      }

      /// <summary>(get) Type of View for display/logging purposes.</summary>
      public virtual string ViewType { get { return viewType; } }

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
      }

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      public virtual void Process(ILogFileHandler logFileHandler)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Process: " + viewName);

         // For each file found by this log handler...
         foreach (string fileName in logFileHandler.FilesFound)
         {
            if (string.IsNullOrEmpty(fileName))
            {
               continue;
            }

            ctx.ConsoleWriteLogLine(String.Format("Processing file: {0}", fileName));
            logFileHandler.OpenLogFile(fileName);

            // For each log line in this file...
            while (!logFileHandler.EOF())
            {
               try
               {
                  ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
                  bTable.ProcessRow(logLine);
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Processing file {0} : {1}", fileName, e.Message));
                  return;
               }
            }
         }
      }

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      public virtual void PostProcess(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Post Process: " + viewName);

         bTable.WriteXmlFile();
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



