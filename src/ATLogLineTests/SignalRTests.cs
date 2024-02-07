using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using ATSamples;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Moq;
using Moq.Protected;
using Contract;
using System.Numerics;
using System;

namespace ATLogLineTests
{
   [TestClass]
    public class SignalRTests
    {
      [TestMethod]
      public void SignalRConnectionState_Established()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection established";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(SignalRConnectionState.ConnectionStateEnum.Established, line.ConnectionState);
         Assert.IsNull(line.Exception);
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_Registered()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection registered";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(SignalRConnectionState.ConnectionStateEnum.Registered, line.ConnectionState);
         Assert.IsNull(line.Exception);
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_Disconnected()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection disconnected";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(SignalRConnectionState.ConnectionStateEnum.Disconnected, line.ConnectionState);
         Assert.IsNull(line.Exception);
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_Connecting()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection state change Connecting";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(SignalRConnectionState.ConnectionStateEnum.Connecting, line.ConnectionState);
         Assert.IsNull(line.Exception);
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_Reconnecting()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection reconnecting";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(SignalRConnectionState.ConnectionStateEnum.Reconnecting, line.ConnectionState);
         Assert.IsNull(line.Exception);
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_Reconnected()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection 5613048e-f7be-4a8f-ab85-5d9b852b1dc1 reconnected to 'http://10.37.152.15:81/activeteller/signalr/'";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(SignalRConnectionState.ConnectionStateEnum.Reconnected, line.ConnectionState);
         Assert.AreEqual("http://10.37.152.15:81/activeteller/signalr/", line.Url);
         Assert.AreEqual("5613048e-f7be-4a8f-ab85-5d9b852b1dc1", line.ConnectionGuid);
         Assert.IsNull(line.Exception);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_ExperiencingProblems()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection experiencing problems";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(SignalRConnectionState.ConnectionStateEnum.ExperiencingProblems, line.ConnectionState);
         Assert.IsNull(line.Exception);
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_FailedToConnect()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection failed to connect. System.Net.Http.HttpRequestException: An error occurred while sending the request. ---> System.Net.WebException: Unable to connect to the remote server ---> System.Net.Sockets.SocketException: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 192.168.5.33:80";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.ConnectionState, SignalRConnectionState.ConnectionStateEnum.Exception);
         Assert.AreEqual("192.168.5.33:80", line.Url);
         Assert.AreEqual("System.Net.Http.HttpRequestException: An error occurred while sending the request. ---> System.Net.WebException: Unable to connect to the remote server ---> System.Net.Sockets.SocketException: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 192.168.5.33:80", line.Exception);
         Assert.IsNull(line.ConnectionGuid);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_HubConnectionError_Timeout()
      {
         string sampleLine = @"2023-09-25 01:04:11 ActiveTeller hub connection error. System.TimeoutException: Couldn't reconnect within the configured timeout of 00:00:15, disconnecting.";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.IsTrue(line.ConnectionState == SignalRConnectionState.ConnectionStateEnum.Exception);
         Assert.AreEqual(line.Exception, "System.TimeoutException: Couldn't reconnect within the configured timeout of 00:00:15, disconnecting.");
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_HubConnectionError_SocketException()
      {
         string sampleLine = @"2023-09-25 12:12:30 ActiveTeller hub connection error. System.Net.Sockets.SocketException (0x80004005): An existing connection was forcibly closed by the remote host";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.IsTrue(line.ConnectionState == SignalRConnectionState.ConnectionStateEnum.Exception);
         Assert.AreEqual(line.Exception, "System.Net.Sockets.SocketException (0x80004005): An existing connection was forcibly closed by the remote host");
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void SignalRConnectionState_HubConnectionError_HttpClientException()
      {
         string sampleLine = @"2023-09-25 06:41:58 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:
{
 Cache-Control: private
 Date: Mon, 25 Sep 2023 12:38:13 GMT
 Server: Microsoft-IIS/10.0
 X-AspNet-Version: 4.0.30319
 X-Powered-By: ASP.NET
 Content-Length: 3502
 Content-Type: text/html; charset=utf-8
}
at Microsoft.AspNet.SignalR.Client.Http.DefaultHttpClient.<>c__DisplayClass5_0.<Get>b__1(HttpResponseMessage responseMessage)
at Microsoft.AspNet.SignalR.TaskAsyncHelper.<>c__DisplayClass31_0`2.<Then>b__0(Task`1 t)
at Microsoft.AspNet.SignalR.TaskAsyncHelper.TaskRunners`2.<>c__DisplayClass3_0.<RunTask>b__0(Task`1 t)
";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         Assert.IsTrue(line.ConnectionState == SignalRConnectionState.ConnectionStateEnum.Exception);
         Assert.AreEqual(line.Exception, "Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:\r\n{\r\n Cache-Control: private\r\n Date: Mon, 25 Sep 2023 12:38:13 GMT\r\n Server: Microsoft-IIS/10.0\r\n X-AspNet-Version: 4.0.30319\r\n X-Powered-By: ASP.NET\r\n Content-Length: 3502\r\n Content-Type: text/html; charset=utf-8\r\n}\r\nat Microsoft.AspNet.SignalR.Client.Http.DefaultHttpClient.<>c__DisplayClass5_0.<Get>b__1(HttpResponseMessage responseMessage)\r\nat Microsoft.AspNet.SignalR.TaskAsyncHelper.<>c__DisplayClass31_0`2.<Then>b__0(Task`1 t)\r\nat Microsoft.AspNet.SignalR.TaskAsyncHelper.TaskRunners`2.<>c__DisplayClass3_0.<RunTask>b__0(Task`1 t)\r\n");
         Assert.IsNull(line.ConnectionGuid);
         Assert.IsNull(line.Url);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void SignalRConnectionState_Unsupported()
      {
         string sampleLine = @"2023-11-17 03:00:22 ActiveTeller connection unsupported text";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         SignalRConnectionState line = null;

         try
         {
            line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"ATLogLine.ConnectionState: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void SignalRConnectionState_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:22 not a connection state logline";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());

         try
         {
            ILogLine line = new SignalRConnectionState(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.AreEqual($"ATLogLine.ConnectionState: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
