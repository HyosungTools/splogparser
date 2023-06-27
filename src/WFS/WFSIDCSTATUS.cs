using Contract;

namespace Impl
{
   public class WFSIDCSTATUS : WFS
   {
      public string fwDevice { get; set; }
      public string fwMedia { get; set; }
      public string fwRetainBin { get; set; }
      public string fwSecurity { get; set; }
      public string usCards { get; set; }
      public string fwChipPower { get; set; }
      public string fwChipModule { get; set; }
      public string fwMagReadModule { get; set; }
      public string SPVersion { get; set; }
      public string EPVersion { get; set; }
      public string ErrorCode { get; set; }
      public string Description { get; set; }

      public WFSIDCSTATUS(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;
                 
         result = fwDeviceFromIDCStatus(nwLogLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         result = fwMediaFromIDCStatus(nwLogLine);
         if (result.success) fwMedia = result.xfsMatch.Trim();

         result = fwRetainBinFromIDCStatus(nwLogLine);
         if (result.success) fwRetainBin = result.xfsMatch.Trim();

         result = fwSecurityFromIDCStatus(nwLogLine);
         if (result.success) fwSecurity = result.xfsMatch.Trim();

         result = usCardsFromIDCStatus(nwLogLine);
         if (result.success) usCards = result.xfsMatch.Trim();

         result = fwChipPowerFromIDCStatus(nwLogLine);
         if (result.success) fwChipPower = result.xfsMatch.Trim();

         result = fwChipModuleFromIDCStatus(nwLogLine);
         if (result.success) fwChipModule = result.xfsMatch.Trim();

         result = fwMagReadModuleFromIDCStatus(nwLogLine);
         if (result.success) fwMagReadModule = result.xfsMatch.Trim();

         result = SPVersionFromIDCStatus(nwLogLine);
         if (result.success) SPVersion = result.xfsMatch.Trim();

         result = EPVersionFromIDCStatus(nwLogLine);
         if (result.success) EPVersion = result.xfsMatch.Trim();

         result = errorCodeFromIDCStatus(nwLogLine);
         if (result.success) ErrorCode = result.xfsMatch.Trim();
         
         return result.subLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) fwDeviceFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) fwMediaFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwMedia = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) fwRetainBinFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwRetainBin = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) fwSecurityFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwSecurity = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) usCardsFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "usCards = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) fwChipPowerFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwChipPower = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) fwChipModuleFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwChipModule = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) fwMagReadModuleFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwMagReadModule = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) SPVersionFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=SPVersion=)(.*?)\\,");         
      }

      public static (bool success, string xfsMatch, string subLogLine) EPVersionFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=EPVersion=)(.*?)\\,");
      }

      public static (bool success, string xfsMatch, string subLogLine) errorCodeFromIDCStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ErrorCode=)(.*?)\\,");
      }
   }
}

