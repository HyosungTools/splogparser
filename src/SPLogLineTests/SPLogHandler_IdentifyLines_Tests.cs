using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using Samples;
using Contract;

namespace SPLogLineTests
{
   [TestClass]
   public class SPLogHandler_IdentifyLines_Tests
   {
      /* 1 - PTR */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_PTR_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_PTR_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.Timestamp == "2023-03-17 08:42:28.033");
         Assert.IsTrue(spLine.HResult == "1");
      }

      /* 2 - IDC */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_IDC_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_INF_IDC_STATUS);
         Assert.IsTrue(logLine is WFSIDCSTATUS);

         WFSIDCSTATUS spLine = (WFSIDCSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_IDC_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-01-30 20:43:30.395");
         Assert.IsTrue(spLine.HResult == "1");

         Assert.IsTrue(spLine.fwDevice == "2"); ;
         Assert.IsTrue(spLine.fwMedia == "3");
         Assert.IsTrue(spLine.fwRetainBin == "4");
         Assert.IsTrue(spLine.fwSecurity == "5");
         Assert.IsTrue(spLine.usCards == "6");
         Assert.IsTrue(spLine.fwChipPower == "7");
         Assert.IsTrue(spLine.fwChipModule == "8");
         Assert.IsTrue(spLine.fwMagReadModule == "9");
         Assert.IsTrue(spLine.ErrorCode == "0000000");
         Assert.IsTrue(spLine.Description == "System OK!");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_IDC_CAPABILITIES()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_INF_IDC_CAPABILITIES);
         Assert.IsTrue(logLine is WFSIDCCAPABILITIES);

         WFSIDCCAPABILITIES spLine = (WFSIDCCAPABILITIES)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_IDC_CAPABILITIES);
         Assert.IsTrue(spLine.Timestamp == "2024-01-16 03:03:57.592");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.fwType == "0x0005"); ;
         Assert.IsTrue(spLine.bCompound == "0");
         Assert.IsTrue(spLine.fwReadTracks == "0x0007");
         Assert.IsTrue(spLine.fwWriteTracks == "0x0000");
         Assert.IsTrue(spLine.fwChipProtocols == "0x0003");
         Assert.IsTrue(spLine.usCards == "0");
         Assert.IsTrue(spLine.fwSecType == "1");
         Assert.IsTrue(spLine.fwPowerOnOption == "1");
         Assert.IsTrue(spLine.fwPowerOffOption == "1");
         Assert.IsTrue(spLine.bFluxSensorProgrammable == "0");
         Assert.IsTrue(spLine.bReadWriteAccessFollowingEject == "0");
         Assert.IsTrue(spLine.fwWriteMode == "0");
         Assert.IsTrue(spLine.fwChipPower == "12");
         Assert.IsTrue(spLine.lpszExtra2 == "AttemptMSReadFirst=0,XFS_MIB_VERSION=0x00000A01");
         Assert.IsTrue(spLine.fwDIPMode == "0");
         Assert.IsTrue(spLine.lpwMemoryChipProtocols == "NULL");
         Assert.IsTrue(spLine.fwEjectPosition == "0");
         Assert.IsTrue(spLine.bPowerSaveControl == "0");
         Assert.IsTrue(spLine.usParkingStations == "0");
         Assert.IsTrue(spLine.bAntiFraudModule == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_IDC_READ_RAW_DATA()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_CMD_IDC_READ_RAW_DATA);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IDC_READ_RAW_DATA);
         Assert.IsTrue(spLine.Timestamp == "2024-01-16 09:31:04.379");
         Assert.IsTrue(spLine.HResult == "-4");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_IDC_CHIP_IO()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_CMD_IDC_CHIP_IO);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IDC_CHIP_IO);
         Assert.IsTrue(spLine.Timestamp == "2024-01-16 09:11:44.180");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_IDC_CHIP_POWER()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_CMD_IDC_CHIP_POWER);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IDC_CHIP_POWER);
         Assert.IsTrue(spLine.Timestamp == "2024-01-12 08:38:30.060");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_EXEE_IDC_MEDIAINSERTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_EXEE_IDC_MEDIAINSERTED);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IDC_MEDIAINSERTED);
         Assert.IsTrue(spLine.Timestamp == "2024-01-16 09:11:41.536");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_IDC_MEDIAREMOVED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_SRVE_IDC_MEDIAREMOVED);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_IDC_MEDIAREMOVED);
         Assert.IsTrue(spLine.Timestamp == "2024-02-07 21:22:32.656");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_USRE_IDC_RETAINBINTHRESHOLD()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_USRE_IDC_RETAINBINTHRESHOLD);
         Assert.IsTrue(logLine is WFSIDCSTATUS);

         WFSIDCRETAINBINTHRESHOLD spLine = (WFSIDCRETAINBINTHRESHOLD)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_USRE_IDC_RETAINBINTHRESHOLD);
         Assert.IsTrue(spLine.Timestamp == "2023-12-07 16:01:20.549");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.fwRetainBin == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_EXEE_IDC_INVALIDMEDIA()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_EXEE_IDC_INVALIDMEDIA);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IDC_INVALIDMEDIA);
         Assert.IsTrue(spLine.Timestamp == "2023-12-07 07:49:56.474");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_EXEE_IDC_MEDIARETAINED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_EXEE_IDC_MEDIARETAINED);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IDC_MEDIARETAINED);
         Assert.IsTrue(spLine.Timestamp == "2024-01-19 13:38:31.092");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_SRVE_IDC_MEDIADETECTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_SRVE_IDC_MEDIADETECTED);
         Assert.IsTrue(logLine is WFSIDCMEDIADETECTED);

         WFSIDCMEDIADETECTED spLine = (WFSIDCMEDIADETECTED)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_IDC_MEDIADETECTED);
         Assert.IsTrue(spLine.Timestamp == "2023-12-23 18:02:15.631");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.lppwResetOut == "3");
      }


      /* 3 - CDM */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CDM_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_STATUS_1);
         Assert.IsTrue(logLine is WFSCDMSTATUS);

         WFSCDMSTATUS spLine = (WFSCDMSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CDM_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 00:59:14.663");
         Assert.IsTrue(spLine.HResult == "1");

         Assert.IsTrue(spLine.fwDevice == "2");
         Assert.IsTrue(spLine.fwSafeDoor == "3");
         Assert.IsTrue(spLine.fwDispenser == "4");
         Assert.IsTrue(spLine.fwIntStacker == "3");
         // lppPositions
         Assert.IsTrue(spLine.fwPosition == "4");
         Assert.IsTrue(spLine.fwShutter == "5");
         Assert.IsTrue(spLine.fwPositionStatus == "6");
         Assert.IsTrue(spLine.fwTransport == "7");
         Assert.IsTrue(spLine.fwTransportStatus == "8");

         Assert.IsTrue(spLine.wDevicePosition == "9");
         Assert.IsTrue(spLine.usPowerSaveRecoveryTime == "10");
         Assert.IsTrue(spLine.wAntiFraudModule == "11");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CDM_CASH_UNIT_INFO()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_1);
         Assert.IsTrue(logLine is WFSCDMCUINFO);

         WFSCDMCUINFO spLine = (WFSCDMCUINFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CDM_CASH_UNIT_INFO);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 00:59:14.655");
         Assert.IsTrue(spLine.HResult == "-1");

         Assert.IsTrue(spLine.lUnitCount == 6);

         Assert.IsTrue(spLine.usNumbers[0] == "1");
         Assert.IsTrue(spLine.usNumbers[5] == "6");
         Assert.IsTrue(spLine.usTypes[0] == "6");
         Assert.IsTrue(spLine.usTypes[5] == "12");
         Assert.IsTrue(spLine.cUnitIDs[0] == "LCU00");
         Assert.IsTrue(spLine.cUnitIDs[5] == "LCU05");
         Assert.IsTrue(spLine.cCurrencyIDs[0] == "");
         Assert.IsTrue(spLine.cCurrencyIDs[5] == "USD");
         Assert.IsTrue(spLine.ulValues[0] == "0");
         Assert.IsTrue(spLine.ulValues[5] == "50");
         Assert.IsTrue(spLine.ulInitialCounts[0] == "0");
         Assert.IsTrue(spLine.ulInitialCounts[5] == "2000");
         Assert.IsTrue(spLine.ulCounts[0] == "0");
         Assert.IsTrue(spLine.ulCounts[5] == "1336");
         Assert.IsTrue(spLine.ulRejectCounts[0] == "1");
         Assert.IsTrue(spLine.ulRejectCounts[5] == "6");
         Assert.IsTrue(spLine.ulMinimums[0] == "0");
         Assert.IsTrue(spLine.ulMinimums[5] == "0");
         Assert.IsTrue(spLine.ulMaximums[0] == "80");
         Assert.IsTrue(spLine.ulMaximums[5] == "0");
         Assert.IsTrue(spLine.usStatuses[0] == "4");
         Assert.IsTrue(spLine.usStatuses[5] == "0");
         Assert.IsTrue(spLine.ulDispensedCounts[0] == "0");
         Assert.IsTrue(spLine.ulDispensedCounts[5] == "846");
         Assert.IsTrue(spLine.ulPresentedCounts[0] == "0");
         Assert.IsTrue(spLine.ulPresentedCounts[5] == "842");
         Assert.IsTrue(spLine.ulRetractedCounts[0] == "1");
         Assert.IsTrue(spLine.ulRetractedCounts[5] == "6");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CDM_PRESENT_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_PRESENT_STATUS);
         Assert.IsTrue(logLine is WFSCDMPRESENTSTATUS);

         WFSCDMPRESENTSTATUS spLine = (WFSCDMPRESENTSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CDM_PRESENT_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-03-02 17:26:22.330");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.CDMDENOM.cCurrencyID == "USD");
         Assert.IsTrue(spLine.CDMDENOM.ulAmount == "1000");
         Assert.IsTrue(spLine.CDMDENOM.usCount == "2");
         Assert.IsTrue(spLine.CDMDENOM.lpulValues[0] == "0");
         Assert.IsTrue(spLine.CDMDENOM.lpulValues[1] == "50");
         Assert.IsTrue(spLine.CDMDENOM.ulCashBox == "0");

         Assert.IsTrue(spLine.wPresentState == "1");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CDM_DISPENSE()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_CMD_CDM_DISPENSE_1);
         Assert.IsTrue(logLine is WFSCDMDENOMINATION);

         WFSCDMDENOMINATION spLine = (WFSCDMDENOMINATION)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CDM_DISPENSE);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 00:33:24.804");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.cCurrencyID == "USD");
         Assert.IsTrue(spLine.ulAmount == "30");
         Assert.IsTrue(spLine.usCount == "31");
         Assert.IsTrue(spLine.lpulValues[0] == "32");
         Assert.IsTrue(spLine.lpulValues[1] == "33");
         Assert.IsTrue(spLine.lpulValues[2] == "34");
         Assert.IsTrue(spLine.lpulValues[3] == "35");
         Assert.IsTrue(spLine.lpulValues[4] == "36");
         Assert.IsTrue(spLine.lpulValues[5] == "37");
         Assert.IsTrue(spLine.ulCashBox == "38");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CDM_PRESENT()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_CMD_CDM_PRESENT_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CDM_PRESENT);
         Assert.IsTrue(spLine.Timestamp == "2023-01-23 22:47:09.590");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CDM_REJECT()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_CMD_CDM_REJECT_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CDM_REJECT);
         Assert.IsTrue(spLine.Timestamp == "2022-10-30 17:21:21.571");
         Assert.IsTrue(spLine.HResult == "-316");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CDM_RETRACT()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_CMD_CDM_RETRACT_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CDM_RETRACT);
         Assert.IsTrue(spLine.Timestamp == "2022-12-18 06:44:37.211");
         Assert.IsTrue(spLine.HResult == "-316");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CDM_RESET()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_CMD_CDM_RESET_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CDM_RESET);
         Assert.IsTrue(spLine.Timestamp == "2022-12-18 21:25:31.995");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CDM_STARTEX()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_CMD_CDM_START_EXCHANGE);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CDM_STARTEX);
         Assert.IsTrue(spLine.Timestamp == "2023-04-03 11:59:51.794");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CDM_ENDEX()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_CMD_CDM_END_EXCHANGE);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CDM_ENDEX);
         Assert.IsTrue(spLine.Timestamp == "2023-04-03 11:59:57.419");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_CDM_CASHUNITINFOCHANGED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_SRVE_CDM_CASHUNITINFOCHANGED_1);
         Assert.IsTrue(logLine is WFSCDMCUINFO);

         WFSCDMCUINFO spLine = (WFSCDMCUINFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED);
         Assert.IsTrue(spLine.Timestamp == "2023-11-01 09:37:41.012");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.lUnitCount == 1);
         Assert.IsTrue(spLine.usNumbers[0] == "3");
         Assert.IsTrue(spLine.usTypes[0] == "12");
         Assert.IsTrue(spLine.cUnitIDs[0] == "LCU02");
         Assert.IsTrue(spLine.cCurrencyIDs[0] == "USD");
         Assert.IsTrue(spLine.ulValues[0] == "20");
         Assert.IsTrue(spLine.ulInitialCounts[0] == "0");
         Assert.IsTrue(spLine.ulCounts[0] == "0");
         Assert.IsTrue(spLine.ulRejectCounts[0] == "0");
         Assert.IsTrue(spLine.ulMinimums[0] == "0");
         Assert.IsTrue(spLine.ulMaximums[0] == "0");
         Assert.IsTrue(spLine.usStatuses[0] == "0");
         Assert.IsTrue(spLine.ulDispensedCounts[0] == "17");
         Assert.IsTrue(spLine.ulPresentedCounts[0] == "0");
         Assert.IsTrue(spLine.ulRetractedCounts[0] == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_CDM_ITEMSTAKEN()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_SRVE_CDM_ITEMSTAKEN_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_CDM_ITEMSTAKEN);
         Assert.IsTrue(spLine.Timestamp == "2022-12-18 23:15:26.187");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 4 - PIN */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_PIN_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_PIN_STATUS);
         Assert.IsTrue(logLine is WFSPINSTATUS);

         WFSPINSTATUS spLine = (WFSPINSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_PIN_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 03:02:10.462");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_PIN_GET_PIN()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_pin.WFS_CMD_PIN_GET_PIN);
         Assert.IsTrue(logLine is WFSPINGETPIN);

         WFSPINGETPIN spLine = (WFSPINGETPIN)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_PIN_GET_PIN);
         Assert.IsTrue(spLine.Timestamp == "2024-01-31 19:21:42.844");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.wCompletion == "1");
         Assert.IsTrue(spLine.usDigits == "4");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_PIN_GET_PIN_2()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_pin.WFS_CMD_PIN_GET_PIN_2);
         Assert.IsTrue(logLine is WFSPINGETPIN);

         WFSPINGETPIN spLine = (WFSPINGETPIN)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_PIN_GET_PIN);
         Assert.IsTrue(spLine.Timestamp == "2024-01-16 09:11:56.394");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.wCompletion == "1");
         Assert.IsTrue(spLine.usDigits == "4");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_PIN_GET_PINBLOCK()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_pin.WFS_CMD_PIN_GET_PINBLOCK);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_PIN_GET_PINBLOCK);
         Assert.IsTrue(spLine.Timestamp == "2024-01-31 19:21:43.433");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_PIN_GET_DATA()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_pin.WFS_CMD_PIN_GET_DATA);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_PIN_GET_DATA);
         Assert.IsTrue(spLine.Timestamp == "2024-02-28 21:24:23.209");
         Assert.IsTrue(spLine.HResult == "-4");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_PIN_RESET()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_pin.WFS_CMD_PIN_RESET);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_PIN_RESET);
         Assert.IsTrue(spLine.Timestamp == "2024-01-26 16:46:02.043");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_EXEE_PIN_KEY()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_pin.WFS_EXEE_PIN_KEY);
         Assert.IsTrue(logLine is WFSPINKEY);

         WFSPINKEY spLine = (WFSPINKEY)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_PIN_KEY);
         Assert.IsTrue(spLine.Timestamp == "2024-01-16 09:16:06.205");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.wCompletion == "6");
         Assert.IsTrue(spLine.ulDigit == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_EXEE_PIN_KE2()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_pin.WFS_EXEE_PIN_KEY_2);
         Assert.IsTrue(logLine is WFSPINKEY);

         WFSPINKEY spLine = (WFSPINKEY)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_PIN_KEY);
         Assert.IsTrue(spLine.Timestamp == "2024-03-01 18:52:53.336");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.wCompletion == "6");
         Assert.IsTrue(spLine.ulDigit == "0");
      }


      /* 5 - CHK  - Ignore */
      [Ignore]
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CHK_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_CHK_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CHK_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-01-04 03:02:10.462");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 6 - DEP  - Ignore */
      [Ignore]
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_DEP_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_DEP_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_DEP_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-01-04 03:02:10.462");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 7 - TTU */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_TTU_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_TTU_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_TTU_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-11-17 09:11:06.122");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 8 - SIU */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_SIU_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_SIU_STATUS);
         Assert.IsTrue(logLine is WFSSIUSTATUS);

         WFSSIUSTATUS spLine = (WFSSIUSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_SIU_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-11-17 09:11:06.385");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.fwSafeDoor == "0x0001");
         Assert.IsTrue(spLine.fwDevice == "0");
         Assert.IsTrue(spLine.opSwitch == "0x0001");
         Assert.IsTrue(spLine.tamper == "0x0000");
         Assert.IsTrue(spLine.cabinet == "0x0001");
         Assert.IsTrue(spLine.ups == "0x0001");
         Assert.IsTrue(spLine.errorCode == "0000000");
         Assert.IsTrue(spLine.description == "Success. System OK!");
      }

      /* 9 - VDM */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_VDM_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_VDM_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_VDM_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-11-17 09:35:16.341");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 10 - CAM */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CAM_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_CAM_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CAM_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-09-26 03:02:46.203");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 11 - ALM - Ignore */
      [Ignore]
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_ALM_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_ALM_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_ALM_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-09-26 03:02:46.203");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 12 - CEU - Ignore */
      [Ignore]
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CEU_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_CEU_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CEU_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-09-26 03:02:46.203");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 13 - CIM */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CIM_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_INF_CIM_STATUS_1);
         Assert.IsTrue(logLine is WFSCIMSTATUS);

         WFSCIMSTATUS spLine = (WFSCIMSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CIM_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 00:59:14.664");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.fwDevice == "1");
         Assert.IsTrue(spLine.fwSafeDoor == "2");
         Assert.IsTrue(spLine.fwAcceptor == "3");
         Assert.IsTrue(spLine.fwIntStacker == "4");
         Assert.IsTrue(spLine.fwStackerItems == "5");
         //Assert.IsTrue(spLine.fwBanknoteReader == "");
         Assert.IsTrue(spLine.bDropBox == "0");

         Assert.IsTrue(spLine.wDevicePosition == "0");
         Assert.IsTrue(spLine.usPowerSaveRecoveryTime == "0");
         Assert.IsTrue(spLine.wMixedMode == "0");
         Assert.IsTrue(spLine.wAntiFraudModule == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CIM_CASH_UNIT_INFO()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_1);
         Assert.IsTrue(logLine is WFSCIMCASHINFO);

         WFSCIMCASHINFO spLine = (WFSCIMCASHINFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 00:59:14.660");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.lUnitCount == 7);
         Assert.IsTrue(spLine.usNumbers[0] == "1");
         Assert.IsTrue(spLine.usNumbers[6] == "7");
         Assert.IsTrue(spLine.fwTypes[0] == "8");
         Assert.IsTrue(spLine.fwTypes[6] == "14");
         Assert.IsTrue(spLine.cUnitIDs[0] == "LCU00");
         Assert.IsTrue(spLine.cUnitIDs[6] == "LCU06");
         Assert.IsTrue(spLine.cCurrencyIDs[0] == "");
         Assert.IsTrue(spLine.cCurrencyIDs[5] == "JKL");
         Assert.IsTrue(spLine.ulValues[0] == "15");
         Assert.IsTrue(spLine.ulValues[6] == "21");
         Assert.IsTrue(spLine.ulCashInCounts[0] == "22");
         Assert.IsTrue(spLine.ulCashInCounts[6] == "28");
         Assert.IsTrue(spLine.ulCounts[0] == "29");
         Assert.IsTrue(spLine.ulCounts[6] == "35");
         Assert.IsTrue(spLine.ulMaximums[0] == "36");
         Assert.IsTrue(spLine.ulMaximums[6] == "42");
         Assert.IsTrue(spLine.usStatuses[0] == "43");
         Assert.IsTrue(spLine.usStatuses[6] == "49");
         Assert.IsTrue(spLine.noteNumbers[4, 0] == "5:2158");
         Assert.IsTrue(spLine.noteNumbers[5, 0] == "6:1336");
         Assert.IsTrue(spLine.ulInitialCounts[0] == "100");
         Assert.IsTrue(spLine.ulInitialCounts[6] == "106");
         Assert.IsTrue(spLine.ulDispensedCounts[0] == "107");
         Assert.IsTrue(spLine.ulDispensedCounts[6] == "113");
         Assert.IsTrue(spLine.ulPresentedCounts[0] == "114");
         Assert.IsTrue(spLine.ulPresentedCounts[6] == "120");
         Assert.IsTrue(spLine.ulRetractedCounts[0] == "121");
         Assert.IsTrue(spLine.ulRetractedCounts[6] == "127");
         Assert.IsTrue(spLine.ulRejectCounts[0] == "128");
         Assert.IsTrue(spLine.ulRejectCounts[6] == "134");
         Assert.IsTrue(spLine.ulMinimums[0] == "135");
         Assert.IsTrue(spLine.ulMinimums[6] == "141");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CIM_CASH_IN_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_1);
         Assert.IsTrue(logLine is WFSCIMCASHINSTATUS);

         WFSCIMCASHINSTATUS spLine = (WFSCIMCASHINSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 23:55:47.441");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.wStatus == "2");
         Assert.IsTrue(spLine.usNumOfRefused == "3");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_CASH_IN_START()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_CASH_IN_START_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_START);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 23:55:47.437");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_CASH_IN()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_CASH_IN_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_CASH_IN);
         Assert.IsTrue(spLine.Timestamp == "2023-01-03 14:42:11.793");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_CASH_IN_END()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_CASH_IN_END_1);
         Assert.IsTrue(logLine is WFSCIMCASHINFO);

         WFSCIMCASHINFO spLine = (WFSCIMCASHINFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_END);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 23:57:44.202");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.lUnitCount == 1);
         Assert.IsTrue(spLine.usNumbers[0] == "7");
         Assert.IsTrue(spLine.fwTypes[0] == "2");
         Assert.IsTrue(spLine.cUnitIDs[0] == "LCU06");
         Assert.IsTrue(spLine.cCurrencyIDs[0] == "");
         Assert.IsTrue(spLine.ulValues[0] == "0");
         Assert.IsTrue(spLine.ulCashInCounts[0] == "1");
         Assert.IsTrue(spLine.ulCounts[0] == "1");
         Assert.IsTrue(spLine.ulMaximums[0] == "1400");
         Assert.IsTrue(spLine.usStatuses[0] == "0");
         Assert.IsTrue(spLine.noteNumbers[0, 16] == "17:1");
         Assert.IsTrue(spLine.ulInitialCounts[0] == "0");
         Assert.IsTrue(spLine.ulDispensedCounts[0] == "0");
         Assert.IsTrue(spLine.ulPresentedCounts[0] == "0");
         Assert.IsTrue(spLine.ulRetractedCounts[0] == "0"); ;
         Assert.IsTrue(spLine.ulRejectCounts[0] == "0");
         Assert.IsTrue(spLine.ulMinimums[0] == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_CASH_IN_ROLLBACK()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_CASH_IN_ROLLBACK_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK);
         Assert.IsTrue(spLine.Timestamp == "2022-10-31 13:26:13.111");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_RETRACT()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_RETRACT_1);
         Assert.IsTrue(logLine is WFSCIMCASHINFO);

         WFSCIMCASHINFO spLine = (WFSCIMCASHINFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_RETRACT);
         Assert.IsTrue(spLine.Timestamp == "2022-10-23 17:52:11.388");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.lUnitCount == 1);
         Assert.IsTrue(spLine.usNumbers[0] == "1");
         Assert.IsTrue(spLine.fwTypes[0] == "4");
         Assert.IsTrue(spLine.cUnitIDs[0] == "LCU00");
         Assert.IsTrue(spLine.cCurrencyIDs[0] == "USD");
         Assert.IsTrue(spLine.ulValues[0] == "0");
         Assert.IsTrue(spLine.ulCashInCounts[0] == "17");
         Assert.IsTrue(spLine.ulCounts[0] == "1");
         Assert.IsTrue(spLine.ulMaximums[0] == "126");
         Assert.IsTrue(spLine.usStatuses[0] == "0");
         Assert.IsTrue(spLine.noteNumbers[0, 0] == "1:3");
         Assert.IsTrue(spLine.ulInitialCounts[0] == "0");
         Assert.IsTrue(spLine.ulDispensedCounts[0] == "0");
         Assert.IsTrue(spLine.ulPresentedCounts[0] == "0");
         Assert.IsTrue(spLine.ulRetractedCounts[0] == "0"); ;
         Assert.IsTrue(spLine.ulRejectCounts[0] == "0");
         Assert.IsTrue(spLine.ulMinimums[0] == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_RESET()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_RESET_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_RESET);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 11:19:13.553");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_START_EXCHANGE()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_START_EXCHANGE);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_STARTEX);
         Assert.IsTrue(spLine.Timestamp == "2023-02-27 07:49:10.694");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_CMD_CIM_ENDEXCHANGE()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_CMD_CIM_END_EXCHANGE);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_CIM_ENDEX);
         Assert.IsTrue(spLine.Timestamp == "2023-04-03 12:00:07.540");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_USRE_CIM_CASHUNITTHRESHOLD()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_USRE_CIM_CASHUNITTHRESHOLD_1);
         Assert.IsTrue(logLine is WFSCIMCASHINFO);

         WFSCIMCASHINFO spLine = (WFSCIMCASHINFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD);
         Assert.IsTrue(spLine.Timestamp == "2023-08-15 15:11:54.556");
         Assert.IsTrue(spLine.HResult == "9");

         Assert.IsTrue(spLine.lUnitCount == 1);
         Assert.IsTrue(spLine.usNumbers[0] == "5");
         Assert.IsTrue(spLine.fwTypes[0] == "1");
         Assert.IsTrue(spLine.cUnitIDs[0] == "LCU04");
         Assert.IsTrue(spLine.cCurrencyIDs[0] == "USD");
         Assert.IsTrue(spLine.ulValues[0] == "20");
         Assert.IsTrue(spLine.ulCashInCounts[0] == "2082");
         Assert.IsTrue(spLine.ulCounts[0] == "1851");
         Assert.IsTrue(spLine.ulMaximums[0] == "0");
         Assert.IsTrue(spLine.usStatuses[0] == "1");
         Assert.IsTrue(spLine.noteNumbers[0, 0] == "5:1851");
         Assert.IsTrue(spLine.ulInitialCounts[0] == "485");
         Assert.IsTrue(spLine.ulDispensedCounts[0] == "716");
         Assert.IsTrue(spLine.ulPresentedCounts[0] == "716");
         Assert.IsTrue(spLine.ulRetractedCounts[0] == "0"); ;
         Assert.IsTrue(spLine.ulRejectCounts[0] == "0");
         Assert.IsTrue(spLine.ulMinimums[0] == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_CIM_CASHUNITINFOCHANGED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_1);
         Assert.IsTrue(logLine is WFSCIMCASHINFO);

         WFSCIMCASHINFO spLine = (WFSCIMCASHINFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 21:44:59.497");
         Assert.IsTrue(spLine.HResult == "-49");

         Assert.IsTrue(spLine.lUnitCount == 1);
         Assert.IsTrue(spLine.usNumbers[0] == "5");
         Assert.IsTrue(spLine.fwTypes[0] == "1");
         Assert.IsTrue(spLine.cUnitIDs[0] == "LCU04");
         Assert.IsTrue(spLine.cCurrencyIDs[0] == "USD");
         Assert.IsTrue(spLine.ulValues[0] == "20");
         Assert.IsTrue(spLine.ulCashInCounts[0] == "17");
         Assert.IsTrue(spLine.ulCounts[0] == "0");
         Assert.IsTrue(spLine.ulMaximums[0] == "0");
         Assert.IsTrue(spLine.usStatuses[0] == "0");
         Assert.IsTrue(spLine.noteNumbers[0, 0] == "5:0");
         Assert.IsTrue(spLine.ulInitialCounts[0] == "2000");
         Assert.IsTrue(spLine.ulDispensedCounts[0] == "17");
         Assert.IsTrue(spLine.ulPresentedCounts[0] == "16");
         Assert.IsTrue(spLine.ulRetractedCounts[0] == "0"); ;
         Assert.IsTrue(spLine.ulRejectCounts[0] == "1");
         Assert.IsTrue(spLine.ulMinimums[0] == "0");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_CIM_ITEMSTAKEN()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_ITEMSTAKEN_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_CIM_ITEMSTAKEN);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 13:22:45.453");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_EXEE_CIM_INPUTREFUSE()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_EXEE_CIM_INPUTREFUSE_1);
         Assert.IsTrue(logLine is WFSCIMINPUTREFUSE);

         WFSCIMINPUTREFUSE spLine = (WFSCIMINPUTREFUSE)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_CIM_INPUTREFUSE);
         Assert.IsTrue(spLine.Timestamp == "2022-10-30 11:47:03.923");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.usReason == "8");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFSCIMNOTEERROR()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_EXEE_CIM_NOTEERROR_2);
         Assert.IsTrue(logLine is WFSCIMNOTEERROR);

         WFSCIMNOTEERROR spLine = (WFSCIMNOTEERROR)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_CIM_NOTEERROR);
         Assert.IsTrue(spLine.Timestamp == "2024-12-11 13:38:13.895");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.usReason == "3");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_CIM_ITEMSPRESENTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_ITEMSPRESENTED_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_CIM_ITEMSPRESENTED);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 13:22:39.162");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_CIM_ITEMSINSERTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_ITEMSINSERTED_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_CIM_ITEMSINSERTED);
         Assert.IsTrue(spLine.Timestamp == "2023-01-24 12:24:40.473");
         Assert.IsTrue(spLine.HResult == "");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_SRVE_CIM_MEDIADETECTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_MEDIADETECTED_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_CIM_MEDIADETECTED);
         Assert.IsTrue(spLine.Timestamp == "2022-12-07 15:58:02.209");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 14 - CRD - Ignore */
      [Ignore]
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_CRD_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_CRD_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_CRD_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-09-26 03:02:46.203");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 15 - BCR - Ignore */
      [Ignore]
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_BCR_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_BCR_STATUS);
         Assert.IsTrue(logLine is WFSDEVSTATUS);

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_BCR_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2022-09-26 03:02:46.203");
         Assert.IsTrue(spLine.HResult == "");
      }

      /* 16 - IPM */
      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_IPM_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_INF_IPM_STATUS);
         Assert.IsTrue(logLine is WFSIPMSTATUS);

         WFSIPMSTATUS spLine = (WFSIPMSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_IPM_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-02-05 10:55:41.409");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.fwDevice == "1");
         Assert.IsTrue(spLine.wAcceptor == "2");
         Assert.IsTrue(spLine.wMedia == "3");
         Assert.IsTrue(spLine.wStacker == "9");
         Assert.IsTrue(spLine.wMixedMode == "27");
         Assert.IsTrue(spLine.wAntiFraudModule == "28");
      }

      [TestMethod]
      public void SPLogHandler_IdentifyLines_WFS_INF_IPM_MEDIA_BIN_INFO()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_INF_IPM_MEDIA_BIN_INFO);
         Assert.IsTrue(logLine is WFSIPMMEDIABININFO);

         WFSIPMMEDIABININFO spLine = (WFSIPMMEDIABININFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);
         Assert.IsTrue(spLine.Timestamp == "2023-03-17 08:44:39.126");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.lUnitCount == 2);
         Assert.IsTrue(spLine.usBinNumbers[0] == "1");
         Assert.IsTrue(spLine.usBinNumbers[1] == "2");
         Assert.IsTrue(spLine.fwTypes[0] == "0x0002");
         Assert.IsTrue(spLine.fwTypes[1] == "0x0001");
         Assert.IsTrue(spLine.wMediaTypes[0] == "0x0003");
         Assert.IsTrue(spLine.wMediaTypes[1] == "0x0002");
         Assert.IsTrue(spLine.lpstrBinIDs[0] == "ALL");
         Assert.IsTrue(spLine.lpstrBinIDs[1] == "CHECK");
         Assert.IsTrue(spLine.ulMediaInCounts[0] == "4");
         Assert.IsTrue(spLine.ulMediaInCounts[1] == "11");
         Assert.IsTrue(spLine.ulCounts[0] == "5");
         Assert.IsTrue(spLine.ulCounts[1] == "12");
         Assert.IsTrue(spLine.ulRetractOperations[0] == "6");
         Assert.IsTrue(spLine.ulRetractOperations[1] == "13");
         Assert.IsTrue(spLine.ulMaximumItems[0] == "8");
         Assert.IsTrue(spLine.ulMaximumItems[1] == "15");
         Assert.IsTrue(spLine.ulMaximumRetractOperations[0] == "9");
         Assert.IsTrue(spLine.ulMaximumRetractOperations[1] == "16");
         Assert.IsTrue(spLine.usStatuses[0] == "10");
         Assert.IsTrue(spLine.usStatuses[1] == "17");
      }

      public void SPLogHandler_IdentifyLines_WFS_INF_IPM_TRANSACTION_STATUS()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_INF_IPM_TRANSACTION_STATUS);
         Assert.IsTrue(logLine is WFSIPMTRANSSTATUS);

         WFSIPMTRANSSTATUS spLine = (WFSIPMTRANSSTATUS)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_INF_IPM_TRANSACTION_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-02-05 10:55:41.409");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.wMediaInTransaction == "1");
         Assert.IsTrue(spLine.usMediaOnStacker == "2");
         Assert.IsTrue(spLine.usLastMediaInTotal == "3");
         Assert.IsTrue(spLine.usLastMediaAddedToStacker == "4");
         Assert.IsTrue(spLine.usTotalItems == "5");
         Assert.IsTrue(spLine.usTotalItemsRefused == "6");
         Assert.IsTrue(spLine.usTotalBunchesRefused == "7");
         Assert.IsTrue(spLine.lpszExtra == "Meaning=The Media-In transaction is active");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_MEDIA_IN()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_MEDIA_IN);
         Assert.IsTrue(logLine is WFSIPMMEDIAIN);

         WFSIPMMEDIAIN spLine = (WFSIPMMEDIAIN)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_MEDIA_IN);
         Assert.IsTrue(spLine.Timestamp == "2023-01-04 22:01:08.467");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.usMediaOnStacker == "0");
         Assert.IsTrue(spLine.usLastMedia == "3");
         Assert.IsTrue(spLine.usLastMediaOnStacker == "0");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_MEDIA_IN_END()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_MEDIA_END);
         Assert.IsTrue(logLine is WFSIPMMEDIAINEND);

         WFSIPMMEDIAINEND spLine = (WFSIPMMEDIAINEND)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_MEDIA_IN_END);
         Assert.IsTrue(spLine.Timestamp == "2023-01-04 11:41:46.296");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.usItemsReturned == "1");
         Assert.IsTrue(spLine.usItemsRefused == "2");
         Assert.IsTrue(spLine.usBunchesRefused == "3");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_MEDIA_IN_ROLLBACK()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_MEDIA_IN_ROLLBACK);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_MEDIA_IN_ROLLBACK);
         Assert.IsTrue(spLine.Timestamp == "2023-01-04 11:54:59.057");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_PRESENT_MEDIA()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_PRESENT_MEDIA);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_PRESENT_MEDIA);
         Assert.IsTrue(spLine.Timestamp == "2023-01-04 22:01:19.090");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_RETRACT_MEDIA()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_RETRACT_MEDIA);
         Assert.IsTrue(logLine is WFSIPMRETRACTMEDIAOUT);

         WFSIPMRETRACTMEDIAOUT spLine = (WFSIPMRETRACTMEDIAOUT)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_RETRACT_MEDIA);
         Assert.IsTrue(spLine.Timestamp == "2022-12-14 13:38:28.611");
         Assert.IsTrue(spLine.HResult == "-14");

         Assert.IsTrue(spLine.usMedia == "");
         Assert.IsTrue(spLine.wRetractLocation == "");
         Assert.IsTrue(spLine.usBinNumber == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_RETRACT_MEDIA_1()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_RETRACT_MEDIA_1);
         Assert.IsTrue(logLine is WFSIPMRETRACTMEDIAOUT);

         WFSIPMRETRACTMEDIAOUT spLine = (WFSIPMRETRACTMEDIAOUT)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_RETRACT_MEDIA);
         Assert.IsTrue(spLine.Timestamp == "2022-12-14 13:38:28.611");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.usMedia == "1");
         Assert.IsTrue(spLine.wRetractLocation == "0x0002");
         Assert.IsTrue(spLine.usBinNumber == "3");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_PRINT_TEXT()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_PRINT_TEXT);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_PRINT_TEXT);
         Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_RESET()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_RESET_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_PRINT_TEXT);
         Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
         Assert.IsTrue(spLine.HResult == "");
      }

      //public void SPLogHandler_IdentifyLines_WFS_CMD_IPM_EXPEL_MEDIA()
      //{
      //   ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
      //   ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_EXPEL_MEDIA);
      //   Assert.IsTrue(logLine is SPLine);

      //   SPLine spLine = (SPLine)logLine;
      //   Assert.IsTrue(spLine.xfsType == XFSType.WFS_CMD_IPM_EXPEL_MEDIA);
      //   Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
      //   Assert.IsTrue(spLine.HResult == "");
      //}

      public void SPLogHandler_IdentifyLines_WFS_EXEE_IPM_MEDIAINSERTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAINSERTED_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IPM_MEDIAINSERTED);
         Assert.IsTrue(spLine.Timestamp == "2023-08-07 14:25:34.211");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_USRE_IPM_MEDIABINTHRESHOLD()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_USRE_IPM_MEDIABINTHRESHOLD_1);
         Assert.IsTrue(logLine is WFSIPMMEDIABININFO);

         WFSIPMMEDIABININFO spLine = (WFSIPMMEDIABININFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_USRE_IPM_MEDIABINTHRESHOLD);
         Assert.IsTrue(spLine.Timestamp == "2023-09-20 15:16:53.879");
         Assert.IsTrue(spLine.HResult == "-1");

         Assert.IsTrue(spLine.lUnitCount == 1);
         Assert.IsTrue(spLine.usBinNumbers[0] == "1");
         Assert.IsTrue(spLine.fwTypes[0] == "0x0002"); ;
         Assert.IsTrue(spLine.wMediaTypes[0] == "0x0003");
         Assert.IsTrue(spLine.lpstrBinIDs[0] == "CHECK");
         Assert.IsTrue(spLine.ulMediaInCounts[0] == "4");
         Assert.IsTrue(spLine.ulCounts[0] == "5");
         Assert.IsTrue(spLine.ulRetractOperations[0] == "6");
         Assert.IsTrue(spLine.ulMaximumItems[0] == "8");
         Assert.IsTrue(spLine.ulMaximumRetractOperations[0] == "9");
         Assert.IsTrue(spLine.usStatuses[0] == "10");
      }

      public void SPLogHandler_IdentifyLines_WFS_SRVE_IPM_MEDIABININFOCHANGED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_SRVE_IPM_MEDIABININFOCHANGED_1);
         Assert.IsTrue(logLine is WFSIPMMEDIABININFO);

         WFSIPMMEDIABININFO spLine = (WFSIPMMEDIABININFO)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_IPM_MEDIABININFOCHANGED);
         Assert.IsTrue(spLine.Timestamp == "2023-08-07 14:26:09.889");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.lUnitCount == 1);
         Assert.IsTrue(spLine.usBinNumbers[0] == "1");
         Assert.IsTrue(spLine.fwTypes[0] == "0x0002"); ;
         Assert.IsTrue(spLine.wMediaTypes[0] == "0x0003");
         Assert.IsTrue(spLine.lpstrBinIDs[0] == "CHECK");
         Assert.IsTrue(spLine.ulMediaInCounts[0] == "4");
         Assert.IsTrue(spLine.ulCounts[0] == "5");
         Assert.IsTrue(spLine.ulRetractOperations[0] == "6");
         Assert.IsTrue(spLine.ulMaximumItems[0] == "8");
         Assert.IsTrue(spLine.ulMaximumRetractOperations[0] == "9");
         Assert.IsTrue(spLine.usStatuses[0] == "10");
      }

      //public void SPLogHandler_IdentifyLines_WFS_EXEE_IPM_MEDIABINERROR()
      //{
      //   ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
      //   ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIABINERROR);
      //   Assert.IsTrue(logLine is WFSIPMEDIABINERROR);

      //   WFSIPMEDIABINERROR spLine = (WFSIPMEDIABINERROR)logLine;
      //   Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IPM_MEDIABINERROR);
      //   Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
      //   Assert.IsTrue(spLine.HResult == "");
      //}

      public void SPLogHandler_IdentifyLines_WFS_SRVE_IPM_MEDIATAKEN()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_SRVE_IPM_MEDIATAKEN_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_IPM_MEDIATAKEN);
         Assert.IsTrue(spLine.Timestamp == "2023-09-06 08:37:38.126");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_SRVE_IPM_MEDIADETECTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_SRVE_IPM_MEDIADETECTED_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_SRVE_IPM_MEDIADETECTED);
         Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_EXEE_IPM_MEDIAPRESENTED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAPRESENTED_1);
         Assert.IsTrue(logLine is SPLine);

         SPLine spLine = (SPLine)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IPM_MEDIAPRESENTED);
         Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
         Assert.IsTrue(spLine.HResult == "");
      }

      public void SPLogHandler_IdentifyLines_WFS_EXEE_IPM_MEDIAREFUSED()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAREFUSED_1);
         Assert.IsTrue(logLine is WFSIPMMEDIAREFUSED);

         WFSIPMMEDIAREFUSED spLine = (WFSIPMMEDIAREFUSED)logLine;
         Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IPM_MEDIAREFUSED);
         Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
         Assert.IsTrue(spLine.HResult == "");

         Assert.IsTrue(spLine.wReason == "4");
         Assert.IsTrue(spLine.wMediaLocation == "2");
         Assert.IsTrue(spLine.bPresentRequired == "0");
      }

      //public void SPLogHandler_IdentifyLines_WFS_EXEE_IPM_MEDIAREJECTED()
      //{
      //   ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
      //   ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAREJECTED);
      //   Assert.IsTrue(logLine is SPLine);

      //   SPLine spLine = (SPLine)logLine;
      //   Assert.IsTrue(spLine.xfsType == XFSType.WFS_EXEE_IPM_MEDIAREJECTED);
      //   Assert.IsTrue(spLine.Timestamp == "2023-08-22 22:31:13.977");
      //   Assert.IsTrue(spLine.HResult == "");
      //}
   }
}
