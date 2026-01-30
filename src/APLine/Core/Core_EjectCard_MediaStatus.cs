using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_EjectCard_MediaStatus : Core
   {
      public string mediaStatus = string.Empty;

      public Core_EjectCard_MediaStatus(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_EjectCard_MediaStatus) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: DeviceFramework.CardReader.MediaStatus is [NOTPRESENT].
         Regex regex = new Regex(@"MediaStatus is \[(?<status>\w+)\]");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            mediaStatus = m.Groups["status"].Value;
         }
      }
   }
}
