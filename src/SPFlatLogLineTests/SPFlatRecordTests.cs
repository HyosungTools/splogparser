using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLineHandler;

namespace SPFlatLogLineTests
{
   /// <summary>
   /// Tests for the generic flat-record decoder, built from REAL Diebold-Nixdorf records pulled from
   /// Work_Bugs/NHSWS-18804-DN/.../[SP]/Nextware/BSTrace.nwlog. Each fixture is a complete
   /// device-anchored record (0003&lt;DEV&gt;0007ACTIVEX ... through the payload). These are the ground
   /// truth for the DN flat dialect; do not "tidy" the strings.
   /// </summary>
   [TestClass]
   public class SPFlatRecordTests
   {
      private static void AssertRecord(string raw, string dev, string cat, string method, string payload, string ts)
      {
         SPFlatRecord r = SPFlatRecord.Decode(raw);
         Assert.IsTrue(r.Ok, "decode should reach at least the method field");
         Assert.AreEqual(dev, r.Device, "device");
         Assert.AreEqual("ACTIVEX", r.Source, "source");
         Assert.AreEqual(cat, r.Category, "category");
         Assert.AreEqual(method, r.Method, "method");
         Assert.AreEqual(payload, r.Payload, "payload");
         Assert.AreEqual(ts, r.NormalTimestamp(), "normal timestamp");
      }

      [TestMethod]
      public void Decode_CDM_GetDeviceStatus()
      {
         AssertRecord(
            "0003CDM0007ACTIVEX00102026/07/10001210:15 01.4600008PROPERTY0021Ctrl::GetDeviceStatus0023DeviceStatus[DEVONLINE]0122429496729512710001730152",
            "CDM", "PROPERTY", "Ctrl::GetDeviceStatus", "DeviceStatus[DEVONLINE]", "2026-07-10 10:15:01.460");
      }

      [TestMethod]
      public void Decode_CDM_Denominate_Invoked()
      {
         AssertRecord(
            "0003CDM0007ACTIVEX00102026/07/07001212:21 29.2690006METHOD0016Ctrl::Denominate0052Invoked {MixAlgorithm[2], Currency[USD], Amount[40]}0144429496729501820001682171",
            "CDM", "METHOD", "Ctrl::Denominate", "Invoked {MixAlgorithm[2], Currency[USD], Amount[40]}", "2026-07-07 12:21:29.269");
      }

      [TestMethod]
      public void Decode_CDM_GetDispenserStatus()
      {
         AssertRecord(
            "0003CDM0007ACTIVEX00102026/07/10001210:15 29.2620008PROPERTY0024Ctrl::GetDispenserStatus0027DispenserStatus[NODISPENSE]0129429496729501200001730702",
            "CDM", "PROPERTY", "Ctrl::GetDispenserStatus", "DispenserStatus[NODISPENSE]", "2026-07-10 10:15:29.262");
      }

      [TestMethod]
      public void Decode_CIM_GetNumberOfLogicalUnit()
      {
         AssertRecord(
            "0003CIM0007ACTIVEX00102026/07/10001210:15 02.0200008PROPERTY0028Ctrl::GetNumberOfLogicalUnit0022NumberOfLogicalUnit[6]0128429496729513350001730249",
            "CIM", "PROPERTY", "Ctrl::GetNumberOfLogicalUnit", "NumberOfLogicalUnit[6]", "2026-07-10 10:15:02.020");
      }

      [TestMethod]
      public void Decode_CIM_AcceptCash_Invoked()
      {
         AssertRecord(
            "0003CIM0007ACTIVEX00102026/07/07001212:21 48.4420006METHOD0016Ctrl::AcceptCash0056Invoked{Insertion TimeOut[30000], Taken TimeOut[150000]}0148429496729501830001683287",
            "CIM", "METHOD", "Ctrl::AcceptCash", "Invoked{Insertion TimeOut[30000], Taken TimeOut[150000]}", "2026-07-07 12:21:48.442");
      }

      [TestMethod]
      public void Decode_CIM_HandleCashInStatus()
      {
         AssertRecord(
            "0003CIM0007ACTIVEX00102026/07/10001210:15 01.8660011INFORMATION0024Ctrl::HandleCashInStatus0016CashIn_Status[3]0121429496729501220001730230",
            "CIM", "INFORMATION", "Ctrl::HandleCashInStatus", "CashIn_Status[3]", "2026-07-10 10:15:01.866");
      }

