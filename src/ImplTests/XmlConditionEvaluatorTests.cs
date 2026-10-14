using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Impl;

namespace ImplTests
{
   [TestClass]
   public class XmlConditionEvaluatorTests
   {
      // Cut down from Device.xml (NHSWS-18440)
      private const string DeviceXml = @"<XmlConfiguration xmlns=""http://www.nh.com/Config"">
  <XmlSection Name=""MultiVendor"">
    <XmlParameters>
      <XmlParam Key=""ATMVendor"" Value=""NCR"">
        <Condition source=""DataDictionary"" section=""System"" key=""ModelName"" value=""NCR"" operator=""Contains"" />
      </XmlParam>
      <XmlParam Key=""ATMVendor"" Value=""DIEBOLD"">
        <Condition source=""DataDictionary"" section=""System"" key=""ModelName"" value=""DN"" operator=""Contains"" />
      </XmlParam>
      <XmlParam Key=""SupportNoteCounting"" Value=""False"" />
    </XmlParameters>
  </XmlSection>
  <XmlSection Name=""CashDispenser"">
    <XmlParameters><XmlParam Key=""DeviceType"" Value=""BRM"" /></XmlParameters>
    <Condition source=""DataDictionary"" section=""System"" key=""ModelName"" value=""8100T"" operator=""EQU"" />
  </XmlSection>
  <XmlSection Name=""CashDispenser"">
    <XmlParameters><XmlParam Key=""DeviceType"" Value=""CDM"" /></XmlParameters>
    <Condition conditionsOperator=""AND"">
      <Condition conditionsOperator=""OR"">
        <Condition source=""DataDictionary"" section=""System"" key=""ModelName"" value=""8300D"" operator=""EQU"" />
        <Condition source=""DataDictionary"" section=""System"" key=""ModelName"" value=""8300T"" operator=""EQU"" />
      </Condition>
      <Condition source=""DataDictionary"" section=""System"" key=""ModelVariation"" value=""Dispenser"" operator=""EQU"" />
    </Condition>
  </XmlSection>
  <XmlSection Name=""CashDispenser"">
    <XmlParameters><XmlParam Key=""AddCashWithLCU"" Value=""True"" /></XmlParameters>
    <Condition conditionsOperator=""AND"">
      <Condition source=""DataDictionary"" section=""System"" key=""ModelName"" value=""NCR"" operator=""Contains"" />
      <Condition source=""DataDictionary"" section=""MultiVendor"" key=""SupportNoteCounting"" value=""True"" operator=""EQU"" />
    </Condition>
  </XmlSection>
</XmlConfiguration>";

      /// <summary>Returns the values that apply, last satisfied entry wins.</summary>
      private static Dictionary<string, string> Resolve(string model, string variation, string supportNoteCounting = "False")
      {
         XmlDocument doc = new XmlDocument();
         doc.LoadXml(DeviceXml);
         XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
         nsmgr.AddNamespace("ns", "http://www.nh.com/Config");

         Func<string, string, string> lookup = (section, key) =>
         {
            if (section == "System" && key == "ModelName") return model;
            if (section == "System" && key == "ModelVariation") return variation;
            if (section == "MultiVendor" && key == "SupportNoteCounting") return supportNoteCounting;
            return null;
         };

         Dictionary<string, string> result = new Dictionary<string, string>();
         foreach (XmlNode p in doc.SelectNodes("//ns:XmlParam", nsmgr))
         {
            if (XmlConditionEvaluator.EvaluateWithAncestors(p, nsmgr, lookup) == true)
            {
               string section = p.SelectSingleNode("ancestor::ns:XmlSection", nsmgr).Attributes["Name"].Value;
               result[section + "." + p.Attributes["Key"].Value] = p.Attributes["Value"].Value;
            }
         }
         return result;
      }

      [TestMethod]
      public void Model8100T_GetsBRM_AndNoVendor()
      {
         Dictionary<string, string> r = Resolve("8100T", null);
         Assert.AreEqual("BRM", r["CashDispenser.DeviceType"]);
         Assert.IsFalse(r.ContainsKey("MultiVendor.ATMVendor"));   // was DIEBOLD before the fix
      }

      [TestMethod]
      public void Model8300T_Dispenser_GetsCDM()
      {
         Dictionary<string, string> r = Resolve("8300T", "Dispenser");
         Assert.AreEqual("CDM", r["CashDispenser.DeviceType"]);
      }

      [TestMethod]
      public void Model8300T_Recycler_DoesNotGetDispenserValue()
      {
         Dictionary<string, string> r = Resolve("8300T", "Recycler");
         Assert.IsFalse(r.ContainsKey("CashDispenser.DeviceType"));
      }

      [TestMethod]
      public void NcrModel_GetsNcrVendor_ContainsOperator()
      {
         Dictionary<string, string> r = Resolve("NCR6632", null);
         Assert.AreEqual("NCR", r["MultiVendor.ATMVendor"]);
      }

      [TestMethod]
      public void ConditionOnConfigValue_SupportNoteCounting()
      {
         Assert.IsFalse(Resolve("NCR6632", null, "False").ContainsKey("CashDispenser.AddCashWithLCU"));
         Assert.AreEqual("True", Resolve("NCR6632", null, "True")["CashDispenser.AddCashWithLCU"]);
      }

