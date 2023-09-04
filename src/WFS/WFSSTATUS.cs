using Contract;

namespace Impl
{
   public class WFSSTATUS : WFS
   {
      public string SPVersion { get; set; }
      public string EPVersion { get; set; }
      public string lpszExtra { get; set; }

      public WFSSTATUS(IContext ctx) : base(ctx)
      {
      }

      // non-destructive read of nwLogLine

      public virtual string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         result = SPVersionFromStatus(nwLogLine);
         if (result.success)
         {
            SPVersion = result.xfsMatch.Trim();
         }
         else
         {
            result = SP_VersionFromStatus(nwLogLine);
            if (result.success)
            {
               SPVersion = result.xfsMatch.Trim();
            }
         }

         result = EPVersionFromStatus(nwLogLine);
         if (result.success)
         {
            EPVersion = result.xfsMatch.Trim();
         }
         else
         {
            result = EP_VersionFromStatus(nwLogLine);
            if (result.success)
            {
               EPVersion = result.xfsMatch.Trim();
            }
         }

         result = lpszExtraFromStatus(nwLogLine);
         if (result.success)
         {
            lpszExtra = result.xfsMatch.Trim();
         }

         return nwLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) SPVersionFromStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=SPVersion=)(.*?)\\,");
      }
      public static (bool success, string xfsMatch, string subLogLine) EPVersionFromStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=EPVersion=)(.*?)\\,");
      }

      public static (bool success, string xfsMatch, string subLogLine) SP_VersionFromStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=SP_Version=)(.*?)\\,");
      }
      public static (bool success, string xfsMatch, string subLogLine) EP_VersionFromStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=EP_Version=)(.*?)\\,");
      }

      public static (bool success, string xfsMatch, string subLogLine) lpszExtraFromStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=lpszExtra = )(.*?)\r\n");
      }
   }
}
