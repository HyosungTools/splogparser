namespace APSamples
{
   public class samples_exception
   {
      public const string EXCEPTION_1 = 
@"[2024-01-16 17:01:48-430][3][][Sensors             ][OnProximityChanged  ][NORMAL]m_NxSensors.OnProximityChanged event received(PRESENT)
[2024-01-16 17:01:50-485][1][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO ejournal(MachineNO,Code,EvtDescription) VALUES ('NM000560','2','[Sensors]ProximityChanged -> PRESENT')
[2024-01-16 17:01:50-490][1][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]
?????????????????????????????????????????????
THREAD       NAME:
EXCEPTION    INFO : Fail to execute query
EXCEPTION MESSAGE : Could not update; currently locked by user 'admin' on machine 'NM000560'.
STACKTRACE        :    at System.Data.OleDb.OleDbCommand.ExecuteCommandTextErrorHandling(OleDbHResult hr)
   at System.Data.OleDb.OleDbCommand.ExecuteCommandTextForSingleResult(tagDBPARAMS dbParams, Object& executeResult)
   at System.Data.OleDb.OleDbCommand.ExecuteCommandText(Object& executeResult)
   at System.Data.OleDb.OleDbCommand.ExecuteCommand(CommandBehavior behavior, Object& executeResult)
   at System.Data.OleDb.OleDbCommand.ExecuteReaderInternal(CommandBehavior behavior, String method)
   at System.Data.OleDb.OleDbCommand.ExecuteNonQuery()
   at MoniPlus2.FW.Journal.JournalHelper.ExecuteQuery(String pQuery, String pFileDirectory, String pMDBFileName)
?????????????????????????????????????????????
?????????????????????????????????????????????

[2024-01-16 17:01:50-522][3][][Sensors             ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(ProximityChanged, PROXIMITY_PRESENT)
[2024-01-16 17:01:50-523][3][][SMIFramework        ][Device_UnSolEvent   ][NORMAL]Update operate window:Sensors
[2024-01-16 17:01:50-523][3][][Main                ][UpdateDeviceStatus  ][NORMAL]parameter pDevice:SNS
[2024-01-16 17:01:50-523][3][][ATMStatusMonitor    ][GetDeviceStatusFullCode][NORMAL]Parameter pShortName:SNS
[2024-01-16 17:01:50-524][3][][Main                ][UpdateDeviceStatus  ][NORMAL]parameter e.EventName :ProximityChanged
[2024-01-16 17:01:50-524][3][][Main                ][UpdateDeviceStatus  ][NORMAL]parameter e.EventParam:PROXIMITY_PRESENT
[2024-01-16 17:01:50-525][3][][Main                ][UpdateDeviceStatus  ][NORMAL]parameter e.EventExtra:
[2024-01-16 17:01:52-356][3][][Sensors             ][OnProximityChanged  ][NORMAL]m_NxSensors.OnProximityChanged event received(NOTPRESENT)
[2024-01-16 17:01:54-403][1][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]INSERT INTO ejournal(MachineNO,Code,EvtDescription) VALUES ('NM000560','2','[Sensors]ProximityChanged -> NOTPRESENT')
[2024-01-16 17:01:54-409][1][][MDBJournalWriter    ][ExecuteQuery        ][NORMAL]
?????????????????????????????????????????????
THREAD       NAME:
EXCEPTION    INFO : Fail to execute query
EXCEPTION MESSAGE : Could not update; currently locked by user 'admin' on machine 'NM000560'.
STACKTRACE        :    at System.Data.OleDb.OleDbCommand.ExecuteCommandTextErrorHandling(OleDbHResult hr)
   at System.Data.OleDb.OleDbCommand.ExecuteCommandTextForSingleResult(tagDBPARAMS dbParams, Object& executeResult)
   at System.Data.OleDb.OleDbCommand.ExecuteCommandText(Object& executeResult)
   at System.Data.OleDb.OleDbCommand.ExecuteCommand(CommandBehavior behavior, Object& executeResult)
   at System.Data.OleDb.OleDbCommand.ExecuteReaderInternal(CommandBehavior behavior, String method)
   at System.Data.OleDb.OleDbCommand.ExecuteNonQuery()
   at MoniPlus2.FW.Journal.JournalHelper.ExecuteQuery(String pQuery, String pFileDirectory, String pMDBFileName)
?????????????????????????????????????????????
?????????????????????????????????????????????

[2024-01-16 17:01:54-442][3][][Sensors             ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(ProximityChanged, PROXIMITY_NOTPRESENT)
[2024-01-16 17:01:54-443][3][][SMIFramework        ][Device_UnSolEvent   ][NORMAL]Update operate window:Sensors
[2024-01-16 17:01:54-443][3][][Main                ][UpdateDeviceStatus  ][NORMAL]parameter pDevice:SNS";

      public const string EXCEPTION_2 =
@"[2024-01-16 03:00:25-971][3][][Signpad             ][Close               ][NORMAL]SignpadClose called
[2024-01-16 03:00:25-971][3][][CoinAcceptor        ][Close               ][NORMAL]CoinAcceptorClose called
[2024-01-16 03:00:25-977][3][][SockAdapter         ][RecvProcAsync       ][NORMAL]Socket Exception
[2024-01-16 03:00:25-978][1][][SockAdapter         ][RecvProcAsync       ][NORMAL]
?????????????????????????????????????????????
THREAD       NAME:
EXCEPTION    INFO : IP : 127.0.0.1
:10054
EXCEPTION MESSAGE : An existing connection was forcibly closed by the remote host
STACKTRACE        :    at System.Net.Sockets.Socket.EndReceive(IAsyncResult asyncResult)
   at MoniPlus2.FW.Protocol.Tcpip.SocketAdapter.RecvProcAsync(IAsyncResult ar)
?????????????????????????????????????????????
?????????????????????????????????????????????

[2024-01-16 03:00:25-979][3][][SockAdapter         ][Close               ][NORMAL]Close socket start
[2024-01-16 03:00:25-979][3][][SockAdapter         ][Close               ][NORMAL]m_Socket is not null
[2024-01-16 03:00:25-979][3][][SockAdapter         ][Close               ][NORMAL]Close socket end";

      public const string EXCEPTION_3 =
   @"[2024-01-16 14:07:22-393][3][][SymXchangeWebServiceRequestFlowPoint][GetEligibleTracking ][NORMAL]Excluded: PaymentTo,      Account: XXXXXX9420, Type: 64, Reason: Failed Transaction Tracking Type requirement
[2024-01-16 14:07:22-393][3][][SymXchangeWebServiceRequestFlowPoint][CreateTransactionListsProcessPreferenceAccess][NORMAL]About to call GetAccountRecord(XXXXXX9424)
[2024-01-16 14:07:22-427][1][][SymXchangeWebServiceRequestFlowPoint][CreateTransactionListsProcessPreferenceAccess][NORMAL]
?????????????????????????????????????????????
THREAD       NAME:StateThread
EXCEPTION    INFO : CreateTransactionListsProcessPreferenceAccess() Exception: 
EXCEPTION MESSAGE : GetAccountRecord() Exception: GetAccountSelectFields() Exception: [MessageId=getAccountSelectFields]  The requested record was not found : The requested record was not found
Exception of type 'NH.CoreProcessor.NHException' was thrown.
Exception of type 'NH.CoreProcessor.NHException' was thrown.
STACKTRACE        :    at NH.CoreProcessor.SymXchange.SymXchange.GetAccountRecord(String pAccountNumber)
   at MoniPlus2.FW.State.FlowPoints.SymXchangeWebServiceRequestFlowPoint.CreateTransactionListsProcessPreferenceAccess(SymXchange pSymXchangeWebService, String pAccountNumber, String pSSN, TransactionListConfiguration pConfig, AccountRecord pAccountRecord, Dictionary`2 pTransactionAccountLists)
?????????????????????????????????????????????
?????????????????????????????????????????????

[2024-01-16 14:07:22-428][3][][SymXchangeWebServiceRequestFlowPoint][GetAccountWarnings  ][NORMAL]Start GetAccountWarnings
[2024-01-16 14:07:22-428][3][][SymXchangeWebServiceRequestFlowPoint][GetAccountNumbersForAccountWarnings][NORMAL]Account Numbers: XXXXXX9420
[2024-01-16 14:07:22-429][3][][SymXchangeWebServiceRequestFlowPoint][GetAccountWarnings  ][NORMAL]Final List of relevant Warning Codes: 108, 84, 117
";
      public const string EXCEPTION_4 =
@"[2024-01-16 11:14:44-648][3][][SymXchangeWebServiceRequestFlowPoint][CreateTransactionListsProcessPreferenceAccess][NORMAL]About to call GetAccountRecord(XXXXXX7436)
[2024-01-16 11:14:44-694][1][][SymXchangeWebServiceRequestFlowPoint][CreateTransactionListsProcessPreferenceAccess][NORMAL]
?????????????????????????????????????????????
THREAD       NAME:StateThread
EXCEPTION    INFO : CreateTransactionListsProcessPreferenceAccess() Exception: 
EXCEPTION MESSAGE : GetAccountRecord() Exception: GetAccountSelectFields() Exception: [MessageId=getAccountSelectFields]  The requested record was not found : The requested record was not found
Exception of type 'NH.CoreProcessor.NHException' was thrown.
Exception of type 'NH.CoreProcessor.NHException' was thrown.
STACKTRACE        :    at NH.CoreProcessor.SymXchange.SymXchange.GetAccountRecord(String pAccountNumber)
   at MoniPlus2.FW.State.FlowPoints.SymXchangeWebServiceRequestFlowPoint.CreateTransactionListsProcessPreferenceAccess(SymXchange pSymXchangeWebService, String pAccountNumber, String pSSN, TransactionListConfiguration pConfig, AccountRecord pAccountRecord, Dictionary`2 pTransactionAccountLists)
?????????????????????????????????????????????
?????????????????????????????????????????????

[2024-01-16 11:14:44-694][3][][SymXchangeWebServiceRequestFlowPoint][CreateTransactionListsProcessPreferenceAccess][NORMAL]About to call GetAccountRecord(XXXXXX7437)
[2024-01-16 11:14:44-715][1][][SymXchangeWebServiceRequestFlowPoint][CreateTransactionListsProcessPreferenceAccess][NORMAL]
?????????????????????????????????????????????
THREAD       NAME:StateThread
EXCEPTION    INFO : CreateTransactionListsProcessPreferenceAccess() Exception: 
EXCEPTION MESSAGE : GetAccountRecord() Exception: GetAccountSelectFields() Exception: [MessageId=getAccountSelectFields]  The requested record was not found : The requested record was not found
Exception of type 'NH.CoreProcessor.NHException' was thrown.
Exception of type 'NH.CoreProcessor.NHException' was thrown.
STACKTRACE        :    at NH.CoreProcessor.SymXchange.SymXchange.GetAccountRecord(String pAccountNumber)
   at MoniPlus2.FW.State.FlowPoints.SymXchangeWebServiceRequestFlowPoint.CreateTransactionListsProcessPreferenceAccess(SymXchange pSymXchangeWebService, String pAccountNumber, String pSSN, TransactionListConfiguration pConfig, AccountRecord pAccountRecord, Dictionary`2 pTransactionAccountLists)
?????????????????????????????????????????????
?????????????????????????????????????????????

[2024-01-16 11:14:44-716][3][][SymXchangeWebServiceRequestFlowPoint][GetAccountWarnings  ][NORMAL]Start GetAccountWarnings
[2024-01-16 11:14:44-716][3][][SymXchangeWebServiceRequestFlowPoint][GetAccountNumbersForAccountWarnings][NORMAL]Account Numbers: XXXXXX7670
[2024-01-16 11:14:44-717][3][][SymXchangeWebServiceRequestFlowPoint][GetAccountWarnings  ][NORMAL]Final List of relevant Warning Codes: 3, 108, 117, 20, 20, 20, 20, 20, 20
[2024-01-16 11:14:44-717][3][][SymXchangeWebServiceRequestFlowPoint][GetCardWarnings     ][NORMAL]Start GetAccountWarnings";
   }
}
