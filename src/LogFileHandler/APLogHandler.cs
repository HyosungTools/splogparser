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
      public APLogHandler(ICreateStreamReader createReader) : base(ParseType.AP, createReader)
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
            char c = logFile[traceFilePos];
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
                  catch (Exception e)
                  {
                     Console.WriteLine(String.Format("Exception {0}", e.Message));
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
         // (bool success, string apLogLine) result;

         /* APLOG_INSTALL */
         if (logLine.StartsWith("=======") && logLine.EndsWith("========\r\n"))
            return new APLine(this, logLine, APLogType.APLOG_INSTALL);

         ///* APLOG_SETTINGS */
         //if (logLine.Contains("[ConfigurationFramework") && logLine.Contains("ProcessXMLFiles") && logLine.Contains("Adding xml file:"))
         //   return new APLine(this, logLine, APLogType.APLOG_SETTINGS_CONFIG);

         ///* APLOG_FLW_CURRENTMODE */
         //if (logLine.Contains("Current Mode:"))
         //   return new APLine(this, logLine, APLogType.APLOG_FLW_CURRENTMODE);

         ///* DEV_UNSOL_EVENT */
         //if (logLine.Contains("[RaiseDeviceUnSolEvent") && logLine.Contains("FireDeviceUnsolEvent"))
         //   return new DevUnSolEvent(this, logLine);

         ///* APLOG_FLW_CARD_PAN */
         //if (logLine.Contains("[NCompleteICCAppSelectState") && logLine.Contains("[SetTrackData") && logLine.Contains("Device.CardReader.PANData"))
         //   return new PANData(this, logLine);

         ///* APLOG_FLW_SWITCH_FIT */
         //if (logLine.Contains("[FITSwitchState") && logLine.Contains("ExecuteState") && logLine.Contains("Next State is to be "))
         //   return new APLine(this, logLine, APLogType.APLOG_FLW_SWITCH_FIT);

         ///* COMM_FRMWORK */
         //if (logLine.Contains("[CommunicationFramework") && logLine.Contains("OnConnectedHost") && logLine.Contains("Host Connected") ||
         //    logLine.Contains("[CommunicationFramework") && logLine.Contains("OnDisconnectedHost") && logLine.Contains("Host disconnected"))
         //   return new CommFrmWork(this, logLine);

         ///* PINPAD */
         //if (logLine.Contains("[Pinpad") && logLine.Contains("OnReadPinComplete"))
         //   return new Pinpad(this, logLine);

         ///* PINPAD */
         //if (logLine.Contains("[Pinpad") && logLine.Contains("OnPinBlockComplete"))
         //   return new Pinpad(this, logLine);

         ///* APLOG_FLW_PINBLOCK_FAILED */
         //if (logLine.Contains("[PinEntryState") && logLine.Contains("[ProcessBuildPINBlock") && logLine.Contains("BuildPINBlock failed"))
         //   return new APLine(this, logLine, APLogType.APLOG_FLW_PINBLOCK_FAILED);

         /////* APLOG_FLW_SCREEN */
         ////if (logLine.Contains("[ShowScreenCore") && logLine.Contains("ShowScreenCore pScreenNumber"))
         ////   return new APLine(this, logLine, APLogType.APLOG_FLW_SCREEN);

         ///* APLOG_FLW_SCREEN_NAME */
         //if (logLine.Contains("[LocalScreenWindowEx") && logLine.Contains("[ProcessPageRequest"))
         //   return new ScreenName(this, logLine);

         ///* APLOG_FLW_STATE */
         //if (logLine.Contains("[HybridFlowEngine") && logLine.Contains("CreateNextState") && logLine.Contains("State created: "))
         //   return new CreateNextState(this, logLine);

         ///* APLOG_FLW_FUNCTIONKEY */
         ///* OLD [ScreenDecoratorLocal][OnFunctionKeySelected][NORMAL]Raising FunctionKeySelected event with values FunctionKey[No], PinInputData[], ResultData[]. */
         //if (logLine.Contains("[ScreenDecoratorLocal") && logLine.Contains("[OnFunctionKeySelected") && logLine.Contains("Raising FunctionKeySelected event with values FunctionKey"))
         //   return new FnKey(this, logLine);

         ///* APLOG_FLW_FUNCTIONKEY */
         ///* NEW [ScreenDecoratorLocal.OnFunctionKeySelected] The No button was pressed.*/
         //if (logLine.Contains("[ScreenDecoratorLocal.OnFunctionKeySelected]"))
         //   return new APLine(this, logLine, APLogType.APLOG_FLW_FUNCTIONKEY2);

         ///* APLOG_FLW_DEVICE_FITNESS */
         //if (logLine.Contains("[GetDeviceFitness") && logLine.Contains("Parameter pDvcStatus:"))
         //   return new APLine(this, logLine, APLogType.APLOG_FLW_DEVICE_FITNESS);


         ///* APLOG_WD */

         /////* APLOG_WD_WITHDRAW */
         ////if (logLine.Contains("Raising FunctionKeySelected event with values FunctionKey[Withdrawal]"))
         ////   return new APLine(this, logLine, APLogType.APLOG_WD_WITHDRAW);

         ///* HLPR_BILLMIX */
         //if (logLine.Contains("[HelperFunctions") && logLine.Contains("SetBufferWithBillMix") && logLine.Contains("Using BillMixList:"))
         //   return new HlpBillMix(this, logLine);

         /////* APLOG_WD_EMVAMOUNT */
         ////if (logLine.Contains("ExecuteState") && logLine.Contains("EMVState.Amount"))
         ////   return new APLine(this, logLine, APLogType.APLOG_WD_EMVAMOUNT);


         /* AddKey */
         if (logLine.Contains("[AbstractConfigHandler") && logLine.Contains("AddMoniplusData") && logLine.Contains("Add Key="))
            return new AddKey(this, logLine);

         /* CASHDISP */
         if (logLine.Contains("[CashDispenser"))
         {
            ILogLine iLine = CashDispenser.Factory(this, logLine);
            if (iLine != null) return iLine;
         }

         /* EJ */
         if (logLine.Contains("INSERT INTO "))
            return new EJInsert(this, logLine);

         //if (logLine.Contains("UpdateTypeInfoToDispense") && logLine.Contains("Dispensing amount in total is "))
         //   return new APLine(this, logLine, APLogType.APLOG_WD_DISPENSE);

         ///* APLOG_WD_PRESENTED */
         //if (logLine.Contains("CashDispenser_Prensented") && logLine.Contains("Present compete event, EventType=CashPresented"))
         //   return new APLine(this, logLine, APLogType.APLOG_WD_PRESENTED);

         ///* APLOG_WD_ITEMSTAKEN */
         //if (logLine.Contains("OnItemsTaken") && logLine.Contains("m_NxCashDispenser.OnItemsTaken event received"))
         //   return new APLine(this, logLine, APLogType.APLOG_WD_ITEMSTAKEN);

         //if (logLine.Contains("SetupCSTListInHostTypeInfo"))
         //   return new APLine(this, logLine, APLogType.APLOG_WD_SETUPCSTLISTINHOST);

         //if (logLine.Contains("SetupNoteTypeInfo") && logLine.Contains("Set NoteType"))
         //   return new APLine(this, logLine, APLogType.APLOG_WD_SETUPNOTETYPEINFO);



         ///* ADLOG_NDC */
         //if (logLine.Contains("Send") && logLine.Contains("ATM2HOST:"))
         //   return new APLine(this, logLine, APLogType.APLOG_NDC_ATM2HOST);
         ///*
         //         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST: 12"))
         //            return (APLogType.APLOG_NDC_ATM2HOST12, logLine);

         //         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST: 22"))
         //            return (APLogType.APLOG_NDC_ATM2HOST22, logLine);

         //         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST: 23"))
         //            return (APLogType.APLOG_NDC_ATM2HOST23, logLine);

         //         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST: 51"))
         //            return (APLogType.APLOG_NDC_ATM2HOST51, logLine);

         //         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST: 61"))
         //            return (APLogType.APLOG_NDC_ATM2HOST51, logLine);

         //         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST:"))
         //            return (APLogType.APLOG_NDC_ATM2HOST, logLine);
         //*/
         //if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM:"))
         //   return new APLine(this, logLine, APLogType.APLOG_NDC_HOST2ATM);
         ///*
         //         if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM: 3"))
         //            return (APLogType.APLOG_NDC_HOST2ATM3, logLine);

         //         if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM: 4"))
         //            return (APLogType.APLOG_NDC_HOST2ATM4, logLine);

         //         if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM: 7"))
         //            return (APLogType.APLOG_NDC_HOST2ATM7, logLine);

         //         if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM:"))
         //            return (APLogType.APLOG_NDC_HOST2ATM, logLine);
         //*/

         ///* APLOG_MEMORY */
         //if (logLine.Contains("MemoryWatchDog") && logLine.Contains("UsedProcessMemory  :"))
         //   return new APLine(this, logLine, APLogType.APLOG_MEMORY);

         ///* OPERATOR */
         //if (logLine.Contains("[NavigateOPMenu") && logLine.Contains("Parameter pMenuName:"))
         //   return new APLine(this, logLine, APLogType.APLOG_OPER_MENUNAME);

         //if (logLine.Contains("m_NxDoors.OnDoorChanged event received"))
         //   return new APLine(this, logLine, APLogType.APLOG_OPER_DOOR);

         return new APLine(this, logLine, APLogType.None);
      }
   }
}
