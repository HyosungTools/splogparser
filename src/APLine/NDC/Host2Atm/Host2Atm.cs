using System;
using Contract;

namespace LogLineHandler
{
   public class Host2Atm : NDC
   {
      public string myName = "Host2Atm";

      public Host2Atm(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
      }

      public static new ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         (bool success, string subLine) result = NDC.IsolateNdcMessage(logLine);
         if (!result.success)
         {
            return null;
         }
         try
         {
            (bool success, string field, string subMessage) result2 = NDC.GetNextFieldBySeparator(result.subLine);

            if (result2.success && result2.field.StartsWith("1"))
               return new Host2Atm1(logFileHandler, logLine, APLogType.NDC_HOST2ATM1);

            if (result2.success && result2.field.StartsWith("3"))
               return new Host2Atm3(logFileHandler, logLine, APLogType.NDC_HOST2ATM3);

            if (result2.success && result2.field.StartsWith("4"))
               return new Host2Atm4(logFileHandler, logLine, APLogType.NDC_HOST2ATM4);

            if (result2.success && result2.field.StartsWith("6"))
               return new Host2Atm6(logFileHandler, logLine, APLogType.NDC_HOST2ATM6);

            if (result2.success && result2.field.StartsWith("7"))
               return new Host2Atm6(logFileHandler, logLine, APLogType.NDC_HOST2ATM7);
         }
         catch (Exception e)
         {
            logFileHandler.ctx.ConsoleWriteLogLine(String.Format("Host2Atm ILogLine.Factory - NDC parsing error - {0}", e.Message));
         }

         return new APLine(logFileHandler, logLine, APLogType.None);
      }

   }
}
