using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMMEDIAREFUSED : SPLine
   {
      public string wReason { get; set; }
      public string wMediaLocation { get; set; }
      public string bPresentRequired { get; set; }

      private const string prefix = "20";

      public WFSIPMMEDIAREFUSED(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_IPM_MEDIAREFUSED) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // wReason
         result = wReasonFromList(logLine);
         if (result.success) wReason = prefix + result.xfsMatch.Trim();

         // wMediaLocation
         result = wMediaLocationFromList(logLine);
         if (result.success) wMediaLocation = result.xfsMatch.Trim();

         // bPresentRequired
         result = bPresentRequiredFromList(result.subLogLine);
         if (result.success) bPresentRequired = result.xfsMatch.Trim();
      }


      // I N D I V I D U A L    A C C E S S O R S


      // wReason  - we dont need to search for this in the table log line, only in the list log line
      protected static (bool success, string xfsMatch, string subLogLine) wReasonFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wReason = \\[)(\\d+)");
      }

      // wMediaLocation
      protected static (bool success, string xfsMatch, string subLogLine) wMediaLocationFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wMediaLocation = \\[)(\\d+)");
      }

      // bPresentRequired
      protected static (bool success, string xfsMatch, string subLogLine) bPresentRequiredFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=bPresentRequired = \\[)(\\d+)");
      }
   }
}
