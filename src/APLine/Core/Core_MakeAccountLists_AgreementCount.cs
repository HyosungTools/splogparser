using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_MakeAccountLists_AgreementCount : Core
   {
      public string eAgreementCount = string.Empty;
      public string cardAgreementCount = string.Empty;

      public Core_MakeAccountLists_AgreementCount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_MakeAccountLists_AgreementCount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: eAgreement count= 2,cardAgreement count= 0
         Regex regex = new Regex(@"eAgreement count=\s*(?<eCount>\d+)\s*,\s*cardAgreement count=\s*(?<cCount>\d+)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            eAgreementCount = m.Groups["eCount"].Value;
            cardAgreementCount = m.Groups["cCount"].Value;
         }
      }
   }
}
