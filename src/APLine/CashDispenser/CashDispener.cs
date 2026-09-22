using Contract;

namespace LogLineHandler
{
   public class CashDispenser
   {
      /// <summary>
      /// MoniPlus2 has logged the class and method two ways:
      ///   older builds:  [CashDispenser       ][OnPresentComplete   ][NORMAL]...
      ///   v25 builds:    [CashDispenser.OnPresentComplete] [TID:1] ...
      /// Match either form so the same parsers work on old and new logs.
      /// </summary>
      private static bool IsMethod(string logLine, string method)
      {
         return logLine.Contains("[" + method) || logLine.Contains("CashDispenser." + method + "]");
      }

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         /* CASH DISPENSER */

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "Open") && logLine.Contains("NumberOfPhysicalUnits"))
            return new CashDispenser_Open(logFileHandler, logLine);


         /* position status */

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnPositionStatusChanged") && logLine.Contains("NOTINPOSITION"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_NotInPosition);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnPositionStatusChanged") && logLine.Contains("INPOSITION"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_InPosition);


         /* dispense */

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnDispenserStatusChanged") && logLine.Contains("NODISPENSE"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnNoDispense);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnDispenserStatusChanged") && logLine.Contains("OK"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDispenserOK);



         /* status - shutter, position, stacker, transport */

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnShutterStatusChanged") && logLine.Contains("OPEN"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnShutterOpen);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnShutterStatusChanged") && logLine.Contains("CLOSED"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnShutterClosed);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnStackerStatusChanged") && logLine.Contains("NOTEMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnStackerNotEmpty);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnStackerStatusChanged") && logLine.Contains("EMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnStackerEmpty);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnPositionStatusChanged") && logLine.Contains("NOTEMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnPositionNotEmpty);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnPositionStatusChanged") && logLine.Contains("EMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnPositionEmpty);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnTransportStatusChanged") && logLine.Contains("NOTEMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnTransportNotEmpty);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnTransportStatusChanged") && logLine.Contains("EMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnTransportEmpty);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnCashUnitChanged"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnCashUnitChanged);


         /* SUMMARY */

         /* summary - set up */

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "SetupCSTListInHostTypeInfo"))
            return new CashDispenser_SetupCSTList(logFileHandler, logLine);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "SetupNoteTypeInfo"))
            return new CashDispenser_SetupNoteType(logFileHandler, logLine);


         /* DISPENSE */

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnDenominateComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDenominateComplete);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "ExecDispense_NDCDDC_LCU"))
            return new CashDispenser_ExecDispense(logFileHandler, logLine);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "DispenseSyncAsync"))
            return new CashDispenser_DispenseSyncAsync(logFileHandler, logLine);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnDispenseComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDispenseComplete);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnPresentComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnPresentComplete);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnRetractComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnRetractComplete);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "OnItemsTaken"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnItemsTaken);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "GetLCULastDispensedCount"))
            return new CashDispenser_GetLCULastDispensedCount(logFileHandler, logLine);

         if (logLine.Contains("[CashDispenser") && IsMethod(logLine, "UpdateTypeInfoToDispense"))
            return new CashDispenser_UpdateTypeInfoToDispense(logFileHandler, logLine);

         return null;
      }
   }
}
