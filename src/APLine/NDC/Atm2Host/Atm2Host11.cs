using Contract;

namespace LogLineHandler
{
   public class Atm2Host11 : Atm2Host
   {
      public string amount = string.Empty;

      public Atm2Host11(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Atm2Host11) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();


      }
   }
}
