using System;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Journal type enumeration for WinCE journal entries
   /// </summary>
   public enum WinCEJournalType
   {
      None,
      Operation,    // O - SF, OA, SA, SD, SG, etc.
      Transaction,  // T - TA, TE, TB, TC, etc.
      Account,      // A - A1-AF, BA-BV, etc.
      EMV,          // EMV data - OD records
      Unknown
   }

   /// <summary>
   /// Represents a parsed line from a WinCE electronic journal file (JNL*.dat).
   /// Text format: Length^KindCode^Sequence^Year^Month^Day^Hour^Min^Sec^Data...^X[0x1C]
   /// </summary>
   public class WinCEJournalLine : LogLine, ILogLine
   {
      // ILogLine implementations
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      /// <summary>
      /// Journal type enumeration value (Operation, Transaction, Account)
      /// </summary>
      public WinCEJournalType journalType { get; set; }

      /// <summary>
      /// Stack/record number (sequential)
      /// </summary>
      public string StackNum { get; set; }

      /// <summary>
      /// 1-2 character record type code (e.g., "SF", "OA", "TA")
      /// </summary>
      public string KindCode { get; set; }

      /// <summary>
      /// Full type string: MainType_KindCode (e.g., "O_SF", "T_TA")
      /// </summary>
      public string FullType { get; set; }

      /// <summary>
      /// Transaction type for transaction records (CW, BI, TR, etc.)
      /// </summary>
      public string TranType { get; set; }

      /// <summary>
      /// Transaction sequence number
      /// </summary>
      public string TranSequence { get; set; }

      /// <summary>
      /// Error code if present
      /// </summary>
      public string ErrorCode { get; set; }

      /// <summary>
      /// Requested amount in cents
      /// </summary>
      public string RequestedAmount { get; set; }

      /// <summary>
      /// Dispensed amount in cents
      /// </summary>
      public string DispensedAmount { get; set; }

      /// <summary>
      /// Surcharge amount in cents
      /// </summary>
      public string SurchargeAmount { get; set; }

      /// <summary>
      /// Transaction completion datetime
      /// </summary>
      public string TranDateTime { get; set; }

      /// <summary>
      /// Remaining data fields as single string
      /// </summary>
      public string Data { get; set; }

      // Transaction Detail properties
      public string TerminalID { get; set; }
      public string FromAccount { get; set; }
      public string ToAccount { get; set; }
      public string BankCode { get; set; }
      public string BankAccount { get; set; }
      public string HostDate { get; set; }
      public string HostTime { get; set; }
      public string AvailableBalance { get; set; }
      public string RetrievalNo { get; set; }
      public string AuthorizationNo { get; set; }
      public string SettleDate { get; set; }
      public string LedgerBalance { get; set; }
      public string ProcedureCount { get; set; }
      public string TransactionResult { get; set; }
      public string CardData { get; set; }
      public string NonCashValue { get; set; }
      public string NonCashType { get; set; }

      // EMV Detail properties
      public string AID { get; set; }
      public string AppName { get; set; }
      public string ARQC { get; set; }
      public string ARPC { get; set; }
      public string ServiceCode { get; set; }
      public string TerminalCapability { get; set; }
      public string POSEntryMode { get; set; }
      public string TVR { get; set; }
      public string IssuerActionCode { get; set; }
      public string EMVRawData { get; set; }

      /// <summary>
      /// Static counter for stack numbering
      /// </summary>
      private static int stackCounter = 0;

      /// <summary>
      /// Reset stack counter (call when starting new file)
      /// </summary>
      public static void ResetStackCounter()
      {
         stackCounter = 0;
      }

      /// <summary>
      /// Constructor
      /// </summary>
      public WinCEJournalLine(ILogFileHandler parent, string logLine, WinCEJournalType journalType) 
         : base(parent, logLine)
      {
         this.journalType = journalType;
         Initialize();
      }

      /// <summary>
      /// Initialize properties from parsed log line
      /// </summary>
      protected virtual void Initialize()
      {
         ParseFields();
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      /// <summary>
      /// Parse the caret-delimited fields from the log line
      /// Format: Length^KindCode^Sequence^Year^Month^Day^Hour^Min^Sec^Data...^X
      /// </summary>
      protected virtual void ParseFields()
      {
         // Initialize all fields
         StackNum = string.Empty;
         KindCode = string.Empty;
         FullType = string.Empty;
         TranType = string.Empty;
         TranSequence = string.Empty;
         ErrorCode = string.Empty;
         RequestedAmount = string.Empty;
         DispensedAmount = string.Empty;
         SurchargeAmount = string.Empty;
         TranDateTime = string.Empty;
         Data = string.Empty;

         // Transaction detail fields
         TerminalID = string.Empty;
         FromAccount = string.Empty;
         ToAccount = string.Empty;
         BankCode = string.Empty;
         BankAccount = string.Empty;
         HostDate = string.Empty;
         HostTime = string.Empty;
         AvailableBalance = string.Empty;
         RetrievalNo = string.Empty;
         AuthorizationNo = string.Empty;
         SettleDate = string.Empty;
         LedgerBalance = string.Empty;
         ProcedureCount = string.Empty;
         TransactionResult = string.Empty;
         CardData = string.Empty;
         NonCashValue = string.Empty;
         NonCashType = string.Empty;

         // EMV fields
         AID = string.Empty;
         AppName = string.Empty;
         ARQC = string.Empty;
         ARPC = string.Empty;
         ServiceCode = string.Empty;
         TerminalCapability = string.Empty;
         POSEntryMode = string.Empty;
         TVR = string.Empty;
         IssuerActionCode = string.Empty;
         EMVRawData = string.Empty;

         if (string.IsNullOrEmpty(logLine))
         {
            return;
         }

         try
         {
            string[] fields = logLine.Split('^');
            if (fields.Length < 9)
            {
               return;
            }

            // Increment and set stack number
            stackCounter++;
            StackNum = stackCounter.ToString("D4");

            // fields[0] = Length (not used)
            KindCode = fields[1].Trim();

            // Build FullType: MainType_KindCode
            char mainType = GetMainType(KindCode);
            FullType = $"{mainType}_{KindCode}";

            // Collect remaining data fields (index 9 onwards)
            if (fields.Length > 9)
            {
               // For transaction records, parse the transaction data
               if (journalType == WinCEJournalType.Transaction)
               {
                  ParseTransactionData(fields);
               }
               else if (journalType == WinCEJournalType.EMV)
               {
                  ParseEMVData(fields);
               }
               else
               {
                  // For non-transactions, just collect raw data
                  for (int i = 9; i < fields.Length; i++)
                  {
                     string field = fields[i].Trim();
                     // Skip the 'X' terminator
                     if (field == "X" || field.StartsWith("X\x1C"))
                     {
                        continue;
                     }
                     if (!string.IsNullOrEmpty(Data))
                     {
                        Data += "^";
                     }
                     Data += field;
                  }
               }
            }
         }
         catch
         {
            // Leave fields empty on parse error
         }
      }

      /// <summary>
      /// Parse transaction-specific data fields
      /// Raw record format (from JNL*.dat):
      /// Length^KindCode^Stack^Year^Month^Day^Hour^Min^Sec^TerminalID^TranNo^TranType^...
      /// 
      /// Field layout (absolute indices):
      /// 0: Length
      /// 1: KindCode (TA, TE, etc.)
      /// 2: Stack number
      /// 3-8: Year, Month, Day, Hour, Min, Sec
      /// 9: Terminal ID
      /// 10: Transaction Number
      /// 11: Transaction Type (CW, BI, TR, etc.)
      /// 12: From Account
      /// 13: To Account
      /// 14: Bank Code
      /// 15: Bank Account
      /// 16: Host Date (MMDDYYYY)
      /// 17: Host Time (HHMMSS)
      /// 18: Available Balance
      /// 19: Retrieval No
      /// 20: Authorization No (part 1)
      /// 21: Authorization No (part 2)
      /// 22: Host Settle Date
      /// 23: Surcharge Amount (cents)
      /// 24: Requested Amount (cents)
      /// 25: Dispensed Amount (cents)
      /// 26: Ledger Balance
      /// 27: Procedure Count
      /// 28: Transaction Result (TRUE/FALSE)
      /// 29: Error Code
      /// 30: Card Data
      /// 31: Non Cash Value
      /// 32: Non Cash Type
      /// </summary>
      private void ParseTransactionData(string[] fields)
      {
         // Terminal ID (field 9)
         if (fields.Length > 9)
         {
            TerminalID = fields[9].Trim();
         }

         // Transaction sequence number (field 10)
         if (fields.Length > 10)
         {
            TranSequence = fields[10].Trim();
         }

         // Transaction Type (field 11)
         if (fields.Length > 11)
         {
            TranType = fields[11].Trim();
         }

         // From Account (field 12)
         if (fields.Length > 12)
         {
            FromAccount = MapAccountCode(fields[12].Trim());
         }

         // To Account (field 13)
         if (fields.Length > 13)
         {
            ToAccount = MapAccountCode(fields[13].Trim());
         }

         // Bank Code (field 14)
         if (fields.Length > 14)
         {
            BankCode = fields[14].Trim();
         }

         // Bank Account (field 15)
         if (fields.Length > 15)
         {
            BankAccount = fields[15].Trim();
         }

         // Host Date (field 16)
         if (fields.Length > 16)
         {
            HostDate = FormatDate(fields[16].Trim());
         }

         // Host Time (field 17)
         if (fields.Length > 17)
         {
            HostTime = FormatTime(fields[17].Trim());
         }

         // Transaction DateTime
         if (fields.Length > 17)
         {
            TranDateTime = FormatDateTime(fields[16].Trim(), fields[17].Trim());
         }

         // Available Balance (field 18)
         if (fields.Length > 18)
         {
            AvailableBalance = FormatAmount(fields[18].Trim());
         }

         // Retrieval No (field 19)
         if (fields.Length > 19)
         {
            RetrievalNo = fields[19].Trim();
         }

         // Authorization No (fields 20 + 21)
         if (fields.Length > 21)
         {
            string auth1 = fields[20].Trim();
            string auth2 = fields[21].Trim();
            AuthorizationNo = $"{auth1} {auth2}".Trim();
         }
         else if (fields.Length > 20)
         {
            AuthorizationNo = fields[20].Trim();
         }

         // Settle Date (field 22)
         if (fields.Length > 22)
         {
            SettleDate = FormatDate(fields[22].Trim());
         }

         // Surcharge Amount (field 23)
         if (fields.Length > 23)
         {
            SurchargeAmount = FormatAmount(fields[23].Trim());
         }

         // Requested Amount (field 24)
         if (fields.Length > 24)
         {
            RequestedAmount = FormatAmount(fields[24].Trim());
         }

         // Dispensed Amount (field 25)
         if (fields.Length > 25)
         {
            DispensedAmount = FormatAmount(fields[25].Trim());
         }

         // Ledger Balance (field 26)
         if (fields.Length > 26)
         {
            LedgerBalance = FormatAmount(fields[26].Trim());
         }

         // Procedure Count (field 27)
         if (fields.Length > 27)
         {
            ProcedureCount = fields[27].Trim();
         }

         // Transaction Result (field 28)
         if (fields.Length > 28)
         {
            TransactionResult = fields[28].Trim();
         }

         // Error Code (field 29)
         if (fields.Length > 29)
         {
            ErrorCode = fields[29].Trim();
            // Clean up error code - remove if all zeros
            if (ErrorCode == "0000000" || ErrorCode == "0")
            {
               ErrorCode = string.Empty;
            }
         }

         // Card Data (field 30)
         if (fields.Length > 30)
         {
            CardData = fields[30].Trim();
         }

         // Non Cash Value (field 31)
         if (fields.Length > 31)
         {
            NonCashValue = FormatAmount(fields[31].Trim());
         }

         // Non Cash Type (field 32)
         if (fields.Length > 32)
         {
            NonCashType = fields[32].Trim();
         }

         // Collect remaining data for reference
         for (int i = 9; i < fields.Length; i++)
         {
            string field = fields[i].Trim();
            if (field == "X" || field.StartsWith("X\x1C"))
            {
               continue;
            }
            if (!string.IsNullOrEmpty(Data))
            {
               Data += "^";
            }
            Data += field;
         }
      }

      /// <summary>
      /// Parse EMV data from OD record
      /// Format: Length^OD^Stack^Year^Month^Day^Hour^Min^Sec^TerminalID^TranNo^AuthNo^TLVData^X
      /// </summary>
      private void ParseEMVData(string[] fields)
      {
         // Terminal ID (field 9)
         if (fields.Length > 9)
         {
            TerminalID = fields[9].Trim();
         }

         // Transaction Number (field 10)
         if (fields.Length > 10)
         {
            TranSequence = fields[10].Trim();
         }

         // Authorization No (field 11)
         if (fields.Length > 11)
         {
            AuthorizationNo = fields[11].Trim();
         }

         // EMV TLV Data (field 12)
         if (fields.Length > 12)
         {
            string tlvData = fields[12].Trim();
            EMVRawData = tlvData;
            ParseEMVTLV(tlvData);
         }
      }

      /// <summary>
      /// Parse EMV TLV (Tag-Length-Value) data
      /// Format from EJViewer source (JournalData.cpp GetEmvDataJournal):
      /// - Tag: 4 characters (e.g., "4F00", "9F26", "9100")
      /// - Length: 2 decimal digits
      /// - Value: length characters
      /// 
      /// Tag mappings:
      /// 4F00 = AID
      /// 5000 = Name (Application Name)
      /// 9F26 = ARQC
      /// 9100 = ARPC
      /// 5F30 = Service Code
      /// 9F33 = Terminal Capability
      /// 9F39 = POS Entry Mode
      /// 9500 = TVR
      /// 9F0E = Issuer Action Code
      /// </summary>
      private void ParseEMVTLV(string tlvData)
      {
         if (string.IsNullOrEmpty(tlvData))
            return;

         try
         {
            int position = 0;
            int emvLength = tlvData.Length;

            // Loop through each TLV (tag/length/value) in the EMV data string
            while (position + 6 < emvLength)
            {
               // Tag is always 4 characters
               string tag = tlvData.Substring(position, 4).ToUpper();
               position += 4;

               // Length is 2 decimal digits
               string strLength = tlvData.Substring(position, 2);
               position += 2;

               if (!int.TryParse(strLength, out int length))
                  break;

               if (position + length > emvLength)
                  break;

               // Value is 'length' characters
               string value = tlvData.Substring(position, length);
               position += length;

               // Map tag to property (matching EJViewer GetEmvTagName)
               switch (tag)
               {
                  case "4F00":
                     AID = value;
                     break;
                  case "5000":
                     AppName = value;
                     break;
                  case "9F26":
                     ARQC = value;
                     break;
                  case "9100":
                     ARPC = value;
                     break;
                  case "5F30":
                     ServiceCode = value;
                     break;
                  case "9F33":
                     TerminalCapability = value;
                     break;
                  case "9F39":
                     POSEntryMode = value;
                     break;
                  case "9500":
                     TVR = value;
                     break;
                  case "9F0E":
                     IssuerActionCode = value;
                     break;
               }
            }
         }
         catch
         {
            // Leave fields as-is on parse error
         }
      }

      /// <summary>
      /// Convert hex string to ASCII
      /// </summary>
      private string HexToAscii(string hex)
      {
         if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
            return hex;

         try
         {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < hex.Length; i += 2)
            {
               string hexByte = hex.Substring(i, 2);
               if (int.TryParse(hexByte, System.Globalization.NumberStyles.HexNumber, null, out int b))
               {
                  if (b >= 32 && b <= 126)
                     sb.Append((char)b);
               }
            }
            return sb.ToString();
         }
         catch
         {
            return hex;
         }
      }

      /// <summary>
      /// Map account code to description
      /// </summary>
      private string MapAccountCode(string code)
      {
         if (string.IsNullOrEmpty(code) || code == "--")
            return string.Empty;

         switch (code.ToUpper())
         {
            case "CA": return "Checking";
            case "SA": return "Savings";
            case "CR": return "Credit Card";
            default: return code;
         }
      }

      /// <summary>
      /// Format date from MMDDYYYY to MM/DD/YYYY
      /// </summary>
      private string FormatDate(string date)
      {
         if (string.IsNullOrEmpty(date) || date.Length < 8)
            return date;

         try
         {
            string month = date.Substring(0, 2);
            string day = date.Substring(2, 2);
            string year = date.Substring(4, 4);
            return $"{month}/{day}/{year}";
         }
         catch
         {
            return date;
         }
      }

      /// <summary>
      /// Format time from HHMMSS to HH:MM:SS
      /// </summary>
      private string FormatTime(string time)
      {
         if (string.IsNullOrEmpty(time) || time.Length < 6)
            return time;

         try
         {
            string hour = time.Substring(0, 2);
            string minute = time.Substring(2, 2);
            string second = time.Substring(4, 2);
            return $"{hour}:{minute}:{second}";
         }
         catch
         {
            return time;
         }
      }

      /// <summary>
      /// Format amount from cents to dollars (e.g., "2000" -> "$20.00")
      /// </summary>
      private string FormatAmount(string amountCents)
      {
         if (string.IsNullOrEmpty(amountCents))
            return string.Empty;

         // Remove any spaces
         amountCents = amountCents.Replace(" ", "");

         if (!int.TryParse(amountCents, out int cents))
            return string.Empty;

         if (cents == 0)
            return "$0.00";

         decimal dollars = cents / 100m;
         return $"${dollars:N2}";
      }

      /// <summary>
      /// Format date and time from MMDDYYYY and HHMMSS
      /// </summary>
      private string FormatDateTime(string date, string time)
      {
         if (string.IsNullOrEmpty(date) || date.Length < 8)
            return string.Empty;

         try
         {
            // Date is MMDDYYYY
            string month = date.Substring(0, 2);
            string day = date.Substring(2, 2);
            string year = date.Substring(4, 4);

            string hour = "00";
            string minute = "00";
            string second = "00";

            if (!string.IsNullOrEmpty(time) && time.Length >= 6)
            {
               hour = time.Substring(0, 2);
               minute = time.Substring(2, 2);
               second = time.Substring(4, 2);
            }

            return $"{month}/{day}/{year} {hour}:{minute}:{second}";
         }
         catch
         {
            return string.Empty;
         }
      }

      /// <summary>
      /// Get main type character from KindCode
      /// O = Operation, T = Transaction, A = Account
      /// </summary>
      private char GetMainType(string kindCode)
      {
         if (string.IsNullOrEmpty(kindCode))
            return 'O';

         // Transaction codes
         string[] transactionCodes = {
            "B", "C", "D", "E", "K0", "K1", "K2", "K3", "K4", "K5",
            "K6", "K7", "K8", "K9", "KA", "KB", "KC", "O", "P", "R",
            "RO", "RP", "TA", "TB", "TC", "TD", "TE", "TF", "TX", "JC",
            "GP", "LT", "LX"
         };

         // Account codes
         string[] accountCodes = {
            "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA",
            "AB", "AC", "AD", "AE", "AF", "BA", "BB", "BC", "BD", "BE",
            "BF", "BG", "BH", "BI", "BK", "BL", "BM", "BN", "BO", "E1",
            "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB",
            "EC", "ED", "EE", "EF", "FA", "FB", "FC", "I", "L", "M",
            "N", "W", "BR", "BS", "BT", "BU", "BV", "BW", "BX",
            "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8"
         };

         foreach (string code in transactionCodes)
         {
            if (kindCode.Equals(code, StringComparison.OrdinalIgnoreCase))
               return 'T';
         }

         foreach (string code in accountCodes)
         {
            if (kindCode.Equals(code, StringComparison.OrdinalIgnoreCase))
               return 'A';
         }

         // Default to Operation
         return 'O';
      }

      /// <summary>
      /// Extract timestamp from caret-delimited date/time fields
      /// Format: Year^Month^Day^Hour^Min^Sec (fields 3-8)
      /// </summary>
      protected override string tsTimestamp()
      {
         string timestamp = LogLine.DefaultTimestamp;

         if (string.IsNullOrEmpty(logLine))
         {
            return timestamp;
         }

         try
         {
            string[] fields = logLine.Split('^');
            if (fields.Length < 9)
            {
               return timestamp;
            }

            string year = fields[3].Trim();
            string month = fields[4].Trim().PadLeft(2, '0');
            string day = fields[5].Trim().PadLeft(2, '0');
            string hour = fields[6].Trim().PadLeft(2, '0');
            string minute = fields[7].Trim().PadLeft(2, '0');
            string second = fields[8].Trim().PadLeft(2, '0');

            // Validate numeric fields
            if (int.TryParse(year, out int y) && y >= 1990 && y <= 2100 &&
                int.TryParse(month, out int m) && m >= 1 && m <= 12 &&
                int.TryParse(day, out int d) && d >= 1 && d <= 31 &&
                int.TryParse(hour, out int h) && h >= 0 && h <= 23 &&
                int.TryParse(minute, out int min) && min >= 0 && min <= 59 &&
                int.TryParse(second, out int s) && s >= 0 && s <= 59)
            {
               timestamp = $"{year}-{month}-{day} {hour}:{minute}:{second}.000";
            }
         }
         catch
         {
            // Return default timestamp on any error
         }

         return timestamp;
      }

      /// <summary>
      /// No HResult for journal entries
      /// </summary>
      protected override string hResult()
      {
         return string.Empty;
      }

      /// <summary>
      /// Determine journal type from KindCode
      /// </summary>
      private static WinCEJournalType GetJournalType(string kindCode)
      {
         if (string.IsNullOrEmpty(kindCode))
            return WinCEJournalType.Unknown;

         // EMV code
         if (kindCode.Equals("OD", StringComparison.OrdinalIgnoreCase))
            return WinCEJournalType.EMV;

         // Transaction codes
         string[] transactionCodes = {
            "B", "C", "D", "E", "K0", "K1", "K2", "K3", "K4", "K5",
            "K6", "K7", "K8", "K9", "KA", "KB", "KC", "O", "P", "R",
            "RO", "RP", "TA", "TB", "TC", "TD", "TE", "TF", "TX", "JC",
            "GP", "LT", "LX"
         };

         // Account codes
         string[] accountCodes = {
            "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA",
            "AB", "AC", "AD", "AE", "AF", "BA", "BB", "BC", "BD", "BE",
            "BF", "BG", "BH", "BI", "BK", "BL", "BM", "BN", "BO", "E1",
            "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB",
            "EC", "ED", "EE", "EF", "FA", "FB", "FC", "I", "L", "M",
            "N", "W", "BR", "BS", "BT", "BU", "BV", "BW", "BX",
            "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8"
         };

         foreach (string code in transactionCodes)
         {
            if (kindCode.Equals(code, StringComparison.OrdinalIgnoreCase))
               return WinCEJournalType.Transaction;
         }

         foreach (string code in accountCodes)
         {
            if (kindCode.Equals(code, StringComparison.OrdinalIgnoreCase))
               return WinCEJournalType.Account;
         }

         return WinCEJournalType.Operation;
      }

      /// <summary>
      /// Factory method to create WinCEJournalLine from raw text.
      /// </summary>
      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (string.IsNullOrEmpty(logLine) || logLine.Length < 20)
         {
            return null;
         }

         try
         {
            string[] fields = logLine.Split('^');

            // Minimum fields: Length, KindCode, Sequence, Year, Month, Day, Hour, Min, Sec
            if (fields.Length < 9)
            {
               return null;
            }

            // Validate KindCode exists
            string kindCode = fields[1].Trim();
            if (string.IsNullOrEmpty(kindCode))
            {
               return null;
            }

            // Validate date/time fields are numeric
            string year = fields[3].Trim();
            string month = fields[4].Trim();
            string day = fields[5].Trim();
            string hour = fields[6].Trim();
            string minute = fields[7].Trim();
            string second = fields[8].Trim();

            if (!int.TryParse(year, out int y) || y < 1990 || y > 2100 ||
                !int.TryParse(month, out int m) || m < 1 || m > 12 ||
                !int.TryParse(day, out int d) || d < 1 || d > 31 ||
                !int.TryParse(hour, out int h) || h < 0 || h > 23 ||
                !int.TryParse(minute, out int min) || min < 0 || min > 59 ||
                !int.TryParse(second, out int s) || s < 0 || s > 59)
            {
               return null;
            }

            WinCEJournalType journalType = GetJournalType(kindCode);
            return new WinCEJournalLine(logFileHandler, logLine, journalType);
         }
         catch
         {
            return null;
         }
      }
   }
}
