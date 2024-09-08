using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host : NDC
   {
      public string myName = "Atm2Host";

      public Atm2Host(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
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

            if (result2.success && result2.field.StartsWith("11"))
               return new Atm2Host11(logFileHandler, logLine, APLogType.NDC_ATM2HOST11);

            if (result2.success && result2.field.StartsWith("12"))
               return new Atm2Host12(logFileHandler, logLine, APLogType.NDC_ATM2HOST12);

            if (result2.success && result2.field.StartsWith("22"))
               return new Atm2Host22(logFileHandler, logLine, APLogType.NDC_ATM2HOST22);

            if (result2.success && result2.field.StartsWith("23"))
               return new Atm2Host22(logFileHandler, logLine, APLogType.NDC_ATM2HOST23);

            if (result2.success && result2.field.StartsWith("51"))
               return new Atm2Host51(logFileHandler, logLine, APLogType.NDC_ATM2HOST51);

            if (result2.success && result2.field.StartsWith("61"))
               return new Atm2Host61(logFileHandler, logLine, APLogType.NDC_ATM2HOST61);

         }
         catch (Exception e)
         {
            logFileHandler.ctx.ConsoleWriteLogLine(String.Format("Atm2Host ILogLine.Factory - NDC parsing error - {0}", e.Message));
         }

         return new APLine(logFileHandler, logLine, APLogType.None);

      }
   }
}
