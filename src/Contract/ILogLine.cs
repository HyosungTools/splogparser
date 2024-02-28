using System.Collections.Generic;

namespace Contract
{
   public interface ILogLine
   {
      string LogFile { get; }

      bool IsRecognized { get; set; }

      bool ThrowExceptionIfNotRecognized { get; set; }
   }
}
