using Contract;

namespace LogLineHandler
{
   public class Core_DetermineNextFlowPoint : Core
   {
      public string nextFlowPoint = string.Empty;

      public Core_DetermineNextFlowPoint(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_DetermineNextFlowPoint) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Next flow point identified as PLACEHOLDER-GetAccountList.
         string findMe = "Next flow point identified as ";

         int idx = logLine.IndexOf(findMe);
         if (idx != -1)
         {
            nextFlowPoint = logLine.Substring(idx + findMe.Length).Trim();
            
            // Remove trailing period if present
            if (nextFlowPoint.EndsWith("."))
            {
               nextFlowPoint = nextFlowPoint.Substring(0, nextFlowPoint.Length - 1);
            }
         }
      }
   }
}
