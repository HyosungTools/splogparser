using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_MakeAccountLists_TotalAccounts : Core
   {
      public string accountCount = string.Empty;

      public Core_MakeAccountLists_TotalAccounts(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_MakeAccountLists_TotalAccounts) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Pulled all agreement accounts. Number of accounts:18
         Regex regex = new Regex(@"Number of accounts:\s*(?<count>\d+)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            accountCount = m.Groups["count"].Value;
         }
      }
   }
}
