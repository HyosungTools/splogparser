using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Settings : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "Settings";
      private bool isRecognized = false;


      public Settings(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.Settings) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[Settings            ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //UserSettings Path: C:\Users\lpina\AppData\Local\Nautilus_Hyosung\NH.ActiveTeller.Client.ex_Url_qnalorbjt4q0zmjb1ac1ptpbvvqeo5na\1.3.0.0\user.config
            //Notification ring duration is -1 and location is notification.wav

            string subtag = "UserSettings Path: ";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("UserSettingsPath", subLogLine.Substring(subtag.Length));
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
         }

         if (!isRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
