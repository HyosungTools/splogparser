using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class IISComment : IILine
   {
      private string className = "IISComment";


      public static string IISVersion { get; private set; } = string.Empty;
      public static string IISApplicationName { get; private set; } = string.Empty;
      public static string IISCommentState { get; private set; } = string.Empty;
      public string Exception { get; private set; } = string.Empty;

      public IISComment(ILogFileHandler parent, string logLine, IILogType awType = IILogType.IISComment) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         //#Software: Microsoft Internet Information Services 10.0
         //#Version: 1.0
         //#Date: 2023-11-14 07:00:00
         //#Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) sc-status sc-substatus sc-win32-status time-taken

         Regex regex = new Regex("#Software: (?<application>.*)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            IISApplicationName = m.Groups["application"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("#Version: (?<version>.*)");
         m = regex.Match(logLine);
         if (m.Success)
         {
            IISVersion = m.Groups["version"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("#Date: (?<timestamp>.*)");
         m = regex.Match(logLine);
         if (m.Success)
         {
            IISCommentState = $"IIS RESTARTED at {m.Groups["timestamp"].Value}";
            IsRecognized = true;
            return;
         }

         if (!IsRecognized)
         {
           throw new Exception($"IILogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
