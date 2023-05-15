
using System.Text.RegularExpressions;

namespace Impl
{
   public static class LogTime
   {
      public static string GetTimeFromLogLine(string logLine)
      {
         string logTime = "2022-01-01T00:00:00.000";
         // Example: P00102022/12/07001201:41 31.6120006Normal0024
         Regex timeRegEx = new Regex("(\\d{4})/(\\d{2})/(\\d{2}).*(\\d{2}:\\d{2}) (\\d{2}\\.\\d{3})");
         Match mtch = timeRegEx.Match(logLine);
         if (mtch.Success && mtch.Groups.Count >= 5)
         {
            logTime = mtch.Groups[1].Value + "-" + mtch.Groups[2].Value + "-" + mtch.Groups[3].Value + "T" +
                      mtch.Groups[4].Value + ":" + mtch.Groups[5].Value;
         }
         return logTime;
      }

      public static string GetTimeFromLogLine2(string logLine)
      {
         string logTime = "22023/04/04 03:03 20.025";
         // Example: FRAMEWORK00102023/04/04001203:05 20.0260011
         //                       2023/04/04 03:03 20.025
         Regex timeRegEx = new Regex("(\\d{4})/(\\d{2})/(\\d{2}).*(\\d{2}:\\d{2}) (\\d{2}\\.\\d{3})");
         Match mtch = timeRegEx.Match(logLine);
         if (mtch.Success && mtch.Groups.Count >= 6)
         {
            logTime = mtch.Groups[1].Value + "/" + mtch.Groups[2].Value + "/" + mtch.Groups[3].Value + " " +
                      mtch.Groups[4].Value + " " + mtch.Groups[5].Value;
         }
         return logTime;
      }
   }
}
