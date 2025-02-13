
namespace Samples
{
   public static class samples_cim
   {
      // INFO
      public const string WFS_INF_CIM_STATUS_1 = @"14124294967295016600087401630006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6640011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[27], LogicalName[CashAcceptor]}01664294967295016800087401640003CDM0009FRAMEWORK00102023/01/24001200:59 14.6640011INFORMATION0021CBaseService::GetInfo0064Srvc=1301 ReqID=17616 Wnd=0x000206A8 Cmd=1301 TimeOut=60000 IO=401684294967295021500087401650007MONIMNG0008XFSAGENT00102023/01/24001200:59 14.6640004INFO0026Agent::ProcessNotification0110Recv WFS_GETINFO_COMPLETE Event(ManagedName=[CashDispenser] hService=[28] dwCommandCode=[301] dwEventID=[301])02154294967295015800087401660012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.6640007DEVCALL0021CBaseService::GetInfo0049HSERVICE[27] CATEGORY[1301] IN BUFFER[0x00000000]01584294967295017100087401670012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.6640007DEVRETN0021CBaseService::GetInfo0062HSERVICE[27] CATEGORY[1301] HRESULT[0], OUT BUFFER[0x014CBF44]01714294967295022500087401680006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6640011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[12], HWND[0x000201a0], REQUESTID[17617], dwCmdCode[1306], dwTimeOut[300000]}02254294967295160900087401690003CIM0003SPI00102023/01/24001200:59 14.6640009XFS_EVENT0013GETINFO[1301]1521WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000206a8],
	RequestID = [17616],
	hService = [27],
	tsTimestamp = [2023/01/24 00:59 14.664],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = [0x00447e44]
	{
		fwDevice = [1],
		fwSafeDoor = [2],
		fwAcceptor = [3],
		fwIntermediateStacker = [4],
		fwStackerItems = [5],
		bDropBox = [0],
		lppPositions =
		{
			fwPosition = [4],
			fwShutter = [0],
			fwPositionStatus = [0],
			fwTransport = [0],
			fwTransportStatus = [0]
		}
		{
			fwPosition = [512],
			fwShutter = [0],
			fwPositionStatus = [0],
			fwTransport = [0],
			fwTransportStatus = [0]
		}
		lpszExtra = [BCMode=REAL-NOTE,BRM20SensorInfo=[1][4][5][0][0][0][0][0][2][2][2][2][2][1][1][0][0][0][0][0][0][0][0][0][0][0][0][0][0][0][0][0][3][1][1][1][1][1][1][1][4][3][3][4][3][3][0][0][3][0][0][3][3][4][0][0][6][6][6][0][0][0][3][0][1][0][0][0][0][0][0][0][...]
		dwGuidLights =
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000]
		wDevicePosition = [0],
		usPowerSaveRecoveryTime = [0]
		wMixedMode = [0]
		wAntiFraudModule = [0]
	}
}";
      public const string WFS_INF_CIM_STATUS_2 = @"14114294967295022400090672850006COMMON0009FRAMEWORK00102023/01/24001221:44 59.5130011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[12], HWND[0x000201e0], REQUESTID[9800], dwCmdCode[1301], dwTimeOut[300000]}02244294967295015900090672860003CDM0007ACTIVEX00102023/01/24001221:44 59.5130011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x00EB3988, lParam=0x05BC60AC01594294967295016600090672870006COMMON0009FRAMEWORK00102023/01/24001221:44 59.5140011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[12], LogicalName[CashAcceptor]}01664294967295015500090672880003CDM0007ACTIVEX00102023/01/24001221:44 59.5140011INFORMATION0025CCdmService::HandleStatus0049GetInfo-Result[Status][ReqID=9796] = {hResult[0]}01554294967295016800090672890003CDM0009FRAMEWORK00102023/01/24001221:44 59.5140011INFORMATION0021CBaseService::GetInfo0064Srvc=1301 ReqID=9800 Wnd=0x000201E0 Cmd=1301 TimeOut=300000 IO=201684294967295019000090672900003CDM0007ACTIVEX00102023/01/24001221:44 59.5140011INFORMATION0024CContextMgr::SetComplete0085SetComplete(Waiting ReqID=[9796], Arrival ReqID=[9796]), Remove ReqID[9796] from List01904294967295015800090672910012CashAcceptor0009FRAMEWORK00102023/01/24001221:44 59.5140007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1301] IN BUFFER[0x00000000]01584294967295013500090672920003CDM0007ACTIVEX00102023/01/24001221:44 59.5140005EVENT0038CCdmService::HandleCashUnitInfoChanged0022FireCashUnitChanged[5]01354294967295017100090672930012CashAcceptor0009FRAMEWORK00102023/01/24001221:44 59.5150007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1301] HRESULT[0], OUT BUFFER[0x1F7E330C]01714294967295160800090672940003CIM0003SPI00102023/01/24001221:44 59.5150009XFS_EVENT0013GETINFO[1301]1520WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [9800],
	hService = [12],
	tsTimestamp = [2023/01/24 21:44 59.515],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = [0x258c811c]
	{
		fwDevice = [0],
		fwSafeDoor = [1],
		fwAcceptor = [1],
		fwIntermediateStacker = [0],
		fwStackerItems = [4],
		bDropBox = [0],
		lppPositions =
		{
			fwPosition = [4],
			fwShutter = [0],
			fwPositionStatus = [0],
			fwTransport = [0],
			fwTransportStatus = [0]
		}
		{
			fwPosition = [512],
			fwShutter = [0],
			fwPositionStatus = [0],
			fwTransport = [0],
			fwTransportStatus = [0]
		}
		lpszExtra = [BCMode=REAL-NOTE,BRM20SensorInfo=[1][4][5][0][0][0][0][0][3][2][2][2][2][1][1][0][0][0][0][0][0][0][0][0][0][0][0][0][0][0][0][0][3][1][1][1][1][1][1][1][4][3][3][4][4][3][0][0][3][0][0][3][2][4][0][0][6][6][6][0][0][0][3][0][1][0][0][0][0][0][0][0][...]
		dwGuidLights =
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000]
		wDevicePosition = [0],
		usPowerSaveRecoveryTime = [0]
		wMixedMode = [0]
		wAntiFraudModule = [0]
	}
}";
      public const string WFS_INF_CIM_STATUS_3 = @"02444294967295022402676474350006COMMON0009FRAMEWORK00102023/03/07001211:00 18.2250011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[11], HWND[0x000201f2], REQUESTID[7500], dwCmdCode[1301], dwTimeOut[300000]}02244294967295019102676474360003CIM0007ACTIVEX00102023/03/07001211:00 18.2250006XFSAPI0017CService::GetInfo0098WFSAsyncGetInfo(hService=11, dwCategory=1301, lpQueryDetails=0x00000000, RequestID=7500) hResult=001914294967295016602676474370006COMMON0009FRAMEWORK00102023/03/07001211:00 18.2250011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[11], LogicalName[CashAcceptor]}01664294967295016802676474380003CDM0009FRAMEWORK00102023/03/07001211:00 18.2260011INFORMATION0021CBaseService::GetInfo0064Srvc=1301 ReqID=7500 Wnd=0x000201F2 Cmd=1301 TimeOut=300000 IO=201684294967295015802676474390012CashAcceptor0009FRAMEWORK00102023/03/07001211:00 18.2260007DEVCALL0021CBaseService::GetInfo0049HSERVICE[11] CATEGORY[1301] IN BUFFER[0x00000000]01584294967295017102676474400012CashAcceptor0009FRAMEWORK00102023/03/07001211:00 18.2260007DEVRETN0021CBaseService::GetInfo0062HSERVICE[11] CATEGORY[1301] HRESULT[0], OUT BUFFER[0x0158149C]01714294967295160802676474410003CIM0003SPI00102023/03/07001211:00 18.2260009XFS_EVENT0013GETINFO[1301]1520WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201f2],
	RequestID = [7500],
	hService = [11],
	tsTimestamp = [2023/03/07 11:00 18.226],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = [0x2493a3c4]
	{
		fwDevice = [0],
		fwSafeDoor = [1],
		fwAcceptor = [0],
		fwIntermediateStacker = [0],
		fwStackerItems = [4],
		bDropBox = [0],
		lppPositions =
		{
			fwPosition = [4],
			fwShutter = [1],
			fwPositionStatus = [0],
			fwTransport = [0],
			fwTransportStatus = [0]
		}
		{
			fwPosition = [512],
			fwShutter = [1],
			fwPositionStatus = [0],
			fwTransport = [0],
			fwTransportStatus = [0]
		}
		lpszExtra = [ItemInfo=LEVEL_2=0
LEVEL_3=0
LEVEL_4=0

,BRM20SensorInfo=0E040203020E02020403050305030202050605050506060602040302020C03030E0E0E0E0E02020202020202020202020204030202030203020202020202020204030E0E0E0E0E0E020203030E0E0E0E06060A06090E0E0E05050604040E0...]
		dwGuidLights =
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000]
		wDevicePosition = [0],
		usPowerSaveRecoveryTime = [0]
		wMixedMode = [0]
		wAntiFraudModule = [0]
	}
}";

      public const string WFS_INF_CIM_CASH_UNIT_INFO_1 = @"02424294967295017100087401350012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.6590007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x1F937974]01714294967295018600087401360007MONIMNG0008XFSAGENT00102023/01/24001200:59 14.6590004INFO0015Agent::FireTrap0092call WFSAsyncGetInfo(ManagedName=[CashDispenser] hService=[28] dwCategory=[301] hResult=[0])01864294967295022400087401370006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6600011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[17], HWND[0x00010638], REQUESTID[17613], dwCmdCode[301], dwTimeOut[300000]}02244294967295012400087401380006COMMON0003SPI00102023/01/24001200:59 14.6600011INFORMATION0010WFPGetInfo0034HSERVICE=27, SrvcVersion=2563(A03)01244294967295016700087401390006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6600011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[17], LogicalName[CashDispenser]}01674294967295297900087401400003CIM0003SPI00102023/01/24001200:59 14.6600009XFS_EVENT0013GETINFO[1303]2891WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [17614],
	hService = [12],
	tsTimestamp = [2023/01/24 00:59 14.660],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x35aa51b4]
	{
		usCount=7
		lppCashIn->
		{
			usNumber		1		2		3		4		5		6		7
			fwType			8		9		11		11		12		13		14
			fwItemType		0x0001		0x0001		0x0004		0x0004		0x0004		0x0004		0x0003
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06
			cCurrencyID		   		   		ABC		DEF		GHI		JKL		   
			ulValues		15		16		17		18		19		20		21
			ulCashInCount		22		23		24		25		26		27		28
			ulCount			29		30		31		32		33		34		35
			ulMaximum		36		37		38		39		40		41		42
			usStatus		43		44		45		46		47		48		49
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
			ulInitialCount		50		51		52		53		54		55		56
			ulDispensedCount	57		58		59		60		61		62		63		
			ulPresentedCount	64		65		66		67		68		69		70		
			ulRetractedCount	71		72		73		74		75		76		77		
			ulRejectCount		78		79		80		81		82		83		84
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
			ulInitialCount		100		101		102		103		104		105		106
			ulDispensedCount	107		108		109		110		111		112		113		
			ulPresentedCount	114		115		116		117		118		119		120		
			ulRetractedCount	121		122		123		124		125		126		127		
			ulRejectCount		128		129		130		131		132		133		134
			ulMinimum		135		136		137		138		139		140		141
			lpszExtra(LCU)		NULL		NULL		NULL		NULL		NULL		NULL		NULL
		}
	}
}";
      public const string WFS_INF_CIM_CASH_UNIT_INFO_2 = @"02454294967295022500164040090006COMMON0009FRAMEWORK00102022/12/07001216:33 16.8110011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[11], HWND[0x00020128], REQUESTID[72824], dwCmdCode[1303], dwTimeOut[300000]}02254294967295012000164040100004CCIM0002SP00102022/12/07001216:33 16.8110011INFORMATION0017CCCIMDev::_Sensor0026BDS07 sensor changed(1->0)01204294967295019200164040110003CIM0007ACTIVEX00102022/12/07001216:33 16.8110006XFSAPI0017CService::GetInfo0099WFSAsyncGetInfo(hService=11, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=72824) hResult=001924294967295016600164040120006COMMON0009FRAMEWORK00102022/12/07001216:33 16.8110011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[11], LogicalName[CashAcceptor]}01664294967295012300164040130004CCIM0002SP00102022/12/07001216:33 16.8110011INFORMATION0017CCCIMDev::_Sensor0029ESC_RESI sensor changed(0->1)01234294967295016900164040140003CIM0009FRAMEWORK00102022/12/07001216:33 16.8110011INFORMATION0021CBaseService::GetInfo0065Srvc=1303 ReqID=72824 Wnd=0x00020128 Cmd=1303 TimeOut=300000 IO=101694294967295015800164040150012CashAcceptor0009FRAMEWORK00102022/12/07001216:33 16.8110007DEVCALL0021CBaseService::GetInfo0049HSERVICE[11] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295012100164040160004CCIM0002SP00102022/12/07001216:33 16.8110011INFORMATION0017CCCIMDev::_Sensor0027BDS_UP sensor changed(0->1)01214294967295017100164040170012CashAcceptor0009FRAMEWORK00102022/12/07001216:33 16.8110007DEVRETN0021CBaseService::GetInfo0062HSERVICE[11] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x006A1924]01714294967295020700164040180004CCIM0002SP00102022/12/07001216:33 16.8110011INFORMATION0017CCCIMDev::_Sensor0113##iCheckLog Start SENSOR_DATA Unit: CCIM, Message: 00282200d0420c002302230200264c004483ff018380f9 iCheckLog End##02074294967295418300164040190003CIM0003SPI00102022/12/07001216:33 16.8110009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00020128],
	RequestID = [72824],
	hService = [11],
	tsTimestamp = [2022/12/07 16:33 16.811],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x2cae943c]
	{
		usCount = [2],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [USD],
			ulValues = [0],
			ulCashInCount = [39],
			ulCount = [4],
			ulMaximum = [126],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [10]
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
					ulCount = [17]
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
					ulCount = [4]
				}
				{
					usNoteID = [17],
					ulCount = [6]
				}
				{
					usNoteID = [0],
					ulCount = [2]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RETRACTCASSETTE,
				cUnitID = [PCU01],
				ulCashInCount = [39],
				ulCount = [39],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [0],
				lpszExtra = NULL,
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usCDMType = [0],
			lpszCashUnitName = [RETRACTCASSETTE],
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
		{
			usNumber = [2],
			fwType = [2],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [USD],
			ulValues = [0],
			ulCashInCount = [4774],
			ulCount = [4771],
			ulMaximum = [0],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [575]
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
					ulCount = [2]
				}
				{
					usNoteID = [8],
					ulCount = [1]
				}
				{
					usNoteID = [9],
					ulCount = [1]
				}
				{
					usNoteID = [10],
					ulCount = [2]
				}
				{
					usNoteID = [11],
					ulCount = [1]
				}
				{
					usNoteID = [12],
					ulCount = [43]
				}
				{
					usNoteID = [13],
					ulCount = [290]
				}
				{
					usNoteID = [14],
					ulCount = [195]
				}
				{
					usNoteID = [15],
					ulCount = [2674]
				}
				{
					usNoteID = [16],
					ulCount = [422]
				}
				{
					usNoteID = [17],
					ulCount = [565]
				}
				{
					usNoteID = [0],
					ulCount = [3]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = CASHCASSETTE,
				cUnitID = [PCU02],
				ulCashInCount = [4774],
				ulCount = [4771],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = NULL,
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtr
		......(More Data)......
	}
}";
      public const string WFS_INF_CIM_CASH_UNIT_INFO_3 = @"02444294967295022401809434980006COMMON0009FRAMEWORK00102023/02/03001211:52 50.2910011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[17], HWND[0x00010058], REQUESTID[9112], dwCmdCode[1303], dwTimeOut[300000]}02244294967295019101809434990003CIM0007ACTIVEX00102023/02/03001211:52 50.2920006XFSAPI0017CService::GetInfo0098WFSAsyncGetInfo(hService=17, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=9112) hResult=001914294967295016601809435000006COMMON0009FRAMEWORK00102023/02/03001211:52 50.2920011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[17], LogicalName[CashAcceptor]}01664294967295015901809435010003CIM0007ACTIVEX00102023/02/03001211:52 50.2920011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x075CA698, lParam=0x0A1737EC01594294967295016801809435020003IPM0009FRAMEWORK00102023/02/03001211:52 50.2920011INFORMATION0021CBaseService::GetInfo0064Srvc=1303 ReqID=9112 Wnd=0x00010058 Cmd=1303 TimeOut=300000 IO=301684294967295015501809435030003CIM0007ACTIVEX00102023/02/03001211:52 50.2920011INFORMATION0031CCimService::HandleCashUnitInfo0043GetInfo-Result[CashUnitInfo] = {hResult[0]}01554294967295015801809435040012CashAcceptor0009FRAMEWORK00102023/02/03001211:52 50.2920007DEVCALL0021CBaseService::GetInfo0049HSERVICE[17] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295017101809435050012CashAcceptor0009FRAMEWORK00102023/02/03001211:52 50.2930007DEVRETN0021CBaseService::GetInfo0062HSERVICE[17] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x008EF5F4]01714294967295418301809435060003CIM0003SPI00102023/02/03001211:52 50.2930009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00010058],
	RequestID = [9112],
	hService = [17],
	tsTimestamp = [2023/02/03 11:52 50.293],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x04bf8a24]
	{
		usCount = [2],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [USD],
			ulValues = [0],
			ulCashInCount = [0],
			ulCount = [8],
			ulMaximum = [75],
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
				lpPhysicalPositionName = RETRACTCASSETTE,
				cUnitID = [PCU01],
				ulCashInCount = [0],
				ulCount = [0],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [0],
				lpszExtra = NULL,
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usCDMType = [0],
			lpszCashUnitName = [RETRACTCASSETTE],
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
		{
			usNumber = [2],
			fwType = [2],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [USD],
			ulValues = [0],
			ulCashInCount = [541],
			ulCount = [541],
			ulMaximum = [0],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [72]
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
					ulCount = [1]
				}
				{
					usNoteID = [12],
					ulCount = [5]
				}
				{
					usNoteID = [13],
					ulCount = [68]
				}
				{
					usNoteID = [14],
					ulCount = [35]
				}
				{
					usNoteID = [15],
					ulCount = [265]
				}
				{
					usNoteID = [16],
					ulCount = [48]
				}
				{
					usNoteID = [17],
					ulCount = [47]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = CASHCASSETTE,
				cUnitID = [PCU02],
				ulCashInCount = [541],
				ulCount = [541],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = NULL,
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usCD
		......(More Data)......
	}
}";

      public const string WFS_INF_CIM_CASH_IN_STATUS_1 = @"02454294967295022500090887650006COMMON0009FRAMEWORK00102023/01/24001223:55 47.4390011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[12], HWND[0x000201e0], REQUESTID[10517], dwCmdCode[1307], dwTimeOut[300000]}02254294967295019200090887660003CIM0007ACTIVEX00102023/01/24001223:55 47.4390006XFSAPI0017CService::GetInfo0099WFSAsyncGetInfo(hService=12, dwCategory=1307, lpQueryDetails=0x00000000, RequestID=10517) hResult=001924294967295016600090887670006COMMON0009FRAMEWORK00102023/01/24001223:55 47.4400011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[12], LogicalName[CashAcceptor]}01664294967295016900090887680003CDM0009FRAMEWORK00102023/01/24001223:55 47.4400011INFORMATION0021CBaseService::GetInfo0065Srvc=1307 ReqID=10517 Wnd=0x000201E0 Cmd=1307 TimeOut=300000 IO=201694294967295015800090887690012CashAcceptor0009FRAMEWORK00102023/01/24001223:55 47.4410007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1307] IN BUFFER[0x00000000]01584294967295017100090887700012CashAcceptor0009FRAMEWORK00102023/01/24001223:55 47.4410007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1307] HRESULT[0], OUT BUFFER[0x1F7E7C44]01714294967295046600090887710003CIM0003SPI00102023/01/24001223:55 47.4410009XFS_EVENT0013GETINFO[1307]0378WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10517],
	hService = [12],
	tsTimestamp = [2023/01/24 23:55 47.441],
	hResult = [0],
	u.dwCommandCode = [1307],
	lpBuffer = [0x0c5d121c]
	{
		wStatus = [2],
		usNumOfRefused = [3],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [0],
			lppNoteNumber = NULL
		}
		lpszExtra = NULL
	}
}";
      public const string WFS_INF_CIM_CASH_IN_STATUS_2 = @"02414294967295022100507509850006COMMON0009FRAMEWORK00102022/11/17001210:26 33.4880011INFORMATION0033CSpCmdDispatcher::DispatchCommand0102pService->GetInfo() {HSERVICE[112], HWND[0x00040790], REQUESTID[59043], dwCmdCode[1307], dwTimeOut[0]}02214294967295016800507509860006COMMON0009FRAMEWORK00102022/11/17001210:26 33.4880011INFORMATION0028CServiceProvider::GetService0054service OK {HSERVICE[112], LogicalName[CashAcceptor1]}01684294967295016400507509870003CIM0009FRAMEWORK00102022/11/17001210:26 33.4880011INFORMATION0021CBaseService::GetInfo0060Srvc=1307 ReqID=59043 Wnd=0x00040790 Cmd=1307 TimeOut=0 IO=701644294967295011100507509880010CCIM-MIXED0002SP00102022/11/17001210:26 33.4880011INFORMATION0009LogThread0019<<< BCLogThread End01114294967295016000507509890013CashAcceptor10009FRAMEWORK00102022/11/17001210:26 33.4880007DEVCALL0021CBaseService::GetInfo0050HSERVICE[112] CATEGORY[1307] IN BUFFER[0x00000000]01604294967295017300507509900013CashAcceptor10009FRAMEWORK00102022/11/17001210:26 33.4880007DEVRETN0021CBaseService::GetInfo0063HSERVICE[112] CATEGORY[1307] HRESULT[0], OUT BUFFER[0x0063DDBC]01734294967295052600507509910013CashAcceptor10003SPI00102022/11/17001210:26 33.4880009XFS_EVENT0013GETINFO[1307]0428WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00040790],
	RequestID = [59043],
	hService = [112],
	tsTimestamp = [2022/11/17 10:26 33.488],
	hResult = [0],
	u.dwCommandCode = [1307],
	lpBuffer = [0x0e67dddc]
	{
		wStatus = [2],
		usNumOfRefused = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [1],
			lppNoteNumber = 
			{
				usNoteID = [15],
				ulCount = [6]
			}
		}
		lpszExtra = NULL
	}
}";
      public const string WFS_INF_CIM_CASH_IN_STATUS_3 = @"02454294967295022508484936160006COMMON0009FRAMEWORK00102022/12/19001218:17 49.1410011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[39], HWND[0x00010344], REQUESTID[18955], dwCmdCode[1307], dwTimeOut[300000]}02254294967295011308484936170006nh BC30002SP00102022/12/19001218:17 49.1430006NORMAL0021CNHUsb3::SendBulkData0018Data Length: 1535101134294967295019208484936180003CIM0007ACTIVEX00102022/12/19001218:17 49.1430006XFSAPI0017CService::GetInfo0099WFSAsyncGetInfo(hService=39, dwCategory=1307, lpQueryDetails=0x00000000, RequestID=18955) hResult=001924294967295016608484936190006COMMON0009FRAMEWORK00102022/12/19001218:17 49.1440011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[39], LogicalName[CashAcceptor]}01664294967295016908484936200003CIM0009FRAMEWORK00102022/12/19001218:17 49.1460011INFORMATION0021CBaseService::GetInfo0065Srvc=1307 ReqID=18955 Wnd=0x00010344 Cmd=1307 TimeOut=300000 IO=301694294967295015808484936210012CashAcceptor0009FRAMEWORK00102022/12/19001218:17 49.1480007DEVCALL0021CBaseService::GetInfo0049HSERVICE[39] CATEGORY[1307] IN BUFFER[0x00000000]01584294967295017108484936220012CashAcceptor0009FRAMEWORK00102022/12/19001218:17 49.1490007DEVRETN0021CBaseService::GetInfo0062HSERVICE[39] CATEGORY[1307] HRESULT[0], OUT BUFFER[0x00878454]01714294967295056808484936230003CIM0003SPI00102022/12/19001218:17 49.1490009XFS_EVENT0013GETINFO[1307]0480WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00010344],
	RequestID = [18955],
	hService = [39],
	tsTimestamp = [2022/12/19 18:17 49.149],
	hResult = [0],
	u.dwCommandCode = [1307],
	lpBuffer = [0x2c898854]
	{
		wStatus = [0],
		usNumOfRefused = [1],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [2],
			lppNoteNumber = 
			{
				usNoteID = [1],
				ulCount = [48]
			}
			{
				usNoteID = [13],
				ulCount = [2]
			}
		}
		lpszExtra = NULL
	}
}";

      // EXECUTE
      public const string WFS_CMD_CIM_CASH_IN_START_1 = @"05024294967295016900090887570012CashAcceptor0009FRAMEWORK00102023/01/24001223:55 47.4370007DEVRETN0021CBaseService::Execute0060HSERVICE[12] COMMAND[1301] HRESULT[0] OUT BUFFER[0x00000000]01694294967295012500090887580003BRM0002SP00102023/01/24001223:55 47.4370011INFORMATION0025CBrmCommand::UpdateStatus0024IdleStatus changed[0->1]01254294967295012200090887590006COMMON0009FRAMEWORK00102023/01/24001223:55 47.4370011INFORMATION0015CProcessor::Run0021---- Wait [2148] ----01224294967295029900090887600003CIM0003SPI00102023/01/24001223:55 47.4370009XFS_EVENT0013EXECUTE[1301]0211WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10516],
	hService = [12],
	tsTimestamp = [2023/01/24 23:55 47.437],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = NULL
}";
      public const string WFS_CMD_CIM_CASH_IN_START_2 = @"05554294967295017512628966600004CCIM0002SP00102023/01/03001216:05 27.2720006RESULT0033CNHDevCommWrapper::ExecuteCommand0070Encrypted Command('î':0xEE, 0x01) Return=[0] Result=[0] Error=[000000]01754294967295028212628966610004CCIM0002SP00102023/01/03001216:05 27.2730011INFORMATION0019CCCIMDev::_ModeSet20186Result : [WAY:34, NOMICR:41, COLOR:47, CST1:41, CST2:41, NationCode:00, DepositMode:4e, CstOperMode:00, ClientInfo:00, EntryInDoingTimeout:60, EntryInWaitTimeout:20, byPermanentError:00]02824294967295015412628966620004CCIM0002SP00102023/01/03001216:05 27.2740011INFORMATION0019CCCIMDev::_ModeSet20058## CountInfo ## BDM=0 ESC=0 RJT=0 RET=0 A6=0 CST1=0 CST2=001544294967295012312628966630004CCIM0002SP00102023/01/03001216:05 27.2760011INFORMATION0020CNHCCIMImpl::Execute0026cash-in transaction active01234294967295014712628966640004CCIM0002SP00102023/01/03001216:05 27.2770011INFORMATION0020CNHCCIMImpl::Execute0050dwCommand=[1301] hResult=[0] lpBuffer=[0x00000000]01474294967295011712628966650004CCIM0002SP00102023/01/03001216:05 27.2870007COMMAND0033CNHDevCommWrapper::ExecuteCommand0011Sensor Read01174294967295016912628966660012CashAcceptor0009FRAMEWORK00102023/01/03001216:05 27.3030007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1301] HRESULT[0] OUT BUFFER[0x00000000]01694294967295014912628966670007nh CCIM0002SP00102023/01/03001216:05 27.3240004DATA0038CNHUsb6::SendControlDataWithEPUSBReset0038-> [8] 0000h: C0 50 00 00 00 00 40 00 01494294967295029812628966680003CIM0003SPI00102023/01/03001216:05 27.3240009XFS_EVENT0013EXECUTE[1301]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000200e0],
	RequestID = [1282],
	hService = [11],
	tsTimestamp = [2023/01/03 16:05 27.324],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = NULL
}";
      public const string WFS_CMD_CIM_CASH_IN_START_3 = @"13474294967295019100442786300006COMMON0009FRAMEWORK00102023/02/02001210:12 41.8700011INFORMATION0025CCommandQueue::RemoveHead0080return (bResult[1]) pCommand{hService[12],..., ReqID[2798], dwCommandCode[1601]}01914294967295013300442786310010CCIM-MIXED0002SP00102023/02/02001210:12 41.8710011INFORMATION0020CNHCCIMImpl::Execute0030call WFS_CMD_CIM_CASH_IN_START01334294967295012200442786320006COMMON0009FRAMEWORK00102023/02/02001210:12 41.8710011INFORMATION0015CProcessor::Run0021Execute{hService[12]}01224294967295016700442786330006COMMON0009FRAMEWORK00102023/02/02001210:12 41.8720011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[12], LogicalName[ItemProcessor]}01674294967295015800442786340013ItemProcessor0009FRAMEWORK00102023/02/02001210:12 41.8720007DEVCALL0021CBaseService::Execute0048HSERVICE[12] COMMAND[1601] IN BUFFER[0x007FA2E4]01584294967295016300442786350010CCIM-MIXED0002SP00102023/02/02001210:12 41.8720011INFORMATION0023CCCIMDev::SetLimitValue0057byNationCode=0x00, ulTotalAmountLimit=0, m_ulTotalCount=001634294967295014000442786360006SENSOR0007ACTIVEX00102023/02/02001210:12 41.8720008PROPERTY0028Ctrl::GetEnhancedAudioStatus0031EnhancedAudioStatus[NOTPRESENT]01404294967295012800442786370010CCIM-MIXED0002SP00102023/02/02001210:12 41.8730011INFORMATION0020CNHCCIMImpl::Execute0025call WFS_CMD_IPM_MEDIA_IN01284294967295012000442786380010CCIM-MIXED0002SP00102023/02/02001210:12 41.8730011INFORMATION0021CCCIMDev::CashInStart0016call CashInStart01204294967295014000442786390006SENSOR0007ACTIVEX00102023/02/02001210:12 41.8730008PROPERTY0028Ctrl::GetEnhancedAudioStatus0031EnhancedAudioStatus[NOTPRESENT]01404294967295013000442786400010CCIM-MIXED0002SP00102023/02/02001210:12 41.8730011INFORMATION0025CNHCCIMImpl::OnIPMMediaIn0022<<<Wait CIM command>>>01304294967295012900442786410010CCIM-MIXED0002SP00102023/02/02001210:12 41.8730011INFORMATION0020CNHCCIMImpl::Execute0026cash-in transaction active01294294967295014000442786420006SENSOR0007ACTIVEX00102023/02/02001210:12 41.8740008PROPERTY0028Ctrl::GetEnhancedAudioStatus0031EnhancedAudioStatus[NOTPRESENT]01404294967295015300442786430010CCIM-MIXED0002SP00102023/02/02001210:12 41.8740011INFORMATION0020CNHCCIMImpl::Execute0050dwCommand=[1301] hResult=[0] lpBuffer=[0x00000000]01534294967295014000442786440006SENSOR0007ACTIVEX00102023/02/02001210:12 41.8740008PROPERTY0028Ctrl::GetEnhancedAudioStatus0031EnhancedAudioStatus[NOTPRESENT]01404294967295016900442786450012CashAcceptor0009FRAMEWORK00102023/02/02001210:12 41.8740007DEVRETN0021CBaseService::Execute0060HSERVICE[14] COMMAND[1301] HRESULT[0] OUT BUFFER[0x00000000]01694294967295019100442786460006COMMON0009FRAMEWORK00102023/02/02001210:12 41.8750011INFORMATION0025CCommandQueue::RemoveHead0080return (bResult[1]) pCommand{hService[14],..., ReqID[2797], dwCommandCode[1302]}01914294967295029800442786470003CIM0003SPI00102023/02/02001210:12 41.8750009XFS_EVENT0013EXECUTE[1301]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010344],
	RequestID = [2796],
	hService = [14],
	tsTimestamp = [2023/02/02 10:12 41.875],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = NULL
}";

      public const string WFS_CMD_CIM_CASH_IN_1 = @"25754294967295016912627003220012CashAcceptor0009FRAMEWORK00102023/01/03001214:42 11.7900007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1302] HRESULT[0] OUT BUFFER[0x0068AF64]01694294967295012012627003230004CCIM0002SP00102023/01/03001214:42 11.7920011INFORMATION0017CCCIMDev::_Sensor0026BDS05 sensor changed(1->0)01204294967295051212627003240003CIM0003SPI00102023/01/03001214:42 11.7930009XFS_EVENT0013EXECUTE[1302]0424WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010334],
	RequestID = [21387],
	hService = [11],
	tsTimestamp = [2023/01/03 14:42 11.793],
	hResult = [0],
	u.dwCommandCode = [1302],
	lpBuffer = [0x1c5eccc4]

	{
		usNumOfNoteNumbers = [3],
		lppNoteNumber = 
		{
			usNoteID = [1],
			ulCount = [1]
		}
		{
			usNoteID = [16],
			ulCount = [2]
		}
		{
			usNoteID = [15],
			ulCount = [3]
		}
	}
}";
      public const string WFS_CMD_CIM_CASH_IN_2 = @"25754294967295016900158070990012CashAcceptor0009FRAMEWORK00102022/12/07001207:50 38.5110007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1302] HRESULT[0] OUT BUFFER[0x006C818C]01694294967295012000158071000004CCIM0002SP00102022/12/07001207:50 38.5110011INFORMATION0017CCCIMDev::_Sensor0026BDS04 sensor changed(1->0)01204294967295012200158071010006COMMON0009FRAMEWORK00102022/12/07001207:50 38.5110011INFORMATION0015CProcessor::Run0021---- Wait [2620] ----01224294967295041500158071020003CIM0003SPI00102022/12/07001207:50 38.5110009XFS_EVENT0013EXECUTE[1302]0327WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00020128],
	RequestID = [56148],
	hService = [11],
	tsTimestamp = [2022/12/07 07:50 38.511],
	hResult = [0],
	u.dwCommandCode = [1302],
	lpBuffer = [0x162e329c]

	{
		usNumOfNoteNumbers = [1],
		lppNoteNumber = 
		{
			usNoteID = [15],
			ulCount = [9]
		}
	}
}";
      public const string WFS_CMD_CIM_CASH_IN_3 = @"05024294967295016900525929740012CashAcceptor0009FRAMEWORK00102022/12/20001212:04 17.3790007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1302] HRESULT[0] OUT BUFFER[0x01714E0C]01694294967295012200525929750006COMMON0009FRAMEWORK00102022/12/20001212:04 17.3790011INFORMATION0015CProcessor::Run0021---- Wait [4916] ----01224294967295051100525929760003CIM0003SPI00102022/12/20001212:04 17.3790009XFS_EVENT0013EXECUTE[1302]0423WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000102f8],
	RequestID = [834],
	hService = [11],
	tsTimestamp = [2022/12/20 12:04 17.379],
	hResult = [0],
	u.dwCommandCode = [1302],
	lpBuffer = [0x223109b4]

	{
		usNumOfNoteNumbers = [3],
		lppNoteNumber = 
		{
			usNoteID = [11],
			ulCount = [1]
		}
		{
			usNoteID = [15],
			ulCount = [9]
		}
		{
			usNoteID = [16],
			ulCount = [9]
		}
	}
}";

      public const string WFS_CMD_CIM_CASH_IN_END_1 = @"05024294967295016900090992990012CashAcceptor0009FRAMEWORK00102023/01/24001223:57 44.2010007DEVRETN0021CBaseService::Execute0060HSERVICE[12] COMMAND[1303] HRESULT[0] OUT BUFFER[0x015184CC]01694294967295012200090993000006COMMON0009FRAMEWORK00102023/01/24001223:57 44.2020011INFORMATION0015CProcessor::Run0021---- Wait [2148] ----01224294967295148400090993010003CIM0003SPI00102023/01/24001223:57 44.2020009XFS_EVENT0013EXECUTE[1303]1396WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10822],
	hService = [12],
	tsTimestamp = [2023/01/24 23:57 44.202],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x3630f974]
	{
		usCount=1
		lppCashIn->
		{
			usNumber		7
			fwType			2
			fwItemType		0x0003
			cUnitID			LCU06
			cCurrencyID		   
			ulValues		0
			ulCashInCount		1
			ulCount			1
			ulMaximum		1400
			usStatus		0
			bAppLock		0

			lppPhysical->
			usNumPhysicalCUs	1		
			lpPhysicalPositionName	CassetteE	
			cUnitID			CST_E
			ulCashInCount		1
			ulCount			1
			ulMaximum		1400
			usPStatus		0
			bHardwareSensor		1
			ulInitialCount		0
			ulDispensedCount	0		
			ulPresentedCount	0		
			ulRetractedCount	0		
			ulRejectCount		0
			lpszExtra(PCU)		[SerialNumber=CJOG203649]

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
			[15]			[16]0		
			[16]			[17]1		
			[17]			[0]0		

			LCU ETC
			usCDMType		0
			lpszCashUnitName	LCU06		
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
      public const string WFS_CMD_CIM_CASH_IN_END_2 = @"03274294967295146612623597620003CIM0003SPI00102023/01/03001211:37 00.1210009XFS_EVENT0013EXECUTE[1303]1378WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010334],
	RequestID = [11983],
	hService = [11],
	tsTimestamp = [2023/01/03 11:37 00.121],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x2ba829bc]
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
			ulCashInCount		2
			ulCount			2
			ulMaximum		0
			usStatus		0
			bAppLock		0

			lppPhysical->
			usNumPhysicalCUs	1		
			lpPhysicalPositionName	CASHCASSETTE	
			cUnitID			PCU02
			ulCashInCount		2
			ulCount			2
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
			[15]			[16]0		
			[16]			[17]2		
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
      public const string WFS_CMD_CIM_CASH_IN_END_3 = @"03474294967295013102416521570010CCIM-MIXED0002SP00102023/01/16001208:14 06.0750011INFORMATION0020CNHCCIMImpl::Execute0028cash-in transaction complete01314294967295014502416521580010CCIM-MIXED0002SP00102023/01/16001208:14 06.0760011INFORMATION0025CCCIMDev::BackUpEPLogFile0037Main EP/BC EP Log Request Start >>>>>01454294967295012902416521590010CCIM-MIXED0002SP00102023/01/16001208:14 06.0760011INFORMATION0017CCCIMDev::_Sensor0029ESC_RESI sensor changed(1->0)01294294967295015302416521600010CCIM-MIXED0002SP00102023/01/16001208:14 06.0760011INFORMATION0025CCCIMDev::BackUpEPLogFile0045## Log Thread Terminat Request(0x03b38978) ##01534294967295015302416521610010CCIM-MIXED0002SP00102023/01/16001208:14 06.0760011INFORMATION0020CNHCCIMImpl::Execute0050dwCommand=[1303] hResult=[0] lpBuffer=[0x005510d4]01534294967295021302416521620010CCIM-MIXED0002SP00102023/01/16001208:14 06.0770011INFORMATION0017CCCIMDev::_Sensor0113##iCheckLog Start SENSOR_DATA Unit: CCIM, Message: 00282240d0020c900302a30200224e004403f0018380f1 iCheckLog End##02134294967295016902416521630012CashAcceptor0009FRAMEWORK00102023/01/16001208:14 06.0770007DEVRETN0021CBaseService::Execute0060HSERVICE[13] COMMAND[1303] HRESULT[0] OUT BUFFER[0x005510D4]01694294967295011302416521640010CCIM-MIXED0002SP00102023/01/16001208:14 06.0770011INFORMATION0009LogThread0021>>> BCLogThread Start01134294967295014302416521650010CCIM-MIXED0002SP00102023/01/16001208:14 06.0770011INFORMATION0025CCCIMDev::BackUpEPLogFile0035<<<<< Main EP/BC EP Log Request End01434294967295012202416521660006COMMON0009FRAMEWORK00102023/01/16001208:14 06.0770011INFORMATION0015CProcessor::Run0021---- Wait [4728] ----01224294967295012702416521670010CCIM-MIXED0002SP00102023/01/16001208:14 06.0770011INFORMATION0017CCCIMDev::_Sensor0027MediaInActive changed(1->0)01274294967295035202416521680003CIM0003SPI00102023/01/16001208:14 06.0770009XFS_EVENT0013EXECUTE[1303]0264WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x0001033e],
	RequestID = [5405],
	hService = [13],
	tsTimestamp = [2023/01/16 08:14 06.077],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x1e1f01cc]
	{
		usCount = [0],
		lppCashIn = NULL
	}
}";

      public const string WFS_CMD_CIM_CASH_IN_ROLLBACK_1 = @"05024294967295016900316511240012CashAcceptor0009FRAMEWORK00102022/10/31001213:26 13.1110007DEVRETN0021CBaseService::Execute0060HSERVICE[13] COMMAND[1304] HRESULT[0] OUT BUFFER[0x00000000]01694294967295012200316511250006COMMON0009FRAMEWORK00102022/10/31001213:26 13.1110011INFORMATION0015CProcessor::Run0021---- Wait [5672] ----01224294967295029800316511260003CIM0003SPI00102022/10/31001213:26 13.1110009XFS_EVENT0013EXECUTE[1304]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00020224],
	RequestID = [4498],
	hService = [13],
	tsTimestamp = [2022/10/31 13:26 13.111],
	hResult = [0],
	u.dwCommandCode = [1304],
	lpBuffer = NULL
}";
      public const string WFS_CMD_CIM_CASH_IN_ROLLBACK_2 = @"15084294967295020000213865160003CIM0007ACTIVEX00102022/11/13001222:15 46.1000006XFSAPI0023CContextMgr::MgrWndProc0101WFS_GETINFO_COMPLETE(RequestID=14798, hService=11, hResult=0, dwCommandCode=1301 lpBuffer=0x284372C4)02004294967295015900213865170003CIM0007ACTIVEX00102022/11/13001222:15 46.1000011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x161D20C8, lParam=0x1DA1A81401594294967295015700213865180003CIM0007ACTIVEX00102022/11/13001222:15 46.1000011INFORMATION0039CCimService::HandleStatus [ReqID=14798]0037GetInfo-Result[Status] = {hResult[0]}01574294967295017200213865190003CIM0007ACTIVEX00102022/11/13001222:15 46.1000011INFORMATION0039CCimService::HandleStatus [ReqID=14798]0052StStacker-old, StStacker-new : { NOTEMPTY , EMPTY }.01724294967295015900213865200003CIM0007ACTIVEX00102022/11/13001222:15 46.1000011INFORMATION0041CCimService::SetStackerStatusChangedEvent0037[AddEvent]StackerStatusChanged[EMPTY]01594294967295017300213865210003CIM0007ACTIVEX00102022/11/13001222:15 46.1010011INFORMATION0039CCimService::HandleStatus [ReqID=14798]0053StInShutter-old, StInShutter-new : { CLOSED , OPEN }.01734294967295016900213865220003CIM0007ACTIVEX00102022/11/13001222:15 46.1010011INFORMATION0041CCimService::SetShutterStatusChangedEvent0047[AddEvent]ShutterStatusChanged[INPOSITION,OPEN]01694294967295017500213865230003CIM0007ACTIVEX00102022/11/13001222:15 46.1010011INFORMATION0039CCimService::HandleStatus [ReqID=14798]0055StOutShutter-old, StOutShutter-new : { CLOSED , OPEN }.01754294967295017000213865240003CIM0007ACTIVEX00102022/11/13001222:15 46.1010011INFORMATION0041CCimService::SetShutterStatusChangedEvent0048[AddEvent]ShutterStatusChanged[OUTPOSITION,OPEN]01704294967295017800213865250003CIM0007ACTIVEX00102022/11/13001222:15 46.1010011INFORMATION0039CCimService::HandleStatus [ReqID=14798]0058StInPosition-old, StInPosition-new : { EMPTY , NOTEMPTY }.01784294967295017500213865260003CIM0007ACTIVEX00102022/11/13001222:15 46.1010011INFORMATION0042CCimService::SetPositionStatusChangedEvent0052[AddEvent]PositionStatusChanged[INPOSITION,NOTEMPTY]01754294967295018000213865270003CIM0007ACTIVEX00102022/11/13001222:15 46.1010011INFORMATION0039CCimService::HandleStatus [ReqID=14798]0060StOutPosition-old, StOutPosition-new : { EMPTY , NOTEMPTY }.01804294967295017600213865280003CIM0007ACTIVEX00102022/11/13001222:15 46.1020011INFORMATION0042CCimService::SetPositionStatusChangedEvent0053[AddEvent]PositionStatusChanged[OUTPOSITION,NOTEMPTY]01764294967295012300213865290003CIM0007ACTIVEX00102022/11/13001222:15 46.1020005EVENT0030CCimService::FireDeferredEvent0018FireItemsPresented01234294967295012800213865300004CCIM0002SP00102022/11/13001222:15 46.1440011INFORMATION0020CNHCCIMImpl::Execute0031cash-in transaction rolled back01284294967295014700213865310004CCIM0002SP00102022/11/13001222:15 46.1450011INFORMATION0020CNHCCIMImpl::Execute0050dwCommand=[1304] hResult=[0] lpBuffer=[0x0065fba4]01474294967295016900213865320012CashAcceptor0009FRAMEWORK00102022/11/13001222:15 46.1450007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1304] HRESULT[0] OUT BUFFER[0x0065FBA4]01694294967295012200213865330006COMMON0009FRAMEWORK00102022/11/13001222:15 46.1460011INFORMATION0015CProcessor::Run0021---- Wait [4708] ----01224294967295029900213865340003CIM0003SPI00102022/11/13001222:15 46.1460009XFS_EVENT0013EXECUTE[1304]0211WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000103d0],
	RequestID = [14796],
	hService = [11],
	tsTimestamp = [2022/11/13 22:15 46.146],
	hResult = [0],
	u.dwCommandCode = [1304],
	lpBuffer = NULL
}";
      public const string WFS_CMD_CIM_CASH_IN_ROLLBACK_3 = @"15084294967295020000156376760003CIM0007ACTIVEX00102022/12/06001221:42 03.5240006XFSAPI0023CContextMgr::MgrWndProc0101WFS_GETINFO_COMPLETE(RequestID=52022, hService=11, hResult=0, dwCommandCode=1301 lpBuffer=0x20CE66F4)02004294967295015900156376770003CIM0007ACTIVEX00102022/12/06001221:42 03.5240011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x1D841668, lParam=0x21AB9FA401594294967295015700156376780003CIM0007ACTIVEX00102022/12/06001221:42 03.5240011INFORMATION0039CCimService::HandleStatus [ReqID=52022]0037GetInfo-Result[Status] = {hResult[0]}01574294967295017200156376790003CIM0007ACTIVEX00102022/12/06001221:42 03.5240011INFORMATION0039CCimService::HandleStatus [ReqID=52022]0052StStacker-old, StStacker-new : { NOTEMPTY , EMPTY }.01724294967295015900156376800003CIM0007ACTIVEX00102022/12/06001221:42 03.5240011INFORMATION0041CCimService::SetStackerStatusChangedEvent0037[AddEvent]StackerStatusChanged[EMPTY]01594294967295017300156376810003CIM0007ACTIVEX00102022/12/06001221:42 03.5240011INFORMATION0039CCimService::HandleStatus [ReqID=52022]0053StInShutter-old, StInShutter-new : { CLOSED , OPEN }.01734294967295016900156376820003CIM0007ACTIVEX00102022/12/06001221:42 03.5400011INFORMATION0041CCimService::SetShutterStatusChangedEvent0047[AddEvent]ShutterStatusChanged[INPOSITION,OPEN]01694294967295017500156376830003CIM0007ACTIVEX00102022/12/06001221:42 03.5400011INFORMATION0039CCimService::HandleStatus [ReqID=52022]0055StOutShutter-old, StOutShutter-new : { CLOSED , OPEN }.01754294967295017000156376840003CIM0007ACTIVEX00102022/12/06001221:42 03.5400011INFORMATION0041CCimService::SetShutterStatusChangedEvent0048[AddEvent]ShutterStatusChanged[OUTPOSITION,OPEN]01704294967295017800156376850003CIM0007ACTIVEX00102022/12/06001221:42 03.5400011INFORMATION0039CCimService::HandleStatus [ReqID=52022]0058StInPosition-old, StInPosition-new : { EMPTY , NOTEMPTY }.01784294967295017500156376860003CIM0007ACTIVEX00102022/12/06001221:42 03.5400011INFORMATION0042CCimService::SetPositionStatusChangedEvent0052[AddEvent]PositionStatusChanged[INPOSITION,NOTEMPTY]01754294967295018000156376870003CIM0007ACTIVEX00102022/12/06001221:42 03.5400011INFORMATION0039CCimService::HandleStatus [ReqID=52022]0060StOutPosition-old, StOutPosition-new : { EMPTY , NOTEMPTY }.01804294967295017600156376880003CIM0007ACTIVEX00102022/12/06001221:42 03.5400011INFORMATION0042CCimService::SetPositionStatusChangedEvent0053[AddEvent]PositionStatusChanged[OUTPOSITION,NOTEMPTY]01764294967295012300156376890003CIM0007ACTIVEX00102022/12/06001221:42 03.5400005EVENT0030CCimService::FireDeferredEvent0018FireItemsPresented01234294967295012800156376900004CCIM0002SP00102022/12/06001221:42 03.5870011INFORMATION0020CNHCCIMImpl::Execute0031cash-in transaction rolled back01284294967295014700156376910004CCIM0002SP00102022/12/06001221:42 03.5870011INFORMATION0020CNHCCIMImpl::Execute0050dwCommand=[1304] hResult=[0] lpBuffer=[0x006a18d4]01474294967295016900156376920012CashAcceptor0009FRAMEWORK00102022/12/06001221:42 03.5870007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1304] HRESULT[0] OUT BUFFER[0x006A18D4]01694294967295012200156376930006COMMON0009FRAMEWORK00102022/12/06001221:42 03.5870011INFORMATION0015CProcessor::Run0021---- Wait [2620] ----01224294967295029900156376940003CIM0003SPI00102022/12/06001221:42 03.5870009XFS_EVENT0013EXECUTE[1304]0211WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00020128],
	RequestID = [52020],
	hService = [11],
	tsTimestamp = [2022/12/06 21:42 03.587],
	hResult = [0],
	u.dwCommandCode = [1304],
	lpBuffer = NULL
}";

      public const string WFS_CMD_CIM_RETRACT_1 = @"02454294967295017009344484960003CIM0009FRAMEWORK00102022/10/23001217:52 11.3870011INFORMATION0021CBaseService::GetInfo0066Srvc=1303 ReqID=196600 Wnd=0x0001032A Cmd=1303 TimeOut=300000 IO=101704294967295012009344484970004CCIM0002SP00102022/10/23001217:52 11.3870011INFORMATION0017CCCIMDev::_Sensor0026RTS03 sensor changed(0->1)01204294967295012209344484980006COMMON0009FRAMEWORK00102022/10/23001217:52 11.3880011INFORMATION0015CProcessor::Run0021---- Wait [4776] ----01224294967295147909344484990003CIM0003SPI00102022/10/23001217:52 11.3880009XFS_EVENT0013EXECUTE[1305]1391WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x0001032a],
	RequestID = [196599],
	hService = [11],
	tsTimestamp = [2022/10/23 17:52 11.388],
	hResult = [0],
	u.dwCommandCode = [1305],
	lpBuffer = [0x35529fac]
	{
		usCount=1
		lppCashIn->
		{
			usNumber		1
			fwType			4
			fwItemType		0x0001
			cUnitID			LCU00
			cCurrencyID		USD
			ulValues		0
			ulCashInCount		17
			ulCount			1
			ulMaximum		126
			usStatus		0
			bAppLock		0

			lppPhysical->
			usNumPhysicalCUs	1		
			lpPhysicalPositionName	RETRACTCASSETTE	
			cUnitID			PCU01
			ulCashInCount		17
			ulCount			17
			ulMaximum		0
			usPStatus		0
			bHardwareSensor		0
			ulInitialCount		0
			ulDispensedCount	0		
			ulPresentedCount	0		
			ulRetractedCount	0		
			ulRejectCount		0
			lpszExtra(PCU)		NULL

			lpNoteNumberList->
			[0]			[1]3		
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
			[14]			[15]1		
			[15]			[16]0		
			[16]			[17]0		
			[17]			[0]13		

			LCU ETC
			usCDMType		0
			lpszCashUnitName	RETRACTCASSETTE	
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
      public const string WFS_CMD_CIM_RETRACT_2 = @"03824294967295013700012812530003CIM0009FRAMEWORK00102023/01/30001212:37 49.6420011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=13, wClass=001374294967295015900012812540003CIM0007ACTIVEX00102023/01/30001212:37 49.6420011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000417, wParam=0x1CC71770, lParam=0x1A93AA8C01594294967295016900012812550012CashAcceptor0009FRAMEWORK00102023/01/30001212:37 49.6430007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1305] HRESULT[0] OUT BUFFER[0x007D56F4]01694294967295012500012812560007nh CCIM0002SP00102023/01/30001212:37 49.6450006NORMAL0016CNHUsb2::Encrypt0034NHDevCrypto_sendMessageEx Return=101254294967295015900012812570003CIM0009FRAMEWORK00102023/01/30001212:37 49.6460011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x0C7B82A0, pCursor->pData->hApp=0x0C7B82A001594294967295012600012812580003CIM0003SPI00102023/01/30001212:37 49.6460011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x05974D28)01264294967295013400012812590003CIM0007ACTIVEX00102023/01/30001212:37 49.6460011INFORMATION0031CCimService::HandleDeviceStatus0022Device Status Changed.01344294967295012200012812600006COMMON0009FRAMEWORK00102023/01/30001212:37 49.6470011INFORMATION0015CProcessor::Run0021---- Wait [2016] ----01224294967295147300012812610003CIM0003SPI00102023/01/30001212:37 49.6470009XFS_EVENT0013EXECUTE[1305]1385WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000103b6],
	RequestID = [2023],
	hService = [11],
	tsTimestamp = [2023/01/30 12:37 49.647],
	hResult = [0],
	u.dwCommandCode = [1305],
	lpBuffer = [0x1ac9a174]
	{
		usCount=1
		lppCashIn->
		{
			usNumber		1
			fwType			4
			fwItemType		0x0001
			cUnitID			LCU00
			cCurrencyID		USD
			ulValues		0
			ulCashInCount		8
			ulCount			1
			ulMaximum		126
			usStatus		0
			bAppLock		0

			lppPhysical->
			usNumPhysicalCUs	1		
			lpPhysicalPositionName	RETRACTCASSETTE	
			cUnitID			PCU01
			ulCashInCount		8
			ulCount			8
			ulMaximum		0
			usPStatus		0
			bHardwareSensor		0
			ulInitialCount		0
			ulDispensedCount	0		
			ulPresentedCount	0		
			ulRetractedCount	0		
			ulRejectCount		0
			lpszExtra(PCU)		NULL

			lpNoteNumberList->
			[0]			[1]4		
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
			[12]			[13]1		
			[13]			[14]1		
			[14]			[15]1		
			[15]			[16]1		
			[16]			[17]0		
			[17]			[0]0		

			LCU ETC
			usCDMType		0
			lpszCashUnitName	RETRACTCASSETTE	
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
      public const string WFS_CMD_CIM_RETRACT_3 = @"03474294967295011402692743100003BRM0002SP00102023/03/08001210:22 40.8740006NORMAL0021CNHBRMDev::MakeOutput0022#LCU changed: Number=001144294967295012502692743110003BRM0002SP00102023/03/08001210:22 40.8750006NORMAL0018CNHBRMDev::Execute0036cash-in transaction ended by RETRACT01254294967295009702692743120003BRM0002SP00102023/03/08001210:22 40.8780006NORMAL0018CNHBRMDev::Execute0008return 000974294967295016902692743130012CashAcceptor0009FRAMEWORK00102023/03/08001210:22 40.8780007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1305] HRESULT[0] OUT BUFFER[0x0130D744]01694294967295012202692743140006COMMON0009FRAMEWORK00102023/03/08001210:22 40.8790011INFORMATION0015CProcessor::Run0021---- Wait [5176] ----01224294967295237402692743150003CIM0003SPI00102023/03/08001210:22 40.8790009XFS_EVENT0013EXECUTE[1305]2286WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00020256],
	RequestID = [2549],
	hService = [11],
	tsTimestamp = [2023/03/08 10:22 40.879],
	hResult = [0],
	u.dwCommandCode = [1305],
	lpBuffer = [0x2477ec64]
	{
		usCount = [1],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [17],
			ulCount = [1],
			ulMaximum = [400],
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
					ulCount = [4]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [10]
				}
				{
					usNoteID = [0],
					ulCount = [3]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RetractCassette,
				cUnitID = [RTCST],
				ulCashInCount = [17],
				ulCount = [1],
				ulMaximum = [400],
				usPStatus = [0],
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
			lpszCashUnitName = (null),
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
	}
}";

      public const string WFS_CMD_CIM_RESET_1 = @"05024294967295016800088601810003CDM0009FRAMEWORK00102023/01/24001211:19 13.5510011INFORMATION0021CBaseService::GetInfo0064Srvc=1303 ReqID=3034 Wnd=0x000201E0 Cmd=1303 TimeOut=300000 IO=201684294967295014000088601820003BRM0002SP00102023/01/24001211:19 13.5520011INFORMATION0019CMonitorThread::Run0045m_oldBrmCimInfo.LCU[i].ulValues changed(5->0)01404294967295015800088601830012CashAcceptor0009FRAMEWORK00102023/01/24001211:19 13.5520007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295016900088601840012CashAcceptor0009FRAMEWORK00102023/01/24001211:19 13.5520007DEVRETN0021CBaseService::Execute0060HSERVICE[27] COMMAND[1313] HRESULT[0] OUT BUFFER[0x00000000]01694294967295014200088601850003BRM0002SP00102023/01/24001211:19 13.5520011INFORMATION0019CMonitorThread::Run0047m_oldBrmCimInfo.LCU[i].ulCount changed(2000->0)01424294967295018900088601860006COMMON0009FRAMEWORK00102023/01/24001211:19 13.5530011INFORMATION0025CCommandQueue::RemoveHead0078return (bResult[1]) pCommand{hService[2],..., ReqID[3032], dwCommandCode[321]}01894294967295029800088601870003CIM0003SPI00102023/01/24001211:19 13.5530009XFS_EVENT0013EXECUTE[1313]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00020620],
	RequestID = [3029],
	hService = [27],
	tsTimestamp = [2023/01/24 11:19 13.553],
	hResult = [0],
	u.dwCommandCode = [1313],
	lpBuffer = NULL
}";
      public const string WFS_CMD_CIM_RESET_2 = @"05024294967295016900698100840012CashAcceptor0009FRAMEWORK00102022/12/07001219:42 13.9360007DEVRETN0021CBaseService::Execute0060HSERVICE[33] COMMAND[1313] HRESULT[0] OUT BUFFER[0x00000000]01694294967295012300698100850006COMMON0009FRAMEWORK00102022/12/07001219:42 13.9360011INFORMATION0015CProcessor::Run0022---- Wait [18964] ----01234294967295030000698100860003CIM0003SPI00102022/12/07001219:42 13.9360009XFS_EVENT0013EXECUTE[1313]0212WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00030044],
	RequestID = [256406],
	hService = [33],
	tsTimestamp = [2022/12/07 19:42 13.936],
	hResult = [0],
	u.dwCommandCode = [1313],
	lpBuffer = NULL
}";
      public const string WFS_CMD_CIM_RESET_3 = @"05024294967295016900530719380012CashAcceptor0009FRAMEWORK00102022/12/21001208:41 10.7150007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1313] HRESULT[0] OUT BUFFER[0x00000000]01694294967295012200530719390006COMMON0009FRAMEWORK00102022/12/21001208:41 10.7150011INFORMATION0015CProcessor::Run0021---- Wait [4856] ----01224294967295029800530719400003CIM0003SPI00102022/12/21001208:41 10.7150009XFS_EVENT0013EXECUTE[1313]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000102f8],
	RequestID = [1551],
	hService = [11],
	tsTimestamp = [2022/12/21 08:41 10.715],
	hResult = [0],
	u.dwCommandCode = [1313],
	lpBuffer = NULL
}";

      // EVENTS
      public const string WFS_USRE_CIM_CASHUNITTHRESHOLD_1 = @"
