using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_StoreCash : Core
   {
      public string result = string.Empty;

      public Core_StoreCash(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_StoreCash) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: CashAcceptor.StoreCash result= OK
         Regex regex = new Regex(@"result=\s*(?<result>\w+)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            result = m.Groups["result"].Value;
         }
      }
   }
}
