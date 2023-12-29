using Contract;

namespace LogLineHandler
{
   public class Core_ProcessWithdrawalTransactionAccount : Core
   {
      public string account;

      public Core_ProcessWithdrawalTransactionAccount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessWithdrawalTransaction_Account) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = "Account Number";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            account = logLine.Substring(idx + findMe.Length + 1);

            // JackHenry has a slightly different form e.g. [NORMAL]Account Number - x4361, Account Type - D, Transaction Code - 58
            idx = account.IndexOf(",");
            if (idx != -1)
            {
               // isolate the account number by skipping over the '-' and truncating on the ','
               account = account.Substring(2, idx - 2);
            }
            account = account.Trim();
         }
      }
   }
}
