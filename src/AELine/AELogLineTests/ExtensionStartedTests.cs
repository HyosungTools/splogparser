using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Moq;
using Moq.Protected;
using Contract;
using System.Numerics;
using System;
using AELogLineTests;

namespace AELogLineTests
{
   [TestClass]
   public class ExtensionStartedTests
   {
      [TestMethod]
      public void ExtensionStarted_Started()
      {
         string sampleLine = @"2023-09-25 03:00:55 [NetOpExtension] The 'NetOpExtension' extension is started.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         ExtensionStarted line = new ExtensionStarted(logFileHandler, sampleLine, AELogType.ExtensionStarted);
         Assert.AreEqual("NetOpExtension", line.extensionName);
         Assert.AreEqual(AELogType.ExtensionStarted, line.aeType);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void ExtensionStarted_Unsupported()
      {
         string sampleLine = @"2023-09-25 03:00:55 [NetOpExtension] The 'NetOpExtension' extension is not started.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         ExtensionStarted line = null;

         try
         {
            line = new ExtensionStarted(logFileHandler, sampleLine, AELogType.ExtensionStarted);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.ExtensionStarted: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void ExtensionStarted_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:21 not an Extension started log line";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         ExtensionStarted line = null;

         try
         {
            line = new ExtensionStarted(logFileHandler, sampleLine, AELogType.ExtensionStarted);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.ExtensionStarted: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
