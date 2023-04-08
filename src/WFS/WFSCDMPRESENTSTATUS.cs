using System.Collections.Generic;
using Contract;

namespace Impl
{
   public class WFSCDMPRESENTSTATUS : WFS
   {
      public WFSCDMDENOMINATION CDMDENOM { get; set; }
      public string wPresentState { get; set; }

      public WFSCDMPRESENTSTATUS(IContext ctx) : base(ctx)
      {
         CDMDENOM = null;
         wPresentState = string.Empty; 
      }
      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         CDMDENOM = new WFSCDMDENOMINATION(this.ctx);
         string subLogLine = CDMDENOM.Initialize(nwLogLine);

         result = wPresentStateFromList(subLogLine);
         wPresentState = result.xfsMatch.Trim();

         return result.subLogLine; 
      }

      // wPresentState
      public static (bool success, string xfsMatch, string subLogLine) wPresentStateFromList(string logLine)
      {
         return WFSMatch(logLine, "(?<=wPresentState = \\[)([a-zA-Z0-9 ]*)\\]");
      }
   }
}
