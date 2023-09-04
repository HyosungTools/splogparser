using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSPINSTATUSTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_pin.WFS_INF_PIN_STATUS;
      }

      [TestMethod]
      public void Test_PIN_fwDevice()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.fwDeviceFromPINStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }

      [TestMethod]
      public void Test_PIN_fwEncStat()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.fwEncStatFromPINStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_PIN_fwAutoBeepMode()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.fwAutoBeepModeFromPINStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }
      [TestMethod]
      public void Test_PIN_dwCertificateState()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.dwCertificateStateFromPINStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_PIN_wDevicePosition()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.wDevicePositionFromPINStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "3");
      }
      [TestMethod]
      public void Test_PIN_usPowerSaveRecoveryTime()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.usPowerSaveRecoveryTimeFromSDMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_PIN_wAntiFraudModule()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.wAntiFraudModuleFromPINStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_PIN_SP_Version()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.SP_VersionFromStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "V 04.22.11");
      }
      [TestMethod]
      public void Test_PIN_EP_Version()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.EP_VersionFromStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "V 15.00.03");
      }
      [TestMethod]
      public void Test_PIN_ErrorCode()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSPINSTATUS.errorCodeFromPINStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0000000");
      }
   }
}
