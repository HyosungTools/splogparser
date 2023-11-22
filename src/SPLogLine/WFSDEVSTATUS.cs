using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSDEVSTATUS : WFSSTATUS
   {
      public string fwDevice { get; set; }

      public WFSDEVSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.None) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = fwDeviceFromDEVStatus(logLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwDeviceFromDEVStatus(string logLine)
      {
         return Util.MatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }

   }
}

