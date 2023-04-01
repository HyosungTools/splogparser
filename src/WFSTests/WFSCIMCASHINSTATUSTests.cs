using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSCIMCASHINSTATUSTests
   {
      [TestMethod]
      public void Test_wStatusFromList()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_1);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINSTATUS.wStatusFromList(result.xfsLine);
         Assert.IsTrue(result2.xfsMatch.Trim() == "2");
      }

      [TestMethod]
      public void Test_usNumOfRefusedFromList()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_1);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCIMCASHINSTATUS.usNumOfRefusedFromList(result.xfsLine);
         Assert.IsTrue(result2.xfsMatch.Trim() == "3");
      }

      [TestMethod]
      public void Test_noteNumbersFromList()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_1);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);

         (bool success, string[,] xfsMatch, string subLogLine) results = WFSCIMCASHINSTATUS.noteNumbersFromList(result.xfsLine);
         Assert.IsTrue(results.xfsMatch[0, 0] == null);
      }
   }
}
