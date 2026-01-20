using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCIMINPUTREFUSE : SPLine
   {
      public string usReason { get; set; }
      public string errorCode { get; set; }

      public WFSCIMINPUTREFUSE(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_CIM_INPUTREFUSE) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // wReason
         result = usReasonFromList(logLine);
         if (result.success) usReason =  result.xfsMatch.Trim();

         // errorCode
         result = errorCodeFromList(logLine);
         if (result.success) errorCode =  result.xfsMatch.Trim();
      }

      // I N D I V I D U A L    A C C E S S O R S

      // usReason  - we dont need to search for this in the table log line, only in the list log line
      protected static (bool success, string xfsMatch, string subLogLine) usReasonFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usReason = \\[)(\\d+)");
      }

      // errorCode
      protected static (bool success, string xfsMatch, string subLogLine) errorCodeFromList(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ErrorCode\s*=\s*)(\d+)");
      }
   }
}
