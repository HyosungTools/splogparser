using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler; 
using LogLineHandler;
using Samples;

namespace SPLogLineTests
{
   [TestClass]
   public class SPLineTests
   {
      [TestMethod]
      public void SPLine_Timestamp()
      {
         SPLine spLine = new SPLine(new SPLogHandler(new CreateTextStreamReaderMock()), samples_general.WFS_INF_PTR_STATUS, XFSType.WFS_INF_PTR_STATUS);
         Assert.IsTrue(spLine.Timestamp == "2023-03-17 08:42:28.033");
      }

      [TestMethod]
      public void SPLine_hResult()
      {
         SPLine spLine = new SPLine(new SPLogHandler(new CreateTextStreamReaderMock()), samples_general.WFS_INF_PTR_STATUS, XFSType.WFS_INF_PTR_STATUS);
         Assert.IsTrue(spLine.HResult == "1");
      }
   }
}
