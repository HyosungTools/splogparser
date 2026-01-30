using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core_UpdateDataFrameworkAmount : Core
   {
      public string dispensedAmount = string.Empty;
      public string requiredAmount = string.Empty;
      public string isCash = string.Empty;
      public string isNoteDispensed = string.Empty;

      public Core_UpdateDataFrameworkAmount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Core_UpdateDataFrameworkAmount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Pattern: Dispensed amount: 40; Required amount: 40; IsCash:True; IsNoteDispensed: True.
         Regex regex = new Regex(@"Dispensed amount:\s*(?<dispensed>\d+);\s*Required amount:\s*(?<required>\d+);\s*IsCash:\s*(?<isCash>\w+);\s*IsNoteDispensed:\s*(?<isNote>\w+)");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            dispensedAmount = m.Groups["dispensed"].Value;
            requiredAmount = m.Groups["required"].Value;
            isCash = m.Groups["isCash"].Value;
            isNoteDispensed = m.Groups["isNote"].Value;
         }
      }
   }
}
