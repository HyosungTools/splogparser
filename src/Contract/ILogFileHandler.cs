using System.IO;

namespace Contract
{
   public interface ILogFileHandler
   {
      string Name { get; }
      bool Initialize(IContext ctx);
      string[] FilesFound { get; set; }
      void OpenLogFile(string fileName, int offset = 0);
      string LogFile { get; }
      bool EOF();
      string ReadLine();
      ILogLine IdentifyLine(string logLine);

      string ParseType { get; }
   }
}
