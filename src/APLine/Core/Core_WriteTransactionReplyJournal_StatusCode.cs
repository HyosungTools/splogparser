using Contract;

namespace LogLineHandler
{
   public class Core_WriteTransactionReplyJournal_StatusCode : Core
   {
      public string statusCode = string.Empty;

      public Core_WriteTransactionReplyJournal_StatusCode(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_WriteTransactionReplyJournal_StatusCode) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Status Code: True (FiservDNA) or Status Code: Success (JackHenry)
         string findMe = "Status Code:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            statusCode = logLine.Substring(idx + findMe.Length).Trim();
            
            // Normalize: "Success" -> "True" for consistency
            if (statusCode.Equals("Success", System.StringComparison.OrdinalIgnoreCase))
            {
               statusCode = "True";
            }
         }
      }
   }
}
