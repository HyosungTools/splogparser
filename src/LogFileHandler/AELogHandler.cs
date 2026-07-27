using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class AELogHandler : LogHandler, ILogFileHandler
   {
      public AELogHandler(ICreateStreamReader createReader, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(ParseType.AE, createReader, Factory)
      {
         LogExpression = "ActiveTellerAgentExtensions_*.*";
         Name = "AELogFileHandler";
      }

      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
      {
         return traceFilePos >= logFile.Length;
      }

      /// <summary>
      /// Read one log line from a twlog file. 
      /// </summary>
      /// <returns></returns>
      public string ReadLine()
      {
         // builder will hold the line
         StringBuilder builder = new StringBuilder();

         bool endOfLine = false;

         // while not EOL or EOF
         while (!endOfLine && !EOF())
         {
            char c = logFile[traceFilePos];
            traceFilePos++;

            // check for end of line or end of file
            if (c == '\n' || EOF())
            {
               endOfLine = true;

               if (c == '\n')
               {
                  break;
               }
            }

            // ignore nulls and non-printing ASCII characters
            if (c > 0 && c < 128 && c != '\r')
            {
               builder.Append(c);
            }
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         //2023-11-17 03:00:22 [MoniPlus2sExtension] The 'MoniPlus2sExtension' extension is started.
         if (logLine.Contains("extension is started"))
         {
            return new ExtensionStarted(this, logLine, AELogType.ExtensionStarted);
         }

         //2023-11-17 03:00:22 [NetOpExtension] The 'NetOpExtension' extension is started.
         if (logLine.Contains("[NetOpExtension]"))
         {
            try
            {
               return new NetOpExtension(this, logLine, AELogType.NetOpExtension);
            }
            catch (Exception)
            {
               // Backstop: newer NetOp message this parser throws on — capture, don't abort the file.
               return new Unmodeled(this, logLine, AELogType.Unmodeled);
            }
         }

         //2023-11-17 03:01:58 [NextwareExtension] The 'NextwareExtension' extension is started.
         if (logLine.Contains("[NextwareExtension]"))
         {
            try
            {
               return new NextwareExtension(this, logLine, AELogType.NextwareExtension);
            }
            catch (Exception)
            {
               // Backstop: newer Nextware message this parser throws on — capture, don't abort the file.
               return new Unmodeled(this, logLine, AELogType.Unmodeled);
            }
         }

         //2023-11-17 03:00:22 [MoniPlus2sExtension] The 'MoniPlus2sExtension' extension is started.
         if (logLine.Contains("[MoniPlus2sExtension]"))
         {
            try
            {
               return new MoniPlus2sExtension(this, logLine, AELogType.MoniPlus2sExtension);
            }
            catch (Exception)
            {
               // Backstop: a newer MoniPlus2s message or JSON field the detailed parser still
               // throws on (a deep "unhandled key" case). Capture the raw line instead of letting
               // it abort the whole file's AE parse; it surfaces in UnmodeledEvents so it can be
               // modeled later.
               return new Unmodeled(this, logLine, AELogType.Unmodeled);
            }
         }

         //2026-07-24 00:30:35 [RecordingUploadManager] Pausing video recording uploads.
         // (added to ActiveTeller after this parser was first written)
         if (logLine.Contains("[RecordingUploadManager]"))
         {
            return new RecordingUploadManager(this, logLine, AELogType.RecordingUploadManager);
         }

         // Any other tagged agent-extension component we don't model yet: capture it so it
         // surfaces in the AE view instead of being silently dropped. Untagged continuation
         // lines (stack-trace frames, etc.) have no leading "<timestamp> [<Tag>]" and fall
         // through to None below.
         if (Regex.IsMatch(logLine, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\s+\[[^\]]+\]"))
         {
            return new Unmodeled(this, logLine, AELogType.Unmodeled);
         }

         return new AELine(this, logLine, AELogType.None);
      }
   }
}
