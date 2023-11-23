using System;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSPINSTATUS : WFSDEVSTATUS
   {
      //public string fwDevice { get; set; }
      public string fwEncStat { get; set; }
      //public string[] dwGuidLights { get; set; }
      public string fwAutoBeepMode { get; set; }      
      public string dwCertificateState { get; set; }
      public string wDevicePosition { get; set; }
      public string usPowerSaveRecoveryTime { get; set; }
      public string wAntiFraudModule { get; set; }
      public string ErrorCode { get; set; }

      public WFSPINSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_PIN_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         //// fwDevice
         //result = fwDeviceFromPINStatus(logLine);
         //if (result.success) fwDevice = result.xfsMatch.Trim();

         // fwEncStat
         result = fwEncStatFromPINStatus(logLine);
         if (result.success) fwEncStat = result.xfsMatch.Trim();
                  
         // fwAutoBeepMode
         result = fwAutoBeepModeFromPINStatus(result.subLogLine);
         if (result.success) fwAutoBeepMode = result.xfsMatch.Trim();

         // dwCertificateState
         result = dwCertificateStateFromPINStatus(result.subLogLine);
         if (result.success) dwCertificateState = result.xfsMatch.Trim();

         // wDevicePosition
         result = wDevicePositionFromPINStatus(result.subLogLine);
         if (result.success) wDevicePosition = result.xfsMatch;

         // usPowerSaveRecoveryTime
         result = usPowerSaveRecoveryTimeFromSDMStatus(result.subLogLine);
         if (result.success) usPowerSaveRecoveryTime = result.xfsMatch.Trim();

         // wAntiFraudModule
         result = wAntiFraudModuleFromPINStatus(result.subLogLine);
         if (result.success) wAntiFraudModule = result.xfsMatch.Trim();

         result = errorCodeFromPINStatus(logLine);
         if (result.success) ErrorCode = result.xfsMatch.Trim();
      }

      //protected static (bool success, string xfsMatch, string subLogLine) fwDeviceFromPINStatus(string logLine)
      //{
      //   return Util.MatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      //}
      protected static (bool success, string xfsMatch, string subLogLine) fwEncStatFromPINStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwEncStat = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) fwAutoBeepModeFromPINStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwAutoBeepMode = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) dwCertificateStateFromPINStatus(string logLine)
      {
         return Util.MatchList(logLine, "dwCertificateState = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wDevicePositionFromPINStatus(string logLine)
      {
         return Util.MatchList(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTimeFromSDMStatus(string logLine)
      {
         return Util.MatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromPINStatus(string logLine)
      {
         return Util.MatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) errorCodeFromPINStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ErrorCode=)(.*?)\\,");
      }
   }
}

