using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum SPFlatType
   {
      CDM_UnitIDs,
      CDM_UnitTypes,
      CDM_UnitValues,
      CDM_UnitCurrencies,
      CDM_UnitInitialCounts,
      CDM_UnitCounts,
      CDM_UnitStatuses,

      CDM_PhysicalUnitNumbers,
      CDM_PhysicalIDs,
      CDM_PhysicalPositionNames,
      CDM_PhysicalInitialCounts,

      CDM_PhysicalStatuses,
      CDM_PhysicalCounts,
      CDM_PhysicalRejectCounts,

      CDM_DenominateInvoked,
      CDM_HandleDenominate,

      CDM_DispenseInvoked,
      CDM_HandleDispense,

      CDM_PresentInvoked,
      CDM_HandlePresent,

      CDM_HandleItemsTaken,

      CIM_StartCashIn,
      CIM_AcceptCash,
      CIM_OpenShutter,
      CIM_CloseShutter,

      /* CIM LOgical Unit */
      CIM_LogicalUnit,
      CIM_LogicalUnit_InitialCount,
      CIM_LogicalUnit_RejectCount,
      CIM_LogicalUnit_RetractedCount,
      CIM_LogicalUnit_DispensedCount,
      CIM_LogicalUnit_PresentedCount,
      CIM_LogicalUnit_TotalCount,
      CIM_LogicalUnit_MaximumCount,
      CIM_LogicalUnit_CashInCount,

      CIM_LogicalUnit_Type,
      CIM_LogicalUnit_Status,
      CIM_LogicalUnit_Number,
      CIM_LogicalUnit_UnitID,
      CIM_LogicalUnit_CurrencyID,
      CIM_LogicalUnit_NumberOfItems,
      CIM_LogicalUnit_NumberOfPCU,


      flat_none,

      /* ERROR */
      Error
   }
   public class SPFlatLine : LogLine, ILogLine
   {
      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      public SPFlatType flatType { get; set; }

      public string Command { get; protected set; }

      public SPFlatLine(ILogFileHandler parent, string logLine, SPFlatType xfsType) : base(parent, logLine)
      {
         this.flatType = xfsType;
         Initialize();
      }
      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      protected override string tsTimestamp()
      {
         // the string from the log file, but return is in normal form
         // (replace '/' with '-' and the 2nd space with a ':')
         string logTime = "2025-01-01 00:00:00.000";

         string pattern = @"(\d{4})/(\d{2})/(\d{2})\d{4}(\d{2}):(\d{2}) (\d{2})\.(\d{3})";
         Match match = Regex.Match(logLine, pattern);

         if (match.Success)
         {
            string year = match.Groups[1].Value;
            string month = match.Groups[2].Value;
            string day = match.Groups[3].Value;
            string hour = match.Groups[4].Value;
            string minute = match.Groups[5].Value;
            string second = match.Groups[6].Value;
            string milli = match.Groups[7].Value;

            logTime = $"{year}-{month}-{day} {hour}:{minute}:{second}.{milli}";
         }
         return logTime;
      }

      protected override string hResult()
      {
         string hResult = "0";

         // Example: hResult = [0],
         Regex timeRegex = new Regex("hResult = \\[(.*)\\]");
         Match mtch = timeRegex.Match(logLine);
         if (mtch.Success)
         {
            hResult = mtch.Groups[1].Value.Trim();
         }

         return hResult == "0" ? "" : hResult;
      }
      public static ILogLine Factory(ILogFileHandler handler, string logLine)
      {
         // Specialized route for CDM Dispense lines
         #region CDM

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::Denominate") && logLine.Contains("Invoked"))
            return new CDMDenominateLine(handler, logLine);

         //if (logLine.Contains("INFORMATION") && logLine.Contains("HandleDenominate") && logLine.Contains("Execute-Result"))
         //   return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandleDenominate);

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::Dispense") && logLine.Contains("Invoked"))
            return new CDMDispenseLine(handler, logLine);

         //if (logLine.Contains("INFORMATION") && logLine.Contains("HandleDispense") && logLine.Contains("Execute-Result"))
         //   return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandleDispense);

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::Present") && logLine.Contains("Invoked"))
            return new SPFlatLine(handler, logLine, SPFlatType.CDM_PresentInvoked);

         //if (logLine.Contains("INFORMATION") && logLine.Contains("HandlePresent") && logLine.Contains("Execute-Result"))
         //   return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandlePresent);

         if (logLine.Contains("EVENT") && logLine.Contains("CCdmService::HandleItemsTaken") && logLine.Contains("FireItemsTaken"))
            return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandleItemsTaken);

         //if (logLine.Contains("EVENT") || logLine.Contains("METHOD") || logLine.Contains("INFORMATION"))
         //   handler.ctx.LogWriteLine(logLine);


         //if (logLine.Contains("CIM0007ACTIVEX"))
         //   handler.ctx.LogWriteLine(logLine);

         // L O G I C A L  U N I T S

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetUnitID"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitIDs);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetUnitType"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitTypes);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetUnitCurrency"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitCurrencies);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetUnitValue"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitValues);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetUnitCount"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitCounts);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetUnitStatus"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitStatuses);

         // P H Y S I C A L  U N I T S

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetUnitPUNumber"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalUnitNumbers);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetPhysicalID"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalIDs);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetPhysicalPositionName"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalPositionNames);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetPhysicalInitialCount"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalInitialCounts);


         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetPhysicalStatus"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalStatuses);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetPhysicalRejectCount"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalRejectCounts);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetPhysicalCount"))
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalCounts);

         #endregion

         // C I M  

         //if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::StartCashIn") && logLine.Contains("Invoked"))
         //   return new SPFlatLine(handler, logLine, SPFlatType.CIM_StartCashIn);

         //if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::AcceptCash") && logLine.Contains("Invoked"))
         //   return new SPFlatLine(handler, logLine, SPFlatType.CIM_AcceptCash);

         //if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::OpenShutter") && logLine.Contains("Invoked"))
         //   return new SPFlatLine(handler, logLine, SPFlatType.CIM_AcceptCash);

         //if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::CloseShutter") && logLine.Contains("Invoked"))
         //   return new SPFlatLine(handler, logLine, SPFlatType.CIM_AcceptCash);

         //// Logical Units

         //if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetLogicalUnit.Type"))
         //   return CIMLogicalUnit.LUFactory(handler, logLine);

         return null;
      }
   }
}
