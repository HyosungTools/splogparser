using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogFileHandler;
using LogLineHandler;
using Samples;
using Contract;

namespace SPFlatLogLineTests
{
    [TestClass]
    public class SPFlatLineTests
    {

        [TestMethod]
        public void Parse_CDM_UnitID_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitID);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_UnitType_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitType);
            Assert.IsTrue(logLine is CDM_UnitType);

            CDM_UnitType line = (CDM_UnitType)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
    
            Assert.AreEqual("RETRACTCASSETTE", line.UnitTypes[0]);
            Assert.AreEqual(5, line.UnitTypes.Length);
                }


        [TestMethod]
        public void Parse_CDM_UnitCurrency_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitCurrency);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_UnitValue_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitValue);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CMD_MaxDispense_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CMD_MaxDispense);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_UnitCount_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitCount);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_UnitStatus_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitStatus);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_DispenseCount_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_DispenseCount);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PresentedCount_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PresentedCount);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_UnitPUNumber_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_UnitPUNumber);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PhysicalUnitID_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalUnitID);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PhysicalPositionName_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalPositionName);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PhysicalInitialCount_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalInitialCount);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PhysicalStatus_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalStatus);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PhysicalCount_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalCount);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PhysicalRejectCount_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PhysicalRejectCount);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PCUCashInCount_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PCUCashInCount);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_DenominateInvoke_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_DenominateInvoke);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandleDemoninate_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_HandleDemoninate);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandleDenominateComplete_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_HandleDenominateComplete);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_DispenseInvoked_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_DispenseInvoked);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandleDispense_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_HandleDispense);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandleDispenseComplete_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_HandleDispenseComplete);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PresentInvoked_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_PresentInvoked);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandlePresent_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_HandlePresent);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CMD_HandlePresentComplete_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CMD_HandlePresentComplete);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandleItemsTaken_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_HandleItemsTaken);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_LastDispense_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_LastDispense);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_StStacker_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_StStacker);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_StShutter_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_StShutter);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_StTransport_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_StTransport);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_StPosition_AsExpected()
        {
            ILogFileHandler handler = new SPFlatLogHandler(new CreateTextStreamReaderMock(), ParseType.SP, SPFlatLine.Factory);
            ILogLine logLine = handler.IdentifyLine(samples_flat_cdm.CDM_StPosition);
            Assert.IsTrue(logLine is SPFlatLine);

            SPFlatLine line = (SPFlatLine)logLine;
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(line.Source), "Source should not be empty");
            }
    }
}