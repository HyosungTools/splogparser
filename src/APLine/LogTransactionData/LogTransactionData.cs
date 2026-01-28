using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using Contract;

namespace LogLineHandler
{
   public class LogTransactionData : APLine
   {
      // FlowPoint (normalized: CMCFlex, SymXchange, JackHenry, etc. - WebService cores only)
      public string FlowPoint { get; set; }

      // FP - from JSON "fp" field (stripped of PLACEHOLDER- and Common-)
      public string FP { get; set; }

      // Session
      public string OnUs { get; set; }
      public string CoreAvailable { get; set; }
      public string NDCAvailable { get; set; }
      public string Language { get; set; }
      public string CustomerId { get; set; }

      // Transaction
      public string TransactionType { get; set; }
      public string Message { get; set; }
      public string AccountNumber { get; set; }
      public string AccountType { get; set; }
      public string Amount { get; set; }
      public string TotalAmount { get; set; }
      public string TotalCashAmount { get; set; }
      public string TotalCheckAmount { get; set; }
      public string BillsInserted { get; set; }

      // Flags
      public string IsContactless { get; set; }

      // Card
      public string Track2 { get; set; }

      // Raw payload (everything after LogTransactionData])
      public string Payload { get; set; }

      // Regex to extract FlowPoint name
      // Matches: [CMCFlexWebServiceRequestFlowPoint.LogTransactionData] or [StandardFlowPoint.LogTransactionData]
      private static Regex flowPointRegex = new Regex(@"\[(\w+FlowPoint)\.LogTransactionData\]");

      public LogTransactionData(ILogFileHandler logFileHandler, string logLine, APLogType apType = APLogType.APLOG_LOGTRANSACTIONDATA)
          : base(logFileHandler, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Parse FlowPoint
         FlowPoint = ParseFlowPoint();

         // Parse raw Payload
         Payload = ParsePayload();

         // Detect format and parse accordingly
         if (Payload.TrimStart().StartsWith("[FLOWPOINT]"))
         {
            ParseJsonPayload();
         }
         else
         {
            ParsePipeDelimitedPayload();
         }
      }

      private string ParseFlowPoint()
      {
         Match m = flowPointRegex.Match(logLine);
         if (m.Success)
         {
            string raw = m.Groups[1].Value;

            // Only keep FlowPoint if it's a WebService core
            if (!raw.Contains("WebServiceRequest"))
            {
               return string.Empty;
            }

            // Normalize: strip suffix to get the core name
            string normalized = raw.Replace("WebServiceRequestFlowPoint", "");

            return normalized;
         }
         return string.Empty;
      }

      private string ParsePayload()
      {
         // Find the position after LogTransactionData]
         int idx = logLine.IndexOf("FlowPoint.LogTransactionData]");
         if (idx < 0) return string.Empty;

         string payload = logLine.Substring(idx + "FlowPoint.LogTransactionData]".Length).Trim();

         // Skip [TID:##] if present
         if (payload.StartsWith("[TID:"))
         {
            int tidEnd = payload.IndexOf(']');
            if (tidEnd > 0)
            {
               payload = payload.Substring(tidEnd + 1).Trim();
            }
         }

         return payload;
      }

      /// <summary>
      /// Format OnUs value: true -> "on-us", false -> "foreign"
      /// </summary>
      private string FormatOnUs(bool value)
      {
         return value ? "on-us" : "foreign";
      }

      /// <summary>
      /// Format OnUs value from string: True -> "on-us", False -> "foreign"
      /// </summary>
      private string FormatOnUs(string value)
      {
         if (string.IsNullOrEmpty(value)) return null;
         return value.Equals("True", StringComparison.OrdinalIgnoreCase) ? "on-us" : "foreign";
      }

      /// <summary>
      /// Format availability value: lowercase (true/false)
      /// </summary>
      private string FormatAvailability(bool value)
      {
         return value ? "true" : "false";
      }

      /// <summary>
      /// Format availability value from string: lowercase
      /// </summary>
      private string FormatAvailability(string value)
      {
         if (string.IsNullOrEmpty(value)) return null;
         return value.ToLower();
      }

      /// <summary>
      /// Format amount: divide by 100 and format as currency (12000 -> "120.00")
      /// </summary>
      private string FormatAmount(string value)
      {
         if (string.IsNullOrEmpty(value)) return null;

         if (long.TryParse(value, out long cents))
         {
            decimal dollars = cents / 100m;
            return dollars.ToString("0.00");
         }

         return value; // Return original if not a valid number
      }

