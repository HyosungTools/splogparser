using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSCIMSTATUSTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_cim.WFS_INF_CIM_STATUS_1;
      }

      [TestMethod]
      public void fwDevice()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMSTATUS.fwDeviceFromCIMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }
      [TestMethod]
      public void fwSafeDoor()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMSTATUS.fwSafeDoorFromCIMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "2");
      }
      [TestMethod]
      public void fwAcceptor()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMSTATUS.fwAcceptorFromCIMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "3");
      }
      [TestMethod]
      public void fwIntermediateStacker()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMSTATUS.fwIntermediateStackerFromCIMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "4");
      }
      [TestMethod]
      public void fwStackerItems()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMSTATUS.fwStackerItemsFromCIMStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "5");
      }
   }
}
