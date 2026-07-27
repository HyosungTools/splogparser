using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Catch-all for ActiveTeller Agent Extensions lines whose component tag the parser does
   /// not model yet. Rather than silently dropping them (the old behaviour, which made every
   /// agent component added after the parser was written invisible to the parse), we capture
   /// the tag and the raw line so unmodeled components surface in the AE view instead of
   /// vanishing. When an unfamiliar tag starts showing up in the UnmodeledEvents sheet, that
   /// is the cue to add a proper line class for it.
   ///
   /// Only real "&lt;timestamp&gt; [&lt;Tag&gt;]" lines reach here; continuation lines (e.g.
   /// stack-trace frames with no leading timestamp/tag) are left as None by the handler.
   /// </summary>
   public class Unmodeled : AELine
   {
      public string Tag { get; set; } = string.Empty;

      public Unmodeled(ILogFileHandler parent, string logLine, AELogType aeType = AELogType.Unmodeled)
         : base(parent, logLine, aeType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         Match m = Regex.Match(logLine, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\s+\[(?<tag>[^\]]+)\]");
         if (m.Success)
         {
            Tag = m.Groups["tag"].Value;
         }

         // Unmodeled by definition — keep the raw payload in the sheet.
         IsRecognized = false;
      }
   }
}
