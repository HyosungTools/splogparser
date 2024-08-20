using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host12 : Atm2Host
   {
      public Atm2Host12(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_ATM2HOST12) : base(parent, logLine, apType)
      {
         msgclass = "1";
         msgsubclass = "2"; 
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {
            // Field b and c - Message Class - 12
            (bool success, string field, string subMessage) result = GetNextFieldBySeparator(ndcmsg);
            if (!result.success)
               return ;

            english = "Unsolicited Status to Host, ";

            // Field d - LUNO
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            // Field - none
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            // Field e1 & e2 - Device Id Graphic and Device Status
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            if (result.field.Length == 1)
            {
               english = english + String.Format("Device Id : {0}, ", DeviceIdInEnglish(result.field.Substring(0, 1)));
            }
            else
            {
               english = english + String.Format("Device Id : {0}, Device Status : {1}", DeviceIdInEnglish(result.field.Substring(0, 1)), result.field.Substring(1));
            }

            // Field e3 - Error Severity
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            if (result.field.Length > 0)
            {
               english = english + String.Format("Error Severity : {0}, ", result.field);
            }

            // Field e4 - Diagnostic Status
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            if (result.field.Length > 0)
            {
               english = english + String.Format("Diagnostic Status : {0}, ", result.field);
            }

            // Field e5 - Supplies Status
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            if (result.field.Length > 0)
            {
               english = english + String.Format("Supplies Status : {0}, ", result.field);
            }

         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return ;
      }
   }
}

