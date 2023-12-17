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

      public const string APLOG_FLW_CURRENTMODE =
@"[2023-11-16 03:06:10-108][3][][PowerUpMode         ][UpdateRMSMonitorLEDs][NORMAL]Current Mode: PowerUp
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

      public const string APLOG_FLW_CARD_PAN_1 = 
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

      public const string APLOG_FLW_SCREEN_NAME =
@"[2023-08-16 09:17:53-137][3][][LocalScreenWindowEx ][ProcessPageRequest  ][NORMAL]Starting to change the display to screen name [SelectBills].
";

      public const string APLOG_FLW_STATE_1 =
@"[2023-08-16 09:20:35-025][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: Common-ValidateTransaction (StandardFlowPoint)
";
      public const string APLOG_FLW_STATE_2 =
@"[2023-08-16 09:20:35-000][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: Common-DetermineTransactionReview (StandardFlowPoint)
";
      public const string APLOG_FLW_STATE_3 =
@"[2023-08-16 09:20:35-166][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: PLACEHOLDER-PerformTransactionRequestNDC (StateWrapperFlowPoint)
";

      public const string APLOG_FLW_FUNCTIONKEY =
@"[2023-06-07 07:41:58-476][3][][ScreenDecoratorLocal][OnFunctionKeySelected][NORMAL]Raising FunctionKeySelected event with values FunctionKey[Enter], PinInputData[], ResultData[].
";

      public const string APLOG_FLW_DEVICE_FITNESS =
@"[2023-11-02 19:09:04-449][3][][FitnessData         ][GetDeviceFitness    ][NORMAL]Parameter pDvcStatus:DEVHWERROR
";
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
   }
}