02434294967295016801399396100003CDM0009FRAMEWORK00102023/08/15001215:11 54.5580011INFORMATION0021CBaseService::GetInfo0064Srvc=1303 ReqID=6880 Wnd=0x00020202 Cmd=1303 TimeOut=300000 IO=201684294967295015901399396110003CDM0009FRAMEWORK00102023/08/15001215:11 54.5590011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x1E5C10E5, pCursor->pData->hApp=0x002D46CD01594294967295012601399396120003CIM0003SPI00102023/08/15001215:11 54.5590011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x1AEF7D30)01264294967295015801399396130012CashAcceptor0009FRAMEWORK00102023/08/15001215:11 54.5590007DEVCALL0021CBaseService::GetInfo0049HSERVICE[11] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295018601399396140007MONIMNG0008XFSAGENT00102023/08/15001215:11 54.5590004INFO0015Agent::FireTrap0092call WFSAsyncGetInfo(ManagedName=[CashAcceptor] hService=[29] dwCategory=[1303] hResult=[0])01864294967295013701399396150003CDM0009FRAMEWORK00102023/08/15001215:11 54.5590011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295123701399396160003CIM0003SPI00102023/08/15001215:11 54.5590009XFS_EVENT0016USER_EVENT[1303]1146WFS_USER_EVENT, 
lpResult =
{
	hWnd = [0x00020202],
	RequestID = [0],
	hService = [11],
	tsTimestamp = [2023/08/15 15:11 54.556],
	hResult = [9],
	u.dwEventID = [1303],
	lpBuffer = [0x256746e4]
	{
		usNumber = [5],
		fwType = [1],
		fwItemType = [0x0004],
		cUnitID = [LCU04],
		cCurrencyID = [USD],
		ulValues = [20],
		ulCashInCount = [2082],
		ulCount = [1851],
		ulMaximum = [0],
		usStatus = [1],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [3],
			lppNoteNumber = 
			{
				usNoteID = [5],
				ulCount = [1851]
			}
			{
				usNoteID = [10],
				ulCount = [0]
			}
			{
				usNoteID = [15],
				ulCount = [0]
			}
		}
		usNumPhysicalCUs = [1],
		lppPhysical = 
		{
			lpPhysicalPositionName = [CassetteC],
			cUnitID = [CST_C],
			ulCashInCount = [2082],
			ulCount = [1851],
			ulMaximum = [0],
			usPStatus = [1],
			bHardwareSensor = [1],
			lpszExtra = [SerialNumber=CEDO274728,DipSW=10001]
			ulInitialCount = [485],
			ulDispensedCount = [716],
			ulPresentedCount = [716],
			ulRetractedCount = [0],
			ulRejectCount = [0],
		}
		lpszExtra = NULL
	}
}
";

      public const string WFS_SRVE_CIM_CASHUNITINFOCHANGED_1 = @"12174294967295015900090672220003CDM0009FRAMEWORK00102023/01/24001221:44 59.5010011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x0BB04582, pCursor->pData->hApp=0x0E770CEA01594294967295019300090672230003CIM0007ACTIVEX00102023/01/24001221:44 59.5010006XFSAPI0023CContextMgr::MgrWndProc0094WFS_SERVICE_EVENT(RequestID=0, hService=12, hResult=0, dwCommandCode=1304 lpBuffer=0x0C59DB24)01934294967295013800090672240003CDM0009FRAMEWORK00102023/01/24001221:44 59.5020011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015900090672250003CIM0007ACTIVEX00102023/01/24001221:44 59.5020011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000415, wParam=0x2E21D918, lParam=0x252068A401594294967295015900090672260003CDM0009FRAMEWORK00102023/01/24001221:44 59.5020011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x0E770CEA, pCursor->pData->hApp=0x0E770CEA01594294967295012600090672270003CIM0003SPI00102023/01/24001221:44 59.5020011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x05AF7508)01264294967295014800090672280003CIM0007ACTIVEX00102023/01/24001221:44 59.5020011INFORMATION0038CCimService::HandleCashUnitInfoChanged0029[AddEvent]FireCashUnitChanged01484294967295013700090672290003CDM0009FRAMEWORK00102023/01/24001221:44 59.5020011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295121700090672300003CIM0003SPI00102023/01/24001221:44 59.5020009XFS_EVENT0019SERVICE_EVENT[1304]1123WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x000206f8],
	RequestID = [0],
	hService = [27],
	tsTimestamp = [2023/01/24 21:44 59.497],
	hResult = [-49],
	u.dwEventID = [1304],
	lpBuffer = [0x00762ef4]
	{
		usNumber = [5],
		fwType = [1],
		fwItemType = [0x0004],
		cUnitID = [LCU04],
		cCurrencyID = [USD],
		ulValues = [20],
		ulCashInCount = [17],
		ulCount = [0],
		ulMaximum = [0],
		usStatus = [0],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [3],
			lppNoteNumber = 
			{
				usNoteID = [5],
				ulCount = [0]
			}
			{
				usNoteID = [10],
				ulCount = [0]
			}
			{
				usNoteID = [15],
				ulCount = [0]
			}
		}
		usNumPhysicalCUs = [1],
		lppPhysical = 
		{
			lpPhysicalPositionName = [CassetteC],
			cUnitID = [CST_C],
			ulCashInCount = [17],
			ulCount = [0],
			ulMaximum = [0],
			usPStatus = [0],
			bHardwareSensor = [1],
			lpszExtra = [SerialNumber=CGJX501608]
			ulInitialCount = [2000],
			ulDispensedCount = [17],
			ulPresentedCount = [16],
			ulRetractedCount = [0],
			ulRejectCount = [1],
		}
		lpszExtra = NULL
	}
}";
      public const string WFS_SRVE_CIM_CASHUNITINFOCHANGED_2 = @"02434294967295016600363573370006COMMON0009FRAMEWORK00102022/12/19001217:48 58.0820011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[2], LogicalName[CashDispenser]}01664294967295015900363573380003CDM0007ACTIVEX00102022/12/19001217:48 58.0820011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x28BF7C08, lParam=0x39393CEC01594294967295015900363573390003CDM0009FRAMEWORK00102022/12/19001217:48 58.0820011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x119162C0, pCursor->pData->hApp=0x14FBB83801594294967295012600363573400003CIM0003SPI00102022/12/19001217:48 58.0820011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x20A3E598)01264294967295019600363573410003CDM0007ACTIVEX00102022/12/19001217:48 58.0820006XFSAPI0022CService::AsyncGetInfo0098WFSAsyncGetInfo(hService=18, dwCategory=301, lpQueryDetails=0x00000000, RequestID=49825) hResult=001964294967295016700363573420003CDM0009FRAMEWORK00102022/12/19001217:48 58.0830011INFORMATION0021CBaseService::GetInfo0063Srvc=301 ReqID=49823 Wnd=0x00010580 Cmd=301 TimeOut=300000 IO=101674294967295016800363573430003CDM0007ACTIVEX00102022/12/19001217:48 58.0830011INFORMATION0031CCdmService::HandleCashUnitInfo0056GetInfo-Result[CashUnitInfo][ReqID=49822] = {hResult[0]}01684294967295013700363573440003CDM0009FRAMEWORK00102022/12/19001217:48 58.0830011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295123000363573450003CIM0003SPI00102022/12/19001217:48 58.0830009XFS_EVENT0019SERVICE_EVENT[1304]1136WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x0001057c],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2022/12/19 17:48 58.079],
	hResult = [0],
	u.dwEventID = [1304],
	lpBuffer = [0x3945c4ec]
	{
		usNumber = [5],
		fwType = [1],
		fwItemType = [0x0004],
		cUnitID = [LCU04],
		cCurrencyID = [USD],
		ulValues = [20],
		ulCashInCount = [149],
		ulCount = [1319],
		ulMaximum = [0],
		usStatus = [0],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [3],
			lppNoteNumber = 
			{
				usNoteID = [5],
				ulCount = [1319]
			}
			{
				usNoteID = [10],
				ulCount = [0]
			}
			{
				usNoteID = [15],
				ulCount = [0]
			}
		}
		usNumPhysicalCUs = [1],
		lppPhysical = 
		{
			lpPhysicalPositionName = [CassetteC],
			cUnitID = [CST_C],
			ulCashInCount = [149],
			ulCount = [1319],
			ulMaximum = [0],
			usPStatus = [0],
			bHardwareSensor = [1],
			lpszExtra = [SerialNumber=CGJX444248]
			ulInitialCount = [1400],
			ulDispensedCount = [230],
			ulPresentedCount = [225],
			ulRetractedCount = [0],
			ulRejectCount = [2],
		}
		lpszExtra = NULL
	}
}";
      public const string WFS_SRVE_CIM_CASHUNITINFOCHANGED_3 = @"03444294967295014601822833180010CCIM-MIXED0002SP00102023/02/05001220:27 06.5020011INFORMATION0023CNHCCIMImpl::UpdateData0040IntermediateStacker status changed(1->0)01464294967295012701822833190010CCIM-MIXED0002SP00102023/02/05001220:27 06.5030011INFORMATION0023CNHCCIMImpl::UpdateData0021LCU[1] of CIM changed01274294967295015901822833200003IPM0009FRAMEWORK00102023/02/05001220:27 06.5030011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x134A582901594294967295013801822833210003IPM0009FRAMEWORK00102023/02/05001220:27 06.5030011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=16, wClass=1301384294967295015901822833220003IPM0009FRAMEWORK00102023/02/05001220:27 06.5040011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x134A5829, pCursor->pData->hApp=0x03DC48A101594294967295013801822833230003IPM0009FRAMEWORK00102023/02/05001220:27 06.5040011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015901822833240003IPM0009FRAMEWORK00102023/02/05001220:27 06.5040011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x03DC48A1, pCursor->pData->hApp=0x1847713901594294967295012601822833250003CIM0003SPI00102023/02/05001220:27 06.5040011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x21669300)01264294967295013801822833260003IPM0009FRAMEWORK00102023/02/05001220:27 06.5040011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295200201822833270003CIM0003SPI00102023/02/05001220:27 06.5040009XFS_EVENT0019SERVICE_EVENT[1304]1908WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x0001034c],
	RequestID = [0],
	hService = [13],
	tsTimestamp = [2023/02/05 20:27 06.503],
	hResult = [0],
	u.dwEventID = [1304],
	lpBuffer = [0x1ecd815c]
	{
		usNumber = [2],
		fwType = [2],
		fwItemType = [0x0001],
		cUnitID = [LCU01],
		cCurrencyID = [USD],
		ulValues = [0],
		ulCashInCount = [1250],
		ulCount = [1250],
		ulMaximum = [0],
		usStatus = [0],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [18],
			lppNoteNumber = 
			{
				usNoteID = [1],
				ulCount = [206]
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
				ulCount = [1]
			}
			{
				usNoteID = [11],
				ulCount = [1]
			}
			{
				usNoteID = [12],
				ulCount = [10]
			}
			{
				usNoteID = [13],
				ulCount = [137]
			}
			{
				usNoteID = [14],
				ulCount = [67]
			}
			{
				usNoteID = [15],
				ulCount = [579]
			}
			{
				usNoteID = [16],
				ulCount = [114]
			}
			{
				usNoteID = [17],
				ulCount = [135]
			}
			{
				usNoteID = [0],
				ulCount = [0]
			}
		}
		usNumPhysicalCUs = [1],
		lppPhysical = 
		{
			lpPhysicalPositionName = [CASHCASSETTE],
			cUnitID = [PCU02],
			ulCashInCount = [1250],
			ulCount = [1250],
			ulMaximum = [0],
			usPStatus = [0],
			bHardwareSensor = [1],
			lpszExtra = NULL
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
		}
		lpszExtra = NULL
	}
}";

      public const string WFS_SRVE_CIM_CASHUNITINFOCHANGED_4 = @"03444294967295014600481492840010CCIM-MIXED0002SP00102023/02/08001221:09 47.0340011INFORMATION0023CNHCCIMImpl::UpdateData0040IntermediateStacker status changed(1->0)01464294967295012700481492850010CCIM-MIXED0002SP00102023/02/08001221:09 47.0340011INFORMATION0023CNHCCIMImpl::UpdateData0021LCU[1] of CIM changed01274294967295015900481492860003IPM0009FRAMEWORK00102023/02/08001221:09 47.0350011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x00CF9F0A01594294967295013800481492870003IPM0009FRAMEWORK00102023/02/08001221:09 47.0350011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=16, wClass=1301384294967295015900481492880003IPM0009FRAMEWORK00102023/02/08001221:09 47.0350011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00CF9F0A, pCursor->pData->hApp=0x00CF9F7A01594294967295013800481492890003IPM0009FRAMEWORK00102023/02/08001221:09 47.0350011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015900481492900003IPM0009FRAMEWORK00102023/02/08001221:09 47.0360011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00CF9F7A, pCursor->pData->hApp=0x1F0DC9EA01594294967295012600481492910003CIM0003SPI00102023/02/08001221:09 47.0360011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x217278B0)01264294967295013800481492920003IPM0009FRAMEWORK00102023/02/08001221:09 47.0360011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295200400481492930003CIM0003SPI00102023/02/08001221:09 47.0360009XFS_EVENT0019SERVICE_EVENT[1304]1910WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x00010348],
	RequestID = [0],
	hService = [14],
	tsTimestamp = [2023/02/08 21:09 47.035],
	hResult = [0],
	u.dwEventID = [1304],
	lpBuffer = [0x29fe4f8c]
	{
		usNumber = [2],
		fwType = [2],
		fwItemType = [0x0001],
		cUnitID = [LCU01],
		cCurrencyID = [USD],
		ulValues = [0],
		ulCashInCount = [2126],
		ulCount = [2126],
		ulMaximum = [0],
		usStatus = [0],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [18],
			lppNoteNumber = 
			{
				usNoteID = [1],
				ulCount = [319]
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
				ulCount = [1]
			}
			{
				usNoteID = [11],
				ulCount = [1]
			}
			{
				usNoteID = [12],
				ulCount = [18]
			}
			{
				usNoteID = [13],
				ulCount = [150]
			}
			{
				usNoteID = [14],
				ulCount = [122]
			}
			{
				usNoteID = [15],
				ulCount = [1144]
			}
			{
				usNoteID = [16],
				ulCount = [196]
			}
			{
				usNoteID = [17],
				ulCount = [175]
			}
			{
				usNoteID = [0],
				ulCount = [0]
			}
		}
		usNumPhysicalCUs = [1],
		lppPhysical = 
		{
			lpPhysicalPositionName = [CASHCASSETTE],
			cUnitID = [PCU02],
			ulCashInCount = [2126],
			ulCount = [2126],
			ulMaximum = [0],
			usPStatus = [0],
			bHardwareSensor = [1],
			lpszExtra = NULL
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
		}
		lpszExtra = NULL
	}
}";
      public const string WFS_SRVE_CIM_CASHUNITINFOCHANGED_5 = @"02434294967295016802745892750003CDM0007ACTIVEX00102023/03/14001214:01 10.9420011INFORMATION0031CCdmService::HandleCashUnitInfo0056GetInfo-Result[CashUnitInfo][ReqID=18198] = {hResult[0]}01684294967295013802745892760003CDM0009FRAMEWORK00102023/03/14001214:01 10.9420011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015702745892770013CashDispenser0009FRAMEWORK00102023/03/14001214:01 10.9420007DEVCALL0021CBaseService::GetInfo0047HSERVICE[2] CATEGORY[301] IN BUFFER[0x00000000]01574294967295019602745892780003CDM0007ACTIVEX00102023/03/14001214:01 10.9420006XFSAPI0022CService::AsyncGetInfo0098WFSAsyncGetInfo(hService=19, dwCategory=301, lpQueryDetails=0x00000000, RequestID=18203) hResult=001964294967295016302745892790003CDM0007ACTIVEX00102023/03/14001214:01 10.9420011INFORMATION0024CContextMgr::SetComplete0058SetComplete(Waiting ReqID=[18199], Arrival ReqID=[18198]) 01634294967295015902745892800003CDM0009FRAMEWORK00102023/03/14001214:01 10.9420011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x06D9C50D, pCursor->pData->hApp=0x003DFBAD01594294967295012602745892810003CIM0003SPI00102023/03/14001214:01 10.9420011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x1AF77DC8)01264294967295019302745892820003CIM0007ACTIVEX00102023/03/14001214:01 10.9420006XFSAPI0023CContextMgr::MgrWndProc0094WFS_SERVICE_EVENT(RequestID=0, hService=18, hResult=0, dwCommandCode=1304 lpBuffer=0x089396D4)01934294967295019302745892830003CDM0007ACTIVEX00102023/03/14001214:01 10.9420011INFORMATION0024CContextMgr::SetComplete0088SetComplete(Waiting ReqID=[18198], Arrival ReqID=[18198]), Remove ReqID[18198] from List01934294967295013802745892840003CDM0009FRAMEWORK00102023/03/14001214:01 10.9420011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295122702745892850003CIM0003SPI00102023/03/14001214:01 10.9420009XFS_EVENT0019SERVICE_EVENT[1304]1133WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x0003006e],
	RequestID = [0],
	hService = [11],
	tsTimestamp = [2023/03/14 14:01 10.942],
	hResult = [0],
	u.dwEventID = [1304],
	lpBuffer = [0x349e73f4]
	{
		usNumber = [6],
		fwType = [1],
		fwItemType = [0x0004],
		cUnitID = [LCU05],
		cCurrencyID = [USD],
		ulValues = [100],
		ulCashInCount = [203],
		ulCount = [76],
		ulMaximum = [0],
		usStatus = [0],
		bAppLock = [0],
		lpNoteNumberList = 
		{
			usNumOfNoteNumbers = [3],
			lppNoteNumber = 
			{
				usNoteID = [7],
				ulCount = [76]
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
			ulCashInCount = [203],
			ulCount = [76],
			ulMaximum = [0],
			usPStatus = [0],
			bHardwareSensor = [1],
			lpszExtra = [SerilNumber=ÿÿÿÿÿÿÿÿÿÿ]
			ulInitialCount = [1476],
			ulDispensedCount = [1603],
			ulPresentedCount = [1581],
			ulRetractedCount = [0],
			ulRejectCount = [22],
		}
		lpszExtra = NULL
	}
}";

      public const string WFS_SRVE_CIM_ITEMSTAKEN_1 = @"
