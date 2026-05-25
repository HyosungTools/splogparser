using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLineHandler;
using Samples;
using System;

namespace SPLogLineTests
{
   [TestClass]
   public class NHCDMTests
   {
      private static readonly Type NHCDMType = typeof(NHCDMSetCashUnitInfoResult);

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_CstNumber()
      {
         // Arrange
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "Cst1", "Cst2", "Cst3", "Cst4", "Cst5", "Cst6" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.CstNumber);

         // Assert
         CollectionAssert.AreEqual(expected, result.CstNumber, String.Format("CstNumber mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_Status()
      {
         // Arrange
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "0", "0", "0", "0", "0", "6" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.Status);

         // Assert
         CollectionAssert.AreEqual(expected, result.Status, String.Format("Status mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_SerialNumber()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "00CDGR232578", "00CDGR232584", "00CDGR237980", "00CDGR232586", "00CDGR237971", "XXXXX" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.SerialNumber);

         // Assert
         CollectionAssert.AreEqual(expected, result.SerialNumber, String.Format("SerialNumber mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_CstID()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "1", "2", "3", "4", "5", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.CstID);
         // Assert
         CollectionAssert.AreEqual(expected, result.CstID, String.Format("CstID mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_CurrencyID()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "USD", "USD", "USD", "USD", "USD", "XXX" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.CurrencyID);

         // Assert
         CollectionAssert.AreEqual(expected, result.CurrencyID, String.Format("CurrencyID mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_Values()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "20", "20", "20", "20", "20", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.Values);
         // Assert
         CollectionAssert.AreEqual(expected, result.Values, String.Format("Values mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_NoteRevision()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "A", "A", "A", "A", "A", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.NoteRevision);
         // Assert
         CollectionAssert.AreEqual(expected, result.NoteRevision, String.Format("NoteRevision mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_Calibration()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "0", "0", "0", "0", "0", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.Calibration);
         // Assert
         CollectionAssert.AreEqual(expected, result.Calibration, String.Format("Calibration mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_MissingCheck()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "1", "1", "1", "1", "1", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.MissingCheck);
         // Assert
         CollectionAssert.AreEqual(expected, result.MissingCheck, String.Format("MissingCheck mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_InitialCount()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "860", "860", "860", "860", "860", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.InitialCount);
         // Assert
         CollectionAssert.AreEqual(expected, result.InitialCount, String.Format("InitialCount mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_CurrentCount()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "745", "779", "747", "759", "758", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.CurrentCount);
         // Assert
         CollectionAssert.AreEqual(expected, result.CurrentCount, String.Format("CurrentCount mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_DispenseCount()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "0", "0", "0", "0", "0", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.DispenseCount);
         // Assert
         CollectionAssert.AreEqual(expected, result.DispenseCount, String.Format("DispenseCount mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_PresentCount()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "0", "0", "0", "0", "0", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.PresentCount);
         // Assert
         CollectionAssert.AreEqual(expected, result.PresentCount, String.Format("PresentCount mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_RejectCount()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "1", "0", "0", "0", "1", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.RejectCount);
         // Assert
         CollectionAssert.AreEqual(expected, result.RejectCount, String.Format("RejectCount mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }

      [TestMethod]
      public void NHCDMSetCashUnitInfoResult_RetractCount()
      {
         var result = new NHCDMSetCashUnitInfoResult(null, samples_nhcdm.NHCDM_1);
         string[] expected = { "0", "0", "0", "0", "0", "0" };

         // Act
         string expectedStr = string.Join(", ", expected);
         string actualStr = string.Join(", ", result.RetractCount);
         // Assert
         CollectionAssert.AreEqual(expected, result.RetractCount, String.Format("RetractCount mismatch. Expected=[{0}] Actual=[{1}]", expectedStr, actualStr));
      }
   }
}
