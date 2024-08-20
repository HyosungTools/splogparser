using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using TCRSamples;

namespace TCRLogLineTests
{
   [TestClass]
   public class TCR_IdentifyLines_General
   {
      [TestMethod]
      public void TCR_INSTALL()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_INSTALL);
         Assert.IsTrue(logLine is TCRMachineInfo);

         TCRMachineInfo tcrLine = (TCRMachineInfo)logLine;
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_INSTALL);
         Assert.IsTrue(tcrLine.installedPackages.Length == 15);
         Assert.IsTrue(tcrLine.installedPrograms.Length == 90);
      }

      [TestMethod]
      public void TCR_ATM2HOST()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_ATM2HOST);
         Assert.IsTrue(logLine is TCRLogLine);

         TCRLogLine tcrLine = (TCRLogLine)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-01 16:11:32.447");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_ATM2HOST);
      }

      [TestMethod]
      public void TCR_HOST2ATM()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_HOST2ATM);
         Assert.IsTrue(logLine is TCRLogLine);

         TCRLogLine tcrLine = (TCRLogLine)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-01 16:11:32.293");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_HOST2ATM);
      }

      [TestMethod]
      public void TCR_ON_UPDATE_SCREENDATA()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_ON_UPDATE_SCREENDATA);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-01 16:11:47.371");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_ON_UPDATE_SCREENDATA);
         Assert.IsTrue(tcrLine.field == "901-Idle-Horizontal-MS500");
      }

      [TestMethod]
      public void TCR_CHANGING_MODE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_CHANGING_MODE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-01 16:13:50.613");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_CHANGING_MODE);
         Assert.IsTrue(tcrLine.field == "OutOfService");
      }

      [TestMethod]
      public void TCR_CHANGEMODE_FAILED()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_CHANGEMODE_FAILED);
         Assert.IsTrue(logLine is TCRLogLine);

         TCRLogLine tcrLine = (TCRLogLine)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-01 16:12:13.756");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_CHANGEMODE_FAILED);
      }

      [TestMethod]
      public void TCR_CURRENTMODE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_CURRENTMODE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-01 16:11:53.895");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_CURRENTMODE);
         Assert.IsTrue(tcrLine.field == "OffLine");
      }

      [TestMethod]
      public void TCR_NEXTSTATE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_NEXTSTATE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-13 09:10:30.899");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_NEXTSTATE);
         Assert.IsTrue(tcrLine.field == "Deposit-ConfirmResult");
      }

      /* DEPOSIT */

      [TestMethod]
      public void TCR_DEP_TELLERID()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_DEP_TELLERID);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 12:22:20.785");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_DEP_TELLERID);
         Assert.IsTrue(tcrLine.field == "Admin2");
      }

      [TestMethod]
      public void TCR_DEP_RESULT()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_DEP_RESULT);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 12:22:20.799");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_DEP_RESULT);
         Assert.IsTrue(tcrLine.field == "SUCCESS");
      }

      [TestMethod]
      public void TCR_DEP_ERRORCODE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_DEP_ERRORCODE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 12:22:20.806");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_DEP_ERRORCODE);
         Assert.IsTrue(tcrLine.field == "");
      }

      [TestMethod]
      public void TCR_DEP_CASHDEPOSITED()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_DEP_CASHDEPOSITED);
         Assert.IsTrue(logLine is TCRLogLineCash);

         TCRLogLineCash tcrLine = (TCRLogLineCash)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 12:22:20.820");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_DEP_CASHDEPOSITED);
         Assert.IsTrue(tcrLine.USD0 == "1");
         Assert.IsTrue(tcrLine.USD1 == "2");
         Assert.IsTrue(tcrLine.USD2 == "3");
         Assert.IsTrue(tcrLine.USD5 == "4");
         Assert.IsTrue(tcrLine.USD10 == "5");
         Assert.IsTrue(tcrLine.USD20 == "6");
         Assert.IsTrue(tcrLine.USD50 == "7");
         Assert.IsTrue(tcrLine.USD100 == "8");
      }

      [TestMethod]
      public void TCR_DEP_AMOUNT()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_DEP_AMOUNT);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 12:22:20.835");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_DEP_AMOUNT);
         Assert.IsTrue(tcrLine.field == "16000");
      }

      [TestMethod]
      public void TCR_DEP_BALANCE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_DEP_BALANCE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 12:22:20.842");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_DEP_BALANCE);
         Assert.IsTrue(tcrLine.field == "64506");
      }

      /* WD */

      [TestMethod]
      public void TCR_WD_TELLERID()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_WD_TELLERID);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 08:08:58.986");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_WD_TELLERID);
         Assert.IsTrue(tcrLine.field == "Admin2");
      }

      [TestMethod]
      public void TCR_WD_RESULT()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_WD_RESULT);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 08:08:58.999");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_WD_RESULT);
         Assert.IsTrue(tcrLine.field == "SUCCESS");
      }

      [TestMethod]
      public void TCR_WD_ERRORCODE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_WD_ERRORCODE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 08:08:59.006");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_WD_ERRORCODE);
         Assert.IsTrue(tcrLine.field == "");
      }

      [TestMethod]
      public void TCR_WD_CASHDISPENSED()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_WD_CASHDISPENSED);
         Assert.IsTrue(logLine is TCRLogLineCash);

         TCRLogLineCash tcrLine = (TCRLogLineCash)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 08:08:59.013");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_WD_CASHDISPENSED);
         Assert.IsTrue(tcrLine.USD0 == "");
         Assert.IsTrue(tcrLine.USD1 == "");
         Assert.IsTrue(tcrLine.USD2 == "");
         Assert.IsTrue(tcrLine.USD5 == "10");
         Assert.IsTrue(tcrLine.USD10 == "7");
         Assert.IsTrue(tcrLine.USD20 == "");
         Assert.IsTrue(tcrLine.USD50 == "");
         Assert.IsTrue(tcrLine.USD100 == "1");
      }

      [TestMethod]
      public void TCR_WD_AMOUNT()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_WD_AMOUNT);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 08:08:59.083");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_WD_AMOUNT);
         Assert.IsTrue(tcrLine.field == "220");
      }

      [TestMethod]
      public void TCR_WD_BALANCE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_WD_BALANCE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-15 08:08:59.095");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_WD_BALANCE);
         Assert.IsTrue(tcrLine.field == "59015");
      }

      [TestMethod]
      public void TCR_HOST_CMD()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_HOST_CMD);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-29 07:46:53.615");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_HOST_CMD);
         Assert.IsTrue(tcrLine.field == "GetPhysicalUnitInformation");
      }

      [TestMethod]
      public void TCR_HOST_CMD_RESPONSE()
      {
         // Create an APLogHandler that created TCR LogLines
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.TCR, TCRLogLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(tcr_samples_general.TCR_HOST_CMD_RESPONSE);
         Assert.IsTrue(logLine is TCRLogLineWithField);

         TCRLogLineWithField tcrLine = (TCRLogLineWithField)logLine;
         Assert.IsTrue(tcrLine.Timestamp == "2024-05-29 07:14:31.836");
         Assert.IsTrue(tcrLine.tcrType == TCRLogType.TCR_HOST_CMD_RESPONSE);
         Assert.IsTrue(tcrLine.field == "False|False|051301");
      }
   }
}
