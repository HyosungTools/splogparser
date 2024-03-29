﻿namespace Contract
{
   public interface ILogFileHandler
   {
      IContext ctx { get; set; }
      string Name { get; }
      bool Initialize(IContext ctx);
      string[] FilesFound { get; set; }
      void OpenLogFile(string fileName, int offset = 0);
      string LogFile { get; }
      bool EOF();
      string ReadLine();
      long LineNumber { get; set; }
      ILogLine IdentifyLine(string logLine);

      ParseType parseType { get; }
   }
}
