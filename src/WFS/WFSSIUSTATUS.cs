using Contract;

namespace Impl
{
   public class WFSSIUSTATUS : WFS
   {
      public string fwSafeDoor { get; set; }

      public WFSSIUSTATUS(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // fwSafeDoor
         result = fwSafeDoorFromSIUStatus(nwLogLine);
         if (result.success) fwSafeDoor = result.xfsMatch.Trim();


         return result.subLogLine;
      }

      public static (bool success, string xfsMatch, string subLogLine) fwSafeDoorFromSIUStatus(string logLine)
      {
         return WFS.WFSMatchList(logLine, "fwDoors\\[WFS_SIU_SAFE\\] = \\[(.*)\\]", "0");
      }
   }
}

