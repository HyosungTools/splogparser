using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class SignInManager : AWLine
   {
      private string className = "SignInManager";


      public string SignInState { get; set; } = string.Empty;
      public string User { get; set; } = string.Empty;
      public string Branch { get; set; } = string.Empty;
      public string Uri { get; set; } = string.Empty;

      public SignInManager(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.SignInManager) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[SignInManager       ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //Attempting to sign into ActiveTeller: userName=lpina, branchId=0
            //The ActiveTeller user lpina is signing in; video uri=192.168.20.144, branchId=0
            //ActiveTeller sign-in received a Success response.
            //The ActiveTeller user is signing out
            //ActiveTeller sign-in received a Unauthorized response.

            string subtag = "The ActiveTeller user is signing out";
            if (subLogLine.StartsWith(subtag))
            {
               SignInState = "SIGNING OUT";
               IsRecognized = true;
            }

            subtag = "ActiveTeller sign-in received a Success response.";
            if (subLogLine.StartsWith(subtag))
            {
               SignInState = "SIGN IN SUCCESS";
               IsRecognized = true;
            }

            subtag = "ActiveTeller sign-in received a Unauthorized response.";
            if (subLogLine.StartsWith(subtag))
            {
               SignInState = "UNAUTHORIZED";
               IsRecognized = true;
            }

            Regex regex = new Regex("Attempting to sign into ActiveTeller: userName=(?<user>.*), branchId=(?<branch>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SignInState = "SIGN IN ATTEMPT";
               User = m.Groups["user"].Value;
               Branch = m.Groups["branch"].Value;
               IsRecognized = true;
            }

            regex = new Regex("The ActiveTeller user (?<user>.*) is signing in; video uri=(?<uri>.*), branchId=(?<branch>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SignInState = "SIGNING IN";
               User = m.Groups["user"].Value;
               Branch = m.Groups["branch"].Value;
               Uri = m.Groups["uri"].Value;
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
