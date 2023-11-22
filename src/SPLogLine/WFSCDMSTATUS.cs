using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCDMSTATUS : WFSDEVSTATUS
   {
      //public string fwDevice { get; set; }
      public string fwSafeDoor { get; set; }
      public string fwDispenser { get; set; }
      public string fwIntStacker { get; set; }
      // lppPositions
      public string fwPosition { get; set; }
      public string fwShutter { get; set; }
      public string fwPositionStatus { get; set; }
      public string fwTransport { get; set; }
      public string fwTransportStatus { get; set; }

      public string wDevicePosition { get; set; }
      public string usPowerSaveRecoveryTime { get; set; }
      public string wAntiFraudModule { get; set; }

      public WFSCDMSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CDM_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         //// fwDevice
         //result = fwDeviceFromCDMStatus(logLine);
         //if (result.success) fwDevice = result.xfsMatch.Trim();

         // fwSafeDoor
         result = fwSafeDoorFromCDMStatus(logLine);
         if (result.success) fwSafeDoor = result.xfsMatch.Trim();

         // fwDispenser
         result = fwDispenserFromSDMStatus(result.subLogLine);
         if (result.success) fwDispenser = result.xfsMatch.Trim();

         // fwIntermediateStacker
         result = fwIntermediateStackerFromCDMStatus(result.subLogLine);
         if (result.success) fwIntStacker = result.xfsMatch.Trim();

         // fwPosition
         result = fwPositionFromCDMStatus(result.subLogLine);
         if (result.success) fwPosition = result.xfsMatch;

         // fwShutter
         result = fwShutterFromSDMStatus(result.subLogLine);
         if (result.success) fwShutter = result.xfsMatch.Trim();

         // fwPositionStatus
         result = fwPositionStatusFromCDMStatus(result.subLogLine);
         if (result.success) fwPositionStatus = result.xfsMatch;

         // fwTransport
         result = fwTransportFromCDMStatus(result.subLogLine);
         if (result.success) fwTransport = result.xfsMatch.Trim();

         // fwTransportStatus
         result = fwTransportStatusFromCDMStatus(result.subLogLine);
         if (result.success) fwTransportStatus = result.xfsMatch.Trim();

         // wDevicePosition
         result = wDevicePositionFromCDMStatus(result.subLogLine);
         if (result.success) wDevicePosition = result.xfsMatch;

         // usPowerSaveRecoveryTime
         result = usPowerSaveRecoveryTimeFromCDMStatus(result.subLogLine);
         if (result.success) usPowerSaveRecoveryTime = result.xfsMatch;

         // wAntiFraudModule
         result = wAntiFraudModuleFromCDMStatus(result.subLogLine);
         if (result.success) wAntiFraudModule = result.xfsMatch;
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwDeviceFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwSafeDoorFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwSafeDoor = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwDispenserFromSDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDispenser = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwIntermediateStackerFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwIntermediateStacker = \\[(.*)\\]", "0");
      }
      // report on the first (output ?) position only. 
      protected static (bool success, string xfsMatch, string subLogLine) fwPositionFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPosition = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwShutterFromSDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwShutter = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwPositionStatusFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwPositionStatus = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwTransportFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwTransport = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwTransportStatusFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwTransportStatus = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wDevicePositionFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTimeFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromCDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}

