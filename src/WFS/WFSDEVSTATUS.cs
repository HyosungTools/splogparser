using Contract;

namespace Impl
{
   public class WFSDEVSTATUS : WFS
   {
      public string fwDevice { get; set; }

      public WFSDEVSTATUS(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = fwDeviceFromCIMStatus(nwLogLine);
         if (result.success) fwDevice = result.xfsMatch.Trim();

         return result.subLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) fwDeviceFromCIMStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }

   }
}

