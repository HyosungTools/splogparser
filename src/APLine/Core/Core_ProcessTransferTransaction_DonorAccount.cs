using Contract;

namespace LogLineHandler
{
   public class Core_ProcessTransferTransaction_DonorAccount : Core
   {
      public string donorAccount = string.Empty;

      public Core_ProcessTransferTransaction_DonorAccount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ProcessTransferTransaction_DonorAccount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Donor Account Number: XXXXXX4727
         string findMe = "Donor Account Number:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            donorAccount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
