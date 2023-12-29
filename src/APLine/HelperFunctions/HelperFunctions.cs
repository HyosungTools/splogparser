using Contract;

namespace LogLineHandler
{
   public class HelperFunctions : APLine
   {
      public HelperFunctions(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
      }

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (logLine.Contains("[HelperFunctions") && logLine.Contains("[GetConfiguredBillMixList") && logLine.Contains("ConfiguredBillMixList:"))
            return new HelperFunctions_GetConfiguredBillMixList(logFileHandler, logLine);

         if (logLine.Contains("[HelperFunctions") && logLine.Contains("[GetFewestBillMixList") && logLine.Contains("FewestBillMixList:"))
            return new HelperFunctions_GetFewestBillMixList(logFileHandler, logLine);

         return null;
      }
   }
}
