using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMMEDIAREJECTED : WFS
   {
      public string wReason { get; set; }

      private const string prefix = "30"; 

      public WFSIPMMEDIAREJECTED(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // wReason
         result = wReasonFromList(nwLogLine);
         if (result.success) wReason = prefix + result.xfsMatch.Trim();

         return result.subLogLine;
      }


      // I N D I V I D U A L    A C C E S S O R S

      // wReason  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) wReasonFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wReason = \\[)(\\d+)");
      }
   }
}
