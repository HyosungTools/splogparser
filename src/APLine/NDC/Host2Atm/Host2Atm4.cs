using System;
using System.Text.RegularExpressions;
using Contract;


using System;
using Contract;

namespace LogLineHandler
{
   public class Host2Atm4 : Host2Atm
   {
      public string notesToDispense = string.Empty;

      public Host2Atm4(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_HOST2ATM4) : base(parent, logLine, apType)
      {
         msgclass = "4";
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
            // b     1     M        Message Class : '4' - Terminal Command
            // c     1     O        Response Flag - future use

            // Read over a, b and c
            Regex regex = new Regex("^(?<abc>[^\u001c]*)(?<rest>.*)?");
            Match m = regex.Match(ndcmsg);
            if (!m.Success)
            {
               return ;
            }

            english = "Transaction Reply to ATM, ";

            // d     3        O     Logical Unit Number - 
            regex = new Regex("^\u001c(?<LUNO>[^\u001c]*)(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

            if (m.Groups["LUNO"].Value.Length > 0)
               english = english + " LUNO : " + m.Groups["LUNO"].Value + ",";

            // e     3        O     Message Sequence Number
            regex = new Regex("^\u001c(?<e>[^\u001c]*)(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

            if (m.Groups["e"].Value.Length > 0)
               english = english + " MSN : " + m.Groups["e"].Value + ",";

            // f     3        O     Next State ID Data
            regex = new Regex("^\u001c(?<f>[^\u001c]*)(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

            english += " Next State : " + m.Groups["f"].Value + ",";

            // g    ?        ?     Notes (and coins) to Dispense
            regex = new Regex("^\u001c(?<g>[^\u001c]*)(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

            english += NotesToDispense(m.Groups["g"].Value);

            // k    4        M     Transaction Serial Number
            regex = new Regex("^\u001c(?<k>.{4})(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

           english += "Transaction Serial Number : " + m.Groups["k"].Value + ",";

            // 1    1        M     Function Identifier
            regex = new Regex("^(?<l>.{1})(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

            switch(m.Groups["l"].Value[0])
            {
               case '1': english += "Deposit and Print,"; break;
               case '2': english += "Dispense and Print"; break;
               case '3': english += "Display and Print"; break;
               case '4': english += "Print Immediate"; break;
               case '5': english += "Set Next State and Print"; break;
               case '6': english += "Night Safe Deposit and Print"; break;
               case '7': english += "Deposit and Print,"; break;
               case '8': english += "Dispense and Print"; break;
               case '9': english += "Display and Print"; break;
               case 'A': english += "Eject Card, Dispense and Print"; break;
               case 'B': english += "Parallel Dispense/Print and Eject Card"; break;
               case 'C': english += "Parallel Dispense/Print and Eject Card"; break;
               case 'E': english += "Reserved (specific command eject)"; break;
               case 'F': english += "Card before Parallel Dispense/Print"; break;
               case 'O': english += "Reserved"; break;
               case 'P': english += "Print Statement and Wait"; break;
               case 'Q': english += "Print Statement and Set Next State"; break;
               case 'R': english += "Reserved (specific command eject)"; break;
               case 'S': english += "Reserved (specific command eject)"; break;
               case 'T': english += "Reserved (specific command eject)"; break;
               case '*': english += "Refund BNA deposited money and set next state"; break;
               case '-': english += "Encash BNA deposited money, issue receipt if requested, and set next state."; break;
               case '\'': english += "Encash BNA deposited money, and wait for another reply from Central."; break;
               case ':': english += "Process CPM cheque"; break;
               default: english += "Unknown command"; break;
            }
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("Host2Atm4 Unexpected parse in message : {0}", ndcmsg));
         }

         return ;
      }

      public static string NotesToDispense(string ndcMessage)
      {
         string English = "Dispense :";
         return English; 
      }
   }
}

