using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace WSView
{
   class WSTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public WSTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is Core coreLine)
            {
               switch (coreLine.apType)
               {
                  /* Customer Identification */
                  case APLogType.Core_ProcessCustomerIdentification_Start:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "ProcessCustomerIdentification", "Start", string.Empty);
                        break;
                     }
                  case APLogType.Core_ProcessCustomerIdentification_LocateCustomer:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "ProcessCustomerIdentification", "LocateCustomer", string.Empty);
                        break;
                     }
                  case APLogType.Core_ProcessCustomerWarning:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "ProcessCustomerWarning", "Start", string.Empty);
                        break;
                     }

                  /* Account Lists */
                  case APLogType.Core_MakeAccountLists_Start:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "MakeAccountLists", "Start", string.Empty);
                        break;
                     }
                  case APLogType.Core_MakeAccountLists_AgreementCount:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_MakeAccountLists_AgreementCount agreementCount)
                        {
                           WS_ROW_ACCOUNTS(agreementCount, "MakeAccountLists", "AgreementCount", 
                              $"eAgreement={agreementCount.eAgreementCount}, cardAgreement={agreementCount.cardAgreementCount}");
                        }
                        break;
                     }
                  case APLogType.Core_MakeAccountLists_TotalAccounts:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_MakeAccountLists_TotalAccounts totalAccounts)
                        {
                           WS_ROW_ACCOUNTCOUNT(totalAccounts, "MakeAccountLists", "TotalAccounts", totalAccounts.accountCount);
                        }
                        break;
                     }
                  case APLogType.Core_MakeAccountLists_UniqueAccounts:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_MakeAccountLists_UniqueAccounts uniqueAccounts)
                        {
                           WS_ROW_ACCOUNTCOUNT(uniqueAccounts, "MakeAccountLists", "UniqueAccounts", uniqueAccounts.accountCount);
                        }
                        break;
                     }

                  /* Flow Navigation */
                  case APLogType.Core_DetermineNextFlowPoint:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_DetermineNextFlowPoint nextFlowPoint)
                        {
                           WS_ROW_FLOWPOINT(nextFlowPoint, "DetermineNextFlowPoint", nextFlowPoint.nextFlowPoint);
                        }
                        break;
                     }

                  /* Withdrawal Transaction */
                  case APLogType.Core_ProcessWithdrawalTransaction_Start:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "ProcessWithdrawalTransaction", "Start", string.Empty);
                        break;
                     }
                  case APLogType.Core_ProcessWithdrawalTransaction_Account:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessWithdrawalTransaction_Account accountLine)
                        {
                           WS_ROW_ACCOUNT(accountLine, "ProcessWithdrawalTransaction", "Account", accountLine.account);
                        }
                        break;
                     }
                  case APLogType.Core_ProcessWithdrawalTransaction_Amount:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessWithdrawalTransaction_Amount amountLine)
                        {
                           WS_ROW_AMOUNT(amountLine, "ProcessWithdrawalTransaction", "Amount", amountLine.amount);
                        }
                        break;
                     }
                  case APLogType.Core_ProcessWithdrawalTransaction_SequenceNumber:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessTransaction_SequenceNumber seqLine)
                        {
                           WS_ROW_SEQUENCE(seqLine, "ProcessWithdrawalTransaction", "SequenceNumber", seqLine.sequenceNumber);
                        }
                        break;
                     }

                  /* Deposit Transaction */
                  case APLogType.Core_ProcessDepositTransaction_Start:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "ProcessDepositTransaction", "Start", string.Empty);
                        break;
                     }
                  case APLogType.Core_ProcessDepositTransaction_Account:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessDepositTransaction_Account accountLine)
                        {
                           WS_ROW_ACCOUNT(accountLine, "ProcessDepositTransaction", "Account", accountLine.account);
                        }
                        break;
                     }
                  case APLogType.Core_ProcessDepositTransaction_CashAmount:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessDepositTransaction_CashAmount cashLine)
                        {
                           WS_ROW_CASHAMOUNT(cashLine, "ProcessDepositTransaction", "CashAmount", cashLine.cashAmount);
                        }
                        break;
                     }
                  case APLogType.Core_ProcessDepositTransaction_CheckAmount:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessDepositTransaction_CheckAmount checkLine)
                        {
                           WS_ROW_CHECKAMOUNT(checkLine, "ProcessDepositTransaction", "CheckAmount", checkLine.checkAmount);
                        }
                        break;
                     }
                  case APLogType.Core_ProcessDepositTransaction_TotalAmount:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessDepositTransaction_TotalAmount totalLine)
                        {
                           WS_ROW_AMOUNT(totalLine, "ProcessDepositTransaction", "TotalAmount", totalLine.amount);
                        }
                        break;
                     }
                  case APLogType.Core_ProcessDepositTransaction_SequenceNumber:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_ProcessTransaction_SequenceNumber seqLine)
                        {
                           WS_ROW_SEQUENCE(seqLine, "ProcessDepositTransaction", "SequenceNumber", seqLine.sequenceNumber);
                        }
                        break;
                     }
                  case APLogType.Core_StoreCash:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_StoreCash storeCash)
                        {
                           WS_ROW_RESULT(storeCash, "StoreCash", "Result", storeCash.result);
                        }
                        break;
                     }

                  /* Transaction Reply/Journal */
                  case APLogType.Core_WriteTransactionReplyJournal_StatusCode:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_WriteTransactionReplyJournal_StatusCode statusLine)
                        {
                           WS_ROW_STATUSCODE(statusLine, "WriteTransactionReplyJournal", "StatusCode", statusLine.statusCode);
                        }
                        break;
                     }
                  case APLogType.Core_WriteTransactionReplyJournal_SequenceNumber:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_WriteTransactionReplyJournal_SequenceNumber seqLine)
                        {
                           WS_ROW_SEQUENCE(seqLine, "WriteTransactionReplyJournal", "SequenceNumber", seqLine.sequenceNumber);
                        }
                        break;
                     }

                  /* Dispense Currency */
                  case APLogType.Core_DispenseCurrency_BillMix:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_DispenseCurrency_BillMix billMixLine)
                        {
                           WS_ROW_BILLMIX(billMixLine, "DispenseCurrency", "BillMix", billMixLine.billMix, billMixLine.transactionAmount);
                        }
                        break;
                     }
                  case APLogType.Core_RequiredBillMixList:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_RequiredBillMixList billMixList)
                        {
                           WS_ROW_BILLMIX(billMixList, "DispenseCurrency", "RequiredBillMixList", billMixList.requiredbillmixlist, string.Empty);
                        }
                        break;
                     }
                  case APLogType.Core_DispensedAmount:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_DispensedAmount dispensedAmount)
                        {
                           WS_ROW_DISPENSED(dispensedAmount, "DispenseCurrency", "DispensedAmount", dispensedAmount.amount);
                        }
                        break;
                     }
                  case APLogType.Core_UpdateDataFrameworkAmount:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_UpdateDataFrameworkAmount updateAmount)
                        {
                           WS_ROW_DISPENSE_VERIFY(updateAmount, "UpdateDataFrameworkAmount", 
                              updateAmount.dispensedAmount, updateAmount.requiredAmount);
                        }
                        break;
                     }

                  /* Card Operations */
                  case APLogType.Core_EjectCard_Command:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "EjectCard", "Command", string.Empty);
                        break;
                     }
                  case APLogType.Core_EjectCard_Result:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_EjectCard_Result ejectResult)
                        {
                           WS_ROW_RESULT(ejectResult, "EjectCard", "Result", ejectResult.result);
                        }
                        break;
                     }
                  case APLogType.Core_EjectCard_MediaStatus:
                     {
                        base.ProcessRow(logLine);
                        if (coreLine is Core_EjectCard_MediaStatus mediaStatus)
                        {
                           WS_ROW_RESULT(mediaStatus, "EjectCard", "MediaStatus", mediaStatus.mediaStatus);
                        }
                        break;
                     }

                  /* Balance Update */
                  case APLogType.Core_UpdateBalances:
                     {
                        base.ProcessRow(logLine);
                        WS_ROW(coreLine, "UpdateBalances", "Start", string.Empty);
                        break;
                     }

                  default:
                     break;
               };
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("WSTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Basic row with method/action only
      /// </summary>
      protected void WS_ROW(Core coreLine, string method, string action, string comment)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = comment;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with account number
      /// </summary>
      protected void WS_ROW_ACCOUNT(Core coreLine, string method, string action, string account)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = account;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_ACCOUNT Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with amount
      /// </summary>
      protected void WS_ROW_AMOUNT(Core coreLine, string method, string action, string amount)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = amount;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_AMOUNT Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with cash amount (deposits)
      /// </summary>
      protected void WS_ROW_CASHAMOUNT(Core coreLine, string method, string action, string cashAmount)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = cashAmount;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_CASHAMOUNT Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with check amount (deposits)
      /// </summary>
      protected void WS_ROW_CHECKAMOUNT(Core coreLine, string method, string action, string checkAmount)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = checkAmount;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_CHECKAMOUNT Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with sequence number
      /// </summary>
      protected void WS_ROW_SEQUENCE(Core coreLine, string method, string action, string sequenceNumber)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = sequenceNumber;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_SEQUENCE Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with status code (host response)
      /// </summary>
      protected void WS_ROW_STATUSCODE(Core coreLine, string method, string action, string statusCode)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = statusCode;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_STATUSCODE Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with bill mix
      /// </summary>
      protected void WS_ROW_BILLMIX(Core coreLine, string method, string action, string billMix, string amount)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = dataRow["amount"] = decimal.TryParse(amount, out decimal cents) ? (cents / 100m).ToString("0.00") : amount;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = billMix;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_BILLMIX Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with dispensed amount
      /// </summary>
      protected void WS_ROW_DISPENSED(Core coreLine, string method, string action, string dispensedAmount)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = dispensedAmount;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_DISPENSED Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with dispense verification (dispensed vs required)
      /// </summary>
      protected void WS_ROW_DISPENSE_VERIFY(Core coreLine, string action, string dispensedAmount, string requiredAmount)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = "UpdateDataFrameworkAmount";
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = dispensedAmount;
            dataRow["requiredAmount"] = requiredAmount;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_DISPENSE_VERIFY Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with next flow point
      /// </summary>
      protected void WS_ROW_FLOWPOINT(Core coreLine, string method, string nextFlowPoint)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = "NextFlowPoint";
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = nextFlowPoint
                .Replace("PLACEHOLDER-", "")
                .Replace("Common-", "");
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_FLOWPOINT Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with account count
      /// </summary>
      protected void WS_ROW_ACCOUNTCOUNT(Core coreLine, string method, string action, string accountCount)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = accountCount;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_ACCOUNTCOUNT Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with accounts (agreement counts)
      /// </summary>
      protected void WS_ROW_ACCOUNTS(Core coreLine, string method, string action, string comment)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = string.Empty;
            dataRow["comment"] = comment;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_ACCOUNTS Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Row with result
      /// </summary>
      protected void WS_ROW_RESULT(Core coreLine, string method, string action, string result)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = coreLine.LogFile;
            dataRow["time"] = coreLine.Timestamp;
            dataRow["error"] = coreLine.HResult;
            dataRow["core"] = coreLine.name;
            dataRow["method"] = method;
            dataRow["action"] = action;
            dataRow["account"] = string.Empty;
            dataRow["amount"] = string.Empty;
            dataRow["cashAmount"] = string.Empty;
            dataRow["checkAmount"] = string.Empty;
            dataRow["sequenceNumber"] = string.Empty;
            dataRow["statusCode"] = string.Empty;
            dataRow["billMix"] = string.Empty;
            dataRow["dispensedAmount"] = string.Empty;
            dataRow["requiredAmount"] = string.Empty;
            dataRow["nextFlowPoint"] = string.Empty;
            dataRow["accountCount"] = string.Empty;
            dataRow["result"] = result;
            dataRow["comment"] = string.Empty;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WS_ROW_RESULT Exception : " + e.Message);
         }
      }
   }
}
