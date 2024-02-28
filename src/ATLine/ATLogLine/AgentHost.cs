using System;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class AgentHost : ATLine
   {
      public enum AgentHostStateEnum
      {
         None,
         Started
      }

      public AgentHostStateEnum state { get; set; }


      public AgentHost(ILogFileHandler parent, string logLine, ATLogType atType = ATLogType.AgentHost) : base(parent, logLine, atType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         state = AgentHostStateEnum.None;

         /*
            2023-11-17 03:00:21 Agent host started
            2023-11-17 03:00:22 Agent host started
         */

         int idx = logLine.IndexOf("Agent host");
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + "Agent host".Length + 1);

            //started
            if (subLogLine == "started")
            {
               IsRecognized = true;
               state = AgentHostStateEnum.Started;
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"ATLogLine.AgentHost: did not recognize the log line '{logLine}'");
         }
      }
   }
}
