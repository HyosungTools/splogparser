using Contract;

namespace LogLineHandler
{
   public class Pinpad : APLine
   {
      public string action = string.Empty;
      public string payload = string.Empty; 

      public Pinpad(ILogFileHandler parent, string logLine, APLogType apType = APLogType.PINPAD) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.LastIndexOf(':');

         // Split the string based on '[' and ']' characters
         string[] terms = logLine.Split('[', ']');
         if (terms.Length > 3 && terms[3].StartsWith("Pinpad"))
         {
            action = terms[4].Replace("[", "").Replace("]", "").Trim();

            idx = logLine.LastIndexOf(']');
            if (idx != -1)
            {
               payload = logLine.Substring(idx + 1);
            }
         }
      }
   }
}
