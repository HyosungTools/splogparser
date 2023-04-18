using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSIPMMEDIAINENDTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_ipm.WFS_CMD_IPM_MEDIA_END;
      }

      [TestMethod]
      public void usItemsReturned()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMMEDIAINEND.usItemsReturnedFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }

      [TestMethod]
      public void usItemsRefused()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMMEDIAINEND.usItemsRefusedFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "2");
      }

      [TestMethod]
      public void usBunchesRefused()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMMEDIAINEND.usBunchesRefusedFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "3");
      }
   }
}
