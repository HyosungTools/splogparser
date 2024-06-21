
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum SSLogType
   {
      /* Not an APLog line we are interested in */
      None,

      EJ_UploadedReceived,
      EJ_Created,
      EJ_Discovered,
      EJ_ImportSucceeded,

      /* ERROR */
      Error
   }

   public class SSLine : LogLine, ILogLine
   {
      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      public SSLogType ssType { get; set; }

      public string ejFileName;

      public SSLine(ILogFileHandler parent, string logLine, SSLogType ssType) : base(parent, logLine)
      {
         this.ssType = ssType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();

         // Isolate the EJ File Name
         if (ssType == SSLogType.EJ_UploadedReceived)
         {
            int idx = logLine.Trim().LastIndexOf(" ");
            if (idx != -1)
            {
               ejFileName = logLine.Substring(idx + 1).Trim();
            }
         }
         else
         {
            int idx = logLine.LastIndexOf("\\");
            if (idx != -1)
            {
               ejFileName = logLine.Substring(idx + 1);
               idx = ejFileName.IndexOf(".");
               if (idx != -1)
               {
                  ejFileName = ejFileName.Substring(0, idx).Trim();
               }
            }
         }
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         // set timeStamp to a default time
         string timestamp = LogLine.DefaultTimestamp;

         // search for timestamp in the log line
         // e.g. 2023-12-08 04:01:31.7350
         string regExp = @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{4}";
         Regex timeRegex = new Regex(regExp);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            timestamp = m.Groups[0].Value;
         }

         return timestamp;
      }

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (logLine.Contains("Settlement.API.Controllers.JournalsController") && logLine.Contains("Upload received for "))
         {
            return new SSLine(logFileHandler, logLine, SSLogType.EJ_UploadedReceived);
         }

         if (logLine.Contains("Settlement.API.Controllers.JournalsController") && logLine.Contains("Created"))
         {
            return new SSLine(logFileHandler, logLine, SSLogType.EJ_Created);
         }

         if (logLine.Contains("JournalImporter.JournalImporter") && logLine.Contains("Discovered"))
         {
           return new SSLine(logFileHandler, logLine, SSLogType.EJ_Discovered);
         }

         if (logLine.Contains("JournalImporter.JournalImporter") && logLine.Contains("Import Succeeded:"))
         {
            return new SSLine(logFileHandler, logLine, SSLogType.EJ_ImportSucceeded);
         }

         logFileHandler.ctx.ConsoleWriteLogLine("null");

         return null;
      }
   }
}
