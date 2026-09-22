using System.Collections.Generic;
using Impl;
using LogFileHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWLogLineTests
{
   /// <summary>
   /// NHSWS-18832.
   ///
   /// AWLogHandler.ReadLine used to read one physical line at a time. The DataFlowManager
   /// remote-control handoff record is written as a header line plus three unprefixed
   /// continuation lines; the continuations matched no class tag in IdentifyLine, became
   /// AWLogType.None, and were discarded - taking the incoming session id with them.
   ///
   /// Every fixture below is copied verbatim from
   /// C:\Work_Bugs\NHSWS-18832\Workstation\Workstation20260818.log.
   /// </summary>
   [TestClass]
   public class AWLogHandlerTests
   {
      /// <summary>lines 1243-1248 - a dropped handoff, the blank line after it, and the next record</summary>
      private static readonly string[] DroppedHandoffLines =
      {
         "[2026-08-18 14:16:41-702][3][DataFlowManager     ]Current remote control session id = 0 ",
         "Incoming event remote control session id = 86524",
         "Current teller session id  = 0",
         "Incoming event teller session id = 9220",
         "",
         "[2026-08-18 14:18:48-570][3][BeeHDVideoControl   ]VideoClient_OnUserNotify: callHandle=0, val=4002, severity=SEVERITY_INFO, userType=NOTIFIER_USER_TYPE_ALL, description=, additionalInfo=, suggestedAction= "
      };

      /// <summary>lines 49-55 - IdleEmpty. Every line carries its own header; nothing here may be joined.</summary>
      private static readonly string[] IdleEmptyLines =
      {
         "[2026-08-18 08:25:32-232][3][IdleEmpty           ]---------PROCESS INFORMATION----------------",
         "[2026-08-18 08:25:32-232][3][IdleEmpty           ]Date  Time  :8/18/2026 8:25:32 AM",
         "[2026-08-18 08:25:32-238][3][IdleEmpty           ]Memory      : 268,144,640",
         "[2026-08-18 08:25:32-238][3][IdleEmpty           ]VM      size: 903,770,112",
         "[2026-08-18 08:25:32-239][3][IdleEmpty           ]Private size: 244,228,096",
         "[2026-08-18 08:25:32-239][2][IdleEmpty           ]Handle count:7968",
         "[2026-08-18 08:25:32-239][3][IdleEmpty           ]--------------------------------------------------"
      };

      /// <summary>lines 1-5 - the file banner</summary>
      private static readonly string[] BannerLines =
      {
         "==========================================================================================================",
         " - Software Name    = C:\\Program Files (x86)\\Nautilus Hyosung\\ActiveTeller\\Workstation\\NH.ActiveTeller.Client.exe",
         " - Version          = 1.4.1.1",
         " - File Description = BlueVerse Teller Workstation",
         "=========================================================================================================="
      };

      [TestMethod]
      public void AssembleRecords_JoinsContinuationLines()
      {
         List<string> records = AWLogHandler.AssembleRecords(DroppedHandoffLines);

         // 6 physical lines -> the joined handoff record, the blank line, the BeeHD record
         Assert.AreEqual(3, records.Count);

         string handoff = records[0];
         StringAssert.Contains(handoff, "Current remote control session id = 0");
         StringAssert.Contains(handoff, "Incoming event remote control session id = 86524");
         StringAssert.Contains(handoff, "Current teller session id  = 0");
         StringAssert.Contains(handoff, "Incoming event teller session id = 9220");
         StringAssert.Contains(handoff, AWLogHandler.ContinuationSeparator);

         // the blank line terminates the record and is passed through untouched
         Assert.AreEqual(string.Empty, records[1]);

         // the following record is left alone
         StringAssert.StartsWith(records[2], "[2026-08-18 14:18:48-570]");
      }

      [TestMethod]
      public void AssembleRecords_DoesNotJoinTimestampedLines()
      {
         // regression guard: IdleEmpty writes seven fully prefixed lines. If these are
         // joined, ProcessStats stops populating.
         List<string> records = AWLogHandler.AssembleRecords(IdleEmptyLines);

         Assert.AreEqual(IdleEmptyLines.Length, records.Count);
         CollectionAssert.AreEqual(IdleEmptyLines, records);
      }

      [TestMethod]
      public void AssembleRecords_LeavesBannerLinesAlone()
      {
         List<string> records = AWLogHandler.AssembleRecords(BannerLines);

         Assert.AreEqual(BannerLines.Length, records.Count);
         CollectionAssert.AreEqual(BannerLines, records);
      }

      [TestMethod]
      public void IsRecordStart_ClassifiesEachLineShape()
      {
         Assert.IsTrue(AWLogHandler.IsRecordStart(DroppedHandoffLines[0]), "timestamped record");
         Assert.IsTrue(AWLogHandler.IsRecordStart(BannerLines[0]), "==== banner");
         Assert.IsTrue(AWLogHandler.IsRecordStart(BannerLines[1]), " - banner");

         Assert.IsFalse(AWLogHandler.IsRecordStart(DroppedHandoffLines[1]), "continuation");
         Assert.IsFalse(AWLogHandler.IsRecordStart(string.Empty), "blank");
         Assert.IsFalse(AWLogHandler.IsRecordStart(null), "null");

         Assert.IsTrue(AWLogHandler.IsContinuation(DroppedHandoffLines[1]));
         Assert.IsFalse(AWLogHandler.IsContinuation(string.Empty), "blank lines terminate a record");
         Assert.IsFalse(AWLogHandler.IsContinuation("   "), "whitespace-only terminates a record");
         Assert.IsFalse(AWLogHandler.IsContinuation(DroppedHandoffLines[0]));
      }
   }
}
