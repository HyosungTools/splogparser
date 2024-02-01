using System;
using System.Collections.Generic;
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
      bool isRecognized = false;

      public enum ServerRequestType
      {
         Unknown,
         SystemParameters,
         TerminalMode,
         RemoteTellerSelected,
         TellerSessionStart,
         TellerTaskEvent,
         TransactionDetails,
         SelfServiceFlow
      }

      public string Operation {  get; set; }
      public string ObjectType { get; set; }
      public string ObjectHandler { get; set; }


      public string RequestMethod { get; set; }
      public string RequestUrl { get; set; }
      public string RequestResult { get; set; }

      public DateTime RequestTime { get; set; } = DateTime.MinValue;
      public long ClientSession { get; set; } = -1;
      public string Terminal { get; set; }
      public string TellerName { get; set; }
      public string TellerId { get; set; }
      public string TellerUri { get; set; }
      public string TaskName { get; set; }
      public string EventName { get; set; }
      public string FlowPoint { get; set; }
      public string CustomerId { get; set; }
      public string RequestName { get; set; }
      public string RequestContext { get; set; }
      public string ApplicationState { get; set; }
      public string TransactionType { get; set; }

      public string Payload { get; set; }


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
               isRecognized = true;
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
               isRecognized = true;
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
               isRecognized = true;

               RequestMethod = m.Groups["request"].Value;
               RequestUrl = m.Groups["url"].Value;
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
               isRecognized = true;

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
               isRecognized = true;

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

            TellerTaskEvent
            {"TaskId":25,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationRequest","Data":"{\"Name\":\"ConfigurationRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-13T10:27:47.3758284-06:00\",\"TaskTimeout\":null}","Id":2237013,"AssetName":"WI000902","TellerSessionId":147548,"TransactionDetail":null,"Timestamp":"2023-11-13T10:27:47.379449-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

            SelfServiceFlow
            {"Id":31114,"AssetName":"NM000559","Timestamp":"2023-09-25T15:26:54.797","CustomerId":"0009652240","FlowPoint":"Common-ProcessTransactionReview","RequestContext":"TransactionRequiringReview","ApplicationState":"InTransaction","TransactionType":"Deposit","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
          */

         if (isRecognized && !string.IsNullOrEmpty(Payload))
         {
            List<ExpandoObject> json = new List<ExpandoObject>();

            try
            {
               if (Payload.StartsWith("["))
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

                  if (dict.TryGetValue("Name", out fieldvalue))
                  {
                     requestType = ServerRequestType.SystemParameters;
                  }

                  else if (dict.TryGetValue("ModeType", out fieldvalue))
                  {
                     requestType = ServerRequestType.TerminalMode;
                  }

                  else if (dict.TryGetValue("StartTime", out fieldvalue))
                  {
                     requestType = ServerRequestType.TellerSessionStart;
                  }

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

                  else if (dict.TryGetValue("TellerSessionRequestId", out fieldvalue))
                  {
                     requestType = ServerRequestType.RemoteTellerSelected;
                  }

                  else if (dict.TryGetValue("TaskName", out fieldvalue))
                  {
                     requestType = ServerRequestType.TellerTaskEvent;
                  }

                  else if (dict.TryGetValue("FlowPoint", out fieldvalue))
                  {
                     requestType = ServerRequestType.SelfServiceFlow;
                  }

                  else
                  {
                     requestType = ServerRequestType.Unknown;
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

                     case ServerRequestType.RemoteTellerSelected:
                        //{"Id":24022,"AssetName":"NM000559","TellerSessionRequestId":30981,"Timestamp":"2023-09-25T09:38:43.3973841-07:00",
                        //"TellerInfo":{"ClientSessionId":3895,"TellerName":"Jesus","VideoConferenceUri":"10.255.254.247","TellerId":"jpinon"}}

                        RequestTime = (DateTime)dict["Timestamp"];
                        Terminal = (string)dict["AssetName"];

                        tellerInfo = (IDictionary<string, object>)dict["TellerInfo"];
                        ClientSession = (long)tellerInfo["ClientSessionId"];
                        TellerName = (string)tellerInfo["TellerName"];
                        TellerId = (string)tellerInfo["TellerId"];
                        TellerUri = (string)tellerInfo["VideoConferenceUri"];
                        break;

                     case ServerRequestType.TellerSessionStart:
                        //{"StartTime":null,"Id":2236837,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,
                        //"Timestamp":"2023-11-13T08:11:57.9696555-06:00",
                        //"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        RequestTime = (DateTime)dict["Timestamp"];
                        Terminal = (string)dict["AssetName"];

                        tellerInfo = (IDictionary<string, object>)dict["TellerInfo"];
                        ClientSession = (long)tellerInfo["ClientSessionId"];
                        TellerName = (string)tellerInfo["TellerName"];
                        TellerId = (string)tellerInfo["TellerId"];
                        TellerUri = (string)tellerInfo["VideoConferenceUri"];
                        break;

                     case ServerRequestType.TellerTaskEvent:
                        //{"TaskId":25,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationRequest",
                        //"Data":"{\"Name\":\"ConfigurationRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-13T10:27:47.3758284-06:00\",\"TaskTimeout\":null}",
                        //"Id":2237013,"AssetName":"WI000902","TellerSessionId":147548,"TransactionDetail":null,"Timestamp":"2023-11-13T10:27:47.379449-06:00",
                        //"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        TaskName = (string)dict["TaskName"];
                        EventName = (string)dict["EventName"];
                        Terminal = (string)dict["AssetName"];

                        dynamic dynamicData = JsonConvert.DeserializeObject<ExpandoObject>((string)dict["Data"], new ExpandoObjectConverter());

                        RequestName = dynamicData.Name;
                        RequestTime = (DateTime)dynamicData.DateTime;

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
                        ClientSession = (long)tellerInfo["ClientSessionId"];
                        TellerName = (string)tellerInfo["TellerName"];
                        TellerId = (string)tellerInfo["TellerId"];
                        TellerUri = (string)tellerInfo["VideoConferenceUri"];
                        break;

                     case ServerRequestType.SelfServiceFlow:
                        //{"Id":31114,"AssetName":"NM000559","Timestamp":"2023-09-25T15:26:54.797","CustomerId":"0009652240","FlowPoint":"Common-ProcessTransactionReview",
                        //"RequestContext":"TransactionRequiringReview","ApplicationState":"InTransaction","TransactionType":"Deposit","Language":"English",
                        //"VoiceGuidance":false,
                        //"RoutingProfile":{"SupportedCallType":"BeeHD"}}

                        Terminal = (string)dict["AssetName"];
                        RequestTime = (DateTime)dict["Timestamp"];
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

            }
         }

         if (!isRecognized)
         {
            throw new Exception($"ATLogLine.ServerCommunication: did not recognize the log line '{logLine}'");
         }
      }
   }
}
