using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSPTRSTATUS : WFSSTATUS
   {
      public string fwDevice { get; set; }
      public string fwMedia { get; set; }
      public string fwPaper_SUPPLYUPPER { get; set; }
      public string fwPaper_SUPPLYLOWER { get; set; }
      public string fwPaper_SUPPLYEXT { get; set; }
      public string fwPaper_SUPPLYAUX { get; set; }
      public string fwPaper_SUPPLYAUX2 { get; set; }
      public string fwPaper_SUPPLYPARK { get; set; }
      public string fwToner { get; set; }
      public string fwInk { get; set; }
      public string fwLamp { get; set; }
      //public string llpRetractBins { get; set; }
      public string usMediaOnStacker { get; set; }

      public string wDevicePosition { get; set; }
      public string usPowerSaveRecoveryTime { get; set; }
      public string wAntiFraudModule { get; set; }

      public WFSPTRSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_PIN_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = fwDeviceFromPTRStatus(logLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         // fwMedia
         result = fwMediaFromPTRStatus(logLine);
         if (result.success) fwMedia = result.xfsMatch.Trim();

         // fwPaper_SUPPLYUPPER
         result = fwPaper_SUPPLYUPPERFromPTRStatus(result.subLogLine);
         if (result.success) fwPaper_SUPPLYUPPER = result.xfsMatch.Trim();

         // fwIntermediateStacker
         result = fwPaper_SUPPLYLOWERFromPTRStatus(result.subLogLine);
         if (result.success) fwPaper_SUPPLYLOWER = result.xfsMatch.Trim();

         // fwPaper_SUPPLYEXT
         result = fwPaper_SUPPLYEXTFromPTRStatus(result.subLogLine);
         if (result.success) fwPaper_SUPPLYEXT = result.xfsMatch;

         // fwPaper_SUPPLYAUX 
         result = fwPaper_SUPPLYAUXFromPTRStatus(result.subLogLine);
         if (result.success) fwPaper_SUPPLYAUX = result.xfsMatch.Trim();

         // fwPaper_SUPPLYAUX2 
         result = fwPaper_SUPPLYAUX2FromPTRStatus(result.subLogLine);
         if (result.success) fwPaper_SUPPLYAUX2 = result.xfsMatch.Trim();

         // fwPaper_SUPPLYPARK
         result = fwPaper_SUPPLYPARKFromPTRStatus(result.subLogLine);
         if (result.success) fwPaper_SUPPLYPARK = result.xfsMatch.Trim();

         // fwToner
         result = fwTonerFromPTRStatus(result.subLogLine);
         if (result.success) fwToner = result.xfsMatch.Trim();

         // fwInk
         result = fwInkFromPTRStatus(result.subLogLine);
         if (result.success) fwInk = result.xfsMatch.Trim();

         // fwLamp
         result = fwLampFromPTRStatus(result.subLogLine);
         if (result.success) fwLamp = result.xfsMatch.Trim();

         // usMediaOnStacker
         result = usMediaOnStackerFromPTRStatus(result.subLogLine);
         if (result.success) usMediaOnStacker = result.xfsMatch.Trim();

         // wDevicePosition
         result = wDevicePositionFromPTRStatus(result.subLogLine);
         if (result.success) usMediaOnStacker = result.xfsMatch.Trim();

         // usPowerSaveRecoveryTime
         result = usPowerSaveRecoveryTimeFromPTRStatus(result.subLogLine);
         if (result.success) usPowerSaveRecoveryTime = result.xfsMatch.Trim();

         // wAntiFraudModule
         result = wAntiFraudModuleFromPTRStatus(result.subLogLine);
         if (result.success) wAntiFraudModule = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwDeviceFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwMediaFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwMedia = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwPaper_SUPPLYUPPERFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPaper\\[WFS_PTR_SUPPLYUPPER\\] = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwPaper_SUPPLYLOWERFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPaper\\[WFS_PTR_SUPPLYLOWER\\] = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwPaper_SUPPLYEXTFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPaper\\[WFS_PTR_SUPPLYEXTERNAL\\] = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwPaper_SUPPLYAUXFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPaper\\[WFS_PTR_SUPPLYAUX\\] = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwPaper_SUPPLYAUX2FromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPaper\\[WFS_PTR_SUPPLYAUX2\\] = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwPaper_SUPPLYPARKFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPaper\\[WFS_PTR_SUPPLYPARK\\] = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwTonerFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwToner = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwInkFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwInk = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwLampFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwLamp = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) usMediaOnStackerFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "usMediaOnStacker = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wDevicePositionFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTimeFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromPTRStatus(string logLine)
      {
         return Util.MatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}

