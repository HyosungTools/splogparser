using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum A2iALogType
   {
      /* Not an A2iA line we are interested in */
      None,

      A2iA,

      /* ERROR */
      Error
   }

    public class A2iALine : LogLine, ILogLine
    {
      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      public A2iALogType a2iaType { get; set; }

      // Fields
      public string machineNum;
      public string invalidityScore;
      public string hwMicr;

      // Data Scores
      public string noDate;
      public string noPayeeName;
      public string noCAR;
      public string noLAR;
      public string noSignature;
      public string noCodeline;
      public string noPayeeEndorsement;

      // OCR
      public string amount;
      public string amountScore;

      public string codeLine;
      public string codeLineScore;

      public string ocrDate;
      public string ocrDateScore;

      public string signature;
      public string signatureScore;

      public string checkNumber;
      public string checkNumberScore;

      //public string noPayeeName;
      public string payeeNameScore;

      public string amountLAR; 
      public string amountLARScore;

      public string amountCAR;
      public string amountCARScore;

      public A2iALine(ILogFileHandler parent, string logLine, A2iALogType a2iaType = A2iALogType.A2iA) : base(parent, logLine)
      {
         this.a2iaType = a2iaType;
         Initialize();
      }

      protected virtual void Initialize()
      {

         HResult = hResult();

         Timestamp = LogLine.DefaultTimestamp;

         Regex regex = new Regex(@"^(?<dateTime>(\d{2}).(\d{2}).(\d{2}) (\d{2}:\d{2}:\d{2}):(\d{3})) (?<rest>.*)?");

         Match m = regex.Match(logLine);
         if (!m.Success)
         {
            // Failed to isolate date/time
            return;
         }

         // Normalize A2iAResult time e.g. "2025-08-15 08:21:19.249"
         Timestamp = "20" + m.Groups[1].Value + "-" + m.Groups[2].Value + "-" + m.Groups[3].Value + " " + m.Groups[4].Value + "." + m.Groups[5].Value;

         regex = new Regex(@"^(?<machineNum>[^:]*(?=:)): (?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate machine number
            return;
         }

         machineNum = m.Groups["machineNum"].Value; 

         regex = new Regex("^Invalidity Result: (?<invalidityScore>[^,]*(?=,)),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate invalidityScore
            return;
         }

         invalidityScore = m.Groups["invalidityScore"].Value;

         // hwMicr

         regex = new Regex(@"^(?<hwMicr>\d)\|(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate hwMicr
            return;
         }

         hwMicr = m.Groups["hwMicr"].Value;

         // Data Scores

         regex = new Regex(@"^(?<noDate>[^,]*),(?<noPayeeName>[^,]*),(?<noCAR>[^,]*),(?<noLAR>[^,]*),(?<noSignature>[^,]*),(?<noCodeline>[^,]*),(?<noPayeeEndorsement>[^,]*),\|(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate 
            return;
         }

         noDate = m.Groups["noDate"].Value;
         noPayeeName = m.Groups["noPayeeName"].Value;
         noCAR = m.Groups["noCAR"].Value;
         noLAR = m.Groups["noLAR"].Value;
         noSignature = m.Groups["noSignature"].Value;
         noCodeline = m.Groups["noCodeline"].Value;
         noPayeeEndorsement = m.Groups["noPayeeEndorsement"].Value;

         // OCR

         regex = new Regex(@"^(?<amount>[^/]*)/(?<amountScore>[^,]+),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         amount = m.Groups["amount"].Value;
         amountScore = m.Groups["amountScore"].Value;

         regex = new Regex(@"^(?<codeLine>[^/]*)/(?<noCodeLineScore>[^,]*),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         codeLine = m.Groups["codeLine"].Value;
         codeLineScore = m.Groups["noCodeLineScore"].Value;

         regex = new Regex(@"^(?<date>\d{1,2}/\d{1,2}/\d{4})/(?<noDateScore>[^,]*),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         ocrDate = m.Groups["date"].Value;
         ocrDateScore = m.Groups["noDateScore"].Value;

         regex = new Regex(@"^(?<signature>[^/]*)/(?<noSignatureScore>[^,]*),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         signature = m.Groups["signature"].Value;
         signatureScore = m.Groups["noSignatureScore"].Value;

         regex = new Regex(@"^(?<checkNumber>[^/]*)/(?<checkNumberScore>[^,]*),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         checkNumber = m.Groups["checkNumber"].Value;
         checkNumberScore = m.Groups["checkNumberScore"].Value;

         regex = new Regex(@"^(?<noPayeeName>[01])/(?<noPayeeNameScore>[^,]*),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         // noPayeeName = m.Groups["noPayeeName"].Value;
         payeeNameScore = m.Groups["noPayeeNameScore"].Value;

         regex = new Regex(@"^(?<date>\d{1,2}/\d{1,2}/\d{4})/(?<noDateScore>[^,]*),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         regex = new Regex(@"^(?<amountLar>[^/]*)/(?<noLarScore>[^,]+),(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         amountLAR = m.Groups["amountLar"].Value;
         amountLARScore = m.Groups["noLarScore"].Value;

         regex = new Regex(@"^(?<amountCar>[^/]+)/(?<noCarScore>[^,]+),LAR used(?<rest>(.*))?");
         m = regex.Match(m.Groups["rest"].Value);
         if (!m.Success)
         {
            // Failed to isolate
            return;
         }

         amountCAR = m.Groups["amountCar"].Value;
         amountCARScore = m.Groups["noCarScore"].Value;
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         // 24-04-27 10:18:16:847
         // set timeStamp to a default time
         string timestamp = LogLine.DefaultTimestamp;

         // search for timestamp in the log line
         // e.g. 2023-12-08 04:01:31.7350
         string regExp = @"^\d{2}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}";
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
         return new A2iALine(logFileHandler, logLine, A2iALogType.A2iA);
      }
   }
}
