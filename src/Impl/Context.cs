using System;
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

      // list of log files to parse
      public string[] nwlogFiles { get; set; }

      // folder to store temp files and Excel file
      public string WorkFolder { get; set; }

      // subfolder to unzip zip file into, also the name of the zip file
      public string SubFolder { get; set; }

      // (full) zip file name
      public string ZipFileName { get; set; }

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ioProvider">object for file system access</param>
      /// <param name="logger">logger object</param>
      /// <param name="workFolder">temporary work folder</param>
      /// <param name="subFolder">folder to hold unzipped data</param>
      /// <param name="zipFileName">name of the zip file to oprate on</param>
      public Context(IFileSystemProvider ioProvider, ILogger logger, string workFolder, string subFolder, string zipFileName)
      {
         this.ioProvider = ioProvider;
         this.logger = logger;
         nwlogFiles = new string[0];
         this.WorkFolder = workFolder;
         this.SubFolder = subFolder;
         this.ZipFileName = zipFileName;
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
         this.LogWriteLine(message);
      }
   }
}
