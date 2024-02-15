using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class VideoManager : AWLine
   {
      private string className = "VideoManager";
      private bool isRecognized = false;

      public string VideoEngineState { get; set; } = string.Empty;
      public string User { get; set; } = string.Empty;
      public string Uri { get; set; } = string.Empty;

      public VideoManager(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.VideoManager) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[VideoManager        ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //Attempting to sign in to BeeHd video: uri=192.168.20.144, user name=lpina

            string subtag = string.Empty;

            Regex regex = new Regex("Attempting to sign in to BeeHd video: uri=(?<uri>.*), user name=(?<user>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoEngineState = "SIGN IN ATTEMPT";
               User = m.Groups["user"].Value;
               Uri =m.Groups["uri"].Value;
               isRecognized = true;
            }
         }

         if (!isRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
