using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   // We see WFSCIMCASHINFO in the following commands: 
   // 
   // WFS_INF_CIM_CASH_UNIT_INFO
   // WFS_CMD_CIM_CASH_IN_END
   // WFS_CMD_CIM_CASH_IN_ROLLBACK
   // WFS_CMD_CIM_RETRACT
   [TestClass]
   public class WFSCIMCASHINFOTests
   {
      const string GET_INFO_COMPLETE_AS_TABLE =
@"02424294967295017100087401350012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.6590007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x1F937974]01714294967295018600087401360007MONIMNG0008XFSAGENT00102023/01/24001200:59 14.6590004INFO0015Agent::FireTrap0092call WFSAsyncGetInfo(ManagedName=[CashDispenser] hService=[28] dwCategory=[301] hResult=[0])01864294967295022400087401370006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6600011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[17], HWND[0x00010638], REQUESTID[17613], dwCmdCode[301], dwTimeOut[300000]}02244294967295012400087401380006COMMON0003SPI00102023/01/24001200:59 14.6600011INFORMATION0010WFPGetInfo0034HSERVICE=27, SrvcVersion=2563(A03)01244294967295016700087401390006COMMON0009FRAMEWORK00102023/01/24001200:59 14.6600011INFORMATION0028CServiceProvider::GetService0053service OK {HSERVICE[17], LogicalName[CashDispenser]}01674294967295297900087401400003CIM0003SPI00102023/01/24001200:59 14.6600009XFS_EVENT0013GETINFO[1303]2891WFS_GETINFO_COMPLETE, 
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
			fwType			8		9		10		11		12		13		14
			fwItemType		0x0001		0x0001		0x0004		0x0004		0x0004		0x0004		0x0003
			cUnitID			LCU00		LCU01		LCU02		LCU03		LCU04		LCU05		LCU06
			cCurrencyID		   		   		ABC		DEF		GHI		JKL		   
			ulValues		15		16		17		18		19		20		21
			ulCashInCount		22		22		23		24		25		26		27
			ulCount			28		29		30		31		32		33		34
			ulMaximum		35		36		37		38		39		40		41
			usStatus		42		43		44		45		46		47		48
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
			ulPresentedCount	0		0		43		215		190		842		0		
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
			ulInitialCount		49		50		51		52		53		54		55
			ulDispensedCount	56		57		58		59		60		61		62		
			ulPresentedCount	63		64		65		66		67		68		69		
			ulRetractedCount	70		71		72		73		74		75		76		
			ulRejectCount		77		78		79		80		81		82		83
			ulMinimum		84		85		86		87		88		89		90
			lpszExtra(LCU)		NULL		NULL		NULL		NULL		NULL		NULL		NULL
		}
	}
}";
      const string GET_INFO_COMPLETE_AS_LIST =
