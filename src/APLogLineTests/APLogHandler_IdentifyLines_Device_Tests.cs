using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using APSamples;
using System;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_IdentifyLines_Device_Tests
   {
      /* CASH DISPENSER */

      [TestMethod]
      public void CashDispenser_Open()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDisp_Open);
         Assert.IsTrue(logLine is CashDispenser_Open);

         CashDispenser_Open apLine = (CashDispenser_Open)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_Open);
         Assert.IsTrue(apLine.Timestamp == "2023-08-15 08:21:19.249");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.numCashUnits == "6");
      }

      /* STATUS */

      /* CDM */

      [TestMethod]
      public void APLOG_CDM_ONLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CDM_ONLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CDM_ONLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.499");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_CDM_OFFLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CDM_OFFLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CDM_OFFLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:22:59.387");
         Assert.IsTrue(apLine.HResult == "");

      }

      [TestMethod]
      public void APLOG_CDM_ONHWERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CDM_ONHWERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CDM_ONHWERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.399");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_CDM_DEVERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CDM_DEVERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CDM_DEVERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:02.264");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_CDM_ONOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CDM_ONOK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CDM_ONOK);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:10:45.642");
         Assert.IsTrue(apLine.HResult == "");
      }

      /* CIM */

      [TestMethod]
      public void APLOG_CIM_ONLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CIM_ONLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CIM_ONLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.499");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_CIM_OFFLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CIM_OFFLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CIM_OFFLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:22:59.387");
         Assert.IsTrue(apLine.HResult == "");

      }

      [TestMethod]
      public void APLOG_CIM_ONHWERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CIM_ONHWERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CIM_ONHWERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.399");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_CIM_DEVERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CIM_DEVERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CIM_DEVERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:02.264");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_CIM_ONOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_CIM_ONOK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CIM_ONOK);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:10:45.642");
         Assert.IsTrue(apLine.HResult == "");
      }

      /* IPM */

      [TestMethod]
      public void APLOG_IPM_ONLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_IPM_ONLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_IPM_ONLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.499");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_IPM_OFFLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_IPM_OFFLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_IPM_OFFLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:22:59.387");
         Assert.IsTrue(apLine.HResult == "");

      }

      [TestMethod]
      public void APLOG_IPM_ONHWERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_IPM_ONHWERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_IPM_ONHWERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.399");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_IPM_DEVERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_IPM_DEVERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_IPM_DEVERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:02.264");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_IPM_ONOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_IPM_ONOK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_IPM_ONOK);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:10:45.642");
         Assert.IsTrue(apLine.HResult == "");
      }

      /* MMA */

      [TestMethod]
      public void APLOG_MMA_ONLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_MMA_ONLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_MMA_ONLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.499");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_MMA_OFFLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_MMA_OFFLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_MMA_OFFLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:22:59.387");
         Assert.IsTrue(apLine.HResult == "");

      }

      [TestMethod]
      public void APLOG_MMA_ONHWERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_MMA_ONHWERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_MMA_ONHWERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.399");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_MMA_DEVERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_MMA_DEVERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_MMA_DEVERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:02.264");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_MMA_ONOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_MMA_ONOK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_MMA_ONOK);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:10:45.642");
         Assert.IsTrue(apLine.HResult == "");
      }

      /* Receipt Printer */

      [TestMethod]
      public void APLOG_RCT_ONLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_RCT_ONLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_RCT_ONLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.499");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_RCT_OFFLINE()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_RCT_OFFLINE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_RCT_OFFLINE);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:22:59.387");
         Assert.IsTrue(apLine.HResult == "");

      }

      [TestMethod]
      public void APLOG_RCT_ONHWERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_RCT_ONHWERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_RCT_ONHWERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.399");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_RCT_DEVERROR()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_RCT_DEVERROR);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_RCT_DEVERROR);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:02.264");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLOG_RCT_ONOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.APLOG_RCT_ONOK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_RCT_ONOK);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:10:45.642");
         Assert.IsTrue(apLine.HResult == "");
      }

      /* position status */

      [TestMethod]
      public void CashDispenser_NotInPosition()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_NotInPosition);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_NotInPosition);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:01.910");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_InPosition()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_InPosition);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_InPosition);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:02.381");
         Assert.IsTrue(apLine.HResult == "");
      }


      /* dispense */

      [TestMethod]
      public void CashDispenser_OnNoDispense()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnNoDispense);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnNoDispense);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:09:37.043");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnDispenserOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnDispenserOK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnDispenserOK);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:34:40.912");
         Assert.IsTrue(apLine.HResult == "");
      }



      /* status - shutter, position, stacker, transport */

      [TestMethod]
      public void CashDispenser_OnShutterOpen()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnShutterOpen);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnShutterOpen);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:36.498");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnShutterClosed()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnShutterClosed);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnShutterClosed);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:26.416");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnStackerNotEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnStackerNotEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnStackerNotEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:19.122");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnStackerEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnStackerEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnStackerEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:23.289");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnTransportNotEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnTransportNotEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnTransportNotEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.618");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnTransportEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnTransportEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnTransportEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.613");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnPositionNotEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnPositionNotEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnPositionNotEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 07:54:49.085");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnPositionEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnPositionEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnPositionEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 05:54:06.326");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnCashUnitChanged()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnCashUnitChanged);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnCashUnitChanged);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:46.523");
         Assert.IsTrue(apLine.HResult == "");
      }


      /* SUMMARY */

      /* summary - set up */

      [TestMethod]
      public void CashDispenser_SetupCSTList()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_SetupCSTList);
         Assert.IsTrue(logLine is CashDispenser_SetupCSTList);

         CashDispenser_SetupCSTList apLine = (CashDispenser_SetupCSTList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_SetupCSTList);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:21.590");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.relation == "1:1");
         Assert.IsTrue(apLine.parent == "A");
         Assert.IsTrue(apLine.child == "2");
      }

      [TestMethod]
      public void CashDispenser_SetupNoteType()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_SetupNoteType);
         Assert.IsTrue(logLine is CashDispenser_SetupNoteType);

         CashDispenser_SetupNoteType apLine = (CashDispenser_SetupNoteType)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_SetupNoteType);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:21.594");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.noteType == "A");
         Assert.IsTrue(apLine.currency == "USD");
         Assert.IsTrue(apLine.value == "1");
         Assert.IsTrue(apLine.splcu == "2");
         Assert.IsTrue(apLine.sppcu == "-1");
      }


      /* DISPENSE */

      [TestMethod]
      public void CashDispenser_OnDenominateComplete()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnDenominateComplete);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnDenominateComplete);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:17.067");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_ExecDispense()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_ExecDispense);
         Assert.IsTrue(logLine is CashDispenser_ExecDispense);

         CashDispenser_ExecDispense apLine = (CashDispenser_ExecDispense)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_ExecDispense);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:21.724");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.hostAmount == "20");
      }

      [TestMethod]
      public void CashDispenser_DispenseSyncAsync()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_DispenseSyncAsync);
         Assert.IsTrue(logLine is CashDispenser_DispenseSyncAsync);

         CashDispenser_DispenseSyncAsync apLine = (CashDispenser_DispenseSyncAsync)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_DispenseSyncAsync);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:21.739");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.mixAlgo == "0");
         Assert.IsTrue(apLine.currency == "USD");
         Assert.IsTrue(apLine.amount == "0");
         Assert.IsTrue(apLine.dispense.Length == 6);
         Assert.IsTrue(apLine.dispense[0] == "0");
         Assert.IsTrue(apLine.dispense[1] == "0");
         Assert.IsTrue(apLine.dispense[2] == "0");
         Assert.IsTrue(apLine.dispense[3] == "0");
         Assert.IsTrue(apLine.dispense[4] == "1");
         Assert.IsTrue(apLine.dispense[5] == "0");

      }

      [TestMethod]
      public void CashDispenser_OnDispenseComplete()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnDispenseComplete);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnDispenseComplete);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:33.721");
         Assert.IsTrue(apLine.HResult == "");
      }
      [TestMethod]
      public void CashDispenser_OnPresentComplete()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnPresentComplete);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnPresentComplete);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:36.543");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnItemsTaken()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_OnItemsTaken);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnItemsTaken);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:26.419");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_GetLCULastDispensedCount()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory);
         ILogLine logLine = logFileHandler.IdentifyLine(samples_devices.CashDispenser_GetLCULastDispensedCount);
         Assert.IsTrue(logLine is CashDispenser_GetLCULastDispensedCount);

         CashDispenser_GetLCULastDispensedCount apLine = (CashDispenser_GetLCULastDispensedCount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_GetLCULastDispensedCount);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:36.555");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.noteType == "A");
         Assert.IsTrue(apLine.amount == "0");
      }
   }
}
