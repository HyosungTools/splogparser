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

   }
}

