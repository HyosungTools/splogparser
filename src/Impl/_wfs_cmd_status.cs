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

   public static class _wfs_cmd_status 
   {
      private static (bool success, string xfsMatch, string subLogLine) GenericMatch(string logLine, string regStr, string def = "0")
      {
         Regex timeRegex = new Regex(regStr);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {

            return (true, m.Groups[1].Value, logLine.Substring(m.Index));
         }

         return (false, def, logLine);
      }
      public static (bool success, string xfsMatch, string subLogLine) fwDevice(string logLine)
      {
         return GenericMatch(logLine, "fwDevice = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwSafeDoor(string logLine)
      {
         return GenericMatch(logLine, "fwSafeDoor = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwDispenser(string logLine)
      {
         return GenericMatch(logLine, "fwDispenser = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwIntermediateStacker(string logLine)
      {
         return GenericMatch(logLine, "fwIntermediateStacker = \\[(.*)\\]", "0");
      }

      // report on the first (output ?) position only. 
      public static (bool success, string xfsMatch, string subLogLine) fwPosition(string logLine)
      {
         return GenericMatch(logLine, "fwPosition = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwShutter(string logLine)
      {
         return GenericMatch(logLine, "fwShutter = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwPositionStatus(string logLine)
      {
         return GenericMatch(logLine, "fwPositionStatus = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwTransport(string logLine)
      {
         return GenericMatch(logLine, "fwTransport = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) fwTransportStatus(string logLine)
      {
         return GenericMatch(logLine, "fwTransportStatus = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wDevicePosition(string logLine)
      {
         return GenericMatch(logLine, "wDevicePosition = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) usPowerSaveRecoveryTime(string logLine)
      {
         return GenericMatch(logLine, "usPowerSaveRecoveryTime = \\[(.*)\\]", "0");
      }
      public static (bool success, string xfsMatch, string subLogLine) wAntiFraudModule(string logLine)
      {
         return GenericMatch(logLine, "wAntiFraudModule = \\[(.*)\\]", "0");
      }
   }
}
