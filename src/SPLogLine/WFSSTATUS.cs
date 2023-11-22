using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSSTATUS : SPLine
   {
      public string SPVersion { get; set; }
      public string EPVersion { get; set; }
      public string lpszExtra { get; set; }

      public WFSSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType) : base(parent, logLine, xfsType)
      {
      }

      // non-destructive read of logLine

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = SPVersionFromStatus(logLine);
         if (result.success)
         {
            SPVersion = result.xfsMatch.Trim();
         }
         else
         {
            result = SP_VersionFromStatus(logLine);
            if (result.success)
            {
               SPVersion = result.xfsMatch.Trim();
            }
         }

         result = EPVersionFromStatus(logLine);
         if (result.success)
         {
            EPVersion = result.xfsMatch.Trim();
         }
         else
         {
            result = EP_VersionFromStatus(logLine);
            if (result.success)
            {
               EPVersion = result.xfsMatch.Trim();
            }
         }

         result = lpszExtraFromStatus(logLine);
         if (result.success)
         {
            lpszExtra = result.xfsMatch.Trim();
         }
      }

      protected static (bool success, string xfsMatch, string subLogLine) SPVersionFromStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=SPVersion=)(.*?)\\,");
      }
      protected static (bool success, string xfsMatch, string subLogLine) EPVersionFromStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=EPVersion=)(.*?)\\,");
      }

      protected static (bool success, string xfsMatch, string subLogLine) SP_VersionFromStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=SP_Version=)(.*?)\\,");
      }
      protected static (bool success, string xfsMatch, string subLogLine) EP_VersionFromStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=EP_Version=)(.*?)\\,");
      }

      protected static (bool success, string xfsMatch, string subLogLine) lpszExtraFromStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=lpszExtra = )(.*?)\r\n");
      }
   }
}
