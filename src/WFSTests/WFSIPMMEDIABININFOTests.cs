using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSIPMMEDIABININFOTests
   {
      private string xfsLine;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_ipm.WFS_INF_IPM_MEDIA_BIN_INFO;
      }

      [TestMethod]
      public void usCount()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         Assert.IsTrue(lUnitCount == 2);
      }

      [TestMethod]
      public void usBinNumbers()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.usBinNumbersFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "1");
         Assert.IsTrue(values[1] == "2");
      }

      [TestMethod]
      public void fwTypes()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.fwTypesFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "0x0002");
         Assert.IsTrue(values[1] == "0x0001");
      }

      [TestMethod]
      public void wMediaTypes()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.wMediaTypesFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "0x0003");
         Assert.IsTrue(values[1] == "0x0002");
      }

      [TestMethod]
      public void lpstrBinIDs()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.lpstrBinIDsFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "ALL");
         Assert.IsTrue(values[1] == "CHECK");
      }

      [TestMethod]
      public void ulMediaInCounts()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.ulMediaInCountsFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "4");
         Assert.IsTrue(values[1] == "11");
      }

      [TestMethod]
      public void ulCounts()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.ulCountsFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "5");
         Assert.IsTrue(values[1] == "12");
      }

      [TestMethod]
      public void ulRetractOperations()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.ulRetractOperationsFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "6");
         Assert.IsTrue(values[1] == "13");
      }

      [TestMethod]
      public void ulMaximumItems()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.ulMaximumItemsFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "8");
         Assert.IsTrue(values[1] == "15");
      }

      [TestMethod]
      public void ulMaximumRetractOperations()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.ulMaximumRetractOperationsFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "9");
         Assert.IsTrue(values[1] == "16");
      }

      [TestMethod]
      public void usStatuses()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

         (bool success, string xfsMatch, string subLogLine) result2 = WFSIPMMEDIABININFO.usCountFromList(result.xfsLine);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());

         string[] values = WFSIPMMEDIABININFO.usStatusesFromList(result2.subLogLine, lUnitCount);
         Assert.IsTrue(values.Length == 2);
         Assert.IsTrue(values[0] == "10");
         Assert.IsTrue(values[1] == "17");
      }
   }
}
