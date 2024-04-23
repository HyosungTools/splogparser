using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class ATLogHandler : LogHandler, ILogFileHandler
   {
      public ATLogHandler(ICreateStreamReader createReader) : base(ParseType.AT, createReader)
      {
         LogExpression = "ActiveTellerAgent_*.*";
         Name = "ATLogFileHandler";
      }

      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public virtual bool EOF()
      {
         return traceFilePos >= logFile.Length;
      }

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
            traceFilePos++;  // advance for EOF() check

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

      public ILogLine IdentifyLine(string logLine)
      {
         /*
            2023-11-17 03:00:21 ActiveTeller Agent version 1.2.5.0 is starting
            2023-11-17 03:00:21 Agent configuration server Url is 'http://10.37.152.15:81/activeteller/'
            2023-11-17 03:00:21 Agent configuration reconnect interval is '5000'
            2023-11-17 03:00:21 Agent host started
            2023-11-17 03:00:21 Connection manager thread starting
            2023-11-17 03:00:21 Attempting to contact the ActiveTeller server
            2023-11-17 03:00:21 MoniPlus2sExtension agent extension found
            2023-11-17 03:00:21 Successfully contacted the ActiveTeller server
            2023-11-17 03:00:21 Connection manager registering client using device id 70-85-C2-18-7C-DA
            2023-11-17 03:00:22 NetOpExtension agent extension found
            2023-11-17 03:00:22 NextwareExtension agent extension found
            2023-11-17 03:00:22 TightVncExtension agent extension found
            2023-11-17 03:00:22 TightVncExtension agent extension is not enabled
            2023-11-17 03:00:22 MoniPlus2sExtension application handler added
            2023-11-17 03:00:22 MoniPlus2sExtension agent extension started
            2023-11-17 03:00:22 NetOpExtension remote control handler added
            2023-11-17 03:00:22 Remote Control handler NetOpExtension listening for an application message from MoniPlus2sExtension
            2023-11-17 03:00:22 NetOpExtension agent extension started
            2023-11-17 03:00:22 NextwareExtension device handler added
            2023-11-17 03:00:22 Device handler NextwareExtension listening for an application message from MoniPlus2sExtension
            2023-11-17 03:00:22 NextwareExtension agent extension started
            2023-11-17 03:00:22 Agent host started
            2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/auth/register server response Created
            2023-11-17 03:00:22 Connection manager preparing the ActiveTeller connection
            2023-11-17 03:00:22 Connection manager configuring the ActiveTeller hub proxy
            2023-11-17 03:00:22 Connection manager initiating ActiveTeller connection
            2023-11-17 03:00:22 ActiveTeller connection state change Connecting
            2023-11-17 03:00:22 Server offline. Unable to send Asset message
            2023-11-17 03:00:22 ActiveTeller connection established
            2023-11-17 03:00:22 Server reconnected. Sending last asset message for 
            2023-11-17 03:00:22 POST Asset message received in AssetMessageHandler
            2023-11-17 03:00:22 Connection manager registering asset using MAC Address 70-85-C2-18-7C-DA
            2023-11-17 03:00:22 Server message OnRequest for list of SystemSetting
            2023-11-17 03:00:22 Server message data [{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"False"},{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"},{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"},{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"True"},{"Name":"JournalHistory","Type":"System.Int32","Value":"999"},{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"},{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"},{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="},{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"},{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"},{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"},{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"999"},{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"999"}]
            2023-11-17 03:00:22 Received system settings broadcast. Remote Desktop Availability is set to Always. Attempting to start Remote Desktop
            2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/assets server response Created
            2023-11-17 03:00:22 ActiveTeller connection registered
            2023-11-17 03:00:22 GET OperatingMode message received in OperatingModeMessageHandler
            2023-11-17 03:00:22 GET http://10.37.152.15:81/activeteller/api/operatingmodes?AssetName= server response OK
            2023-11-17 03:00:22 POST AssetConfiguration message received in AssetConfigurationMessageHandler
            2023-11-17 03:01:57 Remote Control handler NetOpExtension received application message from MoniPlus2sExtension and is no longer waiting
            2023-11-17 03:01:57 Enabled devices are 'PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA'
            2023-11-17 03:01:57 Received asset message. Remote Desktop Availability is set to Always. Attempting to start Remote Desktop
            2023-11-17 03:01:57 POST Asset message received in AssetMessageHandler
            2023-11-17 03:01:57 POST http://10.37.152.15:81/activeteller/api/assets server response Created
            2023-11-17 03:01:57 POST AssetConfiguration message received in AssetConfigurationMessageHandler
            2023-11-17 03:01:57 POST http://10.37.152.15:81/activeteller/api/assetconfigurations server response Created
            2023-11-17 03:01:58 Remote Control handler NetOpExtension received application message from MoniPlus2sExtension and is no longer waiting
            2023-11-17 03:01:58 DELETE http://10.37.152.15:81/activeteller/api/devicestates?assetName=A036201&deviceId=2,4,8,A,B,F,J,K server response OK
            2023-11-17 03:01:58 Device handler is starting monitoring
            2023-11-17 03:01:58 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = 9
            2023-11-17 03:01:58 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:01:58 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = 7
            2023-11-17 03:01:58 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:01:59 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = N
            2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:01:59 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = 6
            2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:01:59 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = 1
            2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:01:59 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = Q
            2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:02:00 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = D
            2023-11-17 03:02:00 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:02:04 DeviceHandler received a DeviceState from NextwareExtension.  DeviceID = O
            2023-11-17 03:02:04 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
            2023-11-17 03:02:00 Enabled devices are 'PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA'
            2023-11-17 03:02:00 POST Asset message received in AssetMessageHandler
            2023-11-17 03:02:00 POST http://10.37.152.15:81/activeteller/api/assets server response Created
            2023-11-17 03:02:00 POST AssetConfiguration message received in AssetConfigurationMessageHandler
            2023-11-17 03:02:00 POST http://10.37.152.15:81/activeteller/api/assetconfigurations server response Created
            2023-11-17 03:02:07 POST ApplicationState message received in ApplicationStateMessageHandler
            2023-11-17 03:02:07 POST http://10.37.152.15:81/activeteller/api/applicationstates server response Created
            2023-11-17 03:02:07 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
            2023-11-17 03:02:07 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
            2023-11-17 03:02:07 Attempted to delete the TellerSessionRequest, but we didn't have one.
            2023-11-17 03:02:07 POST ApplicationState message received in ApplicationStateMessageHandler
            2023-11-17 03:02:07 PUT http://10.37.152.15:81/activeteller/api/applicationstates/11 server response OK
            2023-11-17 03:02:07 GET OperatingMode message received in OperatingModeMessageHandler
            2023-11-17 03:02:08 GET http://10.37.152.15:81/activeteller/api/operatingmodes?AssetName=A036201 server response OK
         	2023-11-17 09:24:00 ActiveTeller hub connection error. System.TimeoutException: The client has been inactive since 11/17/2023 2:23:32 PM and it has exceeded the inactivity timeout of 00:00:25. Stopping the connection.
         */

         /*
	Line   23: 2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/auth/register server response Created
	Line   36: 2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/assets server response Created
	Line   39: 2023-11-17 03:00:22 GET http://10.37.152.15:81/activeteller/api/operatingmodes?AssetName= server response OK
	Line   45: 2023-11-17 03:01:57 POST http://10.37.152.15:81/activeteller/api/assets server response Created
	Line   47: 2023-11-17 03:01:57 POST http://10.37.152.15:81/activeteller/api/assetconfigurations server response Created
	Line   49: 2023-11-17 03:01:58 DELETE http://10.37.152.15:81/activeteller/api/devicestates?assetName=A036201&deviceId=2,4,8,A,B,F,J,K server response OK
	Line   52: 2023-11-17 03:01:58 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   54: 2023-11-17 03:01:58 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
         */

         /*
	Line   23: 2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/auth/register server response Created
	Line   31: 2023-11-17 03:00:22 POST Asset message received in AssetMessageHandler
	Line   36: 2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/assets server response Created
	Line   40: 2023-11-17 03:00:22 POST AssetConfiguration message received in AssetConfigurationMessageHandler
	Line   44: 2023-11-17 03:01:57 POST Asset message received in AssetMessageHandler
	Line   45: 2023-11-17 03:01:57 POST http://10.37.152.15:81/activeteller/api/assets server response Created
	Line   46: 2023-11-17 03:01:57 POST AssetConfiguration message received in AssetConfigurationMessageHandler
	Line   47: 2023-11-17 03:01:57 POST http://10.37.152.15:81/activeteller/api/assetconfigurations server response Created
	Line   52: 2023-11-17 03:01:58 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   54: 2023-11-17 03:01:58 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   56: 2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   58: 2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   60: 2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   62: 2023-11-17 03:01:59 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   64: 2023-11-17 03:02:00 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   66: 2023-11-17 03:02:04 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   68: 2023-11-17 03:02:00 POST Asset message received in AssetMessageHandler
	Line   69: 2023-11-17 03:02:00 POST http://10.37.152.15:81/activeteller/api/assets server response Created
	Line   70: 2023-11-17 03:02:00 POST AssetConfiguration message received in AssetConfigurationMessageHandler
	Line   71: 2023-11-17 03:02:00 POST http://10.37.152.15:81/activeteller/api/assetconfigurations server response Created
	Line   72: 2023-11-17 03:02:07 POST ApplicationState message received in ApplicationStateMessageHandler
	Line   73: 2023-11-17 03:02:07 POST http://10.37.152.15:81/activeteller/api/applicationstates server response Created
	Line   77: 2023-11-17 03:02:07 POST ApplicationState message received in ApplicationStateMessageHandler
	Line   82: 2023-11-17 03:02:15 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   84: 2023-11-17 03:02:15 POST http://10.37.152.15:81/activeteller/api/devicestates server response Created
	Line   85: 2023-11-17 06:05:01 POST ApplicationState message received in ApplicationStateMessageHandler
          */

         /*
	Line   38: 2023-11-17 03:00:22 GET OperatingMode message received in OperatingModeMessageHandler
	Line   39: 2023-11-17 03:00:22 GET http://10.37.152.15:81/activeteller/api/operatingmodes?AssetName= server response OK
	Line   79: 2023-11-17 03:02:07 GET OperatingMode message received in OperatingModeMessageHandler
	Line   80: 2023-11-17 03:02:08 GET http://10.37.152.15:81/activeteller/api/operatingmodes?AssetName=A036201 server response OK
	Line  188: 2023-11-17 06:06:16 GET OperatingMode message received in OperatingModeMessageHandler
	Line  189: 2023-11-17 06:06:16 GET http://10.37.152.15:81/activeteller/api/operatingmodes?AssetName=A036201 server response OK
          */

         /*
   Line   78: 2023-11-17 03:02:07 PUT http://10.37.152.15:81/activeteller/api/applicationstates/11 server response OK
          */

         /*
	Line   49: 2023-11-17 03:01:58 DELETE http://10.37.152.15:81/activeteller/api/devicestates?assetName=A036201&deviceId=2,4,8,A,B,F,J,K server response OK
	Line   74: 2023-11-17 03:02:07 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
	Line   75: 2023-11-17 03:02:07 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
	Line   76: 2023-11-17 03:02:07 Attempted to delete the TellerSessionRequest, but we didn't have one.
         */


         //RESTful
         //Generic
         //Connections (outbound)

         if (logLine.Length >= 20)
         {

            //2023-11-17 03:00:22 Connection manager preparing the ActiveTeller connection
            //2023-11-17 03:00:22 Connection manager configuring the ActiveTeller hub proxy
            //2023-11-17 03:00:22 Connection manager initiating ActiveTeller connection
            //2023-11-17 03:00:22 Connection manager registering asset using MAC Address 70-85-C2-18-7C-DA
            //2023-11-17 03:00:21 Attempting to contact the ActiveTeller server
            //2023-11-17 03:00:21 Successfully contacted the ActiveTeller server
            if (logLine.Substring(20).StartsWith("Connection manager") || logLine.Substring(20).Contains("ActiveTeller server"))
            {
               return new ConnectionManagerAction(this, logLine, ATLogType.ConnectionManagerAction);
            }

            //2023-11-17 00:59:10 ActiveTeller connection reconnecting
            //2023-11-17 03:00:22 ActiveTeller connection state change Connecting

            //2023-09-25 01:04:11 ActiveTeller hub connection error. System.TimeoutException: Couldn't reconnect within the configured timeout of 00:00:15, disconnecting.

            //2023-09-25 06:42:02 ActiveTeller hub connection error. Microsoft.AspNet.SignalR.Client.HttpClientException: StatusCode: 500, ReasonPhrase: 'Internal Server Error', Version: 1.1, Content: System.Net.Http.StreamContent, Headers:
            //{
            //  Cache-Control: private
            //  Date: Mon, 25 Sep 2023 12:38:13 GMT
            //  Server: Microsoft-IIS/10.0
            //  X-AspNet-Version: 4.0.30319
            //  X-Powered-By: ASP.NET
            //  Content-Length: 3502
            //  Content-Type: text/html; charset=utf-8
            //}
            //    at Microsoft.AspNet.SignalR.Client.Http.DefaultHttpClient.<>c__DisplayClass5_0.<Get>b__1(HttpResponseMessage responseMessage)
            //    at Microsoft.AspNet.SignalR.TaskAsyncHelper.<>c__DisplayClass31_0`2.<Then>b__0(Task`1 t)
            //    at Microsoft.AspNet.SignalR.TaskAsyncHelper.TaskRunners`2.<>c__DisplayClass3_0.<RunTask>b__0(Task`1 t)

            //2023-09-25 12:12:30 ActiveTeller hub connection error. System.Net.Sockets.SocketException (0x80004005): An existing connection was forcibly closed by the remote host
            //    at System.Net.Sockets.Socket.EndReceive(IAsyncResult asyncResult)
            //    at System.Net.Sockets.NetworkStream.EndRead(IAsyncResult asyncResult)

            if (logLine.Substring(20).StartsWith("ActiveTeller connection") || logLine.Substring(20).StartsWith("ActiveTeller hub connection"))
            {
               return new SignalRConnectionState(this, logLine, ATLogType.ActiveTellerConnectionState);
            }


            //2023-11-17 03:00:22 TightVncExtension agent extension found
            //2023-11-17 03:00:22 TightVncExtension agent extension is not enabled
            //2023-11-17 03:00:22 MoniPlus2sExtension application handler added
            //2023-11-17 03:00:22 MoniPlus2sExtension agent extension started
            //2023-11-17 03:00:22 NetOpExtension remote control handler added

            //2023-11-17 03:00:22 Remote Control handler NetOpExtension listening for an application message from MoniPlus2sExtension
            //2023-11-17 03:00:22 Device handler NextwareExtension listening for an application message from MoniPlus2sExtension

            //2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/auth/register server response Created
            //2023-09-25 07:46:06 PUT http://192.168.5.33/activeteller/api/applicationstates/11 server response OK
            //2023-11-17 03:00:22 POST Asset message received in AssetMessageHandler

            //2023-11-17 03:00:22 Server offline. Unable to send Asset message
            //2023-11-17 03:00:22 ActiveTeller connection established
            //2023-11-17 03:00:22 Server reconnected. Sending last asset message for 

            //2023-11-17 03:00:22 Server message OnRequest for list of SystemSetting
            //2023-11-17 03:00:22 Server message data [{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"False"},{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"},{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"},{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"True"},{"Name":"JournalHistory","Type":"System.Int32","Value":"999"},{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"},{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"},{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="},{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"},{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"},{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"},{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"999"},{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"999"}]
            //2023-11-17 03:00:22 Received system settings broadcast. Remote Desktop Availability is set to Always. Attempting to start Remote Desktop
            //2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/assets server response Created
            //2023-09-25 07:46:06 PUT http://192.168.5.33/activeteller/api/applicationstates/11 server response OK  (?? ERROR ??)
            //2023-11-13 15:17:09 DELETE http://10.201.10.118/activeteller/api/devicestates?assetName=WI000902&deviceId=2,4,A,B,F,J,K server response OK

            //2023-11-17 03:00:22 ActiveTeller connection registered

            //2023-11-17 03:00:21 Agent host started
            if (logLine.Substring(20).StartsWith("Agent host"))
            {
               return new AgentHost(this, logLine, ATLogType.AgentHost);
            }

            //2023-11-17 03:00:21 Agent configuration reconnect interval is '5000'
            if (logLine.Substring(20).StartsWith("Agent configuration"))
            {
               return new AgentConfiguration(this, logLine, ATLogType.AgentConfiguration);
            }

            //2023-11-13 23:55:28 Server message OnUpdate for OperatingMode
            //2023-11-13 23:55:28 Server message data {"AssetId":5,"AssetName":"WI000902","ModeType":"Scheduled","ModeName":"SelfService","CoreStatus":"","CoreProperties":""}
            if (logLine.Substring(20).StartsWith("Server message") || logLine.Contains("server response"))
            {
               return new ServerRequests(this, logLine, ATLogType.ServerRequest);
            }

            //2023-09-25 01:05:09 POST ApplicationState message received in ApplicationStateMessageHandler
            //2023-09-25 01:05:09 GET OperatingMode message received in OperatingModeMessageHandler
            //2023-09-25 01:05:09 POST AssetConfiguration message received in AssetConfigurationMessageHandler
            //2023-09-25 02:15:11 POST ApplicationState message received in ApplicationStateMessageHandler
            //2023-09-25 02:15:11 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
            //2023-09-25 02:15:11 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
            //2023-09-25 02:15:11 Attempted to delete the TellerSessionRequest, but we didn't have one.
            if (logLine.Contains("message received in") || logLine.Substring(20).StartsWith("Attempted to "))
            {
               return new ServerRequests(this, logLine, ATLogType.ServerRequest);
            }
         }
         else
         {
            //??
            //{
            //  Cache-Control: private
            //  Date: Mon, 25 Sep 2023 12:38:13 GMT
            //  Server: Microsoft-IIS/10.0
            //  X-AspNet-Version: 4.0.30319
            //  X-Powered-By: ASP.NET
            //  Content-Length: 3502
            //  Content-Type: text/html; charset=utf-8
            //}
         }

         return new ATLine(this, logLine, ATLogType.None);
      }
   }
}
