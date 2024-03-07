using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Read BeeHD files (rvbeehd*.*)
   /// </summary>
   public class BELogHandler : LogHandler, ILogFileHandler
   {
      public BELogHandler(ICreateStreamReader createReader) : base(ParseType.BE, createReader)
      {
         LogExpression = "rvbeehd*.*";    // .log or .txt
         Name = "BELogFileHandler";
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
                  // if the next char after '\n' is between 'A' and 'Z' we are at the EOL

                  if (logFile.Length > (traceFilePos + 1))
                  {
                     char cNext = logFile[traceFilePos + 1];
                     endOfLine = (cNext >= 'A' && cNext <= 'Z') || (cNext >= '0' && cNext <= '9');
                  }
                  else
                  {
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
         // if we get here we are only dealing with BHD Log Lines
         return new LogLineHandler.BELine(this, logLine);
      }



      public string LogSummary()
      {
         return $"** LOGSUMMARY ** Parser {ParseType.BE}";

         //return $"** LOGSUMMARY ** Parser {ParseType.BE}, Filter {LogExpression}, File {fileName}, LineCount {LinesCount}, Version {InputLogVersion}, Source {FileSource}, From {FirstTimestamp}, To {LastTimestamp}";
      }

      /// <summary>
      /// a list of SIP Session Summaries
      /// </summary>
      /// <returns></returns>
      public List<string> SessionSummaries()
      {
         return BELine.SipSessionSummaries();
      }

      /// <summary>
      /// a list tables describing SIP Session events
      /// </summary>
      /// <returns></returns>
      public List<DataTable> SessionTables()
      {
         List<DataTable> sessionTables = BELine.SipSessionTables();

         return sessionTables;
      }

      /// <summary>
      /// Release the session objects and the memory they occupy.
      /// </summary>
      public void ReleaseSessionTables()
      {
         BELine.ReleaseSipSessionTables();
      }

   }
}

