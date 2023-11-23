using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Commands where this structure is used: 
   //

   public class WFSIPMMEDIAINEND : SPLine
   {
      public string usItemsReturned { get; set; }
      public string usItemsRefused { get; set; }
      public string usBunchesRefused { get; set; }

      public WFSIPMMEDIAINEND(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_CMD_IPM_MEDIA_IN_END) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // usItemsReturned
         result = usItemsReturnedFromList(logLine);
         if (result.success) usItemsReturned = result.xfsMatch.Trim();

         // usItemsRefused
         result = usItemsRefusedFromList(logLine);
         if (result.success) usItemsRefused = result.xfsMatch.Trim();

         // usBunchesRefused
         result = usBunchesRefusedFromList(result.subLogLine);
         if (result.success) usBunchesRefused = result.xfsMatch.Trim();
      }

      // usItemsReturned
      protected static (bool success, string xfsMatch, string subLogLine) usItemsReturnedFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usItemsReturned = \\[)(\\d+)");
      }

      // usItemsRefused
      protected static (bool success, string xfsMatch, string subLogLine) usItemsRefusedFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usItemsRefused = \\[)(\\d+)");
      }

      // usBunchesRefused
      protected static (bool success, string xfsMatch, string subLogLine) usBunchesRefusedFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usBunchesRefused = \\[)(\\d+)");
      }
   }
}
