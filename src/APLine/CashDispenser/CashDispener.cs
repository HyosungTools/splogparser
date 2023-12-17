using Contract;

namespace LogLineHandler
{
   public class CashDispenser
   {

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         /* CASH DISPENSER */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[Open") && logLine.Contains("NumberOfPhysicalUnits"))
            return new CashDispenser_Open(logFileHandler, logLine);



         /* STATUS */

         /* device */
         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnDeviceStatusChanged") && logLine.Contains("DEVONLINE"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnLine);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnDeviceStatusChanged") && logLine.Contains("DEVOFFLINE"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OffLine);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnDeviceStatusChanged") && logLine.Contains("DEVHWERROR"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnHWError);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[RaiseDeviceUnSolEvent") && logLine.Contains("DEVICE_ERROR"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_DeviceError);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[RaiseDeviceUnSolEvent") && logLine.Contains("DEVICE_OK"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDeviceOK);


         /* position status */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnPositionStatusChanged") && logLine.Contains("NOTINPOSITION"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_NotInPosition);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnPositionStatusChanged") && logLine.Contains("INPOSITION"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_InPosition);


         /* dispense */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnDispenserStatusChanged") && logLine.Contains("NODISPENSE"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnNoDispense);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnDispenserStatusChanged") && logLine.Contains("OK"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDispenserOK);



         /* status - shutter, position, stacker, transport */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnShutterStatusChanged") && logLine.Contains("OPEN"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnShutterOpen);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnShutterStatusChanged") && logLine.Contains("CLOSED"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnShutterClosed);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnStackerStatusChanged") && logLine.Contains("NOTEMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnStackerNotEmpty);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnStackerStatusChanged") && logLine.Contains("EMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnStackerEmpty);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnPositionStatusChanged") && logLine.Contains("NOTEMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnPositionNotEmpty);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnPositionStatusChanged") && logLine.Contains("EMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnPositionEmpty);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnTransportStatusChanged") && logLine.Contains("NOTEMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnTransportNotEmpty);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnTransportStatusChanged") && logLine.Contains("EMPTY"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnTransportEmpty);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnCashUnitChanged"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnCashUnitChanged);


         /* SUMMARY */

         /* summary - set up */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[SetupCSTListInHostTypeInfo"))
            return new CashDispenser_SetupCSTList(logFileHandler, logLine);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[SetupNoteTypeInfo"))
            return new CashDispenser_SetupNoteType(logFileHandler, logLine);


         /* DISPENSE */

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnDenominateComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDenominateComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[ExecDispense_NDCDDC_LCU"))
            return new CashDispenser_ExecDispense(logFileHandler, logLine);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[DispenseSyncAsync"))
            return new CashDispenser_DispenseSyncAsync(logFileHandler, logLine);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnDispenseComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnDispenseComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnPresentComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnPresentComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnRetractComplete"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnRetractComplete);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[OnItemsTaken"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnItemsTaken);

         if (logLine.Contains("[CashDispenser") && logLine.Contains("[GetLCULastDispensedCount"))
            return new APLine(logFileHandler, logLine, APLogType.CashDispenser_OnItemsTaken);

         return null; 
      }
   }
}
