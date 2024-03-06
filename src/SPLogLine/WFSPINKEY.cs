using System;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSPINKEY : WFSDEVSTATUS
   {
      public string wCompletion { get; set; }
      public string ulDigit { get; set; }

      public WFSPINKEY(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_PIN_KEY) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // wCompletion
         result = wCompletionFromPINKey(logLine);
         if (result.success) wCompletion = result.xfsMatch.Trim();

         // ulDigit
         result = ulDigitFromPINKey(result.subLogLine);
         if (result.success) ulDigit = result.xfsMatch.Trim();

      }

      protected static (bool success, string xfsMatch, string subLogLine) wCompletionFromPINKey(string logLine)
      {
         return Util.MatchList(logLine, "wCompletion = \\[(.*)\\]", "0");
      }
      protected static (bool success, string xfsMatch, string subLogLine) ulDigitFromPINKey(string logLine)
      {
         return Util.MatchList(logLine, "ulDigit = \\[(.*)\\]", "0");
      }
   }
}

