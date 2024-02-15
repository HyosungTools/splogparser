using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class PermissionsManager : AWLine
   {
      private string className = "PermissionsManager";
      private bool isRecognized = false;

      public string State { get; set; } = string.Empty;


      public PermissionsManager(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.PermissionsManager) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[PermissionsManager  ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //Retrieving the user's permissions

            string subtag = "Retrieving the user's permissions";
            if (subLogLine.StartsWith(subtag))
            {
               State = "RETRIEVING USER PERMISSIONS";
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
