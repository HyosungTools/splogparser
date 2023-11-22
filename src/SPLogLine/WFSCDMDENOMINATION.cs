using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCDMDENOMINATION : SPLine
   {
      public string cCurrencyID { get; set; }
      public string ulAmount { get; set; }
      public string usCount { get; set; }
      public string[] lpulValues { get; set; }
      public string ulCashBox { get; set; }

      public WFSCDMDENOMINATION(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.None) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;
         (bool success, string[] xfsMatch, string subLogLine) results;

         // cCurrencyID
         result = cCurrencyIDFromList(logLine);
         if (result.success) cCurrencyID = result.xfsMatch.Trim();

         // ulAmount
         result = ulAmountFromList(result.subLogLine);
         if (result.success) ulAmount = result.xfsMatch.Trim();

         // usCount
         result = usCountFromList(result.subLogLine);
         if (result.success) usCount = result.xfsMatch.Trim();

         // lpulValues
         results = lpulValuesFromList(result.subLogLine);
         if (results.success) lpulValues = results.xfsMatch;

         // ulCashBox
         result = ulCashBoxFromList(results.subLogLine);
         if (result.success) ulCashBox = result.xfsMatch.Trim();
      }

      // cCurrencyID
      protected static (bool success, string xfsMatch, string subLogLine) cCurrencyIDFromList(string logLine)
      {
         return Util.Match(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]");
      }
      // ulAmount
      protected static (bool success, string xfsMatch, string subLogLine) ulAmountFromList(string logLine)
      {
         return Util.Match(logLine, "(?<=ulAmount = \\[)(\\d+)\\]");
      }
      // usCount
      protected static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine)
      {
         return Util.Match(logLine, "(?<=usCount = \\[)(\\d+)\\]");
      }

      // lpulValues
      protected static (bool success, string[] xfsMatch, string subLogLine) lpulValuesFromList(string logLine)
      {
         // lpulValues = [0, 0, 0, 4, 4, 0],
         return Util.MatchListToArray(logLine, "(?<=lpulValues = \\[)([0-9 ,]*)");
      }

      // ulCashBox 
      protected static (bool success, string xfsMatch, string subLogLine) ulCashBoxFromList(string logLine)
      {
         return Util.Match(logLine, "(?<=ulCashBox = \\[)(\\d+)\\]");
      }
   }
}
