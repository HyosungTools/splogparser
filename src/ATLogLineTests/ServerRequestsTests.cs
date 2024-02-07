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
    public class ServerRequestsTests
   {
      [TestMethod]
      public void ServerRequests_OnRequest_listofSystemSetting()
      {
         string sampleLine = @"2023-11-13 04:53:23 Server message OnRequest for list of SystemSetting";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("OnRequest", line.Operation);
         Assert.AreEqual("list of SystemSetting", line.ObjectType);
         Assert.AreEqual("2023-11-13 04:53:23", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_OnUpdate_OperatingMode()
      {
         string sampleLine = @"2023-11-13 07:55:28 Server message OnUpdate for OperatingMode";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("OnUpdate", line.Operation);
         Assert.AreEqual("OperatingMode", line.ObjectType);
         Assert.AreEqual("2023-11-13 07:55:28", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_OnDelete_TellerSessionRequest()
      {
         string sampleLine = @"2023-11-13 08:16:32 Server message OnDelete for TellerSessionRequest";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("OnDelete", line.Operation);
         Assert.AreEqual("TellerSessionRequest", line.ObjectType);
         Assert.AreEqual("2023-11-13 08:16:32", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_POST_ApplicationState()
      {
         string sampleLine = @"2023-09-25 01:05:09 POST ApplicationState message received in ApplicationStateMessageHandler";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("post", line.Operation);
         Assert.AreEqual("ApplicationState", line.ObjectType);
         Assert.AreEqual("ApplicationStateMessageHandler", line.ObjectHandler);
         Assert.AreEqual("2023-09-25 01:05:09", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_GET_OperatingMode()
      {
         string sampleLine = @"2023-09-25 01:05:09 GET OperatingMode message received in OperatingModeMessageHandler";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("get", line.Operation);
         Assert.AreEqual("OperatingMode", line.ObjectType);
         Assert.AreEqual("OperatingModeMessageHandler", line.ObjectHandler);
         Assert.AreEqual("2023-09-25 01:05:09", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_POST_AssetConfiguration()
      {
         string sampleLine = @"2023-09-25 01:05:09 POST AssetConfiguration message received in AssetConfigurationMessageHandler";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("post", line.Operation);
         Assert.AreEqual("AssetConfiguration", line.ObjectType);
         Assert.AreEqual("AssetConfigurationMessageHandler", line.ObjectHandler);
         Assert.AreEqual("2023-09-25 01:05:09", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_DELETE_TellerSessionRequest()
      {
         string sampleLine = @"2023-09-25 02:15:11 DELETE TellerSessionRequest message received in TellerSessionRequestMessageHandler";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("delete", line.Operation);
         Assert.AreEqual("TellerSessionRequest", line.ObjectType);
         Assert.AreEqual("TellerSessionRequestMessageHandler", line.ObjectHandler);
         Assert.AreEqual("2023-09-25 02:15:11", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_AttemptedDelete_TellerSessionRequest()
      {
         string sampleLine = @"2023-09-25 02:15:11 Attempted to delete the TellerSessionRequest, but we didn't have one.";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("delete", line.Operation);
         Assert.AreEqual("TellerSessionRequest", line.ObjectType);
         Assert.AreEqual("ERROR - NO HANDLER", line.ObjectHandler);
         Assert.AreEqual("2023-09-25 02:15:11", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_Post_ApiAssets()
      {
         string sampleLine = @"2023-11-17 03:00:22 POST http://10.37.152.15:81/activeteller/api/assets server response Created";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("POST", line.RequestMethod);
         Assert.AreEqual("Created", line.RequestResult);
         Assert.AreEqual("http://10.37.152.15:81/activeteller/api/assets", line.RequestUrl);
         Assert.IsNull(line.ObjectType);
         Assert.IsNull(line.ObjectHandler);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_Put_ApiApplicationStates()
      {
         string sampleLine = @"2023-09-25 07:46:06 PUT http://192.168.5.33/activeteller/api/applicationstates/11 server response OK";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("PUT", line.RequestMethod);
         Assert.AreEqual("OK", line.RequestResult);
         Assert.AreEqual("http://192.168.5.33/activeteller/api/applicationstates/11", line.RequestUrl);
         Assert.IsNull(line.ObjectType);
         Assert.IsNull(line.ObjectHandler);
         Assert.AreEqual("2023-09-25 07:46:06", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerRequests_Delete_ApiDeviceStates()
      {
         string sampleLine = @"2023-11-13 15:17:09 DELETE http://10.201.10.118/activeteller/api/devicestates?assetName=WI000902&deviceId=2,4,A,B,F,J,K server response OK";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("DELETE", line.RequestMethod);
         Assert.AreEqual("OK", line.RequestResult);
         Assert.AreEqual("http://10.201.10.118/activeteller/api/devicestates?assetName=WI000902&deviceId=2,4,A,B,F,J,K", line.RequestUrl);
         Assert.IsNull(line.ObjectType);
         Assert.IsNull(line.ObjectHandler);
         Assert.AreEqual("2023-11-13 15:17:09", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_1()
      {
         string sampleLine = @"2023-11-13 04:53:23 Server message data [{""Name"":""AutoAssignRemoteTeller"",""Type"":""System.Boolean"",""Value"":""True""},{""Name"":""DefaultTerminalFeatureSet"",""Type"":""System.Int32"",""Value"":""1""},{""Name"":""DefaultTerminalProfile"",""Type"":""System.Int32"",""Value"":""1""},{""Name"":""ForceSkypeSignIn"",""Type"":""System.Boolean"",""Value"":""True""},{""Name"":""JournalHistory"",""Type"":""System.Int32"",""Value"":""60""},{""Name"":""NetOpLicenseKey"",""Type"":""System.String"",""Value"":""*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#""},{""Name"":""RemoteDesktopAvailability"",""Type"":""DropDownList"",""Value"":""Always""},{""Name"":""RemoteDesktopPassword"",""Type"":""System.Security.SecureString"",""Value"":""xnXpgq6Zgi5XLqZd1XUgwA==""},{""Name"":""SingleSignOn"",""Type"":""System.Boolean"",""Value"":""False""},{""Name"":""TraceLogHistory"",""Type"":""System.Int32"",""Value"":""120""},{""Name"":""TransactionResultHistory"",""Type"":""System.Int32"",""Value"":""30""},{""Name"":""TransactionTypeHistory"",""Type"":""System.Int32"",""Value"":""60""},{""Name"":""UploadedFileHistory"",""Type"":""System.Int32"",""Value"":""7""}]";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("[{\"Name\":\"AutoAssignRemoteTeller\",\"Type\":\"System.Boolean\",\"Value\":\"True\"},{\"Name\":\"DefaultTerminalFeatureSet\",\"Type\":\"System.Int32\",\"Value\":\"1\"},{\"Name\":\"DefaultTerminalProfile\",\"Type\":\"System.Int32\",\"Value\":\"1\"},{\"Name\":\"ForceSkypeSignIn\",\"Type\":\"System.Boolean\",\"Value\":\"True\"},{\"Name\":\"JournalHistory\",\"Type\":\"System.Int32\",\"Value\":\"60\"},{\"Name\":\"NetOpLicenseKey\",\"Type\":\"System.String\",\"Value\":\"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#\"},{\"Name\":\"RemoteDesktopAvailability\",\"Type\":\"DropDownList\",\"Value\":\"Always\"},{\"Name\":\"RemoteDesktopPassword\",\"Type\":\"System.Security.SecureString\",\"Value\":\"xnXpgq6Zgi5XLqZd1XUgwA==\"},{\"Name\":\"SingleSignOn\",\"Type\":\"System.Boolean\",\"Value\":\"False\"},{\"Name\":\"TraceLogHistory\",\"Type\":\"System.Int32\",\"Value\":\"120\"},{\"Name\":\"TransactionResultHistory\",\"Type\":\"System.Int32\",\"Value\":\"30\"},{\"Name\":\"TransactionTypeHistory\",\"Type\":\"System.Int32\",\"Value\":\"60\"},{\"Name\":\"UploadedFileHistory\",\"Type\":\"System.Int32\",\"Value\":\"7\"}]", line.Payload);
         Assert.IsNull(line.RequestMethod);
         Assert.IsNull(line.RequestResult);
         Assert.IsNull(line.ObjectType);
         Assert.IsNull(line.ObjectHandler);
         Assert.AreEqual("2023-11-13 04:53:23", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_2()
      {
         string sampleLine = @"2023-11-13 07:55:28 Server message data {""AssetId"":5,""AssetName"":""WI000902"",""ModeType"":""Scheduled"",""ModeName"":""SelfService"",""CoreStatus"":"""",""CoreProperties"":""""}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"AssetId\":5,\"AssetName\":\"WI000902\",\"ModeType\":\"Scheduled\",\"ModeName\":\"SelfService\",\"CoreStatus\":\"\",\"CoreProperties\":\"\"}", line.Payload);
         Assert.IsNull(line.RequestMethod);
         Assert.IsNull(line.RequestResult);
         Assert.IsNull(line.ObjectType);
         Assert.IsNull(line.ObjectHandler);
         Assert.AreEqual("2023-11-13 07:55:28", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_3()
      {
         string sampleLine = @"2023-11-13 07:55:29 Server message data {""AssetId"":5,""AssetName"":""WI000902"",""ModeType"":""Scheduled"",""ModeName"":""Standard"",""CoreStatus"":"""",""CoreProperties"":""""}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"AssetId\":5,\"AssetName\":\"WI000902\",\"ModeType\":\"Scheduled\",\"ModeName\":\"Standard\",\"CoreStatus\":\"\",\"CoreProperties\":\"\"}", line.Payload);
         Assert.IsNull(line.RequestMethod);
         Assert.IsNull(line.RequestResult);
         Assert.IsNull(line.ObjectType);
         Assert.IsNull(line.ObjectHandler);
         Assert.AreEqual("2023-11-13 07:55:29", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_4()
      {
         string sampleLine = @"2023-11-13 08:16:32 Server message data {""Id"":155420,""AssetName"":""WI000902"",""Timestamp"":""2023-11-13T08:14:18.303"",""CustomerId"":"""",""FlowPoint"":""Common-RequestAssistance"",""RequestContext"":""TellerIdentificationButton"",""ApplicationState"":""PostIdle"",""TransactionType"":"""",""Language"":""English"",""VoiceGuidance"":false,""RoutingProfile"":{""ClientSessionId"":""6782"",""SupportedCallType"":""BeeHD""}}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"Id\":155420,\"AssetName\":\"WI000902\",\"Timestamp\":\"2023-11-13T08:14:18.303\",\"CustomerId\":\"\",\"FlowPoint\":\"Common-RequestAssistance\",\"RequestContext\":\"TellerIdentificationButton\",\"ApplicationState\":\"PostIdle\",\"TransactionType\":\"\",\"Language\":\"English\",\"VoiceGuidance\":false,\"RoutingProfile\":{\"ClientSessionId\":\"6782\",\"SupportedCallType\":\"BeeHD\"}}", line.Payload);
         Assert.IsNull(line.RequestMethod);
         Assert.IsNull(line.RequestResult);
         Assert.IsNull(line.ObjectType);
         Assert.IsNull(line.ObjectHandler);
         Assert.AreEqual("2023-11-13 08:16:32", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }



      /*      TODO Unit Tests

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



      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void ServerRequests_Unsupported()
      {
         string sampleLine = @"2023-11-17 03:00:21 Server message unsupported text";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = null;

         try
         {
            line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"ATLogLine.ServerCommunication: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void ServerRequests_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:22 not an server message logline";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());

         try
         {
            ILogLine line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.AreEqual($"ATLogLine.ServerCommunication: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
