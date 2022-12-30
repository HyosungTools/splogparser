using System;
using System.IO;
using Contract;

namespace Impl
{
    /// <summary>
    /// Implementation of the ILogger interface for write to a standard log line
    /// </summary>
    public class Logger : ILogger
    {
        string logFileName;

        public Logger(IFileSystemProvider ioProvider, string workFolder, string logFileName)
        { 
            this.logFileName = workFolder + "\\" + logFileName + ".log";
            if (ioProvider.Exists(logFileName))
                ioProvider.Delete(logFileName);
        }

        public void WriteLog(string message)
        {
            using (StreamWriter writer = new StreamWriter(logFileName, true))
                writer.WriteLine($"{DateTime.Now} : {message}");
        }
    }
}
