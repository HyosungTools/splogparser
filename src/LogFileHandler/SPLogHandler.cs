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
      public SPLogHandler(ICreateStreamReader createReader, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(ParseType.SP, createReader, Factory)
      {
         LogExpression = "*.nwlog";
         Name = "SPLogFileHandler";
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
                  // if the next char after '\n' is a '\t', '{', '(', '<', ' ', '-'  or letter, we are not at EOL
                  char cNext = logFile[traceFilePos + 1];
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

