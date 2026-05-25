using Contract;
using System;
using System.Text.RegularExpressions;

namespace LogLineHandler
{
   /// <summary>
   /// Parses the CHCDUDevControl::SetCashUnitInfoResult log line.
   /// This is a single long line (NORMAL severity) containing a formatted table
   /// of physical cassette configuration — one 110-char labeled row per field,
   /// concatenated back-to-back and delimited by ##.
   /// 
   /// Provides per-cassette arrays (up to 6) for: CstNumber, Status, SerialNumber,
   /// CstID, CurrencyID, Values, NoteRevision, Calibration, MissingCheck,
   /// InitialCount, CurrentCount, DispenseCount, PresentCount, RejectCount, RetractCount.
   /// </summary>
   public class NHCDMSetCashUnitInfoResult : SPLine
   {
      // Per-cassette arrays — index 0 = Cst1, index 1 = Cst2, etc.
      public string[] CstNumber { get; private set; } = new string[0];
      public string[] Status { get; private set; } = new string[0];
      public string[] SerialNumber { get; private set; } = new string[0];
      public string[] CstID { get; private set; } = new string[0];
      public string[] CurrencyID { get; private set; } = new string[0];
      public string[] Values { get; private set; } = new string[0];
      public string[] NoteRevision { get; private set; } = new string[0];
      public string[] Calibration { get; private set; } = new string[0];
      public string[] MissingCheck { get; private set; } = new string[0];
      public string[] InitialCount { get; private set; } = new string[0];
      public string[] CurrentCount { get; private set; } = new string[0];
      public string[] DispenseCount { get; private set; } = new string[0];
      public string[] PresentCount { get; private set; } = new string[0];
      public string[] RejectCount { get; private set; } = new string[0];
      public string[] RetractCount { get; private set; } = new string[0];

      // Regex to extract timestamp from binary prefix: date(10) + length(4) + time(12)
      private static readonly Regex _timestampRegex =
         new Regex(@"(\d{4}/\d{2}/\d{2})\d{4}(\d{2}:\d{2} \d{2}\.\d{3})", RegexOptions.Compiled);

      public NHCDMSetCashUnitInfoResult(ILogFileHandler parent, string logLine)
         : base(parent, logLine, XFSType.NHCDM_SETCASHUNITINFORESULT)
      {
      }

      protected override void Initialize()
      {
         Timestamp = ParseTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = "";   // no hResult field in this line type
         ParsePayload();
      }

      private string ParseTimestamp()
      {
         Match m = _timestampRegex.Match(logLine);
         if (!m.Success)
            return "2022/01/01 00:00 00.000";

         // Combine "2026/04/14" + " " + "03:04 02.765" then normalize like SPLine.tsTimestamp()
         string combined = m.Groups[1].Value + " " + m.Groups[2].Value;
         // Replace '/' with '-'
         combined = combined.Replace('/', '-');
         // Replace char at position 16 (space between "HH:MM" and "SS.mmm") with ':'
         if (combined.Length >= 17)
            combined = combined.Remove(16, 1).Insert(16, ":");
         return combined;
      }

      private static readonly string[] ExpectedLabels = new string[]
         {
            "",
            "Cst Number :",
            "Status :",
            "Serial Number :",
            "CstID :",
            "CurrencyID :",
            "Values :",
            "Note Revision :",
            "Calibration :",
            "MissingCheck :",
            "InitialCount :",
            "CurrentCount :",
            "DispenseCount :",
            "PresentCount :",
            "RejectCount :",
            "RetractCount :"
         };

      private void ParsePayload()
      {
         // Split on "##" — each segment is one labeled row.
         // Segments before the first ## and after the last == are noise; skip them.
         string[] segments = logLine.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);

         for (int i = 1; i < segments.Length && i < ExpectedLabels.Length; i++)
         {
            string payload = segments[i].Trim();
            if (!payload.StartsWith(ExpectedLabels[i]))
            {
               if (parentHandler != null)
                  this.parentHandler.ctx.LogWriteLine(String.Format("NHCDMSetCashUnitInfoResult: unexpected segment {0}, expected '{1}', got '{2}'",
                     i, ExpectedLabels[i], payload.Substring(0, Math.Min(30, payload.Length))));
               continue;
            }

            // Cst Number :                Cst1           Cst2           Cst3           Cst4           Cst5           Cst6
            // The data always occupies the last 90 characters of a 110-char segment, so we can reliably extract it regardless of label length.

            if (payload.Length > 107)
               payload = payload.Substring(0, 107);

            string dataArea = payload.Substring(payload.Length - 87);
            string[] values = dataArea.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            switch (i)
            {
               case 1: /* Cst Number :     */ CstNumber = values; break;
               case 2: /* Status :         */ Status = values; break;
               case 3: /* Serial Number :  */ SerialNumber = values; break;
               case 4: /* CstID :          */ CstID = values; break;
               case 5: /* CurrencyID :     */ CurrencyID = values; break;
               case 6: /* Values :         */ Values = values; break;
               case 7: /* Note Revision :  */ NoteRevision = values; break;
               case 8: /* Calibration :    */ Calibration = values; break;
               case 9: /* MissingCheck :   */ MissingCheck = values; break;
               case 10: /* InitialCount :  */ InitialCount = values; break;
               case 11: /* CurrentCount :  */ CurrentCount = values; break;
               case 12: /* DispenseCount : */ DispenseCount = values; break;
               case 13: /* PresentCount :  */ PresentCount = values; break;
               case 14: /* RejectCount :   */ RejectCount = values; break;
               case 15: /* RetractCount :  */ RetractCount = values; break;
                  // Unknown labels are silently ignored
            }
         }
      }
   }
}
