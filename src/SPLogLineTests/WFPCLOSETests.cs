using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using Samples;
using Contract;

namespace SPLogLineTests
{
   [TestClass]
   public class WFPCLOSETests
   {
      [TestMethod]
      public void WFPCLOSE_Timestamp()
      {
         ILogFileHandler logFileHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_general.WFPCLOSE);
         string expected = "2023-04-04 02:59:48.532";

         Assert.IsTrue(logLine is WFPCLOSE);
         WFPCLOSE spLine = (WFPCLOSE)logLine;
         Assert.AreEqual(expected, spLine.Timestamp, $"Expected {expected} Actual {spLine.Timestamp}");
      }

      [TestMethod]
      public void WFPCLOSE_hResult()
      {
         WFPCLOSE spLine = new WFPCLOSE(new SPLogHandler(new CreateTextStreamReaderMock()), samples_general.WFPCLOSE, XFSType.WFPCLOSE);
         Assert.IsTrue(spLine.HResult == "");
      }
   }
}
