using System.IO;
using Contract; 

namespace Impl
{
   public class FileReaderFactory
   {
      static public ILogReader CreateLogReader(IContext ctx, string traceFile)
      {
         // If the file extension is *.nwlog, we are creating a trace file reader
         if (traceFile.Contains(".nwlog"))
         {
            return new TraceFileReader(ctx.ioProvider.OpenTextFile(traceFile), 28);
         }

         return (ILogReader)null;
      }
   }
}
