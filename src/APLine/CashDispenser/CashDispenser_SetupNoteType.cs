using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class CashDispenser_SetupNoteType : APLine
   {
      public string noteType;
      public string currency;
      public string value;
      public string splcu;
      public string sppcu;

      public CashDispenser_SetupNoteType(ILogFileHandler parent, string logLine, APLogType apType = APLogType.CashDispenser_SetupNoteType) : base(parent, logLine, apType)
      {

      }

      protected override void Initialize()
      {
         base.Initialize();

         string findMe = "Set NoteType";
         int idx = logLine.IndexOf(findMe);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx);

            // subLogLine looks like this:
            // Set NoteTypeA Currency:[USD] Value:[1] SPLCUIndex:[2] SPPCUIndex:[-1]

            Regex regex = new Regex("^Set NoteType(?<notetype>.) Currency:\\[(?<currency>.*?)\\] Value:\\[(?<value>.*?)\\] SPLCUIndex:\\[(?<splcu>.*?)\\] SPPCUIndex:\\[(?<sppcu>.*?)\\]");
            Match m = regex.Match(subLogLine);
            if (!m.Success)
            {
               return;
            }

            noteType = m.Groups["notetype"].Value;
            currency = m.Groups["currency"].Value;
            value = m.Groups["value"].Value; 
            splcu = m.Groups["splcu"].Value;
            sppcu = m.Groups["sppcu"].Value;
         }

      }
   }
}
