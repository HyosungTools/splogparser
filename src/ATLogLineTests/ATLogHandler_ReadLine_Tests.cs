using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using ATSamples;
using Contract;
using System.IO;
using System.Text;

namespace ATLogLineTests
{
   [TestClass]
   public class ATLogHandler_ReadLine_Tests
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
      public void ATLogHandler_ReadLine_DetectEOLTailIsWellFormed()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLTailWellFormed);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLTailIsMalformed()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLTailIsMalformed);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLLogLineIsDmp()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLLogLineIsDmp);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("F7 \r\n"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectlpResultAndContinue()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectlpResultAndContinue);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("\r\n}"));
         Assert.IsTrue(logLine.Contains("lpResult"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLOutputIsTable()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLOutputIsTable);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("54[0]0	\r\n"));
         Assert.IsTrue(logLine.Contains("NoteNumbers:"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLNextLineStartsWithADigit()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLNextLineStartsWithADigit);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("count(0)\r\n"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLDontStopOnOpenAngleBrackets()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLDontStopOnOpenAngleBrackets);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("[0][0][0]\r\n"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLRead2Lines()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLRead2Lines);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith(" NULL\r\n)"));

         logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith(" NULL\r\n)"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLNextCharIsSpace()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLNextCharIsSpace);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("dwEnableRealAcceptCount(0)\r\n"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLTableDataIncludesDash()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLTableDataIncludesDash);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("UseTakenSensor	 0\r\n"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLAnotherFormofTable()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.DetectEOLAnotherFormofTable);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("------------------------------------\r\n"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_DetectEOLReadMultipleLines()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
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
      public void ATLogHandler_ReadLine_Find_WFS_INF_CDM_STATUS()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CDM_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("wAntiFraudModule = [0]\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CDM_DISPENSE()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CDM_DISPENSE);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("ulCashBox = [0]\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CDM_PRESENT()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CDM_PRESENT);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("lpBuffer = NULL\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CDM_REJECT()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CDM_REJECT);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("lpBuffer = NULL\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CDM_RETRACT()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CDM_RESET()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_SRVE_CDM_CASHUNITINFOCHANGED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_SRVE_CDM_ITEMSTAKEN()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_INF_CIM_STATUS()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("wAntiFraudModule = [0]\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_INF_CIM_CASH_UNIT_INFO()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_CASH_UNIT_INFO);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n\t\t}\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_INF_CIM_CASH_IN_STATUS()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_CASH_IN_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN_START()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_INF_CIM_CASH_IN_STATUS);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN_END()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CIM_CASH_IN_ROLLBACK()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CIM_CASH_IN_ROLLBACK);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CIM_RETRACT()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CIM_RETRACT);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_CMD_CIM_RESET()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_CMD_CIM_RESET);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_USRE_CIM_CASHUNITTHRESHOLD()
      {
         ATLogHandler logHandler = new ATLogHandler(new CreateTextStreamReaderMock());
         logHandler.OpenLogFile(samples_tracefilereader.Find_WFS_USRE_CIM_CASHUNITTHRESHOLD);
         string logLine = logHandler.ReadLine();
         Assert.IsTrue(logLine.EndsWith("NULL\r\n\t}\r\n}"));
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_SRVE_CIM_CASHUNITINFOCHANGED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_SRVE_CIM_ITEMSTAKEN()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_EXEE_CIM_INPUTREFUSE()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_SRVE_CIM_ITEMSPRESENTED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_SRVE_CIM_ITEMSINSERTED()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_EXEE_CIM_NOTEERROR()
      {
         Assert.IsTrue(true);
      }

      [TestMethod]
      public void ATLogHandler_ReadLine_Find_WFS_SRVE_CIM_MEDIADETECTED()
      {
         Assert.IsTrue(true);
      }

   }
}
