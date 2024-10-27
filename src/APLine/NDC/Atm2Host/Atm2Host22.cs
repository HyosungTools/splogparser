using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host22 : Atm2Host
   {
      public Atm2Host22(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_ATM2HOST22) : base(parent, logLine, apType)
      {
         msgclass = "2";
         msgsubclass = "2";
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {

            // Field Size  Man/Opt  Description
            // a     Var   M        Header - protocol dependent
            // b     1     M        Message Class : '2' - Solicited message
            // c     1     M        Message Sub-Class : '2' - Status message
            (bool success, string field, string subMessage) result = NDC.GetNextFieldBySeparator(ndcmsg);
            if (!result.success || !result.field.StartsWith("22"))
               return ;
 
            english = "Solicited Status to Host, ";

            // d     3/9      M     Logical Unit Number -
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            english = english + string.Format("LUNO '{0}',", result.field);

            // char separator = (char)Convert.ToInt32("1c", 16);

            // the format of the rest of the message varies
            // e     8        ?     Time Variant - only present if Data Security selected RegEx: [0-9A-Fa-f]{8}\\u001c


            (bool success, string msgMatch, string remainder) regExResult;
            char f = (char) 0;

            // is Time Variant present? 
            regExResult = NDC.NDCMatch(result.subMessage, "\u001c([0-9A-Fa-f]{8})\u001c([89ABCF]).*");
            if (regExResult.success)
            {
               // Time Variant and Status Indicator
            }
            else
            {
               // Only Status Indicator Present 
               regExResult = NDC.NDCMatch(result.subMessage, "^\u001c([89ABCF]).*");
               if (regExResult.success)
               {
                  f = regExResult.msgMatch[0];
               }
            }
            
            switch (f)
            {
               case '8':
                  {
                     english = english + String.Format("Device fault (terminal device in abnormal state),");
                     break;
                  }
               case '9':
                  {
                     english = english + String.Format("Ready (instruction completed successfully),");
                     break;
                  }
               case 'A':
                  {
                     english = english + String.Format("Command reject (won't do),");
                     break;
                  }
               case 'B':
                  {
                     english = english + String.Format("Ready (Transaction Reply completed successfully),");
                     break;
                  }
               case 'C':
                  {
                     english = english + String.Format("Specific command reject (won't do),");
                     break;
                  }
               case 'F':
                  {
                     english = english + String.Format("Terminal State (response to Terminal Command),");
                     break;
                  }
               default:
                  {
                     english = english + String.Format("Unknown,");
                     break;
                  }
            }

            if (f == 'C')
            {
               english = english + SpecificCommandReject(myName, result.subMessage);
            }

            if (f == 'F')
            {
               english = english + TerminalState(myName, result.subMessage);
            }

            if (f == '8')
            {
               english = english + SolicitedDevice(myName, result.subMessage);
            }

            return ;
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return ;
      }

      private static string SpecificCommandReject(string myName, string subMessage)
      {
         string English = string.Empty;

         // Table 9-4 Specific Command Reject - Status Information
         // Field Size     M/O   Description
         // g1    1        M     Status Value - Gives the reason for the reject command

         return English; 
      }

      private static string TerminalState(string myName, string subMessage)
      {
         string English = string.Empty;

         return English;
      }

      // Device Fault Status Response - p9-59
      // The following table shows the solicited device fault status messages which may be returned for each Transaction Reply command.
      // Transaction Reply             Command Device Faults
      // Deposit and Print             Depository
      // Dispense and Print            Cash Handler, Coin Dispenser
      // Print Immediate               None
      // Set Next State and Print      None
      // Night Safe Deposit and Print  None
      // Card Before Cash              Card Reader/Writer, Cash Handler, Coin Dispenser
      // Fast Cash                     Cash Handler, Coin Dispenser
      // Card Before Parallel Dispense and Print   Card Reader/Writer, Cash Handler, Coin Dispenser
      // Print Statement and Wait                  Statement Printer and Receipt in sideways mode
      // Print Statement and Set Next State        Statement Printer and Receipt in sideways mode
      // Refund                                    Bunch Note Acceptor
      // Encash                                    Bunch Note Acceptor
      // Process Cheque                            Cheque Processing Module. 

      private static string SolicitedDevice(string myName, string subMessage)
      {
         string English = string.Empty; 

         try
         {
            // Match for g1 and g2. If there's a FS leave it on the table
            Regex regex = new Regex("^\u001c8\u001c(?<g1>.)(?<g2>[^\u001c]*)(?<rest>.*)?");
            Match m = regex.Match(subMessage);
            if (m.Success)
            {
               // Device Identification Graphic
               string DIG = m.Groups["g1"].Value; 
               English = English + NDC.DeviceIdInEnglish(DIG);

               // Transaction Status
               English = English + " Trans Status : " + m.Groups["g2"].Value + ",";

               if (m.Groups["rest"].Value.Length > 0)
               {
                  // Match for g3. If there's a FS leave it on the table
                  subMessage = m.Groups["rest"].Value;
                  regex = new Regex("^\u001c(?<g3>[^\u001c]*)(?<rest>.*)?");
                  m = regex.Match(subMessage);
                  if (m.Success)
                  {
                     English = English + " Error Severity :";
                     if (m.Groups["g3"].Value.Contains("4")) English = English + " Fatal,";
                     else if (m.Groups["g3"].Value.Contains("3")) English = English + " Suspend,";
                     else if (m.Groups["g3"].Value.Contains("2")) English = English + " Warning,";
                     else if (m.Groups["g3"].Value.Contains("1")) English = English + " Routine,";
                     else English = English + " No Error,";

                     if (m.Groups["rest"].Value.Length > 0)
                     {
                        // Match for g4. If there's a FS leave it on the table
                        subMessage = m.Groups["rest"].Value;
                        regex = new Regex("^\u001c(?<g4>[^\u001c]*)(?<rest>.*)?");
                        m = regex.Match(subMessage);
                        if (m.Success)
                        {
                           English = English + " Diagnostic Status :";

                           // Characters 1 and 2 contain a main error status value (M-Status) in the range 0 - 99,
                           // transmitted as two characters which give the decimal representation of the M - Status value.
                           English = English + "#MStatus#" + NDC.DeviceIdInEnglish(DIG) + "#" + m.Groups["g4"].Value.Substring(0, 2) + "#,";

                           // Characters 3 to n (M-Data) contain detailed diagnostic information related to the device.
                           English = English + "#MData#" + DIG + "#" + m.Groups["g4"].Value.Substring(2) + "#,";

                           if (m.Groups["rest"].Value.Length > 0)
                           {
                              // Match for g5. If there's a FS leave it on the table
                              subMessage = m.Groups["rest"].Value;
                              regex = new Regex("^\u001c(?<g5>[^\u001c]*)(?<rest>.*)?");
                              m = regex.Match(subMessage);
                              if (m.Success)
                              {
                                 English = English + " Supplies Status :";
                                 if (m.Groups["g5"].Value.Contains("4")) English = English + " Overfill,";
                                 else if (m.Groups["g5"].Value.Contains("3")) English = English + " Media Out,";
                                 else if (m.Groups["g5"].Value.Contains("2")) English = English + " Media Low,";
                                 else if (m.Groups["g5"].Value.Contains("1")) English = English + " Good,";
                                 else English = English + " No New State,";
                              }
                           }
                        }
                     }
                  }

               }
            }
         }
         catch(Exception e)
         {
            string err = e.Message; 
         }

         return English; 
      }
   }
}




