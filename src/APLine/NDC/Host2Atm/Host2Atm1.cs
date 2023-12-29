using Contract;

namespace LogLineHandler
{
   public class Host2Atm1 : Host2Atm
   {

      public Host2Atm1(ILogFileHandler parent, string logLine, APLogType apType = APLogType.Host2Atm1) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();


      }
   }
}
