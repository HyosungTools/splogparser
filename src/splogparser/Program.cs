using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Impl;
using sp_logparser;

namespace splogparser
{
   class Program
   {
      static void Main(string[] args)
      {
         if (args.Length < 1)
         {
            Console.WriteLine("sp-logparser + <zipfile>");
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

         if (!Utilities.FindAllTraceFiles(ctx))
         {
            ctx.ConsoleWriteLogLine("Failed to find trace files");
            return;

         }

         ctx.ConsoleWriteLogLine("Number of trace files found :" + ctx.nwlogFiles.Length);

         //set CurrentDirectory to the base directory that the assembly resolver uses to probe for assemblies.
         if (!ctx.ioProvider.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory))
         {
            ctx.ConsoleWriteLogLine("Failed to set current directory to the application directory");
            return;
         }

         ctx.ConsoleWriteLogLine("Application folder: " + ctx.ioProvider.GetCurrentDirectory());

         // Load the Views in the application directory
         ViewLoader loader = new ViewLoader(ctx.ioProvider.GetCurrentDirectory());
         IEnumerable<IView> views = loader.Views;

         ctx.ConsoleWriteLogLine("Number of Views : " + views.Count().ToString());

         // for each View DLL found
         foreach (IView view in views)
         {
            try
            {
               // Pass in the Context for processing all files found
               ctx.ConsoleWriteLogLine("Calling View: " + view.Name);
               view.Process(ctx);
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine("EXCEPTION : Processing view :" + ex.Message);
            }
         }

         string excelFileName = ctx.WorkFolder + "\\" + Path.GetFileNameWithoutExtension(ctx.ZipFileName) + ".xlsx";

         // if the Excel file exists, delete it.
         Console.WriteLine("If the Excel file exists, delete it:");
         if (ctx.ioProvider.Exists(excelFileName))
         {
            ctx.ioProvider.Delete(excelFileName);
         }

         // for each View DLL found
         foreach (IView view in views)
         {
            try
            {
               // Pass in the Context for processing all files found
               ctx.ConsoleWriteLogLine("Calling View to write Excel: " + view.Name);
               view.WriteExcel(ctx);
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine("EXCEPTION : Write Excel processing view :" + ex.Message);
            }
         }

         // for each View DLL found
         foreach (IView view in views)
         {
            try
            {
               // Pass in the Context for processing all files found
               ctx.ConsoleWriteLogLine("Calling cleanup: " + view.Name);
               view.Cleanup(ctx);
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine("EXCEPTION : Cleanup processing view :" + ex.Message);
            }
         }
         Console.WriteLine("Application End");
      }
   }
}
