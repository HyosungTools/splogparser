using Contract;

namespace LogLineHandler
{
   public class CommFrmWork : APLine
   {
      public string action = string.Empty;
      public string payload = string.Empty;

      public CommFrmWork(ILogFileHandler parent, string logLine, APLogType apType = APLogType.COMM_FRMWORK) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         // Split the string based on '[' and ']' characters
         string[] terms = logLine.Split('[', ']');
         if (terms.Length > 6)
         {
            action = terms[3].Replace("[", "").Replace("]", "").Trim();

            string lookFor = "NORMAL]";
            int idx = logLine.IndexOf(lookFor);
            if (idx != -1)
            {
               payload = logLine.Substring(idx + lookFor.Length + 1);

            }
         }
      }
   }
}
