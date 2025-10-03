using System;
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

      private static readonly Regex TimeRegex = new Regex(@"(?<DATE>\d{4}/\d{2}/\d{2})\d{4}(?<TIME>\d{2}:\d{2} \d{2}\.\d{3})", RegexOptions.Compiled);
      private static readonly string logTime = "2022/01/01 00:00 00.000";

      protected override string tsTimestamp()
      {
         if (string.IsNullOrEmpty(logLine))
         {
            Console.WriteLine("ParseTimestamp: Empty log line.");
            return logTime;
         }

         // 2023/04/04001203:05 20.026
         var match = TimeRegex.Match(logLine);
         if (!match.Success)
         {
            Console.WriteLine($"tsTimestamp - logLine : {logLine}");
            Console.WriteLine($"tsTimestamp - No timestamp match found.");
            return logTime;
         }

         string date = match.Groups["DATE"].Value.Replace("/", "-");
         string time = match.Groups["TIME"].Value.Replace(" ", ":");
         string formatted = $"{date} {time}";

         return formatted;
      }
   }
}
