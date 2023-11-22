using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class APLine : LogLine, ILogLine
   {
      public APLine(ILogFileHandler parent, string logLine) : base(parent, logLine)
      {
      }

      protected override string hResult()
      {
         throw new System.NotImplementedException();
      }

      protected override string tsTimestamp()
      {
         throw new System.NotImplementedException();
      }
   }
}
