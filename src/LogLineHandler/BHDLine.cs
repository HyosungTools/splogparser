using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class BHDLine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }

      bool success = false;
      public string headTag = string.Empty;

      public string source = string.Empty;
      public string logLevel = string.Empty;
      public string payLoad = string.Empty;

      public BHDLine(ILogFileHandler parent, string logLine) : base(parent, logLine)
      {
         // remove all \r and \n from the line
         logLine = logLine.Replace("\r", "").Replace("\n", "");

         // parse the log line as part of initialization
      }

      virtual protected void Initialize()
      {
         Timestamp = tsTimestamp();

         // headTag - any character up to but not including a space
         Regex regex = new Regex("^(?<headTag>[^ ]*)(?<rest>.*)");
         Match m = regex.Match(logLine);
         success = m.Success;
         if (success)
         {
            headTag = m.Groups["headTag"].Value;

            // timestamp 
            regex = new Regex("[^d]*(?<timeStamp>\\d{2}/\\d{2}/\\d{2} \\d{2}:\\d{2}:\\d{2}\\.\\d{3}).(?<rest>.*)$");
            m = regex.Match(m.Groups["rest"].Value);
            success = m.Success;
            if (success)
            {
               Timestamp = m.Groups["timeStamp"].Value;

               // source - any character up to but not including a space
               regex = new Regex("(?<source>[^ ]*).*?:(?<rest>.*)$");
               m = regex.Match(m.Groups["rest"].Value);
               success = m.Success;
               if (success)
               {
                  source = m.Groups["source"].Value;

                  // logLevel - 
                  regex = new Regex("[^A-Z]*(?<logLevel>[^ ]*)(?<rest>.*)$");
                  m = regex.Match(m.Groups["rest"].Value);
                  success = m.Success;
                  if (success)
                  {
                     logLevel = m.Groups["logLevel"].Value;

                     // payload - everything else
                     regex = new Regex("[^A-Za-z0-9]*(?<payload>.*)$");
                     m = regex.Match(m.Groups["rest"].Value);
                     success = m.Success;
                     if (success)
                     {
                        payLoad = m.Groups["payload"].Value;
                     }
                  }
               }

               // post process the timestamp so its in a form suitable for Excel timestamp math
               // e.g. 2023-07-25 11:35:09.107
               Timestamp = Timestamp.Replace("/", "-");
            }
         }
      }

      protected override string tsTimestamp()
      {
         throw new System.NotImplementedException();
      }

      protected override string hResult()
      {
         throw new System.NotImplementedException();
      }
   }
}