pCursor->pData->hApp=0x2AC211A201594294967295013700089113540003CDM0009FRAMEWORK00102023/01/24001213:22 45.4530011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295015900089113550003CDM0009FRAMEWORK00102023/01/24001213:22 45.4540011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC211A2, pCursor->pData->hApp=0x2AC2292201594294967295013800089113560003CDM0009FRAMEWORK00102023/01/24001213:22 45.4540011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015900089113570003CDM0009FRAMEWORK00102023/01/24001213:22 45.4540011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC22922, pCursor->pData->hApp=0x0BB0458201594294967295012600089113580003CIM0003SPI00102023/01/24001213:22 45.4540011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x00000000)01264294967295013700089113590003CDM0009FRAMEWORK00102023/01/24001213:22 45.4550011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295029400089113600003CIM0003SPI00102023/01/24001213:22 45.4550009XFS_EVENT0019SERVICE_EVENT[1307]0200WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2023/01/24 13:22 45.453],
	hResult = [0],
	u.dwEventID = [1307],
	lpBuffer = NULL
";

      public const string WFS_EXEE_CIM_INPUTREFUSE_1 = @"
03544294967295011400313252470003BRM0002SP00102022/10/30001211:47 03.9230011INFORMATION0024CCimService::OnCimCashIn0014Skewed Note(0)01144294967295019600313252480003CIM0007ACTIVEX00102022/10/30001211:47 03.9230006XFSAPI0023CContextMgr::MgrWndProc0097WFS_EXECUTE_EVENT(RequestID=1913, hService=13, hResult=0, dwCommandCode=1316 lpBuffer=0x1FB57004)01964294967295014500313252490003BRM0002SP00102022/10/30001211:47 03.9230006NORMAL0028CBrmCommand::SetErrorFlicker0046## Set ErrorFlicker Status : CONTINUOUS (0x80)01454294967295033100313252500003CIM0003SPI00102022/10/30001211:47 03.9230009XFS_EVENT0019EXECUTE_EVENT[1309]0237WFS_EXECUTE_EVENT, 
lpResult =
{
	hWnd = [0x00020222],
	RequestID = [1913],
	hService = [13],
	tsTimestamp = [2022/10/30 11:47 03.923],
	hResult = [0],
	u.dwEventID = [1309],
	lpBuffer = [0x1fb56d5c]
	{
		usReason = [8]
	}
";

      public const string WFS_SRVE_CIM_ITEMSPRESENTED_1 = @"
wClass=1301374294967295015900089107540003CDM0009FRAMEWORK00102023/01/24001213:22 39.1630011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC211A2, pCursor->pData->hApp=0x2AC2292201594294967295013800089107550003CDM0009FRAMEWORK00102023/01/24001213:22 39.1630011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015900089107560003CDM0009FRAMEWORK00102023/01/24001213:22 39.1630011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC22922, pCursor->pData->hApp=0x0BB0458201594294967295012600089107570003CIM0003SPI00102023/01/24001213:22 39.1630011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x2A737328)01264294967295013700089107580003CDM0009FRAMEWORK00102023/01/24001213:22 39.1630011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295038500089107590003CIM0003SPI00102023/01/24001213:22 39.1640009XFS_EVENT0019SERVICE_EVENT[1310]0291WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2023/01/24 13:22 39.162],
	hResult = [0],
	u.dwEventID = [1310],
	lpBuffer = [0x35d1d244]
	{
		wPosition = [4]
		wAdditionalBunches = [1]
		usBunchesRemaining = [0]
	}
