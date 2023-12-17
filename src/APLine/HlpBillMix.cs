using Contract;

namespace LogLineHandler
{
   public class HlpBillMix : APLine
   {
      public string billMix { get; set; }

      public HlpBillMix(ILogFileHandler parent, string logLine, APLogType apType = APLogType.HLPR_BILLMIX) : base(parent, logLine, apType)
      {
         billMix = string.Empty;
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.LastIndexOf(':');
         if (idx != -1)
         {
            billMix = logLine.Substring(idx + 1).Replace("~"," x ").Trim();
         }
      }
   }
}
