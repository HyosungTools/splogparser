using System.Collections.Generic;
using Contract;

namespace Impl
{
   public class WFPCLOSE : WFS
   {
      public string hService;

      public WFPCLOSE(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         result = hServiceFromList(nwLogLine);
         hService = result.xfsMatch;

         return result.subLogLine;
      }

      // hService
      public static (bool success, string xfsMatch, string subLogLine) hServiceFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=hService = \\[)(\\d+)");
      }
   }
}
