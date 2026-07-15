using System;
using Contract;

namespace LogLineHandler
{
   public class Atm2Host : NDC
   {
      public string myName = "Atm2Host";

      public Atm2Host(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
      }

      public static new ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         (bool success, string subLine) result = NDC.IsolateNdcMessage(logLine);
         if (!result.success)
         {
            return null;
         }
         try
         {
            (bool success, string field, string subMessage) result2 = NDC.GetNextFieldBySeparator(result.subLine);

            if (result2.success && result2.field.StartsWith("11"))
               return new Atm2Host11(logFileHandler, logLine, APLogType.NDC_ATM2HOST11);

            if (result2.success && result2.field.StartsWith("12"))
               return new Atm2Host12(logFileHandler, logLine, APLogType.NDC_ATM2HOST12);

            if (result2.success && result2.field.StartsWith("22"))
               return new Atm2Host22(logFileHandler, logLine, APLogType.NDC_ATM2HOST22);

            if (result2.success && result2.field.StartsWith("23"))
               return new Atm2Host22(logFileHandler, logLine, APLogType.NDC_ATM2HOST23);

            if (result2.success && result2.field.StartsWith("51"))
               return new Atm2Host51(logFileHandler, logLine, APLogType.NDC_ATM2HOST51);

            if (result2.success && result2.field.StartsWith("61"))
               return new Atm2Host61(logFileHandler, logLine, APLogType.NDC_ATM2HOST61);

         }
         catch (Exception e)
         {
            logFileHandler.ctx.ConsoleWriteLogLine(String.Format("Atm2Host ILogLine.Factory - NDC parsing error - {0}", e.Message));
         }

         return new APLine(logFileHandler, logLine, APLogType.None);

      }

      // ------------------------------------------------------------------
      // Device status decoders, shared by solicited (22) and unsolicited
      // (12) status messages - the spec defines these tables once per
      // device as "Solicited/Unsolicited".
      // see Advance NDC Reference Manual, Chapter 9, Device Status Information
      // ------------------------------------------------------------------

      // see p 9-60 - standard error severity coding, one character per unit
      protected static string SeverityInEnglish(string code)
      {
         if (code == "0")
         {
            return "no error";
         }
         if (code == "1")
         {
            return "routine";
         }
         if (code == "2")
         {
            return "warning";
         }
         if (code == "3")
         {
            return "suspend";
         }
         if (code == "4")
         {
            return "fatal";
         }
         return code;
      }

      // see p 9-77 Table 9-35 Cash Handler Status - Advance NDC Reference Manual
      // Character 1 (T-code) is the transaction/device status.
      // Characters 2-9 (T-data) are notes-dispensed counts, two digits per
      // cassette type (types 1-4; types 1-7 if Enhanced Config option 76 = 001,
      // with optional additional spray-dispenser count pairs following).
      protected static string CashHandler_e2(string deviceStatus)
      {
         if (string.IsNullOrEmpty(deviceStatus))
         {
            return string.Empty;
         }

         string description = string.Empty;

         string e2_Byte1 = deviceStatus.Substring(0, 1);
         deviceStatus = deviceStatus.Substring(1);

         if (e2_Byte1 == "0")
         {
            description += "0 (Successful operation, but an exception condition occurred - see later fields)";
         }
         else if (e2_Byte1 == "1")
         {
            description += "1 (Short dispense; for a spray dispenser can also mean an extra note dispensed)";
         }
         else if (e2_Byte1 == "2")
         {
            description += "2 (No notes dispensed)";
         }
         else if (e2_Byte1 == "3")
         {
            description += "3 (Notes dispensed unknown - cardholder may have had access to presented notes; counts show requested values)";
         }
         else if (e2_Byte1 == "4")
         {
            description += "4 (No notes dispensed or card not ejected - stack failed, notes purged before card eject)";
         }
         else if (e2_Byte1 == "5")
         {
            description += "5 (Notes retracted after Present time-out, number of notes unknown)";
         }
         else
         {
            description += e2_Byte1;
         }

         // Notes dispensed in the last operation, two digits per cassette type
         if (deviceStatus.Length >= 2)
         {
            description += ", Last dispense";
            int noteType = 1;
            while (deviceStatus.Length >= 2)
            {
               string count = deviceStatus.Substring(0, 2);
               deviceStatus = deviceStatus.Substring(2);
               description += String.Format(", type{0}={1}", noteType, count);
               noteType = noteType + 1;
            }
         }

         return description;
      }

      // see p 9-79 Table 9-35 - Cash Handler Error Severity (5 or 8 characters)
      // character 0 = whole device, characters 1-4 (or 1-7) = cassette types
      protected static string CashHandler_e3(string severity)
      {
         if (string.IsNullOrEmpty(severity))
         {
            return string.Empty;
         }

         string description = severity + " (device=" + SeverityInEnglish(severity.Substring(0, 1));

         for (int i = 1; i < severity.Length; i++)
         {
            description += String.Format(", type{0}={1}", i, SeverityInEnglish(severity.Substring(i, 1)));
         }

         description += ")";
         return description;
      }

