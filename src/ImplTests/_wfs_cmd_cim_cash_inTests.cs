using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class _wfs_cmd_cim_cash_inTests
   {
      private const string WFS_EXECUTE_COMPLETE =
@"05024294967295016900088859740012CashAcceptor0009FRAMEWORK00102023/01/24001212:33 51.3080007DEVRETN0021CBaseService::Execute0060HSERVICE[12] COMMAND[1302] HRESULT[0] OUT BUFFER[0x014FB7B4]01694294967295012200088859750006COMMON0009FRAMEWORK00102023/01/24001212:33 51.3090011INFORMATION0015CProcessor::Run0021---- Wait [2148] ----01224294967295046500088859760003CIM0003SPI00102023/01/24001212:33 51.3090009XFS_EVENT0013EXECUTE[1302]0377WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000201e0],
	RequestID = [3749],
	hService = [12],
	tsTimestamp = [2023/01/24 12:33 51.309],
	hResult = [0],
	u.dwCommandCode = [1302],
	lpBuffer = [0x35d1c984]

	{
		usNumOfNoteNumbers = [2],
		lppNoteNumber = 
		{
			usNoteID = [16],
			ulCount = [10]
		}
		{
			usNoteID = [17],
			ulCount = [37]
		}
	}
}";

      private const string WFS_EXECUTE_COMPLETE2 =
@"25754294967295016900055891050012CashAcceptor0009FRAMEWORK00102023/02/24001211:30 35.5370007DEVRETN0021CBaseService::Execute0060HSERVICE[12] COMMAND[1302] HRESULT[0] OUT BUFFER[0x0070679C]01694294967295012000055891060004CCIM0002SP00102023/02/24001211:30 35.5380011INFORMATION0017CCCIMDev::_Sensor0026BDS05 sensor changed(1->0)01204294967295012200055891070006COMMON0009FRAMEWORK00102023/02/24001211:30 35.5380011INFORMATION0015CProcessor::Run0021---- Wait [4588] ----01224294967295041400055891080003CIM0003SPI00102023/02/24001211:30 35.5380009XFS_EVENT0013EXECUTE[1302]0326WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000200b6],
	RequestID = [3714],
	hService = [12],
	tsTimestamp = [2023/02/24 11:30 35.538],
	hResult = [0],
	u.dwCommandCode = [1302],
	lpBuffer = [0x1e3de4a4]

	{
		usNumOfNoteNumbers = [1],
		lppNoteNumber = 
		{
			usNoteID = [17],
			ulCount = [1]
		}
	}
}";

      private string xfsLine = string.Empty;
      private string xfsLine2 = string.Empty; 

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = WFS_EXECUTE_COMPLETE;
         xfsLine2 = WFS_EXECUTE_COMPLETE2;
      }

      [TestMethod]
      public void Test_Counts()
      {
         (int usCount, string subLogLine) result = _wfs_cmd_cim_cash_in.usNumOfNoteNumbers(xfsLine);
         Assert.IsTrue(result.usCount == 2);
      }

      [TestMethod]
      void Test_Counts2()
      {
         (int usCount, string subLogLine) result = _wfs_cmd_cim_cash_in.usNumOfNoteNumbers(xfsLine2);
         Assert.IsTrue(result.usCount == 1);
      }

      [TestMethod]
      public void Test_usNoteIDsFromList()
      {
         (int usCount, string subLogLine) result = _wfs_cmd_cim_cash_in.usNumOfNoteNumbers(xfsLine);
         string[] values = _wfs_cmd_cim_cash_in.usNoteIDsFromList(xfsLine);

         Assert.IsTrue(result.usCount == 2);
         Assert.IsTrue(values.Length == result.usCount);
         Assert.IsTrue(values[0] == "16");
         Assert.IsTrue(values[1] == "17");
      }

      [TestMethod]
      public void Test_ulCountsFromList()
      {
         (int usCount, string subLogLine) result = _wfs_cmd_cim_cash_in.usNumOfNoteNumbers(xfsLine);
         string[] values = _wfs_cmd_cim_cash_in.ulCountsFromList(xfsLine);

         Assert.IsTrue(result.usCount == 2);
         Assert.IsTrue(values.Length == result.usCount);
         Assert.IsTrue(values[0] == "10");
         Assert.IsTrue(values[1] == "37");
      }
   }
}
