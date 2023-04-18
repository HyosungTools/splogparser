using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSIPMTRANSSTATUSTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_ipm.WFS_INF_IPM_TRANSACTION_STATUS;
      }

      [TestMethod]
      public void wMediaInTransaction()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.wMediaInTransactionFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }

      [TestMethod]
      public void usMediaOnStacker()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.usMediaOnStackerFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "2");
      }

      [TestMethod]
      public void usLastMediaInTotal()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.usLastMediaInTotalFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "3");
      }

      [TestMethod]
      public void usLastMediaAddedToStacker()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.usLastMediaAddedToStackerFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "4");
      }

      [TestMethod]
      public void usTotalItems()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.usTotalItemsFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "5");
      }

      [TestMethod]
      public void usTotalItemsRefused()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.usTotalItemsRefusedFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "6");
      }

      [TestMethod]
      public void usTotalBunchesRefused()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.usTotalBunchesRefusedFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "7");
      }

      [TestMethod]
      public void lpszExtra()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMTRANSSTATUS.lpszExtraFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "The Media-In transaction is active.");
      }
   }
}
