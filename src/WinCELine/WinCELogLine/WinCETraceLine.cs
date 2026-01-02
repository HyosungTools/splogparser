using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Log type enumeration for WinCE trace entries
   /// </summary>
   public enum WinCETraceType
   {
      None,
      Operation,      // 'o'
      OperationUpper, // 'O'
      Fatal,          // 'f'
      System          // 's'
   }

   /// <summary>
   /// Represents a parsed line from a WinCE trace log file (*.log).
   /// Binary format: Type(1) + Timestamp(3) + Code(11) + Message(variable) + CRLF
   /// </summary>
   public class WinCETraceLine : LogLine, ILogLine
   {
      // ILogLine implementations
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      /// <summary>
      /// Trace type enumeration value
      /// </summary>
      public WinCETraceType traceType { get; set; }

      /// <summary>
      /// Log level indicator character: 'o', 'O', 'f', 's'
      /// </summary>
      public char LogType { get; set; }

      /// <summary>
      /// 11-character layer/module identifier (e.g., "1A010000000")
      /// </summary>
      public string Code { get; set; }

      /// <summary>
      /// Trace message text
      /// </summary>
      public string Message { get; set; }

      /// <summary>
      /// Date portion from filename (YYYY-MM format) used for timestamp construction
      /// </summary>
      private string fileDate;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="parent">The handler processing this line</param>
      /// <param name="logLine">Raw log line data</param>
      /// <param name="traceType">Parsed trace type</param>
      /// <param name="fileDate">Date from filename (YYYY-MM format)</param>
      public WinCETraceLine(ILogFileHandler parent, string logLine, WinCETraceType traceType, string fileDate) 
         : base(parent, logLine)
      {
         this.traceType = traceType;
         this.fileDate = fileDate;
         Initialize();
      }

      /// <summary>
      /// Initialize properties from parsed log line
      /// </summary>
      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
         ParseFields();
      }

      /// <summary>
      /// Parse the binary fields from the log line
      /// </summary>
      protected virtual void ParseFields()
      {
         if (string.IsNullOrEmpty(logLine) || logLine.Length < 15)
         {
            LogType = ' ';
            Code = string.Empty;
            Message = string.Empty;
            return;
         }

         // Type is first byte
         LogType = logLine[0];

         // Code is bytes 4-14 (11 characters)
         Code = logLine.Substring(4, 11);

         // Message is everything after code, trimmed
         Message = logLine.Substring(15).TrimEnd('\r', '\n').Trim();
      }

      /// <summary>
      /// Extract timestamp from packed binary bytes + filename date
      /// Format: fileDate (YYYY-MM) + Day + Hour:Minute
      /// </summary>
      protected override string tsTimestamp()
      {
         string timestamp = LogLine.DefaultTimestamp;

         if (string.IsNullOrEmpty(logLine) || logLine.Length < 4)
         {
            return timestamp;
         }

         try
         {
            // Extract packed timestamp bytes
            int day = (int)logLine[1];
            int hour = (int)logLine[2];
            int minute = (int)logLine[3];

            // Validate ranges
            if (day >= 1 && day <= 31 && hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59)
            {
               // Build timestamp using fileDate (YYYY-MM) + day + time
               timestamp = $"{fileDate}-{day:D2} {hour:D2}:{minute:D2}:00.000";
            }
         }
         catch
         {
            // Return default timestamp on any error
         }

         return timestamp;
      }

      /// <summary>
      /// No HResult for trace logs
      /// </summary>
      protected override string hResult()
      {
         return string.Empty;
      }

      private static string ExtractDateFromFilename(string fileName)
      {
         // Parse NN_YYYYMMDD.log -> YYYY-MM
         string baseName = System.IO.Path.GetFileNameWithoutExtension(fileName);
         var match = System.Text.RegularExpressions.Regex.Match(baseName, @"_(\d{4})(\d{2})\d{2}$");
         if (match.Success)
         {
            return $"{match.Groups[1].Value}-{match.Groups[2].Value}";
         }
         return DateTime.Now.ToString("yyyy-MM"); // fallback
      }

      /// <summary>
      /// Factory method to create WinCETraceLine from raw binary data.
      /// </summary>
      /// <param name="logFileHandler">The handler processing this line</param>
      /// <param name="logLine">Raw line data as string (may contain binary bytes)</param>
      /// <param name="fileDate">Date portion (YYYY-MM) extracted from filename</param>
      /// <returns>WinCETraceLine if valid, null otherwise</returns>
      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         // Guard against null LogFile
         string fileName = logFileHandler?.LogFile;
         if (string.IsNullOrEmpty(fileName))
         {
            return null;
         }

         // Extract date from filename (format: NN_YYYYMMDD.log)
         string fileDate = ExtractDateFromFilename(logFileHandler.LogFile);

         // Minimum length: Type(1) + Timestamp(3) + Code(11) = 15 bytes plus some message
         if (string.IsNullOrEmpty(logLine) || logLine.Length < 15)
         {
            return null;
         }

         try
         {
            // Determine trace type from first byte
            char logType = logLine[0];
            WinCETraceType traceType;

            switch (logType)
            {
               case 'o':
                  traceType = WinCETraceType.Operation;
                  break;
               case 'O':
                  traceType = WinCETraceType.OperationUpper;
                  break;
               case 'f':
                  traceType = WinCETraceType.Fatal;
                  break;
               case 's':
                  traceType = WinCETraceType.System;
                  break;
               default:
                  return null; // Invalid log type
            }

            // Validate timestamp bytes are in range
            int day = (int)logLine[1];
            int hour = (int)logLine[2];
            int minute = (int)logLine[3];

            if (day < 1 || day > 31 || hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
               return null;
            }

            return new WinCETraceLine(logFileHandler, logLine, traceType, fileDate);
         }
         catch
         {
            return null;
         }
      }
   }
}
