using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCIMNOTEERROR : SPLine
   {
      public string usReason { get; set; }

      public WFSCIMNOTEERROR(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_CIM_NOTEERROR) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // wReason
         result = usReasonFromList(logLine);
         if (result.success) usReason =  result.xfsMatch.Trim();
      }

      // I N D I V I D U A L    A C C E S S O R S

      // usReason  - we dont need to search for this in the table log line, only in the list log line
      protected static (bool success, string xfsMatch, string subLogLine) usReasonFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usReason = \\[)(\\d+)");
      }
   }
}
