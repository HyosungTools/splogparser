using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LogLineHandler
{
   public class ServerRequests : ATLine
   {
      public enum ServerRequestType
      {
         Unknown,
         SystemParameters,
         TerminalMode,
         RemoteDesktopAvailable,
         RemoteDesktopUnavailable,
         RemoteTellerSessionContactInfo,
         RemoteTellerSessionStart,
         RemoteTellerTaskEvent,
         TransactionDetails,
         RemoteTellerSessionRequestContext
      }

      public string Operation { get; set; } = string.Empty;
      public string ObjectType { get; set; } = string.Empty;
      public string ObjectHandler { get; set; } = string.Empty;


      /// <summary>
      /// GET, POST, etc
      /// </summary>
      public string RequestMethod { get; set; } = string.Empty;


      /// <summary>
      /// Full URL to the server
      /// </summary>
      public string RequestUrl { get; set; } = string.Empty;

      /// <summary>
      /// Domain part of the server URL
      /// </summary>
      public string RequestDomain { get; set; } = string.Empty;

      /// <summary>
      /// Path part of the server URL (URL minus the Domain part)
      /// </summary>
      public string RequestPath { get; set; } = string.Empty;

      public string RequestResult { get; set; } = string.Empty;

      public string RequestTimeUTC { get; set; } = string.Empty;
      public string RequestId { get; set; } = string.Empty;

      public long ClientSession { get; set; } = -1;
      public string AssetName { get; set; } = string.Empty;
      public string Terminal { get; set; } = string.Empty;
      public string SessionId { get; set; } = string.Empty;
      public string TellerName { get; set; } = string.Empty;
      public string TellerId { get; set; } = string.Empty;
      public string TellerUri { get; set; } = string.Empty;
      public string TaskName { get; set; } = string.Empty;
      public string EventName { get; set; } = string.Empty;
      public string FlowPoint { get; set; } = string.Empty;
      public string CustomerId { get; set; } = string.Empty;
      public string RequestName { get; set; } = string.Empty;
      public string RequestContext { get; set; } = string.Empty;
      public string ApplicationState { get; set; } = string.Empty;
      public string TransactionType { get; set; } = string.Empty;

      public string Payload { get; set; } = string.Empty;


      public ServerRequests(ILogFileHandler parent, string logLine, ATLogType atType = ATLogType.ServerRequest) : base(parent, logLine, atType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         /*
	         2023-11-13 04:53:23 Server message OnRequest for list of SystemSetting
	         2023-11-13 07:55:28 Server message OnUpdate for OperatingMode
	         2023-11-13 04:53:23 Server message data [{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"True"},{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"},{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"},{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"True"},{"Name":"JournalHistory","Type":"System.Int32","Value":"60"},{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"},{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"},{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="},{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"},{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"},{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"},{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"60"},{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"7"}]
	         2023-11-13 07:55:28 Server message data {"AssetId":5,"AssetName":"WI000902","ModeType":"Scheduled","ModeName":"SelfService","CoreStatus":"","CoreProperties":""}

	         2023-11-13 07:55:29 Server message OnUpdate for OperatingMode
	         2023-11-13 07:55:29 Server message data {"AssetId":5,"AssetName":"WI000902","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}
	         2023-11-13 08:16:32 Server message OnDelete for TellerSessionRequest
	         2023-11-13 08:16:32 Server message data {"Id":155420,"AssetName":"WI000902","Timestamp":"2023-11-13T08:14:18.303","CustomerId":"","FlowPoint":"Common-RequestAssistance","RequestContext":"TellerIdentificationButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"ClientSessionId":"6782","SupportedCallType":"BeeHD"}}

            2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/assets server response Created
            2023-09-25 07:46:06 PUT http://192.168.5.33/activeteller/api/applicationstates/11 server response OK  (?? ERROR ??)
            2023-11-13 15:17:09 DELETE http://10.201.10.118/activeteller/api/devicestates?assetName=WI000902&deviceId=2,4,A,B,F,J,K server response OK

            2023-09-25 01:05:09 POST ApplicationState message received in ApplicationStateMessageHandler
            2023-09-25 01:05:09 GET OperatingMode message received in OperatingModeMessageHandler
            2023-09-25 01:05:09 POST AssetConfiguration message received in AssetConfigurationMessageHandler
            2023-09-25 02:15:11 POST ApplicationState message received in ApplicationStateMessageHandler
            2023-09-25 02:15:11 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
            2023-09-25 02:15:11 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler
            2023-09-25 02:15:11 Attempted to delete the TellerSessionRequest, but we didn't have one.
         */

         int idx = logLine.IndexOf("Server message data ");
         if (idx != (-1))
         {
            string subLogLine = logLine.Substring(idx + "Server message data ".Length);

            if ((subLogLine.StartsWith("{") && subLogLine.EndsWith("}")) ||
               (subLogLine.StartsWith("[") && subLogLine.EndsWith("]")))
            {
               IsRecognized = true;
               Operation = "Message received";
               Payload = subLogLine;
            }
         }

         idx = logLine.IndexOf("Server message On");
         if (idx != (-1))
         {
            string subLogLine = logLine.Substring(idx + "Server message On".Length - 2);

            //OnRequest for list of SystemSetting
            //OnAdd for RemoteControlTask
            //OnDelete for TellerSessionRequest
            //OnUpdate for OperatingMode

            Regex regex = new Regex("^(?<type>.*) for (?<list>list of ){0,1}(?<object>.*)$");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               Operation = m.Groups["type"].Value;
               ObjectType = $"{m.Groups["list"].Value}{m.Groups["object"].Value}";
            }
         }

         idx = logLine.IndexOf("server response");
         if (idx != (-1))
         {
            //2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/assets server response Created
            //2023-09-25 07:46:06 PUT http://192.168.5.33/activeteller/api/applicationstates/11 server response OK  (?? ERROR ??)
            //2023-11-13 15:17:09 DELETE http://10.201.10.118/activeteller/api/devicestates?assetName=WI000902&deviceId=2,4,A,B,F,J,K server response OK

            string subLogLine = logLine.Substring(20);

            Regex regex = new Regex("^(?<request>.*) (?<url>.*) server response (?<result>.*)$");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;

               RequestMethod = m.Groups["request"].Value.ToUpper();
               RequestUrl = m.Groups["url"].Value;
               RequestPath = RequestUrl.Substring(RequestUrl.ToLower().IndexOf("/activeteller"));
               RequestDomain = RequestUrl.Substring(0, RequestUrl.ToLower().IndexOf("/activeteller"));
               RequestResult = m.Groups["result"].Value;
            }
         }

         idx = logLine.IndexOf("message received in");
         if (idx != (-1))
         {
            //POST ApplicationState message received in ApplicationStateMessageHandler
            string subLogLine = logLine.Substring(20);

            Regex regex = new Regex("^(?<request>.*) (?<type>.*) message received in (?<handler>.*)$");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;

               Operation = m.Groups["request"].Value.ToLower();
               ObjectType = m.Groups["type"].Value;
               ObjectHandler = m.Groups["handler"].Value;
            }
         }

         idx = logLine.IndexOf("Attempted to ");
         if (idx != (-1))
         {
            //Attempted to delete the TellerSessionRequest, but we didn't have one.

            string subLogLine = logLine.Substring(20);

            Regex regex = new Regex("^Attempted to (?<request>.*) the (?<type>.*), but we didn't have one.$");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;

               Operation = m.Groups["request"].Value;
               ObjectType = m.Groups["type"].Value;
               ObjectHandler = "ERROR - NO HANDLER";
            }
         }

         /* Payloads in a teller session - which fields can be split out into columns to produce something readable?
          * MoniPlus2sExtensions does the same thing
          * 
            SystemParameters
            [{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"False"},{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"},{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"},{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"False"},{"Name":"JournalHistory","Type":"System.Int32","Value":"90"},{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"},{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"},{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="},{"Name":"SavedImagesHistory","Type":"System.Int32","Value":"90"},{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"},{"Name":"SmtpFromEmail","Type":"System.String","Value":"noreply@domain.com"},{"Name":"SmtpPassword","Type":"System.Security.SecureString","Value":"Z7BKbZXrF+PMTVNvarcz4Q=="},{"Name":"SmtpPortNumber","Type":"System.String","Value":"587"},{"Name":"SmtpServer","Type":"System.String","Value":"127.0.0.1"},{"Name":"SmtpUsername","Type":"System.String","Value":"admin"},{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"},{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"},{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"60"},{"Name":"TwilioAccountId","Type":"System.Security.SecureString","Value":"KXwVYDO5FJ1CAQSc359+C1E9GzftP0uwtPzE6jsCui9VydQ4Sm9NKUMZ5rWxtGlG"},{"Name":"TwilioApiKey","Type":"System.Security.SecureString","Value":"XDL1Pih/5D3gOJTc+rRnFMF3+NJHPgoaQDv61cIk62++YER61nDveWllUgZgsS/5"},{"Name":"TwilioPhoneNumber","Type":"System.String","Value":"+19375551212"},{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"7"},{"Name":"VideoRecordingHistory","Type":"System.Int32","Value":"90"}]

            TerminalMode
            {"AssetId":11,"AssetName":"NM000559","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}

            TellerSessionStart
            {"StartTime":null,"Id":2236837,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:11:57.9696555-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

            TransactionDetails
            {"Id":152341,"AssetName":"NM000559","TellerSessionId":24022,"TransactionDetail":{"Accounts":[{"Action":1,"Amount":"220000","Warnings":[],"AccountType":"SHARE","Id":41478,"TransactionDetailId":20219,"Review":{"Id":31557,"Approval":{"TellerAmount":null,"TellerApproval":0,"Reason":null,"TransactionItemReviewId":31557},"ReasonForReview":0,"TransactionItemId":41478}}],"Id":20219,"TellerSessionActivityId":152341,"TransactionType":"CheckDeposit","ApproverId":null,"IdScans":[],"Checks":[{"AcceptStatus":"CHANGE","Amount":"220000","AmountRead":"1500","AmountScore":216,"BackImageRelativeUri":"api/checkimages/46820","CheckDateRead":"9/18/2023","CheckDateScore":63,"CheckIndex":0,"FrontImageRelativeUri":"api/checkimages/46819","ImageBack":"D:\\CHECK21\\Bottom1.jpg","ImageFront":"D:\\CHECK21\\Top1.jpg","InvalidReason":"","Id":41477,"TransactionDetailId":20219,"Review":{"Id":31556,"Approval":{"TellerAmount":"220000","TellerApproval":2,"Reason":"","TransactionItemReviewId":31556},"ReasonForReview":1,"TransactionItemId":41477}}],"TransactionCashDetails":[],"TransactionOtherAmounts":[],"TransactionWarnings":[]},"Timestamp":"2023-09-25T09:40:25.7365541-07:00","TellerInfo":{"ClientSessionId":3895,"TellerName":"Jesus","VideoConferenceUri":"10.255.254.247","TellerId":"jpinon"}}

            RemoteTellerSelected
            {"Id":24022,"AssetName":"NM000559","TellerSessionRequestId":30981,"Timestamp":"2023-09-25T09:38:43.3973841-07:00","TellerInfo":{"ClientSessionId":3895,"TellerName":"Jesus","VideoConferenceUri":"10.255.254.247","TellerId":"jpinon"}}
         
            RemoteTellerAvailable
            {"AssetName":"21PLEA03D","TellerSessionRequestId":20628,"Availability":1,"ExternalRoutingIdentifier":null,"Timestamp":"2023-11-17T18:23:07.4571957-06:00"}
            {"AssetName":"21PLEA03D","TellerSessionRequestId":20628,"Availability":1,"ExternalRoutingIdentifier":null,"Timestamp":"2023-11-17T18:23:07.4571957-06:00"}

            TellerTaskEvent
            {"TaskId":25,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationRequest","Data":"{\"Name\":\"ConfigurationRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-13T10:27:47.3758284-06:00\",\"TaskTimeout\":null}","Id":2237013,"AssetName":"WI000902","TellerSessionId":147548,"TransactionDetail":null,"Timestamp":"2023-11-13T10:27:47.379449-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

            SelfServiceFlow
            {"Id":31114,"AssetName":"NM000559","Timestamp":"2023-09-25T15:26:54.797","CustomerId":"0009652240","FlowPoint":"Common-ProcessTransactionReview","RequestContext":"TransactionRequiringReview","ApplicationState":"InTransaction","TransactionType":"Deposit","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
          */

         if (IsRecognized && !string.IsNullOrEmpty(Payload))
         {
            List<ExpandoObject> json = new List<ExpandoObject>();

            try
            {
               if (Payload.StartsWith("[") && Payload.EndsWith("]"))
               {
                  // only SystemParameters - ignore, nothing useful

                  // TODO
                  // json = JsonConvert.DeserializeObject<List<ExpandoObject>>(Payload, new ExpandoObjectConverter());
               }
               else
               {
                  json.Add(JsonConvert.DeserializeObject<ExpandoObject>(Payload, new ExpandoObjectConverter()));
               }

               ServerRequestType requestType = ServerRequestType.Unknown;
               
               foreach (ExpandoObject obj in json)
               {
                  // categorize the type of object

                  IDictionary<string, object> dict = (IDictionary<string, object>)obj;
                  object fieldvalue;

                  // simple cases first, requiring only one field name to know the request type
                  if (dict.TryGetValue("Name", out fieldvalue))
                  {
                     requestType = ServerRequestType.SystemParameters;
                  }

                  else if (dict.TryGetValue("TaskName", out fieldvalue))
                  {
                     requestType = ServerRequestType.RemoteTellerTaskEvent;
                  }

                  else if (dict.TryGetValue("FlowPoint", out fieldvalue))
                  {
                     requestType = ServerRequestType.RemoteTellerSessionRequestContext;
                  }

                  else if (dict.TryGetValue("Availability", out fieldvalue))
                  {
                     if (fieldvalue.ToString() == "1")
                     {
                        requestType = ServerRequestType.RemoteDesktopAvailable;
                     }
                     else
                     {
                        requestType = ServerRequestType.RemoteDesktopUnavailable;
                     }
                  }

                  else if (dict.TryGetValue("TellerSessionRequestId", out fieldvalue))
                  {
                     requestType = ServerRequestType.RemoteTellerSessionContactInfo;
                  }

                  else if (dict.TryGetValue("ModeType", out fieldvalue))
                  {
                     requestType = ServerRequestType.TerminalMode;
                  }

                  else if (dict.TryGetValue("StartTime", out fieldvalue))
                  {
                     requestType = ServerRequestType.RemoteTellerSessionStart;
                  }

                  // TellerSessionStart also has TransactionDetail - so do this check last
                  else if (dict.TryGetValue("TransactionDetail", out fieldvalue) && fieldvalue != null)
                  {
                     foreach (KeyValuePair<string,object> detail in (ExpandoObject)fieldvalue)
                     {
                        if (detail.Key == "Accounts")
                        {
                           requestType = ServerRequestType.TransactionDetails;
                        }
                     }
                  }

                  else
                  {
                     requestType = ServerRequestType.Unknown;
                  }


                  // messages contain clues to the times on Server and ATM machines.  Teller workstation does not have such timestamp, another method must be used.
                  //
                  // ParseType (use to identify the type of machine where the log was generated)
                  // Sent/Receive (message direction indicates the source and destination machines)
                  //
                  // RECEIVED AT ATM FROM SERVER
                  //(ActiveTellerAgent_20231104_030107.log)           2023-11-04 10:45:07 Server message data {"Id":34442,"AssetName":"NM000562","TellerSessionRequestId":43278,"Timestamp":"2023-11-04T09:48:07.4570066-07:00","TellerInfo":{"ClientSessionId":5140,"TellerName":"Jorge","VideoConferenceUri":"192.168.20.142","TellerId":"jocadena"}}
                  //(ActiveTellerAgentExtensions_20231104_030107.log) 2023-11-04 10:45:07 [MoniPlus2sExtension] Sending TellerSession to application: {"Id":34442,"AssetName":"NM000562","TellerSessionRequestId":43278,"Timestamp":"2023-11-04T09:48:07.4570066-07:00","TellerInfo":{"ClientSessionId":5140,"TellerName":"Jorge","VideoConferenceUri":"192.168.20.142","TellerId":"jocadena"}}
                  //
                  //(ActiveTellerAgent_20231104_030107.log)           2023-11-04 11:06:20 Server message data {"TaskId":9,"TaskName":"ScanIdTask","EventName":"ScanIdRequest","Data":"{\"Name\":\"ScanIdRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-04T10:09:20.7777751-07:00\",\"TaskTimeout\":null}","Id":228115,"AssetName":"NM000562","TellerSessionId":34462,"TransactionDetail":null,"Timestamp":"2023-11-04T10:09:20.7827608-07:00","TellerInfo":{"ClientSessionId":5140,"TellerName":"Jorge","VideoConferenceUri":"192.168.20.142","TellerId":"jocadena"}}
                  //(ActiveTellerAgentExtensions_20231104_030107.log) 2023-11-04 11:06:20 [MoniPlus2sExtension] Sending RemoteControlTaskMessage to application: {"AssetName":"NM000562","TaskId":9,"TaskName":"ScanIdTask","EventName":"ScanIdRequest","EventData":"{\"Name\":\"ScanIdRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-04T10:09:20.7777751-07:00\",\"TaskTimeout\":null}","Extras":null,"TransactionData":null,"TellerInfo":{"ClientSessionId":5140,"TellerName":"Jorge","VideoConferenceUri":"192.168.20.142","TellerId":"jocadena"}}
                  //
                  // ATM RESPONSE SENT TO SERVER (with FlowPoint)
                  //                                                  2023-11-04 11:06:26 [MoniPlus2sExtension] Firing agent message event: RemoteControlEvent - POST - {"TaskId":9,"TaskName":"ScanIdTask","EventName":"IdScanCompleted","Data":"{\"Name\":\"IdScanCompleted\",\"Detail\":\"OK\",\"TransactionDetail\":{\"Accounts\":null,\"Id\":0,\"TellerSessionActivityId\":0,\"TransactionType\":null,\"ApproverId\":null,\"IdScans\":[{\"BackImageName\":null,\"BackImageRelativeUri\":null,\"FrontImageName\":\"C:\\\\IDSImages\\\\FRONT_20231104_110620.JPG\",\"FrontImageRelativeUri\":null,\"ScanIndex\":0,\"Id\":0,\"TransactionDetailId\":0,\"Review\":null}],\"Checks\":null,\"TransactionCashDetails\":null,\"TransactionOtherAmounts\":null,\"TransactionWarnings\":null}}","Id":0,"AssetName":"NM000562","TellerSessionId":34462,"TransactionDetail":{"Accounts":null,"Id":0,"TellerSessionActivityId":0,"TransactionType":null,"ApproverId":null,"IdScans":[{"BackImageName":null,"BackImageRelativeUri":null,"FrontImageName":"C:\\IDSImages\\FRONT_20231104_110620.JPG","FrontImageRelativeUri":null,"ScanIndex":0,"Id":0,"TransactionDetailId":0,"Review":null}],"Checks":null,"TransactionCashDetails":null,"TransactionOtherAmounts":null,"TransactionWarnings":null},"Timestamp":"2023-11-04T11:06:26.219672-07:00","TellerInfo":{"ClientSessionId":5140,"TellerName":null,"VideoConferenceUri":null,"TellerId":null}}
                  //
                  // TELLER SENT TO ATM VIA THE SERVER
                  //(Workstation20231104.log.1210.bak)                [2023-11-04 11:09:20-839][3][OnExecute           ]Executing ScanIdTask 9
                  //
                  // RECEIVED AT SERVER FROM TELLER
                  //???
                  //
                  // SENT BY SERVER TO ATM
                  //???
                  //
                  // RECEIVED AT WORKSTATION FROM SERVER
                  //(Workstation20231104.log.1210.bak)                [2023-11-04 11:09:26-593][3][DataFlowManager     ]Received IdScanCompleted event for ScanIdTask 9 for asset NM000562
                  //
                  // ATM RESPONSE SENT TO SERVER (with FlowPoint)
                  //2023-11-04 11:06:26 [MoniPlus2sExtension] Firing agent message event: RemoteControlEvent - POST - {"TaskId":9,"TaskName":"ScanIdTask","EventName":"IdScanCompleted","Data":"{\"Name\":\"IdScanCompleted\",\"Detail\":\"OK\",\"TransactionDetail\":{\"Accounts\":null,\"Id\":0,\"TellerSessionActivityId\":0,\"TransactionType\":null,\"ApproverId\":null,\"IdScans\":[{\"BackImageName\":null,\"BackImageRelativeUri\":null,\"FrontImageName\":\"C:\\\\IDSImages\\\\FRONT_20231104_110620.JPG\",\"FrontImageRelativeUri\":null,\"ScanIndex\":0,\"Id\":0,\"TransactionDetailId\":0,\"Review\":null}],\"Checks\":null,\"TransactionCashDetails\":null,\"TransactionOtherAmounts\":null,\"TransactionWarnings\":null}}","Id":0,"AssetName":"NM000562","TellerSessionId":34462,"TransactionDetail":{"Accounts":null,"Id":0,"TellerSessionActivityId":0,"TransactionType":null,"ApproverId":null,"IdScans":[{"BackImageName":null,"BackImageRelativeUri":null,"FrontImageName":"C:\\IDSImages\\FRONT_20231104_110620.JPG","FrontImageRelativeUri":null,"ScanIndex":0,"Id":0,"TransactionDetailId":0,"Review":null}],"Checks":null,"TransactionCashDetails":null,"TransactionOtherAmounts":null,"TransactionWarnings":null},"Timestamp":"2023-11-04T11:06:26.219672-07:00","TellerInfo":{"ClientSessionId":5140,"TellerName":null,"VideoConferenceUri":null,"TellerId":null}}
                  //2023-09-25 00:26:19 [MoniPlus2sExtension] Firing agent message event: ApplicationState - POST - {"Id":0,"AssetName":"NM000559","ApplicationAvailability":0,"Customer":{"CustomerId":""},"Timestamp":"2023-09-25T00:26:19.0346019-07:00","FlowPoint":"Common-InsertCardRead","State":"Identification","OperatingMode":"SelfService","TransactionType":"","Language":"English","VoiceGuidance":false}
                  //2023-09-25 00:26:41 [MoniPlus2sExtension] Firing agent message event: ApplicationState - POST - {"Id":0,"AssetName":"NM000559","ApplicationAvailability":0,"Customer":{"CustomerId":"0009754489"},"Timestamp":"2023-09-25T00:26:41.1266685-07:00","FlowPoint":"Common-DetermineDoYouWantBalanceInquiry","State":"Identification","OperatingMode":"SelfService","TransactionType":"CustomerIdentification","Language":"English","VoiceGuidance":false}
                  //
                  // RECEIVED AT SERVER FROM ATM (has FlowPoint)
                  //(ActiveTellerServer_20231128_010002.log)          2023-11-28 08:01:11 TellerRequestManager.HandleTellerSessionRequest handled tellerRequest {"Id":23565,"AssetName":"21PLEA04D","Timestamp":"2023-11-28T08:01:13.3746831-06:00","CustomerId":"0000632448","CustomerName":"PHILLIPS,CAMERON","FlowPoint":"Common-RequestAssistance","RequestContext":"HelpButton","ApplicationState":"MainMenu","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
                  //
                  // RECEIVED AT SERVER FROM ATM
                  //(ActiveTellerServer.log)                          2023-10-16 08:48:11 Get - /activeteller/api/TellerActivities?userid=20&start=2023-10-16T00%3a00%3a00.000-06%3a00&end=2023-10-16T23%3a59%3a59.000-06%3a00
                  /*(ActiveTellerServer.log)                          2023-10-16 08:48:17 The following exception occurred while processing teller activity detail to display:
                                                                        System.ArgumentNullException: Value cannot be null.
                                                                        Parameter name: format
                                                                           at System.String.FormatHelper(IFormatProvider provider, String format, ParamsArray args)
                                                                           at NH.ActiveTeller.Server.Observers.TellerSessionJournalWriter.BuildActivityDescription(TellerActivity tellerActivity)
                                                                           at NH.ActiveTeller.Server.Providers.TellerActivityProvider.MapRecordToModel(TellerJournalRecord record)
                                                                        {"Id":357996,"ActivityDescription":"Teller session start","ActivityDetail":null,"ActivityDetailToDisplay":"TellerSession_Insert_Withdrawal","ActivityName":"Withdrawal","ActivityState":"Insert","ActivityType":"TellerSession","AssetName":"TX005019","BranchName":"Zaragoza","BranchNumber":"310","ClientSessionId":4541,"CustomerName":"DURAN,ARLEEN","SourceApplication":2,"SystemActivity":0,"TellerControlTaskId":null,"TellerSessionId":29411,"Timestamp":"2023-10-16T08:07:23.92","UserId":20,"UserName":"kpetroni"}
                                                                        {"Id":358016,"ActivityDescription":"Teller session start","ActivityDetail":null,"ActivityDetailToDisplay":"TellerSession_Insert_PostIdle","ActivityName":"PostIdle","ActivityState":"Insert","ActivityType":"TellerSession","AssetName":"TX005016","BranchName":"Lee Trevino","BranchNumber":"305","ClientSessionId":4541,"CustomerName":"","SourceApplication":2,"SystemActivity":0,"TellerControlTaskId":null,"TellerSessionId":29414,"Timestamp":"2023-10-16T08:19:53.743","UserId":20,"UserName":"kpetroni"}
                  //
                  // SENT TO WORKSTATION FROM SERVER
                  //???

                  // AT SERVER
                  //   TellerRequestManager
                    2023-11-17 08:00:58 TellerRequestManager.HandleTellerSessionRequest is using RoutingRule.PREFER_BRANCH for tellerRequest from 21PLEA03D
	                 2023-11-17 08:00:58 TellerRequestManager.HandleTellerSessionRequest handled tellerRequest {"Id":20055,"AssetName":"21PLEA03D","Timestamp":"2023-11-17T08:01:02.043053-06:00","CustomerId":"","CustomerName":"","FlowPoint":"Common-RequestAssistance","RequestContext":"HelpButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}




                  2023-11-28 10:51:14 Get - /ActiveTeller/api/v2/TellerTransactions?userid=2%2c3%2c12%2c5%2c6%2c8%2c4%2c20%2c27%2c29%2c34%2c36%2c26%2c28%2c22%2c35%2c14%2c24%2c37%2c38%2c43%2c39%2c15%2c42%2c16%2c31%2c33%2c45%2c40%2c44%2c46%2c41%2c52%2c51%2c47%2c54%2c55%2c48%2c53%2c49%2c50%2c56&assetname=12PROS04D%2c14STUR02L%2c12PROS01L%2c14STUR05D%2c18OAKCRK01L%2c21PLEA01L%2c21PLEA02L%2c09KENO02D%2c05APP01L%2c07LOO01D%2c09KENO01D%2c12PROS03D%2c21PLEA03D%2c14STUR03D%2c18OAKCRK03D%2c16CENT04D%2c16CENT06D%2c18OAKCRK04D%2c14STUR01L%2c09KENO03D%2c18OAKCRK02L%2c12PROS02L%2c25MUK02D%2c14STUR04D%2c25MUK01L%2c21PLEA04D%2c16CENT01L%2c27MTSIN01L&start=2023-11-27T00%3a00%3a00.000-06%3a00&end=2023-11-27T23%3a59%3a59.000-06%3a00&transactionstatus=1,2

ActiveTellerServer_20231128_010002.log
2023-11-28 08:27:33 Connection 39a56020-c423-4623-a3a2-4c87f9ac42ce: OnConnected event fired.
2023-11-28 08:27:33 Connection 39a56020-c423-4623-a3a2-4c87f9ac42ce: UpdateTellerSessionStatisticsSubscription was called with True
2023-11-28 08:27:33 UpdateSubscriptionForSelections: clientSessionId: 4292, old: 0, new: 29
2023-11-28 08:27:33 Connection 39a56020-c423-4623-a3a2-4c87f9ac42ce: Connection was added to the connectionMap. Count=35
2023-11-28 08:27:33 Connection 39a56020-c423-4623-a3a2-4c87f9ac42ce: Connection was added to the clientSessionMap for clientSessionId 4292. Count=8
2023-11-28 08:27:33 Connection 39a56020-c423-4623-a3a2-4c87f9ac42ce: starting SendOpenTellerSessionRequests
2023-11-28 08:27:33 Connection 39a56020-c423-4623-a3a2-4c87f9ac42ce: finishing SendOpenTellerSessionRequests
2023-11-28 08:27:33 SetTellerAvailability was called with available for client session 4292.
2023-11-28 08:27:33 [BEFORE] RemotePool.FireResourceAvailable - tellerResource.Id = 4292; UnavailableResources.Count = 0; AssignedList.Count = 0; AvailableResources.Count = 7
2023-11-28 08:27:33 [AFTER] RemotePool.FireResourceAvailable - tellerResource.Id = 4292; UnavailableResources.Count = 0; AssignedList.Count = 0; AvailableResources.Count = 8
..
2023-11-28 11:01:28 Connection c421f779-9a47-4f3b-848b-f894dfa6cbdd: Connection was updated in the connectionMap. Count=37
2023-11-28 11:01:28 Connection c421f779-9a47-4f3b-848b-f894dfa6cbdd: Connection was updated in the assetMap for '12PROS04D'. Count=27
2023-11-28 11:01:28 Post - /ActiveTeller/api/tellersessionrequests
2023-11-28 11:01:28 TerminalProfileHoursPolicy.Find - Asset 25 is currently in terminal profile hours 1: returning RoutingRule.PREFER_BRANCH.
2023-11-28 11:01:28 TellerRequestManager.HandleTellerSessionRequest is using RoutingRule.PREFER_BRANCH for tellerRequest from 12PROS04D
2023-11-28 11:01:28 Client session 4292 can handle the request from 12PROS04D
2023-11-28 11:01:28 TellerRequestManager.HandleTellerSessionRequest handled tellerRequest {"Id":23660,"AssetName":"12PROS04D","Timestamp":"2023-11-28T11:01:32.2281847-06:00","CustomerId":"","CustomerName":"","FlowPoint":"Common-RequestAssistance","RequestContext":"HelpButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
2023-11-28 11:01:29 Post - /ActiveTeller/api/remotecontroltasks
2023-11-28 11:01:29 Post - /ActiveTeller/api/remotecontrolevents
2023-11-28 11:01:33 Put - /ActiveTeller/api/applicationstates/5

Workstation20231128.log
[2023-11-28 11:01:28-311][3][DataFlowManager     ]Teller session request 23660 for asset 12PROS04D
[2023-11-28 11:01:28-311][3][MainWindow          ]Teller session requested for asset 12PROS04D for teller session request 23660. IsAcceptable=True. 
[2023-11-28 11:01:28-327][3][ConnectionManager   ]Update teller session statistics: Pending=1 Current=4
[2023-11-28 11:01:28-327][3][DataFlowManager     ]Teller session statistics update received
[2023-11-28 11:01:29-643][3][DataFlowManager     ]Received ItemsTaken event for DispenseTask 25 for asset 16CENT06D
[2023-11-28 11:01:29-644][3][WithdrawalTaskHandler]OnItemsTaken started.
[2023-11-28 11:01:29-644][3][WithdrawalTaskHandler]AddReceiptList started.
[2023-11-28 11:01:29-645][3][WithdrawalTaskHandler]InitializeData started.
[2023-11-28 11:01:29-646][3][WithdrawalTaskHandler]CheckForPartialDispense started.
[2023-11-28 11:01:29-647][3][CompleteTask        ]Completing DispenseTask 25
[2023-11-28 11:01:29-647][3][CompleteTask        ]Completing DispenseTask 25
[2023-11-28 11:01:33-174][3][MainWindow          ]BtnEndConference was clicked
[2023-11-28 11:01:34-484][3][UIManager           ]BtnEndConference set to disabled and visible
[2023-11-28 11:01:34-484][3][BeeHDVideoControl   ]StopVideoCall

                   */


                  if (dict.ContainsKey("AssetName"))
                  {
                     AssetName = (string)dict["AssetName"];
                  }

                  if (dict.ContainsKey("Timestamp"))
                  {
                     // if conversion to DateTime sees "ANY" timezone offset in the string .NET adjusts the result to LocalTime
                     // FOR THE MACHINE ON WHICH THIS PARSER IS RUNNING, not the source machine of the log file
                     string wonkyTimestamp_DONOTUSE = dict["Timestamp"].ToString();

                     try
                     {
                        MachineTime machineTime = new MachineTime(DateTime.Parse(Timestamp), AssetName, AssetName, requestType.ToString(), Payload);

                        MachineTimesList.Add(machineTime);

                        //MachineTimes.Add(machineTime.LogSourceMachine, machineTime);
                     }
                     catch (Exception ex)
                     {
                        // failed format - ignore for now
                     }
                  }


                  // process the request

                  IDictionary<string, object> tellerInfo;
                  IDictionary<string, object> data;

                  switch (requestType)
                  {
                     case ServerRequestType.SystemParameters:
                        //{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"False"}
                        //{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"}
                        //{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"}
                        //{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"False"}
                        //{"Name":"JournalHistory","Type":"System.Int32","Value":"90"}
                        //{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"}
                        //{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"}
                        //{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="}
                        //{"Name":"SavedImagesHistory","Type":"System.Int32","Value":"90"}
                        //{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"}
                        //{"Name":"SmtpFromEmail","Type":"System.String","Value":"noreply@domain.com"}
                        //{"Name":"SmtpPassword","Type":"System.Security.SecureString","Value":"Z7BKbZXrF+PMTVNvarcz4Q=="}
                        //{"Name":"SmtpPortNumber","Type":"System.String","Value":"587"}
                        //{"Name":"SmtpServer","Type":"System.String","Value":"127.0.0.1"}
                        //{"Name":"SmtpUsername","Type":"System.String","Value":"admin"}
                        //{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"}
                        //{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"}
                        //{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"60"}
                        //{"Name":"TwilioAccountId","Type":"System.Security.SecureString","Value":"KXwVYDO5FJ1CAQSc359+C1E9GzftP0uwtPzE6jsCui9VydQ4Sm9NKUMZ5rWxtGlG"}
                        //{"Name":"TwilioApiKey","Type":"System.Security.SecureString","Value":"XDL1Pih/5D3gOJTc+rRnFMF3+NJHPgoaQDv61cIk62++YER61nDveWllUgZgsS/5"}
                        //{"Name":"TwilioPhoneNumber","Type":"System.String","Value":"+19375551212"}
                        //{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"7"}
                        //{"Name":"VideoRecordingHistory","Type":"System.Int32","Value":"90"}
                        break;

                     case ServerRequestType.TerminalMode:
                        //{"AssetId":11,"AssetName":"NM000559","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}
                        break;

                     case ServerRequestType.RemoteDesktopAvailable:
                     case ServerRequestType.RemoteDesktopUnavailable:
                        //{"AssetName":"21PLEA03D","TellerSessionRequestId":20628,"Availability":1,"ExternalRoutingIdentifier":null,"Timestamp":"2023-11-17T18:23:07.4571957-06:00"}
                        RequestTimeUTC = ((DateTime)dict["Timestamp"]).ToUniversalTime().ToString(LogLine.DateTimeFormatStringMsec);
                        AssetName = (string)dict["AssetName"];
                        RequestId = dict["TellerSessionRequestId"].ToString();
                        break;

                     case ServerRequestType.RemoteTellerSessionContactInfo:
                        //{"Id":24022,"AssetName":"NM000559","TellerSessionRequestId":30981,"Timestamp":"2023-09-25T09:38:43.3973841-07:00",
                        //"TellerInfo":{"ClientSessionId":3895,"TellerName":"Jesus","VideoConferenceUri":"10.255.254.247","TellerId":"jpinon"}}

                        //{"Id":18850,"AssetName":"21PLEA03D","TellerSessionRequestId":20055,"Timestamp":"2023-11-17T08:01:11.084482-06:00",
                        //"TellerInfo":{"ClientSessionId":3880,"TellerName":"Alyssa","VideoConferenceUri":"192.168.20.25","TellerId":"ahall"}}
                        RequestTimeUTC = ((DateTime)dict["Timestamp"]).ToUniversalTime().ToString(LogLine.DateTimeFormatStringMsec);

                        RequestId = dict["TellerSessionRequestId"].ToString();

                        AssetName = (string)dict["AssetName"];
                        SessionId = dict["Id"].ToString();

                        tellerInfo = (IDictionary<string, object>)dict["TellerInfo"];
                        ClientSession = (long)tellerInfo["ClientSessionId"];
                        TellerName = (string)tellerInfo["TellerName"];
                        TellerId = (string)tellerInfo["TellerId"];
                        TellerUri = (string)tellerInfo["VideoConferenceUri"];
                        break;

                     case ServerRequestType.RemoteTellerSessionStart:
                        //{"StartTime":null,"Id":2236837,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,
                        //"Timestamp":"2023-11-13T08:11:57.9696555-06:00",
                        //"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        RequestTimeUTC = ((DateTime)dict["Timestamp"]).ToUniversalTime().ToString(ServerRequests.DateTimeFormatStringMsec);
                        AssetName = (string)dict["AssetName"];
                        SessionId = dict["Id"].ToString();

                        tellerInfo = (IDictionary<string, object>)dict["TellerInfo"];
                        ClientSession = (long)tellerInfo["ClientSessionId"];
                        TellerName = (string)tellerInfo["TellerName"];
                        TellerId = (string)tellerInfo["TellerId"];
                        TellerUri = (string)tellerInfo["VideoConferenceUri"];
                        break;

                     case ServerRequestType.RemoteTellerTaskEvent:
                        //{"TaskId":25,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationRequest",
                        //"Data":"{\"Name\":\"ConfigurationRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-13T10:27:47.3758284-06:00\",\"TaskTimeout\":null}",
                        //"Id":2237013,"AssetName":"WI000902","TellerSessionId":147548,"TransactionDetail":null,"Timestamp":"2023-11-13T10:27:47.379449-06:00",
                        //"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        TaskName = (string)dict["TaskName"];
                        EventName = (string)dict["EventName"];
                        AssetName = (string)dict["AssetName"];
                        SessionId = dict["Id"].ToString();

                        dynamic dynamicData = JsonConvert.DeserializeObject<ExpandoObject>((string)dict["Data"], new ExpandoObjectConverter());

                        RequestName = dynamicData.Name;
                        RequestTimeUTC = ((DateTime)dynamicData.DateTime).ToUniversalTime().ToString(ServerRequests.DateTimeFormatStringMsec);

                        tellerInfo = (IDictionary<string, object>)dict["TellerInfo"];
                        ClientSession = (long)tellerInfo["ClientSessionId"];
                        TellerName = (string)tellerInfo["TellerName"];
                        TellerId = (string)tellerInfo["TellerId"];
                        TellerUri = (string)tellerInfo["VideoConferenceUri"];
                        break;

                     case ServerRequestType.TransactionDetails:
                        //{"Id":152341,"AssetName":"NM000559","TellerSessionId":24022,
                        //"TransactionDetail":{
                        // "Accounts":[
                        //  {"Action":1,"Amount":"220000","Warnings":[],"AccountType":"SHARE","Id":41478,"TransactionDetailId":20219,
                        //   "Review":{
                        //     "Id":31557,
                        //     "Approval":{
                        //       "TellerAmount":null,"TellerApproval":0,"Reason":null,"TransactionItemReviewId":31557},
                        //     "ReasonForReview":0,"TransactionItemId":41478}
                        //  }
                        // ],
                        // "Id":20219,"TellerSessionActivityId":152341,"TransactionType":"CheckDeposit","ApproverId":null,
                        // "IdScans":[],
                        // "Checks":[
                        //   {"AcceptStatus":"CHANGE","Amount":"220000","AmountRead":"1500","AmountScore":216,"BackImageRelativeUri":"api/checkimages/46820","CheckDateRead":"9/18/2023","CheckDateScore":63,"CheckIndex":0,"FrontImageRelativeUri":"api/checkimages/46819","ImageBack":"D:\\CHECK21\\Bottom1.jpg","ImageFront":"D:\\CHECK21\\Top1.jpg","InvalidReason":"","Id":41477,"TransactionDetailId":20219,
                        //    "Review":{"Id":31556,
                        //    "Approval":{"TellerAmount":"220000","TellerApproval":2,"Reason":"","TransactionItemReviewId":31556},
                        //    "ReasonForReview":1,"TransactionItemId":41477}
                        //  }
                        // ],
                        //"TransactionCashDetails":[],
                        //"TransactionOtherAmounts":[],
                        //"TransactionWarnings":[]},
                        //"Timestamp":"2023-09-25T09:40:25.7365541-07:00",
                        //"TellerInfo":{"ClientSessionId":3895,"TellerName":"Jesus","VideoConferenceUri":"10.255.254.247","TellerId":"jpinon"}}

                        if (dict["TransactionDetail"] != null)
                        {
                           data = (IDictionary<string, object>)dict["TransactionDetail"];

                           TransactionType = (string)data["TransactionType"];
                           //string Accounts = (string)data["Accounts"];
                        }

                        tellerInfo = (IDictionary<string, object>)dict["TellerInfo"];
                        SessionId = long.Parse(dict["TellerSessionId"].ToString()).ToString();
                        ClientSession = (long)tellerInfo["ClientSessionId"];
                        TellerName = (string)tellerInfo["TellerName"];
                        TellerId = (string)tellerInfo["TellerId"];
                        TellerUri = (string)tellerInfo["VideoConferenceUri"];
                        break;

                     case ServerRequestType.RemoteTellerSessionRequestContext:
                        //{"Id":31114,"AssetName":"NM000559","Timestamp":"2023-09-25T15:26:54.797","CustomerId":"0009652240","FlowPoint":"Common-ProcessTransactionReview",
                        //"RequestContext":"TransactionRequiringReview","ApplicationState":"InTransaction","TransactionType":"Deposit","Language":"English",
                        //"VoiceGuidance":false,
                        //"RoutingProfile":{"SupportedCallType":"BeeHD"}}

                        AssetName = (string)dict["AssetName"];
                        RequestTimeUTC = ((DateTime)dict["Timestamp"]).ToUniversalTime().ToString(ServerRequests.DateTimeFormatStringMsec);
                        FlowPoint = (string)dict["FlowPoint"];
                        CustomerId = (string)dict["CustomerId"];
                        RequestContext = (string)dict["RequestContext"];
                        ApplicationState = (string)dict["ApplicationState"];
                        TransactionType = (string)dict["TransactionType"];
                        break;

                     default:
                        break;
                  }
               }
            }
            catch (Exception ex)
            {
               throw new Exception($"AELogLine.ServerCommunication: error processing log line '{logLine}'\n{ex}");
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"ATLogLine.ServerCommunication: did not recognize the log line '{logLine}'");
         }
      }
   }
}
