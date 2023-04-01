using Contract;

namespace Impl
{
   public class WFSCDMDENOMINATION : WFS
   {
      public string cCurrencyID { get; set; }
      public string ulAmount { get; set; }
      public string usCount { get; set; }
      public string[] lpulValues { get; set; }
      public string ulCashBox { get; set; }

      public WFSCDMDENOMINATION(IContext ctx) : base (ctx)
      {
      }

      public string Initialize(string logLine)
      {
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

         return result.subLogLine;
      }

      // cCurrencyID
      public static (bool success, string xfsMatch, string subLogLine) cCurrencyIDFromList(string logLine)
      {
         return WFSMatch(logLine, "(?<=cCurrencyID = \\[)([a-zA-Z0-9 ]*)\\]");
      }
      // ulAmount
      public static (bool success, string xfsMatch, string subLogLine) ulAmountFromList(string logLine)
      {
         return WFSMatch(logLine, "(?<=ulAmount = \\[)(\\d+)\\]");
      }
      // usCount
      public static (bool success, string xfsMatch, string subLogLine) usCountFromList(string logLine)
      {
         return WFSMatch(logLine, "(?<=usCount = \\[)(\\d+)\\]");
      }

      // lpulValues
      public static (bool success, string[] xfsMatch, string subLogLine) lpulValuesFromList(string logLine)
      {
         // lpulValues = [0, 0, 0, 4, 4, 0],
         return WFSMatchListToArray(logLine, "(?<=lpulValues = \\[)([0-9 ,]*)");
      }

      // ulCashBox 
      public static (bool success, string xfsMatch, string subLogLine) ulCashBoxFromList(string logLine)
      {
         return WFSMatch(logLine, "(?<=ulCashBox = \\[)(\\d+)\\]");
      }
   }
}
