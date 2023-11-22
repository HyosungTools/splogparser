using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Contract;
using Impl;
using LogFileHandler;


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
         if (args.Length < 1)
         {
            Console.WriteLine("splogparser + <zipfile>");
            return;
         }

         if (!args[0].ToUpper().Contains(".ZIP"))
         {
            Console.WriteLine("expected a zip file");
            return;
         }

         // Define the FileSystemProvider for this run
         IFileSystemProvider ioProvider = new FileSystemProvider();

         // Use the FileSystemProvider to gather information
         string zipFileName = args[0];
         string workFolder = ioProvider.GetCurrentDirectory();
         string subFolder = ioProvider.GetFileNameWithoutExtension(zipFileName);

         //Define the Logger
         ILogger logger = new Logger(ioProvider, workFolder, subFolder);

         // Define the Context. 
         IContext ctx = new Context(ioProvider, logger, workFolder, subFolder, zipFileName);

         // Write out settings so far...
         ctx.ConsoleWriteLogLine("Application Start");
         ctx.ConsoleWriteLogLine("ZipFileName: " + $"{zipFileName}");
         ctx.ConsoleWriteLogLine("Work Folder: " + ctx.WorkFolder);
         ctx.ConsoleWriteLogLine("unzip to   : " + ctx.WorkFolder + "\\" + ctx.SubFolder);

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
                  viewName = thisView.Name;
                  ctx.ConsoleWriteLogLine(String.Format("Initialising View: {0}", viewName));
                  thisView.Initialize(ctx);
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


         // P R O C E S S   T I M E  S E R I E S  F I L E  P R O C E S S I N G

         // Define the LogFileHandlers
         ctx.ConsoleWriteLogLine(String.Format("Define the LogFileHandlers"));
         ILogFileHandler logFileHandler = (ILogFileHandler)new SPLogHandler(new CreateTextStreamReader());
         ctx.logFileHandlers.Add(logFileHandler);

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

            ctx.ConsoleWriteLogLine(String.Format("FileLogHandler {0} found {1} files.", fileHandler.Name, fileHandler.FilesFound.Length));
            using (loader.Container)
            {
               try
               {
                  foreach (IView thisView in loader.Views)
                  {
                     viewName = thisView.Name;
                     thisView.Process(fileHandler);
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
                  viewName = thisView.Name;
                  thisView.PostProcess(ctx);
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

         // Start the stopwatch
         stopwatch = new Stopwatch();

         // W R I T E   E X C E L

         string excelFileName = ctx.WorkFolder + "\\" + Path.GetFileNameWithoutExtension(ctx.ZipFileName) + "_SP.xlsx";

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
                  viewName = thisView.Name;
                  ctx.ConsoleWriteLogLine(String.Format("Calling View to write Excel: {0}", viewName));
                  thisView.WriteExcel(ctx);
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
                  viewName = thisView.Name;
                  ctx.ConsoleWriteLogLine(String.Format("Calling cleanup: {0}", viewName));
                  thisView.Cleanup(ctx);
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

         Console.WriteLine("Application End");
      }
   }
}
