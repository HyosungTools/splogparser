using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host51 : Atm2Host
   {

      public Atm2Host51(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_ATM2HOST51) : base(parent, logLine, apType)
      {
         msgclass = "5";
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
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return ;
      }
   }
}
