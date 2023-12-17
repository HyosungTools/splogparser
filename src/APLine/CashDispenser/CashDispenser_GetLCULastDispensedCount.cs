using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CashDispenser_GetLCULastDispensedCount : APLine
   {
      public string noteType;
      public string amount;

      public CashDispenser_GetLCULastDispensedCount(ILogFileHandler parent, string logLine, APLogType apType = APLogType.CashDispenser_GetLCULastDispensedCount) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // e.g. Last Dispensed Count A = 0 

         string findMe = "Last Dispensed Count ";

         int idx = logLine.IndexOf(findMe);
         if (idx != -1)
         {
            string[] terms = logLine.Substring(idx + findMe.Length + 1).Replace(" ","").Split('=');
            noteType = terms[0];
            amount = terms[1].Trim();
         }
      }
   }
}
