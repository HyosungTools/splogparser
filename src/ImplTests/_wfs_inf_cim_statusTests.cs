using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class _wfs_inf_cim_statusTests
   {
      string xfsLine =
@"
02444294967295016900087405680003CDM0009FRAMEWORK00102023/01/24001200:59 14.7750011INFORMATION0021CBaseService::GetInfo0065Srvc=1301 ReqID=17639 Wnd=0x000201A0 Cmd=1301 TimeOut=300000 IO=201694294967295013500087405690003CDM0007ACTIVEX00102023/01/24001200:59 14.7760005EVENT0038CCdmService::HandleCashUnitInfoChanged0022FireCashUnitChanged[6]01354294967295018600087405700007MONIMNG0008XFSAGENT00102023/01/24001200:59 14.7760004INFO0015Agent::FireTrap0092call WFSAsyncGetInfo(ManagedName=[CashAcceptor] hService=[27] dwCategory=[1303] hResult=[0])01864294967295015800087405710012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.7770007DEVCALL0021CBaseService::GetInfo0049HSERVICE[12] CATEGORY[1301] IN BUFFER[0x00000000]01584294967295013500087405720003CDM0007ACTIVEX00102023/01/24001200:59 14.7770005EVENT0038CCdmService::HandleCashUnitInfoChanged0022FireCashUnitChanged[5]01354294967295017100087405730012CashAcceptor0009FRAMEWORK00102023/01/24001200:59 14.7770007DEVRETN0021CBaseService::GetInfo0062HSERVICE[12] CATEGORY[1301] HRESULT[0], OUT BUFFER[0x014CC244]01714294967295013500087405740003CDM0007ACTIVEX00102023/01/24001200:59 14.7770005EVENT0038CCdmService::HandleCashUnitInfoChanged0022FireCashUnitChanged[4]01354294967295022400087405750006COMMON0009FRAMEWORK00102023/01/24001200:59 14.7770011INFORMATION0033CSpCmdDispatcher::DispatchCommand0105pService->GetInfo() {HSERVICE[27], HWND[0x000206a8], REQUESTID[17640], dwCmdCode[1303], dwTimeOut[60000]}02244294967295160900087405760003CIM0003SPI00102023/01/24001200:59 14.7770009XFS_EVENT0013GETINFO[1301]1521WFS_GETINFO_COMPLETE, 
lpResult =
{
	hWnd = [0x000201a0],
	RequestID = [17639],
	hService = [12],
	tsTimestamp = [2023/01/24 00:59 14.777],
	hResult = [0],
	u.dwCommandCode = [1301],
	lpBuffer = [0x792a55fc]
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

      [TestInitialize]
      public void TestInitialize()
      {
      }

      [TestMethod]
      public void fwDevice()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_status.fwDevice(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void fwSafeDoor()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_status.fwSafeDoor(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }
      [TestMethod]
      public void fwAcceptor()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_status.fwAcceptor(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }
      [TestMethod]
      public void fwIntermediateStacker()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_status.fwIntermediateStacker(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void fwStackerItems()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_status.fwStackerItems(xfsLine);
         Assert.IsTrue(result.xfsMatch == "4");
      }
   }
}
