using Contract;

namespace LogLineHandler
{
   public class Core_ProcessWithdrawalTransactionAmount : Core
   {
      public string amount;

      public Core_ProcessWithdrawalTransactionAmount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessWithdrawalTransaction_Amount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = " ";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            amount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
