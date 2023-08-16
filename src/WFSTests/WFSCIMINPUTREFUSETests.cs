using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSCIMInputRefuseTests
   {

      string xfsLineList = string.Empty;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLineList = samples_cim.WFS_CIM_INPUT_REFUSE_1;

      }

      [TestMethod]
      public void TestTimestamp()
      {
         string timeStamp = lpResult.tsTimestamp(xfsLineList);
         Assert.IsTrue(timeStamp == "2023-03-30 16:04:44.157");

      }
      [TestMethod]
      public void TesthResult()
      {
         string hResult = lpResult.hResult(xfsLineList);
         Assert.IsTrue(hResult == "");

      }
      [TestMethod]
      public void Test_usReasonFromList_1()
      {
         xfsLineList = samples_cim.WFS_CIM_INPUT_REFUSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_EXEE_CIM_INPUTREFUSE);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMINPUTREFUSE.usReasonFromList(xfsLineList);
         int lReason = int.Parse(result2.xfsMatch.Trim());
         Assert.IsTrue(lReason == 1);
      }

      [TestMethod]
      public void Test_usReasonFromList_2()
      {
         xfsLineList = samples_cim.WFS_CIM_INPUT_REFUSE_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_EXEE_CIM_INPUTREFUSE);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMINPUTREFUSE.usReasonFromList(xfsLineList);
         int lReason = int.Parse(result2.xfsMatch.Trim());
         Assert.IsTrue(lReason == 2);
      }

      [TestMethod]
      public void Test_usReasonFromList_3()
      {
         xfsLineList = samples_cim.WFS_CIM_INPUT_REFUSE_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_EXEE_CIM_INPUTREFUSE);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMINPUTREFUSE.usReasonFromList(xfsLineList);
         int lReason = int.Parse(result2.xfsMatch.Trim());
         Assert.IsTrue(lReason == 3);
      }

   }
}
