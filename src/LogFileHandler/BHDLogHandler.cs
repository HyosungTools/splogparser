using System;
using System.Text;
using Contract;
using LogLineHandler; 

namespace LogFileHandler
{
   /// <summary>
   /// Read BeeHD files (rvbeehd*.*)
   /// </summary>
   public class BHDLogHandler : LogHandler, ILogFileHandler
   {
      public BHDLogHandler(ICreateStreamReader createReader) : base(ParseType.AT, createReader)
      {
         LogExpression = "rvbeehd*.*";
         Name = "BHDLogFileHandler";
      }

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
                     // if the next char after '\n' is between 'A' and 'Z' we are at the EOL
                     char cNext = logFile[traceFilePos + 1];
                     endOfLine = (cNext >= 'A' && cNext <= 'Z') || (cNext >= '0' && cNext <= '9');
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
         return new BHDLine(this, logLine); 
      }

   }
}
