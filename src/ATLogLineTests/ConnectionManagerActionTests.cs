using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using ATSamples;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Moq;
using Moq.Protected;
using Contract;
using System.Numerics;
using System;

namespace ATLogLineTests
{
   [TestClass]
    public class ConnectionManagerActionTests
    {
      [TestMethod]
      public void ConnectionManagerAction_ManagerThreadStarting()
      {
         string sampleLine = @"2023-11-17 03:00:21 Connection manager thread starting";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.ManagerThreadStarting);
         Assert.IsNull(line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_AttemptingActiveTellerServerContact()
      {
         string sampleLine = @"2023-11-17 03:00:21 Attempting to contact the ActiveTeller server";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.AttemptingActiveTellerServerContact);
         Assert.IsNull(line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_ActiveTellerServerContacted()
      {
         string sampleLine = @"2023-11-17 03:00:21 Successfully contacted the ActiveTeller server";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.ActiveTellerServerContacted);
         Assert.IsNull(line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_RegisteringClient()
      {
         string sampleLine = @"2023-11-17 03:00:21 Connection manager registering client using device id 70-85-C2-18-7C-DA";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.RegisteringClient);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_PreparingActiveTellerConnection()
      {
         string sampleLine = @"2023-11-17 03:00:22 Connection manager preparing the ActiveTeller connection";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.PreparingActiveTellerConnection);
         Assert.IsNull(line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_ConfiguringActiveTellerHubProxy()
      {
         string sampleLine = @"2023-11-17 03:00:22 Connection manager configuring the ActiveTeller hub proxy";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.ConfiguringActiveTellerHubProxy);
         Assert.IsNull(line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_InitiatingActiveTellerConnection()
      {
         string sampleLine = @"2023-11-17 03:00:22 Connection manager initiating ActiveTeller connection";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.InitiatingActiveTellerConnection);
         Assert.IsNull(line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_RegisteringAssetUsingMAC()
      {
         string sampleLine = @"2023-11-17 03:00:22 Connection manager registering asset using MAC Address 70-85-C2-18-7C-DA";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.RegisteringAssetUsingMAC);
         Assert.IsNull(line.deviceId);
         Assert.AreEqual("70-85-C2-18-7C-DA", line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void ConnectionManagerAction_RegistrationException()
      {
         string sampleLine = @"2023-11-17 09:24:30 Connection manager registration exception";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, ConnectionManagerAction.ConnectionManagerActionEnum.RegistrationException);
         Assert.IsNull(line.deviceId);
         Assert.IsNull(line.macAddress);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void ConnectionManagerAction_Unsupported()
      {
         string sampleLine = @"Connection manager unsupported text";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         ConnectionManagerAction line = null;

         try
         {
            line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"ATLogLine.ConnectionManagerAction: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void ConnectionManagerAction_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:22 not a connection manager logline";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());

         try
         {
            ILogLine line = new ConnectionManagerAction(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.AreEqual($"ATLogLine.ConnectionManagerAction: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
