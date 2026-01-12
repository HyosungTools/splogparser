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
      FatalUpper,     // 'F'
      System,         // 's'
      Timeout         // 't'
   }

   /// <summary>
   /// Represents a parsed line from a WinCE trace log file (*.log).
   /// Binary format: Type(1) + Hour(1) + Minute(1) + Second(1) + Code(11) + Message(variable) + CRLF
   /// 
   /// The Code field breaks down as:
   ///   - Unit (2 chars): Module identifier (e.g., "1A", "1E", "54")
   ///   - Func (2 chars): Function code (e.g., "01", "12", "30")
   ///   - Error (7 chars): Error code (e.g., "0000000", "9999999")
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
      /// Log level indicator character: 'o', 'O', 'f', 'F', 's', 't'
      /// </summary>
      public char LogType { get; set; }

      /// <summary>
      /// Full 11-character code (Unit + Func + Error combined)
      /// </summary>
      public string Code { get; set; }

      /// <summary>
      /// 2-character unit/module identifier (e.g., "1A", "1E", "54")
      /// Maps to Module Name via lookup table
      /// </summary>
      public string Unit { get; set; }

      /// <summary>
      /// 2-character function code (e.g., "01", "12", "30")
      /// Maps to Class Name and Func Name via lookup table
      /// </summary>
      public string Func { get; set; }

      /// <summary>
      /// 7-character error code (e.g., "0000000", "9999999")
      /// Non-zero values indicate errors
      /// </summary>
      public string Error { get; set; }

      /// <summary>
      /// Trace message/data text
      /// </summary>
      public string Message { get; set; }

      /// <summary>
      /// Date portion from filename (YYYY-MM-DD format) used for timestamp construction
      /// </summary>
      private string fileDate;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="parent">The handler processing this line</param>
      /// <param name="logLine">Raw log line data</param>
      /// <param name="traceType">Parsed trace type</param>
      /// <param name="fileDate">Date from filename (YYYY-MM-DD format)</param>
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
            Unit = string.Empty;
            Func = string.Empty;
            Error = string.Empty;
            Message = string.Empty;
            return;
         }

         // Type is first byte
         LogType = logLine[0];

         // Code is bytes 4-14 (11 characters)
         Code = logLine.Substring(4, 11);

         // Break down Code into components
         // Unit: chars 0-1 (e.g., "1A", "1E", "54")
         Unit = Code.Substring(0, 2);

         // Func: chars 2-3 (e.g., "01", "12", "30")
         Func = Code.Substring(2, 2);

         // Error: chars 4-10 (e.g., "0000000", "9999999")
         Error = Code.Substring(4, 7);

         // Message is everything after code, trimmed
         Message = logLine.Substring(15).TrimEnd('\r', '\n').Trim();
      }

      /// <summary>
      /// Extract timestamp from packed binary bytes + filename date
      /// Binary bytes: Hour(1) + Minute(1) + Second(1)
      /// Format: fileDate (YYYY-MM-DD) + Hour:Minute:Second
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
            // Extract packed timestamp bytes (bytes 1, 2, 3 are hour, minute, second)
            int hour = (int)logLine[1];
            int minute = (int)logLine[2];
            int second = (int)logLine[3];

            // Validate ranges
            if (hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59 && second >= 0 && second <= 59)
            {
               // Build timestamp using fileDate (YYYY-MM-DD) + time with seconds
               timestamp = $"{fileDate} {hour:D2}:{minute:D2}:{second:D2}.000";
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

      /// <summary>
      /// Extract full date from filename (YYYY-MM-DD format)
      /// </summary>
      private static string ExtractDateFromFilename(string fileName)
      {
         // Parse NN_YYYYMMDD.log -> YYYY-MM-DD
         string baseName = System.IO.Path.GetFileNameWithoutExtension(fileName);
         var match = System.Text.RegularExpressions.Regex.Match(baseName, @"_(\d{4})(\d{2})(\d{2})$");
         if (match.Success)
         {
            return $"{match.Groups[1].Value}-{match.Groups[2].Value}-{match.Groups[3].Value}";
         }
         return DateTime.Now.ToString("yyyy-MM-dd"); // fallback
      }

      /// <summary>
      /// Factory method to create WinCETraceLine from raw binary data.
      /// </summary>
      /// <param name="logFileHandler">The handler processing this line</param>
      /// <param name="logLine">Raw line data as string (may contain binary bytes)</param>
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
               case 'F':
                  traceType = WinCETraceType.FatalUpper;
                  break;
               case 's':
                  traceType = WinCETraceType.System;
                  break;
               case 't':
                  traceType = WinCETraceType.Timeout;
                  break;
               default:
                  return null; // Invalid log type
            }

            // Validate timestamp bytes are in range (hour, minute, second)
            int hour = (int)logLine[1];
            int minute = (int)logLine[2];
            int second = (int)logLine[3];

            if (hour < 0 || hour > 23 || minute < 0 || minute > 59 || second < 0 || second > 59)
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
