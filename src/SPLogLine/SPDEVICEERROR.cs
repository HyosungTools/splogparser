using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class SPDEVICEERROR : SPLine
   {
      public string ClassName { get; set; }     // CCCIMDev, CBRM20
      public string Operation { get; set; }     // _Retract, _DepositCheck, Dispense, _Initialize
      public string ErrorCode { get; set; }     // 9889, 4C0D, 70D1A2
      public string ErrorMessage { get; set; }  // "Fail to _Retract with error=[9889]"

      // Packed binary timestamp: date, 4-digit length prefix, then time
      private static readonly Regex timestampRegex = new Regex(
         @"(\d{4}/\d{2}/\d{2})\d{4}(\d{2}:\d{2} \d{2}\.\d{3})",
         RegexOptions.Compiled);

      // "Fail to X with ... error=[hex]" or "Failed to X with ... error=[hex]"
      private static readonly Regex errorRegex = new Regex(
         @"Fail(?:ed)? to (\w+) with.*?error=\[([A-Fa-f0-9]+)\]",
         RegexOptions.Compiled);

      // Class::Operation before the error message
      private static readonly Regex classOpRegex = new Regex(
         @"(\w+)::(_?\w+)",
         RegexOptions.Compiled);

      public SPDEVICEERROR(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.DEVICE_ERROR)
         : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();

         // Parse class::operation
         Match classMatch = classOpRegex.Match(logLine);
         if (classMatch.Success)
         {
            ClassName = classMatch.Groups[1].Value;
            Operation = classMatch.Groups[2].Value;
         }

         // Parse error message and code - Operation here wins over classOpRegex
         // because it captures the actual failed action
         Match errorMatch = errorRegex.Match(logLine);
         if (errorMatch.Success)
         {
            Operation = errorMatch.Groups[1].Value;
            ErrorCode = errorMatch.Groups[2].Value;
            ErrorMessage = errorMatch.Value;
         }

         Console.WriteLine($"SPDEVICEERROR: {Timestamp} {ClassName}::{Operation} error=[{ErrorCode}]");
      }

      protected override string tsTimestamp()
      {
         // Extract timestamp from packed binary format
         // e.g. "00102026/03/31001210:36 21.192" -> "2026-03-31 10:36:21.192"
         Match m = timestampRegex.Match(logLine);
         if (m.Success)
         {
            string logTime = m.Groups[1].Value + " " + m.Groups[2].Value;

            // normalize: replace '/' with '-'
            logTime = logTime.Replace('/', '-');

            // replace space between minutes and seconds with ':'
            // "2026-03-31 10:36 21.192" -> "2026-03-31 10:36:21.192"
            if (logTime.Length >= 17)
            {
               logTime = logTime.Remove(16, 1).Insert(16, ":");
            }

            return logTime;
         }

         return base.tsTimestamp();
      }

      protected override string hResult()
      {
         // Use the hex error code as hResult
         Match m = errorRegex.Match(logLine);
         if (m.Success)
         {
            return m.Groups[2].Value;
         }

         return base.hResult();
      }
   }
}
