using Contract;

namespace LogLineHandler
{
   public class Core_ProcessWithdrawalTransaction_Amount : Core
   {
      public string amount = string.Empty;

      public Core_ProcessWithdrawalTransaction_Amount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessWithdrawalTransaction_Amount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = " ";
         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            // isolate amount
            amount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
