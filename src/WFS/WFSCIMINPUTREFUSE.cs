using Contract;

namespace Impl
{
   public class WFSCIMINPUTREFUSE : WFS
   {
      public string usReason { get; set; }

      public WFSCIMINPUTREFUSE(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // wReason
         result = usReasonFromList(nwLogLine);
         if (result.success) usReason =  result.xfsMatch.Trim();

         return result.subLogLine;
      }

      // I N D I V I D U A L    A C C E S S O R S

      // usReason  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) usReasonFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usReason = \\[)(\\d+)");
      }
   }
}
