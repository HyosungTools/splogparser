﻿using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class IdentifyLineTest
   {

      [TestMethod]
      public void Identify_None()
      {
         string logLine = @"
46.4570011INFORMATION0010WFPGetInfo0030HSERVICE=21, 
SrvcVersion=3(03)01204294967295023600087674410003SIU0003SPI00102023/01/24001208:50 46.4570007XFS_CMD0012GETINFO
[801]0151hResult[0] = WFPGetInfo(
hService = [21],
dwCategory = [801],
dwTimeOut = [0],
hWnd = [0x00010666],
ReqID = [850],
lpQueryDetails = NULL
)
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.None);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_STATUS()
      {
         string logLine = @"
06354294967295022400146871410006COMMON0009FRAMEWORK00102022/12/18001221:09 08.8210011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[18], HWND[0x00060046], REQUESTID[63863], dwCmdCode[303], dwTimeOut[300000]}02244294967295141100146871420003CDM0003SPI00102022/12/18001221:09 08.8210009XFS_EVENT0012GETINFO[301]1324WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [63862],
	hService = [2],
	tsTimestamp = [2022/12/18 21:09 08.820],
	hResult = [0],
	u.dwCommandCode = [301],
	lpBuffer = [0x33374804]
	{
";
         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_INF_CDM_STATUS);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_CASH_UNIT_INFO()
      {
         string logLine = @"
02434294967295019500146871070003CDM0007ACTIVEX00102022/12/18001221:09 08.8160006XFSAPI0022CService::AsyncGetInfo0097WFSAsyncGetInfo(hService=2, dwCategory=301, lpQueryDetails=0x00000000, RequestID=63862) hResult=001954294967295014600146871080003CDM0009FRAMEWORK00102022/12/18001221:09 08.8160011INFORMATION0034CBaseService::PostSystemErrorEvent0029Hardware Error Message Report01464294967295019600146871090003CDM0007ACTIVEX00102022/12/18001221:09 08.8160006XFSAPI0022CService::AsyncGetInfo0098WFSAsyncGetInfo(hService=18, dwCategory=303, lpQueryDetails=0x00000000, RequestID=63863) hResult=001964294967295017000146871100013CashDispenser0009FRAMEWORK00102022/12/18001221:09 08.8160007DEVRETN0021CBaseService::GetInfo0060HSERVICE[2] CATEGORY[303] HRESULT[0], OUT BUFFER[0x1F3219EC]01704294967295018900146871110003CIM0007ACTIVEX00102022/12/18001221:09 08.8160006XFSAPI0023CContextMgr::MgrWndProc0090WFS_SYSTEM_EVENT(RequestID=0, hService=12, hResult=0, dwCommandCode=4 lpBuffer=0x33B1C64C)01894294967295015900146871120003CDM0009FRAMEWORK00102022/12/18001221:09 08.8160011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x0755C10D01594294967295012400146871130006COMMON0003SPI00102022/12/18001221:09 08.8170011INFORMATION0010WFPGetInfo0034HSERVICE=18, SrvcVersion=2563(A03)01244294967295022300146871140006COMMON0009FRAMEWORK00102022/12/18001221:09 08.8170011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[2], HWND[0x00010574], REQUESTID[63862], dwCmdCode[301], dwTimeOut[300000]}02234294967295161200146871150003CDM0003SPI00102022/12/18001221:09 08.8170009XFS_EVENT0012GETINFO[303]1525WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [63861],
	hService = [2],
	tsTimestamp = [2022/12/18 21:09 08.817],
	hResult = [0],
	u.dwCommandCode = [303],
	lpBuffer = [0x33b3493c]
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_INF_CDM_CASH_UNIT_INFO);
      }

      [TestMethod]
      public void Identify_WFS_CMD_CDM_DISPENSE()
      {
         string logLine = @"
05024294967295016800149510870013CashDispenser0009FRAMEWORK00102022/12/18001223:15 18.3220007DEVRETN0021CBaseService::Execute0058HSERVICE[2] COMMAND[302] HRESULT[0] OUT BUFFER[0x01518D44]01684294967295012200149510880006COMMON0009FRAMEWORK00102022/12/18001223:15 18.3220011INFORMATION0015CProcessor::Run0021---- Wait [4900] ----01224294967295043000149510890003CDM0003SPI00102022/12/18001223:15 18.3220009XFS_EVENT0012EXECUTE[302]0343WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [68773],
	hService = [2],
	tsTimestamp = [2022/12/18 23:15 18.322],
	hResult = [0],
	u.dwCommandCode = [302],
	lpBuffer = [0x1e8eda64]
	{
		cCurrencyID = [USD],
		ulAmount = [300],
		usCount = [6],
		lpulValues = [0, 0, 0, 0, 5, 2],
		ulCashBox = [0]
	}
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CDM_DISPENSE);
      }
      [TestMethod]
      public void Identify_WFS_CMD_CDM_PRESENT()
      {
         string logLine = @"
05104294967295016800149520220013CashDispenser0009FRAMEWORK00102022/12/18001223:15 21.6770007DEVRETN0021CBaseService::Execute0058HSERVICE[2] COMMAND[303] HRESULT[0] OUT BUFFER[0x00000000]01684294967295016600149520230006COMMON0009FRAMEWORK00102022/12/18001223:15 21.6770011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[2], LogicalName[CashDispenser]}01664294967295012500149520240003BRM0002SP00102022/12/18001223:15 21.6770011INFORMATION0025CBrmCommand::UpdateStatus0024IdleStatus changed[0->1]01254294967295015900149520250003CDM0007ACTIVEX00102022/12/18001223:15 21.6770011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x08FFC540, lParam=0x079577BC01594294967295012200149520260006COMMON0009FRAMEWORK00102022/12/18001223:15 21.6770011INFORMATION0015CProcessor::Run0021---- Wait [4900] ----01224294967295029600149520270003CDM0003SPI00102022/12/18001223:15 21.6770009XFS_EVENT0012EXECUTE[303]0209WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [68798],
	hService = [2],
	tsTimestamp = [2022/12/18 23:15 21.677],
	hResult = [0],
	u.dwCommandCode = [303],
	lpBuffer = NULL
}
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CDM_PRESENT);
      }
      [TestMethod]
      public void Identify_WFS_CMD_CDM_REJECT()
      {
         string logLine = @"
05024294967295017100131319720013CashDispenser0009FRAMEWORK00102022/12/18001213:46 43.4140007DEVRETN0021CBaseService::Execute0061HSERVICE[2] COMMAND[304] HRESULT[-316] OUT BUFFER[0x00000000]01714294967295012200131319730006COMMON0009FRAMEWORK00102022/12/18001213:46 43.4150011INFORMATION0015CProcessor::Run0021---- Wait [4900] ----01224294967295029900131319740003CDM0003SPI00102022/12/18001213:46 43.4150009XFS_EVENT0012EXECUTE[304]0212WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [33194],
	hService = [2],
	tsTimestamp = [2022/12/18 13:46 43.415],
	hResult = [-316],
	u.dwCommandCode = [304],
	lpBuffer = NULL
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CDM_REJECT);
      }

      [TestMethod]
      public void Identify_WFS_CMD_CMD_RETRACT()
      {
         string logLine = @"
05024294967295017100117172650013CashDispenser0009FRAMEWORK00102022/12/18001206:44 37.2100007DEVRETN0021CBaseService::Execute0061HSERVICE[2] COMMAND[305] HRESULT[-316] OUT BUFFER[0x00000000]01714294967295012200117172660006COMMON0009FRAMEWORK00102022/12/18001206:44 37.2110011INFORMATION0015CProcessor::Run0021---- Wait [4900] ----01224294967295029800117172670003CDM0003SPI00102022/12/18001206:44 37.2110009XFS_EVENT0012EXECUTE[305]0211WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [2525],
	hService = [2],
	tsTimestamp = [2022/12/18 06:44 37.211],
	hResult = [-316],
	u.dwCommandCode = [305],
	lpBuffer = NULL
}
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CMD_RETRACT);
      }
      [TestMethod]
      public void Identify_WFS_CMD_CDM_RESET()
      {
         string logLine = @"
05024294967295016800147753740013CashDispenser0009FRAMEWORK00102022/12/18001221:25 31.9940007DEVRETN0021CBaseService::Execute0058HSERVICE[2] COMMAND[321] HRESULT[0] OUT BUFFER[0x00000000]01684294967295012200147753750006COMMON0009FRAMEWORK00102022/12/18001221:25 31.9950011INFORMATION0015CProcessor::Run0021---- Wait [4900] ----01224294967295029600147753760003CDM0003SPI00102022/12/18001221:25 31.9950009XFS_EVENT0012EXECUTE[321]0209WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [65357],
	hService = [2],
	tsTimestamp = [2022/12/18 21:25 31.995],
	hResult = [0],
	u.dwCommandCode = [321],
	lpBuffer = NULL
}
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CDM_RESET);
      }
      [TestMethod]
      public void Identify_WFS_SRVE_CDM_CASHUNITINFOCHANGED()
      {
         string logLine = @"
pCursor->pData->hApp=0x1F8215B501594294967295012600147655930003CDM0003SPI00102022/12/18001221:23 03.5830011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x202A0DA0)01264294967295013700147655940003CDM0009FRAMEWORK00102022/12/18001221:23 03.5840011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=13, wClass=301374294967295033100147655950005BRM600002SP00102022/12/18001221:23 03.5840004DATA0024CNHUsb2::SendControlData0236<- [64] 0000h: 00 4F 41 00 00 00 00 00 00 00 00 00 00 00 00 00 
0010h: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
0020h: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
0030h: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
03314294967295103100147655960003CDM0003SPI00102022/12/18001221:23 03.5840009XFS_EVENT0018SERVICE_EVENT[304]0938WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [0],
	hService = [2],
	tsTimestamp = [2022/12/18 21:23 03.582],
	hResult = [0],
	u.dwEventID = [304],
	lpBuffer = [0x1f053fdc]
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED);
      }
      [TestMethod]
      public void Identify_WFS_SRVE_CDM_ITEMSTAKEN()
      {
         string logLine = @"
changed(1->0)01364294967295012500149526070003BRM0002SP00102022/12/18001223:15 26.1860011INFORMATION0034CMonitorThread::CheckStatusChanged0015Item taken!(01)01254294967295015900149526080003CDM0009FRAMEWORK00102022/12/18001223:15 26.1870011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x0755C10D01594294967295013600149526090003CDM0009FRAMEWORK00102022/12/18001223:15 26.1870011INFORMATION0028CBaseService::BroadcastEvent0025pData->wClass=3, wClass=301364294967295015900149526100003CDM0009FRAMEWORK00102022/12/18001223:15 26.1870011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x0755C10D, pCursor->pData->hApp=0x1F8215B501594294967295012600149526110003CDM0003SPI00102022/12/18001223:15 26.1870011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x00000000)01264294967295013700149526120003CDM0009FRAMEWORK00102022/12/18001223:15 26.1870011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=13, wClass=301374294967295029100149526130003CDM0003SPI00102022/12/18001223:15 26.1870009XFS_EVENT0018SERVICE_EVENT[309]0198WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x00010574],
	RequestID = [0],
	hService = [2],
	tsTimestamp = [2022/12/18 23:15 26.187],
	hResult = [0],
	u.dwEventID = [309],
	lpBuffer = NULL
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_SRVE_CDM_ITEMSTAKEN);
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_STATUS()
      {
         string logLine = @"
lpBuffer=0x3655CE64)02044294967295015800087409080012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 15.0710007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1301] IN BUFFER[0x00000000]01584294967295019200087409090004DOOR0007ACTIVEX00102023/01/24001200:59 15.0710006XFSAPI0023CContextMgr::MgrWndProc0092WFS_SERVICE_EVENT(RequestID=0, hService=4, hResult=0, dwCommandCode=801 lpBuffer=0x78758CDC)01924294967295017100087409100012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 15.0720007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1301] HRESULT[0], OUT BUFFER[0x014CB644]01714294967295019700087409110009GUIDLIGHT0007ACTIVEX00102023/01/24001200:59 15.0720006XFSAPI0023CContextMgr::MgrWndProc0092WFS_SERVICE_EVENT(RequestID=0, hService=7, hResult=0, dwCommandCode=801 lpBuffer=0x787648EC)01974294967295019400087409120006SENSOR0007ACTIVEX00102023/01/24001200:59 15.0730006XFSAPI0023CContextMgr::MgrWndProc0092WFS_SERVICE_EVENT(RequestID=0, hService=8, hResult=0, dwCommandCode=801 lpBuffer=0x789A27DC)01944294967295160900087409130003CIM0003SPI00102023/01/24001200:59 15.0730009XFS_EVENT0013GETINFO[1301]1521WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [17646],
	hService = [12],
	tsTimestamp = [2023/01/24 00:59 15.072],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = [0x35c6e3a4]
	{
		fwDevice = [0],
		fwSafeDoor = [1],
		fwAcceptor = [1],
		fwIntermediateStacker = [0],
		fwStackerItems = [4],
		bDropBox = [0],
		lppPositions =
		{
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_INF_CIM_STATUS);
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_UNIT_INFO()
      {
         string logLine = @"
BUFFER[0x1F937974]01714294967295018600087401360007MONIMNG0008XFSAGENT00102023/01/24001200:59 14.6590004INFO0015Agent::FireTrap0092call WFSAsyncGetInfo(ManagedName=[CashDispenser] hService=[28] dwCategory=[301] hResult=[0])01864294967295022400087401370006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6600011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[17], HWND[0x00010638], REQUESTID[17613], dwCmdCode[301], dwTimeOut[300000]}02244294967295012400087401380006COMMON0003SPI00102023/01/24001200:59 14.6600011INFORMATION0010WFPGetInfo0034HSERVICE=27, SrvcVersion=2563(A03)01244294967295016700087401390006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6600011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[17], LogicalName[CashDispenser]}01674294967295297900087401400003CIM0003SPI00102023/01/24001200:59 14.6600009XFS_EVENT0013GETINFO[1303]2891WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [17614],
	hService = [12],
	tsTimestamp = [2023/01/24 00:59 14.660],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x35aa51b4]
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_IN_STATUS()
      {
         string logLine = @"
02454294967295022500090887650006COMMON0009FRAMEWORK00102023/01/24001223:55 47.4390011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[12], HWND[0x000201e0], REQUESTID[10517], dwCmdCode[1307], dwTimeOut[300000]}02254294967295019200090887660003CIM0007ACTIVEX00102023/01/24001223:55 47.4390006XFSAPI0017CService::GetInfo0099WFSAsyncGetInfo(hService=12, dwCategory=1307, lpQueryDetails=0x00000000, RequestID=10517) hResult=001924294967295016600090887670006COMMON0009FRAMEWORK00102023/01/24001223:55 47.4400011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[12], LogicalName[CashAcceptor]}01664294967295016900090887680003CDM0009FRAMEWORK00102023/01/24001223:55 47.4400011INFORMATION0021CBaseService::GetInfo0065Srvc=1307 ReqID=10517 Wnd=0x000201E0 Cmd=1307 TimeOut=300000 IO=201694294967295015800090887690012CashAcceptor0009FRAMEWORK00102023/01/24001223:55 47.4410007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1307] IN BUFFER[0x00000000]01584294967295017100090887700012CashAcceptor0009FRAMEWORK00102023/01/24001223:55 47.4410007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1307] HRESULT[0], OUT BUFFER[0x1F7E7C44]01714294967295046600090887710003CIM0003SPI00102023/01/24001223:55 47.4410009XFS_EVENT0013GETINFO[1307]0378WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10517],
	hService = [12],
	tsTimestamp = [2023/01/24 23:55 47.441],
	hResult = [0],
	u.dwCommandCode = [1307],
	lpBuffer = [0x0c5d121c]
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);
      }


      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_START()
      {
         string logLine = @"
05024294967295016900090887570012CashAcceptor0009FRAMEWORK00102023/01/24001223:55 47.4370007DEVRETN0021CBaseService::Execute0060HSERVICE[12] COMMAND[1301] HRESULT[0] OUT BUFFER[0x00000000]01694294967295012500090887580003BRM0002SP00102023/01/24001223:55 47.4370011INFORMATION0025CBrmCommand::UpdateStatus0024IdleStatus changed[0->1]01254294967295012200090887590006COMMON0009FRAMEWORK00102023/01/24001223:55 47.4370011INFORMATION0015CProcessor::Run0021---- Wait [2148] ----01224294967295029900090887600003CIM0003SPI00102023/01/24001223:55 47.4370009XFS_EVENT0013EXECUTE[1301]0211WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10516],
	hService = [12],
	tsTimestamp = [2023/01/24 23:55 47.437],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = NULL
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CIM_CASH_IN_START);
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN()
      {
         string logLine = @"
05024294967295016900090983410012CashAcceptor0009FRAMEWORK00102023/01/24001223:57 34.3680007DEVRETN0021CBaseService::Execute0060HSERVICE[12] COMMAND[1302] HRESULT[0] OUT BUFFER[0x015187C4]01694294967295012200090983420006COMMON0009FRAMEWORK00102023/01/24001223:57 34.3680011INFORMATION0015CProcessor::Run0021---- Wait [2148] ----01224294967295041500090983430003CIM0003SPI00102023/01/24001223:57 34.3680009XFS_EVENT0013EXECUTE[1302]0327WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10763],
	hService = [12],
	tsTimestamp = [2023/01/24 23:57 34.368],
	hResult = [0],
	u.dwCommandCode = [1302],
	lpBuffer = [0x255bb8ec]

	{
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CIM_CASH_IN);
      }


      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_END()
      {
         string logLine = @"
05024294967295016900090911310012CashAcceptor0009FRAMEWORK00102023/01/24001223:56 19.2250007DEVRETN0021CBaseService::Execute0060HSERVICE[12] COMMAND[1303] HRESULT[0] OUT BUFFER[0x1F7E5D8C]01694294967295012200090911320006COMMON0009FRAMEWORK00102023/01/24001223:56 19.2260011INFORMATION0015CProcessor::Run0021---- Wait [2148] ----01224294967295148400090911330003CIM0003SPI00102023/01/24001223:56 19.2260009XFS_EVENT0013EXECUTE[1303]1396WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [10579],
	hService = [12],
	tsTimestamp = [2023/01/24 23:56 19.226],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x255c2c4c]
	{
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CIM_CASH_IN_END);
      }


      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_ROLLBACK()
      {
         string logLine = @"
05024294967295016900316511240012CashAcceptor0009FRAMEWORK00102022/10/31001213:26 13.1110007DEVRETN0021CBaseService::Execute0060HSERVICE[13] COMMAND[1304] HRESULT[0] OUT BUFFER[0x00000000]01694294967295012200316511250006COMMON0009FRAMEWORK00102022/10/31001213:26 13.1110011INFORMATION0015CProcessor::Run0021---- Wait [5672] ----01224294967295029800316511260003CIM0003SPI00102022/10/31001213:26 13.1110009XFS_EVENT0013EXECUTE[1304]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00020224],
	RequestID = [4498],
	hService = [13],
	tsTimestamp = [2022/10/31 13:26 13.111],
	hResult = [0],
	u.dwCommandCode = [1304],
	lpBuffer = NULL
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK);
      }


      [TestMethod]
      public void Identify_WFS_CMD_CIM_RETRACT()
      {
         string logLine = @"
WFS_CMD_CIM_RETRACT01214294967295013600507215460004CCIM0002SP00102022/11/17001209:59 52.4890011INFORMATION0025CNHCCIMImpl::OnCimRetract0034CashInStatus : 0, DepositMode : 0101364294967295011500507215470004CCIM0002SP00102022/11/17001209:59 52.4900005ERROR0025CNHCCIMImpl::OnCimRetract0019No items to retract01154294967295013300507215480004CCIM0002SP00102022/11/17001209:59 52.4910011INFORMATION0020CNHCCIMImpl::Execute0036cash-in transaction ended by RETRACT01334294967295015100507215490004CCIM0002SP00102022/11/17001209:59 52.4930011INFORMATION0020CNHCCIMImpl::Execute0054dwCommand=[1305] hResult=[-1316] lpBuffer=[0x00000000]01514294967295017500507215500013CashAcceptor10009FRAMEWORK00102022/11/17001209:59 52.4940007DEVRETN0021CBaseService::Execute0065HSERVICE[108] COMMAND[1305] HRESULT[-1316] OUT BUFFER[0x00000000]01754294967295012200507215510006COMMON0009FRAMEWORK00102022/11/17001209:59 52.4940011INFORMATION0015CProcessor::Run0021---- Wait [6184] ----01224294967295031400507215520013CashAcceptor10003SPI00102022/11/17001209:59 52.4940009XFS_EVENT0013EXECUTE[1305]0216WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x0005077c],
	RequestID = [56833],
	hService = [108],
	tsTimestamp = [2022/11/17 09:59 52.494],
	hResult = [-1316],
	u.dwCommandCode = [1305],
	lpBuffer = NULL
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CIM_RETRACT);
      }


      [TestMethod]
      public void Identify_WFS_CMD_CIM_RESET()
      {
         string logLine = @"
IO=201684294967295014000088601820003BRM0002SP00102023/01/24001211:19 13.5520011INFORMATION0019CMonitorThread::Run0045m_oldBrmCimInfo.LCU[i].ulValues changed(5->0)01404294967295015800088601830012CashAcceptor0009FRAMEWORK00102023/01/24001211:19 13.5520007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295016900088601840012CashAcceptor0009FRAMEWORK00102023/01/24001211:19 13.5520007DEVRETN0021CBaseService::Execute0060HSERVICE[27] COMMAND[1313] HRESULT[0] OUT BUFFER[0x00000000]01694294967295014200088601850003BRM0002SP00102023/01/24001211:19 13.5520011INFORMATION0019CMonitorThread::Run0047m_oldBrmCimInfo.LCU[i].ulCount changed(2000->0)01424294967295018900088601860006COMMON0009FRAMEWORK00102023/01/24001211:19 13.5530011INFORMATION0025CCommandQueue::RemoveHead0078return (bResult[1]) pCommand{hService[2],..., ReqID[3032], dwCommandCode[321]}01894294967295029800088601870003CIM0003SPI00102023/01/24001211:19 13.5530009XFS_EVENT0013EXECUTE[1313]0210WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x00020620],
	RequestID = [3029],
	hService = [27],
	tsTimestamp = [2023/01/24 11:19 13.553],
	hResult = [0],
	u.dwCommandCode = [1313],
	lpBuffer = NULL
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_CMD_CIM_RESET);
      }


      [TestMethod]
      public void Identify_WFS_USRE_CIM_CASHUNITTHRESHOLD()
      {
         string logLine = @"
02434294967295013700087580220003CDM0009FRAMEWORK00102023/01/24001208:31 09.8690011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295019000087580230003CIM0007ACTIVEX00102023/01/24001208:31 09.8690006XFSAPI0017CService::GetInfo0097WFSAsyncGetInfo(hService=12, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=525) hResult=001904294967295015900087580240003CDM0009FRAMEWORK00102023/01/24001208:31 09.8690011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC211A2, pCursor->pData->hApp=0x2AC2292201594294967295013800087580250003CDM0009FRAMEWORK00102023/01/24001208:31 09.8700011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015900087580260003CDM0009FRAMEWORK00102023/01/24001208:31 09.8700011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC22922, pCursor->pData->hApp=0x0BB0458201594294967295012600087580270003CIM0003SPI00102023/01/24001208:31 09.8700011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x2A7374C8)01264294967295013700087580280003CDM0009FRAMEWORK00102023/01/24001208:31 09.8700011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295122400087580290003CIM0003SPI00102023/01/24001208:31 09.8710009XFS_EVENT0016USER_EVENT[1303]1133WFS_USER_EVENT, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2023/01/24 08:31 09.868],
	hResult = [0],
	u.dwEventID = [1303],
	lpBuffer = [0x086b88c4]
	{
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD);
      }


      [TestMethod]
      public void Identify_WFS_SRVE_CIM_CASHUNITINFOCHANGED()
      {
         string logLine = @"
BUFFER[0x00000000]01584294967295013800087403470003CDM0009FRAMEWORK00102023/01/24001200:59 14.7340011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295019600087403480003CDM0007ACTIVEX00102023/01/24001200:59 14.7350006XFSAPI0022CService::AsyncGetInfo0098WFSAsyncGetInfo(hService=17, dwCategory=301, lpQueryDetails=0x00000000, RequestID=17632) hResult=001964294967295015900087403490003CDM0009FRAMEWORK00102023/01/24001200:59 14.7350011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2A8FEA6A, pCursor->pData->hApp=0x0F0726FA01594294967295012600087403500003CIM0003SPI00102023/01/24001200:59 14.7350011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x2A2A7F40)01264294967295017100087403510013CashDispenser0009FRAMEWORK00102023/01/24001200:59 14.7350007DEVRETN0021CBaseService::GetInfo0061HSERVICE[17] CATEGORY[303] HRESULT[0], OUT BUFFER[0x0156496C]01714294967295015900087403520003CDM0007ACTIVEX00102023/01/24001200:59 14.7350011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x080B2C38, lParam=0x0C60B93C01594294967295013700087403530003CDM0009FRAMEWORK00102023/01/24001200:59 14.7350011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295122600087403540003CIM0003SPI00102023/01/24001200:59 14.7350009XFS_EVENT0019SERVICE_EVENT[1304]1132WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2023/01/24 00:59 14.732],
	hResult = [0],
	u.dwEventID = [1304],
	lpBuffer = [0x78763efc]
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED);
      }


      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSTAKEN()
      {
         string logLine = @"
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

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_SRVE_CIM_ITEMSTAKEN);
      }


      [TestMethod]
      public void Identify_WFS_EXEE_CIM_INPUTREFUSE()
      {
         string logLine = @"
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

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_EXEE_CIM_INPUTREFUSE);
      }


      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSPRESENTED()
      {
         string logLine = @"
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

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_SRVE_CIM_ITEMSPRESENTED);
      }


      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSINSERTED()
      {
         string logLine = @"
0]01534294967295011700089128740003BRM0002SP00102023/01/24001213:23 01.0920011INFORMATION0025CCimService::CheckDeposit0016CheckDeposit End01174294967295015900089128750003CDM0009FRAMEWORK00102023/01/24001213:23 01.0960011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x2AC211A201594294967295013700089128760003CDM0009FRAMEWORK00102023/01/24001213:23 01.0960011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295015900089128770003CDM0009FRAMEWORK00102023/01/24001213:23 01.0970011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC211A2, pCursor->pData->hApp=0x2AC2292201594294967295013800089128780003CDM0009FRAMEWORK00102023/01/24001213:23 01.0970011INFORMATION0028CBaseService::BroadcastEvent0027pData->wClass=13, wClass=1301384294967295015900089128790003CDM0009FRAMEWORK00102023/01/24001213:23 01.0980011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x2AC22922, pCursor->pData->hApp=0x0BB0458201594294967295012600089128800003CIM0003SPI00102023/01/24001213:23 01.0980011INFORMATION0026CServiceAgent::ReportEvent0023call UnPack(0x00000000)01264294967295013700089128810003CDM0009FRAMEWORK00102023/01/24001213:23 01.0980011INFORMATION0028CBaseService::BroadcastEvent0026pData->wClass=3, wClass=1301374294967295029400089128820003CIM0003SPI00102023/01/24001213:23 01.0990009XFS_EVENT0019SERVICE_EVENT[1311]0200WFS_SERVICE_EVENT, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [0],
	hService = [12],
	tsTimestamp = [2023/01/24 13:23 01.096],
	hResult = [0],
	u.dwEventID = [1311],
	lpBuffer = NULL
";

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_SRVE_CIM_ITEMSINSERTED);
      }


      [TestMethod]
      public void Identify_WFS_EXEE_CIM_NOTEERROR()
      {
         string logLine = @"
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

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_EXEE_CIM_NOTEERROR);
      }


      [TestMethod]
      public void Identify_WFS_SRVE_CIM_MEDIADETECTED()
      {
         string logLine = @"
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

         XFSType xfsType = LogLine.IdentifyLine(logLine);
         Assert.IsTrue(xfsType == XFSType.WFS_SRVE_CIM_MEDIADETECTED);
      }
   }
}