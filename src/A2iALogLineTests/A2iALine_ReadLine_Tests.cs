using System;
using Contract;
using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples;

namespace A2iALogLineTests
{
   [TestClass]
   public class A2iALine_ReadLine_Tests
   {
      [TestMethod]
      public void Read_A2iA_logline_1()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new A2iALogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_A2iA_logline.A2iA_logline_1);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is A2iALine);

         A2iALine a2iaLine = (A2iALine)logLine;
         Assert.IsTrue(a2iaLine.Timestamp == "2024-06-05 09:07:17.058");
         Assert.IsTrue(a2iaLine.machineNum == "XB5784");
         Assert.IsTrue(a2iaLine.invalidityScore == "7");
         Assert.IsTrue(a2iaLine.hwMicr == "2");

         Assert.IsTrue(a2iaLine.noDate == "1");
         Assert.IsTrue(a2iaLine.noPayeeName == "2");
         Assert.IsTrue(a2iaLine.noCAR == "3");
         Assert.IsTrue(a2iaLine.noLAR == "4");
         Assert.IsTrue(a2iaLine.noSignature == "5");
         Assert.IsTrue(a2iaLine.noCodeline == "6");
         Assert.IsTrue(a2iaLine.noPayeeEndorsement == "7");

         // OCR

         Assert.IsTrue(a2iaLine.amount == "124142");
         Assert.IsTrue(a2iaLine.amountScore == "730");

         Assert.IsTrue(a2iaLine.codeLine == ".231387602._xxxxxx0401,_1111");
         Assert.IsTrue(a2iaLine.codeLineScore == "1");

         Assert.IsTrue(a2iaLine.ocrDate == "9/5/2024");
         Assert.IsTrue(a2iaLine.ocrDateScore == "2");

         Assert.IsTrue(a2iaLine.signature == "1");
         Assert.IsTrue(a2iaLine.signatureScore == "1");

         Assert.IsTrue(a2iaLine.checkNumber == "1111");
         Assert.IsTrue(a2iaLine.checkNumberScore == "1000");

         Assert.IsTrue(a2iaLine.payeeNameScore == "1");

         Assert.IsTrue(a2iaLine.amountLAR == "124701");
         Assert.IsTrue(a2iaLine.amountLARScore == "2");

         Assert.IsTrue(a2iaLine.amountCAR == "124142");
         Assert.IsTrue(a2iaLine.amountCARScore == "7");
      }

      [TestMethod]
      public void Read_A2iA_logline_2()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new A2iALogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_A2iA_logline.A2iA_logline_2);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is A2iALine);

         A2iALine a2iaLine = (A2iALine)logLine;
         Assert.IsTrue(a2iaLine.Timestamp == "2024-06-05 10:28:23.037");
         Assert.IsTrue(a2iaLine.machineNum == "XB5784");
         Assert.IsTrue(a2iaLine.invalidityScore == "38");
         Assert.IsTrue(a2iaLine.hwMicr == "2");

         Assert.IsTrue(a2iaLine.noDate == "2");
         Assert.IsTrue(a2iaLine.noPayeeName == "2");
         Assert.IsTrue(a2iaLine.noCAR == "2");
         Assert.IsTrue(a2iaLine.noLAR == "2");
         Assert.IsTrue(a2iaLine.noSignature == "2");
         Assert.IsTrue(a2iaLine.noCodeline == "2");
         Assert.IsTrue(a2iaLine.noPayeeEndorsement == "0");

         // OCR

         Assert.IsTrue(a2iaLine.amount == "10078");
         Assert.IsTrue(a2iaLine.amountScore == "748");

         Assert.IsTrue(a2iaLine.codeLine == ".231387602._xxxxxx0401,_1112");
         Assert.IsTrue(a2iaLine.codeLineScore == "0");

         Assert.IsTrue(a2iaLine.ocrDate == "11/8/2024");
         Assert.IsTrue(a2iaLine.ocrDateScore == "2");

         Assert.IsTrue(a2iaLine.signature == "1");
         Assert.IsTrue(a2iaLine.signatureScore == "11");

         Assert.IsTrue(a2iaLine.checkNumber == "1112");
         Assert.IsTrue(a2iaLine.checkNumberScore == "1000");

         Assert.IsTrue(a2iaLine.payeeNameScore == "3");

         Assert.IsTrue(a2iaLine.amountLAR == "10078");
         Assert.IsTrue(a2iaLine.amountLARScore == "0");

         Assert.IsTrue(a2iaLine.amountCAR == "10078");
         Assert.IsTrue(a2iaLine.amountCARScore == "38");
      }

      [TestMethod]
      public void Read_A2iA_logline_3()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new A2iALogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_A2iA_logline.A2iA_logline_3);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is A2iALine);

         A2iALine a2iaLine = (A2iALine)logLine;
         Assert.IsTrue(a2iaLine.Timestamp == "2024-06-09 00:11:04.478");
         Assert.IsTrue(a2iaLine.machineNum == "XB5742");
         Assert.IsTrue(a2iaLine.invalidityScore == "89");
         Assert.IsTrue(a2iaLine.hwMicr == "2");

         Assert.IsTrue(a2iaLine.noDate == "2");
         Assert.IsTrue(a2iaLine.noPayeeName == "2");
         Assert.IsTrue(a2iaLine.noCAR == "2");
         Assert.IsTrue(a2iaLine.noLAR == "2");
         Assert.IsTrue(a2iaLine.noSignature == "2");
         Assert.IsTrue(a2iaLine.noCodeline == "2");
         Assert.IsTrue(a2iaLine.noPayeeEndorsement == "0");

         // OCR

         Assert.IsTrue(a2iaLine.amount == "1227");
         Assert.IsTrue(a2iaLine.amountScore == "977");

         Assert.IsTrue(a2iaLine.codeLine == ",72635274,_.111300880._xxxxx2110,");
         Assert.IsTrue(a2iaLine.codeLineScore == "3");

         Assert.IsTrue(a2iaLine.ocrDate == "5/13/2024");
         Assert.IsTrue(a2iaLine.ocrDateScore == "5");

         Assert.IsTrue(a2iaLine.signature == "1");
         Assert.IsTrue(a2iaLine.signatureScore == "30");

         Assert.IsTrue(a2iaLine.checkNumber == "72635274");
         Assert.IsTrue(a2iaLine.checkNumberScore == "1000");

         Assert.IsTrue(a2iaLine.payeeNameScore == "1");

         Assert.IsTrue(a2iaLine.amountLAR == "");
         Assert.IsTrue(a2iaLine.amountLARScore == "89");

         Assert.IsTrue(a2iaLine.amountCAR == "1227");
         Assert.IsTrue(a2iaLine.amountCARScore == "0");
      }

      [TestMethod]
      public void ReadEntireA2iAFile()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new A2iALogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_A2iA_entirefile.A2iA_EntireFile_1);

         _ = logFileHandler.ReadLine();
         while (!logFileHandler.EOF())
         {
            try
            {
               _ = logFileHandler.ReadLine();
            }
            catch (Exception)
            {
               Assert.IsTrue(false);
               return;
            }
         }

         Assert.IsTrue(true);
         return;
      }
   }
}
