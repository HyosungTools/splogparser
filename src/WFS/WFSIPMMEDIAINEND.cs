using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //

   public class WFSIPMMEDIAINEND : WFS
   {
      public string usItemsReturned { get; set; }
      public string usItemsRefused { get; set; }
      public string usBunchesRefused { get; set; }


      public WFSIPMMEDIAINEND(IContext ctx) : base(ctx)
      {

      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // usItemsReturned
         result = usItemsReturnedFromList(nwLogLine);
         if (result.success) usItemsReturned = result.xfsMatch.Trim();

         // usItemsRefused
         result = usItemsRefusedFromList(nwLogLine);
         if (result.success) usItemsRefused = result.xfsMatch.Trim();

         // usBunchesRefused
         result = usBunchesRefusedFromList(result.subLogLine);
         if (result.success) usBunchesRefused = result.xfsMatch.Trim();


         return result.subLogLine;
      }

      // usItemsReturned
      public static (bool success, string xfsMatch, string subLogLine) usItemsReturnedFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usItemsReturned = \\[)(\\d+)");
      }

      // usItemsRefused
      public static (bool success, string xfsMatch, string subLogLine) usItemsRefusedFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usItemsRefused = \\[)(\\d+)");
      }

      // usBunchesRefused
      public static (bool success, string xfsMatch, string subLogLine) usBunchesRefusedFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usBunchesRefused = \\[)(\\d+)");
      }
   }
}
