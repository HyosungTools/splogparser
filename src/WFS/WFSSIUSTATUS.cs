using Contract;

namespace Impl
{
   public class WFSSIUSTATUS : WFS
   {
      public string fwSafeDoor { get; set; }
      public string fwDevice { get; set; }      
      public string opSwitch { get; set; }
      public string tamper { get; set; }
      public string intTamper { get; set; }
      public string cabinet { get; set; }
      public string SP_Version { get; set; }
      public string EP_Version { get; set; }
      public string errorCode { get; set; }
      public string description { get; set; }

      public WFSSIUSTATUS(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // fwSafeDoor
         result = fwSafeDoorFromSIUStatus(nwLogLine);
         if (result.success) fwSafeDoor = result.xfsMatch.Trim();

         result = fwDeviceFromSIUStatus(nwLogLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         result = opSwitchFromSIUStatus(nwLogLine);
         if (result.success) opSwitch = result.xfsMatch.Trim();

         result = tamperFromSIUStatus(nwLogLine);
         if (result.success) tamper = result.xfsMatch.Trim();

         result = intTamperFromSIUStatus(nwLogLine);
         if (result.success) intTamper = result.xfsMatch.Trim();

         result = cabinetFromSIUStatus(nwLogLine);
         if (result.success) cabinet = result.xfsMatch.Trim();

         result = SP_VersionFromSIUStatus(nwLogLine);
         if (result.success) SP_Version = result.xfsMatch.Trim();

         result = EP_VersionFromSIUStatus(nwLogLine);
         if (result.success) EP_Version = result.xfsMatch.Trim();

         result = errorCodeFromSIUStatus(nwLogLine);
         if (result.success) errorCode = result.xfsMatch.Trim();

         result = descriptionFromSIUStatus(nwLogLine);
         if (result.success) description = result.xfsMatch.Trim();


         return result.subLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) fwSafeDoorFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDoors\\[WFS_SIU_SAFE\\] = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) fwDeviceFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) opSwitchFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwSensors\\[WFS_SIU_OPERATORSWITCH\\] = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) tamperFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwSensors\\[WFS_SIU_TAMPER\\] = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) intTamperFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwSensors\\[WFS_SIU_INTTAMPER\\] = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) cabinetFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDoors\\[WFS_SIU_CABINET\\] = \\[(.*)\\]", "0");
      }

      public static (bool success, string xfsMatch, string subLogLine) SP_VersionFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=lpszExtra = \\[SP_Version=)(.*?)\\,"); 
      }

      public static (bool success, string xfsMatch, string subLogLine) EP_VersionFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=EP_Version=)(.*?)\\,");  
      }

      public static (bool success, string xfsMatch, string subLogLine) errorCodeFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ErrorCode=)(.*?)\\,");
      }

      public static (bool success, string xfsMatch, string subLogLine) descriptionFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=Description=)(.*?)\\,");
      }

   }
}

