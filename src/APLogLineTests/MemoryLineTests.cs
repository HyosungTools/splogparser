using System;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APLogLineTests
{
   /// <summary>
   /// Parser tests for the five resource-snapshot line formats (APLOG_MEMORY).
   ///
   /// NOTE: the 'handler' field below mirrors however the existing
   /// APLineFieldTests constructs its ILogFileHandler. If that test uses a
   /// different field name or factory helper, reuse it here verbatim - the
   /// only requirement is a non-null handler whose ParseType is AP.
   /// </summary>
   [TestClass]
   public class MemoryLineTests
   {
      private const string DateTimeLine = "[2026-06-03 12:49:16-648] Date  Time  :6/3/2026 12:49:16 PM   count:1276";
      private const string MemoryLine_ = "[2026-06-03 12:49:16-652] Memory      : 479,989,760";
      private const string VMSizeLine = "[2026-06-03 12:49:16-652] VM      size:1,457,496,064";
      private const string PrivateLine = "[2026-06-03 12:49:16-652] Private size: 498,593,792";
      private const string HandleLine = "[2026-06-03 12:49:16-652] Handle count:5947";

      [TestMethod]
      public void Factory_Identifies_All_Five_Metrics()
      {
         Assert.AreEqual(MemoryMetric.DateTimeCount, ((MemoryLine)MemoryLine.Factory(null, DateTimeLine)).metric);
         Assert.AreEqual(MemoryMetric.Memory, ((MemoryLine)MemoryLine.Factory(null, MemoryLine_)).metric);
         Assert.AreEqual(MemoryMetric.VMSize, ((MemoryLine)MemoryLine.Factory(null, VMSizeLine)).metric);
         Assert.AreEqual(MemoryMetric.PrivateSize, ((MemoryLine)MemoryLine.Factory(null, PrivateLine)).metric);
         Assert.AreEqual(MemoryMetric.HandleCount, ((MemoryLine)MemoryLine.Factory(null, HandleLine)).metric);
      }

      [TestMethod]
      public void Factory_Returns_Null_For_NonResourceLine()
      {
         // A normal AP line that happens to contain 'Memory' must not be claimed.
         string notResource = "[2026-06-03 12:49:16-652] [SomeClass.Method] Low memory warning cleared";
         Assert.IsNull(MemoryLine.Factory(null, notResource));
      }

      [TestMethod]
      public void DateTimeCount_Parses_Count_Not_TimeOfDay()
      {
         // The trap: LastIndexOf(':') must skip the 12:49:16 colons and land on count:1276.
         MemoryLine line = (MemoryLine)MemoryLine.Factory(null, DateTimeLine);
         Assert.AreEqual(1276L, line.value);
      }

      [TestMethod]
      public void Size_Metrics_Parse_CommaSeparated_Bytes()
      {
         Assert.AreEqual(479989760L, ((MemoryLine)MemoryLine.Factory(null, MemoryLine_)).value);
         Assert.AreEqual(1457496064L, ((MemoryLine)MemoryLine.Factory(null, VMSizeLine)).value);
         Assert.AreEqual(498593792L, ((MemoryLine)MemoryLine.Factory(null, PrivateLine)).value);
         Assert.AreEqual(5947L, ((MemoryLine)MemoryLine.Factory(null, HandleLine)).value);
      }

      [TestMethod]
      public void Timestamp_Is_Extracted_From_Leading_Bracket()
      {
         MemoryLine line = (MemoryLine)MemoryLine.Factory(null, PrivateLine);
         Assert.AreEqual("2026-06-03 12:49:16.652", line.Timestamp);
      }

      [TestMethod]
      public void APLine_Factory_Routes_Resource_Lines_To_MemoryLine()
      {
         // End-to-end through the real AP dispatcher.
         Assert.IsInstanceOfType(APLine.Factory(null, MemoryLine_), typeof(MemoryLine));
         Assert.IsInstanceOfType(APLine.Factory(null, HandleLine), typeof(MemoryLine));
      }
   }
}
