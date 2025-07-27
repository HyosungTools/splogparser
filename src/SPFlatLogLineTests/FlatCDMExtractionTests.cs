
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLineHandler;
using Contract;
using LogFileHandler;
using Samples;
using System;

namespace SPFlatLogLineTests
{
   [TestClass]
   public class FlatCDMExtractionTests
   {

      [TestMethod]
      public void CDMDispenseInvoked()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(),SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_DispenseInvoked);
         Assert.IsInstanceOfType(logLine, typeof(CDMDispenseLine));

         CDMDispenseLine flat = (CDMDispenseLine)logLine;

         Assert.AreEqual(0, flat.MixAlgorithm);
         Assert.AreEqual("USD", flat.Currency);
         Assert.AreEqual(0, flat.Amount);
         Assert.AreEqual(0, flat.Present);
         Assert.AreEqual(60000, flat.Timeout);
         CollectionAssert.AreEqual(new[] { 0, 0, 2, 0, 0 }, flat.NoteCounts);
      }


      // U N I T  T E S T  C A S E S

      [TestMethod]
      public void CDMUnitCurrencies()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitCurrency);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMUnitCurrencies: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "", "", "USD", "USD", "USD" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMUnitIDs()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitID);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // GetUnitID0043UnitID[(51060)(51060)(51063)(51066)(51171)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMUnitIDs: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "51060", "51060", "51063", "51066", "51171" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMUnitValues()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitValue);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // GetUnitValue0028UnitValue[(0)(0)(5)(20)(50)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMUnitValues: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "0", "0", "5", "20", "50" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMUnitCounts()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitCount);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // GetUnitCount0035UnitCount[(0)(2)(2826)(5893)(2910)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMUnitCounts: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "0", "2", "2826", "5893", "2910" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMUnitStatuses()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitStatus);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // GetUnitStatus0033UnitStatus[(OK)(OK)(LOW)(OK)(OK)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMUnitStatuses: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "OK", "OK", "LOW", "OK", "OK" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMPhysicalUnitNumbers()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitPUNumber);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // Ctrl::GetUnitPUNumber0032UnitPUNumber[(1)(2)(3)(4)(4)(5)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMPhysicalUnitNumbers: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "1", "2", "3", "4", "4", "5" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMPhysicalIDs()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalID);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // Ctrl::GetPhysicalID0054PhysicalID[(51060)(51060)(51063)(51066)(51068)(51171)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMPhysicalUnitNumbers: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "51060", "51060", "51063", "51066", "51068", "51171" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMPhysicalPositionNames()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalPositionName);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // Ctrl::GetPhysicalPositionName0086PhysicalPositionName[(COMPARTMENT1.RET)(COMPARTMENT1.REJ)(SLOT1)(SLOT2)(SLOT3)(SLOT4)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMPhysicalUnitNumbers: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "COMPARTMENT1.RET", "COMPARTMENT1.REJ", "SLOT1", "SLOT2", "SLOT3", "SLOT4" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMPhysicalInitialCounts()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalInitialCount);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // Ctrl::GetPhysicalInitialCount0052PhysicalInitialCount[(0)(0)(3000)(3000)(3000)(3000)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMPhysicalUnitNumbers: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "0", "0", "3000", "3000", "3000", "3000" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMPhysicalStatuses()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalStatus);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // Ctrl::GetPhysicalStatus0044PhysicalStatus[(OK)(OK)(LOW)(EMPTY)(OK)(OK)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMPhysicalUnitNumbers: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "OK", "OK", "LOW", "EMPTY", "OK", "OK" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMPhysicalRejectCounts()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalRejectCount);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // Ctrl::GetPhysicalRejectCount0039PhysicalRejectCount[(0)(0)(2)(0)(0)(0)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMPhysicalUnitNumbers: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "0", "0", "2", "0", "0", "0" }, actual: flat.unitList);
      }

      [TestMethod]
      public void CDMPhysicalCounts()
      {
         ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), SPFlatLine.Factory);

         ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalCount);
         Assert.IsInstanceOfType(logLine, typeof(CDMUnitList));

         // Ctrl::GetPhysicalCount0045PhysicalCount[(0)(2)(2826)(3000)(2893)(2910)]
         CDMUnitList flat = (CDMUnitList)logLine;
         Console.WriteLine("CDMPhysicalUnitNumbers: " + string.Join(", ", flat.unitList));
         CollectionAssert.AreEqual(new[] { "0", "2", "2826", "3000", "2893", "2910" }, actual: flat.unitList);
      }
   }
}
