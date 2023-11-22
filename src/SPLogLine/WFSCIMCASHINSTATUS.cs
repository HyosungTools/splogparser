using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCIMCASHINSTATUS : SPLine
   {
      public string wStatus { get; set; }
      public string usNumOfRefused { get; set; }
      public string[,] noteNumbers { get; set; }

      public WFSCIMCASHINSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_CIM_CASH_IN_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;
         (bool success, string[,] xfsMatch, string subLogLine) results;

         result = wStatusFromList(logLine);
         wStatus = result.xfsMatch.Trim();

         result = usNumOfRefusedFromList(result.subLogLine);
         usNumOfRefused = result.xfsMatch.Trim();

         results = noteNumbersFromList(result.subLogLine);
         noteNumbers = results.xfsMatch;
      }

      // wStatus  - 
      protected static (bool success, string xfsMatch, string subLogLine) wStatusFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wStatus = \\[)(\\d+)");
      }

      // usNumOfRefused  - 
      public static (bool success, string xfsMatch, string subLogLine) usNumOfRefusedFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usNumOfRefused = \\[)(\\d+)");
      }

      // noteNumbers  - 
      protected static (bool success, string[,] xfsMatch, string subLogLine) noteNumbersFromList(string logLine)
      {
         return (true, WFSCIMNOTENUMBERLIST.NoteNumberListFromList(logLine), logLine); 
      }
   }
}

