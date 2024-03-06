using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIDCCAPABILITIES : WFSDEVSTATUS
   {
      public string fwType { get; set; }
      public string bCompound { get; set; }
      public string fwReadTracks { get; set; }
      public string fwWriteTracks { get; set; }
      public string fwChipProtocols { get; set; }
      public string usCards { get; set; }
      public string fwSecType { get; set; }
      public string fwPowerOnOption { get; set; }
      public string fwPowerOffOption { get; set; }
      public string bFluxSensorProgrammable { get; set; }
      public string bReadWriteAccessFollowingEject { get; set; }
      public string fwWriteMode { get; set; }
      public string fwChipPower { get; set; }
      public string lpszExtra2 { get; set; }
      public string fwDIPMode { get; set; }
      public string lpwMemoryChipProtocols { get; set; }
      public string fwEjectPosition { get; set; }
      public string bPowerSaveControl { get; set; }
      public string usParkingStations { get; set; }
      public string bAntiFraudModule { get; set; }


      public WFSIDCCAPABILITIES(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_IDC_CAPABILITIES) : base(parent, logLine, xfsType: xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = fwTypeFromIDCCapabilities(logLine);
         if (result.success) fwType = result.xfsMatch.Trim();

         result = bCompoundFromIDCCapabilities(logLine);
         if (result.success) bCompound = result.xfsMatch.Trim();

         result = fwReadTracksFromIDCCapabilities(logLine);
         if (result.success) fwReadTracks = result.xfsMatch.Trim();

         result = fwWriteTracksFromIDCCapabilities(logLine);
         if (result.success) fwWriteTracks = result.xfsMatch.Trim();

         result = fwChipProtocolsFromIDCCapabilities(logLine);
         if (result.success) fwChipProtocols = result.xfsMatch.Trim();

         result = usCardsFromIDCCapabilities(logLine);
         if (result.success) usCards = result.xfsMatch.Trim();

         result = fwSecTypeFromIDCCapabilities(logLine);
         if (result.success) fwSecType = result.xfsMatch.Trim();

         result = fwPowerOnOptionFromIDCCapabilities(logLine);
         if (result.success) fwPowerOnOption = result.xfsMatch.Trim();

         result = fwPowerOffOptionFromIDCCapabilities(logLine);
         if (result.success) fwPowerOffOption = result.xfsMatch.Trim();

         result = bFluxSensorProgrammableFromIDCCapabilities(logLine);
         if (result.success) bFluxSensorProgrammable = result.xfsMatch.Trim();

         result = bReadWriteAccessFollowingEjectFromIDCCapabilities(logLine);
         if (result.success) bReadWriteAccessFollowingEject = result.xfsMatch.Trim();

         result = fwWriteModeFromIDCCapabilities(logLine);
         if (result.success) fwWriteMode = result.xfsMatch.Trim();

         result = fwChipPowerFromIDCCapabilities(logLine);
         if (result.success) fwChipPower = result.xfsMatch.Trim();

         result = lpszExtra2FromIDCCapabilities(logLine);
         if (result.success) lpszExtra2 = result.xfsMatch.Trim();

         result = fwDIPModeFromIDCCapabilities(logLine);
         if (result.success) fwDIPMode = result.xfsMatch.Trim();

         result = lpwMemoryChipProtocolsFromIDCCapabilities(logLine);
         if (result.success) lpwMemoryChipProtocols = result.xfsMatch.Trim();

         result = fwEjectPositionFromIDCCapabilities(logLine);
         if (result.success) fwEjectPosition = result.xfsMatch.Trim();

         result = bPowerSaveControlFromIDCCapabilities(logLine);
         if (result.success) bPowerSaveControl = result.xfsMatch.Trim();

         result = usParkingStationsFromIDCCapabilities(logLine);
         if (result.success) usParkingStations = result.xfsMatch.Trim();

         result = bAntiFraudModuleFromIDCCapabilities(logLine);
         if (result.success) bAntiFraudModule = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwTypeFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwType = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bCompoundFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "bCompound = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwReadTracksFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwReadTracks = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwWriteTracksFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwWriteTracks = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwChipProtocolsFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwChipProtocols = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwSecTypeFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwSecType = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usCardsFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "usCards = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwPowerOnOptionFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwPowerOnOption = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwPowerOffOptionFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwPowerOffOption = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bFluxSensorProgrammableFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "bFluxSensorProgrammable = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bReadWriteAccessFollowingEjectFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "bReadWriteAccessFollowingEject = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwWriteModeFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwWriteMode = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwChipPowerFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwChipPower = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) lpszExtra2FromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "lpszExtra = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwDIPModeFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwDIPMode = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) lpwMemoryChipProtocolsFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "lpwMemoryChipProtocols = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwEjectPositionFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "fwEjectPosition = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bPowerSaveControlFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "bPowerSaveControl = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usParkingStationsFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "usParkingStations = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bAntiFraudModuleFromIDCCapabilities(string logLine)
      {
         return Util.MatchList(logLine, "bAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}


