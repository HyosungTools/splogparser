using Contract;

namespace LogLineHandler
{
   public class CashDispenser_ExecDispense : APLine
   {
      public string hostAmount;

      public CashDispenser_ExecDispense(ILogFileHandler parent, string logLine, APLogType apType = APLogType.CashDispenser_ExecDispense) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = "Host amount is ";

         int idx = logLine.IndexOf(findMe);
         if (idx != -1)
         {
            hostAmount = logLine.Substring(idx + findMe.Length).Trim();
         }
      }
   }
}