      private void ParseJsonPayload()
      {
         try
         {
            // Remove [FLOWPOINT] prefix
            string jsonStr = Payload;
            if (jsonStr.StartsWith("[FLOWPOINT]"))
            {
               jsonStr = jsonStr.Substring("[FLOWPOINT]".Length).Trim();
            }

            // Parse JSON
            using (JsonDocument doc = JsonDocument.Parse(jsonStr))
            {
               JsonElement root = doc.RootElement;

               // FP field - strip PLACEHOLDER- and Common-
               if (root.TryGetProperty("fp", out JsonElement fpElement))
               {
                  string fpRaw = fpElement.GetString() ?? string.Empty;
                  FP = fpRaw
                      .Replace("PLACEHOLDER-", "")
                      .Replace("Common-", "");
               }

               // CurrentSession
               if (root.TryGetProperty("CurrentSession", out JsonElement sessionElement))
               {
                  if (sessionElement.TryGetProperty("OnUs", out JsonElement onUsElement))
                  {
                     if (onUsElement.ValueKind == JsonValueKind.True || onUsElement.ValueKind == JsonValueKind.False)
                        OnUs = FormatOnUs(onUsElement.GetBoolean());
                     else
                        OnUs = FormatOnUs(onUsElement.ToString());
                  }

                  if (sessionElement.TryGetProperty("CoreAvailable", out JsonElement coreElement))
                  {
                     if (coreElement.ValueKind == JsonValueKind.True || coreElement.ValueKind == JsonValueKind.False)
                        CoreAvailable = FormatAvailability(coreElement.GetBoolean());
                     else
                        CoreAvailable = FormatAvailability(coreElement.ToString());
                  }

                  if (sessionElement.TryGetProperty("NDCAvailable", out JsonElement ndcElement))
                  {
                     if (ndcElement.ValueKind == JsonValueKind.True || ndcElement.ValueKind == JsonValueKind.False)
                        NDCAvailable = FormatAvailability(ndcElement.GetBoolean());
                     else
                        NDCAvailable = FormatAvailability(ndcElement.ToString());
                  }

                  if (sessionElement.TryGetProperty("Language", out JsonElement langElement))
                     Language = langElement.GetString();

                  if (sessionElement.TryGetProperty("IdentificationNumber", out JsonElement idElement))
                     CustomerId = idElement.GetString();
               }

               // CurrentTransaction
               if (root.TryGetProperty("CurrentTransaction", out JsonElement transElement))
               {
                  if (transElement.TryGetProperty("Type", out JsonElement typeElement))
                     TransactionType = typeElement.GetString();

                  if (transElement.TryGetProperty("Message", out JsonElement msgElement))
                     Message = msgElement.GetString();

                  if (transElement.TryGetProperty("AccountNumber", out JsonElement acctNumElement))
                     AccountNumber = acctNumElement.GetString();

                  if (transElement.TryGetProperty("AccountType", out JsonElement acctTypeElement))
                     AccountType = acctTypeElement.GetString();

                  if (transElement.TryGetProperty("Amount", out JsonElement amountElement))
                     Amount = FormatAmount(amountElement.ToString());

                  if (transElement.TryGetProperty("TotalAmount", out JsonElement totalAmtElement))
                     TotalAmount = FormatAmount(totalAmtElement.ToString());

                  if (transElement.TryGetProperty("TotalCashAmount", out JsonElement cashAmtElement))
                     TotalCashAmount = FormatAmount(cashAmtElement.ToString());

                  if (transElement.TryGetProperty("TotalCheckAmount", out JsonElement checkAmtElement))
                     TotalCheckAmount = FormatAmount(checkAmtElement.ToString());

                  if (transElement.TryGetProperty("BillsInserted", out JsonElement billsElement))
                     BillsInserted = billsElement.ToString();
               }

               // TransactionInfo
               if (root.TryGetProperty("TransactionInfo", out JsonElement infoElement))
               {
                  if (infoElement.TryGetProperty("Track2", out JsonElement track2Element))
                     Track2 = track2Element.GetString();
               }

               // TransactionFlag
               if (root.TryGetProperty("TransactionFlag", out JsonElement flagElement))
               {
                  if (flagElement.TryGetProperty("IsContactlessTransaction", out JsonElement contactlessElement))
                  {
                     if (contactlessElement.ValueKind == JsonValueKind.True || contactlessElement.ValueKind == JsonValueKind.False)
                        IsContactless = FormatAvailability(contactlessElement.GetBoolean());
                     else
                        IsContactless = FormatAvailability(contactlessElement.ToString());
                  }
               }
            }
         }
         catch (JsonException)
         {
            // JSON parsing failed - leave fields as null
         }
      }

      private void ParsePipeDelimitedPayload()
      {
         if (string.IsNullOrEmpty(Payload)) return;

         // Split by pipe delimiter
         string[] pairs = Payload.Split('|');

         foreach (string pair in pairs)
         {
            // Split by " - " (space-dash-space)
            int sepIdx = pair.IndexOf(" - ");
            if (sepIdx < 0) continue;

            string key = pair.Substring(0, sepIdx).Trim();
            string value = pair.Substring(sepIdx + 3).Trim();

            // Map to properties
            switch (key)
            {
               // Session
               case "CurrentSession.OnUs":
                  OnUs = FormatOnUs(value);
                  break;
               case "CurrentSession.CoreAvailable":
                  CoreAvailable = FormatAvailability(value);
                  break;
               case "CurrentSession.NDCAvailable":
                  NDCAvailable = FormatAvailability(value);
                  break;
               case "CurrentSession.Language":
                  Language = value;
                  break;
               case "CurrentSession.IdentificationNumber":
                  CustomerId = value;
                  break;

               // Transaction
               case "CurrentTransaction.Type":
                  TransactionType = value;
                  break;
               case "CurrentTransaction.Message":
                  Message = value;
                  break;
               case "CurrentTransaction.AccountNumber":
                  AccountNumber = value;
                  break;
               case "CurrentTransaction.AccountType":
                  AccountType = value;
                  break;
               case "CurrentTransaction.Amount":
                  Amount = FormatAmount(value);
                  break;
               case "CurrentTransaction.TotalAmount":
                  TotalAmount = FormatAmount(value);
                  break;
               case "CurrentTransaction.TotalCashAmount":
                  TotalCashAmount = FormatAmount(value);
                  break;
               case "CurrentTransaction.TotalCheckAmount":
                  TotalCheckAmount = FormatAmount(value);
                  break;
               case "CurrentTransaction.BillsInserted":
                  BillsInserted = value;
                  break;

               // Flags
               case "TransactionFlag.IsContactlessTransaction":
                  IsContactless = FormatAvailability(value);
                  break;

               // Card
               case "TransactionInfo.Track2":
                  Track2 = value;
                  break;
            }
         }
      }

      public static new ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (logLine.Contains("FlowPoint.LogTransactionData]"))
         {
            return new LogTransactionData(logFileHandler, logLine);
         }
         return null;
      }
   }
}
