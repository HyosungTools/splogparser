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
         Assert.AreEqual("/activeteller/api/assets", line.RequestPath);
         Assert.AreEqual("http://10.37.152.15:81", line.RequestDomain);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
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
         Assert.AreEqual("/activeteller/api/applicationstates/11", line.RequestPath);
         Assert.AreEqual("http://192.168.5.33", line.RequestDomain);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
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
         Assert.AreEqual(line.RequestDomain, "http://10.201.10.118");
         Assert.AreEqual(line.RequestPath, "/activeteller/api/devicestates?assetName=WI000902&deviceId=2,4,A,B,F,J,K");
         Assert.AreEqual(line.RequestResult, "OK");
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual("2023-11-13 15:17:09", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_SystemParameters()
      {
         string sampleLine = @"2023-11-13 04:53:23 Server message data [{""Name"":""AutoAssignRemoteTeller"",""Type"":""System.Boolean"",""Value"":""True""},{""Name"":""DefaultTerminalFeatureSet"",""Type"":""System.Int32"",""Value"":""1""},{""Name"":""DefaultTerminalProfile"",""Type"":""System.Int32"",""Value"":""1""},{""Name"":""ForceSkypeSignIn"",""Type"":""System.Boolean"",""Value"":""True""},{""Name"":""JournalHistory"",""Type"":""System.Int32"",""Value"":""60""},{""Name"":""NetOpLicenseKey"",""Type"":""System.String"",""Value"":""*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#""},{""Name"":""RemoteDesktopAvailability"",""Type"":""DropDownList"",""Value"":""Always""},{""Name"":""RemoteDesktopPassword"",""Type"":""System.Security.SecureString"",""Value"":""xnXpgq6Zgi5XLqZd1XUgwA==""},{""Name"":""SingleSignOn"",""Type"":""System.Boolean"",""Value"":""False""},{""Name"":""TraceLogHistory"",""Type"":""System.Int32"",""Value"":""120""},{""Name"":""TransactionResultHistory"",""Type"":""System.Int32"",""Value"":""30""},{""Name"":""TransactionTypeHistory"",""Type"":""System.Int32"",""Value"":""60""},{""Name"":""UploadedFileHistory"",""Type"":""System.Int32"",""Value"":""7""}]";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("[{\"Name\":\"AutoAssignRemoteTeller\",\"Type\":\"System.Boolean\",\"Value\":\"True\"},{\"Name\":\"DefaultTerminalFeatureSet\",\"Type\":\"System.Int32\",\"Value\":\"1\"},{\"Name\":\"DefaultTerminalProfile\",\"Type\":\"System.Int32\",\"Value\":\"1\"},{\"Name\":\"ForceSkypeSignIn\",\"Type\":\"System.Boolean\",\"Value\":\"True\"},{\"Name\":\"JournalHistory\",\"Type\":\"System.Int32\",\"Value\":\"60\"},{\"Name\":\"NetOpLicenseKey\",\"Type\":\"System.String\",\"Value\":\"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#\"},{\"Name\":\"RemoteDesktopAvailability\",\"Type\":\"DropDownList\",\"Value\":\"Always\"},{\"Name\":\"RemoteDesktopPassword\",\"Type\":\"System.Security.SecureString\",\"Value\":\"xnXpgq6Zgi5XLqZd1XUgwA==\"},{\"Name\":\"SingleSignOn\",\"Type\":\"System.Boolean\",\"Value\":\"False\"},{\"Name\":\"TraceLogHistory\",\"Type\":\"System.Int32\",\"Value\":\"120\"},{\"Name\":\"TransactionResultHistory\",\"Type\":\"System.Int32\",\"Value\":\"30\"},{\"Name\":\"TransactionTypeHistory\",\"Type\":\"System.Int32\",\"Value\":\"60\"},{\"Name\":\"UploadedFileHistory\",\"Type\":\"System.Int32\",\"Value\":\"7\"}]", line.Payload);
         Assert.AreEqual(line.RequestDomain, string.Empty);
         Assert.AreEqual(line.RequestPath, string.Empty);
         Assert.AreEqual(line.RequestResult, string.Empty);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual(line.FlowPoint,string.Empty);
         Assert.AreEqual(line.TellerUri, string.Empty);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("2023-11-13 04:53:23", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_TerminalMode()
      {
         string sampleLine = @"2023-11-13 07:55:28 Server message data {""AssetId"":5,""AssetName"":""WI000902"",""ModeType"":""Scheduled"",""ModeName"":""SelfService"",""CoreStatus"":"""",""CoreProperties"":""""}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"AssetId\":5,\"AssetName\":\"WI000902\",\"ModeType\":\"Scheduled\",\"ModeName\":\"SelfService\",\"CoreStatus\":\"\",\"CoreProperties\":\"\"}", line.Payload);
         Assert.AreEqual(line.RequestDomain, string.Empty);
         Assert.AreEqual(line.RequestPath, string.Empty);
         Assert.AreEqual(line.RequestResult, string.Empty);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual(line.FlowPoint, string.Empty);
         Assert.AreEqual(line.TellerUri, string.Empty);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("2023-11-13 07:55:28", line.Timestamp);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_RemoteTellerSelected()
      {
         string sampleLine = @"2023-09-25 10:42:29 Server message data {""Id"":24022,""AssetName"":""NM000559"",""TellerSessionRequestId"":30981,""Timestamp"":""2023-09-25T09:38:43.3973841-07:00"",""TellerInfo"":{""ClientSessionId"":3895,""TellerName"":""Mike"",""VideoConferenceUri"":""10.255.254.247"",""TellerId"":""mluckham""}}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"Id\":24022,\"AssetName\":\"NM000559\",\"TellerSessionRequestId\":30981,\"Timestamp\":\"2023-09-25T09:38:43.3973841-07:00\",\"TellerInfo\":{\"ClientSessionId\":3895,\"TellerName\":\"Mike\",\"VideoConferenceUri\":\"10.255.254.247\",\"TellerId\":\"mluckham\"}}", line.Payload);
         Assert.AreEqual(line.RequestMethod, string.Empty);
         Assert.AreEqual(line.RequestDomain, string.Empty);
         Assert.AreEqual(line.RequestPath, string.Empty);
         Assert.AreEqual(line.RequestResult, string.Empty);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("2023-09-25 10:42:29", line.Timestamp);
         Assert.AreEqual(3895, line.ClientSessionId);
         Assert.AreEqual("mluckham", line.TellerId);
         Assert.AreEqual("Mike", line.TellerName);
         Assert.AreEqual("NM000559", line.AssetName);
         Assert.AreEqual("24022", line.MessageId);
         Assert.AreEqual("10.255.254.247", line.TellerUri);

         Assert.AreEqual("2023-09-25 04:38:43.3973", line.RequestTimeUTC);
         Assert.AreEqual("2023-09-25 10:42:29", line.Timestamp);

         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_SelfServiceFlow()
      {
         string sampleLine = @"2023-11-13 08:16:32 Server message data {""Id"":155420,""AssetName"":""WI000902"",""Timestamp"":""2023-11-13T08:14:18.303"",""CustomerId"":"""",""FlowPoint"":""Common-RequestAssistance"",""RequestContext"":""TellerIdentificationButton"",""ApplicationState"":""PostIdle"",""TransactionType"":"""",""Language"":""English"",""VoiceGuidance"":false,""RoutingProfile"":{""ClientSessionId"":""6782"",""SupportedCallType"":""BeeHD""}}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"Id\":155420,\"AssetName\":\"WI000902\",\"Timestamp\":\"2023-11-13T08:14:18.303\",\"CustomerId\":\"\",\"FlowPoint\":\"Common-RequestAssistance\",\"RequestContext\":\"TellerIdentificationButton\",\"ApplicationState\":\"PostIdle\",\"TransactionType\":\"\",\"Language\":\"English\",\"VoiceGuidance\":false,\"RoutingProfile\":{\"ClientSessionId\":\"6782\",\"SupportedCallType\":\"BeeHD\"}}", line.Payload);
         Assert.AreEqual(line.RequestDomain, string.Empty);
         Assert.AreEqual(line.RequestPath, string.Empty);
         Assert.AreEqual(line.RequestResult, string.Empty);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("2023-11-13 08:16:32", line.Timestamp);
         Assert.AreEqual("Common-RequestAssistance", line.FlowPoint);
         Assert.AreEqual("TellerIdentificationButton", line.RequestContext);
         Assert.AreEqual("PostIdle", line.ApplicationState);
         Assert.AreEqual("WI000902", line.AssetName);
         Assert.AreEqual(line.MessageId, string.Empty);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_TellerSessionStart()
      {
         string sampleLine = @"2023-11-13 08:16:32 Server message data {""StartTime"":null,""Id"":2236837,""AssetName"":""WI000902"",""TellerSessionId"":147534,""TransactionDetail"":null,""Timestamp"":""2023-11-13T08:11:57.9696555-06:00"",""TellerInfo"":{""ClientSessionId"":6782,""TellerName"":""Mike"",""VideoConferenceUri"":""10.206.20.47"",""TellerId"":""mluckham""}}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"StartTime\":null,\"Id\":2236837,\"AssetName\":\"WI000902\",\"TellerSessionId\":147534,\"TransactionDetail\":null,\"Timestamp\":\"2023-11-13T08:11:57.9696555-06:00\",\"TellerInfo\":{\"ClientSessionId\":6782,\"TellerName\":\"Mike\",\"VideoConferenceUri\":\"10.206.20.47\",\"TellerId\":\"mluckham\"}}", line.Payload);
         Assert.AreEqual(line.RequestDomain, string.Empty);
         Assert.AreEqual(line.RequestPath, string.Empty);
         Assert.AreEqual(line.RequestResult, string.Empty);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual(line.FlowPoint, string.Empty);
         Assert.AreEqual(line.RequestContext, string.Empty);
         Assert.AreEqual(line.ApplicationState, string.Empty);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("2023-11-13 08:16:32", line.Timestamp);
         Assert.AreEqual(line.ApplicationState, string.Empty);
         Assert.AreEqual("WI000902", line.AssetName);
         Assert.AreEqual("2236837", line.MessageId);
         Assert.AreEqual("mluckham", line.TellerId);
         Assert.AreEqual("Mike", line.TellerName);
         Assert.AreEqual("10.206.20.47", line.TellerUri);
         Assert.AreEqual(6782, line.ClientSessionId);

         Assert.AreEqual("2023-11-13 02:11:57.9696", line.RequestTimeUTC);

         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_TransactionDetails()
      {
         string sampleLine = @"2023-11-13 08:16:32 Server message data {""Id"":152341,""AssetName"":""NM000559"",""TellerSessionId"":24022,""TransactionDetail"":{""Accounts"":[{""Action"":1,""Amount"":""220000"",""Warnings"":[],""AccountType"":""SHARE"",""Id"":41478,""TransactionDetailId"":20219,""Review"":{""Id"":31557,""Approval"":{""TellerAmount"":null,""TellerApproval"":0,""Reason"":null,""TransactionItemReviewId"":31557},""ReasonForReview"":0,""TransactionItemId"":41478}}],""Id"":20219,""TellerSessionActivityId"":152341,""TransactionType"":""CheckDeposit"",""ApproverId"":null,""IdScans"":[],""Checks"":[{""AcceptStatus"":""CHANGE"",""Amount"":""220000"",""AmountRead"":""1500"",""AmountScore"":216,""BackImageRelativeUri"":""api/checkimages/46820"",""CheckDateRead"":""9/18/2023"",""CheckDateScore"":63,""CheckIndex"":0,""FrontImageRelativeUri"":""api/checkimages/46819"",""ImageBack"":""D:\\CHECK21\\Bottom1.jpg"",""ImageFront"":""D:\\CHECK21\\Top1.jpg"",""InvalidReason"":"""",""Id"":41477,""TransactionDetailId"":20219,""Review"":{""Id"":31556,""Approval"":{""TellerAmount"":""220000"",""TellerApproval"":2,""Reason"":"""",""TransactionItemReviewId"":31556},""ReasonForReview"":1,""TransactionItemId"":41477}}],""TransactionCashDetails"":[],""TransactionOtherAmounts"":[],""TransactionWarnings"":[]},""Timestamp"":""2023-09-25T09:40:25.7365541-07:00"",""TellerInfo"":{""ClientSessionId"":3895,""TellerName"":""Mike"",""VideoConferenceUri"":""10.255.254.247"",""TellerId"":""mluckham""}}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"Id\":152341,\"AssetName\":\"NM000559\",\"TellerSessionId\":24022,\"TransactionDetail\":{\"Accounts\":[{\"Action\":1,\"Amount\":\"220000\",\"Warnings\":[],\"AccountType\":\"SHARE\",\"Id\":41478,\"TransactionDetailId\":20219,\"Review\":{\"Id\":31557,\"Approval\":{\"TellerAmount\":null,\"TellerApproval\":0,\"Reason\":null,\"TransactionItemReviewId\":31557},\"ReasonForReview\":0,\"TransactionItemId\":41478}}],\"Id\":20219,\"TellerSessionActivityId\":152341,\"TransactionType\":\"CheckDeposit\",\"ApproverId\":null,\"IdScans\":[],\"Checks\":[{\"AcceptStatus\":\"CHANGE\",\"Amount\":\"220000\",\"AmountRead\":\"1500\",\"AmountScore\":216,\"BackImageRelativeUri\":\"api/checkimages/46820\",\"CheckDateRead\":\"9/18/2023\",\"CheckDateScore\":63,\"CheckIndex\":0,\"FrontImageRelativeUri\":\"api/checkimages/46819\",\"ImageBack\":\"D:\\\\CHECK21\\\\Bottom1.jpg\",\"ImageFront\":\"D:\\\\CHECK21\\\\Top1.jpg\",\"InvalidReason\":\"\",\"Id\":41477,\"TransactionDetailId\":20219,\"Review\":{\"Id\":31556,\"Approval\":{\"TellerAmount\":\"220000\",\"TellerApproval\":2,\"Reason\":\"\",\"TransactionItemReviewId\":31556},\"ReasonForReview\":1,\"TransactionItemId\":41477}}],\"TransactionCashDetails\":[],\"TransactionOtherAmounts\":[],\"TransactionWarnings\":[]},\"Timestamp\":\"2023-09-25T09:40:25.7365541-07:00\",\"TellerInfo\":{\"ClientSessionId\":3895,\"TellerName\":\"Mike\",\"VideoConferenceUri\":\"10.255.254.247\",\"TellerId\":\"mluckham\"}}", line.Payload);
         Assert.AreEqual(line.RequestDomain, string.Empty);
         Assert.AreEqual(line.RequestPath, string.Empty);
         Assert.AreEqual(line.RequestResult, string.Empty);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("2023-11-13 08:16:32", line.Timestamp);
         Assert.AreEqual(line.FlowPoint, string.Empty);
         Assert.AreEqual(line.RequestContext, string.Empty);
         Assert.AreEqual(line.ApplicationState, string.Empty);
         Assert.AreEqual(line.AssetName, string.Empty);
         Assert.AreEqual("24022", line.SessionRequestId);
         Assert.AreEqual("mluckham", line.TellerId);
         Assert.AreEqual("Mike", line.TellerName);
         Assert.AreEqual("10.255.254.247", line.TellerUri);
         Assert.AreEqual(3895, line.ClientSessionId);
         Assert.AreEqual(line.RequestTimeUTC, string.Empty);
         Assert.AreEqual("CheckDeposit", line.TransactionType);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ServerMessageData_TellerTaskEvent()
      {
         string sampleLine = @"2023-11-13 08:16:32 Server message data {""TaskId"":25,""TaskName"":""ConfigurationQueryTask"",""EventName"":""ConfigurationRequest"",""Data"":""{\""Name\"":\""ConfigurationRequest\"",\""TellerId\"":null,\""DateTime\"":\""2023-11-13T10:27:47.3758284-06:00\"",\""TaskTimeout\"":null}"",""Id"":2237013,""AssetName"":""WI000902"",""TellerSessionId"":147548,""TransactionDetail"":null,""Timestamp"":""2023-11-13T10:27:47.379449-06:00"",""TellerInfo"":{""ClientSessionId"":6782,""TellerName"":""Mike"",""VideoConferenceUri"":""10.206.20.47"",""TellerId"":""mluckham""}}";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ServerRequests line = new ServerRequests(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("{\"TaskId\":25,\"TaskName\":\"ConfigurationQueryTask\",\"EventName\":\"ConfigurationRequest\",\"Data\":\"{\\\"Name\\\":\\\"ConfigurationRequest\\\",\\\"TellerId\\\":null,\\\"DateTime\\\":\\\"2023-11-13T10:27:47.3758284-06:00\\\",\\\"TaskTimeout\\\":null}\",\"Id\":2237013,\"AssetName\":\"WI000902\",\"TellerSessionId\":147548,\"TransactionDetail\":null,\"Timestamp\":\"2023-11-13T10:27:47.379449-06:00\",\"TellerInfo\":{\"ClientSessionId\":6782,\"TellerName\":\"Mike\",\"VideoConferenceUri\":\"10.206.20.47\",\"TellerId\":\"mluckham\"}}", line.Payload);
         Assert.AreEqual(line.RequestDomain, string.Empty);
         Assert.AreEqual(line.RequestPath, string.Empty);
         Assert.AreEqual(line.RequestResult, string.Empty);
         Assert.AreEqual(line.ObjectType, string.Empty);
         Assert.AreEqual(line.ObjectHandler, string.Empty);
         Assert.AreEqual("Message received", line.Operation);
         Assert.AreEqual("2023-11-13 08:16:32", line.Timestamp);
         Assert.AreEqual(line.FlowPoint, string.Empty);
         Assert.AreEqual(line.RequestContext, string.Empty);
         Assert.AreEqual(line.ApplicationState, string.Empty);
         Assert.AreEqual("WI000902", line.AssetName);
         Assert.AreEqual("2237013", line.MessageId);
         Assert.AreEqual("mluckham", line.TellerId);
         Assert.AreEqual("Mike", line.TellerName);
         Assert.AreEqual("10.206.20.47", line.TellerUri);
         Assert.AreEqual(6782, line.ClientSessionId);
         Assert.AreEqual("ConfigurationRequest", line.RequestName);
         Assert.AreEqual("ConfigurationRequest", line.EventName);
         Assert.AreEqual("ConfigurationQueryTask", line.TaskName);

         Assert.AreEqual("2023-11-13 04:27:47.3758", line.RequestTimeUTC);

         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void ServerRequests_Unsupported_ExceptionThrown()
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
