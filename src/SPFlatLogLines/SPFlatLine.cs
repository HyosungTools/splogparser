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

      CDM_CashUnitTrace,
      CDM_XFSResult,

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

      Device,

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
         if (string.IsNullOrEmpty(logLine))
            return null;

         // Generic SP-level failure: any XFS completion reporting a negative hResult, e.g.
         //   WFS_EXECUTE_COMPLETE(RequestID=846, hService=13, hResult=-1316, dwCommandCode=1303 ...)
         // Checked on the raw line because it is unambiguous and framing-independent.
         if (logLine.Contains("_COMPLETE(") && logLine.Contains("hResult=-"))
            return new SPFlatLine(handler, logLine, SPFlatType.Error);

         // Decode the record into fields. Route on Method/Payload (reliable regardless of framing).
         SPFlatRecord rec = SPFlatRecord.Decode(logLine);
         if (!rec.Ok)
            return null;

         string method = rec.Method;      // e.g. "Ctrl::GetCashInStatus.Value", "CCimService::HandleCashIn"
         string payload = rec.Payload;    // e.g. "CashInStatus[0].Value[5]"
         string category = rec.Category;  // PROPERTY / INFORMATION / METHOD / EVENT / XFSAPI / ERROR

         // Small helpers so service-class case variants (CCimService vs CCIMService) don't matter.
         bool MethodHas(string s) { return method.IndexOf(s, System.StringComparison.OrdinalIgnoreCase) >= 0; }
         bool PayloadHas(string s) { return payload.IndexOf(s, System.StringComparison.OrdinalIgnoreCase) >= 0; }

         // =========================================================================================
         // C D M
         // =========================================================================================

         // Dispense lifecycle (DN uses Denominate; FI machines also emit Dispense/Present).
         if (method == "Ctrl::Denominate" && category == "METHOD" && PayloadHas("Invoked"))
            return new CDMDenominateLine(handler, logLine);

         if (MethodHas("HandleDenominate") && PayloadHas("Execute-Result[Denominate]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandleDenominate);

         if (method == "Ctrl::Dispense" && category == "METHOD" && PayloadHas("Invoked"))
            return new CDMDispenseLine(handler, logLine);

         if (MethodHas("HandleDispense") && PayloadHas("Execute-Result[Dispense]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandleDispense);

         if (method == "Ctrl::Present" && category == "METHOD" && PayloadHas("Invoked"))
            return new SPFlatLine(handler, logLine, SPFlatType.CDM_PresentInvoked);

         if (MethodHas("HandlePresent") && PayloadHas("Execute-Result[Present]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandlePresent);

         if (MethodHas("HandleItemsTaken") && MethodHas("Cdm"))
            return new SPFlatLine(handler, logLine, SPFlatType.CDM_HandleItemsTaken);

         // Consolidated CDM cash-unit dump (DN dialect): every logical unit in one line as parallel
         // arrays incl. UnitValue (the per-cassette denomination). Feeds the CDM 'Summary' worksheet -
         // the DN equivalent of the FI per-property GetUnit* lines below.
         if (method == "Ctrl::TraceCDMCashUnitInfo")
            return new CDMCashUnitTrace(handler, logLine);

         // DN dispense lifecycle: DN logs dispense/present/retract as XFS command-code events
         // (FireXFSEvent [dwCommandCode=NNN, hResult=N]) via CCDMService::HandleXFSResult, NOT as
         // Ctrl::Dispense / HandleDispense. Scoped to the CDM service class so CIM's HandleXFSResult
         // (CCIMService::) is not swept in. The line carries the command code + hResult.
         if (MethodHas("CDMService::HandleXFSResult") && PayloadHas("FireXFSEvent"))
            return new CDMXFSResultLine(handler, logLine);

         // FI-dialect per-property logical units (kept for Hyosung-flat machines).
         if (category == "PROPERTY" && method == "Ctrl::GetUnitID")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitIDs);
         if (category == "PROPERTY" && method == "Ctrl::GetUnitType")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitTypes);
         if (category == "PROPERTY" && method == "Ctrl::GetUnitCurrencyID")   // FI method is ...CurrencyID
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitCurrencies);
         if (category == "PROPERTY" && method == "Ctrl::GetUnitValue")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitValues);
         if (category == "PROPERTY" && method == "Ctrl::GetUnitCount")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitCounts);
         if (category == "PROPERTY" && method == "Ctrl::GetUnitStatus")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_UnitStatuses);

         // FI-dialect physical units.
         if (category == "PROPERTY" && method == "Ctrl::GetUnitPUNumber")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalUnitNumbers);
         if (category == "PROPERTY" && method == "Ctrl::GetPhysicalID")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalIDs);
         if (category == "PROPERTY" && method == "Ctrl::GetPhysicalPositionName")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalPositionNames);
         if (category == "PROPERTY" && method == "Ctrl::GetPhysicalInitialCount")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalInitialCounts);
         if (category == "PROPERTY" && method == "Ctrl::GetPhysicalStatus")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalStatuses);
         if (category == "PROPERTY" && method == "Ctrl::GetPhysicalRejectCount")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalRejectCounts);
         if (category == "PROPERTY" && method == "Ctrl::GetPhysicalCount")
            return new CDMUnitList(handler, logLine, SPFlatType.CDM_PhysicalCounts);

         // =========================================================================================
         // C I M
         // =========================================================================================

         // Consolidated cash-unit dump (DN + FI both emit this). Must precede LUFactory.
         if (MethodHas("TraceCIMCashUnitInfo") || PayloadHas("LogicalUnit[0].Number["))
            return new CIMCashUnitTrace(handler, logLine);

         // FI-dialect per-property logical unit (Ctrl::GetLogicalUnit.X -> LogicalUnit[n].X[v]).
         if (category == "PROPERTY" && method.StartsWith("Ctrl::GetLogicalUnit."))
            return CIMLogicalUnit.LUFactory(handler, logLine);

         // Deposit lifecycle - methods invoked. NOTE the DN fix: Ctrl::StartCashIn (no "Ex").
         if ((method == "Ctrl::StartCashIn" || method == "Ctrl::StartCashInEx") && category == "METHOD")
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_StartCashIn);
         if (method == "Ctrl::SetCashInLimit" && category == "METHOD")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_SetCashInLimit);
         if (method == "Ctrl::AcceptCash" && category == "METHOD")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_AcceptCash);
         if (method == "Ctrl::StoreCash" && category == "METHOD")
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_StoreCash);
         if (method == "Ctrl::RollbackCash" && category == "METHOD")
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_RollbackCash);

         // Deposit lifecycle - results (Execute-Result payload token is the unambiguous discriminator).
         if (PayloadHas("Execute-Result[CashInStart]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCashInStart);
         if (PayloadHas("Execute-Result[CashInEnd]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCashInEnd);
         if (PayloadHas("Execute-Result[CashInRollback]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleRollback);
         if (PayloadHas("Execute-Result[CashIn]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCashIn);
         if (MethodHas("FireStoreCashComplete") || PayloadHas("FireStoreCashComplete"))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_StoreCashComplete);
         if (MethodHas("HandleRetract") && PayloadHas("Execute-Result[Retract]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleRetract);
         if (MethodHas("HandleReset") && PayloadHas("Execute-Result[Reset]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleReset);
         if (MethodHas("HandleOpenSht") && PayloadHas("Execute-Result[OpenShutter]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleOpenShutter);
         if (MethodHas("HandleCloseSht") && PayloadHas("Execute-Result[CloseShutter]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_HandleCloseShutter);
         if (MethodHas("HandleItemsInserted") && category == "INFORMATION")
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_ItemsInserted);
         if (MethodHas("CimService::HandleItemsTaken") && category == "INFORMATION")
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_ItemsTaken);
         if (MethodHas("HandleInputRefuse"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_InputRefuse);
         if (MethodHas("HandleCountsChanged"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_CountsChanged);
         if (MethodHas("HandleCashUnitInfo") && PayloadHas("GetInfo-Result[CashUnitInfo]"))
            return new SPFlatLine(handler, logLine, SPFlatType.CIM_CashUnitInfo);

         // Accepted-note detail.
         if (MethodHas("HandleCashIn") && PayloadHas("NoteID("))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_NoteID);
         if (MethodHas("HandleCashIn") && PayloadHas("NoteCount("))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_NoteCount);

         // Cash-in status.
         if (method == "Ctrl::HandleCashInStatus" && PayloadHas("CashIn_Status["))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatus);
         if (method == "Ctrl::HandleCashInStatus" && PayloadHas("CashIn_Refused["))
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInRefused);
         if (method == "Ctrl::GetLastCashInStatus")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_LastCashInStatus);
         if (method == "Ctrl::GetNumberOfCashInStatus")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_NumberOfCashInStatus);
         if (method == "Ctrl::GetCashInStatus.Value")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusValue);
         if (method == "Ctrl::GetCashInStatus.ID")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusID);
         if (method == "Ctrl::GetCashInStatus.ItemCount")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusItemCount);
         if (method == "Ctrl::GetCashInStatus.CurrencyID")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusCurrencyID);
         if (method == "Ctrl::GetCashInStatus.Exponent")
            return new CIMCashInLine(handler, logLine, SPFlatType.CIM_CashInStatusExponent);

         // Per-device flat views (IPM/IDC/PIN): any record attributed to a device that has its own flat
         // view becomes a generic device line; that device's table filters by rec.Device and routes on
         // method/payload. Requires the device-attributing framing in SPFlatLogHandler. CDM/CIM already
         // returned above, so they never reach here.
         if (rec.Device == "IPM" || rec.Device == "IDC" || rec.Device == "PIN")
            return new SPFlatDeviceLine(handler, logLine);

         // =========================================================================================
         // TODO - NEXT INCREMENT (needs new SPFlatType values + a new line class + table cases).
         // These are the DN-only routes with no existing home yet. Left here (as comments) so the full
         // intended routing is visible; uncomment as each new type lands so the project keeps compiling.
         //
         //   // CDM consolidated cash-unit dump (the DN equivalent of CIM's TraceCIMCashUnitInfo).
         //   // Add: enum SPFlatType.CDM_CashUnitTrace  +  class CDMCashUnitTrace : SPFlatLine
         //   //      (parse NumberOfLogicalUnits[n], UnitNumber[(..)], UnitType[(..)], UnitID[(..)],
         //   //       UnitCurrencyID[(..)], UnitValue[(..)], UnitCount[(..)], UnitStatus[(..)]).
         //   if (method == "Ctrl::TraceCDMCashUnitInfo")
         //      return new CDMCashUnitTrace(handler, logLine);
         //
         //   // Device / dispenser / acceptor / shutter status over time (both CDM and CIM).
         //   // Add: enum SPFlatType.DeviceStatus  +  a generic status line carrying method+payload.
         //   if (category == "PROPERTY" && (method == "Ctrl::GetDeviceStatus"
         //         || method == "Ctrl::GetDispenserStatus" || method == "Ctrl::GetAcceptorStatus"
         //         || method == "Ctrl::GetShutterStatus"   || method == "Ctrl::GetSafeDoorStatus"))
         //      return new SPFlatStatusLine(handler, logLine, rec);   // carry rec so the table reads fields
         // =========================================================================================

         return null;
      }
   }

}
