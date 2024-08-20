using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum TCRLogType
   {
      /* Not an APLog line we are interested in */
      None,

      TCR_INSTALL,

      TCR_ATM2HOST,
      TCR_HOST2ATM,
      TCR_HOST_RECEIVED_DATA,
      TCR_ATM_SENDMESSAGE_SYNC,

      TCR_ON_UPDATE_SCREENDATA,

      TCR_CHANGING_MODE,
      TCR_CHANGEMODE_FAILED,
      TCR_CURRENTMODE,

      /* TCR STATE */

      TCR_NEXTSTATE,

      /* TCR TRANSACTION */

      /* DEPOSIT */

      TCR_DEP_TELLERID,
      TCR_DEP_RESULT,
      TCR_DEP_ERRORCODE,
      TCR_DEP_CASHDEPOSITED,
      TCR_DEP_AMOUNT,
      TCR_DEP_BALANCE,

      /* WITHDRAWAL */

      TCR_WD_TELLERID,
      TCR_WD_RESULT,
      TCR_WD_ERRORCODE,
      TCR_WD_CASHDISPENSED,
      TCR_WD_AMOUNT,
      TCR_WD_BALANCE,

      TCR_HOST_CMD,
      TCR_HOST_CMD_RESPONSE,





      /* ERROR */
      Error
   }

   public class TCRLogLine : LogLine, ILogLine
   {
      public TCRLogType tcrType { get; set; }

      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      public TCRLogLine(ILogFileHandler parent, string logLine, TCRLogType tcrType) : base(parent, logLine)
      {
         this.tcrType = tcrType;
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
         if (logLine.Contains("[TCRInServiceMode") && logLine.Contains("ProcessHostReceivedData:"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_HOST_CMD);

         /* TCR_INSTALL */
         if (logLine.StartsWith("=======") && logLine.EndsWith("========\r\n"))
            return new TCRMachineInfo(logFileHandler, logLine);

         /* ATM2HOST HOST2ATM */
         if (logLine.Contains("Send") && logLine.Contains("ATM2HOST:"))
            return new TCRLogLine(logFileHandler, logLine, TCRLogType.TCR_ATM2HOST);

         if (logLine.Contains("[WriteParameters") && logLine.Contains("SendMessageSync:"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_ATM_SENDMESSAGE_SYNC);


         if (logLine.Contains("[ProcessHostReceivedData") && logLine.Contains("ProcessHostReceivedData:"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_HOST_RECEIVED_DATA);

         if (logLine.Contains("RecvProcAsync") && logLine.Contains("HOST2ATM:"))
            return new TCRLogLine(logFileHandler, logLine, TCRLogType.TCR_HOST2ATM);



         if (logLine.Contains("[TCRLocalScreenWindow") && logLine.Contains("OnUpdateScreenData"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_ON_UPDATE_SCREENDATA);

         /* CHANGE MODE */

         if (logLine.Contains("[ATMTaskManager") && logLine.Contains("---------------Trying to change mode to"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_CHANGING_MODE);

         if (logLine.Contains("[ATMTaskManager") && logLine.Contains("---------------ERROR: SwitchATMMode()"))
            return new TCRLogLine(logFileHandler, logLine, TCRLogType.TCR_CHANGEMODE_FAILED);

         if (logLine.Contains("[ModeFramework") && logLine.Contains("OnModeChangeEvent"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_CURRENTMODE);


         if (logLine.Contains("[LocalFlowEngine") && logLine.Contains("State created:"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_NEXTSTATE);


         /* DEPOSIT */

         if (logLine.Contains("[Deposit") && logLine.Contains("TellerID="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_DEP_TELLERID);

         if (logLine.Contains("[Deposit") && logLine.Contains("Result="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_DEP_RESULT);

         if (logLine.Contains("[Deposit") && logLine.Contains("ErrorCode="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_DEP_ERRORCODE);

         if (logLine.Contains("[Deposit") && logLine.Contains("CashDeposited="))
            return new TCRLogLineCash(logFileHandler, logLine, TCRLogType.TCR_DEP_CASHDEPOSITED);

         if (logLine.Contains("[Deposit") && logLine.Contains("Amount="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_DEP_AMOUNT);

         if (logLine.Contains("[Deposit") && logLine.Contains("Balance="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_DEP_BALANCE);

         /* WD */

         if (logLine.Contains("[Dispense") && logLine.Contains("TellerID="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_WD_TELLERID);

         if (logLine.Contains("[Dispense") && logLine.Contains("Result="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_WD_RESULT);

         if (logLine.Contains("[Dispense") && logLine.Contains("ErrorCode="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_WD_ERRORCODE);

         if (logLine.Contains("[Dispense") && logLine.Contains("CashDispensed="))
            return new TCRLogLineCash(logFileHandler, logLine, TCRLogType.TCR_WD_CASHDISPENSED);

         if (logLine.Contains("[Dispense") && logLine.Contains("Amount="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_WD_AMOUNT);

         if (logLine.Contains("[Dispense") && logLine.Contains("Balance="))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_WD_BALANCE);





         if (logLine.Contains("[ModeHelperFunction") && logLine.Contains("Send the result of the InquiryCommand:"))
            return new TCRLogLineWithField(logFileHandler, logLine, TCRLogType.TCR_HOST_CMD_RESPONSE);

         return null;
      }


   }
}
