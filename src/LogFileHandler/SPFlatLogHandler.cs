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

         // Match all timestamp starts
         var matches = TimestampRegex.Matches(_fullText);
         foreach (Match match in matches)
         {
            _lineOffsets.Add(match.Index);
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
