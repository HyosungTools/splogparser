using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class StringResourceManager : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "StringResourceManager";
      private bool isRecognized = false;


      public StringResourceManager(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.StringResourceManager) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[StringResourceManager]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //CurrentCulture = [en-US]
            //There is no StringResource\ResourceDictionary.xaml

            string subtag = "CurrentCulture = ";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("CurrentCulture", subLogLine.Substring(subtag.Length));
               isRecognized = true;
            }

            subtag = "There is no StringResource\\ResourceDictionary.xaml";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("ResourceDictionaryXaml", "none");
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
