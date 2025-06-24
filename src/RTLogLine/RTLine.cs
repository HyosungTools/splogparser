using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum RTLogType
   {
      ENONE
   }

   public class RTLine : LogLine, ILogLine
   {
      public RTLogType rtType { get; set; }

      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      protected override string tsTimestamp()
      {
         //throw new System.NotImplementedException();
         return ""; 
      }

      protected override string hResult()
      {
         return "";
      }

      public RTLine(ILogFileHandler parent, string logLine, RTLogType rtType) : base(parent, logLine)
      {
         this.rtType = rtType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = false; // bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         return new RTLine(logFileHandler, logLine, RTLogType.ENONE);
      }
   }
}