";

      public const string WFS_SRVE_CIM_ITEMSINSERTED_1 = @"02944294967295015900088769120003CDM0009FRAMEWORK00102023/01/24001212:24 40.4750011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x0BB04582, pCursor->pData->hApp=0x0E770CEA01594294967295019300088769130003CIM0007ACTIVEX00102023/01/24001212:24 40.4760006XFSAPI0023CContextMgr::MgrWndProc0094WFS_SERVICE_EVENT(RequestID=0, hService=12, hResult=0, dwCommandCode=1311 lpBuffer=0x00000000)01934294967295013800088769140003CDM0009FRAMEWORK00102023/01/24001212:24 40.4760011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015900088769150003CIM0007ACTIVEX00102023/01/24001212:24 40.4760011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000415, wParam=0x2D402718, lParam=0x2520909401594294967295015900088769160003CDM0009FRAMEWORK00102023/01/24001212:24 40.4770011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x0E770CEA, pCursor->pData->hApp=0x0E770CEA01594294967295012600088769170003CIM0003SPI00102023/01/24001212:24 40.4770011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x00000000)01264294967295014000088769180003CIM0007ACTIVEX00102023/01/24001212:24 40.4770011INFORMATION0032CCimService::HandleItemsInserted0027[AddEvent]FireItemsInserted01404294967295013700088769190003CDM0009FRAMEWORK00102023/01/24001212:24 40.4770011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295029400088769200003CIM0003SPI00102023/01/24001212:24 40.4770009XFS_EVENT0019SERVICE_EVENT[1311]0200WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x000206f8],
	RequestID = [0],
	hService = [27],
	tsTimestamp = [2023/01/24 12:24 40.473],
	hResult = [0],
	u.dwEventID = [1311],
	lpBuffer = NULL
}";
      public const string WFS_SRVE_CIM_ITEMSINSERTED_2 = @"0110h: 31 38 5D 20 5B 43 43 49 4D 54 41 11084294967295017500164388980004CCIM0002SP00102022/12/07001216:58 17.6100006RESULT0033CNHDevCommWrapper::ExecuteCommand0070Encrypted Command('î':0xEE, 0x00) Return=[0] Result=[0] Error=[000000]01754294967295015500164388990004CCIM0002SP00102022/12/07001216:58 17.6110011INFORMATION0020CCCIMDev::_ActionLog0058## CountInfo ## BDM=0 ESC=0 RJT=0 RET=0 A6=0 CST1=0 CST2=001554294967295015900164389000003CIM0009FRAMEWORK00102022/12/07001216:58 17.6110011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x1942113601594294967295011700164389010004CCIM0002SP00102022/12/07001216:58 17.6110007COMMAND0033CNHDevCommWrapper::ExecuteCommand0011Sensor Read01174294967295013800164389020003CIM0009FRAMEWORK00102022/12/07001216:58 17.6120011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295014900164389030007nh CCIM0002SP00102022/12/07001216:58 17.6120004DATA0038CNHUsb6::SendControlDataWithEPUSBReset0038-> [8] 0000h: C0 50 00 00 00 00 40 00 01494294967295015900164389040003CIM0009FRAMEWORK00102022/12/07001216:58 17.6120011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x19421136, pCursor->pData->hApp=0x09B7D7BE01594294967295012600164389050003CIM0003SPI00102022/12/07001216:58 17.6120011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x00000000)01264294967295013800164389060003CIM0009FRAMEWORK00102022/12/07001216:58 17.6130011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=16, wClass=1301384294967295029400164389070003CIM0003SPI00102022/12/07001216:58 17.6130009XFS_EVENT0019SERVICE_EVENT[1311]0200WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x00020128],
	RequestID = [0],
	hService = [11],
	tsTimestamp = [2022/12/07 16:58 17.611],
	hResult = [0],
	u.dwEventID = [1311],
	lpBuffer = NULL
}";
      public const string WFS_SRVE_CIM_ITEMSINSERTED_3 = @"04694294967295021702725619240003BRM0002SP00102023/03/10001212:57 01.8960006NORMAL0015CBRM20::_Sensor0131##iCheckLog Start SENSOR_DATA Unit: BRM20, Message:E1C0F80000C000E4040C00080008000C004C1063606000200430610000010000 iCheckLog End##02174294967295015902725619250003CDM0009FRAMEWORK00102023/03/10001212:57 01.8970011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x1E7A9AB501594294967295013702725619260003CDM0009FRAMEWORK00102023/03/10001212:57 01.8970011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295015902725619270003CDM0009FRAMEWORK00102023/03/10001212:57 01.8980011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x1E7A9AB5, pCursor->pData->hApp=0x1E7A92BD01594294967295013802725619280003CDM0009FRAMEWORK00102023/03/10001212:57 01.8980011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015902725619290003CDM0009FRAMEWORK00102023/03/10001212:57 01.8980011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x1E7A92BD, pCursor->pData->hApp=0x06C4C8BD01594294967295012602725619300003CIM0003SPI00102023/03/10001212:57 01.8980011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x1B1A7338)01264294967295013802725619310003CDM0009FRAMEWORK00102023/03/10001212:57 01.8980011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295038702725619320003CIM0003SPI00102023/03/10001212:57 01.8980009XFS_EVENT0019SERVICE_EVENT[1311]0293WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x00030070],
	RequestID = [0],
	hService = [11],
	tsTimestamp = [2023/03/10 12:57 01.897],
	hResult = [0],
	u.dwEventID = [1311],
	lpBuffer = [0x2493225c]
	{
		wPosition = [516]
		wAdditionalBunches = [0]
		usBunchesRemaining = [0]
	}
}";

      public const string WFS_EXEE_CIM_NOTEERROR_1 = @"
}04014294967295019600089139090003CIM0007ACTIVEX00102023/01/24001213:23 32.6850006XFSAPI0023CContextMgr::MgrWndProc0097WFS_EXECUTE_EVENT(RequestID=4627, hService=12, hResult=0, dwCommandCode=1316 lpBuffer=0x35DD1804)01964294967295017100089139100003BRM0002SP00102023/01/24001213:23 32.6860011INFORMATION0024CCimService::OnCimCashIn0071[0] InputPosition=Not Empty, usRefusedCount=1, DetectOnCSM=1, hResult=001714294967295015900089139110003CIM0007ACTIVEX00102023/01/24001213:23 32.6860011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000414, wParam=0x00000000, lParam=0x25244E4C01594294967295012500089139120003BRM0002SP00102023/01/24001213:23 32.6860011INFORMATION0024CCimService::OnCimCashIn0025Some item is rejected!(1)01254294967295013100089139130003CIM0007ACTIVEX00102023/01/24001213:23 32.6860004INFO0032CCimService::HandleInfoAvailable0025usLevel=3, usNumOfItems=101314294967295033100089139140003CIM0003SPI00102023/01/24001213:23 32.6870009XFS_EVENT0019EXECUTE_EVENT[1312]0237WFS_EXECUTE_EVENT, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [4627],
	hService = [12],
	tsTimestamp = [2023/01/24 13:23 32.687],
	hResult = [0],
	u.dwEventID = [1309],
