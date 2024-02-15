using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class IdleEmpty : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "IdleEmpty";
      private bool isRecognized = false;


      public IdleEmpty(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.IdleEmpty) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[IdleEmpty           ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //---------PROCESS INFORMATION----------------
            //Date  Time  :9/13/2023 10:04:16 AM
            //Memory      : 187,736,064
            //VM      size: 782,123,008
            //Private size: 172,941,312
            //Handle count:7801
            //--------------------------------------------------

            // ignore
            isRecognized = true;
           

            /*
            string subtag = "UserIdleEmpty Path: ";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("UserIdleEmptyPath", subLogLine.Substring(subtag.Length));
               isRecognized = true;
            }

            Regex regex = new Regex("Notification ring duration is (?<duration>[\\-0-9]*) and location is (?<path>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("RingDuration", m.Groups["duration"].Value);
               SettingDict.Add("RingPath", m.Groups["path"].Value);
               isRecognized = true;
            }
            */
         }

         if (!isRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
