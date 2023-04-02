using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class WFSCIMNOTENUMBERLISTTests
   {
      [TestMethod]
      public void Test_LPPCASHIN_TABLE_1()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_TABLE_1);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatch(idLine.xfsLine, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[2, 0] == "1:1994");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_2()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_TABLE_2);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatch(idLine.xfsLine, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[2, 0] == "1:509");
         Assert.IsTrue(noteNumberList[4, 0] == "5:765");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_3()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_TABLE_3);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatch(idLine.xfsLine, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[5, 0] == "7:5");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_4()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_TABLE_4);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatch(idLine.xfsLine, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[1, 10] == "11:0");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_5()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_TABLE_5);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatch(idLine.xfsLine, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[1, 16] == "17:67");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_1()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_LIST_1);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatch(idLine.xfsLine, "(?<=usCount = \\[)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[0, 16] == "17:117");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_2()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_LIST_2);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatchList(idLine.xfsLine, "(?<=usCount = \\[)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[0, 14] == "15:1717");

      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_3()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_LIST_3);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatchList(idLine.xfsLine, "(?<=usCount = \\[)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[1, 7] == "8:88");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_4()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_LIST_4);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_CMD_CIM_CASH_IN_END);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatchList(idLine.xfsLine, "(?<=usCount = \\[)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[0, 10] == "11:101");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_5()
      {
         (XFSType xfsType, string xfsLine) idLine = IdentifyLines.XFSLine(samples_lppcashin.LPPCASHIN_LIST_5);
         Assert.IsTrue(idLine.xfsType == XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

         (bool success, string xfsMatch, string subLogLine) result = WFS.WFSMatchList(idLine.xfsLine, "(?<=usCount = \\[)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());

         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine, lUnitCount);
         Assert.IsTrue(noteNumberList[1, 0] == "1:999");
      }

      [TestMethod]
      public void Test_WFS_INF_CIM_CASH_IN_STATUS_1()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINSTATUS.usNumOfRefusedFromList(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_1);
         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 0] == null);
      }

      [TestMethod]
      public void Test_WFS_INF_CIM_CASH_IN_STATUS_2()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINSTATUS.usNumOfRefusedFromList(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_2);
         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 0] == "15:6");
      }

      [TestMethod]
      public void Test_WFS_INF_CIM_CASH_IN_STATUS_3()
      {
         (bool success, string xfsMatch, string subLogLine) result = WFSCIMCASHINSTATUS.usNumOfRefusedFromList(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_3);
         string[,] noteNumberList = WFSCIMNOTENUMBERLIST.NoteNumberListFromList(result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 0] == "1:48");
         Assert.IsTrue(noteNumberList[0, 1] == "13:2");
      }
   }
}
