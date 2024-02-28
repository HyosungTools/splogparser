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
   public class NetOpExtensionTests
   {
      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void NetOpExtension_Started()
      {
         string sampleLine = @"2023-09-25 03:00:55 [NetOpExtension] The 'NetOpExtension' extension is started.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
      }

      [TestMethod]
      public void NetOpExtension_SetConfigurationEmptyOrNullModel()
      {
         string sampleLine = @"2023-11-17 03:00:22 [NetOpExtension] Tried to set NetOp configurations but the model is null or empty.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("null", line.ModelName);
         Assert.AreEqual("TRIED", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-17 03:00:22", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_SetConfigurationNoModel()
      {
         string sampleLine = @"2023-11-13 03:01:05 [NetOpExtension] Tried to set NetOp configurations but the model was not provided.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("not provided", line.ModelName);
         Assert.AreEqual("TRIED", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-13 03:01:05", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_CheckingConfigurationFor()
      {
         string sampleLine = @"2023-11-17 03:01:57 [NetOpExtension] Checking if NetOp configuration exists for 7800I";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("7800I", line.ModelName);
         Assert.AreEqual("CHECKING", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-17 03:01:57", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_AttemptingGetAssetConfigurationFor()
      {
         string sampleLine = @"2023-11-17 03:01:57 [NetOpExtension] Attempting to get Asset Config XML for 7800I";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("7800I", line.ModelName);
         Assert.AreEqual("GETTING ASSET CONFIG", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-17 03:01:57", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_AttemptingUpdateNetopIni()
      {
         string sampleLine = @"2023-11-13 03:02:58 [NetOpExtension] Attempting to update the netop.ini.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("null", line.ModelName);
         Assert.AreEqual("TRIED TO UPDATE netop.ini", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-13 03:02:58", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_UpdatedNetopIni()
      {
         string sampleLine = @"2023-11-13 03:02:58 [NetOpExtension] Updated the netop.ini.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("null", line.ModelName);
         Assert.AreEqual("UPDATED netop.ini", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-13 03:02:58", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_NetopServiceStopped()
      {
         string sampleLine = @"2023-11-13 03:02:58 [NetOpExtension] The NetOp service was stopped.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("null", line.ModelName);
         Assert.AreEqual("STOPPED NetOp service", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-13 03:02:58", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_NetopServiceStarted()
      {
         string sampleLine = @"2023-11-13 03:02:58 [NetOpExtension] The NetOp service was started.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("null", line.ModelName);
         Assert.AreEqual("STARTED NetOp service", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-13 03:02:58", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_ConfiguringNetopWithoutVideo()
      {
         string sampleLine = @"2023-11-17 03:01:57 [NetOpExtension] Configuring NetOp without video for model: 7800I";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("7800I", line.ModelName);
         Assert.AreEqual("CONFIGURING (NO VIDEO)", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-17 03:01:57", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_AttemptingGetAssetConfig()
      {
         string sampleLine = @"2023-11-17 03:01:57 [NetOpExtension] Attempting to get Asset Config XML for 7800I";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("7800I", line.ModelName);
         Assert.AreEqual("GETTING ASSET CONFIG", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-17 03:01:57", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_RemoteDesktopServerAlreadyRunning()
      {
         string sampleLine = @"2023-11-17 03:01:58 [NetOpExtension] The remote desktop server is already running";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual(string.Empty, line.ModelName);
         Assert.AreEqual(string.Empty, line.ConfigurationState);
         Assert.AreEqual("RUNNING", line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-17 03:01:58", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_AttemptGetStandardConfiguration()
      {
         string sampleLine = @"2023-11-13 03:02:58 [NetOpExtension] Attempting to get ""Standard"" configuration revision 1 or earlier for model 7800I.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("7800I", line.ModelName);
         Assert.AreEqual("GETTING CONFIGURATION Standard rev 1", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-13 03:02:58", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      public void NetOpExtension_LocatedStandardConfiguration()
      {
         string sampleLine = @"2023-11-13 03:02:58 [NetOpExtension] Located ""Standard"" configuration revision 1 for model 7800I.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         Assert.AreEqual("7800I", line.ModelName);
         Assert.AreEqual("LOCATED CONFIGURAITON Standard rev 1", line.ConfigurationState);
         Assert.AreEqual(string.Empty, line.RemoteDesktopServerState);
         Assert.AreEqual("2023-11-13 03:02:58", line.Timestamp);
         Assert.AreEqual(AELogType.NetOpExtension, line.aeType);
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void NetOpExtension_Unsupported()
      {
         string sampleLine = @"2023-09-25 03:00:55 [NetOpExtension] Is not a supported line.";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = null;

         try
         {
            line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.NetOpExtension: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void NetOpExtension_UnrecognizedLogline_ExceptionThrown()
      {
         string sampleLine = @"2023-11-17 03:00:21 not an Extension started log line";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         NetOpExtension line = null;

         try
         {
            line = new NetOpExtension(logFileHandler, sampleLine, AELogType.NetOpExtension);
         }
         catch (Exception ex)
         {
            Assert.IsNull(line);

            Assert.AreEqual($"AELogLine.NetOpExtension: did not recognize the log line '{sampleLine}'", ex.Message);
            throw;
         }
      }
   }
}
