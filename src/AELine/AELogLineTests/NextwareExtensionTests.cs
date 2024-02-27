using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Moq;
using Moq.Protected;
using Contract;
using System.Numerics;
using System;
using AELogLineTests;

namespace AELogLineTests
{
   [TestClass]
   public class NextwareExtensionTests
   {
      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void NextwareExtension_Started()
      {
         string sampleLine = @"2023-09-25 03:00:55 [NextwareExtension] The 'NextwareExtension' extension is started.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
      }

      [TestMethod]
      public void NextwareExtension_Start()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] NextwareExtension : Start";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.IsTrue(line.ExtensionRunning);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_End()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] NextwareExtension : End";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.IsFalse(line.ExtensionRunning);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_OpenSessionSync()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Calling OpenSessionSync: Device=NXVendorDependentModeXClass";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.AreEqual("NXVendorDependentModeXClass", line.MonitoringDeviceName);
         Assert.AreEqual("SYNC", line.MonitoringDeviceChanges);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_StartMonitoring()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] StartMonitoring [start] .....................";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.AreEqual("MONITORING STARTED", line.MonitoringDeviceChanges);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_OpenSession()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Calling OpenSession: Device = NXPin";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.AreEqual("NXPin", line.MonitoringDeviceName);
         Assert.AreEqual("OPEN SESSION", line.MonitoringDeviceChanges);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_OpenSessionSucceeded()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] OpenSession succeeded: Device = NXPin, Elapsed = 00:00:00.0200079";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.AreEqual("NXPin", line.MonitoringDeviceName);
         Assert.AreEqual("OPEN SESSION SUCCEEDED", line.MonitoringDeviceChanges);
         Assert.AreEqual("00:00:00.0200079", line.MonitoringElapsed);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_OnDeviceStatusChanged()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] NextwareExtension : OnDeviceStatusChanged: NH.Agent.Extensions.Nextware.NXPin";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.AreEqual("NH.Agent.Extensions.Nextware.NXPin", line.MonitoringDeviceName);
         Assert.AreEqual("STATUS CHANGED", line.MonitoringDeviceChanges);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_EncStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""EncStatus\"":\""0\"",\""DevicePositionStatus\"":\""3\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""xyz\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("EncStatus", line.StatusDeviceName);
         Assert.AreEqual("EncStatus: 0,DevicePositionStatus: 3,PowerSaveRecoveryTime: 0,LogicalServiceName: xyz,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_AcceptorStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""AcceptorStatus\"":\""0\"",\""MediaStatus\"":\""1\"",\""TonerStatus\"":\""0\"",\""InkStatus\"":\""3\"",\""FrontImageScannerStatus\"":\""0\"",\""BackImageScannerStatus\"":\""0\"",\""MICRReaderStatus\"":\""0\"",\""StackerStatus\"":\""0\"",\""ReBuncherStatus\"":\""0\"",\""MediaFeederStatus\"":\""0\"",\""PositionStatus_Input\"":\""0\"",\""PositionStatus_Output\"":\""0\"",\""PositionStatus_Refused\"":\""0\"",\""ShutterStatus_Input\"":\""0\"",\""ShutterStatus_Output\"":\""0\"",\""ShutterStatus_Refused\"":\""0\"",\""TransportStatus_Input\"":\""0\"",\""TransportStatus_Output\"":\""\"",\""TransportStatus_Refused\"":\""\"",\""TransportMediaStatus_Input\"":\""0\"",\""TransportMediaStatus_Output\"":\""0\"",\""TransportMediaStatus_Refused\"":\""0\"",\""DevicePositionStatus\"":\""0\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("AcceptorStatus", line.StatusDeviceName);
         Assert.AreEqual("AcceptorStatus: 0,MediaStatus: 1,TonerStatus: 0,InkStatus: 3,FrontImageScannerStatus: 0,BackImageScannerStatus: 0,MICRReaderStatus: 0,StackerStatus: 0,ReBuncherStatus: 0,MediaFeederStatus: 0,PositionStatus_Input: 0,PositionStatus_Output: 0,PositionStatus_Refused: 0,ShutterStatus_Input: 0,ShutterStatus_Output: 0,ShutterStatus_Refused: 0,TransportStatus_Input: 0,TransportStatus_Output: ,TransportStatus_Refused: ,TransportMediaStatus_Input: 0,TransportMediaStatus_Output: 0,TransportMediaStatus_Refused: 0,DevicePositionStatus: 0,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_EncStatus2()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""EncStatus\"":\""0\"",\""DevicePositionStatus\"":\""3\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("EncStatus", line.StatusDeviceName);
         Assert.AreEqual("EncStatus: 0,DevicePositionStatus: 3,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_SafeDoorStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""SafeDoorStatus\"":\""1\"",\""DispenserStatus\"":\""1\"",\""IntermediateStackerStatus\"":\""0\"",\""ShutterStatus\"":\""0\"",\""PositionStatus\"":\""0\"",\""TransportStatus\"":\""0\"",\""TransportStatusStatus\"":\""0\"",\""DevicePositionStatus\"":\""3\"",\""PowerSaveRecoveryTime\"":0,\""UnitCurrencyID\"":[\""   \"",\""USD\"",\""USD\"",\""USD\"",\""USD\""],\""UnitValue\"":[0,1,5,20,50],\""UnitStatus\"":[\""0\"",\""0\"",\""3\"",\""0\"",\""3\""],\""UnitCount\"":[0,999,274,3127,707],\""UnitType\"":[\""REJECTCASSETTE\"",\""BILLCASSETTE\"",\""BILLCASSETTE\"",\""BILLCASSETTE\"",\""BILLCASSETTE\""],\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("SafeDoorStatus", line.StatusDeviceName);
         Assert.AreEqual("SafeDoorStatus: 1,DispenserStatus: 1,IntermediateStackerStatus: 0,ShutterStatus: 0,PositionStatus: 0,TransportStatus: 0,DevicePositionStatus: 3,PowerSaveRecoveryTime: 0,UnitCurrencyID: [   ,USD,USD,USD,USD,],UnitValue: [0,1,5,20,50,],UnitStatus: [0,0,3,0,3,],UnitCount: [0,999,274,3127,707,],UnitType: [REJECTCASSETTE,BILLCASSETTE,BILLCASSETTE,BILLCASSETTE,BILLCASSETTE,],LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_CabinetStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""CabinetStatus\"":\""1\"",\""SafeStatus\"":\""1\"",\""VandalShieldStatus\"":null,\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("CabinetStatus", line.StatusDeviceName);
         Assert.AreEqual("CabinetStatus: 1,SafeStatus: 1,VandalShieldStatus: ,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_MediaStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""MediaStatus\"":\""2\"",\""RetainBinStatus\"":\""2\"",\""SecurityStatus\"":\""1\"",\""NumberOfCardsRetained\"":\""0\"",\""ChipPowerStatus\"":\""5\"",\""DevicePositionStatus\"":\""3\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("MediaStatus", line.StatusDeviceName);
         Assert.AreEqual("MediaStatus: 2,RetainBinStatus: 2,SecurityStatus: 1,NumberOfCardsRetained: 0,ChipPowerStatus: 5,Media_DevicePositionStatus: 3,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_CardUnitStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""CardUnitStatus\"":\""1\"",\""PinpadStatus\"":\""1\"",\""NotesDispenserStatus\"":\""1\"",\""CoinDispenserStatus\"":\""1\"",\""ReceiptPrinterStatus\"":\""1\"",\""PassbookPrinterStatus\"":null,\""EnvelopeDepositoryStatus\"":null,\""ChequeUnitStatus\"":\""1\"",\""BillAcceptorStatus\"":\""1\"",\""EnvelopeDispenserStatus\"":null,\""DocumentPrinterStatus\"":\""1\"",\""CoinAcceptorStatus\"":\""1\"",\""ScannerStatus\"":\""1\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("CardUnitStatus", line.StatusDeviceName);
         Assert.AreEqual("CardUnitStatus: 1,PinpadStatus: 1,NotesDispenserStatus: 1,CoinDispenserStatus: 1,ReceiptPrinterStatus: 1,PassbookPrinterStatus: ,EnvelopeDepositoryStatus: ,ChequeUnitStatus: 1,BillAcceptorStatus: 1,EnvelopeDispenserStatus: ,DocumentPrinterStatus: 1,CoinAcceptorStatus: 1,ScannerStatus: 1,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_OperatorSwitchStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""OperatorSwitchStatus\"":\""1\"",\""TamperStatus\"":null,\""IntTamperStatus\"":null,\""SeismicStatus\"":null,\""HeatStatus\"":null,\""ProximityStatus\"":null,\""AmblightStatus\"":null,\""EnhancedAudioStatus\"":\""2\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("OperatorSwitchStatus", line.StatusDeviceName);
         Assert.AreEqual("OperatorSwitchStatus: 1,TamperStatus: ,IntTamperStatus: ,SeismicStatus: ,HeatStatus: ,ProximityStatus: ,AmblightStatus: ,EnhancedAudioStatus: 2,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_OpenCloseStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""OpenCloseStatus\"":\""1\"",\""FasciaLightStatus\"":null,\""AudioStatus\"":\""1\"",\""HeatingStatus\"":null,\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("OpenCloseStatus", line.StatusDeviceName);
         Assert.AreEqual("OpenCloseStatus: 1,FasciaLightStatus: ,AudioStatus: 1,HeatingStatus: ,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_VolumeStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""VolumeStatus\"":1000,\""UpsStatus\"":\""0\"",\""GreenLedStatus\"":\""1\"",\""AmberLedStatus\"":\""2\"",\""RedLedStatus\"":\""2\"",\""AudibleAlarmStatus\"":null,\""EnhancedAudioControlStatus\"":\""2\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);

         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("VolumeStatus", line.StatusDeviceName);
         Assert.AreEqual("VolumeStatus: 1000,UpsStatus: 0,GreenLedStatus: 1,AmberLedStatus: 2,RedLedStatus: 2,AudibleAlarmStatus: ,EnhancedAudioControlStatus: 2,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      public void NextwareExtension_AgentMessageEvent_CheckAcceptorStatus()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NextwareExtension] Firing agent message event: DeviceState - POST - {""Id"":0,""MacAddress"":""70-85-C2-18-7C-DA"",""DeviceID"":""9"",""DeviceClass"":null,""DisplayName"":null,""Status"":""0"",""AssetName"":null,""MediaStatus"":null,""DeviceSpecificStatus"":""{\""CheckAcceptorStatus\"":\""0\"",\""MediaStatus\"":\""1\"",\""TonerStatus\"":\""3\"",\""InkStatus\"":\""3\"",\""FrontImageScannerStatus\"":\""0\"",\""BackImageScannerStatus\"":\""0\"",\""MICRReaderStatus\"":\""0\"",\""StackerStatus\"":\""0\"",\""ReBuncherStatus\"":\""0\"",\""MediaFeederStatus\"":\""0\"",\""PositionStatus_Input\"":\""0\"",\""PositionStatus_Output\"":\""0\"",\""PositionStatus_Refused\"":\""0\"",\""ShutterStatus_Input\"":\""0\"",\""ShutterStatus_Output\"":\""0\"",\""ShutterStatus_Refused\"":\""0\"",\""TransportStatus_Input\"":\""0\"",\""TransportStatus_Output\"":\""0\"",\""TransportStatus_Refused\"":\""0\"",\""TransportMediaStatus_Input\"":\""0\"",\""TransportMediaStatus_Output\"":\""0\"",\""TransportMediaStatus_Refused\"":\""0\"",\""DevicePositionStatus\"":\""0\"",\""PowerSaveRecoveryTime\"":0,\""LogicalServiceName\"":\""\"",\""ExtraInformation\"":\""\""}"",""Timestamp"":""0001-01-01T00:00:00""}";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         Assert.AreEqual(0, line.Id);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.MacAddress);
         Assert.AreEqual("9", line.DeviceId);
         Assert.IsNull(line.DeviceClass);
         Assert.IsNull(line.DisplayName);
         Assert.AreEqual("0", line.Status);
         Assert.IsNull(line.AssetName);

         Assert.AreEqual("CheckAcceptorStatus", line.StatusDeviceName);
         Assert.AreEqual("CheckAcceptorStatus: 0,MediaStatus: 1,TonerStatus: 3,InkStatus: 3,FrontImageScannerStatus: 0,BackImageScannerStatus: 0,MICRReaderStatus: 0,StackerStatus: 0,ReBuncherStatus: 0,MediaFeederStatus: 0,PositionStatus_Input: 0,PositionStatus_Output: 0,PositionStatus_Refused: 0,ShutterStatus_Input: 0,ShutterStatus_Output: 0,ShutterStatus_Refused: 0,TransportStatus_Input: 0,TransportStatus_Output: 0,TransportStatus_Refused: 0,TransportMediaStatus_Input: 0,TransportMediaStatus_Output: 0,TransportMediaStatus_Refused: 0,DevicePositionStatus: 0,PowerSaveRecoveryTime: 0,LogicalServiceName: ,ExtraInformation: ,", line.DeviceStatus);

         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NextwareExtension, line.aeType);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void NextwareExtension_Unsupported()
      {
         string sampleLine = @"2023-09-25 03:00:55 [NextwareExtension] Is not a supported line.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = null;

         try
         {
            line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.NextwareExtension: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void NextwareExtension_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:21 not an Extension started log line";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NextwareExtension line = null;

         try
         {
            line = new NextwareExtension(logFileHandler, sampleLine, AELogType.NextwareExtension);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.NextwareExtension: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
