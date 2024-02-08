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
using Samples;
using System;

namespace AELogLineTests
{
   [TestClass]
   public class AELineTests
   {

      [TestMethod]
      public void DetectEOFOneLineFileEndsWithNewline()
      {
         string sampleFile =
            "03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData\n";

         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
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
         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
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
         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
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
         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(sampleFile);
         string read = logFileHandler.ReadLine();

         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadPrevData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual("03284294967295011200698021500005CSM500002SP00102022/12/07001219:31 12.2730006NORMAL0021CNHUsb2::ReadNextData", read);

         read = logFileHandler.ReadLine();
         Assert.AreEqual(string.Empty, read);
      }

      [TestMethod]
      public void AELine_Timestamp()
      {
         string sampleFile = "2023-11-17 03:00:32 ActiveTellerExtensions connection state change Connecting";

         AELine atLine = new AELine(new AELogHandler(new CreateTextStreamReaderMock()), sampleFile, AELogType.None);
         Assert.IsTrue(atLine.Timestamp == "2023-11-17 03:00:32");
      }

      [TestMethod]
      public void AELine_hResult()
      {
         string sampleFile = "2023-11-17 03:00:22 ActiveTellerExtensions connection state change Connecting";

         AELine atLine = new AELine(new AELogHandler(new CreateTextStreamReaderMock()), sampleFile, AELogType.None);
         Assert.IsTrue(atLine.HResult == "");
      }

      [TestMethod]
      public void ReadEntireFile_RawLines()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_entirefile.SAMPLE_ATEXTENSION_FILE);

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
               return;
            }
         }

         Assert.IsTrue(true);
         return;
      }

      [TestMethod]
      public void ReadEntireFile_IdentifyAllLines()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new AELogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_entirefile.SAMPLE_ATEXTENSION_FILE);

         Assert.IsFalse(logFileHandler.EOF());

         while (!logFileHandler.EOF())
         {
            string nextLine = logFileHandler.ReadLine();

            try
            {
               ILogLine logLine = logFileHandler.IdentifyLine(nextLine);

               if (logLine is LogLineHandler.ExtensionStarted aeLogLine)
               {
                  Assert.AreEqual(AELogType.ExtensionStarted, aeLogLine.aeType);
               }
               else if (logLine is LogLineHandler.NetOpExtension noLogLine)
               {
                  Assert.AreEqual(AELogType.NetOpExtension, noLogLine.aeType);
               }
               else if (logLine is LogLineHandler.NextwareExtension neLogLine)
               {
                  Assert.AreEqual(AELogType.NextwareExtension, neLogLine.aeType);
               }
               else if (logLine is LogLineHandler.MoniPlus2sExtension mpLogLine)
               {
                  Assert.AreEqual(AELogType.MoniPlus2sExtension, mpLogLine.aeType);
               }
               else if (logLine is AELine aeGenericLogLine)
               {
                  Assert.AreEqual(AELogType.None, aeGenericLogLine.aeType);

                  throw new Exception($"Line was classified as generic '{nextLine}'");
               }
               else
               {
                  throw new Exception($"Failed to identify line or classify it as generic '{nextLine}'");
               }
            }
            catch (Exception e)
            {
               throw;
            }
         }

         Assert.IsTrue(logFileHandler.EOF());
         return;
      }
   }
}

