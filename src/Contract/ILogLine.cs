using System.Collections.Generic;

namespace Contract
{
   public interface ILogLine
   {
      string LogFile { get; }

      string Timestamp { get; }
      bool IsValidTimestamp { get; set; }

      bool IsRecognized { get; set; }

      bool IgnoreThisLine { get; set; }

      bool ThrowExceptionIfNotRecognized { get; set; }
   }
}
