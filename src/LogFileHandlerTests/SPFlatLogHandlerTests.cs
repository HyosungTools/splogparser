using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Contract;
using LogLineHandler;
using LogFileHandler;

namespace LogFileHandlerTests
{
   [TestClass]
   public class SPFlatLogHandlerTests
   {
      private string GetTestFilePath(string fileName)
      {
         string basePath = Path.GetDirectoryName(typeof(SPFlatLogHandlerTests).Assembly.Location);
         return Path.GetFullPath(Path.Combine(basePath, @"..\..\..\..\testdata", fileName));
      }


      [TestMethod]
      public void SPFlatLogHandler_ShouldReadAllLines()
      {
         var handler = new SPFlatLogHandler(new CreateTextStreamReader(), SPFlatLine.Factory);
         handler.OpenLogFile(GetTestFilePath(@"SPFlatLogHandler\BSTrace.nwlog"));

         Assert.IsTrue(handler.LineOffsets.Count > 0, "No timestamp lines found");

         int count = 0;
         string line;
         while ((line = handler.ReadLine()) != null) count++;

         Console.WriteLine($"Total timestamp lines: {handler.LineOffsets.Count}");
         Console.WriteLine($"Total lines read: {count}");

         Assert.AreEqual(64789, count);
      }
   }
}

