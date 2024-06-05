using System;
using Contract;

namespace LogLineHandler
{
   public class Host2Atm7 : Host2Atm
   {
      public Host2Atm7(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_HOST2ATM7) : base(parent, logLine, apType)
      {
         msgclass = "7";
         msgsubclass = "";
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

      public override bool ParseToEnglish()
      {
         return ParseToEnglishBrief();
      }
   }
}
