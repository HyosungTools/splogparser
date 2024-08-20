using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSamples
{
    public class samples_general
    {
      public const string APLOG_INSTALL =
@"==========================================================================================================";

      public const string APLOG_SETTINGS_CONFIG =
@"[2023-11-16 03:01:00-649][3][][ConfigurationFramework][ProcessXMLFiles     ][NORMAL]Adding xml file: C:\Hyosung\MoniPlus2S\Config\Application\Communication.xml
";

      public const string APLOG_CURRENTMODE =
@"[2023-11-16 03:06:10-108][3][][PowerUpMode         ][UpdateRMSMonitorLEDs][NORMAL]Current Mode: PowerUp
";
      public const string APLOG_HOST =
@"[2023-11-16 03:06:09-811][3][][CommunicationFramework][OnConnectedHost][NORMAL] Host Connected
";

      // Card Reader
      public const string APLOG_CARD_OPEN =
@"[2024-01-16 03:02:56-855][3][][CardReader          ][Open                ][NORMAL]CardReader.OpenSessionSync method returned OK
";
      public const string APLOG_CARD_CLOSE =
@"[2024-01-16 03:00:00-153][3][][CardReader          ][Close               ][NORMAL]CardReaderClose called
";
      public const string APLOG_CARD_ONMEDIAINSERTED = 
@"[2024-01-16 09:11:41-552][3][][CardReader          ][OnMediaInserted     ][NORMAL][CardReader Log] OnMediaInserted
";
      public const string APLOG_CARD_ONREADCOMPLETE =
@"[2024-01-16 09:11:42-573][3][][CardReader          ][OnReadComplete      ][NORMAL]OnReadComplete event received
";
      public const string APLOG_CARD_ONEJECTCOMPLETE =
@"[2024-01-16 09:17:28-435][3][][CardReader          ][OnEjectComplete     ][NORMAL]m_NxCard.OnEjectComplete event received
";
      public const string APLOG_CARD_ONMEDIAREMOVED =
@"[2024-01-16 09:17:31-888][3][][CardReader          ][OnMediaRemoved      ][NORMAL]m_NxCard.OnMediaRemoved event received
";
      public const string APLOG_CARD_PAN =
@"[2024-01-16 10:03:33-675][3][][NCompleteICCAppSelectState][SetTrackData        ][NORMAL]Device.CardReader.PANData    : 411395XXXXXX1667
";

      // Pin
      public const string APLOG_PIN_OPEN =
@"[2024-02-27 03:01:12-695][3][][Pinpad              ][Open                ][NORMAL]Pinpad.OpenSessionSync method returned OK
";
      public const string APLOG_PIN_CLOSE =
@"[2024-02-27 03:00:12-160][3][][Pinpad              ][Close               ][NORMAL]PinpadClose called
";
      public const string APLOG_PIN_ISPCI =
@"[2024-02-28 10:14:47-401][3][][Pinpad              ][CheckTheEppIsPci    ][NORMAL]This is PCI EPP
";
      public const string APLOG_PIN_ISTR31 =
@"[2024-02-27 03:01:12-979][3][][Pinpad              ][CheckTheEppSupportTR31][NORMAL]TR31 is supported.
";
      public const string APLOG_PIN_ISTR34 =
@"[2024-02-27 03:01:12-979][3][][Pinpad              ][CheckTheEppSupportTR34][NORMAL]TR34 is supported.
";
      public const string APLOG_PIN_KEYIMPORTED =
@"[2024-02-27 03:02:40-740][3][][Pinpad              ][OnKeyImported       ][NORMAL]m_NxPin.OnKeyImported event received
";
      public const string APLOG_PIN_RAND =
@"[2024-02-27 06:00:45-140][3][][Pinpad              ][OnRandomNumberGenerated][NORMAL]m_NxPin.RandomNumberGenerated event received
";
      public const string APLOG_PIN_PINBLOCK =
@"[2024-02-27 06:00:50-356][3][][Pinpad              ][OnPinBlockComplete  ][NORMAL]m_NxPin.OnPinBlockComplete event received
";
      public const string APLOG_PIN_PINBLOCK_FAILED =
@"[2023-08-16 14:22:41-356][2][][PinEntryState       ][ProcessBuildPINBlock][NORMAL]BuildPINBlock failed, next state is DFC
";
      public const string APLOG_PIN_TIMEOUT =
@"[2024-01-16 16:04:06-363][3][][Pinpad              ][OnTimeout           ][NORMAL]m_NxPin.OnTimeout event received
";
      public const string APLOG_PIN_READCOMPLETE =
@"[2024-01-16 13:52:24-388][3][][Pinpad              ][OnReadPinComplete   ][NORMAL]m_NxPin.OnReadPinComplete event received
";

      public const string APLOG_DISPLAYLOAD =
   @"[2024-01-16 10:12:49-021][3][][LocalScreenWindowEx ][DisplayLoadCompleted][NORMAL][MainFrame.LoadCompleted] for screen [MainMenu].
";

      public const string APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE =
   @"[2024-01-16 10:48:45-625][3][][LocalXmlHelper      ][ProcessExecuteNode  ][NORMAL]ProcessExecuteNode(): About to execute: Class: HelperFunctions, Method: UpdateCheck21XML
";

      public const string APLOG_STATE_CREATED =
   @"[2024-01-16 09:11:46-843][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: Common-EnterPIN (StateWrapperFlowPoint)
";

      public const string APLOG_FUNCTIONKEY_SELECTED =
   @"[2024-01-16 16:42:56-456][3][][ScreenDecoratorLocal][OnFunctionKeySelected][NORMAL]Raising FunctionKeySelected event with values FunctionKey[Withdrawal], PinInputData[], ResultData[].
";

      public const string APLOG_FUNCTIONKEY_SELECTED2 =
@"  INFO [2024-03-08 08:58:12-281] [ScreenDecoratorLocal.OnFunctionKeySelected] The Yes button was pressed.
";

      public const string APLOG_DEVICE_FITNESS =
@"[2023-11-27 16:20:42-413][3][][FitnessData][GetDeviceFitness][NORMAL]Parameter pDvcStatus:DEVHWERROR
";

      public const string DEV_UNSOL_EVENT_1 =
@"[2023-11-27 16:20:42-413][3][][CashAcceptor        ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(PositionStatusChanged, POSITION_EMPTY)
";
      public const string DEV_UNSOL_EVENT_2 =
@"[2023-11-27 16:21:08-992][3][][CardReader          ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(ChipPowerStatusChanged, CHIPPOWER_NOCARD)
";
      public const string DEV_UNSOL_EVENT_3 =
@"[2023-11-27 16:42:59-748][3][][BundleCheckAcceptor ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(MediaBinInfoChanged, MEDIABIN_STATUS)
";

      public const string APLOG_CARD_PAN_1 = 
@"[2023-11-16 14:19:39-217][3][][NCompleteICCAppSelectState][SetTrackData        ][NORMAL]Device.CardReader.PANData    : 405610XXXXXX2366
";

      public const string APLOG_FLW_SWITCH_FIT =
@"[2023-11-16 14:17:24-748][3][][FITSwitchState      ][ExecuteState        ][NORMAL]Next State is to be 001
";

      public const string COMM_FRMWORK_1 =
@"[2023-11-16 03:06:09-624] [3] [] [CommunicationFramework] [Initialize] [NORMAL] Socket Type = CLIENT, Destination = 10.25.15.6, Port Number:6725
";
      public const string COMM_FRMWORK_2 =
@"[2023-11-16 03:00:06-108][3] [] [CommunicationFramework] [SendMessage] [NORMAL] Send: TERM_MSG=EJ_UPLOADBLOCK,EXT=NONE
";
      public const string COMM_FRMWORK_3 =
@"[2023-11-16 03:06:09-811][3][][CommunicationFramework][OnConnectedHost][NORMAL] Host Connected
";
      public const string COMM_FRMWORK_4 =
@"[2023-11-16 03:06:09-967][3][][CommunicationFramework][SendMessage][NORMAL] Send: TERM_MSG=UNSOL_POWER_FAILUER,EXT=NONE
";

      public const string PINPAD_1 =
@"[2023-11-27 16:20:04-259][3][][Pinpad              ][OnReadPinComplete   ][NORMAL]m_NxPin.OnReadPinComplete event received
";
      public const string PINPAD_2 =
@"[2023-11-27 16:20:04-337][3][][Pinpad              ][BuildPINBlock       ][NORMAL]PinFormat = ISO0, PadChar=15
";
      public const string PINPAD_3 =
@"[2023-11-27 16:20:04-744][3][][Pinpad              ][OnPinBlockComplete  ][NORMAL]m_NxPin.OnPinBlockComplete event received
";
      public const string PINPAD_4 =
@"[2023-11-27 16:22:26-983][3][][Pinpad              ][OnRandomNumberGenerated][NORMAL]m_NxPin.RandomNumberGenerated event received
";
      public const string PINBLOCK_FAILED =
@"[2023-08-16 14:22:41-356][2][][PinEntryState       ][ProcessBuildPINBlock][NORMAL]BuildPINBlock failed, next state is DFC
";



//      public const string APLOG_DISPLAYLOAD =
//@"[2023-08-16 09:17:53-137][3][][LocalScreenWindowEx ][ProcessPageRequest  ][NORMAL]Starting to change the display to screen name [SelectBills].
//";

      public const string APLOG_STATE_CREATED_1 =
@"[2023-08-16 09:20:35-025][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: Common-ValidateTransaction (StandardFlowPoint)
";
      public const string APLOG_STATE_CREATED_2 =
@"[2023-08-16 09:20:35-000][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: Common-DetermineTransactionReview (StandardFlowPoint)
";
      public const string APLOG_STATE_CREATED_3 =
@"[2023-08-16 09:20:35-166][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: PLACEHOLDER-PerformTransactionRequestNDC (StateWrapperFlowPoint)
";

//      public const string APLOG_FUNCTIONKEY_SELECTED =
//@"[2023-06-07 07:41:58-476][3][][ScreenDecoratorLocal][OnFunctionKeySelected][NORMAL]Raising FunctionKeySelected event with values FunctionKey[Enter], PinInputData[], ResultData[].
//";

//      public const string APLOG_DEVICE_FITNESS =
//@"[2023-11-02 19:09:04-449][3][][FitnessData         ][GetDeviceFitness    ][NORMAL]Parameter pDvcStatus:DEVHWERROR
//";
      /* Add Key */

      public const string AddKey_1 =
@"[2023-10-31 20:49:31-901][3][][AbstractConfigHandler][AddMoniplusData     ][NORMAL]TimerTable Add Key=00, Value=015
";
      public const string AddKey_2 =
@"[2023-10-31 20:49:31-907][3][][AbstractConfigHandler][AddMoniplusData     ][NORMAL]OptionTable Add Key=Option00, Value=002
";
      public const string AddKey_3 =
@"[2023-10-31 20:57:21-435][3][][AbstractConfigHandler][AddMoniplusData     ][NORMAL]ScreenTable Add Key=143, Value=     TRANSACTION COMPLETED
";
      public const string AddKey_4 =
@"[2023-10-31 04:01:39-960][3][][AbstractConfigHandler][AddMoniplusData     ][NORMAL]TcpIp Add Key=RemoteIP, Value=127.0.0.1
";
      public const string AddKey_5 =
@"[2023-10-31 04:01:39-971][3][][AbstractConfigHandler][AddMoniplusData     ][NORMAL]DialUp Add Key=EnablePredial, Value=False
";
      public const string AddKey_6 =
@"[2023-12-13 03:01:11-589][3][][AbstractConfigHandler][AddMoniplusData     ][NORMAL]Remoting Add Key=RemoteAddress, Value=
";

      /* EJ */

      public const string EJ_1 =
@"[2023-06-07 12:18:39-809][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Session] (ATMId,StartDate,IdentificationType,IdentificationNumberMasked,AuthenticationType,AuthenticationNumberMasked) VALUES ('XB5367','06/07/2023 12:18:39 PM','AccountNumber','9706','SSN','4052')
";
      public const string EJ_2 =
@"[2023-06-07 13:01:59-823][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Transaction] (ATMId,IdRelatedTx,SessionId,[ATMDateTime],TransactionDateTime,TransactionType,SequenceNumber,AccountNumberMasked,AccountType,AmountRequested,AmountDispensed,AmountDeposited,HostType,TotalCashAmount,TotalCheckAmount,TotalChecksDeposited,Success) VALUES ('XB5367',0,106,'6/7/2023 1:01:59 PM','6/7/2023 1:01:59 PM','FastCash','11','6562','Share',50,50,0,'Core',50,0,0,True)
";

      public const string EJ_3 =
@"[2023-12-15 11:43:49-404][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Session] (StartDate,IdentificationType,IdentificationNumberMasked,AuthenticationType) VALUES ('12/15/2023 11:43:49 AM','BankCard','5868','PIN')
";


      public const string HLPR_BILLMIX =
@"[2023-10-31 20:57:19-260][3][][HelperFunctions     ][SetBufferWithBillMix][NORMAL]Using BillMixList: 100~0|20~1|5~0|1~0
";

      public const string CASHDISP_DISPENSE =
@"[2023-10-31 18:00:43-776][3][][CashDispenser       ][DispenseSyncAsync   ][NORMAL]MixAlgo=0, Currency=USD, Amount=0, Disp=0 0 0 24 19 15 
";

      public const string SYMX_DISPENSE =
@"[2023-10-31 18:00:43-655][3][][SymXchangeWebServiceRequestFlowPoint][DispenseCurrency    ][NORMAL]RequiredBillMixList: 100~15|20~19|5~24|1~0
";

      /* EMV */

      public const string APLOG_EMV_INIT =
@"[2024-06-27 08:37:41-763][3][][CardReader          ][EMV_Initial         ][NORMAL][CardReader Log] Call EMV_Initial()";

      public const string APLOG_EMV_INITCHIP =
@"[2024-06-27 08:38:12-599][3][][EMVProcessing       ][InitializeChip      ][NORMAL]InitializeChip() start";

      public const string APLOG_EMV_BUILD_CANDIDATE_LIST =
@"[2024-06-27 08:38:13-460][3][][EMVProcessing       ][BuildCandidateList  ][NORMAL]Device.CardReader.EMV_Sel_BuildCandidateApp()' Result = 1";

      public const string APLOG_EMV_CREATE_APPNAME_LIST =
@"[2024-06-13 16:25:15-783][3][][BeginICCAppSelectionLocalFlowPoint][CreateAppNameList   ][NORMAL]AppNameList  = VISA DEBITO|DEBITO ATH";

      public const string APLOG_EMV_APP_SELECTED =
@"[2024-06-13 05:08:42-563][3][][EMVProcessing       ][AppSelect           ][NORMAL]SelectedAID:A0000000980840, SelectedAIDName:ATH DEBITO";

      public const string APLOG_EMV_PAN =
@"[2024-06-13 05:08:48-185][3][][NCompleteICCAppSelectState][SetTrackData        ][NORMAL]Device.CardReader.PANData    : 487038XXXXXX5106";

      public const string APLOG_EMV_CURRENCY_TYPE =
@"[2024-06-13 05:09:04-186][3][][NSetICCTranDataState][SetCurrencyType     ][NORMAL]      strCurrencyType : 5F2A0208405F360100";

      public const string APLOG_EMV_OFFLINE_AUTH =
@"[2024-06-13 05:09:04-472][3][][CardReader          ][EMV_OffDataAuth     ][NORMAL][CardReader Log] Return EMV_OffDataAuth() : 1";


      /* ManagementJournal */

      public const string APLOG_INSERVICE_ENTERED =
@"
[2024-06-19 06:14:35-032][3][][ManagementFramework ][ModeFramework_ModeChangedEvent][NORMAL]ATMModeName.InService StartRMSService
";
      public const string APLOG_TRANSACTION_TIMEOUT =
@"[2024-06-27 08:31:27-243][3][][ManagementJournal   ][WriteFile           ][NORMAL]Parameter pFileData:TRANSACTION TIMEOUT";

   }
}
