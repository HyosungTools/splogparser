using System.IO;
using Contract;

namespace LogLineHandler
{
   public abstract class LogLine
   {
      // my parent
      public ILogFileHandler parentHandler;

      // my logfile
      public string LogFile
      {
         get { return parentHandler.LogFile; }
      }

      // my logLine
      public string logLine;

      public LogLine(ILogFileHandler parent, string logLine)
      {
         parentHandler = parent;
         this.logLine = logLine;
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
