using System;
using System.Collections.Generic;
using System.IO;
using Contract;

namespace Impl
{
   /// <summary>
   /// class for carrying a bag of information pertaining to the current run. 
   /// </summary>
   public class Context : IContext
   {
      // the object for interfacing with the file system
      public IFileSystemProvider ioProvider { get; set; }

      // logger object
      public ILogger logger { get; set; }

      // logFile handlers
      public List<ILogFileHandler> logFileHandlers { get; }

      public ILogFileHandler activeHandler { get; set; }

      // folder to store temp files and Excel file
      public string WorkFolder { get; set; }

      // subfolder to unzip zip file into, also the name of the zip file
      public string SubFolder { get; set; }

      // (full) zip file name
      public string ZipFileName { get; set; }

      // Command Line Options
      public IOptions opts { get; set; }


      /// <summary>
      /// Returns a standard Excel filename
      /// </summary>
      /// <returns></returns>
      public string ExcelFileName
      {
         get
         {
            string extension = ".xlsx";
            string filename = Path.GetFileNameWithoutExtension(ZipFileName) + opts.Suffix() + extension;

            // illegal characters for Excel filename
            char[] badchars = new char[] { '<', '>', '?', '[', ']', ':', '|', '*' };
            foreach (char c in badchars)
            {
               filename = filename.Replace(c, '_');
            }

            string excelFilePath = WorkFolder + "\\" + filename;

            // truncate path longer than 218 characters
            int maxpathlen = 218;

            if (excelFilePath.Length > maxpathlen)
            {
               ConsoleWriteLogLine($"Excel output filename too long ({excelFilePath.Length}), truncating to {maxpathlen} characters");

               excelFilePath = excelFilePath.Replace(extension, string.Empty).Substring(0, (maxpathlen - extension.Length)) + extension;
            }

            return excelFilePath;
         }
      }



      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ioProvider">object for file system access</param>
      /// <param name="logger">logger object</param>
      /// <param name="workFolder">temporary work folder</param>
      /// <param name="subFolder">folder to hold unzipped data</param>
      /// <param name="zipFileName">name of the zip file to oprate on</param>
      public Context(IFileSystemProvider ioProvider, ILogger logger, string workFolder, string subFolder, string zipFileName, IOptions opts)
      {
         this.ioProvider = ioProvider;
         this.logger = logger;
         logFileHandlers = new List<ILogFileHandler>();
         WorkFolder = workFolder;
         SubFolder = subFolder;
         ZipFileName = zipFileName;
         this.opts = opts;
      }

      /// <summary>
      /// Write a log line to the logfile
      /// </summary>
      /// <param name="message">log line</param>
      public void LogWriteLine(string message)
      {
         logger.WriteLog(message);
      }
      /// <summary>
      /// Write a log line to the console and the log file
      /// </summary>
      /// <param name="message">log line to write</param>
      public void ConsoleWriteLogLine(string message)
      {
         Console.WriteLine(message);
         LogWriteLine(message);
      }
   }
}
