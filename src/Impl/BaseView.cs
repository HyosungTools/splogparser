using System;
using Contract;

namespace Impl
{
   public abstract class BaseView
   {
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
      public string ViewType { get { return viewType; } }

      /// <summary>(get) name of View for display/logging purposes.</summary>
      public string Name { get { return viewName; } }


      /// <summary>
      /// Abstract - Every View must implement CreateTableInstance
      /// </summary>
      /// <param name="logWriter">The singleton instance of LogWriter.</param>
      /// <param name="logInfo">The singleton instance of LogInfo, which holds the session context.</param>
      /// <returns></returns>
      protected abstract BaseTable CreateTableInstance(IContext ctx);

      /// <summary>Call to process the datatable (merge of all log lines)</summary>
      /// <returns>void</returns>
      public virtual void Process(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Start: " + viewName);

         // create a table instance
         BaseTable bTable = CreateTableInstance(ctx);

         // for each file found
         foreach (string traceFile in ctx.nwlogFiles)
         {
            if (traceFile != null)
            {
               ctx.ConsoleWriteLogLine("Processing file: " + traceFile);
               TraceFileReader reader = new TraceFileReader(ctx.ioProvider.OpenTextFile(traceFile), 28);
               {
                  // for each log line in the file
                  while (!reader.EOF())
                  {
                     try
                     {
                        // pass the log line to the View for (possible) processing
                        bTable.ProcessRow(ctx.ioProvider.GetFileName(traceFile), reader.ReadLine());
                     }
                     catch (Exception ex)
                     {
                        ctx.ConsoleWriteLogLine("EXCEPTION : Processing view :" + ex.Message);
                     }
                  }
               }
            }
         }

         // write the table built out to file
         bTable.WriteXmlFile();
      }

      public virtual void WriteExcel(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("------------------------------------------------");
         ctx.ConsoleWriteLogLine("Start WriteExcel: " + viewName);

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
         ctx.ConsoleWriteLogLine("Start Cleanup: " + viewName);

         // Cleanup to Xml
         BaseTable bTable = CreateTableInstance(ctx);
         bTable.CleanupXMLFile();

         // update logInfo and return
         ctx.ConsoleWriteLogLine("End Cleanup: " + viewName);
      }
   }
}



