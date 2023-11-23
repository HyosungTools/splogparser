using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMMEDIAREJECTED : SPLine
   {
      public string wReason { get; set; }

      private const string prefix = "30";

      public WFSIPMMEDIAREJECTED(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_IPM_MEDIAREJECTED) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result = wReasonFromList(logLine);
         if (result.success) wReason = prefix + result.xfsMatch.Trim();
      }


      // I N D I V I D U A L    A C C E S S O R S

      // wReason  - we dont need to search for this in the table log line, only in the list log line
      protected static (bool success, string xfsMatch, string subLogLine) wReasonFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wReason = \\[)(\\d+)");
      }
   }
}
