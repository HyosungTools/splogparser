using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplTests
{
   public static class samples_lppcashin
   {
      public const string LPPCASHIN_TABLE_1 = @"12304294967295015800087404600012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.7530007DEVCALL0021CBaseService::GetInfo0049HSERVICE[27] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295016300087404610003CDM0007ACTIVEX00102023/01/24001200:59 14.7530011INFORMATION0024CContextMgr::SetComplete0058SetComplete(Waiting ReqID=[17631], Arrival ReqID=[17625]) 01634294967295017500087404620007MONIMNG0008XFSAGENT00102023/01/24001200:59 14.7540004INFO0026Agent::ProcessNotification0070Recv WFS_SERVICE_EVENT Event(ManagedName=[CashAcceptor] hService=[27])01754294967295016300087404630003CDM0007ACTIVEX00102023/01/24001200:59 14.7540011INFORMATION0024CContextMgr::SetComplete0058SetComplete(Waiting ReqID=[17628], Arrival ReqID=[17625]) 01634294967295017100087404640012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.7540007DEVRETN0021CBaseService::GetInfo0062HSERVICE[27] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x0155E134]01714294967295019300087404650003CDM0007ACTIVEX00102023/01/24001200:59 14.7540011INFORMATION0024CContextMgr::SetComplete0088SetComplete(Waiting ReqID=[17625], Arrival ReqID=[17625]), Remove ReqID[17625] from List01934294967295022400087404660006COMMON0009FRAMEWORK00102023/01/24001200:59 14.7550011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[17], HWND[0x00010638], REQUESTID[17627], dwCmdCode[301], dwTimeOut[300000]}02244294967295019300087404670003CIM0007ACTIVEX00102023/01/24001200:59 14.7550006XFSAPI0023CContextMgr::MgrWndProc0094WFS_SERVICE_EVENT(RequestID=0, hService=12, hResult=0, dwCommandCode=1304 lpBuffer=0x35B7E3FC)01934294967295016700087404680006COMMON0009FRAMEWORK00102023/01/24001200:59 14.7550011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[17], LogicalName[CashDispenser]}01674294967295297900087404690003CIM0003SPI00102023/01/24001200:59 14.7550009XFS_EVENT0013GETINFO[1303]2891WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000206a8],
	RequestID = [17634],
	hService = [27],
	tsTimestamp = [2023/01/24 00:59 14.755],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x004caa4c]
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
      public const string LPPCASHIN_TABLE_2 = @"14104294967295017500293933260007MONIMNG0008XFSAGENT00102022/10/29001210:41 53.4330004INFO0026Agent::ProcessNotification0070Recv WFS_SERVICE_EVENT Event(ManagedName=[CashAcceptor] hService=[32])01754294967295019100293933270003CIM0007ACTIVEX00102022/10/29001210:41 53.4340006XFSAPI0017CService::GetInfo0098WFSAsyncGetInfo(hService=13, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=2243) hResult=001914294967295016600293933280006COMMON0009FRAMEWORK00102022/10/29001210:41 53.4340011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[13], LogicalName[CashAcceptor]}01664294967295015900293933290003CDM0007ACTIVEX00102022/10/29001210:41 53.4340011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x15EDA238, lParam=0x1F24A57C01594294967295016800293933300003CDM0009FRAMEWORK00102022/10/29001210:41 53.4340011INFORMATION0021CBaseService::GetInfo0064Srvc=1303 ReqID=2240 Wnd=0x00030064 Cmd=1303 TimeOut=300000 IO=201684294967295015500293933310003CDM0007ACTIVEX00102022/10/29001210:41 53.4340011INFORMATION0025CCdmService::HandleStatus0049GetInfo-Result[Status][ReqID=2232] = {hResult[0]}01554294967295015800293933320012CashAcceptor0009FRAMEWORK00102022/10/29001210:41 53.4350007DEVCALL0021CBaseService::GetInfo0049HSERVICE[13] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295016100293933330003CDM0007ACTIVEX00102022/10/29001210:41 53.4350011INFORMATION0024CContextMgr::SetComplete0056SetComplete(Waiting ReqID=[2237], Arrival ReqID=[2232]) 01614294967295016100293933340003CDM0007ACTIVEX00102022/10/29001210:41 53.4350011INFORMATION0024CContextMgr::SetComplete0056SetComplete(Waiting ReqID=[2236], Arrival ReqID=[2232]) 01614294967295019000293933350003CDM0007ACTIVEX00102022/10/29001210:41 53.4350011INFORMATION0024CContextMgr::SetComplete0085SetComplete(Waiting ReqID=[2232], Arrival ReqID=[2232]), Remove ReqID[2232] from List01904294967295017100293933360012CashAcceptor0009FRAMEWORK00102022/10/29001210:41 53.4350007DEVRETN0021CBaseService::GetInfo0062HSERVICE[13] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x01722E74]01714294967295022300293933370006COMMON0009FRAMEWORK00102022/10/29001210:41 53.4350011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[20], HWND[0x0001068a], REQUESTID[2238], dwCmdCode[303], dwTimeOut[300000]}02234294967295016700293933380006COMMON0009FRAMEWORK00102022/10/29001210:41 53.4360011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[20], LogicalName[CashDispenser]}01674294967295296300293933390003CIM0003SPI00102022/10/29001210:41 53.4360009XFS_EVENT0013GETINFO[1303]2875WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00030064],
	RequestID = [2240],
	hService = [13],
	tsTimestamp = [2022/10/29 10:41 53.435],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x345fdc94]
	{
		usCount=7
		lppCashIn->
		{
			usNumber		1		2		3		4		5		6		7
			fwType			4		5		1		1		1		1		2
			fwItemType		0x0001		0x0001		0x0004		0x0004		0x0004		0x0004		0x0003
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06
			cCurrencyID		   		   		USD		USD		USD		USD		   
			ulValues		0		0		1		5		20		100		0
			ulCashInCount		88		2		639		229		1710		299		253
			ulCount			1		2		509		0		765		0		253
			ulMaximum		80		210		0		0		0		0		1400
			usStatus		4		0		0		4		0		4		4
			bAppLock		0		0		0		0		0		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		1		1		1		1		1		
			lpPhysicalPositionName	RetractCassette	RejectCassette	CassetteA	CassetteB	CassetteC	CassetteD	CassetteE	
			cUnitID			RTCST		RJCST		CST_A		CST_B		CST_C		CST_D		CST_E
			ulCashInCount		88		2		639		229		1710		299		253
			ulCount			1		2		509		0		765		0		253
			ulMaximum		80		210		0		0		0		0		1400
			usPStatus		4		0		0		4		0		4		4
			bHardwareSensor		1		1		1		1		1		1		1
			ulInitialCount		0		0		100		200		300		100		0
			ulDispensedCount	0		0		230		429		1245		399		0		
			ulPresentedCount	0		0		229		421		1236		395		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		2		1		0		1		0		0
			lpszExtra(PCU)		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=CGJX453030]		[SerialNumber=CGJX445170]		[SerialNumber=CGJX453110]		[SerialNumber=CGJX453364]		[SerialNumber=CJOG200790]

			lpNoteNumberList->
			[0]			[1]14		[1]1		[1]509		[3]0		[5]765		[7]0		[1]0		
			[1]			[2]0		[2]0				[8]0		[10]0		[12]0		[2]0		
			[2]			[3]0		[3]0				[13]0		[15]0		[17]0		[3]0		
			[3]			[4]0		[4]0										[4]0		
			[4]			[5]0		[5]0										[5]0		
			[5]			[6]0		[6]0										[6]0		
			[6]			[7]0		[7]0										[7]0		
			[7]			[8]0		[8]0										[8]0		
			[8]			[9]0		[9]0										[9]0		
			[9]			[10]0		[10]0										[10]0		
			[10]			[11]0		[11]0										[11]0		
			[11]			[12]0		[12]0										[12]0		
			[12]			[13]0		[13]0										[13]1		
			[13]			[14]0		[14]0										[14]62		
			[14]			[15]0		[15]1										[15]1		
			[15]			[16]0		[16]0										[16]189		
			[16]			[17]0		[17]0										[17]0		
			[17]			[0]74		[0]0										[0]0		

			LCU ETC
			usCDMType		0		0		0		0		0		0		0
			lpszCashUnitName	LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06		
			ulInitialCount		0		0		100		200		300		100		0
			ulDispensedCount	0		0		230		429		1245		399		0		
			ulPresentedCount	0		0		229		421		1236		395		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		2		1		0		1		0		0
			ulMinimum		0		0		0		0		0		0		0
			lpszExtra(LCU)		NULL		NULL		NULL		NULL		NULL		NULL		NULL
		}
	}
}";
      public const string LPPCASHIN_TABLE_3 = @"29634294967295015900314503540003CDM0007ACTIVEX00102022/10/30001218:04 37.0520011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x011D3940, lParam=0x077F891C01594294967295016800314503550003CDM0009FRAMEWORK00102022/10/30001218:04 37.0520011INFORMATION0021CBaseService::GetInfo0064Srvc=1303 ReqID=5934 Wnd=0x00020222 Cmd=1303 TimeOut=300000 IO=201684294967295015500314503560003CDM0007ACTIVEX00102022/10/30001218:04 37.0530011INFORMATION0025CCdmService::HandleStatus0049GetInfo-Result[Status][ReqID=5927] = {hResult[0]}01554294967295021600314503570007MONIMNG0008XFSAGENT00102022/10/30001218:04 37.0530004INFO0026Agent::ProcessNotification0111Recv WFS_GETINFO_COMPLETE Event(ManagedName=[CashAcceptor] hService=[32] dwCommandCode=[1303] dwEventID=[1303])02164294967295015800314503580012CashAcceptor0009FRAMEWORK00102022/10/30001218:04 37.0530007DEVCALL0021CBaseService::GetInfo0049HSERVICE[13] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295016100314503590003CDM0007ACTIVEX00102022/10/30001218:04 37.0530011INFORMATION0024CContextMgr::SetComplete0056SetComplete(Waiting ReqID=[5931], Arrival ReqID=[5927]) 01614294967295016100314503600003CDM0007ACTIVEX00102022/10/30001218:04 37.0530011INFORMATION0024CContextMgr::SetComplete0056SetComplete(Waiting ReqID=[5930], Arrival ReqID=[5927]) 01614294967295019000314503610003CDM0007ACTIVEX00102022/10/30001218:04 37.0540011INFORMATION0024CContextMgr::SetComplete0085SetComplete(Waiting ReqID=[5927], Arrival ReqID=[5927]), Remove ReqID[5927] from List01904294967295017100314503620012CashAcceptor0009FRAMEWORK00102022/10/30001218:04 37.0540007DEVRETN0021CBaseService::GetInfo0062HSERVICE[13] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x015D7D5C]01714294967295022200314503630006COMMON0009FRAMEWORK00102022/10/30001218:04 37.0540011INFORMATION0033CSpCmdDispatcher::DispatchCommand0103pService->GetInfo() {HSERVICE[33], HWND[0x00010702], REQUESTID[5932], dwCmdCode[303], dwTimeOut[60000]}02224294967295016700314503640006COMMON0009FRAMEWORK00102022/10/30001218:04 37.0540011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[33], LogicalName[CashDispenser]}01674294967295296300314503650003CIM0003SPI00102022/10/30001218:04 37.0540009XFS_EVENT0013GETINFO[1303]2875WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00020222],
	RequestID = [5934],
	hService = [13],
	tsTimestamp = [2022/10/30 18:04 37.054],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x1f989ac4]
	{
		usCount=7
		lppCashIn->
		{
			usNumber		1		2		3		4		5		6		7
			fwType			4		5		1		1		1		1		2
			fwItemType		0x0001		0x0001		0x0004		0x0004		0x0004		0x0004		0x0003
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06
			cCurrencyID		   		   		USD		USD		USD		USD		   
			ulValues		0		0		1		5		20		100		0
			ulCashInCount		2		0		392		231		2597		357		541
			ulCount			1		0		349		3		1640		5		541
			ulMaximum		80		210		0		0		0		0		1400
			usStatus		4		4		0		0		0		0		0
			bAppLock		0		0		0		0		0		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		1		1		1		1		1		
			lpPhysicalPositionName	RetractCassette	RejectCassette	CassetteA	CassetteB	CassetteC	CassetteD	CassetteE	
			cUnitID			RTCST		RJCST		CST_A		CST_B		CST_C		CST_D		CST_E
			ulCashInCount		2		0		392		231		2597		357		541
			ulCount			1		0		349		3		1640		5		541
			ulMaximum		80		210		0		0		0		0		1400
			usPStatus		4		4		0		0		0		0		0
			bHardwareSensor		1		1		1		1		1		1		1
			ulInitialCount		0		0		100		200		300		100		0
			ulDispensedCount	0		0		143		428		1257		452		0		
			ulPresentedCount	0		0		142		420		1246		448		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		0		0		0		0		0		0
			lpszExtra(PCU)		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=NULLSERIALNUMBER]		[SerialNumber=CGJX450412]		[SerialNumber=CGJX450720]		[SerialNumber=CGJX450536]		[SerialNumber=CGJX451962]		[SerialNumber=CJOG200848]

			lpNoteNumberList->
			[0]			[1]0		[1]0		[1]349		[3]3		[5]1640		[7]5		[1]0		
			[1]			[2]0		[2]0				[8]0		[10]0		[12]0		[2]0		
			[2]			[3]0		[3]0				[13]0		[15]0		[17]0		[3]0		
			[3]			[4]0		[4]0										[4]0		
			[4]			[5]0		[5]0										[5]0		
			[5]			[6]0		[6]0										[6]0		
			[6]			[7]0		[7]0										[7]0		
			[7]			[8]0		[8]0										[8]0		
			[8]			[9]0		[9]0										[9]0		
			[9]			[10]0		[10]0										[10]0		
			[10]			[11]0		[11]0										[11]1		
			[11]			[12]0		[12]0										[12]0		
			[12]			[13]0		[13]0										[13]1		
			[13]			[14]0		[14]0										[14]201		
			[14]			[15]2		[15]0										[15]0		
			[15]			[16]0		[16]0										[16]338		
			[16]			[17]0		[17]0										[17]0		
			[17]			[0]0		[0]0										[0]0		

			LCU ETC
			usCDMType		0		0		0		0		0		0		0
			lpszCashUnitName	LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06		
			ulInitialCount		0		0		100		200		300		100		0
			ulDispensedCount	0		0		143		428		1257		452		0		
			ulPresentedCount	0		0		142		420		1246		448		0		
			ulRetractedCount	0		0		0		0		0		0		0		
			ulRejectCount		0		0		0		0		0		0		0
			ulMinimum		0		0		0		0		0		0		0
			lpszExtra(LCU)		NULL		NULL		NULL		NULL		NULL		NULL		NULL
		}
	}
}";
      public const string LPPCASHIN_TABLE_4 = @"02414294967295022100507196750006COMMON0009FRAMEWORK00102022/11/17001209:58 43.3260011INFORMATION0033CSpCmdDispatcher::DispatchCommand0102pService->GetInfo() {HSERVICE[108], HWND[0x0005077c], REQUESTID[56721], dwCmdCode[1303], dwTimeOut[0]}02214294967295016800507196760006COMMON0009FRAMEWORK00102022/11/17001209:58 43.3260011INFORMATION0028CServiceProvider::GetService0054service OK {HSERVICE[108], LogicalName[CashAcceptor1]}01684294967295016400507196770003CIM0009FRAMEWORK00102022/11/17001209:58 43.3260011INFORMATION0021CBaseService::GetInfo0060Srvc=1303 ReqID=56721 Wnd=0x0005077C Cmd=1303 TimeOut=0 IO=701644294967295016000507196780013CashAcceptor10009FRAMEWORK00102022/11/17001209:58 43.3260007DEVCALL0021CBaseService::GetInfo0050HSERVICE[108] CATEGORY[1303] IN BUFFER[0x00000000]01604294967295017300507196790013CashAcceptor10009FRAMEWORK00102022/11/17001209:58 43.3260007DEVRETN0021CBaseService::GetInfo0063HSERVICE[108] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x00608F3C]01734294967295174300507196800013CashAcceptor10003SPI00102022/11/17001209:58 43.3260009XFS_EVENT0013GETINFO[1303]1645WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x0005077c],
	RequestID = [56721],
	hService = [108],
	tsTimestamp = [2022/11/17 09:58 43.326],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x285ce77c]
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
			ulCashInCount		0		0
			ulCount			0		0
			ulMaximum		50		0
			usStatus		0		0
			bAppLock		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		
			lpPhysicalPositionName	RETRACTCASSETTE	CASHCASSETTE	
			cUnitID			PCU01		PCU02
			ulCashInCount		0		0
			ulCount			0		0
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
			[11]			[12]0		[12]0		
			[12]			[13]0		[13]0		
			[13]			[14]0		[14]0		
			[14]			[15]0		[15]0		
			[15]			[16]0		[16]0		
			[16]			[17]0		[17]0		
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
      public const string LPPCASHIN_TABLE_5 = @"02434294967295022300375677280006COMMON0009FRAMEWORK00102023/01/19001203:12 01.5760011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[19], HWND[0x000e0046], REQUESTID[134], dwCmdCode[1303], dwTimeOut[300000]}02234294967295020200375677290003CIM0007ACTIVEX00102023/01/19001203:12 01.5760006XFSAPI0022CService::AsyncGetInfo0104WFSAsyncGetInfo(hService=19, WFS_INF_CIM_CASH_UNIT_INFO, lpQueryDetails=0x00000000, ReqID=134) hResult=002024294967295016600375677300006COMMON0009FRAMEWORK00102023/01/19001203:12 01.5760011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[19], LogicalName[CashAcceptor]}01664294967295016700375677310003IPM0009FRAMEWORK00102023/01/19001203:12 01.5760011INFORMATION0021CBaseService::GetInfo0063Srvc=1303 ReqID=134 Wnd=0x000E0046 Cmd=1303 TimeOut=300000 IO=301674294967295015800375677320012CashAcceptor0009FRAMEWORK00102023/01/19001203:12 01.5760007DEVCALL0021CBaseService::GetInfo0049HSERVICE[19] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295017100375677330012CashAcceptor0009FRAMEWORK00102023/01/19001203:12 01.5760007DEVRETN0021CBaseService::GetInfo0062HSERVICE[19] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x04ABC814]01714294967295174700375677340003CIM0003SPI00102023/01/19001203:12 01.5760009XFS_EVENT0013GETINFO[1303]1659WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000e0046],
	RequestID = [134],
	hService = [19],
	tsTimestamp = [2023/01/19 03:12 01.576],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x052827e4]
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
			ulCashInCount		0		537
			ulCount			6		537
			ulMaximum		126		0
			usStatus		0		0
			bAppLock		0		0

			lppPhysical->
			usNumPhysicalCUs	1		1		
			lpPhysicalPositionName	RETRACTCASSETTE	CASHCASSETTE	
			cUnitID			PCU01		PCU02
			ulCashInCount		0		537
			ulCount			0		537
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
			[0]			[1]0		[1]173		
			[1]			[2]0		[2]0		
			[2]			[3]0		[3]1		
			[3]			[4]0		[4]0		
			[4]			[5]0		[5]1		
			[5]			[6]0		[6]0		
			[6]			[7]0		[7]0		
			[7]			[8]0		[8]0		
			[8]			[9]0		[9]0		
			[9]			[10]0		[10]0		
			[10]			[11]0		[11]0		
			[11]			[12]0		[12]2		
			[12]			[13]0		[13]53		
			[13]			[14]0		[14]18		
			[14]			[15]0		[15]192		
			[15]			[16]0		[16]30		
			[16]			[17]0		[17]67		
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


      public const string LPPCASHIN_LIST_1 = @"02404294967295022100410206000006COMMON0009FRAMEWORK00102022/12/08001216:21 48.9210011INFORMATION0033CSpCmdDispatcher::DispatchCommand0102pService->GetInfo() {HSERVICE[2], HWND[0x00040190], REQUESTID[965], dwCmdCode[303], dwTimeOut[300000]}02214294967295019300410206010003CDM0007ACTIVEX00102022/12/08001216:21 48.9210006XFSAPI0022CService::AsyncGetInfo0095WFSAsyncGetInfo(hService=2, dwCategory=301, lpQueryDetails=0x00000000, RequestID=966) hResult=001934294967295016600410206020006COMMON0009FRAMEWORK00102022/12/08001216:21 48.9210011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[2], LogicalName[CashDispenser]}01664294967295418300410206030003CIM0003SPI00102022/12/08001216:21 48.9210009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201fc],
	RequestID = [963],
	hService = [11],
	tsTimestamp = [2022/12/08 16:21 48.921],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x0e0dc92c]
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
					ulCount = [101]
				}
				{
					usNoteID = [2],
					ulCount = [102]
				}
				{
					usNoteID = [3],
					ulCount = [103]
				}
				{
					usNoteID = [4],
					ulCount = [104]
				}
				{
					usNoteID = [5],
					ulCount = [105]
				}
				{
					usNoteID = [6],
					ulCount = [106]
				}
				{
					usNoteID = [7],
					ulCount = [107]
				}
				{
					usNoteID = [8],
					ulCount = [108]
				}
				{
					usNoteID = [9],
					ulCount = [109]
				}
				{
					usNoteID = [10],
					ulCount = [110]
				}
				{
					usNoteID = [11],
					ulCount = [111]
				}
				{
					usNoteID = [12],
					ulCount = [112]
				}
				{
					usNoteID = [13],
					ulCount = [113]
				}
				{
					usNoteID = [14],
					ulCount = [114]
				}
				{
					usNoteID = [15],
					ulCount = [115]
				}
				{
					usNoteID = [16],
					ulCount = [116]
				}
				{
					usNoteID = [17],
					ulCount = [117]
				}
				{
					usNoteID = [18],
					ulCount = [118]
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
					ulCount = [111]
				}
				{
					usNoteID = [2],
					ulCount = [222]
				}
				{
					usNoteID = [3],
					ulCount = [333]
				}
				{
					usNoteID = [4],
					ulCount = [444]
				}
				{
					usNoteID = [5],
					ulCount = [555]
				}
				{
					usNoteID = [6],
					ulCount = [666]
				}
				{
					usNoteID = [7],
					ulCount = [777]
				}
				{
					usNoteID = [8],
					ulCount = [888]
				}
				{
					usNoteID = [9],
					ulCount = [999]
				}
				{
					usNoteID = [10],
					ulCount = [1010]
				}
				{
					usNoteID = [11],
					ulCount = [111]
				}
				{
					usNoteID = [12],
					ulCount = [1212]
				}
				{
					usNoteID = [13],
					ulCount = [1313]
				}
				{
					usNoteID = [14],
					ulCount = [1414]
				}
				{
					usNoteID = [15],
					ulCount = [1515]
				}
				{
					usNoteID = [16],
					ulCount = [1616]
				}
				{
					usNoteID = [17],
					ulCount = [1717]
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
      public const string LPPCASHIN_LIST_2 = @"02414294967295418300526282080003CIM0003SPI00102022/12/20001215:03 06.1070009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00010060],
	RequestID = [1570],
	hService = [17],
	tsTimestamp = [2022/12/20 15:03 06.107],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x074398a4]
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
			ulMaximum = [80],
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
					ulCount = [1717]
				}
				{
					usNoteID = [16],
					ulCount = [1616]
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
				ulMaximum = [80],
				usPStatus = [4],
				bHardwareSensor = [1],
				lpszExtra = [SerialNumber=NULLSERIALNUMBER],
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
			fwItemType = [0x0003],
			cUnitID = [LCU01],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [6],
			ulCount = [6],
			ulMaximum = [210],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [99]
				}
				{
					usNoteID = [2],
					ulCount = [98]
				}
				{
					usNoteID = [3],
					ulCount = [97]
				}
				{
					usNoteID = [4],
					ulCount = [96]
				}
				{
					usNoteID = [5],
					ulCount = [95]
				}
				{
					usNoteID = [6],
					ulCount = [94]
				}
				{
					usNoteID = [7],
					ulCount = [93]
				}
				{
					usNoteID = [8],
					ulCount = [92]
				}
				{
					usNoteID = [9],
					ulCount = [91]
				}
				{
					usNoteID = [10],
					ulCount = [90]
				}
				{
					usNoteID = [11],
					ulCount = [89]
				}
				{
					usNoteID = [12],
					ulCount = [88]
				}
				{
					usNoteID = [13],
					ulCount = [187]
				}
				{
					usNoteID = [14],
					ulCount = [86]
				}
				{
					usNoteID = [15],
					ulCount = [85]
				}
				{
					usNoteID = [16],
					ulCount = [84]
				}
				{
					usNoteID = [17],
					ulCount = [83]
				}
				{
					usNoteID = [0],
					ulCount = [82]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RejectCassette,
				cUnitID = [RJCST],
				ulCashInCount = [6],
				ulCount = [6],
				ulMaximum = [210],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = [SerialNumber=CFHO202608],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [6],
			}

		......(More Data)......
	}
}";
      public const string LPPCASHIN_LIST_3 = @"02434294967295015800533262580012CashAcceptor0009FRAMEWORK00102022/12/21001217:53 12.1910007DEVCALL0021CBaseService::GetInfo0049HSERVICE[17] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295016700533262590003CDM0007ACTIVEX00102022/12/21001217:53 12.1910011INFORMATION0031CCdmService::HandleCashUnitInfo0055GetInfo-Result[CashUnitInfo][ReqID=4538] = {hResult[0]}01674294967295018600533262600007MONIMNG0008XFSAGENT00102022/12/21001217:53 12.1910004INFO0015Agent::FireTrap0092call WFSAsyncGetInfo(ManagedName=[CashAcceptor] hService=[31] dwCategory=[1303] hResult=[0])01864294967295016100533262610003CDM0007ACTIVEX00102022/12/21001217:53 12.1910011INFORMATION0024CContextMgr::SetComplete0056SetComplete(Waiting ReqID=[4539], Arrival ReqID=[4538]) 01614294967295017100533262620012CashAcceptor0009FRAMEWORK00102022/12/21001217:53 12.1910007DEVRETN0021CBaseService::GetInfo0062HSERVICE[17] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x017137D4]01714294967295019000533262630003CDM0007ACTIVEX00102022/12/21001217:53 12.1910011INFORMATION0024CContextMgr::SetComplete0085SetComplete(Waiting ReqID=[4538], Arrival ReqID=[4538]), Remove ReqID[4538] from List01904294967295022300533262640006COMMON0009FRAMEWORK00102022/12/21001217:53 12.1920011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[18], HWND[0x00010066], REQUESTID[4541], dwCmdCode[301], dwTimeOut[300000]}02234294967295418300533262650003CIM0003SPI00102022/12/21001217:53 12.1920009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00010060],
	RequestID = [4547],
	hService = [17],
	tsTimestamp = [2022/12/21 17:53 12.192],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x00c04c6c]
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
			ulMaximum = [80],
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
				ulMaximum = [80],
				usPStatus = [4],
				bHardwareSensor = [1],
				lpszExtra = [SerialNumber=NULLSERIALNUMBER],
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
			fwItemType = [0x0003],
			cUnitID = [LCU01],
			cCurrencyID = [   ],
			ulValues = [0],
			ulCashInCount = [6],
			ulCount = [6],
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
					ulCount = [88]
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
					ulCount = [1]
				}
				{
					usNoteID = [13],
					ulCount = [1]
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
					ulCount = [1]
				}
				{
					usNoteID = [0],
					ulCount = [3]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RejectCassette,
				cUnitID = [RJCST],
				ulCashInCount = [6],
				ulCount = [6],
				ulMaximum = [210],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = [SerialNumber=CFHO202608],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [6],
			}

		......(More Data)......
	}
}";
      public const string LPPCASHIN_LIST_4 = @"25814294967295014901833059800007nh CCIM0002SP00102023/02/07001219:21 22.8040004DATA0038CNHUsb6::SendControlDataWithEPUSBReset0038-> [8] 0000h: C0 41 FF 40 00 00 40 00 01494294967295012201833059810006COMMON0009FRAMEWORK00102023/02/07001219:21 22.8040011INFORMATION0015CProcessor::Run0021---- Wait [5664] ----01224294967295234501833059820003CIM0003SPI00102023/02/07001219:21 22.8040009XFS_EVENT0013EXECUTE[1303]2257WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010356],
	RequestID = [12514],
	hService = [13],
	tsTimestamp = [2023/02/07 19:21 22.803],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x0e777a24]
	{
		usCount = [1],
		lppCashIn =
		{
			usNumber = [2],
			fwType = [2],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [USD],
			ulValues = [0],
			ulCashInCount = [1],
			ulCount = [1],
			ulMaximum = [0],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [11]
				}
				{
					usNoteID = [2],
					ulCount = [22]
				}
				{
					usNoteID = [3],
					ulCount = [33]
				}
				{
					usNoteID = [4],
					ulCount = [44]
				}
				{
					usNoteID = [5],
					ulCount = [55]
				}
				{
					usNoteID = [6],
					ulCount = [66]
				}
				{
					usNoteID = [7],
					ulCount = [77]
				}
				{
					usNoteID = [8],
					ulCount = [88]
				}
				{
					usNoteID = [9],
					ulCount = [99]
				}
				{
					usNoteID = [10],
					ulCount = [100]
				}
				{
					usNoteID = [11],
					ulCount = [101]
				}
				{
					usNoteID = [12],
					ulCount = [102]
				}
				{
					usNoteID = [13],
					ulCount = [103]
				}
				{
					usNoteID = [14],
					ulCount = [104]
				}
				{
					usNoteID = [15],
					ulCount = [105]
				}
				{
					usNoteID = [16],
					ulCount = [106]
				}
				{
					usNoteID = [17],
					ulCount = [107]
				}
				{
					usNoteID = [0],
					ulCount = [010]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = CASHCASSETTE,
				cUnitID = [PCU02],
				ulCashInCount = [1],
				ulCount = [1],
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
			usCDMType = [0],
			lpszCashUnitName = CASHCASSETTE,
			ulInitialCount = [0],
			ulDispensedCount = [0],
			ulPresentedCount = [0],
			ulRetractedCount = [0],
			ulRejectCount = [0],
			ulMinimum = [0]
		}
	}
}";
      public const string LPPCASHIN_LIST_5 = @"54.9640005EVENT0041CCdmService::SetShutterStatusChangedEvent0042FireShutterStatusChanged{NewValue[CLOSED]}01584294967295012302742019490003BRM0002SP00102023/03/14001211:50 54.9640006NORMAL0027CBRM20::UpdateCashUnitCount0025ulCount=48, ulOldCount=4801234294967295022302742019500006COMMON0009FRAMEWORK00102023/03/14001211:50 54.9640011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[19], HWND[0x000105c2], REQUESTID[6909], dwCmdCode[303], dwTimeOut[300000]}02234294967295418302742019510003CIM0003SPI00102023/03/14001211:50 54.9640009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x0003006e],
	RequestID = [6908],
	hService = [11],
	tsTimestamp = [2023/03/14 11:50 54.964],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x23d66eb4]
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
			ulCashInCount = [48],
			ulCount = [48],
			ulMaximum = [0],
			usStatus = [0],
			bAppLock = [0],
			lpNoteNumberList = 
			{
				usNumOfNoteNumbers = [18],
				lppNoteNumber = 
				{
					usNoteID = [1],
					ulCount = [999]
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
					ulCount = [3]
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
					ulCount = [11]
				}
				{
					usNoteID = [13],
					ulCount = [7]
				}
				{
					usNoteID = [14],
					ulCount = [0]
				}
				{
					usNoteID = [15],
					ulCount = [1]
				}
				{
					usNoteID = [16],
					ulCount = [0]
				}
				{
					usNoteID = [17],
					ulCount = [7]
				}
				{
					usNoteID = [0],
					ulCount = [19]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RejectCassette,
				cUnitID = [RJCST],
				ulCashInCount = [48],
				ulCount = [48],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = [SerilNumber=NULLSERIALNUMBER],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [4
		......(More Data)......
	}
}";

   }
}
