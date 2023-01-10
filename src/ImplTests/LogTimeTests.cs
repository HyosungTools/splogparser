using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImplTests
{
   [TestClass]
   public class LogTimeTests
   {
      [TestMethod]
      public void GetTimeFromLogLineSuccess()
      {
         string logLine = ")02394294967295021600168650980006COMMON0009FRAMEWORK00102022/12/07001222:14 49.6930011INFORMATION0033CSpCmdDispatcher::DispatchCommand0097m_pCmdQ->Insert() {HSERVICE[4";
         string logTime = LogTime.GetTimeFromLogLine(logLine);
         Assert.IsTrue(logTime.Equals("2022-12-07T22:14:49.693"));
      }
      [TestMethod]
      public void GetTimeFromLogLineFailLogLineTooShort()
      {
         string logLine = "}30064294967295016000161907620006COMMON0009FRAMEWORK00102022/12/07001213:55 13.490001";
         string logTime = LogTime.GetTimeFromLogLine(logLine);
         Assert.IsTrue(logTime.Equals("2022-12-07T13:55:13.490"));
      }
      [TestMethod]
      public void GetTimeFromLogLineFailNoDate()
      {
         string logLine = ")02394294967295021600168650980006COMMON0009FRAMEWORK001007001222:14 49.6930011INFORMATION0033CSpCmdDispatcher::DispatchCommand0097m_pCmdQ->Insert() {HSERVICE[4";
         string logTime = LogTime.GetTimeFromLogLine(logLine);
         Assert.IsTrue(logTime.Equals("2022-01-01T00:00:00.000"));
      }
   }
}
