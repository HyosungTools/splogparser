using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSCDMSTATUSTests
   {
      static string logLine = samples_cdm.WFS_INF_CDM_STATUS_1;

      [TestMethod]
      public void fwDevice()
      {
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         (bool success, string xfsMatch, string subLogLine) result2 = WFSCDMSTATUS.fwDeviceFromCDMStatus(logLine);
         Assert.IsTrue(result2.xfsMatch == "0");
      }
      [TestMethod]
      public void fwSafeDoor()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwSafeDoorFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "1");
      }
      [TestMethod]
      public void fwDispenser()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwDispenserFromSDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "2");
      }
      [TestMethod]
      public void fwIntermediateStacker()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwIntermediateStackerFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "3");
      }
      [TestMethod]
      public void fwPosition()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwPositionFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "4");
      }
      [TestMethod]
      public void fwShutter()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwShutterFromSDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "5");
      }
      [TestMethod]
      public void fwPositionStatus()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwPositionStatusFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "6");
      }
      [TestMethod]
      public void fwTransport()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwTransportFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "7");
      }
      [TestMethod]
      public void fwTransportStatus()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.fwTransportStatusFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "8");
      }
      [TestMethod]
      public void wDevicePosition()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.wDevicePositionFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "9");
      }
      [TestMethod]
      public void usPowerSaveRecoveryTime()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.usPowerSaveRecoveryTimeFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "10");
      }
      [TestMethod]
      public void wAntiFraudModule()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCDMSTATUS.wAntiFraudModuleFromCDMStatus(logLine);
         Assert.IsTrue(result.xfsMatch == "11");
      }
   }
}
