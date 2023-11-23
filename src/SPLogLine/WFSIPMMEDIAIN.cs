using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMMEDIAIN : SPLine
   {
      public string usMediaOnStacker { get; set; }
      public string usLastMedia { get; set; }
      public string usLastMediaOnStacker { get; set; }

      public WFSIPMMEDIAIN(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_CMD_IPM_MEDIA_IN) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         result = usMediaOnStackerFromList(logLine);
         usMediaOnStacker = result.xfsMatch.Trim();

         result = usLastMediaFromList(result.subLogLine);
         usLastMedia = result.xfsMatch.Trim();

         result = usLastMediaOnStackerFromList(result.subLogLine);
         usLastMediaOnStacker = result.xfsMatch.Trim();
      }

      // I N D I V I D U A L    A C C E S S O R S


      // usMediaOnStacker 
      protected static (bool success, string xfsMatch, string subLogLine) usMediaOnStackerFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usMediaOnStacker = \\[)(\\d+)");
      }

      // usLastMedia
      protected static (bool success, string xfsMatch, string subLogLine) usLastMediaFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulValues = \\[)(\\d+)");
      }

      // usLastMediaOnStacker
      protected static (bool success, string xfsMatch, string subLogLine) usLastMediaOnStackerFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usLastMediaOnStacker = \\[)(\\d+)");
      }
   }
}
