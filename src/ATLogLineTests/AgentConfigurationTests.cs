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
    public class AgentConfigurationTests
    {
      [TestMethod]
      public void AgentConfiguration_ServerUrl()
      {
         string sampleLine = @"2023-11-17 03:00:21 Agent configuration server Url is 'http://10.37.152.15:81/activeteller/'";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         AgentConfiguration line = new AgentConfiguration(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual("http://10.37.152.15:81/activeteller/", line.serverUrl);
         Assert.AreEqual(line.reconnectInterval, 0);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      public void AgentConfiguration_ReconnectInterval()
      {
         string sampleLine = @"2023-11-17 03:00:21 Agent configuration reconnect interval is '5000'";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         AgentConfiguration line = new AgentConfiguration(logFileHandler, sampleLine, ATLogType.None);
         Assert.IsNull(line.serverUrl);
         Assert.AreEqual(line.reconnectInterval, 5000);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void AgentConfiguration_Unsupported()
      {
         string sampleLine = @"2023-11-17 03:00:22 Agent configuration unsupported text";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         AgentConfiguration line = null;

         try
         {
            line = new AgentConfiguration(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"ATLogLine.AgentConfiguration: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void AgentConfiguration_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:22 not an agent configuration logline";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());

         try
         {
            ILogLine line = new AgentConfiguration(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.AreEqual($"ATLogLine.AgentConfiguration: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
