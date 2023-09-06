using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSSYSEVENTTests
   {
      string xfsLineList = string.Empty;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLineList = samples_general.WFSSYSEVENT_1;
      }

      [TestMethod]
      public void TestTimestamp()
      {
         string timeStamp = lpResult.tsTimestamp(xfsLineList);
         Assert.IsTrue(timeStamp == "2023-02-27 07:35:53.824");

      }
      [TestMethod]
      public void TesthResult()
      {
         string hResult = lpResult.hResult(xfsLineList);
         Assert.IsTrue(hResult == "");

      }
      [TestMethod]
      public void Test_logicalNameFromSysEvent()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SYSEVENT);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSSYSEVENT.logicalNameFromSysEvent(xfsLineList);
         Assert.IsTrue(result2.success);
         Assert.IsTrue("CashAcceptor" == result2.xfsMatch.Trim());
      }
      [TestMethod]
      public void Test_physicalNameFromSysEvent()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SYSEVENT);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSSYSEVENT.physicalNameFromSysEvent(xfsLineList);
         Assert.IsTrue(result2.success);
         Assert.IsTrue("NHCCIM" == result2.xfsMatch.Trim());
      }
   }
}
