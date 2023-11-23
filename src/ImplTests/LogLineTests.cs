using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class LogLineTests
   {
      [TestMethod]
      public void FindNextLineSuccess()
      {
         string logLine =
             @"lpResult =
               {
                  hWnd = [0x0005032e],
	               RequestID = [0],
	               hService = [10],
	               tsTimestamp = [2022 / 09 / 25 19:31 04.714],
	               hResult = [0],
	               u.dwEventID = [801],
	               lpBuffer = [0x0050cf44]
                  {
                     wPortType = [16],
		               wPortIndex = [0],
		               wPortStatus = [0x0008],
		               lpszExtra = []
                  }
               }";

         (bool found, string oneLine, string subLogLine) = LogLine.ReadNextLine(logLine);
         Assert.IsTrue(found);
         Assert.IsTrue(oneLine.StartsWith("lpResult"));
         Assert.IsTrue(subLogLine.TrimStart().StartsWith("{"));
      }
      [TestMethod]
      public void FindNextLineFail()
      {
         string logLine =
             @"lpResult =";

         (bool found, string oneLine, string subLogLine) nextLine = LogLine.ReadNextLine(logLine);
         nextLine = LogLine.ReadNextLine(nextLine.subLogLine);
         Assert.IsFalse(nextLine.found);
         Assert.IsTrue(string.IsNullOrEmpty(nextLine.oneLine));
      }
      [TestMethod]
      public void FindLineSuccess()
      {
         string logLine =
             @"lpResult =
               {
                  hWnd = [0x0005032e],
	               RequestID = [0],
	               hService = [10],
	               tsTimestamp = [2022 / 09 / 25 19:31 04.714],
	               hResult = [0],
	               u.dwEventID = [801],
	               lpBuffer = [0x0050cf44]
                  {
                     wPortType = [16],
		               wPortIndex = [0],
		               wPortStatus = [0x0008],
		               lpszExtra = []
                  }
               }";

         (bool found, string oneLine, string subLogLine) = LogLine.FindLine(logLine, "[801]");
         Assert.IsTrue(found);
         Assert.IsTrue(oneLine.TrimStart().StartsWith("u.dwEventID"));
         Assert.IsTrue(subLogLine.TrimStart().StartsWith("lpBuffer"));
      }
      [TestMethod]
      public void FindLineFail()
      {
         string logLine =
             @"lpResult =
               {
                  hWnd = [0x0005032e],
	               RequestID = [0],
	               hService = [10],
	               tsTimestamp = [2022 / 09 / 25 19:31 04.714],
	               hResult = [0],
	               u.dwEventID = [801],
	               lpBuffer = [0x0050cf44]
                  {
                     wPortType = [16],
		               wPortIndex = [0],
		               wPortStatus = [0x0008],
		               lpszExtra = []
                  }
               }";

         (bool found, string oneLine, string subLogLine) = LogLine.FindLine(logLine, "[000]");
         Assert.IsFalse(found);
         Assert.IsTrue(string.IsNullOrEmpty(oneLine));
         Assert.IsTrue(subLogLine == logLine);
      }
   }
}