      // see p 9-80 Table 9-35 - Cash Handler Supplies Status (5 characters)
      // character 0 = reject bin, characters 1-4 (or 1-7) = cassette types
      protected static string CashHandler_e5(string supplies)
      {
         if (string.IsNullOrEmpty(supplies))
         {
            return string.Empty;
         }

         string rejectBin = supplies.Substring(0, 1);
         string rejectText = rejectBin;
         if (rejectBin == "0")
         {
            rejectText = "no new state";
         }
         else if (rejectBin == "1")
         {
            rejectText = "no overfill";
         }
         else if (rejectBin == "4")
         {
            rejectText = "overfill";
         }

         string description = supplies + " (reject bin=" + rejectText;

         for (int i = 1; i < supplies.Length; i++)
         {
            string code = supplies.Substring(i, 1);
            string codeText = code;
            if (code == "0")
            {
               codeText = "no new state";
            }
            else if (code == "1")
            {
               codeText = "sufficient";
            }
            else if (code == "2")
            {
               codeText = "low";
            }
            else if (code == "3")
            {
               codeText = "out of notes";
            }
            description += String.Format(", type{0}={1}", i, codeText);
         }

         description += ")";
         return description;
      }

      // see p 9-75 Table 9-34 Card Reader/Writer Status - Advance NDC Reference Manual
      protected static string CardReader_e2(string deviceStatus)
      {
         if (string.IsNullOrEmpty(deviceStatus))
         {
            return string.Empty;
         }

         string e2_Byte1 = deviceStatus.Substring(0, 1);

         if (e2_Byte1 == "0")
         {
            return "0 (No transaction exception - see later fields for severity/diagnostic/supplies changes)";
         }
         if (e2_Byte1 == "1")
         {
            return "1 (Cardholder did not take card in time - card captured or jammed)";
         }
         if (e2_Byte1 == "2")
         {
            return "2 (Mechanism failed to eject card - card captured or jammed)";
         }
         if (e2_Byte1 == "3")
         {
            return "3 (Mechanism failed to update requested tracks on card)";
         }
         if (e2_Byte1 == "4")
         {
            return "4 (Invalid track data received from Central)";
         }
         if (e2_Byte1 == "7")
         {
            return "7 (Error in track data)";
         }

         return deviceStatus;
      }

      // see p 9-76 Table 9-34 - Card Reader Supplies Status (capture bin)
      protected static string CardReader_e5(string supplies)
      {
         if (supplies == "0")
         {
            return "0 (no new state)";
         }
         if (supplies == "1")
         {
            return "1 (no overfill condition - capture bin)";
         }
         if (supplies == "4")
         {
            return "4 (overfill condition - capture bin)";
         }
         return supplies;
      }

      // see p 9-81 Table 9-36 Depository Status - Advance NDC Reference Manual
      protected static string Depository_e2(string deviceStatus)
      {
         if (string.IsNullOrEmpty(deviceStatus))
         {
            return string.Empty;
         }

         string e2_Byte1 = deviceStatus.Substring(0, 1);

         if (e2_Byte1 == "0")
         {
            return "0 (Successful operation, but an exception condition occurred - see following field)";
         }
         if (e2_Byte1 == "1")
         {
            return "1 (Time-out on cardholder deposit)";
         }
         if (e2_Byte1 == "2")
         {
            return "2 (Failure to enable mechanism for a deposit)";
         }
         if (e2_Byte1 == "3")
         {
            return "3 (Envelope/document jam or deposit failed - cardholder HAS access, or access in doubt)";
         }
         if (e2_Byte1 == "4")
         {
            return "4 (Envelope/document jam or deposit failed - cardholder does NOT have access)";
         }

         return deviceStatus;
      }

      // see p 9-81 Table 9-36 - Depository Supplies Status (deposit bin);
      // note: this field is not sent when a deposit time-out occurs
      protected static string Depository_e5(string supplies)
      {
         if (supplies == "0")
         {
            return "0 (no envelope deposited)";
         }
         if (supplies == "1")
         {
            return "1 (no overfill condition)";
         }
         if (supplies == "4")
         {
            return "4 (overfill detected)";
         }
         return supplies;
      }

      // Per-device dispatch for the transaction/device status field (e2/g2)
      protected static string DeviceStatusInEnglish(string deviceId, string deviceStatus)
      {
         if (deviceId == "E")
         {
            return CashHandler_e2(deviceStatus);
         }
         if (deviceId == "D")
         {
            return CardReader_e2(deviceStatus);
         }
         if (deviceId == "F")
         {
            return Depository_e2(deviceStatus);
         }
         return deviceStatus;
      }

      // Per-device dispatch for the error severity field (e3/g3);
      // each character is standard-coded, labels vary by device
      protected static string ErrorSeverityInEnglish(string deviceId, string severity)
      {
         if (string.IsNullOrEmpty(severity))
         {
            return string.Empty;
         }

         if (deviceId == "E")
         {
            return CashHandler_e3(severity);
         }

         // all other devices: decode each character with the standard coding
         string codes = string.Empty;
         for (int i = 0; i < severity.Length; i++)
         {
            if (i > 0)
            {
               codes += ", ";
            }
            codes += SeverityInEnglish(severity.Substring(i, 1));
         }
         return String.Format("{0} ({1})", severity, codes);
      }

      // Per-device dispatch for the supplies status field (e5/g5)
      protected static string SuppliesStatusInEnglish(string deviceId, string supplies)
      {
         if (deviceId == "E")
         {
            return CashHandler_e5(supplies);
         }
         if (deviceId == "D")
         {
            return CardReader_e5(supplies);
         }
         if (deviceId == "F")
         {
            return Depository_e5(supplies);
         }
         return supplies;
      }
   }
}
