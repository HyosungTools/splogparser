
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class _wfs_note_numbersTests
   {
      [TestMethod]
      public void Test_LPPCASHIN_TABLE_1()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_TABLE_1, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string [,] noteNumberList = _wfs_note_numbers.NoteNumberListFromTable(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[2, 0] == "1:1994");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_2()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_TABLE_2, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromTable(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[2, 0] == "1:509");
         Assert.IsTrue(noteNumberList[4, 0] == "5:765");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_3()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_TABLE_3, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromTable(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[5, 0] == "7:5");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_4()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_TABLE_4, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromTable(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[1, 10] == "11:0");
      }

      [TestMethod]
      public void Test_LPPCASHIN_TABLE_5()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_TABLE_5, "(?<=usCount=)(\\d+)");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromTable(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[1, 16] == "17:67");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_1()
      {
        (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_LIST_1, "(?<=usCount = )\\[(\\d+)\\]");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 16] == "17:117");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_2()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_LIST_2, "(?<=usCount = )\\[(\\d+)\\]");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 14] == "15:1717");

      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_3()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_LIST_3, "(?<=usCount = )\\[(\\d+)\\]");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[1, 7] == "8:88");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_4()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_LIST_4, "(?<=usCount = )\\[(\\d+)\\]");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 10] == "11:101");
      }

      [TestMethod]
      public void Test_LPPCASHIN_LIST_5()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_base.GenericMatch(samples_lppcashin.LPPCASHIN_LIST_5, "(?<=usCount = )\\[(\\d+)\\]");
         int lUnitCount = int.Parse(result.xfsMatch.Trim());
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(lUnitCount, result.subLogLine);
         Assert.IsTrue(noteNumberList[1, 0] == "1:999");
      }

      [TestMethod]
      public void Test_WFS_INF_CIM_CASH_IN_STATUS_1()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_cash_in_status.usNumOfRefused(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_1);
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(1, result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 0] == null);
      }

      [TestMethod]
      public void Test_WFS_INF_CIM_CASH_IN_STATUS_2()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_cash_in_status.usNumOfRefused(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_2);
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(1, result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 0] == "15:6");
      }

      [TestMethod]
      public void Test_WFS_INF_CIM_CASH_IN_STATUS_3()
      {
         (bool success, string xfsMatch, string subLogLine) result = _wfs_inf_cim_cash_in_status.usNumOfRefused(samples_cim.WFS_INF_CIM_CASH_IN_STATUS_3);
         string[,] noteNumberList = _wfs_note_numbers.NoteNumberListFromList(1, result.subLogLine);
         Assert.IsTrue(noteNumberList[0, 0] == "1:48");
         Assert.IsTrue(noteNumberList[0, 1] == "13:2");
      }
   }
}
