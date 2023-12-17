using Contract;

namespace LogLineHandler
{
   public class PANData : APLine
   {
      public string PAN = string.Empty;
      public PANData(ILogFileHandler parent, string logLine, APLogType apType = APLogType.APLOG_FLW_CARD_PAN) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.LastIndexOf(':');
         if (idx != -1)
         {
            PAN = logLine.Substring(idx + 1).Trim(); 
         }
      }
   }
}
