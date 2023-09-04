using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSSIUSTATUSTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_siu.WFS_INF_SIU_STATUS;
      }

      [TestMethod]
      public void Test_SIU_fwSafeDoor()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.fwSafeDoorFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0x0001");
      }

      [TestMethod]
      public void Test_SIU_fwDevice()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.fwDeviceFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_SIU_opSwitch()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.opSwitchFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0x0001");
      }
      [TestMethod]
      public void Test_SIU_tamper()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.tamperFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0x0000");
      }
      [TestMethod]
      public void Test_SIU_intTamper()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.intTamperFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0x0000");
      }
      [TestMethod]
      public void Test_SIU_cabinet()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.cabinetFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0x0001");
      }
      [TestMethod]
      public void Test_SIU_SP_Version()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.SP_VersionFromStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "V 04.20.66");
      }
      [TestMethod]
      public void Test_SIU_EP_Version()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.EP_VersionFromStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "V020031");
      }
      [TestMethod]
      public void Test_SIU_EP_errorCode()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.errorCodeFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0000000");
      }

      [TestMethod]
      public void Test_SIU_description()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSSIUSTATUS.descriptionFromSIUStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "Success.System OK!");
      }
   }
}
