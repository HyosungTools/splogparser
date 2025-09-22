using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using Samples;
using Contract;
using System;

namespace SPLogLineTests
{
   [TestClass]
   public class SPLogHandler_IdentifyLines_Tests
   {
      /* 1 - PTR Tests */
      [TestMethod]
      public void ShouldParseWfsInfPtrStatusCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFS_INF_PTR_STATUS);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual("2023-03-17 08:42:28.033", spLine.Timestamp,
             $"Expected Timestamp: 2023-03-17 08:42:28.033, Actual: {spLine.Timestamp}");
         Assert.AreEqual("1", spLine.HResult,
             $"Expected HResult: 1, Actual: {spLine.HResult}");
      }

      /* 2 - IDC Tests */
      [TestMethod]
      public void ShouldParseWfsInfIdcStatusCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_INF_IDC_STATUS);
         Assert.IsInstanceOfType(logLine, typeof(WFSIDCSTATUS),
             $"Expected type: WFSIDCSTATUS, Actual: {logLine.GetType()}");

         WFSIDCSTATUS spLine = (WFSIDCSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_INF_IDC_STATUS, spLine.xfsType,
             $"Expected xfsType: WFS_INF_IDC_STATUS, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-01-30 20:43:30.395", spLine.Timestamp,
             $"Expected Timestamp: 2023-01-30 20:43:30.395, Actual: {spLine.Timestamp}");
         Assert.AreEqual("1", spLine.HResult,
             $"Expected HResult: 1, Actual: {spLine.HResult}");
         Assert.AreEqual("2", spLine.fwDevice,
             $"Expected fwDevice: 2, Actual: {spLine.fwDevice}");
         Assert.AreEqual("3", spLine.fwMedia,
             $"Expected fwMedia: 3, Actual: {spLine.fwMedia}");
         Assert.AreEqual("4", spLine.fwRetainBin,
             $"Expected fwRetainBin: 4, Actual: {spLine.fwRetainBin}");
         Assert.AreEqual("5", spLine.fwSecurity,
             $"Expected fwSecurity: 5, Actual: {spLine.fwSecurity}");
         Assert.AreEqual("6", spLine.usCards,
             $"Expected usCards: 6, Actual: {spLine.usCards}");
         Assert.AreEqual("7", spLine.fwChipPower,
             $"Expected fwChipPower: 7, Actual: {spLine.fwChipPower}");
         Assert.AreEqual("8", spLine.fwChipModule,
             $"Expected fwChipModule: 8, Actual: {spLine.fwChipModule}");
         Assert.AreEqual("9", spLine.fwMagReadModule,
             $"Expected fwMagReadModule: 9, Actual: {spLine.fwMagReadModule}");
         Assert.AreEqual("0000000", spLine.ErrorCode,
             $"Expected ErrorCode: 0000000, Actual: {spLine.ErrorCode}");
         Assert.AreEqual("System OK!", spLine.Description,
             $"Expected Description: System OK!, Actual: {spLine.Description}");
      }

      [TestMethod]
      public void ShouldParseWfsInfIdcCapabilitiesCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_INF_IDC_CAPABILITIES);
         Assert.IsInstanceOfType(logLine, typeof(WFSIDCCAPABILITIES),
             $"Expected type: WFSIDCCAPABILITIES, Actual: {logLine.GetType()}");

         WFSIDCCAPABILITIES spLine = (WFSIDCCAPABILITIES)logLine;
         Assert.AreEqual(XFSType.WFS_INF_IDC_CAPABILITIES, spLine.xfsType,
             $"Expected xfsType: WFS_INF_IDC_CAPABILITIES, Actual: {spLine.xfsType}");
         Assert.AreEqual("2024-01-16 03:03:57.592", spLine.Timestamp,
             $"Expected Timestamp: 2024-01-16 03:03:57.592, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
         Assert.AreEqual("0x0005", spLine.fwType,
             $"Expected fwType: 0x0005, Actual: {spLine.fwType}");
         Assert.AreEqual("0", spLine.bCompound,
             $"Expected bCompound: 0, Actual: {spLine.bCompound}");
         Assert.AreEqual("0x0007", spLine.fwReadTracks,
             $"Expected fwReadTracks: 0x0007, Actual: {spLine.fwReadTracks}");
         Assert.AreEqual("0x0000", spLine.fwWriteTracks,
             $"Expected fwWriteTracks: 0x0000, Actual: {spLine.fwWriteTracks}");
         Assert.AreEqual("0x0003", spLine.fwChipProtocols,
             $"Expected fwChipProtocols: 0x0003, Actual: {spLine.fwChipProtocols}");
         Assert.AreEqual("0", spLine.usCards,
             $"Expected usCards: 0, Actual: {spLine.usCards}");
         Assert.AreEqual("1", spLine.fwSecType,
             $"Expected fwSecType: 1, Actual: {spLine.fwSecType}");
         Assert.AreEqual("1", spLine.fwPowerOnOption,
             $"Expected fwPowerOnOption: 1, Actual: {spLine.fwPowerOnOption}");
         Assert.AreEqual("1", spLine.fwPowerOffOption,
             $"Expected fwPowerOffOption: 1, Actual: {spLine.fwPowerOffOption}");
         Assert.AreEqual("0", spLine.bFluxSensorProgrammable,
             $"Expected bFluxSensorProgrammable: 0, Actual: {spLine.bFluxSensorProgrammable}");
         Assert.AreEqual("0", spLine.bReadWriteAccessFollowingEject,
             $"Expected bReadWriteAccessFollowingEject: 0, Actual: {spLine.bReadWriteAccessFollowingEject}");
         Assert.AreEqual("0", spLine.fwWriteMode,
             $"Expected fwWriteMode: 0, Actual: {spLine.fwWriteMode}");
         Assert.AreEqual("12", spLine.fwChipPower,
             $"Expected fwChipPower: 12, Actual: {spLine.fwChipPower}");
         Assert.AreEqual("AttemptMSReadFirst=0,XFS_MIB_VERSION=0x00000A01", spLine.lpszExtra2,
             $"Expected lpszExtra2: AttemptMSReadFirst=0,XFS_MIB_VERSION=0x00000A01, Actual: {spLine.lpszExtra2}");
         Assert.AreEqual("0", spLine.fwDIPMode,
             $"Expected fwDIPMode: 0, Actual: {spLine.fwDIPMode}");
         Assert.AreEqual("NULL", spLine.lpwMemoryChipProtocols,
             $"Expected lpwMemoryChipProtocols: NULL, Actual: {spLine.lpwMemoryChipProtocols}");
         Assert.AreEqual("0", spLine.fwEjectPosition,
             $"Expected fwEjectPosition: 0, Actual: {spLine.fwEjectPosition}");
         Assert.AreEqual("0", spLine.bPowerSaveControl,
             $"Expected bPowerSaveControl: 0, Actual: {spLine.bPowerSaveControl}");
         Assert.AreEqual("0", spLine.usParkingStations,
             $"Expected usParkingStations: 0, Actual: {spLine.usParkingStations}");
         Assert.AreEqual("0", spLine.bAntiFraudModule,
             $"Expected bAntiFraudModule: 0, Actual: {spLine.bAntiFraudModule}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIdcReadRawDataCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_CMD_IDC_READ_RAW_DATA);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IDC_READ_RAW_DATA, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IDC_READ_RAW_DATA, Actual: {spLine.xfsType}");
         Assert.AreEqual("2024-01-16 09:31:04.379", spLine.Timestamp,
             $"Expected Timestamp: 2024-01-16 09:31:04.379, Actual: {spLine.Timestamp}");
         Assert.AreEqual("-4", spLine.HResult,
             $"Expected HResult: -4, Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIdcChipIoCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_CMD_IDC_CHIP_IO);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IDC_CHIP_IO, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IDC_CHIP_IO, Actual: {spLine.xfsType}");
         Assert.AreEqual("2024-01-16 09:11:44.180", spLine.Timestamp,
             $"Expected Timestamp: 2024-01-16 09:11:44.180, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIdcChipPowerCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_CMD_IDC_CHIP_POWER);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IDC_CHIP_POWER, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IDC_CHIP_POWER, Actual: {spLine.xfsType}");
         Assert.AreEqual("2024-01-12 08:38:30.060", spLine.Timestamp,
             $"Expected Timestamp: 2024-01-12 08:38:30.060, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsExeeIdcMediaInsertedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_EXEE_IDC_MEDIAINSERTED);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_EXEE_IDC_MEDIAINSERTED, spLine.xfsType,
             $"Expected xfsType: WFS_EXEE_IDC_MEDIAINSERTED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2024-01-16 09:11:41.536", spLine.Timestamp,
             $"Expected Timestamp: 2024-01-16 09:11:41.536, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsSrveIdcMediaRemovedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_SRVE_IDC_MEDIAREMOVED);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_SRVE_IDC_MEDIAREMOVED, spLine.xfsType,
             $"Expected xfsType: WFS_SRVE_IDC_MEDIAREMOVED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2024-02-07 21:22:32.656", spLine.Timestamp,
             $"Expected Timestamp: 2024-02-07 21:22:32.656, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsUsreIdcRetainBinThresholdCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_USRE_IDC_RETAINBINTHRESHOLD);
         Assert.IsInstanceOfType(logLine, typeof(WFSIDCRETAINBINTHRESHOLD),
             $"Expected type: WFSIDCRETAINBINTHRESHOLD, Actual: {logLine.GetType()}");

         WFSIDCRETAINBINTHRESHOLD spLine = (WFSIDCRETAINBINTHRESHOLD)logLine;
         Assert.AreEqual(XFSType.WFS_USRE_IDC_RETAINBINTHRESHOLD, spLine.xfsType,
             $"Expected xfsType: WFS_USRE_IDC_RETAINBINTHRESHOLD, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-12-07 16:01:20.549", spLine.Timestamp,
             $"Expected Timestamp: 2023-12-07 16:01:20.549, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
         Assert.AreEqual("0", spLine.fwRetainBin,
             $"Expected fwRetainBin: 0, Actual: {spLine.fwRetainBin}");
      }

      [TestMethod]
      public void ShouldParseWfsExeeIdcInvalidMediaCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_EXEE_IDC_INVALIDMEDIA);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_EXEE_IDC_INVALIDMEDIA, spLine.xfsType,
             $"Expected xfsType: WFS_EXEE_IDC_INVALIDMEDIA, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-12-07 07:49:56.474", spLine.Timestamp,
             $"Expected Timestamp: 2023-12-07 07:49:56.474, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsExeeIdcMediaRetainedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_EXEE_IDC_MEDIARETAINED);
         Assert.IsInstanceOfType(logLine, typeof(WFSDEVSTATUS),
             $"Expected type: WFSDEVSTATUS, Actual: {logLine.GetType()}");

         WFSDEVSTATUS spLine = (WFSDEVSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_EXEE_IDC_MEDIARETAINED, spLine.xfsType,
             $"Expected xfsType: WFS_EXEE_IDC_MEDIARETAINED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2024-01-19 13:38:31.092", spLine.Timestamp,
             $"Expected Timestamp: 2024-01-19 13:38:31.092, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsSrveIdcMediaDetectedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_idc.WFS_SRVE_IDC_MEDIADETECTED);
         Assert.IsInstanceOfType(logLine, typeof(WFSIDCMEDIADETECTED),
             $"Expected type: WFSIDCMEDIADETECTED, Actual: {logLine.GetType()}");

         WFSIDCMEDIADETECTED spLine = (WFSIDCMEDIADETECTED)logLine;
         Assert.AreEqual(XFSType.WFS_SRVE_IDC_MEDIADETECTED, spLine.xfsType,
             $"Expected xfsType: WFS_SRVE_IDC_MEDIADETECTED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-12-23 18:02:15.631", spLine.Timestamp,
             $"Expected Timestamp: 2023-12-23 18:02:15.631, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
         Assert.AreEqual("3", spLine.lppwResetOut,
             $"Expected lppwResetOut: 3, Actual: {spLine.lppwResetOut}");
      }

      /* 3 - CDM Tests */
      [TestMethod]
      public void ShouldParseWfsInfCdmStatusCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_STATUS_1);
         Assert.IsInstanceOfType(logLine, typeof(WFSCDMSTATUS),
             $"Expected type: WFSCDMSTATUS, Actual: {logLine.GetType()}");

         WFSCDMSTATUS spLine = (WFSCDMSTATUS)logLine;
         Assert.AreEqual(XFSType.WFS_INF_CDM_STATUS, spLine.xfsType,
             $"Expected xfsType: WFS_INF_CDM_STATUS, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-01-24 00:59:14.663", spLine.Timestamp,
             $"Expected Timestamp: 2023-01-24 00:59:14.663, Actual: {spLine.Timestamp}");
         Assert.AreEqual("1", spLine.HResult,
             $"Expected HResult: 1, Actual: {spLine.HResult}");
         Assert.AreEqual("2", spLine.fwDevice,
             $"Expected fwDevice: 2, Actual: {spLine.fwDevice}");
         Assert.AreEqual("3", spLine.fwSafeDoor,
             $"Expected fwSafeDoor: 3, Actual: {spLine.fwSafeDoor}");
         Assert.AreEqual("4", spLine.fwDispenser,
             $"Expected fwDispenser: 4, Actual: {spLine.fwDispenser}");
         Assert.AreEqual("3", spLine.fwIntStacker,
             $"Expected fwIntStacker: 3, Actual: {spLine.fwIntStacker}");
         Assert.AreEqual("4", spLine.fwPosition,
             $"Expected fwPosition: 4, Actual: {spLine.fwPosition}");
         Assert.AreEqual("5", spLine.fwShutter,
             $"Expected fwShutter: 5, Actual: {spLine.fwShutter}");
         Assert.AreEqual("6", spLine.fwPositionStatus,
             $"Expected fwPositionStatus: 6, Actual: {spLine.fwPositionStatus}");
         Assert.AreEqual("7", spLine.fwTransport,
             $"Expected fwTransport: 7, Actual: {spLine.fwTransport}");
         Assert.AreEqual("8", spLine.fwTransportStatus,
             $"Expected fwTransportStatus: 8, Actual: {spLine.fwTransportStatus}");
         Assert.AreEqual("9", spLine.wDevicePosition,
             $"Expected wDevicePosition: 9, Actual: {spLine.wDevicePosition}");
         Assert.AreEqual("10", spLine.usPowerSaveRecoveryTime,
             $"Expected usPowerSaveRecoveryTime: 10, Actual: {spLine.usPowerSaveRecoveryTime}");
         Assert.AreEqual("11", spLine.wAntiFraudModule,
             $"Expected wAntiFraudModule: 11, Actual: {spLine.wAntiFraudModule}");
      }

      //  [TestMethod]
      //  public void ShouldParseWfsInfCdmCashUnitInfoCorrectly()
      //  {
      //     ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
      //     ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_1);
      //     Assert.IsInstanceOfType(logLine, typeof(WFSCDMCUINFO), $"Expected type: WFSCDMCUINFO, Actual: {logLine.GetType()}");

      //     WFSCDMCUINFO spLine = (WFSCDMCUINFO)logLine;
      //     Assert.AreEqual(XFSType.WFS_INF_CDM_CASH_UNIT_INFO, spLine.xfsType, $"Expected xfsType: WFS_INF_CDM_CASH_UNIT_INFO, Actual: {spLine.xfsType}");
      //     Assert.AreEqual(6, spLine.lUnitCount, $"Expected lUnitCount: 6, Actual: {spLine.lUnitCount}");
      //     Assert.IsFalse(spLine.IsTruncated, $"Expected bTruncated: false, Actual: {spLine.IsTruncated}");

      //     Console.WriteLine("usNumbers"); 
      //     Assert.AreEqual("1",  spLine.usNumbers[0], $"Expected usNumbers[0]: 1, Actual: {spLine.usNumbers[0]}");
      //     Assert.AreEqual("6",  spLine.usNumbers[5], $"Expected usNumbers[6]: 6, Actual: {spLine.usNumbers[5]}");
      //     Assert.AreEqual("6",  spLine.usTypes[0], $"Expected fwTypes[0]: 8, Actual: {spLine.usTypes[0]}");
      //     Assert.AreEqual("12", spLine.usTypes[5], $"Expected fwTypes[6]: 14, Actual: {spLine.usTypes[5]}");

      //     Assert.AreEqual("LCU00", spLine.cUnitIDs[0], $"Expected cUnitIDs[0]: LCU00, Actual: {spLine.cUnitIDs[0]}");
      //     Assert.AreEqual("LCU05", spLine.cUnitIDs[5], $"Expected cUnitIDs[6]: LCU05, Actual: {spLine.cUnitIDs[5]}");
      //     Assert.AreEqual("   ", spLine.cCurrencyIDs[0], $"Expected cCurrencyIDs[0]: '   ', Actual: {spLine.cCurrencyIDs[0]}");
      //     Assert.AreEqual("USD", spLine.cCurrencyIDs[5], $"Expected cCurrencyIDs[5]: USD, Actual: {spLine.cCurrencyIDs[5]}");
      //     Assert.AreEqual("0", spLine.ulValues[0], $"Expected ulValues[0]: 0, Actual: {spLine.ulValues[0]}");
      //     Assert.AreEqual("50", spLine.ulValues[5], $"Expected ulValues[6]: 50, Actual: {spLine.ulValues[6]}");
      //     Assert.AreEqual("0", spLine.ulInitialCounts[0], $"Expected ulCashInCounts[0]: 0, Actual: {spLine.ulInitialCounts[0]}");
      //     Assert.AreEqual("2000", spLine.ulInitialCounts[5], $"Expected ulCashInCounts[6]: 2000, Actual: {spLine.ulInitialCounts[5]}");
      //     Console.WriteLine("ulCounts");
      //     Assert.AreEqual("0", spLine.ulCounts[0], $"Expected ulCounts[0]: 0, Actual: {spLine.ulCounts[0]}");
      //     Assert.AreEqual("1336", spLine.ulCounts[5], $"Expected ulCounts[5]: 1336, Actual: {spLine.ulCounts[5]}");
      //     Assert.AreEqual("80", spLine.ulMaximums[0], $"Expected ulMaximums[0]: 80, Actual: {spLine.ulMaximums[0]}");
      //     Assert.AreEqual("0", spLine.ulMaximums[5], $"Expected ulMaximums[5]: 0, Actual: {spLine.ulMaximums[5]}");
      //     Assert.AreEqual("4", spLine.usStatuses[0], $"Expected usStatuses[0]: 4, Actual: {spLine.usStatuses[0]}");
      //     Assert.AreEqual("0", spLine.usStatuses[5], $"Expected usStatuses[5]: 0, Actual: {spLine.usStatuses[5]}");
      //     Console.WriteLine("ulInitialCounts");
      //     Assert.AreEqual("0", spLine.ulInitialCounts[0], $"Expected ulInitialCounts[0]: 0, Actual: {spLine.ulInitialCounts[0]}");
      //     Assert.AreEqual("2000", spLine.ulInitialCounts[5], $"Expected ulInitialCounts[5]: 2000, Actual: {spLine.ulInitialCounts[5]}");
      //     Assert.AreEqual("0", spLine.ulDispensedCounts[0], $"Expected ulDispensedCounts[0]: 0, Actual: {spLine.ulDispensedCounts[0]}");
      //     Assert.AreEqual("846", spLine.ulDispensedCounts[5], $"Expected ulDispensedCounts[5]: 846, Actual: {spLine.ulDispensedCounts[5]}");
      //     Assert.AreEqual("0", spLine.ulPresentedCounts[0], $"Expected ulPresentedCounts[0]: 0, Actual: {spLine.ulPresentedCounts[0]}");
      //     Assert.AreEqual("842", spLine.ulPresentedCounts[5], $"Expected ulPresentedCounts[5]: 842, Actual: {spLine.ulPresentedCounts[5]}");
      //     Assert.AreEqual("1", spLine.ulRetractedCounts[0], $"Expected ulRetractedCounts[0]: 1, Actual: {spLine.ulRetractedCounts[0]}");
      //     Assert.AreEqual("6", spLine.ulRetractedCounts[5], $"Expected ulRetractedCounts[5]: 6, Actual: {spLine.ulRetractedCounts[5]}");

      //     // Expected values for physical units (from lppPhysical-> section)
      //     var expectedPhysical = new[]
      //     {
      //    new { PositionName = "RetractCassette", UnitID = "RTCST", InitialCount = "0", Count = "0", RejectCount = "0", Maximum = "80", PStatus = "4", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "123" },
      //    new { PositionName = "RejectCassette", UnitID = "RJCST", InitialCount = "0", Count = "0", RejectCount = "0", Maximum = "210", PStatus = "4", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
      //    new { PositionName = "CassetteA", UnitID = "CST_A", InitialCount = "2000", Count = "1994", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "43", Presented = "43", Retracted = "0" },
      //    new { PositionName = "CassetteB", UnitID = "CST_B", InitialCount = "2000", Count = "1797", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "219", Presented = "215", Retracted = "0" },
      //    new { PositionName = "CassetteC", UnitID = "CST_C", InitialCount = "2000", Count = "2158", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "194", Presented = "190", Retracted = "0" },
      //    new { PositionName = "CassetteD", UnitID = "CST_D", InitialCount = "2000", Count = "1336", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "846", Presented = "842", Retracted = "456" }
      //};

      //     for (int i = 0; i < spLine.listPhysical.Count; i++)
      //     {
      //        // Verify physical unit fields
      //        var pcu = spLine.listPhysical[i];
      //        Assert.AreEqual(expectedPhysical[i].PositionName, pcu.lpPhysicalPositionName, $"listPhysical[{i}].lpPhysicalPositionName mismatch");
      //        Assert.AreEqual(expectedPhysical[i].UnitID, pcu.cUnitID, $"listPhysical[{i}].cUnitID mismatch");
      //        Assert.AreEqual(expectedPhysical[i].InitialCount, pcu.ulInitialCount, $"listPhysical[{i}].ulInitialCount mismatch");
      //        Assert.AreEqual(expectedPhysical[i].Count, pcu.ulCount, $"listPhysical[{i}].ulCount mismatch");
      //        Assert.AreEqual(expectedPhysical[i].RejectCount, pcu.ulRejectCount, $"listPhysical[{i}].ulRejectCount mismatch");
      //        Assert.AreEqual(expectedPhysical[i].Maximum, pcu.ulMaximum, $"listPhysical[{i}].ulMaximum mismatch");
      //        Assert.AreEqual(expectedPhysical[i].PStatus, pcu.usPStatus, $"listPhysical[{i}].usPStatus mismatch");
      //        Assert.AreEqual(expectedPhysical[i].HardwareSensor, pcu.bHardwareSensor, $"listPhysical[{i}].bHardwareSensor mismatch");
      //        Assert.AreEqual(expectedPhysical[i].Dispensed, pcu.ulDispensedCount, $"listPhysical[{i}].ulDispensedCount mismatch");
      //        Assert.AreEqual(expectedPhysical[i].Presented, pcu.ulPresentedCount, $"listPhysical[{i}].ulPresentedCount mismatch");
      //        Assert.AreEqual(expectedPhysical[i].Retracted, pcu.ulRetractedCount, $"listPhysical[{i}].ulRetractedCount mismatch");
      //     }

      //  }

      /* 4 - IPM Tests */
      [TestMethod]
      public void ShouldParseWfsCmdIpmMediaInRollbackCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_MEDIA_IN_ROLLBACK);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IPM_MEDIA_IN_ROLLBACK, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IPM_MEDIA_IN_ROLLBACK, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-01-04 11:54:59.057", spLine.Timestamp,
             $"Expected Timestamp: 2023-01-04 11:54:59.057, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIpmPresentMediaCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_PRESENT_MEDIA);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IPM_PRESENT_MEDIA, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IPM_PRESENT_MEDIA, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-01-04 22:01:19.090", spLine.Timestamp,
             $"Expected Timestamp: 2023-01-04 22:01:19.090, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIpmRetractMediaCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_RETRACT_MEDIA);
         Assert.IsInstanceOfType(logLine, typeof(WFSIPMRETRACTMEDIAOUT),
             $"Expected type: WFSIPMRETRACTMEDIAOUT, Actual: {logLine.GetType()}");

         WFSIPMRETRACTMEDIAOUT spLine = (WFSIPMRETRACTMEDIAOUT)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IPM_RETRACT_MEDIA, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IPM_RETRACT_MEDIA, Actual: {spLine.xfsType}");
         Assert.AreEqual("2022-12-14 13:38:28.611", spLine.Timestamp,
             $"Expected Timestamp: 2022-12-14 13:38:28.611, Actual: {spLine.Timestamp}");
         Assert.AreEqual("-14", spLine.HResult,
             $"Expected HResult: -14, Actual: {spLine.HResult}");
         //Assert.AreEqual("", spLine.usMedia,
         //    $"Expected usMedia: '', Actual: {spLine.usMedia}");
         //Assert.AreEqual("", spLine.wRetractLocation,
         //    $"Expected wRetractLocation: '', Actual: {spLine.wRetractLocation}");
         //Assert.AreEqual("", spLine.usBinNumber,
         //    $"Expected usBinNumber: '', Actual: {spLine.usBinNumber}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIpmRetractMedia1Correctly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_RETRACT_MEDIA_1);
         Assert.IsInstanceOfType(logLine, typeof(WFSIPMRETRACTMEDIAOUT),
             $"Expected type: WFSIPMRETRACTMEDIAOUT, Actual: {logLine.GetType()}");

         WFSIPMRETRACTMEDIAOUT spLine = (WFSIPMRETRACTMEDIAOUT)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IPM_RETRACT_MEDIA, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IPM_RETRACT_MEDIA, Actual: {spLine.xfsType}");
         Assert.AreEqual("2022-12-20 12:47:46.783", spLine.Timestamp,
             $"Expected Timestamp: 2022-12-14 13:38:28.611, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
         Assert.AreEqual("1", spLine.usMedia,
             $"Expected usMedia: 1, Actual: {spLine.usMedia}");
         Assert.AreEqual("0x0002", spLine.wRetractLocation,
             $"Expected wRetractLocation: 0x0002, Actual: {spLine.wRetractLocation}");
         Assert.AreEqual("3", spLine.usBinNumber,
             $"Expected usBinNumber: 3, Actual: {spLine.usBinNumber}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIpmPrintTextCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_PRINT_TEXT);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IPM_PRINT_TEXT, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IPM_PRINT_TEXT, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-08-22 22:31:13.977", spLine.Timestamp,
             $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsCmdIpmResetCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_RESET_1);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_CMD_IPM_RESET, spLine.xfsType,
             $"Expected xfsType: WFS_CMD_IPM_RESET, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-08-11 18:16:44.674", spLine.Timestamp,
             $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      //[TestMethod]
      //public void ShouldParseWfsCmdIpmExpelMediaCorrectly()
      //{
      //   ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
      //   ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_CMD_IPM_EXPEL_MEDIA);
      //   Assert.IsInstanceOfType(logLine, typeof(SPLine),
      //       $"Expected type: SPLine, Actual: {logLine.GetType()}");

      //   SPLine spLine = (SPLine)logLine;
      //   Assert.AreEqual(XFSType.WFS_CMD_IPM_EXPEL_MEDIA, spLine.xfsType,
      //       $"Expected xfsType: WFS_CMD_IPM_EXPEL_MEDIA, Actual: {spLine.xfsType}");
      //   Assert.AreEqual("2023-08-22 22:31:13.977", spLine.Timestamp,
      //       $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
      //   Assert.AreEqual("", spLine.HResult,
      //       $"Expected HResult: '', Actual: {spLine.HResult}");
      //}

      [TestMethod]
      public void ShouldParseWfsExeeIpmMediaInsertedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAINSERTED_1);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_EXEE_IPM_MEDIAINSERTED, spLine.xfsType,
             $"Expected xfsType: WFS_EXEE_IPM_MEDIAINSERTED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-08-07 14:25:34.211", spLine.Timestamp,
             $"Expected Timestamp: 2023-08-07 14:25:34.211, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsUsreIpmMediaBinThresholdCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_USRE_IPM_MEDIABINTHRESHOLD_1);
         Assert.IsInstanceOfType(logLine, typeof(WFSIPMMEDIABININFO),
             $"Expected type: WFSIPMMEDIABININFO, Actual: {logLine.GetType()}");

         WFSIPMMEDIABININFO spLine = (WFSIPMMEDIABININFO)logLine;
         Assert.AreEqual(XFSType.WFS_USRE_IPM_MEDIABINTHRESHOLD, spLine.xfsType,
             $"Expected xfsType: WFS_USRE_IPM_MEDIABINTHRESHOLD, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-09-20 15:16:53.879", spLine.Timestamp,
             $"Expected Timestamp: 2023-09-20 15:16:53.879, Actual: {spLine.Timestamp}");
         Assert.AreEqual("-1", spLine.HResult,
             $"Expected HResult: -1, Actual: {spLine.HResult}");
         Assert.AreEqual(1, spLine.lUnitCount,
             $"Expected lUnitCount: 1, Actual: {spLine.lUnitCount}");
         Assert.AreEqual("1", spLine.usBinNumbers[0],
             $"Expected usBinNumbers[0]: 1, Actual: {spLine.usBinNumbers[0]}");
         Assert.AreEqual("0x0002", spLine.fwTypes[0],
             $"Expected fwTypes[0]: 0x0002, Actual: {spLine.fwTypes[0]}");
         Assert.AreEqual("0x0003", spLine.wMediaTypes[0],
             $"Expected wMediaTypes[0]: 0x0003, Actual: {spLine.wMediaTypes[0]}");
         Assert.AreEqual("CHECK", spLine.lpstrBinIDs[0],
             $"Expected lpstrBinIDs[0]: CHECK, Actual: {spLine.lpstrBinIDs[0]}");
         Assert.AreEqual("4", spLine.ulMediaInCounts[0],
             $"Expected ulMediaInCounts[0]: 4, Actual: {spLine.ulMediaInCounts[0]}");
         Assert.AreEqual("5", spLine.ulCounts[0],
             $"Expected ulCounts[0]: 5, Actual: {spLine.ulCounts[0]}");
         Assert.AreEqual("6", spLine.ulRetractOperations[0],
             $"Expected ulRetractOperations[0]: 6, Actual: {spLine.ulRetractOperations[0]}");
         Assert.AreEqual("8", spLine.ulMaximumItems[0],
             $"Expected ulMaximumItems[0]: 8, Actual: {spLine.ulMaximumItems[0]}");
         Assert.AreEqual("9", spLine.ulMaximumRetractOperations[0],
             $"Expected ulMaximumRetractOperations[0]: 9, Actual: {spLine.ulMaximumRetractOperations[0]}");
         Assert.AreEqual("10", spLine.usStatuses[0],
             $"Expected usStatuses[0]: 10, Actual: {spLine.usStatuses[0]}");
      }

      [TestMethod]
      public void ShouldParseWfsSrveIpmMediaBinInfoChangedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_SRVE_IPM_MEDIABININFOCHANGED_1);
         Assert.IsInstanceOfType(logLine, typeof(WFSIPMMEDIABININFO),
             $"Expected type: WFSIPMMEDIABININFO, Actual: {logLine.GetType()}");

         WFSIPMMEDIABININFO spLine = (WFSIPMMEDIABININFO)logLine;
         Assert.AreEqual(XFSType.WFS_SRVE_IPM_MEDIABININFOCHANGED, spLine.xfsType,
             $"Expected xfsType: WFS_SRVE_IPM_MEDIABININFOCHANGED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-08-07 14:26:09.889", spLine.Timestamp,
             $"Expected Timestamp: 2023-08-07 14:26:09.889, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
         Assert.AreEqual(1, spLine.lUnitCount,
             $"Expected lUnitCount: 1, Actual: {spLine.lUnitCount}");
         Assert.AreEqual("1", spLine.usBinNumbers[0],
             $"Expected usBinNumbers[0]: 1, Actual: {spLine.usBinNumbers[0]}");
         Assert.AreEqual("0x0002", spLine.fwTypes[0],
             $"Expected fwTypes[0]: 0x0002, Actual: {spLine.fwTypes[0]}");
         Assert.AreEqual("0x0003", spLine.wMediaTypes[0],
             $"Expected wMediaTypes[0]: 0x0003, Actual: {spLine.wMediaTypes[0]}");
         Assert.AreEqual("CHECK", spLine.lpstrBinIDs[0],
             $"Expected lpstrBinIDs[0]: CHECK, Actual: {spLine.lpstrBinIDs[0]}");
         Assert.AreEqual("4", spLine.ulMediaInCounts[0],
             $"Expected ulMediaInCounts[0]: 4, Actual: {spLine.ulMediaInCounts[0]}");
         Assert.AreEqual("5", spLine.ulCounts[0],
             $"Expected ulCounts[0]: 5, Actual: {spLine.ulCounts[0]}");
         Assert.AreEqual("6", spLine.ulRetractOperations[0],
             $"Expected ulRetractOperations[0]: 6, Actual: {spLine.ulRetractOperations[0]}");
         Assert.AreEqual("8", spLine.ulMaximumItems[0],
             $"Expected ulMaximumItems[0]: 8, Actual: {spLine.ulMaximumItems[0]}");
         Assert.AreEqual("9", spLine.ulMaximumRetractOperations[0],
             $"Expected ulMaximumRetractOperations[0]: 9, Actual: {spLine.ulMaximumRetractOperations[0]}");
         Assert.AreEqual("10", spLine.usStatuses[0],
             $"Expected usStatuses[0]: 10, Actual: {spLine.usStatuses[0]}");
      }

      //[TestMethod]
      //public void ShouldParseWfsExeeIpmMediaBinErrorCorrectly()
      //{
      //   ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
      //   ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIABINERROR);
      //   Assert.IsInstanceOfType(logLine, typeof(WFSIPMEDIABINERROR),
      //       $"Expected type: WFSIPMEDIABINERROR, Actual: {logLine.GetType()}");

      //   WFSIPMEDIABINERROR spLine = (WFSIPMEDIABINERROR)logLine;
      //   Assert.AreEqual(XFSType.WFS_EXEE_IPM_MEDIABINERROR, spLine.xfsType,
      //       $"Expected xfsType: WFS_EXEE_IPM_MEDIABINERROR, Actual: {spLine.xfsType}");
      //   Assert.AreEqual("2023-08-22 22:31:13.977", spLine.Timestamp,
      //       $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
      //   Assert.AreEqual("", spLine.HResult,
      //       $"Expected HResult: '', Actual: {spLine.HResult}");
      //}

      [TestMethod]
      public void ShouldParseWfsSrveIpmMediaTakenCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_SRVE_IPM_MEDIATAKEN_1);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_SRVE_IPM_MEDIATAKEN, spLine.xfsType,
             $"Expected xfsType: WFS_SRVE_IPM_MEDIATAKEN, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-09-06 08:37:38.126", spLine.Timestamp,
             $"Expected Timestamp: 2023-09-06 08:37:38.126, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsSrveIpmMediaDetectedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_SRVE_IPM_MEDIADETECTED_1);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_SRVE_IPM_MEDIADETECTED, spLine.xfsType,
             $"Expected xfsType: WFS_SRVE_IPM_MEDIADETECTED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2025-06-03 09:48:30.901", spLine.Timestamp,
             $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsExeeIpmMediaPresentedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAPRESENTED_1);
         Assert.IsInstanceOfType(logLine, typeof(SPLine),
             $"Expected type: SPLine, Actual: {logLine.GetType()}");

         SPLine spLine = (SPLine)logLine;
         Assert.AreEqual(XFSType.WFS_EXEE_IPM_MEDIAPRESENTED, spLine.xfsType,
             $"Expected xfsType: WFS_EXEE_IPM_MEDIAPRESENTED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-10-19 14:21:56.220", spLine.Timestamp,
             $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult,
             $"Expected HResult: '', Actual: {spLine.HResult}");
      }

      [TestMethod]
      public void ShouldParseWfsExeeIpmMediaRefusedCorrectly()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAREFUSED_1);
         Assert.IsInstanceOfType(logLine, typeof(WFSIPMMEDIAREFUSED), $"Expected type: WFSIPMMEDIAREFUSED, Actual: {logLine.GetType()}");

         WFSIPMMEDIAREFUSED spLine = (WFSIPMMEDIAREFUSED)logLine;
         Assert.AreEqual(XFSType.WFS_EXEE_IPM_MEDIAREFUSED, spLine.xfsType, $"Expected xfsType: WFS_EXEE_IPM_MEDIAREFUSED, Actual: {spLine.xfsType}");
         Assert.AreEqual("2023-09-06 13:29:33.919", spLine.Timestamp, $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
         Assert.AreEqual("", spLine.HResult, $"Expected HResult: '', Actual: {spLine.HResult}");
         Assert.AreEqual("4", spLine.wReason, $"Expected wReason: 4, Actual: {spLine.wReason}");
         Assert.AreEqual("2", spLine.wMediaLocation, $"Expected wMediaLocation: 2, Actual: {spLine.wMediaLocation}");
         Assert.AreEqual("0", spLine.bPresentRequired, $"Expected bPresentRequired: 0, Actual: {spLine.bPresentRequired}");
      }

      //[TestMethod]
      //public void ShouldParseWfsExeeIpmMediaRejectedCorrectly()
      //{
      //   ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
      //   ILogLine logLine = logFileHandler.IdentifyLine(samples_ipm.WFS_EXEE_IPM_MEDIAREJECTED);
      //   Assert.IsInstanceOfType(logLine, typeof(SPLine),
      //       $"Expected type: SPLine, Actual: {logLine.GetType()}");

      //   SPLine spLine = (SPLine)logLine;
      //   Assert.AreEqual(XFSType.WFS_EXEE_IPM_MEDIAREJECTED, spLine.xfsType,
      //       $"Expected xfsType: WFS_EXEE_IPM_MEDIAREJECTED, Actual: {spLine.xfsType}");
      //   Assert.AreEqual("2023-08-22 22:31:13.977", spLine.Timestamp,
      //       $"Expected Timestamp: 2023-08-22 22:31:13.977, Actual: {spLine.Timestamp}");
      //   Assert.AreEqual("", spLine.HResult,
      //       $"Expected HResult: '', Actual: {spLine.HResult}");
      //}

      [TestMethod]
      public void WFS_INF_CDM_CASH_UNIT_INFO_1()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_1);
         Assert.IsTrue(logLine is WFSCDMCUINFO);

         WFSCDMCUINFO info = (WFSCDMCUINFO)logLine;

         // Verify logical unit count and truncation status
         Assert.AreEqual(6, info.lUnitCount, "Expected 6 logical cash units");
         Assert.IsFalse(info.IsTruncated, "Log should not be truncated");

         // Expected values for logical units (from WFS_INF_CDM_CASH_UNIT_INFO_1, no physical data)
         var expectedLogical = new[]
         {
        new { Number = "1", Type = "6", UnitID = "LCU00", Currency = "   ", Value = "0", InitialCount = "0", Count = "0", RejectCount = "1", Minimum = "0", Maximum = "80", Status = "4", Dispensed = "0", Presented = "0", Retracted = "1" },
        new { Number = "2", Type = "2", UnitID = "LCU01", Currency = "   ", Value = "0", InitialCount = "0", Count = "0", RejectCount = "2", Minimum = "0", Maximum = "210", Status = "4", Dispensed = "0", Presented = "0", Retracted = "2" },
        new { Number = "3", Type = "12", UnitID = "LCU02", Currency = "USD", Value = "1", InitialCount = "2000", Count = "1994", RejectCount = "3", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "43", Presented = "43", Retracted = "3" },
        new { Number = "4", Type = "12", UnitID = "LCU03", Currency = "USD", Value = "5", InitialCount = "2000", Count = "1797", RejectCount = "4", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "219", Presented = "215", Retracted = "4" },
        new { Number = "5", Type = "12", UnitID = "LCU04", Currency = "USD", Value = "20", InitialCount = "2000", Count = "2158", RejectCount = "5", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "194", Presented = "190", Retracted = "5" },
        new { Number = "6", Type = "12", UnitID = "LCU05", Currency = "USD", Value = "50", InitialCount = "2000", Count = "1336", RejectCount = "6", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "846", Presented = "842", Retracted = "6" }
    };

         // Expected values for physical units (from lppPhysical-> section)
         var expectedPhysical = new[]
         {
        new { PositionName = "RetractCassette", UnitID = "RTCST", InitialCount = "0", Count = "0", RejectCount = "0", Maximum = "80", PStatus = "4", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "123" },
        new { PositionName = "RejectCassette", UnitID = "RJCST", InitialCount = "0", Count = "0", RejectCount = "0", Maximum = "210", PStatus = "4", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { PositionName = "CassetteA", UnitID = "CST_A", InitialCount = "2000", Count = "1994", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "43", Presented = "43", Retracted = "0" },
        new { PositionName = "CassetteB", UnitID = "CST_B", InitialCount = "2000", Count = "1797", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "219", Presented = "215", Retracted = "0" },
        new { PositionName = "CassetteC", UnitID = "CST_C", InitialCount = "2000", Count = "2158", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "194", Presented = "190", Retracted = "0" },
        new { PositionName = "CassetteD", UnitID = "CST_D", InitialCount = "2000", Count = "1336", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "846", Presented = "842", Retracted = "456" }
    };

         // Verify logical unit fields
         for (int i = 0; i < info.lUnitCount; i++)
         {
            Assert.AreEqual(expectedLogical[i].Number, info.usNumbers[i], $"usNumbers[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Type, info.usTypes[i], $"usTypes[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].UnitID, info.cUnitIDs[i], $"cUnitIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Currency, info.cCurrencyIDs[i], $"cCurrencyIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Value, info.ulValues[i], $"ulValues[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].InitialCount, info.ulInitialCounts[i], $"ulInitialCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Count, info.ulCounts[i], $"ulCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].RejectCount, info.ulRejectCounts[i], $"ulRejectCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Minimum, info.ulMinimums[i], $"ulMinimums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Maximum, info.ulMaximums[i], $"ulMaximums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Status, info.usStatuses[i], $"usStatuses[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Dispensed, info.ulDispensedCounts[i], $"ulDispensedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Presented, info.ulPresentedCounts[i], $"ulPresentedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Retracted, info.ulRetractedCounts[i], $"ulRetractedCounts[{i}] mismatch");
         }

         for (int i = 0; i < info.listPhysical.Count; i++)
         {
            // Verify physical unit fields
            var pcu = info.listPhysical[i];
            Assert.AreEqual(expectedPhysical[i].PositionName, pcu.lpPhysicalPositionName, $"listPhysical[{i}].lpPhysicalPositionName mismatch");
            Assert.AreEqual(expectedPhysical[i].UnitID, pcu.cUnitID, $"listPhysical[{i}].cUnitID mismatch");
            Assert.AreEqual(expectedPhysical[i].InitialCount, pcu.ulInitialCount, $"listPhysical[{i}].ulInitialCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Count, pcu.ulCount, $"listPhysical[{i}].ulCount mismatch");
            Assert.AreEqual(expectedPhysical[i].RejectCount, pcu.ulRejectCount, $"listPhysical[{i}].ulRejectCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Maximum, pcu.ulMaximum, $"listPhysical[{i}].ulMaximum mismatch");
            Assert.AreEqual(expectedPhysical[i].PStatus, pcu.usPStatus, $"listPhysical[{i}].usPStatus mismatch");
            Assert.AreEqual(expectedPhysical[i].HardwareSensor, pcu.bHardwareSensor, $"listPhysical[{i}].bHardwareSensor mismatch");
            Assert.AreEqual(expectedPhysical[i].Dispensed, pcu.ulDispensedCount, $"listPhysical[{i}].ulDispensedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Presented, pcu.ulPresentedCount, $"listPhysical[{i}].ulPresentedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Retracted, pcu.ulRetractedCount, $"listPhysical[{i}].ulRetractedCount mismatch");
         }
      }

      [TestMethod]
      public void WFS_INF_CDM_CASH_UNIT_INFO_2()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_2);
         Assert.IsTrue(logLine is WFSCDMCUINFO);

         WFSCDMCUINFO info = (WFSCDMCUINFO)logLine;

         // Verify logical unit count and truncation status
         Assert.AreEqual(6, info.lUnitCount, "Expected 6 logical cash units");
         Assert.IsFalse(info.IsTruncated, "Log should not be truncated");

         // Expected values for logical units (from WFS_INF_CDM_CASH_UNIT_INFO_2)
         var expectedLogical = new[]
         {
        new { Number = "1", Type = "6", UnitID = "LCU00", Currency = "   ", Value = "0", InitialCount = "0", Count = "0", RejectCount = "0", Minimum = "0", Maximum = "80", Status = "4", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { Number = "2", Type = "2", UnitID = "LCU01", Currency = "   ", Value = "0", InitialCount = "0", Count = "2", RejectCount = "2", Minimum = "0", Maximum = "210", Status = "0", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { Number = "3", Type = "12", UnitID = "LCU02", Currency = "USD", Value = "1", InitialCount = "1400", Count = "1334", RejectCount = "1", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "101", Presented = "95", Retracted = "0" },
        new { Number = "4", Type = "12", UnitID = "LCU03", Currency = "USD", Value = "5", InitialCount = "1400", Count = "1336", RejectCount = "0", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "78", Presented = "75", Retracted = "0" },
        new { Number = "5", Type = "12", UnitID = "LCU04", Currency = "USD", Value = "20", InitialCount = "1400", Count = "1362", RejectCount = "1", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "187", Presented = "182", Retracted = "0" },
        new { Number = "6", Type = "12", UnitID = "LCU05", Currency = "USD", Value = "100", InitialCount = "1400", Count = "1342", RejectCount = "0", Minimum = "0", Maximum = "0", Status = "0", Dispensed = "87", Presented = "85", Retracted = "0" }
    };

         // Verify logical unit fields
         for (int i = 0; i < info.lUnitCount; i++)
         {
            Assert.AreEqual(expectedLogical[i].Number, info.usNumbers[i], $"usNumbers[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Type, info.usTypes[i], $"usTypes[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].UnitID, info.cUnitIDs[i], $"cUnitIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Currency, info.cCurrencyIDs[i], $"cCurrencyIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Value, info.ulValues[i], $"ulValues[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].InitialCount, info.ulInitialCounts[i], $"ulInitialCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Count, info.ulCounts[i], $"ulCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].RejectCount, info.ulRejectCounts[i], $"ulRejectCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Minimum, info.ulMinimums[i], $"ulMinimums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Maximum, info.ulMaximums[i], $"ulMaximums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Status, info.usStatuses[i], $"usStatuses[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Dispensed, info.ulDispensedCounts[i], $"ulDispensedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Presented, info.ulPresentedCounts[i], $"ulPresentedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Retracted, info.ulRetractedCounts[i], $"ulRetractedCounts[{i}] mismatch");
         }

         // Expected values for physical units (from lppPhysical-> section)
         var expectedPhysical = new[]
         {
        new { PositionName = "RetractCassette", UnitID = "RTCST", InitialCount = "0", Count = "0", RejectCount = "0", Maximum = "80", PStatus = "4", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { PositionName = "RejectCassette", UnitID = "RJCST", InitialCount = "0", Count = "2", RejectCount = "2", Maximum = "210", PStatus = "0", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { PositionName = "CassetteA", UnitID = "CST_A", InitialCount = "1400", Count = "1334", RejectCount = "1", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "101", Presented = "95", Retracted = "0" },
        new { PositionName = "CassetteB", UnitID = "CST_B", InitialCount = "1400", Count = "1336", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "78", Presented = "75", Retracted = "0" },
        new { PositionName = "CassetteC", UnitID = "CST_C", InitialCount = "1400", Count = "1362", RejectCount = "1", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "187", Presented = "182", Retracted = "0" },
        new { PositionName = "CassetteD", UnitID = "CST_D", InitialCount = "1400", Count = "1342", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "87", Presented = "85", Retracted = "0" }
    };

         for (int i = 0; i < info.listPhysical.Count; i++)
         {
            // Verify physical unit fields
            var pcu = info.listPhysical[i];
            Assert.AreEqual(expectedPhysical[i].PositionName, pcu.lpPhysicalPositionName, $"listPhysical[{i}].lpPhysicalPositionName mismatch");
            Assert.AreEqual(expectedPhysical[i].UnitID, pcu.cUnitID, $"listPhysical[{i}].cUnitID mismatch");
            Assert.AreEqual(expectedPhysical[i].InitialCount, pcu.ulInitialCount, $"listPhysical[{i}].ulInitialCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Count, pcu.ulCount, $"listPhysical[{i}].ulCount mismatch");
            Assert.AreEqual(expectedPhysical[i].RejectCount, pcu.ulRejectCount, $"listPhysical[{i}].ulRejectCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Maximum, pcu.ulMaximum, $"listPhysical[{i}].ulMaximum mismatch");
            Assert.AreEqual(expectedPhysical[i].PStatus, pcu.usPStatus, $"listPhysical[{i}].usPStatus mismatch");
            Assert.AreEqual(expectedPhysical[i].HardwareSensor, pcu.bHardwareSensor, $"listPhysical[{i}].bHardwareSensor mismatch");
            Assert.AreEqual(expectedPhysical[i].Dispensed, pcu.ulDispensedCount, $"listPhysical[{i}].ulDispensedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Presented, pcu.ulPresentedCount, $"listPhysical[{i}].ulPresentedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Retracted, pcu.ulRetractedCount, $"listPhysical[{i}].ulRetractedCount mismatch");
         }
      }

      [TestMethod]
      public void WFS_INF_CDM_CASH_UNIT_INFO_3()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_3);
         Assert.IsTrue(logLine is WFSCDMCUINFO, "Log line should be identified as WFSCDMCUINFO");

         WFSCDMCUINFO info = (WFSCDMCUINFO)logLine;

         // Verify logical unit count and truncation status
         Assert.AreEqual(5, info.lUnitCount, "Expected 6 logical cash units from here 'usCount = [6]'");
         Assert.IsTrue(info.IsTruncated, "Log should be marked as truncated due to '......(More Data)......'");

         // Expected values for logical units (from WFS_INF_CDM_CASH_UNIT_INFO_3)
         var expectedLogical = new[]
         {
        new { Number = "1", Type = "6", UnitID = "LCU00", Currency = "   ", Value = "0", InitialCount = "0", Count = "0", RejectCount = "0", Minimum = "0", Maximum = "400", Status = "4", NumPhysicalCUs = "2", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { Number = "2", Type = "2", UnitID = "LCU01", Currency = "   ", Value = "0", InitialCount = "0", Count = "36", RejectCount = "36", Minimum = "0", Maximum = "0", Status = "0", NumPhysicalCUs = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { Number = "3", Type = "12", UnitID = "LCU02", Currency = "USD", Value = "1", InitialCount = "1092", Count = "1044", RejectCount = "0", Minimum = "0", Maximum = "0", Status = "0", NumPhysicalCUs = "1", Dispensed = "306", Presented = "106", Retracted = "0" },
        new { Number = "4", Type = "12", UnitID = "LCU03", Currency = "USD", Value = "5", InitialCount = "1874", Count = "1690", RejectCount = "14", Minimum = "0", Maximum = "0", Status = "0", NumPhysicalCUs = "1", Dispensed = "205", Presented = "191", Retracted = "0" },
        new { Number = "5", Type = "12", UnitID = "LCU04", Currency = "USD", Value = "20", InitialCount = "1237", Count = "1052", RejectCount = "0", Minimum = "0", Maximum = "0", Status = "0", NumPhysicalCUs = "1", Dispensed = "252", Presented = "252", Retracted = "0" }
    };

         // Verify logical unit fields
         for (int i = 0; i < info.lUnitCount; i++)
         {
            Assert.AreEqual(expectedLogical[i].Number, info.usNumbers[i], $"usNumbers[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Type, info.usTypes[i], $"usTypes[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].UnitID, info.cUnitIDs[i], $"cUnitIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Currency, info.cCurrencyIDs[i], $"cCurrencyIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Value, info.ulValues[i], $"ulValues[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].InitialCount, info.ulInitialCounts[i], $"ulInitialCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Count, info.ulCounts[i], $"ulCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].RejectCount, info.ulRejectCounts[i], $"ulRejectCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Minimum, info.ulMinimums[i], $"ulMinimums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Maximum, info.ulMaximums[i], $"ulMaximums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Status, info.usStatuses[i], $"usStatuses[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Dispensed, info.ulDispensedCounts[i], $"ulDispensedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Presented, info.ulPresentedCounts[i], $"ulPresentedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].Retracted, info.ulRetractedCounts[i], $"ulRetractedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].NumPhysicalCUs, info.usNumPhysicalCUss[i], $"numPhysicalCUs[{i}] should be 1");
            Assert.AreEqual(6, info.listPhysical.Count, $"listPhysical[{i}] should have 1 physical unit");
         }

         // Expected values for physical units (from lppPhysical blocks)
         var expectedPhysical = new[]
         {
        new { PositionName = "RetractCassette", UnitID = "RTCST", InitialCount = "0", Count = "0", RejectCount = "0", Maximum = "400", PStatus = "4", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { PositionName = "RetractCassette", UnitID = "RTCST", InitialCount = "0", Count = "0", RejectCount = "0", Maximum = "400", PStatus = "4", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { PositionName = "RejectCassette", UnitID = "RJCST", InitialCount = "0", Count = "36", RejectCount = "36", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "0", Presented = "0", Retracted = "0" },
        new { PositionName = "CassetteA", UnitID = "CST_A", InitialCount = "1092", Count = "1044", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "306", Presented = "106", Retracted = "0" },
        new { PositionName = "CassetteB", UnitID = "CST_B", InitialCount = "1874", Count = "1690", RejectCount = "14", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "205", Presented = "191", Retracted = "0" },
        new { PositionName = "CassetteC", UnitID = "CST_C", InitialCount = "1237", Count = "1052", RejectCount = "0", Maximum = "0", PStatus = "0", HardwareSensor = "1", Dispensed = "252", Presented = "252", Retracted = "0" },
        new { PositionName = "", UnitID = "", InitialCount = "", Count = "", RejectCount = "", Maximum = "", PStatus = "", HardwareSensor = "", Dispensed = "", Presented = "", Retracted = "" }
    };

         for (int i = 0; i < info.listPhysical.Count; i++)
         {
            // Verify physical unit fields
            var pcu = info.listPhysical[i];
            Assert.AreEqual(expectedPhysical[i].PositionName, pcu.lpPhysicalPositionName, $"listPhysical[{i}].lpPhysicalPositionName mismatch");
            Assert.AreEqual(expectedPhysical[i].UnitID, pcu.cUnitID, $"listPhysical[{i}].cUnitID mismatch");
            Assert.AreEqual(expectedPhysical[i].InitialCount, pcu.ulInitialCount, $"listPhysical[{i}].ulInitialCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Count, pcu.ulCount, $"listPhysical[{i}].ulCount mismatch");
            Assert.AreEqual(expectedPhysical[i].RejectCount, pcu.ulRejectCount, $"listPhysical[{i}].ulRejectCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Maximum, pcu.ulMaximum, $"listPhysical[{i}].ulMaximum mismatch");
            Assert.AreEqual(expectedPhysical[i].PStatus, pcu.usPStatus, $"listPhysical[{i}].usPStatus mismatch");
            Assert.AreEqual(expectedPhysical[i].HardwareSensor, pcu.bHardwareSensor, $"listPhysical[{i}].bHardwareSensor mismatch");
            Assert.AreEqual(expectedPhysical[i].Dispensed, pcu.ulDispensedCount, $"listPhysical[{i}].ulDispensedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Presented, pcu.ulPresentedCount, $"listPhysical[{i}].ulPresentedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].Retracted, pcu.ulRetractedCount, $"listPhysical[{i}].ulRetractedCount mismatch");
         }
      }

      [TestMethod]
      public void WFS_INF_CIM_CASH_UNIT_INFO_1()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_1);
         Assert.IsTrue(logLine is WFSCIMCASHINFO);

         WFSCIMCASHINFO info = (WFSCIMCASHINFO)logLine;

         // Verify logical unit count and truncation status
         Assert.AreEqual(7, info.lUnitCount, $"Expected 7, Actual {info.lUnitCount}");
         Assert.IsFalse(info.IsTruncated, "Log should not be truncated");

         // Expected values for logical units (from WFS_INF_CIM_CASH_UNIT_INFO_1)
         var expectedLogical = new[]
         {
              new { usNumber = "1", fwType = "8",  fwItemType = "0x0001", cUnitID = "LCU00", cCurrencyID = "   ", ulValues = "15", ulCashInCount = "22", ulCount = "29", ulMaximum = "36", usStatus = "43", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "LCU00", ulInitialCount = "100", ulDispensedCount = "107", ulPresentedCount = "114", ulRetractedCount = "121", ulRejectCount = "128", ulMinimum = "135", lpszExtraLCU = "NULL" },
              new { usNumber = "2", fwType = "9",  fwItemType = "0x0001", cUnitID = "LCU01", cCurrencyID = "   ", ulValues = "16", ulCashInCount = "23", ulCount = "30", ulMaximum = "37", usStatus = "44", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "LCU01", ulInitialCount = "101", ulDispensedCount = "108", ulPresentedCount = "115", ulRetractedCount = "122", ulRejectCount = "129", ulMinimum = "136", lpszExtraLCU = "NULL" },
              new { usNumber = "3", fwType = "11", fwItemType = "0x0004", cUnitID = "LCU02", cCurrencyID = "ABC", ulValues = "17", ulCashInCount = "24", ulCount = "31", ulMaximum = "38", usStatus = "45", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "LCU02", ulInitialCount = "102", ulDispensedCount = "109", ulPresentedCount = "116", ulRetractedCount = "123", ulRejectCount = "130", ulMinimum = "137", lpszExtraLCU = "NULL" },
              new { usNumber = "4", fwType = "11", fwItemType = "0x0004", cUnitID = "LCU03", cCurrencyID = "DEF", ulValues = "18", ulCashInCount = "25", ulCount = "32", ulMaximum = "39", usStatus = "46", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "LCU03", ulInitialCount = "103", ulDispensedCount = "110", ulPresentedCount = "117", ulRetractedCount = "124", ulRejectCount = "131", ulMinimum = "138", lpszExtraLCU = "NULL" },
              new { usNumber = "5", fwType = "12", fwItemType = "0x0004", cUnitID = "LCU04", cCurrencyID = "GHI", ulValues = "19", ulCashInCount = "26", ulCount = "33", ulMaximum = "40", usStatus = "47", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "LCU04", ulInitialCount = "104", ulDispensedCount = "111", ulPresentedCount = "118", ulRetractedCount = "125", ulRejectCount = "132", ulMinimum = "139", lpszExtraLCU = "NULL" },
              new { usNumber = "6", fwType = "13", fwItemType = "0x0004", cUnitID = "LCU05", cCurrencyID = "JKL", ulValues = "20", ulCashInCount = "27", ulCount = "34", ulMaximum = "41", usStatus = "48", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "LCU05", ulInitialCount = "105", ulDispensedCount = "112", ulPresentedCount = "119", ulRetractedCount = "126", ulRejectCount = "133", ulMinimum = "140", lpszExtraLCU = "NULL" },
              new { usNumber = "7", fwType = "14", fwItemType = "0x0003", cUnitID = "LCU06", cCurrencyID = "   ", ulValues = "21", ulCashInCount = "28", ulCount = "35", ulMaximum = "42", usStatus = "49", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "LCU06", ulInitialCount = "106", ulDispensedCount = "113", ulPresentedCount = "120", ulRetractedCount = "127", ulRejectCount = "134", ulMinimum = "141", lpszExtraLCU = "NULL" }
         };


         Console.WriteLine("logical");
         Console.WriteLine($"logical count {info.lUnitCount}");
         Console.WriteLine($"physical count {info.listPhysical.Count}");

         // Verify logical unit fields
         for (int i = 0; i < info.lUnitCount; i++)
         {
            Assert.AreEqual(expectedLogical[i].usNumber, info.usNumbers[i], $"usNumbers[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].fwType, info.fwTypes[i], $"fwTypes[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].fwItemType, info.fwItemTypes[i], $"fwItemTypes[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].cUnitID, info.cUnitIDs[i], $"cUnitIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].cCurrencyID, info.cCurrencyIDs[i], $"cCurrencyIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulValues, info.ulValues[i], $"ulValues[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulCashInCount, info.ulCashInCounts[i], $"ulCashInCount[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulCount, info.ulCounts[i], $"ulCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulMaximum, info.ulMaximums[i], $"ulMaximums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].usStatus, info.usStatuses[i], $"usStatuses[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].bAppLock, info.bAppLocks[i], $"bAppLocks[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].usCDMType, info.usCDMTypes[i], $"usCDMType[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].lpszCashUnitName, info.lpszCashUnitNames[i], $"lpszCashUnitNames[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulInitialCount, info.ulInitialCounts[i], $"ulInitialCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulDispensedCount, info.ulDispensedCounts[i], $"ulDispensedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulPresentedCount, info.ulPresentedCounts[i], $"ulPresentedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulRetractedCount, info.ulRetractedCounts[i], $"ulRetractedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulRejectCount, info.ulRejectCounts[i], $"ulRejectCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulMinimum, info.ulMinimums[i], $"ulMinimums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].lpszExtraLCU, info.lpszExtraLCUs[i], $"lpszExtraLCUs[{i}] mismatch");
         }

         // Expected values for physical units (from lppPhysical-> section)
         var expectedPhysical = new[]
         {
             new { usNumPhysicalCUs = "1", lpPhysicalPositionName = "RetractCassette", cUnitID = "RTCST", ulCashInCount = "0", ulCount = "0", ulMaximum = "80", usPStatus = "4", bHardwareSensor = "1", ulInitialCount = "50", ulDispensedCount = "57", ulPresentedCount = "64", ulRetractedCount = "71", ulRejectCount = "78", lpszExtra = "[SerialNumber=NULLSERIALNUMBER]" },
             new { usNumPhysicalCUs = "1", lpPhysicalPositionName = "RejectCassette", cUnitID = "RJCST", ulCashInCount = "0", ulCount = "0", ulMaximum = "210", usPStatus = "4", bHardwareSensor = "1", ulInitialCount = "51", ulDispensedCount = "58", ulPresentedCount = "65", ulRetractedCount = "72", ulRejectCount = "79", lpszExtra = "[SerialNumber=NULLSERIALNUMBER]" },
             new { usNumPhysicalCUs = "1", lpPhysicalPositionName = "CassetteA", cUnitID = "CST_A", ulCashInCount = "37", ulCount = "1994", ulMaximum = "0", usPStatus = "2", bHardwareSensor = "1", ulInitialCount = "52", ulDispensedCount = "59", ulPresentedCount = "66", ulRetractedCount = "73", ulRejectCount = "80", lpszExtra = "[SerialNumber=CGJX501482]" },
             new { usNumPhysicalCUs = "1", lpPhysicalPositionName = "CassetteB", cUnitID = "CST_B", ulCashInCount = "16", ulCount = "1797", ulMaximum = "0", usPStatus = "2", bHardwareSensor = "1", ulInitialCount = "53", ulDispensedCount = "60", ulPresentedCount = "67", ulRetractedCount = "74", ulRejectCount = "81", lpszExtra = "[SerialNumber=CGJX501516]" },
             new { usNumPhysicalCUs = "1", lpPhysicalPositionName = "CassetteC", cUnitID = "CST_C", ulCashInCount = "352", ulCount = "2158", ulMaximum = "0", usPStatus = "1", bHardwareSensor = "1", ulInitialCount = "54", ulDispensedCount = "61", ulPresentedCount = "68", ulRetractedCount = "75", ulRejectCount = "82", lpszExtra = "[SerialNumber=CGJX501608]" },
             new { usNumPhysicalCUs = "1", lpPhysicalPositionName = "CassetteD", cUnitID = "CST_D", ulCashInCount = "182", ulCount = "1336", ulMaximum = "0", usPStatus = "0", bHardwareSensor = "1", ulInitialCount = "55", ulDispensedCount = "62", ulPresentedCount = "69", ulRetractedCount = "76", ulRejectCount = "83", lpszExtra = "[SerialNumber=CGJX469344]" },
             new { usNumPhysicalCUs = "1", lpPhysicalPositionName = "CassetteE", cUnitID = "CST_E", ulCashInCount = "149", ulCount = "149", ulMaximum = "1400", usPStatus = "0", bHardwareSensor = "1", ulInitialCount = "56", ulDispensedCount = "63", ulPresentedCount = "70", ulRetractedCount = "77", ulRejectCount = "84", lpszExtra = "[SerialNumber=CJOG203649]" }
         };

         Console.WriteLine("physical");

         for (int i = 0; i < info.listPhysical.Count; i++)
         {
            // Verify physical unit fields
            var pcu = info.listPhysical[i];
            Assert.AreEqual(expectedPhysical[i].usNumPhysicalCUs, pcu.usNumPhysicalCU, $"listPhysical[{i}].usNumPhysicalCUs mismatch");
            Assert.AreEqual(expectedPhysical[i].lpPhysicalPositionName, pcu.lpPhysicalPositionName, $"listPhysical[{i}].lpPhysicalPositionName mismatch");
            Assert.AreEqual(expectedPhysical[i].cUnitID, pcu.cUnitID, $"listPhysical[{i}].cUnitID mismatch");
            Assert.AreEqual(expectedPhysical[i].ulCashInCount, pcu.ulCashInCount, $"listPhysical[{i}].ulCashInCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulCount, pcu.ulCount, $"listPhysical[{i}].ulCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulMaximum, pcu.ulMaximum, $"listPhysical[{i}].ulMaximum mismatch");
            Assert.AreEqual(expectedPhysical[i].usPStatus, pcu.usPStatus, $"listPhysical[{i}].usPStatus mismatch");
            Assert.AreEqual(expectedPhysical[i].bHardwareSensor, pcu.bHardwareSensor, $"listPhysical[{i}].bHardwareSensor mismatch");
            Assert.AreEqual(expectedPhysical[i].ulInitialCount, pcu.ulInitialCount, $"listPhysical[{i}].ulInitialCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulDispensedCount, pcu.ulDispensedCount, $"listPhysical[{i}].ulDispensedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulPresentedCount, pcu.ulPresentedCount, $"listPhysical[{i}].ulPresentedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulRetractedCount, pcu.ulRetractedCount, $"listPhysical[{i}].ulRetractedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulRejectCount, pcu.ulRejectCount, $"listPhysical[{i}].ulRejectCount mismatch");
            Assert.AreEqual(expectedPhysical[i].lpszExtra, pcu.lpszExtra, $"listPhysical[{i}].lpszExtra mismatch");
         }

         Console.WriteLine("note numbers");

         if (info.noteNumbers == null)
         {
            Console.WriteLine("noteNumberList is null");
            return;
         }

         string[,] expectedNote = new string[7, 20]
         {
            { "1:0", "2:0", "3:0", "4:0", "5:0", "6:0", "7:0", "8:0", "9:0", "10:0", "11:0", "12:0", "13:0", "14:0", "15:0", "16:0", "17:0", "0:0", null, null },
            { "1:0", "2:0", "3:0", "4:0", "5:0", "6:0", "7:0", "8:0", "9:0", "10:0", "11:0", "12:0", "13:0", "14:0", "15:0", "16:0", "17:0", "0:0", null, null },
            { "1:1994", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, null, null },
            { "3:1797", "8:0", "13:0", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, null, null },
            { "5:2158", "10:0", "15:0", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, null, null },
            { "6:1336", "11:0", "16:0", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, null, null },
            { "1:0", "2:0", "3:0", "4:0", "5:0", "6:0", "7:0", "8:0", "9:0", "10:0", "11:0", "12:0", "13:10", "14:11", "15:20", "16:0", "17:108", "0:0", null, null }
         };

         int rows = info.noteNumbers.GetLength(0); // Number of logical units
         int cols = info.noteNumbers.GetLength(1); // Number of fields per unit (e.g., usNoteID, ulCount)

         Assert.AreEqual(7, rows, $"Expected 7 logical units, Actual {rows}");
         Assert.AreEqual(20, cols, $"Expected 20 Note Types, Actual {cols}");


         for (int i = 0; i < rows; i++)
         {
            //Console.Write("{ ");
            for (int j = 0; j < cols; j++)
            {
               Assert.AreEqual(expectedNote[i, j], info.noteNumbers[i, j], $"Expected expectedNote[{i},{j}] = { expectedNote[i, j]}, Actual info.noteNumbers[{i}, {j}] = {info.noteNumbers[i, j]}");
               //if (info.noteNumbers[i, j] == string.Empty) Console.Write("String.Empty");
               //else if (info.noteNumbers[i, j] == null) Console.Write("null");
               //else Console.Write($"\"{info.noteNumbers[i, j]}\", ");
            }
            //Console.Write("},");
            //Console.WriteLine("");
         }
         //Assert.AreEqual(true, false);
      }

      [TestMethod]
      public void WFS_INF_CIM_CASH_UNIT_INFO_2()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2);
         Assert.IsTrue(logLine is WFSCIMCASHINFO);

         WFSCIMCASHINFO info = (WFSCIMCASHINFO)logLine;

         // Verify logical unit count and truncation status
         Assert.AreEqual(2, info.lUnitCount, $"Expected 2, Actual {info.lUnitCount}");
         Assert.AreEqual(true, info.IsTruncated, $"Expected true, Actual {info.IsTruncated}");

         // Expected values for logical units (from WFS_INF_CIM_CASH_UNIT_INFO_2)
         var expectedLogical = new[]
         {
              new { usNumber = "1", fwType = "4",  fwItemType = "0x0001", cUnitID = "LCU00", cCurrencyID = "USD", ulValues = "0", ulCashInCount = "39", ulCount = "4", ulMaximum = "126", usStatus = "0", bAppLock = "0", usCDMType = "0", lpszCashUnitName = "RETRACTCASSETTE", ulInitialCount = "0", ulDispensedCount = "0", ulPresentedCount = "0", ulRetractedCount = "0", ulRejectCount = "0", ulMinimum = "0", lpszExtraLCU = "NULL" },
              new { usNumber = "2", fwType = "1",  fwItemType = "0x0002", cUnitID = "LCU00", cCurrencyID = "USD", ulValues = "3", ulCashInCount = "4", ulCount = "5", ulMaximum = "6", usStatus = "7", bAppLock = "8", usCDMType = "27", lpszCashUnitName = "TESTCASSETTE", ulInitialCount = "28", ulDispensedCount = "29", ulPresentedCount = "30", ulRetractedCount = "31", ulRejectCount = "32", ulMinimum = "33", lpszExtraLCU = "NULL" }
         };


         Console.WriteLine("logical");
         Console.WriteLine($"logical count {info.lUnitCount}");

         // Verify logical unit fields
         for (int i = 0; i < info.lUnitCount; i++)
         {
            Console.WriteLine($"Iterating over logical unit fields {i}");

            Assert.AreEqual(expectedLogical[i].usNumber, info.usNumbers[i], $"usNumbers[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].fwType, info.fwTypes[i], $"fwTypes[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].fwItemType, info.fwItemTypes[i], $"fwItemTypes[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].cUnitID, info.cUnitIDs[i], $"cUnitIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].cCurrencyID, info.cCurrencyIDs[i], $"cCurrencyIDs[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulValues, info.ulValues[i], $"ulValues[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulCashInCount, info.ulCashInCounts[i], $"ulCashInCount[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulCount, info.ulCounts[i], $"ulCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulMaximum, info.ulMaximums[i], $"ulMaximums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].usStatus, info.usStatuses[i], $"usStatuses[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].bAppLock, info.bAppLocks[i], $"bAppLocks[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].usCDMType, info.usCDMTypes[i], $"usCDMType[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].lpszCashUnitName, info.lpszCashUnitNames[i], $"lpszCashUnitNames[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulInitialCount, info.ulInitialCounts[i], $"ulInitialCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulDispensedCount, info.ulDispensedCounts[i], $"ulDispensedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulPresentedCount, info.ulPresentedCounts[i], $"ulPresentedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulRetractedCount, info.ulRetractedCounts[i], $"ulRetractedCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulRejectCount, info.ulRejectCounts[i], $"ulRejectCounts[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].ulMinimum, info.ulMinimums[i], $"ulMinimums[{i}] mismatch");
            Assert.AreEqual(expectedLogical[i].lpszExtraLCU, info.lpszExtraLCUs[i], $"lpszExtraLCUs[{i}] mismatch");
         }

         Console.WriteLine($"physical count {info.listPhysical.Count}");

         // Expected values for physical units (from lppPhysical-> section)
         var expectedPhysical = new[]
         {
             new { usNumPhysicalCUs = "", lpPhysicalPositionName = "RETRACTCASSETTE", cUnitID = "PCU01", ulCashInCount = "39", ulCount = "39", ulMaximum = "0", usPStatus = "0", bHardwareSensor = "0", lpszExtra = "NULL", ulInitialCount = "0", ulDispensedCount = "0", ulPresentedCount = "0", ulRetractedCount = "0", ulRejectCount = "0" },
             new { usNumPhysicalCUs = "", lpPhysicalPositionName = "TESTCASSETTE", cUnitID = "PCU09", ulCashInCount = "27", ulCount = "28", ulMaximum = "29", usPStatus = "30", bHardwareSensor = "31", lpszExtra = "NULL", ulInitialCount = "32", ulDispensedCount = "33", ulPresentedCount = "34", ulRetractedCount = "35", ulRejectCount = "36" }
         };

         Console.Write("physical");

         for (int i = 0; i < info.listPhysical.Count; i++)
         {
            Console.WriteLine($"Iterating over listPhysical {i}");

            // Verify physical unit fields
            var pcu = info.listPhysical[i];
            Assert.AreEqual(expectedPhysical[i].usNumPhysicalCUs, pcu.usNumPhysicalCU, $"listPhysical[{i}].usNumPhysicalCUs mismatch");
            Assert.AreEqual(expectedPhysical[i].lpPhysicalPositionName, pcu.lpPhysicalPositionName, $"listPhysical[{i}].lpPhysicalPositionName mismatch");
            Assert.AreEqual(expectedPhysical[i].cUnitID, pcu.cUnitID, $"listPhysical[{i}].cUnitID mismatch");
            Assert.AreEqual(expectedPhysical[i].ulCashInCount, pcu.ulCashInCount, $"listPhysical[{i}].ulCashInCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulCount, pcu.ulCount, $"listPhysical[{i}].ulCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulMaximum, pcu.ulMaximum, $"listPhysical[{i}].ulMaximum mismatch");
            Assert.AreEqual(expectedPhysical[i].usPStatus, pcu.usPStatus, $"listPhysical[{i}].usPStatus mismatch");
            Assert.AreEqual(expectedPhysical[i].bHardwareSensor, pcu.bHardwareSensor, $"listPhysical[{i}].bHardwareSensor mismatch");
            Assert.AreEqual(expectedPhysical[i].lpszExtra, pcu.lpszExtra, $"listPhysical[{i}].lpszExtra mismatch");
            Assert.AreEqual(expectedPhysical[i].ulInitialCount, pcu.ulInitialCount, $"listPhysical[{i}].ulInitialCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulDispensedCount, pcu.ulDispensedCount, $"listPhysical[{i}].ulDispensedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulPresentedCount, pcu.ulPresentedCount, $"listPhysical[{i}].ulPresentedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulRetractedCount, pcu.ulRetractedCount, $"listPhysical[{i}].ulRetractedCount mismatch");
            Assert.AreEqual(expectedPhysical[i].ulRejectCount, pcu.ulRejectCount, $"listPhysical[{i}].ulRejectCount mismatch");
         }
      }
   }
}
