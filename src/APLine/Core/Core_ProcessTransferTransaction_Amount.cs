using Contract;

namespace LogLineHandler
{
   public class Core_ProcessTransferTransaction_Amount : Core
   {
      public string amount = string.Empty;

      public Core_ProcessTransferTransaction_Amount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessTransferTransaction_Amount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Amount: 350 or Amount - 350
         if (logLine.Contains("Amount:"))
         {
            string findMe = "Amount:";
            int idx = logLine.LastIndexOf(findMe);
            if (idx != -1)
            {
               amount = logLine.Substring(idx + findMe.Length).Trim();
            }
         }
         else if (logLine.Contains("Amount -"))
         {
            string findMe = "Amount -";
            int idx = logLine.LastIndexOf(findMe);
            if (idx != -1)
            {
               amount = logLine.Substring(idx + findMe.Length).Trim();
            }
         }
      }
   }
}
