using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Contract;
using RegEx;

namespace LogLineHandler
{
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

         Console.WriteLine($"MatchTable - size: {values.Count} elements : {string.Join("|", values)}");
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
         Console.WriteLine($"ResizeArray - size: {temp.Length} elements : {string.Join("|", temp)}");
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
         logicalUnits.thisUnit = Util.RemovePhysicalUnit(logicalUnits.thisUnit);

         for (int i = 0; i < lUnitCount; i++)
         {
            if (!logicalUnits.thisUnit.Contains("...(More Data)..."))
            {
               values.Add(logicalUnits.thisUnit);
            }
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
            logicalUnits.thisUnit = Util.RemovePhysicalUnit(logicalUnits.thisUnit);
         }
         return values.ToArray();
      }


      protected static string[] GetPhysicalUnits(string logLine, int lUnitCount = 1)
      {
         List<string> values = new List<string>();
         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);
         List<string> physicalUnits = Util.ExtractPhysicalUnits(logicalUnits.thisUnit);
         foreach (string unit in physicalUnits)
         {
            values.Add(unit);
         }

         for (int i = 1; i < lUnitCount; i++)
         {
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
            physicalUnits = Util.ExtractPhysicalUnits(logicalUnits.thisUnit);
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
            values.Add(WFSCUINFO.usNumber(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usType
      protected static string[] usTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.usType(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] fwTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.fwType(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] fwItemTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.fwItemType(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulInitialCounts
      protected static string[] ulInitialCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulInitialCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulCashInCounts
      protected static string[] ulCashInCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulCashInCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - cUnitIDs
      protected static string[] cUnitIDsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.cUnitID(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - cCurrencyIDs
      protected static string[] cCurrencyIDsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.cCurrencyID(part).xfsMatch/*.Trim()*/);  // preserve 3 spaces for no currency
         }
         return /*Util.TrimAll*/(Util.Resize(values.ToArray(), lUnitCount)); // preserve 3 spaces for no currency
      }

      // iterate over the logical units and pull out - ulValue
      protected static string[] ulValuesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulValue(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulCount
      protected static string[] ulCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulMaximum
      protected static string[] ulMaximumsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulMaximum(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulMinimum
      protected static string[] ulMinimumsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulMinimum(part).xfsMatch.Trim());
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
            values.Add(WFSCUINFO.ulRejectCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulDispensedCount
      protected static string[] ulDispensedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulDispensedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulPresentedCount
      protected static string[] ulPresentedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulPresentedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - ulRetractedCount
      protected static string[] ulRetractedCountsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.ulRetractedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usStatus
      protected static string[] usStatusesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.usStatus(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] bAppLocksFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.bAppLock(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      // iterate over the logical units and pull out - usNumPhysicalCUs
      protected static string[] usNumPhysicalCUsFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.usNumPhysicalCUs(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] usCDMTypesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.usCDMType(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] lpszCashUnitNamesFromList(string[] logicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in logicalParts)
         {
            values.Add(WFSCUINFO.lpszCashUnitName(part).xfsMatch.Trim());
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
            values.Add(WFSCUINFO.cUnitID(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulInitialCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.ulInitialCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulCashInCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.ulCashInCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.ulCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulRejectCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.ulRejectCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulMaximumsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.ulMaximum(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_usPStatusesFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.usPStatus(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_bHardwareSensorsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.bHardwareSensor(part).xfsMatch.Trim());
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
            values.Add(WFSCUINFO.ulDispensedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulPresentedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.ulPresentedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[] p_ulRetractedCountsFromList(string[] physicalParts, int lUnitCount)
      {
         List<string> values = new List<string>();
         foreach (string part in physicalParts)
         {
            values.Add(WFSCUINFO.ulRetractedCount(part).xfsMatch.Trim());
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      }

      protected static string[,] noteNumberListFromList(string[] logicalParts, int lUnitCount = 1)
      {
         return WFSCIMNOTENUMBERLIST.NoteNumberListFromList(logicalParts, lUnitCount);
      }

      // I N D I V I D U A L   L I S T   A C C E S S O R S 

      // Individual Accessors
      protected static (bool success, string xfsMatch, string subLogLine) usNumber(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usNumber\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) cUnitID(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=cUnitID\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) cCurrencyID(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=cCurrencyID\s*=\s*\[)([a-zA-Z0-9 ]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulValue(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulValues\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulMaximum(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulMaximum\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usStatus(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usStatus\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bAppLock(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=bAppLock\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usType(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usType\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwType(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=fwType\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) fwItemType(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=fwItemType\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulInitialCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulInitialCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulMinimum(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulMinimum\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulRejectCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulRejectCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulDispensedCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulDispensedCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulPresentedCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulPresentedCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) ulRetractedCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulRetractedCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usNumPhysicalCUs(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usNumPhysicalCUs\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) usCDMType(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usCDMType\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) lpszCashUnitName(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=lpszCashUnitName\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) lppPhysicalBlock(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=lppPhysical\s*=\s*\{)(.*?(?=}\s*(?:ulDispensedCount|}|{)))", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) physical_cUnitID(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=cUnitID\s*=\s*\[)([a-zA-Z0-9_]*)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) physical_usPStatus(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usPStatus\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) physical_bHardwareSensor(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=bHardwareSensor\s*=\s*\[)(\d+)(?=\])", "");
      }

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

      protected static (bool success, string xfsMatch, string subLogLine) usPStatus(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=usPStatus\s*=\s*\[)(\d+)(?=\])", "");
      }

      protected static (bool success, string xfsMatch, string subLogLine) bHardwareSensor(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=bHardwareSensor\s*=\s*\[)(\d+)(?=\])", "");
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

      // ulCashInCount
      protected static (bool success, string xfsMatch, string subLogLine) ulCashInCount(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ulCashInCount\s*=\s*\[)(\d+)(?=\])", "");
      }

      // lppPhysical  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) lppPhysical(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=lppPhysical = {.*})");
      }


      //// wReason  - we dont need to search for this in the table log line, only in the list log line
      //protected static (bool success, string xfsMatch, string subLogLine) wReasonFromList(string logLine)
      //{
      //   return Util.MatchList(logLine, @"(?<=wReason\s*=\s*\[)(\d+)(?=\])", "");
      //}

      //// wMediaLocation
      //protected static (bool success, string xfsMatch, string subLogLine) wMediaLocationFromList(string logLine)
      //{
      //   return Util.MatchList(logLine, @"(?<=wMediaLocation\s*=\s*\[)(\d+)(?=\])", "");
      //}

      //// bPresentRequired
      //protected static (bool success, string xfsMatch, string subLogLine) bPresentRequiredFromList(string logLine)
      //{
      //   return Util.MatchList(logLine, @"(?<=bPresentRequired\s*=\s*\[)(\d+)(?=\])", "");
      //}

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
         string pattern = $@"(?<=[\n\s]*{Regex.Escape(propertyName)}\s*=\s*\[)([^\]]*])(?=\])";
         return Util.MatchList(logLine, pattern, "");
      }

      //// L I S T    A C C E S S    F U N C T I O N S 

      //protected static string[] usNumbersFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(usNumber(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));

      //}

      //protected static string[] fwTypesFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(fwType(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulCashInCountsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulCashInCount(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] cUnitIDsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(cUnitID(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount, ""));
      //}

      //protected static string[] cCurrencyIDsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(cCurrencyID(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount, ""));
      //}

      //protected static string[] ulValuesFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulValue(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulCountsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulMaximumsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulMaximum(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] usStatusesFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(usStatus(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[,] noteNumberListFromList(string logLine, int lUnitCount = 1)
      //{
      //   return WFSCIMNOTENUMBERLIST.NoteNumberListFromList(logLine, lUnitCount);
      //}

      //protected static string[] ulInitialCountsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulInitialCount(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulDispensedCountsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulDispensedCount(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulPresentedCountsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulPresentedCount(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulRetractedCountsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulRetractedCount(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulRejectCountsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulRejectCount(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}

      //protected static string[] ulMinimumsFromList(string logLine, int lUnitCount = 1)
      //{
      //   List<string> values = new List<string>();
      //   (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

      //   for (int i = 0; i < lUnitCount; i++)
      //   {
      //      values.Add(ulMinimum(logicalUnits.thisUnit).xfsMatch.Trim());
      //      logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
      //   }
      //   return Util.TrimAll(Util.Resize(values.ToArray(), lUnitCount));
      //}
   }
}