";

      public const string WFS_EXEE_CIM_NOTEERROR_2 = @"
03964294967295015900204979020003CIM0007ACTIVEX00102024/12/11001213:38 13.8940011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000415, wParam=0x0A7657C8, lParam=0x0A84858C01594294967295019100204979030003CIM0007ACTIVEX00102024/12/11001213:38 13.8950006XFSAPI0017CService::GetInfo0098WFSAsyncGetInfo(hService=11, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=1367) hResult=001914294967295016600204979040006COMMON0009FRAMEWORK00102024/12/11001213:38 13.8950011INFORMATION0028CServiceProvider::GetService0052service OK { HSERVICE [11], LogicalName [CashAcceptor]}01664294967295034000204979050012CashAcceptor0003SPI00102024/12/11001213:38 13.8950009XFS_EVENT0019EXECUTE_EVENT[1312]0237WFS_EXECUTE_EVENT, 
lpResult =
{
	hWnd = [0x00060394],
	RequestID = [1336],
	hService = [11],
	tsTimestamp = [2024/12/11 13:38 13.895],
	hResult = [0],
	u.dwEventID = [1312],
	lpBuffer = [0x22ae173c]
	{
		usReason = [3]
   }
}";

   public const string WFS_SRVE_CIM_MEDIADETECTED_1 = @"
