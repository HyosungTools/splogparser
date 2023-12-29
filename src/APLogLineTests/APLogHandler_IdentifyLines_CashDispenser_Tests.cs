using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using APSamples;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_IdentifyLines_CashDispenser_Tests
   {
      /* CASH DISPENSER */

      [TestMethod]
      public void CashDispenser_Open()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDisp_Open);
         Assert.IsTrue(logLine is CashDispenser_Open);

         CashDispenser_Open apLine = (CashDispenser_Open)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_Open);
         Assert.IsTrue(apLine.Timestamp == "2023-08-15 08:21:19.249");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.numCashUnits == "6");
      }

      /* STATUS */

      /* device */

      [TestMethod]
      public void CashDispenser_OnLine()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnLine);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnLine);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.499");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OffLine()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OffLine);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OffLine);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:22:59.387");
         Assert.IsTrue(apLine.HResult == "");

      }

      [TestMethod]
      public void CashDispenser_OnHWError()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnHWError);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnHWError);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.399");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_DeviceError()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_DeviceError);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_DeviceError);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:02.264");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnDeviceOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_DeviceOK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnDeviceOK);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:10:45.642");
         Assert.IsTrue(apLine.HResult == "");
      }


      /* position status */

      [TestMethod]
      public void CashDispenser_NotInPosition()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_NotInPosition);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_NotInPosition);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 11:39:01.910");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_InPosition()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_InPosition);
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
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnNoDispense);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnNoDispense);
         Assert.IsTrue(apLine.Timestamp == "2023-11-01 08:09:37.043");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnDispenserOK()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnDispenserOK);
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
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnShutterOpen);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnShutterOpen);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:36.498");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnShutterClosed()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnShutterClosed);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnShutterClosed);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:26.416");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnStackerNotEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnStackerNotEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnStackerNotEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:19.122");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnStackerEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnStackerEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnStackerEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:23.289");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnTransportNotEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnTransportNotEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnTransportNotEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:13.618");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnTransportEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnTransportEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnTransportEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:48:45.613");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnPositionNotEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnPositionNotEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnPositionNotEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 07:54:49.085");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnPositionEmpty()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnPositionEmpty);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnPositionEmpty);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 05:54:06.326");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnCashUnitChanged()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnCashUnitChanged);
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
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_SetupCSTList);
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
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_SetupNoteType);
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
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnDenominateComplete);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnDenominateComplete);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:17.067");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_ExecDispense()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_ExecDispense);
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
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_DispenseSyncAsync);
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
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnDispenseComplete);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnDispenseComplete);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:33.721");
         Assert.IsTrue(apLine.HResult == "");
      }
      [TestMethod]
      public void CashDispenser_OnPresentComplete()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnPresentComplete);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnPresentComplete);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:57:36.543");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_OnItemsTaken()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_OnItemsTaken);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.CashDispenser_OnItemsTaken);
         Assert.IsTrue(apLine.Timestamp == "2023-10-31 20:59:26.419");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void CashDispenser_GetLCULastDispensedCount()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cashdisp.CashDispenser_GetLCULastDispensedCount);
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
