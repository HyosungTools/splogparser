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
        public void Parse_CDM_UnitID_Line()
        {
            string line = samples_flat_cdm.CDM_UnitID;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
            Assert.AreEqual("51060", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_UnitType_Line()
        {
            string line = samples_flat_cdm.CDM_UnitType;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
            Assert.AreEqual("RETRACTCASSETTE", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_UnitCurrency_Line()
        {
            string line = samples_flat_cdm.CDM_UnitCurrency;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
        }


        [TestMethod]
        public void Parse_CDM_UnitValue_Line()
        {
            string line = samples_flat_cdm.CDM_UnitValue;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CMD_MaxDispense_Line()
        {
            string line = samples_flat_cdm.CMD_MaxDispense;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_UnitCount_Line()
        {
            string line = samples_flat_cdm.CDM_UnitCount;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_UnitStatus_Line()
        {
            string line = samples_flat_cdm.CDM_UnitStatus;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
            Assert.AreEqual("OK", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_DispenseCount_Line()
        {
            string line = samples_flat_cdm.CDM_DispenseCount;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(2, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PresentedCount_Line()
        {
            string line = samples_flat_cdm.CDM_PresentedCount;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(1, parsed.Values.Length);
            Assert.AreEqual("109", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_UnitPUNumber_Line()
        {
            string line = samples_flat_cdm.CDM_UnitPUNumber;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(6, parsed.Values.Length);
            Assert.AreEqual("1", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PhysicalUnitID_Line()
        {
            string line = samples_flat_cdm.CDM_PhysicalUnitID;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(6, parsed.Values.Length);
            Assert.AreEqual("51060", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PhysicalPositionName_Line()
        {
            string line = samples_flat_cdm.CDM_PhysicalPositionName;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(6, parsed.Values.Length);
            Assert.AreEqual("COMPARTMENT1.RET", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PhysicalInitialCount_Line()
        {
            string line = samples_flat_cdm.CDM_PhysicalInitialCount;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(6, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PhysicalStatus_Line()
        {
            string line = samples_flat_cdm.CDM_PhysicalStatus;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(6, parsed.Values.Length);
            Assert.AreEqual("OK", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PhysicalCount_Line()
        {
            string line = samples_flat_cdm.CDM_PhysicalCount;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(6, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PhysicalRejectCount_Line()
        {
            string line = samples_flat_cdm.CDM_PhysicalRejectCount;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(6, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_PCUCashInCount_Line()
        {
            string line = samples_flat_cdm.CDM_PCUCashInCount;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(1, parsed.Values.Length);
            Assert.AreEqual("2", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_DenominateInvoke_Line()
        {
            string line = samples_flat_cdm.CDM_DenominateInvoke;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandleDemoninate_Line()
        {
            string line = samples_flat_cdm.CDM_HandleDemoninate;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual("0", parsed.GetNamedStringValue("hResult"));
        }


        [TestMethod]
        public void Parse_CDM_HandleDenominateComplete_Line()
        {
            string line = samples_flat_cdm.CDM_HandleDenominateComplete;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_DispenseInvoked_Line()
        {
            string line = samples_flat_cdm.CDM_DispenseInvoked;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_HandleDispense_Line()
        {
            string line = samples_flat_cdm.CDM_HandleDispense;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual("0", parsed.GetNamedStringValue("hResult"));
        }


        [TestMethod]
        public void Parse_CDM_HandleDispenseComplete_Line()
        {
            string line = samples_flat_cdm.CDM_HandleDispenseComplete;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_PresentInvoked_Line()
        {
            string line = samples_flat_cdm.CDM_PresentInvoked;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandlePresent_Line()
        {
            string line = samples_flat_cdm.CDM_HandlePresent;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual("0", parsed.GetNamedStringValue("hResult"));
        }


        [TestMethod]
        public void Parse_CMD_HandlePresentComplete_Line()
        {
            string line = samples_flat_cdm.CMD_HandlePresentComplete;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_HandleItemsTaken_Line()
        {
            string line = samples_flat_cdm.CDM_HandleItemsTaken;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_LastDispense_Line()
        {
            string line = samples_flat_cdm.CDM_LastDispense;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
                Assert.AreEqual(5, parsed.Values.Length);
            Assert.AreEqual("0", parsed.GetStringValue(0));
        }


        [TestMethod]
        public void Parse_CDM_StStacker_Line()
        {
            string line = samples_flat_cdm.CDM_StStacker;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_StShutter_Line()
        {
            string line = samples_flat_cdm.CDM_StShutter;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_StTransport_Line()
        {
            string line = samples_flat_cdm.CDM_StTransport;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }


        [TestMethod]
        public void Parse_CDM_StPosition_Line()
        {
            string line = samples_flat_cdm.CDM_StPosition;
            var parsed = SPFlatLine.Factory(null, line);
            Assert.IsNotNull(parsed, "Parsed line should not be null");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Timestamp), "Timestamp should not be empty");
            Assert.IsFalse(string.IsNullOrWhiteSpace(parsed.Source), "Source should not be empty");
            }
    }
}