using System;
using System.IO;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Handler for WinCE trace log files (*.log).
   /// Binary format files with pattern: NN_YYYYMMDD.log
   /// </summary>
   public class WinCETraceLogHandler : LogHandler, ILogFileHandler
   {
      /// <summary>
      /// Date portion extracted from current filename (YYYY-MM format)
      /// Used to construct full timestamps from packed binary day/hour/minute
      /// </summary>
      private string currentFileDate = string.Empty;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="createReader">Stream reader factory</param>
      public WinCETraceLogHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.WinCE, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(parseType, createReader, Factory)
      {
         Name = "WinCETraceLogHandler";
         LogExpression = "*_????????.log";
      }

      /// <summary>
      /// Opens a log file and extracts the date from the filename.
      /// Filename format: NN_YYYYMMDD.log
      /// </summary>
      /// <param name="fileName">Full path to log file</param>
      /// <param name="offset">Optional offset into file</param>
      public override void OpenLogFile(string fileName, int offset = 0)
      {
         base.OpenLogFile(fileName, offset);

         // Extract date from filename pattern: NN_YYYYMMDD.log
         string baseName = Path.GetFileNameWithoutExtension(fileName);
         Match match = Regex.Match(baseName, @"_(\d{4})(\d{2})(\d{2})$");
         if (match.Success)
         {
            currentFileDate = $"{match.Groups[1].Value}-{match.Groups[2].Value}";
         }
         else
         {
            currentFileDate = DateTime.Now.ToString("yyyy-MM");
         }
      }

      /// <summary>
      /// Reads the next line from the binary log file.
      /// Each line ends with CRLF.
      /// </summary>
      /// <returns>Raw line as string (may contain binary bytes)</returns>
      public string ReadLine()
      {
         if (traceFilePos >= logFile.Length)
         {
            return string.Empty;
         }

         int startPos = traceFilePos;

         // Find end of line (CRLF)
         while (traceFilePos < logFile.Length)
         {
            // Check for CRLF (need room for both characters)
            if (traceFilePos < logFile.Length - 1 &&
                logFile[traceFilePos] == '\r' && logFile[traceFilePos + 1] == '\n')
            {
               traceFilePos += 2; // Skip past CRLF
               break;
            }
            traceFilePos++;
         }

         int length = traceFilePos - startPos;
         if (length <= 0)
         {
            return string.Empty;
         }

         return new string(logFile, startPos, length);
      }

      /// <summary>
      /// Check if we've reached end of file
      /// </summary>
      /// <returns>True if at or past end of file</returns>
      public bool EOF()
      {
         return traceFilePos >= logFile.Length;
      }

      /// <summary>
      /// Identify and parse a raw line into a WinCETraceLine
      /// </summary>
      /// <param name="logLine">Raw line data</param>
      /// <returns>Parsed WinCETraceLine or null</returns>
      public ILogLine IdentifyLine(string logLine)
      {
         return Factory(this, logLine);
      }
   }
}
