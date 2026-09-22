using Impl;
using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWLogLineTests
{
   /// <summary>
   /// NHSWS-18832 - the remote-control handoff.
   ///
   /// When the workstation has no current remote control session (id 0) and the server
   /// pushes an incoming remote-control event, the event is discarded and the teller's
   /// Remote Desktop silently never opens. On 2026-08-18 this happened on 3 of the 10
   /// sessions that attempted remote control, across three ATMs and two tellers, and the
   /// workbook reported disposition "Clean" for every one of them.
   ///
   /// The lines below are copied verbatim from Workstation20260818.log and are shown here
   /// already assembled by AWLogHandler.ReadLine (header + three continuations joined with
   /// AWLogHandler.ContinuationSeparator).
   /// </summary>
   [TestClass]
   public class DataFlowManagerTests
   {
      private AWLogHandler handler;

      [TestInitialize]
      public void Setup()
      {
         // mirrors Program.cs: new AWLogHandler(new CreateTextStreamReader())
         handler = new AWLogHandler(new CreateTextStreamReader());
      }

      private static string Join(params string[] physicalLines)
      {
         return string.Join(AWLogHandler.ContinuationSeparator, physicalLines);
      }

      /// <summary>lines 1243-1246 - teller session 9220, asset A070965, the first of three attempts</summary>
      private static string DroppedRecord()
      {
         return Join(
            "[2026-08-18 14:16:41-702][3][DataFlowManager     ]Current remote control session id = 0",
            "Incoming event remote control session id = 86524",
            "Current teller session id  = 0",
            "Incoming event teller session id = 9220");
      }

      /// <summary>lines 128-131 - teller session 9213, asset A070959, a handoff that worked</summary>
      private static string MatchedRecord()
      {
         return Join(
            "[2026-08-18 08:42:37-741][3][DataFlowManager     ]Current remote control session id = 86461",
            "Incoming event remote control session id = 86461",
            "Current teller session id  = 9213",
            "Incoming event teller session id = 9213");
      }

      [TestMethod]
      public void DroppedHandoff_IsFlagged()
      {
         DataFlowManager line = new DataFlowManager(handler, DroppedRecord());

         Assert.IsTrue(line.IsRecognized);
         Assert.AreEqual("0", line.RcSessionIdCurrent);
         Assert.AreEqual("86524", line.RcSessionIdIncoming);
         Assert.AreEqual("0", line.TellerSessionIdCurrent);
         Assert.AreEqual("9220", line.TellerSessionIdIncoming);

         StringAssert.StartsWith(line.RemoteControlHandoff, "DROPPED");
         StringAssert.Contains(line.RemoteControlHandoff, "86524");
         StringAssert.Contains(line.RemoteControlHandoff, "9220");

         // the existing column still carries the current id, now with the incoming one
         StringAssert.Contains(line.RemoteControlSessionState, "CURRENT remote control session id 0");
         StringAssert.Contains(line.RemoteControlSessionState, "INCOMING 86524");
      }

      [TestMethod]
      public void MatchedHandoff_IsNotFlagged()
      {
         DataFlowManager line = new DataFlowManager(handler, MatchedRecord());

         Assert.IsTrue(line.IsRecognized);
         Assert.AreEqual("86461", line.RcSessionIdCurrent);
         Assert.AreEqual("86461", line.RcSessionIdIncoming);
         Assert.AreEqual("9213", line.TellerSessionIdIncoming);

         StringAssert.StartsWith(line.RemoteControlHandoff, "MATCHED");
         Assert.IsFalse(line.RemoteControlHandoff.Contains("DROPPED"));
      }

      [TestMethod]
      public void WithoutContinuationLines_PreservesLegacyBehaviour()
      {
         // an older capture, or a run before the handler change - the header line alone
         DataFlowManager line = new DataFlowManager(
            handler,
            "[2026-08-18 14:16:41-702][3][DataFlowManager     ]Current remote control session id = 0 ");

         Assert.IsTrue(line.IsRecognized);
         Assert.AreEqual("0", line.RcSessionIdCurrent);
         Assert.AreEqual(string.Empty, line.RcSessionIdIncoming);

         // no incoming id means no verdict - we must not claim a drop we cannot see
         Assert.AreEqual(string.Empty, line.RemoteControlHandoff);
         Assert.AreEqual("CURRENT remote control session id 0", line.RemoteControlSessionState);
      }

      [TestMethod]
      public void AssembledRecord_DoesNotDisturbNeighbouringRegexes()
      {
         // DataFlowManager.Initialize runs every regex in sequence and later matches
         // overwrite earlier ones. The joined record contains the words "teller session"
         // and "control session"; none of the existing regexes may claim it.
         DataFlowManager line = new DataFlowManager(handler, DroppedRecord());

         Assert.AreEqual(string.Empty, line.ControlSessionStatus, "Control session regex must not match");
         Assert.AreEqual(string.Empty, line.TellerSessionRequest, "Teller session regexes must not match");
         Assert.AreEqual(string.Empty, line.TaskStatusEvent);
         Assert.AreEqual(string.Empty, line.Asset);
      }
   }
}
