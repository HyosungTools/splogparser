using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLineHandler;
using Samples;
using System;

namespace SPLogLineTests
{
   [TestClass]
   public class WFSCUINFOTests
   {
      private static readonly Type WFSCUINFOType = typeof(WFSCUINFO);

      // T A B L E   A C C E S S   F U N C T I O N S

      [TestMethod]
      public void WFSCUINFO_usCountFromTable()
      {
         // Arrange
         (bool success, string xfsMatch, string subLogLine) expected = (true, "7", ""); // subLogLine is the match start

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (ValueTuple<bool, string, string>)privateType.InvokeStatic("usCountFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE, 1);

         // Assert
         Assert.IsTrue(result.Item1, "Expected success");
         Assert.AreEqual(expected.Item2, result.Item2, $"Expected xfsMatch: {expected.Item2}, Actual: {result.Item2}");
      }

      [TestMethod]
      public void WFSCUINFO_usNumbersFromTable()
      {
         // Arrange
         string[] expected = { "1", "2", "3", "4", "5", "6", "7" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("usNumbersFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_fwTypeFromTable()
      {
         // Arrange
         string[] expected = { "8", "9", "11", "11", "12", "13", "14" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("fwTypesFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_fwItemTypeFromTable()
      {
         // Arrange
         string[] expected = { "0x0001", "0x0001", "0x0004", "0x0004", "0x0004", "0x0004", "0x0003" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("fwItemTypesFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_cUnitIDsFromTable()
      {
         // Arrange
         string[] expected = { "LCU00", "LCU01", "LCU02", "LCU03", "LCU04", "LCU05", "LCU06" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_cCurrencyIDsFromTable()
      {
         // Arrange
         string[] expected = { "   ", "   ", "ABC", "DEF", "GHI", "JKL", "   " };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("cCurrencyIDsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulValuesFromTable()
      {
         // Arrange
         string[] expected = { "15", "16", "17", "18", "19", "20", "21" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulValuesFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulCashInCountsFromTable()
      {
         // Arrange
         string[] expected = { "22", "23", "24", "25", "26", "27", "28" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulCashInCountsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulCountsFromTable()
      {
         // Arrange
         string[] expected = { "29", "30", "31", "32", "33", "34", "35" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulCountsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulMaximumsFromTable()
      {
         // Arrange
         string[] expected = { "36", "37", "38", "39", "40", "41", "42" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulMaximumsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_usStatusesFromTable()
      {
         // Arrange
         string[] expected = { "43", "44", "45", "46", "47", "48", "49" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("usStatusesFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_bAppLocksFromTable()
      {
         // Arrange
         string[] expected = { "0", "0", "0", "0", "0", "0", "0" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("bAppLocksFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_TABLE);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_usCDMTypesFromTable()
      {
         // Arrange
         string[] expected = { "0", "0", "0", "0", "0", "0", "0" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("usCDMTypesFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_lpszCashUnitNamesFromTable()
      {
         // Arrange
         string[] expected = { "LCU00", "LCU01", "LCU02", "LCU03", "LCU04", "LCU05", "LCU06" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("lpszCashUnitNamesFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulInitialCountsFromTable()
      {
         // Arrange
         string[] expected = { "100", "101", "102", "103", "104", "105", "106" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulInitialCountsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulDispensedCountsFromTable()
      {
         // Arrange
         string[] expected = { "107", "108", "109", "110", "111", "112", "113" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulDispensedCountsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulPresentedCountsFromTable()
      {
         // Arrange
         string[] expected = { "114", "115", "116", "117", "118", "119", "120" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulPresentedCountsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulRetractedCountsFromTable()
      {
         // Arrange
         string[] expected = { "121", "122", "123", "124", "125", "126", "127" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulRetractedCountsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulRejectCountsFromTable()
      {
         // Arrange
         string[] expected = { "128", "129", "130", "131", "132", "133", "134" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulRejectCountsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulMinimumsFromTable()
      {
         // Arrange
         string[] expected = { "135", "136", "137", "138", "139", "140", "141" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulMinimumsFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_lpszExtrasFromTable()
      {
         // Arrange
         string[] expected = { "NULL", "NULL", "NULL", "NULL", "NULL", "NULL", "NULL" };

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (string[])privateType.InvokeStatic("lpszExtrasFromTable", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_LCU_ETC);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // L I S T    A C C E S S    F U N C T I O N S 

      [TestMethod]
      public void WFSCUINFO_usCountFromList()
      {
         // Arrange
         (bool success, string xfsMatch, string subLogLine) expected = (true, "3", ""); // subLogLine is the match start

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         var result = (ValueTuple<bool, string, string>)privateType.InvokeStatic("usCountFromList", samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2, 1);

         // Assert
         Assert.IsTrue(result.Item1, "Expected success");
         Assert.AreEqual(expected.Item2, result.Item2, $"Expected xfsMatch: {expected.Item2}, Actual: {result.Item2}");
      }

      [TestMethod]
      public void WFSCUINFO_usNumbersFromList()
      {
         // Arrange
         string[] expected = { "1", "2" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2; 
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("usNumbersFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_fwTypesFromList()
      {
         // Arrange
         string[] expected = { "4", "1" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("fwTypesFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_fwItemTypesFromList()
      {
         // Arrange
         string[] expected = { "0x0001", "0x0002" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("fwItemTypesFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_cUnitIDsFromList()
      {
         // Arrange
         string[] expected = { "LCU00", "LCU00" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_cCurrencyIDsFromList()
      {
         // Arrange
         string[] expected = { "USD", "USD" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("cCurrencyIDsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulValuesFromList()
      {
         // Arrange
         string[] expected = { "0", "3" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulValuesFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulCashInCountsFromList()
      {
         // Arrange
         string[] expected = { "39", "4" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulCashInCountsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulCountsFromList()
      {
         // Arrange
         string[] expected = { "4", "5" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulCountsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulMaximumsFromList()
      {
         // Arrange
         string[] expected = { "126", "6" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulMaximumsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_usStatusesFromList()
      {
         // Arrange
         string[] expected = { "0", "7" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("usStatusesFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_bAppLocksFromList()
      {
         // Arrange
         string[] expected = { "0", "8" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("bAppLocksFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_usNumPhysicalCUsFromList()
      {
         // Arrange
         string[] expected = { "1", "1" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("usNumPhysicalCUsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_lpszExtrasFromList()
      {
         // Arrange
         string[] expected = { "NULL", "NULL" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("lpszExtrasFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_usCDMTypesFromList()
      {
         // Arrange
         string[] expected = { "0", "27" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("usCDMTypesFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_lpszCashUnitNamesFromList()
      {
         // Arrange
         string[] expected = { "RETRACTCASSETTE", "TESTCASSETTE" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("lpszCashUnitNamesFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulInitialCountsFromList()
      {
         // Arrange
         string[] expected = { "0", "28" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulInitialCountsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulDispensedCountsFromList()
      {
         // Arrange
         string[] expected = { "0", "29" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulDispensedCountsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulPresentedCountsFromList()
      {
         // Arrange
         string[] expected = { "0", "30" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulPresentedCountsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulRetractedCountsFromList()
      {
         // Arrange
         string[] expected = { "0", "31" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulRetractedCountsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulRejectCountsFromList()
      {
         // Arrange
         string[] expected = { "0", "32" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulRejectCountsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_ulMinimumsFromList()
      {
         // Arrange
         string[] expected = { "0", "33" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] logicalUnits = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 2);
         var result = (string[])privateType.InvokeStatic("ulMinimumsFromList", logicalUnits, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_lpPhysicalPositionNamesFromList()
      {
         // Arrange
         string[] expected = { "RETRACTCASSETTE", "TESTCASSETTE" };
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         int indexOfList = logLine.IndexOf("lppCashIn =");
         string logicalSubLogLine = logLine.Substring(indexOfList);


         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] physicalUnitParts = (string[])privateType.InvokeStatic("GetPhysicalUnits", logicalSubLogLine, 2);
         foreach (string part in physicalUnitParts)
            Console.WriteLine($"PCU : {part}");
         var result = (string[])privateType.InvokeStatic("lpPhysicalPositionNamesFromList", physicalUnitParts, 2);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCUINFO_lpPhysicalPositionNamesFromList2()
      {
         // Arrange
         string[] expected = { "RetractCassette", "RetractCassette", "RejectCassette", "CassetteA", "CassetteB" };
         string logLine = samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_3;
         int indexOfList = logLine.IndexOf("lppList =");
         string logicalSubLogLine = logLine.Substring(indexOfList);


         // Act
         var privateType = new PrivateType(WFSCUINFOType);
         string[] physicalUnitParts = (string[])privateType.InvokeStatic("GetPhysicalUnits", logicalSubLogLine, 5);
         foreach (string pu in physicalUnitParts)
            Console.WriteLine($"physical_cUnitID : {pu}");
         var result = (string[])privateType.InvokeStatic("lpPhysicalPositionNamesFromList", physicalUnitParts, 5);

         // Assert
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }
   }
}
