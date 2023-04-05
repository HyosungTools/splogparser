using System;
using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class IdentifyLineTests
   {

      [TestMethod]
      public void Identify_None()
      {
         string logLine = samples_general.WFS_NONE; 

         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.None);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_STATUS_1()
      {
         string logLine = samples_cdm.WFS_INF_CDM_STATUS_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CDM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
         Console.WriteLine(result.xfsLine);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_STATUS_2()
      {
         string logLine = samples_cdm.WFS_INF_CDM_STATUS_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CDM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
         Console.WriteLine(result.xfsLine);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_STATUS_3()
      {
         string logLine = samples_cdm.WFS_INF_CDM_STATUS_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CDM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
         Console.WriteLine(result.xfsLine);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_CASH_UNIT_INFO_1()
      {
         string logLine = samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CDM_CASH_UNIT_INFO);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
         Console.WriteLine(result.xfsLine);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_CASH_UNIT_INFO_2()
      {
         string logLine = samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CDM_CASH_UNIT_INFO);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
         Console.WriteLine(result.xfsLine);
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_CASH_UNIT_INFO_3()
      {
         string logLine = samples_cdm.WFS_INF_CDM_CASH_UNIT_INFO_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CDM_CASH_UNIT_INFO);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
         Console.WriteLine(result.xfsLine);
      }

      [TestMethod]
      public void Identify_WFS_CMD_CDM_DISPENSE()
      {
         string logLine = samples_cdm.WFS_CMD_CDM_DISPENSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_DISPENSE);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CDM_PRESENT_1()
      {
         string logLine = samples_cdm.WFS_CMD_CDM_PRESENT_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_PRESENT);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }
      [TestMethod]
      public void Identify_WFS_CMD_CDM_REJECT_1()
      {
         string logLine = samples_cdm.WFS_CMD_CDM_REJECT_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_REJECT);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CMD_RETRACT_1()
      {
         string logLine = samples_cdm.WFS_CMD_CDM_RETRACT_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_RETRACT);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }
      [TestMethod]
      public void Identify_WFS_CMD_CDM_RESET_1()
      {
         string logLine = samples_cdm.WFS_CMD_CDM_RESET_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CDM_RESET);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }
      [TestMethod]
      public void Identify_WFS_SRVE_CDM_CASHUNITINFOCHANGED_1()
      {
         string logLine = samples_cdm.WFS_SRVE_CDM_CASHUNITINFOCHANGED_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }
      [TestMethod]
      public void Identify_WFS_SRVE_CDM_ITEMSTAKEN()
      {
         string logLine = samples_cdm.WFS_SRVE_CDM_ITEMSTAKEN_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CDM_ITEMSTAKEN);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      // C  I  M  

      [TestMethod]
      public void Identify_WFS_INF_CIM_STATUS_1()
      {
         string logLine = samples_cim.WFS_INF_CIM_STATUS_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_STATUS_2()
      {
         string logLine = samples_cim.WFS_INF_CIM_STATUS_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_STATUS_3()
      {
         string logLine = samples_cim.WFS_INF_CIM_STATUS_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_UNIT_INFO_1()
      {
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_UNIT_INFO_2()
      {
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_UNIT_INFO_3()
      {
         string logLine = samples_cim.WFS_INF_CIM_CASH_UNIT_INFO_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_IN_STATUS_1()
      {
         string logLine = samples_cim.WFS_INF_CIM_CASH_IN_STATUS_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_IN_STATUS_2()
      {
         string logLine = samples_cim.WFS_INF_CIM_CASH_IN_STATUS_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_CASH_IN_STATUS_3()
      {
         string logLine = samples_cim.WFS_INF_CIM_CASH_IN_STATUS_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_CASH_IN_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }


      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_START_1()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_START_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_START);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_START_2()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_START_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_START);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }
      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_START_3()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_START_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_START);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_1()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_2()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_3()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_END_1()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_END_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_END);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_END_2()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_END_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_END);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_END_3()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_END_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_END);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_ROLLBACK_1()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_ROLLBACK_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_ROLLBACK_2()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_ROLLBACK_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_CASH_IN_ROLLBACK_3()
      {
         string logLine = samples_cim.WFS_CMD_CIM_CASH_IN_ROLLBACK_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_RETRACT_1()
      {
         string logLine = samples_cim.WFS_CDM_CIM_RETRACT_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_RETRACT);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_RETRACT_2()
      {
         string logLine = samples_cim.WFS_CDM_CIM_RETRACT_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_RETRACT);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_RETRACT_3()
      {
         string logLine = samples_cim.WFS_CDM_CIM_RETRACT_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_RETRACT);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }


      [TestMethod]
      public void Identify_WFS_CMD_CIM_RESET_1()
      {
         string logLine = samples_cim.WFS_CDM_CIM_RESET_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_RESET);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_RESET_2()
      {
         string logLine = samples_cim.WFS_CDM_CIM_RESET_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_RESET);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_CMD_CIM_RESET_3()
      {
         string logLine = samples_cim.WFS_CDM_CIM_RESET_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_CMD_CIM_RESET);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }


      [TestMethod]
      public void Identify_WFS_USRE_CIM_CASHUNITTHRESHOLD_1()
      {
         string logLine = samples_cim.WFS_USRE_CIM_CASHUNITTHRESHOLD_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }


      [TestMethod]
      public void Identify_WFS_SRVE_CIM_CASHUNITINFOCHANGED_1()
      {
         string logLine = samples_cim.WFS_SRVE_CIM_CASHUNITINFOCHANGED_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }


      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSTAKEN_1()
      {
         string logLine = samples_cim.WFS_SRVE_CIM_ITEMSTAKEN_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CIM_ITEMSTAKEN);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_EXEE_CIM_INPUTREFUSE_1()
      {
         string logLine = samples_cim.WFS_EXEE_CIM_INPUTREFUSE_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_EXEE_CIM_INPUTREFUSE);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSPRESENTED_1()
      {
         string logLine = samples_cim.WFS_SRVE_CIM_ITEMSPRESENTED_1;

         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CIM_ITEMSPRESENTED);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSINSERTED_1()
      {
         string logLine = samples_cim.WFS_SRVE_CIM_ITEMSINSERTED_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CIM_ITEMSINSERTED);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSINSERTED_2()
      {
         string logLine = samples_cim.WFS_SRVE_CIM_ITEMSINSERTED_2;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CIM_ITEMSINSERTED);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_SRVE_CIM_ITEMSINSERTED_3()
      {
         string logLine = samples_cim.WFS_SRVE_CIM_ITEMSINSERTED_3;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CIM_ITEMSINSERTED);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_EXEE_CIM_NOTEERROR_1()
      {
         string logLine = samples_cim.WFS_EXEE_CIM_NOTEERROR_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_EXEE_CIM_NOTEERROR);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_SRVE_CIM_MEDIADETECTED_1()
      {
         string logLine = samples_cim.WFS_SRVE_CIM_MEDIADETECTED_1;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_SRVE_CIM_MEDIADETECTED);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_PTR_STATUS()
      {
         string logLine = samples_general.WFS_INF_PTR_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_PTR_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_IDC_STATUS()
      {
         string logLine = samples_general.WFS_INF_IDC_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IDC_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CDM_STATUS()
      {
         string logLine = samples_general.WFS_INF_CDM_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CDM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_PIN_STATUS()
      {
         string logLine = samples_general.WFS_INF_PIN_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_PIN_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [Ignore]
      [TestMethod]
      public void Identify_WFS_INF_CHK_STATUS()
      {
         // TODO 

      }

      [Ignore]
      [TestMethod]
      public void Identify_WFS_INF_DEP_STATUS()
      {
         // TODO
      }

      [TestMethod]
      public void Identify_WFS_INF_TTU_STATUS()
      {
         string logLine = samples_general.WFS_INF_TTU_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_TTU_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_SIU_STATUS()
      {
         string logLine = samples_general.WFS_INF_SIU_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_SIU_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_VDM_STATUS()
      {
         string logLine = samples_general.WFS_INF_VDM_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_VDM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [TestMethod]
      public void Identify_WFS_INF_CAM_STATUS()
      {
         string logLine = samples_general.WFS_INF_CAM_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CAM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [Ignore]
      [TestMethod]
      public void Identify_WFS_INF_ALM_STATUS()
      {
         // TODO 
      }

      [Ignore]
      [TestMethod]
      public void Identify_WFS_INF_CEU_STATUS()
      {
         // TODO 
      }

      [TestMethod]
      public void Identify_WFS_INF_CIM_STATUS()
      {
         string logLine = samples_general.WFS_INF_CIM_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_CIM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }

      [Ignore]
      [TestMethod]
      public void Identify_WFS_INF_CRD_STATUS()
      {
         // TODO 
      }

      [Ignore]
      [TestMethod]
      public void Identify_WFS_INF_BCR_STATUS()
      {
         // TODO 
      }

      [TestMethod]
      public void Identify_WFS_INF_IPM_STATUS()
      {
         string logLine = samples_general.WFS_INF_IPM_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
         Assert.IsTrue(result.xfsType == XFSType.WFS_INF_IPM_STATUS);
         Assert.IsTrue(result.xfsLine.StartsWith("lpResult"));
      }
   }
}
