using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// A DN CDM command completion, logged as an XFS result event rather than a named handler:
   ///
   ///   EVENT  CCDMService::HandleXFSResult  ->  FireXFSEvent [uiMsg=408, dwCommandCode=302, hResult=0]
   ///
   /// This is where a Diebold-Nixdorf machine records dispense / present / reject / reset etc. - there
   /// is no Ctrl::Dispense or CCdmService::HandleDispense on DN. dwCommandCode is the XFS
   /// WFS_CMD_CDM_* command number; hResult is the outcome (base class turns it into HResult: "" for
   /// success, the code for a fault).
   ///
   /// The command number is turned into an English name in the table layer via the Messages lookup
   /// (type "dwCommandCode"), so the mapping is team-maintained data, not hardcoded here.
   /// </summary>
   public class CDMXFSResultLine : SPFlatLine
   {
      /// <summary>The XFS WFS_CMD_CDM_* command code, or -1 if not present.</summary>
      public int CommandCode { get; private set; }

      public CDMXFSResultLine(ILogFileHandler handler, string line, SPFlatType flatType = SPFlatType.CDM_XFSResult)
         : base(handler, line, flatType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();
         CommandCode = -1;
         Match c = Regex.Match(logLine, @"dwCommandCode=(-?\d+)");
         int parsed;
         if (c.Success && int.TryParse(c.Groups[1].Value, out parsed))
         {
            CommandCode = parsed;
         }
      }
   }
}
