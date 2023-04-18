using System;
using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMEDIABINERROR : WFS
   {
      public string wFailure { get; set; }
      public WFSIPMMEDIABININFO lpMediaBin { get; set; }

      private const string prefix = "10"; 

      public WFSIPMEDIABINERROR(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = wFailureFromList(nwLogLine);
         if (result.success) wFailure = prefix + result.xfsMatch.Trim();

         // lpMediaBin
         WFSIPMMEDIABININFO lpMediaBin = new WFSIPMMEDIABININFO(ctx);

         try
         {
            lpMediaBin.Initialize(nwLogLine);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("WFSIPMMBERROR Exception {0}.", e.Message));
         }

         return result.subLogLine;
      }

      // I N D I V I D U A L    A C C E S S O R S


      // wFailure
      public static (bool success, string xfsMatch, string subLogLine) wFailureFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wFailure = \\[)(\\d+)");
      }
   }
}
