using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// A flat-format CIM (cash-in) log line that carries a value worth extracting:
   /// accepted note IDs and counts, cash-in status, per-denomination escrow detail,
   /// deposit limits and the store-cash completion flag.
   ///
   /// Payload shapes handled:
   ///    NoteID(39971)                                          -> Value = 39971
   ///    NoteCount(2)                                           -> Value = 2
   ///    CashInStatus[0].Value[100]                             -> Index = 0, Value = 100
   ///    CashInStatus[0].ID[39971]                              -> Index = 0, Value = 39971
   ///    CashInStatus[0].ItemCount[1]                           -> Index = 0, Value = 1
   ///    NumberOfCashInStatus[1]                                -> Value = 1
   ///    LastCashInStatus[OK]                                   -> Value = OK
   ///    CashIn_Status[2]                                       -> Value = 2
   ///    CashIn_Refused[0]                                      -> Value = 0
   ///    FireStoreCashComplete[1]                               -> Value = 1
   ///    Invoked {TotalItemsLimit[40], CurrencyID[NULL], ...}   -> Value = brace contents
   /// </summary>
   public class CIMCashInLine : SPFlatLine
   {
      /// <summary>Cash-in status slot index (CashInStatus[n].*), or -1 when not applicable.</summary>
      public int Index { get; private set; }

      /// <summary>The extracted value for this line's flatType; empty if extraction failed.</summary>
      public string Value { get; private set; }

      public CIMCashInLine(ILogFileHandler parent, string logLine, SPFlatType flatType) : base(parent, logLine, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         Index = -1;
         Value = string.Empty;

         switch (flatType)
         {
            case SPFlatType.CIM_SetCashInLimit:
            case SPFlatType.CIM_AcceptCash:
               Value = ExtractBraces();
               break;

            case SPFlatType.CIM_NoteID:
               Value = ExtractParens("NoteID");
               break;

            case SPFlatType.CIM_NoteCount:
               Value = ExtractParens("NoteCount");
               break;

            case SPFlatType.CIM_NumberOfCashInStatus:
               Value = ExtractBrackets("NumberOfCashInStatus");
               break;

            case SPFlatType.CIM_CashInStatusValue:
               Index = ExtractStatusIndex();
               Value = ExtractBrackets(".Value");
               break;

            case SPFlatType.CIM_CashInStatusID:
               Index = ExtractStatusIndex();
               Value = ExtractBrackets(".ID");
               break;

            case SPFlatType.CIM_CashInStatusItemCount:
               Index = ExtractStatusIndex();
               Value = ExtractBrackets(".ItemCount");
               break;

            case SPFlatType.CIM_CashInStatusCurrencyID:
               Index = ExtractStatusIndex();
               Value = ExtractBrackets(".CurrencyID");
               break;

            case SPFlatType.CIM_CashInStatusExponent:
               Index = ExtractStatusIndex();
               Value = ExtractBrackets(".Exponent");
               break;

            case SPFlatType.CIM_CashInStatus:
               Value = ExtractBrackets("CashIn_Status");
               break;

            case SPFlatType.CIM_CashInRefused:
               Value = ExtractBrackets("CashIn_Refused");
               break;

            case SPFlatType.CIM_LastCashInStatus:
               Value = ExtractBrackets("LastCashInStatus");
               break;

            case SPFlatType.CIM_StoreCashComplete:
               Value = ExtractBrackets("FireStoreCashComplete");
               break;

            default:
               break;
         }
      }

      /// <summary>Extract the value from a key[value] payload, e.g. LastCashInStatus[OK].</summary>
      private string ExtractBrackets(string key)
      {
         string result = string.Empty;

         Match mtch = Regex.Match(logLine, Regex.Escape(key) + @"\[([^\]]*)\]");
         if (mtch.Success)
         {
            result = mtch.Groups[1].Value.Trim();
         }

         return result;
      }

      /// <summary>Extract the value from a key(value) payload, e.g. NoteID(39971).</summary>
      private string ExtractParens(string key)
      {
         string result = string.Empty;

         Match mtch = Regex.Match(logLine, Regex.Escape(key) + @"\(([^)]*)\)");
         if (mtch.Success)
         {
            result = mtch.Groups[1].Value.Trim();
         }

         return result;
      }

      /// <summary>Extract the slot index from a CashInStatus[n].* payload.</summary>
      private int ExtractStatusIndex()
      {
         int index = -1;

         Match mtch = Regex.Match(logLine, @"CashInStatus\[(\d+)\]\.");
         if (mtch.Success)
         {
            int parsed;
            if (int.TryParse(mtch.Groups[1].Value, out parsed))
            {
               index = parsed;
            }
         }

         return index;
      }

      /// <summary>Extract the contents of the Invoked {...} brace group, e.g. deposit limits or timeouts.</summary>
      private string ExtractBraces()
      {
         string result = string.Empty;

         Match mtch = Regex.Match(logLine, @"\{([^}]*)\}");
         if (mtch.Success)
         {
            result = mtch.Groups[1].Value.Trim();
         }

         return result;
      }
   }
}
