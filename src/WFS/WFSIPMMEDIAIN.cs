using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMMEDIAIN : WFS
   {
      public string usMediaOnStacker { get; set; }
      public string usLastMedia { get; set; }
      public string usLastMediaOnStacker { get; set; }

      public WFSIPMMEDIAIN(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         result = usMediaOnStackerFromList(nwLogLine);
         usMediaOnStacker = result.xfsMatch.Trim();

         result = usLastMediaFromList(result.subLogLine);
         usLastMedia = result.xfsMatch.Trim();

         result = usLastMediaOnStackerFromList(result.subLogLine);
         usLastMediaOnStacker = result.xfsMatch.Trim();

         return result.subLogLine;
      }

      // I N D I V I D U A L    A C C E S S O R S


      // usMediaOnStacker 
      public static (bool success, string xfsMatch, string subLogLine) usMediaOnStackerFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usMediaOnStacker = \\[)(\\d+)");
      }

      // usLastMedia
      public static (bool success, string xfsMatch, string subLogLine) usLastMediaFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=ulValues = \\[)(\\d+)");
      }

      // usLastMediaOnStacker
      public static (bool success, string xfsMatch, string subLogLine) usLastMediaOnStackerFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usLastMediaOnStacker = \\[)(\\d+)");
      }
   }
}
