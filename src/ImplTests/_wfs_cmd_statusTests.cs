using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class _wfs_cmd_statusTests
   {
      static string logLine = @"
	{
		fwDevice = [0],
		fwSafeDoor = [1],
		fwDispenser = [2],
		fwIntermediateStacker = [3],
		lppPositions =
		{
			fwPosition = [4],
			fwShutter = [5],
			fwPositionStatus = [6],
			fwTransport = [7],
			fwTransportStatus = [8]
		}
		lpszExtra = [ErrorCode=1234567,Description=[0000000]System OK!,ErrCode=0000000,ErrMsg=System OK!,Position=Unknown,SP_Version=V 04.21.28,EP_Version=V 02.00.97 ,Boot_Version=D 02.00.05,CST1_SerialNo=CDGR281456,CST2_SerialNo=CDGR281467,CST3_SerialNo=CDGR305935,CST4_...],
		dwGuidLights =
			[0x00000000], [0x00000000], [0x00000000], [0x00000000],
			[0x00000000], [0x00000000], [0x00000000], [0x00000000]
		wDevicePosition = [9],
		usPowerSaveRecoveryTime = [10],
		wAntiFraudModule = [11]
	}
}
";
      [TestMethod]
      public void fwDevice()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwDevice(logLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void fwSafeDoor()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwSafeDoor(logLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }
      [TestMethod]
      public void fwDispenser()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwDispenser(logLine);
         Assert.IsTrue(result.xfsMatch == "2");
      }
      [TestMethod]
      public void fwIntermediateStacker()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwIntermediateStacker(logLine);
         Assert.IsTrue(result.xfsMatch == "3");
      }
      [TestMethod]
      public void fwPosition()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwPosition(logLine);
         Assert.IsTrue(result.xfsMatch == "4");
      }
      [TestMethod]
      public void fwShutter()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwShutter(logLine);
         Assert.IsTrue(result.xfsMatch == "5");
      }
      [TestMethod]
      public void fwPositionStatus()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwPositionStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "6");
      }
      [TestMethod]
      public void fwTransport()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwTransport(logLine);
         Assert.IsTrue(result.xfsMatch == "7");
      }
      [TestMethod]
      public void fwTransportStatus()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.fwTransportStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "8");
      }
      [TestMethod]
      public void wDevicePosition()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.wDevicePosition(logLine);
         Assert.IsTrue(result.xfsMatch == "9");
      }
      [TestMethod]
      public void usPowerSaveRecoveryTime()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.usPowerSaveRecoveryTime(logLine);
         Assert.IsTrue(result.xfsMatch == "10");
      }
      [TestMethod]
      public void wAntiFraudModule()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_status.wAntiFraudModule(logLine);
         Assert.IsTrue(result.xfsMatch == "11");
      }
   }
}
