using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Contract;
using Impl;
using LogFileHandler;
using CommandLine;
using System.Collections.Generic;

namespace splogparser
{
   class Program
   {
      private static bool _ExtractZipFiles(IContext ctx, string currentFolder)
      {
         string[] zipFiles = ctx.ioProvider.GetFiles(currentFolder, "*.zip");
         foreach (string zipFile in zipFiles)
         {
            string newFolderName = ctx.ioProvider.GetFileNameWithoutExtension(zipFile);

            ctx.ConsoleWriteLogLine(String.Format("Create directory : {0}", currentFolder + "\\" + newFolderName));

            if (!ctx.ioProvider.CreateDirectory(currentFolder + "\\" + newFolderName))
            {
               ctx.ConsoleWriteLogLine("Failed to create directory:" + zipFile);
               return false;
            }

            // Extract current zip file
            ctx.ioProvider.ExtractToDirectory(zipFile, currentFolder + "\\" + newFolderName);
         }

         foreach (string directory in Directory.GetDirectories(currentFolder))
         {
            _ExtractZipFiles(ctx, directory);
         }

         return true;
      }

      public static bool ExtractZipFiles(IContext ctx)
      {
         // Extract current zip file
         ctx.ioProvider.ExtractToDirectory(ctx.WorkFolder + "\\" + ctx.ZipFileName, ctx.WorkFolder + "\\" + ctx.SubFolder);
         return _ExtractZipFiles(ctx, ctx.WorkFolder + "\\" + ctx.SubFolder);
      }

      static void Main(string[] args)
      {
         Parser.Default.ParseArguments<Options>(args)
             .WithParsed(Run)
             .WithNotParsed(HandleParseError);
      }

      private static void HandleParseError(IEnumerable<Error> errs)
      {
         if (errs.IsVersion())
         {
            Console.WriteLine("Version Request");
            return;
         }

         if (errs.IsHelp())
         {
            Console.WriteLine("Help Request");
            return;
         }
         Console.WriteLine("Parser Fail");
      }

      private static void Run(Options opts)
      {
         // Define the FileSystemProvider for this run
         IFileSystemProvider ioProvider = new FileSystemProvider();

         // Use the FileSystemProvider to gather information
         string zipFileName = opts.InputFile;
         string workFolder = ioProvider.GetCurrentDirectory();
         string subFolder = ioProvider.GetFileNameWithoutExtension(zipFileName);

         //Define the Logger
         ILogger logger = new Logger(ioProvider, workFolder, subFolder);

         // Define the Context. 
         IContext ctx = new Context(ioProvider, logger, workFolder, subFolder, zipFileName, opts);

         // Write out settings so far...
         ctx.ConsoleWriteLogLine("Application Start");
         ctx.ConsoleWriteLogLine("ZipFileName: " + $"{zipFileName}");
         ctx.ConsoleWriteLogLine("Work Folder: " + ctx.WorkFolder);
         ctx.ConsoleWriteLogLine("unzip to   : " + ctx.WorkFolder + "\\" + ctx.SubFolder);

         ctx.ConsoleWriteLogLine("opts.APViews :" + ctx.opts.APViews);
         ctx.ConsoleWriteLogLine("opts.ATViews :" + ctx.opts.ATViews);
         ctx.ConsoleWriteLogLine("opts.AWViews :" + ctx.opts.AWViews);
         ctx.ConsoleWriteLogLine("opts.SPViews :" + ctx.opts.SPViews);
         ctx.ConsoleWriteLogLine("opts.RTViews :" + ctx.opts.RTViews);

         ctx.ConsoleWriteLogLine(String.Format("IsAP : {0}", ctx.opts.IsAP ? "true" : "false"));
         ctx.ConsoleWriteLogLine(String.Format("APView Contains  : {0}", ctx.opts.APViews));

         ctx.ConsoleWriteLogLine(String.Format("IsSP : {0}", ctx.opts.IsSP ? "true" : "false"));
         ctx.ConsoleWriteLogLine(String.Format("SPView Contains  : {0}", ctx.opts.SPViews));

         // Only create a LogFileHandler if their ParseType was specified on the command line
         ctx.ConsoleWriteLogLine(String.Format("Create the LogFileHandlers"));

         // AP 
         if (ctx.opts.IsAP) ctx.logFileHandlers.Add((ILogFileHandler)new APLogHandler(new CreateTextStreamReader()));

         // AT 
         // if (ctx.IsAT) ctx.logFileHandlers.Add((ILogFileHandler)new ATLogHandler(new CreateTextStreamReader()));

         // AW
         // if (ctx.IsAW) ctx.logFileHandlers.Add((ILogFileHandler)new AWLogHandler(new CreateTextStreamReader()));

         // SP
         if (ctx.opts.IsSP) ctx.logFileHandlers.Add((ILogFileHandler)new SPLogHandler(new CreateTextStreamReader()));

         // RT
         // if (ctx.IsRT) ctx.logFileHandlers.Add((ILogFileHandler)new RTLogHandler(new CreateTextStreamReader()));

         // SS
         if (ctx.opts.IsSS) ctx.logFileHandlers.Add((ILogFileHandler)new SSLogHandler(new CreateTextStreamReader()));


         // if the unzip folder already exists delete it
         if (ctx.ioProvider.DirExists(ctx.WorkFolder + "\\" + ctx.SubFolder))
         {
            Console.WriteLine("Folder already exists. Deleting the folder.");
            ctx.ioProvider.DeleteDir(ctx.WorkFolder + "\\" + ctx.SubFolder, true);
         }

         // create it
         if (!ctx.ioProvider.CreateDirectory(ctx.WorkFolder + "\\" + ctx.SubFolder))
         {
            ctx.ConsoleWriteLogLine("Failed to create directory:" + ctx.SubFolder);
            return;
         }

         // Unzip the zip file
         ctx.ConsoleWriteLogLine("Unzipping the archive...");
         if (!ExtractZipFiles(ctx))
         {
            ctx.ConsoleWriteLogLine("Failed to extract files.");
            return;
         }

         //set CurrentDirectory to the base directory that the assembly resolver uses to probe for assemblies.
         if (!ctx.ioProvider.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory))
         {
            ctx.ConsoleWriteLogLine("Failed to set current directory to the application directory");
            return;
         }

