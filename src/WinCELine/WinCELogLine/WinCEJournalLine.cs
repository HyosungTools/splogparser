using System;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Journal type enumeration for WinCE journal entries
   /// </summary>
   public enum WinCEJournalType
   {
      None,
      Operation,    // SF, OA, SA, SD, SG, D3
      Transaction,  // SC, CW, BI
      Unknown
   }

   /// <summary>
   /// Represents a parsed line from a WinCE electronic journal file (JNL*.dat).
   /// Text format: Length^KindCode^Sequence^Year^Month^Day^Hour^Min^Sec^Data...^X[0x1C]
   /// </summary>
   public class WinCEJournalLine : LogLine, ILogLine
   {
      // ILogLine implementations
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      /// <summary>
      /// Journal type enumeration value
      /// </summary>
      public WinCEJournalType journalType { get; set; }

      /// <summary>
      /// Record sequence number
      /// </summary>
      public string Sequence { get; set; }

      /// <summary>
      /// 2-character record type code (e.g., "SF", "OA", "CW")
      /// </summary>
      public string KindCode { get; set; }

      /// <summary>
      /// Remaining data fields as single string
      /// </summary>
      public string Data { get; set; }

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="parent">The handler processing this line</param>
      /// <param name="logLine">Raw log line data</param>
      /// <param name="journalType">Parsed journal type</param>
      public WinCEJournalLine(ILogFileHandler parent, string logLine, WinCEJournalType journalType) 
         : base(parent, logLine)
      {
         this.journalType = journalType;
         Initialize();
      }

      /// <summary>
      /// Initialize properties from parsed log line
      /// </summary>
      protected virtual void Initialize()
      {
         ParseFields();
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      /// <summary>
      /// Parse the caret-delimited fields from the log line
      /// Format: Length^KindCode^Sequence^Year^Month^Day^Hour^Min^Sec^Data...^X
      /// </summary>
      protected virtual void ParseFields()
      {
         KindCode = string.Empty;
         Sequence = string.Empty;
         Data = string.Empty;

         if (string.IsNullOrEmpty(logLine))
         {
            return;
         }

         try
         {
            string[] fields = logLine.Split('^');
            if (fields.Length < 9)
            {
               return;
            }

            // fields[0] = Length (not used)
            KindCode = fields[1].Trim();
            Sequence = fields[2].Trim();

            // Collect remaining data fields (index 9 onwards)
            if (fields.Length > 9)
            {
               for (int i = 9; i < fields.Length; i++)
               {
                  string field = fields[i].Trim();
                  // Skip the 'X' terminator
                  if (field == "X" || field.StartsWith("X\x1C"))
                  {
                     continue;
                  }
                  if (!string.IsNullOrEmpty(Data))
                  {
                     Data += "^";
                  }
                  Data += field;
               }
            }
         }
         catch
         {
            // Leave fields empty on parse error
         }
      }

      /// <summary>
      /// Extract timestamp from caret-delimited date/time fields
      /// Format: Year^Month^Day^Hour^Min^Sec (fields 3-8)
      /// </summary>
      protected override string tsTimestamp()
      {
         string timestamp = LogLine.DefaultTimestamp;

         if (string.IsNullOrEmpty(logLine))
         {
            return timestamp;
         }

         try
         {
            string[] fields = logLine.Split('^');
            if (fields.Length < 9)
            {
               return timestamp;
            }

            string year = fields[3].Trim();
            string month = fields[4].Trim().PadLeft(2, '0');
            string day = fields[5].Trim().PadLeft(2, '0');
            string hour = fields[6].Trim().PadLeft(2, '0');
            string minute = fields[7].Trim().PadLeft(2, '0');
            string second = fields[8].Trim().PadLeft(2, '0');

            // Validate numeric fields
            if (int.TryParse(year, out int y) && y >= 1990 && y <= 2100 &&
                int.TryParse(month, out int m) && m >= 1 && m <= 12 &&
                int.TryParse(day, out int d) && d >= 1 && d <= 31 &&
                int.TryParse(hour, out int h) && h >= 0 && h <= 23 &&
                int.TryParse(minute, out int min) && min >= 0 && min <= 59 &&
                int.TryParse(second, out int s) && s >= 0 && s <= 59)
            {
               timestamp = $"{year}-{month}-{day} {hour}:{minute}:{second}.000";
            }
         }
         catch
         {
            // Return default timestamp on any error
         }

         return timestamp;
      }

      /// <summary>
      /// No HResult for journal entries
      /// </summary>
      protected override string hResult()
      {
         return string.Empty;
      }

      /// <summary>
      /// Determine journal type from KindCode
      /// </summary>
      /// <param name="kindCode">2-character kind code</param>
      /// <returns>WinCEJournalType</returns>
      private static WinCEJournalType GetJournalType(string kindCode)
      {
         switch (kindCode)
         {
            // Operation codes
            case "SF":  // System/Fiscal Start
            case "OA":  // Operational Alert
            case "SA":  // Status Alert
            case "SD":  // Status Change
            case "SG":  // System/General
            case "D3":  // Day Total
               return WinCEJournalType.Operation;

            // Transaction codes
            case "SC":  // Transaction Completion
            case "CW":  // Cash Withdrawal
            case "BI":  // Balance Inquiry
               return WinCEJournalType.Transaction;

            default:
               return WinCEJournalType.Unknown;
         }
      }

      /// <summary>
      /// Factory method to create WinCEJournalLine from raw text.
      /// Format: Length^KindCode^Sequence^Year^Month^Day^Hour^Min^Sec^Data...^X
      /// </summary>
      /// <param name="logFileHandler">The handler processing this line</param>
      /// <param name="logLine">Raw line data</param>
      /// <returns>WinCEJournalLine if valid, null otherwise</returns>
      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (string.IsNullOrEmpty(logLine) || logLine.Length < 20)
         {
            return null;
         }

         try
         {
            // Split by caret delimiter
            string[] fields = logLine.Split('^');

            // Minimum fields: Length, KindCode, Sequence, Year, Month, Day, Hour, Min, Sec
            if (fields.Length < 9)
            {
               return null;
            }

            // Validate KindCode exists
            string kindCode = fields[1].Trim();
            if (string.IsNullOrEmpty(kindCode))
            {
               return null;
            }

            // Validate date/time fields are numeric
            string year = fields[3].Trim();
            string month = fields[4].Trim();
            string day = fields[5].Trim();
            string hour = fields[6].Trim();
            string minute = fields[7].Trim();
            string second = fields[8].Trim();

            if (!int.TryParse(year, out int y) || y < 1990 || y > 2100 ||
                !int.TryParse(month, out int m) || m < 1 || m > 12 ||
                !int.TryParse(day, out int d) || d < 1 || d > 31 ||
                !int.TryParse(hour, out int h) || h < 0 || h > 23 ||
                !int.TryParse(minute, out int min) || min < 0 || min > 59 ||
                !int.TryParse(second, out int s) || s < 0 || s > 59)
            {
               return null;
            }

            WinCEJournalType journalType = GetJournalType(kindCode);
            return new WinCEJournalLine(logFileHandler, logLine, journalType);
         }
         catch
         {
            return null;
         }
      }
   }
}
