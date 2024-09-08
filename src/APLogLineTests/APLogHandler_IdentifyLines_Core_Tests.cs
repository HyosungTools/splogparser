using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using APSamples;
using System;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_IdentifyLines_Core_Tests
   {
      /* CORE */

      /* FiservDNA */

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Amount_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Amount_1);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Amount);

         Core_ProcessWithdrawalTransaction_Amount apLine = (Core_ProcessWithdrawalTransaction_Amount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Amount);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 12:18:15.179");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "FiservDNA");
         Assert.IsTrue(apLine.amount == "500");
      }

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Account_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Account_1);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Account);

         Core_ProcessWithdrawalTransaction_Account apLine = (Core_ProcessWithdrawalTransaction_Account)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Account);
         Assert.IsTrue(apLine.Timestamp == "2023-11-16 11:09:19.900");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "FiservDNA");
         Assert.IsTrue(apLine.account == "XXX5754");
      }

      [TestMethod]
      public void Core_RequiredBillMixList_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_RequiredBillMixList_1);
         Assert.IsTrue(logLine is Core_RequiredBillMixList);

         Core_RequiredBillMixList apLine = (Core_RequiredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_RequiredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 12:18:15.596");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "FiservDNA");
         Assert.IsTrue(apLine.requiredbillmixlist == "100~0|20~6|5~76");
      }

      [TestMethod]
      public void Core_DispensedAmount_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_DispensedAmount_1);
         Assert.IsTrue(logLine is Core_DispensedAmount);

         Core_DispensedAmount apLine = (Core_DispensedAmount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_DispensedAmount);
         Assert.IsTrue(apLine.Timestamp == "2023-06-07 12:18:33.379");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "FiservDNA");
         Assert.IsTrue(apLine.amount == "500");
      }


      /* JackHenry */

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Amount_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcesWithdrawalTransaction_Amount_2);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Amount);

         Core_ProcessWithdrawalTransaction_Amount apLine = (Core_ProcessWithdrawalTransaction_Amount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Amount);
         Assert.IsTrue(apLine.Timestamp == "2023-10-05 16:16:11.896");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "JackHenry");
         Assert.IsTrue(apLine.amount == "60");
      }

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Account_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Account_2);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Account);

         Core_ProcessWithdrawalTransaction_Account apLine = (Core_ProcessWithdrawalTransaction_Account)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Account);
         Assert.IsTrue(apLine.Timestamp == "2023-10-05 14:46:27.344");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "JackHenry");
         Assert.IsTrue(apLine.account == "x4361");
      }

      [TestMethod]
      public void Core_RequiredBillMixList_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_RequiredBillMixList_2);
         Assert.IsTrue(logLine is Core_RequiredBillMixList);

         Core_RequiredBillMixList apLine = (Core_RequiredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_RequiredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-11-21 12:13:05.982");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "JackHenry");
         Assert.IsTrue(apLine.requiredbillmixlist == "100~0|20~4|5~4|1~0");
      }

      [TestMethod]
      public void Core_DispensedAmount_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_DispensedAmount_2);
         Assert.IsTrue(logLine is Core_DispensedAmount);

         Core_DispensedAmount apLine = (Core_DispensedAmount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_DispensedAmount);
         Assert.IsTrue(apLine.Timestamp == "2023-11-21 12:13:14.418");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "JackHenry");
         Assert.IsTrue(apLine.amount == "100");
      }


      /* SymXchange */

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Amount_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Amount_3);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Amount);

         Core_ProcessWithdrawalTransaction_Amount apLine = (Core_ProcessWithdrawalTransaction_Amount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Amount);
         Assert.IsTrue(apLine.Timestamp == "2023-11-13 10:37:45.919");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "SymXchange");
         Assert.IsTrue(apLine.amount == "800");
      }

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Account_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Account_3);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Account);

         Core_ProcessWithdrawalTransaction_Account apLine = (Core_ProcessWithdrawalTransaction_Account)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Account);
         Assert.IsTrue(apLine.Timestamp == "2023-11-13 10:37:45.914");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "SymXchange");
         Assert.IsTrue(apLine.account == "XXXXXX8451");
      }

      [TestMethod]
      public void Core_RequiredBillMixList_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_RequiredBillMixList_3);
         Assert.IsTrue(logLine is Core_RequiredBillMixList);

         Core_RequiredBillMixList apLine = (Core_RequiredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_RequiredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-11-22 14:53:08.481");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "SymXchange");
         Assert.IsTrue(apLine.requiredbillmixlist == "100~0|20~25|5~0|1~0");
      }

      [TestMethod]
      public void Core_DispensedAmount_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_DispensedAmount_3);
         Assert.IsTrue(logLine is Core_DispensedAmount);

         Core_DispensedAmount apLine = (Core_DispensedAmount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_DispensedAmount);
         Assert.IsTrue(apLine.Timestamp == "2023-11-22 14:53:20.513");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "SymXchange");
         Assert.IsTrue(apLine.amount == "500");
      }


      /* CUAnswers */

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Amount_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Amount_4);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Amount);

         Core_ProcessWithdrawalTransaction_Amount apLine = (Core_ProcessWithdrawalTransaction_Amount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Amount);
         Assert.IsTrue(apLine.Timestamp == "2023-11-06 14:59:17.355");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CUAnswers");
         Assert.IsTrue(apLine.amount == "60");
      }

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Account_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Account_4);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Account);

         Core_ProcessWithdrawalTransaction_Account apLine = (Core_ProcessWithdrawalTransaction_Account)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Account);
         Assert.IsTrue(apLine.Timestamp == "2023-11-06 14:59:17.350");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CUAnswers");
         Assert.IsTrue(apLine.account == "XX9725");
      }

      [TestMethod]
      public void Core_RequiredBillMixList_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_RequiredBillMixList_4);
         Assert.IsTrue(logLine is Core_RequiredBillMixList);

         Core_RequiredBillMixList apLine = (Core_RequiredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_RequiredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-11-06 14:59:23.484");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CUAnswers");
         Assert.IsTrue(apLine.requiredbillmixlist == "100~0|20~2|5~4|1~0");
      }

      [TestMethod]
      public void Core_DispensedAmount_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_DispensedAmount_4);
         Assert.IsTrue(logLine is Core_DispensedAmount);

         Core_DispensedAmount apLine = (Core_DispensedAmount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_DispensedAmount);
         Assert.IsTrue(apLine.Timestamp == "2023-11-06 14:59:31.859");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CUAnswers");
         Assert.IsTrue(apLine.amount == "60");
      }

      /* CMFlex */


      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Amount_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Amount_5);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Amount);

         Core_ProcessWithdrawalTransaction_Amount apLine = (Core_ProcessWithdrawalTransaction_Amount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Amount);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 10:32:45.152");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CMCFlex");
         Assert.IsTrue(apLine.amount == "1");
      }

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Account_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Account_5);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Account);

         Core_ProcessWithdrawalTransaction_Account apLine = (Core_ProcessWithdrawalTransaction_Account)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Account);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 10:32:45.144");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CMCFlex");
         Assert.IsTrue(apLine.account == "x2754");
      }

      [TestMethod]
      public void Core_RequiredBillMixList_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_RequiredBillMixList_5);
         Assert.IsTrue(logLine is Core_RequiredBillMixList);

         Core_RequiredBillMixList apLine = (Core_RequiredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_RequiredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 10:33:03.975");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CMCFlex");
         Assert.IsTrue(apLine.requiredbillmixlist == "100~0|20~0|5~0|1~1");
      }

      [TestMethod]
      public void Core_DispensedAmount_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_DispensedAmount_5);
         Assert.IsTrue(logLine is Core_DispensedAmount);

         Core_DispensedAmount apLine = (Core_DispensedAmount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_DispensedAmount);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 10:33:12.687");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "CMCFlex");
         Assert.IsTrue(apLine.amount == "1");
      }

      /* KeyBridge */


      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Amount_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Amount_6);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Amount);

         Core_ProcessWithdrawalTransaction_Amount apLine = (Core_ProcessWithdrawalTransaction_Amount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Amount);
         Assert.IsTrue(apLine.Timestamp == "2023-12-12 10:39:36.985");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "KeyBridge");
         Assert.IsTrue(apLine.amount == "100");
      }

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Account_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Account_6);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Account);

         Core_ProcessWithdrawalTransaction_Account apLine = (Core_ProcessWithdrawalTransaction_Account)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Account);
         Assert.IsTrue(apLine.Timestamp == "2023-12-12 10:39:36.984");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "KeyBridge");
         Assert.IsTrue(apLine.account == "XXX3160");
      }

      [TestMethod]
      public void Core_RequiredBillMixList_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_RequiredBillMixList_6);
         Assert.IsTrue(logLine is Core_RequiredBillMixList);

         Core_RequiredBillMixList apLine = (Core_RequiredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_RequiredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-12-12 02:02:49.495");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "KeyBridge");
         Assert.IsTrue(apLine.requiredbillmixlist == "20~1");
      }

      [TestMethod]
      public void Core_DispensedAmount_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_DispensedAmount_6);
         Assert.IsTrue(logLine is Core_DispensedAmount);

         Core_DispensedAmount apLine = (Core_DispensedAmount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_DispensedAmount);
         Assert.IsTrue(apLine.Timestamp == "2023-12-12 02:02:57.167");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "KeyBridge");
         Assert.IsTrue(apLine.amount == "20");
      }

      /* AccessAdvantage */


      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Amount_7()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Amount_7);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Amount);

         Core_ProcessWithdrawalTransaction_Amount apLine = (Core_ProcessWithdrawalTransaction_Amount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Amount);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 09:03:47.232");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "AccessAdvantage");
         Assert.IsTrue(apLine.amount == "25");
      }

      [TestMethod]
      public void Core_ProcessWithdrawalTransaction_Account_7()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_ProcessWithdrawalTransaction_Account_7);
         Assert.IsTrue(logLine is Core_ProcessWithdrawalTransaction_Account);

         Core_ProcessWithdrawalTransaction_Account apLine = (Core_ProcessWithdrawalTransaction_Account)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_ProcessWithdrawalTransaction_Account);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 09:03:47.232");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "AccessAdvantage");
         Assert.IsTrue(apLine.account == "X4050");
      }

      [TestMethod]
      public void Core_RequiredBillMixList_7()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_RequiredBillMixList_7);
         Assert.IsTrue(logLine is Core_RequiredBillMixList);

         Core_RequiredBillMixList apLine = (Core_RequiredBillMixList)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_RequiredBillMixList);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 09:03:48.015");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "AccessAdvantage");
         Assert.IsTrue(apLine.requiredbillmixlist == "100~0|20~1|5~1|1~0");
      }

      [TestMethod]
      public void Core_DispensedAmount_7()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock(), ParseType.AP, APLine.Factory); 
         ILogLine logLine = logFileHandler.IdentifyLine(samples_core.Core_DispensedAmount_7);
         Assert.IsTrue(logLine is Core_DispensedAmount);

         Core_DispensedAmount apLine = (Core_DispensedAmount)logLine;
         Assert.IsTrue(apLine.apType == APLogType.Core_DispensedAmount);
         Assert.IsTrue(apLine.Timestamp == "2023-12-04 09:03:55.474");
         Assert.IsTrue(apLine.HResult == "");

         Assert.IsTrue(apLine.name == "AccessAdvantage");
         Assert.IsTrue(apLine.amount == "25");
      }

   }
}
