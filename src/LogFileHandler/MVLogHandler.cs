using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads MoniView server logs (MoniViewServerLog*.txt) one logical line at a time.
   /// A logical line is a timestamped line plus any following continuation lines (SQL text,
   /// stack traces) that do not themselves start with a yyyy-MM-dd timestamp.
   /// </summary>
   public class MVLogHandler : LogHandler, ILogFileHandler
   {
      public MVLogHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.MV, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(parseType, createReader, Factory)
      {
         LogExpression = "MoniViewServerLog*.txt";
         Name = "MVLogFileHandler";
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
      /// Read one logical line. End-of-line is the point where the NEXT line begins with a
      /// yyyy-MM-dd timestamp; anything else after a newline is a continuation of this line.
      /// </summary>
      public string ReadLine()
      {
         StringBuilder builder = new StringBuilder();

         bool endOfLine = false;

         while (!endOfLine && !EOF())
         {
            char c = ' ';
            try
            {
               c = logFile[traceFilePos];
            }
            catch (Exception e)
            {
               this.ctx.ConsoleWriteLogLine(String.Format("Exception {0} - traceFilePos = {1} logFile.Length = {2}", e.Message, traceFilePos, logFile.Length));
            }

            if (c < 128)
            {
               builder.Append(c);

               if (c.Equals('\n'))
               {
                  try
                  {
                     // Peek the start of the next line. A new logical line begins with a date,
                     // e.g. 2026-03-20 -> digit,digit,digit,digit,'-'. Continuation lines
                     // (INSERT INTO..., '   at System...', 'Full Trace:') do not.
                     char d0 = logFile[traceFilePos + 1];
                     char d1 = logFile[traceFilePos + 2];
                     char d2 = logFile[traceFilePos + 3];
                     char d3 = logFile[traceFilePos + 4];
                     char dash = logFile[traceFilePos + 5];

                     endOfLine =
                        (
                           char.IsDigit(d0) && char.IsDigit(d1) && char.IsDigit(d2) && char.IsDigit(d3) && (dash == '-')
                        );
                  }
                  catch (Exception)
                  {
                     // ran off the end of the buffer -> this was the last line
                     endOfLine = true;
                  }
               }
            }

            traceFilePos++;
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         return Factory(this, logLine);
      }
   }
}
