using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_ExecuteState : Core
   {
      public string timeElapsed = string.Empty;

      public Core_ExecuteState(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_ExecuteState) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Process  Time Elapsed = 8669.0482
         Regex regex = new Regex(@"Time Elapsed\s*=\s*(?<time>[\d.]+)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            timeElapsed = m.Groups["time"].Value;
         }
      }
   }
}
