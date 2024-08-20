using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class APLogHandler : LogHandler, ILogFileHandler
   {

      public APLogHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.AP, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(parseType, createReader, Factory)
      {
         LogExpression = "APLog*.*";
         Name = "APLogFileHandler";
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
            char c = ' ';
            try
            {
               c = logFile[traceFilePos];
            }
            catch (Exception e)
            {
               this.ctx.ConsoleWriteLogLine(String.Format("Exception {0} - traceFilePos = {1} logFile.Length = {2}", e.Message, traceFilePos, logFile.Length));
            }
            if (c < 128)
            {
               builder.Append(c);

               // generally, '\n' means EOL
               if (c.Equals('\n'))
               {
                  try
                  {
                     // OLD: if the next char after '\n' is a '['  we are at EOL
                     char cNext = logFile[traceFilePos + 1];
                     endOfLine = (cNext == '[');

                     if (!endOfLine)
                     {
                        // NEW: look for '  INFO [' or '  WARN [' or ' ERROR ['
                        char c1 = logFile[traceFilePos + 1];
                        char c2 = logFile[traceFilePos + 2];
                        char c3 = logFile[traceFilePos + 3];
                        char c4 = logFile[traceFilePos + 4];
                        char c5 = logFile[traceFilePos + 5];
                        char c6 = logFile[traceFilePos + 6];
                        char c7 = logFile[traceFilePos + 7];
                        char c8 = logFile[traceFilePos + 8];

                        endOfLine =
                           (
                              ((c1 == ' ') && (c2 == ' ') && (c3 == 'I') && (c4 == 'N') && (c5 == 'F') && (c6 == 'O') && (c7 == ' ') && (c8 == '[')) ||
                              ((c1 == ' ') && (c2 == ' ') && (c3 == 'W') && (c4 == 'A') && (c5 == 'R') && (c6 == 'N') && (c7 == ' ') && (c8 == '[')) ||
                              ((c1 == ' ') && (c2 == 'E') && (c3 == 'R') && (c4 == 'R') && (c5 == 'O') && (c6 == 'R') && (c7 == ' ') && (c8 == '['))
                           );
                     }
                  }
                  catch (Exception)
                  {
                     endOfLine = true;
                  }
               }
            }
            traceFilePos++;
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         if (this.Factory != null)
         {
            return Factory(this, logLine);
         }

         // (bool success, string apLogLine) result;

         /* APLOG_INSTALL */
         if (logLine.StartsWith("=======") && logLine.EndsWith("========\r\n"))
            return new MachineInfo(this, logLine);


         /* APLOG_SETTINGS_CONFIG */
         if (logLine.Contains("[ConfigurationFramework") && logLine.Contains("ProcessXMLFiles") && logLine.Contains("Adding xml file:"))
            return new APLineField(this, logLine, APLogType.APLOG_SETTINGS_CONFIG);


         /* APLOG_CURRENTMODE */
         if (logLine.Contains("Current Mode: "))
            return new APLineField(this, logLine, APLogType.APLOG_CURRENTMODE);

         /* APLOG_HOST */
         if (logLine.Contains("[CommunicationFramework") && logLine.Contains("OnConnectedHost") && logLine.Contains("Host Connected") ||
             logLine.Contains("[CommunicationFramework") && logLine.Contains("OnDisconnectedHost") && logLine.Contains("Host disconnected"))
            return new APLineField(this, logLine, APLogType.APLOG_HOST);


         /* [CardReader          ] */
         if (logLine.Contains("[CardReader") && logLine.Contains("CardReader.OpenSessionSync"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_OPEN);

         if (logLine.Contains("[CardReader") && logLine.Contains("CardReaderClose called"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_CLOSE);

         if (logLine.Contains("[CardReader") && logLine.Contains("OnMediaStatusChanged") && logLine.Contains("(PRESENT)"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_ONMEDIAPRESENT);

         if (logLine.Contains("[CardReader") && logLine.Contains("OnMediaStatusChanged") && logLine.Contains("(NOTPRESENT)"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_ONMEDIANOTPRESENT);

         if (logLine.Contains("[CardReader") && logLine.Contains("OnMediaInserted"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_ONMEDIAINSERTED);

         if (logLine.Contains("[CardReader") && logLine.Contains("OnReadComplete"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_ONREADCOMPLETE);

         if (logLine.Contains("[CardReader") && logLine.Contains("OnEjectComplete"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_ONEJECTCOMPLETE);

         if (logLine.Contains("[CardReader") && logLine.Contains("OnMediaRemoved"))
            return new APLine(this, logLine, APLogType.APLOG_CARD_ONMEDIAREMOVED);

         if (logLine.Contains("Device.CardReader.PANData    :"))
            return new APLineField(this, logLine, APLogType.APLOG_CARD_PAN);

         /* EMV */

         if (logLine.Contains("[CardReader") && logLine.Contains("EMV_Initial"))
            return new APLine(this, logLine, APLogType.APLOG_EMV_INIT);

         if (logLine.Contains("[EMVProcessing") && logLine.Contains("InitializeChip() start"))
            return new APLine(this, logLine, APLogType.APLOG_EMV_INITCHIP);

         if (logLine.Contains("[EMVProcessing") && logLine.Contains("Device.CardReader.EMV_Sel_BuildCandidateApp()' Result ="))
            return new APLineField(this, logLine, APLogType.APLOG_EMV_BUILD_CANDIDATE_LIST);

         if (logLine.Contains("[BeginICCAppSelectionLocalFlowPoint") && logLine.Contains("AppNameList  ="))
            return new APLineField(this, logLine, APLogType.APLOG_EMV_CREATE_APPNAME_LIST);

         if (logLine.Contains("[EMVProcessing") && logLine.Contains("SelectedAID:"))
            return new APLineField(this, logLine, APLogType.APLOG_EMV_APP_SELECTED);

         if (logLine.Contains("[NCompleteICCAppSelectState") && logLine.Contains("Device.CardReader.PANData"))
            return new APLineField(this, logLine, APLogType.APLOG_EMV_PAN);

         if (logLine.Contains("[NSetICCTranDataState") && logLine.Contains("strCurrencyType :"))
            return new APLineEmvCurrencyType(this, logLine, APLogType.APLOG_EMV_CURRENCY_TYPE);

         if (logLine.Contains("[CardReader") && logLine.Contains("Return EMV_OffDataAuth() :"))
            return new APLineField(this, logLine, APLogType.APLOG_EMV_OFFLINE_AUTH);

         if (logLine.Contains("[CommunicationFramework") && logLine.Contains("EXT=FAULT_SMART_CARDREADER"))
            return new APLineField(this, logLine, APLogType.APLOG_EMV_FAULT_SMART_CARDREADER);

         /* ManagementJournal */

         if (logLine.Contains("[ManagementJournal") && logLine.Contains("INSERVICE ENTERED"))
            return new APLineField(this, logLine, APLogType.APLOG_INSERVICE_ENTERED);

         if (logLine.Contains("[ManagementJournal") && logLine.Contains("TRANSACTION TIMEOUT"))
            return new APLineField(this, logLine, APLogType.APLOG_TRANSACTION_TIMEOUT);

         /* APLOG_FLW_SWITCH_FIT */
         if (logLine.Contains("[FITSwitchState") && logLine.Contains("ExecuteState") && logLine.Contains("Next State is to be "))
            return new APLineField(this, logLine, APLogType.APLOG_FLW_SWITCH_FIT);


         /* [Pinpad              */
         if (logLine.Contains("[Pinpad") && logLine.Contains("[Open"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_OPEN);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[Close"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_CLOSE);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[CheckTheEppIsPci"))
            return new APLineField(this, logLine, APLogType.APLOG_PIN_ISPCI);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[CheckTheEppSupportTR31"))
            return new APLineField(this, logLine, APLogType.APLOG_PIN_ISTR31);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[CheckTheEppSupportTR34"))
            return new APLineField(this, logLine, APLogType.APLOG_PIN_ISTR34);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[OnKeyImported"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_KEYIMPORTED);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[OnRandomNumberGenerated"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_RAND);

         if (logLine.Contains("[Pinpad") && logLine.Contains("OnPinBlockComplete"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_PINBLOCK);

         if (logLine.Contains("[PinEntryState") && logLine.Contains("BuildPINBlock failed"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_PINBLOCK_FAILED);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[OnTimeout"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_TIMEOUT);

         if (logLine.Contains("[Pinpad") && logLine.Contains("[OnReadPinComplete"))
            return new APLine(this, logLine, APLogType.APLOG_PIN_READCOMPLETE);



         if (logLine.Contains("[LocalScreenWindowEx") && logLine.Contains("[DisplayLoadCompleted"))
            return new APLineField(this, logLine, APLogType.APLOG_DISPLAYLOAD);

         if (logLine.Contains("[ScreenWindow") && logLine.Contains("LogAdditionalInformation"))
            return new APLineField(this, logLine, APLogType.APLOG_SCREENWINDOW);


         if (logLine.Contains("[LocalXmlHelper") && logLine.Contains("About to execute: Class: HelperFunctions, Method:"))
            return new APLineField(this, logLine, APLogType.APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE);


         if (logLine.Contains("[HybridFlowEngine") && logLine.Contains("State created:"))
            return new APLineField(this, logLine, APLogType.APLOG_STATE_CREATED);



         /* OLD [ScreenDecoratorLocal][OnFunctionKeySelected][NORMAL]Raising FunctionKeySelected event with values FunctionKey[No], PinInputData[], ResultData[]. */
         if (logLine.Contains("[OnFunctionKeySelected") && logLine.Contains("Raising FunctionKeySelected event with values FunctionKey"))
            return new APLineField(this, logLine, APLogType.APLOG_FUNCTIONKEY_SELECTED);

         /* NEW [ScreenDecoratorLocal.OnFunctionKeySelected] The No button was pressed.*/
         if (logLine.Contains("[ScreenDecoratorLocal.OnFunctionKeySelected]"))
            return new APLineField(this, logLine, APLogType.APLOG_FUNCTIONKEY_SELECTED2);


         if (logLine.Contains("[GetDeviceFitness") && logLine.Contains("Parameter pDvcStatus:"))
            return new APLineField(this, logLine, APLogType.APLOG_DEVICE_FITNESS);


         if (logLine.Contains("?????????????????????????????????????????????"))
            return new APLine(this, logLine, APLogType.APLOG_EXCEPTION);


         /* AddKey */
         if (logLine.Contains("[AbstractConfigHandler") && logLine.Contains("AddMoniplusData") && logLine.Contains("Add Key="))
            return new AddKey(this, logLine);

         /* CASHDISP */
         if (logLine.Contains("[CashDispenser"))
         {
            ILogLine iLine = CashDispenser.Factory(this, logLine);
            if (iLine != null) return iLine;
         }



         /* CORE */
         if (logLine.Contains("WebServiceRequestFlowPoint"))
         {
            ILogLine iLine = Core.Factory(this, logLine);
            if (iLine != null) return iLine;
         }

         /* EJ */
         if (logLine.Contains("INSERT INTO "))
            return new EJInsert(this, logLine);


         /* HELPER FUNCTIONS */
         if (logLine.Contains("[HelperFunctions") && logLine.Contains("[GetConfiguredBillMixList") && logLine.Contains("ConfiguredBillMixList:"))
            return new APLineField(this, logLine, APLogType.HelperFunctions_GetConfiguredBillMixList);

         if (logLine.Contains("[HelperFunctions") && logLine.Contains("[GetFewestBillMixList") && logLine.Contains("FewestBillMixList:"))
            return new APLineField(this, logLine, APLogType.HelperFunctions_GetFewestBillMixList);

         /* NDC */
         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST:"))
            return Atm2Host.Factory(this, logLine);

         if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM:"))
            return Host2Atm.Factory(this, logLine);


         /* CASH DISPENSER */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("OnDenominateComplete"))
            return new APLine(this, logLine, APLogType.CashDispenser_OnDenominateComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("OnPresentComplete"))
            return new APLine(this, logLine, APLogType.CashDispenser_OnPresentComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("OnItemsTaken"))
            return new APLine(this, logLine, APLogType.CashDispenser_OnItemsTaken);



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


         return new APLine(this, logLine, APLogType.None);
      }
   }
}
