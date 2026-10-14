using System;
using System.Collections.Generic;
using System.Xml;

namespace Impl
{
   /// <summary>
   /// Evaluates the <Condition> elements MoniPlus2 attaches to XmlSection and XmlParam nodes.
   ///
   /// Three-valued result:
   ///    true  - condition satisfied, the value applies
   ///    false - condition not satisfied, the value does not apply
   ///    null  - cannot tell (e.g. ModelName unknown, unrecognised operator or source)
   ///
   /// Leaf:   <Condition source="DataDictionary" section="System" key="ModelName" value="8100T" operator="EQU" />
   ///         operators: EQU, NEQ, Contains, !Contains
   /// Group:  <Condition conditionsOperator="AND|OR"> ...child Conditions... </Condition>
   /// Several Condition children on the same node are ANDed.
   ///
   /// When a reasons list is passed, it receives the DataDictionary values that made the
   /// condition true (only from branches that were actually satisfied), e.g. "8300I", "Recycler",
   /// "SupportNoteCounting=True". Empty for unconditional params.
   /// </summary>
   public static class XmlConditionEvaluator
   {
      public static bool? Evaluate(XmlNode owner, XmlNamespaceManager nsmgr, Func<string, string, string> lookup)
      {
         return Evaluate(owner, nsmgr, lookup, null);
      }

      public static bool? EvaluateWithAncestors(XmlNode node, XmlNamespaceManager nsmgr, Func<string, string, string> lookup)
      {
         return EvaluateWithAncestors(node, nsmgr, lookup, null);
      }

      /// <summary>
      /// Evaluate the direct Condition children of one node.
      /// </summary>
      /// <param name="owner">XmlSection or XmlParam node</param>
      /// <param name="nsmgr">namespace manager with "ns" mapped to http://www.nh.com/Config</param>
      /// <param name="lookup">(section, key) => current value, or null if unknown</param>
      /// <param name="reasons">optional; receives the values that satisfied the condition</param>
      public static bool? Evaluate(XmlNode owner, XmlNamespaceManager nsmgr, Func<string, string, string> lookup, List<string> reasons)
      {
         bool? result = true;
         foreach (XmlNode condition in owner.SelectNodes("ns:Condition", nsmgr))
         {
            result = And(result, EvaluateCondition(condition, nsmgr, lookup, reasons));
         }
         return result;
      }

      /// <summary>
      /// Evaluate a param together with the conditions on every enclosing element (XmlSection etc.).
      /// Reasons are listed outermost first (section before param).
      /// </summary>
      public static bool? EvaluateWithAncestors(XmlNode node, XmlNamespaceManager nsmgr, Func<string, string, string> lookup, List<string> reasons)
      {
         bool? result = true;
         List<List<string>> perLevel = new List<List<string>>();

         for (XmlNode n = node; n != null && n.NodeType == XmlNodeType.Element; n = n.ParentNode)
         {
            List<string> levelReasons = reasons == null ? null : new List<string>();
            result = And(result, Evaluate(n, nsmgr, lookup, levelReasons));
            if (result == false)
               break;
            if (levelReasons != null)
               perLevel.Insert(0, levelReasons);
         }

         if (reasons != null && result == true)
         {
            foreach (List<string> level in perLevel)
               AddDistinct(reasons, level);
         }
         return result;
      }

      private static bool? EvaluateCondition(XmlNode condition, XmlNamespaceManager nsmgr, Func<string, string, string> lookup, List<string> reasons)
      {
         // group node
         string groupOperator = Attr(condition, "conditionsOperator");
         if (groupOperator != null)
         {
            bool isOr;
            if (groupOperator.Equals("OR", StringComparison.OrdinalIgnoreCase))
               isOr = true;
            else if (groupOperator.Equals("AND", StringComparison.OrdinalIgnoreCase))
               isOr = false;
            else
               return null;

            bool? acc = isOr ? (bool?)false : true;
            List<string> groupReasons = reasons == null ? null : new List<string>();

            foreach (XmlNode child in condition.SelectNodes("ns:Condition", nsmgr))
            {
               // only keep reasons from children that were actually satisfied
               List<string> childReasons = reasons == null ? null : new List<string>();
               bool? v = EvaluateCondition(child, nsmgr, lookup, childReasons);
               if (v == true && groupReasons != null)
                  AddDistinct(groupReasons, childReasons);
               acc = isOr ? Or(acc, v) : And(acc, v);
            }

            if (acc == true && reasons != null)
               AddDistinct(reasons, groupReasons);
            return acc;
         }

         // leaf node
         if (!"DataDictionary".Equals(Attr(condition, "source"), StringComparison.OrdinalIgnoreCase))
            return null;

         string section = Attr(condition, "section");
         string key = Attr(condition, "key");
         string actual = lookup(section, key);
         if (actual == null)
            return null;

         string expected = Attr(condition, "value") ?? string.Empty;
         string op = (Attr(condition, "operator") ?? string.Empty).ToUpperInvariant();

         // Operators that gate XmlParams in MoniPlus2 config. Others seen in the config
         // (GTR, LSS, ContainsItem, RegularExpressionMatch, !MOD) only gate screens/receipts
         // on runtime transaction data, so they evaluate as unknown here.
         bool? result;
         bool negative = false;
         switch (op)
         {
            case "EQU":
               result = string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
               break;
            case "NEQ":
               result = !string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
               negative = true;
               break;
            case "CONTAINS":
               result = actual.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0;
               break;
            case "!CONTAINS":
               result = actual.IndexOf(expected, StringComparison.OrdinalIgnoreCase) < 0;
               negative = true;
               break;
            default:
               return null;
         }

         if (result == true && reasons != null)
            AddDistinct(reasons, new[] { negative ? DescribeNegative(section, key, expected) : Describe(section, key, actual) });

         return result;
      }

      /// <summary>
      /// System values (ModelName, ModelVariation) are self-explanatory; anything else shows key=value.
      /// </summary>
      private static string Describe(string section, string key, string actual)
      {
         if ("System".Equals(section, StringComparison.OrdinalIgnoreCase))
            return actual;
         return key + "=" + actual;
      }

      /// <summary>
      /// A satisfied NEQ / !Contains is explained by what the value is not, e.g. "not NCR".
      /// </summary>
      private static string DescribeNegative(string section, string key, string expected)
      {
         if ("System".Equals(section, StringComparison.OrdinalIgnoreCase))
            return "not " + expected;
         return key + "!=" + expected;
      }

      private static void AddDistinct(List<string> target, IEnumerable<string> items)
      {
         foreach (string s in items)
         {
            if (!target.Exists(t => string.Equals(t, s, StringComparison.OrdinalIgnoreCase)))
               target.Add(s);
         }
      }

      public static bool? And(bool? a, bool? b)
      {
         if (a == false || b == false) return false;
         if (a == null || b == null) return null;
         return true;
      }

      public static bool? Or(bool? a, bool? b)
      {
         if (a == true || b == true) return true;
         if (a == null || b == null) return null;
         return false;
      }

      private static string Attr(XmlNode node, string name)
      {
         return node.Attributes?[name]?.Value;
      }
   }
}
