using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CashDispenser_Open : APLine
   {
      public string numCashUnits;

      public CashDispenser_Open(ILogFileHandler parent, string logLine, APLogType apType = APLogType.CashDispenser_Open) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // CDM NumberOfPhysicalUnits is 6
         string[] terms = logLine.Split(' ');
         if (terms.Length > 0)
         {
            numCashUnits = terms[terms.Length - 1].Trim();
         }
      }
   }
}
