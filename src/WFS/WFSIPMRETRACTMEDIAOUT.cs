using System.Collections.Generic;
using Contract;

namespace Impl
{

   public class WFSIPMRETRACTMEDIAOUT : WFS
   {
      public string usMedia { get; set; }
      public string wRetractLocation { get; set; }
      public string usBinNumber { get; set; }


      public WFSIPMRETRACTMEDIAOUT(IContext ctx) : base(ctx)
      {

      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         result = usMediaFromList(nwLogLine);
         usMedia = result.xfsMatch.Trim();

         result = wRetractLocationFromList(result.subLogLine);
         wRetractLocation = result.xfsMatch.Trim();

         result = usBinNumberFromList(result.subLogLine);
         usBinNumber = result.xfsMatch.Trim();

         return result.subLogLine;
      }

      // I N D I V I D U A L    A C C E S S O R S


      // usMedia  
      public static (bool success, string xfsMatch, string subLogLine) usMediaFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usMedia = \\[)(\\d+)");
      }

      // wRetractLocation
      public static (bool success, string xfsMatch, string subLogLine) wRetractLocationFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wRetractLocation = \\[)([a-zA-Z0-9]*)\\]", "0");
      }

      // usBinNumber
      public static (bool success, string xfsMatch, string subLogLine) usBinNumberFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usBinNumber = \\[)([a-zA-Z0-9 ]*)\\]", "0");
      }
   }
}
