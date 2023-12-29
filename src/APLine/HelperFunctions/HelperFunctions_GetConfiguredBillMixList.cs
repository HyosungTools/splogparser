using Contract;

namespace LogLineHandler
{
   public class HelperFunctions_GetConfiguredBillMixList : HelperFunctions
   {
      public string configuredbillmixlist;

      public HelperFunctions_GetConfiguredBillMixList(ILogFileHandler parent, string logLine, APLogType apType = APLogType.HelperFunctions_GetConfiguredBillMixList) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = "ConfiguredBillMixList:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            configuredbillmixlist = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