      private static List<string> Reasons(string paramValue, string model, string variation, string supportNoteCounting = "False")
      {
         XmlDocument doc = new XmlDocument();
         doc.LoadXml(DeviceXml);
         XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
         nsmgr.AddNamespace("ns", "http://www.nh.com/Config");
         XmlNode p = doc.SelectSingleNode("//ns:XmlParam[@Value='" + paramValue + "']", nsmgr);

         List<string> reasons = new List<string>();
         XmlConditionEvaluator.EvaluateWithAncestors(p, nsmgr, (section, key) =>
         {
            if (section == "System" && key == "ModelName") return model;
            if (section == "System" && key == "ModelVariation") return variation;
            if (section == "MultiVendor" && key == "SupportNoteCounting") return supportNoteCounting;
            return null;
         }, reasons);
         return reasons;
      }

      [TestMethod]
      public void Reasons_OnlyFromSatisfiedBranch()
      {
         // OR(8300D, 8300T) AND Dispenser - only 8300T matched, so 8300D must not appear
         CollectionAssert.AreEqual(new[] { "8300T", "Dispenser" }, Reasons("CDM", "8300T", "Dispenser"));
      }

      [TestMethod]
      public void Reasons_ContainsShowsActualModel()
      {
         CollectionAssert.AreEqual(new[] { "NCR6632" }, Reasons("NCR", "NCR6632", null));
      }

      [TestMethod]
      public void Reasons_ConfigValueShowsKeyEqualsValue()
      {
         CollectionAssert.AreEqual(new[] { "NCR6632", "SupportNoteCounting=True" }, Reasons("True", "NCR6632", null, "True"));
      }

      [TestMethod]
      public void Reasons_EmptyWhenConditionFails()
      {
         Assert.AreEqual(0, Reasons("CDM", "8300T", "Recycler").Count);
      }

      [TestMethod]
      public void Reasons_EmptyForUnconditionalParam()
      {
         Assert.AreEqual(0, Reasons("False", "8300T", "Dispenser").Count);
      }

      [TestMethod]
      public void NegativeOperators()
      {
         const string xml = @"<XmlConfiguration xmlns=""http://www.nh.com/Config"">
  <XmlSection Name=""A""><XmlParameters><XmlParam Key=""NotNcr"" Value=""1"" /></XmlParameters>
    <Condition source=""DataDictionary"" section=""System"" key=""ModelName"" value=""NCR"" operator=""!Contains"" /></XmlSection>
  <XmlSection Name=""B""><XmlParameters><XmlParam Key=""NotRecycler"" Value=""2"" /></XmlParameters>
    <Condition source=""DataDictionary"" section=""System"" key=""ModelVariation"" value=""Recycler"" operator=""NEQ"" /></XmlSection>
</XmlConfiguration>";
         XmlDocument doc = new XmlDocument();
         doc.LoadXml(xml);
         XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
         nsmgr.AddNamespace("ns", "http://www.nh.com/Config");
         XmlNode notNcr = doc.SelectSingleNode("//ns:XmlParam[@Key='NotNcr']", nsmgr);
         XmlNode notRecycler = doc.SelectSingleNode("//ns:XmlParam[@Key='NotRecycler']", nsmgr);

         Func<string, string, string> h8300t = (s, k) => k == "ModelName" ? "8300T" : "Recycler";
         Func<string, string, string> ncr = (s, k) => k == "ModelName" ? "NCR6632" : "Dispenser";

         List<string> reasons = new List<string>();
         Assert.AreEqual(true, XmlConditionEvaluator.EvaluateWithAncestors(notNcr, nsmgr, h8300t, reasons));
         CollectionAssert.AreEqual(new[] { "not NCR" }, reasons);
         Assert.AreEqual(false, XmlConditionEvaluator.EvaluateWithAncestors(notNcr, nsmgr, ncr));

         Assert.AreEqual(false, XmlConditionEvaluator.EvaluateWithAncestors(notRecycler, nsmgr, h8300t));
         Assert.AreEqual(true, XmlConditionEvaluator.EvaluateWithAncestors(notRecycler, nsmgr, ncr));
      }

      [TestMethod]
      public void UnknownModel_IsUnresolved_NotFalse()
      {
         XmlDocument doc = new XmlDocument();
         doc.LoadXml(DeviceXml);
         XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
         nsmgr.AddNamespace("ns", "http://www.nh.com/Config");
         XmlNode brm = doc.SelectSingleNode("//ns:XmlParam[@Value='BRM']", nsmgr);

         Assert.IsNull(XmlConditionEvaluator.EvaluateWithAncestors(brm, nsmgr, (s, k) => null));
         // Unconditional params still apply when the model is unknown
         XmlNode snc = doc.SelectSingleNode("//ns:XmlParam[@Key='SupportNoteCounting']", nsmgr);
         Assert.AreEqual(true, XmlConditionEvaluator.EvaluateWithAncestors(snc, nsmgr, (s, k) => null));
      }
   }
}
