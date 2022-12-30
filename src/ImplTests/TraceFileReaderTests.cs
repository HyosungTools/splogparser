using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace ImplTests
{
    /// <summary>
    /// Test different end-of-line scenarios when reading an nwlog file. 
    /// End-of-line is a misnomer; it's really end of information-block. 
    /// So the start could be 'lpResult =' and 30 log lines later end with '}'
    /// </summary>
    [TestClass]
    public class TraceFileReaderTests
    {
        [TestMethod]
        public void DetectEOLlpResultIsWellFormed()
        {
            // Test Sample Line
            string sampleFile = "\t\tusPowerSaveRecoveryTime = [0],\r\n\t\twAntiFraudModule = [0]\r\n\t}\r\n}30064294967295016000161";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("}\r\n}"));
        }

        [TestMethod]
        public void DetectEOLlpResultIsMalformed()
        {
            // Test Sample Line
            string sampleFile = "\t\t\t\tulRejectCount = [0],\r\n\t\t\t\tulMax\r\n\t\t......(More Data)......\r\n\t}\r\n}4182429496729";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("}\r\n}"));
        }
        [TestMethod]
        public void DetectEOLLogLineIsDmp()
        {
            // Test Sample Line
            string sampleFile = "0010h: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 \r\n0020h: 00 00 00 00 00 00 00 00 00 00 00 00 00 00 11 11 \r\n";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("00 \r\n"));
        }
        [TestMethod]
        public void DetectEOLBreakInOutput()
        {
            // Test Sample Line - not necessarily the behaviour we want, but should break before the ]
            string sampleFile = "SuccessReply Sensor Data=[70 92 23 00 00 1E 1C 18 10 1C 00 04 00 00 00 \r\n00 ]018742949672950164001620149400";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("00 \r\n"));
        }
        [TestMethod]
        public void DetectEOLOutputIsTable()
        {
            // Test Sample Line - some log lines are in table form break should be at the end of every line
            string sampleFile = "usCount=3\r\nusBinNumber:                    1\t    2\t    3\t\r\nfwType:                         4\t    2\t    1\t\r\nfwItemType:                     1\t    1\t   32\t\r\nwMediaType:                     2\t    0\t    1\t\r\nszUnitID:                   PCU01\tPCU02\tCHECK\t\r\n";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("usCount=3\r\n"));
        }
        [TestMethod]
        public void DetectEOLLineisDash()
        {
            // Test Sample Line
            string sampleFile = "DIP04[1] DIP05[1] DIP06[1] DIP07[1] DIP08[1]\r\n-----------------------------------------------------------------------------------\r\nShutterOpen   [0] ShutterClose";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("DIP08[1]\r\n"));
        }
        [TestMethod]
        public void DetectEOLIsolatesAWellFormedlpResult()
        {
            // Test Sample Line
            string sampleFile = "0213WFS_EXECUTE_COMPLETE, \r\nlpResult =\r\n{\r\n\thWnd = [0x00020318],\r\n\tRequestID = [67090],\r\n\thService = [13],\r\n\ttsTimestamp = [2022/12/07 14:07 44.838],\r\n\thResult = [-14],\r\n\tu.dwCommandCode = [1610],\r\n\tlpBuffer = NULL\r\n}030142949672950";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine(); // read up to the lpResult
            logLine = reader.ReadLine();        // read the complete lpResult
            Assert.IsTrue(logLine.StartsWith("lpResult ="));
            Assert.IsTrue(logLine.EndsWith("lpBuffer = NULL\r\n}"));
        }
        [TestMethod]
        public void DetectEndOfLine8()
        {
            // Test Sample Line
            string sampleFile = "ReqID = [67648],\r\n\tlpCmdData = [0x2cdbf6f4]\r\n)02394";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("[0x2cdbf6f4]\r\n)"));
        }
        [TestMethod]
        public void DetectEndOfLine9()
        {
            // Test Sample Line
            string sampleFile = "Bit Off/On Function\r\n------------------------------------";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("Function\r\n"));
        }
        [TestMethod]
        public void DetectEndOfLine10()
        {
            // Test Sample Line
            string sampleFile = "41 3C 00 00 40 00 00 00 00 00 00 00 54 00 00 00 \r\n0010h: 00 00 00 00";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("54 00 00 00 \r\n"));
        }
        [TestMethod]
        public void DetectEndOfLine11()
        {
            // Test Sample Line
            string sampleFile = "ulValues:             4\t    2\t\r\nfwType:               1\t    1\t\r\nfwItemTyp:            0\t    0\t\r\nulMinimum:        \t\t\r\n131742949672950";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("2\t\r\n"));
        }
        [TestMethod]
        public void DetectEndOfLine12()
        {
            // Test Sample Line
            string sampleFile = "WFS_CMD_IPM_RESET command.]\r\n\t}\r\n}223442949";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("}\r\n}"));
        }
        [TestMethod]
        public void DetectEndOfLine13()
        {
            // Test Sample Line
            string sampleFile = "hWnd = [0x00020128],\r\n\tReqID = [68106],\r\n\tlpQueryDetails = NULL\r\n)024542";
            StreamReader testStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sampleFile)));

            // Test TraceFileReader can detect end of line
            TraceFileReader reader = new TraceFileReader(testStream);
            string logLine = reader.ReadLine();
            Assert.IsTrue(logLine.EndsWith("= NULL\r\n)"));
        }
    }
}
