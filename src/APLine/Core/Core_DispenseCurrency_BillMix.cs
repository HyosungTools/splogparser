using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_DispenseCurrency_BillMix : Core
   {
      public string billMix = string.Empty;
      public string transactionAmount = string.Empty;

      public Core_DispenseCurrency_BillMix(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_DispenseCurrency_BillMix) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Bill Mix: 50~0|20~2|5~0|1~0 Transaction Amount: 4000
         Regex regex = new Regex(@"Bill Mix:\s*(?<billMix>[\d~\|]+)\s+Transaction Amount:\s*(?<amount>\d+)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            billMix = m.Groups["billMix"].Value;
            transactionAmount = m.Groups["amount"].Value;
         }
      }
   }
}
