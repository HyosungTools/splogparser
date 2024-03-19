using System;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class AWLogHandler : LogHandler, ILogFileHandler
   {
      public AWLogHandler(ICreateStreamReader createReader) : base(ParseType.AW, createReader)
      {
         LogExpression = "Workstation*.*";
         Name = "AWLogFileHandler";
      }


      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
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
            //==========================================================================================================
            // - Software Name    = C:\Program Files (x86)\Nautilus Hyosung\ActiveTeller\Workstation\NH.ActiveTeller.Client.exe
            // - Version          = 1.3.0.0
            // - File Description = ActiveTeller Workstation
            //==========================================================================================================
            //[2023-09-13 10:02:12-399][3][Settings            ]UserSettings Path: C:\Users\lpina\AppData\Local\Nautilus_Hyosung\NH.ActiveTeller.Client.ex_Url_qnalorbjt4q0zmjb1ac1ptpbvvqeo5na\1.3.0.0\user.config

            char c = logFile[traceFilePos];
            traceFilePos++;

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
         if (logLine.StartsWith("============"))
         {
            //==========================================================================================================

            // ignore
            return new AWLine(this, logLine, AWLogType.None);
         }

         if (logLine.StartsWith(" - ") && logLine.Length > 21)
         {
            if (logLine[1] == '-' && logLine[20] == '=')
            {
               // - Software Name    = C:\Program Files (x86)\Nautilus Hyosung\ActiveTeller\Workstation\NH.ActiveTeller.Client.exe
               // - Version          = 1.3.0.0
               // - File Description = ActiveTeller Workstation

               //string key = logLine.Substring(3, 17).Trim();
               //string value = logLine.Substring(22).Trim();

               // ignore
               return new AWLine(this, logLine, AWLogType.None);
            }
         }

         if (logLine.Contains("[Settings            ]"))
         {
            //UserSettings Path: C:\Users\lpina\AppData\Local\Nautilus_Hyosung\NH.ActiveTeller.Client.ex_Url_qnalorbjt4q0zmjb1ac1ptpbvvqeo5na\1.3.0.0\user.config

            return new Settings(this, logLine, AWLogType.Settings);
         }

         if (logLine.Contains("[StringResourceManager]"))
         {
            //CurrentCulture = [en-US]
            //There is no StringResource\ResourceDictionary.xaml

            return new StringResourceManager(this, logLine, AWLogType.StringResourceManager);
         }

         if (logLine.Contains("[ConfigurationManager]"))
         {
            //Setting NetOp license key
            //MoveNext: feature list [{"Name":"CustomerSearch","Options":null},{"Name":"TellerControl","Options":null},{"Name":"TellerInitiatedSession","Options":null},{"Name":"CustomerIdentification","Options":null},{"Name":"InvalidCheckEditing","Options":null},{"Name":"CheckHolds","Options":null},{"Name":"PeerToPeerVideo","Options":null},{"Name":"SavedImagesEnabled","Options":null},{"Name":"RemoteDesktop","Options":null}]
            //MoveNext: settings list [{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"False"},{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"},{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"},{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"False"},{"Name":"JournalHistory","Type":"System.Int32","Value":"90"},{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"},{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"},{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="},{"Name":"SavedImagesHistory","Type":"System.Int32","Value":"90"},{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"},{"Name":"SmtpFromEmail","Type":"System.String","Value":"noreply@domain.com"},{"Name":"SmtpPassword","Type":"System.Security.SecureString","Value":"Z7BKbZXrF+PMTVNvarcz4Q=="},{"Name":"SmtpPortNumber","Type":"System.String","Value":"587"},{"Name":"SmtpServer","Type":"System.String","Value":"127.0.0.1"},{"Name":"SmtpUsername","Type":"System.String","Value":"admin"},{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"},{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"},{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"60"},{"Name":"TwilioAccountId","Type":"System.Security.SecureString","Value":"KXwVYDO5FJ1CAQSc359+C1E9GzftP0uwtPzE6jsCui9VydQ4Sm9NKUMZ5rWxtGlG"},{"Name":"TwilioApiKey","Type":"System.Security.SecureString","Value":"XDL1Pih/5D3gOJTc+rRnFMF3+NJHPgoaQDv61cIk62++YER61nDveWllUgZgsS/5"},{"Name":"TwilioPhoneNumber","Type":"System.String","Value":"+19375551212"},{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"7"},{"Name":"VideoRecordingHistory","Type":"System.Int32","Value":"90"}]

            return new ConfigurationManager(this, logLine, AWLogType.ConfigurationManager);
         }

         if (logLine.Contains("[BeeHDVideoControl   ]"))
         {
	         //AcceptVideoCall
	         //GetCallDevices: Successfully set AV devices.
	         //HoldVideoCall
	         //Initialize: Initialized BEEHD Client. SDK version = 4.7.24.8 
	         //Initialize: Video client initializing.
	         //ResumeVideoCall
	         //SetSelectedCamera: Attempting to set the selected camera HD Pro Webcam C920
	         //SetSelectedCamera: BeeHD set camera to [HD Pro Webcam C920] at index [0]
	         //SetSelectedMicrophone: Attempting to set the selected microphone [Headset Microphone (Plantronics Blackwire 3220 Series)]
	         //SetSelectedMicrophone: BeeHD set microphone to [Headset Microphone (Plantronics Blackwire 3220 Series)] at index [0]
	         //SetSelectedSpeaker: Attempting to set the selected speakers Headset Earphone (Plantronics Blackwire 3220 Series)
	         //SetSelectedSpeaker: BeeHD set speaker to [Headset Earphone (Plantronics Blackwire 3220 Series)] at index [1]
	         //SignInServer: SignURI = 192.168.20.144
	         //StopVideoCall
	         //Uninitialize
	         //UpdateCurrentVideoSessionState: Update video session state: SessionStopped
	         //VideoClient_CallStateChanges: callHandle=276570400, prevCallState=RVV2OIP_CALLSTATECONNECTED, callState=RVV2OIP_CALLSTATEDROPPING, callStateReason=RVV2OIP_CALLSTATEREASON_IDLE, callType=RVV2OIP_CALLTYPEVIDEO, remoteCallerName=192.168.53.233, isOutgoing=False
	         //VideoClient_OnHistoryRecordAdded=> kmcRequest=192.168.61.39@192.168.61.39, dateTime=1694623441, duration=33, callType=RVV2OIP_CALLTYPEVIDEO, callProtocol=V2OIP_PROTOCOLTYPE_SIP, isOutgoing=False, isMissedCall=False, isEncrypted=False
	         //VideoClient_OnNewIncomingCall: callHandle=276570400
	         //VideoClient_OnRemoteHold: callHandle=278854480, islocal=True
	         //VideoClient_OnRemoteUnHold: callHandle=278854480, islocal=True
	         //VideoClient_OnUserNotify: callHandle=0, val=2013, severity=SEVERITY_WARNING, userType=NOTIFIER_USER_TYPE_ALL, description=The Feature is Not Supported in the License, additionalInfo=McType MCPTT is not licensed, suggestedAction= 

            return new BeeHDVideoControl(this, logLine, AWLogType.BeeHDVideoControl);
         }

         if (logLine.Contains("[VideoManager        ]"))
         {
            //Attempting to sign in to BeeHd video: uri=192.168.20.144, user name=lpina

            return new VideoManager(this, logLine, AWLogType.VideoManager);
         }

         if (logLine.Contains("[SignInManager       ]"))
         {
            //Attempting to sign into ActiveTeller: userName=lpina, branchId=0
            //The ActiveTeller user lpina is signing in; video uri=192.168.20.144, branchId=0
            //ActiveTeller sign-in received a Success response.
            //The ActiveTeller user is signing out
            //ActiveTeller sign-in received a Unauthorized response.

            return new SignInManager(this, logLine, AWLogType.SignInManager);
         }

         if (logLine.Contains("[PermissionsManager  ]"))
         {
            //Retrieving the user's permissions
   
            return new PermissionsManager(this, logLine, AWLogType.PermissionsManager);
         }

         if (logLine.Contains("[MainWindow          ]"))
         {
            //A new call event from the video control arrived. CanAcceptVideoCall=True, VideoSessonState=SessionConnecting
            //Assistance request information for 
            //BtnEndConference was clicked
            //Disconnected from ActiveTeller
            //Disconnecting from ActiveTeller
            //Done adding teller session for asset TX005022 for teller session request 27950
            //Finished and unloaded Report
            //Finished and unloaded TransactionReview
            //HandleTellerSesssionChanged(Deleted), VideoSessionState=SessionConnecting
            //Loading...
            //MainWindow is closing
            //Moving teller video window to position 3
            //OnInSessionApplicationStateChanged: FlowPoint=
            //OnInSessionApplicationStateChanged: FlowPoint=Common-BufferReceipt
            //PeerToPeer_SignIn. SignIn URI = 192.168.20.144
            //Received RemoteControlSessionChanged event.
            //Remote desktop connected successfully to the asset
            //Signed out from ActiveTeller
            //Starting the ActiveTeller connection because the user is allowed to assist customers
            //Teller session added for asset TX005022 for teller session request 27950
            //Teller session request deleted for teller session request 27945
            //Teller session requested for asset TX005208 for teller session request 28038. IsAcceptable=True. 
            //TransactionReview request information for CashDeposit
            //TransactionReview request information for CheckDeposit
            //Window_Loaded complete
            //Remote desktop encountered an error while connecting to the asset.

            return new MainWindow(this, logLine, AWLogType.MainWindow);
         }

         if (logLine.Contains("[IdleEmpty           ]"))
         {
            //---------PROCESS INFORMATION----------------
            //Date  Time  :9/13/2023 10:04:16 AM
            //Memory      : 187,736,064
            //VM      size: 782,123,008
            //Private size: 172,941,312
            //Handle count:7801
            //--------------------------------------------------

            return new IdleEmpty(this, logLine, AWLogType.IdleEmpty);
         }

         if (logLine.Contains("[ConnectionManager   ]"))
         {
            //ActiveTeller connection 25bc99ad-403b-4e63-8ab7-3afe0a942034 connected to http://V00483107/activeteller/SignalR/signalr/
            //ActiveTeller connection 25bc99ad-403b-4e63-8ab7-3afe0a942034 disconnected
            //ActiveTeller connection registered
            //ActiveTeller connection state change Connecting
            //ActiveTeller connection thread has completed.
            //ActiveTeller connection thread has started.
            //Attempting to start the ActiveTeller connection
            //Deleting remote control session 134647
            //Failed retrieving image from uri api/checkimages/41950 with status Gone
            //Registering the connection to the server for client session 3537.
            //Remote control session 134378 created
            //Request connection...
            //Request teller activities for uri TellerActivities?userid=21&start=2023-09-13T00:00:00.000-06:00&end=2023-09-13T23:59:59.000-06:00
            //Retrieving image with uri api/IdImages/3376
            //Sending approval response for teller session 21409
            //Sending assist session for teller session 21497
            //Sending remote control session for teller session 21413
            //Sending teller session for request 28044
            //Setting teller availability to Available
            //Thread Run has started. Requesting connection to http://V00483107/activeteller/SignalR
            //Update teller session statistics: Pending=0 Current=0
            //Updating teller session statistics subscription: enabled
            //Returned asset health list with count of 15

            return new ConnectionManager(this, logLine, AWLogType.ConnectionManager);
         }

         if (logLine.Contains("[DataFlowManager     ]"))
         {
            // transaction review request 134370 for asset TX005020 added
            // transaction review request 134646 for asset TX005006 updated
            //ActiveTeller connection state changed to Connected
            //ActiveTeller connection state changed to Registered
            //Application state Identification update for asset TX005018 during  for mode Standard
            //Application state Identification update for asset TX005018 during CustomerIdentification for mode Standard
            //Application state Idle update for asset TX005018 during  for mode Standard
            //Application state InTransaction update for asset NM000562 during CheckDeposit for mode Standard
            //Application state InTransaction update for asset TX005006 during CashDeposit for mode Standard
            //Application state InTransaction update for asset TX005009 during CheckDeposit for mode Standard
            //Application state InTransaction update for asset TX005011 during  for mode Standard
            //Application state InTransaction update for asset TX005011 during CashPayment for mode Standard
            //Application state InTransaction update for asset TX005018 during  for mode Standard
            //Application state InTransaction update for asset TX005018 during CheckDeposit for mode Standard
            //Application state InTransaction update for asset TX005018 during CustomerIdentification for mode Standard
            //Application state InTransaction update for asset TX005018 during DepositTBD for mode Standard
            //Application state InTransaction update for asset TX005018 during PaymentTBD for mode Standard
            //Application state InTransaction update for asset TX005018 during PinAuthentication for mode Standard
            //Application state InTransaction update for asset TX005018 during TransferPayment for mode Standard
            //Application state InTransaction update for asset TX005018 during Withdrawal for mode Standard
            //Application state InTransaction update for asset TX005020 during Deposit for mode Standard
            //Application state InTransaction update for asset TX005022 during CheckDeposit for mode Standard
            //Application state MainMenu update for asset TX005018 during  for mode Standard
            //Assist request 135043 for asset TX005011 added
            //Check image api/checkimages/41923 available
            //Control session 134520 for asset TX005009 updated for  transaction
            //Control session 134647 for asset TX005006 deleted
            //Firing remote control session 134996 started for asset NM000564
            //Firing system settings changed event
            //ID Scan image api/IdImages/3377 available
            //No event handlers were found during attempt to fire ActiveTeller connection state changed event.
            //Received AcceptCashCompleted event for DepositTask 8 for asset TX005006
            //Received CommitDepositCompleted event for CheckCashingTask 6 for asset NM000564
            //Received CommitDepositCompleted event for DepositTask 13 for asset TX005011
            //Received ConfigurationReceived event for ConfigurationQueryTask 6 for asset TX005006
            //Received CustomerInputReceived event for CustomerInputTask 7 for asset TX005006
            //Received IdScanCompleted event for ScanIdTask 2 for asset NM000564
            //Received ItemsInserted event for CheckCashingTask 4 for asset NM000564
            //Received ItemsInserted event for DepositTask 12 for asset TX005011
            //Received ItemsPresented event for CheckCashingTask 6 for asset NM000564
            //Received ItemsTaken event for CheckCashingTask 6 for asset NM000564
            //Received MediaReadCompleted event for CheckCashingTask 4 for asset NM000564
            //Received MediaReadCompleted event for DepositTask 17 for asset TX005014
            //Received TaskCompleted event for PrintReceiptTask 7 for asset NM000564
            //Sending transaction item approval: CheckIndex=0, TellerApproval=Approved, TellerAmount=1700000, Reason=
            //Teller session 21408 for asset TX005208 accepted by Diana.
            //Teller session 21497 for asset TX005011 created
            //Teller session request 27933 for asset TX005009 deleted
            //Teller session request 27934 for asset TX005007
            //Teller session statistics update received
            //Transaction item 0 Approved for amount 10000 with reason 

            return new DataFlowManager(this, logLine, AWLogType.DataFlowManager);
         }

         if (logLine.Contains("[DeviceFactory       ]"))
         {
            //Bill dispenser device state {"SafeDoorStatus":"1","DispenserStatus":"0","IntermediateStackerStatus":"0","ShutterStatus":"0","PositionStatus":"0","TransportStatus":"0","TransportStatusStatus":"0","DevicePositionStatus":"0","PowerSaveRecoveryTime":0,"UnitCurrencyID":["   ","   ","USD","USD","USD","USD"],"UnitValue":[0,0,1,5,20,100],"UnitStatus":["0","4","0","0","0","0"],"UnitCount":[0,0,1000,1000,1000,1600],"UnitType":["RETRACTCASSETTE","REJECTCASSETTE","RECYCLING","RECYCLING","RECYCLING","RECYCLING"],"LogicalServiceName":"","ExtraInformation":""}
            //Coin dispenser device state {"SafeDoorStatus":"1","DispenserStatus":"0","IntermediateStackerStatus":"5","ShutterStatus":"4","PositionStatus":"3","TransportStatus":"3","TransportStatusStatus":"4","DevicePositionStatus":"3","PowerSaveRecoveryTime":0,"UnitCurrencyID":["USD","USD","USD","USD"],"UnitValue":[1,5,10,25],"UnitStatus":["0","0","0","0"],"UnitCount":[598,300,99,236],"UnitType":["COINDISPENSER","COINDISPENSER","COINDISPENSER","COINDISPENSER"],"LogicalServiceName":"","ExtraInformation":""}
            //Item processor device state {"AcceptorStatus":"0","MediaStatus":"1","TonerStatus":"0","InkStatus":"3","FrontImageScannerStatus":"0","BackImageScannerStatus":"0","MICRReaderStatus":"0","StackerStatus":"0","ReBuncherStatus":"0","MediaFeederStatus":"0","PositionStatus_Input":"0","PositionStatus_Output":"0","PositionStatus_Refused":"0","ShutterStatus_Input":"0","ShutterStatus_Output":"0","ShutterStatus_Refused":"0","TransportStatus_Input":"0","TransportStatus_Output":"","TransportStatus_Refused":"","TransportMediaStatus_Input":"0","TransportMediaStatus_Output":"0","TransportMediaStatus_Refused":"0","DevicePositionStatus":"0","PowerSaveRecoveryTime":0,"LogicalServiceName":"","ExtraInformation":""}

            return new DeviceFactory(this, logLine, AWLogType.DeviceFactory);
         }

         // unidentified line
         return new AWLine(this, logLine, AWLogType.None);
      }
   }
}
