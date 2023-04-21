using System;
using System.Collections.Generic;
using System.Linq;
using Impl;
using Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSIPMRETRACTMEDIAOUTTests
   {
      private string xfsLine;
      private string xfsLine2;
      private string xfsLine3;

      [TestInitialize]
      public void TestInitialize()
      {
         xfsLine = samples_ipm.WFS_CMD_IPM_RETRACT_MEDIA;
         xfsLine2 = samples_ipm.WFS_CMD_IPM_RETRACT_MEDIA_1;
         xfsLine3 = samples_ipm.WFS_CMD_IPM_RETRACT_MEDIA_2;
      }

      [TestMethod]
      public void usMedia()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMRETRACTMEDIAOUT.usMediaFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");

         result = WFSIPMRETRACTMEDIAOUT.usMediaFromList(xfsLine2);
         Assert.IsTrue(result.xfsMatch == "1");

         result = WFSIPMRETRACTMEDIAOUT.usMediaFromList(xfsLine3);
         Assert.IsTrue(result.xfsMatch == "0");

      }

      [TestMethod]
      public void wRetractLocation()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMRETRACTMEDIAOUT.wRetractLocationFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");

         result = WFSIPMRETRACTMEDIAOUT.wRetractLocationFromList(xfsLine2);
         Assert.IsTrue(result.xfsMatch == "0x0002");

         result = WFSIPMRETRACTMEDIAOUT.wRetractLocationFromList(xfsLine3);
         Assert.IsTrue(result.xfsMatch == "0");
      }

      [TestMethod]
      public void usBinNumber()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSIPMRETRACTMEDIAOUT.usBinNumberFromList(xfsLine);
         Assert.IsTrue(result.xfsMatch == "0");

         result = WFSIPMRETRACTMEDIAOUT.usBinNumberFromList(xfsLine2);
         Assert.IsTrue(result.xfsMatch == "3");

         result = WFSIPMRETRACTMEDIAOUT.usBinNumberFromList(xfsLine3);
         Assert.IsTrue(result.xfsMatch == "0");
      }
   }
}
