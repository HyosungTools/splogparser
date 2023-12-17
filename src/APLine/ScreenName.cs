using Contract;

namespace LogLineHandler
{
   public class ScreenName : APLine
   {
      public string name = string.Empty;

      public ScreenName(ILogFileHandler parent, string logLine, APLogType apType = APLogType.APLOG_FLW_SCREEN_NAME) : base(parent, logLine, apType)
      {
      }
      protected override void Initialize()
      {
         base.Initialize();

         // Split the string based on '[' and ']' characters
         string[] terms = logLine.Split('[', ']');
         if (terms.Length > 0)
            name = terms[terms.Length - 1].Replace("[", "").Replace("]", "");
      }

   }
}
