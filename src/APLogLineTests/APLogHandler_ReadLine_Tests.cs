using System;
using Contract;
using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_ReadLine_Tests
   {
      [TestMethod]
      public void DetectInstallLine1()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_logLine_installation.SAMPLE_APLOG_OPENER_1);

         // Reads the entire installation record. 
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is MachineInfo);

         MachineInfo apLine = (MachineInfo)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_INSTALL);

         Assert.IsTrue(apLine.installedPrograms[148,0] == "Windows 10 Update Assistant");
         Assert.IsTrue(apLine.installedPackages[51,0] == "WinLockUpdate-01.00.06.00-20190503");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         APLine apLine2 = (APLine)logLine;
         Assert.IsTrue(apLine2.Timestamp == "2023-03-30 18:08:32.876");

      }

      [TestMethod]
      public void DetectSingleLine1()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_logLine_singleline.SAMPLE_SINGLELINE_1);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:08:32.948");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:08:32.952");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:08:32.954");
      }

      [TestMethod]
      public void DetectMultiLine1()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_logLine_multilines.SAMPLE_MULTILINE_1);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:51:46.749");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:51:46.751");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:51:46.751");
      }

      [TestMethod]
      public void DetectMultiLine2()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_logLine_multilines.SAMPLE_MULTILINE_2);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:51:46.812");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:51:46.813");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:51:46.813");
      }

      [TestMethod]
      public void DetectMultiLine3()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_logLine_multilines.SAMPLE_MULTILINE_3);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:53:05.454");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:53:05.455");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:53:05.456");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 18:53:05.457");
      }

      [TestMethod]
      public void DetectMultiLine4()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_logLine_multilines.SAMPLE_MULTILINE_4);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 14:29:16.594");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 14:29:16.594");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 14:29:16.595");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 14:29:16.595");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-03-30 14:29:16.596");
      }
      [TestMethod]
      public void ReadEntireFile()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_entirefile.SAMPLE_FILE);

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
      public void CashDepot_DetectSingleLine1()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_cashdepot_loglines.SAMPLE_CASHDEPOT_MULTILINE_1);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-07-24 22:07:41.392");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-07-24 22:07:41.392");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);

         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-07-24 22:07:41.392");
      }
   }
}
