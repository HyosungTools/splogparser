using Contract;

namespace LogLineHandler
{
   public class Core_ProcessDepositTransaction_CashAmount : Core
   {
      public string cashAmount = string.Empty;

      public Core_ProcessDepositTransaction_CashAmount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessDepositTransaction_CashAmount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Cash Amount: 100
         string findMe = "Cash Amount:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            cashAmount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
