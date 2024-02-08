using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LogLineHandler
{
   public class NextwareExtension : AELine
   {
      bool isRecognized = false;

      // monitoring state
      public string MonitoringDeviceChanges { get; set; } = string.Empty;
      public string MonitoringDeviceName { get; set; } = string.Empty;
      public string MonitoringElapsed { get; set; } = string.Empty;


      // generic for all
      public long Id { get; set; }
      public string MacAddress { get; set; } = String.Empty;
      public string DeviceId { get; set; } = string.Empty;
      public string DeviceClass { get; set; } = string.Empty;
      public string DisplayName { get; set; } = string.Empty;
      public string Status { get; set; } = string.Empty;
      public string AssetName { get; set; } = string.Empty;
      public string DeviceMediaStatus { get; set; } = string.Empty;  // renamed to avoid conflict with MediaStatus
      public string DeviceStateTimestampUTC { get; set; } = string.Empty;    // new to avoid conflict with parent class property

      // device-specific
      public string StatusDeviceName { get; set; } = string.Empty;

      // encoder
      public string EncStatus { get; set; } = string.Empty;
      public string DevicePositionStatus { get; set; } = string.Empty;
      public long PowerSaveRecoveryTime { get; set; } = 0;
      public string LogicalServiceName { get; set; } = string.Empty;
      public string ExtraInformation { get; set; } = string.Empty;

      // safe door
      public string SafeDoorStatus { get; set; } = string.Empty;
      public string DispenserStatus { get; set; } = string.Empty;
      public string IntermediateStackerStatus { get; set; } = string.Empty;
      public string ShutterStatus { get; set; } = string.Empty;
      public string PositionStatus { get; set; } = string.Empty;
      public string TransportStatus { get; set; } = string.Empty;
      public string TransportStatusStatus { get; set; } = string.Empty;
      public List<string> UnitCurrencyID { get; set; }
      public List<long> UnitValue { get; set; }
      public List<string> UnitStatus { get; set; }
      public List<long> UnitCount { get; set; }
      public List<string> UnitType { get; set; }

      // cabinet
      public string CabinetStatus { get; set; } = string.Empty;
      public string SafeStatus { get; set; } = string.Empty;
      public string VandalShieldStatus { get; set; } = string.Empty;

      // media (1)
      public string MediaStatus { get; set; } = string.Empty;
      public string RetainBinStatus { get; set; } = string.Empty;
      public string SecurityStatus { get; set; } = string.Empty;
      public string NumberOfCardsRetained { get; set; } = string.Empty;
      public string ChipPowerStatus { get; set; } = string.Empty;

      // media(2)

      public List<string> PaperStatus { get; set; }
      public string Media_TonerStatus { get; set; } = string.Empty;  // see duplicate check acceptor
      public string Media_InkStatus { get; set; } = string.Empty;    // see duplicate check accepter
      public string LampStatus { get; set; } = string.Empty;
      public List<string> RetractBinStatus { get; set; }
      public long MediaOnStacker { get; set; } = 0;
      public string Media_DevicePositionStatus { get; set; } = string.Empty;  // see duplicate encoder



      // card unit
      public string CardUnitStatus { get; set; } = string.Empty;
      public string PinpadStatus { get; set; } = string.Empty;
      public string NotesDispenserStatus { get; set; } = string.Empty;
      public string CoinDispenserStatus { get; set; } = string.Empty;
      public string ReceiptPrinterStatus { get; set; } = string.Empty;
      public string PassbookPrinterStatus { get; set; } = string.Empty;
      public string EnvelopeDepositoryStatus { get; set; } = string.Empty;
      public string ChequeUnitStatus { get; set; } = string.Empty;
      public string BillAcceptorStatus { get; set; } = string.Empty;
      public string EnvelopeDispenserStatus { get; set; } = string.Empty;
      public string DocumentPrinterStatus { get; set; } = string.Empty;
      public string CoinAcceptorStatus { get; set; } = string.Empty;
      public string ScannerStatus { get; set; } = string.Empty;

      // operator switch
      public string OperatorSwitchStatus { get; set; } = string.Empty;
      public string TamperStatus { get; set; } = string.Empty;
      public string IntTamperStatus { get; set; } = string.Empty;
      public string SeismicStatus { get; set; } = string.Empty;
      public string HeatStatus { get; set; } = string.Empty;
      public string ProximityStatus { get; set; } = string.Empty;
      public string AmblightStatus { get; set; } = string.Empty;
      public string EnhancedAudioStatus { get; set; } = string.Empty;

      // open-close
      public string OpenCloseStatus { get; set; } = string.Empty;
      public string FasciaLightStatus { get; set; } = string.Empty;
      public string AudioStatus { get; set; } = string.Empty;
      public string HeatingStatus { get; set; } = string.Empty;

      // volume
      public long VolumeStatus { get; set; } = 0;
      public string UpsStatus { get; set; } = string.Empty;
      public string GreenLedStatus { get; set; } = string.Empty;
      public string AmberLedStatus { get; set; } = string.Empty;
      public string RedLedStatus { get; set; } = string.Empty;
      public string AudibleAlarmStatus { get; set; } = string.Empty;
      public string EnhancedAudioControlStatus { get; set; } = string.Empty;

      // check acceptor
      public string CheckAcceptorStatus { get; set; } = string.Empty;
      public string TonerStatus { get; set; } = string.Empty;
      public string InkStatus { get; set; } = string.Empty;
      public string FrontImageScannerStatus { get; set; } = string.Empty;
      public string BackImageScannerStatus { get; set; } = string.Empty;
      public string MICRReaderStatus { get; set; } = string.Empty;
      public string StackerStatus { get; set; } = string.Empty;
      public string ReBuncherStatus { get; set; } = string.Empty;
      public string MediaFeederStatus { get; set; } = string.Empty;
      public string PositionStatus_Input { get; set; } = string.Empty;
      public string PositionStatus_Output { get; set; } = string.Empty;
      public string PositionStatus_Refused { get; set; } = string.Empty;
      public string ShutterStatus_Input { get; set; } = string.Empty;
      public string ShutterStatus_Output { get; set; } = string.Empty;
      public string ShutterStatus_Refused { get; set; } = string.Empty;
      public string TransportStatus_Input { get; set; } = string.Empty;
      public string TransportStatus_Output { get; set; } = string.Empty;
      public string TransportStatus_Refused { get; set; } = string.Empty;
      public string TransportMediaStatus_Input { get; set; } = string.Empty;
      public string TransportMediaStatus_Output { get; set; } = string.Empty;
      public string TransportMediaStatus_Refused { get; set; } = string.Empty;



      public NextwareExtension(ILogFileHandler parent, string logLine, AELogType aeType = AELogType.NextwareExtension) : base(parent, logLine, aeType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         /*
            2023-11-17 03:01:58 [NextwareExtension] The 'NextwareExtension' extension is started.
            2023-11-17 03:01:58 [NextwareExtension] Calling OpenSessionSync: Device=NXVendorDependentModeXClass
            2023-11-17 03:01:58 [NextwareExtension] NextwareExtension : Start
            2023-11-17 03:01:58 [NextwareExtension] NextwareExtension : End
            2023-11-17 03:01:58 [NextwareExtension] StartMonitoring [start] .....................
            2023-11-17 03:01:58 [NextwareExtension] Calling OpenSession: Device = NXPin
            2023-11-17 03:01:58 [NextwareExtension] OpenSession succeeded: Device = NXPin, Elapsed = 00:00:00.0200079
            2023-11-17 03:01:58 [NextwareExtension] NextwareExtension : OnDeviceStatusChanged: NH.Agent.Extensions.Nextware.NXPin
            2023-11-17 03:01:58 [NextwareExtension] Firing agent message event: DeviceState - POST - {"Id":0,"MacAddress":"70-85-C2-18-7C-DA","DeviceID":"9","DeviceClass":null,"DisplayName":null,"Status":"0","AssetName":null,"MediaStatus":null,"DeviceSpecificStatus":"{\"EncStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}","Timestamp":"0001-01-01T00:00:00"}
          */

         int idx = logLine.IndexOf("[NextwareExtension]");
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + "[NextwareExtension]".Length + 1);

            //Started monitoring device status changes.
            if (subLogLine == "Started monitoring device status changes.")
            {
               isRecognized = true;
               MonitoringDeviceChanges = "STARTED";
            }

            else if (subLogLine.Contains("DeviceState - POST - {"))
            {
               //Firing agent message event: DeviceState - POST - {"Id":0,"MacAddress":"70-85-C2-18-7C-DA","DeviceID":"9","DeviceClass":null,"DisplayName":null,"Status":"0","AssetName":null,"MediaStatus":null,"DeviceSpecificStatus":"{\"EncStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}","Timestamp":"0001-01-01T00:00:00"}
               /*
                * {"Id":0,
                * "MacAddress":"70-85-C2-18-7C-DA",
                * "DeviceID":"9",
                * "DeviceClass":null,
                * "DisplayName":null,
                * "Status":"0",
                * "AssetName":null,
                * "MediaStatus":null,
                * "DeviceSpecificStatus":"{
                *   \"EncStatus\":\"0\",
                *   \"DevicePositionStatus\":\"3\",
                *   \"PowerSaveRecoveryTime\":0,
                *   \"LogicalServiceName\":\"\"
                *   \"ExtraInformation\":\"\"
                * }",
                * "Timestamp":"0001-01-01T00:00:00"} 
                */

               /* DeviceSpecificStatus
                  {\"EncStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"SafeDoorStatus\":\"1\",\"DispenserStatus\":\"1\",\"IntermediateStackerStatus\":\"0\",\"ShutterStatus\":\"0\",\"PositionStatus\":\"0\",\"TransportStatus\":\"0\",\"TransportStatusStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"UnitCurrencyID\":[\"   \",\"USD\",\"USD\",\"USD\",\"USD\"],\"UnitValue\":[0,1,5,20,50],\"UnitStatus\":[\"0\",\"0\",\"3\",\"0\",\"3\"],\"UnitCount\":[0,999,274,3127,707],\"UnitType\":[\"REJECTCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\"],\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"SafeDoorStatus\":\"1\",\"DispenserStatus\":\"2\",\"IntermediateStackerStatus\":\"0\",\"ShutterStatus\":\"0\",\"PositionStatus\":\"0\",\"TransportStatus\":\"0\",\"TransportStatusStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"UnitCurrencyID\":[\"   \",\"USD\",\"USD\",\"USD\",\"USD\"],\"UnitValue\":[0,1,5,20,50],\"UnitStatus\":[\"6\",\"0\",\"3\",\"0\",\"3\"],\"UnitCount\":[0,999,265,3099,701],\"UnitType\":[\"REJECTCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\"],\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"SafeDoorStatus\":\"1\",\"DispenserStatus\":\"1\",\"IntermediateStackerStatus\":\"0\",\"ShutterStatus\":\"0\",\"PositionStatus\":\"0\",\"TransportStatus\":\"0\",\"TransportStatusStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"UnitCurrencyID\":[\"   \",\"USD\",\"USD\",\"USD\",\"USD\"],\"UnitValue\":[0,1,5,20,50],\"UnitStatus\":[\"0\",\"0\",\"3\",\"0\",\"3\"],\"UnitCount\":[0,999,265,3099,701],\"UnitType\":[\"REJECTCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\"],\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"SafeDoorStatus\":\"1\",\"DispenserStatus\":\"2\",\"IntermediateStackerStatus\":\"0\",\"ShutterStatus\":\"0\",\"PositionStatus\":\"0\",\"TransportStatus\":\"0\",\"TransportStatusStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"UnitCurrencyID\":[\"   \",\"USD\",\"USD\",\"USD\",\"USD\"],\"UnitValue\":[0,1,5,20,50],\"UnitStatus\":[\"6\",\"0\",\"3\",\"0\",\"3\"],\"UnitCount\":[0,996,256,3009,685],\"UnitType\":[\"REJECTCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\"],\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"SafeDoorStatus\":\"1\",\"DispenserStatus\":\"1\",\"IntermediateStackerStatus\":\"0\",\"ShutterStatus\":\"0\",\"PositionStatus\":\"0\",\"TransportStatus\":\"0\",\"TransportStatusStatus\":\"0\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"UnitCurrencyID\":[\"   \",\"USD\",\"USD\",\"USD\",\"USD\"],\"UnitValue\":[0,1,5,20,50],\"UnitStatus\":[\"0\",\"0\",\"3\",\"0\",\"3\"],\"UnitCount\":[0,996,256,3009,685],\"UnitType\":[\"REJECTCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\"],\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"CabinetStatus\":\"1\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"2\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"2\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"1\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"2\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"2\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"2\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CabinetStatus\":\"1\",\"SafeStatus\":\"1\",\"VandalShieldStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"MediaStatus\":\"2\",\"RetainBinStatus\":\"2\",\"SecurityStatus\":\"1\",\"NumberOfCardsRetained\":\"0\",\"ChipPowerStatus\":\"5\",\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"MediaStatus\":\"0\",\"PaperStatus\":[\"1\",null,null,null,null,null],\"TonerStatus\":\"0\",\"InkStatus\":\"3\",\"LampStatus\":\"3\",\"RetractBinStatus\":[],\"MediaOnStacker\":0,\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"MediaStatus\":\"1\",\"PaperStatus\":[null,null,null,null,null,null],\"TonerStatus\":\"3\",\"InkStatus\":\"3\",\"LampStatus\":\"3\",\"RetractBinStatus\":[],\"MediaOnStacker\":0,\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"MediaStatus\":\"0\",\"PaperStatus\":[\"0\",null,null,null,null,null],\"TonerStatus\":\"0\",\"InkStatus\":\"3\",\"LampStatus\":\"3\",\"RetractBinStatus\":[],\"MediaOnStacker\":0,\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"MediaStatus\":\"0\",\"PaperStatus\":[\"1\",null,null,null,null,null],\"TonerStatus\":\"0\",\"InkStatus\":\"3\",\"LampStatus\":\"3\",\"RetractBinStatus\":[],\"MediaOnStacker\":0,\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"MediaStatus\":\"1\",\"PaperStatus\":[\"2\",null,null,null,null,null],\"TonerStatus\":\"0\",\"InkStatus\":\"3\",\"LampStatus\":\"3\",\"RetractBinStatus\":[],\"MediaOnStacker\":0,\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"MediaStatus\":\"0\",\"PaperStatus\":[\"0\",null,null,null,null,null],\"TonerStatus\":\"0\",\"InkStatus\":\"3\",\"LampStatus\":\"3\",\"RetractBinStatus\":[],\"MediaOnStacker\":0,\"DevicePositionStatus\":\"3\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"CardUnitStatus\":\"1\",\"PinpadStatus\":\"1\",\"NotesDispenserStatus\":\"1\",\"CoinDispenserStatus\":\"1\",\"ReceiptPrinterStatus\":\"1\",\"PassbookPrinterStatus\":null,\"EnvelopeDepositoryStatus\":null,\"ChequeUnitStatus\":\"1\",\"BillAcceptorStatus\":\"1\",\"EnvelopeDispenserStatus\":null,\"DocumentPrinterStatus\":\"1\",\"CoinAcceptorStatus\":\"1\",\"ScannerStatus\":\"1\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"OperatorSwitchStatus\":\"1\",\"TamperStatus\":null,\"IntTamperStatus\":null,\"SeismicStatus\":null,\"HeatStatus\":null,\"ProximityStatus\":null,\"AmblightStatus\":null,\"EnhancedAudioStatus\":\"2\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"OpenCloseStatus\":\"1\",\"FasciaLightStatus\":null,\"AudioStatus\":\"1\",\"HeatingStatus\":null,\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"VolumeStatus\":1000,\"UpsStatus\":\"0\",\"GreenLedStatus\":\"1\",\"AmberLedStatus\":\"2\",\"RedLedStatus\":\"2\",\"AudibleAlarmStatus\":null,\"EnhancedAudioControlStatus\":\"2\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"

                  {\"CheckAcceptorStatus\":\"0\",\"MediaStatus\":\"1\",\"TonerStatus\":\"3\",\"InkStatus\":\"3\",\"FrontImageScannerStatus\":\"0\",\"BackImageScannerStatus\":\"0\",\"MICRReaderStatus\":\"0\",\"StackerStatus\":\"0\",\"ReBuncherStatus\":\"0\",\"MediaFeederStatus\":\"0\",\"PositionStatus_Input\":\"0\",\"PositionStatus_Output\":\"0\",\"PositionStatus_Refused\":\"0\",\"ShutterStatus_Input\":\"0\",\"ShutterStatus_Output\":\"0\",\"ShutterStatus_Refused\":\"0\",\"TransportStatus_Input\":\"0\",\"TransportStatus_Output { get; set; }\"TransportStatus_Refused { get; set; }\"TransportMediaStatus_Input\":\"0\",\"TransportMediaStatus_Output\":\"0\",\"TransportMediaStatus_Refused\":\"0\",\"DevicePositionStatus\":\"0\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CheckAcceptorStatus\":\"0\",\"MediaStatus\":\"1\",\"TonerStatus\":\"3\",\"InkStatus\":\"3\",\"FrontImageScannerStatus\":\"0\",\"BackImageScannerStatus\":\"0\",\"MICRReaderStatus\":\"0\",\"StackerStatus\":\"0\",\"ReBuncherStatus\":\"0\",\"MediaFeederStatus\":\"0\",\"PositionStatus_Input\":\"0\",\"PositionStatus_Output\":\"0\",\"PositionStatus_Refused\":\"0\",\"ShutterStatus_Input\":\"0\",\"ShutterStatus_Output\":\"0\",\"ShutterStatus_Refused\":\"0\",\"TransportStatus_Input\":\"0\",\"TransportStatus_Output { get; set; }\"TransportStatus_Refused { get; set; }\"TransportMediaStatus_Input\":\"0\",\"TransportMediaStatus_Output\":\"0\",\"TransportMediaStatus_Refused\":\"0\",\"DevicePositionStatus\":\"0\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CheckAcceptorStatus\":\"0\",\"MediaStatus\":\"1\",\"TonerStatus\":\"3\",\"InkStatus\":\"3\",\"FrontImageScannerStatus\":\"0\",\"BackImageScannerStatus\":\"0\",\"MICRReaderStatus\":\"0\",\"StackerStatus\":\"0\",\"ReBuncherStatus\":\"0\",\"MediaFeederStatus\":\"0\",\"PositionStatus_Input\":\"0\",\"PositionStatus_Output\":\"0\",\"PositionStatus_Refused\":\"0\",\"ShutterStatus_Input\":\"0\",\"ShutterStatus_Output\":\"0\",\"ShutterStatus_Refused\":\"0\",\"TransportStatus_Input\":\"0\",\"TransportStatus_Output { get; set; }\"TransportStatus_Refused { get; set; }\"TransportMediaStatus_Input\":\"0\",\"TransportMediaStatus_Output\":\"0\",\"TransportMediaStatus_Refused\":\"0\",\"DevicePositionStatus\":\"1\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CheckAcceptorStatus\":\"0\",\"MediaStatus\":\"1\",\"TonerStatus\":\"3\",\"InkStatus\":\"3\",\"FrontImageScannerStatus\":\"0\",\"BackImageScannerStatus\":\"0\",\"MICRReaderStatus\":\"0\",\"StackerStatus\":\"0\",\"ReBuncherStatus\":\"0\",\"MediaFeederStatus\":\"0\",\"PositionStatus_Input\":\"0\",\"PositionStatus_Output\":\"0\",\"PositionStatus_Refused\":\"0\",\"ShutterStatus_Input\":\"0\",\"ShutterStatus_Output\":\"0\",\"ShutterStatus_Refused\":\"0\",\"TransportStatus_Input\":\"0\",\"TransportStatus_Output { get; set; }\"TransportStatus_Refused { get; set; }\"TransportMediaStatus_Input\":\"0\",\"TransportMediaStatus_Output\":\"0\",\"TransportMediaStatus_Refused\":\"0\",\"DevicePositionStatus\":\"0\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                  {\"CheckAcceptorStatus\":\"0\",\"MediaStatus\":\"1\",\"TonerStatus\":\"3\",\"InkStatus\":\"3\",\"FrontImageScannerStatus\":\"0\",\"BackImageScannerStatus\":\"0\",\"MICRReaderStatus\":\"0\",\"StackerStatus\":\"0\",\"ReBuncherStatus\":\"0\",\"MediaFeederStatus\":\"0\",\"PositionStatus_Input\":\"0\",\"PositionStatus_Output\":\"0\",\"PositionStatus_Refused\":\"0\",\"ShutterStatus_Input\":\"0\",\"ShutterStatus_Output\":\"0\",\"ShutterStatus_Refused\":\"0\",\"TransportStatus_Input\":\"0\",\"TransportStatus_Output { get; set; }\"TransportStatus_Refused { get; set; }\"TransportMediaStatus_Input\":\"0\",\"TransportMediaStatus_Output\":\"0\",\"TransportMediaStatus_Refused\":\"0\",\"DevicePositionStatus\":\"1\",\"PowerSaveRecoveryTime\":0,\"LogicalServiceName { get; set; }\"ExtraInformation\":\"\"}"
                */

               //"{\"Id\":0,\"MacAddress\":\"00-01-2E-A5-F2-30\",\"DeviceID\":\"M\",\"DeviceClass\":null,\"DisplayName\":null,\"Status\":\"0\",\"AssetName\":null,\"MediaStatus\":null,\"DeviceSpecificStatus\":\"{\\\"AcceptorStatus\\\":\\\"0\\\",\\\"MediaStatus\\\":\\\"1\\\",\\\"TonerStatus\\\":\\\"0\\\",\\\"InkStatus\\\":\\\"3\\\",\\\"FrontImageScannerStatus\\\":\\\"0\\\",\\\"BackImageScannerStatus\\\":\\\"0\\\",\\\"MICRReaderStatus\\\":\\\"0\\\",\\\"StackerStatus\\\":\\\"0\\\",\\\"ReBuncherStatus\\\":\\\"0\\\",\\\"MediaFeederStatus\\\":\\\"0\\\",\\\"PositionStatus_Input\\\":\\\"0\\\",\\\"PositionStatus_Output\\\":\\\"0\\\",\\\"PositionStatus_Refused\\\":\\\"0\\\",\\\"ShutterStatus_Input\\\":\\\"0\\\",\\\"ShutterStatus_Output\\\":\\\"0\\\",\\\"ShutterStatus_Refused\\\":\\\"0\\\",\\\"TransportStatus_Input\\\":\\\"0\\\",\\\"TransportStatus_Output\\\":\\\"\\\",\\\"TransportStatus_Refused\\\":\\\"\\\",\\\"TransportMediaStatus_Input\\\":\\\"0\\\",\\\"TransportMediaStatus_Output\\\":\\\"0\\\",\\\"TransportMediaStatus_Refused\\\":\\\"0\\\",\\\"DevicePositionStatus\\\":\\\"0\\\",\\\"PowerSaveRecoveryTime\\\":0,\\\"LogicalServiceName\\\":\\\"\\\",\\\"ExtraInformation\\\":\\\"\\\"}\",\"Timestamp\":\"0001-01-01T00:00:00\"}"
               isRecognized = true;
               idx = subLogLine.IndexOf("{");

               string jsonPayload = subLogLine.Substring(idx);

               try
               {
                  dynamic dynamicDeviceState = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());

                  Id = dynamicDeviceState.Id;
                  MacAddress = dynamicDeviceState.MacAddress;
                  DeviceId = dynamicDeviceState.DeviceID;
                  DeviceClass = dynamicDeviceState.DeviceClass;
                  DisplayName = dynamicDeviceState.DisplayName;
                  Status = dynamicDeviceState.Status;
                  AssetName = dynamicDeviceState.AssetName;
                  DeviceMediaStatus = dynamicDeviceState.MediaStatus;

                  DeviceStateTimestampUTC = dynamicDeviceState.Timestamp.ToUniversalTime().ToString(LogLine.DateTimeFormatStringMsec);

                  try
                  {
                     dynamic dynamicDeviceSpecificStatus = JsonConvert.DeserializeObject<ExpandoObject>(dynamicDeviceState.DeviceSpecificStatus, new ExpandoObjectConverter());

                     KeyValuePair<string, object> first = ((IDictionary<String, object>)dynamicDeviceSpecificStatus).First();

                     switch (first.Key)
                     {
                        case "EncStatus":
                           /*
                              \"EncStatus\":\"0\",
                              \"DevicePositionStatus\":\"3\",
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                            */
                           EncStatus = dynamicDeviceSpecificStatus.EncStatus;
                           DevicePositionStatus = dynamicDeviceSpecificStatus.DevicePositionStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "SafeDoorStatus":
                           /*
                              \"SafeDoorStatus\":\"1\",
                              \"DispenserStatus\":\"1\",
                              \"IntermediateStackerStatus\":\"0\",
                              \"ShutterStatus\":\"0\",
                              \"PositionStatus\":\"0\",
                              \"TransportStatus\":\"0\",
                              \"TransportStatusStatus\":\"0\",
                              \"DevicePositionStatus\":\"3\",
                              \"PowerSaveRecoveryTime\":0,
                              \"UnitCurrencyID\":[\"   \",\"USD\",\"USD\",\"USD\",\"USD\"],
                              \"UnitValue\":[0,1,5,20,50],
                              \"UnitStatus\":[\"0\",\"0\",\"3\",\"0\",\"3\"],
                              \"UnitCount\":[0,999,274,3127,707],
                              \"UnitType\":[\"REJECTCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\",\"BILLCASSETTE\"],
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                            */
                           SafeDoorStatus = dynamicDeviceSpecificStatus.SafeDoorStatus;
                           DispenserStatus = dynamicDeviceSpecificStatus.DispenserStatus;
                           IntermediateStackerStatus = dynamicDeviceSpecificStatus.IntermediateStackerStatus;
                           ShutterStatus = dynamicDeviceSpecificStatus.ShutterStatus;
                           PositionStatus = dynamicDeviceSpecificStatus.PositionStatus;
                           TransportStatus = dynamicDeviceSpecificStatus.TransportStatus;
                           TransportStatusStatus = dynamicDeviceSpecificStatus.TransportStatusStatus;
                           DevicePositionStatus = dynamicDeviceSpecificStatus.DevicePositionStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;

                           // copy arrays
                           UnitCurrencyID = new List<string>();
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitCurrencyID) { UnitCurrencyID.Add(obj); }

                           UnitValue = new List<long>();
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitValue) { UnitValue.Add(obj); }

                           UnitStatus = new List<string>();
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitStatus) { UnitStatus.Add(obj); }

                           UnitCount = new List<long>();
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitCount) { UnitCount.Add(obj); }

                           UnitType = new List<string>();
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitType) { UnitType.Add(obj); }

                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "CabinetStatus":
                           /*
                              \"CabinetStatus\":\"1\",
                              \"SafeStatus\":\"1\",
                              \"VandalShieldStatus\":null,
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                           */
                           CabinetStatus = dynamicDeviceSpecificStatus.CabinetStatus;
                           SafeStatus = dynamicDeviceSpecificStatus.SafeStatus;
                           VandalShieldStatus = dynamicDeviceSpecificStatus.VandalShieldStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "MediaStatus":
                           /*
                            (1)
                              \"MediaStatus\":\"2\",
                              \"RetainBinStatus\":\"2\",
                              \"SecurityStatus\":\"1\",
                              \"NumberOfCardsRetained\":\"0\",
                              \"ChipPowerStatus\":\"5\",
                              \"DevicePositionStatus\":\"3\",
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"

                            (2)
                              \"MediaStatus\":\"0\",
                              \"PaperStatus\":[\"1\",null,null,null,null,null],
                              \"TonerStatus\":\"0\",
                              \"InkStatus\":\"3\",
                              \"LampStatus\":\"3\",
                              \"RetractBinStatus\":[],
                              \"MediaOnStacker\":0,
                              \"DevicePositionStatus\":\"3\",
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\",
                              \"ExtraInformation\":\"\"
                           */


                           MediaStatus = dynamicDeviceSpecificStatus.MediaStatus;

                           if (((IDictionary<String, object>)dynamicDeviceSpecificStatus).ContainsKey("RetainBinStatus"))
                           {
                              RetainBinStatus = dynamicDeviceSpecificStatus.RetainBinStatus;
                              SecurityStatus = dynamicDeviceSpecificStatus.SecurityStatus;
                              NumberOfCardsRetained = dynamicDeviceSpecificStatus.NumberOfCardsRetained;
                              ChipPowerStatus = dynamicDeviceSpecificStatus.ChipPowerStatus;
                           }
                           else
                           {
                              // copy arrays
                              PaperStatus = new List<string>();
                              foreach (var obj in dynamicDeviceSpecificStatus.PaperStatus) { PaperStatus.Add(obj); }

                              RetractBinStatus = new List<string>();
                              foreach (var obj in dynamicDeviceSpecificStatus.RetractBinStatus) { RetractBinStatus.Add(obj); }

                              Media_TonerStatus = dynamicDeviceSpecificStatus.TonerStatus;
                              Media_InkStatus = dynamicDeviceSpecificStatus.InkStatus;
                              LampStatus = dynamicDeviceSpecificStatus.LampStatus;
                              MediaOnStacker = dynamicDeviceSpecificStatus.MediaOnStacker;
                           }

                           Media_DevicePositionStatus = dynamicDeviceSpecificStatus.DevicePositionStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "CardUnitStatus":
                           /*
                              \"CardUnitStatus\":\"1\",
                              \"PinpadStatus\":\"1\",
                              \"NotesDispenserStatus\":\"1\",
                              \"CoinDispenserStatus\":\"1\",
                              \"ReceiptPrinterStatus\":\"1\",
                              \"PassbookPrinterStatus\":null,
                              \"EnvelopeDepositoryStatus\":null,
                              \"ChequeUnitStatus\":\"1\",
                              \"BillAcceptorStatus\":\"1\",
                              \"EnvelopeDispenserStatus\":null,
                              \"DocumentPrinterStatus\":\"1\",
                              \"CoinAcceptorStatus\":\"1\",
                              \"ScannerStatus\":\"1\",
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                           */
                           CardUnitStatus = dynamicDeviceSpecificStatus.CardUnitStatus;
                           PinpadStatus = dynamicDeviceSpecificStatus.PinpadStatus;
                           NotesDispenserStatus = dynamicDeviceSpecificStatus.NotesDispenserStatus;
                           CoinDispenserStatus = dynamicDeviceSpecificStatus.CoinDispenserStatus;
                           ReceiptPrinterStatus = dynamicDeviceSpecificStatus.ReceiptPrinterStatus;
                           PassbookPrinterStatus = dynamicDeviceSpecificStatus.PassbookPrinterStatus;
                           EnvelopeDepositoryStatus = dynamicDeviceSpecificStatus.EnvelopeDepositoryStatus;
                           ChequeUnitStatus = dynamicDeviceSpecificStatus.ChequeUnitStatus;
                           BillAcceptorStatus = dynamicDeviceSpecificStatus.BillAcceptorStatus;
                           EnvelopeDispenserStatus = dynamicDeviceSpecificStatus.EnvelopeDispenserStatus;
                           DocumentPrinterStatus = dynamicDeviceSpecificStatus.DocumentPrinterStatus;
                           CoinAcceptorStatus = dynamicDeviceSpecificStatus.CoinAcceptorStatus;
                           ScannerStatus = dynamicDeviceSpecificStatus.ScannerStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "OperatorSwitchStatus":
                           /*
                              \"OperatorSwitchStatus\":\"1\",
                              \"TamperStatus\":null,
                              \"IntTamperStatus\":null,
                              \"SeismicStatus\":null,
                              \"HeatStatus\":null,
                              \"ProximityStatus\":null,
                              \"AmblightStatus\":null,
                              \"EnhancedAudioStatus\":\"2\",
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                           */
                           OperatorSwitchStatus = dynamicDeviceSpecificStatus.OperatorSwitchStatus;
                           TamperStatus = dynamicDeviceSpecificStatus.TamperStatus;
                           IntTamperStatus = dynamicDeviceSpecificStatus.IntTamperStatus;
                           SeismicStatus = dynamicDeviceSpecificStatus.SeismicStatus;
                           HeatStatus = dynamicDeviceSpecificStatus.HeatStatus;
                           ProximityStatus = dynamicDeviceSpecificStatus.ProximityStatus;
                           AmblightStatus = dynamicDeviceSpecificStatus.AmblightStatus;
                           EnhancedAudioStatus = dynamicDeviceSpecificStatus.EnhancedAudioStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "OpenCloseStatus":
                           /*
                              \"OpenCloseStatus\":\"1\",
                              \"FasciaLightStatus\":null,
                              \"AudioStatus\":\"1\",
                              \"HeatingStatus\":null,
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                           */
                           OpenCloseStatus = dynamicDeviceSpecificStatus.OpenCloseStatus;
                           FasciaLightStatus = dynamicDeviceSpecificStatus.FasciaLightStatus;
                           AudioStatus = dynamicDeviceSpecificStatus.AudioStatus;
                           HeatingStatus = dynamicDeviceSpecificStatus.HeatingStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "VolumeStatus":
                           /*
                              \"VolumeStatus\":1000,
                              \"UpsStatus\":\"0\",
                              \"GreenLedStatus\":\"1\",
                              \"AmberLedStatus\":\"2\",
                              \"RedLedStatus\":\"2\",
                              \"AudibleAlarmStatus\":null,
                              \"EnhancedAudioControlStatus\":\"2\",
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                           */
                           VolumeStatus = dynamicDeviceSpecificStatus.VolumeStatus;
                           UpsStatus = dynamicDeviceSpecificStatus.UpsStatus;
                           GreenLedStatus = dynamicDeviceSpecificStatus.GreenLedStatus;
                           AmberLedStatus = dynamicDeviceSpecificStatus.AmberLedStatus;
                           RedLedStatus = dynamicDeviceSpecificStatus.RedLedStatus;
                           AudibleAlarmStatus = dynamicDeviceSpecificStatus.AudibleAlarmStatus;
                           EnhancedAudioControlStatus = dynamicDeviceSpecificStatus.EnhancedAudioControlStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        case "AcceptorStatus":
                           //{\\\"AcceptorStatus\\\":\\\"0\\\",
                           //\\\"MediaStatus\\\":\\\"1\\\",
                           //\\\"TonerStatus\\\":\\\"0\\\",
                           //\\\"InkStatus\\\":\\\"3\\\",
                           //\\\"FrontImageScannerStatus\\\":\\\"0\\\",
                           //\\\"BackImageScannerStatus\\\":\\\"0\\\",
                           //\\\"MICRReaderStatus\\\":\\\"0\\\",
                           //\\\"StackerStatus\\\":\\\"0\\\",
                           //\\\"ReBuncherStatus\\\":\\\"0\\\",
                           //\\\"MediaFeederStatus\\\":\\\"0\\\",
                           //\\\"PositionStatus_Input\\\":\\\"0\\\",
                           //\\\"PositionStatus_Output\\\":\\\"0\\\",
                           //\\\"PositionStatus_Refused\\\":\\\"0\\\",
                           //\\\"ShutterStatus_Input\\\":\\\"0\\\",
                           //\\\"ShutterStatus_Output\\\":\\\"0\\\",
                           //\\\"ShutterStatus_Refused\\\":\\\"0\\\",
                           //\\\"TransportStatus_Input\\\":\\\"0\\\",
                           //\\\"TransportStatus_Output\\\":\\\"\\\",
                           //\\\"TransportStatus_Refused\\\":\\\"\\\",
                           //\\\"TransportMediaStatus_Input\\\":\\\"0\\\",
                           //\\\"TransportMediaStatus_Output\\\":\\\"0\\\",
                           //\\\"TransportMediaStatus_Refused\\\":\\\"0\\\",
                           //\\\"DevicePositionStatus\\\":\\\"0\\\",
                           //\\\"PowerSaveRecoveryTime\\\":0,
                           //\\\"LogicalServiceName\\\":\\\"\\\",
                           //\\\"ExtraInformation\\\":\\\"\\\"}"
                           string val = dynamicDeviceSpecificStatus.AcceptorStatus;
                           val = dynamicDeviceSpecificStatus.MediaStatus;
                           val = dynamicDeviceSpecificStatus.TonerStatus;
                           val = dynamicDeviceSpecificStatus.InkStatus;
                           val = dynamicDeviceSpecificStatus.FrontImageScannerStatus;
                           val = dynamicDeviceSpecificStatus.BackImageScannerStatus;
                           val = dynamicDeviceSpecificStatus.MICRReaderStatus;
                           val = dynamicDeviceSpecificStatus.StackerStatus;
                           val = dynamicDeviceSpecificStatus.ReBuncherStatus;
                           val = dynamicDeviceSpecificStatus.MediaFeederStatus;
                           val = dynamicDeviceSpecificStatus.PositionStatus_Input;
                           val = dynamicDeviceSpecificStatus.PositionStatus_Output;
                           val = dynamicDeviceSpecificStatus.PositionStatus_Refused;
                           val = dynamicDeviceSpecificStatus.ShutterStatus_Input;
                           val = dynamicDeviceSpecificStatus.ShutterStatus_Output;
                           val = dynamicDeviceSpecificStatus.ShutterStatus_Refused;
                           val = dynamicDeviceSpecificStatus.TransportStatus_Input;
                           val = dynamicDeviceSpecificStatus.TransportStatus_Output;
                           val = dynamicDeviceSpecificStatus.TransportStatus_Refused;
                           val = dynamicDeviceSpecificStatus.TransportMediaStatus_Input;
                           val = dynamicDeviceSpecificStatus.TransportMediaStatus_Output;
                           val = dynamicDeviceSpecificStatus.TransportMediaStatus_Refused;
                           val = dynamicDeviceSpecificStatus.DevicePositionStatus;
                           long recoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           val = dynamicDeviceSpecificStatus.LogicalServiceName;
                           val = dynamicDeviceSpecificStatus.ExtraInformation;
                           break;

                        case "CheckAcceptorStatus":
                           /*
                              \"CheckAcceptorStatus\":\"0\",
                              \"MediaStatus\":\"1\",
                              \"TonerStatus\":\"3\",
                              \"InkStatus\":\"3\",
                              \"FrontImageScannerStatus\":\"0\",
                              \"BackImageScannerStatus\":\"0\",
                              \"MICRReaderStatus\":\"0\",
                              \"StackerStatus\":\"0\",
                              \"ReBuncherStatus\":\"0\",
                              \"MediaFeederStatus\":\"0\",
                              \"PositionStatus_Input\":\"0\",
                              \"PositionStatus_Output\":\"0\",
                              \"PositionStatus_Refused\":\"0\",
                              \"ShutterStatus_Input\":\"0\",
                              \"ShutterStatus_Output\":\"0\",
                              \"ShutterStatus_Refused\":\"0\",
                              \"TransportStatus_Input\":\"0\",
                              \"TransportStatus_Output\":\"0\",
                              \"TransportStatus_Refused\":\"0\",
                              \"TransportMediaStatus_Input\":\"0\",
                              \"TransportMediaStatus_Output\":\"0\",
                              \"TransportMediaStatus_Refused\":\"0\",
                              \"DevicePositionStatus\":\"0\",
                              \"PowerSaveRecoveryTime\":0,
                              \"LogicalServiceName\":\"\"
                              \"ExtraInformation\":\"\"
                           */
                           CheckAcceptorStatus = dynamicDeviceSpecificStatus.CheckAcceptorStatus;
                           MediaStatus = dynamicDeviceSpecificStatus.MediaStatus;
                           TonerStatus = dynamicDeviceSpecificStatus.TonerStatus;
                           InkStatus = dynamicDeviceSpecificStatus.InkStatus;
                           FrontImageScannerStatus = dynamicDeviceSpecificStatus.FrontImageScannerStatus;
                           BackImageScannerStatus = dynamicDeviceSpecificStatus.BackImageScannerStatus;
                           MICRReaderStatus = dynamicDeviceSpecificStatus.MICRReaderStatus;
                           StackerStatus = dynamicDeviceSpecificStatus.StackerStatus;
                           ReBuncherStatus = dynamicDeviceSpecificStatus.ReBuncherStatus;
                           MediaFeederStatus = dynamicDeviceSpecificStatus.MediaFeederStatus;
                           PositionStatus_Input = dynamicDeviceSpecificStatus.PositionStatus_Input;
                           PositionStatus_Output = dynamicDeviceSpecificStatus.PositionStatus_Output;
                           PositionStatus_Refused = dynamicDeviceSpecificStatus.PositionStatus_Refused;
                           ShutterStatus_Input = dynamicDeviceSpecificStatus.ShutterStatus_Input;
                           ShutterStatus_Output = dynamicDeviceSpecificStatus.ShutterStatus_Output;
                           ShutterStatus_Refused = dynamicDeviceSpecificStatus.ShutterStatus_Refused;
                           TransportStatus_Input = dynamicDeviceSpecificStatus.TransportStatus_Input;
                           TransportStatus_Output = dynamicDeviceSpecificStatus.TransportStatus_Output;
                           TransportStatus_Refused = dynamicDeviceSpecificStatus.TransportStatus_Refused;
                           TransportMediaStatus_Input = dynamicDeviceSpecificStatus.TransportMediaStatus_Input;
                           TransportMediaStatus_Output = dynamicDeviceSpecificStatus.TransportMediaStatus_Output;
                           TransportMediaStatus_Refused = dynamicDeviceSpecificStatus.TransportMediaStatus_Refused;
                           DevicePositionStatus = dynamicDeviceSpecificStatus.DevicePositionStatus;
                           PowerSaveRecoveryTime = dynamicDeviceSpecificStatus.PowerSaveRecoveryTime;
                           LogicalServiceName = dynamicDeviceSpecificStatus.LogicalServiceName;
                           ExtraInformation = dynamicDeviceSpecificStatus.ExtraInformation;
                           // set last to indicate success
                           StatusDeviceName = first.Key;
                           break;

                        default:
                           // not yet unsupported
                           throw new Exception($"{first.Key} is not supported");
                     }


                  }
                  catch (Exception ex)
                  {
                     throw new Exception($"AELogLine.NetwareExtension: failed to deserialize inner Json payload for log line '{logLine}'\n{ex}");
                  }
               }
               catch (Exception ex)
               {
                  throw new Exception($"AELogLine.NetwareExtension: failed to deserialize outer Json payload for log line '{logLine}'\n{ex}");
               }
            }

            else
            {
               //NextwareExtension : Start
               //NextwareExtension : End
               Regex regex = new Regex("NextwareExtension : (?<action>.*)$");
               Match m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  // TODO
               }

               //StartMonitoring [start] .....................
               //StartMonitoring [end] .....................
               regex = new Regex("StartMonitoring \\[(?<action>.*)\\] \\.*$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  MonitoringDeviceChanges = $"MONITORING {m.Groups["action"].Value.ToUpper()}ED";
               }

               //NextwareExtension : OnDeviceStatusChanged: NH.Agent.Extensions.Nextware.NXDoors
               regex = new Regex("NextwareExtension : OnDeviceStatusChanged: (?<device>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  MonitoringDeviceChanges = "STATUS CHANGED";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
               }

               //Calling OpenSessionSync: Device=NXVendorDependentModeXClass
               regex = new Regex("Calling OpenSessionSync: Device=(?<device>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  MonitoringDeviceChanges = "SYNC";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
               }

               //Calling OpenSession: Device=NXPin
               regex = new Regex("Calling OpenSession: Device = (?<device>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  MonitoringDeviceChanges = "OPEN SESSION";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
               }

               //OpenSession succeeded: Device=NXPin, Elapsed=00:00:00.0200079
               regex = new Regex("OpenSession succeeded: Device = (?<device>.*), Elapsed = (?<timespan>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  isRecognized = true;
                  MonitoringDeviceChanges = "OPEN SESSION SUCCEEDED";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
                  MonitoringElapsed = m.Groups["timespan"].Value;

               }
            }
         }

         if (!isRecognized)
         {
            throw new Exception($"AELogLine.NetwareExtension: did not recognize the log line '{logLine}'");
         }
      }
   }
}
