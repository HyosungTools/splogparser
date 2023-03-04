using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class _wfs_inf_cdm_cash_unit_infoTests
   {
      const string GET_INFO_COMPLETE =
@"02424294967295016600358535370006COMMON0009FRAMEWORK00102022/12/19001216:01 26.0150011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[2], LogicalName[CashDispenser]}01664294967295014500358535380003BRM0002SP00102022/12/19001216:01 26.0150011INFORMATION0019CMonitorThread::Run0050m_oldBrmStatus.OutputPosition.fwHand changed(1->0)01454294967295015900358535390003CDM0007ACTIVEX00102022/12/19001216:01 26.0150011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x01493AD8, lParam=0x08F07C5C01594294967295019500358535400003CDM0007ACTIVEX00102022/12/19001216:01 26.0150006XFSAPI0022CService::AsyncGetInfo0097WFSAsyncGetInfo(hService=2, dwCategory=301, lpQueryDetails=0x00000000, RequestID=39299) hResult=001954294967295016700358535410003CDM0009FRAMEWORK00102022/12/19001216:01 26.0160011INFORMATION0021CBaseService::GetInfo0063Srvc=303 ReqID=39297 Wnd=0x00010580 Cmd=303 TimeOut=300000 IO=101674294967295015600358535420003CDM0007ACTIVEX00102022/12/19001216:01 26.0160011INFORMATION0025CCdmService::HandleStatus0050GetInfo-Result[Status][ReqID=39296] = {hResult[0]}01564294967295011900358535430003BRM0002SP00102022/12/19001216:01 26.0160011INFORMATION0019CMonitorThread::Run0024fwChecking changed(0->1)01194294967295015700358535440013CashDispenser0009FRAMEWORK00102022/12/19001216:01 26.0160007DEVCALL0021CBaseService::GetInfo0047HSERVICE[2] CATEGORY[303] IN BUFFER[0x00000000]01574294967295015800358535450003CDM0007ACTIVEX00102022/12/19001216:01 26.0160011INFORMATION0025CCdmService::HandleStatus0052StStacker-old, StStacker-new : { NOTEMPTY , EMPTY }.01584294967295015900358535460003CDM0007ACTIVEX00102022/12/19001216:01 26.0160011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x28B3B7A0, lParam=0x392EF0B401594294967295015700358535470003CDM0007ACTIVEX00102022/12/19001216:01 26.0170005EVENT0041CCdmService::SetStackerStatusChangedEvent0041FireStackerStatusChanged{NewValue[EMPTY]}01574294967295015400358535480003CDM0009FRAMEWORK00102022/12/19001216:01 26.0170011INFORMATION0032CBaseService::CheckStatusChanged0039Device Status Message Report.[Status=0]01544294967295015600358535490003CDM0007ACTIVEX00102022/12/19001216:01 26.0170011INFORMATION0025CCdmService::HandleStatus0050GetInfo-Result[Status][ReqID=39294] = {hResult[0]}01564294967295017000358535500013CashDispenser0009FRAMEWORK00102022/12/19001216:01 26.0170007DEVRETN0021CBaseService::GetInfo0060HSERVICE[2] CATEGORY[303] HRESULT[0], OUT BUFFER[0x03105C6C]01704294967295015500358535510003CDM0007ACTIVEX00102022/12/19001216:01 26.0170011INFORMATION0025CCdmService::HandleStatus0049StShutter-old, StShutter-new : { CLOSED , OPEN }.01554294967295015900358535520003CDM0009FRAMEWORK00102022/12/19001216:01 26.0170011INFORMATION0028CBaseService::BroadcastEvent0048hApp=0x00000000, pCursor->pData->hApp=0x118D5E7801594294967295015800358535530003CDM0007ACTIVEX00102022/12/19001216:01 26.0170011INFORMATION0025CCdmService::HandleStatus0052StStacker-old, StStacker-new : { NOTEMPTY , EMPTY }.01584294967295022300358535540006COMMON0009FRAMEWORK00102022/12/19001216:01 26.0180011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[2], HWND[0x00010580], REQUESTID[39298], dwCmdCode[309], dwTimeOut[300000]}02234294967295015600358535550003CDM0007ACTIVEX00102022/12/19001216:01 26.0180005EVENT0041CCdmService::SetShutterStatusChangedEvent0040FireShutterStatusChanged{NewValue[OPEN]}01564294967295160600358535560003CDM0003SPI00102022/12/19001216:01 26.0180009XFS_EVENT0012GETINFO[303]1519WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00010580],
	RequestID = [39297],
	hService = [2],
	tsTimestamp = [2022/12/19 16:01 26.018],
	hResult = [0],
	u.dwCommandCode = [303],
	lpBuffer = [0x526afb84]
	{
		usTellerID=0
		usCount=6
		lppList->
		{
			usNumber		1		2		3		4		5		6
			usType			6		2		12		12		12		12
			lpszCashUnitName	[LCU00]		[LCU01]		[LCU02]		[LCU03]		[LCU04]		[LCU05]		
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05
			cCurrencyID		   		   		USD		USD		USD		USD
			ulValues		0		0		1		5		20		100
			ulInitialCount		0		0		1400		1400		1400		1400
			ulCount			0		2		1364		1354		1377		1352
			ulRejectCount		0		2		1		0		1		0
			ulMinimum		0		0		0		0		0		0
			ulMaximum		80		210		0		0		0		0
			bAppLock		0		0		0		0		0		0
			usStatus		4		0		0		0		0		0
			ulDispensedCount	0		0		71		60		154		67		
			ulPresentedCount	0		0		65		57		146		67		
			ulRetractedCount	0		0		0		0		0		0		

			lppPhysical->
			usNumPhysicalCUs	1		1		1		1		1		1		
			lpPhysicalPositionName	RetractCassette	RejectCassette	CassetteA	CassetteB	CassetteC	CassetteD	
			cUnitID			RTCST		RJCST		CST_A		CST_B		CST_C		CST_D
			ulInitialCount		0		0		1400		1400		1400		1400
			ulCount			0		2		1364		1354		1377		1352
			ulRejectCount		0		2		1		0		1		0
			ulMaximum		80		210		0		0		0		0
			usPStatus		4		0		0		0		0		0
			bHardwareSensor		1		1		1		1		1		1
			ulDispensedCount	0		0		71		60		154		67		
			ulPresentedCount	0		0		65		57		146		67		
			ulRetractedCount	0		0		0		0		0		0		

		}
	}
}";
      string xfsLine = string.Empty; 

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = GET_INFO_COMPLETE; 
      }

      [TestMethod]
      public void TestTimestamp()
      {
         string timeStamp = lpResult.tsTimestamp(xfsLine);
         Assert.IsTrue(timeStamp == "2022/12/19 16:01 26.018");
         
      }
      [TestMethod]
      public void TesthResult()
      {
         string hResult = lpResult.hResult(xfsLine);
         Assert.IsTrue(hResult == "0");

      }
      [TestMethod]
      public void Test_usCount()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
         Assert.IsTrue(result.xfsMatch == "6");
      }
      [TestMethod]
      public void Test_usType()
      {
         // given the line "usType			6		2		12		12		12		12"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.usType(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }
      [TestMethod]
      public void Test_cUnitID()
      {
         // given the line "cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.cUnitID(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_cCurrencyID()
      {
         // given the line "cCurrencyID		   		   		USD		USD		USD		USD"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.cCurrencyID(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulValues()
      {
         // given the line "ulValues		0		0		1		5		20		100"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulValues(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulInitialCount()
      {
         // given the line "ulInitialCount		0		0		1400		1400		1400		1400"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulInitialCount(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulMinimum()
      {
         // given the line "ulMinimum		0		0		0		0		0		0"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulMinimum(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulMaximum()
      {
         // given the line "ulMaximum		80		210		0		0		0		0"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulMaximum(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }
   }
}
