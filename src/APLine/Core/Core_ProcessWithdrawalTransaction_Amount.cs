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

         // FiservDNA, SymXchange, CUAnswers, KeyBridge, AccessAdvantage pattern: Total Amount: 40
         if (logLine.Contains("Total Amount:"))
         {
            string findMe = "Total Amount:";
            int idx = logLine.LastIndexOf(findMe);
            if (idx != -1)
            {
               amount = logLine.Substring(idx + findMe.Length).Trim();
            }
         }
         // JackHenry pattern: Amount - 650
         else if (logLine.Contains("Amount -"))
         {
            string findMe = "Amount -";
            int idx = logLine.LastIndexOf(findMe);
            if (idx != -1)
            {
               amount = logLine.Substring(idx + findMe.Length).Trim();
            }
         }
         // CMCFlex pattern: Amount: 1 (no "Total", uses colon)
         else if (logLine.Contains("Amount:"))
         {
            string findMe = "Amount:";
            int idx = logLine.LastIndexOf(findMe);
            if (idx != -1)
            {
               amount = logLine.Substring(idx + findMe.Length).Trim();
            }
         }
      }
   }
}
