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
      char[] result;

      // pointer into the results array
      int resultPos = 0;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="trace">StreamReader pointing to the trace file to read</param>
      /// <param name="offset">Starting offset for log file parsing (default 0)</param>
      public TraceFileReader(StreamReader trace, int offset = 0)
      {
         result = new char[trace.BaseStream.Length];
         trace.Read(result, 0, (int)trace.BaseStream.Length);
         trace.Close();
         resultPos = offset;
      }

      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
      {
         return resultPos >= result.Length;
      }

      /// <summary>
      /// Read one log line from a twlog file. 
      /// </summary>
      /// <returns></returns>
      public string ReadLine()
      {
         StringBuilder builder = new StringBuilder();

         bool endOfLine = false;

         // while not EOL or EOF
         while (!endOfLine && !EOF())
         {
            char c = result[resultPos];
            if (c < 128)
            {
               builder.Append(c);

               if (c.Equals('\n'))
               {
                  // if the next char after '\n' is a '\t', '{' or '(' we are not at EOL
                  char cNext = result[resultPos + 1];
                  endOfLine = !(cNext == '\t' || cNext == '(' || cNext == '{');

                  // if we are at EOL and the next char was a ')' or '}' add it
                  if (endOfLine)
                  {
                     if (cNext == ')' || cNext == '}')
                     {
                        builder.Append(cNext);
                     }
                  }
               }
            }
            resultPos++;
         }

         return builder.ToString();
      }
   }
}

