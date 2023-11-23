using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIDCSTATUS : WFSDEVSTATUS
   {
      public string fwMedia { get; set; }
      public string fwRetainBin { get; set; }
      public string fwSecurity { get; set; }
      public string usCards { get; set; }
      public string fwChipPower { get; set; }
      public string fwChipModule { get; set; }
      public string fwMagReadModule { get; set; }
      public string ErrorCode { get; set; }
      public string Description { get; set; }

      public WFSIDCSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_IDC_STATUS) : base(parent, logLine, xfsType: xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;
                 
         //result = fwDeviceFromIDCStatus(logLine);
         //if (result.success) fwDevice = result.xfsMatch.Trim();

         result = fwMediaFromIDCStatus(logLine);
         if (result.success) fwMedia = result.xfsMatch.Trim();

         result = fwRetainBinFromIDCStatus(logLine);
         if (result.success) fwRetainBin = result.xfsMatch.Trim();

         result = fwSecurityFromIDCStatus(logLine);
         if (result.success) fwSecurity = result.xfsMatch.Trim();

         result = usCardsFromIDCStatus(logLine);
         if (result.success) usCards = result.xfsMatch.Trim();

         result = fwChipPowerFromIDCStatus(logLine);
         if (result.success) fwChipPower = result.xfsMatch.Trim();

         result = fwChipModuleFromIDCStatus(logLine);
         if (result.success) fwChipModule = result.xfsMatch.Trim();

         result = fwMagReadModuleFromIDCStatus(logLine);
         if (result.success) fwMagReadModule = result.xfsMatch.Trim();

         result = errorCodeFromIDCStatus(logLine);
         if (result.success) ErrorCode = result.xfsMatch.Trim();

         result = DescriptionFromIDCStatus(logLine);
         if (result.success) Description = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwDeviceFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwMediaFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwMedia = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwRetainBinFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwRetainBin = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwSecurityFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwSecurity = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usCardsFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "usCards = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwChipPowerFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwChipPower = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwChipModuleFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwChipModule = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwMagReadModuleFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwMagReadModule = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) errorCodeFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ErrorCode=)(.*?)\\,");
      }

      protected static (bool success, string xfsMatch, string subLogLine) DescriptionFromIDCStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=Description=)(.*?)\\]");
      }
   }
}

