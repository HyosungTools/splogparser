using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using Samples;

namespace SPLogLineTests
{
   public class WFPCLOSETests
   {
      [TestMethod]
      public void WFPCLOSE_Timestamp()
      {
         WFPCLOSE spLine = new WFPCLOSE(new SPLogHandler(new CreateTextStreamReaderMock()), samples_general.WFPCLOSE, XFSType.WFPCLOSE);
         Assert.IsTrue(spLine.Timestamp == "2023-03-17 08:42:28.033");
      }

      [TestMethod]
      public void WFPCLOSE_hResult()
      {
         WFPCLOSE spLine = new WFPCLOSE(new SPLogHandler(new CreateTextStreamReaderMock()), samples_general.WFPCLOSE, XFSType.WFPCLOSE);
         Assert.IsTrue(spLine.HResult == "");
      }
   }
}
