using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LogLineHandler
{
   public class MoniPlus2sExtension : AELine
   {
      /// <summary>
      /// A value indicating whether a remote teller connection is active.
      /// </summary>
      public static bool _RemoteTellerActive = false;

      public class AssetCapabilities
      {
         public AssetCapabilities(string assetName, string name, string value)
         {
            AssetName = assetName;
            Name = name;
            Value = value;
         }

         public override string ToString()
         {
            return $"{AssetName},{Name},{Value}";
         }

         public string AssetName { get; private set; } = string.Empty;
         public string Name { get; private set; } = string.Empty;
         public string Value { get; private set; } = string.Empty;
      }

      bool isRecognized = false;



      // object tracking Ids
      public long Asset_Id { get; set; } = 0;
      public long AssetState_Id { get; set; } = 0;

      public long ApplicationState_Id { get;set; } = 0;

      public long TransactionDetail_Id { get; set; } = 0;
      public long TransactionDetail_TellerSessionActivityId { get; set; } = 0;

      public long SessionRequest_Id { get; set; } = 0;

      public long RemoteControlSession_Id { get; set; } = 0;
      public long RemoteControlSession_TellerSessionRequestId { get; set; } = 0;

      public long RemoteControl_TaskId { get; set; } = 0;

      public long TellerSession_Id { get; set; } = 0;
      public long TellerSessionRequest_Id { get; set; } = 0;

      public long TellerInfo_ClientSessionId { get; set; } = 0;

      public List<long> CashDetails_Id { get; set; } = new List<long>();

      public List<long> CurrencyItems_Id { get; set; } = new List<long>();
      public List<long> CurrencyItems_CashDetailId { get; set; } = new List<long>();

      public List<long> Checks_Id { get; set; } = new List<long>();
      public List<long> Checks_TransactionDetailId { get; set; } = new List<long>();

      public List<long> Accounts_Id { get; set; } = new List<long>();
      public List<long> Accounts_TransactionDetailId { get; set; } = new List<long>();

      public List<long> Review_Id { get; set; } = new List<long>();
      public long ReviewRequest_Id { get; set; } = 0;
      public List<long> Review_TransactionItemId { get; set; } = new List<long>();
      public List<long> Review_ItemReviewId { get; set; } = new List<long>();

      public string CustomerId { get; set; } = string.Empty;




      // general
      public long ApplicationAvailability { get; set; } = 0;
      public DateTime FlowTimestamp { get; set; } = DateTime.MinValue;
      public string FlowPoint { get; set; } = string.Empty;
      public string TransactionType { get; set; } = string.Empty;
      public string Language { get; set; } = string.Empty;
      public bool VoiceGuidance { get; set; } = false;
      public string RequestContext { get; set; } = string.Empty;



      // operating mode and state
      public string OperatingMode { get; set; } = string.Empty;
      public string State { get; set; } = string.Empty;
      public string ApplicationState { get; set; } = string.Empty;




      // hardware devices
      public string AssetName { get; set; } = string.Empty;
      public string EnabledDeviceList { get; set; } = string.Empty;


      // network
      public string IpAddress { get; set; } = string.Empty;
      public string MacAddress { get; set; } = string.Empty;


      // physical assets and status
      public string Manufacturer { get; set; } = string.Empty;
      public string Model { get; set; } = string.Empty;
      public string Name { get; set; } = string.Empty;

      public List<AssetCapabilities> Capabilities = new List<AssetCapabilities>();
      public string Status { get; set; } = string.Empty;
      public DateTime StatusChangedTime { get; set; } = DateTime.MinValue;
      public DateTime StatusReceivedTime { get; set; } = DateTime.MinValue;




      // operating mode
      public string OperatingMode_ModeType { get; set; } = string.Empty;
      public string OperatingMode_ModeName { get; set; } = string.Empty;
      public string OperatingMode_CoreStatus { get; set; } = string.Empty;
      public string OperatingMode_CoreProperties { get; set; } = string.Empty;



      // video conference method
      public string SupportedCallType { get; set; } = string.Empty;
      public string CallRouting_Summary { get; set; } = string.Empty;



      // Active Teller (workstation)
      public string TellerId { get; set; } = string.Empty;
      public string TellerName { get; set; } = string.Empty;
      public string TellerVideoConferenceUri { get; set; } = string.Empty;
      public string TellerInfo_Summary { get; set; } = string.Empty;



      // teller session request
      public DateTime TellerSessionRequest_Timestamp { get; set; } = DateTime.MinValue;



      // remote control session
      public DateTime RemoteControlSession_StartTime { get; set; } = DateTime.MinValue;
      public DateTime RemoteControlSession_TellerSessionRequestTimestamp { get; set; } = DateTime.MinValue;



      // remote control task
      public string RemoteControl_TaskName { get; set; } = string.Empty;
      public string RemoteControl_EventName { get; set; } = string.Empty;
      public string RemoteControl_AssetName { get; set; } = string.Empty;
      public string RemoteControlTask_EventData_Name { get; set; } = string.Empty;
      public string RemoteControlTask_EventData_TellerId { get; set; } = string.Empty;
      public DateTime RemoteControlTask_EventData_DateTime { get; set; } = DateTime.MinValue;
      public long RemoteControlTask_EventData_TaskTimeout { get; set; } = 0;



      // accounts
      public List<long> Accounts_Action { get; set; } = new List<long>();
      public List<string> Accounts_AccountType { get; set; } = new List<string>();
      public List<string> Accounts_Amount { get; set; } = new List<string>();
      public List<string> Accounts_Summary { get; set; } = new List<string>();



      // transactions
      public List<string> TransactionDetail_ApproverId { get; set; } = new List<string>();
      public List<string> TransactionDetail_Summary { get; set; } = new List<string>();
      public List<string> TransactionData_Summary { get; set; } = new List<string>();
      public List<string> Transaction_Warnings_Summary { get; set; } = new List<string>();
      public List<string> TransactionOtherAmounts_Summary { get; set; } = new List<string>();



      // checks
      public List<string> Checks_Amount { get; set; } = new List<string>();
      public List<string> Checks_AcceptStatus { get; set; } = new List<string>();
      public List<string> Checks_AmountRead { get; set; } = new List<string>();
      public List<long> Checks_AmountScore { get; set; } = new List<long>();
      public List<string> Checks_BackImageRelativeUri { get; set; } = new List<string>();
      public List<string> Checks_CheckDateRead { get; set; } = new List<string>();
      public List<long> Checks_CheckDateScore { get; set; } = new List<long>();
      public List<long> Checks_CheckIndex { get; set; } = new List<long>();
      public List<string> Checks_FrontImageRelativeUri { get; set; } = new List<string>();
      public List<string> Checks_ImageBack { get; set; } = new List<string>();
      public List<string> Checks_ImageFront { get; set; } = new List<string>();
      public List<string> Checks_InvalidReason { get; set; } = new List<string>();
      public List<string> Checks_Summary { get; set; } = new List<string>();



      // cash
      public List<string> CashDetails_Amount { get; set; } = new List<string>();
      public List<long> CashDetails_CashTransactionType { get; set; } = new List<long>();
      public List<string> CashDetails_Currency { get; set; } = new List<string>();
      public List<string> CashDetails_Summary { get; set; } = new List<string>();


      // currency
      public List<long> CurrencyItems_Value { get; set; } = new List<long>();
      public List<long> CurrencyItems_Quantity { get; set; } = new List<long>();
      public List<long> CurrencyItems_MediaType { get; set; } = new List<long>();
      public List<string> CurrencyItems_Summary { get; set; } = new List<string>();



      // id scans
      public List<string> IdScans_Summary { get; set; } = new List<string>();



      // reviews
      public List<long> Review_ReasonForReview { get; set; } = new List<long>();
      public List<string> Review_TellerAmount { get; set; } = new List<string>();
      public List<long> Review_TellerApproval { get; set; } = new List<long>();
      public List<string> Review_Reason { get; set; } = new List<string>();
      public List<string> Review_Summary { get; set; } = new List<string>();



      // (ALWAYS LAST)
      // application (server) comms - RESTful requests
      public string CommunicationResult_Comment { get; set; } = string.Empty;
      public string RestResource { get; set; } = string.Empty;
      public string MessageBody { get; set; } = string.Empty;
      public string HttpRequest { get; set; } = string.Empty;
      public string ApplicationConnectionState { get; set; } = string.Empty;










      public MoniPlus2sExtension(ILogFileHandler parent, string logLine, AELogType aeType = AELogType.MoniPlus2sExtension) : base(parent, logLine, aeType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         /*
            2023-11-17 03:00:22 [MoniPlus2sExtension] The 'MoniPlus2sExtension' extension is started.
            2023-11-17 03:00:22 [MoniPlus2sExtension] An agent message handler has been added to MoniPlus2sExtension
            2023-11-17 03:00:22 [MoniPlus2sExtension] An agent message handler has been added to MoniPlus2sExtension
            *  2023-11-17 03:00:22 [MoniPlus2sExtension] Firing agent message event: Asset - POST - {"Capabilities":null,"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":null,"Model":null,"Name":null,"Status":"Offline","StatusChangedTime":"2023-11-17T03:00:22.2989197-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}
            *  2023-11-17 03:00:22 [MoniPlus2sExtension] There is no connection to the application.  Failed to send OperatingMode: {"AssetId":1,"AssetName":"A036205","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}
            2023-11-17 03:00:28 [MoniPlus2sExtension] Failed to connect. No connection could be made because the target machine actively refused it 127.0.0.1:1301.
            2023-11-17 03:01:57 [MoniPlus2sExtension] Connected.
            2023-11-17 03:01:57 [MoniPlus2sExtension] Received AssetStateMessage from application
            2023-11-17 03:01:57 [MoniPlus2sExtension] Could not find the type data for EnabledDeviceList
            *  2023-11-17 03:01:57 [MoniPlus2sExtension] Firing agent message event: EnabledDeviceList - POST - PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA
            *  2023-11-17 03:01:57 [MoniPlus2sExtension] Firing agent message event: Asset - POST - {"Capabilities":[{"AssetName":"A036201","Name":"IsInvalidCheckEditingSupported","Value":"True"},{"AssetName":"A036201","Name":"IsLoanPaymentSupported","Value":"True"},{"AssetName":"A036201","Name":"IsCheckHoldsSupported","Value":"True"},{"AssetName":"A036201","Name":"ModeSwitching","Value":"NonTransaction"},{"AssetName":"A036201","Name":"SupportedCallType","Value":"BeeHD"}],"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":"","Model":"7800I","Name":"A036201","Status":"Out of Service","StatusChangedTime":"2023-11-17T03:01:57.6118107-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}
            2023-11-17 03:01:58 [MoniPlus2sExtension] An agent message handler has been removed from MoniPlus2sExtension
            2023-11-17 03:01:58 [MoniPlus2sExtension] An agent message handler has been removed from MoniPlus2sExtension
            2023-11-17 03:02:00 [MoniPlus2sExtension] Received AssetStateMessage from application
            2023-11-17 03:02:00 [MoniPlus2sExtension] Could not find the type data for EnabledDeviceList
            *  2023-11-17 03:02:00 [MoniPlus2sExtension] Firing agent message event: EnabledDeviceList - POST - PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA
            *  2023-11-17 03:02:00 [MoniPlus2sExtension] Firing agent message event: Asset - POST - {"Capabilities":[{"AssetName":"A036201","Name":"IsInvalidCheckEditingSupported","Value":"True"},{"AssetName":"A036201","Name":"IsLoanPaymentSupported","Value":"True"},{"AssetName":"A036201","Name":"IsCheckHoldsSupported","Value":"True"},{"AssetName":"A036201","Name":"ModeSwitching","Value":"NonTransaction"},{"AssetName":"A036201","Name":"SupportedCallType","Value":"BeeHD"}],"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":"","Model":"7800I","Name":"A036201","Status":"In Service","StatusChangedTime":"2023-11-17T03:02:00.230442-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}
            2023-11-17 03:02:07 [MoniPlus2sExtension] Received ApplicationState from application
            *  2023-11-17 03:02:07 [MoniPlus2sExtension] Firing agent message event: ApplicationState - POST - {"Id":0,"AssetName":"A036201","ApplicationAvailability":0,"Customer":{"CustomerId":""},"Timestamp":"2023-11-17T03:02:07.7619075-05:00","FlowPoint":"Common-Idle","State":null,"OperatingMode":"Standard","TransactionType":"","Language":"English","VoiceGuidance":false}
            2023-11-17 03:02:07 [MoniPlus2sExtension] Received TellerSessionRequest from application
            *  2023-11-17 06:05:15 [MoniPlus2sExtension] Firing agent message event: ApplicationState - POST - {"Id":0,"AssetName":"A036201","ApplicationAvailability":0,"Customer":{"CustomerId":""},"Timestamp":"2023-11-17T06:05:15.6556201-05:00","FlowPoint":"Common-MainMenu","State":"Identification","OperatingMode":"Standard","TransactionType":"","Language":"English","VoiceGuidance":false}

            * 2023-11-13 08:06:23 [MoniPlus2sExtension] Firing agent message event: TellerSessionRequest - POST - {"Id":0,"AssetName":"WI000902","Timestamp":"2023-11-13T08:06:23.8980976-06:00","CustomerId":"","FlowPoint":"Common-RequestAssistance","RequestContext":"TellerIdentificationButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
            * 2023-11-13 08:07:13 [MoniPlus2sExtension] Sending TellerSession to application: {"Id":147534,"AssetName":"WI000902","TellerSessionRequestId":155419,"Timestamp":"2023-11-13T08:11:45.3664556-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}
            * 2023-11-13 08:07:23 [MoniPlus2sExtension] Firing agent message event: AssistRequest - POST - {"CustomerId":"","FlowPoint":"Common-RequestAssistance","ApplicationState":"PostIdle","TransactionType":null,"Language":"English","VoiceGuidance":false,"Id":0,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:07:23.4869909-06:00","TellerInfo":null}
            * 
            * 2023-11-13 08:07:26 [MoniPlus2sExtension] Sending StartRemoteControlSessionMessage to application: {"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}
            * 2023-11-13 08:07:29 [MoniPlus2sExtension] Firing agent message event: RemoteControlSession - POST - {"StartTime":"2023-11-13T08:07:29.226071-06:00","Id":2236837,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:11:57.9696555-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}
            * 2023-11-13 08:07:29 [MoniPlus2sExtension] Sending RemoteControlTaskMessage to application: {"AssetName":"WI000902","TaskId":1,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationRequest","EventData":"{\"Name\":\"ConfigurationRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-13T08:12:01.058318-06:00\",\"TaskTimeout\":null}","Extras":null,"TransactionData":null,"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}
            2023-11-13 08:07:29 [MoniPlus2sExtension] Firing agent message event: RemoteControlEvent - POST - {"TaskId":1,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationReceived","Data":"{\"CanDispenseCash\":true,\"CanDispenseCoin\":true,\"CanDispenseCheck\":false,\"CanDepositCash\":false,\"CanDepositCoin\":false,\"CanDepositCheck\":false,\"CanDepositCashCheck\":true,\"IsCheckCashingSurchargeSupported\":true,\"IsReceiptBufferingSupported\":true,\"IsTransferSupported\":true,\"IsPaymentSupported\":true,\"IsMultiDeviceDepositSupported\":false,\"IsWithdrawalToThePennySupported\":true,\"Name\":\"ConfigurationReceived\",\"Detail\":\"OK\",\"TransactionDetail\":null}","Id":0,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:07:29.8308256-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":null,"VideoConferenceUri":null,"TellerId":null}}

            2023-11-13 08:09:13 [MoniPlus2sExtension] Sending TransactionReviewMessage to application: {"Extras":"{\"Accounts\":[],\"Id\":410230,\"TellerSessionActivityId\":2236847,\"TransactionType\":null,\"ApproverId\":null,\"IdScans\":[],\"Checks\":[{\"AcceptStatus\":\"NULL\",\"Amount\":\"3600\",\"AmountRead\":\"3600\",\"AmountScore\":1000,\"BackImageRelativeUri\":\"api/checkimages/1400130\",\"CheckDateRead\":\"11/12/2023\",\"CheckDateScore\":47,\"CheckIndex\":0,\"FrontImageRelativeUri\":\"api/checkimages/1400129\",\"ImageBack\":\"D:\\\\CHECK21\\\\Bottom1.jpg\",\"ImageFront\":\"D:\\\\CHECK21\\\\Top1.jpg\",\"InvalidReason\":\"\",\"Id\":783007,\"TransactionDetailId\":410230,\"Review\":{\"Id\":608334,\"Approval\":{\"TellerAmount\":\"3600\",\"TellerApproval\":1,\"Reason\":\"\",\"TransactionItemReviewId\":608334},\"ReasonForReview\":0,\"TransactionItemId\":783007}},{\"AcceptStatus\":\"NULL\",\"Amount\":\"4000\",\"AmountRead\":\"4000\",\"AmountScore\":1000,\"BackImageRelativeUri\":\"api/checkimages/1400132\",\"CheckDateRead\":\"1/9/2023\",\"CheckDateScore\":1,\"CheckIndex\":1,\"FrontImageRelativeUri\":\"api/checkimages/1400131\",\"ImageBack\":\"D:\\\\CHECK21\\\\Bottom2.jpg\",\"ImageFront\":\"D:\\\\CHECK21\\\\Top2.jpg\",\"InvalidReason\":\"\",\"Id\":783008,\"TransactionDetailId\":410230,\"Review\":{\"Id\":608335,\"Approval\":{\"TellerAmount\":\"4000\",\"TellerApproval\":1,\"Reason\":\"\",\"TransactionItemReviewId\":608335},\"ReasonForReview\":0,\"TransactionItemId\":783008}}],\"TransactionCashDetails\":[{\"Amount\":\"55\",\"CashTransactionType\":1,\"Currency\":\"USD\",\"TransactionCurrencyItems\":[{\"Id\":355075,\"TransactionCashDetailId\":783006,\"Value\":20,\"Quantity\":2,\"MediaType\":0},{\"Id\":355076,\"TransactionCashDetailId\":783006,\"Value\":5,\"Quantity\":1,\"MediaType\":0},{\"Id\":355077,\"TransactionCashDetailId\":783006,\"Value\":10,\"Quantity\":1,\"MediaType\":0}],\"Id\":783006,\"TransactionDetailId\":410230,\"Review\":null}],\"TransactionOtherAmounts\":[],\"TransactionWarnings\":[]}","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}
         */

         string searchFor = "[MoniPlus2sExtension]";
         int idx = logLine.IndexOf(searchFor);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + searchFor.Length + 1);

            // EXTERNAL COMMS

            //Connected.
            if (subLogLine == "Connected.")
            {
               isRecognized = true;
               ApplicationConnectionState = "CONNECTED";
            }

            //Disconnected.
            else if (subLogLine == "Disconnected.")
            {
               isRecognized = true;
               ApplicationConnectionState = "DISCONNECTED";
            }

            //Could not find the type data for EnabledDeviceList
            else if (subLogLine == "Could not find the type data for EnabledDeviceList")
            {
               isRecognized = true;
               // TODO
            }

            //An agent message handler has been added to MoniPlus2sExtension
            else if (subLogLine == "An agent message handler has been added to MoniPlus2sExtension")
            {
               isRecognized = true;
               // TODO
            }

            //An agent message handler has been removed from MoniPlus2sExtension
            else if (subLogLine == "An agent message handler has been removed from MoniPlus2sExtension")
            {
               isRecognized = true;
               // TODO
            }

            else
            {
               //Failed to connect. No connection could be made because the target machine actively refused it 127.0.0.1:1301.
               Regex regex = new Regex("Failed to connect. No connection could be made because the target machine actively refused it (?<ipaddress>[0-9\\.].*):(?<port>[0-9]*)\\.$");
               Match m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  ApplicationConnectionState = "NO CONNECTION - REFUSED";

                  IpAddress = $"{m.Groups["ipaddress"].Value}:{m.Groups["port"].Value}";
               }

               //Failed to send <RESOURCETYPE>: <MESSAGEBODY>
               //There is no connection to the application.  Failed to send OperatingMode: {"AssetId":1,"AssetName":"A036205","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}
               regex = new Regex("There is no connection to the application.  Failed to send (?<resourcetype>.*): (?<body>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  RestResource = m.Groups["resourcetype"].Value;
                  MessageBody = m.Groups["body"].Value;

                  ApplicationConnectionState = "NO CONNECTION";
                  CommunicationResult_Comment = "SEND FAILED";
               }

               //Sending <RESOURCETYPE> to application: <MESSAGEBODY>
               //Sending OperatingMode to application: {"AssetId":11,"AssetName":"A036201","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}
               //Sending TellerSession to application: {"Id":147534,"AssetName":"WI000902","TellerSessionRequestId":155419,"Timestamp":"2023-11-13T08:11:45.3664556-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}
               regex = new Regex("Sending (?<resourcetype>.*) to application: (?<body>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  RestResource = m.Groups["resourcetype"].Value;
                  MessageBody = m.Groups["body"].Value;

                  CommunicationResult_Comment = "SENDING";
               }

               //Received <RESOURCETYPE> from application
               //Received AssetStateMessage from application
               regex = new Regex("Received (?<resourcetype>.*) from application");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  RestResource = m.Groups["resourcetype"].Value;

                  CommunicationResult_Comment = "RECEIVED";
               }

               // INTERNAL AGENT COMMS

               //Firing agent message event: OperatingMode - GET - 
               //Firing agent message event: ApplicationState - POST - 
               //Firing agent message event: TellerSessionRequest - DELETE - 
               //Firing agent message event: ApplicationState - POST - {"Id":0,"AssetName":"A036201","ApplicationAvailability":0,"Customer":{"CustomerId":""},"Timestamp":"2023-11-17T03:02:07.7619075-05:00","FlowPoint":"Common-Idle","State":null,"OperatingMode":"Standard","TransactionType":"","Language":"English","VoiceGuidance":false}
               //Firing agent message event: Asset - POST - {"Capabilities":null,"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":null,"Model":null,"Name":null,"Status":"Offline","StatusChangedTime":"2023-11-17T03:00:22.2989197-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}
               //Firing agent message event: TellerSessionRequest - POST - {"Id":0,"AssetName":"WI000902","Timestamp":"2023-11-13T08:06:23.8980976-06:00","CustomerId":"","FlowPoint":"Common-RequestAssistance","RequestContext":"TellerIdentificationButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
               //Firing agent message event: EnabledDeviceList - POST - PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA
               //Firing agent message event: AssistRequest - POST - {"CustomerId":"","FlowPoint":"Common-RequestAssistance","ApplicationState":"PostIdle","TransactionType":null,"Language":"English","VoiceGuidance":false,"Id":0,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:07:23.4869909-06:00","TellerInfo":null}
               searchFor = "Firing agent message event: <RESOURCETYPE> - <HTTPREQUEST> - <MESSAGEBODY>";

               regex = new Regex("Firing agent message event: (?<resourcetype>.*) - (?<httprequest>.*) - (?<body>.*)?$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  RestResource = m.Groups["resourcetype"].Value;
                  HttpRequest = m.Groups["httprequest"].Value;
                  MessageBody = m.Groups["body"].Value;
               }

               //"Sending... {\"ClassName\":\"TellerSessionRequest\",\"Data\":null,\"Method\":\"DELETE\"}"
               regex = new Regex("Sending... (?<requestjson>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;

                  dynamic dynamicSending = JsonConvert.DeserializeObject<ExpandoObject>(m.Groups["requestjson"].ToString(), new ExpandoObjectConverter());

                  RestResource = dynamicSending.ClassName;
                  MessageBody = dynamicSending.Data;
                  HttpRequest = dynamicSending.Method;

                  if (dynamicSending.Data != null)
                  {
                     //TODO
                     throw new Exception($"AELogLine.MoniPlus2sExtension: TODO - dynamicSending Data field is not null for log line '{logLine}'");
                  }
               }

               // PARSE THE MESSAGE BODY

               if (MessageBody != null && MessageBody != string.Empty)
               {
                  switch (RestResource)
                  {
                     case "TellerSessionRequest":

                        //{"Id":0,"AssetName":"WI000902","Timestamp":"2023-11-13T08:06:23.8980976-06:00","CustomerId":"","FlowPoint":"Common-RequestAssistance","RequestContext":"TellerIdentificationButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}

                        string jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicTellerSessionRequest = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{
                           //"Id":0,
                           //"AssetName":"WI000902",
                           //"Timestamp":"2023-11-13T08:06:23.8980976-06:00",
                           //"CustomerId":"",
                           //"FlowPoint":"Common-RequestAssistance",
                           //"RequestContext":"TellerIdentificationButton",
                           //"ApplicationState":"PostIdle",
                           //"TransactionType":"",
                           //"Language":"English",
                           //"VoiceGuidance":false,
                           //"RoutingProfile":{
                           //  "SupportedCallType":"BeeHD"
                           // }
                           //}

                           SessionRequest_Id = dynamicTellerSessionRequest.Id;
                           AssetName = dynamicTellerSessionRequest.AssetName;
                           FlowTimestamp = dynamicTellerSessionRequest.Timestamp;
                           CustomerId = dynamicTellerSessionRequest.CustomerId;
                           FlowPoint = dynamicTellerSessionRequest.FlowPoint;
                           RequestContext = dynamicTellerSessionRequest.RequestContext;
                           ApplicationState = dynamicTellerSessionRequest.ApplicationState;
                           TransactionType = dynamicTellerSessionRequest.TransactionType;
                           Language = dynamicTellerSessionRequest.Language;
                           VoiceGuidance = dynamicTellerSessionRequest.VoiceGuidance;

                           CallRouting_Summary = ProcessRoutingProfile(dynamicTellerSessionRequest.RoutingProfile);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize TellerSessionRequest Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     case "TellerSession":

                        //{"Id":147534,"AssetName":"WI000902","TellerSessionRequestId":155419,"Timestamp":"2023-11-13T08:11:45.3664556-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicTellerSession = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{
                           //"Id":147534,
                           //"AssetName":"WI000902",
                           //"TellerSessionRequestId":155419,
                           //"Timestamp":"2023-11-13T08:11:45.3664556-06:00",
                           //"TellerInfo":{
                           //  "ClientSessionId":6782,
                           //  "TellerName":"Andrea",
                           //  "VideoConferenceUri":"10.206.20.47",
                           //  "TellerId":"aspringman"
                           //  }
                           //}

                           TellerSession_Id = dynamicTellerSession.Id;
                           AssetName = dynamicTellerSession.AssetName;
                           TellerSessionRequest_Timestamp = dynamicTellerSession.Timestamp;
                           TellerSessionRequest_Id = dynamicTellerSession.TellerSessionRequestId;

                           TellerInfo_Summary = ProcessTellerInfo(dynamicTellerSession.TellerInfo);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize Tellersession Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     case "AssistRequest":

                        //{"CustomerId":"","FlowPoint":"Common-RequestAssistance","ApplicationState":"PostIdle","TransactionType":null,"Language":"English","VoiceGuidance":false,"Id":0,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:07:23.4869909-06:00","TellerInfo":null}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicAssistRequest = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{"CustomerId":"",
                           //"FlowPoint":"Common-RequestAssistance",
                           //"ApplicationState":"PostIdle",
                           //"TransactionType":null,
                           //"Language":"English",
                           //"VoiceGuidance":false,
                           //"Id":0,
                           //"AssetName":"WI000902",
                           //"TellerSessionId":147534,
                           //"TransactionDetail":null,
                           //"Timestamp":"2023-11-13T08:07:23.4869909-06:00",
                           //"TellerInfo":null
                           //}

                           CustomerId = dynamicAssistRequest.CustomerId;
                           FlowPoint = dynamicAssistRequest.FlowPoint;
                           ApplicationState = dynamicAssistRequest.ApplicationState;
                           TransactionType = dynamicAssistRequest.TransactionType;
                           Language = dynamicAssistRequest.Language;
                           VoiceGuidance = dynamicAssistRequest.VoiceGuidance;
                           TellerSessionRequest_Id = dynamicAssistRequest.Id;
                           AssetName = dynamicAssistRequest.AssetName;
                           TellerSession_Id = dynamicAssistRequest.TellerSessionId;
                           TellerSessionRequest_Timestamp = dynamicAssistRequest.Timestamp;

                           TransactionDetail_Summary.Add(ProcessTransactionDetail(dynamicAssistRequest.TransactionDetail));

                           TellerInfo_Summary = ProcessTellerInfo(dynamicAssistRequest.TellerInfo);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize AssistRequest Json payload for log line '{logLine}'\n{ex}");
                        }

                        break;

                     case "StartRemoteControlSessionMessage":

                        //{"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicTellerInfo = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //"TellerInfo":{
                           // "ClientSessionId":6782,
                           // "TellerName":"Andrea",
                           // "VideoConferenceUri":"10.206.20.47",
                           // "TellerId":"aspringman"
                           //}

                           TellerInfo_Summary = ProcessTellerInfo(dynamicTellerInfo.TellerInfo);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize TellerInfo Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     case "TransactionReviewMessage":

                        //{"Extras":"{
                        //\"Accounts\":[],
                        //\"Id\":410230,
                        //\"TellerSessionActivityId\":2236847,
                        //\"TransactionType\":null,
                        //\"ApproverId\":null,
                        //\"IdScans\":[],
                        //\"Checks\":[
                        //{\"AcceptStatus\":\"NULL\",
                        //\"Amount\":\"3600\",
                        //\"AmountRead\":\"3600\",
                        //\"AmountScore\":1000,
                        //\"BackImageRelativeUri\":\"api/checkimages/1400130\",
                        //\"CheckDateRead\":\"11/12/2023\",
                        //\"CheckDateScore\":47,
                        //\"CheckIndex\":0,
                        //\"FrontImageRelativeUri\":\"api/checkimages/1400129\",
                        //\"ImageBack\":\"D:\\\\CHECK21\\\\Bottom1.jpg\",
                        //\"ImageFront\":\"D:\\\\CHECK21\\\\Top1.jpg\",
                        //\"InvalidReason\":\"\",
                        //\"Id\":783007,
                        //\"TransactionDetailId\":410230,
                        //\"Review\":{
                        //\"Id\":608334,
                        //\"Approval\":{
                        //\"TellerAmount\":\"3600\",
                        //\"TellerApproval\":1,
                        //\"Reason\":\"\",
                        //\"TransactionItemReviewId\":608334
                        //},
                        //\"ReasonForReview\":0,
                        //\"TransactionItemId\":783007}
                        //},{......}
                        //],
                        //\"TransactionCashDetails\":
                        //[
                        //{\"Amount\":\"55\",
                        //\"CashTransactionType\":1,
                        //\"Currency\":\"USD\",
                        //\"TransactionCurrencyItems\":
                        //[
                        //{\"Id\":355075,
                        //\"TransactionCashDetailId\":783006,
                        //\"Value\":20,
                        //\"Quantity\":2,
                        //\"MediaType\":0
                        //},{,,,}
                        //],
                        //\"Id\":783006,
                        //\"TransactionDetailId\":410230,
                        //\"Review\":null
                        //}
                        //],
                        //\"TransactionOtherAmounts\":[],
                        //\"TransactionWarnings\":[]}",
                        //"TellerInfo":
                        //{
                        //"ClientSessionId":6782,
                        //"TellerName":"Andrea",
                        //"VideoConferenceUri":"10.206.20.47",
                        //"TellerId":"aspringman"
                        //}
                        //}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicTransactionReviewMessage = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           if (dynamicTransactionReviewMessage.Extras != null)
                           {
                              try
                              {
                                 dynamic dynamicExtras = JsonConvert.DeserializeObject<ExpandoObject>(dynamicTransactionReviewMessage.Extras, new ExpandoObjectConverter());

                                 TransactionDetail_Summary.Add(ProcessTransactionDetail(dynamicExtras));
                              }
                              catch (Exception ex)
                              {
                                 throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize TransactionReviewMessage.Extras Json payload for log line '{logLine}'\n{ex}");
                              }
                           }

                           TellerInfo_Summary = ProcessTellerInfo(dynamicTransactionReviewMessage.TellerInfo);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize TransactionReviewMessage Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     case "RemoteControlSession":

                        //{"StartTime":"2023-11-13T08:07:29.226071-06:00","Id":2236837,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:11:57.9696555-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicRemoteControlSession = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{"StartTime":"2023-11-13T08:07:29.226071-06:00",
                           //"Id":2236837,
                           //"AssetName":"WI000902",
                           //"TellerSessionId":147534,
                           //"TransactionDetail":null,
                           //"Timestamp":"2023-11-13T08:11:57.9696555-06:00",
                           //"TellerInfo":{
                           // "ClientSessionId":6782,
                           // "TellerName":"Andrea",
                           // "VideoConferenceUri":"10.206.20.47",
                           // "TellerId":"aspringman"
                           //}
                           //}

                           RemoteControlSession_StartTime = dynamicRemoteControlSession.StartTime;
                           RemoteControlSession_Id = dynamicRemoteControlSession.Id;
                           AssetName = dynamicRemoteControlSession.AssetName;
                           RemoteControlSession_TellerSessionRequestId = dynamicRemoteControlSession.TellerSessionId;
                           RemoteControlSession_TellerSessionRequestTimestamp = dynamicRemoteControlSession.Timestamp;

                           TransactionDetail_Summary.Add(ProcessTransactionDetail(dynamicRemoteControlSession.TransactionDetail));

                           TellerInfo_Summary = ProcessTellerInfo(dynamicRemoteControlSession.TellerInfo);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize RemoteControlSession Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     case "RemoteControlTaskMessage":

                        //{"AssetName":"WI000902","TaskId":1,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationRequest","EventData":"{\"Name\":\"ConfigurationRequest\",\"TellerId\":null,\"DateTime\":\"2023-11-13T08:12:01.058318-06:00\",\"TaskTimeout\":null}","Extras":null,"TransactionData":null,"TellerInfo":{"ClientSessionId":6782,"TellerName":"Andrea","VideoConferenceUri":"10.206.20.47","TellerId":"aspringman"}}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicRemoteControlTaskMessage = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{"AssetName":"WI000902",
                           //"TaskId":1,
                           //"TaskName":"ConfigurationQueryTask",
                           //"EventName":"ConfigurationRequest",
                           //"EventData":"{
                           // \"Name\":\"ConfigurationRequest\",
                           // \"TellerId\":null,
                           // \"DateTime\":\"2023-11-13T08:12:01.058318-06:00\",
                           // \"TaskTimeout\":null
                           //}",
                           //"Extras":null,
                           //"TransactionData":null,
                           //"TellerInfo":{
                           //  "ClientSessionId":6782,
                           //  "TellerName":"Andrea",
                           //  "VideoConferenceUri":"10.206.20.47",
                           //  "TellerId":"aspringman"
                           // }
                           //}

                           RemoteControl_AssetName = dynamicRemoteControlTaskMessage.AssetName;
                           RemoteControl_TaskId = dynamicRemoteControlTaskMessage.TaskId;
                           RemoteControl_TaskName = dynamicRemoteControlTaskMessage.TaskName;
                           RemoteControl_EventName = dynamicRemoteControlTaskMessage.EventName;

                           if (dynamicRemoteControlTaskMessage.EventData != null)
                           {
                              // TODO (deserialize)

                              dynamic dynamicRemoteControlTaskMessageEventData = JsonConvert.DeserializeObject<ExpandoObject>(dynamicRemoteControlTaskMessage.EventData, new ExpandoObjectConverter());

                              RemoteControlTask_EventData_Name = dynamicRemoteControlTaskMessageEventData.Name;
                              RemoteControlTask_EventData_TellerId = dynamicRemoteControlTaskMessageEventData.TellerId != null ? dynamicRemoteControlTaskMessageEventData.TellerId : string.Empty;
                              RemoteControlTask_EventData_DateTime = dynamicRemoteControlTaskMessageEventData.DateTime;
                              RemoteControlTask_EventData_TaskTimeout = dynamicRemoteControlTaskMessageEventData.TaskTimeout != null ? dynamicRemoteControlTaskMessageEventData.TaskTimeout : -1;
                           }

                           if (dynamicRemoteControlTaskMessage.Extras != null)
                           {
                              // TODO
                              dynamic dynamicExtras = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                              TransactionDetail_Summary.Add(ProcessTransactionDetail(dynamicExtras));
                           }

                           if (dynamicRemoteControlTaskMessage.TransactionData != null)
                           {
                              // TODO
                              dynamic dynamicRemoteControlTaskMessageTransactionData = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                              TransactionData_Summary.Add(ProcessTransactionData(dynamicRemoteControlTaskMessageTransactionData));
                           }

                           TellerInfo_Summary = ProcessTellerInfo(dynamicRemoteControlTaskMessage.TellerInfo);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize RemoteControlTaskMessage Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     case "RemoteControlEvent":

                        //{"TaskId":1,"TaskName":"ConfigurationQueryTask","EventName":"ConfigurationReceived","Data":"{\"CanDispenseCash\":true,\"CanDispenseCoin\":true,\"CanDispenseCheck\":false,\"CanDepositCash\":false,\"CanDepositCoin\":false,\"CanDepositCheck\":false,\"CanDepositCashCheck\":true,\"IsCheckCashingSurchargeSupported\":true,\"IsReceiptBufferingSupported\":true,\"IsTransferSupported\":true,\"IsPaymentSupported\":true,\"IsMultiDeviceDepositSupported\":false,\"IsWithdrawalToThePennySupported\":true,\"Name\":\"ConfigurationReceived\",\"Detail\":\"OK\",\"TransactionDetail\":null}","Id":0,"AssetName":"WI000902","TellerSessionId":147534,"TransactionDetail":null,"Timestamp":"2023-11-13T08:07:29.8308256-06:00","TellerInfo":{"ClientSessionId":6782,"TellerName":null,"VideoConferenceUri":null,"TellerId":null}}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicRemoteControlEvent = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{"TaskId":1,
                           //"TaskName":"ConfigurationQueryTask",
                           //"EventName":"ConfigurationReceived",
                           //"Data":"{
                           // \"CanDispenseCash\":true,
                           // \"CanDispenseCoin\":true,
                           // \"CanDispenseCheck\":false,
                           // \"CanDepositCash\":false,
                           // \"CanDepositCoin\":false,
                           // \"CanDepositCheck\":false,
                           // \"CanDepositCashCheck\":true,
                           // \"IsCheckCashingSurchargeSupported\":true,
                           // \"IsReceiptBufferingSupported\":true,
                           // \"IsTransferSupported\":true,
                           // \"IsPaymentSupported\":true,
                           // \"IsMultiDeviceDepositSupported\":false,
                           // \"IsWithdrawalToThePennySupported\":true,
                           // \"Name\":\"ConfigurationReceived\",
                           // \"Detail\":\"OK\",
                           // \"TransactionDetail\":null
                           //}",
                           //"Id":0,
                           //"AssetName":"WI000902",
                           //"TellerSessionId":147534,
                           //"TransactionDetail":null,
                           //"Timestamp":"2023-11-13T08:07:29.8308256-06:00",
                           //"TellerInfo":{
                           // "ClientSessionId":6782,
                           // "TellerName":null,
                           // "VideoConferenceUri":null,
                           // "TellerId":null
                           //}
                           //}

                           RemoteControl_AssetName = dynamicRemoteControlEvent.AssetName;
                           RemoteControl_TaskId = dynamicRemoteControlEvent.TaskId;
                           RemoteControl_TaskName = dynamicRemoteControlEvent.TaskName;
                           RemoteControl_EventName = dynamicRemoteControlEvent.EventName;

                           if (((IDictionary<string,object>)dynamicRemoteControlEvent).ContainsKey("EventData"))
                           {
                              // TODO
                           }

                           if (((IDictionary<string, object>)dynamicRemoteControlEvent).ContainsKey("Extras"))
                           {
                              // TODO
                           }

                           if (((IDictionary<string, object>)dynamicRemoteControlEvent).ContainsKey("TransactionData"))
                           {
                              // TODO
                           }

                           TellerInfo_Summary = ProcessTellerInfo(dynamicRemoteControlEvent.TellerInfo);
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize RemoteControlEvent Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     case "OperatingMode":

                        //{"AssetId":1,"AssetName":"A036205","ModeType":"Scheduled","ModeName":"Standard","CoreStatus":"","CoreProperties":""}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicOperatingMode = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{
                           //"AssetId":1,
                           //"AssetName":"A036205",
                           //"ModeType":"Scheduled",
                           //"ModeName":"Standard",
                           //"CoreStatus":"",
                           //"CoreProperties":""
                           //}

                           Asset_Id = dynamicOperatingMode.AssetId;
                           AssetName = dynamicOperatingMode.AssetName;
                           OperatingMode_ModeType = dynamicOperatingMode.ModeType;
                           OperatingMode_ModeName = dynamicOperatingMode.ModeName;
                           OperatingMode_CoreStatus = dynamicOperatingMode.CoreStatus;
                           OperatingMode_CoreProperties = dynamicOperatingMode.CoreProperties;
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize OperatingMode Json payload for log line '{logLine}'\n{ex}");
                        }

                        break;

                     case "ApplicationState":

                        //{"Id":0,"AssetName":"A036201","ApplicationAvailability":0,"Customer":{"CustomerId":""},"Timestamp":"2023-11-17T03:02:07.7619075-05:00","FlowPoint":"Common-Idle","State":null,"OperatingMode":"Standard","TransactionType":"","Language":"English","VoiceGuidance":false}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicApplicationState = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           //{
                           //"Id":0,
                           //"AssetName":"A036201",
                           //"ApplicationAvailability":0,
                           //"Customer":{
                           // "CustomerId":""
                           // },
                           // "Timestamp":"2023-11-17T03:02:07.7619075-05:00",
                           // "FlowPoint":"Common-Idle",
                           // "State":null,
                           // "OperatingMode":"Standard",
                           // "TransactionType":"",
                           // "Language":"English",
                           // "VoiceGuidance":false
                           // }

                           ApplicationState_Id = dynamicApplicationState.Id;
                           AssetName = dynamicApplicationState.AssetName;
                           ApplicationAvailability = dynamicApplicationState.ApplicationAvailability;

                           if (dynamicApplicationState.Customer != null)
                           {
                              try
                              {
                                 foreach (KeyValuePair<string, object> kvp in dynamicApplicationState.Customer)
                                 {
                                    switch (kvp.Key)
                                    {
                                       case "CustomerId":
                                          CustomerId = (string)kvp.Value;
                                          break;

                                       default:
                                          throw new Exception($"Unhandled customer key value {kvp.Key}");
                                    }
                                 }
                              }
                              catch (Exception ex)
                              {
                                 throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize Customer Json payload for log line '{logLine}'\n{ex}");
                              }
                           }

                           FlowTimestamp = dynamicApplicationState.Timestamp;
                           FlowPoint = dynamicApplicationState.FlowPoint;
                           State = dynamicApplicationState.State != null ? dynamicApplicationState.State : string.Empty;
                           OperatingMode = dynamicApplicationState.OperatingMode;
                           TransactionType = dynamicApplicationState.TransactionType;
                            Language = dynamicApplicationState.Language;
                            VoiceGuidance = dynamicApplicationState.VoiceGuidance;
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize ApplicationState Json payload for log line '{logLine}'\n{ex}");
                        }

                        break;

                     case "Asset":

                        //{"Capabilities":[{"AssetName":"A036201","Name":"IsInvalidCheckEditingSupported","Value":"True"},{"AssetName":"A036201","Name":"IsLoanPaymentSupported","Value":"True"},{"AssetName":"A036201","Name":"IsCheckHoldsSupported","Value":"True"},{"AssetName":"A036201","Name":"ModeSwitching","Value":"NonTransaction"},{"AssetName":"A036201","Name":"SupportedCallType","Value":"BeeHD"}],"Id":0,"IpAddress":"10.10.210.194","MacAddress":"70-85-C2-18-7C-DA","Manufacturer":"","Model":"7800I","Name":"A036201","Status":"Out of Service","StatusChangedTime":"2023-11-17T03:01:57.6118107-05:00","StatusReceivedTime":"0001-01-01T00:00:00"}

                        jsonPayload = MessageBody;

                        try
                        {
                           dynamic dynamicAssetState = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           AssetState_Id = dynamicAssetState.Id;
                           IpAddress = dynamicAssetState.IpAddress;
                           MacAddress = dynamicAssetState.MacAddress;
                           Manufacturer = dynamicAssetState.Manufacturer != null ? dynamicAssetState.Manufacturer : string.Empty;
                           Model = dynamicAssetState.Model != null ? dynamicAssetState.Model : string.Empty;
                           Name = dynamicAssetState.Name != null ? dynamicAssetState.Name : string.Empty;
                           Status = dynamicAssetState.Status;
                           StatusChangedTime = dynamicAssetState.StatusChangedTime;
                           StatusReceivedTime = dynamicAssetState.StatusReceivedTime;

                           if (dynamicAssetState.Capabilities != null)
                           {
                              foreach (ExpandoObject obj in dynamicAssetState.Capabilities)
                              {
                                 string assetName = string.Empty;
                                 string name = string.Empty;
                                 string value = string.Empty;

                                 foreach (KeyValuePair<string, object> kvp in obj)
                                 {
                                    switch (kvp.Key)
                                    {
                                       case "AssetName":
                                          assetName = kvp.Value.ToString();
                                          break;

                                       case "Name":
                                          name = kvp.Value.ToString();
                                          break;

                                       case "Value":
                                          value = kvp.Value.ToString();
                                          break;

                                       default:
                                          break;
                                    }
                                 }

                                 if (assetName != null)
                                 {
                                    Capabilities.Add(new AssetCapabilities(assetName, name, value));
                                 }
                              }
                           }
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize Asset Json payload for log line '{logLine}'\n{ex}");
                        }

                        break;

                     case "EnabledDeviceList":

                        // PIN,CDM,DOR,IDC,SPR,GUD,SNS,IND,AUX,VDM,IDS,MMA

                        EnabledDeviceList = MessageBody;

                        break;

                     case "TransactionReviewRequest":

                        //{
                        //"CustomerId":"",
                        //"FlowPoint":"Common-ProcessTransactionReview",
                        //"ApplicationState":"InTransaction",
                        //"TransactionType":null,
                        //"Language":"English",
                        //"VoiceGuidance":false,
                        //"Id":0,
                        //"AssetName":"WI000902",
                        //"TellerSessionId":147572,
                        //"TransactionDetail":{
                        //  "Accounts":[{
                        //    "Action":1,"Amount":"14000","Warnings":[],"AccountType":"Checking","Id":0,"TransactionDetailId":0,"Review":null
                        //  }],
                        //  "Id":0,
                        //  "TellerSessionActivityId":0,
                        //  "TransactionType":"Deposit",
                        //  "ApproverId":null,
                        //  "IdScans":null,
                        //  "Checks":
                        //  [
                        //    {"AcceptStatus":"NULL",
                        //     "Amount":"2500",
                        //     "AmountRead":"2500",
                        //     "AmountScore":978,
                        //     "BackImageRelativeUri":null,
                        //     "CheckDateRead":"11/10/2023",
                        //     "CheckDateScore":127,
                        //     "CheckIndex":0,
                        //     "FrontImageRelativeUri":null,
                        //     "ImageBack":"D:\\CHECK21\\Bottom1.jpg",
                        //     "ImageFront":"D:\\CHECK21\\Top1.jpg",
                        //     "InvalidReason":"",
                        //     "Id":0,
                        //     "TransactionDetailId":0,
                        //     "Review":{
                        //       "Id":0,
                        //       "Approval":null,
                        //       "ReasonForReview":0,
                        //       "TransactionItemId":0
                        //      }
                        //    },
                        //    {"AcceptStatus":"CHANGE",
                        //      "Amount":"9500",
                        //      "AmountRead":"9500",
                        //      "AmountScore":280,
                        //      "BackImageRelativeUri":null,
                        //      "CheckDateRead":"11/9/2023",
                        //      "CheckDateScore":964,
                        //      "CheckIndex":1,
                        //      "FrontImageRelativeUri":null,
                        //      "ImageBack":"D:\\CHECK21\\Bottom2.jpg",
                        //      "ImageFront":"D:\\CHECK21\\Top2.jpg",
                        //      "InvalidReason":"",
                        //      "Id":0,
                        //      "TransactionDetailId":0,
                        //      "Review":{
                        //        "Id":0,"
                        //        Approval":null,
                        //        "ReasonForReview":1,
                        //        "TransactionItemId":0
                        //      }
                        //    },
                        //    {..}
                        //  ],
                        //  "TransactionCashDetails":
                        //  [
                        //    {"Amount":"2000",
                        //    "CashTransactionType":1,
                        //    "Currency":"USD",
                        //    "TransactionCurrencyItems":[],
                        //    "Id":0,
                        //    "TransactionDetailId":0,
                        //    "Review":{
                        //      "Id":0,
                        //      "Approval":null,
                        //      "ReasonForReview":0,
                        //      "TransactionItemId":0
                        //    }
                        //  }
                        //  ],
                        //  "TransactionOtherAmounts":null,
                        //  "TransactionWarnings":null
                        //},
                        //"Timestamp":"2023-11-13T13:37:36.4944033-06:00",
                        //"TellerInfo":null
                        //}'

                        jsonPayload = MessageBody;

                        // TODO

                        try
                        {
                           dynamic dynamicTransactionReviewRequest = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                           CustomerId = dynamicTransactionReviewRequest.CustomerId;
                           FlowPoint = dynamicTransactionReviewRequest.FlowPoint;
                           ApplicationState = dynamicTransactionReviewRequest.ApplicationState;
                           TransactionType = dynamicTransactionReviewRequest.TransactionType;
                           Language = dynamicTransactionReviewRequest.Language;
                           VoiceGuidance = dynamicTransactionReviewRequest.VoiceGuidance;
                           ReviewRequest_Id = dynamicTransactionReviewRequest.Id;
                           AssetName = dynamicTransactionReviewRequest.AssetName;
                           TellerSession_Id = dynamicTransactionReviewRequest.TellerSessionId;

                           TransactionDetail_Summary.Add(ProcessTransactionDetail(dynamicTransactionReviewRequest.TransactionDetail));
                        }
                        catch (Exception ex)
                        {
                           throw new Exception($"AELogLine.MoniPlus2sExtension: failed to deserialize TransactionReviewRequest Json payload for log line '{logLine}'\n{ex}");
                        }
                        break;

                     default:
                        throw new Exception($"AELogLine.MoniPlus2sExtension: did not handle Resource '{RestResource}'for log line '{logLine}'\n");
                  }
               }
            }
         }

         if (!isRecognized)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: did not recognize the log line '{logLine}'");
         }


         // tracking when a remote teller call has started
         if (RequestContext == "TransactionRequiringReview" ||
             RequestContext == "TellerIdentificationButton" ||
             SupportedCallType != string.Empty)
         {
            _RemoteTellerActive = true;
         }

         // tracking when a remote teller call has ended
         if (_RemoteTellerActive && RestResource == "TellerSessionRequest" && HttpRequest == "DELETE")
         {
            _RemoteTellerActive = false;
         }
      }



      private string ProcessTransactionData(ExpandoObject transactionData)
      {
         if (transactionData == null)
         {
            return string.Empty;
         }

         // TODO - NO EXAMPLE

         StringBuilder sb = new StringBuilder();

         try
         {
            foreach (KeyValuePair<string, object> kvp in transactionData)
            {
               sb.Append($"{kvp.Key}:");

               switch (kvp.Key)
               {
                  case "TODO":
                     break;

                  default:
                     sb.Append($"<TODO>{kvp.Value.ToString()}");
                     throw new Exception($"unhandled key '{kvp.Key}'");
               }

               sb.Append(";");
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"ProcessTransactionData failed: {ex}");
         }

         return sb.ToString();
      }

      private string ProcessTransactionDetail(ExpandoObject transactionDetail)
      {
         if (transactionDetail == null)
         {
            return string.Empty;
         }

         //"TransactionDetail":{
         //  "Accounts":
         //  [
         //    {..},
         //    {..}
         //  ],
         //  "Id":0,
         //  "TellerSessionActivityId":0,
         //  "TransactionType":"Deposit",
         //  "ApproverId":null,
         //  "IdScans":null,
         //  "Checks":
         //  [
         //    {..},
         //    {..}
         //  ],
         //  "TransactionCashDetails":
         //  [
         //    {..},
         //    {..}
         //  ],
         //  "TransactionOtherAmounts":
         //  [
         //    {..},
         //    {..}
         //  ],
         //  "TransactionWarnings":
         //  [
         //    {..},
         //    {..}
         //  ]
         //}

         StringBuilder sb = new StringBuilder();

         try
         {
            foreach (KeyValuePair<string, object> kvp in transactionDetail)
            {
               sb.Append($"{kvp.Key}:");

               switch (kvp.Key)
               {
                  case "Id":
                     TransactionDetail_Id = (long)kvp.Value;
                     sb.Append($"{TransactionDetail_Id}");
                     break;

                  case "TellerSessionActivityId":
                     TransactionDetail_TellerSessionActivityId = (long)kvp.Value;
                     sb.Append($"{TransactionDetail_TellerSessionActivityId}");
                     break;

                  case "TransactionType":
                     TransactionType = (string)kvp.Value;
                     sb.Append($"{TransactionType}");
                     break;

                  case "ApproverId":
                     TransactionDetail_ApproverId.Add((string)kvp.Value);
                     sb.Append($"{TransactionDetail_ApproverId.Last()}");
                     break;

                  case "IdScans":
                     IdScans_Summary.Add(ProcessIdScans(kvp.Value));
                     sb.Append($"{IdScans_Summary.Last()}");
                     break;

                  case "TransactionCashDetails":
                     CashDetails_Summary.Add(ProcessTransactionCashDetails(kvp.Value));
                     sb.Append($"{CashDetails_Summary.Last()}");
                     break;

                  case "TransactionOtherAmounts":
                     TransactionOtherAmounts_Summary.Add(ProcessTransactionOtherAmounts(kvp.Value));
                     sb.Append($"{TransactionOtherAmounts_Summary.Last()}");
                     break;

                  case "TransactionWarnings":
                     Transaction_Warnings_Summary.Add(ProcessTransactionWarnings(kvp.Value));
                     sb.Append($"{Transaction_Warnings_Summary.Last()}");
                     break;

                  case "Checks":
                     Checks_Summary.Add(ProcessChecks(kvp.Value));
                     sb.Append($"{Checks_Summary.Last()}");
                     break;

                  case "Accounts":
                     Accounts_Summary.Add(ProcessAccounts(kvp.Value));
                     sb.Append($"{Accounts_Summary.Last()}");
                     break;

                  default:
                     // TODO
                     throw new Exception($"unhandled key '{kvp.Key}'");
               }

               sb.Append($";");
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessTransactionDetail failed, {ex}.");
         }

         return sb.ToString();
      }

      private string ProcessAccounts(object info)
      {
         //  "Accounts":[{
         //    "Action":1,"Amount":"14000","Warnings":[],"AccountType":"Checking","Id":0,"TransactionDetailId":0,"Review":null
         //  }],

         if (info == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            List<object> details = (List<object>)info;

            foreach (ExpandoObject obj in details)
            {
               foreach (KeyValuePair<string, object> kvp in obj)
               {
                  sb.Append($"{kvp.Key}:");

                  // TODO
                  switch (kvp.Key)
                  {
                     case "Action":
                        Accounts_Action.Add((long)kvp.Value);
                        sb.Append($"{Accounts_Action.Last()}");
                        break;

                     case "Amount":
                        Accounts_Amount.Add((string)kvp.Value);
                        sb.Append($"{Accounts_Amount.Last()}");
                        break;

                     case "Warnings":
                        // TODO
                        Transaction_Warnings_Summary.Add(ProcessTransactionWarnings(kvp.Value));
                        sb.Append($"{Transaction_Warnings_Summary.Last()}");
                        break;

                     case "AccountType":
                        Accounts_AccountType.Add((string)kvp.Value);
                        sb.Append($"{Accounts_AccountType.Last()}");
                        break;

                     case "Id":
                        Accounts_Id.Add((long)kvp.Value);
                        sb.Append($"{Accounts_Id.Last()}");
                        break;

                     case "TransactionDetailId":
                        Accounts_TransactionDetailId.Add((long)kvp.Value);
                        sb.Append($"{Accounts_TransactionDetailId.Last()}");
                        break;

                     case "Review":
                        Review_Summary.Add(ProcessReview((ExpandoObject)kvp.Value));
                        sb.Append($"{Review_Summary.Last()}");
                        break;

                     default:
                        sb.Append($"<TODO>{kvp.Value.ToString()}");
                        throw new Exception($"unhandled key '{kvp.Key}' in TransactionDetail|Accounts");
                  }

                  sb.Append($";");
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessAccounts failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessChecks(object info)
      {
         if (info == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            List<object> details = (List<object>)info;

            //  "Checks":
            //  [
            //    {"AcceptStatus":"NULL",
            //     "Amount":"2500",
            //     "AmountRead":"2500",
            //     "AmountScore":978,
            //     "BackImageRelativeUri":null,
            //     "CheckDateRead":"11/10/2023",
            //     "CheckDateScore":127,
            //     "CheckIndex":0,
            //     "FrontImageRelativeUri":null,
            //     "ImageBack":"D:\\CHECK21\\Bottom1.jpg",
            //     "ImageFront":"D:\\CHECK21\\Top1.jpg",
            //     "InvalidReason":"",
            //     "Id":0,
            //     "TransactionDetailId":0,
            //     "Review":{
            //       "Id":0,
            //       "Approval":null,
            //       "ReasonForReview":0,
            //       "TransactionItemId":0
            //      }
            //    },

            foreach (ExpandoObject cObj in details)
            {
               foreach (KeyValuePair<string, object> kvp in cObj)
               {
                  sb.Append($"{kvp.Key}:");

                  switch (kvp.Key)
                  {
                     case "AcceptStatus":
                        Checks_AcceptStatus.Add((string)kvp.Value);
                        sb.Append($"{Checks_AcceptStatus.Last()}");
                        break;

                     case "Amount":
                        Checks_Amount.Add((string)kvp.Value);
                        sb.Append($"{Checks_Amount.Last()}");
                        break;

                     case "AmountRead":
                        Checks_AmountRead.Add((string)kvp.Value);
                        sb.Append($"{Checks_AmountRead.Last()}");
                        break;

                     case "AmountScore":
                        Checks_AmountScore.Add((long)kvp.Value);
                        sb.Append($"{Checks_AmountScore.Last()}");
                        break;

                     case "BackImageRelativeUri":
                        Checks_BackImageRelativeUri.Add((string)kvp.Value);
                        sb.Append($"{Checks_BackImageRelativeUri.Last()}");
                        break;

                     case "CheckDateRead":
                        Checks_CheckDateRead.Add((string)kvp.Value);
                        sb.Append($"{Checks_CheckDateRead.Last()}");
                        break;

                     case "CheckDateScore":
                        Checks_CheckDateScore.Add((long)kvp.Value);
                        sb.Append($"{Checks_CheckDateScore.Last()}");
                        break;

                     case "CheckIndex":
                        Checks_CheckIndex.Add((long)kvp.Value);
                        sb.Append($"{Checks_CheckIndex.Last()}");
                        break;

                     case "FrontImageRelativeUri":
                        Checks_FrontImageRelativeUri.Add((string)kvp.Value);
                        sb.Append($"{Checks_FrontImageRelativeUri.Last()}");
                        break;

                     case "ImageBack":
                        Checks_ImageBack.Add((string)kvp.Value);
                        sb.Append($"{Checks_ImageBack.Last()}");
                        break;

                     case "ImageFront":
                        Checks_ImageFront.Add((string)kvp.Value);
                        sb.Append($"{Checks_ImageFront.Last()}");
                        break;

                     case "InvalidReason":
                        Checks_InvalidReason.Add((string)kvp.Value);
                        sb.Append($"{Checks_InvalidReason.Last()}");
                        break;

                     case "Id":
                        Checks_Id.Add((long)kvp.Value);
                        sb.Append($"{Checks_Id.Last()}");
                        break;

                     case "TransactionDetailId":
                        Checks_TransactionDetailId.Add((long)kvp.Value);
                        sb.Append($"{Checks_TransactionDetailId.Last()}");
                        break;

                     case "Review":
                        Review_Summary.Add(ProcessReview((ExpandoObject)kvp.Value));
                        sb.Append($"{Review_Summary.Last()}");
                        break;

                     default:
                        sb.Append($"<TODO>{kvp.Value.ToString()}");
                        throw new Exception($"unhandled key '{kvp.Key}' in ProcessChecks");
                  }

                  sb.Append(";");
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessChecks failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessTransactionWarnings(object info)
      {
         if (info == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            List<object> details = (List<object>)info;

            foreach (ExpandoObject obj in details)
            {
               foreach (KeyValuePair<string, object> kvp in obj)
               {
                  sb.Append($"{kvp.Key}:");

                  switch (kvp.Key)
                  {
                     case "TODO":
                        break;

                     default:
                        sb.Append($"<TODO>{kvp.Value.ToString()}");
                        throw new Exception($"unhandled key '{kvp.Key}' in ProcessTransactionWarnings");
                  }

                  sb.Append($";");
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessTransactionWarnings failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessTransactionCurrencyItems(object info)
      {
         if (info == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            List<object> objects = (List<object>)info;

            //"TransactionCurrencyItems\":[
            //  {
            //    \"Id\":355075,
            //    \"TransactionCashDetailId\":783006,
            //    \"Value\":20,
            //    \"Quantity\":2,
            //    \"MediaType\":0
            //  },
            //  {..}
            //]

            foreach (object obj in objects)
            {
               foreach (KeyValuePair<string, object> kvp in (ExpandoObject)obj)
               {
                  sb.Append($"{kvp.Key}:");

                  switch (kvp.Key)
                  {
                     case "Id":
                        CurrencyItems_Id.Add((long)kvp.Value);
                        sb.Append($"{CurrencyItems_Id.Last()}");
                        break;

                     case "TransactionCashDetailId":
                        CurrencyItems_CashDetailId.Add((long)kvp.Value);
                        sb.Append($"{CurrencyItems_CashDetailId.Last()}");
                        break;

                     case "Value":
                        CurrencyItems_Value.Add((long)kvp.Value);
                        sb.Append($"{CurrencyItems_Value.Last()}");
                        break;

                     case "Quantity":
                        CurrencyItems_Quantity.Add((long)kvp.Value);
                        sb.Append($"{CurrencyItems_Quantity.Last()}");
                        break;

                     case "MediaType":
                        CurrencyItems_MediaType.Add((long)kvp.Value);
                        sb.Append($"{CurrencyItems_MediaType.Last()}");
                        break;

                     default:
                        sb.Append($"<TODO>{kvp.Value.ToString()}");
                        throw new Exception($"unhandled key '{kvp.Key}' in ProcessTransactionCurrencyItems");
                  }

                  sb.Append(";");
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessTransactionCurrency failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessTransactionOtherAmounts(object info)
      {
         if (info == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            List<object> details = (List<object>)info;

            foreach (ExpandoObject obj in details)
            {
               foreach (KeyValuePair<string, object> kvp in obj)
               {

                  sb.Append($"{kvp.Key}:");

                  switch (kvp.Key)
                  {
                     case "TODO":
                        break;

                     default:
                        sb.Append($"<TODO>{kvp.Value.ToString()}");
                        throw new Exception($"unhandled key '{kvp.Key}' in ProcessTransactionOtherAmounts");
                  }

                  sb.Append($";");
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessTransactionOtherAmounts failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessTransactionCashDetails(object info)
      {
         if (info == null)
         {
            return string.Empty;
         }

         // {
         //   "Amount":"2000",
         //   "CashTransactionType":1,
         //   "Currency":"USD",
         //   "TransactionCurrencyItems":[],
         //   "Id":0,
         //   "TransactionDetailId":0,
         //   "Review":{
         //     "Id":0,
         //     "Approval":null,
         //     "ReasonForReview":0,
         //     "TransactionItemId":0
         //   }
         // }

         StringBuilder sb = new StringBuilder();

         try
         {
            List<object> details = (List<object>)info;

            foreach (ExpandoObject obj in details)
            {
               foreach (KeyValuePair<string, object> kvp in obj)
               {
                  sb.Append($"{kvp.Key}:");

                  switch (kvp.Key)
                  {
                     case "Amount":
                        CashDetails_Amount.Add((string)kvp.Value);
                        sb.Append($"{CashDetails_Amount.Last()}");
                        break;

                     case "CashTransactionType":
                        CashDetails_CashTransactionType.Add((long)kvp.Value);
                        sb.Append($"{CashDetails_CashTransactionType.Last()}");
                        break;

                     case "Currency":
                        CashDetails_Currency.Add((string)kvp.Value);
                        sb.Append($"{CashDetails_Currency.Last()}");
                        break;

                     case "TransactionCurrencyItems":
                        CurrencyItems_Summary.Add(ProcessTransactionCurrencyItems(kvp.Value));
                        sb.Append($"{CurrencyItems_Summary.Last()}");
                        break;

                     case "Id":
                        CashDetails_Id.Add((long)kvp.Value);
                        sb.Append($"{CashDetails_Id.Last()}");
                        break;

                     case "TransactionDetailId":
                        Checks_TransactionDetailId.Add((long)kvp.Value);
                        sb.Append($"{Checks_TransactionDetailId.Last()}");
                        break;

                     case "Review":
                        Review_Summary.Add(ProcessReview((ExpandoObject)kvp.Value));
                        sb.Append($"{Review_Summary.Last()}");
                        break;

                     default:
                        sb.Append($"<TODO>{kvp.Value.ToString()}");
                        throw new Exception($"unhandled key '{kvp.Key}' in ProcessTransactionCashDetails");
                  }

                  sb.Append(";");
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessTransactionCashDetails failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessIdScans(object info)
      {
         if (info == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            List<object> objects = (List<object>)info;

            foreach (ExpandoObject obj in objects)
            {
               foreach (KeyValuePair<string, object> kvp in obj)
               {
                  sb.Append($"{kvp.Key}:");

                  switch (kvp.Key)
                  {
                     case "TODO":
                        break;

                     default:
                        sb.Append($"<TODO>{kvp.Value.ToString()}");
                        throw new Exception($"unhandled key '{kvp.Key}' in ProcessIdScans");
                  }

                  sb.Append($";");
               }
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessIdScans failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessReview(ExpandoObject review)
      {
         if (review == null)
         {
            return string.Empty;
         }

         // {
         //  "Id":0,
         //  "Approval":null,
         //  "ReasonForReview":0,
         //  "TransactionItemId":0
         // }

         StringBuilder sb = new StringBuilder();

         try
         {
            foreach (KeyValuePair<string, object> kvpR in review)
            {
               sb.Append($"{kvpR.Key}:");

               switch (kvpR.Key)
               {
                  case "Id":
                     Review_Id.Add((long)kvpR.Value);
                     sb.Append(Review_Id.Last());
                     break;

                  case "Approval":
                     sb.Append($"[");

                     if (kvpR.Value != null)
                     {
                        foreach (KeyValuePair<string, object> kvpA in (ExpandoObject)kvpR.Value)
                        {
                           sb.Append($"{kvpA.Key}:");

                           switch (kvpA.Key)
                           {
                              case "TellerAmount":
                                 Review_TellerAmount.Add((string)kvpA.Value);
                                 sb.Append($"{Review_TellerAmount.Last()}");
                                 break;

                              case "TellerApproval":
                                 Review_TellerApproval.Add((long)kvpA.Value);
                                 sb.Append($"{Review_TellerApproval.Last()}");
                                 break;

                              case "Reason":
                                 Review_Reason.Add((string)kvpA.Value);
                                 sb.Append($"{Review_Reason.Last()}");
                                 break;

                              case "TransactionItemReviewId":
                                 Review_ItemReviewId.Add((long)kvpA.Value);
                                 sb.Append($"{Review_ItemReviewId.Last()}");
                                 break;

                              default:
                                 sb.Append($"<TODO>{kvpA.Value.ToString()}");
                                 throw new Exception($"unhandled key '{kvpA.Key}' in ProcessReview|Approval");
                           }

                           sb.Append(";");
                        }
                     }

                     sb.Append($"]");
                     break;

                  case "ReasonForReview":
                     Review_ReasonForReview.Add((long)kvpR.Value);
                     sb.Append($"{Review_ReasonForReview.Last()}");
                     break;

                  case "TransactionItemId":
                     Review_TransactionItemId.Add((long)kvpR.Value);
                     sb.Append($"{Review_TransactionItemId.Last()}");
                     break;

                  default:
                     sb.Append($"<TODO>{kvpR.Value.ToString()}");
                     throw new Exception($"unhandled key '{kvpR.Key}' in ProcessReview");
               }

               sb.Append(";");
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessReview failed. {ex}");
         }

         return sb.ToString();
      }

      private string ProcessRoutingProfile(ExpandoObject routingProfile)
      {
         if (routingProfile == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            foreach (KeyValuePair<string, object> kvp in routingProfile)
            {
               sb.Append($"{kvp.Key}:");

               switch (kvp.Key)
               {
                  case "SupportedCallType":
                     SupportedCallType = (string)kvp.Value;
                     sb.Append($"{SupportedCallType}");
                     break;

                  default:
                     // TODO
                     sb.Append($"<TODO>{kvp.Value.ToString()}");
                     throw new Exception($"unhandled key '{kvp.Key}'");
               }

               sb.Append(";");
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessRoutingProfile failed. {ex}");
         }

         return sb.ToString();
      }


      private string ProcessTellerInfo(ExpandoObject tellerInfo)
      {
         if (tellerInfo == null)
         {
            return string.Empty;
         }

         StringBuilder sb = new StringBuilder();

         try
         {
            foreach (KeyValuePair<string, object> kvp in tellerInfo)
            {
               sb.Append($"{kvp.Key}:");

               switch (kvp.Key)
               {
                  case "ClientSessionId":
                     TellerInfo_ClientSessionId = (long)kvp.Value;
                     sb.Append($"{TellerInfo_ClientSessionId}");
                     break;

                  case "TellerName":
                     TellerName = (string)kvp.Value;
                     sb.Append($"{TellerName}");
                     break;

                  case "VideoConferenceUri":
                     TellerVideoConferenceUri = (string)kvp.Value;
                     sb.Append($"{TellerVideoConferenceUri}");
                     break;

                  case "TellerId":
                     TellerId = (string)kvp.Value;
                     sb.Append($"{TellerId}");
                     break;

                  default:
                     sb.Append($"<TODO>{kvp.Value.ToString()}");
                     throw new Exception($"unhandled key '{kvp.Key}'");
               }

               sb.Append(";");
            }
         }
         catch (Exception ex)
         {
            throw new Exception($"AELogLine.MoniPlus2sExtension: ProcessTellerInfo failed. {ex}");
         }

         return sb.ToString();
      }
   }
}
