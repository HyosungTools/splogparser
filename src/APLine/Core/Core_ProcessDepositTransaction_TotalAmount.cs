using Contract;

namespace LogLineHandler
{
   public class Core_ProcessDepositTransaction_TotalAmount : Core
   {
      public string amount = string.Empty;

      public Core_ProcessDepositTransaction_TotalAmount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessDepositTransaction_TotalAmount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Total Amount: 100
         string findMe = "Total Amount:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            amount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
