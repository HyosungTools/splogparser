using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLineHandler;
using Samples;
using System;

namespace SPLogLineTests
{
   [TestClass]
   public class WFSCIMCASHUNFOTests
   {
      private static readonly Type WFSCDMCUINFOType = typeof(WFSCIMCASHINFO);

      private static string GetLogicalSubLogLineTable(string logLine)
      {
         int indexOfLogical = logLine.IndexOf("lppList");
         int indexOfPhysical = logLine.IndexOf("lppPhysical");
         Assert.IsTrue(indexOfLogical > 0, "lppList not found in log line.");
         Assert.IsTrue(indexOfPhysical > 0, "lppPhysical not found in log line.");
         string logicalSubLogLine = logLine.Substring(indexOfLogical);
         if (indexOfPhysical > 0)
         {
            logicalSubLogLine = logLine.Substring(indexOfLogical, indexOfPhysical - indexOfLogical);
         }
         return logicalSubLogLine; 
      }

      // Helper method to extract lppPhysical substring
      private static string GetPhysicalSubLogLineTable(string logLine)
      {
         int indexOfTable = logLine.IndexOf("lppList");
         int indexOfPhysical = logLine.IndexOf("lppPhysical");
         Assert.IsTrue(indexOfTable > 0, "lppList not found in log line.");
         Assert.IsTrue(indexOfPhysical > 0, "lppPhysical not found in log line.");
         return logLine.Substring(indexOfPhysical);
      }

      private static string GetLogicalSubLogLineList(string logLine)
      {
         int indexOfLogical = logLine.IndexOf("lppList");
         Assert.IsTrue(indexOfLogical > 0, "lppList not found in log line.");
         string logicalSubLogLine = logLine.Substring(indexOfLogical);
         return logicalSubLogLine;
      }

      // L O G I C A L   T A B L E
      #region LogicalTable

      [TestMethod]
      public void WFSCDMCUINFO_usNumbersFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "1", "2", "3", "4", "5", "6" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("usNumbersFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_usTypesFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "6", "2", "12", "12", "12", "12" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("usTypesFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_cUnitIDsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "LCU00", "LCU01", "LCU02", "LCU03", "LCU04", "LCU05" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_cCurrencyIDsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "   ", "   ", "USD", "USD", "USD", "USD" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("cCurrencyIDsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulValuesFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "1", "5", "20", "100" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulValuesFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulInitialCountsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "200", "900", "1500", "360" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulInitialCountsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulCountsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "5", "174", "246", "503", "163" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulCountsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulRejectCountsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "5", "0", "2", "3", "0" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulRejectCountsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulMinimumsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "0", "0", "0", "0" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulMinimumsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulMaximumsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "400", "2000", "0", "0", "0", "0" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulMaximumsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_usStatusesFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "4", "0", "3", "3", "0", "3" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("usStatusesFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulDispensedCountsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "32", "663", "1036", "206" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulDispensedCountsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulPresentedCountsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "32", "661", "1033", "206" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulPresentedCountsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      [TestMethod]
      public void WFSCDMCUINFO_ulRetractedCountsFromTable()
      {
         // Arrange
         string subLogLine = GetLogicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "0", "0", "0", "0" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulRetractedCountsFromTable", subLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }
      #endregion

      // P H Y S I C A L   T A B L E
      #region PhysicalTable

      [TestMethod]
      public void WFSCDMCUINFO_usNumPhysicalCUsFromTable()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "1", "1", "1", "1", "1", "1" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("usNumPhysicalCUsFromTable", physicalSubLogLine);
         Console.WriteLine($"result: {string.Join("|", result)}");

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for lpPhysicalPositionNamesFromTable
      [TestMethod]
      public void WFSCDMCUINFO_lpPhysicalPositionNamesFromTable()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "RetractCassette", "RejectCassette", "CassetteA", "CassetteB", "CassetteC", "CassetteD" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("lpPhysicalPositionNamesFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for cUnitIDsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_cUnitIDsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "RTCST", "RJCST", "CST_A", "CST_B", "CST_C", "CST_D" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_ulInitialCountsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_ulInitialCountsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "200", "900", "1500", "360" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulInitialCountsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_ulCountsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_ulCountsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "5", "174", "246", "503", "163" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulCountsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_ulRejectCountsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_ulRejectCountsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "5", "0", "2", "3", "0" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulRejectCountsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_ulMaximumsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_ulMaximumsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "400", "2000", "0", "0", "0", "0" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulMaximumsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_usPStatusesFromTable
      [TestMethod]
      public void WFSCDMCUINFO_usPStatusesFromTable()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "4", "0", "3", "3", "0", "3" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("usPStatusesFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_bHardwareSensorsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_bHardwareSensorsFromTable()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "1", "1", "1", "1", "1", "1" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("bHardwareSensorsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_ulDispensedCountsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_ulDispensedCountsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "32", "663", "1036", "206" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulDispensedCountsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_ulPresentedCountsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_ulPresentedCountsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "32", "661", "1033", "206" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulPresentedCountsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }

      // Tests for p_ulRetractedCountsFromTable
      [TestMethod]
      public void WFSCDMCUINFO_ulRetractedCountsFromTable_1()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLineTable(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "0", "0", "0", "0", "0" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("ulRetractedCountsFromTable", physicalSubLogLine);

         // Assert
         Assert.AreEqual(expected.Length, result.Length, $"Expected {expected.Length} Actual {result.Length}");
         CollectionAssert.AreEqual(expected, result, $"Expected {string.Join("|", expected)} Actual {string.Join("|", result)} .");
      }
      #endregion

   }
}

