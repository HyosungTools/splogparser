using Contract;

namespace LogLineHandler
{
   public class Core_ProcessTransferTransaction_RecipientAccount : Core
   {
      public string recipientAccount = string.Empty;

      public Core_ProcessTransferTransaction_RecipientAccount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessTransferTransaction_RecipientAccount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Recipient Account Number: XXXXXX4727
         string findMe = "Recipient Account Number:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            recipientAccount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
