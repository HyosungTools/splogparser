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
    public class AgentHostTests
    {
      [TestMethod]
      public void AgentHost_Started()
      {
         string sampleLine = @"2023-11-17 03:00:21 Agent host started";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         AgentHost line = new AgentHost(logFileHandler, sampleLine, ATLogType.None);
         Assert.AreEqual(line.state, AgentHost.AgentHostStateEnum.Started);
         Assert.AreEqual(line.atType, ATLogType.None);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void AgentHost_Unsupported()
      {
         string sampleLine = @"2023-11-17 03:00:21 Agent host unsupported text";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         AgentHost line = null;

         try
         {
            line = new AgentHost(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"ATLogLine.AgentHost: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void AgentHost_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:22 not an agent host logline";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());

         try
         {
            ILogLine line = new AgentHost(logFileHandler, sampleLine, ATLogType.None);
         }
         catch (Exception ex)
         {
            Assert.AreEqual($"ATLogLine.AgentHost: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
