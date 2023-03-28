using System.Text.RegularExpressions;

namespace Impl
{
   /// class to isolate this data
   ///
   /// fwDevice = [0],
   /// fwSafeDoor = [1],
   /// fwDispenser = [0],
   /// fwIntermediateStacker = [1],
   /// lppPositions =
   /// {
   ///   fwPosition = [2048],
   ///	fwShutter = [0],
   ///	fwPositionStatus = [0],
   ///	fwTransport = [0],
   ///	fwTransportStatus = [0]
   /// }
   /// lpszExtra = [ErrorCode = 0000000, Description =[0000000]System OK!, ErrCode = 0000000, ErrMsg = System OK!, Position = Unknown, SP_Version = V 04.21.28, EP_Version = V 02.00.97, Boot_Version = D
   /// 

   public static class _wfs_inf_cdm_status
   {
      public static (bool success, string xfsMatch, string subLogLine) fwDevice(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwSafeDoor(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwSafeDoor = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwDispenser(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwDispenser = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwIntermediateStacker(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwIntermediateStacker = \\[(.*)\\]", "0");
      }

      // report on the first (output ?) position only. 
      public static (bool success, string xfsMatch, string subLogLine) fwPosition(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwPosition = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwShutter(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwShutter = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwPositionStatus(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwPositionStatus = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwTransport(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwTransport = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwTransportStatus(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "fwTransportStatus = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wDevicePosition(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTime(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wAntiFraudModule(string logLine)
      {
         return _wfs_base.GenericMatchList(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}
