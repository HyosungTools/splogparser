using Contract;

namespace Impl
{
   public class WFSCIMCASHINSTATUS : WFS
   {
      public string wStatus { get; set; }
      public string usNumOfRefused { get; set; }
      public string[,] noteNumbers { get; set; }

      public WFSCIMCASHINSTATUS(IContext ctx) : base(ctx)
      { }

      public string Initialize(string logLine)
      {
         // access the values
         (bool success, string xfsMatch, string subLogLine) result;
         (bool success, string[,] xfsMatch, string subLogLine) results;

         result = wStatusFromList(logLine);
         wStatus = result.xfsMatch.Trim();

         result = usNumOfRefusedFromList(result.subLogLine);
         usNumOfRefused = result.xfsMatch.Trim();

         results = noteNumbersFromList(result.subLogLine);
         noteNumbers = results.xfsMatch;

         return logLine; 
      }

      // wStatus  - 
      public static (bool success, string xfsMatch, string subLogLine) wStatusFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wStatus = \\[)(\\d+)");
      }

      // usNumOfRefused  - 
      public static (bool success, string xfsMatch, string subLogLine) usNumOfRefusedFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usNumOfRefused = \\[)(\\d+)");
      }

      // noteNumbers  - 
      public static (bool success, string[,] xfsMatch, string subLogLine) noteNumbersFromList(string logLine)
      {
         return (true, WFSCIMNOTENUMBERLIST.NoteNumberListFromList(logLine), logLine); 
      }
   }
}

