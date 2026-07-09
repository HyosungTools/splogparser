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

      /* CIM deposit lifecycle - methods invoked */
      CIM_StartCashIn,
      CIM_SetCashInLimit,
      CIM_AcceptCash,
      CIM_StoreCash,
      CIM_RollbackCash,
      CIM_OpenShutter,
      CIM_CloseShutter,

      /* CIM deposit lifecycle - results and events */
      CIM_HandleCashInStart,
      CIM_HandleCashIn,
      CIM_HandleCashInEnd,
      CIM_StoreCashComplete,
      CIM_HandleRollback,
      CIM_HandleRetract,
      CIM_HandleReset,
      CIM_HandleOpenShutter,
      CIM_HandleCloseShutter,
      CIM_ItemsInserted,
      CIM_ItemsTaken,
      CIM_InputRefuse,
      CIM_CountsChanged,
      CIM_CashUnitInfo,
      CIM_CashUnitTrace,

      /* CIM accepted note detail */
      CIM_NoteID,
      CIM_NoteCount,
      CIM_CashInStatus,
      CIM_CashInRefused,
      CIM_LastCashInStatus,
      CIM_NumberOfCashInStatus,
      CIM_CashInStatusValue,
      CIM_CashInStatusID,
      CIM_CashInStatusItemCount,
      CIM_CashInStatusCurrencyID,
      CIM_CashInStatusExponent,

      /* CIM Logical Unit */
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

         // Flat lines carry hResult in three shapes:
         //    Execute-Result[CashInEnd] = {hResult[-1316]}   (service handler result)
         //    hResult = [0]                                  (legacy bracketed form)
         //    hResult=-1316                                  (WFS_EXECUTE_COMPLETE / WFS_GETINFO_COMPLETE)
         Regex bracketRegex = new Regex(@"hResult\s*=?\s*\[([^\]]*)\]");
         Match mtch = bracketRegex.Match(logLine);
         if (mtch.Success)
         {
            hResult = mtch.Groups[1].Value.Trim();
         }
         else
         {
            Regex plainRegex = new Regex(@"hResult=(-?\d+)");
            mtch = plainRegex.Match(logLine);
            if (mtch.Success)
            {
               hResult = mtch.Groups[1].Value.Trim();
            }
         }

         return hResult == "0" ? "" : hResult;
      }
      public static ILogLine Factory(ILogFileHandler handler, string logLine)
      {
         // Generic SP-level failure: any XFS completion message reporting a negative hResult
         // (e.g. WFS_EXECUTE_COMPLETE(RequestID=846, hService=13, hResult=-1316, dwCommandCode=1303 ...))
         if (logLine.Contains("_COMPLETE(") && logLine.Contains("hResult=-"))
            return new SPFlatLine(handler, logLine, SPFlatType.Error);

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

         #region CIM

         // C A S H  U N I T  /  C A S S E T T E  C O U N T S

         // Consolidated dump (DieboldNixdorf flat logs): one line carries every field for one
         // logical unit. Must be checked BEFORE LUFactory - the Trace line contains
         // "].InitialCount[" etc. and would otherwise be misclassified as a single field.
         if (logLine.Contains("CLogicalUnit::TraceCIMCashUnitInfo"))
            return new CIMCashUnitTrace(handler, logLine);

         // Per-property format (FI-style flat logs): Ctrl::GetLogicalUnit.X -> LogicalUnit[n].X[v]
         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetLogicalUnit."))
            return CIMLogicalUnit.LUFactory(handler, logLine);

         // D E P O S I T  L I F E C Y C L E  -  M E T H O D S  I N V O K E D

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::StartCashInEx") && logLine.Contains("Invoked"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_StartCashIn);

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::SetCashInLimit") && logLine.Contains("Invoked"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_SetCashInLimit);

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::AcceptCash") && logLine.Contains("Invoked"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_AcceptCash);

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::StoreCash") && logLine.Contains("Invoked"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_StoreCash);

         if (logLine.Contains("METHOD") && logLine.Contains("Ctrl::RollbackCash") && logLine.Contains("Invoked"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_RollbackCash);

         // NOTE: Ctrl::OpenShutter / Ctrl::CloseShutter / Ctrl::RetractEx / Ctrl::ResetEx are not
         // CIM-unique method names (IPM and CDM controls expose the same names) and the flat record
         // format does not let us cheaply attribute a METHOD line to a device. The unambiguous
         // CCimService::Handle* result lines below carry the outcome, timestamp and hResult instead.

         // D E P O S I T  L I F E C Y C L E  -  R E S U L T S  A N D  E V E N T S

         // Handler class name appears as both CCimService:: and CCIMService:: in the same log,
         // so match on the unique Execute-Result payload token where possible.

         if (logLine.Contains("Execute-Result[CashInStart]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCashInStart);

         if (logLine.Contains("Execute-Result[CashInEnd]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCashInEnd);

         if (logLine.Contains("Execute-Result[CashInRollback]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleRollback);

         if (logLine.Contains("Execute-Result[CashIn]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCashIn);

         if (logLine.Contains("EVENT") && logLine.Contains("FireStoreCashComplete"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_StoreCashComplete);

         if ((logLine.Contains("CimService::HandleRetract") || logLine.Contains("CIMService::HandleRetract")) && logLine.Contains("Execute-Result[Retract]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleRetract);

         if ((logLine.Contains("CimService::HandleReset") || logLine.Contains("CIMService::HandleReset")) && logLine.Contains("Execute-Result[Reset]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleReset);

         // CDM emits the same OpenShutter/CloseShutter/CashUnitInfo/CountsChanged payloads via
         // CCdmService::, so these checks must be constrained to the CIM service class name.

         if ((logLine.Contains("CimService::HandleOpenSht") || logLine.Contains("CIMService::HandleOpenSht")) && logLine.Contains("Execute-Result[OpenShutter]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleOpenShutter);

         if ((logLine.Contains("CimService::HandleCloseSht") || logLine.Contains("CIMService::HandleCloseSht")) && logLine.Contains("Execute-Result[CloseShutter]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCloseShutter);

         if ((logLine.Contains("CimService::HandleItemsInserted") || logLine.Contains("CIMService::HandleItemsInserted")) && logLine.Contains("INFORMATION"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_ItemsInserted);

         if ((logLine.Contains("CimService::HandleItemsTaken") || logLine.Contains("CIMService::HandleItemsTaken")) && logLine.Contains("INFORMATION"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_ItemsTaken);

         if (logLine.Contains("CimService::HandleInputRefuse") || logLine.Contains("CIMService::HandleInputRefuse"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_InputRefuse);

         if (logLine.Contains("CimService::HandleCountsChanged") || logLine.Contains("CIMService::HandleCountsChanged"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_CountsChanged);

         if ((logLine.Contains("CimService::HandleCashUnitInfo") || logLine.Contains("CIMService::HandleCashUnitInfo")) && logLine.Contains("GetInfo-Result[CashUnitInfo]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_CashUnitInfo);

         // A C C E P T E D  N O T E  D E T A I L

         if ((logLine.Contains("CimService::HandleCashIn") || logLine.Contains("CIMService::HandleCashIn")) && logLine.Contains("NoteID("))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_NoteID);

         if ((logLine.Contains("CimService::HandleCashIn") || logLine.Contains("CIMService::HandleCashIn")) && logLine.Contains("NoteCount("))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_NoteCount);

         if (logLine.Contains("Ctrl::HandleCashInStatus") && logLine.Contains("CashIn_Status["))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatus);

         if (logLine.Contains("Ctrl::HandleCashInStatus") && logLine.Contains("CashIn_Refused["))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInRefused);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetLastCashInStatus"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_LastCashInStatus);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetNumberOfCashInStatus"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_NumberOfCashInStatus);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetCashInStatus.Value"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusValue);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetCashInStatus.ID"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusID);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetCashInStatus.ItemCount"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusItemCount);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetCashInStatus.CurrencyID"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusCurrencyID);

         if (logLine.Contains("PROPERTY") && logLine.Contains("Ctrl::GetCashInStatus.Exponent"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusExponent);

         #endregion

         return null;
      }
   }
}
