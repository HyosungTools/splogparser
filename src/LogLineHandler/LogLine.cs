using System;
using System.IO;
using System.Runtime.CompilerServices;
using Contract;

namespace LogLineHandler
{
   public abstract class LogLine
   {
      public static string DateTimeFormatStringMsec = "yyyy-MM-dd hh:mm:ss.ffff";
      public static string TimeFormatStringMsec = "hh:mm:ss.ffff";
      public static string DefaultTimestamp = @"2020-01-01 00:00:00.000";


      public static string DateTimestampToTableString(DateTime dt)
      {
         return (dt == DateTime.MinValue) || (dt == default(DateTime)) ? string.Empty : dt.ToString(DateTimeFormatStringMsec);
      }

      public static string TimestampToTableString(DateTime dt)
      {
         return (dt == DateTime.MinValue) || (dt == default(DateTime)) ? string.Empty : dt.ToString(TimeFormatStringMsec);
      }


      // my parent
      public ILogFileHandler parentHandler;

      // my logfile
      public string LogFile
      {
         get { return parentHandler.LogFile; }
      }

      // my logLine
      public string logLine;

      /// <summary>
      /// A value indicating whether the log line is recognized by the parser. 
      /// </summary>
      public bool IsRecognized { get; set; } = false;

      /// <summary>
      /// A value indicating whether the log line should be ignored.
      /// </summary>
      public bool IgnoreThisLine { get; set; } = false;

      /// <summary>
      /// A value indicating whether the Timestamp property contains a valid value.
      /// </summary>
      public bool IsValidTimestamp { get; set; } = false;

      /// <summary>
      /// A value indicating whether an exception should be thrown if the log line
      /// is not recognized by the parser.
      /// </summary>
      public bool ThrowExceptionIfNotRecognized { get; set; } = false;


      public LogLine(ILogFileHandler parent, string logLine)
      {
         parentHandler = parent;
         this.logLine = logLine;
      }

      protected LogLine(ILogFileHandler parent)
      {
          this.parentHandler = parent;
      }

      /// <summary>
      /// Gets the Timestamp from the logline text.
      /// </summary>
      /// <returns>A formatted DateTime string.</returns>
      protected abstract string tsTimestamp();

      /// <summary>
      /// Gets the value of an hResult field from the logline text.
      /// </summary>
      /// <returns></returns>
      protected abstract string hResult();


      /// <summary>
      /// Gets a value indicating whether the timestamp string contains a valid DateTime formatted string.
      /// </summary>
      /// <param name="timestamp"></param>
      /// <returns></returns>
      protected virtual bool bCheckValidTimestamp(string timestamp)
      {
         if (timestamp == DefaultTimestamp)
         {
            return false;
         }

         DateTime dtResult;
         return DateTime.TryParse(timestamp, out dtResult);
      }


      /// <summary>
      /// Given a nwlog log line (that can span multiple lines in the log file) read one line
      /// Returns a tuple where: 
      /// found - indicates whether a line was found
      /// oneLine - is the line, 
      /// subLogLine - is the rest of the logLine
      /// </summary>
      /// <param name="logLine">the nwlog line (that can span multiple lines in the log file)</param>
      /// <returns>tuple</returns>
      public static (bool found, string oneLine, string subLogLine) ReadNextLine(string logLine)
      {
         string subLogLine = logLine;
         StreamReader streamReader = new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(subLogLine)));
         string oneLine = streamReader.ReadLine();
         if (!(string.IsNullOrEmpty(oneLine) || string.IsNullOrWhiteSpace(oneLine)))
         {
            // we were able to read one line - for the next line trim any leftover \r\n from the found line
            subLogLine = subLogLine.Substring(oneLine.Length).TrimStart();
            return (true, oneLine, subLogLine);
         }
         // we were not able to read a line
         return (false, string.Empty, subLogLine);
      }
   }
}