03584294967295015900163264180003CIM0009FRAMEWORK00102022/12/07001215:58 02.2090011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x19421136, pCursor->pData->hApp=0x09B7D7BE01594294967295012600163264190003CIM0003SPI00102022/12/07001215:58 02.2090011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x00000000)01264294967295019300163264200003IPM0007ACTIVEX00102022/12/07001215:58 02.2090006XFSAPI0023CContextMgr::MgrWndProc0094WFS_SERVICE_EVENT(RequestID=0, hService=13, hResult=0, dwCommandCode=1610 lpBuffer=0x20F149A4)01934294967295013800163264210003CIM0009FRAMEWORK00102022/12/07001215:58 02.2090011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=16, wClass=1301384294967295029400163264220003CIM0003SPI00102022/12/07001215:58 02.2090009XFS_EVENT0019SERVICE_EVENT[1314]0200WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x00020128],
	RequestID = [0],
	hService = [11],
	tsTimestamp = [2022/12/07 15:58 02.209],
	hResult = [0],
	u.dwEventID = [1314],
	lpBuffer = NULL
";

      public const string WFS_CIM_INPUT_REFUSE_1 =
@"02514294967295033207615981240003CIM0003SPI00102023/03/30001216:04 44.1570009XFS_EVENT0019EXECUTE_EVENT[1309]0238WFS_EXECUTE_EVENT, 
lpResult =
{
	hWnd = [0x000201d8],
	RequestID = [15989],
	hService = [13],
	tsTimestamp = [2023/03/30 16:04 44.157],
	hResult = [0],
	u.dwEventID = [1309],
	lpBuffer = [0x2a0529b4]
	{
		usReason = [1]
	}
}";

      public const string WFS_CIM_INPUT_REFUSE_2 =
