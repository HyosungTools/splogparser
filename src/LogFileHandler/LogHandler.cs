using System.IO;
using Contract; 

namespace LogFileHandler
{
    public class LogHandler
    {
      protected IContext ctx;
      protected ICreateStreamReader createReader;

      public ParseType parseType { get; }

      // My Name (for logging purposes)
      public string Name { get; set; }

      // Log File
      public string LogFile
      {
         get
         {
            return ctx.ioProvider.GetFileName(fileName);
         }
      }

      // all files
      public string[] FilesFound { get; set; }

      // file name
      public string fileName; 

      // entire log file
      protected char[] logFile;

      // pointer into logfile
      protected int traceFilePos = 0;

      protected string LogExpression;

      public LogHandler(ParseType parseType, ICreateStreamReader createReader)
      {
         this.parseType = parseType; 
         this.createReader = createReader; 
      }

      public bool Initialize(IContext ctx)
      {
         // find all files
         this.ctx = ctx; 
         FilesFound = ctx.ioProvider.GetFiles(ctx.WorkFolder + "\\" + ctx.SubFolder, LogExpression);
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
