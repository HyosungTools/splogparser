using System;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class AgentConfiguration : ATLine
   {
      public string serverUrl { get; set; }
      public long reconnectInterval { get; set; }

      public AgentConfiguration(ILogFileHandler parent, string logLine, ATLogType atType = ATLogType.AgentConfiguration) : base(parent, logLine, atType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         /*
            2023-11-17 03:00:21 Agent configuration server Url is 'http://10.37.152.15:81/activeteller/'
            2023-11-17 03:00:21 Agent configuration reconnect interval is '5000'
         */

         int idx = logLine.IndexOf("Agent configuration");
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + "Agent configuration".Length + 1);

            //server Url is 'http://10.37.152.15:81/activeteller/'
            Regex regex = new Regex("server Url is \\'(?<url>.*)\\'$");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               serverUrl = m.Groups["url"].Value;
            }

            //reconnect interval is '5000'
            regex = new Regex("reconnect interval is \\'(?<interval>[0-9]*)\\'$");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               reconnectInterval = long.Parse(m.Groups["interval"].Value);
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"ATLogLine.AgentConfiguration: did not recognize the log line '{logLine}'");
         }
      }
   }
}
