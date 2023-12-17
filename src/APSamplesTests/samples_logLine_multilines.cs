namespace Samples
{
   public static class samples_logLine_multilines
   {
      public const string SAMPLE_MULTILINE_1 =
         @"[2023-03-30 18:51:46-749][3][][SockAdapter         ][RecvProcAsync       ][NORMAL]Queued total len : 412
[2023-03-30 18:51:46-751][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 415200040157978105150B@      CREDIT CASH ADVANCE       C@166M@176:01 CARD NO: 546316XXXXXX1538


3LOCATION: 2929 3RD AVE N
:ESCANABA?4MI
A0000000041010
MASTERCARD

 CARD NO: XXXXXXXXXXXX1538

5DATE8TIME8TERMINAL
303/30/23606:52PM6CKI561


SEQ NBR:  57978AMT:7$40.00
?ATM OWNER FEE6$2.00
?TOTAL=$42.00
CREDIT CASH ADVANCE

?THANK YOU
>011, """"  """"5CAM910A482BFD68F57D08A000128A023030
[2023-03-30 18:51:46-751] [3] [] [SockAdapter] [AnalysisPacket] [NORMAL] Multi frame arrived, m_LenAdjust =0
[2023-03-30 18:51:46-752] [3] [] [TcpComm] [OnReceivedSockEvent] [NORMAL] Socket event arrived: DataReceived";

      public const string SAMPLE_MULTILINE_2 =
         @"[2023-03-30 18:51:46-812][3][][TransactionReplyParser][CleanPrinterData    ][NORMAL]Parameter pDeviceName:SPR
[2023-03-30 18:51:46-813][3][][TransactionReplyParser][ParseTranReplyNDC   ][NORMAL]Receipt Cleaned Data=LOCATION: 2929 3RD AVE N
          ESCANABA                   MI
A0000000041010
MASTERCARD

 CARD NO: XXXXXXXXXXXX1538

     DATE        TIME        TERMINAL
   03/30/23      06:52PM      CKI561


SEQ NBR:  5797        AMT:       $40.00
               ATM OWNER FEE      $2.00
               TOTAL             $42.00
CREDIT CASH ADVANCE

               THANK YOU

[2023-03-30 18:51:46-813][3][][TransactionReplyParser][CleanPrinterData    ][NORMAL]Parameter pData      : CARD NO: 546316XXXXXX1538";

      public const string SAMPLE_MULTILINE_3 =
@"[2023-03-30 18:53:05-454][3][][TransactionReplyParser][CleanPrinterData    ][NORMAL]Parameter pDeviceName:SPR
[2023-03-30 18:53:05-455][3][][TransactionReplyParser][ParseTranReplyNDC   ][NORMAL]Receipt Cleaned Data=
[2023-03-30 18:53:05-456][3][][TransactionReplyParser][CleanPrinterData    ][NORMAL]Parameter pData      : CARD NO: 511314XXXXXX7731
000026836020

LOCATION: 2929 3RD AVE N
:ESCANABA?4MI
A0000000042203
US DEBIT

 CARD NO: XXXXXXXXXXXX7731

5DATE8TIME8TERMINAL
303/30/23606:53PM6CKI561


SEQ NBR:  5798
CHECKING BALANCE5XXXXXXXX6020

[2023-03-30 18:53:05-457][3][][TransactionReplyParser][CleanPrinterData    ][NORMAL]Parameter pDeviceName:JPR
[2023-03-30 18:53:05-479][3][][LocalXmlHelper      ][LoadConfigurationFiles][NORMAL]Finished loading xml: C:\Hyosung\MoniPlus2S\ConfigApplication\Application\BaseTransactionReplyConfig.xml
[2023-03-30 18:53:05-484][3][][LocalXmlHelper      ][LoadConfigurationFiles][NORMAL]Finished loading xml: C:\Hyosung\MoniPlus2S\ConfigNetwork\Application\TransactionReplyConfig.xml
[2023-03-30 18:53:05-563][3][][ManagementFramework ][SendMessage         ][NORMAL]Parameter pRecordId:1011";

      public const string SAMPLE_MULTILINE_4 =
         @"[2023-03-30 14:29:16-594][3][][LyncVideoControl    ][OnAudioChannel_ActionAvailabilityChanged][NORMAL][Event] OnAudioChannel_ActionAvailabilityChanged, Action:Stop => IsAvailable:False
[2023-03-30 14:29:16-594][3][][ManagementFramework ][SendMessage         ][NORMAL]Parameter pData    :JG B    
[2023-03-30 14:29:16-595][3][][MessageFramework    ][MakeTransactionRequestMessage][NORMAL]Parameter pCondition:Track1:True
Track2:True
Track3:True
PinBuffer:True
OPCode:True
Amount:True
BufferB:True
BufferC:True
Optional Data Field U:False
Optional Data Field V:False
Optional Data Field Z:False
SendCashAcceptor:False
OptionalSendCountsOfGoodAndSuspect:False
OptionalSendCountsOfSuspect:False
OptionalSendCountsOfCounterfeit:False
SendChequeProcessor:False
SendBunchChequeDeposit:False

[2023-03-30 14:29:16-595][3][][MessageFramework    ][MakeMessage         ][NORMAL]Parameter pCmd:TRANSACT_REQUEST
[2023-03-30 14:29:16-596][3][][MessageFramework    ][MakeMessage         ][NORMAL]Parameter pExt:NONE
[2023-03-30 14:29:16-596][3][][MessageFramework    ][NeedMacData         ][NORMAL]Parameter pCmd:TRANSACT_REQUEST";
   }
}
