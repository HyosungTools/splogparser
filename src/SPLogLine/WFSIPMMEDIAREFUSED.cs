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


         int indexOflpResult = logLine.IndexOf("lpResult =");
         string logicalSubLogLine = logLine.Substring(indexOflpResult);
         Console.WriteLine(String.Format("WFSIPMMEDIAREFUSED : logicalSubLogLine : {0}", logicalSubLogLine));

         // e.g wReason = [4],
         result = NumericPropertyFromList(logicalSubLogLine, "wReason");
         if (result.success) wReason = result.xfsMatch.Trim();

         // e.g. wMediaLocation = [2],
         result = NumericPropertyFromList(logicalSubLogLine, "wMediaLocation");
         if (result.success) wMediaLocation = result.xfsMatch.Trim();

         // e.g. bPresentRequired = [0],
         result = NumericPropertyFromList(logicalSubLogLine, "bPresentRequired");
         if (result.success) bPresentRequired = result.xfsMatch.Trim();
      }
   }
}
