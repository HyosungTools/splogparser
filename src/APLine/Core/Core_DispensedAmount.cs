using Contract;

namespace LogLineHandler
{
   public class Core_DispensedAmount : Core
   {
      public string amount;

      public Core_DispensedAmount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_DispensedAmount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.LastIndexOf("=");
         if (idx != -1)
         {
            amount = logLine.Substring(idx + 1).Trim();
         }
      }
   }
}
