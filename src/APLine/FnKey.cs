using Contract;

namespace LogLineHandler
{
   public class FnKey : APLine
   {
      public string key = string.Empty;

      public FnKey(ILogFileHandler parent, string logLine, APLogType apType = APLogType.APLOG_FLW_FUNCTIONKEY) : base(parent, logLine, apType)
      {
      }
      protected override void Initialize()
      {
         base.Initialize();

         // Split the string based on '[' and ']' characters
         string[] terms = logLine.Split('[', ']');
         if (terms.Length >= 6)
         {
            key = terms[6].Replace("[", "").Replace("]", "");
         }

      }
   }
}
