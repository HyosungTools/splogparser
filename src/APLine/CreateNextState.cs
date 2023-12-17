using Contract;

namespace LogLineHandler
{
   public class CreateNextState : APLine
   {
      public string nextState = string.Empty;

      public CreateNextState(ILogFileHandler parent, string logLine, APLogType apType = APLogType.APLOG_FLW_SCREEN_NAME) : base(parent, logLine, apType)
      {
      }
      protected override void Initialize()
      {
         base.Initialize();

         string findMe = "State created: ";
         int idx = logLine.IndexOf(findMe);
         if (idx != -1)
         {
            nextState = logLine.Substring(idx + findMe.Length + 1).Replace("", "Common-").Replace("PLACEHOLDER-", "");
            idx = nextState.IndexOf("(");
            if (idx != -1)
            {
               nextState = nextState.Substring(0, idx - 1);
            }
         }
      }

   }
}
