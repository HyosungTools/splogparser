using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace TransDataView
{
   class TransDataTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public TransDataTable(IContext ctx, string viewName) : base(ctx, viewName)
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
            if (logLine is LogTransactionData transactionData)
            {
               base.ProcessRow(logLine);

               switch (transactionData.apType)
               {
                  case APLogType.APLOG_LOGTRANSACTIONDATA:
                     {
                        LOGTRANSACTIONDATA(transactionData);
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("TransDataTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void LOGTRANSACTIONDATA(LogTransactionData transactionData)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["TransData"].Rows.Add();

            dataRow["file"] = transactionData.LogFile;
            dataRow["time"] = transactionData.Timestamp;
            dataRow["error"] = transactionData.HResult;

            dataRow["tid"] = transactionData.TID ?? string.Empty;
            dataRow["flowpoint"] = transactionData.FlowPoint ?? string.Empty;
            dataRow["fp"] = transactionData.FP ?? string.Empty;

            // Session
            dataRow["onus"] = transactionData.OnUs ?? string.Empty;
            dataRow["coreavailable"] = transactionData.CoreAvailable ?? string.Empty;
            dataRow["ndcavailable"] = transactionData.NDCAvailable ?? string.Empty;
            dataRow["language"] = transactionData.Language ?? string.Empty;
            dataRow["customerid"] = transactionData.CustomerId ?? string.Empty;

            // Transaction
            dataRow["transactiontype"] = transactionData.TransactionType ?? string.Empty;
            dataRow["message"] = transactionData.Message ?? string.Empty;
            dataRow["accountnumber"] = transactionData.AccountNumber ?? string.Empty;
            dataRow["accounttype"] = transactionData.AccountType ?? string.Empty;
            dataRow["amount"] = transactionData.Amount ?? string.Empty;
            dataRow["totalamount"] = transactionData.TotalAmount ?? string.Empty;
            dataRow["totalcashamount"] = transactionData.TotalCashAmount ?? string.Empty;
            dataRow["totalcheckamount"] = transactionData.TotalCheckAmount ?? string.Empty;
            dataRow["billsinserted"] = transactionData.BillsInserted ?? string.Empty;

            // Flags
            dataRow["iscontactless"] = transactionData.IsContactless ?? string.Empty;

            // Card
            dataRow["track2"] = transactionData.Track2 ?? string.Empty;

            // Raw payload
            dataRow["payload"] = transactionData.Payload ?? string.Empty;

            dTableSet.Tables["TransData"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("LOGTRANSACTIONDATA Exception : " + e.Message);
         }
      }
   }
}
