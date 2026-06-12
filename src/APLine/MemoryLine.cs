using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum MemoryMetric
   {
      None,
      DateTimeCount,    /* Date  Time  :6/3/2026 12:49:16 PM   count:1276 */
      Memory,           /* Memory      : 479,989,760                      */
      VMSize,           /* VM      size:1,457,496,064                     */
      PrivateSize,      /* Private size: 498,593,792                      */
      HandleCount       /* Handle count:5947                              */
   }

   /// <summary>
   /// One line of a periodic resource usage snapshot written by the application.
   /// A full snapshot is a group of five consecutive lines (date/time + count,
   /// memory, VM size, private size, handle count) sharing a timestamp.
   /// </summary>
   public class MemoryLine : APLine
   {
      /// <summary>Which metric this line carries.</summary>
      public MemoryMetric metric { get; private set; }

      /// <summary>Parsed numeric value: bytes for size metrics, a count otherwise.</summary>
      public long value { get; private set; }

      public MemoryLine(ILogFileHandler parent, string logLine, MemoryMetric metric)
         : base(parent, logLine, APLogType.APLOG_MEMORY)
      {
         this.metric = metric;
         this.value = ParseValue(logLine);
      }

      /// <summary>
      /// The value is the digits (possibly comma separated) following the last ':' in the line.
      /// This holds for all five metric line formats.
      /// </summary>
      private static long ParseValue(string logLine)
      {
         int lastColon = logLine.LastIndexOf(':');
         if (lastColon < 0 || lastColon == logLine.Length - 1)
         {
            return 0;
         }

         string raw = logLine.Substring(lastColon + 1).Trim().Replace(",", "");

         Match m = Regex.Match(raw, @"^\d+");
         if (!m.Success)
         {
            return 0;
         }

         long result = 0;
         long.TryParse(m.Value, out result);
         return result;
      }

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (Regex.IsMatch(logLine, @"Date\s+Time\s+:.+count:\d+"))
            return new MemoryLine(logFileHandler, logLine, MemoryMetric.DateTimeCount);

         if (Regex.IsMatch(logLine, @"\bMemory\s+:\s*[\d,]+"))
            return new MemoryLine(logFileHandler, logLine, MemoryMetric.Memory);

         if (Regex.IsMatch(logLine, @"\bVM\s+size:\s*[\d,]+"))
            return new MemoryLine(logFileHandler, logLine, MemoryMetric.VMSize);

         if (Regex.IsMatch(logLine, @"\bPrivate size:\s*[\d,]+"))
            return new MemoryLine(logFileHandler, logLine, MemoryMetric.PrivateSize);

         if (Regex.IsMatch(logLine, @"\bHandle count:\s*\d+"))
            return new MemoryLine(logFileHandler, logLine, MemoryMetric.HandleCount);

         return null;
      }
   }
}
