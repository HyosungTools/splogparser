using System;
using System.Collections.Generic;
using Contract;
using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples;

namespace SPLogLineTests
{
   [TestClass]
   public class WFSCUINFOCHANGEDTests
   {
      private static readonly Type WFSCIMCASHINFOType = typeof(WFSCIMCASHINFO);

      [TestMethod]
      public void WFSCUINFOCHANGED_WFS_SRVE_CIM_CASHUNITINFOCHANGED_1()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_1);
         Assert.IsInstanceOfType(logLine, typeof(WFSCIMCASHINFO), $"Expected type: WFSCIMCASHINFO, Actual: {logLine.GetType()}");

         // Arrange
         int indexOfList = samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_1.IndexOf("lpBuffer = ");

         // L O G I C A L
         string logicalSubLogLine = samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_1.Substring(indexOfList);

         // Act
         var privateType = new PrivateType(WFSCIMCASHINFOType);

         // usNumber
         var result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "usNumber");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("5", result.xfsMatch.Trim(), $"Expected 5, Actual {result.xfsMatch.Trim()}");

         // fwType
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "fwType");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("1", result.xfsMatch.Trim(), $"Expected 1, Actual {result.xfsMatch.Trim()}");

         // fwItemType
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("StringPropertyFromList", logicalSubLogLine, "fwItemType");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("0x0004", result.xfsMatch.Trim(), $"Expected 0x0004, Actual {result.xfsMatch.Trim()}");

         // cUnitID
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("StringPropertyFromList", logicalSubLogLine, "cUnitID");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("LCU04", result.xfsMatch.Trim(), $"Expected LCU04, Actual {result.xfsMatch.Trim()}");

         // cCurrencyID
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("StringPropertyFromList", logicalSubLogLine, "cCurrencyID");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("USD", result.xfsMatch.Trim(), $"Expected USD, Actual {result.xfsMatch.Trim()}");

         // ulValues
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "ulValues");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("20", result.xfsMatch.Trim(), $"Expected 20, Actual {result.xfsMatch.Trim()}");

         // ulCashInCount
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "ulCashInCount");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("17", result.xfsMatch.Trim(), $"Expected 17, Actual {result.xfsMatch.Trim()}");

         // ulCount
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "ulCount");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("0", result.xfsMatch.Trim(), $"Expected 0, Actual {result.xfsMatch.Trim()}");

         // ulMaximum
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "ulMaximum");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("0", result.xfsMatch.Trim(), $"Expected 0, Actual {result.xfsMatch.Trim()}");

         // usStatus
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "usStatus");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("0", result.xfsMatch.Trim(), $"Expected 0, Actual {result.xfsMatch.Trim()}");

         // bAppLock
         result = ((bool success, string xfsMatch, string subLogLine))privateType.InvokeStatic("NumericPropertyFromList", logicalSubLogLine, "bAppLock");
         Console.WriteLine($"result: {result.xfsMatch.Trim()}");
         Assert.AreEqual("0", result.xfsMatch.Trim(), $"Expected 0, Actual {result.xfsMatch.Trim()}");


         // lpNoteNumberList
         string[] logicalUnitParts = (string[])privateType.InvokeStatic("GetLogicalUnits", logicalSubLogLine, 1);
         Console.WriteLine($"result: {logicalUnitParts.Length}");
         Assert.AreEqual(1, logicalUnitParts.Length, $"Expected 1, Actual {logicalUnitParts.Length}");

         string[,] noteNumbers = (string[,])privateType.InvokeStatic("noteNumberListFromList", logicalUnitParts, 1);
         Console.WriteLine($"noteNumbers Length: {noteNumbers.Length}");
         Assert.AreEqual(20, noteNumbers.Length, $"Expected 20, Actual {noteNumbers.Length}");


      }

      [TestMethod]
      public void WFSCUINFOCHANGED_WFS_SRVE_CIM_CASHUNITINFOCHANGED_2()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_2);
         Assert.IsInstanceOfType(logLine, typeof(WFSCIMCASHINFO), $"Expected type: WFSCIMCASHINFO, Actual: {logLine.GetType()}");
      }

      [TestMethod]
      public void WFSCUINFOCHANGED_WFS_SRVE_CIM_CASHUNITINFOCHANGED_3()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_3);
         Assert.IsInstanceOfType(logLine, typeof(WFSCIMCASHINFO), $"Expected type: WFSCIMCASHINFO, Actual: {logLine.GetType()}");
      }

      [TestMethod]
      public void WFSCUINFOCHANGED_WFS_SRVE_CIM_CASHUNITINFOCHANGED_4()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_4);
         Assert.IsInstanceOfType(logLine, typeof(WFSCIMCASHINFO), $"Expected type: WFSCIMCASHINFO, Actual: {logLine.GetType()}");
      }

      [TestMethod]
      public void WFSCUINFOCHANGED_WFS_SRVE_CIM_CASHUNITINFOCHANGED_5()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_5);
         Assert.IsInstanceOfType(logLine, typeof(WFSCIMCASHINFO), $"Expected type: WFSCIMCASHINFO, Actual: {logLine.GetType()}");
      }

      [TestMethod]
      public void WFSCUINFOCHANGED_WFS_SRVE_CIM_CASHUNITINFOCHANGED_6()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_6);
         Assert.IsInstanceOfType(logLine, typeof(WFSCIMCASHINFO), $"Expected type: WFSCIMCASHINFO, Actual: {logLine.GetType()}");
      }

      [TestMethod]
      public void WFSCUINFOCHANGED_WFS_SRVE_CIM_CASHUNITINFOCHANGED_7()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_7);
         Assert.IsInstanceOfType(logLine, typeof(WFSCIMCASHINFO), $"Expected type: WFSCIMCASHINFO, Actual: {logLine.GetType()}");
      }
   }
}
