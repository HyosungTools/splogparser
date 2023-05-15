using System.Collections.Generic;
using Contract;

namespace Impl
{
   public class WFPOPEN : WFS
   {
      public string hService;
      public string lpszLogicalName;
      public string lpszAppID;

      public WFPOPEN(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         result = hServiceFromList(nwLogLine);
         hService = result.xfsMatch;

         result = lpszLogicalNameFromList(result.subLogLine);
         lpszLogicalName = result.xfsMatch; 

         result = lpszAppIDFromList(result.subLogLine);
         lpszAppID = result.xfsMatch;

         return result.subLogLine; 
      }


      // hService
      public static (bool success, string xfsMatch, string subLogLine) hServiceFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=hService = \\[)(\\d+)");
      }

      // lpszLogicalName
      public static (bool success, string xfsMatch, string subLogLine) lpszLogicalNameFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=lpszLogicalName = \\[)(\\w+)");
      }

      // lpszAppID
      public static (bool success, string xfsMatch, string subLogLine) lpszAppIDFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=lpszAppID = \\[)([A-Za-z0-9. ]+)");
      }

      public static string device(string logLine)
      {
         (bool success, string xfsMatch, string subLogLine) result = WFPOPEN.lpszLogicalNameFromList(logLine);

         switch (result.xfsMatch)
         {
            case "ReceiptPrinter":  return "PTR";
            case "CardReader":      return "IDC";
            case "CashDispenser":   return "CDM";
            case "Encryptor":       return "PIN";
            case "CheckModule":     return "CHK";
            case "Deposit":         return "DEP";
            case "TextTerminal":    return "TTU";
            case "Sensors":         return "SIU";
            case "VendorDependentMode": return "VDM";
            case "Camera":          return "CAM";
            case "Alarm":           return "ALM";
            case "CashDeposit":     return "CIM";
            case "BarCodeReader":   return "BCR";
            case "ItemProcessor":   return "IPM";
            default:                return string.Empty;
         }
      }
   }
}
