using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class SSLogHandler : LogHandler, ILogFileHandler
   {
      public SSLogHandler(ICreateStreamReader createReader) : base(ParseType.SS, createReader)
      {
         LogExpression = "settlement-api-all-*.log";
         Name = "SettlementServer"; 
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
                  endOfLine = true; 
               }
            }
            traceFilePos++;
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         if (logLine.Contains("Settlement.API.Controllers.JournalsController") || logLine.Contains("JournalImporter.JournalImporter"))
         {
            //ctx.ConsoleWriteLogLine(String.Format("Found line: {0}", logLine));
            ILogLine iLine = SSLine.Factory(this, logLine);
            if (iLine != null) return iLine;
         }

         return new SSLine(this, logLine, SSLogType.None);
      }
   }
}

