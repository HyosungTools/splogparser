using System;
using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   // Commands where this structure is used: 
   //
   // WFS_CMD_IPM_MEDIA_IN


   public class WFSIPMMEDIAREFUSED : WFSCUINFO
   {
      public string wReason { get; set; }
      public string wMediaLocation { get; set; }
      public string bPresentRequired { get; set; }

      public string errorCode { get; set; }

      private const string prefix = "20";

      public WFSIPMMEDIAREFUSED(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_IPM_MEDIAREFUSED) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         wReason = string.Empty;
         wMediaLocation = string.Empty;
         bPresentRequired = string.Empty;

         // guard: truncated ("more data") records may not contain lpResult
         int indexOflpResult = logLine.IndexOf("lpResult =");
         string logicalSubLogLine = indexOflpResult >= 0 ? logLine.Substring(indexOflpResult) : logLine;

         // e.g wReason = [4],  ->  "204" to match IPMView.xml wReason codes (201..2018)
         result = NumericPropertyFromList(logicalSubLogLine, "wReason");
         if (result.success) wReason = prefix + result.xfsMatch.Trim();

         // e.g. wMediaLocation = [2],
         result = NumericPropertyFromList(logicalSubLogLine, "wMediaLocation");
         if (result.success) wMediaLocation = result.xfsMatch.Trim();

         // e.g. bPresentRequired = [0],
         result = NumericPropertyFromList(logicalSubLogLine, "bPresentRequired");
         if (result.success) bPresentRequired = result.xfsMatch.Trim();

         // errorCode
         result = errorCodeFromList(logLine);
         if (result.success) errorCode = result.xfsMatch.Trim();
      }

      // I N D I V I D U A L    A C C E S S O R S

      // errorCode
      protected static (bool success, string xfsMatch, string subLogLine) errorCodeFromList(string logLine)
      {
         return Util.MatchList(logLine, @"(?<=ErrorCode\s*=\s*)(\d+)");
      }
   }
}
