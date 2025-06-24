using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   public class RTLogHandler : LogHandler, ILogFileHandler
   {
      // Hide the base class member
      string[] logFileLines;
      int logLineIndex = 0; 

      /// <summary>
      /// Constructor - reads the entire trace file into the traceFile array
      /// </summary>
      public RTLogHandler(ICreateStreamReader createReader, ParseType parseType = ParseType.RT, Func<ILogFileHandler, string, ILogLine> Factory = null) : base(ParseType.RT, createReader, Factory)
      {
         LogExpression = "JNL*.dat";
         Name = "RTLogFileHandler";
      }

      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
      {
         this.ctx.ConsoleWriteLogLine(String.Format("RTLogHandler.EOF: logLineIndex : {0} logFile.Length {1}", logLineIndex, logFile.Length));
         return logLineIndex >= logFileLines.Length;
      }

      public override void OpenLogFile(string fileName, int offset = 0)
      {
         this.ctx.ConsoleWriteLogLine(String.Format("LogHandler.OpenLogFile: filename : {0}", fileName));
         this.fileName = fileName;
         StreamReader reader = createReader.Create(fileName);
         string logFile = reader.ReadToEnd();
         this.ctx.ConsoleWriteLogLine(String.Format("LogHandler.OpenLogFile: filesize : {0} logfile size: {1}", reader.BaseStream.Length, logFile.Length));

         reader.Close();
         traceFilePos = offset;

         // Split the file into lines
         logFileLines = logFile.Split('\x1C'); // or '\u001C'
         this.ctx.ConsoleWriteLogLine(String.Format("LogHandler.OpenLogFile: number of lines read : {0}", logFileLines.Length));

      }

      /// <summary>
      /// Read one log line from a twlog file. 
      /// </summary>
      /// <returns></returns>
      public string ReadLine()
      {
         string returnLine = "Hello World";
         //this.ctx.ConsoleWriteLogLine(String.Format("RTLogHandler.ReadLine : logLineIndex : {0} logFileLines.Length : {1}", logLineIndex, logFileLines.Length));
         //if (logLineIndex < logFileLines.Length)
         //{
         //   returnLine = logFileLines[logLineIndex]; 
         //}
         logLineIndex++;
         return returnLine; 
      }
      public ILogLine IdentifyLine(string logLine)
      {
         this.ctx.ConsoleWriteLogLine(String.Format("RTLogHandler.IdentifyLine : logLine : {0}", logLine));
         return RTLine.Factory(this, logLine);
      }
   }
}
