using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Commands where this structure is used: 
   //
   // WFS_INF_IPM_TRANSACTION_STATUS


   public class WFSIPMTRANSSTATUS : SPLine
   {
      public string wMediaInTransaction { get; set; }
      public string usMediaOnStacker { get; set; }
      public string usLastMediaInTotal { get; set; }
      public string usLastMediaAddedToStacker { get; set; }
      public string usTotalItems { get; set; }
      public string usTotalItemsRefused { get; set; }
      public string usTotalBunchesRefused { get; set; }
      public string lpszExtra { get; set; }

      public WFSIPMTRANSSTATUS(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_INF_IPM_TRANSACTION_STATUS) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result; 

         result = wMediaInTransactionFromList(logLine);
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
      }


      // I N D I V I D U A L    A C C E S S O R S


      // wMediaInTransaction  - we dont need to search for this in the table log line, only in the list log line
      protected static (bool success, string xfsMatch, string subLogLine) wMediaInTransactionFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wMediaInTransaction = \\[)(\\d+)");
      }

      // usMediaOnStackerFromList
      protected static (bool success, string xfsMatch, string subLogLine) usMediaOnStackerFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usMediaOnStacker = \\[)(\\d+)");
      }

      // usLastMediaInTotal
      protected static (bool success, string xfsMatch, string subLogLine) usLastMediaInTotalFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usLastMediaInTotal = \\[)(\\d+)");
      }

      // usLastMediaAddedToStacker
      protected static (bool success, string xfsMatch, string subLogLine) usLastMediaAddedToStackerFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usLastMediaAddedToStacker = \\[)(\\d+)");
      }

      // usTotalItems  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) usTotalItemsFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usTotalItems = \\[)(\\d+)");
      }

      // usTotalItemsRefused
      protected static (bool success, string xfsMatch, string subLogLine) usTotalItemsRefusedFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usTotalItemsRefused = \\[)(\\d+)");
      }

      // usTotalBunchesRefused
      protected static (bool success, string xfsMatch, string subLogLine) usTotalBunchesRefusedFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usTotalBunchesRefused = \\[)(\\d+)");
      }

      // lpszExtra
      protected static (bool success, string xfsMatch, string subLogLine) lpszExtraFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=lpszExtra = \\[Meaning=)(.*?)\\]");
      }
   }
}
