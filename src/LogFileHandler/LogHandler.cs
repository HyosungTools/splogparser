using System;
using System.IO;
using Contract;

namespace LogFileHandler
{
   public class LogHandler
   {
      // Factory method for creating LogLines
      protected Func<ILogFileHandler, string, ILogLine> Factory;

      public IContext ctx { get; set; }
      protected ICreateStreamReader createReader;

      public ParseType parseType { get; }

      // My Name (for logging purposes)
      public string Name { get; set; }

      // Log File
      public string LogFile
      {
         get
         {
            if (ctx?.ioProvider != null)
               return ctx.ioProvider.GetFileName(fileName);
            return Path.GetFileName(fileName);  // Fallback when ctx is not set
         }
      }

      public long LineNumber { get; set; }

      // all files
      public string[] FilesFound { get; set; }

      // file name
      public string fileName;

      // entire log file
      protected char[] logFile;

      // pointer into logfile
      protected int traceFilePos = 0;

      protected string LogExpression;

      // correction of the timestamps in the log file to UTC time
      protected TimeSpan UtcTimeOffset { get; set; } = TimeSpan.Zero;


      public LogHandler(ParseType parseType, ICreateStreamReader createReader, Func<ILogFileHandler, string, ILogLine> Factory)
      {
         this.parseType = parseType;
         this.createReader = createReader;
         this.Factory = Factory;
      }

      /// <summary>
      /// Root folder under which Initialize() searches (recursively) for this handler's log files.
      /// Defaults to the unzipped input folder (WorkFolder\SubFolder). A handler whose files can live
      /// OUTSIDE that folder overrides this to widen the search - e.g. the *.nwlog trace the
      /// ActiveTeller software writes alongside its own logs, which is not inside the ATM's [SP] folder.
      /// </summary>
      protected virtual string LogSearchRoot(IContext ctx)
      {
         return ctx.WorkFolder + "\\" + ctx.SubFolder;
      }

      // virtual so a handler can replace file discovery wholesale (e.g. ATNwLogHandler). Most handlers
      // only need to widen WHERE it searches - override LogSearchRoot for that instead of Initialize.
      public virtual bool Initialize(IContext ctx)
      {
         // find all files (GetFiles recurses AllDirectories under the search root)
         this.ctx = ctx;
         FilesFound = ctx.ioProvider.GetFiles(LogSearchRoot(ctx), LogExpression);
         return FilesFound.Length > 0;
      }

      public virtual void OpenLogFile(string fileName, int offset = 0)
      {
         this.fileName = fileName;
         StreamReader reader = createReader.Create(fileName);
         logFile = new char[reader.BaseStream.Length];
         reader.Read(logFile, 0, (int)reader.BaseStream.Length);
         reader.Close();
         traceFilePos = offset;
      }
   }
}