      [TestMethod]
      public void Decode_CIM_GetCashInStatusValue()
      {
         AssertRecord(
            "0003CIM0007ACTIVEX00102026/07/07001212:21 57.4840008PROPERTY0027Ctrl::GetCashInStatus.Value0024CashInStatus[0].Value[5]0129429496729501270001683549",
            "CIM", "PROPERTY", "Ctrl::GetCashInStatus.Value", "CashInStatus[0].Value[5]", "2026-07-07 12:21:57.484");
      }

      [TestMethod]
      public void Decode_CIM_GetAcceptorStatus()
      {
         AssertRecord(
            "0003CIM0007ACTIVEX00102026/07/10001210:15 29.2720008PROPERTY0023Ctrl::GetAcceptorStatus0024AcceptorStatus[DEGRADED]0125429496729501230001730722",
            "CIM", "PROPERTY", "Ctrl::GetAcceptorStatus", "AcceptorStatus[DEGRADED]", "2026-07-10 10:15:29.272");
      }

      [TestMethod]
      public void Decode_Empty_IsNotOk()
      {
         SPFlatRecord r = SPFlatRecord.Decode("");
         Assert.IsFalse(r.Ok);
         Assert.AreEqual("?", r.Device);
      }

      // ----------------------------------------------------------------------------------------
      // Regression: the FI/Hyosung-flat samples are TIMESTAMP-framed (they start at the date; the
      // 0003<DEV>0007ACTIVEX anchor appears at the END, belonging to the next record). The decoder
      // must anchor on the record's own timestamp - NOT grab that trailing anchor - and report
      // Device="?" for this framing. Method/Category/Payload must still decode correctly so routing
      // works for both dialects. These strings are the real FI samples from Samples.samples_flat_cdm.
      // ----------------------------------------------------------------------------------------

      [TestMethod]
      public void Decode_FI_TimestampFramed_GetUnitID()
      {
         SPFlatRecord r = SPFlatRecord.Decode(
            "2025/06/06001213:43 51.8410008PROPERTY0015Ctrl::GetUnitID0043UnitID[(51060)(51060)(51063)(51066)(51171)]01364294967295012700031739520003CDM0007ACTIVEX0010");
         Assert.IsTrue(r.Ok);
         Assert.AreEqual("?", r.Device, "timestamp-framed line has no attributable device");
         Assert.AreEqual("PROPERTY", r.Category);
         Assert.AreEqual("Ctrl::GetUnitID", r.Method);
         Assert.AreEqual("UnitID[(51060)(51060)(51063)(51066)(51171)]", r.Payload);
         Assert.AreEqual("2025-06-06 13:43:51.841", r.NormalTimestamp());
      }

      [TestMethod]
      public void Decode_FI_TimestampFramed_GetUnitCurrencyID()
      {
         // NOTE: the FI method is "Ctrl::GetUnitCurrencyID", not "...Currency" - the routing must use
         // the full name.
         SPFlatRecord r = SPFlatRecord.Decode(
            "2025/06/06001213:43 51.8250008PROPERTY0023Ctrl::GetUnitCurrencyID0041UnitCurrencyID[(   )(   )(USD)(USD)(USD)]01424294967295012400031739350003CDM0007ACTIVEX0010");
         Assert.IsTrue(r.Ok);
         Assert.AreEqual("?", r.Device);
         Assert.AreEqual("Ctrl::GetUnitCurrencyID", r.Method);
         Assert.AreEqual("UnitCurrencyID[(   )(   )(USD)(USD)(USD)]", r.Payload);
      }

      [TestMethod]
      public void Decode_FI_TimestampFramed_DispenseInvoked()
      {
         SPFlatRecord r = SPFlatRecord.Decode(
            "2025/06/06001213:44 54.1890006METHOD0014Ctrl::Dispense0108Invoked {MixAlgorithm[0], Currency[USD], Amount[0], NoteCounts[(0)(0)(2)(0)(0)], Present[0], Timeout[60000]}01984294967295013000031762460003CDM0007ACTIVEX0010");
         Assert.IsTrue(r.Ok);
         Assert.AreEqual("METHOD", r.Category);
         Assert.AreEqual("Ctrl::Dispense", r.Method);
         Assert.AreEqual("2025-06-06 13:44:54.189", r.NormalTimestamp());
      }
   }
}
