using Contract;

namespace LogLineHandler
{
   public class DevUnSolEvent : APLine
   {
      public string device = string.Empty;
      public string statusChanged = string.Empty; 
      public string evt = string.Empty;

      public DevUnSolEvent(ILogFileHandler parent, string logLine, APLogType apType = APLogType.DEV_UNSOL_EVENT) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Split the string based on '[' and ']' characters
         string[] terms = logLine.Split('[', ']');
         if (terms.Length > 6)
         {
            device = terms[3].Replace("[", "").Replace("]", "").Trim();
            terms = logLine.Split('(', ')');
            if (terms.Length > 0)
            {
               terms = terms[0].Replace("[", "").Replace("]", "").Trim().Split(',');
               if (terms.Length > 0)
               {
                  statusChanged = terms[0].Replace("Changed", "");
                  if (terms[1].IndexOf('_') != -1)
                  {
                     evt = terms[1].Substring(terms[1].IndexOf('_') + 1);
                  }
                  else
                  {
                     evt = terms[1];
                  }
               }
            }
         }
      }
   }
}
