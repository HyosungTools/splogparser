using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
   /// <summary>
   /// Parsed lpszExtra key-value pairs from CDM STATUS.
   /// Fields may not always be present; all default to empty string.
   /// </summary>
   public class LpszExtra
   {
      public string ErrorCode { get; set; } = string.Empty;
      public string Description { get; set; } = string.Empty;
      public string ErrCode { get; set; } = string.Empty;
      public string ErrMsg { get; set; } = string.Empty;
      public string Position { get; set; } = string.Empty;
      public string SP_Version { get; set; } = string.Empty;
      public string EP_Version { get; set; } = string.Empty;

      /// <summary>
      /// Returns true if any field was populated.
      /// </summary>
      public bool HasData =>
         !string.IsNullOrEmpty(ErrorCode) ||
         !string.IsNullOrEmpty(Description) ||
         !string.IsNullOrEmpty(ErrCode) ||
         !string.IsNullOrEmpty(ErrMsg) ||
         !string.IsNullOrEmpty(Position) ||
         !string.IsNullOrEmpty(SP_Version) ||
         !string.IsNullOrEmpty(EP_Version);

      /// <summary>
      /// Parse lpszExtra from a CDM STATUS log line. Returns null if lpszExtra is not present.
      /// Format: lpszExtra = [Key1=Value1,Key2=Value2,...]
      /// Values may contain brackets, so we search for each key individually.
      /// </summary>
      public static LpszExtra Parse(string logLine)
      {
         // First check if lpszExtra is even present
         if (string.IsNullOrEmpty(logLine) || !logLine.Contains("lpszExtra"))
            return null;

         LpszExtra extra = new LpszExtra();

         extra.ErrorCode = ExtractValue(logLine, "ErrorCode");
         extra.Description = ExtractValue(logLine, "Description");
         extra.ErrCode = ExtractValue(logLine, "ErrCode");
         extra.ErrMsg = ExtractValue(logLine, "ErrMsg");
         extra.Position = ExtractValue(logLine, "Position");
         extra.SP_Version = ExtractValue(logLine, "SP_Version");
         extra.EP_Version = ExtractValue(logLine, "EP_Version");

         return extra.HasData ? extra : null;
      }

      /// <summary>
      /// Extract a value for a given key from the lpszExtra content.
      /// Matches KeyName= followed by everything up to the next ,KnownKey= or end of lpszExtra (]).
      /// </summary>
      private static string ExtractValue(string logLine, string key)
      {
         // Match key=value where value ends at the next ,Word= or ]
         string pattern = key + @"=(.+?)(?=,\w+=|\]\s*,|\]$|\]\s)";
         Match match = Regex.Match(logLine, pattern);
         return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
      }
   }

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

      public LpszExtra lpszExtra { get; set; }

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

         // lpszExtra (may not be present)
         lpszExtra = LpszExtra.Parse(logLine);

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

