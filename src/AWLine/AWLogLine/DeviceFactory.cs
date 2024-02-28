using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;
using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LogLineHandler
{
   public class DeviceFactory : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "DeviceFactory";


      public DeviceFactory(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.DeviceFactory) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[DeviceFactory       ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //Bill dispenser device state {"SafeDoorStatus":"1","DispenserStatus":"0","IntermediateStackerStatus":"0","ShutterStatus":"0","PositionStatus":"0","TransportStatus":"0","TransportStatusStatus":"0","DevicePositionStatus":"0","PowerSaveRecoveryTime":0,"UnitCurrencyID":["   ","   ","USD","USD","USD","USD"],"UnitValue":[0,0,1,5,20,100],"UnitStatus":["0","4","0","0","0","0"],"UnitCount":[0,0,1000,1000,1000,1600],"UnitType":["RETRACTCASSETTE","REJECTCASSETTE","RECYCLING","RECYCLING","RECYCLING","RECYCLING"],"LogicalServiceName":"","ExtraInformation":""}
            //Bill acceptor device state {"SafeDoorStatus":"1","AcceptorStatus":"0","IntermediateStackerStatus":"0","BankNoteReaderStatus":"0","PositionStatus":{"4":"UNKNOWN","512":"UNKNOWN"},"ShutterStatus":"0","TransportStatus":"0","DevicePositionStatus":"0","PowerSaveRecoveryTime":0,"IsJCM":false,"ShortName":"BNA","LogicalServiceName":"","ExtraInformation":""}
            //Coin dispenser device state {"SafeDoorStatus":"1","DispenserStatus":"0","IntermediateStackerStatus":"5","ShutterStatus":"4","PositionStatus":"3","TransportStatus":"3","TransportStatusStatus":"4","DevicePositionStatus":"3","PowerSaveRecoveryTime":0,"UnitCurrencyID":["USD","USD","USD","USD"],"UnitValue":[1,5,10,25],"UnitStatus":["0","0","0","0"],"UnitCount":[598,300,99,236],"UnitType":["COINDISPENSER","COINDISPENSER","COINDISPENSER","COINDISPENSER"],"LogicalServiceName":"","ExtraInformation":""}
            //Item processor device state {"AcceptorStatus":"0","MediaStatus":"1","TonerStatus":"0","InkStatus":"3","FrontImageScannerStatus":"0","BackImageScannerStatus":"0","MICRReaderStatus":"0","StackerStatus":"0","ReBuncherStatus":"0","MediaFeederStatus":"0","PositionStatus_Input":"0","PositionStatus_Output":"0","PositionStatus_Refused":"0","ShutterStatus_Input":"0","ShutterStatus_Output":"0","ShutterStatus_Refused":"0","TransportStatus_Input":"0","TransportStatus_Output":"","TransportStatus_Refused":"","TransportMediaStatus_Input":"0","TransportMediaStatus_Output":"0","TransportMediaStatus_Refused":"0","DevicePositionStatus":"0","PowerSaveRecoveryTime":0,"LogicalServiceName":"","ExtraInformation":""}

            /*
            string subtag = "UserDeviceFactory Path: ";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("UserDeviceFactoryPath", subLogLine.Substring(subtag.Length));
               IsRecognized = true;
            }
            */

            Regex regex = new Regex("Bill (?<device>.*) device state (?<json>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               string jsonPayload = m.Groups["json"].Value;
               IsRecognized = true;

               try
               {
                  dynamic dynamicObject = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());
               }
               catch (Exception ex)
               {
                  throw new Exception($"AWLogLine.{className}: failed to deserialize Bill dispenser device Json payload for log line '{logLine}'\n{ex}");
               }

               SettingDict.Add("DeviceState", $"Bill {m.Groups["device"].Value} device");
               SettingDict.Add("Json", jsonPayload);
            }

            regex = new Regex("Coin (?<device>.*) device state (?<json>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               string jsonPayload = m.Groups["json"].Value;
               IsRecognized = true;

               try
               {
                  dynamic dynamicObject = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());
               }
               catch (Exception ex)
               {
                  throw new Exception($"AWLogLine.{className}: failed to deserialize Coin dispenser device Json payload for log line '{logLine}'\n{ex}");
               }

               SettingDict.Add("DeviceState", $"Coin {m.Groups["device"].Value} device");
               SettingDict.Add("Json", jsonPayload);
            }

            regex = new Regex("Item processor device state (?<json>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               string jsonPayload = m.Groups["json"].Value;
               IsRecognized = true;

               try
               {
                  dynamic dynamicObject = JsonConvert.DeserializeObject<ExpandoObject>(jsonPayload, new ExpandoObjectConverter());
               }
               catch (Exception ex)
               {
                  throw new Exception($"AWLogLine.{className}: failed to deserialize Item processor device Json payload for log line '{logLine}'\n{ex}");
               }

               SettingDict.Add("DeviceState", "Item Processor device");
               SettingDict.Add("Json", jsonPayload);
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
