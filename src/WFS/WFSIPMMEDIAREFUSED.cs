using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMMEDIAREFUSED : WFS
   {
      public string wReason { get; set; }
      public string wMediaLocation { get; set; }
      public string bPresentRequired { get; set; }

      private const string prefix = "20";

      public WFSIPMMEDIAREFUSED(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // wReason
         result = wReasonFromList(nwLogLine);
         if (result.success) wReason = prefix + result.xfsMatch.Trim();

         // wMediaLocation
         result = wMediaLocationFromList(nwLogLine);
         if (result.success) wMediaLocation = result.xfsMatch.Trim();

         // bPresentRequired
         result = bPresentRequiredFromList(result.subLogLine);
         if (result.success) bPresentRequired = result.xfsMatch.Trim();

         return result.subLogLine;
      }


      // I N D I V I D U A L    A C C E S S O R S


      // wReason  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) wReasonFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wReason = \\[)(\\d+)");
      }

      // wMediaLocation
      public static (bool success, string xfsMatch, string subLogLine) wMediaLocationFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wMediaLocation = \\[)(\\d+)");
      }

      // bPresentRequired
      public static (bool success, string xfsMatch, string subLogLine) bPresentRequiredFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=bPresentRequired = \\[)(\\d+)");
      }
   }
}
