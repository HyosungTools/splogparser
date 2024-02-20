using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LogLineHandler
{
   public class NextwareExtension : AELine
   {
      // Start/End
      public bool ExtensionRunning = false;

      public string MonitoringDeviceChanges { get; set; } = string.Empty;
      public string MonitoringDeviceName { get; set; } = string.Empty;
      public string MonitoringElapsed { get; set; } = string.Empty;
      public long Id { get; set; }
      public string MacAddress { get; set; } = String.Empty;
      public string DeviceId { get; set; } = string.Empty;
      public string DeviceClass { get; set; } = string.Empty;
      public string DisplayName { get; set; } = string.Empty;
      public string Status { get; set; } = string.Empty;
      public string AssetName { get; set; } = string.Empty;
      public string DeviceMediaStatus { get; set; } = string.Empty;  // renamed to avoid conflict with MediaStatus
      public string DeviceStateTimestampUTC { get; set; } = string.Empty;    // new to avoid conflict with parent class property
      public string StatusDeviceName { get; set; } = string.Empty;
      public string DeviceStatus {  get; set; } = string.Empty;


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
          //2023-11-20 03:08:17 [NextwareExtension] DetermineDevicesToMonitor: Start
          */

         int idx = logLine.IndexOf("[NextwareExtension]");
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + "[NextwareExtension]".Length + 1);

            //Started monitoring device status changes.
            if (subLogLine == "Started monitoring device status changes.")
            {
               IsRecognized = true;
               MonitoringDeviceChanges = "STARTED";
            }

            else if (subLogLine == "DetermineDevicesToMonitor: Start")
            {
               IsRecognized = true;
               MonitoringDeviceChanges = "FIND DEVICES TO MONITOR";
            }

            else if (subLogLine == "DetermineDevicesToMonitor: End")
            {
               IsRecognized = true;
               MonitoringDeviceChanges = "FINISHED FINDING DEVICES TO MONITOR";
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

                     // build a string describing the status
                     StringBuilder sb = new StringBuilder();

                     StatusDeviceName = first.Key;

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
                           sb.Append($"EncStatus: {dynamicDeviceSpecificStatus.EncStatus},");
                           sb.Append($"DevicePositionStatus: {dynamicDeviceSpecificStatus.DevicePositionStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");
                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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
                           sb.Append($"SafeDoorStatus: {dynamicDeviceSpecificStatus.SafeDoorStatus},");
                           sb.Append($"DispenserStatus: {dynamicDeviceSpecificStatus.DispenserStatus},");
                           sb.Append($"IntermediateStackerStatus: {dynamicDeviceSpecificStatus.IntermediateStackerStatus},");
                           sb.Append($"ShutterStatus: {dynamicDeviceSpecificStatus.ShutterStatus},");
                           sb.Append($"PositionStatus: {dynamicDeviceSpecificStatus.PositionStatus},");
                           sb.Append($"TransportStatus: {dynamicDeviceSpecificStatus.TransportStatus},");
                           sb.Append($"TransportStatusStatus: {dynamicDeviceSpecificStatus.TransportStatusStatus},");
                           sb.Append($"DevicePositionStatus: {dynamicDeviceSpecificStatus.DevicePositionStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append("UnitCurrencyID: [");
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitCurrencyID)
                           {
                              sb.Append($"{obj},");
                           }
                           sb.Append("],");

                           sb.Append("UnitValue: [");
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitValue)
                           {
                              sb.Append($"{obj},");
                           }
                           sb.Append("],");

                           sb.Append("UnitStatus: [");
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitStatus)
                           {
                              sb.Append($"{obj},");
                           }
                           sb.Append("],");

                           sb.Append("UnitCount: [");
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitCount)
                           {
                              sb.Append($"{obj},");
                           }
                           sb.Append("],");

                           sb.Append("UnitType: [");
                           foreach (var obj in dynamicDeviceSpecificStatus.UnitType)
                           {
                              sb.Append($"{obj},");
                           }
                           sb.Append("],");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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
                           sb.Append($"CabinetStatus: {dynamicDeviceSpecificStatus.CabinetStatus},");
                           sb.Append($"SafeStatus: {dynamicDeviceSpecificStatus.SafeStatus},");
                           sb.Append($"VandalShieldStatus: {dynamicDeviceSpecificStatus.VandalShieldStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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

                           sb.Append($"MediaStatus: {dynamicDeviceSpecificStatus.MediaStatus},");

                           if (((IDictionary<String, object>)dynamicDeviceSpecificStatus).ContainsKey("RetainBinStatus"))
                           {
                              sb.Append($"RetainBinStatus: {dynamicDeviceSpecificStatus.RetainBinStatus},");
                              sb.Append($"SecurityStatus: {dynamicDeviceSpecificStatus.SecurityStatus},");
                              sb.Append($"NumberOfCardsRetained: {dynamicDeviceSpecificStatus.NumberOfCardsRetained},");
                              sb.Append($"ChipPowerStatus: {dynamicDeviceSpecificStatus.ChipPowerStatus},");
                           }
                           else
                           {
                              // copy arrays
                              sb.Append("PaperStatus: [");
                              foreach (var obj in dynamicDeviceSpecificStatus.PaperStatus)
                              {
                                 sb.Append($"{obj},");
                              }
                              sb.Append("],");


                              sb.Append("RetractBinStatus: [");
                              foreach (var obj in dynamicDeviceSpecificStatus.RetractBinStatus)
                              {
                                 sb.Append($"{obj},");
                              }
                              sb.Append("],");

                              sb.Append($"Media_TonerStatus: {dynamicDeviceSpecificStatus.TonerStatus},");
                              sb.Append($"Media_InkStatus: {dynamicDeviceSpecificStatus.InkStatus},");
                              sb.Append($"LampStatus: {dynamicDeviceSpecificStatus.LampStatus},");
                              sb.Append($"MediaOnStacker: {dynamicDeviceSpecificStatus.MediaOnStacker},");
                           }

                           sb.Append($"Media_DevicePositionStatus: {dynamicDeviceSpecificStatus.DevicePositionStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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
                           sb.Append($"CardUnitStatus: {dynamicDeviceSpecificStatus.CardUnitStatus},");
                           sb.Append($"PinpadStatus: {dynamicDeviceSpecificStatus.PinpadStatus},");
                           sb.Append($"NotesDispenserStatus: {dynamicDeviceSpecificStatus.NotesDispenserStatus},");
                           sb.Append($"CoinDispenserStatus: {dynamicDeviceSpecificStatus.CoinDispenserStatus},");
                           sb.Append($"ReceiptPrinterStatus: {dynamicDeviceSpecificStatus.ReceiptPrinterStatus},");
                           sb.Append($"PassbookPrinterStatus: {dynamicDeviceSpecificStatus.PassbookPrinterStatus},");
                           sb.Append($"EnvelopeDepositoryStatus: {dynamicDeviceSpecificStatus.EnvelopeDepositoryStatus},");
                           sb.Append($"ChequeUnitStatus: {dynamicDeviceSpecificStatus.ChequeUnitStatus},");
                           sb.Append($"BillAcceptorStatus: {dynamicDeviceSpecificStatus.BillAcceptorStatus},");
                           sb.Append($"EnvelopeDispenserStatus: {dynamicDeviceSpecificStatus.EnvelopeDispenserStatus},");
                           sb.Append($"DocumentPrinterStatus: {dynamicDeviceSpecificStatus.DocumentPrinterStatus},");
                           sb.Append($"CoinAcceptorStatus: {dynamicDeviceSpecificStatus.CoinAcceptorStatus},");
                           sb.Append($"ScannerStatus: {dynamicDeviceSpecificStatus.ScannerStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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
                           sb.Append($"OperatorSwitchStatus: {dynamicDeviceSpecificStatus.OperatorSwitchStatus},");
                           sb.Append($"TamperStatus: {dynamicDeviceSpecificStatus.TamperStatus},");
                           sb.Append($"IntTamperStatus: {dynamicDeviceSpecificStatus.IntTamperStatus},");
                           sb.Append($"SeismicStatus: {dynamicDeviceSpecificStatus.SeismicStatus},");
                           sb.Append($"HeatStatus: {dynamicDeviceSpecificStatus.HeatStatus},");
                           sb.Append($"ProximityStatus: {dynamicDeviceSpecificStatus.ProximityStatus},");
                           sb.Append($"AmblightStatus: {dynamicDeviceSpecificStatus.AmblightStatus},");
                           sb.Append($"EnhancedAudioStatus: {dynamicDeviceSpecificStatus.EnhancedAudioStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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
                           sb.Append($"OpenCloseStatus: {dynamicDeviceSpecificStatus.OpenCloseStatus},");
                           sb.Append($"FasciaLightStatus: {dynamicDeviceSpecificStatus.FasciaLightStatus},");
                           sb.Append($"AudioStatus: {dynamicDeviceSpecificStatus.AudioStatus},");
                           sb.Append($"HeatingStatus: {dynamicDeviceSpecificStatus.HeatingStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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
                           sb.Append($"VolumeStatus: {dynamicDeviceSpecificStatus.VolumeStatus},");
                           sb.Append($"UpsStatus: {dynamicDeviceSpecificStatus.UpsStatus},");
                           sb.Append($"GreenLedStatus: {dynamicDeviceSpecificStatus.GreenLedStatus},");
                           sb.Append($"AmberLedStatus: {dynamicDeviceSpecificStatus.AmberLedStatus},");
                           sb.Append($"RedLedStatus: {dynamicDeviceSpecificStatus.RedLedStatus},");
                           sb.Append($"AudibleAlarmStatus: {dynamicDeviceSpecificStatus.AudibleAlarmStatus},");
                           sb.Append($"EnhancedAudioControlStatus: {dynamicDeviceSpecificStatus.EnhancedAudioControlStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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

                           sb.Append($"AcceptorStatus: {dynamicDeviceSpecificStatus.AcceptorStatus},");
                           sb.Append($"MediaStatus: {dynamicDeviceSpecificStatus.MediaStatus},");
                           sb.Append($"TonerStatus: {dynamicDeviceSpecificStatus.TonerStatus},");
                           sb.Append($"InkStatus: {dynamicDeviceSpecificStatus.InkStatus},");
                           sb.Append($"FrontImageScannerStatus: {dynamicDeviceSpecificStatus.FrontImageScannerStatus},");
                           sb.Append($"BackImageScannerStatus: {dynamicDeviceSpecificStatus.BackImageScannerStatus},");
                           sb.Append($"MICRReaderStatus: {dynamicDeviceSpecificStatus.MICRReaderStatus},");
                           sb.Append($"StackerStatus: {dynamicDeviceSpecificStatus.StackerStatus},");
                           sb.Append($"ReBuncherStatus: {dynamicDeviceSpecificStatus.ReBuncherStatus},");
                           sb.Append($"MediaFeederStatus: {dynamicDeviceSpecificStatus.MediaFeederStatus},");
                           sb.Append($"PositionStatus_Input: {dynamicDeviceSpecificStatus.PositionStatus_Input},");
                           sb.Append($"PositionStatus_Output: {dynamicDeviceSpecificStatus.PositionStatus_Output},");
                           sb.Append($"PositionStatus_Refused: {dynamicDeviceSpecificStatus.PositionStatus_Refused},");
                           sb.Append($"ShutterStatus_Input: {dynamicDeviceSpecificStatus.ShutterStatus_Input},");
                           sb.Append($"ShutterStatus_Output: {dynamicDeviceSpecificStatus.ShutterStatus_Output},");
                           sb.Append($"ShutterStatus_Refused: {dynamicDeviceSpecificStatus.ShutterStatus_Refused},");
                           sb.Append($"TransportStatus_Input: {dynamicDeviceSpecificStatus.TransportStatus_Input},");
                           sb.Append($"TransportStatus_Output: {dynamicDeviceSpecificStatus.TransportStatus_Output},");
                           sb.Append($"TransportStatus_Refused: {dynamicDeviceSpecificStatus.TransportStatus_Refused},");
                           sb.Append($"TransportMediaStatus_Input: {dynamicDeviceSpecificStatus.TransportMediaStatus_Input},");
                           sb.Append($"TransportMediaStatus_Output: {dynamicDeviceSpecificStatus.TransportMediaStatus_Output},");
                           sb.Append($"TransportMediaStatus_Refused: {dynamicDeviceSpecificStatus.TransportMediaStatus_Refused},");
                           sb.Append($"DevicePositionStatus: {dynamicDeviceSpecificStatus.DevicePositionStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
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
                           sb.Append($"CheckAcceptorStatus: {dynamicDeviceSpecificStatus.CheckAcceptorStatus},");
                           sb.Append($"MediaStatus: {dynamicDeviceSpecificStatus.MediaStatus},");
                           sb.Append($"TonerStatus: {dynamicDeviceSpecificStatus.TonerStatus},");
                           sb.Append($"InkStatus: {dynamicDeviceSpecificStatus.InkStatus},");
                           sb.Append($"FrontImageScannerStatus: {dynamicDeviceSpecificStatus.FrontImageScannerStatus},");
                           sb.Append($"BackImageScannerStatus: {dynamicDeviceSpecificStatus.BackImageScannerStatus},");
                           sb.Append($"MICRReaderStatus: {dynamicDeviceSpecificStatus.MICRReaderStatus},");
                           sb.Append($"StackerStatus: {dynamicDeviceSpecificStatus.StackerStatus},");
                           sb.Append($"ReBuncherStatus: {dynamicDeviceSpecificStatus.ReBuncherStatus},");
                           sb.Append($"MediaFeederStatus: {dynamicDeviceSpecificStatus.MediaFeederStatus},");
                           sb.Append($"PositionStatus_Input: {dynamicDeviceSpecificStatus.PositionStatus_Input},");
                           sb.Append($"PositionStatus_Output: {dynamicDeviceSpecificStatus.PositionStatus_Output},");
                           sb.Append($"PositionStatus_Refused: {dynamicDeviceSpecificStatus.PositionStatus_Refused},");
                           sb.Append($"ShutterStatus_Input: {dynamicDeviceSpecificStatus.ShutterStatus_Input},");
                           sb.Append($"ShutterStatus_Output: {dynamicDeviceSpecificStatus.ShutterStatus_Output},");
                           sb.Append($"ShutterStatus_Refused: {dynamicDeviceSpecificStatus.ShutterStatus_Refused},");
                           sb.Append($"TransportStatus_Input: {dynamicDeviceSpecificStatus.TransportStatus_Input},");
                           sb.Append($"TransportStatus_Output: {dynamicDeviceSpecificStatus.TransportStatus_Output},");
                           sb.Append($"TransportStatus_Refused: {dynamicDeviceSpecificStatus.TransportStatus_Refused},");
                           sb.Append($"TransportMediaStatus_Input: {dynamicDeviceSpecificStatus.TransportMediaStatus_Input},");
                           sb.Append($"TransportMediaStatus_Output: {dynamicDeviceSpecificStatus.TransportMediaStatus_Output},");
                           sb.Append($"TransportMediaStatus_Refused: {dynamicDeviceSpecificStatus.TransportMediaStatus_Refused},");
                           sb.Append($"DevicePositionStatus: {dynamicDeviceSpecificStatus.DevicePositionStatus},");
                           sb.Append($"PowerSaveRecoveryTime: {dynamicDeviceSpecificStatus.PowerSaveRecoveryTime},");

                           sb.Append($"LogicalServiceName: {dynamicDeviceSpecificStatus.LogicalServiceName},");
                           sb.Append($"ExtraInformation: {dynamicDeviceSpecificStatus.ExtraInformation},");

                           DeviceStatus = sb.ToString();
                           IsRecognized = true;
                           break;

                        default:
                           // not yet unsupported
                           throw new Exception($"{first.Key} device-state is not supported");
                     }
                  }
                  catch (Exception ex)
                  {
                     throw new Exception($"AELogLine.NextwareExtension: failed to deserialize inner Json payload for log line '{logLine}'\n{ex}");
                  }
               }
               catch (Exception ex)
               {
                  throw new Exception($"AELogLine.NextwareExtension: failed to deserialize outer Json payload for log line '{logLine}'\n{ex}");
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
                  if (m.Groups["action"].Value == "Start")
                  {
                     IsRecognized = true;
                     ExtensionRunning = true;
                  }
                  else if(m.Groups["action"].Value == "End")
                  {
                     IsRecognized = true;
                     ExtensionRunning = false;
                  }
               }

               //StartMonitoring [start] .....................
               //StartMonitoring [end] .....................
               regex = new Regex("StartMonitoring \\[(?<action>.*)\\] \\.*$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  IsRecognized = true;
                  MonitoringDeviceChanges = $"MONITORING {m.Groups["action"].Value.ToUpper()}ED";
               }

               //NextwareExtension : OnDeviceStatusChanged: NH.Agent.Extensions.Nextware.NXDoors
               regex = new Regex("NextwareExtension : OnDeviceStatusChanged: (?<device>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  IsRecognized = true;
                  MonitoringDeviceChanges = "STATUS CHANGED";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
               }

               //Calling OpenSessionSync: Device=NXVendorDependentModeXClass
               regex = new Regex("Calling OpenSessionSync: Device=(?<device>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  IsRecognized = true;
                  MonitoringDeviceChanges = "SYNC";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
               }

               //Calling OpenSession: Device=NXPin
               regex = new Regex("Calling OpenSession: Device = (?<device>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  IsRecognized = true;
                  MonitoringDeviceChanges = "OPEN SESSION";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
               }

               //OpenSession succeeded: Device=NXPin, Elapsed=00:00:00.0200079
               regex = new Regex("OpenSession succeeded: Device = (?<device>.*), Elapsed = (?<timespan>.*)$");
               m = regex.Match(subLogLine);
               if (m.Success)
               {
                  IsRecognized = true;
                  MonitoringDeviceChanges = "OPEN SESSION SUCCEEDED";
                  MonitoringDeviceName = $"{m.Groups["device"].Value}";
                  MonitoringElapsed = m.Groups["timespan"].Value;
               }
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"AELogLine.NextwareExtension: did not recognize the log line '{logLine}'");
         }
      }
   }
}
