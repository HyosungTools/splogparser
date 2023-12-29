using Contract;

namespace LogLineHandler
{
   public class Core_RequiredBillMixList : Core
   {
      public string requiredbillmixlist;

      public Core_RequiredBillMixList(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_RequiredBillMixList) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = ":";

         int idx = logLine.LastIndexOf(findMe);
         if (idx != -1)
         {
            requiredbillmixlist = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
