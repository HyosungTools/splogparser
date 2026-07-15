using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Host2Atm4 : Host2Atm
   {
      public Host2Atm4(ILogFileHandler parent, string logLine, APLogType apType = APLogType.NDC_HOST2ATM4) : base(parent, logLine, apType)
      {
         msgclass = "4";
         msgsubclass = "";
      }

      protected override void Initialize()
      {
         base.Initialize();

         english = string.Empty;

         try
         {
            // Transaction Reply Command - see Table 10-16, p10-47, Advance NDC Reference Manual.
            // Head layout (consumer transaction reply), separators shown:
            //   4 {FS}{FS}{FS} <nextstate> {FS} <MSN/timevariant> {FS} <serial+function+config> {FS}{FS}{SO} <screen text> ...
            // The amount is carried on the paired Transaction Request (Atm2Host11);
            // this reply is decoded for next state, function, and displayed screen name.

            // Field b/c - Message Class '4' (+ optional response flag)
            (bool success, string field, string subMessage) result = NDC.GetNextFieldBySeparator(ndcmsg);
            if (!result.success || !result.field.StartsWith("4"))
               return;

            english = "Transaction Reply to ATM, ";

            // Field d - LUNO (optional, usually empty)
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            if (result.field.Length > 0)
               english = english + String.Format("LUNO : {0}, ", result.field);

            // Field e - Message Sequence Number / Time Variant (optional, usually empty)
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // Field f - Next State ID
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            if (result.field.Length > 0)
               english = english + String.Format("Next State : {0}, ", result.field);

            // Field e (MSN / Time Variant, 3 or 8 chars) - housekeeping, skip
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success)
               return;

            // Next field carries the transaction serial number, the Function
            // Identifier, and function/config digits, e.g. "0778A105110".
            // The serial is leading digits; the Function ID is the first
            // non-digit (NDC function IDs are letters/symbols), so locate it
            // rather than assuming a fixed offset (serial length varies).
            result = NDC.GetNextFieldBySeparator(result.subMessage);
            if (!result.success && result.field.Length == 0)
               return;

            string fnField = result.field;
            english = english + FunctionInEnglish(fnField);

            // Screen name: the human-readable run in the {SO}-delimited display
            // data that follows. Surface it as triage context (e.g.
            // "WITHDRAWAL FROM CHECKING"), then stop before the receipt/config trailer.
            string screen = ExtractScreenName(result.subMessage);
            if (screen.Length > 0)
               english = english + String.Format("Screen : {0}, ", screen);
         }
         catch (Exception e)
         {
            this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("{0} Unexpected parse in message : {1}, {2}", myName, ndcmsg, e.Message));
         }

         return;
      }

      // Splits "0778A105110" into serial ("0778") + Function Identifier ('A')
      // and decodes the function per Table 10-16 (field l), p10-49/50.
      private string FunctionInEnglish(string fnField)
      {
         if (string.IsNullOrEmpty(fnField))
         {
            return string.Empty;
         }

         // find the first non-digit - that's the Function Identifier
         int fnPos = -1;
         for (int i = 0; i < fnField.Length; i++)
         {
            if (fnField[i] < '0' || fnField[i] > '9')
            {
               fnPos = i;
               break;
            }
         }

         string result = string.Empty;

         if (fnPos > 0)
         {
            result = result + String.Format("Serial : {0}, ", fnField.Substring(0, fnPos));
         }

         if (fnPos < 0)
         {
            // all digits, no function identifier present
            return result;
         }

         char fn = fnField[fnPos];
         string desc = string.Empty;

         switch (fn)
         {
            case '1':
            case '7': desc = "Deposit and Print"; break;
            case '2':
            case '8': desc = "Dispense and Print"; break;
            case '3':
            case '9': desc = "Display and Print"; break;
            case '4': desc = "Print Immediate"; break;
            case '5': desc = "Set Next State and Print"; break;
            case '6': desc = "Night Safe Deposit and Print"; break;
            case 'A': desc = "Eject Card, Dispense and Print (card before cash)"; break;
            case 'B':
            case 'C': desc = "Parallel Dispense/Print and Eject Card"; break;
            case 'E': desc = "Reserved (specific command reject)"; break;
            case 'F': desc = "Card before Parallel Dispense/Print"; break;
            case 'O': desc = "Reserved"; break;
            case 'P': desc = "Print Statement and Wait"; break;
            case 'Q': desc = "Print Statement and Set Next State"; break;
            case 'R':
            case 'S':
            case 'T': desc = "Reserved (specific command reject)"; break;
            case '*': desc = "Refund BNA deposited money and set next state"; break;
            case '-': desc = "Encash BNA deposited money, issue receipt if requested, and set next state"; break;
            case '\'': desc = "Encash BNA deposited money, and wait for another reply from Central"; break;
            case ':': desc = "Process CPM cheque"; break;
            default: desc = String.Format("Unknown function '{0}'", fn); break;
         }

         result = result + desc + ", ";
         return result;
      }

      // The display/screen data after the head is {SO}-delimited (Shift-Out,
      // 0x0E) print/screen formatting. Pull the first substantial run of
      // readable letters (the screen name, e.g. "WITHDRAWAL FROM CHECKING")
      // as triage context; ignore the rest of the receipt/config trailer.
      private string ExtractScreenName(string tail)
      {
         if (string.IsNullOrEmpty(tail))
         {
            return string.Empty;
         }

         // longest run of letters and spaces, at least a few chars long
         Match m = Regex.Match(tail, "[A-Za-z][A-Za-z ]{4,}[A-Za-z]");
         if (m.Success)
         {
            return m.Value.Trim();
         }

         return string.Empty;
      }
   }
}
