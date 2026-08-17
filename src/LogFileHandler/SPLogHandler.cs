using System;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads Trace files (.nwlog) one line at a time
   /// </summary>
   public class SPLogHandler : LogHandler, ILogFileHandler
   {
      /// <summary>
      /// Constructor - reads the entire trace file into the traceFile array
      /// </summary>
      public SPLogHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.SP, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(parseType, createReader, Factory)
      {
         LogExpression = "*.nwlog";
         Name = "SPLogFileHandler";
      }

      /// <summary>
      /// nwlog traces are not confined to the ATM's [SP] folder: the ActiveTeller software writes its
      /// own *.nwlog next to the ActiveTeller logs (a sibling of the unzipped input folder). Search
      /// the whole work folder so those device traces are picked up too, not just the ones under
      /// SubFolder. GetFiles already recurses AllDirectories, so widening the root is all that's needed.
      /// </summary>
      protected override string LogSearchRoot(IContext ctx)
      {
         return ctx.WorkFolder;
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
            if (c < 128)
            {
               builder.Append(c);

               // generally, '\n' means EOL
               if (c.Equals('\n'))
               {
                  // if the next char after '\n' is a '\t', '{', '(', '<', ' ', '-'  or letter, we are not at EOL.
                  // Guard the lookahead: at the last char (a trailing '\n') traceFilePos+1 is past the buffer;
                  // reading it threw IndexOutOfRange *before* traceFilePos++ ran, and BaseView's while(!EOF())
                  // loop then re-read the same position forever (a hang). Treat past-end as end-of-line.
                  char cNext = (traceFilePos + 1 < logFile.Length) ? logFile[traceFilePos + 1] : '\0';
                  endOfLine = !(cNext == '\r' || cNext == '\t' || cNext == '(' || cNext == '{' || cNext == '<' || cNext == ' ' || cNext == '-' || char.IsLetter(cNext));

                  // if we are at EOL and the next char is a ')' or '}' add it
                  if (endOfLine)
                  {
                     if (cNext == ')' || cNext == '}')
                     {
                        builder.Append(cNext);
                     }
                  }
               }
            }
            traceFilePos++;
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         return SPLine.Factory(this, logLine);
      }
   }
}

