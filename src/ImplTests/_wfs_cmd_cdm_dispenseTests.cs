using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class _wfs_cmd_cdm_dispenseTests
   {
      private string xfsLine = string.Empty; 

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = 
@"07.2420009XFS_EVENT0012EXECUTE[302]0341WFS_EXECUTE_COMPLETE, 
lpResult =
{
	hWnd = [0x000104b4],
	RequestID = [42698],
	hService = [40],
	tsTimestamp = [2022/11/17 06:36 07.242],
	hResult = [0],
	u.dwCommandCode = [302],
	lpBuffer = [0x0404767c]
	{
		cCurrencyID = [USD],
		ulAmount = [100],
		usCount = [5],
		lpulValues = [1, 2, 3, 4, 5],
		ulCashBox = [0]
	}
}";
      }

      [TestMethod]
      public void Test_cCurrencyID()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_cdm_dispense.cCurrencyID(xfsLine);
         Assert.IsTrue(result.success);
         Assert.IsTrue(result.xfsMatch.Trim() == "USD");
      }

      [TestMethod]
      public void Test_ulAmount()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_cdm_dispense.ulAmount(xfsLine);
         Assert.IsTrue(result.success);
         Assert.IsTrue(result.xfsMatch.Trim() == "100");

      }

      [TestMethod]
      public void Test_usCount()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_cdm_dispense.usCount(xfsLine);
         Assert.IsTrue(result.success);
         Assert.IsTrue(result.xfsMatch.Trim() == "5");
      }

      [TestMethod]
      public void Test_lpulValues()
      {
         (bool success, string[] xfsMatch, string subLogLine) results = _wfs_cmd_cdm_dispense.lpulValues(xfsLine);
         Assert.IsTrue(results.success);
         Assert.IsTrue(results.xfsMatch.Length == 5);
         Assert.IsTrue(results.xfsMatch[0] == "1");
         Assert.IsTrue(results.xfsMatch[1] == "2");
         Assert.IsTrue(results.xfsMatch[2] == "3");
         Assert.IsTrue(results.xfsMatch[3] == "4");
         Assert.IsTrue(results.xfsMatch[4] == "5");

      }

      [TestMethod]
      public void Test_ulCashBox()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_cmd_cdm_dispense.ulCashBox(xfsLine);
         Assert.IsTrue(result.success);
         Assert.IsTrue(result.xfsMatch.Trim() == "0");
      }
   }
}
