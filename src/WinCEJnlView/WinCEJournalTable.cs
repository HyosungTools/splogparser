using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace WinCEJournalView
{
   /// <summary>
   /// Table class for WinCE electronic journal data.
   /// Processes WinCEJournalLine objects into DataTable rows matching EJViewer output format.
   /// </summary>
   internal class WinCEJournalTable : BaseTable
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="ctx">Application context</param>
      /// <param name="viewName">Name of the parent view</param>
      public WinCEJournalTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process a log line into a table row.
      /// Maps WinCEJournalLine properties to columns matching EJViewer format.
      /// </summary>
      /// <param name="logLine">The log line to process</param>
      public override void ProcessRow(ILogLine logLine)
      {
         if (!(logLine is WinCEJournalLine journalLine))
         {
            return;
         }

         try
         {
            // Always add to Summary
            AddSummaryRow(journalLine);

            // Add to detail worksheets based on type
            if (journalLine.journalType == WinCEJournalType.Transaction)
            {
               AddTransactionDetailsRow(journalLine);
            }
            else if (journalLine.journalType == WinCEJournalType.EMV)
            {
               AddEMVDetailsRow(journalLine);
            }
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine($"WinCEJournalTable.ProcessRow exception: {ex.Message}");
         }
      }

      /// <summary>
      /// Prep the tables for Excel - add English descriptions
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         string tableName = string.Empty;

         try
         {
            // S U M M A R Y   T A B L E
            tableName = "Summary";

            // Add English descriptions to Summary table
            AddEnglishToSummaryTable();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Exception processing the {tableName} table - {e.Message}");
         }

         try
         {
            // T R A N S A C T I O N   D E T A I L S   T A B L E
            tableName = "TransactionDetails";

            // Add English descriptions to TransactionDetails table
            AddEnglishToTransactionDetailsTable();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Exception processing the {tableName} table - {e.Message}");
         }

         return base.WriteExcelFile();
      }

      /// <summary>
      /// Add English descriptions to Summary table columns
      /// </summary>
      private void AddEnglishToSummaryTable()
      {
         if (!dTableSet.Tables.Contains("Summary"))
            return;

         DataTable summaryTable = dTableSet.Tables["Summary"];

         foreach (DataRow row in summaryTable.Rows)
         {
            try
            {
               // typedesc: Extract main type (first char) from type column and look up
               string fullType = row["type"]?.ToString() ?? string.Empty;
               if (!string.IsNullOrEmpty(fullType) && fullType.Length > 0)
               {
                  string mainType = fullType.Substring(0, 1);
                  row["typedesc"] = LookupMessage("MainType", mainType);
               }

               // trantypedesc: Look up trantype code
               string tranType = row["trantype"]?.ToString() ?? string.Empty;
               if (!string.IsNullOrEmpty(tranType))
               {
                  row["trantypedesc"] = LookupMessage("TranType", tranType);
               }

               // description: Look up KindCode (extract from type column after underscore)
               if (!string.IsNullOrEmpty(fullType) && fullType.Contains("_"))
               {
                  string kindCode = fullType.Substring(fullType.IndexOf('_') + 1);
                  row["description"] = LookupMessage("KindCode", kindCode);
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine($"AddEnglishToSummaryTable row exception: {ex.Message}");
            }
         }

         summaryTable.AcceptChanges();
      }

      /// <summary>
      /// Add English descriptions to TransactionDetails table columns
      /// </summary>
      private void AddEnglishToTransactionDetailsTable()
      {
         if (!dTableSet.Tables.Contains("TransactionDetails"))
            return;

         DataTable detailsTable = dTableSet.Tables["TransactionDetails"];

         foreach (DataRow row in detailsTable.Rows)
         {
            try
            {
               // trantypedesc: Look up trantype code
               string tranType = row["trantype"]?.ToString() ?? string.Empty;
               if (!string.IsNullOrEmpty(tranType))
               {
                  row["trantypedesc"] = LookupMessage("TranType", tranType);
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine($"AddEnglishToTransactionDetailsTable row exception: {ex.Message}");
            }
         }

         detailsTable.AcceptChanges();
      }

      /// <summary>
      /// Add row to Summary worksheet
      /// </summary>
      private void AddSummaryRow(WinCEJournalLine journalLine)
      {
         DataRow dataRow = dTableSet.Tables["Summary"].NewRow();

         dataRow["file"] = journalLine.LogFile;
         dataRow["time"] = FormatStackDateTime(journalLine.Timestamp);
         dataRow["stacknum"] = journalLine.StackNum;
         dataRow["type"] = journalLine.FullType;
         dataRow["typedesc"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["trantype"] = journalLine.TranType;
         dataRow["trantypedesc"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["sequence"] = journalLine.TranSequence;
         dataRow["description"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["requestedamt"] = journalLine.RequestedAmount;
         dataRow["dispensedamt"] = journalLine.DispensedAmount;
         dataRow["surchargeamt"] = journalLine.SurchargeAmount;
         dataRow["errorcode"] = journalLine.ErrorCode;
         dataRow["trandatetime"] = journalLine.TranDateTime;

         dTableSet.Tables["Summary"].Rows.Add(dataRow);
      }

      /// <summary>
      /// Add row to TransactionDetails worksheet
      /// </summary>
      private void AddTransactionDetailsRow(WinCEJournalLine journalLine)
      {
         if (!dTableSet.Tables.Contains("TransactionDetails"))
            return;

         DataRow dataRow = dTableSet.Tables["TransactionDetails"].NewRow();

         dataRow["file"] = journalLine.LogFile;
         dataRow["time"] = FormatStackDateTime(journalLine.Timestamp);
         dataRow["stacknum"] = journalLine.StackNum;
         dataRow["terminalid"] = journalLine.TerminalID;
         dataRow["transno"] = journalLine.TranSequence;
         dataRow["trantype"] = journalLine.TranType;
         dataRow["trantypedesc"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["fromaccount"] = journalLine.FromAccount;
         dataRow["toaccount"] = journalLine.ToAccount;
         dataRow["bankcode"] = journalLine.BankCode;
         dataRow["hostdate"] = journalLine.HostDate;
         dataRow["hosttime"] = journalLine.HostTime;
         dataRow["availablebalance"] = journalLine.AvailableBalance;
         dataRow["retrievalno"] = journalLine.RetrievalNo;
         dataRow["authorizationno"] = journalLine.AuthorizationNo;
         dataRow["settledate"] = journalLine.SettleDate;
         dataRow["surchargeamt"] = journalLine.SurchargeAmount;
         dataRow["requestedamt"] = journalLine.RequestedAmount;
         dataRow["dispensedamt"] = journalLine.DispensedAmount;
         dataRow["ledgerbalance"] = journalLine.LedgerBalance;
         dataRow["procedurecount"] = journalLine.ProcedureCount;
         dataRow["transactionresult"] = journalLine.TransactionResult;
         dataRow["errorcode"] = journalLine.ErrorCode;
         dataRow["carddata"] = journalLine.CardData;
         dataRow["noncashvalue"] = journalLine.NonCashValue;
         dataRow["noncashtype"] = journalLine.NonCashType;

         dTableSet.Tables["TransactionDetails"].Rows.Add(dataRow);
      }

      /// <summary>
      /// Add row to EMVDetails worksheet
      /// </summary>
      private void AddEMVDetailsRow(WinCEJournalLine journalLine)
      {
         if (!dTableSet.Tables.Contains("EMVDetails"))
            return;

         DataRow dataRow = dTableSet.Tables["EMVDetails"].NewRow();

         dataRow["file"] = journalLine.LogFile;
         dataRow["time"] = FormatStackDateTime(journalLine.Timestamp);
         dataRow["stacknum"] = journalLine.StackNum;
         dataRow["terminalid"] = journalLine.TerminalID;
         dataRow["transno"] = journalLine.TranSequence;
         dataRow["authorizationno"] = journalLine.AuthorizationNo;
         dataRow["aid"] = journalLine.AID;
         dataRow["appname"] = journalLine.AppName;
         dataRow["arqc"] = journalLine.ARQC;
         dataRow["arpc"] = journalLine.ARPC;
         dataRow["servicecode"] = journalLine.ServiceCode;
         dataRow["terminalcapability"] = journalLine.TerminalCapability;
         dataRow["posentrymode"] = journalLine.POSEntryMode;
         dataRow["tvr"] = journalLine.TVR;
         dataRow["issueractioncode"] = journalLine.IssuerActionCode;
         dataRow["rawdata"] = journalLine.EMVRawData;

         dTableSet.Tables["EMVDetails"].Rows.Add(dataRow);
      }

      /// <summary>
      /// Format timestamp to MM/DD/YYYY HH:MM:SS format
      /// </summary>
      private string FormatStackDateTime(string timestamp)
      {
         if (string.IsNullOrEmpty(timestamp) || timestamp.Length < 19)
            return timestamp;

         try
         {
            // Input: 2024-11-05 00:12:06.000
            // Output: 11/05/2024 00:12:06
            string year = timestamp.Substring(0, 4);
            string month = timestamp.Substring(5, 2);
            string day = timestamp.Substring(8, 2);
            string time = timestamp.Substring(11, 8);

            return $"{month}/{day}/{year} {time}";
         }
         catch
         {
            return timestamp;
         }
      }

      /// <summary>
      /// Generic lookup in Messages table by type and code
      /// </summary>
      private string LookupMessage(string messageType, string code)
      {
         if (string.IsNullOrEmpty(code))
            return string.Empty;

         try
         {
            if (dTableSet.Tables.Contains("Messages"))
            {
               DataTable messagesTable = dTableSet.Tables["Messages"];
               foreach (DataRow row in messagesTable.Rows)
               {
                  string rowType = row["type"]?.ToString();
                  string rowCode = row["code"]?.ToString();
                  if (rowType == messageType && rowCode == code)
                  {
                     return row["description"]?.ToString() ?? string.Empty;
                  }
               }
            }
         }
         catch
         {
            // Return empty on lookup failure
         }

         return string.Empty;
      }
   }
}
