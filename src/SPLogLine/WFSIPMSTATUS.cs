using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIPMSTATUS : WFSSTATUS
   {
      public string fwDevice { get; set; }
      public string wAcceptor { get; set; }
      public string wMedia { get; set; }
      public string wStacker { get; set; }
      public string wMixedMode { get; set; }
      public string wAntiFraudModule { get; set; }

      public WFSIPMSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_IPM_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = fwDeviceFromIPMStatus(logLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         // wAcceptor
         result = wAcceptorFromIPMStatus(logLine);
         if (result.success) wAcceptor = result.xfsMatch.Trim();

         // wMedia
         result = wMediaFromIPMStatus(result.subLogLine);
         if (result.success) wMedia = result.xfsMatch.Trim();

         // wStacker
         result = wStackerFromIPMStatus(result.subLogLine);
         if (result.success) wStacker = result.xfsMatch.Trim();

         // wMixedMode
         result = wMixedModeFromIPMStatus(result.subLogLine);
         if (result.success) wMixedMode = result.xfsMatch;

         // wAntiFraudModule
         result = wAntiFraudModuleFromIPMStatus(result.subLogLine);
         if (result.success) wAntiFraudModule = result.xfsMatch;
      }


      protected static (bool success, string xfsMatch, string subLogLine) fwDeviceFromIPMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wAcceptorFromIPMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wAcceptor = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wMediaFromIPMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wMedia = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wStackerFromIPMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wStacker = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wMixedModeFromIPMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wMixedMode = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromIPMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}

