using System.IO;
using System.Text;

namespace Impl
{
   /// <summary>
   /// Reads Trace files (.nwlog) one line at a time
   /// </summary>
   public class TraceFileReader
   {
      // entire trace file
      char[] traceFile;

      // pointer into the traceFile
      int traceFilePos = 0;

      /// <summary>
      /// Constructor - reads the entire trace file into the traceFile array
      /// </summary>
      /// <param name="trace">StreamReader pointing to the trace file to read</param>
      /// <param name="offset">Starting offset for log file parsing (default 0)</param>
      public TraceFileReader(StreamReader trace, int offset = 0)
      {
         traceFile = new char[trace.BaseStream.Length];
         trace.Read(traceFile, 0, (int)trace.BaseStream.Length);
         trace.Close();
         traceFilePos = offset;
      }

      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
      {
         return traceFilePos >= traceFile.Length;
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
            char c = traceFile[traceFilePos];
            if (c < 128)
            {
               builder.Append(c);

               // generally, '\n' means EOL
               if (c.Equals('\n'))
               {
                  // if the next char after '\n' is a '\t', '{', '(', '<', ' ', '-'  or letter, we are not at EOL
                  char cNext = traceFile[traceFilePos + 1];
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
   }
}

