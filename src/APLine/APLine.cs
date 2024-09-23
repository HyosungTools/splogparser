using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum APLogType
   {
      /* Not an APLog line we are interested in */
      None,

      // an install line
      APLOG_INSTALL,

      APLOG_SETTINGS,
      APLOG_SETTINGS_CONFIG,

      APLOG_CURRENTMODE,
      APLOG_HOST,

      APLOG_CARD,
      APLOG_CARD_OPEN,
      APLOG_CARD_CLOSE,
      APLOG_CARD_ONMEDIAPRESENT,
      APLOG_CARD_ONMEDIANOTPRESENT,
      APLOG_CARD_ONMEDIAINSERTED,
      APLOG_CARD_ONREADCOMPLETE,
      APLOG_CARD_ONEJECTCOMPLETE,
      APLOG_CARD_ONMEDIAREMOVED,
      APLOG_CARD_PAN,

      APLOG_RFID_DELETE,
      APLOG_RFID_ACCEPTCANCELLED,
      APLOG_RFID_ONMEDIAINSERTED,
      APLOG_RFID_ONMEDIAREMOVED,
      APLOG_RFID_TIMEREXPIRED,
      APLOG_RFID_ONMEDIAPRESENT,
      APLOG_RFID_ONMEDIANOTPRESENT,
      APLOG_RFID_WAITCOMMANDCOMPLETE,
      APLOG_RFID_COMMAND_COMPLETE_ERROR,

      APLOG_EMV_INIT,
      APLOG_EMV_INITCHIP,
      APLOG_EMV_BUILD_CANDIDATE_LIST,
      APLOG_EMV_CREATE_APPNAME_LIST,
      APLOG_EMV_APP_SELECTED,
      APLOG_EMV_PAN,
      APLOG_EMV_CURRENCY_TYPE,
      APLOG_EMV_OFFLINE_AUTH,
      APLOG_EMV_FAULT_SMART_CARDREADER, 


      APLOG_INSERVICE_ENTERED,
      APLOG_TRANSACTION_TIMEOUT,

      APLOG_FLW_SWITCH_FIT,

      APLOG_PIN,
      APLOG_PIN_OPEN,
      APLOG_PIN_CLOSE,
      APLOG_PIN_ISPCI,
      APLOG_PIN_ISTR31,
      APLOG_PIN_ISTR34,
      APLOG_PIN_KEYIMPORTED,
      APLOG_PIN_RAND,
      APLOG_PIN_PINBLOCK,
      APLOG_PIN_PINBLOCK_FAILED,
      APLOG_PIN_TIMEOUT,
      APLOG_PIN_READCOMPLETE,

      APLOG_DISPLAYLOAD,
      APLOG_SCREENWINDOW, 
      APLOG_KEYPRESS,

      APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE,

      APLOG_STATE_CREATED,
      APLOG_FUNCTIONKEY_SELECTED,
      APLOG_FUNCTIONKEY_SELECTED2,
      APLOG_DEVICE_FITNESS,

      APLOG_EXCEPTION,

      EJInsert,

      APLOG_ADDKEY,

      DEV_UNSOL_EVENT,

      COMM_FRMWORK,

      PINPAD,


      /* CASH DISPENSER */

      CashDispenser_Open,

      /* STATUS */

      /* device */
      CashDispenser_OnLine,
      CashDispenser_OffLine,
      CashDispenser_OnHWError,
      CashDispenser_DeviceError,
      CashDispenser_OnDeviceOK,


      /* position status */

      CashDispenser_NotInPosition,
      CashDispenser_InPosition,


      /* dispense */

      CashDispenser_OnNoDispense,
      CashDispenser_OnDispenserOK,
      CashDispenser_DeGrade,



      /* status - shutter, position, stacker, transport */

      CashDispenser_OnShutterOpen,
      CashDispenser_OnShutterClosed,
      CashDispenser_OnStackerNotEmpty,
      CashDispenser_OnStackerEmpty,
      CashDispenser_OnPositionNotEmpty,
      CashDispenser_OnPositionEmpty,
      CashDispenser_OnTransportNotEmpty,
      CashDispenser_OnTransportEmpty,
      CashDispenser_OnCashUnitChanged,



      /* NDC */
      NDC,
      NDC_ATM2HOST11,
      NDC_ATM2HOST12,
      NDC_ATM2HOST22,
      NDC_ATM2HOST23,
      NDC_ATM2HOST51,
      NDC_ATM2HOST61,
      NDC_HOST2ATM1,
      NDC_HOST2ATM3,
      NDC_HOST2ATM4,
      NDC_HOST2ATM6,
      NDC_HOST2ATM7,

      /* SUMMARY */

      /* summary - set up */

      CashDispenser_SetupCSTList,
      CashDispenser_SetupNoteType,


      /* DISPENSE */

      CashDispenser_OnDenominateComplete,
      CashDispenser_ExecDispense,
      CashDispenser_DispenseSyncAsync,
      CashDispenser_OnDispenseComplete,
      CashDispenser_OnPresentComplete,
      CashDispenser_OnRetractComplete,
      CashDispenser_OnItemsTaken,
      CashDispenser_GetLCULastDispensedCount,
      CashDispenser_UpdateTypeInfoToDispense,

      /* CORE */
      Core_ProcessWithdrawalTransaction_Account,
      Core_ProcessWithdrawalTransaction_Amount,
      Core_DispensedAmount,
      Core_RequiredBillMixList,

      /* HELPER FUNCTIONS */
      HelperFunctions,
      HelperFunctions_GetConfiguredBillMixList,
      HelperFunctions_GetFewestBillMixList,

      SYMX_DISPENSE,


      /*
[2023-04-29 13:17:31-169][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Transaction] (ATMId,IdRelatedTx,SessionId,[ATMDateTime],TransactionDateTime,TransactionType,SequenceNumber,AccountNumberMasked,AccountType,AmountRequested,AmountDispensed,AmountDeposited,HostType,TotalCashAmount,TotalCheckAmount,TotalChecksDeposited,Success) VALUES ('DE00901',0,14454,'4/29/2023 1:17:30 PM','4/29/2023 1:17:30 PM','Advice','9397','6295','Checking',100,0,0,'Core',200,0,0,True)
      */

      APLOG_WD_WITHDRAW,
      APLOG_WD_EMVAMOUNT,
      APLOG_WD_ATM2HOST,
      APLOG_WD_HOST2ATM,
      APLOG_WD_HOSTAMOUNT,
      APLOG_WD_DISPENSE,
      APLOG_WD_PRESENTED,
      APLOG_WD_ITEMSTAKEN,

      APLOG_WD_SETUPCSTLISTINHOST,
      APLOG_WD_SETUPNOTETYPEINFO,

      APLOG_MEMORY,

      APLOG_OPER_MENUNAME,
      APLOG_OPER_DOOR,

      /* Account */

      APLOG_ACCOUNT_ENTERED,

      /* Operator Menu */

      APLOG_OPERATOR_MENU,


      /* ERROR */

      APLOG_ERROR, 

      Error
   }

   public class APLine : LogLine, ILogLine
   {
      public APLogType apType { get; set; }

      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      public APLine(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine)
      {
         this.apType = apType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         // set timeStamp to a default time
         string timestamp = LogLine.DefaultTimestamp;

         // search for timestamp in the log line
         string regExp = @"\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}-\d{3}\]";
         Regex timeRegex = new Regex(regExp);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            timestamp = m.Groups[0].Value;

            // post process the timestamp so its in a form suitable for Excel timestamp math
            timestamp = timestamp.Replace("[", "");
            timestamp = timestamp.Replace("]", "");

            int lastIndex = timestamp.LastIndexOf('-');
            if (lastIndex > -1)
            {
               timestamp = timestamp.Substring(0, lastIndex) + "." + timestamp.Substring(lastIndex + 1);
            }
         }

         return timestamp;
      }

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         /* APLOG_INSTALL */
         if (logLine.StartsWith("=======") && logLine.EndsWith("========\r\n"))
            return new MachineInfo(logFileHandler, logLine);


         /* APLOG_SETTINGS_CONFIG */
         if (logLine.Contains("[ConfigurationFramework") && logLine.Contains("ProcessXMLFiles") && logLine.Contains("Adding xml file:"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_SETTINGS_CONFIG);


         /* APLOG_CURRENTMODE */
         if (logLine.Contains("UpdateRMSMonitorLEDs") && logLine.Contains("Current Mode: "))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_CURRENTMODE);

         /* APLOG_HOST */
         if (logLine.Contains("[CommunicationFramework") && logLine.Contains("OnConnectedHost") && logLine.Contains("Host Connected") ||
             logLine.Contains("[CommunicationFramework") && logLine.Contains("OnDisconnectedHost") && logLine.Contains("Host disconnected"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_HOST);


         /* RFID */
         if (logLine.Contains("[RFIDReader") && logLine.Contains("DeleteTrackData"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_DELETE);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("AcceptCancelled"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_ACCEPTCANCELLED);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("OnMediaInserted"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_ONMEDIAINSERTED);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("OnMediaRemoved"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_ONMEDIAREMOVED);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("TimerExpired"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_TIMEREXPIRED);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("OnMediaStatusChanged") && logLine.Contains("(PRESENT)"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_ONMEDIAPRESENT);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("RaiseDeviceUnSolEvent") && logLine.Contains("MEDIA_NOTPRESENT"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_ONMEDIANOTPRESENT);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("WaitCommandComplete returns"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_RFID_WAITCOMMANDCOMPLETE);

         if (logLine.Contains("[RFIDReader") && logLine.Contains("Command completed with error :"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_RFID_COMMAND_COMPLETE_ERROR);


         /* [CardReader          ] */

         if (logLine.Contains("[CardReader") && !logLine.Contains("[RFIDReader"))
         {
            if (logLine.Contains("[Open") || logLine.Contains("CardReader.Open"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_OPEN);

            if (logLine.Contains("CardReaderClose called"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_CLOSE);

            if (logLine.Contains("DeleteTrackData"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_RFID_DELETE);

            if (logLine.Contains("OnMediaStatusChanged") && logLine.Contains("(PRESENT)"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_ONMEDIAPRESENT);

            if (logLine.Contains("OnMediaStatusChanged") && logLine.Contains("(NOTPRESENT)"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_ONMEDIANOTPRESENT);

            if (logLine.Contains("OnMediaInserted"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_ONMEDIAINSERTED);

            if (logLine.Contains("OnReadComplete"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_ONREADCOMPLETE);

            if (logLine.Contains("OnEjectComplete"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_ONEJECTCOMPLETE);

            if (logLine.Contains("OnMediaRemoved"))
               return new APLine(logFileHandler, logLine, APLogType.APLOG_CARD_ONMEDIAREMOVED);
         }

         if (logLine.Contains("Device.CardReader.PANData    :"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_CARD_PAN);


         /* EMV */

         if (logLine.Contains("[CardReader") && logLine.Contains("EMV_Initial"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_EMV_INIT);

         if (logLine.Contains("[EMVProcessing") && logLine.Contains("InitializeChip() start"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_EMV_INITCHIP);

         if (logLine.Contains("[EMVProcessing") && logLine.Contains("Device.CardReader.EMV_Sel_BuildCandidateApp()' Result ="))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_EMV_BUILD_CANDIDATE_LIST);

         if (logLine.Contains("[BeginICCAppSelectionLocalFlowPoint") && logLine.Contains("AppNameList  ="))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_EMV_CREATE_APPNAME_LIST);

         if (logLine.Contains("[EMVProcessing") && logLine.Contains("SelectedAID:"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_EMV_APP_SELECTED);

         if (logLine.Contains("[NCompleteICCAppSelectState") && logLine.Contains("Device.CardReader.PANData"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_EMV_PAN);

         if (logLine.Contains("[NSetICCTranDataState") && logLine.Contains("strCurrencyType :"))
            return new APLineEmvCurrencyType(logFileHandler, logLine, APLogType.APLOG_EMV_CURRENCY_TYPE);

         if (logLine.Contains("[CardReader") && logLine.Contains("Return EMV_OffDataAuth() :"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_EMV_OFFLINE_AUTH);

         if (logLine.Contains("[CommunicationFramework") && logLine.Contains("EXT=FAULT_SMART_CARDREADER"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_EMV_FAULT_SMART_CARDREADER);

         /* ManagementJournal */

         if (logLine.Contains("[ManagementJournal") && logLine.Contains("INSERVICE ENTERED"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_INSERVICE_ENTERED);

         if (logLine.Contains("[ManagementJournal") && logLine.Contains("TRANSACTION TIMEOUT"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_TRANSACTION_TIMEOUT);

         /* APLOG_FLW_SWITCH_FIT */
         if (logLine.Contains("[FITSwitchState") && logLine.Contains("ExecuteState") && logLine.Contains("Next State is to be "))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_FLW_SWITCH_FIT);


         /* [Pinpad              */
         if (logLine.Contains("[Pinpad") && logLine.Contains("Open"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_OPEN);

         if (logLine.Contains("[Pinpad") && logLine.Contains("Close"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_CLOSE);

         if (logLine.Contains("[Pinpad") && logLine.Contains("CheckTheEppIsPci"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_PIN_ISPCI);

         if (logLine.Contains("[Pinpad") && logLine.Contains("CheckTheEppSupportTR31"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_PIN_ISTR31);

         if (logLine.Contains("[Pinpad") && logLine.Contains("CheckTheEppSupportTR34"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_PIN_ISTR34);

         if (logLine.Contains("[Pinpad") && logLine.Contains("OnKeyImported"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_KEYIMPORTED);

         if (logLine.Contains("[Pinpad") && logLine.Contains("OnRandomNumberGenerated"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_RAND);

         if (logLine.Contains("[Pinpad") && logLine.Contains("OnPinBlockComplete"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_PINBLOCK);

         if (logLine.Contains("[PinEntryState") && logLine.Contains("BuildPINBlock failed"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_PINBLOCK_FAILED);

         if (logLine.Contains("[Pinpad") && logLine.Contains("OnTimeout"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_TIMEOUT);

         if (logLine.Contains("[Pinpad") && logLine.Contains("OnReadPinComplete"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_PIN_READCOMPLETE);



         if (logLine.Contains("[LocalScreenWindowEx") && logLine.Contains("DisplayLoadCompleted"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_DISPLAYLOAD);

         if (logLine.Contains("[ScreenWindow") && logLine.Contains("LogAdditionalInformation"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_SCREENWINDOW);

         if (logLine.Contains("[ScreenDecoratorLocal") && logLine.Contains("PinpadKeyPressed"))
            return new APLine(logFileHandler, logLine, APLogType.APLOG_KEYPRESS);

         if (logLine.Contains("[LocalXmlHelper") && logLine.Contains("About to execute: Class: HelperFunctions, Method:"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE);


         if (logLine.Contains("[HybridFlowEngine") && logLine.Contains("State created:"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_STATE_CREATED);



         /* OLD [ScreenDecoratorLocal][OnFunctionKeySelected][NORMAL]Raising FunctionKeySelected event with values FunctionKey[No], PinInputData[], ResultData[]. */
         if (logLine.Contains("[OnFunctionKeySelected") && logLine.Contains("Raising FunctionKeySelected event with values FunctionKey"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_FUNCTIONKEY_SELECTED);

         /* NEW [ScreenDecoratorLocal.OnFunctionKeySelected] The No button was pressed.*/
         if (logLine.Contains("[ScreenDecoratorLocal.OnFunctionKeySelected]"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_FUNCTIONKEY_SELECTED2);


         if (logLine.Contains("[GetDeviceFitness") && logLine.Contains("Parameter pDvcStatus:"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_DEVICE_FITNESS);


         if (logLine.Contains("?????????????????????????????????????????????"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_EXCEPTION);


         /* AddKey */
         if (logLine.Contains("[AbstractConfigHandler") && logLine.Contains("AddMoniplusData") && logLine.Contains("Add Key="))
            return new AddKey(logFileHandler, logLine);

         /* CASHDISP */
         if (logLine.Contains("[CashDispenser"))
         {
            ILogLine iLine = CashDispenser.Factory(logFileHandler, logLine);
            if (iLine != null) return iLine;
         }



         /* CORE */
         if (logLine.Contains("WebServiceRequestFlowPoint"))
         {
            ILogLine iLine = Core.Factory(logFileHandler, logLine);
            if (iLine != null) return iLine;
         }

         /* EJ */
         if (logLine.Contains("INSERT INTO "))
            return new EJInsert(logFileHandler, logLine);


         /* HELPER FUNCTIONS */
         if (logLine.Contains("[HelperFunctions") && logLine.Contains("[GetConfiguredBillMixList") && logLine.Contains("ConfiguredBillMixList:"))
            return new APLineField(logFileHandler, logLine, APLogType.HelperFunctions_GetConfiguredBillMixList);

         if (logLine.Contains("[HelperFunctions") && logLine.Contains("[GetFewestBillMixList") && logLine.Contains("FewestBillMixList:"))
            return new APLineField(logFileHandler, logLine, APLogType.HelperFunctions_GetFewestBillMixList);

         /* NDC */
         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST:"))
            return Atm2Host.Factory(logFileHandler, logLine);

         if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM:"))
            return Host2Atm.Factory(logFileHandler, logLine);


         /* CASH DISPENSER */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("OnDenominateComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDenominateComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("OnPresentComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnPresentComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("OnItemsTaken"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnItemsTaken);



         //   CashDispenser_Open,

         ///* STATUS */

         ///* device */
         //CashDispenser_OnLine,
         //CashDispenser_OffLine,
         //CashDispenser_OnHWError,
         //CashDispenser_DeviceError,
         //CashDispenser_OnDeviceOK,


         ///* position status */

         //CashDispenser_NotInPosition,
         //CashDispenser_InPosition,


         ///* dispense */

         //CashDispenser_OnNoDispense,
         //CashDispenser_OnDispenserOK,
         //CashDispenser_DeGrade,



         ///* status - shutter, position, stacker, transport */

         //CashDispenser_OnShutterOpen,
         //CashDispenser_OnShutterClosed,
         //CashDispenser_OnStackerNotEmpty,
         //CashDispenser_OnStackerEmpty,
         //CashDispenser_OnPositionNotEmpty,
         //CashDispenser_OnPositionEmpty,
         //CashDispenser_OnTransportNotEmpty,
         //CashDispenser_OnTransportEmpty,
         //CashDispenser_OnCashUnitChanged,

         /* Account */

         if (logLine.Contains("[Account") && logLine.Contains("Account is Entered. Account ="))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_ACCOUNT_ENTERED);

         /* Operator Menu */
         if (logLine.Contains("[OperatorWindow") && logLine.Contains("Parameter pMenuName:"))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_OPERATOR_MENU);

         /* Error */
         if (logLine.Contains(" ERROR ["))
            return new APLineField(logFileHandler, logLine, APLogType.APLOG_ERROR);
         


         return null;
      }
   }
}
