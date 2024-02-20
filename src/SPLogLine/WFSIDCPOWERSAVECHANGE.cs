using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIDCPOWERSAVECHANGE : WFSDEVSTATUS
   {
      public string usPowerSaveRecoveryTime { get; set; }

      public WFSIDCPOWERSAVECHANGE(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_SRVE_IDC_POWER_SAVE_CHANGE) : base(parent, logLine, xfsType: xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = usPowerSaveRecoveryTimeFromPowerSaveChange(logLine);
         if (result.success) usPowerSaveRecoveryTime = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTimeFromPowerSaveChange(string logLine)
      {
         return Util.MatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }

   }
}

