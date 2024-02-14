
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum AELogType
   {
      /* Not an log line we are interested in */
      None,

      ExtensionStarted,
      NetOpExtension,
      NextwareExtension,
      MoniPlus2sExtension,

      /* ERROR */
      Error
   }

   public class AELine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }
      public string HResult { get; set; }
      public AELogType aeType { get; set; }

      public AELine(ILogFileHandler parent, string logLine, AELogType aeType) : base(parent, logLine)
      {
         this.aeType = aeType;
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

         //2023-11-17 00:59:17 [MoniPlus2sExtension] Sending OperatingMode to application: {"AssetId":11,"AssetName":"A036201","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}

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
