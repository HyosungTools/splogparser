using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSSIUSTATUS : WFSDEVSTATUS
   {
      public string fwSafeDoor { get; set; }      
      public string opSwitch { get; set; }
      public string tamper { get; set; }
      public string intTamper { get; set; }
      public string cabinet { get; set; }
      public string ups { get; set; }
      public string errorCode { get; set; }
      public string description { get; set; }

      public WFSSIUSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_SIU_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // fwSafeDoor
         result = fwSafeDoorFromSIUStatus(logLine);
         if (result.success) fwSafeDoor = result.xfsMatch.Trim();

         result = opSwitchFromSIUStatus(logLine);
         if (result.success) opSwitch = result.xfsMatch.Trim();

         result = tamperFromSIUStatus(logLine);
         if (result.success) tamper = result.xfsMatch.Trim();

         result = intTamperFromSIUStatus(logLine);
         if (result.success) intTamper = result.xfsMatch.Trim();

         result = cabinetFromSIUStatus(logLine);
         if (result.success) cabinet = result.xfsMatch.Trim();

         result = upsFromSIUStatus(logLine);
         if (result.success) ups = result.xfsMatch.Trim();

         result = errorCodeFromSIUStatus(logLine);
         if (result.success) errorCode = result.xfsMatch.Trim();

         result = descriptionFromSIUStatus(logLine);
         if (result.success) description = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwSafeDoorFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDoors\\[WFS_SIU_SAFE\\] = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) opSwitchFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwSensors\\[WFS_SIU_OPERATORSWITCH\\] = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) tamperFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwSensors\\[WFS_SIU_TAMPER\\] = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) intTamperFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwSensors\\[WFS_SIU_INTTAMPER\\] = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) cabinetFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDoors\\[WFS_SIU_CABINET\\] = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) upsFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwAuxiliaries\\[WFS_SIU_UPS\\] = \\[(.*)\\]", "0");
      }

      protected static (bool success, string xfsMatch, string subLogLine) errorCodeFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ErrorCode=)(.*?)\\,");
      }

      protected static (bool success, string xfsMatch, string subLogLine) descriptionFromSIUStatus(string logLine)
      {
         return Util.MatchList(logLine, "(?<=Description=)(.*?)\\,");
      }

   }
}

