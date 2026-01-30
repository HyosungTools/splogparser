using Contract;

namespace LogLineHandler
{
   public class Core_WriteTransactionReplyJournal_SequenceNumber : Core
   {
      public string sequenceNumber = string.Empty;

      public Core_WriteTransactionReplyJournal_SequenceNumber(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_WriteTransactionReplyJournal_SequenceNumber) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Sequence Number: 3964
         string findMe = "Sequence Number:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            sequenceNumber = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
