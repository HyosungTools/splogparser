using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSamples
{
    public static class samples_signalr
    {
      public const string ACTIVETELLER_CONNECTION = @"2023-11-17 03:00:22 ActiveTeller connection state change Connecting
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
2023-09-25 06:41:55 ActiveTeller connection reconnecting";

      public const string ACTIVETELLER_HUB_CONNECTION_ERROR_1 = @"2023-09-25 06:41:58 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:
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
  at Microsoft.AspNet.SignalR.TaskAsyncHelper.TaskRunners`2.<>c__DisplayClass3_0.<RunTask>b__0(Task`1 t)";

      public const string ACTIVETELLER_HUB_CONNECTION_ERROR_2 = @"2023-09-25 06:42:02 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:
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
  at Microsoft.AspNet.SignalR.TaskAsyncHelper.TaskRunners`2.<>c__DisplayClass3_0.<RunTask>b__0(Task`1 t)";

      public const string ACTIVETELLER_HUB_CONNECTION_ERROR_3 = @"2023-09-25 06:42:10 ActiveTeller hub connection error. System.TimeoutException: Couldn't reconnect within the configured timeout of 00:00:15, disconnecting.";


      public const string ACTIVETELLER_HUB_CONNECTION_ERROR_4 = @"2023-09-25 01:04:11 ActiveTeller hub connection error. System.TimeoutException: Couldn't reconnect within the configured timeout of 00:00:15, disconnecting.";

      public const string ACTIVETELLER_HUB_CONNECTION_ERROR_5 = @"2023-09-25 06:42:02 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:";

      public const string ACTIVETELLER_HUB_CONNECTION_ERROR_6 = @"2023-09-25 12:12:30 ActiveTeller hub connection error. System.Net.Sockets.SocketException (0x80004005): An existing connection was forcibly closed by the remote host";

      public const string ACTIVETELLER_HTTPREQUEST_ERROR = @"failed to connect. System.Net.Http.HttpRequestException: An error occurred while sending the request. ---> System.Net.WebException: Unable to connect to the remote server ---> System.Net.Sockets.SocketException: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 192.168.5.33:80";

   }
}
