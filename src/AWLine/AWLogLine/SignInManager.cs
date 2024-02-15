using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class SignInManager : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "SignInManager";
      private bool isRecognized = false;


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
               SettingDict.Add("ActiveTellerUserSignInState", "SIGNING OUT");
               isRecognized = true;
            }

            subtag = "ActiveTeller sign-in received a Success response.";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("ActiveTellerUserSignInState", "SIGN IN SUCCESS");
               isRecognized = true;
            }

            subtag = "ActiveTeller sign-in received a Unauthorized response.";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("ActiveTellerUserSignInState", "UNAUTHORIZED");
               isRecognized = true;
            }

            Regex regex = new Regex("Attempting to sign into ActiveTeller: userName=(?<user>.*), branchId=(?<branch>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("ActiveTellerUserSignInState", "SIGN IN ATTEMPT");
               SettingDict.Add("User", m.Groups["user"].Value);
               SettingDict.Add("Branch", m.Groups["branch"].Value);
               isRecognized = true;
            }

            regex = new Regex("The ActiveTeller user (?<user>.*) is signing in; video uri=(?<uri>.*), branchId=(?<branch>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("ActiveTellerUserSignInState", "SIGNING IN");
               SettingDict.Add("User", m.Groups["user"].Value);
               SettingDict.Add("Branch", m.Groups["branch"].Value);
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
