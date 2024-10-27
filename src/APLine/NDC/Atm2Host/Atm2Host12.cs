using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host12 : Atm2Host
   {
      public Atm2Host12(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_ATM2HOST12) : base(parent, logLine, apType)
      {
         msgclass = "1";
         msgsubclass = "2"; 
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {
            // Field b and c - Message Class - 12
            (bool success, string field, string subMessage) result = GetNextFieldBySeparator(ndcmsg);
            if (!result.success)
               return ;

            english = "Unsolicited Status to Host, ";

            // Field d - LUNO
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            // Field - none - two FS back to back
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
            {
               return;
            }

            // Field e1 & e2 - Device Id Graphic and Device Status
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success && result.field == string.Empty)
            {
               return;
            }

            // Isolate DIG
            string deviceId = result.field.Substring(0, 1);

            //if (deviceId == "B")
            //{
            //   ;
            //}
            //if (deviceId == "D")
            //{
            //   ;
            //}
            //if (deviceId == "E")
            //{
            //   ;
            //}
            //if (deviceId == "F")
            //{
            //   ;
            //}
            //if (deviceId == "G")
            //{
            //   ;
            //}
            //if (deviceId == "H")
            //{
            //   ;
            //}
            //if (deviceId == "K")
            //{
            //   ;
            //}
            //if (deviceId == "L")
            //{
            //   ;
            //}
            if (deviceId == "P")
            {
               if (result.field.Length == 1)
               {
                  english = english + String.Format("Device Id : {0}, ", deviceId + " (Sensors) ");
               }
               else
               {
                  english = english + String.Format("Device Id : {0}, Device Status : {1}", deviceId + " (Sensors) ", UnsolSensor_e2(result.field.Substring(1)));
               }
            }
            //if (deviceId == "Q")
            //{
            //   ;
            //}
            //if (deviceId == "R")
            //{
            //   ;
            //}
            //if (deviceId == "V")
            //{
            //   ;
            //}
            //if (deviceId == "Y")
            //{
            //   ;
            //}
            //if (deviceId == "d")
            //{
            //   ;
            //}
            //if (deviceId == "e")
            //{
            //   ;
            //}
            //if (deviceId == "f")
            //{
            //   ;
            //}
            //if (deviceId == "c")
            //{
            //   ;
            //}
            //if (deviceId == "q")
            //{
            //   ;
            //}
            //if (deviceId == "w")
            //{
            //   ;
            //}
            //if (deviceId == "\\")
            //{
            //   ;
            //}

            else
            {
               if (result.field.Length == 1)
               {
                  english = english + String.Format("Device Id : {0}, ", DeviceIdInEnglish(result.field.Substring(0, 1)));
               }
               else
               {
                  english = english + String.Format("Device Id : {0}, Device Status : {1}", DeviceIdInEnglish(result.field.Substring(0, 1)), result.field.Substring(1));
               }
            }

            // Field e3 - Error Severity
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            if (result.field.Length > 0)
            {
               string g3 = string.Empty; 
               if (result.field == "0")
               {
                  g3 = "(no error)";
               }
               if (result.field == "1")
               {
                  g3 = "(routine)";
               }
               if (result.field == "2")
               {
                  g3 = "(warning)";
               }
               if (result.field == "3")
               {
                  g3 = "(suspend)";
               }
               if (result.field == "4")
               {
                  g3 = "(fatal)";
               }
               english = english + String.Format("Error Severity : {0}, {1}", result.field, g3);
            }

            // Field e4 - Diagnostic Status
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            if (result.field.Length > 0)
            {
               english = english + String.Format("Diagnostic Status : {0}, ", result.field);
            }

            // Field e5 - Supplies Status
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return ;

            if (result.field.Length > 0)
            {
               english = english + String.Format("Supplies Status : {0}, ", result.field);
            }

         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return ;
      }

      // see p 9-91 Table 9-43 Sensors Status - Advanced NDC Guide
      private string UnsolSensor_e2(string deviceStatus)
      {
         if (string.IsNullOrEmpty(deviceStatus))
         {
            return string.Empty; 
         }

         string description = string.Empty; 

         string e2_Byte1 = deviceStatus.Substring(0, 1); 
         deviceStatus = deviceStatus.Substring(1);

         if (e2_Byte1 == "1")
         {
            description += ", ‘TI’ sensor change";
         }
         if (e2_Byte1 == "2")
         {
            description += ", Mode change";

            // If byte 1 = ‘2’, mode change, the next byte gives details of the current state:
            string e2_Byte2 = deviceStatus.Substring(0, 1);

            if (e2_Byte2 == "0")
            {
               description += ", Supervisor mode exit";
            }
            if (e2_Byte2 == "1")
            {
               description += ", Supervisor mode entry";
            }
         }
         if (e2_Byte1 == "3")
         {
            description += ", Alarm state change";
         }
         if (e2_Byte1 == "5")
         {
            description += ", Full TI and full alarms change detected";
         }
         if (e2_Byte1 == "6")
         {
            description += ", Flexible TI and alarms change detected";
         }

         return description; 
      }
   }
}

