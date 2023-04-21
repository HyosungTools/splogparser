using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSIPMSTATUSTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_ipm.WFS_INF_IPM_STATUS;
      }

      [TestMethod]
      public void fwDevice()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMSTATUS.fwDeviceFromIPMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }

      [TestMethod]
      public void wAcceptor()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMSTATUS.wAcceptorFromIPMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "2");
      }

      [TestMethod]
      public void wMedia()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMSTATUS.wMediaFromIPMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "3");
      }

      [TestMethod]
      public void wStacker()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMSTATUS.wStackerFromIPMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "9");
      }

      [TestMethod]
      public void wMixedMode()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMSTATUS.wMixedModeFromIPMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "27");
      }

      [TestMethod]
      public void wAntiFraudModule()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMSTATUS.wAntiFraudModuleFromIPMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "28");
      }
   }
}
