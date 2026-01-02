using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Handler for WinCE electronic journal files (JNL*.dat).
   /// Text format files with records separated by 'X' + File Separator (0x1C).
   /// </summary>
   public class WinCEJournalHandler : LogHandler, ILogFileHandler
   {
      /// <summary>
      /// File separator character used to delimit records
      /// </summary>
      private const char FILE_SEPARATOR = '\x1C';

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="createReader">Stream reader factory</param>
      public WinCEJournalHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.WinCE, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(parseType, createReader, Factory)
      {
         Name = "WinCEJournalHandler";
         LogExpression = "JNL*.dat";
      }

      /// <summary>
      /// Reads the next record from the journal file.
      /// Records are separated by 'X' followed by File Separator (0x1C).
      /// </summary>
      /// <returns>Raw record as string</returns>
      public string ReadLine()
      {
         if (traceFilePos >= logFile.Length)
         {
            return string.Empty;
         }

         StringBuilder record = new StringBuilder();

         // Read until we hit the record terminator (X followed by 0x1C)
         while (traceFilePos < logFile.Length)
         {
            char c = logFile[traceFilePos];
            record.Append(c);
            traceFilePos++;

            // Check for record terminator: 'X' followed by File Separator
            if (c == FILE_SEPARATOR && record.Length >= 2)
            {
               // Found the end of a record
               break;
            }
         }

         return record.ToString();
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
      /// Identify and parse a raw line into a WinCEJournalLine
      /// </summary>
      /// <param name="logLine">Raw line data</param>
      /// <returns>Parsed WinCEJournalLine or null</returns>
      public ILogLine IdentifyLine(string logLine)
      {
         return Factory(this, logLine);
      }
   }
}
