using Samples;
using Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPLogParserTests
{
   [TestClass]
   public class LogFindTests
   {
      [TestMethod]
      public void FindMarkerInLogLineFail()
      {
         // HCDUSensor log line should contain "Shutter Open = ["
         string logLine = "00 ]01874294967295016400162014940003CDM0002SP00102022/12/07001214:06 26.9950011INFORMATION0024HCDUSensor::UpdateSensor0064Sensor Data=[70 92 23 00 00 1E 1C 18 10 1C 00 04 00 00 00 \r\n";
         (_, string foundStr, string subLogLine) = LogFind.FindByMarker(logLine, "Shutter Open = [", 1);
         Assert.IsTrue(string.IsNullOrEmpty(foundStr));
         Assert.IsTrue(logLine.Length == subLogLine.Length);
      }
      [TestMethod]
      public void FindMarkerInLogLineSuccess()
      {
         // A successful search finds "0" and subLogLine begins "]...
         string logLine = "00 ]01644294967295013800162014950003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Shutter Open = [0], Lock = [1], Close = [1]01384294967295014400162014960003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0049HS005 [1], HS013 [1], HS014 [1], ITem Taken = [0]01444294967295016600162014970003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0071Stacker Empty = [0], Output Position Empty = [0], Transport Empty = [1]01664294967295012700162014980003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0032Reject Full = [0], Missing = [0]01274294967295016800162014990003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0073Carriage Home Position = [0], Out Position = [1], CDU Dock Position = [1]01684294967295014800162015000003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0053Stacker Check At Sensor = [0], TM Dip Switch ON = [0]01484294967295013800162015010003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#1 Missing = [0], Empty = [0], Low = [0]01384294967295013800162015020003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#2 Missing = [0], Empty = [0], Low = [0]01384294967295013800162015030003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#3 Missing = [0], Empty = [0], Low = [1]01384294967295013800162015040003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#4 Missing = [0], Empty = [1], Low = [1]01384294967295013800162015050003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#5 Missing = [0], Empty = [0], Low = [0]01384294967295013800162015060003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#6 Missing = [1], Empty = [1], Low = [1]01384294967295020700162015070003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0105## Total Dispense Count : TotalReqCount [12], TotalCstOutCount [12], TotalUserCount[12], TotRejCount[0]##02074294967295019500162015080003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0093## Total Reject Count : TotRejCount[0], TotRejCase[0], TotRealRejCount[0], NeedLogBackup[0]##01954294967295022300162015090003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0121## Cst#1 : CstIndex[1], ReqCount [10], CstOutCount [10], UserCount[10], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02234294967295019900162015100003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#1 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020400162015110003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0102## Cst#1 : CstOutIN [10], CstOutOUT[10], CstStackIN [10], CstStackOUT[10], CstRejIN[0], CstRejOUT[0]##02044294967295018000162015120003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0078## Cst#1 : DoubleAIN [10], DoubleAOUT [10], DoubleBIN [10], DoubleBOUT [10] ##01804294967295019600162015130003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0094## Cst#1 : DoubleSyncAIN [10], DoubleSyncAOUT [10], DoubleSyncBIN [10], DoubleSyncBOUT [10] ##01964294967295022000162015140003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#2 : CstIndex[2], ReqCount [2], CstOutCount [2], UserCount[2], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015150003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#2 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015160003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#2 : CstOutIN [2], CstOutOUT[2], CstStackIN [2], CstStackOUT[2], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015170003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#2 : DoubleAIN [2], DoubleAOUT [2], DoubleBIN [2], DoubleBOUT [2] ##01764294967295019200162015180003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#2 : DoubleSyncAIN [2], DoubleSyncAOUT [2], DoubleSyncBIN [2], DoubleSyncBOUT [2] ##01924294967295022000162015190003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#3 : CstIndex[3], ReqCount [0], CstOutCount [0], UserCount[0], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015200003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#3 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015210003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#3 : CstOutIN [0], CstOutOUT[0], CstStackIN [0], CstStackOUT[0], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015220003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#3 : DoubleAIN [0], DoubleAOUT [0], DoubleBIN [0], DoubleBOUT [0] ##01764294967295019200162015230003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#3 : DoubleSyncAIN [0], DoubleSyncAOUT [0], DoubleSyncBIN [0], DoubleSyncBOUT [0] ##01924294967295022000162015240003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#4 : CstIndex[4], ReqCount [0], CstOutCount [0], UserCount[0], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015250003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#4 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015260003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#4 : CstOutIN [0], CstOutOUT[0], CstStackIN [0], CstStackOUT[0], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015270003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#4 : DoubleAIN [0], DoubleAOUT [0], DoubleBIN [0], DoubleBOUT [0] ##01764294967295019200162015280003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#4 : DoubleSyncAIN [0], DoubleSyncAOUT [0], DoubleSyncBIN [0], DoubleSyncBOUT [0] ##01924294967295022000162015290003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#5 : CstIndex[5], ReqCount [0], CstOutCount [0], UserCount[0], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015300003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#5 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015310003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#5 : CstOutIN [0], CstOutOUT[0], CstStackIN [0], CstStackOUT[0], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015320003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#5 : DoubleAIN [0], DoubleAOUT [0], DoubleBIN [0], DoubleBOUT [0] ##01764294967295019200162015330003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#5 : DoubleSyncAIN [0], DoubleSyncAOUT [0], DoubleSyncBIN [0], DoubleSyncBOUT [0] ##01924294967295018900162015340003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0087CashUnit1 - Send Req. : 10, Cst Gate : 10, Out Require : 10, Out Pass : 10, Reject : 0.01894294967295020300162015350003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015360003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit2 - Send Req. : 2, Cst Gate : 2, Out Require : 2, Out Pass : 2, Reject : 0.01854294967295020300162015370003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015380003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit3 - Send Req. : 0, Cst Gate : 0, Out Require : 0, Out Pass : 0, Reject : 0.01854294967295020300162015390003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015400003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit4 - Send Req. : 0, Cst Gate : 0, Out Require : 0, Out Pass : 0, Reject : 0.01854294967295020300162015410003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015420003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit5 - Send Req. : 0, Cst Gate : 0, Out Require : 0, Out Pass : 0, Reject : 0.01854294967295020300162015430003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295016300162015440003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[0] = 0, m_ulUserCount[0] = 0, m_ulRejectCount[0] = 001634294967295016500162015450003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0070m_ulCstOutCount[1] = 10, m_ulUserCount[1] = 10, m_ulRejectCount[1] = 001654294967295016300162015460003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[2] = 2, m_ulUserCount[2] = 2, m_ulRejectCount[2] = 001634294967295016300162015470003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[3] = 0, m_ulUserCount[3] = 0, m_ulRejectCount[3] = 001634294967295016300162015480003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[4] = 0, m_ulUserCount[4] = 0, m_ulRejectCount[4] = 001634294967295016300162015490003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[5] = 0, m_ulUserCount[5] = 0, m_ulRejectCount[5] = 001634294967295016500162015500003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0070m_ulTotCstOutCount = 12, m_ulTotUserCount = 12, m_ulTotRejectCount = 001654294967295013000162015510003CDM0002SP00102022/12/07001214:06 27.0420006Normal0024CNHCDM4HCDUDev::Dispense0035Last Dispensed Notes is 10,2,0,0,0.01304294967295019200162015520003CDM0002SP00102022/12/07001214:06 27.0420003SNS0029CNHCdm4HCDUDev::RegWriteError0095##iCheckLog Start SENSOR_DATA: CDU10, Message: 70922300001E1C18101C000400000000 iCheckLog End##01924294967295015400162015530003CDM0002SP00102022/12/07001214:06 27.0580006Normal0028CNHCDM4HCDUDev::CDULogBackup0055Read Dispense Normal Log [Sensor_Change] LogType3 !!!!!01544294967295012900162015540003CDM0002SP00102022/12/07001214:06 27.0580006Normal0041CHCDUDevControl::ExecuteReadDispenseLog()0017Read Dispense Log01294294967295015300162015550005CDU300002SP00102022/12/07001214:06 27.0580006COMMON0033CHCDUDevControl::ExecuteLogBackup0047##Entering: [CHCDUDevControl::ExecuteLogBackup]01534294967295013300162015560003CDM0010USBDEVCOMM00102022/12/07001214:06 27.0580006NORMAL0027CUSB_HCDU::SendBulkData4Log0027[nh KCDU] => dwBulkSize[64]01334294967295013300162015570003CDM0010USBDEVCOMM00102022/12/07001214:06 27.0580006NORMAL0027CUSB_HCDU::SendBulkData4Log0027[nh KCDU] => Data.dwSize[0]01334294967295012700162015580003CDM0010USBDEVCOMM00102022/12/07001214:06 27.1200011INFORMATION0025CUSB_HCDU::WriteLogtoFile0018SaveLogData Return01274294967295015400162015590003CDM0010USBDEVCOMM00102022/12/07001214:06 27.1200006NORMAL0027CUSB_HCDU::SendBulkData4Log0048##### Bulk MI = 4D, Result = 4F, replyType = 00)01544294967295015200162015600005CDU300002SP00102022/12/07001214:06 27.1200006COMMON0033CHCDUDevControl::ExecuteLogBackup0046##Leaving: [CHCDUDevControl::ExecuteLogBackup]01524294967295013800162015610003CDM0009FRAMEWORK00102022/12/07001214:06 27.1200011INFORMATION0027CDM_SET_PRESENTSTATUS_EXTRA0028Invoke[ExtraArray.Size = 10]01384294967295013200162015620003CDM0009FRAMEWORK00102022/12/07001214:06 27.1200011INFORMATION0037CCdmService::CalculateDispensedResult0012Amount : 20.01324294967295014500162015630003CDM0009FRAMEWORK00102022/12/07001214:06 27.1200011INFORMATION0037CCdmService::CalculateDispensedResult0025UserCount : 0 10 2 0 0 0.01454294967295014700162015640003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0037CCdmService::CalculateDispensedResult0027CstOutCount : 0 10 2 0 0 0.01474294967295014600162015650003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0037CCdmService::CalculateDispensedResult0026RejectCount : 0 0 0 0 0 0.01464294967295013500162015660003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0020CCdmService::Execute0032WFS_CMD_CDM_DISPENSE{hResult[0]}01354294967295015300162015670003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0027CDM_POST_SYSE_DEVICE_STATUS0043Invoke[wDevStatus = 0, lpszPhysicalName = ]01534294967295016800162015680013CashDispenser0009FRAMEWORK00102022/12/07001214:06 27.1360007DEVRETN0021CBaseService::Execute0058HSERVICE[2] COMMAND[302] HRESULT[0] OUT BUFFER[0x00B2B78C]01684294967295015300162015690003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0031CCdmService::CheckStatusChanged0039Device Status Message Report.[Status=0]01534294967295012200162015700006COMMON0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0015CProcessor::Run0021---- Wait [6936] ----01224294967295042700162015710003CDM0003SPI00102022/12/07001214:06 27.1360009XFS_EVENT0012EXECUTE[302]0340WFS_EXECUTE_COMPLETE, \r\n";
         (_, string foundStr, string subLogLine) = LogFind.FindByMarker(logLine, "Shutter Open = [", 1);
         Assert.IsFalse(string.IsNullOrEmpty(foundStr));
         Assert.IsTrue(foundStr.Equals("0"));
         Assert.IsTrue(logLine.Length > subLogLine.Length);
         Assert.IsTrue(subLogLine.StartsWith("]"));
      }
      [TestMethod]
      public void FindMarkerInLogLineFail2()
      {
         // HCDUSensor log line should contain "Shutter Open = ["
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
                    ";
         (_, string foundStr, string subLogLine) = LogFind.FindByMarker(logLine, "{", "}");
         Assert.IsTrue(string.IsNullOrEmpty(foundStr));
         Assert.IsTrue(logLine.Length == subLogLine.Length);
      }
      [TestMethod]
      public void FindMarkerInLogLineSuccess2()
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
         (_, string foundStr, string subLogLine) = LogFind.FindByMarker(logLine, "{", "}");
         Assert.IsFalse(string.IsNullOrEmpty(foundStr));
         Assert.IsFalse(logLine.Length == subLogLine.Length);
      }
      [TestMethod]
      public void FindByBracketFailNoMatch()
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
		                lpszExtra = []";

         (bool found, string foundStr, string subLogLine) = LogFind.FindByBracket(logLine, '{', '}');
         Assert.IsFalse(found);
         Assert.IsTrue(string.IsNullOrEmpty(foundStr));
         Assert.IsTrue(logLine.Length == subLogLine.Length);
      }
      [TestMethod]
      public void FindByBracketFailWrongOrder()
      {
         string logLine =
             @"lpResult =
                }
                    hWnd = [0x0005032e],
	                RequestID = [0],
	                hService = [10],
	                tsTimestamp = [2022 / 09 / 25 19:31 04.714],
	                hResult = [0],
	                u.dwEventID = [801],
	                lpBuffer = [0x0050cf44]
                    }
                        wPortType = [16],
		                wPortIndex = [0],
		                wPortStatus = [0x0008],
		                lpszExtra = []
                    {
                {";

         (bool found, string foundStr, string subLogLine) = LogFind.FindByBracket(logLine, '{', '}');
         Assert.IsFalse(found);
         Assert.IsTrue(string.IsNullOrEmpty(foundStr));
         Assert.IsTrue(logLine.Length == subLogLine.Length);
      }
      [TestMethod]
      public void FindByBracketSuccess()
      {
         string logLine =
             @"lpResult =
                {
                    wPortType = [16],
		            wPortIndex = [0],
		            wPortStatus = [0x0008],
		            lpszExtra = []
                }
                other stuff";

         (bool found, string foundStr, string subLogLine) = LogFind.FindByBracket(logLine, '{', '}');
         Assert.IsTrue(found);
         Assert.IsFalse(string.IsNullOrEmpty(foundStr));
         Assert.IsFalse(subLogLine.Contains("{"));
         Assert.IsFalse(subLogLine.Contains("}"));
      }

      [TestMethod]
      public void FindByBracketSuccessNested()
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

         (bool found, string foundStr, string subLogLine) result = LogFind.FindByBracket(logLine, '{', '}');
         Assert.IsTrue(result.found);
         Assert.IsFalse(string.IsNullOrEmpty(result.foundStr));
         Assert.IsTrue(result.foundStr.Contains("{"));
         Assert.IsTrue(result.foundStr.Contains("}"));

         result = LogFind.FindByBracket(result.foundStr, '{', '}');
         Assert.IsTrue(result.found);
         Assert.IsFalse(string.IsNullOrEmpty(result.foundStr));
         Assert.IsFalse(result.foundStr.Contains("{"));
         Assert.IsFalse(result.foundStr.Contains("}"));


      }
      [TestMethod]
      public void FindCstsInHCDUSensorLogLine()
      {
         string logLine = "00 ]01644294967295013800162014950003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Shutter Open = [0], Lock = [1], Close = [1]01384294967295014400162014960003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0049HS005 [1], HS013 [1], HS014 [1], ITem Taken = [0]01444294967295016600162014970003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0071Stacker Empty = [0], Output Position Empty = [0], Transport Empty = [1]01664294967295012700162014980003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0032Reject Full = [0], Missing = [0]01274294967295016800162014990003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0073Carriage Home Position = [0], Out Position = [1], CDU Dock Position = [1]01684294967295014800162015000003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0053Stacker Check At Sensor = [0], TM Dip Switch ON = [0]01484294967295013800162015010003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#1 Missing = [0], Empty = [0], Low = [0]01384294967295013800162015020003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#2 Missing = [0], Empty = [0], Low = [0]01384294967295013800162015030003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#3 Missing = [0], Empty = [0], Low = [1]01384294967295013800162015040003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#4 Missing = [0], Empty = [1], Low = [1]01384294967295013800162015050003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#5 Missing = [0], Empty = [0], Low = [0]01384294967295013800162015060003CDM0002SP00102022/12/07001214:06 26.9950006Normal0024HCDUSensor::UpdateSensor0043Cst#6 Missing = [1], Empty = [1], Low = [1]01384294967295020700162015070003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0105## Total Dispense Count : TotalReqCount [12], TotalCstOutCount [12], TotalUserCount[12], TotRejCount[0]##02074294967295019500162015080003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0093## Total Reject Count : TotRejCount[0], TotRejCase[0], TotRealRejCount[0], NeedLogBackup[0]##01954294967295022300162015090003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0121## Cst#1 : CstIndex[1], ReqCount [10], CstOutCount [10], UserCount[10], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02234294967295019900162015100003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#1 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020400162015110003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0102## Cst#1 : CstOutIN [10], CstOutOUT[10], CstStackIN [10], CstStackOUT[10], CstRejIN[0], CstRejOUT[0]##02044294967295018000162015120003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0078## Cst#1 : DoubleAIN [10], DoubleAOUT [10], DoubleBIN [10], DoubleBOUT [10] ##01804294967295019600162015130003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0094## Cst#1 : DoubleSyncAIN [10], DoubleSyncAOUT [10], DoubleSyncBIN [10], DoubleSyncBOUT [10] ##01964294967295022000162015140003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#2 : CstIndex[2], ReqCount [2], CstOutCount [2], UserCount[2], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015150003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#2 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015160003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#2 : CstOutIN [2], CstOutOUT[2], CstStackIN [2], CstStackOUT[2], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015170003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#2 : DoubleAIN [2], DoubleAOUT [2], DoubleBIN [2], DoubleBOUT [2] ##01764294967295019200162015180003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#2 : DoubleSyncAIN [2], DoubleSyncAOUT [2], DoubleSyncBIN [2], DoubleSyncBOUT [2] ##01924294967295022000162015190003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#3 : CstIndex[3], ReqCount [0], CstOutCount [0], UserCount[0], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015200003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#3 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015210003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#3 : CstOutIN [0], CstOutOUT[0], CstStackIN [0], CstStackOUT[0], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015220003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#3 : DoubleAIN [0], DoubleAOUT [0], DoubleBIN [0], DoubleBOUT [0] ##01764294967295019200162015230003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#3 : DoubleSyncAIN [0], DoubleSyncAOUT [0], DoubleSyncBIN [0], DoubleSyncBOUT [0] ##01924294967295022000162015240003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#4 : CstIndex[4], ReqCount [0], CstOutCount [0], UserCount[0], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015250003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#4 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015260003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#4 : CstOutIN [0], CstOutOUT[0], CstStackIN [0], CstStackOUT[0], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015270003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#4 : DoubleAIN [0], DoubleAOUT [0], DoubleBIN [0], DoubleBOUT [0] ##01764294967295019200162015280003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#4 : DoubleSyncAIN [0], DoubleSyncAOUT [0], DoubleSyncBIN [0], DoubleSyncBOUT [0] ##01924294967295022000162015290003CDM0002SP00102022/12/07001214:06 27.0110006NORMAL0031CHCDUDevControl::DispenseResult0118## Cst#5 : CstIndex[5], ReqCount [0], CstOutCount [0], UserCount[0], TotRejCount[0], TotRejCase[0], RealRejCount[0] ##02204294967295019900162015300003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0097## Cst#5 : ShortRej[0], LongRej[0], SkewRej [0], GapRej [0], Hole[0], Double[0], UnknownRej[0] ##01994294967295020000162015310003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0098## Cst#5 : CstOutIN [0], CstOutOUT[0], CstStackIN [0], CstStackOUT[0], CstRejIN[0], CstRejOUT[0]##02004294967295017600162015320003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0074## Cst#5 : DoubleAIN [0], DoubleAOUT [0], DoubleBIN [0], DoubleBOUT [0] ##01764294967295019200162015330003CDM0002SP00102022/12/07001214:06 27.0260006NORMAL0031CHCDUDevControl::DispenseResult0090## Cst#5 : DoubleSyncAIN [0], DoubleSyncAOUT [0], DoubleSyncBIN [0], DoubleSyncBOUT [0] ##01924294967295018900162015340003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0087CashUnit1 - Send Req. : 10, Cst Gate : 10, Out Require : 10, Out Pass : 10, Reject : 0.01894294967295020300162015350003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015360003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit2 - Send Req. : 2, Cst Gate : 2, Out Require : 2, Out Pass : 2, Reject : 0.01854294967295020300162015370003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015380003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit3 - Send Req. : 0, Cst Gate : 0, Out Require : 0, Out Pass : 0, Reject : 0.01854294967295020300162015390003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015400003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit4 - Send Req. : 0, Cst Gate : 0, Out Require : 0, Out Pass : 0, Reject : 0.01854294967295020300162015410003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295018500162015420003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0083CashUnit5 - Send Req. : 0, Cst Gate : 0, Out Require : 0, Out Pass : 0, Reject : 0.01854294967295020300162015430003CDM0002SP00102022/12/07001214:06 27.0260006Normal0031CNHCDM4HCDUDev::DispensedResult0101Kind of reject - Skew : 0, Gap : 0, Long : 0, Short : 0, Double : 0, Hole : 0, Unknown : 0, Half : 0.02034294967295016300162015440003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[0] = 0, m_ulUserCount[0] = 0, m_ulRejectCount[0] = 001634294967295016500162015450003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0070m_ulCstOutCount[1] = 10, m_ulUserCount[1] = 10, m_ulRejectCount[1] = 001654294967295016300162015460003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[2] = 2, m_ulUserCount[2] = 2, m_ulRejectCount[2] = 001634294967295016300162015470003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[3] = 0, m_ulUserCount[3] = 0, m_ulRejectCount[3] = 001634294967295016300162015480003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[4] = 0, m_ulUserCount[4] = 0, m_ulRejectCount[4] = 001634294967295016300162015490003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0068m_ulCstOutCount[5] = 0, m_ulUserCount[5] = 0, m_ulRejectCount[5] = 001634294967295016500162015500003CDM0002SP00102022/12/07001214:06 27.0260006Normal0024CNHCDM4HCDUDev::Dispense0070m_ulTotCstOutCount = 12, m_ulTotUserCount = 12, m_ulTotRejectCount = 001654294967295013000162015510003CDM0002SP00102022/12/07001214:06 27.0420006Normal0024CNHCDM4HCDUDev::Dispense0035Last Dispensed Notes is 10,2,0,0,0.01304294967295019200162015520003CDM0002SP00102022/12/07001214:06 27.0420003SNS0029CNHCdm4HCDUDev::RegWriteError0095##iCheckLog Start SENSOR_DATA: CDU10, Message: 70922300001E1C18101C000400000000 iCheckLog End##01924294967295015400162015530003CDM0002SP00102022/12/07001214:06 27.0580006Normal0028CNHCDM4HCDUDev::CDULogBackup0055Read Dispense Normal Log [Sensor_Change] LogType3 !!!!!01544294967295012900162015540003CDM0002SP00102022/12/07001214:06 27.0580006Normal0041CHCDUDevControl::ExecuteReadDispenseLog()0017Read Dispense Log01294294967295015300162015550005CDU300002SP00102022/12/07001214:06 27.0580006COMMON0033CHCDUDevControl::ExecuteLogBackup0047##Entering: [CHCDUDevControl::ExecuteLogBackup]01534294967295013300162015560003CDM0010USBDEVCOMM00102022/12/07001214:06 27.0580006NORMAL0027CUSB_HCDU::SendBulkData4Log0027[nh KCDU] => dwBulkSize[64]01334294967295013300162015570003CDM0010USBDEVCOMM00102022/12/07001214:06 27.0580006NORMAL0027CUSB_HCDU::SendBulkData4Log0027[nh KCDU] => Data.dwSize[0]01334294967295012700162015580003CDM0010USBDEVCOMM00102022/12/07001214:06 27.1200011INFORMATION0025CUSB_HCDU::WriteLogtoFile0018SaveLogData Return01274294967295015400162015590003CDM0010USBDEVCOMM00102022/12/07001214:06 27.1200006NORMAL0027CUSB_HCDU::SendBulkData4Log0048##### Bulk MI = 4D, Result = 4F, replyType = 00)01544294967295015200162015600005CDU300002SP00102022/12/07001214:06 27.1200006COMMON0033CHCDUDevControl::ExecuteLogBackup0046##Leaving: [CHCDUDevControl::ExecuteLogBackup]01524294967295013800162015610003CDM0009FRAMEWORK00102022/12/07001214:06 27.1200011INFORMATION0027CDM_SET_PRESENTSTATUS_EXTRA0028Invoke[ExtraArray.Size = 10]01384294967295013200162015620003CDM0009FRAMEWORK00102022/12/07001214:06 27.1200011INFORMATION0037CCdmService::CalculateDispensedResult0012Amount : 20.01324294967295014500162015630003CDM0009FRAMEWORK00102022/12/07001214:06 27.1200011INFORMATION0037CCdmService::CalculateDispensedResult0025UserCount : 0 10 2 0 0 0.01454294967295014700162015640003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0037CCdmService::CalculateDispensedResult0027CstOutCount : 0 10 2 0 0 0.01474294967295014600162015650003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0037CCdmService::CalculateDispensedResult0026RejectCount : 0 0 0 0 0 0.01464294967295013500162015660003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0020CCdmService::Execute0032WFS_CMD_CDM_DISPENSE{hResult[0]}01354294967295015300162015670003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0027CDM_POST_SYSE_DEVICE_STATUS0043Invoke[wDevStatus = 0, lpszPhysicalName = ]01534294967295016800162015680013CashDispenser0009FRAMEWORK00102022/12/07001214:06 27.1360007DEVRETN0021CBaseService::Execute0058HSERVICE[2] COMMAND[302] HRESULT[0] OUT BUFFER[0x00B2B78C]01684294967295015300162015690003CDM0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0031CCdmService::CheckStatusChanged0039Device Status Message Report.[Status=0]01534294967295012200162015700006COMMON0009FRAMEWORK00102022/12/07001214:06 27.1360011INFORMATION0015CProcessor::Run0021---- Wait [6936] ----01224294967295042700162015710003CDM0003SPI00102022/12/07001214:06 27.1360009XFS_EVENT0012EXECUTE[302]0340WFS_EXECUTE_COMPLETE, \r\n";

         // Cst#1 Missing = [0], Empty = [0], Low = [0] 
         (bool found, string foundStr, string subLogLine) result;

         for (int i = 1; i <= 6; i++)
         {
            result = LogFind.FindByMarker(logLine, "Missing = [", 1);
            Assert.IsFalse(string.IsNullOrEmpty(result.foundStr));
            Assert.IsTrue(logLine.Length > result.subLogLine.Length);

            logLine = result.subLogLine;

            result = LogFind.FindByMarker(logLine, "Empty = [", 1);
            Assert.IsFalse(string.IsNullOrEmpty(result.foundStr));
            Assert.IsTrue(logLine.Length > result.subLogLine.Length);

            logLine = result.subLogLine;

            result = LogFind.FindByMarker(logLine, "Low = [", 1);
            Assert.IsFalse(string.IsNullOrEmpty(result.foundStr));
            Assert.IsTrue(logLine.Length > result.subLogLine.Length);
         }
      }

   }
}
