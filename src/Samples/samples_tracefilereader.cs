namespace Samples
{
   public static class samples_tracefilereader
   {
      public const string NOTE_TABLE_1 = @"29794294967295016900087405350003CDM0009FRAMEWORK00102023/01/24001200:59 14.7670011INFORMATION0021CBaseService::GetInfo0065Srvc=1303 ReqID=17636 Wnd=0x000201A0 Cmd=1303 TimeOut=300000 IO=201694294967295015800087405360012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.7670007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295021600087405370007MONIMNG0008XFSAGENT00102023/01/24001200:59 14.7680004INFO0026Agent::ProcessNotification0111Recv WFS_GETINFO_COMPLETE Event(ManagedName=[CashAcceptor] hService=[27] dwCommandCode=[1303] dwEventID=[1303])02164294967295017100087405380012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.7680007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x01564BEC]01714294967295022500087405390006COMMON0009FRAMEWORK00102023/01/24001200:59 14.7680011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[12], HWND[0x000201a0], REQUESTID[17637], dwCmdCode[1303], dwTimeOut[300000]}02254294967295016600087405400006COMMON0009FRAMEWORK00102023/01/24001200:59 14.7680011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[12], LogicalName[CashAcceptor]}01664294967295297900087405410003CIM0003SPI00102023/01/24001200:59 14.7680009XFS_EVENT0013GETINFO[1303]2891WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [17636],
	hService = [12],
	tsTimestamp = [2023/01/24 00:59 14.768],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x35a9e954]
	{
		usCount=7
		lppCashIn->
		{
			usNumber		1		2		3		4		5		6		7
			fwType			4		5		1		1		1		1		2
			fwItemType		0x0001		0x0001		0x0004		0x0004		0x0004		0x0004		0x0003
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06
			cCurrencyID		   		   		USD		USD		USD		USD		   
			ulValues		0		0		1		5		20		50		0
			ulCashInCount		0		0		37		16		352		182		149
			ulCount			0		0		1994		1797		2158		1336		149
			ulMaximum		80		210		0		0		0		0		1400
			usStatus		4		4		2		2		1		0		0
			bAppLock		0		0		0		0		0		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		1		1		1		1		1		
			lpPhysicalPositionName	RetractCassette	RejectCassette	CassetteA	CassetteB	CassetteC	CassetteD	CassetteE	
			cUnitID			RTCST		RJCST		CST_A		CST_B		CST_C		CST_D		CST_E
			ulCashInCount		0		0		37		16		352		182		149
			ulCount			0		0		1994		1797		2158		1336		149
			ulMaximum		80		210		0		0		0		0		1400
			usPStatus		4		4		2		2		1		0		0
			bHardwareSensor		1		1		1		1		1		1		1
			ulInitialCount		0		0		2000		2000		2000		2000		0
			ulDispensedCount	0		0		43		219		194		846		0		
			ulPresentedCount	0		0		43		219		194		846		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		0		0		0		0		0		0
			lpszExtra(PCU)		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=CGJX501482]		[SerialNumber=CGJX501516]		[SerialNumber=CGJX501608]		[SerialNumber=CGJX469344]		[SerialNumber=CJOG203649]

			lpNoteNumberList->
			[0]			[1]0		[1]0		[1]1994		[3]1797		[5]2158		[6]1336		[1]0		
			[1]			[2]0		[2]0				[8]0		[10]0		[11]0		[2]0		
			[2]			[3]0		[3]0				[13]0		[15]0		[16]0		[3]0		
			[3]			[4]0		[4]0										[4]0		
			[4]			[5]0		[5]0										[5]0		
			[5]			[6]0		[6]0										[6]0		
			[6]			[7]0		[7]0										[7]0		
			[7]			[8]0		[8]0										[8]0		
			[8]			[9]0		[9]0										[9]0		
			[9]			[10]0		[10]0										[10]0		
			[10]			[11]0		[11]0										[11]0		
			[11]			[12]0		[12]0										[12]0		
			[12]			[13]0		[13]0										[13]10		
			[13]			[14]0		[14]0										[14]11		
			[14]			[15]0		[15]0										[15]20		
			[15]			[16]0		[16]0										[16]0		
			[16]			[17]0		[17]0										[17]108		
			[17]			[0]0		[0]0										[0]0		

			LCU ETC
			usCDMType		0		0		0		0		0		0		0
			lpszCashUnitName	LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06		
			ulInitialCount		0		0		2000		2000		2000		2000		0
			ulDispensedCount	0		0		43		219		194		846		0		
			ulPresentedCount	0		0		43		219		194		846		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		0		0		0		0		0		0
			ulMinimum		0		0		0		0		0		0		0
			lpszExtra(LCU)		NULL		NULL		NULL		NULL		NULL		NULL		NULL
		}
	}
}";
      public const string NOTE_TABLE_2 = @"02454294967295022500090983480006COMMON0009FRAMEWORK00102023/01/24001223:57 34.3690011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[12], HWND[0x000201e0], REQUESTID[10812], dwCmdCode[1303], dwTimeOut[300000]}02254294967295019200090983490003CIM0007ACTIVEX00102023/01/24001223:57 34.3700006XFSAPI0017CService::GetInfo0099WFSAsyncGetInfo(hService=12, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=10812) hResult=001924294967295016100090983500003BRM0002SP00102023/01/24001223:57 34.3700011INFORMATION0037CBcuCommand::CBcuReadImageThread::Run0048[IMAGE_MODE_REJECT] Read thread is completed!(1)01614294967295016600090983510006COMMON0009FRAMEWORK00102023/01/24001223:57 34.3700011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[12], LogicalName[CashAcceptor]}01664294967295016900090983520003CDM0009FRAMEWORK00102023/01/24001223:57 34.3710011INFORMATION0021CBaseService::GetInfo0065Srvc=1303 ReqID=10812 Wnd=0x000201E0 Cmd=1303 TimeOut=300000 IO=201694294967295015800090983530012CashAcceptor0009FRAMEWORK00102023/01/24001223:57 34.3710007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295017100090983540012CashAcceptor0009FRAMEWORK00102023/01/24001223:57 34.3720007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x1F7E65BC]01714294967295291300090983550003CIM0003SPI00102023/01/24001223:57 34.3730009XFS_EVENT0013GETINFO[1303]2825WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10812],
	hService = [12],
	tsTimestamp = [2023/01/24 23:57 34.372],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x255bb7d4]
	{
		usCount=7
		lppCashIn->
		{
			usNumber		1		2		3		4		5		6		7
			fwType			4		5		1		1		1		1		2
			fwItemType		0x0001		0x0001		0x0004		0x0004		0x0004		0x0004		0x0003
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06
			cCurrencyID		   		   		USD		USD		USD		USD		   
			ulValues		0		0		1		5		20		50		0
			ulCashInCount		0		1		13		0		17		17		50
			ulCount			0		1		3		0		0		0		50
			ulMaximum		80		210		0		0		0		0		1400
			usStatus		4		0		2		1		0		0		0
			bAppLock		0		0		0		0		0		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		1		1		1		1		1		
			lpPhysicalPositionName	RetractCassette	RejectCassette	CassetteA	CassetteB	CassetteC	CassetteD	CassetteE	
			cUnitID			RTCST		RJCST		CST_A		CST_B		CST_C		CST_D		CST_E
			ulCashInCount		0		1		13		0		17		17		50
			ulCount			0		1		3		0		0		0		50
			ulMaximum		80		210		0		0		0		0		1400
			usPStatus		4		0		2		1		0		0		0
			bHardwareSensor		1		1		1		1		1		1		1
			ulInitialCount		0		0		2000		2000		2000		2000		0
			ulDispensedCount	0		0		10		0		17		17		0		
			ulPresentedCount	0		0		10		0		16		17		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		1		0		0		1		0		0
			lpszExtra(PCU)		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=CGJX501482]		[SerialNumber=CGJX501516]		[SerialNumber=CGJX501608]		[SerialNumber=CGJX469344]		[SerialNumber=CJOG203649]

			lpNoteNumberList->
			[0]			[1]0		[1]0		[1]3		[3]0		[5]0		[6]0		[1]0		
			[1]			[2]0		[2]0				[8]0		[10]0		[11]0		[2]0		
			[2]			[3]0		[3]0				[13]0		[15]0		[16]0		[3]0		
			[3]			[4]0		[4]0										[4]0		
			[4]			[5]0		[5]0										[5]0		
			[5]			[6]0		[6]0										[6]0		
			[6]			[7]0		[7]0										[7]0		
			[7]			[8]0		[8]0										[8]0		
			[8]			[9]0		[9]0										[9]0		
			[9]			[10]0		[10]0										[10]0		
			[10]			[11]0		[11]0										[11]0		
			[11]			[12]0		[12]0										[12]1		
			[12]			[13]0		[13]0										[13]3		
			[13]			[14]0		[14]0										[14]4		
			[14]			[15]0		[15]1										[15]0		
			[15]			[16]0		[16]0										[16]0		
			[16]			[17]0		[17]0										[17]42		
			[17]			[0]0		[0]0										[0]0		

			LCU ETC
			usCDMType		0		0		0		0		0		0		0
			lpszCashUnitName	LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06		
			ulInitialCount		0		0		2000		2000		2000		2000		0
			ulDispensedCount	0		0		10		0		17		17		0		
			ulPresentedCount	0		0		10		0		16		17		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		1		0		0		1		0		0
			ulMinimum		0		0		0		0		0		0		0
			lpszExtra(LCU)		NULL		NULL		NULL		NULL		NULL		NULL		NULL
		}
	}
}";
      public const string NOTE_TABLE_3 = @"05084294967295016600087192950006COMMON0009FRAMEWORK00102023/01/23001222:47 09.6170011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[12], LogicalName[CashAcceptor]}01664294967295015900087192960003CDM0007ACTIVEX00102023/01/23001222:47 09.6170011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x17130848, lParam=0x24BF48BC01594294967295016900087192970003CDM0009FRAMEWORK00102023/01/23001222:47 09.6170011INFORMATION0021CBaseService::GetInfo0065Srvc=1303 ReqID=16858 Wnd=0x000201A0 Cmd=1303 TimeOut=300000 IO=201694294967295017000087192980003CDM0007ACTIVEX00102023/01/23001222:47 09.6170011INFORMATION0032CCdmService::HandlePresentStatus0057GetInfo-Result[PresentStatus][ReqID=16854] = {hResult[0]}01704294967295015800087192990012CashAcceptor0009FRAMEWORK00102023/01/23001222:47 09.6180007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295016300087193000003CDM0007ACTIVEX00102023/01/23001222:47 09.6180011INFORMATION0024CContextMgr::SetComplete0058SetComplete(Waiting ReqID=[16855], Arrival ReqID=[16854]) 01634294967295019300087193010003CDM0007ACTIVEX00102023/01/23001222:47 09.6180011INFORMATION0024CContextMgr::SetComplete0088SetComplete(Waiting ReqID=[16854], Arrival ReqID=[16854]), Remove ReqID[16854] from List01934294967295017100087193020012CashAcceptor0009FRAMEWORK00102023/01/23001222:47 09.6180007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x0156487C]01714294967295022300087193030006COMMON0009FRAMEWORK00102023/01/23001222:47 09.6180011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[2], HWND[0x0002019c], REQUESTID[16855], dwCmdCode[301], dwTimeOut[300000]}02234294967295016600087193040006COMMON0009FRAMEWORK00102023/01/23001222:47 09.6190011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[2], LogicalName[CashDispenser]}01664294967295297900087193050003CIM0003SPI00102023/01/23001222:47 09.6190009XFS_EVENT0013GETINFO[1303]2891WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [16858],
	hService = [12],
	tsTimestamp = [2023/01/23 22:47 09.618],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x35aa0dbc]
	{
		usCount=7
		lppCashIn->
		{
			usNumber		1		2		3		4		5		6		7
			fwType			4		5		1		1		1		1		2
			fwItemType		0x0001		0x0001		0x0004		0x0004		0x0004		0x0004		0x0003
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06
			cCurrencyID		   		   		USD		USD		USD		USD		   
			ulValues		0		0		1		5		20		50		0
			ulCashInCount		0		0		37		16		352		182		149
			ulCount			0		0		1994		1803		2163		1340		149
			ulMaximum		80		210		0		0		0		0		1400
			usStatus		4		4		2		2		1		0		0
			bAppLock		0		0		0		0		0		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		1		1		1		1		1		
			lpPhysicalPositionName	RetractCassette	RejectCassette	CassetteA	CassetteB	CassetteC	CassetteD	CassetteE	
			cUnitID			RTCST		RJCST		CST_A		CST_B		CST_C		CST_D		CST_E
			ulCashInCount		0		0		37		16		352		182		149
			ulCount			0		0		1994		1803		2163		1340		149
			ulMaximum		80		210		0		0		0		0		1400
			usPStatus		4		4		2		2		1		0		0
			bHardwareSensor		1		1		1		1		1		1		1
			ulInitialCount		0		0		2000		2000		2000		2000		0
			ulDispensedCount	0		0		43		213		189		842		0		
			ulPresentedCount	0		0		43		209		186		842		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		0		0		0		0		0		0
			lpszExtra(PCU)		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=CGJX501482]		[SerialNumber=CGJX501516]		[SerialNumber=CGJX501608]		[SerialNumber=CGJX469344]		[SerialNumber=CJOG203649]

			lpNoteNumberList->
			[0]			[1]0		[1]0		[1]1994		[3]1803		[5]2163		[6]1340		[1]0		
			[1]			[2]0		[2]0				[8]0		[10]0		[11]0		[2]0		
			[2]			[3]0		[3]0				[13]0		[15]0		[16]0		[3]0		
			[3]			[4]0		[4]0										[4]0		
			[4]			[5]0		[5]0										[5]0		
			[5]			[6]0		[6]0										[6]0		
			[6]			[7]0		[7]0										[7]0		
			[7]			[8]0		[8]0										[8]0		
			[8]			[9]0		[9]0										[9]0		
			[9]			[10]0		[10]0										[10]0		
			[10]			[11]0		[11]0										[11]0		
			[11]			[12]0		[12]0										[12]0		
			[12]			[13]0		[13]0										[13]10		
			[13]			[14]0		[14]0										[14]11		
			[14]			[15]0		[15]0										[15]20		
			[15]			[16]0		[16]0										[16]0		
			[16]			[17]0		[17]0										[17]108		
			[17]			[0]0		[0]0										[0]0		

			LCU ETC
			usCDMType		0		0		0		0		0		0		0
			lpszCashUnitName	LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06		
			ulInitialCount		0		0		2000		2000		2000		2000		0
			ulDispensedCount	0		0		43		213		189		842		0		
			ulPresentedCount	0		0		43		209		186		842		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		0		0		0		0		0		0
			ulMinimum		0		0		0		0		0		0		0
			lpszExtra(LCU)		NULL		NULL		NULL		NULL		NULL		NULL		NULL
		}
	}
}";
      public const string NOTE_TABLE_4 = @"25754294967295016912626238010012CashAcceptor0009FRAMEWORK00102023/01/03001213:46 07.6220007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1303] HRESULT[0] OUT BUFFER[0x00717BEC]01694294967295012512626238020007nh CCIM0002SP00102023/01/03001213:46 07.6230006NORMAL0016CNHUsb2::Encrypt0034NHDevCrypto_sendMessageEx Return=101254294967295146612626238030003CIM0003SPI00102023/01/03001213:46 07.6240009XFS_EVENT0013EXECUTE[1303]1378WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010334],
	RequestID = [19298],
	hService = [11],
	tsTimestamp = [2023/01/03 13:46 07.624],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x1c5eb054]
	{
		usCount=1
		lppCashIn->
		{
			usNumber		2
			fwType			2
			fwItemType		0x0001
			cUnitID			LCU01
			cCurrencyID		USD
			ulValues		0
			ulCashInCount		1
			ulCount			1
			ulMaximum		0
			usStatus		0
			bAppLock		0

			lppPhysical->
			usNumPhysicalCUs	1		
			lpPhysicalPositionName	CASHCASSETTE	
			cUnitID			PCU02
			ulCashInCount		1
			ulCount			1
			ulMaximum		0
			usPStatus		0
			bHardwareSensor		1
			ulInitialCount		0
			ulDispensedCount	0		
			ulPresentedCount	0		
			ulRetractedCount	0		
			ulRejectCount		0
			lpszExtra(PCU)		NULL

			lpNoteNumberList->
			[0]			[1]0		
			[1]			[2]0		
			[2]			[3]0		
			[3]			[4]0		
			[4]			[5]0		
			[5]			[6]0		
			[6]			[7]0		
			[7]			[8]0		
			[8]			[9]0		
			[9]			[10]0		
			[10]			[11]0		
			[11]			[12]0		
			[12]			[13]0		
			[13]			[14]0		
			[14]			[15]0		
			[15]			[16]1		
			[16]			[17]0		
			[17]			[0]0		

			LCU ETC
			usCDMType		0
			lpszCashUnitName	CASHCASSETTE	
			ulInitialCount		0
			ulDispensedCount	0		
			ulPresentedCount	0		
			ulRetractedCount	0		
			ulRejectCount		0
			ulMinimum		0
			lpszExtra(LCU)		NULL
		}
	}
}";
      public const string NOTE_TABLE_5 = @"17394294967295017100183290540003IPM0007ACTIVEX00102022/12/14001213:35 43.7280005EVENT0031CIPMService::StatusChangedEvent0065FireTransportMediaStatusChanged{Position[INPUT], NewValue[EMPTY]}01714294967295016900183290550003IPM0009FRAMEWORK00102022/12/14001213:35 43.7280011INFORMATION0021CBaseService::GetInfo0065Srvc=1303 ReqID=10162 Wnd=0x0001005C Cmd=1303 TimeOut=300000 IO=301694294967295015900183290560003IPM0007ACTIVEX00102022/12/14001213:35 43.7280011INFORMATION0030CIpmService::HandleStatus101540048OutputPosition.wTransportMediaStatus{ 1 ==> 0 }.01594294967295015800183290570012CashAcceptor0009FRAMEWORK00102022/12/14001213:35 43.7280007DEVCALL0021CBaseService::GetInfo0049HSERVICE[18] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295017200183290580003IPM0007ACTIVEX00102022/12/14001213:35 43.7280005EVENT0031CIPMService::StatusChangedEvent0066FireTransportMediaStatusChanged{Position[OUTPUT], NewValue[EMPTY]}01724294967295015400183290590003IPM0007ACTIVEX00102022/12/14001213:35 43.7280011INFORMATION0030CIpmService::HandleStatus101540043RefusedPosition.wPositionStatus{ 0 ==> 1 }.01544294967295017100183290600012CashAcceptor0009FRAMEWORK00102022/12/14001213:35 43.7280007DEVRETN0021CBaseService::GetInfo0062HSERVICE[18] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x007B619C]01714294967295017000183290610003IPM0007ACTIVEX00102022/12/14001213:35 43.7280005EVENT0031CIPMService::StatusChangedEvent0064FirePositionStatusChanged{Position[REFUSED], NewValue[NOTEMPTY]}01704294967295016000183290620003IPM0007ACTIVEX00102022/12/14001213:35 43.7290011INFORMATION0030CIpmService::HandleStatus101540049RefusedPosition.wTransportMediaStatus{ 1 ==> 0 }.01604294967295173900183290630003CIM0003SPI00102022/12/14001213:35 43.7290009XFS_EVENT0013GETINFO[1303]1651WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x0001005c],
	RequestID = [10162],
	hService = [18],
	tsTimestamp = [2022/12/14 13:35 43.729],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x0b6509fc]
	{
		usCount=2
		lppCashIn->
		{
			usNumber		1		2
			fwType			4		2
			fwItemType		0x0001		0x0001
			cUnitID			LCU00		LCU01
			cCurrencyID		USD		USD
			ulValues		0		0
			ulCashInCount		0		80
			ulCount			3		80
			ulMaximum		126		0
			usStatus		0		0
			bAppLock		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		
			lpPhysicalPositionName	RETRACTCASSETTE	CASHCASSETTE	
			cUnitID			PCU01		PCU02
			ulCashInCount		0		80
			ulCount			0		80
			ulMaximum		0		0
			usPStatus		0		0
			bHardwareSensor		0		1
			ulInitialCount		0		0
			ulDispensedCount	0		0		
			ulPresentedCount	0		0		
			ulRetractedCount	0		0		
			ulRejectCount		0		0
			lpszExtra(PCU)		NULL		NULL

			lpNoteNumberList->
			[0]			[1]0		[1]0		
			[1]			[2]0		[2]0		
			[2]			[3]0		[3]0		
			[3]			[4]0		[4]0		
			[4]			[5]0		[5]0		
			[5]			[6]0		[6]0		
			[6]			[7]0		[7]0		
			[7]			[8]0		[8]0		
			[8]			[9]0		[9]0		
			[9]			[10]0		[10]0		
			[10]			[11]0		[11]0		
			[11]			[12]0		[12]1		
			[12]			[13]0		[13]5		
			[13]			[14]0		[14]1		
			[14]			[15]0		[15]51		
			[15]			[16]0		[16]13		
			[16]			[17]0		[17]9		
			[17]			[0]0		[0]0		

			LCU ETC
			usCDMType		0		0
			lpszCashUnitName	RETRACTCASSETTE	CASHCASSETTE	
			ulInitialCount		0		0
			ulDispensedCount	0		0		
			ulPresentedCount	0		0		
			ulRetractedCount	0		0		
			ulRejectCount		0		0
			ulMinimum		0		0
			lpszExtra(LCU)		NULL		NULL
		}
	}
}";

      public const string NOTE_LIST_1 = @"02434294967295015800087403460013CashDispenser0009FRAMEWORK00102023/01/24001200:59 14.7340007DEVCALL0021CBaseService::GetInfo0048HSERVICE[17] CATEGORY[303] IN BUFFER[0x00000000]01584294967295013800087403470003CDM0009FRAMEWORK00102023/01/24001200:59 14.7340011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295019600087403480003CDM0007ACTIVEX00102023/01/24001200:59 14.7350006XFSAPI0022CService::AsyncGetInfo0098WFSAsyncGetInfo(hService=17, dwCategory=301, lpQueryDetails=0x00000000, RequestID=17632) hResult=001964294967295015900087403490003CDM0009FRAMEWORK00102023/01/24001200:59 14.7350011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2A8FEA6A, pCursor->pData->hApp=0x0F0726FA01594294967295012600087403500003CIM0003SPI00102023/01/24001200:59 14.7350011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x2A2A7F40)01264294967295017100087403510013CashDispenser0009FRAMEWORK00102023/01/24001200:59 14.7350007DEVRETN0021CBaseService::GetInfo0061HSERVICE[17] CATEGORY[303] HRESULT[0], OUT BUFFER[0x0156496C]01714294967295015900087403520003CDM0007ACTIVEX00102023/01/24001200:59 14.7350011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x080B2C38, lParam=0x0C60B93C01594294967295013700087403530003CDM0009FRAMEWORK00102023/01/24001200:59 14.7350011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295122600087403540003CIM0003SPI00102023/01/24001200:59 14.7350009XFS_EVENT0019SERVICE_EVENT[1304]1132WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2023/01/24 00:59 14.732],
	hResult = [0],
	u.dwEventID = [1304],
	lpBuffer = [0x78763efc]
	{
		usNumber = [4],
		fwType = [1],
		fwItemType = [0x0004],
		cUnitID = [LCU03],
		cCurrencyID = [USD],
		ulValues = [5],
		ulCashInCount = [16],
		ulCount = [1797],
		ulMaximum = [0],
		usStatus = [2],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [3],
			lppNoteNumber = 
			{
				usNoteID = [3],
				ulCount = [1797]
			}
			{
				usNoteID = [8],
				ulCount = [0]
			}
			{
				usNoteID = [13],
				ulCount = [0]
			}
		}
		usNumPhysicalCUs = [1],
		lppPhysical = 
		{
			lpPhysicalPositionName = [CassetteB],
			cUnitID = [CST_B],
			ulCashInCount = [16],
			ulCount = [1797],
			ulMaximum = [0],
			usPStatus = [2],
			bHardwareSensor = [1],
			lpszExtra = [SerialNumber=CGJX501516]
			ulInitialCount = [2000],
			ulDispensedCount = [219],
			ulPresentedCount = [219],
			ulRetractedCount = [0],
			ulRejectCount = [0],
		}
		lpszExtra = NULL
	}
}";
      public const string NOTE_LIST_2 = @"21974294967295017100694866300012CashAcceptor0009FRAMEWORK00102022/12/07001210:39 53.8390007DEVRETN0021CBaseService::GetInfo0062HSERVICE[22] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x1DE028F4]01714294967295418300694866310003CIM0003SPI00102022/12/07001210:39 53.8390009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00030044],
	RequestID = [248947],
	hService = [22],
	tsTimestamp = [2022/12/07 10:39 53.839],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x30fd9c44]
	{
		usCount = [3],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [2],
			ulCount = [1],
			ulMaximum = [80],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [0]
				}
				{
					usNoteID = [2],
					ulCount = [0]
				}
				{
					usNoteID = [3],
					ulCount = [0]
				}
				{
					usNoteID = [4],
					ulCount = [0]
				}
				{
					usNoteID = [5],
					ulCount = [0]
				}
				{
					usNoteID = [6],
					ulCount = [0]
				}
				{
					usNoteID = [7],
					ulCount = [0]
				}
				{
					usNoteID = [8],
					ulCount = [0]
				}
				{
					usNoteID = [9],
					ulCount = [0]
				}
				{
					usNoteID = [10],
					ulCount = [0]
				}
				{
					usNoteID = [11],
					ulCount = [0]
				}
				{
					usNoteID = [12],
					ulCount = [0]
				}
				{
					usNoteID = [13],
					ulCount = [0]
				}
				{
					usNoteID = [14],
					ulCount = [0]
				}
				{
					usNoteID = [15],
					ulCount = [2]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [0]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RetractCassette,
				cUnitID = [RTCST],
				ulCashInCount = [2],
				ulCount = [1],
				ulMaximum = [80],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = [SerialNumber=NULLSERIALNUMBER,DipSW=00000],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usCDMType = [0],
			lpszCashUnitName = [LCU00],
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
		{
			usNumber = [2],
			fwType = [5],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [14],
			ulCount = [14],
			ulMaximum = [210],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [0]
				}
				{
					usNoteID = [2],
					ulCount = [0]
				}
				{
					usNoteID = [3],
					ulCount = [0]
				}
				{
					usNoteID = [4],
					ulCount = [0]
				}
				{
					usNoteID = [5],
					ulCount = [0]
				}
				{
					usNoteID = [6],
					ulCount = [0]
				}
				{
					usNoteID = [7],
					ulCount = [0]
				}
				{
					usNoteID = [8],
					ulCount = [0]
				}
				{
					usNoteID = [9],
					ulCount = [0]
				}
				{
					usNoteID = [10],
					ulCount = [0]
				}
				{
					usNoteID = [11],
					ulCount = [0]
				}
				{
					usNoteID = [12],
					ulCount = [0]
				}
				{
					usNoteID = [13],
					ulCount = [0]
				}
				{
					usNoteID = [14],
					ulCount = [0]
				}
				{
					usNoteID = [15],
					ulCount = [8]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [0]
				}
				{
					usNoteID = [0],
					ulCount = [6]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RejectCassette,
				cUnitID = [RJCST],
				ulCashInCount = [14],
				ulCount = [14],
				ulMaximum = [210],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = [SerialNumber=CMBJ200047,DipSW=00000],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
	
		......(More Data)......
	}
}";
      public const string NOTE_LIST_3 = @"04304294967295013700365582900003CDM0009FRAMEWORK00102022/12/19001218:48 53.0830011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295122900365582910003CIM0003SPI00102022/12/19001218:48 53.0830009XFS_EVENT0019SERVICE_EVENT[1304]1135WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x0001057c],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2022/12/19 18:48 53.080],
	hResult = [0],
	u.dwEventID = [1304],
	lpBuffer = [0x1f6610bc]
	{
		usNumber = [6],
		fwType = [1],
		fwItemType = [0x0004],
		cUnitID = [LCU05],
		cCurrencyID = [USD],
		ulValues = [100],
		ulCashInCount = [32],
		ulCount = [1327],
		ulMaximum = [0],
		usStatus = [0],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [3],
			lppNoteNumber = 
			{
				usNoteID = [7],
				ulCount = [1327]
			}
			{
				usNoteID = [12],
				ulCount = [0]
			}
			{
				usNoteID = [17],
				ulCount = [0]
			}
		}
		usNumPhysicalCUs = [1],
		lppPhysical = 
		{
			lpPhysicalPositionName = [CassetteD],
			cUnitID = [CST_D],
			ulCashInCount = [32],
			ulCount = [1327],
			ulMaximum = [0],
			usPStatus = [0],
			bHardwareSensor = [1],
			lpszExtra = [SerialNumber=CGJX444212]
			ulInitialCount = [1400],
			ulDispensedCount = [105],
			ulPresentedCount = [104],
			ulRetractedCount = [0],
			ulRejectCount = [0],
		}
		lpszExtra = NULL
	}
}";
      public const string NOTE_LIST_4 = @"02444294967295017102715811060012CashAcceptor0009FRAMEWORK00102023/03/09001212:57 16.5390007DEVRETN0021CBaseService::GetInfo0062HSERVICE[37] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x0151AC0C]01714294967295015902715811070003CDM0007ACTIVEX00102023/03/09001212:57 16.5390011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000415, wParam=0x01252B78, lParam=0x07FF8AEC01594294967295019102715811080003CIM0007ACTIVEX00102023/03/09001212:57 16.5390006XFSAPI0017CService::GetInfo0098WFSAsyncGetInfo(hService=49, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=9567) hResult=001914294967295022302715811090006COMMON0009FRAMEWORK00102023/03/09001212:57 16.5400011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[38], HWND[0x000201f8], REQUESTID[9561], dwCmdCode[303], dwTimeOut[300000]}02234294967295012402715811100006COMMON0003SPI00102023/03/09001212:57 16.5400011INFORMATION0010WFPGetInfo0034HSERVICE=39, SrvcVersion=2563(A03)01244294967295418302715811110003CIM0003SPI00102023/03/09001212:57 16.5400009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000105d8],
	RequestID = [9560],
	hService = [37],
	tsTimestamp = [2023/03/09 12:57 16.540],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x07edbb34]
	{
		usCount = [7],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [0],
			ulCount = [0],
			ulMaximum = [400],
			usStatus = [4],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [0]
				}
				{
					usNoteID = [2],
					ulCount = [0]
				}
				{
					usNoteID = [3],
					ulCount = [0]
				}
				{
					usNoteID = [4],
					ulCount = [0]
				}
				{
					usNoteID = [5],
					ulCount = [0]
				}
				{
					usNoteID = [6],
					ulCount = [0]
				}
				{
					usNoteID = [7],
					ulCount = [0]
				}
				{
					usNoteID = [8],
					ulCount = [0]
				}
				{
					usNoteID = [9],
					ulCount = [0]
				}
				{
					usNoteID = [10],
					ulCount = [0]
				}
				{
					usNoteID = [11],
					ulCount = [0]
				}
				{
					usNoteID = [12],
					ulCount = [0]
				}
				{
					usNoteID = [13],
					ulCount = [0]
				}
				{
					usNoteID = [14],
					ulCount = [0]
				}
				{
					usNoteID = [15],
					ulCount = [0]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [0]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RetractCassette,
				cUnitID = [RTCST],
				ulCashInCount = [0],
				ulCount = [0],
				ulMaximum = [400],
				usPStatus = [4],
				bHardwareSensor = [1],
				lpszExtra = [SerilNumber=NULLSERIALNUMBER],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usCDMType = [0],
			lpszCashUnitName = [(null)],
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
		{
			usNumber = [2],
			fwType = [5],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [4],
			ulCount = [4],
			ulMaximum = [0],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [0]
				}
				{
					usNoteID = [2],
					ulCount = [0]
				}
				{
					usNoteID = [3],
					ulCount = [0]
				}
				{
					usNoteID = [4],
					ulCount = [0]
				}
				{
					usNoteID = [5],
					ulCount = [0]
				}
				{
					usNoteID = [6],
					ulCount = [0]
				}
				{
					usNoteID = [7],
					ulCount = [0]
				}
				{
					usNoteID = [8],
					ulCount = [0]
				}
				{
					usNoteID = [9],
					ulCount = [0]
				}
				{
					usNoteID = [10],
					ulCount = [0]
				}
				{
					usNoteID = [11],
					ulCount = [0]
				}
				{
					usNoteID = [12],
					ulCount = [0]
				}
				{
					usNoteID = [13],
					ulCount = [4]
				}
				{
					usNoteID = [14],
					ulCount = [0]
				}
				{
					usNoteID = [15],
					ulCount = [0]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [0]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RejectCassette,
				cUnitID = [RJCST],
				ulCashInCount = [4],
				ulCount = [4],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = [SerilNumber=NULLSERIALNUMBER],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [4],
		
		......(More Data)......
	}
}";
      public const string NOTE_LIST_5 = @"14114294967295015902682728900003CIM0007ACTIVEX00102023/03/07001213:52 25.4620011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x09515E18, lParam=0x07A7E0FC01594294967295016602682728910006COMMON0009FRAMEWORK00102023/03/07001213:52 25.4630011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[11], LogicalName[CashAcceptor]}01664294967295015902682728920003CDM0007ACTIVEX00102023/03/07001213:52 25.4630011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x272097B0, lParam=0x2B493E2C01594294967295015502682728930003CIM0007ACTIVEX00102023/03/07001213:52 25.4630011INFORMATION0031CCimService::HandleCashUnitInfo0043GetInfo-Result[CashUnitInfo] = {hResult[0]}01554294967295016902682728940003CDM0009FRAMEWORK00102023/03/07001213:52 25.4630011INFORMATION0021CBaseService::GetInfo0065Srvc=1303 ReqID=17507 Wnd=0x000201F2 Cmd=1303 TimeOut=300000 IO=201694294967295015602682728950003CDM0007ACTIVEX00102023/03/07001213:52 25.4630011INFORMATION0025CCdmService::HandleStatus0050GetInfo-Result[Status][ReqID=17505] = {hResult[0]}01564294967295015802682728960012CashAcceptor0009FRAMEWORK00102023/03/07001213:52 25.4630007DEVCALL0021CBaseService::GetInfo0049HSERVICE[11] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295015502682728970003CDM0007ACTIVEX00102023/03/07001213:52 25.4640011INFORMATION0025CCdmService::HandleStatus0049StShutter-old, StShutter-new : { CLOSED , OPEN }.01554294967295017102682728980012CashAcceptor0009FRAMEWORK00102023/03/07001213:52 25.4640007DEVRETN0021CBaseService::GetInfo0062HSERVICE[11] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x0158339C]01714294967295015602682728990003CDM0007ACTIVEX00102023/03/07001213:52 25.4640005EVENT0041CCdmService::SetShutterStatusChangedEvent0040FireShutterStatusChanged{NewValue[OPEN]}01564294967295022402682729000006COMMON0009FRAMEWORK00102023/03/07001213:52 25.4640011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[19], HWND[0x000105d6], REQUESTID[17508], dwCmdCode[303], dwTimeOut[300000]}02244294967295016702682729010006COMMON0009FRAMEWORK00102023/03/07001213:52 25.4640011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[19], LogicalName[CashDispenser]}01674294967295418302682729020003CIM0003SPI00102023/03/07001213:52 25.4640009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201f2],
	RequestID = [17507],
	hService = [11],
	tsTimestamp = [2023/03/07 13:52 25.464],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x244bf9b4]
	{
		usCount = [7],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [0],
			ulCount = [0],
			ulMaximum = [400],
			usStatus = [4],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [0]
				}
				{
					usNoteID = [2],
					ulCount = [0]
				}
				{
					usNoteID = [3],
					ulCount = [0]
				}
				{
					usNoteID = [4],
					ulCount = [0]
				}
				{
					usNoteID = [5],
					ulCount = [0]
				}
				{
					usNoteID = [6],
					ulCount = [0]
				}
				{
					usNoteID = [7],
					ulCount = [0]
				}
				{
					usNoteID = [8],
					ulCount = [0]
				}
				{
					usNoteID = [9],
					ulCount = [0]
				}
				{
					usNoteID = [10],
					ulCount = [0]
				}
				{
					usNoteID = [11],
					ulCount = [0]
				}
				{
					usNoteID = [12],
					ulCount = [0]
				}
				{
					usNoteID = [13],
					ulCount = [0]
				}
				{
					usNoteID = [14],
					ulCount = [0]
				}
				{
					usNoteID = [15],
					ulCount = [0]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [0]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RetractCassette,
				cUnitID = [RTCST],
				ulCashInCount = [0],
				ulCount = [0],
				ulMaximum = [400],
				usPStatus = [4],
				bHardwareSensor = [1],
				lpszExtra = [SerilNumber=NULLSERIALNUMBER],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usCDMType = [0],
			lpszCashUnitName = [(null)],
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
		{
			usNumber = [2],
			fwType = [5],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [0],
			ulCount = [0],
			ulMaximum = [0],
			usStatus = [4],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [0]
				}
				{
					usNoteID = [2],
					ulCount = [0]
				}
				{
					usNoteID = [3],
					ulCount = [0]
				}
				{
					usNoteID = [4],
					ulCount = [0]
				}
				{
					usNoteID = [5],
					ulCount = [0]
				}
				{
					usNoteID = [6],
					ulCount = [0]
				}
				{
					usNoteID = [7],
					ulCount = [0]
				}
				{
					usNoteID = [8],
					ulCount = [0]
				}
				{
					usNoteID = [9],
					ulCount = [0]
				}
				{
					usNoteID = [10],
					ulCount = [0]
				}
				{
					usNoteID = [11],
					ulCount = [0]
				}
				{
					usNoteID = [12],
					ulCount = [0]
				}
				{
					usNoteID = [13],
					ulCount = [0]
				}
				{
					usNoteID = [14],
					ulCount = [0]
				}
				{
					usNoteID = [15],
					ulCount = [0]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [0]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RejectCassette,
				cUnitID = [RJCST],
				ulCashInCount = [0],
				ulCount = [0],
				ulMaximum = [0],
				usPStatus = [4],
				bHardwareSensor = [1],
				lpszExtra = [SerilNumber=NULLSERIALNUMBER],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
	
		......(More Data)......
	}
}";

   }
}
