using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CustomerDataExtension : AVLine
   {
      private string className = "CustomerDataExtension";


      public string ExtensionState { get; set; } = string.Empty;

      public CustomerDataExtension(ILogFileHandler parent, string logLine, AVLogType awType = AVLogType.None) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         //[SymXchangeCustomerDataExtension] The 'SymXchangeCustomerDataExtension' extension is started.
         //[SymXchangeCustomerDataExtension] PerformQuery exception: 
         //[MessageId=getAccountSelectFields]  The requested record was not found : The requested record was not found
         //
         //Server stack trace: 
         //   at System.ServiceModel.Channels.ServiceChannel.HandleReply(ProxyOperationRuntime operation, ProxyRpc& rpc)
         //   at System.ServiceModel.Channels.ServiceChannel.Call(String action, Boolean oneway, ProxyOperationRuntime operation, Object[] ins, Object[] outs, TimeSpan timeout)
         //   at System.ServiceModel.Channels.ServiceChannelProxy.InvokeService(IMethodCallMessage methodCall, ProxyOperationRuntime operation)
         //   at System.ServiceModel.Channels.ServiceChannelProxy.Invoke(IMessage message)
         //
         //[0]: 2023-10-16 01:00:01 ActiveTeller Server version 1.3.1.0 is starting
         //   at System.Runtime.Remoting.Proxies.RealProxy.HandleReturnMessage(IMessage reqMsg, IMessage retMsg)
         //   at System.Runtime.Remoting.Proxies.RealProxy.PrivateInvoke(MessageData& msgData, Int32 type)
         //   at SymXchangeWSDL.SymXchangeAccountWSDL.AccountService.getAccountSelectFields(getAccountSelectFieldsRequest request)
         //   at SymXchangeWSDL.SymXchangeAccountWSDL.AccountServiceClient.SymXchangeWSDL.SymXchangeAccountWSDL.AccountService.getAccountSelectFields(getAccountSelectFieldsRequest request)
         //   at SymXchangeWSDL.SymXchangeAccountWSDL.AccountServiceClient.getAccountSelectFields(AccountSelectFieldsRequest Request)
         //   at NH.ActiveTeller.Server.Extensions.SymXchange.CustomerData.AccountQuery.PerformQuery(NameValueCollection criteria, AccountServiceClient proxy, AccountSelectFieldsRequest request)
         //[SymXchangeCustomerDataExtension] PerformQuery exception: 
         //System.ServiceModel.CommunicationException: The maximum message size quota for incoming messages (65536) has been exceeded. To increase the quota, use the MaxReceivedMessageSize property on the appropriate binding element. ---> System.ServiceModel.QuotaExceededException: The maximum message size quota for incoming messages (65536) has been exceeded. To increase the quota, use the MaxReceivedMessageSize property on the appropriate binding element.
         //   --- End of inner exception stack trace ---
         //
         //Server stack trace: 
         //   at System.ServiceModel.Channels.MessageEncoder.BufferMessageStream(Stream stream, BufferManager bufferManager, Int32 maxBufferSize)
         //   at System.ServiceModel.Channels.MessageEncoder.ReadMessage(Stream stream, BufferManager bufferManager, Int32 maxBufferSize, String contentType)
         //   at System.ServiceModel.Channels.HttpInput.ReadChunkedBufferedMessage(Stream inputStream)
         //   at System.ServiceModel.Channels.HttpInput.ParseIncomingMessage(HttpRequestMessage httpRequestMessage, Exception& requestException)
         //   at System.ServiceModel.Channels.HttpChannelFactory`1.HttpRequestChannel.HttpChannelRequest.WaitForReply(TimeSpan timeout)
         //   at System.ServiceModel.Channels.RequestChannel.Request(Message message, TimeSpan timeout)
         //   at System.ServiceModel.Dispatcher.RequestChannelBinder.Request(Message message, TimeSpan timeout)
         //   at System.ServiceModel.Channels.ServiceChannel.Call(String action, Boolean oneway, ProxyOperationRuntime operation, Object[] ins, Object[] outs, TimeSpan timeout)
         //   at System.ServiceModel.Channels.ServiceChannelProxy.InvokeService(IMethodCallMessage methodCall, ProxyOperationRuntime operation)
         //   at System.ServiceModel.Channels.ServiceChannelProxy.Invoke(IMessage message)
         //
         //[0]: 
         //   at System.Runtime.Remoting.Proxies.RealProxy.HandleReturnMessage(IMessage reqMsg, IMessage retMsg)
         //   at System.Runtime.Remoting.Proxies.RealProxy.PrivateInvoke(MessageData& msgData, Int32 type)
         //   at SymXchangeWSDL.SymXchangeAccountWSDL.AccountService.getAccountSelectFields(getAccountSelectFieldsRequest request)
         //   at SymXchangeWSDL.SymXchangeAccountWSDL.AccountServiceClient.SymXchangeWSDL.SymXchangeAccountWSDL.AccountService.getAccountSelectFields(getAccountSelectFieldsRequest request)
         //   at SymXchangeWSDL.SymXchangeAccountWSDL.AccountServiceClient.getAccountSelectFields(AccountSelectFieldsRequest Request)
         //   at NH.ActiveTeller.Server.Extensions.SymXchange.CustomerData.AccountQuery.PerformQuery(NameValueCollection criteria, AccountServiceClient proxy, AccountSelectFieldsRequest request)
         //[SymXchangeCustomerDataExtension] Could not find a customer with customer ID: 0009777818 and customer name: RIVERA,OMAR. Returning a default customer.


         string tag = $"[{avType.ToString()}]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + tag.Length).Trim();

            Regex regex = new Regex("PerformQuery exception:");
            Match m = regex.Match(logLine);
            if (m.Success)
            {
               ExtensionState = subLogLine;
               IsRecognized = true;
            }
            else
            {
               ExtensionState = logLine;
               IsRecognized = true;
            }
         }

         if (!IsRecognized)
         {
           throw new Exception($"AVLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
