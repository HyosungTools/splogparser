using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{

   public class WFSIPMMEDIAPRESENTED : SPLine
   {
      public string wPosition { get; set; }
      public string usBunchIndex { get; set; }
      public string usTotalBunches { get; set; }

      public WFSIPMMEDIAPRESENTED(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_IPM_MEDIAPRESENTED) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // wPosition
         result = wPositionFromList(logLine);
         if (result.success) wPosition = result.xfsMatch.Trim();

         // usBunchIndex
         result = usBunchIndexFromList(logLine);
         if (result.success) usBunchIndex = result.xfsMatch.Trim();

         // usTotalBunches
         result = usTotalBunchesFromList(result.subLogLine);
         if (result.success) usTotalBunches = result.xfsMatch.Trim();
      }

      // I N D I V I D U A L    A C C E S S O R S

      // wPosition
      protected static (bool success, string xfsMatch, string subLogLine) wPositionFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wPosition = \\[)(\\d+)");
      }

      // usBunchIndex
      protected static (bool success, string xfsMatch, string subLogLine) usBunchIndexFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usBunchIndex = \\[)(\\d+)");
      }

      // usTotalBunches
      protected static (bool success, string xfsMatch, string subLogLine) usTotalBunchesFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usTotalBunches = \\[)(\\d+)");
      }
   }
}
