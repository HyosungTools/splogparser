using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class ExtensionStarted : AELine
   {
      public string extensionName { get; set; } = string.Empty;

      public ExtensionStarted(ILogFileHandler parent, string logLine, AELogType aeType = AELogType.ExtensionStarted) : base(parent, logLine, aeType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.IndexOf('[');
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx);

            //2023-11-17 03:00:22 [MoniPlus2sExtension] The 'MoniPlus2sExtension' extension is started.
            //2023-11-17 03:00:22 [NetOpExtension] The 'NetOpExtension' extension is started.
            //2023-11-17 03:01:58 [NextwareExtension] The 'NextwareExtension' extension is started.

            Regex regex = new Regex("\\[(?<extension>.*)\\] The '\\k<extension>' extension is started.");
            Match m = regex.Match(subLogLine);
            if (!m.Success)
            {
               throw new Exception($"AELogLine.ExtensionStarted: did not recognize the log line '{logLine}'");
            }

            extensionName = m.Groups["extension"].Value;
            IsRecognized = true;
         }

         if (!IsRecognized)
         {
            throw new Exception($"AELogLine.ExtensionStarted: did not recognize the log line '{logLine}'");
         }
      }
   }
}
