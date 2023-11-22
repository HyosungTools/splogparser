using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCIMSTATUS : WFSSTATUS
   {
      public string fwDevice { get; set; }
      public string fwSafeDoor { get; set; }
      public string fwAcceptor { get; set; }
      public string fwIntStacker { get; set; }
      public string fwStackerItems { get; set; }
      //public string fwBanknoteReader{ get; set; }
      public string bDropBox { get; set; }
      public string[,] positions { get; set; }
      public string[] dwGuideLights { get; set; }
      public string wDevicePosition { get; set; }
      public string usPowerSaveRecoveryTime { get; set; }
      public string wMixedMode { get; set; }
      public string wAntiFraudModule { get; set; }

      public WFSCIMSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CIM_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;
         (bool success, string[] xfsMatch, string subLogLine) results1D;
         (bool success, string[,] xfsMatch, string subLogLine) results2D;

         // fwDevice
         result = fwDeviceFromCIMStatus(logLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         // fwSafeDoor
         result = fwSafeDoorFromCIMStatus(logLine);
         if (result.success) fwSafeDoor = result.xfsMatch.Trim();

         // fwAcceptor
         result = fwAcceptorFromCIMStatus(result.subLogLine);
         if (result.success) fwAcceptor = result.xfsMatch.Trim();

         // fwIntermediateStacker
         result = fwIntermediateStackerFromCIMStatus(result.subLogLine);
         if (result.success) fwIntStacker = result.xfsMatch.Trim();

         // fwStackerItems
         result = fwStackerItemsFromCIMStatus(result.subLogLine);
         if (result.success) fwStackerItems = result.xfsMatch.Trim();

         // fwBanknoteReader
         //result = fwBanknoteReaderFromCIMStatus(result.subLogLine);
         //if (result.success) fwBanknoteReader = result.xfsMatch.Trim();

         // bDropBox
         result = bDropBoxFromCIMStatus(result.subLogLine);
         if (result.success) bDropBox = result.xfsMatch.Trim();

         // positions
         results2D = positionsFromCIMStatus(result.subLogLine);
         if (result.success) positions = results2D.xfsMatch;

         result = lpszExtraFromCIMStatusFromCIMStatus(result.subLogLine);
         if (result.success) lpszExtra = result.xfsMatch.Trim();

         //// dwGuidLights
         results1D = dwGuidLightsFromCIMStatus(result.subLogLine);
         if (result.success) dwGuideLights = results1D.xfsMatch;

         // wDevicePosition
         result = wDevicePositionFromCIMStatus(result.subLogLine);
         if (result.success) wDevicePosition = result.xfsMatch;

         // usPowerSaveRecoveryTime
         result = usPowerSaveRecoveryTimeFromCIMStatus(result.subLogLine);
         if (result.success) usPowerSaveRecoveryTime = result.xfsMatch;

         // wMixedMode
         result = wMixedModeFromCIMStatus(result.subLogLine);
         if (result.success) wMixedMode = result.xfsMatch;

         // wAntiFraudModule
         result = wAntiFraudModuleFromCIMStatus(result.subLogLine);
         if (result.success) wAntiFraudModule = result.xfsMatch;
      }


      protected static (bool success, string xfsMatch, string subLogLine) fwDeviceFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwSafeDoorFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwSafeDoor = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwAcceptorFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwAcceptor = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwIntermediateStackerFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwIntermediateStacker = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwStackerItemsFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwStackerItems = \\[(.*)\\]", "0");
      }
      //public static (bool success, string xfsMatch, string subLogLine) fwBanknoteReaderFromCIMStatus(string logLine)
      //{
      //   return Util.MatchList(logLine, "fwBanknoteReader = \\[(.*)\\]", "0");
      //}
      protected static (bool success, string xfsMatch, string subLogLine) bDropBoxFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "bDropBox = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwTransportStatusFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwTransportStatus = \\[(.*)\\]", "0");
      }
      protected static (bool success, string[,] xfsMatch, string subLogLine) positionsFromCIMStatus(string logLine)
      {
         // TODO 
         return (false, new string[0, 0], logLine);
      }
      protected static (bool success, string xfsMatch, string subLogLine) lpszExtraFromCIMStatusFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "lpszExtra = \\[(.*)\\]", "0");
      }
      protected static (bool success, string[] xfsMatch, string subLogLine) dwGuidLightsFromCIMStatus(string logLine)
      {
         // TODO 
         return (false, new string[0], logLine);
      }
      protected static (bool success, string xfsMatch, string subLogLine) wDevicePositionFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTimeFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wMixedModeFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wMixedMode = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromCIMStatus(string logLine)
      {
         return Util.MatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}

