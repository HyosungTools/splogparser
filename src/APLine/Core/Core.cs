using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class Core : APLine
   {
      public string name;

      public Core(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         name = string.Empty;

         // e.g. [KeyBridgeWebServiceRequestFlowPoint]
         Regex regex = new Regex("^.*\\[(?<corename>.*?)WebServiceRequestFlowPoint.*$");
         Match m = regex.Match(logLine);
         if (m.Success)
         {
            name = m.Groups["corename"].Value;
         }
      }

      public static new ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         /* CORE  - WebServiceRequestFlowPoint */

         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ProcessWithdrawalTransaction") && logLine.Contains("Amount"))
            return new Core_ProcessWithdrawalTransaction_Amount(logFileHandler, logLine);

         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("ProcessWithdrawalTransaction") && logLine.Contains("Account"))
            return new Core_ProcessWithdrawalTransaction_Account(logFileHandler, logLine);

         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("DispenseCurrency") && logLine.Contains("RequiredBillMixList"))
            return new Core_RequiredBillMixList(logFileHandler, logLine);

         if (logLine.Contains("WebServiceRequestFlowPoint") && logLine.Contains("DispenseCurrency") && logLine.Contains("dispensedAmount"))
            return new Core_DispensedAmount(logFileHandler, logLine);

         return null;
      }
   }
}
