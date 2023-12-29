using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using APSamples;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_IdentifyLines_HelperFunctions_Tests
   {
      /* CORE */

      /* FiservDNA */

      [TestMethod]
      public void HelperFunctions_GetConfiguredBillMixList()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_helperfunctions.HelperFunctions_GetConfiguredBillMixList);
         Assert.IsTrue(logLine is HelperFunctions_GetConfiguredBillMixList);

         HelperFunctions_GetConfiguredBillMixList apLine = (HelperFunctions_GetConfiguredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.HelperFunctions_GetConfiguredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-12-19 20:18:13.380");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.configuredbillmixlist == "20~2|5~0");

      }

      [TestMethod]
      public void HelperFunctions_GetFewestBillMixList()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_helperfunctions.HelperFunctions_GetFewestBillMixList);
         Assert.IsTrue(logLine is HelperFunctions_GetFewestBillMixList);

         HelperFunctions_GetFewestBillMixList apLine = (HelperFunctions_GetFewestBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.HelperFunctions_GetFewestBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-12-19 22:30:32.531");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.fewestbillmixlist == "20~5|5~0");

      }
   }
}
