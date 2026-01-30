using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_MakeAccountLists_UniqueAccounts : Core
   {
      public string accountCount = string.Empty;

      public Core_MakeAccountLists_UniqueAccounts(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_MakeAccountLists_UniqueAccounts) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: After removing duplicates. Number of accounts:9
         Regex regex = new Regex(@"Number of accounts:\s*(?<count>\d+)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            accountCount = m.Groups["count"].Value;
         }
      }
   }
}
