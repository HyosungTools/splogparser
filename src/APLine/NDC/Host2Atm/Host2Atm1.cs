using System;
using Contract;

namespace LogLineHandler
{
   public class Host2Atm1 : Host2Atm
   {

      public Host2Atm1(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_HOST2ATM1) : base(parent, logLine, apType)
      {
         msgclass = "1";
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
            // b     1     M        Message Class : '1' - Terminal Command
            // c     1     O        Response Flag - future use 
            (bool success, string field, string subMessage) result = NDC.GetNextFieldBySeparator(ndcmsg);
            if (!result.success || !result.field.StartsWith("1"))
               return;

            english = "Command from Host, ";

            // d     3        O     Logical Unit Number - 
            // FS    1        M
            // e     3        O     Message Sequence Number
            // FS    1        M
            // f     1        M     Command Code
            // g     1        O     Message Coord Num
            // h     ?        M     Trailer

            // Field d - LUNO
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // Field e - Message Sequence/Time Variant - may or may not be there, dont care
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // Field f - Command Code
            result = GetNextFieldBySize(result.subMessage, 1);
            if (!result.success)
               return;

            char f = (result.field.Length > 0) ? result.field[0] : (char)0;
            char g = (result.field.Length > 1) ? result.field[1] : (char)0;

            switch (result.field[0])
            {
               case '1':
                  {
                     english = english + "Go into Service (start-up), ";
                     break;
                  }

               case '2':
                  {
                     english = english + "Go out of service (shut-down), ";
                     if (g == '0')
                        english = english + "Standard OOS Screen, ";
                     if (g == '1')
                        english = english + "Temporary OOS Screen, ";
                     break;
                  }
               case '3':
                  {
                     english = english + "Send Config ID, ";
                     break;
                  }
               case '4':
                  {
                     english = english + "Send Supply Counters, ";
                     if (g == '0')
                        english = english + "Send basic supply counters, ";
                     if (g == '1')
                        english = english + "Send extended supply counters, ";
                     break;
                  }
               case '5':
                  {
                     english = english + "Send Tally Information, ";
                     break;
                  }
               case '6':
                  {
                     english = english + "Send Error Log Information, ";
                     break;
                  }
               case '7':
                  {
                     english = english + "Send Configuration Information, ";
                     switch (g)
                     {
                        case '1':
                           {
                              english = english + "Send hardware config data only,";
                              break;
                           }
                        case '2':
                           {
                              english = english + "Send supplies data only,";
                              break;
                           }
                        case '3':
                           {
                              english = english + "Send fitness data only, ";
                              break;
                           }
                        case '4':
                           {
                              english = english + "Send tamper and sensor status data only,";
                              break;
                           }
                        case '5':
                           {
                              english = english + "Send software ID and release number data only, ";
                              break;
                           }
                        case '6':
                           {
                              english = english + "Send enhanced configuration data, ";
                              break;
                           }
                        case '7':
                           {
                              english = english + "Send local configuration option digits, ";
                              break;
                           }
                        case '8':
                           {
                              english = english + "Send note definitions, ";
                              break;
                           }
                        default:
                           {
                              break;
                           }
                     }
                     break;
                  }
               case '8':
                  {
                     english = english + "Send Date & Time Information, ";
                     break;
                  }
               case 'F':
                  {
                     english = english + "Disconnect, ";
                     break;
                  }
               case 'G':
                  {
                     english = english + "Maintain connection to complete transaction, ";
                     break;
                  }
               default:
                  {
                     english = english + "Unknown command, ";
                     break;
                  }
            }
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return;
      }
   }
}
