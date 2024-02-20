using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIDCRETAINBINTHRESHOLD : WFSDEVSTATUS
   {
      public string fwRetainBin { get; set; }

      public WFSIDCRETAINBINTHRESHOLD(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_USRE_IDC_RETAINBINTHRESHOLD) : base(parent, logLine, xfsType: xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = fwRetainBinFromBinThreshold(logLine);
         if (result.success) fwRetainBin = result.xfsMatch.Trim();
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwRetainBinFromBinThreshold(string logLine)
      {
         return Util.MatchList(logLine, "fwRetainBin = \\[(.*)\\]", "0");
      }

   }
}

