using RegEx;
using Contract;
using System;
using System.Collections.Generic;

namespace LogLineHandler
{
   public class EJInsert : APLine
   {
      public string ejTable = string.Empty;
      public string[] ejColumns = null;
      public string[] ejValues = null;

      public EJInsert(ILogFileHandler parent, string logLine, APLogType apType = APLogType.EJInsert) : base(parent, logLine, apType)
      {
      }

      static List<string> ExtractOutermostTerms(string input)
      {
         Stack<int> stack = new Stack<int>();
         List<string> terms = new List<string>();
         int start = -1;

         for (int i = 0; i < input.Length; i++)
         {
            char current = input[i];

            if (current == '(')
            {
               if (stack.Count == 0)
               {
                  start = i; // Mark the start of an outermost term
               }
               stack.Push(i);
            }
            else if (current == ')')
            {
               stack.Pop();
               if (stack.Count == 0) // End of an outermost term
               {
                  // Add the term without brackets
                  terms.Add(input.Substring(start + 1, i - start - 1));
               }
            }
         }

         return terms;
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.IndexOf("INSERT INTO ");
         if (idx == -1)
         {
            return;
         }
         string subLogLine = string.Empty; 
         try
         {

            // e.g. INSERT INTO[Session] (ATMId, StartDate, IdentificationType) VALUES('NM000560','01/16/2024 16:58:24 PM','TellerIdentification')
            // e.g. INSERT INTO[Transaction] (ATMId, IdRelatedTx, SessionId, TellerId,[ATMDateTime], TransactionDateTime, TransactionType, AccountNumberMasked, AccountNumber, AccountType, AmountRequested, AmountDispensed, AmountDeposited, HostType, TotalCashAmount, TotalCheckAmount, TotalChecksDeposited, Success) VALUES('NM000560', 0, 1888, 'Jorge', '1/16/2024 4:58:24 PM', '1/16/2024 4:58:24 PM', 'CashDeposit', '7458', '9797458', 'Savings', 43, 0, 43, 'ActiveTeller', 43, 0, 0, True)
            // e.g. INSERT INTO ejournal(MachineNO, Code, EvtDescription) VALUES('NM000560', '2', '[Sensors]ProximityChanged -> PRESENT')
            // e.g. INSERT INTO ejournal(MachineNO,Code,EvtDescription) VALUES ('9410D203','2','TRANSACTION REQUESTING: OPcode[BA     A]')
            // e.g. INSERT INTO ejournal(MachineNO, Code, EvtDescription, OPCode, Track2, Amount, TranSerialNo, ReqCnt_A, ReqCnt_B, ReqCnt_C, ReqCnt_D, ReqCnt_E, ReqCnt_F, ReqCnt_G, Deno_1, Deno_2, Deno_3, Deno_4, DispCnt_1, DispCnt_2, DispCnt_3, DispCnt_4, RemCnt_1, RemCnt_2, RemCnt_3, RemCnt_4, PickCnt_1, PickCnt_2, PickCnt_3, PickCnt_4, RejCnt_1, RejCnt_2, RejCnt_3, RejCnt_4, Line_1, Line_4, Line_6, Line_7, Line_8, Line_9, Line_10, Line_13, Line_14, Line_16, Line_17, Line_19, Line_20) VALUES('9410D203', '1', 'TRANSACTION DATA (COMPLETED)', 'BA     A', '510639XXXXXX4256', '00060000', '8113', '00', '00', '00', '06', '0', '0', '0', 'A', 'B', 'C', 'D', '0', '0', '0', '6', '1897', '703', '808', '464', '0', '0', '0', '6', '0', '0', '0', '0', '09/29/24  18:05     9410D203', '************4256-0', '164-20 NORTHERN BLVD ', 'FLUSHING, NY', 'RECORD NO.              8113', 'WITHDRAWAL        $   600.00', 'FROM CHECKING', 'TOTAL            $   2430.98 ', 'AVAILABLE        $   2430.98 ', 'US Debit                        ', 'A0000000042203', '


            // process the log line to remove all redundant text
            subLogLine = logLine.Substring(idx);
            subLogLine = subLogLine.Replace("INSERT INTO", "");
            subLogLine = subLogLine.Replace("[Session] ", "Session ");
            subLogLine = subLogLine.Replace("[Transaction] ", "Transaction ");
            subLogLine = subLogLine.Replace("ejournal(", "ejournal (");

            idx = subLogLine.IndexOf("(");
            if (idx == -1)
            {
               return;
            }

            // Session, Transaction or ejournal
            ejTable = subLogLine.Substring(0, idx).Trim();
            subLogLine = subLogLine.Substring(idx).Trim();

            // Get rid of VALUES, leaving 2 terms - columns and values
            subLogLine = subLogLine.Replace("VALUES", "");

            // There are two terms - the columns term and the values term. Isolate both terms 
            List<String> terms = ExtractOutermostTerms(subLogLine);
            if (terms.Count != 2)
            {
               this.parentHandler.ctx.ConsoleWriteLogLine(String.Format("Failed to isolate two terms: {0}", subLogLine));
            }

            ejColumns = terms[0].Split(',');
            ejValues = terms[1].Split(',');

            for (int i = 0; i < ejColumns.Length; i++)
            {
               ejColumns[i] = ejColumns[i].Replace("'","").Trim();
               ejValues[i] = ejValues[i].Replace("'", "").Trim();
            }
         }
         catch(Exception e)
         {
            parentHandler.ctx.ConsoleWriteLogLine(String.Format("EJInsert EXCEPTION Failed to parse : {0}", subLogLine));
         }
      }
   }
}
