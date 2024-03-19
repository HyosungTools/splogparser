using System.Collections.Generic;
using System.IO;

namespace Contract
{
   /// <summary>
   /// Interface for Context object. A way to pass around common information.  
   /// </summary>
   public interface IContext
   {
      // File System Provider for this run. 
      IFileSystemProvider ioProvider { get; }

      // Logger instance for this run. 
      ILogger logger { get; }

      // List of Log File Handlers
      List<ILogFileHandler> logFileHandlers { get; }

      // Active Log File Handler
      ILogFileHandler activeHandler { get; set; }

      // Folder to place all temporary work files and resulting Excel file
      string WorkFolder { get; set; }

      // Subfolder containing unzipped contents of log zip
      string SubFolder { get; set; }

      // Name of the log zip file
      string ZipFileName { get; set; }

      string ExcelFileName { get; }

      // Commandline Options
      IOptions opts { get; set; }

      /// <summary>
      /// Write a log line to the log file
      /// </summary>
      /// <returns>void</returns> 
      void LogWriteLine(string message);

      /// <summary>
      /// Write a log line to the console and the log file
      /// </summary>
      /// <returns>void</returns> 
      void ConsoleWriteLogLine(string message);
   }
}
