using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIDCMEDIADETECTED : WFSDEVSTATUS
   {
      public string lppwResetOut { get; set; }

      public WFSIDCMEDIADETECTED(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_SRVE_IDC_MEDIADETECTED) : base(parent, logLine, xfsType: xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = lppwResetOutFromMediaDetected(logLine);
         if (result.success) lppwResetOut = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) lppwResetOutFromMediaDetected(string logLine)
      {
         return Util.MatchList(logLine, "lppwResetOut = \\[(.*)\\]", "0");
      }

   }
}

