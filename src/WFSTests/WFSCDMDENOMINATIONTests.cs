using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSCDMDENOMINATIONTests
   {
      string xfsLineList = string.Empty;

      [TestMethod]
      public void Test_cCurrencyIDFromList()
      {
         xfsLineList = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_DISPENSE);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMDENOMINATION.cCurrencyIDFromList(result.xfsLine);
         Assert.IsTrue(result2.xfsMatch == "USD");
      }

      [TestMethod]
      public void Test_ulAmountFromList()
      {
         xfsLineList = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_DISPENSE);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMDENOMINATION.ulAmountFromList(result.xfsLine);
         Assert.IsTrue(result2.xfsMatch == "30");
      }

      [TestMethod]
      public void Test_usCountFromList()
      {
         xfsLineList = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_DISPENSE);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMDENOMINATION.usCountFromList(result.xfsLine);
         Assert.IsTrue(result2.xfsMatch == "31");
      }

      [TestMethod]
      public void Test_lpulValuesFromList()
      {
         xfsLineList = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_DISPENSE);

         (bool success, string[] xfsMatch, string subLogLine) result2 = WFSCDMDENOMINATION.lpulValuesFromList(result.xfsLine);
         Assert.IsTrue(result2.xfsMatch.Length == 6);
         Assert.IsTrue(result2.xfsMatch[0] == "32");
         Assert.IsTrue(result2.xfsMatch[1] == "33");
         Assert.IsTrue(result2.xfsMatch[2] == "34");
         Assert.IsTrue(result2.xfsMatch[3] == "35");
         Assert.IsTrue(result2.xfsMatch[4] == "36");
         Assert.IsTrue(result2.xfsMatch[5] == "37");
      }

      [TestMethod]
      public void Test_ulCashBoxFromList()
      {
         xfsLineList = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineList);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_DISPENSE);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMDENOMINATION.ulCashBoxFromList(result.xfsLine);
         Assert.IsTrue(result2.xfsMatch == "38");
      }
   }
}
