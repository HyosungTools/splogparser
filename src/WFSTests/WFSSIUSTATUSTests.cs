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
   }
}
