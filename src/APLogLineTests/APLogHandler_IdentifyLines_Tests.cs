using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using APSamples;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_IdentifyLines_Tests
   {
      [TestMethod]
      public void AddKey_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.AddKey_1);
         Assert.IsTrue(logLine is AddKey);

         AddKey apLine = (AddKey)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_ADDKEY);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:49:31.901");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.tableName == "TimerTable");
         Assert.IsTrue(apLine.keyName == "00");
         Assert.IsTrue(apLine.value == "015");
      }

      [TestMethod]
      public void AddKey_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.AddKey_2);
         Assert.IsTrue(logLine is AddKey);

         AddKey apLine = (AddKey)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_ADDKEY);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:49:31.907");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.tableName == "OptionTable");
         Assert.IsTrue(apLine.keyName == "Option00");
         Assert.IsTrue(apLine.value == "002");
      }

      [TestMethod]
      public void AddKey_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.AddKey_3);
         Assert.IsTrue(logLine is AddKey);

         AddKey apLine = (AddKey)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_ADDKEY);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:21.435");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.tableName == "ScreenTable");
         Assert.IsTrue(apLine.keyName == "143");
         Assert.IsTrue(apLine.value == "TRANSACTION COMPLETED");
      }

      [TestMethod]
      public void AddKey_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.AddKey_4);
         Assert.IsTrue(logLine is AddKey);

         AddKey apLine = (AddKey)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_ADDKEY);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 04:01:39.960");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.tableName == "TcpIp");
         Assert.IsTrue(apLine.keyName == "RemoteIP");
         Assert.IsTrue(apLine.value == "127.0.0.1");
      }

      [TestMethod]
      public void AddKey_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.AddKey_5);
         Assert.IsTrue(logLine is AddKey);

         AddKey apLine = (AddKey)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_ADDKEY);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 04:01:39.971");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.tableName == "DialUp");
         Assert.IsTrue(apLine.keyName == "EnablePredial");
         Assert.IsTrue(apLine.value == "False");
      }

      [TestMethod]
      public void AddKey_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.AddKey_6);
         Assert.IsTrue(logLine is AddKey);

         AddKey apLine = (AddKey)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_ADDKEY);
         Assert.IsTrue(apLine.Timestamp == "2023-12-13 03:01:11.589");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.tableName == "Remoting");
         Assert.IsTrue(apLine.keyName == "RemoteAddress");
         Assert.IsTrue(apLine.value == "");
      }

      [TestMethod]
      public void EJ_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.EJ_1);
         Assert.IsTrue(logLine is EJInsert);

         EJInsert apLine = (EJInsert)logLine;
         Assert.IsTrue(apLine.apType == APLogType.EJInsert);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 12:18:39.809");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.ejTable == "Session");
         Assert.IsTrue(apLine.ejColumns.Length == 6);
         Assert.IsTrue(apLine.ejColumns[0] == "ATMId");
         Assert.IsTrue(apLine.ejColumns[1] == "StartDate");
         Assert.IsTrue(apLine.ejColumns[2] == "IdentificationType");
         Assert.IsTrue(apLine.ejColumns[3] == "IdentificationNumberMasked");
         Assert.IsTrue(apLine.ejColumns[4] == "AuthenticationType");
         Assert.IsTrue(apLine.ejColumns[5] == "AuthenticationNumberMasked");

         Assert.IsTrue(apLine.ejValues.Length == 6);
         Assert.IsTrue(apLine.ejValues[0] == "XB5367");
         Assert.IsTrue(apLine.ejValues[1] == "06/07/2023 12:18:39 PM");
         Assert.IsTrue(apLine.ejValues[2] == "AccountNumber");
         Assert.IsTrue(apLine.ejValues[3] == "9706");
         Assert.IsTrue(apLine.ejValues[4] == "SSN");
         Assert.IsTrue(apLine.ejValues[5] == "4052");

      }

      [TestMethod]
      public void EJ_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.EJ_2);
         Assert.IsTrue(logLine is EJInsert);

         EJInsert apLine = (EJInsert)logLine;
         Assert.IsTrue(apLine.apType == APLogType.EJInsert);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 13:01:59.823");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.ejTable == "Transaction");
         Assert.IsTrue(apLine.ejColumns.Length == 17);
         Assert.IsTrue(apLine.ejColumns[0] == "ATMId");
         Assert.IsTrue(apLine.ejColumns[1] == "IdRelatedTx");
         Assert.IsTrue(apLine.ejColumns[2] == "SessionId");
         Assert.IsTrue(apLine.ejColumns[3] == "ATMDateTime");
         Assert.IsTrue(apLine.ejColumns[4] == "TransactionDateTime");
         Assert.IsTrue(apLine.ejColumns[5] == "TransactionType");
         Assert.IsTrue(apLine.ejColumns[6] == "SequenceNumber");
         Assert.IsTrue(apLine.ejColumns[7] == "AccountNumberMasked");
         Assert.IsTrue(apLine.ejColumns[8] == "AccountType");
         Assert.IsTrue(apLine.ejColumns[9] == "AmountRequested");
         Assert.IsTrue(apLine.ejColumns[10] == "AmountDispensed");
         Assert.IsTrue(apLine.ejColumns[11] == "AmountDeposited");
         Assert.IsTrue(apLine.ejColumns[12] == "HostType");
         Assert.IsTrue(apLine.ejColumns[13] == "TotalCashAmount");
         Assert.IsTrue(apLine.ejColumns[14] == "TotalCheckAmount");
         Assert.IsTrue(apLine.ejColumns[15] == "TotalChecksDeposited");
         Assert.IsTrue(apLine.ejColumns[16] == "Success");

         Assert.IsTrue(apLine.ejValues.Length == 17);
         Assert.IsTrue(apLine.ejValues[0] == "XB5367");
         Assert.IsTrue(apLine.ejValues[1] == "0");
         Assert.IsTrue(apLine.ejValues[2] == "106");
         Assert.IsTrue(apLine.ejValues[3] == "6/7/2023 1:01:59 PM");
         Assert.IsTrue(apLine.ejValues[4] == "6/7/2023 1:01:59 PM");
         Assert.IsTrue(apLine.ejValues[5] == "FastCash");
         Assert.IsTrue(apLine.ejValues[6] == "11");
         Assert.IsTrue(apLine.ejValues[7] == "6562");
         Assert.IsTrue(apLine.ejValues[8] == "Share");
         Assert.IsTrue(apLine.ejValues[9] == "50");
         Assert.IsTrue(apLine.ejValues[10] == "50");
         Assert.IsTrue(apLine.ejValues[11] == "0");
         Assert.IsTrue(apLine.ejValues[12] == "Core");
         Assert.IsTrue(apLine.ejValues[13] == "50");
         Assert.IsTrue(apLine.ejValues[14] == "0");
         Assert.IsTrue(apLine.ejValues[15] == "0");
         Assert.IsTrue(apLine.ejValues[16] == "True");
      }


      [TestMethod]
      public void EJ_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.EJ_3);
         Assert.IsTrue(logLine is EJInsert);

         EJInsert apLine = (EJInsert)logLine;
         Assert.IsTrue(apLine.apType == APLogType.EJInsert);
         Assert.IsTrue(apLine.Timestamp == "2023-12-15 11:43:49.404");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.ejTable == "Session");
         Assert.IsTrue(apLine.ejColumns.Length == 4);
         Assert.IsTrue(apLine.ejColumns[0] == "StartDate");
         Assert.IsTrue(apLine.ejColumns[1] == "IdentificationType");
         Assert.IsTrue(apLine.ejColumns[2] == "IdentificationNumberMasked");
         Assert.IsTrue(apLine.ejColumns[3] == "AuthenticationType");

         Assert.IsTrue(apLine.ejValues.Length == 4);
         Assert.IsTrue(apLine.ejValues[0] == "12/15/2023 11:43:49 AM");
         Assert.IsTrue(apLine.ejValues[1] == "BankCard");
         Assert.IsTrue(apLine.ejValues[2] == "5868");
         Assert.IsTrue(apLine.ejValues[3] == "PIN");
      }

      [TestMethod]
      public void APLOG_EMV_INIT()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_EMV_INIT);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_EMV_INIT);
         Assert.IsTrue(apLine.Timestamp == "2024-06-27 08:37:41.763");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_EMV_INITCHIP()
      {

      }

      [TestMethod]
      public void APLOG_EMV_BUILD_CANDIDATE_LIST()
      {

      }

      [TestMethod]
      public void APLOG_EMV_CREATE_APPNAME_LIST()
      {

      }

      [TestMethod]
      public void APLOG_EMV_APP_SELECTED()
      {

      }

      [TestMethod]
      public void APLOG_EMV_PAN()
      {

      }

      [TestMethod]
      public void APLOG_EMV_CURRENCY_TYPE()
      {

      }

      [TestMethod]
      public void APLOG_EMV_IAPLOG_EMV_OFFLINE_AUTHNIT()
      {

      }

      [TestMethod]
      public void APLOG_INSERVICE_ENTERED()
      {

      }

      [TestMethod]
      public void APLOG_TRANSACTION_TIMEOUT()
      {

      }
   }
}
