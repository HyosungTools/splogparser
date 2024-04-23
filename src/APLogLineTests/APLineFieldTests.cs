using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using APSamples;
using System;

namespace APLogLineTests
{
   [TestClass]
   public class APLineFieldTests
   {
      [TestMethod]
      public void APLine_APLOG_SETTINGS_CONFIG()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_SETTINGS_CONFIG);
         Assert.IsTrue(logLine is APLineField);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_SETTINGS_CONFIG);
         Assert.IsTrue(apLine.Timestamp == "2023-11-16 03:01:00.649");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == @"C:\Hyosung\MoniPlus2S\Config\Application\Communication.xml");
      }

      [TestMethod]
      public void APLine_APLOG_CURRENTMODE()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CURRENTMODE);
         Assert.IsTrue(logLine is APLineField);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CURRENTMODE);
         Assert.IsTrue(apLine.Timestamp == "2023-11-16 03:06:10.108");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "PowerUp");
      }

      [TestMethod]
      public void APLine_APLOG_CARD_OPEN()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CARD_OPEN);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CARD_OPEN);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 03:02:56.855");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_CARD_CLOSED()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CARD_CLOSE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CARD_CLOSE);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 03:00:00.153");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_CARD_ONMEDIAINSERTED()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CARD_ONMEDIAINSERTED);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CARD_ONMEDIAINSERTED);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 09:11:41.552");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_CARD_ONREADCOMPLETE()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CARD_ONREADCOMPLETE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CARD_ONREADCOMPLETE);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 09:11:42.573");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_CARD_ONEJECTCOMPLETE()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CARD_ONEJECTCOMPLETE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CARD_ONEJECTCOMPLETE);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 09:17:28.435");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_CARD_ONMEDIAREMOVED()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CARD_ONMEDIAREMOVED);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CARD_ONMEDIAREMOVED);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 09:17:31.888");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_CARD_PAN()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_CARD_PAN);
         Assert.IsTrue(logLine is APLineField);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_CARD_PAN);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 10:03:33.675");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "411395XXXXXX1667");
      }


      [TestMethod]
      public void APLine_APLOG_PIN_OPEN()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_OPEN);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_OPEN);
         Assert.IsTrue(apLine.Timestamp == "2024-02-27 03:01:12.695");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_CLOSE()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_CLOSE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_CLOSE);
         Assert.IsTrue(apLine.Timestamp == "2024-02-27 03:00:12.160");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_ISPCI()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_ISPCI);
         Assert.IsTrue(logLine is APLineField);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_ISPCI);
         Assert.IsTrue(apLine.Timestamp == "2024-02-28 10:14:47.401");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "PCI EPP");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_ISTR31()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_ISTR31);
         Assert.IsTrue(logLine is APLineField);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_ISTR31);
         Assert.IsTrue(apLine.Timestamp == "2024-02-27 03:01:12.979");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "TR31");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_ISTR34()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_ISTR34);
         Assert.IsTrue(logLine is APLineField);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_ISTR34);
         Assert.IsTrue(apLine.Timestamp == "2024-02-27 03:01:12.979");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "TR34");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_KEYIMPORTED()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_KEYIMPORTED);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_KEYIMPORTED);
         Assert.IsTrue(apLine.Timestamp == "2024-02-27 03:02:40.740");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_RAND()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_RAND);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_RAND);
         Assert.IsTrue(apLine.Timestamp == "2024-02-27 06:00:45.140");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_PINBLOCK()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_PINBLOCK);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_PINBLOCK);
         Assert.IsTrue(apLine.Timestamp == "2024-02-27 06:00:50.356");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_PINBLOCK_FAILED()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_PINBLOCK_FAILED);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_PINBLOCK_FAILED);
         Assert.IsTrue(apLine.Timestamp == "2023-08-16 14:22:41.356");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_TIMEOUT()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_TIMEOUT);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_TIMEOUT);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 16:04:06.363");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_PIN_READCOMPLETE()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_PIN_READCOMPLETE);
         Assert.IsTrue(logLine is APLine);

         APLine apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_PIN_READCOMPLETE);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 13:52:24.388");
         Assert.IsTrue(apLine.HResult == "");
      }

      [TestMethod]
      public void APLine_APLOG_DISPLAYLOAD()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_DISPLAYLOAD);
         Assert.IsTrue(logLine is APLine);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_DISPLAYLOAD);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 10:12:49.021");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "MainMenu");
      }

      [TestMethod]
      public void APLine_APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE);
         Assert.IsTrue(logLine is APLine);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 10:48:45.625");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "UpdateCheck21XML");
      }

      [TestMethod]
      public void APLine_APLOG_STATE_CREATED()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_STATE_CREATED);
         Assert.IsTrue(logLine is APLine);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_STATE_CREATED);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 09:11:46.843");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "Common-EnterPIN");
      }

      [TestMethod]
      public void APLine_APLOG_FUNCTIONKEY_SELECTED()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_FUNCTIONKEY_SELECTED);
         Assert.IsTrue(logLine is APLine);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_FUNCTIONKEY_SELECTED);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 16:42:56.456");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "Withdrawal");
      }

      [TestMethod]
      public void APLine_APLOG_FUNCTIONKEY_SELECTED2()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_FUNCTIONKEY_SELECTED2);
         Assert.IsTrue(logLine is APLine);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_FUNCTIONKEY_SELECTED2);
         Assert.IsTrue(apLine.Timestamp == "2024-03-08 08:58:12.281");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "Yes");
      }

      [TestMethod]
      public void APLine_APLOG_DEVICE_FITNESS()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.APLOG_DEVICE_FITNESS);
         Assert.IsTrue(logLine is APLine);

         APLineField apLine = (APLineField)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_DEVICE_FITNESS);
         Assert.IsTrue(apLine.Timestamp == "2023-11-27 16:20:42.413");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.field == "DEVHWERROR");
      }

      [TestMethod]
      public void APLine_APLOG_EXCEPTION_1()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_exception.EXCEPTION_1);

         // First two lines are not an EXCEPTION
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);
         APLine apLine = (APLine)logLine;
         Assert.IsFalse(apLine.apType == APLogType.APLOG_EXCEPTION);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 17:01:48.430");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);
         apLine = (APLine)logLine;
         Assert.IsFalse(apLine.apType == APLogType.APLOG_EXCEPTION);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 17:01:50.485");

         // Third Line is an EXCEPTION
         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);
         apLine = (APLine)logLine;
         Assert.IsTrue(apLine.apType == APLogType.APLOG_EXCEPTION);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 17:01:50.490");

         // Fourth Line is not an EXCEPTION
         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is APLine);
         apLine = (APLine)logLine;
         Assert.IsFalse(apLine.apType == APLogType.APLOG_EXCEPTION);
         Assert.IsTrue(apLine.Timestamp == "2024-01-16 17:01:50.522");
      }
   }
}
