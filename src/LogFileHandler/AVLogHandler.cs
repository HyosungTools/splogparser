using System;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads Active Teller Server and Active Teller Server Extension logs 
   /// </summary>
   public class AVLogHandler : LogHandler, ILogFileHandler
   {
      public AVLogHandler(ICreateStreamReader createReader, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(ParseType.AV, createReader, Factory)
      {
         LogExpression = "ActiveTellerServer*.*";    // ActiveTellerServer, ActiveTellerServerExtensions
         Name = "AVLogFileHandler";
      }



      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
      {
         return traceFilePos >= logFile.Length;
      }

      /*
      /// <summary>
      /// Read one log line from a twlog file. 
      /// </summary>
      /// <returns></returns>
      public string ReadLine()
      {
         // builder will hold the line
         StringBuilder builder = new StringBuilder();

         bool endOfLine = false;

         // while not EOL or EOF
         while (!endOfLine && !EOF())
         {
            char c = logFile[traceFilePos];
            traceFilePos++;

            // check for end of line or end of file
            if (c == '\n' || EOF())
            {
               endOfLine = true;

               if (c == '\n')
               {
                  break;
               }
            }

            // ignore nulls and non-printing ASCII characters
            if (c > 0 && c < 128 && c != '\r')
            {
               builder.Append(c);
            }
         }

         return builder.ToString();
      }
      */

      public string ReadLine()
      {
         return ReadMultiLine();
      }

      /// <summary>
      /// Read one log line from a log file, assuring that each result begins with a timestamp and combining intermediate lines if necessary.
      /// </summary>
      /// <returns></returns>
      private string ReadMultiLine()
      {
         // builder will hold the line
         StringBuilder builder = new StringBuilder();

         bool endOfLine = false;

         // while not EOL or EOF
         while (!endOfLine && !EOF())
         {
            char c = logFile[traceFilePos];
            traceFilePos++;

            // check for end of line or end of file
            if (c == '\n' || EOF())
            {
               endOfLine = true;

               if (c == '\n')
               {
                  // look ahead to the next character, which is the first character on the next line (2023-mm-dd)
                  // continue collecting the currently line unless the next line begins with what looks like a timestamp
                  if (!EOF())
                  {
                     if (logFile[traceFilePos] != '2')
                     {
                        endOfLine = false;
                        continue;
                     }
                  }

                  break;
               }
            }

            // ignore nulls and non-printing ASCII characters
            if (c > 0 && c < 128 && c != '\r')
            {
               builder.Append(c);
            }
         }

         return builder.ToString();
      }


      public ILogLine IdentifyLine(string logLine)
      {
         /* ActiveTellerServer*.log
   2023-11-20 01:00:02 ActiveTeller Server version 1.3.0.0 is starting
   2023-11-20 01:00:03 Looking in 'D:\Inetpub\ActiveTellerWebsite\ActiveTeller\bin\NH.ActiveTeller.Server.Extensions.*.dll' for extensions

(SymXchange is one possible CORE software that banks use, others JXchange, etc)
   ActiveTellerServerExtensions*.log
   2023-11-20 01:00:04 [SymXchangeCustomerDataExtension] The 'SymXchangeCustomerDataExtension' extension is started.
   2023-11-20 01:01:09 [SymXchangeCustomerDataExtension] PerformQuery exception: 
   System.ServiceModel.FaultException: [MessageId=getAccountSelectFields]  The requested record was not found : The requested record was not found

   Server stack trace: 
      at System.ServiceModel.Channels.ServiceChannel.HandleReply(ProxyOperationRuntime operation, ProxyRpc& rpc)
      at System.ServiceModel.Channels.ServiceChannel.Call(String action, Boolean oneway, ProxyOperationRuntime operation, Object[] ins, Object[] outs, TimeSpan timeout)
      at System.ServiceModel.Channels.ServiceChannelProxy.InvokeService(IMethodCallMessage methodCall, ProxyOperationRuntime operation)
      at System.ServiceModel.Channels.ServiceChannelProxy.Invoke(IMessage message)

   Exception rethrown at [0]: 
      at System.Runtime.Remoting.Proxies.RealProxy.HandleReturnMessage(IMessage reqMsg, IMessage retMsg)
      at System.Runtime.Remoting.Proxies.RealProxy.PrivateInvoke(MessageData& msgData, Int32 type)
      at SymXchangeWSDL.SymXchangeAccountWSDL.AccountService.getAccountSelectFields(getAccountSelectFieldsRequest request)
      at SymXchangeWSDL.SymXchangeAccountWSDL.AccountServiceClient.SymXchangeWSDL.SymXchangeAccountWSDL.AccountService.getAccountSelectFields(getAccountSelectFieldsRequest request)
      at SymXchangeWSDL.SymXchangeAccountWSDL.AccountServiceClient.getAccountSelectFields(AccountSelectFieldsRequest Request)
      at NH.ActiveTeller.Server.Extensions.SymXchange.CustomerData.AccountQuery.PerformQuery(NameValueCollection criteria, AccountServiceClient proxy, AccountSelectFieldsRequest request)
         */

         //2023-10-16 01:01:08 [SymXchangeCustomerDataExtension]
         Regex regex = new Regex("\\[(?<extension>.*)CustomerDataExtension\\] (?<sublogline>.*)");
         Match m = regex.Match(logLine.Substring(20));
         if (m.Success)
         {
            // process ActiveTellerServerExtension log

            string coreBankingExtension = m.Groups["extension"].Value;
            string subLogLine = m.Groups["sublogline"].Value;

            // expected that all log lines in the ActiveTellerServerExtension log file are for the same extension type
            return new CustomerDataExtension(this, logLine, AVLine.GetCoreBankingAVLogType(coreBankingExtension));
         }

         // process ActiveTellerServer log

         return new Startup(this, logLine, AVLogType.Startup);
      }
   }
}
