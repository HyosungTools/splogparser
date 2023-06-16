using System.Text.RegularExpressions;

namespace Impl
{
   public static class lpResult
   {
      public static string tsTimestamp(string logLine)
      {
         // the string from the log file, but return is in normal form
         // (replace '/' with '-' and the 2nd space with a ':')
         string logTime = "2022/01/01 00:00 00.000";

         // Example: tsTimestamp = [2023/02/01 21:03 11.557],
         Regex timeRegex = new Regex("tsTimestamp = \\[(.{23})\\]");
         Match mtch = timeRegex.Match(logLine);
         if (mtch.Success)
         {
            logTime = mtch.Groups[1].Value;
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

      public static string hResult(string logLine)
      {
         string hResult = "0";

         // Example: hResult = [0],
         Regex timeRegex = new Regex("hResult = \\[(.*)\\]");
         Match mtch = timeRegex.Match(logLine);
         if (mtch.Success)
         {
            hResult = mtch.Groups[1].Value.Trim();
         }

         return hResult == "0" ? "" : hResult;
      }
   }
}
