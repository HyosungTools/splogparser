using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogFileHandler;

namespace LogLineHandler
{
   public class SPFlatLogHandler : LogHandler, ILogFileHandler
   {
      private static readonly Regex TimestampRegex =
          new Regex(@"\d{4}/\d{2}/\d{2}\d{6}:\d{2} \d{2}\.\d{3}", RegexOptions.Compiled);

      // A device envelope that can sit immediately before a record's date: 0003<DEV>0007ACTIVEX0010
      // (device tag + source "ACTIVEX" + the date field's 4-char length prefix). Exactly 22 chars.
      private static readonly Regex DeviceEnvelopeRegex =
          new Regex(@"^0003[A-Z]{3}0007ACTIVEX0010$", RegexOptions.Compiled);

      private List<int> _lineOffsets = new List<int>();
      private int _currentLineIndex = 0;
      private string _fullText = "";

      public List<int> LineOffsets => _lineOffsets;

      public SPFlatLogHandler(ICreateStreamReader createReader, Func<ILogFileHandler, string, ILogLine> factory = null)
          : base(ParseType.SF, createReader, factory)
      {
         LogExpression = "*.nwlog";
         Name = "SPFlatLogHandler";
      }

      /// <summary>
      /// See SPLogHandler: nwlog traces also live with the ActiveTeller logs, outside the ATM's [SP]
      /// folder. Widen the search to the whole work folder so every *.nwlog is found, not only those
      /// under the unzipped input folder (SubFolder).
      /// </summary>
      protected override string LogSearchRoot(IContext ctx)
      {
         return ctx.WorkFolder;
      }

      public override void OpenLogFile(string fileName, int offset = 0)
      {
         base.OpenLogFile(fileName);
         _lineOffsets.Clear();
         _currentLineIndex = 0;

         // Clean the char buffer into a string
         var sb = new StringBuilder();
         foreach (char c in logFile)
         {
            if ((c >= 32 && c <= 126) || c == '\t')
               sb.Append(c);
            else
               sb.Append(' ');
         }

         _fullText = sb.ToString();

         // Match all timestamp starts. The timestamp (record date) is the reliable per-record
         // delimiter - every record has one. Only ~70% of records carry a device envelope, so we do
         // NOT frame on the device tag (that would swallow the ~30% framework/SP records).
         //
         // Device attribution: when a device envelope (0003<DEV>0007ACTIVEX0010) sits immediately
         // before a record's date, extend that line back to include it, so the record carries its own
         // device tag and SPFlatRecord.Decode can attribute it. Records without an envelope (framework
         // chatter) are unaffected, and no record is dropped - still one line per timestamp.
         var matches = TimestampRegex.Matches(_fullText);
         foreach (Match match in matches)
         {
            int start = match.Index;
            if (start >= 22 && DeviceEnvelopeRegex.IsMatch(_fullText.Substring(start - 22, 22)))
            {
               start -= 22;
            }
            _lineOffsets.Add(start);
         }

         Console.WriteLine($"[SUMMARY] Found {_lineOffsets.Count} timestamp lines.");
      }

      public string ReadLine()
      {
         if (_currentLineIndex >= _lineOffsets.Count)
            return null;

         int start = _lineOffsets[_currentLineIndex];
         int end = (_currentLineIndex + 1 < _lineOffsets.Count)
             ? _lineOffsets[_currentLineIndex + 1]
             : _fullText.Length;

         _currentLineIndex++;

         return _fullText.Substring(start, end - start).Trim();
      }

      public bool EOF()
      {
         return _currentLineIndex >= _lineOffsets.Count;
      }

      public void Close()
      {
         // No cleanup needed
      }

      public ILogLine IdentifyLine(string logLine)
      {
         return Factory?.Invoke(this, logLine);
      }
   }
}
