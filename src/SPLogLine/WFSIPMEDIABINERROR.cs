using System;
using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSIPMEDIABINERROR : SPLine
   {
      public string wFailure { get; set; }
      public WFSIPMMEDIABININFO lpMediaBin { get; set; }

      private const string prefix = "10";

      public WFSIPMEDIABINERROR(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.WFS_EXEE_IPM_MEDIABINERROR) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         (bool success, string xfsMatch, string subLogLine) result;

         // fwDevice
         result = wFailureFromList(logLine);
         if (result.success) wFailure = prefix + result.xfsMatch.Trim();

         // lpMediaBin
         WFSIPMMEDIABININFO lpMediaBin = new WFSIPMMEDIABININFO(parentHandler, logLine);
      }

      // I N D I V I D U A L    A C C E S S O R S


      // wFailure
      protected static (bool success, string xfsMatch, string subLogLine) wFailureFromList(string logLine)
      {
         return Util.MatchList(logLine, "(?<=wFailure = \\[)(\\d+)");
      }
   }
}
