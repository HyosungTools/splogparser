using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CashDispenser_DispenseSyncAsync : APLine
   {
      public string mixAlgo;
      public string currency;
      public string amount;
      public string[] dispense;

      public CashDispenser_DispenseSyncAsync(ILogFileHandler parent, string logLine, APLogType apType = APLogType.CashDispenser_DispenseSyncAsync) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // MixAlgo=0, Currency=USD, Amount=0, Disp=0 0 0 0 1 0 

         string findMe = "MixAlgo";

         int idx = logLine.IndexOf(findMe);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx);

            Regex regex = new Regex("^MixAlgo=(?<mixAlgo>.), Currency=(?<currency>.*?), Amount=(?<amount>.*?), Disp=(?<dispense>.*?)\r\n");
            Match m = regex.Match(subLogLine);
            if (!m.Success)
            {
               return;
            }

            mixAlgo = m.Groups["mixAlgo"].Value;
            currency = m.Groups["currency"].Value;
            amount = m.Groups["amount"].Value;
            dispense = m.Groups["dispense"].Value.Trim().Split(' ');

         }
      }
   }
}
