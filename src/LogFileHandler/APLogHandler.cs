﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class APLogHandler : LogHandler, ILogFileHandler
   {
      public string ParseType { get; }

      public APLogHandler(ICreateStreamReader createReader) : base(createReader)
      {
         ParseType = "AP";
         LogExpression = "APLog*.*";
         Name = "APLogFileHandler";
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
                  try
                  {
                     // OLD: if the next char after '\n' is a '['  we are at EOL
                     char cNext = logFile[traceFilePos + 1];
                     endOfLine = (cNext == '[');

                     if (!endOfLine)
                     {
                        // NEW: look for '  INFO [' or '  WARN [' or ' ERROR ['
                        char c1 = logFile[traceFilePos + 1];
                        char c2 = logFile[traceFilePos + 2];
                        char c3 = logFile[traceFilePos + 3];
                        char c4 = logFile[traceFilePos + 4];
                        char c5 = logFile[traceFilePos + 5];
                        char c6 = logFile[traceFilePos + 6];
                        char c7 = logFile[traceFilePos + 7];
                        char c8 = logFile[traceFilePos + 8];

                        endOfLine =
                           (
                              ((c1 == ' ') && (c2 == ' ') && (c3 == 'I') && (c4 == 'N') && (c5 == 'F') && (c6 == 'O') && (c7 == ' ') && (c8 == '[')) ||
                              ((c1 == ' ') && (c2 == ' ') && (c3 == 'W') && (c4 == 'A') && (c5 == 'R') && (c6 == 'N') && (c7 == ' ') && (c8 == '[')) ||
                              ((c1 == ' ') && (c2 == 'E') && (c3 == 'R') && (c4 == 'R') && (c5 == 'O') && (c6 == 'R') && (c7 == ' ') && (c8 == '['))
                           );
                     }
                  }
                  catch (Exception e)
                  {
                     Console.WriteLine(String.Format("Exception {0}", e.Message));
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
         return new APLine(this, logLine);
      }
   }
}