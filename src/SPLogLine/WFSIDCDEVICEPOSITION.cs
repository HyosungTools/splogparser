using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIDCDEVICEPOSITION : WFSDEVSTATUS
   {
      public string wPosition { get; set; }

      public WFSIDCDEVICEPOSITION(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_SRVE_IDC_DEVICEPOSITION) : base(parent, logLine, xfsType: xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = wPositionFromDevicePosition(logLine);
         if (result.success) wPosition = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) wPositionFromDevicePosition(string logLine)
      {
         return Util.MatchList(logLine, "wPosition = \\[(.*)\\]", "0");
      }

   }
}

