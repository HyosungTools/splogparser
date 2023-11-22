using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFPOPEN : SPLine
   {
      public string hService;
      public string lpszLogicalName;
      public string lpszAppID;

      public WFPOPEN(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFPOPEN) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result = Util.MatchList(logLine, "(?<=hService = \\[)(\\d+)");
         hService = result.xfsMatch;

         result = Util.MatchList(logLine, "(?<=lpszLogicalName = \\[)(\\w+)");
         lpszLogicalName = result.xfsMatch;

         result = Util.MatchList(logLine, "(?<=lpszAppID = \\[)([A-Za-z0-9. ]+)");
         lpszAppID = result.xfsMatch;
      }

      protected override string tsTimestamp()
      {
         // the WFPCLOSE log lines doesnt have a timestamp like all the others. 
         // 
         // the string from the log file, but return is in normal form
         // (replace '/' with '-' and the 2nd space with a ':')
         string logTime = "2022/01/01 00:00 00.000";

         // Example: 2023/04/04001202:59 48.532
         Regex timeRegex = new Regex(@"^.*(?<DATE>\d{4}/\d{2}/\d{2})(?<TIME> \d{2}:\d{2} \d{2}\.\d{3}).*$");
         Match mtch = timeRegex.Match(logLine);
         if (mtch.Success)
         {
            logTime = mtch.Groups["DATE"].Value + mtch.Groups["TIME"].Value;
         }

         // replace / with -
         logTime = logTime.Replace('/', '-');

         // replace the 17th character (' ' with ':')
         char replacementChar = ':';

         if (logTime.Length >= 17)
         {
            logTime = logTime.Remove(16, 1).Insert(16, replacementChar.ToString());
         }

         return logTime;
      }
   }
}
