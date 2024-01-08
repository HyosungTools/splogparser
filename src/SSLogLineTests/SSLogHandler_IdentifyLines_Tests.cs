using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using Samples;
using LogLineHandler;

namespace LogFileHandler
{
   [TestClass]
   public class SSLogHandler_IdentifyLines_Tests
   {
      [TestMethod]
      public void EJ_UploadedReceived()
      {
         ILogFileHandler logFileHandler = new SSLogHandler(new CreateTextStreamReader());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_settlementserver.EJ_UploadedReceived);
         Assert.IsTrue(logLine is SSLine);

         SSLine ssLine = (SSLine)logLine;
         Assert.IsTrue(ssLine.ssType == SSLogType.EJ_UploadedReceived);
         Assert.IsTrue(ssLine.Timestamp == "2023-12-08 04:00:29.7861");
         Assert.IsTrue(ssLine.HResult == "");

         Assert.IsTrue(ssLine.ejFileName == "ej2_663256");
      }

      [TestMethod]
      public void EJ_Created()
      {
         ILogFileHandler logFileHandler = new SSLogHandler(new CreateTextStreamReader());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_settlementserver.EJ_Created);
         Assert.IsTrue(logLine is SSLine);

         SSLine ssLine = (SSLine)logLine;
         Assert.IsTrue(ssLine.ssType == SSLogType.EJ_Created);
         Assert.IsTrue(ssLine.Timestamp == "2023-12-08 04:00:42.1852");
         Assert.IsTrue(ssLine.HResult == "");

         Assert.IsTrue(ssLine.ejFileName == "ej2_663296");
      }

      [TestMethod]
      public void EJ_Discovered()
      {
         ILogFileHandler logFileHandler = new SSLogHandler(new CreateTextStreamReader());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_settlementserver.EJ_Discovered);
         Assert.IsTrue(logLine is SSLine);

         SSLine ssLine = (SSLine)logLine;
         Assert.IsTrue(ssLine.ssType == SSLogType.EJ_Discovered);
         Assert.IsTrue(ssLine.Timestamp == "2023-12-08 04:08:50.1275");
         Assert.IsTrue(ssLine.HResult == "");

         Assert.IsTrue(ssLine.ejFileName == "ej2_663256");
      }

      [TestMethod]
      public void EJ_ImportSucceeded()
      {
         ILogFileHandler logFileHandler = new SSLogHandler(new CreateTextStreamReader());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_settlementserver.EJ_ImportSucceeded);
         Assert.IsTrue(logLine is SSLine);

         SSLine ssLine = (SSLine)logLine;
         Assert.IsTrue(ssLine.ssType == SSLogType.EJ_ImportSucceeded);
         Assert.IsTrue(ssLine.Timestamp == "2023-12-08 04:11:58.5057");
         Assert.IsTrue(ssLine.HResult == "");

         Assert.IsTrue(ssLine.ejFileName == "ej2_663257");
      }
   }
}
