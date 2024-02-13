using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Moq;
using Moq.Protected;
using Contract;
using System.Numerics;
using System;
using AELogLineTests;
using System.Runtime.Remoting;

namespace AELogLineTests
{
   [TestClass]
   public class MoniPlus2sExtensionTests
   {
      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void MoniPlus2sExtension_Started()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] The 'MoniPlus2sExtension' extension is started.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
      }

      [TestMethod]
      public void MoniPlus2sExtension_AgentHandlerAdded()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] An agent message handler has been added to MoniPlus2sExtension";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         // no fields updated
      }

      [TestMethod]
      public void MoniPlus2sExtension_AgentHandlerRemoved()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] An agent message handler has been removed from MoniPlus2sExtension";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         // no fields updated
      }

      [TestMethod]
      public void MoniPlus2sExtension_FailedToConnect()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Failed to connect. No connection could be made because the target machine actively refused it 127.0.0.1:1301.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("NO CONNECTION - REFUSED", line.ApplicationConnectionState);
         Assert.AreEqual("127.0.0.1:1301", line.IpAddress);
      }

      [TestMethod]
      public void MoniPlus2sExtension_Connected()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Connected.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("CONNECTED", line.ApplicationConnectionState);
         Assert.AreEqual(string.Empty, line.IpAddress);
      }

      [TestMethod]
      public void MoniPlus2sExtension_ReceivedAssetStateMessage()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Received AssetStateMessage from application";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("RECEIVED", line.CommunicationResult_Comment);
         Assert.AreEqual("AssetStateMessage", line.RestResource);
      }

      [TestMethod]
      public void MoniPlus2sExtension_CouldNotFindEnabledDeviceListData()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Could not find the type data for EnabledDeviceList";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         // no fields updated
      }

      [TestMethod]
      public void MoniPlus2sExtension_NoConnectionToApplication()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] There is no connection to the application.  Failed to send OperatingMode: {""AssetId"":1,""AssetName"":""A036205"",""ModeType"":""Scheduled"",""ModeName"":""Standard"",""CoreStatus"":"""",""CoreProperties"":""""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("NO CONNECTION", line.ApplicationConnectionState);
         Assert.AreEqual("SEND FAILED", line.CommunicationResult_Comment);
         Assert.AreEqual("Standard", line.OperatingMode_ModeName);
         Assert.AreEqual("Scheduled", line.OperatingMode_ModeType);
         Assert.AreEqual("A036205", line.AssetName);
         Assert.AreEqual(1, line.Asset_Id);
         // no fields updated
      }

      [TestMethod]
      public void MoniPlus2sExtension_ReceivedTellerSessionRequest()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Received TellerSessionRequest from application";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("RECEIVED", line.CommunicationResult_Comment);
         Assert.AreEqual("TellerSessionRequest", line.RestResource);
      }

      [TestMethod]
      public void MoniPlus2sExtension_SendingTellerSessionMessage()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Sending TellerSession to application: {""Id"":147534,""AssetName"":""WI000902"",""TellerSessionRequestId"":155419,""Timestamp"":""2023-11-13T08:11:45.3664556-06:00"",""TellerInfo"":{""ClientSessionId"":6782,""TellerName"":""Mike"",""VideoConferenceUri"":""10.200.100.200"",""TellerId"":""mluckham""}}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("SENDING", line.CommunicationResult_Comment);
         Assert.AreEqual("TellerSession", line.RestResource);
         Assert.AreEqual("WI000902", line.AssetName);
         Assert.AreEqual(147534, line.TellerSession_Id);
         Assert.AreEqual("mluckham", line.TellerId);
         Assert.AreEqual("Mike", line.TellerName);
         Assert.AreEqual("10.200.100.200", line.TellerVideoConferenceUri);
         Assert.AreEqual(6782, line.TellerInfo_ClientSessionId);
         Assert.AreEqual(155419, line.TellerSessionRequest_Id);
      }

      [TestMethod]
      public void MoniPlus2sExtension_SendingRemoteControlSessionMessage()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Sending StartRemoteControlSessionMessage to application: {""TellerInfo"":{""ClientSessionId"":6782,""TellerName"":""Mike"",""VideoConferenceUri"":""10.200.100.200"",""TellerId"":""mluckham""}}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("SENDING", line.CommunicationResult_Comment);
         Assert.AreEqual("StartRemoteControlSessionMessage", line.RestResource);
         Assert.AreEqual("mluckham", line.TellerId);
         Assert.AreEqual("Mike", line.TellerName);
         Assert.AreEqual("10.200.100.200", line.TellerVideoConferenceUri);
         Assert.AreEqual(6782, line.TellerInfo_ClientSessionId);
      }

      [TestMethod]
      public void MoniPlus2sExtension_SendingRemoteControlTaskMessage()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Sending RemoteControlTaskMessage to application: {""AssetName"":""WI000902"",""TaskId"":1,""TaskName"":""ConfigurationQueryTask"",""EventName"":""ConfigurationRequest"",""EventData"":""{\""Name\"":\""ConfigurationRequest\"",\""TellerId\"":null,\""DateTime\"":\""2023-11-13T08:12:01.058318-06:00\"",\""TaskTimeout\"":null}"",""Extras"":null,""TransactionData"":null,""TellerInfo"":{""ClientSessionId"":6782,""TellerName"":""Mike"",""VideoConferenceUri"":""10.200.100.200"",""TellerId"":""mluckham""}}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("SENDING", line.CommunicationResult_Comment);
         Assert.AreEqual("RemoteControlTaskMessage", line.RestResource);
         Assert.AreEqual("WI000902", line.RemoteControl_AssetName);
         Assert.AreEqual(1, line.RemoteControl_TaskId);
         Assert.AreEqual("ConfigurationQueryTask", line.RemoteControl_TaskName);
         Assert.AreEqual("ConfigurationRequest", line.RemoteControl_EventName);
         Assert.AreEqual("ConfigurationRequest", line.RemoteControlTask_EventData_Name);
         Assert.AreEqual("mluckham", line.TellerId);
         Assert.AreEqual("Mike", line.TellerName);
         Assert.AreEqual("10.200.100.200", line.TellerVideoConferenceUri);
         Assert.AreEqual("2023-11-13 02:12:01.0583", line.RemoteControlTask_EventData_DateTimeUTC);   // converted to UCT
      }

      [TestMethod]
      public void MoniPlus2sExtension_SendingTransactionReviewMessage()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Sending TransactionReviewMessage to application: {""Extras"":""{\""Accounts\"":[],\""Id\"":410230,\""TellerSessionActivityId\"":2236847,\""TransactionType\"":null,\""ApproverId\"":null,\""IdScans\"":[],\""Checks\"":[{\""AcceptStatus\"":\""NULL\"",\""Amount\"":\""3600\"",\""AmountRead\"":\""3600\"",\""AmountScore\"":1000,\""BackImageRelativeUri\"":\""api/checkimages/1400130\"",\""CheckDateRead\"":\""11/12/2023\"",\""CheckDateScore\"":47,\""CheckIndex\"":0,\""FrontImageRelativeUri\"":\""api/checkimages/1400129\"",\""ImageBack\"":\""D:\\\\CHECK21\\\\Bottom1.jpg\"",\""ImageFront\"":\""D:\\\\CHECK21\\\\Top1.jpg\"",\""InvalidReason\"":\""\"",\""Id\"":783007,\""TransactionDetailId\"":410230,\""Review\"":{\""Id\"":608334,\""Approval\"":{\""TellerAmount\"":\""3600\"",\""TellerApproval\"":1,\""Reason\"":\""\"",\""TransactionItemReviewId\"":608334},\""ReasonForReview\"":0,\""TransactionItemId\"":783007}},{\""AcceptStatus\"":\""NULL\"",\""Amount\"":\""4000\"",\""AmountRead\"":\""4000\"",\""AmountScore\"":1000,\""BackImageRelativeUri\"":\""api/checkimages/1400132\"",\""CheckDateRead\"":\""1/9/2023\"",\""CheckDateScore\"":1,\""CheckIndex\"":1,\""FrontImageRelativeUri\"":\""api/checkimages/1400131\"",\""ImageBack\"":\""D:\\\\CHECK21\\\\Bottom2.jpg\"",\""ImageFront\"":\""D:\\\\CHECK21\\\\Top2.jpg\"",\""InvalidReason\"":\""\"",\""Id\"":783008,\""TransactionDetailId\"":410230,\""Review\"":{\""Id\"":608335,\""Approval\"":{\""TellerAmount\"":\""4000\"",\""TellerApproval\"":1,\""Reason\"":\""\"",\""TransactionItemReviewId\"":608335},\""ReasonForReview\"":0,\""TransactionItemId\"":783008}}],\""TransactionCashDetails\"":[{\""Amount\"":\""55\"",\""CashTransactionType\"":1,\""Currency\"":\""USD\"",\""TransactionCurrencyItems\"":[{\""Id\"":355075,\""TransactionCashDetailId\"":783006,\""Value\"":20,\""Quantity\"":2,\""MediaType\"":0},{\""Id\"":355076,\""TransactionCashDetailId\"":783006,\""Value\"":5,\""Quantity\"":1,\""MediaType\"":0},{\""Id\"":355077,\""TransactionCashDetailId\"":783006,\""Value\"":10,\""Quantity\"":1,\""MediaType\"":0}],\""Id\"":783006,\""TransactionDetailId\"":410230,\""Review\"":null}],\""TransactionOtherAmounts\"":[],\""TransactionWarnings\"":[]}"",""TellerInfo"":{""ClientSessionId"":6782,""TellerName"":""Andrea"",""VideoConferenceUri"":""10.200.100.200"",""TellerId"":""aspringman""}}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         Assert.AreEqual("2023-09-25 03:00:55", line.Timestamp);
         Assert.AreEqual("SENDING", line.CommunicationResult_Comment);
         Assert.AreEqual("TransactionReviewMessage", line.RestResource);
         Assert.AreEqual(string.Empty, line.Accounts_Summary[0]);

         Assert.AreEqual("55", line.CashDetails_Amount[0]);
         Assert.AreEqual(1, line.CashDetails_CashTransactionType[0]);
         Assert.AreEqual("USD", line.CashDetails_Currency[0]);

         Assert.AreEqual("NULL", line.Checks_AcceptStatus[0]);
         Assert.AreEqual("NULL", line.Checks_AcceptStatus[1]);
         Assert.AreEqual("3600", line.Checks_Amount[0]);
         Assert.AreEqual("4000", line.Checks_Amount[1]);
         Assert.AreEqual("3600", line.Checks_AmountRead[0]);
         Assert.AreEqual("4000", line.Checks_AmountRead[1]);
         Assert.AreEqual(1000, line.Checks_AmountScore[0]);
         Assert.AreEqual(1000, line.Checks_AmountScore[1]);
         // everything should be here, can also Assert some other fields but omit many others to save time coding
         Assert.AreEqual("AcceptStatus:NULL;Amount:3600;AmountRead:3600;AmountScore:1000;BackImageRelativeUri:api/checkimages/1400130;CheckDateRead:11/12/2023;CheckDateScore:47;CheckIndex:0;FrontImageRelativeUri:api/checkimages/1400129;ImageBack:D:\\CHECK21\\Bottom1.jpg;ImageFront:D:\\CHECK21\\Top1.jpg;InvalidReason:;Id:783007;TransactionDetailId:410230;Review:Id:608334;Approval:[TellerAmount:3600;TellerApproval:1;Reason:;TransactionItemReviewId:608334;];ReasonForReview:0;TransactionItemId:783007;;AcceptStatus:NULL;Amount:4000;AmountRead:4000;AmountScore:1000;BackImageRelativeUri:api/checkimages/1400132;CheckDateRead:1/9/2023;CheckDateScore:1;CheckIndex:1;FrontImageRelativeUri:api/checkimages/1400131;ImageBack:D:\\CHECK21\\Bottom2.jpg;ImageFront:D:\\CHECK21\\Top2.jpg;InvalidReason:;Id:783008;TransactionDetailId:410230;Review:Id:608335;Approval:[TellerAmount:4000;TellerApproval:1;Reason:;TransactionItemReviewId:608335;];ReasonForReview:0;TransactionItemId:783008;;", line.Checks_Summary[0]);
         Assert.AreEqual(string.Empty, line.Checks_InvalidReason[0]);
         Assert.AreEqual(string.Empty, line.Checks_InvalidReason[1]);
         Assert.AreEqual("11/12/2023", line.Checks_CheckDateRead[0]);
         Assert.AreEqual("1/9/2023", line.Checks_CheckDateRead[1]);
         Assert.AreEqual(47, line.Checks_CheckDateScore[0]);
         Assert.AreEqual(1, line.Checks_CheckDateScore[1]);
         Assert.AreEqual(0, line.Checks_CheckIndex[0]);
         Assert.AreEqual(1, line.Checks_CheckIndex[1]);
      }

      /*
       *  2023-11-17 03:00:22 [MoniPlus2sExtension] Firing agent message event: Asset - POST - {"Capabilities":null,"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":null,"Model":null,"Name":null,"Status":"Offline","StatusChangedTime":"2023-11-17T03:00:22.2989197-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}
       *  2023-11-17 03:01:57 [MoniPlus2sExtension] Firing agent message event: EnabledDeviceList - POST - PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA
       *  2023-11-17 03:01:57 [MoniPlus2sExtension] Firing agent message event: Asset - POST - {"Capabilities":[{"AssetName":"A036201","Name":"IsInvalidCheckEditingSupported","Value":"True"},{"AssetName":"A036201","Name":"IsLoanPaymentSupported","Value":"True"},{"AssetName":"A036201","Name":"IsCheckHoldsSupported","Value":"True"},{"AssetName":"A036201","Name":"ModeSwitching","Value":"NonTransaction"},{"AssetName":"A036201","Name":"SupportedCallType","Value":"BeeHD"}],"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":"","Model":"7800I","Name":"A036201","Status":"Out of Service","StatusChangedTime":"2023-11-17T03:01:57.6118107-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}
       *  2023-11-17 03:02:00 [MoniPlus2sExtension] Firing agent message event: EnabledDeviceList - POST - PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA
       *  2023-11-17 03:02:00 [MoniPlus2sExtension] Firing agent message event: Asset - POST - {"Capabilities":[{"AssetName":"A036201","Name":"IsInvalidCheckEditingSupported","Value":"True"},{"AssetName":"A036201","Name":"IsLoanPaymentSupported","Value":"True"},{"AssetName":"A036201","Name":"IsCheckHoldsSupported","Value":"True"},{"AssetName":"A036201","Name":"ModeSwitching","Value":"NonTransaction"},{"AssetName":"A036201","Name":"SupportedCallType","Value":"BeeHD"}],"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":"","Model":"7800I","Name":"A036201","Status":"In Service","StatusChangedTime":"2023-11-17T03:02:00.230442-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}
       *  2023-11-17 03:02:07 [MoniPlus2sExtension] Firing agent message event: ApplicationState - POST - {"Id":0,"AssetName":"A036201","ApplicationAvailability":0,"Customer":{"CustomerId":""},"Timestamp":"2023-11-17T03:02:07.7619075-05:00","FlowPoint":"Common-Idle","State":null,"OperatingMode":"Standard","TransactionType":"","Language":"English","VoiceGuidance":false}
       *  2023-11-17 06:05:15 [MoniPlus2sExtension] Firing agent message event: ApplicationState - POST - {"Id":0,"AssetName":"A036201","ApplicationAvailability":0,"Customer":{"CustomerId":""},"Timestamp":"2023-11-17T06:05:15.6556201-05:00","FlowPoint":"Common-MainMenu","State":"Identification","OperatingMode":"Standard","TransactionType":"","Language":"English","VoiceGuidance":false}
       *  2023-11-13 08:06:23 [MoniPlus2sExtension] Firing agent message event: TellerSessionRequest - POST - {"Id":0,"AssetName":"WI000902","Timestamp":"2023-11-13T08:06:23.8980976-06:00","CustomerId":"","FlowPoint":"Common-RequestAssistance","RequestContext":"TellerIdentificationButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
       *  2023-11-13 08:07:23 [MoniPlus2sExtension] Firing agent message event: AssistRequest - POST - {"CustomerId":"","FlowPoint":"Common-RequestAssistance","ApplicationState":"PostIdle","TransactionType":null,"Language":"English","VoiceGuidance":false,"Id":0,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:07:23.4869909-06:00","TellerInfo":null}
       *  2023-11-13 08:07:29 [MoniPlus2sExtension] Firing agent message event: RemoteControlSession - POST - {"StartTime":"2023-11-13T08:07:29.226071-06:00","Id":2236837,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:11:57.9696555-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.200.100.200","TellerId":"aspringman"}}
       *  2023-11-13 08:07:29 [MoniPlus2sExtension] Firing agent message event: RemoteControlEvent - POST - {"TaskId":1,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationReceived","Data":"{\"CanDispenseCash\":true,\"CanDispenseCoin\":true,\"CanDispenseCheck\":false,\"CanDepositCash\":false,\"CanDepositCoin\":false,\"CanDepositCheck\":false,\"CanDepositCashCheck\":true,\"IsCheckCashingSurchargeSupported\":true,\"IsReceiptBufferingSupported\":true,\"IsTransferSupported\":true,\"IsPaymentSupported\":true,\"IsMultiDeviceDepositSupported\":false,\"IsWithdrawalToThePennySupported\":true,\"Name\":\"ConfigurationReceived\",\"Detail\":\"OK\",\"TransactionDetail\":null}","Id":0,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:07:29.8308256-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":null,"VideoConferenceUri":null,"TellerId":null}}
       */


      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void MoniPlus2sExtension_Unsupported()
      {
         string sampleLine = @"2023-09-25 03:00:55 [MoniPlus2sExtension] Is not a supported line.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = null;

         try
         {
            line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.MoniPlus2sExtension: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void MoniPlus2sExtension_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:21 not an Extension started log line";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         MoniPlus2sExtension line = null;

         try
         {
            line = new MoniPlus2sExtension(logFileHandler, sampleLine, AELogType.MoniPlus2sExtension);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.MoniPlus2sExtension: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
