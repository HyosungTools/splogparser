using Contract;

namespace LogLineHandler
{
   public class HelperFunctions_GetFewestBillMixList : HelperFunctions
   {
      public string fewestbillmixlist;
      public HelperFunctions_GetFewestBillMixList(ILogFileHandler parent, string logLine, APLogType apType = APLogType.HelperFunctions_GetFewestBillMixList) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = "FewestBillMixList:";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            fewestbillmixlist = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
