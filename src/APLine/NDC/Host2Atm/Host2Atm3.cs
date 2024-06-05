using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Host2Atm3 : Host2Atm
   {
      public Host2Atm3(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_HOST2ATM3) : base(parent, logLine, apType)
      {
         msgclass = "3";
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
            // b     1     M        Message Class : '3' - Terminal Command
            // c     1     O        Response Flag - future use

            // Read over a, b and c
            Regex regex = new Regex("^(?<abc>[^\u001c]*)(?<rest>.*)?");
            Match m = regex.Match(ndcmsg);
            if (!m.Success)
            {
               return ;
            }

            english = "Data Command, ";

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

            // f     1        M     Message Sub-Class
            regex = new Regex("^\u001c(?<f>.)(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

            string messageSubClass = m.Groups["f"].Value;

            // g     1        M     Message Identifier
            regex = new Regex("^\u001c(?<g>.)(?<rest>.*)?");
            m = regex.Match(m.Groups["rest"].Value);
            if (!m.Success)
            {
               return ;
            }

            if (m.Groups["g"].Value.Length > 0)
            {
               if (messageSubClass == "1")
               {
                  switch (m.Groups["g"].Value[0])
                  {
                     case '1':
                        {
                           english += ScreenDataLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case '2':
                        {
                           english += StateTableLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case '3':
                        {
                           english += ConfigurationParameterLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case '5':
                        {
                           english += FITDataLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case '6':
                        {
                           english += ConfigurationIDNumberLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case 'A':
                        {
                           english += EnhancedConfigurationParameterLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case 'B':
                        {
                           english += MACFieldSelectionLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case 'C':
                        {
                           english += DateAndTimeLoad(m.Groups["rest"].Value);
                           break;
                        }
                     case 'E':
                        {
                           english += DispenserCurrencyCassetteMappingFile(m.Groups["rest"].Value);
                           break;
                        }
                     case 'I':
                        {
                           english += XMLConfigurationDownload(m.Groups["rest"].Value);
                           break;
                        }
                     default:
                        {
                           english += " Reserved,";
                           break;
                        }
                  }
               }
               else if (messageSubClass == "2")
               {
                  english += InteractiveTransactionResponse(m.Groups["rest"].Value);
               }

               else if (messageSubClass == "3")
               {
                  english += EncryptionKeyChange(m.Groups["rest"].Value);
               }

               else if (messageSubClass == "4")
               {
                  english += ExtendedEncryptionKeyChange(m.Groups["rest"].Value);
               }
            }
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return ;

      }

      public static string ScreenDataLoad(string ndcMessage)
         {
            string English = " Screen/Keyboard Data Load,";
            return English;
         }

         public static string StateTableLoad(string ndcMessage)
         {
            string English = " State Table Load,";

            // h     3        M     State Number
            Regex regex = new Regex("^\u001c(?<h>.{3})(?<rest>.*)?");
            Match m = regex.Match(ndcMessage);
            if (!m.Success)
               return English;

            English += "State Number : " + m.Groups["h"].Value + ",";

            // i     Var(25)   M     State Table Data
            //regex = new Regex("^(?<i>[^\u001c]*)(?<rest>.*)?");

            return English;
         }

         public static string ConfigurationParameterLoad(string ndcMessage)
         {
            string English = " Configuration Parameter Load,";
            return English;
         }

         public static string FITDataLoad(string ndcMessage)
         {
            string English = " FIT Data Load,";
            return English;
         }

         public static string ConfigurationIDNumberLoad(string ndcMessage)
         {
            string English = " Configuration ID Number Load,";
            return English;
         }

         public static string EnhancedConfigurationParameterLoad(string ndcMessage)
         {
            string English = " Enhanced Configuration Parameter Load,";
            return English;
         }

         public static string MACFieldSelectionLoad(string ndcMessage)
         {
            string English = " MAC Field Selection Load,";
            return English;
         }

         public static string DateAndTimeLoad(string ndcMessage)
         {
            string English = " Date and Time Load,";
            return English;
         }

         public static string DispenserCurrencyCassetteMappingFile(string ndcMessage)
         {
            string English = " Dispenser Currency Cassette Mapping File,";
            return English;
         }

         public static string XMLConfigurationDownload(string ndcMessage)
         {
            string English = " XML Configuration Download,";
            return English;
         }

         public static string InteractiveTransactionResponse(string ndcMessage)
         {
            string English = " Interactive Transaction Response,";
            return English;
         }

         public static string EncryptionKeyChange(string ndcMessage)
         {
            string English = " Encryption Key Change,";
            return English;
         }

         public static string ExtendedEncryptionKeyChange(string ndcMessage)
         {
            string English = " Extended Encryption Key Change,";
            return English;
         }
      }
   }

