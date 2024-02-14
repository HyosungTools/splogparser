
using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum ATLogType
   {
      /* Not a log line we are interested in */
      None,

      ActiveTellerConnectionState,
      AgentConfiguration,
      AgentHost,
      ConnectionManagerAction,
      ServerRequest,

      /* ERROR */
      Error
   }

   public class ATLine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }
      public string HResult { get; set; }
      public ATLogType atType { get; set; }

      public ATLine(ILogFileHandler parent, string logLine, ATLogType atType) : base(parent, logLine)
      {
         this.atType = atType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         HResult = hResult();
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         // set timeStamp to a default time
         string timestamp = @"2023-01-01 00:00:00.000";

         // 2023-11-16 03:00:47 Connection manager registering client using device id 70-85-C2-18-7C-DA

         // search for timestamp in the log line
         string regExp = @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}";
         Regex timeRegex = new Regex(regExp);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            timestamp = m.Groups[0].Value;
         }

         return timestamp;
      }
   }
}
