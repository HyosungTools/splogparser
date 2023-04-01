using Contract;

namespace Impl
{
   public class WFSCDMSTATUS : WFS
   {
      public string fwDevice { get; set; }
      public string fwSafeDoor { get; set; }
      public string fwDispenser { get; set; }
      public string fwIntStacker { get; set; }
      // lppPositions
      public string fwPosition { get; set; }
      public string fwShutter { get; set; }
      public string fwPositionStatus { get; set; }
      public string fwTransport { get; set; }
      public string fwTransportStatus { get; set; }

      //public string lpszExtra { get; set; }
      //public string[] dwGuideLights { get; set; }

      public string wDevicePosition { get; set; }
      public string usPowerSaveRecoveryTime { get; set; }
      public string wAntiFraudModule { get; set; }

      public WFSCDMSTATUS(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = fwDeviceFromCDMStatus(nwLogLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         // fwSafeDoor
         result = fwSafeDoorFromCDMStatus(nwLogLine);
         if (result.success) fwSafeDoor = result.xfsMatch.Trim();

         // fwDispenser
         result = fwDispenserFromSDMStatus(result.subLogLine);
         if (result.success) fwDispenser = result.xfsMatch.Trim();

         // fwIntermediateStacker
         result = fwIntermediateStackerFromCDMStatus(result.subLogLine);
         if (result.success) fwIntStacker = result.xfsMatch.Trim();

         // fwPositionStatus
         result = fwPositionStatusFromCDMStatus(result.subLogLine);
         if (result.success) fwPositionStatus = result.xfsMatch;

         // fwShutter
         result = fwShutterFromSDMStatus(result.subLogLine);
         if (result.success) fwShutter = result.xfsMatch.Trim();



         // fwTransport
         result = fwTransportFromCDMStatus(result.subLogLine);
         if (result.success) fwTransport = result.xfsMatch.Trim();

         // fwTransportStatus
         result = fwTransportStatusFromCDMStatus(result.subLogLine);
         if (result.success) fwTransportStatus = result.xfsMatch.Trim();

         // wDevicePosition
         result = wDevicePositionFromCDMStatus(result.subLogLine);
         if (result.success) wDevicePosition = result.xfsMatch.Trim();

         return result.subLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) fwDeviceFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwSafeDoorFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwSafeDoor = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwDispenserFromSDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDispenser = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwIntermediateStackerFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwIntermediateStacker = \\[(.*)\\]", "0");
      }
      // report on the first (output ?) position only. 
      public static (bool success, string xfsMatch, string subLogLine) fwPositionFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwPosition = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwShutterFromSDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwShutter = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwPositionStatusFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwPositionStatus = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwTransportFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwTransport = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwTransportStatusFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwTransportStatus = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wDevicePositionFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTimeFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromCDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}

