using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Contract;
using LogLineHandler;
using LogFileHandler;

namespace LogFileHandlerTests
{
   [TestClass]
   public class WinCEJournalHandlerTests
   {
      private string GetTestFilePath(string fileName)
      {
         string basePath = Path.GetDirectoryName(typeof(WinCEJournalHandlerTests).Assembly.Location);
         return Path.GetFullPath(Path.Combine(basePath, @"..\..\..\..\testdata", fileName));
      }

      [TestMethod]
      public void WinCEJournalHandler_ShouldReadAllLines()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCEJournalHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCEJournalLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCEJournalHandler\JNL00_20250308.dat"));

         // Act
         int lineCount = 0;
         while (!handler.EOF())
         {
            string line = handler.ReadLine();
            if (!string.IsNullOrEmpty(line))
               lineCount++;
         }

         // Assert
         Console.WriteLine($"Total records read: {lineCount}");
         Assert.IsTrue(lineCount > 0, "No records were read from the file");
      }

      [TestMethod]
      public void WinCEJournalHandler_ShouldParseAllLinesSuccessfully()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCEJournalHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCEJournalLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCEJournalHandler\JNL00_20250308.dat"));

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
         Console.WriteLine($"Valid records: {validCount}");
         Console.WriteLine($"Invalid records: {invalidCount}");
         Assert.IsTrue(validCount > 0, "No valid records were parsed");
      }

      [TestMethod]
      public void WinCEJournalHandler_ShouldParseJournalFields()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCEJournalHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCEJournalLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCEJournalHandler\JNL00_20250308.dat"));

         // Act - get first valid line
         WinCEJournalLine journalLine = null;
         while (!handler.EOF() && journalLine == null)
         {
            string rawLine = handler.ReadLine();
            journalLine = handler.IdentifyLine(rawLine) as WinCEJournalLine;
         }

         // Assert
         Assert.IsNotNull(journalLine, "Should parse at least one record");
         Assert.IsFalse(string.IsNullOrEmpty(journalLine.KindCode), "KindCode should not be empty");
         Assert.IsFalse(string.IsNullOrEmpty(journalLine.StackNum), "StackNum should not be empty");
         Assert.IsTrue(journalLine.IsValidTimestamp, "Timestamp should be valid");

         Console.WriteLine($"KindCode: {journalLine.KindCode}");
         Console.WriteLine($"FullType: {journalLine.FullType}");
         Console.WriteLine($"StackNum: {journalLine.StackNum}");
         Console.WriteLine($"Timestamp: {journalLine.Timestamp}");
         Console.WriteLine($"JournalType: {journalLine.journalType}");
      }

      [TestMethod]
      public void WinCEJournalHandler_ShouldExtractValidTimestamp()
      {
         // Arrange - construct handler the same way Program.cs does
         var handler = new WinCEJournalHandler(new CreateTextStreamReader(), ParseType.WinCE, WinCEJournalLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"WinCEJournalHandler\JNL00_20250308.dat"));

         // Act - get first valid line
         WinCEJournalLine journalLine = null;
         while (!handler.EOF() && journalLine == null)
         {
            string rawLine = handler.ReadLine();
            journalLine = handler.IdentifyLine(rawLine) as WinCEJournalLine;
         }

         // Assert
         Assert.IsNotNull(journalLine, "Should parse at least one record");
         Assert.IsTrue(journalLine.IsValidTimestamp, "Timestamp should be valid");
         Assert.IsTrue(journalLine.Timestamp.Contains("-"), "Timestamp should contain date separators");
         Assert.IsTrue(journalLine.Timestamp.Contains(":"), "Timestamp should contain time separators");

         Console.WriteLine($"Timestamp: {journalLine.Timestamp}");
      }
   }
}
