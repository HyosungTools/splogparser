using System.Collections.Generic;
using Contract;

namespace Impl
{

   public class WFSIPMMEDIAPRESENTED : WFS
   {
      public string wPosition { get; set; }
      public string usBunchIndex { get; set; }
      public string usTotalBunches { get; set; }


      public WFSIPMMEDIAPRESENTED(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // wPosition
         result = wPositionFromList(nwLogLine);
         if (result.success) wPosition = result.xfsMatch.Trim();

         // usBunchIndex
         result = usBunchIndexFromList(nwLogLine);
         if (result.success) usBunchIndex = result.xfsMatch.Trim();

         // usTotalBunches
         result = usTotalBunchesFromList(result.subLogLine);
         if (result.success) usTotalBunches = result.xfsMatch.Trim();

         return result.subLogLine;
      }

      // I N D I V I D U A L    A C C E S S O R S

      // wPosition
      public static (bool success, string xfsMatch, string subLogLine) wPositionFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wPosition = \\[)(\\d+)");
      }

      // usBunchIndex
      public static (bool success, string xfsMatch, string subLogLine) usBunchIndexFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usBunchIndex = \\[)(\\d+)");
      }

      // usTotalBunches
      public static (bool success, string xfsMatch, string subLogLine) usTotalBunchesFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usTotalBunches = \\[)(\\d+)");
      }
   }
}
