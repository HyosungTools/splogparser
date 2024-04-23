using LogFileHandler;
using LogLineHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using Samples;
using System;

namespace APLogLineTests
{
   [TestClass]
   public class APLogHandler_IdentifyLines_NDC_Tests
   {
      [TestMethod]
      public void Atm2Host11_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_1);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host11_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_2);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host11_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_3);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host11_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_4);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host11_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_5);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host11_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_6);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host11_CashDepot_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_CASHDEPOT_1);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host11_CashDepot_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST11_CASHDEPOT_2);
         Assert.IsTrue(logLine is Atm2Host11);

         Atm2Host11 apLine = (Atm2Host11)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "1");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Request to Host"));
      }

      [TestMethod]
      public void Atm2Host12_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST12_1);
         Assert.IsTrue(logLine is Atm2Host12);

         Atm2Host12 apLine = (Atm2Host12)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Unsolicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host12_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST12_2);
         Assert.IsTrue(logLine is Atm2Host12);

         Atm2Host12 apLine = (Atm2Host12)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Unsolicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host12_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST12_3);
         Assert.IsTrue(logLine is Atm2Host12);

         Atm2Host12 apLine = (Atm2Host12)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Unsolicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host12_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST12_4);
         Assert.IsTrue(logLine is Atm2Host12);

         Atm2Host12 apLine = (Atm2Host12)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Unsolicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host12_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST12_5);
         Assert.IsTrue(logLine is Atm2Host12);

         Atm2Host12 apLine = (Atm2Host12)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Unsolicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host22_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST22_1);
         Assert.IsTrue(logLine is Atm2Host22);

         Atm2Host22 apLine = (Atm2Host22)logLine;
         Assert.IsTrue(apLine.msgclass == "2" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Solicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host22_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST22_2);
         Assert.IsTrue(logLine is Atm2Host22);

         Atm2Host22 apLine = (Atm2Host22)logLine;
         Assert.IsTrue(apLine.msgclass == "2" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Solicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host22_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST22_3);
         Assert.IsTrue(logLine is Atm2Host22);

         Atm2Host22 apLine = (Atm2Host22)logLine;
         Assert.IsTrue(apLine.msgclass == "2" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Solicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host22_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST22_4);
         Assert.IsTrue(logLine is Atm2Host22);

         Atm2Host22 apLine = (Atm2Host22)logLine;
         Assert.IsTrue(apLine.msgclass == "2" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Solicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host22_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST22_5);
         Assert.IsTrue(logLine is Atm2Host22);

         Atm2Host22 apLine = (Atm2Host22)logLine;
         Assert.IsTrue(apLine.msgclass == "2" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Solicited Status to Host"));
      }

      [TestMethod]
      public void Atm2Host22_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.ATM2HOST22_6);
         Assert.IsTrue(logLine is Atm2Host22);

         Atm2Host22 apLine = (Atm2Host22)logLine;
         Assert.IsTrue(apLine.msgclass == "2" && apLine.msgsubclass == "2");
         Assert.IsTrue(apLine.english.StartsWith("Solicited Status to Host"));
      }

      [TestMethod]
      public void Host2Atm4_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_1);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_2);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_3);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_4);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_5);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_6()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_6);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_7()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_7);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_8()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_8);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_9()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_9);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }

      [TestMethod]
      public void Host2Atm4_10()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM4_10);
         Assert.IsTrue(logLine is Host2Atm4);

         Host2Atm4 apLine = (Host2Atm4)logLine;
         Assert.IsTrue(apLine.msgclass == "4" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Transaction Reply to ATM"));
      }
      public void Host2Atm1_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM1_1);
         Assert.IsTrue(logLine is Host2Atm1);

         Host2Atm1 apLine = (Host2Atm1)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Command from Host"));
      }

      [TestMethod]
      public void Host2Atm1_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM1_2);
         Assert.IsTrue(logLine is Host2Atm1);

         Host2Atm1 apLine = (Host2Atm1)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Command from Host"));
      }

      [TestMethod]
      public void Host2Atm1_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM1_4);
         Assert.IsTrue(logLine is Host2Atm1);

         Host2Atm1 apLine = (Host2Atm1)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Command from Host"));
      }

      [TestMethod]
      public void Host2Atm1_5()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM1_5);
         Assert.IsTrue(logLine is Host2Atm1);

         Host2Atm1 apLine = (Host2Atm1)logLine;
         Assert.IsTrue(apLine.msgclass == "1" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Command from Host"));
      }

      [TestMethod]
      public void Host2Atm3_1()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM3_1);
         Assert.IsTrue(logLine is Host2Atm3);

         Host2Atm3 apLine = (Host2Atm3)logLine;
         Assert.IsTrue(apLine.msgclass == "3" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Data Command"));
      }

      [TestMethod]
      public void Host2Atm3_2()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM3_2);
         Assert.IsTrue(logLine is Host2Atm3);

         Host2Atm3 apLine = (Host2Atm3)logLine;
         Assert.IsTrue(apLine.msgclass == "3" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Data Command"));
      }

      [TestMethod]
      public void Host2Atm3_3()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM3_3);
         Assert.IsTrue(logLine is Host2Atm3);

         Host2Atm3 apLine = (Host2Atm3)logLine;
         Assert.IsTrue(apLine.msgclass == "3" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Data Command"));
      }

      [TestMethod]
      public void Host2Atm3_4()
      {
         ILogFileHandler logFileHandler = new APLogHandler(new CreateTextStreamReaderMock());
         ILogLine logLine = logFileHandler.IdentifyLine(samples_ndc.HOST2ATM3_4);
         Assert.IsTrue(logLine is Host2Atm3);

         Host2Atm3 apLine = (Host2Atm3)logLine;
         Assert.IsTrue(apLine.msgclass == "3" && apLine.msgsubclass == "");
         Assert.IsTrue(apLine.english.StartsWith("Data Command"));
      }
   }
}
