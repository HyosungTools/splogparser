namespace Samples
{
   public static class samples_ejinsert
   {
      public const string EJINSERT_1 =
@"[2023-07-29 00:00:10-785][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Session] (ATMId,StartDate,IdentificationType,IdentificationNumberMasked,IdentificationNumber,AuthenticationType) VALUES ('57261009','07/29/2023 00:00:10 AM','BankCard','5247','5537340003345247','PIN')";

      public const string EJINSERT_2 =
@"[2023-07-29 08:38:53-725][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Transaction] (ATMId,IdRelatedTx,SessionId,[ATMDateTime],TransactionDateTime,TransactionType,SequenceNumber,AccountNumber,AmountRequested,AmountDispensed,AmountDeposited,HostType,TotalCashAmount,TotalCheckAmount,TotalChecksDeposited,Success) VALUES ('57261009',0,601,'7/29/2023 8:38:53 AM','7/29/2023 8:38:53 AM','PinAuthentication','2648','007',0,0,0,'NDC',0,0,0,True)";

      public const string EJINSERT_3 =
@"[2023-09-12 07:30:39-094][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Session] (ATMId,StartDate,IdentificationType,IdentificationNumberMasked,AuthenticationType) VALUES ('T2790017','09/12/2023 07:30:39 AM','BankCard','0029','PIN')";

      public const string EJINSERT_4 =
@"[2023-09-12 07:30:39-109][3][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO [Transaction] (ATMId,IdRelatedTx,SessionId,[ATMDateTime],TransactionDateTime,TransactionType,SequenceNumber,AccountNumberMasked,AccountType,AmountRequested,AmountDispensed,AmountDeposited,HostType,TotalCashAmount,TotalCheckAmount,TotalChecksDeposited,Success) VALUES ('T2790017',0,810,'9/12/2023 7:30:38 AM','9/12/2023 7:30:38 AM','CheckDeposit','7087','3159','Checking',230.03,0,230.03,'NDC',0,230.03,1,True)";
   }
}
