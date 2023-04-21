using System.Collections.Generic;
using Contract;

namespace Impl
{
   // Commands where this structure is used: 
   //
   // WFS_INF_IPM_TRANSACTION_STATUS


   public class WFSIPMTRANSSTATUS : WFS
   {
      public string wMediaInTransaction { get; set; }
      public string usMediaOnStacker { get; set; }
      public string usLastMediaInTotal { get; set; }
      public string usLastMediaAddedToStacker { get; set; }
      public string usTotalItems { get; set; }
      public string usTotalItemsRefused { get; set; }
      public string usTotalBunchesRefused { get; set; }
      public string lpszExtra { get; set; }

      public WFSIPMTRANSSTATUS(IContext ctx) : base(ctx)
      {
      }

      public string Initialize(string nwLogLine)
      {
         (bool success, string xfsMatch, string subLogLine) result; 

         result = wMediaInTransactionFromList(nwLogLine);
         wMediaInTransaction = result.xfsMatch.Trim(); 
         
         result = usMediaOnStackerFromList(result.subLogLine);
         usMediaOnStacker = result.xfsMatch.Trim(); 

         result = usLastMediaInTotalFromList(result.subLogLine);
         usLastMediaInTotal = result.xfsMatch.Trim();

         result = usLastMediaAddedToStackerFromList(result.subLogLine);
         usLastMediaAddedToStacker = result.xfsMatch.Trim();

         result = usTotalItemsFromList(result.subLogLine);
         usTotalItems = result.xfsMatch.Trim();

         result = usTotalItemsRefusedFromList(result.subLogLine);
         usTotalItemsRefused = result.xfsMatch.Trim();

         result = usTotalBunchesRefusedFromList(result.subLogLine);
         usTotalBunchesRefused = result.xfsMatch.Trim();

         result = lpszExtraFromList(result.subLogLine);
         lpszExtra = result.xfsMatch.Trim();

         return result.subLogLine;
      }


      // I N D I V I D U A L    A C C E S S O R S


      // wMediaInTransaction  - we dont need to search for this in the table log line, only in the list log line
      public static (bool success, string xfsMatch, string subLogLine) wMediaInTransactionFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=wMediaInTransaction = \\[)(\\d+)");
      }

      // usMediaOnStackerFromList
      public static (bool success, string xfsMatch, string subLogLine) usMediaOnStackerFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usMediaOnStacker = \\[)(\\d+)");
      }

      // usLastMediaInTotal
      public static (bool success, string xfsMatch, string subLogLine) usLastMediaInTotalFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usLastMediaInTotal = \\[)(\\d+)");
      }

      // usLastMediaAddedToStacker
      public static (bool success, string xfsMatch, string subLogLine) usLastMediaAddedToStackerFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usLastMediaAddedToStacker = \\[)(\\d+)");
      }

      // usTotalItems  - singular search from a list-style log line
      public static (bool success, string xfsMatch, string subLogLine) usTotalItemsFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usTotalItems = \\[)(\\d+)");
      }

      // usTotalItemsRefused
      public static (bool success, string xfsMatch, string subLogLine) usTotalItemsRefusedFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usTotalItemsRefused = \\[)(\\d+)");
      }

      // usTotalBunchesRefused
      public static (bool success, string xfsMatch, string subLogLine) usTotalBunchesRefusedFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=usTotalBunchesRefused = \\[)(\\d+)");
      }

      // lpszExtra
      public static (bool success, string xfsMatch, string subLogLine) lpszExtraFromList(string logLine)
      {
         return WFS.WFSMatchList(logLine, "(?<=lpszExtra = \\[Meaning=)(.*?)\\]");
      }
   }
}
