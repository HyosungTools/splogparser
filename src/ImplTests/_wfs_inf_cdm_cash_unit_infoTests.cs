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
      const string GET_INFO_COMPLETE2 =
@"
14144294967295016600159585730006COMMON0009FRAMEWORK00102022/12/07001210:18 59.5080011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[2], LogicalName[CashDispenser]}01664294967295015900159585740003CDM0007ACTIVEX00102022/12/07001210:18 59.5080011INFORMATION0022CMsgWnd::DefWindowProc0056message=0x00000407, wParam=0x1DE33088, lParam=0x1BFBBB7401594294967295016700159585750003CDM0009FRAMEWORK00102022/12/07001210:18 59.5080011INFORMATION0021CBaseService::GetInfo0063Srvc=303 ReqID=59758 Wnd=0x00020108 Cmd=303 TimeOut=300000 IO=101674294967295015600159585760003CDM0007ACTIVEX00102022/12/07001210:18 59.5090011INFORMATION0025CCdmService::HandleStatus0050GetInfo-Result[Status][ReqID=59757] = {hResult[0]}01564294967295015700159585770013CashDispenser0009FRAMEWORK00102022/12/07001210:18 59.5090007DEVCALL0021CBaseService::GetInfo0047HSERVICE[2] CATEGORY[303] IN BUFFER[0x00000000]01574294967295015500159585780003CDM0007ACTIVEX00102022/12/07001210:18 59.5090011INFORMATION0025CCdmService::HandleStatus0049StShutter-old, StShutter-new : { OPEN , CLOSED }.01554294967295015800159585790003CDM0007ACTIVEX00102022/12/07001210:18 59.5090005EVENT0041CCdmService::SetShutterStatusChangedEvent0042FireShutterStatusChanged{NewValue[CLOSED]}01584294967295012200159585800003CDM0007ACTIVEX00102022/12/07001210:18 59.5110008PROPERTY0021Ctrl::GetDeviceStatus0023DeviceStatus[DEVONLINE]01224294967295019300159585810003CDM0007ACTIVEX00102022/12/07001210:18 59.5120011INFORMATION0024CContextMgr::SetComplete0088SetComplete(Waiting ReqID=[59757], Arrival ReqID=[59757]), Remove ReqID[59757] from List01934294967295011800159585820003CDM0007ACTIVEX00102022/12/07001210:18 59.5130005EVENT0029CCdmService::HandleItemsTaken0014FireItemsTaken01184294967295014100159585830003CDM0009FRAMEWORK00102022/12/07001210:18 59.5140011INFORMATION0020CCdmService::GetInfo0038WFS_INF_CDM_CASH_UNIT_INFO{hResult[0]}01414294967295017000159585840013CashDispenser0009FRAMEWORK00102022/12/07001210:18 59.5140007DEVRETN0021CBaseService::GetInfo0060HSERVICE[2] CATEGORY[303] HRESULT[0], OUT BUFFER[0x00B24B4C]01704294967295022300159585850006COMMON0009FRAMEWORK00102022/12/07001210:18 59.5140011INFORMATION0033CSpCmdDispatcher::DispatchCommand0104pService->GetInfo() {HSERVICE[2], HWND[0x00020108], REQUESTID[59759], dwCmdCode[301], dwTimeOut[300000]}02234294967295016600159585860006COMMON0009FRAMEWORK00102022/12/07001210:18 59.5150011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[2], LogicalName[CashDispenser]}01664294967295418200159585870003CDM0003SPI00102022/12/07001210:18 59.5150009XFS_EVENT0012GETINFO[303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00020108],
	RequestID = [59758],
	hService = [2],
	tsTimestamp = [2022/12/07 10:18 59.514],
	hResult = [0],
	u.dwCommandCode = [303],
	lpBuffer = [0x20f11d6c]
	{
		usTellerID = [0],
		usCount = [5],
		lppList =
		{
			usNumber = [1],
			usType = [11],
			lpszCashUnitName = NULL,
			cUnitID = [LCU00],
			cCurrencyID = [   ],
			ulValues = [12],
			ulInitialCount = [13],
			ulCount = [14],
			ulRejectCount = [15],
			ulMinimum = [16],
			ulMaximum = [17],
			bAppLock = [0],
			usStatus = [18],
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = [Cst_0],
				cUnitID = [PCU00],
				ulInitialCount = [0],
				ulCount = [0],
				ulRejectCount = [0],
				ulMaximum = [300],
				usPStatus = [0],
				bHardwareSensor = [0],
				ulDispensedCount = [19],
				ulPresentedCount = [110],
				ulRetractedCount = [111]
			}
			ulDispensedCount = [19],
			ulPresentedCount = [110],
			ulRetractedCount = [111]
		}
		{
			usNumber = [2],
			usType = [21],
			lpszCashUnitName = NULL,
			cUnitID = [LCU01],
			cCurrencyID = [USD],
			ulValues = [22],
			ulInitialCount = [23],
			ulCount = [24],
			ulRejectCount = [25],
			ulMinimum = [26],
			ulMaximum = [27],
			bAppLock = [0],
			usStatus = [28],
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = [Cst_1],
				cUnitID = [PCU01],
				ulInitialCount = [3000],
				ulCount = [3000],
				ulRejectCount = [0],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [0],
				ulDispensedCount = [29],
				ulPresentedCount = [210],
				ulRetractedCount = [211]
			}
			ulDispensedCount = [29],
			ulPresentedCount = [210],
			ulRetractedCount = [211]
		}
		{
			usNumber = [3],
			usType = [31],
			lpszCashUnitName = NULL,
			cUnitID = [LCU02],
			cCurrencyID = [ABC],
			ulValues = [32],
			ulInitialCount = [33],
			ulCount = [34],
			ulRejectCount = [35],
			ulMinimum = [36],
			ulMaximum = [37],
			bAppLock = [0],
			usStatus = [38],
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = [Cst_2],
				cUnitID = [PCU02],
				ulInitialCount = [3000],
				ulCount = [3000],
				ulRejectCount = [0],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [1],
				ulDispensedCount = [39],
				ulPresentedCount = [310],
				ulRetractedCount = [311]
			}
			ulDispensedCount = [39],
			ulPresentedCount = [310],
			ulRetractedCount = [311]
		}
		{
			usNumber = [4],
			usType = [41],
			lpszCashUnitName = NULL,
			cUnitID = [LCU03],
			cCurrencyID = [CAD],
			ulValues = [42],
			ulInitialCount = [43],
			ulCount = [44],
			ulRejectCount = [45],
			ulMinimum = [46],
			ulMaximum = [47],
			bAppLock = [0],
			usStatus = [48],
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = [Cst_3],
				cUnitID = [PCU03],
				ulInitialCount = [3000],
				ulCount = [3000],
				ulRejectCount = [0],
				ulMaximum = [0],
				usPStatus = [3],
				bHardwareSensor = [1],
				ulDispensedCount = [49],
				ulPresentedCount = [410],
				ulRetractedCount = [411]
			}
			ulDispensedCount = [49],
			ulPresentedCount = [410],
			ulRetractedCount = [411]
		}
		{
			usNumber = [5],
			usType = [51],
			lpszCashUnitName = NULL,
			cUnitID = [LCU04],
			cCurrencyID = [FRA],
			ulValues = [52],
			ulInitialCount = [53],
			ulCount = [54],
			ulRejectCount = [55],
			ulMinimum = [56],
			ulMaximum = [57],
			bAppLock = [0],
			usStatus = [58],
			usNumPhysicalCUs = [2],
			lppPhysical = 
			{
				lpPhysicalPositionName = [Cst_4],
				cUnitID = [PCU04],
				ulInitialCount = [3000],
				ulCount = [3000],
				ulRejectCount = [0],
				ulMaximum = [0],
				usPStatus = [4],
				bHardwareSensor = [1],
				ulDispensedCount = [59],
				ulPresentedCount = [510],
				ulRetractedCount = [511]
			}
			{
				lpPhysicalPositionName = [Cst_5],
				cUnitID = [PCU05],
				ulInitialCount = [3000],
				ulCount = [3000],
				ulRejectCount = [0],
				ulMax
		......(More Data)......
	}
}
";
      string xfsLine = string.Empty;
      string xfsLine2 = string.Empty;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = GET_INFO_COMPLETE;
         xfsLine2 = GET_INFO_COMPLETE2;
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
         Assert.IsTrue(hResult == "");

      }
      [TestMethod]
      public void Test_usCount()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);
         Assert.IsTrue(result.xfsMatch == "6");
      }

      [TestMethod]
      public void Test_usTypesFromTable()
      {
         // given the line "usType			6		2		12		12		12		12"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.usTypesFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }
      [TestMethod]
      public void Test_cUnitIDsFromTable()
      {
         // given the line "cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.cUnitIDsFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromTable()
      {
         // given the line "cCurrencyID		   		   		USD		USD		USD		USD"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.cCurrencyIDsFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulValuesFromTable()
      {
         // given the line "ulValues		0		0		1		5		20		100"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulValuesFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulInitialCountsFromTable()
      {
         // given the line "ulInitialCount		0		0		1400		1400		1400		1400"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulInitialCountsFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulCountsFromTable()
      {
         // given the line "ulCount		0		0		1400		1400		1400		1400"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulCountsFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulMinimumsFromTable()
      {
         // given the line "ulMinimum		0		0		0		0		0		0"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulMinimumsFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulMaximumsFromTable()
      {
         // given the line "ulMaximum		80		210		0		0		0		0"
         // prove we can isolate the values and the resulting array is usCount long
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine);

         (bool success, string[] xfsMatch, string subLogLine) valuesResult = _wfs_inf_cdm_cash_unit_info.ulMaximumsFromTable(xfsLine);
         Assert.IsTrue(valuesResult.xfsMatch.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_usNumbersFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.usNumbersFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
         Assert.IsTrue(int.Parse(countResult.xfsMatch) == 5);
         for (int i = 0; i < valuesResult.Length; i++)
         {
            Assert.IsTrue(int.Parse(valuesResult[i]) == i + 1);
         }
      }

      [TestMethod]
      public void Test_usTypesFromList()
      {
        (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.usTypesFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }
      [TestMethod]
      public void Test_cUnitIDsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);         int ulCount = int.Parse(countResult.xfsMatch);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.cUnitIDsFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.cCurrencyIDsFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulValuesFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.ulValuesFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulInitialCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.ulInitialCountsFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.ulCountsFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulMinimumsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.ulMinimumsFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }

      [TestMethod]
      public void Test_ulMaximumsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) countResult = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);

         string[] valuesResult = _wfs_inf_cdm_cash_unit_info.ulMaximumsFromList(xfsLine2);
         Assert.IsTrue(valuesResult.Length == int.Parse(countResult.xfsMatch));
      }


      [TestMethod]
      public void Test_usCount2()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cdm_cash_unit_info.usCount(xfsLine2);
         Assert.IsTrue(result.xfsMatch == "5");
      }

      [TestMethod]
      public void Test_usNumber()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "1");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "2");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "3");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "4");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "5");

      }

      [TestMethod]
      public void Test_usType()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.usType(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "11");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usType(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "21");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usType(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "31");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usType(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "41");

         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.usType(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "51");
      }

      [TestMethod]
      public void Test_cUnitID()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.cUnitID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "LCU00");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cUnitID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "LCU01");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cUnitID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "LCU02");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cUnitID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "LCU03");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cUnitID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "LCU04");
      }

      [TestMethod]
      public void Test_cCurrencyID()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.cCurrencyID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "   ");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cCurrencyID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "USD");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cCurrencyID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "ABC");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cCurrencyID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "CAD");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.cCurrencyID(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "FRA");
      }

      [TestMethod]
      public void Test_ulValue()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulValue(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "12");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulValue(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "22");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulValue(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "32");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulValue(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "42");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulValue(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "52");
      }

      [TestMethod]
      public void Test_ulInitialCount()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulInitialCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "13");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulInitialCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "23");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulInitialCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "33");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulInitialCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "43");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulInitialCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "53");

      }

      [TestMethod]
      public void Test_ulMinimum()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulMinimum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "16");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMinimum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "26");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMinimum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "36");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMinimum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "46");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMinimum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "56");
      }

      [TestMethod]
      public void Test_ulMaximum()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulMaximum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "17");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMaximum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "27");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMaximum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "37");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMaximum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "47");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulMaximum(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "57");
      }

      [TestMethod]
      public void Test_ulCount()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "14");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "24");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "34");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "44");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "54");
      }

      [TestMethod]
      public void Test_ulRejectCount()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulRejectCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "15");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRejectCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "25");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRejectCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "35");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRejectCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "45");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRejectCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "55");
      }

      [TestMethod]
      public void Test_ulDispensedCount()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "19");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "29");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "39");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "49");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulDispensedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "59");
      }

      [TestMethod]
      public void Test_ulPresentedCount()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "110");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "210");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "310");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "410");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulPresentedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "510");
      }

      [TestMethod]
      public void Test_ulRetractedCount()
      {
         string thisNumberStartsHere = string.Empty;
         string nextNumberStartsHere = xfsLine2;

         // given a list log line, can you isolate usNumber? 
         (bool success, string xfsMatch, string subLogLine) valueResult = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "111");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "211");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "311");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "411");

         // you need to search for the next usNumber before you do anything else
         valueResult = _wfs_inf_cdm_cash_unit_info.usNumber(valueResult.subLogLine);
         nextNumberStartsHere = valueResult.subLogLine;

         // if you save off the sub-string, can you get the next number? 
         valueResult = _wfs_inf_cdm_cash_unit_info.ulRetractedCount(nextNumberStartsHere);
         Assert.IsTrue(valueResult.xfsMatch == "511");
      }

   }
}
