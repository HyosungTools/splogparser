using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using Samples;
using Contract;
using System;

namespace SPLogLineTests
{
   public class WFSCDMCUINFOTests
   {
      private static readonly Type WFSCDMCUINFOType = typeof(WFSCDMCUINFO);

      // Helper method to extract lppPhysical substring
      private static string GetPhysicalSubLogLine(string logLine)
      {
         int indexOfTable = logLine.IndexOf("lppList->");
         int indexOfPhysical = logLine.IndexOf("lppPhysical");
         Assert.IsTrue(indexOfTable > 0, "lppList-> not found in log line.");
         Assert.IsTrue(indexOfPhysical > 0, "lppPhysical not found in log line.");
         return logLine.Substring(indexOfPhysical);
      }

      // Tests for usNumPhysicalCUsFromTable
      [TestMethod]
      public void usNumPhysicalCUsFromTable_ValidLogLine_ReturnsSixCounts()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "1", "1", "1", "1", "1", "1" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("usNumPhysicalCUsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six usNumPhysicalCUs values.");
      }

      [TestMethod]
      public void usNumPhysicalCUsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("usNumPhysicalCUsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for lpPhysicalPositionNamesFromTable
      [TestMethod]
      public void lpPhysicalPositionNamesFromTable_ValidLogLine_ReturnsSixNames()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "RetractCassette", "RejectCassette", "CassetteA", "CassetteB", "CassetteC", "CassetteD" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("lpPhysicalPositionNamesFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six lpPhysicalPositionName values.");
      }

      [TestMethod]
      public void lpPhysicalPositionNamesFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("lpPhysicalPositionNamesFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for cUnitIDsFromTable
      [TestMethod]
      public void cUnitIDsFromTable_ValidLogLine_ReturnsSixUnitIDs()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "RTCST", "RJCST", "CST_A", "CST_B", "CST_C", "CST_D" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromTable", physicalSubLogLine, 6);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six cUnitID values.");
      }

      [TestMethod]
      public void cUnitIDsFromTable_EmptyLogLine_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromTable", physicalSubLogLine, 6);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array for empty log line.");
      }

      [TestMethod]
      public void cUnitIDsFromTable_NoUnitIDField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "lpPhysicalPositionName RetractCassette\tRejectCassette";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromTable", physicalSubLogLine, 6);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when cUnitID field is missing.");
      }

      [TestMethod]
      public void cUnitIDsFromTable_ExtraTabs_ReturnsCorrectUnitIDs()
      {
         // Arrange
         string logLine = "lppList->{usCount=6 lppPhysical->{cUnitID RTCST\t\tRJCST\tCST_A\tCST_B\tCST_C\tCST_D}}";
         string physicalSubLogLine = GetPhysicalSubLogLine(logLine);
         string[] expected = new[] { "RTCST", "RJCST", "CST_A", "CST_B", "CST_C", "CST_D" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("cUnitIDsFromTable", physicalSubLogLine, 6);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected correct cUnitID values despite extra tabs.");
      }

      // Tests for p_ulInitialCountsFromTable
      [TestMethod]
      public void p_ulInitialCountsFromTable_ValidLogLine_ReturnsSixCounts()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "100", "200", "300", "400", "500", "600" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulInitialCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six ulInitialCount values.");
      }

      [TestMethod]
      public void p_ulInitialCountsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulInitialCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_ulCountsFromTable
      [TestMethod]
      public void p_ulCountsFromTable_ValidLogLine_ReturnsSixCounts()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "50", "60", "70", "80", "90", "100" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six ulCount values.");
      }

      [TestMethod]
      public void p_ulCountsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_ulRejectCountsFromTable
      [TestMethod]
      public void p_ulRejectCountsFromTable_ValidLogLine_ReturnsSixCounts()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "1", "2", "3", "4", "5", "6" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulRejectCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six ulRejectCount values.");
      }

      [TestMethod]
      public void p_ulRejectCountsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulRejectCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_ulMaximumsFromTable
      [TestMethod]
      public void p_ulMaximumsFromTable_ValidLogLine_ReturnsSixValues()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "1000", "2000", "3000", "4000", "5000", "6000" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulMaximumsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six ulMaximum values.");
      }

      [TestMethod]
      public void p_ulMaximumsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulMaximumsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_usPStatusesFromTable
      [TestMethod]
      public void p_usPStatusesFromTable_ValidLogLine_ReturnsSixStatuses()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "1", "2", "3", "4", "5" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_usPStatusesFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six usPStatus values.");
      }

      [TestMethod]
      public void p_usPStatusesFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_usPStatusesFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_bHardwareSensorsFromTable
      [TestMethod]
      public void p_bHardwareSensorsFromTable_ValidLogLine_ReturnsSixSensors()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "1", "0", "1", "0", "1" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_bHardwareSensorsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six bHardwareSensor values.");
      }

      [TestMethod]
      public void p_bHardwareSensorsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_bHardwareSensorsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_ulDispensedCountsFromTable
      [TestMethod]
      public void p_ulDispensedCountsFromTable_ValidLogLine_ReturnsSixCounts()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "10", "20", "30", "40", "50", "60" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulDispensedCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six ulDispensedCount values.");
      }

      [TestMethod]
      public void p_ulDispensedCountsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulDispensedCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_ulPresentedCountsFromTable
      [TestMethod]
      public void p_ulPresentedCountsFromTable_ValidLogLine_ReturnsSixCounts()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "5", "15", "25", "35", "45", "55" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulPresentedCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six ulPresentedCount values.");
      }

      [TestMethod]
      public void p_ulPresentedCountsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulPresentedCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }

      // Tests for p_ulRetractedCountsFromTable
      [TestMethod]
      public void p_ulRetractedCountsFromTable_ValidLogLine_ReturnsSixCounts()
      {
         // Arrange
         string physicalSubLogLine = GetPhysicalSubLogLine(samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_PHY_1);
         string[] expected = new[] { "0", "1", "2", "3", "4", "5" };

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulRetractedCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected six ulRetractedCount values.");
      }

      [TestMethod]
      public void p_ulRetractedCountsFromTable_NoField_ReturnsEmptyArray()
      {
         // Arrange
         string physicalSubLogLine = "cUnitID RTCST\tRJCST";
         string[] expected = Array.Empty<string>();

         // Act
         var privateType = new PrivateType(WFSCDMCUINFOType);
         var result = (string[])privateType.InvokeStatic("p_ulRetractedCountsFromTable", physicalSubLogLine);

         // Assert
         CollectionAssert.AreEqual(expected, result, "Expected empty array when field is missing.");
      }
   }
}

