using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CashDispenser_UpdateTypeInfoToDispense : APLine
   {
      public string dispenseAmount;

      public CashDispenser_UpdateTypeInfoToDispense(ILogFileHandler parent, string logLine, APLogType apType = APLogType.CashDispenser_UpdateTypeInfoToDispense) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = "Dispensing amount in total is ";

         int idx = logLine.IndexOf(findMe);
         if (idx != -1)
         {
            dispenseAmount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }

   }
}
