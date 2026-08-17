using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using Contract;
using LogLineHandler;
using LogFileHandler;

namespace LogFileHandlerTests
{
   [TestClass]
   public class SPLogHandlerTests
   {
      /// <summary>
      /// Regression for the ReadLine hang: a .nwlog whose final byte is '\n' made ReadLine() read one
      /// past the buffer (logFile[traceFilePos + 1]) and throw IndexOutOfRangeException *before*
      /// advancing traceFilePos. BaseView's while(!EOF()) loop then re-read the same position forever,
      /// hanging the whole run. With the lookahead guarded, the handler reaches EOF cleanly.
      /// </summary>
      [TestMethod]
      public void SPLogHandler_TrailingNewline_ReadsToEofWithoutThrowingOrHanging()
      {
         string path = Path.Combine(Path.GetTempPath(),
            "sp_trailing_nl_" + Guid.NewGuid().ToString("N") + ".nwlog");

         // ASCII, no BOM, ends in '\n' so the last char sits at the final buffer index (reproduces the bug).
         File.WriteAllBytes(path, Encoding.ASCII.GetBytes("  INFO service provider line\n"));

         try
         {
            var handler = new SPLogHandler(new CreateTextStreamReader(), ParseType.SP, SPLine.Factory);
            handler.OpenLogFile(path);

            int guard = 0;
            while (!handler.EOF())
            {
               handler.ReadLine();
               Assert.IsTrue(++guard < 1000,
                  "ReadLine never reached EOF on a newline-terminated file (hang regression).");
            }
         }
         finally
         {
            File.Delete(path);
         }
      }
   }
}
