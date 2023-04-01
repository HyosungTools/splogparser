using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSCDMCUINFOTests
   {

      string xfsLineTable = string.Empty;
      string xfsLineList = string.Empty;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLineTable = samples_cdm.GET_INFO_COMPLETE_AS_TABLE;
         xfsLineList = samples_cdm.GET_INFO_COMPLETE_AS_LIST;
      }

      [TestMethod]
      public void TestTimestamp()
      {
         string timeStamp = lpResult.tsTimestamp(xfsLineTable);
         Assert.IsTrue(timeStamp == "2022/12/19 16:01 26.018");

      }
      [TestMethod]
      public void TesthResult()
      {
         string hResult = lpResult.hResult(xfsLineTable);
         Assert.IsTrue(hResult == "");

      }
      [TestMethod]
      public void Test_usCountFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         Assert.IsTrue(lUnitCount == 6);
      }

      [TestMethod]
      public void Test_usNumbersFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.usNumbersFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "1");
         Assert.IsTrue(values[1] == "2");
         Assert.IsTrue(values[2] == "3");
         Assert.IsTrue(values[3] == "4");
         Assert.IsTrue(values[4] == "5");
         Assert.IsTrue(values[5] == "6");
      }

      [TestMethod]
      public void Test_usTypesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.usTypesFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "6");
         Assert.IsTrue(values[1] == "7");
         Assert.IsTrue(values[2] == "8");
         Assert.IsTrue(values[3] == "9");
         Assert.IsTrue(values[4] == "10");
         Assert.IsTrue(values[5] == "11");
      }

      [TestMethod]
      public void Test_cUnitIDsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.cUnitIDsFromTable(result.xfsLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "LCU00");
         Assert.IsTrue(values[1] == "LCU01");
         Assert.IsTrue(values[2] == "LCU02");
         Assert.IsTrue(values[3] == "LCU03");
         Assert.IsTrue(values[4] == "LCU04");
         Assert.IsTrue(values[5] == "LCU05");
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.cCurrencyIDsFromTable(result.xfsLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "");
         Assert.IsTrue(values[1] == "");
         Assert.IsTrue(values[2] == "USA");
         Assert.IsTrue(values[3] == "USB");
         Assert.IsTrue(values[4] == "USC");
         Assert.IsTrue(values[5] == "USD");
      }

      [TestMethod]
      public void Test_ulValuesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulValuesFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "12");
         Assert.IsTrue(values[1] == "13");
         Assert.IsTrue(values[2] == "14");
         Assert.IsTrue(values[3] == "15");
         Assert.IsTrue(values[4] == "16");
         Assert.IsTrue(values[5] == "17");
      }

      [TestMethod]
      public void Test_ulInitialCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulInitialCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "18");
         Assert.IsTrue(values[1] == "19");
         Assert.IsTrue(values[2] == "20");
         Assert.IsTrue(values[3] == "21");
         Assert.IsTrue(values[4] == "22");
         Assert.IsTrue(values[5] == "23");
      }

      [TestMethod]
      public void Test_ulCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "24");
         Assert.IsTrue(values[1] == "25");
         Assert.IsTrue(values[2] == "26");
         Assert.IsTrue(values[3] == "27");
         Assert.IsTrue(values[4] == "28");
         Assert.IsTrue(values[5] == "29");
      }

      [TestMethod]
      public void Test_ulRejectCountsFromTableFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulRejectCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "30");
         Assert.IsTrue(values[1] == "31");
         Assert.IsTrue(values[2] == "32");
         Assert.IsTrue(values[3] == "33");
         Assert.IsTrue(values[4] == "34");
         Assert.IsTrue(values[5] == "35");
      }

      [TestMethod]
      public void Test_ulMinimumsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulMinimumsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "36");
         Assert.IsTrue(values[1] == "37");
         Assert.IsTrue(values[2] == "38");
         Assert.IsTrue(values[3] == "39");
         Assert.IsTrue(values[4] == "40");
         Assert.IsTrue(values[5] == "41");
      }

      [TestMethod]
      public void Test_ulMaximumsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulMaximumsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "42");
         Assert.IsTrue(values[1] == "43");
         Assert.IsTrue(values[2] == "44");
         Assert.IsTrue(values[3] == "45");
         Assert.IsTrue(values[4] == "46");
         Assert.IsTrue(values[5] == "47");
      }

      [TestMethod]
      public void Test_usStatusesFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.usStatusesFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "48");
         Assert.IsTrue(values[1] == "49");
         Assert.IsTrue(values[2] == "50");
         Assert.IsTrue(values[3] == "51");
         Assert.IsTrue(values[4] == "52");
         Assert.IsTrue(values[5] == "53");
      }

      [TestMethod]
      public void Test_ulDispensedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulDispensedCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "54");
         Assert.IsTrue(values[1] == "55");
         Assert.IsTrue(values[2] == "56");
         Assert.IsTrue(values[3] == "57");
         Assert.IsTrue(values[4] == "58");
         Assert.IsTrue(values[5] == "59");
      }

      [TestMethod]
      public void Test_ulPresentedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulPresentedCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "60");
         Assert.IsTrue(values[1] == "61");
         Assert.IsTrue(values[2] == "62");
         Assert.IsTrue(values[3] == "63");
         Assert.IsTrue(values[4] == "64");
         Assert.IsTrue(values[5] == "65");
      }

      [TestMethod]
      public void Test_ulRetractedCountsFromTable()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLineTable);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMCUINFO.usCountFromTable(xfsLineTable);
         int lUnitCount = int.Parse(result2.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.ulRetractedCountsFromTable(result.xfsLine);

         Assert.IsTrue(lUnitCount == 6);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "66");
         Assert.IsTrue(values[1] == "67");
         Assert.IsTrue(values[2] == "68");
         Assert.IsTrue(values[3] == "69");
         Assert.IsTrue(values[4] == "70");
         Assert.IsTrue(values[5] == "71");
      }


      [TestMethod]
      public void Test_usNumbersFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.usNumbersFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "1");
         Assert.IsTrue(values[1] == "2");
         Assert.IsTrue(values[2] == "3");
         Assert.IsTrue(values[3] == "4");
         Assert.IsTrue(values[4] == "5");
      }

      [TestMethod]
      public void Test_usTypesFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] values = WFSCDMCUINFO.usTypesFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(values.Length == lUnitCount);
         Assert.IsTrue(values[0] == "11");
         Assert.IsTrue(values[1] == "21");
         Assert.IsTrue(values[2] == "31");
         Assert.IsTrue(values[3] == "41");
         Assert.IsTrue(values[4] == "51");
      }
      [TestMethod]
      public void Test_cUnitIDsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.cUnitIDsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "LCU00");
         Assert.IsTrue(results[1] == "LCU01");
         Assert.IsTrue(results[2] == "LCU02");
         Assert.IsTrue(results[3] == "LCU03");
         Assert.IsTrue(results[4] == "LCU04");
      }

      [TestMethod]
      public void Test_cCurrencyIDsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.cCurrencyIDsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "");
         Assert.IsTrue(results[1] == "USD");
         Assert.IsTrue(results[2] == "ABC");
         Assert.IsTrue(results[3] == "CAD");
         Assert.IsTrue(results[4] == "FRA");
      }

      [TestMethod]
      public void Test_ulValuesFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulValuesFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "12");
         Assert.IsTrue(results[1] == "22");
         Assert.IsTrue(results[2] == "32");
         Assert.IsTrue(results[3] == "42");
         Assert.IsTrue(results[4] == "52");
      }

      [TestMethod]
      public void Test_ulInitialCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulInitialCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "13");
         Assert.IsTrue(results[1] == "23");
         Assert.IsTrue(results[2] == "33");
         Assert.IsTrue(results[3] == "43");
         Assert.IsTrue(results[4] == "53");
      }

      [TestMethod]
      public void Test_ulCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "14");
         Assert.IsTrue(results[1] == "24");
         Assert.IsTrue(results[2] == "34");
         Assert.IsTrue(results[3] == "44");
         Assert.IsTrue(results[4] == "54");
      }

      public void Test_ulRejectCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulRejectCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "15");
         Assert.IsTrue(results[1] == "25");
         Assert.IsTrue(results[2] == "35");
         Assert.IsTrue(results[3] == "45");
         Assert.IsTrue(results[4] == "55");
      }

      [TestMethod]
      public void Test_ulMinimumsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulMinimumsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "16");
         Assert.IsTrue(results[1] == "26");
         Assert.IsTrue(results[2] == "36");
         Assert.IsTrue(results[3] == "46");
         Assert.IsTrue(results[4] == "56");
      }

      [TestMethod]
      public void Test_ulMaximumsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulMaximumsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "17");
         Assert.IsTrue(results[1] == "27");
         Assert.IsTrue(results[2] == "37");
         Assert.IsTrue(results[3] == "47");
         Assert.IsTrue(results[4] == "57");
      }

      [TestMethod]
      public void Test_usStatusesFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.usStatusesFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "18");
         Assert.IsTrue(results[1] == "28");
         Assert.IsTrue(results[2] == "38");
         Assert.IsTrue(results[3] == "48");
         Assert.IsTrue(results[4] == "58");
      }

      [TestMethod]
      public void Test_ulDispensedCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulDispensedCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "19");
         Assert.IsTrue(results[1] == "29");
         Assert.IsTrue(results[2] == "39");
         Assert.IsTrue(results[3] == "49");
         Assert.IsTrue(results[4] == "59");
      }

      [TestMethod]
      public void Test_ulPresentedCountsFromLists()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulPresentedCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "110");
         Assert.IsTrue(results[1] == "210");
         Assert.IsTrue(results[2] == "310");
         Assert.IsTrue(results[3] == "410");
         Assert.IsTrue(results[4] == "510");
      }

      [TestMethod]
      public void Test_ulRetractedCountsFromList()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMCUINFO.usCountFromList(xfsLineList);
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[] results = WFSCDMCUINFO.ulRetractedCountsFromList(result.subLogLine, lUnitCount);

         Assert.IsTrue(lUnitCount == 5);
         Assert.IsTrue(results.Length == lUnitCount);
         Assert.IsTrue(results[0] == "111");
         Assert.IsTrue(results[1] == "211");
         Assert.IsTrue(results[2] == "311");
         Assert.IsTrue(results[3] == "411");
         Assert.IsTrue(results[4] == "511");
      }
   }
}
