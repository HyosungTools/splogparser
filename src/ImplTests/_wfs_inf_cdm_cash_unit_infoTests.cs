using System.Collections.Generic;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class _wfs_inf_cdm_cash_unit_infoTests
   {
      const string GET_INFO_COMPLETE_AS_TABLE =
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
			usType			6		7		8		9		10		11
			lpszCashUnitName	[LCU00]		[LCU01]		[LCU02]		[LCU03]		[LCU04]		[LCU05]		
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05
			cCurrencyID		   		   		USA		USB		USC		USD
			ulValues		12		13		14		15		16		17
			ulInitialCount		18		19		20		21		22		23
			ulCount			24		25		26		27		28		29
			ulRejectCount		30		31		32		33		34		35
			ulMinimum		36		37		38		39		40		41
			ulMaximum		42		43		44		45		46		47
			bAppLock		0		0		0		0		0		0
			usStatus		48		49		50		51		52		53
			ulDispensedCount	54		55		56		57		58		59		
			ulPresentedCount	60		61		62		63		64		65		
			ulRetractedCount	66		67		68		69		70		71		

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
      const string GET_INFO_COMPLETE_AS_LIST =
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
      string xfsLineTable = string.Empty;
      string xfsLineList = string.Empty;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLineTable = GET_INFO_COMPLETE_AS_TABLE;
         xfsLineList = GET_INFO_COMPLETE_AS_LIST;
      }

      [TestMethod]
      public void TestTimestamp()
      {
         string timeStamp = lpResult.tsTimestamp(xfsLineTable);
         Assert.IsTrue(timeStamp == "2022/12/19 16:01 26.018");
         
      }
      [TestMethod]
      public void TesthResult()
      {
         string hResult = lpResult.hResult(xfsLineTable);
         Assert.IsTrue(hResult == "");

      }
      [TestMethod]
      public void Test_usCountFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         Assert.IsTrue(usCount == 6);
      }

      [TestMethod]
      public void Test_usNumbersFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.usNumbersFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "1");
         Assert.IsTrue(values[1] == "2");
         Assert.IsTrue(values[2] == "3");
         Assert.IsTrue(values[3] == "4");
         Assert.IsTrue(values[4] == "5");
         Assert.IsTrue(values[5] == "6");
      }

      [TestMethod]
      public void Test_usTypesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.usTypesFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "6");
         Assert.IsTrue(values[1] == "7");
         Assert.IsTrue(values[2] == "8");
         Assert.IsTrue(values[3] == "9");
         Assert.IsTrue(values[4] == "10");
         Assert.IsTrue(values[5] == "11");
      }

      [TestMethod]
      public void Test_cUnitIDsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.cUnitIDsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "LCU00");
         Assert.IsTrue(values[1] == "LCU01");
         Assert.IsTrue(values[2] == "LCU02");
         Assert.IsTrue(values[3] == "LCU03");
         Assert.IsTrue(values[4] == "LCU04");
         Assert.IsTrue(values[5] == "LCU05");
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.cCurrencyIDsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "   ");
         Assert.IsTrue(values[1] == "   ");
         Assert.IsTrue(values[2] == "USA");
         Assert.IsTrue(values[3] == "USB");
         Assert.IsTrue(values[4] == "USC");
         Assert.IsTrue(values[5] == "USD");
      }

      [TestMethod]
      public void Test_ulValuesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulValuesFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "12");
         Assert.IsTrue(values[1] == "13");
         Assert.IsTrue(values[2] == "14");
         Assert.IsTrue(values[3] == "15");
         Assert.IsTrue(values[4] == "16");
         Assert.IsTrue(values[5] == "17");
      }

      [TestMethod]
      public void Test_ulInitialCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulInitialCountsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "18");
         Assert.IsTrue(values[1] == "19");
         Assert.IsTrue(values[2] == "20");
         Assert.IsTrue(values[3] == "21");
         Assert.IsTrue(values[4] == "22");
         Assert.IsTrue(values[5] == "23");
      }

      [TestMethod]
      public void Test_ulCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulCountsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "24");
         Assert.IsTrue(values[1] == "25");
         Assert.IsTrue(values[2] == "26");
         Assert.IsTrue(values[3] == "27");
         Assert.IsTrue(values[4] == "28");
         Assert.IsTrue(values[5] == "29");
      }

      [TestMethod]
      public void Test_ulRejectCountsFromTableFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulRejectCountsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "30");
         Assert.IsTrue(values[1] == "31");
         Assert.IsTrue(values[2] == "32");
         Assert.IsTrue(values[3] == "33");
         Assert.IsTrue(values[4] == "34");
         Assert.IsTrue(values[5] == "35");
      }

      [TestMethod]
      public void Test_ulMinimumsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulMinimumsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "36");
         Assert.IsTrue(values[1] == "37");
         Assert.IsTrue(values[2] == "38");
         Assert.IsTrue(values[3] == "39");
         Assert.IsTrue(values[4] == "40");
         Assert.IsTrue(values[5] == "41");
      }

      [TestMethod]
      public void Test_ulMaximumsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulMaximumsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "42");
         Assert.IsTrue(values[1] == "43");
         Assert.IsTrue(values[2] == "44");
         Assert.IsTrue(values[3] == "45");
         Assert.IsTrue(values[4] == "46");
         Assert.IsTrue(values[5] == "47");
      }

      [TestMethod]
      public void Test_usStatusesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.usStatusesFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "48");
         Assert.IsTrue(values[1] == "49");
         Assert.IsTrue(values[2] == "50");
         Assert.IsTrue(values[3] == "51");
         Assert.IsTrue(values[4] == "52");
         Assert.IsTrue(values[5] == "53");
      }

      [TestMethod]
      public void Test_ulDispensedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulDispensedCountsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "54");
         Assert.IsTrue(values[1] == "55");
         Assert.IsTrue(values[2] == "56");
         Assert.IsTrue(values[3] == "57");
         Assert.IsTrue(values[4] == "58");
         Assert.IsTrue(values[5] == "59");
      }

      [TestMethod]
      public void Test_ulPresentedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulPresentedCountsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "60");
         Assert.IsTrue(values[1] == "61");
         Assert.IsTrue(values[2] == "62");
         Assert.IsTrue(values[3] == "63");
         Assert.IsTrue(values[4] == "64");
         Assert.IsTrue(values[5] == "65");
      }

      [TestMethod]
      public void Test_ulRetractedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         int usCount = _wfs_inf_cdm_cash_unit_info.usCountFromTable(result.xfsLine);
         string[] values = _wfs_inf_cdm_cash_unit_info.ulRetractedCountsFromTable(result.xfsLine);

         Assert.IsTrue(usCount == 6);
         Assert.IsTrue(values.Length == usCount);
         Assert.IsTrue(values[0] == "66");
         Assert.IsTrue(values[1] == "67");
         Assert.IsTrue(values[2] == "68");
         Assert.IsTrue(values[3] == "69");
         Assert.IsTrue(values[4] == "70");
         Assert.IsTrue(values[5] == "71");
      }
      

      [TestMethod]
      public void Test_usNumbersFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] values = _wfs_inf_cdm_cash_unit_info.usNumbersFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(values.Length == result.usCount);
         Assert.IsTrue(values[0] == "1");
         Assert.IsTrue(values[1] == "2");
         Assert.IsTrue(values[2] == "3");
         Assert.IsTrue(values[3] == "4");
         Assert.IsTrue(values[4] == "5");
      }

      [TestMethod]
      public void Test_usTypesFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] values = _wfs_inf_cdm_cash_unit_info.usTypesFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(values.Length == result.usCount);
         Assert.IsTrue(values[0] == "11");
         Assert.IsTrue(values[1] == "21");
         Assert.IsTrue(values[2] == "31");
         Assert.IsTrue(values[3] == "41");
         Assert.IsTrue(values[4] == "51");
      }
      [TestMethod]
      public void Test_cUnitIDsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.cUnitIDsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "LCU00");
         Assert.IsTrue(results[1] == "LCU01");
         Assert.IsTrue(results[2] == "LCU02");
         Assert.IsTrue(results[3] == "LCU03");
         Assert.IsTrue(results[4] == "LCU04");
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.cCurrencyIDsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "");
         Assert.IsTrue(results[1] == "USD");
         Assert.IsTrue(results[2] == "ABC");
         Assert.IsTrue(results[3] == "CAD");
         Assert.IsTrue(results[4] == "FRA");
      }

      [TestMethod]
      public void Test_ulValuesFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulValuesFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "12");
         Assert.IsTrue(results[1] == "22");
         Assert.IsTrue(results[2] == "32");
         Assert.IsTrue(results[3] == "42");
         Assert.IsTrue(results[4] == "52");
      }

      [TestMethod]
      public void Test_ulInitialCountsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulInitialCountsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "13");
         Assert.IsTrue(results[1] == "23");
         Assert.IsTrue(results[2] == "33");
         Assert.IsTrue(results[3] == "43");
         Assert.IsTrue(results[4] == "53");
      }

      [TestMethod]
      public void Test_ulCountsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulCountsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "14");
         Assert.IsTrue(results[1] == "24");
         Assert.IsTrue(results[2] == "34");
         Assert.IsTrue(results[3] == "44");
         Assert.IsTrue(results[4] == "54");
      }

      public void Test_ulRejectCountsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulRejectCountsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "15");
         Assert.IsTrue(results[1] == "25");
         Assert.IsTrue(results[2] == "35");
         Assert.IsTrue(results[3] == "45");
         Assert.IsTrue(results[4] == "55");
      }

      [TestMethod]
      public void Test_ulMinimumsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulMinimumsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "16");
         Assert.IsTrue(results[1] == "26");
         Assert.IsTrue(results[2] == "36");
         Assert.IsTrue(results[3] == "46");
         Assert.IsTrue(results[4] == "56");
      }

      [TestMethod]
      public void Test_ulMaximumsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulMaximumsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "17");
         Assert.IsTrue(results[1] == "27");
         Assert.IsTrue(results[2] == "37");
         Assert.IsTrue(results[3] == "47");
         Assert.IsTrue(results[4] == "57");
      }

      [TestMethod]
      public void Test_usStatusesFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.usStatusesFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "18");
         Assert.IsTrue(results[1] == "28");
         Assert.IsTrue(results[2] == "38");
         Assert.IsTrue(results[3] == "48");
         Assert.IsTrue(results[4] == "58");
      }

      [TestMethod]
      public void Test_ulDispensedCountsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulDispensedCountsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "19");
         Assert.IsTrue(results[1] == "29");
         Assert.IsTrue(results[2] == "39");
         Assert.IsTrue(results[3] == "49");
         Assert.IsTrue(results[4] == "59");
      }

      [TestMethod]
      public void Test_ulPresentedCountsFromLists()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulPresentedCountsFromLists(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "110");
         Assert.IsTrue(results[1] == "210");
         Assert.IsTrue(results[2] == "310");
         Assert.IsTrue(results[3] == "410");
         Assert.IsTrue(results[4] == "510");
      }

      [TestMethod]
      public void Test_ulRetractedCountsFromList()
      {
         (int usCount, string xfsLine) result = _wfs_inf_cdm_cash_unit_info.usCountFromList(xfsLineList);
         string[] results = _wfs_inf_cdm_cash_unit_info.ulRetractedCountsFromList(xfsLineList);

         Assert.IsTrue(result.usCount == 5);
         Assert.IsTrue(results.Length == result.usCount);
         Assert.IsTrue(results[0] == "111");
         Assert.IsTrue(results[1] == "211");
         Assert.IsTrue(results[2] == "311");
         Assert.IsTrue(results[3] == "411");
         Assert.IsTrue(results[4] == "511");
      }
   }
}
