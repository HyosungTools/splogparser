using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class PhysicalCIMCU
   {
      public string usNumPhysicalCU { get; set; } = "";
      public string lpPhysicalPositionName { get; set; } = "";
      public string cUnitID { get; set; } = "";
      public string ulCashInCount { get; set; } = "";
      public string ulCount { get; set; } = "";
      public string ulMaximum { get; set; } = "";
      public string usPStatus { get; set; } = "";
      public string bHardwareSensor { get; set; } = "";
      public string ulInitialCount { get; set; } = "";
      public string ulDispensedCount { get; set; } = "";
      public string ulPresentedCount { get; set; } = "";
      public string ulRetractedCount { get; set; } = "";
      public string ulRejectCount { get; set; } = "";
      public string lpszExtra { get; set; } = "";
   }

   public abstract class WFSCUINFO : SPLine
   {

      public WFSCUINFO(ILogFileHandler parent, string logLine, XFSType xfsType) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
      }

      public static (bool success, string xfsMatch, string subLogLine) Match(string logLine, string regStr, string def = "0")
      {
         Regex timeRegex = new Regex(regStr);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }

      public static string[] MatchTable(string logLine, string regStr)
      {
         List<string> values = new List<string>();
         Regex regEx = new Regex(regStr);
         Match m = regEx.Match(logLine);
         if (m.Success)
         {
            values = m.Groups[1].Value.Split('\t').ToList();
            values.RemoveAll(s => string.IsNullOrEmpty(s));

            for (int i = 0; i < values.Count; i++)
            {
               if (!values[i].Contains("   "))
               {
                  values[i] = values[i].Trim();
               }
            }
         }

         return values.ToArray();
      }

      private T[] ResizeArray<T>(T[] array, int newSize, T defaultValue = default)
      {
         if (array == null)
            return new T[newSize];

         var temp = array;
         Array.Resize(ref temp, newSize);
         for (int i = array.Length; i < newSize; i++)
         {
            temp[i] = defaultValue;
         }
         return temp;
      }

      // T A B L E   A C C E S S   F U N C T I O N S

      protected static (bool success, string xfsMatch, string subLogLine) usCountFromTable(string logLine, int lUnitCount = 1)
      {
         return Match(logLine, @"(?<=usCount=)(\d+)(?=\s*(?:\n|$))", lUnitCount.ToString());
      }


      protected static string[] usNumbersFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=usNumber\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] usTypesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=usType\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] lpszCashUnitNamesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=lpszCashUnitName\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }
      protected static string[] cUnitIDsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=cUnitID\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] cCurrencyIDsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=cCurrencyID\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulValuesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulValues\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulInitialCountsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulInitialCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulMinimumsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulMinimum\s+)([^\n]*)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulMaximumsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulMaximum\s+)([^\n]*)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] bAppLocksFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=bAppLock\s+)([^\n]*)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] usStatusesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=usStatus\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulRejectCountsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulRejectCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulDispensedCountsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulDispensedCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulPresentedCountsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulPresentedCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulRetractedCountsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulRetractedCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] fwTypesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=fwType\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] fwItemTypesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=fwItemType\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulCashInCountsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulCashInCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulCountsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] usNumPhysicalCUsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=usNumPhysicalCUs\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] lpPhysicalPositionNamesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=lpPhysicalPositionName\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] ulCashInCountFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=ulCashInCount\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] usPStatusesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=usPStatus\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] bHardwareSensorsFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=bHardwareSensor\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] lpszExtrasFromTable(string logLine)
      {
         // Updated regex: Look behind for "lpszExtra(PCU) " (accounting for the parentheses and optional whitespace)
         // Captures the rest of the line's content until newline or end.
         return MatchTable(logLine, @"(?<=lpszExtra\(...\)\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[] usCDMTypesFromTable(string logLine)
      {
         return MatchTable(logLine, @"(?<=usCDMType\s+)([^\r\n]*)(?=\s*(?:\r?\n|$))");
      }

      protected static string[,] noteNumberListFromTable(string logLine, int lUnitCount = 1)
      {
         return WFSCIMNOTENUMBERLIST.NoteNumberListFromTable(logLine, lUnitCount);
      }

      protected static string[] GetFieldFromTable(string logLine, string fieldName)
      {
         string regex = $@"(?<={Regex.Escape(fieldName)}\s+)([^\r\n])(?=\s(?:\r?\n|$))";
         return MatchTable(logLine, regex);
      }


      // L I S T    A C C E S S    F U N C T I O N S 

      // List Access Methods
      protected static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine, int lUnitCount = 1)
      {
         return Util.Match(logLine, @"(?<=usCount\s*=\s*\[)(\d+)(?=\])", lUnitCount.ToString());
      }

      protected static string[] GetLogicalUnits(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);
         logicalUnits.thisUnit = RemovePhysicalUnit(logicalUnits.thisUnit);

         for (int i = 0; i < lUnitCount; i++)
         {
            if (!logicalUnits.thisUnit.Contains("...(More Data)..."))
            {
               values.Add(logicalUnits.thisUnit);
            }
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
            logicalUnits.thisUnit = RemovePhysicalUnit(logicalUnits.thisUnit);
         }
         return values.ToArray();
      }


      protected static string[] GetPhysicalUnits(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);
         List<string> physicalUnits = ExtractPhysicalUnits(logicalUnits.thisUnit);
         foreach (string unit in physicalUnits)
         {
            values.Add(unit);
         }

         for (int i = 1; i < lUnitCount; i++)
         {
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
            physicalUnits = ExtractPhysicalUnits(logicalUnits.thisUnit);
            foreach (string unit in physicalUnits)
            {
               values.Add(unit);
            }
         }
         return values.ToArray();
      }


      // iterate over the logical units and pull out - usNumber
      protected static string[] usNumbersFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "usNumber").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usType
      protected static string[] usTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "usType").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] fwTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "fwType").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] fwItemTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(StringPropertyFromList(part, "fwItemType").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulInitialCounts
      protected static string[] ulInitialCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulInitialCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulCashInCounts
      protected static string[] ulCashInCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulCashInCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - cUnitIDs
      protected static string[] cUnitIDsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(StringPropertyFromList(part, "cUnitID").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - cCurrencyIDs
      protected static string[] cCurrencyIDsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.cCurrencyID_Preserved(part).xfsMatch/*.Trim()*/);  // preserve 3 spaces for no currency
         }
         return /*Util.TrimAll*/(Util.Resize(values.ToArray(), lUnitCount)); // preserve 3 spaces for no currency
      }

      protected static (bool success, string xfsMatch, string subLogLine) cCurrencyID_Preserved(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=cCurrencyID\s*=\s*\[)([a-zA-Z0-9 ]*)(?=\])", "");
      }

      // iterate over the logical units and pull out - ulValue
      protected static string[] ulValuesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulValues").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulCount
      protected static string[] ulCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulMaximum
      protected static string[] ulMaximumsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulMaximum").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulMinimum
      protected static string[] ulMinimumsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulMinimum").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] lpszExtrasFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.lpszExtra(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulRejectCount
      protected static string[] ulRejectCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulRejectCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulDispensedCount
      protected static string[] ulDispensedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulDispensedCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulPresentedCount
      protected static string[] ulPresentedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulPresentedCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulRetractedCount
      protected static string[] ulRetractedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulRetractedCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usStatus
      protected static string[] usStatusesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "usStatus").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] bAppLocksFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "bAppLock").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usNumPhysicalCUs
      protected static string[] usNumPhysicalCUsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "usNumPhysicalCUs").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] usCDMTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(NumericPropertyFromList(part, "usCDMType").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] lpszCashUnitNamesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(StringPropertyFromList(part, "lpszCashUnitName").xfsMatch.Trim());
            //values.Add(WFSCUINFO.lpszCashUnitName(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] lpPhysicalPositionNamesFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.lpPhysicalPositionName(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_cUnitIDsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(StringPropertyFromList(part, "cUnitID").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulInitialCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulInitialCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulCashInCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulCashInCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulRejectCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulRejectCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulMaximumsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulMaximum").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_usPStatusesFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "usPStatus").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_bHardwareSensorsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "bHardwareSensor").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_lpszExtrasFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.lpszExtra(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulDispensedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulDispensedCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulPresentedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulPresentedCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulRetractedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(NumericPropertyFromList(part, "ulRetractedCount").xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[,] noteNumberListFromList(string[] logicalParts, int lUnitCount = 1)
      {
         return WFSCIMNOTENUMBERLIST.NoteNumberListFromList(logicalParts, lUnitCount);
      }

      //// I N D I V I D U A L   L I S T   A C C E S S O R S 

      //protected static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      //{
      //   return Util.MatchList(logLine, @"(?<=cCurrencyID\s*=\s*\[)([a-zA-Z0-9 ]*)(?=\])", "");
      //}

      protected static (bool success, string xfsMatch, string subLogLine) lpPhysicalPositionName(string logLine)
      {
         string pattern = @"(?<=lpPhysicalPositionName\s*=\s*)(?:\[([^\]]*)\]|([^,\r\n]+))";
         var match = Regex.Match(logLine, pattern);
         if (match.Success)
         {
            string value = match.Groups[1].Success ? match.Groups[1].Value.Trim() : match.Groups[2].Value.Trim();
            value = value.Trim('[', ']'); // Additional safeguard
            return (true, value, logLine.Substring(match.Index));
         }
         return (false, "", logLine);
      }

      protected static (bool success, string xfsMatch, string subLogLine) lpszExtra(string logLine)
      {
         string pattern = @"(?<=lpszExtra\s*=\s*)(?:\[([^\]]*)\]|([^,\r\n]+))";
         var match = Regex.Match(logLine, pattern);
         if (match.Success)
         {
            string value = match.Groups[1].Success ? match.Groups[1].Value.Trim() : match.Groups[2].Value.Trim();
            return (true, value, logLine.Substring(match.Index));
         }
         return (false, "", logLine);
      }

      /// <summary>
      /// Parses a numeric property value from a multi-line log string in list format (e.g., "wReason = [4]").
      /// </summary>
      /// <param name="logLine">The log string to parse, which may span multiple lines.</param>
      /// <param name="propertyName">The XFS property name (e.g., "wReason", "wMediaLocation").</param>
      /// <returns>A tuple containing (success, xfsMatch, subLogLine), where success indicates if parsing was successful,
      /// xfsMatch is the extracted numeric value (as a string), and subLogLine is the remaining log string.</returns>
      public static (bool success, string xfsMatch, string subLogLine) NumericPropertyFromList(string logLine, string propertyName)
      {
         if (string.IsNullOrEmpty(logLine) || string.IsNullOrEmpty(propertyName))
         {
            return (false, "", logLine);
         }

         // Match property anywhere in the string, allowing whitespace/newlines before it
         string pattern = $@"(?<=[\n\s]*{Regex.Escape(propertyName)}\s*=\s*\[)(\d+)(?=\])";
         return Util.MatchList(logLine, pattern, "");
      }

      /// <summary>
      /// Parses a string property value from a multi-line log string in list format (e.g., "cUnitID = [LCU00]").
      /// </summary>
      /// <param name="logLine">The log string to parse, which may span multiple lines.</param>
      /// <param name="propertyName">The XFS property name (e.g., "cUnitID", "cCurrencyID").</param>
      /// <returns>A tuple containing (success, xfsMatch, subLogLine), where success indicates if parsing was successful,
      /// xfsMatch is the extracted string value, and subLogLine is the remaining log string.</returns>
      public static (bool success, string xfsMatch, string subLogLine) StringPropertyFromList(string logLine, string propertyName)
      {
         if (string.IsNullOrEmpty(logLine) || string.IsNullOrEmpty(propertyName))
         {
            return (false, "", logLine);
         }

         // Match property anywhere in the string, allowing whitespace/newlines before it
         string pattern = $@"(?<=[\n\s]*{Regex.Escape(propertyName)}\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])";
         return Util.MatchList(logLine, pattern, "");
      }

      public static string RemovePhysicalUnit(string logicalUnitString)
      {
         // Find usNumPhysicalCUs = [
         string numKey = "usNumPhysicalCUs = [";
         int numStart = logicalUnitString.IndexOf(numKey);
         if (numStart == -1)
         {
            // no physical units
            return logicalUnitString;
         }
         numStart += numKey.Length;
         int numEnd = logicalUnitString.IndexOf("]", numStart);
         if (numEnd == -1)
         {
            return logicalUnitString;
         }
         string numStr = logicalUnitString.Substring(numStart, numEnd - numStart);
         if (!int.TryParse(numStr, out int numUnits) || numUnits <= 0)
         {
            // no physical units
            return logicalUnitString;
         }

         // Find lppPhysical =
         string physKey = "lppPhysical =";
         int start = logicalUnitString.IndexOf(physKey, numEnd);
         if (start == -1)
         {
            return logicalUnitString;
         }

         int i = start + physKey.Length;

         // Process each physical unit block
         for (int unit = 0; unit < numUnits; unit++)
         {
            // Skip whitespace to find next {
            while (i < logicalUnitString.Length && char.IsWhiteSpace(logicalUnitString[i]))
            {
               i++;
            }
            if (i >= logicalUnitString.Length || logicalUnitString[i] != '{')
            {
               // Malformed, return original
               return logicalUnitString;
            }

            int braceCount = 1;
            i++; // Skip {
            while (i < logicalUnitString.Length && braceCount > 0)
            {
               if (logicalUnitString[i] == '{')
               {
                  braceCount++;
               }
               else if (logicalUnitString[i] == '}')
               {
                  braceCount--;
               }
               i++;
            }

            if (braceCount != 0)
            {
               // Unbalanced, return original
               return logicalUnitString;
            }
         }

         // i is now after the last }
         int end = i;

         // Remove the section
         return logicalUnitString.Substring(0, start) + logicalUnitString.Substring(end);
      }

      public static string RemoveNoteNumberList(string logicalUnitString)
      {
         // Find bAppLock = [
         string numKey = "bAppLock = [";
         int numStart = logicalUnitString.IndexOf(numKey);
         if (numStart == -1)
         {
            return logicalUnitString;
         }
         numStart += numKey.Length;
         int numEnd = logicalUnitString.IndexOf("]", numStart);
         if (numEnd == -1)
         {
            return logicalUnitString;
         }
         string numStr = logicalUnitString.Substring(numStart, numEnd - numStart);
         if (!int.TryParse(numStr, out int numUnits) || numUnits <= 0)
         {
            return logicalUnitString;
         }

         // Find lppPhysical =
         string physKey = "lpNoteNumberList =";
         int start = logicalUnitString.IndexOf(physKey, numEnd);
         if (start == -1)
         {
            return logicalUnitString;
         }

         int i = start + physKey.Length;

         // Process each physical unit block
         for (int unit = 0; unit < numUnits; unit++)
         {
            // Skip whitespace to find next {
            while (i < logicalUnitString.Length && char.IsWhiteSpace(logicalUnitString[i]))
            {
               i++;
            }
            if (i >= logicalUnitString.Length || logicalUnitString[i] != '{')
            {
               // Malformed, return original
               return logicalUnitString;
            }

            int braceCount = 1;
            i++; // Skip {
            while (i < logicalUnitString.Length && braceCount > 0)
            {
               if (logicalUnitString[i] == '{')
               {
                  braceCount++;
               }
               else if (logicalUnitString[i] == '}')
               {
                  braceCount--;
               }
               i++;
            }

            if (braceCount != 0)
            {
               // Unbalanced, return original
               return logicalUnitString;
            }
         }

         // i is now after the last }
         int end = i;

         // Remove the section
         return logicalUnitString.Substring(0, start) + logicalUnitString.Substring(end);
      }

      public static List<string> ExtractPhysicalUnits(string logicalUnitString)
      {
         List<string> physicalUnits = new List<string>();

         // Find usNumPhysicalCUs = [
         string numKey = "usNumPhysicalCUs = [";
         int numStart = logicalUnitString.IndexOf(numKey);
         if (numStart == -1)
         {
            return physicalUnits; // Empty list if not found
         }
         numStart += numKey.Length;
         int numEnd = logicalUnitString.IndexOf("]", numStart);
         if (numEnd == -1)
         {
            return physicalUnits;
         }
         string numStr = logicalUnitString.Substring(numStart, numEnd - numStart);
         if (!int.TryParse(numStr, out int numUnits) || numUnits <= 0)
         {
            return physicalUnits;
         }

         // Find lppPhysical =
         string physKey = "lppPhysical =";
         int start = logicalUnitString.IndexOf(physKey, numEnd);
         if (start == -1)
         {
            return physicalUnits;
         }

         int i = start + physKey.Length;

         // Process each physical unit block
         for (int unit = 0; unit < numUnits; unit++)
         {
            // Skip whitespace to find next {
            while (i < logicalUnitString.Length && char.IsWhiteSpace(logicalUnitString[i]))
            {
               i++;
            }
            if (i >= logicalUnitString.Length || logicalUnitString[i] != '{')
            {
               // Malformed, stop and return what we have
               break;
            }

            int blockStart = i;
            int braceCount = 1;
            i++; // Skip {
            while (i < logicalUnitString.Length && braceCount > 0)
            {
               if (logicalUnitString[i] == '{')
               {
                  braceCount++;
               }
               else if (logicalUnitString[i] == '}')
               {
                  braceCount--;
               }
               i++;
            }

            if (braceCount != 0)
            {
               // Unbalanced, skip this one
               continue;
            }

            // Extract the block from blockStart to i-1 (including braces)
            string block = logicalUnitString.Substring(blockStart, i - blockStart);
            physicalUnits.Add(block);
         }

         return physicalUnits;
      }

      public static List<string> ExtractNoteNumberList(string logicalUnitString)
      {
         List<string> noteNumberList = new List<string>();

         // Find usNumOfNoteNumbers = [
         string numKey = "usNumOfNoteNumbers = [";
         int numStart = logicalUnitString.IndexOf(numKey);
         if (numStart == -1)
         {
            return noteNumberList; // Empty list if not found
         }
         numStart += numKey.Length;
         int numEnd = logicalUnitString.IndexOf("]", numStart);
         if (numEnd == -1)
         {
            return noteNumberList;
         }
         string numStr = logicalUnitString.Substring(numStart, numEnd - numStart);
         if (!int.TryParse(numStr, out int numUnits) || numUnits <= 0)
         {
            return noteNumberList;
         }

         // Find lppPhysical =
         string physKey = "lppNoteNumber =";
         int start = logicalUnitString.IndexOf(physKey, numEnd);
         if (start == -1)
         {
            return noteNumberList;
         }

         int i = start + physKey.Length;

         // Process each physical unit block
         for (int unit = 0; unit < numUnits; unit++)
         {
            // Skip whitespace to find next {
            while (i < logicalUnitString.Length && char.IsWhiteSpace(logicalUnitString[i]))
            {
               i++;
            }
            if (i >= logicalUnitString.Length || logicalUnitString[i] != '{')
            {
               // Malformed, stop and return what we have
               break;
            }

            int blockStart = i;
            int braceCount = 1;
            i++; // Skip {
            while (i < logicalUnitString.Length && braceCount > 0)
            {
               if (logicalUnitString[i] == '{')
               {
                  braceCount++;
               }
               else if (logicalUnitString[i] == '}')
               {
                  braceCount--;
               }
               i++;
            }

            if (braceCount != 0)
            {
               // Unbalanced, skip this one
               continue;
            }

            // Extract the block from blockStart to i-1 (including braces)
            string block = logicalUnitString.Substring(blockStart, i - blockStart);
            noteNumberList.Add(block);
         }

         return noteNumberList;
      }
   }
}
