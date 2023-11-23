using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using Samples;
using Contract;
using System.IO;
using System.Text;

namespace SPLogLineTests
{
   [TestClass]
   public class SPLogHandler_ReadLine_Tests
   {
      /// <summary>
      /// Test different end-of-line scenarios when reading an nwlog file. 
      /// End-of-line is a misnomer; it's really end of information-block. 
      /// So the start could be 'lpResult =' and 30 log lines later end with '}'
      /// 
      /// Note: make sure you start the string on a new-line so you dont accidentally add a \r\n to the test. 
      /// 
      /// </summary>

      /* 1 */
      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLTailIsWellFormed()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLTailWellFormed);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLTailIsMalformed()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLTailIsMalformed);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLLogLineIsDmp()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLLogLineIsDmp);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("F7 \r\n"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectlpResultAndContinue()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectlpResultAndContinue);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("\r\n}"));
         Assert.IsTrue(logLine.Contains("lpResult"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLOutputIsTable()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLOutputIsTable);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("54[0]0	\r\n"));
         Assert.IsTrue(logLine.Contains("NoteNumbers:"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLNextLineStartsWithADigit()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLNextLineStartsWithADigit);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("count(0)\r\n"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLDontStopOnOpenAngleBrackets()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLDontStopOnOpenAngleBrackets);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("[0][0][0]\r\n"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLRead2Lines()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLRead2Lines);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith(" NULL\r\n)"));

         logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith(" NULL\r\n)"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLNextCharIsSpace()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLNextCharIsSpace);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("dwEnableRealAcceptCount(0)\r\n"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLTableDataIncludesDash()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLTableDataIncludesDash);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("UseTakenSensor	 0\r\n"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLAnotherFormofTable()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLAnotherFormofTable);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("------------------------------------\r\n"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_DetectEOLReadMultipleLines()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLReadMultipleLines);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("[0x30fe083c]\r\n}"));

         logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("[0x30fcca2c]\r\n)"));

         logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("[0x30fcca2c]\r\n}"));

         logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("lpQueryDetails = NULL\r\n)"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_INF_CDM_STATUS()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CDM_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("wAntiFraudModule = [0]\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CDM_DISPENSE()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CDM_DISPENSE);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("ulCashBox = [0]\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CDM_PRESENT()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CDM_PRESENT);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("lpBuffer = NULL\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CDM_REJECT()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CDM_REJECT);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("lpBuffer = NULL\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CDM_RETRACT()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CDM_RESET()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_SRVE_CDM_CASHUNITINFOCHANGED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_SRVE_CDM_ITEMSTAKEN()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_INF_CIM_STATUS()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("wAntiFraudModule = [0]\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_INF_CIM_CASH_UNIT_INFO()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_CASH_UNIT_INFO);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n\t\t}\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_INF_CIM_CASH_IN_STATUS()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_CASH_IN_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN_START()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_CASH_IN_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN_END()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN_ROLLBACK()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CIM_CASH_IN_ROLLBACK);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CIM_RETRACT()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CIM_RETRACT);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_CMD_CIM_RESET()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CIM_RESET);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_USRE_CIM_CASHUNITTHRESHOLD()
      {
         SPLogHandler logHandler = new SPLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_USRE_CIM_CASHUNITTHRESHOLD);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_SRVE_CIM_CASHUNITINFOCHANGED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_SRVE_CIM_ITEMSTAKEN()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_EXEE_CIM_INPUTREFUSE()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_SRVE_CIM_ITEMSPRESENTED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_SRVE_CIM_ITEMSINSERTED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_EXEE_CIM_NOTEERROR()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void SPLogHandler_ReadLine_Find_WFS_SRVE_CIM_MEDIADETECTED()
      {
         Assert.IsTrue(true);
      }

   }
}
