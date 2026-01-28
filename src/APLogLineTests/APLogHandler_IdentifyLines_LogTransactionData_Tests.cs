using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using APSamples;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_IdentifyLines_LogTransactionData_Tests
   {
      /* ========================================== */
      /* LogTransactionData - Pipe Delimited Format */
      /* (WebService cores - FlowPoint populated)   */
      /* ========================================== */

      [TestMethod]
      public void LogTransactionData_PipeDelimited_CMCFlex_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_PipeDelimited_CMCFlex_1);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-20 21:39:17.677");
         Assert.IsTrue(apLine.HResult == "");
         Assert.IsTrue(apLine.TID == "35");

         Assert.IsTrue(apLine.FlowPoint == "CMCFlex");
         Assert.IsNull(apLine.FP); // No FP in pipe-delimited format
         Assert.IsTrue(apLine.OnUs == "on-us");
         Assert.IsTrue(apLine.Message == "PinAuthentication");
         Assert.IsTrue(apLine.TransactionType == "CustomerIdentification");
         Assert.IsTrue(apLine.CoreAvailable == "true");
         Assert.IsTrue(apLine.NDCAvailable == "true");
         Assert.IsTrue(apLine.Track2 == ";************5774=****************0000?");
         Assert.IsTrue(apLine.CustomerId == "************5774");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");

         // Not present in this line
         Assert.IsNull(apLine.AccountNumber);
         Assert.IsNull(apLine.AccountType);
         Assert.IsNull(apLine.Amount);
      }

      [TestMethod]
      public void LogTransactionData_PipeDelimited_CMCFlex_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_PipeDelimited_CMCFlex_2);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-20 21:39:48.843");
         Assert.IsTrue(apLine.TID == "35");

         Assert.IsTrue(apLine.FlowPoint == "CMCFlex");
         Assert.IsNull(apLine.FP);
         Assert.IsTrue(apLine.OnUs == "on-us");
         Assert.IsTrue(apLine.Message == "Withdrawal");
         Assert.IsTrue(apLine.TransactionType == "Withdrawal");
         Assert.IsTrue(apLine.AccountNumber == "*****9856");
         Assert.IsTrue(apLine.AccountType == "SD");
         Assert.IsTrue(apLine.Amount == "10.00");
         Assert.IsTrue(apLine.CoreAvailable == "true");
         Assert.IsTrue(apLine.NDCAvailable == "true");
         Assert.IsTrue(apLine.Track2 == ";************5774=****************0000?");
         Assert.IsTrue(apLine.CustomerId == "************5774");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_PipeDelimited_SymXchange_NoTID()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_PipeDelimited_SymXchange_1);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2025-06-24 17:43:42.370");

         // No TID in this line
         Assert.IsNull(apLine.TID);

         Assert.IsTrue(apLine.FlowPoint == "SymXchange");
         Assert.IsNull(apLine.FP);
         Assert.IsTrue(apLine.OnUs == "on-us");
         Assert.IsTrue(apLine.Message == "BalanceInquiry");
         Assert.IsTrue(apLine.TransactionType == "BalanceInquiry");
         Assert.IsTrue(apLine.CoreAvailable == "true");
         Assert.IsTrue(apLine.NDCAvailable == "true");
         Assert.IsTrue(apLine.CustomerId == "XXXXXX2603");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      /* ========================================== */
      /* LogTransactionData - JSON Format          */
      /* (Non-WebService - FlowPoint is empty)     */
      /* ========================================== */

      [TestMethod]
      public void LogTransactionData_Json_StateWrapper_Idle()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_StateWrapper_Idle);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 02:59:58.034");
         Assert.IsTrue(apLine.TID == "28");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "Idle"); // Common- stripped
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_Standard_PreBeginICCInit()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_Standard_PreBeginICCInit);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:13:09.278");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "PreBeginICCInitForDip"); // Common- stripped
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_StateWrapper_CompleteICCAppSel()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_StateWrapper_CompleteICCAppSel);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:13:13.516");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "CompleteICCAppSel"); // Common- stripped
         Assert.IsTrue(apLine.Track2 == ";************5548=****************0000?");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_Standard_Placeholder()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_Standard_Placeholder);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:13:13.548");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "PostInsertCardFITMatch"); // PLACEHOLDER- stripped
         Assert.IsTrue(apLine.OnUs == "foreign");
         Assert.IsTrue(apLine.Track2 == ";************5548=****************0000?");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_Standard_DetermineAuth()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_Standard_DetermineAuth);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:13:13.604");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "DetermineAuthentication"); // Common- stripped
         Assert.IsTrue(apLine.OnUs == "foreign");
         Assert.IsTrue(apLine.CoreAvailable == "false");
         Assert.IsTrue(apLine.NDCAvailable == "false");
         Assert.IsTrue(apLine.CustomerId == "************5548");
         Assert.IsTrue(apLine.Track2 == ";************5548=****************0000?");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_StandardKeyEntry_AccountSelection()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_StandardKeyEntry_AccountSelection);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:13:32.224");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "Withdrawal-AccountSelection"); // No prefix to strip
         Assert.IsTrue(apLine.OnUs == "foreign");
         Assert.IsTrue(apLine.CoreAvailable == "false");
         Assert.IsTrue(apLine.NDCAvailable == "true");
         Assert.IsTrue(apLine.CustomerId == "************5548");
         Assert.IsTrue(apLine.TransactionType == "Withdrawal");
         Assert.IsTrue(apLine.AccountType == "Checking");
         Assert.IsTrue(apLine.Track2 == ";************5548=****************0000?");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_StandardKeyEntry_EnterAmount()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_StandardKeyEntry_EnterAmount);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:13:56.535");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "Withdrawal-EnterAmount");
         Assert.IsTrue(apLine.OnUs == "foreign");
         Assert.IsTrue(apLine.CoreAvailable == "false");
         Assert.IsTrue(apLine.NDCAvailable == "true");
         Assert.IsTrue(apLine.CustomerId == "************5548");
         Assert.IsTrue(apLine.TransactionType == "Withdrawal");
         Assert.IsTrue(apLine.AccountType == "Checking");
         Assert.IsTrue(apLine.Amount == "500.00");
         Assert.IsTrue(apLine.Track2 == ";************5548=****************0000?");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_BeginICCAppSelectionLocal()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_BeginICCAppSelectionLocal);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:13:11.094");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "BeginICCAppSel"); // Common- stripped
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }

      [TestMethod]
      public void LogTransactionData_Json_ValidateTransactionData()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_logtransactiondata.LogTransactionData_Json_ValidateTransactionData);
         Assert.IsTrue(logLine is LogTransactionData);

         LogTransactionData apLine = (LogTransactionData)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOGTRANSACTIONDATA);
         Assert.IsTrue(apLine.Timestamp == "2026-01-02 09:14:02.248");
         Assert.IsTrue(apLine.TID == "33");

         Assert.IsTrue(apLine.FlowPoint == ""); // Not a WebService core
         Assert.IsTrue(apLine.FP == "PreTransactionRequest"); // Common- stripped
         Assert.IsTrue(apLine.OnUs == "foreign");
         Assert.IsTrue(apLine.CoreAvailable == "false");
         Assert.IsTrue(apLine.NDCAvailable == "true");
         Assert.IsTrue(apLine.CustomerId == "************5548");
         Assert.IsTrue(apLine.TransactionType == "Withdrawal");
         Assert.IsTrue(apLine.AccountType == "Checking");
         Assert.IsTrue(apLine.Amount == "500.00");
         Assert.IsTrue(apLine.Track2 == ";************5548=****************0000?");
         Assert.IsTrue(apLine.IsContactless == "false");
         Assert.IsTrue(apLine.Language == "English");
      }
   }
}
