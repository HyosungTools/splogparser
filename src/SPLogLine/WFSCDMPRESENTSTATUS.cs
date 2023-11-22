using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCDMPRESENTSTATUS : SPLine
   {
      public WFSCDMDENOMINATION CDMDENOM { get; set; }
      public string wPresentState { get; set; }

      public WFSCDMPRESENTSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CDM_PRESENT_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         CDMDENOM = new WFSCDMDENOMINATION(this.parentHandler, logLine, XFSType.None);
         (bool success, string xfsMatch, string subLogLine) result = wPresentStateFromList(logLine);
         wPresentState = result.xfsMatch.Trim();
      }

      // wPresentState
      protected static (bool success, string xfsMatch, string subLogLine) wPresentStateFromList(string logLine)
      {
         return Util.Match(logLine, "(?<=wPresentState = \\[)([a-zA-Z0-9 ]*)\\]");
      }
   }
}
