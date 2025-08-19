using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLineHandler;
using Samples;
using System;

namespace SPLogLineTests
{
   [TestClass]
   public class WFSCDMDENOMINATIONTests
   {
      private static readonly Type WFSCDMDENOMINATIONType = typeof(WFSCDMDENOMINATION);

      // Tests for usNumPhysicalCUsFromTable
      [TestMethod]
      public void WFSCDMDENOMINATION_cCurrencyIDFromList()
      {
         // Arrange
         string logLine = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         string expected = "USD";

         // Act
         var privateType = new PrivateType(WFSCDMDENOMINATIONType);
         var result = (ValueTuple<bool, string, string>)privateType.InvokeStatic("cCurrencyIDFromList", logLine);

         // Assert
         //(bool success, string xfsMatch, string subLogLine)
         Assert.IsTrue(result.Item1, "Expected cCurrencyIDFromList to succeed.");
         Assert.AreEqual(expected, result.Item2, $"Expected {expected}, actual {result.Item2}");
      }

      [TestMethod]
      public void WFSCDMDENOMINATION_ulAmountFromList()
      {
         // Arrange
         string logLine = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         string expected = "500";

         // Act
         var privateType = new PrivateType(WFSCDMDENOMINATIONType);
         var result = (ValueTuple<bool, string, string>)privateType.InvokeStatic("ulAmountFromList", logLine);

         // Assert
         //(bool success, string xfsMatch, string subLogLine)
         Assert.IsTrue(result.Item1, "Expected ulAmountFromList to succeed.");
         Assert.AreEqual(expected, result.Item2, $"Expected {expected}, actual {result.Item2}");
      }

      [TestMethod]
      public void WFSCDMDENOMINATION_usCountFromList()
      {
         // Arrange
         string logLine = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         string expected = "6";

         // Act
         var privateType = new PrivateType(WFSCDMDENOMINATIONType);
         var result = (ValueTuple<bool, string, string>)privateType.InvokeStatic("usCountFromList", logLine);

         // Assert
         //(bool success, string xfsMatch, string subLogLine)
         Assert.IsTrue(result.Item1, "Expected usCountFromList to succeed.");
         Assert.AreEqual(expected, result.Item2, $"Expected {expected}, actual {result.Item2}");
      }

      [TestMethod]
      public void WFSCDMDENOMINATION_lpulValuesFromList()
      {
         // Arrange
         string logLine = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         string[] expected = new[] { "0", "0", "0", "4", "9", "3" };

         // Act
         var privateType = new PrivateType(WFSCDMDENOMINATIONType);
         var result = (ValueTuple<bool, string[], string>)privateType.InvokeStatic("lpulValuesFromList", logLine);

         // Assert
         Assert.IsTrue(result.Item1, "Expected lpulValuesFromList to succeed.");
         CollectionAssert.AreEqual(expected, result.Item2, $"Expected lpulValues '{string.Join("|", expected)}', Actual lpulValues '{string.Join("|", result.Item2)}'.");
         Assert.IsNotNull(result.Item3, "SubLogLine should not be null.");
      }

      [TestMethod]
      public void WFSCDMDENOMINATION_ulCashBoxFromList()
      {
         // Arrange
         string logLine = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         string expected = "0";

         // Act
         var privateType = new PrivateType(WFSCDMDENOMINATIONType);
         var result = (ValueTuple<bool, string, string>)privateType.InvokeStatic("ulCashBoxFromList", logLine);

         // Assert
         //(bool success, string xfsMatch, string subLogLine)
         Assert.IsTrue(result.Item1, "Expected ulCashBoxFromList to succeed.");
         Assert.AreEqual(expected, result.Item2, $"Expected {expected}, Actual {result.Item2}");
      }
   }
}
