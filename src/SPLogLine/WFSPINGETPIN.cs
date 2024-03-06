using System;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSPINGETPIN : WFSDEVSTATUS
   {
      public string wCompletion { get; set; }
      public string usDigits { get; set; }

      public WFSPINGETPIN(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_CMD_PIN_GET_PIN) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // wCompletion
         result = wCompletionFromPINKey(logLine);
         if (result.success) wCompletion = result.xfsMatch.Trim();

         // usDigits
         result = usDigitsFromPINKey(logLine);
         if (result.success) usDigits = result.xfsMatch.Trim();

      }

      protected static (bool success, string xfsMatch, string subLogLine) wCompletionFromPINKey(string logLine)
      {
         return Util.MatchList(logLine, "wCompletion = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) usDigitsFromPINKey(string logLine)
      {
         return Util.MatchList(logLine, "usDigits = \\[(.*)\\]", "0");
      }
   }
}

