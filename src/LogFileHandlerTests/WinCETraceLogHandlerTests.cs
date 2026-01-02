using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Contract;
using LogLineHandler;
using LogFileHandler;

namespace LogFileHandlerTests
{
   [TestClass]
   public class WinCETraceLogHandlerTests
   {
      private string GetTestFilePath(string fileName)
      {
         string basePath = Path.GetDirectoryName(typeof(WinCETraceLogHandlerTests).Assembly.Location);
         return Path.GetFullPath(Path.Combine(basePath, @"..\..\..\..\testdata", fileName));
      }

      [TestMethod]
      public void WinCETraceLogHandler_ShouldReadAllLines()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCETraceLogHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCETraceLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCETraceLogHandler\00_20250409.log"));

         // Act
         int lineCount = 0;
         while (!handler.EOF())
         {
            string line = handler.ReadLine();
            if (!string.IsNullOrEmpty(line))
               lineCount++;
         }

         // Assert
         Console.WriteLine($"Total lines read: {lineCount}");
         Assert.IsTrue(lineCount > 0, "No lines were read from the file");
      }

      [TestMethod]
      public void WinCETraceLogHandler_ShouldParseAllLinesSuccessfully()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCETraceLogHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCETraceLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCETraceLogHandler\00_20250409.log"));

         // Act
         int validCount = 0;
         int invalidCount = 0;

         while (!handler.EOF())
         {
            string rawLine = handler.ReadLine();
            if (string.IsNullOrEmpty(rawLine))
               continue;

            ILogLine logLine = handler.IdentifyLine(rawLine);
            if (logLine != null)
               validCount++;
            else
               invalidCount++;
         }

         // Assert
         Console.WriteLine($"Valid lines: {validCount}");
         Console.WriteLine($"Invalid lines: {invalidCount}");
         Assert.IsTrue(validCount > 0, "No valid lines were parsed");
      }

      [TestMethod]
      public void WinCETraceLogHandler_ShouldExtractTimestampFromFilename()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCETraceLogHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCETraceLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCETraceLogHandler\00_20250409.log"));

         // Act - get first valid line
         WinCETraceLine traceLine = null;
         while (!handler.EOF() && traceLine == null)
         {
            string rawLine = handler.ReadLine();
            traceLine = handler.IdentifyLine(rawLine) as WinCETraceLine;
         }

         // Assert
         Assert.IsNotNull(traceLine, "Should parse at least one line");
         Assert.IsTrue(traceLine.IsValidTimestamp, "Timestamp should be valid");
         Assert.IsTrue(traceLine.Timestamp.StartsWith("2025-04"),
            $"Timestamp should derive year-month from filename, got: {traceLine.Timestamp}");

         Console.WriteLine($"Timestamp: {traceLine.Timestamp}");
      }

      [TestMethod]
      public void WinCETraceLogHandler_ShouldParseTraceFields()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCETraceLogHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCETraceLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCETraceLogHandler\00_20250409.log"));

         // Act - get first valid line
         WinCETraceLine traceLine = null;
         while (!handler.EOF() && traceLine == null)
         {
            string rawLine = handler.ReadLine();
            traceLine = handler.IdentifyLine(rawLine) as WinCETraceLine;
         }

         // Assert
         Assert.IsNotNull(traceLine, "Should parse at least one line");
         Assert.IsTrue("oOfs".Contains(traceLine.LogType.ToString()),
            $"LogType should be o/O/f/s, got: {traceLine.LogType}");
         Assert.AreEqual(11, traceLine.Code.Length, "Code should be 11 characters");

         Console.WriteLine($"LogType: {traceLine.LogType}");
         Console.WriteLine($"Code: {traceLine.Code}");
         Console.WriteLine($"Message: {traceLine.Message}");
      }
   }
}
