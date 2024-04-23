using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contract;

namespace LogLineHandler
{
   public enum IILogType
   {
      /* Not a log line we are interested in */
      None,
      IISComment,
      IISRequest
   }

   public class IILine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }
      public string HResult { get; set; }
      public IILogType iiType { get; set; }

      public string lineNumber = string.Empty;

      public IILine(ILogFileHandler parent, string logLine, IILogType iiType) : base(parent, logLine)
      {
         this.iiType = iiType;
         Initialize();


         lineNumber = parent.LineNumber.ToString();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         // set timeStamp to a default time
         string timestamp = LogLine.DefaultTimestamp;

         // #Software: Microsoft Internet Information Services 10.0
         // #Version: 1.0
         // #Date: 2023-11-14 00:00:00
         // #Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) sc-status sc-substatus sc-win32-status time-taken
         // 2023-11-14 00:00:00 10.201.33.12 PUT /ActiveTeller/api/applicationstates/6 - 80 - 10.50.230.10 - - 200 0 0 110

         // search for timestamp in the log line
         string regExp = @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}";
         Regex timeRegex = new Regex(regExp);
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            timestamp = m.Groups[0].Value;
         }

         return timestamp;
      }
   }
}
