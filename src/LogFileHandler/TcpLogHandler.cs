using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads MoniView TCP trace captures (TcpTrace_*.txt) one message block at a time. A block runs
   /// from a "[timestamp]" line up to (but not including) the next "[" line, i.e. the timestamp
   /// header, the direction/IP/Len header, and the hex-dump rows for a single message.
   /// </summary>
   public class TcpLogHandler : LogHandler, ILogFileHandler
   {
      public TcpLogHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.MV, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(parseType, createReader, Factory)
      {
         LogExpression = "TcpTrace_*.txt";
         Name = "TcpLogFileHandler";
      }

      public bool EOF()
      {
         return traceFilePos >= logFile.Length;
      }

      /// <summary>
      /// Read one message block. End-of-block is the point where the next line begins with '[',
      /// which is the start of the following message's timestamp line.
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
                     char next = logFile[traceFilePos + 1];
                     endOfLine = (next == '[');
                  }
                  catch (Exception)
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
         return Factory(this, logLine);
      }
   }
}