@"02514294967295033207615981240003CIM0003SPI00102023/03/30001216:04 44.1570009XFS_EVENT0019EXECUTE_EVENT[1309]0238WFS_EXECUTE_EVENT, 
lpResult =
{
	hWnd = [0x000201d8],
	RequestID = [15989],
	hService = [13],
	tsTimestamp = [2023/03/30 16:04 44.157],
	hResult = [0],
	u.dwEventID = [1309],
	lpBuffer = [0x2a0529b4]
	{
		usReason = [2]
	}
}";

      public const string WFS_CIM_INPUT_REFUSE_3 =
@"02514294967295033207615981240003CIM0003SPI00102023/03/30001216:04 44.1570009XFS_EVENT0019EXECUTE_EVENT[1309]0238WFS_EXECUTE_EVENT, 
lpResult =
{
	hWnd = [0x000201d8],
	RequestID = [15989],
	hService = [13],
	tsTimestamp = [2023/03/30 16:04 44.157],
	hResult = [0],
	u.dwEventID = [1309],
	lpBuffer = [0x2a0529b4]
	{
		usReason = [3]
	}
}";
      public const string WFS_CMD_CIM_START_EXCHANGE =
@"03654294967295019008198050270006COMMON0009FRAMEWORK00102023/02/27001207:49 10.6840011INFORMATION0025CCommandQueue::RemoveHead0079return (bResult[1]) pCommand{hService[11],..., ReqID[801], dwCommandCode[1310]}01904294967295018408198050280003CIM0007ACTIVEX00102023/02/27001207:49 10.6840006XFSAPI0021CService::ExecuteSync0087WFSAsyncExecute(hService=11, dwCommand=1310, lpCmdData=0x1CAAEE2C, ReqID=801) hResult=001844294967295012208198050290006COMMON0009FRAMEWORK00102023/02/27001207:49 10.6850011INFORMATION0015CProcessor::Run0021Execute{hService[11]}01224294967295015708198050300003CIM0007ACTIVEX00102023/02/27001207:49 10.6850011INFORMATION0028CService::WaitForAsyncCallEx0048WaitForAsyncCallEx RequestID=801, Timeout=30000001574294967295016608198050310006COMMON0009FRAMEWORK00102023/02/27001207:49 10.6860011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[11], LogicalName[CashAcceptor]}01664294967295015708198050320012CashAcceptor0009FRAMEWORK00102023/02/27001207:49 10.6870007DEVCALL0021CBaseService::Execute0048HSERVICE[11] COMMAND[1310] IN BUFFER[0x007ECBAC]01574294967295012808198050330004CCIM0002SP00102023/02/27001207:49 10.6900011INFORMATION0020CNHCCIMImpl::Execute0031call WFS_CMD_CIM_START_EXCHANGE01284294967295014708198050340004CCIM0002SP00102023/02/27001207:49 10.6910011INFORMATION0020CNHCCIMImpl::Execute0050dwCommand=[1310] hResult=[0] lpBuffer=[0x0082315c]01474294967295016908198050350012CashAcceptor0009FRAMEWORK00102023/02/27001207:49 10.6930007DEVRETN0021CBaseService::Execute0060HSERVICE[11] COMMAND[1310] HRESULT[0] OUT BUFFER[0x0082315C]01694294967295012208198050360006COMMON0009FRAMEWORK00102023/02/27001207:49 10.6940011INFORMATION0015CProcessor::Run0021---- Wait [4152] ----01224294967295418308198050370003CIM0003SPI00102023/02/27001207:49 10.6950009XFS_EVENT0013EXECUTE[1310]4095WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010348],
	RequestID = [801],
	hService = [11],
	tsTimestamp = [2023/02/27 07:49 10.694],
	hResult = [0],
	u.dwCommandCode = [1310],
	lpBuffer = [0x1c923ffc]
	{
		usCount = [2],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [USD],
			ulValues = [0],
			ulCashInCount = [57],
			ulCount = [1],
			ulMaximum = [50],
			usStatus = [1],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [52]
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
					ulCount = [1]
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
					ulCount = [4]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RETRACTCASSETTE,
				cUnitID = [PCU01],
				ulCashInCount = [57],
				ulCount = [58],
				ulMaximum = [0],
				usPStatus = [1],
				bHardwareSensor = [0],
				lpszExtra = NULL,
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usCDMType = [0],
			lpszCashUnitName = RETRACTCASSETTE,
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
		{
			usNumber = [2],
			fwType = [2],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [USD],
			ulValues = [0],
			ulCashInCount = [372],
			ulCount = [372],
			ulMaximum = [0],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [19]
				}
				{
					usNoteID = [2],
					ulCount = [1]
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
					ulCount = [13]
				}
				{
					usNoteID = [13],
					ulCount = [14]
				}
				{
					usNoteID = [14],
					ulCount = [6]
				}
				{
					usNoteID = [15],
					ulCount = [157]
				}
				{
					usNoteID = [16],
					ulCount = [80]
				}
				{
					usNoteID = [17],
					ulCount = [82]
				}
				{
					usNoteID = [0],
					ulCount = [0]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = CASHCASSETTE,
				cUnitID = [PCU02],
				ulCashInCount = [372],
				ulCount = [372],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = NULL,
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [0],
			}
			lpszExtra = NULL,
			usC
		......(More Data)......
	}
}";
      public const string WFS_CMD_CIM_END_EXCHANGE =
@"13344294967295012707649020350010CCIM-MIXED0002SP00102023/04/03001212:00 07.5360011INFORMATION0023CNHCCIMImpl::UpdateData0021LCU[1] of CIM changed01274294967295015307649020360010CCIM-MIXED0002SP00102023/04/03001212:00 07.5380011INFORMATION0020CNHCCIMImpl::Execute0050dwCommand=[1311] hResult=[0] lpBuffer=[0x00000000]01534294967295016907649020370012CashAcceptor0009FRAMEWORK00102023/04/03001212:00 07.5390007DEVRETN0021CBaseService::Execute0060HSERVICE[13] COMMAND[1311] HRESULT[0] OUT BUFFER[0x00000000]01694294967295012207649020380006COMMON0009FRAMEWORK00102023/04/03001212:00 07.5400011INFORMATION0015CProcessor::Run0021---- Wait [2560] ----01224294967295029807649020390003CIM0003SPI00102023/04/03001212:00 07.5400009XFS_EVENT0013EXECUTE[1311]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000201b6],
	RequestID = [2682],
	hService = [13],
	tsTimestamp = [2023/04/03 12:00 07.540],
	hResult = [0],
	u.dwCommandCode = [1311],
	lpBuffer = NULL
}";


   }
}
