using Contract;

namespace Impl
{
   public class WFSIPMSTATUS : WFSSTATUS
   {
      public string fwDevice { get; set; }
      public string wAcceptor { get; set; }
      public string wMedia { get; set; }
      public string wStacker { get; set; }
      public string wMixedMode { get; set; }
      public string wAntiFraudModule { get; set; }


      public WFSIPMSTATUS(IContext ctx) : base(ctx)
      {
      }

      public override string Initialize(string nwLogLine)
      {
         base.Initialize(nwLogLine);

         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = fwDeviceFromIPMStatus(nwLogLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         // wAcceptor
         result = wAcceptorFromIPMStatus(nwLogLine);
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

         return result.subLogLine;
      }


      public static (bool success, string xfsMatch, string subLogLine) fwDeviceFromIPMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wAcceptorFromIPMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wAcceptor = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wMediaFromIPMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wMedia = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wStackerFromIPMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wStacker = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wMixedModeFromIPMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wMixedMode = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromIPMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}