//switch (m.Groups["g1"].Value)
//   {
//      case "A": English = "Time of Day Clock,"; break;
//      case "B": English = "Power Fail,"; break;
//      case "D": English = "Card Reader,"; break;
//      case "E": English = "Cash Handler,"; break;
//      case "F": English = "Depository,"; break;
//      case "G": English = "Receipt Printer,"; break;
//      case "H": English = "Journal Printer,"; break;
//      case "K": English = "Night Safe Depository,"; break;
//      case "L": English = "Encryptor,"; break;
//      case "P": English = "SIU,"; break;
//      case "Q": English = "Touch Screen Keyboard,"; break;
//      case "R": English = "Supervisor,"; break;
//      case "V": English = "Statement Printer,"; break;
//      case "w": English = "Bunch Note Acceptor,"; break;
//      case "\\": English = "Envelope Dispenser,"; break;
//      case "q": English = " CPM (Cheque Processing Module),"; break;
//      case "Y": English = "Coin Dispenser,"; break;
//      case "f": English = "Barcode Reade,"; break;
//      default: English = "Unknown device,"; break;
//   }

//// Transaction Status
//English = English +  " Transaction Status : " + m.Groups["g2"].Value + ",";

//// Error Severity
//switch (m.Groups["g3"].Value.Trim())
//{
//   case "0": English = English + "No Error, Continue to Use,"; break;
//   case "1": English = English + "Routine, Continue to Use,"; break;
//   case "2": English = English + "Warning, Continue to Use,"; break;
//   case "3": English = English + "Suspend, Terminal will Suspend processing on completion of the current transaction, card holder tampering suspected, "; break;
//   case "4": English = English + "Fatal, Device is out of service, operator intervention is required, "; break;
//   default: English = English + " Unknown Severity,"; break;
//}
////\u001c(?<g3>.{0,14})\u001c(?<g4>.*)\u001c(?<g5>.{0,8})"
//// Diagnostic Status
//English = English + " M-Status : " + m.Groups["g4"].Value.Trim() + ",";

//// Supplies Status
//English = English + " Supplies-Status : " + m.Groups["g5"].Value.Trim() + ",";