         ctx.ConsoleWriteLogLine("Application folder: " + ctx.ioProvider.GetCurrentDirectory());

         // Create a Stopwatch instance
         Stopwatch stopwatch = new Stopwatch();

         // Start the stopwatch
         stopwatch.Start();

         // I N I T I A L I Z E  V I E W S

         // Load the Views in the application directory
         ViewLoader loader = new ViewLoader(ctx.ioProvider.GetCurrentDirectory());
         string viewName = string.Empty;

         using (loader.Container)
         {
            try
            {
               // Import and use the plugins (they will be instantiated only when accessed)
               foreach (IView thisView in loader.Views)
               {
                  // Only Initialize a View if their ParseType was specified in the command line and their name is mentioned in the arguments
                  // (or the argument was '*')

                  viewName = thisView.Name;

                  if (ctx.opts.RunView(thisView.parseType, thisView.Name))
                  {
                     viewName = thisView.Name;
                     ctx.ConsoleWriteLogLine(String.Format("\nInitializing view : {0}", viewName));
                     thisView.Initialize(ctx);
                     continue;
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Initialising view {0}: {1}", viewName, e.Message));
               return;
            }
         }


         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         TimeSpan elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         // Start the stopwatch
         stopwatch = new Stopwatch();

         // P R E   P R O C E S S   V I E W S

         using (loader.Container)
         {
            try
            {
               foreach (IView thisView in loader.Views)
               {
                  // Only call a View if either the view name is mentioned in the arguments (or the argument was '*')
                  if (ctx.opts.RunView(thisView.parseType, thisView.Name))
                  {
                     viewName = thisView.Name;
                     ctx.ConsoleWriteLogLine(String.Format("PreProcessing view : {0}", viewName));
                     thisView.PreProcess(ctx);
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Processing view {0} : {1}", viewName, e.Message));
               return;
            }
         }

         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         // Start the stopwatch
         stopwatch = new Stopwatch();

         // P R O C E S S   T I M E  S E R I E S  F I L E  P R O C E S S I N G

         // For each Log Handler defined...
         foreach (ILogFileHandler fileHandler in ctx.logFileHandlers)
         {

            // Find all files by calling File Handler Initialize
            ctx.ConsoleWriteLogLine(String.Format("FileLogHandler {0} Find all files...", fileHandler.Name));
            if (!fileHandler.Initialize(ctx))
            {
               ctx.ConsoleWriteLogLine(String.Format("LogFileHandler {0} failed to Initialize.", fileHandler.Name));
               continue;
            }

            // Set the Active Handler
            ctx.activeHandler = fileHandler;

            ctx.ConsoleWriteLogLine(String.Format("FileLogHandler {0} found {1} files.", ctx.activeHandler.Name, ctx.activeHandler.FilesFound.Length));
            using (loader.Container)
            {
               try
               {
                  foreach (IView thisView in loader.Views)
                  {
                     // Only call a View if the View ParseType matches the LogFileHandler ParseType and either the view name is
                     // mentioned in the arguments (or the argument was '*')
                     if (ctx.opts.RunView(thisView.parseType, thisView.Name))
                     {
                        if (thisView.parseType == ctx.activeHandler.parseType)
                        {
                           viewName = thisView.Name;
                           ctx.ConsoleWriteLogLine(String.Format("Processing view : {0}", viewName));
                           thisView.Process(ctx);
                        }
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Processing view {0} : {1}", viewName, e.Message));
                  return;
               }
            }
         }

         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         // Start the stopwatch
         stopwatch = new Stopwatch();

         // P O S T   P R O C E S S - W R I T E  O U T  X M L

         using (loader.Container)
         {
            try
            {
               // Import and use the plugins (they will be instantiated only when accessed)
               foreach (IView thisView in loader.Views)
               {
                  // Only Post Process a View if their ParseType was specified in the command line
                  if (ctx.opts.RunView(thisView.parseType, thisView.Name))
                  {
                     viewName = thisView.Name;
                     ctx.ConsoleWriteLogLine(String.Format("Post-Processing view : {0}", viewName));
                     thisView.PostProcess(ctx);
                  }
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Processing view {0} : {1}", viewName, ex.Message));
               return;
            }
         }

         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");


         // P R E  A N A L Y Z E

         using (loader.Container)
         {
            try
            {
               // Import and use the plugins (they will be instantiated only when accessed)
               foreach (IView thisView in loader.Views)
               {

                  viewName = thisView.Name;
                  ctx.ConsoleWriteLogLine(String.Format("PreAnalyze view : {0}", viewName));
                  thisView.PreAnalyze(ctx);
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : PreAnalyze view {0} : {1}", viewName, ex.Message));
               return;
            }
         }

         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         // Start the stopwatch
         stopwatch = new Stopwatch();


         // A N A L Y Z E

         using (loader.Container)
         {
            try
            {
               // Import and use the plugins (they will be instantiated only when accessed)
               foreach (IView thisView in loader.Views)
               {

                  viewName = thisView.Name;
                  ctx.ConsoleWriteLogLine(String.Format("Analyze view : {0}", viewName));
                  thisView.Analyze(ctx);
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Analyze view {0} : {1}", viewName, ex.Message));
               return;
            }
         }

         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         // Start the stopwatch
         stopwatch = new Stopwatch();

         // P O S T   A N A L Y Z E

         using (loader.Container)
         {
            try
            {
               // Import and use the plugins (they will be instantiated only when accessed)
               foreach (IView thisView in loader.Views)
               {

                  viewName = thisView.Name;
                  ctx.ConsoleWriteLogLine(String.Format("PostAnalyze view : {0}", viewName));
                  thisView.PostAnalyze(ctx);
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Post Analyze view {0} : {1}", viewName, ex.Message));
               return;
            }
         }

         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         // Start the stopwatch
         stopwatch = new Stopwatch();


         // W R I T E   E X C E L

         string excelFileName = ctx.WorkFolder + "\\" + Path.GetFileNameWithoutExtension(ctx.ZipFileName) + ctx.opts.Suffix() + ".xlsx";

         // if the Excel file exists, delete it.
         Console.WriteLine("If the Excel file exists, delete it:");
         if (ctx.ioProvider.Exists(excelFileName))
         {
            ctx.ioProvider.Delete(excelFileName);
         }

         // for each View DLL found
         using (loader.Container)
         {
            try
            {
               // Import and use the plugins (they will be instantiated only when accessed)
               foreach (IView thisView in loader.Views)
               {
                  // Only call WriteExcel for a View if their ParseType was specified in the command line
                  if (ctx.opts.RunView(thisView.parseType, thisView.Name))
                  {
                     viewName = thisView.Name;
                     ctx.ConsoleWriteLogLine(String.Format("Write Excel view : {0}", viewName));
                     thisView.WriteExcel(ctx);
                  }
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Writing Excel view {0} : {1}", viewName, ex.Message));
               return;
            }
         }


         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         // Start the stopwatch
         stopwatch = new Stopwatch();

         // for each View DLL found
         using (loader.Container)
         {
            try
            {
               // Import and use the plugins (they will be instantiated only when accessed)
               foreach (IView thisView in loader.Views)
               {
                  // Only call Cleanup for a View if their ParseType was specified in the command line
                  if (ctx.opts.RunView(thisView.parseType, thisView.Name))
                  {
                     viewName = thisView.Name;
                     ctx.ConsoleWriteLogLine(String.Format("Cleanup view : {0}", viewName));
                     thisView.Cleanup(ctx);
                  }
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION : Calling Cleanup view {0} : {1}", viewName, ex.Message));
               return;
            }
         }


         // Stop the stopwatch
         stopwatch.Stop();

         // Log the elapsed time
         elapsedTime = stopwatch.Elapsed;
         ctx.ConsoleWriteLogLine($"Elapsed Time: {elapsedTime.TotalMilliseconds} milliseconds");

         ctx.ConsoleWriteLogLine("Application End");
      }
   }
}
