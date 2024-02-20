using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIDCTRACKDETECTED : WFSDEVSTATUS
   {
      public string fwTracks { get; set; }

      public WFSIDCTRACKDETECTED(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_IDC_TRACKDETECTED) : base(parent, logLine, xfsType: xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = fwTracksFromTrackDetected(logLine);
         if (result.success) fwTracks = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwTracksFromTrackDetected(string logLine)
      {
         return Util.MatchList(logLine, "fwTracks = \\[(.*)\\]", "0");
      }

   }
}

