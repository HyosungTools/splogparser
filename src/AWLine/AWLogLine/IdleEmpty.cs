using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class IdleEmpty : AWLine
   {
      private string className = "IdleEmpty";


      public string Memory { get; set; } = string.Empty;
      public string VMSize {  get; set; } = string.Empty;
      public string PrivateSize { get; set; } = string.Empty;
      public string HandleCount { get; set; } = string.Empty;


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
            IsRecognized = true;


            /*
            string subtag = "UserIdleEmpty Path: ";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("UserIdleEmptyPath", subLogLine.Substring(subtag.Length));
               IsRecognized = true;
            }
            */

            Regex regex = new Regex("Memory      : (?<size>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               Memory = m.Groups["size"].Value;
               IsRecognized = true;
            }

            regex = new Regex("VM      size: (?<size>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VMSize = m.Groups["size"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Private size: (?<size>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               PrivateSize = m.Groups["size"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Handle count:(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               HandleCount = m.Groups["count"].Value;
               IsRecognized = true;
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
