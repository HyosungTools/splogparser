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
            ";

         string logTime = lpResult.tsTimestamp(lpResultLine);
         Assert.IsTrue(logTime.Equals("2023/02/01 21:03 11.557"));
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
         Assert.IsTrue(logTime.Equals("2023/02/01 21:03 11.557"));
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

         string logTime = lpResult.hResult(lpResultLine);
         Assert.IsTrue(logTime.Equals("-123"));
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

         string logTime = lpResult.hResult(lpResultLine);
         Assert.IsTrue(logTime.Equals(""));
      }
   }
}
