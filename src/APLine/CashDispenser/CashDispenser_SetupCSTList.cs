using Contract;

namespace LogLineHandler
{
   public class CashDispenser_SetupCSTList : APLine
   {
      public string relation;
      public string parent;
      public string child;

      public CashDispenser_SetupCSTList(ILogFileHandler parent, string logLine, APLogType apType = APLogType.CashDispenser_SetupCSTList) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.LastIndexOf(':');
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + 1);
            string[] terms = subLogLine.Split(' ');

            if (terms.Length == 6)
            {
               relation = "1:" + terms[0];
               parent = terms[2];
               child = terms[5].Trim(); ;
            }
         }
      }
   }
}
