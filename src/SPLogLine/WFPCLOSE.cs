using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFPCLOSE : SPLine
   {
      public string hService;

      public WFPCLOSE(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFPCLOSE) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (_, string xfsMatch, _) = Util.MatchList(logLine, "(?<=hService = \\[)(\\d+)");
         hService = xfsMatch;
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
