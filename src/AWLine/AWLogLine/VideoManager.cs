using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class VideoManager : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "VideoManager";
      private bool isRecognized = false;


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
               SettingDict.Add("VideoEngineState", "SIGN IN ATTEMPT");
               SettingDict.Add("User", m.Groups["user"].Value);
               SettingDict.Add("URI", m.Groups["uri"].Value);
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
