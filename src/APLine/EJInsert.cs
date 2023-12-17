using RegEx;
using Contract;

namespace LogLineHandler
{
   public class EJInsert : APLine
   {
      public string ejTable = string.Empty;
      public string[] ejColumns = null;
      public string[] ejValues = null;

      public EJInsert(ILogFileHandler parent, string logLine, APLogType apType = APLogType.EJInsert) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         int idx = logLine.IndexOf("INSERT INTO ");
         if (idx == -1)
         {
            return;
         }

         // e.g. INSERT INTO [Session] (ATMId,StartDate,IdentificationType,IdentificationNumberMasked,AuthenticationType,AuthenticationNumberMasked) VALUES
         // ('XB5367','06/07/2023 12:18:39 PM','AccountNumber','9706','SSN','4052')

         // process the log line to remove all redundant text
         string subLogLine = logLine.Substring(idx).Replace("INSERT INTO ", "").Replace("[", "").Replace("]", "").Replace(" VALUES", "").Replace("'", "");

         idx = subLogLine.IndexOf("(");
         if (idx == -1)
         {
            return; 
         }

         ejTable = subLogLine.Substring(0, idx - 1).Trim();
         subLogLine = subLogLine.Substring(idx).Trim();

         string[] terms = subLogLine.Split(')');
         if (terms.Length > 1)
         {
            ejColumns = terms[0].Replace("(", "").Replace(")", "").Split(',');
            ejValues = terms[1].Replace("(", "").Replace(")", "").Split(',');
         }

         for (int i = 0; i < ejColumns.Length; i++)
         {
            ejColumns[i] = ejColumns[i].Trim();
            ejValues[i] = ejValues[i].Trim(); 
         }
      }
   }
}
