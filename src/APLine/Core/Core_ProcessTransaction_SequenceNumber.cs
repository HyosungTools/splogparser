using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Shared class for sequence number extraction from both withdrawal and deposit transactions
   /// </summary>
   public class Core_ProcessTransaction_SequenceNumber : Core
   {
      public string sequenceNumber = string.Empty;

      public Core_ProcessTransaction_SequenceNumber(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
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
