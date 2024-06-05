using System;
using Contract;
using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples;

namespace APLogLineTests
{
   [TestClass]
   public class SSLogHandler_ReadLine_Tests
   {
      [TestMethod]
      public void ReadSingleLine()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new SSLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_settlementserver_loglines.EJ_StartOfFile_1);

         // Test we can read one line at a time
         ILogLine logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is SSLine);

         SSLine apLine = (SSLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-10-27 01:00:09.1906");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is SSLine);

         apLine = (SSLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-10-27 01:00:13.8599");

         logLine = logFileHandler.IdentifyLine(logFileHandler.ReadLine());
         Assert.IsTrue(logLine is SSLine);

         apLine = (SSLine)logLine;
         Assert.IsTrue(apLine.Timestamp == "2023-10-27 01:00:14.1002");
      }

      [TestMethod]
      public void ReadEntireFile()
      {
         // Test Sample Line
         ILogFileHandler logFileHandler = new SSLogHandler(new CreateTextStreamReaderMock());
         logFileHandler.OpenLogFile(samples_settlementserver_entirefile.EJ_EntireFile_1);

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
