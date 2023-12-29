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

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
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
               return new Host2Atm1(logFileHandler, logLine, APLogType.Atm2Host11);
         }
         catch (Exception e)
         {
            logFileHandler.ctx.ConsoleWriteLogLine(String.Format("Atm2Host ILogLine.Factory - NDC parsing error - {0}", e.Message));
         }

         return null;

      }
   }
}