@"02454294967295022502763227650006COMMON0009FRAMEWORK00102023/03/15001217:54 56.7230011INFORMATION0033CSpCmdDispatcher::DispatchCommand0106pService->GetInfo() {HSERVICE[11], HWND[0x00020276], REQUESTID[32317], dwCmdCode[1303], dwTimeOut[300000]}02254294967295019202763227660003CIM0007ACTIVEX00102023/03/15001217:54 56.7230006XFSAPI0017CService::GetInfo0099WFSAsyncGetInfo(hService=11, dwCategory=1303, lpQueryDetails=0x00000000, RequestID=32317) hResult=001924294967295016602763227670006COMMON0009FRAMEWORK00102023/03/15001217:54 56.7230011INFORMATION0028CServiceProvider::GetService0052service OK {HSERVICE[11], LogicalName[CashAcceptor]}01664294967295016902763227680003CDM0009FRAMEWORK00102023/03/15001217:54 56.7230011INFORMATION0021CBaseService::GetInfo0065Srvc=1303 ReqID=32317 Wnd=0x00020276 Cmd=1303 TimeOut=300000 IO=201694294967295015802763227690012CashAcceptor0009FRAMEWORK00102023/03/15001217:54 56.7230007DEVCALL0021CBaseService::GetInfo0049HSERVICE[11] CATEGORY[1303] IN BUFFER[0x00000000]01584294967295017102763227700012CashAcceptor0009FRAMEWORK00102023/03/15001217:54 56.7230007DEVRETN0021CBaseService::GetInfo0062HSERVICE[11] CATEGORY[1303] HRESULT[0], OUT BUFFER[0x0126EC9C]01714294967295418302763227710003CIM0003SPI00102023/03/15001217:54 56.7230009XFS_EVENT0013GETINFO[1303]4095WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x00020276],
	RequestID = [32317],
	hService = [11],
	tsTimestamp = [2023/03/15 17:54 56.723],
	hResult = [0],
	u.dwCommandCode = [1303],
	lpBuffer = [0x3b1ba334]
	{
		usCount = [7],
		lppCashIn =
		{
			usNumber = [1],
			fwType = [3],
			fwItemType = [0x0001],
			cUnitID = [LCU00],
			cCurrencyID = [   ],
			ulValues = [5],
			ulCashInCount = [7],
			ulCount = [9],
			ulMaximum = [11],
			usStatus = [13],
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
				ulCashInCount = [4],
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
			lpszCashUnitName = [(null)],
			ulInitialCount = [111],
			ulDispensedCount = [15],
			ulPresentedCount = [16],
			ulRetractedCount = [17],
			ulRejectCount = [18],
			ulMinimum = [19]
		}
		{
			usNumber = [2],
			fwType = [4],
			fwItemType = [0x0001],
			cUnitID = [LCU01],
			cCurrencyID = [USD],
			ulValues = [6],
			ulCashInCount = [8],
			ulCount = [10],
			ulMaximum = [12],
			usStatus = [14],
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
					ulCount = [8]
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
					ulCount = [7]
				}
				{
					usNoteID = [0],
					ulCount = [25]
				}
			}
			usNumPhysicalCUs = [1],
			lppPhysical = 
			{
				lpPhysicalPositionName = RejectCassette,
				cUnitID = [RJCST],
				ulCashInCount = [56],
				ulCount = [56],
				ulMaximum = [0],
				usPStatus = [0],
				bHardwareSensor = [1],
				lpszExtra = [SerilNumber=NULLSERIALNUMBER],
				ulInitialCount = [0],
				ulDispensedCount = [0],
				ulPresentedCount = [0],
				ulRetractedCount = [0],
				ulRejectCount = [
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
      public void Test_WFS_INF_CIM_CASH_UNIT_1()
      {
         //IContext ctx = null; 
         //WFSCIMCASHINFO cashInfo = new WFSCIMCASHINFO(ctx);
         //cashInfo.Initialize(samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_1);

         //Assert.IsTrue(cashInfo.lUnitCount == 7);
         //Assert.IsTrue(cashInfo.usNumbers[0] == "1");
         //Assert.IsTrue(cashInfo.fwTypes[0] == "8");
         //Assert.IsTrue(cashInfo.cUnitIDs[0] == "LCU00");
         //Assert.IsTrue(cashInfo.cCurrencyIDs[0] == "");
         //Assert.IsTrue(cashInfo.ulValues[0] == "15");
         //Assert.IsTrue(cashInfo.ulCashInCounts[0] == "22");
         //Assert.IsTrue(cashInfo.ulCounts[0] == "29");
         //Assert.IsTrue(cashInfo.ulMaximums[0] == "36");
         //Assert.IsTrue(cashInfo.usStatuses[0] == "43");
         //Assert.IsTrue(cashInfo.ulInitialCounts[0] == "100");
         //Assert.IsTrue(cashInfo.ulDispensedCounts[0] == "107");
         //Assert.IsTrue(cashInfo.ulPresentedCounts[0] == "114");
         //Assert.IsTrue(cashInfo.ulRetractedCounts[0] == "121");
         //Assert.IsTrue(cashInfo.ulRejectCounts[0] == "128");
         //Assert.IsTrue(cashInfo.ulMinimums[0] == "135");

      }

      [TestMethod]
      public void Test_()
      {

      }

      [TestMethod]
      public void TestTimestamp()
      {
         string timeStamp = lpResult.tsTimestamp(xfsLineTable);
         Assert.IsTrue(timeStamp == "2023-01-24 00:59:14.660");

         timeStamp = lpResult.tsTimestamp(xfsLineList);
         Assert.IsTrue(timeStamp == "2023-03-15 17:54:56.723");

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
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         Assert.IsTrue(lUnitCount == 7);
      }

      [TestMethod]
      public void Test_usNumbersFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.usNumbersFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "1");
         Assert.IsTrue(values[1] == "2");
         Assert.IsTrue(values[2] == "3");
         Assert.IsTrue(values[3] == "4");
         Assert.IsTrue(values[4] == "5");
         Assert.IsTrue(values[5] == "6");
         Assert.IsTrue(values[6] == "7");
      }

      [TestMethod]
      public void Test_usTypesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.fwTypesFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "8");
         Assert.IsTrue(values[1] == "9");
         Assert.IsTrue(values[2] == "10");
         Assert.IsTrue(values[3] == "11");
         Assert.IsTrue(values[4] == "12");
         Assert.IsTrue(values[5] == "13");
         Assert.IsTrue(values[6] == "14");
      }

      [TestMethod]
      public void Test_cUnitIDsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.cUnitIDsFromTable(result.xfsLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "LCU00");
         Assert.IsTrue(values[1] == "LCU01");
         Assert.IsTrue(values[2] == "LCU02");
         Assert.IsTrue(values[3] == "LCU03");
         Assert.IsTrue(values[4] == "LCU04");
         Assert.IsTrue(values[5] == "LCU05");
         Assert.IsTrue(values[6] == "LCU06");
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.cCurrencyIDsFromTable(result.xfsLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "");
         Assert.IsTrue(values[1] == "");
         Assert.IsTrue(values[2] == "ABC");
         Assert.IsTrue(values[3] == "DEF");
         Assert.IsTrue(values[4] == "GHI");
         Assert.IsTrue(values[5] == "JKL");
         Assert.IsTrue(values[6] == "");
      }

      [TestMethod]
      public void Test_ulValuesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulValuesFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "15");
         Assert.IsTrue(values[1] == "16");
         Assert.IsTrue(values[2] == "17");
         Assert.IsTrue(values[3] == "18");
         Assert.IsTrue(values[4] == "19");
         Assert.IsTrue(values[5] == "20");
         Assert.IsTrue(values[6] == "21");
      }

      [TestMethod]
      public void Test_ulCashInCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulCashInCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "22");
         Assert.IsTrue(values[1] == "22");
         Assert.IsTrue(values[2] == "23");
         Assert.IsTrue(values[3] == "24");
         Assert.IsTrue(values[4] == "25");
         Assert.IsTrue(values[5] == "26");
         Assert.IsTrue(values[6] == "27");
      }

      [TestMethod]
      public void Test_ulCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "28");
         Assert.IsTrue(values[1] == "29");
         Assert.IsTrue(values[2] == "30");
         Assert.IsTrue(values[3] == "31");
         Assert.IsTrue(values[4] == "32");
         Assert.IsTrue(values[5] == "33");
         Assert.IsTrue(values[6] == "34");
      }

      [TestMethod]
      public void Test_ulMaximumsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulMaximumsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "35");
         Assert.IsTrue(values[1] == "36");
         Assert.IsTrue(values[2] == "37");
         Assert.IsTrue(values[3] == "38");
         Assert.IsTrue(values[4] == "39");
         Assert.IsTrue(values[5] == "40");
         Assert.IsTrue(values[6] == "41");
      }

      [TestMethod]
      public void Test_usStatusesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.usStatusesFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "42");
         Assert.IsTrue(values[1] == "43");
         Assert.IsTrue(values[2] == "44");
         Assert.IsTrue(values[3] == "45");
         Assert.IsTrue(values[4] == "46");
         Assert.IsTrue(values[5] == "47");
         Assert.IsTrue(values[6] == "48");
      }

      [TestMethod]
      public void Test_ulInitialCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulInitialCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "49");
         Assert.IsTrue(values[1] == "50");
         Assert.IsTrue(values[2] == "51");
         Assert.IsTrue(values[3] == "52");
         Assert.IsTrue(values[4] == "53");
         Assert.IsTrue(values[5] == "54");
         Assert.IsTrue(values[6] == "55");
      }

      [TestMethod]
      public void Test_ulDispensedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulDispensedCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "56");
         Assert.IsTrue(values[1] == "57");
         Assert.IsTrue(values[2] == "58");
         Assert.IsTrue(values[3] == "59");
         Assert.IsTrue(values[4] == "60");
         Assert.IsTrue(values[5] == "61");
         Assert.IsTrue(values[6] == "62");
      }

      [TestMethod]
      public void Test_ulPresentedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulPresentedCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "63");
         Assert.IsTrue(values[1] == "64");
         Assert.IsTrue(values[2] == "65");
         Assert.IsTrue(values[3] == "66");
         Assert.IsTrue(values[4] == "67");
         Assert.IsTrue(values[5] == "68");
         Assert.IsTrue(values[6] == "69");
      }

      [TestMethod]
      public void Test_ulRetractedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulRetractedCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "70");
         Assert.IsTrue(values[1] == "71");
         Assert.IsTrue(values[2] == "72");
         Assert.IsTrue(values[3] == "73");
         Assert.IsTrue(values[4] == "74");
         Assert.IsTrue(values[5] == "75");
         Assert.IsTrue(values[6] == "76");
      }

      [TestMethod]
      public void Test_ulRejectCountsFromTableFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulRejectCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "77");
         Assert.IsTrue(values[1] == "78");
         Assert.IsTrue(values[2] == "79");
         Assert.IsTrue(values[3] == "80");
         Assert.IsTrue(values[4] == "81");
         Assert.IsTrue(values[5] == "82");
         Assert.IsTrue(values[6] == "83");
      }

      [TestMethod]
      public void Test_ulMinimumsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINFO.usCountFromTable(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulMinimumsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "84");
         Assert.IsTrue(values[1] == "85");
         Assert.IsTrue(values[2] == "86");
         Assert.IsTrue(values[3] == "87");
         Assert.IsTrue(values[4] == "88");
         Assert.IsTrue(values[5] == "89");
         Assert.IsTrue(values[6] == "90");
      }


      [TestMethod]
      public void Test_usNumbersFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.usNumbersFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "1");
         Assert.IsTrue(values[1] == "2");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_usTypesFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.fwTypesFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "3");
         Assert.IsTrue(values[1] == "4");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_cUnitIDsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.cUnitIDsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "LCU00");
         Assert.IsTrue(values[1] == "LCU01");
         Assert.IsTrue(values[2] == "");
         Assert.IsTrue(values[3] == "");
         Assert.IsTrue(values[4] == "");
         Assert.IsTrue(values[5] == "");
         Assert.IsTrue(values[6] == "");
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.cCurrencyIDsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "");
         Assert.IsTrue(values[1] == "USD");
         Assert.IsTrue(values[2] == "");
         Assert.IsTrue(values[3] == "");
         Assert.IsTrue(values[4] == "");
         Assert.IsTrue(values[5] == "");
         Assert.IsTrue(values[6] == "");
      }

      [TestMethod]
      public void Test_ulValuesFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulValuesFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "5");
         Assert.IsTrue(values[1] == "6");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_ulCashInCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulCashInCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "7");
         Assert.IsTrue(values[1] == "8");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_ulCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "9");
         Assert.IsTrue(values[1] == "10");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_ulMaximumsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulMaximumsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "11");
         Assert.IsTrue(values[1] == "12");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_usStatusesFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.usStatusesFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "13");
         Assert.IsTrue(values[1] == "14");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      public void Test_ulInitialCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulInitialCountsFromList(result.subLogLine, lUnitCount);
         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "111");
         Assert.IsTrue(values[1] == "0");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      public void Test_ulDispensedCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulDispensedCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount); ;
         Assert.IsTrue(values[0] == "15");
         Assert.IsTrue(values[1] == "0");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_ulPresentedCountsFromLists()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulPresentedCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "16");
         Assert.IsTrue(values[1] == "0");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_ulRetractedCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulRetractedCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "17");
         Assert.IsTrue(values[1] == "0");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_ulRejectCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulRejectCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "18");
         Assert.IsTrue(values[1] == "0");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }

      [TestMethod]
      public void Test_ulMinimumsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCIMCASHINFO.ulMinimumsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 7);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "19");
         Assert.IsTrue(values[1] == "0");
         Assert.IsTrue(values[2] == "0");
         Assert.IsTrue(values[3] == "0");
         Assert.IsTrue(values[4] == "0");
         Assert.IsTrue(values[5] == "0");
         Assert.IsTrue(values[6] == "0");
      }
   }
}
