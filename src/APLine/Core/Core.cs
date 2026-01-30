using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core : APLine
   {
      public string name;

      public Core(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         name = string.Empty;

         // e.g. [KeyBridgeWebServiceRequestFlowPoint] or [FiservDNAWebServiceRequestFlowPoint]
         Regex regex = new Regex("^.*\\[(?<corename>.*?)WebServiceRequestFlowPoint.*$");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            name = m.Groups["corename"].Value;
         }
      }

      public static new ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         /* ================================================================
          * CORE - WebServiceRequestFlowPoint
          * ================================================================ */

         // ----------------------------------------------------------------
         // Customer Identification
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ProcessCustomerIdentification"))
         {
            if (logLine.Contains("Start ProcessCustomerIdentification"))
               return new Core(logFileHandler, logLine, APLogType.Core_ProcessCustomerIdentification_Start);

            if (logLine.Contains("About to call LocateCustomer"))
               return new Core(logFileHandler, logLine, APLogType.Core_ProcessCustomerIdentification_LocateCustomer);
         }

         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ProcessCustomerWarning") && logLine.Contains("Start"))
            return new Core(logFileHandler, logLine, APLogType.Core_ProcessCustomerWarning);

         // ----------------------------------------------------------------
         // Account Lists
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("MakeAccountLists"))
         {
            if (logLine.Contains("Start MakeAccountLists"))
               return new Core(logFileHandler, logLine, APLogType.Core_MakeAccountLists_Start);

            if (logLine.Contains("eAgreement count="))
               return new Core_MakeAccountLists_AgreementCount(logFileHandler, logLine);

            if (logLine.Contains("Pulled all agreement accounts"))
               return new Core_MakeAccountLists_TotalAccounts(logFileHandler, logLine);

            if (logLine.Contains("After removing duplicates"))
               return new Core_MakeAccountLists_UniqueAccounts(logFileHandler, logLine);
         }

         // ----------------------------------------------------------------
         // Flow Navigation
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("DetermineNextFlowPoint") && logLine.Contains("Next flow point identified as"))
            return new Core_DetermineNextFlowPoint(logFileHandler, logLine);

         // ----------------------------------------------------------------
         // Withdrawal Transaction
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ProcessWithdrawalTransaction"))
         {
            if (logLine.Contains("Start ProcessWithdrawalTransaction"))
               return new Core(logFileHandler, logLine, APLogType.Core_ProcessWithdrawalTransaction_Start);

            // FiservDNA: "Account Number:" or JackHenry: "Account Number -"
            if (logLine.Contains("Account Number"))
               return new Core_ProcessWithdrawalTransaction_Account(logFileHandler, logLine);

            // FiservDNA/SymXchange/etc: "Total Amount:" or JackHenry: "Amount -" or CMCFlex: "Amount:" 
            // (but not "Dispensed amount")
            if (logLine.Contains("Total Amount:") || 
                logLine.Contains("Amount -") || 
                (logLine.Contains("Amount:") && !logLine.Contains("Dispensed")))
               return new Core_ProcessWithdrawalTransaction_Amount(logFileHandler, logLine);

            if (logLine.Contains("Sequence Number") || logLine.Contains("Sequence number"))
               return new Core_ProcessTransaction_SequenceNumber(logFileHandler, logLine, APLogType.Core_ProcessWithdrawalTransaction_SequenceNumber);
         }

         // ----------------------------------------------------------------
         // Deposit Transaction
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ProcessDepositTransaction"))
         {
            if (logLine.Contains("Start ProcessDepositTransaction"))
               return new Core(logFileHandler, logLine, APLogType.Core_ProcessDepositTransaction_Start);

            if (logLine.Contains("Account Number:"))
               return new Core_ProcessDepositTransaction_Account(logFileHandler, logLine);

            if (logLine.Contains("Cash Amount:"))
               return new Core_ProcessDepositTransaction_CashAmount(logFileHandler, logLine);

            if (logLine.Contains("Check Amount:"))
               return new Core_ProcessDepositTransaction_CheckAmount(logFileHandler, logLine);

            if (logLine.Contains("Total Amount:"))
               return new Core_ProcessDepositTransaction_TotalAmount(logFileHandler, logLine);

            if (logLine.Contains("Sequence Number:"))
               return new Core_ProcessTransaction_SequenceNumber(logFileHandler, logLine, APLogType.Core_ProcessDepositTransaction_SequenceNumber);
         }

         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("StoreCash") && logLine.Contains("result="))
            return new Core_StoreCash(logFileHandler, logLine);

         // ----------------------------------------------------------------
         // Transaction Reply/Journal
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("WriteTransactionReplyJournal"))
         {
            if (logLine.Contains("Status Code:"))
               return new Core_WriteTransactionReplyJournal_StatusCode(logFileHandler, logLine);

            if (logLine.Contains("Sequence Number:"))
               return new Core_WriteTransactionReplyJournal_SequenceNumber(logFileHandler, logLine);
         }

         // ----------------------------------------------------------------
         // Dispense Currency
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("DispenseCurrency"))
         {
            if (logLine.Contains("Bill Mix:") && logLine.Contains("Transaction Amount:"))
               return new Core_DispenseCurrency_BillMix(logFileHandler, logLine);

            if (logLine.Contains("RequiredBillMixList:"))
               return new Core_RequiredBillMixList(logFileHandler, logLine);

            if (logLine.Contains("dispensedAmount="))
               return new Core_DispensedAmount(logFileHandler, logLine);
         }

         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("UpdateDataFrameworkAmount") && logLine.Contains("Dispensed amount:"))
            return new Core_UpdateDataFrameworkAmount(logFileHandler, logLine);

         // ----------------------------------------------------------------
         // Card Operations
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("EjectCard"))
         {
            if (logLine.Contains("Issuing command to eject"))
               return new Core(logFileHandler, logLine, APLogType.Core_EjectCard_Command);

            if (logLine.Contains("result is ["))
               return new Core_EjectCard_Result(logFileHandler, logLine);

            if (logLine.Contains("MediaStatus is ["))
               return new Core_EjectCard_MediaStatus(logFileHandler, logLine);
         }

         // ----------------------------------------------------------------
         // Balance Update
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("UpdateBalances") && logLine.Contains("Start"))
            return new Core(logFileHandler, logLine, APLogType.Core_UpdateBalances);

         // ----------------------------------------------------------------
         // Execute State (timing information)
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ExecuteState") && logLine.Contains("Time Elapsed"))
            return new Core_ExecuteState(logFileHandler, logLine);

         // ----------------------------------------------------------------
         // Transfer Transaction
         // ----------------------------------------------------------------
         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ProcessTransferTransaction"))
         {
            if (logLine.Contains("Donor Account"))
               return new Core_ProcessTransferTransaction_DonorAccount(logFileHandler, logLine);

            if (logLine.Contains("Recipient Account"))
               return new Core_ProcessTransferTransaction_RecipientAccount(logFileHandler, logLine);

            if (logLine.Contains("Amount:") || logLine.Contains("Amount -"))
               return new Core_ProcessTransferTransaction_Amount(logFileHandler, logLine);
         }

         return null;
      }
   }
}
