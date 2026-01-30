using Contract;

namespace LogLineHandler
{
   public class Core_ProcessDepositTransaction_Account : Core
   {
      public string account = string.Empty;

      public Core_ProcessDepositTransaction_Account(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessDepositTransaction_Account) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Account Number: XXXXX0163
         string findMe = "Account Number:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            account = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
