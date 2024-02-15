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
using Samples;
using Impl;

namespace ATLogLineTests
{
   [TestClass]
   public class ATLineTests
   {

      [TestMethod]
      public void DetectEOFOneLineFileEndsWithNewline()
      {
         string sampleFile =
            "03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData\n";

         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(sampleFile);
         logFileHandler.ReadLine();
      }


      [TestMethod]
      public void DetectEOFMultilineEndsWithNewline()
      {
         string sampleFile =
@"03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData
03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadNextData" + "\n";

         // Test Sample Line
         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(sampleFile);
         string read = logFileHandler.ReadLine();

         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadNextData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual(string.Empty, read);
      }


      [TestMethod]
      public void DetectEOFMultilineEndsWithReturn()
      {
         string sampleFile =
@"03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData
03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadNextData" + "\r";

         // Test Sample Line
         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(sampleFile);
         string read = logFileHandler.ReadLine();

         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadNextData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual(string.Empty, read);
      }


      [TestMethod]
      public void DetectEOFMultilineEndsNoNewlineOrReturn()
      {
         string sampleFile =
@"03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData
03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadNextData";

         // Test Sample Line
         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(sampleFile);
         string read = logFileHandler.ReadLine();

         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadNextData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual(string.Empty, read);
      }

      [TestMethod]
      public void ATLine_Timestamp()
      {
         string sampleFile = "2023-11-17 03:00:32 ActiveTeller connection state change Connecting";

         ATLine atLine = new ATLine(new ATLogHandler(new CreateTextStreamReaderMock()), sampleFile, ATLogType.None);
         Assert.IsTrue(atLine.Timestamp == "2023-11-17 03:00:32");
      }

      [TestMethod]
      public void ATLine_hResult()
      {
         string sampleFile = "2023-11-17 03:00:22 ActiveTeller connection state change Connecting";

         ATLine atLine = new ATLine(new ATLogHandler(new CreateTextStreamReaderMock()), sampleFile, ATLogType.None);
         Assert.IsTrue(atLine.HResult == "");
      }

      [TestMethod]
      public void ReadEntireFile_RawLines()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_entirefile.SAMPLE_ATAGENT_FILE);

         _ = logFileHandler.ReadLine();
         while (!logFileHandler.EOF())
         {
            try
            {
               _ = logFileHandler.ReadLine();
            }
            catch (Exception e)
            {
               Assert.IsTrue(false);
               throw e;
            }
         }

         Assert.IsTrue(true);
         return;
      }

      [TestMethod]
      public void ReadEntireFile_IdentifyAllLines()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_entirefile.SAMPLE_ATAGENT_FILE);

         Assert.IsFalse(logFileHandler.EOF());

         bool ignoringBraces = false;

         while (!logFileHandler.EOF())
         {
            string nextLine = logFileHandler.ReadLine();

            try
            {
               ILogLine logLine = logFileHandler.IdentifyLine(nextLine);

               if (logLine is LogLineHandler.SignalRConnectionState atLogLine)
               {
                  Assert.AreEqual(ATLogType.ActiveTellerConnectionState, atLogLine.atType);
               }
               else if (logLine is LogLineHandler.ConnectionManagerAction atLogLine2)
               {
                  Assert.AreEqual(ATLogType.ConnectionManagerAction, atLogLine2.atType);
               }
               else if (logLine is LogLineHandler.AgentConfiguration acLogLine)
               {
                  Assert.AreEqual(ATLogType.AgentConfiguration, acLogLine.atType);
               }
               else if (logLine is LogLineHandler.AgentHost ahLogLine)
               {
                  Assert.AreEqual(ATLogType.AgentHost, ahLogLine.atType);
               }
               else if (logLine is LogLineHandler.ServerRequests scLogLine)
               {
                  Assert.AreEqual(ATLogType.ServerRequest, scLogLine.atType);
               }
               else if (logLine is ATLine atGenericLogLine)
               {
                  Assert.AreEqual(ATLogType.None, atGenericLogLine.atType);

                  if (nextLine == "{")
                  {
                     ignoringBraces = true;
                  }
                  else if (nextLine == "}")
                  {
                     ignoringBraces = false;
                  }

                  else if (ignoringBraces)
                  {
                     Assert.IsTrue(nextLine.StartsWith("  "));
                  }

                  else
                  {
                     Assert.IsTrue(
                        nextLine.Contains("is starting") ||
                        nextLine.Contains("agent extension found") ||
                        nextLine.Contains("handler added") ||
                        nextLine.Contains("extension started") ||
                        nextLine.Contains("listening for an application message") ||
                        nextLine.Contains("agent extension is not enabled") ||
                        nextLine.Contains("Attempting to start Remote Desktop") ||
                        nextLine.Contains("is no longer waiting") ||
                        nextLine.Contains("Enabled devices are") ||
                        nextLine.Contains("received a DeviceState from NextwareExtension") ||
                        nextLine.Contains("starting monitoring") ||
                        nextLine.Contains("Server reconnected") ||
                        nextLine.Contains("Received system settings broadcast") ||
                        nextLine.StartsWith("   at ") ||
                        nextLine.Contains("End of inner exception stack trace"),
                        $"Log line was unintenionally ignored '{nextLine}'"
                        );
                  }
               }
               else
               {
                  throw new Exception($"Failed to identify line or classify it as generic '{nextLine}'");
               }
            }
            catch (Exception e)
            {
               throw e;
            }
         }

         Assert.IsFalse(ignoringBraces);
         Assert.IsTrue(logFileHandler.EOF());
         return;
      }
   }
}

