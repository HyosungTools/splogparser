
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
      APLOG_CARD_ONMEDIAINSERTED,
      APLOG_CARD_ONREADCOMPLETE,
      APLOG_CARD_ONEJECTCOMPLETE,
      APLOG_CARD_ONMEDIAREMOVED,
      APLOG_CARD_PAN,

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

      /* ERROR */
      Error
   }

   public class APLine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }
      public string HResult { get; set; }
      public APLogType apType { get; set; }

      public APLine(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine)
      {
         this.apType = apType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         HResult = hResult();
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         // set timeStamp to a default time
         string timestamp = @"2023-01-01 00:00:00.000";

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
   }
}
