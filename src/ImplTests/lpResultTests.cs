using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class lpResultTests
   {
      [TestMethod]
      public void GetlpResultTime()
      {
         string xfsLine = Samples.samples_general.WFS_INF_PTR_STATUS;
         (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(xfsLine);
         string logTime = lpResult.tsTimestamp(result.xfsLine);
         Assert.IsTrue(logTime.Equals("2023-03-17 08:42:28.033"));
      }

      [TestMethod]
      public void GetFirstlpResultTime()
      {
         string lpResultLine = @"
            lpResult =
            {
	            hWnd = [0x000100dc],
	            RequestID = [60984],
	            hService = [10],
	            tsTimestamp = [2023/02/01 21:03 11.557],
	            hResult = [-123],
	            u.dwCommandCode = [302],
	            lpBuffer = [0x12416bec]
	            tsTimestamp = [2023/02/01 00:00 00.000],
            ";

         string logTime = lpResult.tsTimestamp(lpResultLine);
         Assert.IsTrue(logTime.Equals("2023-02-01 21:03:11.557"));
      }

      [TestMethod]
      public void GetDefaultlpResultTime()
      {
         string lpResultLine = @"
            lpResult =
            {
	            hWnd = [0x000100dc],
	            RequestID = [60984],
	            hService = [10],
	            hResult = [-123],
	            u.dwCommandCode = [302],
	            lpBuffer = [0x12416bec]
            ";

         string logTime = lpResult.tsTimestamp(lpResultLine);
         Assert.IsTrue(logTime.Equals("2022-01-01 00:00:00.000"));
      }

      [TestMethod]
      public void GetlpResulthResult()
      {
         string lpResultLine = @"
            lpResult =
            {
	            hWnd = [0x000100dc],
	            RequestID = [60984],
	            hService = [10],
	            tsTimestamp = [2023/02/01 21:03 11.557],
	            hResult = [-123],
	            u.dwCommandCode = [302],
	            lpBuffer = [0x12416bec]
	            tsTimestamp = [2023/02/01 00:00 00.000],
            ";

         string hResult = lpResult.hResult(lpResultLine);
         Assert.IsTrue(hResult.Equals("-123"));
      }

      [TestMethod]
      public void GetlpResulthResult0()
      {
         string lpResultLine = @"
            lpResult =
            {
	            hWnd = [0x000100dc],
	            RequestID = [60984],
	            hService = [10],
	            tsTimestamp = [2023/02/01 21:03 11.557],
	            hResult = [0],
	            u.dwCommandCode = [302],
	            lpBuffer = [0x12416bec]
	            tsTimestamp = [2023/02/01 00:00 00.000],
            ";

         string hResult = lpResult.hResult(lpResultLine);
         Assert.IsTrue(hResult.Equals(""));
      }
   }
}
