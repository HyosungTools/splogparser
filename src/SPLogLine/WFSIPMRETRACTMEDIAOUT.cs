using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{

   public class WFSIPMRETRACTMEDIAOUT : SPLine
   {
      public string usMedia { get; set; }
      public string wRetractLocation { get; set; }
      public string usBinNumber { get; set; }

      public WFSIPMRETRACTMEDIAOUT(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_CMD_IPM_RETRACT_MEDIA) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = usMediaFromList(logLine);
         usMedia = result.xfsMatch.Trim();

         result = wRetractLocationFromList(result.subLogLine);
         wRetractLocation = result.xfsMatch.Trim();

         result = usBinNumberFromList(result.subLogLine);
         usBinNumber = result.xfsMatch.Trim();
      }

      // I N D I V I D U A L    A C C E S S O R S


      // usMedia  
      protected static (bool success, string xfsMatch, string subLogLine) usMediaFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usMedia = \\[)(\\d+)");
      }

      // wRetractLocation
      protected static (bool success, string xfsMatch, string subLogLine) wRetractLocationFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wRetractLocation = \\[)([a-zA-Z0-9]*)\\]", "0");
      }

      // usBinNumber
      protected static (bool success, string xfsMatch, string subLogLine) usBinNumberFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usBinNumber = \\[)([a-zA-Z0-9 ]*)\\]", "0");
      }
   }
}
