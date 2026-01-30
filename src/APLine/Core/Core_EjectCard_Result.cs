using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_EjectCard_Result : Core
   {
      public string result = string.Empty;

      public Core_EjectCard_Result(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_EjectCard_Result) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: CardReader.EjectCard(5000) result is [OK].
         Regex regex = new Regex(@"result is \[(?<result>\w+)\]");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            result = m.Groups["result"].Value;
         }
      }
   }
}
