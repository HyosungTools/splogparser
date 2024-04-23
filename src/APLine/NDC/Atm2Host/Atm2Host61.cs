using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host61 : Atm2Host
   {
      public Atm2Host61(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_ATM2HOST61) : base(parent, logLine, apType)
      {
         msgclass = "6";
         msgsubclass = "1"; 
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}", myName, ndcmsg));
         }

         return ;
      }
   }
}

