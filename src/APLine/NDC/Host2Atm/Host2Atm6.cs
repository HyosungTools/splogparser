using System;
using Contract;

namespace LogLineHandler
{
   public class Host2Atm6 : Host2Atm
   {
      // EJ Commands
      //
      // 61 - Acknowledge EJ upload block
      // 62 - Acknowledge and Stop EJ
      // 63 - Options and Timers 
      //
      // This command may be sent by Central at any time to enable electronic journal upload,
      // and to specify the options and timer values to be used.It may also be sent after
      // receiving a power fail message from the SST to re-instate the EJ upload feature.

      public Host2Atm6(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_HOST2ATM6) : base(parent, logLine, apType)
      {
         msgclass = "6";
         msgsubclass = "";
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {

            // Field Size  Man/Opt  Description
            // a     Var   M        Header - protocol dependent
            // b     1     M        Message Class : '6' - Unsolicited message
            (bool success, string field, string subMessage) result = NDC.GetNextFieldBySeparator(ndcmsg);
            if (!result.success)
               return ;

            english = "EJ Command, ";

            // Read over 2 Field Separators
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            // c     1     M        Command Type. The Command Type is: 
            //                      1 - Acknowledge EJ upload block
            //                      2 - Acknowledge and Stop EJ
            //                      3 - Options and Timers                     
            result = GetNextFieldBySize(result.subMessage, 1);
            if (!result.success)
               return ;

            if (result.field[0] == '1')
            {
               english += AcknowledgeUploadBlock(result.subMessage);
            }
            else if (result.field[0] == '2')
            {
               english += AcknowledgeAndStopEJ(result.subMessage);
            }
            else if (result.field[0] == '3')
            {
               english += OptionsAndTimers(result.subMessage);
            }
            else
            {
               english += string.Format("Command Type Unknown '{0}", result.field[0]);
            }
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("Host2Atm4 Unexpected parse in message : {0}", ndcmsg));
         }

         return ;
      }

      static string AcknowledgeUploadBlock(string fields)
      {
         string English = "Acknowledge EJ upload block";



         return English;
      }
      static string AcknowledgeAndStopEJ(string fields)
      {
         string English = "Acknowledge and Stop EJ";



         return English;
      }

      static string OptionsAndTimers(string ndcMessage)
      {
         string English = "EJ Options and Timers,";

         // Fields 'd' and 'e' are repeated if both options are sent in the same message. 
         // No FS is requred between options, but a FS is mandatory before field 'f' - Timer Number
         // d     2     O        Option Number.
         // e     3     O        Option Value.
         (bool success, string field, string subMessage) result = NDC.GetNextFieldBySeparator(ndcMessage);
         if (!result.success)
            return English;

         // we have (probably) 2 x d+e fields
         // Option 60 - EJ Upload Block Size - Max size of upload block - capped at 350. 
         // Option 61 - Retry Threshold. Max attempts to try uploading before giving up (zero means forever). 

         (bool success, string xfsMatch, string subLogLine) result2 = NDC.NDCMatch(result.field, "(?<=60)(\\d{3})");
         if (result.success)
         {
            English = English + string.Format(" for EJ Upload block size use '{0}',", result2.xfsMatch);
         }

         result2 = NDC.NDCMatch(result.field, "(?<=61)(\\d{3})");
         if (result.success)
         {
            if (result2.xfsMatch == "000")
            {
               English = English + string.Format(" retry upload is infinite times,");
            }
            else
            {
               English = English + string.Format(" retry upload '{0}' times,", result2.xfsMatch);
            }
         }

         //// f     2     O        Timer Number. Possible Value is
         ////                      Timer 60 - EJ Ack Timer. Max time in sec to wait from Central before sending block
         //// g     3     O        Timer Value. Range 000-255. Default is 255. 000 means infinite. 
         //result = GetNextFieldBySize(result.subMessage, 5);
         //if (!result.success)
         //   return English;

         //result2 = NDC.NDCMatch(result.field, "(?<=60)(\\d{3})");
         //if (result2.success)
         //{
         //   English = English + string.Format(" max time (sec) to wait for ACK from host is '{0}' seconds,", result2.xfsMatch);
         //}

         return English;

      }
   }
}
