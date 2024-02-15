
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum AWLogType
   {
      /* Not an log line we are interested in */
      None,
      Settings,
      StringResourceManager,
      ConfigurationManager,
      BeeHDVideoControl,
      VideoManager,
      SignInManager,
      PermissionsManager,
      MainWindow,
      IdleEmpty,
      ConnectionManager,
      DataFlowManager,
      DeviceFactory,

      /* ERROR */
      Error
   }

   public class AWLine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }
      public string HResult { get; set; }
      public AWLogType awType { get; set; }

      public AWLine(ILogFileHandler parent, string logLine, AWLogType awType) : base(parent, logLine)
      {
         this.awType = awType;
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
         //[2023-09-13 10:02:21-717][3][BeeHDVideoControl   ]Initialize: Video client initializing.

         // search for timestamp in the log line
         string regExp = @"\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}-\d{3}\]";
         Regex timeRegex = new Regex(regExp);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            // strip [..] and convert to standard form
            timestamp = m.Groups[0].Value.Substring(1, m.Groups[0].Length-2).Replace("-",".");
         }

         return timestamp;
      }
   }
}
