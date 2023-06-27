using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSIDCSTATUSTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_idc.WFS_INF_IDC_STATUS;
      }

      [TestMethod]
      public void Test_IDC_fwDevice()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.fwDeviceFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }

      [TestMethod]
      public void Test_IDC_fwMedia()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.fwMediaFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "7");
      }
      [TestMethod]
      public void Test_IDC_fwRetainBin()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.fwRetainBinFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "2");
      }
      [TestMethod]
      public void Test_IDC_fwSecurity()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.fwSecurityFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }
      [TestMethod]
      public void Test_IDC_usCards()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.usCardsFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_IDC_fwChipPower()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.fwChipPowerFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_IDC_fwChipModule()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.fwChipModuleFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_IDC_fwMagReadModule()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.fwMagReadModuleFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");
      }
      [TestMethod]
      public void Test_IDC_SPVersion()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.SPVersionFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "V 04.30.36");
      }
      [TestMethod]
      public void Test_IDC_EPVersion()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.EPVersionFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "5293-01M");
      }
      [TestMethod]
      public void Test_IDC_ErrorCode()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIDCSTATUS.errorCodeFromIDCStatus(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0000000");
      }
           
   }
}
