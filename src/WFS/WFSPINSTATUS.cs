﻿using System;
using Contract;

namespace Impl
{
   public class WFSPINSTATUS : WFS
   {
      public string fwDevice { get; set; }
      public string fwEncStat { get; set; }
      //public string[] dwGuidLights { get; set; }
      public string fwAutoBeepMode { get; set; }      
      public string dwCertificateState { get; set; }
      public string wDevicePosition { get; set; }
      public string usPowerSaveRecoveryTime { get; set; }
      public string wAntiFraudModule { get; set; }
      public string SP_Version { get; set; }
      public string EP_Version { get; set; }
      public string ErrorCode { get; set; }
      public WFSPINSTATUS(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;
         //(bool success, string[] xfsMatch, string subLogLine) results1D;

         // fwDevice
         result = fwDeviceFromPINStatus(nwLogLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         // fwEncStat
         result = fwEncStatFromPINStatus(nwLogLine);
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

         result = SP_VersionFromPINStatus(nwLogLine);
         if (result.success) SP_Version = result.xfsMatch.Trim();

         result = EP_VersionFromPINStatus(nwLogLine);
         if (result.success) EP_Version = result.xfsMatch.Trim();

         result = errorCodeFromPINStatus(nwLogLine);
         if (result.success) ErrorCode = result.xfsMatch.Trim();

         return result.subLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) fwDeviceFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwEncStatFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwEncStat = \\[(.*)\\]", "0");
      }     
      public static (bool success, string xfsMatch, string subLogLine) fwAutoBeepModeFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwAutoBeepMode = \\[(.*)\\]", "0");
      }      
      public static (bool success, string xfsMatch, string subLogLine) dwCertificateStateFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "dwCertificateState = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wDevicePositionFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTimeFromSDMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wAntiFraudModuleFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) SP_VersionFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=SP_Version=)(.*?)\\,");
      }
      public static (bool success, string xfsMatch, string subLogLine) EP_VersionFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=EP_Version=)(.*?)\\,");
      }
      public static (bool success, string xfsMatch, string subLogLine) errorCodeFromPINStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ErrorCode=)(.*?)\\,");
      }
   }
}
