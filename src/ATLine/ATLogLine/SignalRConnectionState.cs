using System;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class SignalRConnectionState : ATLine
   {
      public enum ConnectionStateEnum
      {
         None,
         Connecting,
         Reconnecting,
         Connected,
         Reconnected,
         Established,
         Registered,
         Disconnected,
         ExperiencingProblems,
         Exception
      }

      public ConnectionStateEnum ConnectionState { get; set; }
      public string Url { get; set; }
      public string ConnectionGuid { get; set; }
      public string Exception { get; set; }


      public SignalRConnectionState(ILogFileHandler parent, string logLine, ATLogType atType = ATLogType.ActiveTellerConnectionState) : base(parent, logLine, atType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         ConnectionState = ConnectionStateEnum.None;
         bool isHubConnectionError = false;

         /*
            2023-11-17 03:00:22 ActiveTeller connection state change Connecting
              2023-11-17 03:00:22 ActiveTeller connection established
              2023-11-17 03:00:22 ActiveTeller connection registered
              2023-11-17 09:24:30 ActiveTeller connection disconnected
            2023-11-17 09:24:30 ActiveTeller connection state change Connecting
              2023-11-17 09:24:30 ActiveTeller connection established
              2023-11-17 09:24:30 ActiveTeller connection registered
              2023-11-17 15:20:02 ActiveTeller connection reconnecting
              2023-11-17 15:20:02 ActiveTeller connection established
            2023-11-17 15:20:02 ActiveTeller connection 5613048e-f7be-4a8f-ab85-5d9b852b1dc1 reconnected to 'http://10.37.152.15:81/activeteller/signalr/'
            2023-11-17 15:20:02 ActiveTeller connection registered
            2023-11-17 18:07:50 ActiveTeller connection experiencing problems

            2023-09-25 06:41:55 ActiveTeller connection reconnecting
            2023-09-25 06:41:58 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:
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
            2023-09-25 06:42:02 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:
            {
              Cache-Control: private
              Date: Mon, 25 Sep 2023 12:38:17 GMT
              Server: Microsoft-IIS/10.0
              X-AspNet-Version: 4.0.30319
              X-Powered-By: ASP.NET
              Content-Length: 3502
              Content-Type: text/html; charset=utf-8
            }
               at Microsoft.AspNet.SignalR.Client.Http.DefaultHttpClient.<>c__DisplayClass5_0.<Get>b__1(HttpResponseMessage responseMessage)
               at Microsoft.AspNet.SignalR.TaskAsyncHelper.<>c__DisplayClass31_0`2.<Then>b__0(Task`1 t)
               at Microsoft.AspNet.SignalR.TaskAsyncHelper.TaskRunners`2.<>c__DisplayClass3_0.<RunTask>b__0(Task`1 t)
            2023-09-25 06:42:10 ActiveTeller hub connection error. System.TimeoutException: Couldn't reconnect within the configured timeout of 00:00:15, disconnecting.

         */

         string search = "ActiveTeller connection ";
         int idx = logLine.IndexOf(search);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + search.Length);

            switch (subLogLine)
            {
               case "established":
                  ConnectionState = ConnectionStateEnum.Established;
                  break;

               case "reconnecting":
                  ConnectionState = ConnectionStateEnum.Reconnecting;
                  break;

               case "registered":
                  ConnectionState = ConnectionStateEnum.Registered;
                  break;

               case "disconnected":
                  ConnectionState = ConnectionStateEnum.Disconnected;
                  break;

               case "experiencing problems":
                  ConnectionState = ConnectionStateEnum.ExperiencingProblems;
                  break;

               case "state change Connecting":
                  ConnectionState = ConnectionStateEnum.Connecting;
                  break;
            }

            //2023-11-17 15:20:02 ActiveTeller connection 5613048e-f7be-4a8f-ab85-5d9b852b1dc1 reconnected to 'http://10.37.152.15:81/activeteller/signalr/'

            search = "reconnected to";
            if (ConnectionState == ConnectionStateEnum.None && subLogLine.Contains(search))
            {
               Regex regex = new Regex("(?<guid>.*) reconnected to (?<url>.*)$");
               Match m = regex.Match(subLogLine);
               if (m.Success)
               {
                  ConnectionState = ConnectionStateEnum.Reconnected;
                  ConnectionGuid = m.Groups["guid"].Value;
                  Url = m.Groups["url"].Value.Replace("'", string.Empty);
               }
            }

            //"failed to connect. System.Net.Http.HttpRequestException: An error occurred while sending the request. ---> System.Net.WebException: Unable to connect to the remote server ---> System.Net.Sockets.SocketException: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 192.168.5.33:80"

            search = "failed to connect. ";
            if (ConnectionState == ConnectionStateEnum.None && subLogLine.Contains(search))
            {
               ConnectionState = ConnectionStateEnum.Exception;
               Exception = subLogLine.Substring(subLogLine.IndexOf(search) + search.Length);

               search = "connected host has failed to respond ";
               Url = subLogLine.Substring(subLogLine.IndexOf(search) + search.Length);
            }
         }

         //2023-09-25 01:04:11 ActiveTeller hub connection error. System.TimeoutException: Couldn't reconnect within the configured timeout of 00:00:15, disconnecting.
         //2023-09-25 06:42:02 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:
         //2023-09-25 12:12:30 ActiveTeller hub connection error. System.Net.Sockets.SocketException (0x80004005): An existing connection was forcibly closed by the remote host

         search = "ActiveTeller hub connection error. ";
         idx = logLine.IndexOf(search);
         if (idx != -1)
         {
            isHubConnectionError = true;

            ConnectionState = ConnectionStateEnum.Exception;
            Exception = logLine.Substring(idx + search.Length);
         }

         if (ConnectionState == ConnectionStateEnum.None && !isHubConnectionError)
         {
            throw new Exception($"ATLogLine.ConnectionState: did not recognize the log line '{logLine}'");
         }
      }
   }
}
