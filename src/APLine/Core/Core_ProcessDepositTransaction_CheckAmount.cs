using Contract;

namespace LogLineHandler
{
   public class Core_ProcessDepositTransaction_CheckAmount : Core
   {
      public string checkAmount = string.Empty;

      public Core_ProcessDepositTransaction_CheckAmount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessDepositTransaction_CheckAmount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Check Amount: 0
         string findMe = "Check Amount:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            checkAmount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
