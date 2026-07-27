using System;
using System.IO;
using Contract;

namespace Impl
{
   public class Logger : ILogger, IDisposable
   {
      private readonly string logFileName;
      private readonly StreamWriter writer;

      public Logger(IFileSystemProvider ioProvider, string workFolder, string logFileName)
      {
         this.logFileName = workFolder + "\\" + logFileName + ".log";
         if (ioProvider.Exists(this.logFileName))
            ioProvider.Delete(this.logFileName);

         // open ONCE and keep it open. AutoFlush keeps per-line durability
         // (you still see the log if the run crashes) without reopening the file every call.
         writer = new StreamWriter(this.logFileName, append: true) { AutoFlush = true };
      }

      public void WriteLog(string message)
      {
         writer.WriteLine($"{DateTime.Now} : {message}");
      }

      public void Dispose()
      {
         writer?.Flush();
         writer?.Dispose();
      }
   }
}
