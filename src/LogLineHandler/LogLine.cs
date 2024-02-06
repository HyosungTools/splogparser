using System;
using System.IO;
using Contract;

namespace LogLineHandler
{
   public abstract class LogLine
   {
      public static string DateTimeFormatStringMsec = "yyyy-MM-dd hh:mm:ss.ffff";
      public static string TimeFormatStringMsec = "hh:mm:ss.ffff";

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

      // whether the log line is recognized (parsed)
      public bool IsRecognized { get; set; } = false;

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

      protected abstract string tsTimestamp();
      protected abstract string hResult();


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
