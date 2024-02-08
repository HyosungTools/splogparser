using System;
using System.Linq;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class AELogHandler : LogHandler, ILogFileHandler
   {
      public AELogHandler(ICreateStreamReader createReader) : base(ParseType.AE, createReader)
      {
         LogExpression = "ActiveTellerAgentExtensions_*.*";
         Name = "AELogFileHandler";
      }

      /// <summary>
      /// EOF test
      /// </summary>
      /// <returns>true if read to EOF; false otherwise</returns>
      public bool EOF()
      {
         return traceFilePos >= logFile.Length;
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
            //2023-11-17 03:01:58 [NextwareExtension] The 'NextwareExtension' extension is started.

            char c = logFile[traceFilePos];
            traceFilePos++;

            // check for end of line or end of file
            if (c == '\n' || EOF())
            {
               endOfLine = true;

               if (c == '\n')
               {
                  break;
               }
            }

            // ignore nulls and non-printing ASCII characters
            if (c > 0 && c < 128 && c != '\r')
            {
               builder.Append(c);
            }
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         // (bool success, string apLogLine) result;

         /* APLOG_INSTALL */
         //if (logLine.StartsWith("=======") && logLine.EndsWith("========\r\n"))
         //   return new APLine(this, logLine, APLogType.APLOG_INSTALL);

         /* AddKey */
         //if (logLine.Contains("[AbstractConfigHandler") && logLine.Contains("AddMoniplusData") && logLine.Contains("Add Key="))
         //   return new AddKey(this, logLine);

         /* CASHDISP */
         //if (logLine.Contains("[CashDispenser"))
         //{
         //   ILogLine iLine = CashDispenser.Factory(this, logLine);
         //   if (iLine != null) return iLine;
         //}

         ///* NDC */
         //if (logLine.Contains("[RecvProcAsync") && logLine.Contains("HOST2ATM:"))
         //{
         //   ILogLine iLine = Host2Atm.Factory(this, logLine);
         //   if (iLine != null) return iLine;
         //}

         //if (logLine.Contains("[RecvProcAsync") && logLine.Contains("ATM2HOST:"))
         //{
         //   ILogLine iLine = Atm2Host.Factory(this, logLine);
         //   if (iLine != null) return iLine;
         //}

         /* CORE */
         //if (logLine.Contains("WebServiceRequestFlowPoint"))
         //{
         //   ILogLine iLine = Core.Factory(this, logLine);
         //   if (iLine != null) return iLine;
         //}

         /* EJ */
         //if (logLine.Contains("INSERT INTO "))
         //   return new EJInsert(this, logLine);

         /* HELPER FUNCTIONS */
         //if (logLine.Contains("[HelperFunctions"))
         //{
         //   ILogLine iLine = HelperFunctions.Factory(this, logLine);
         //   if (iLine != null) return iLine;
         //}

         //2023-11-17 03:00:22 [MoniPlus2sExtension] The 'MoniPlus2sExtension' extension is started.
         if (logLine.Contains("extension is started"))
         {
            return new ExtensionStarted(this, logLine, AELogType.ExtensionStarted);
         }

         //2023-11-17 03:00:22 [NetOpExtension] The 'NetOpExtension' extension is started.
         if (logLine.Contains("[NetOpExtension]"))
         {
            return new NetOpExtension(this, logLine, AELogType.NetOpExtension);
         }

         //2023-11-17 03:01:58 [NextwareExtension] The 'NextwareExtension' extension is started.
         if (logLine.Contains("[NextwareExtension]"))
         {
            return new NextwareExtension(this, logLine, AELogType.NextwareExtension);
         }

         //2023-11-17 03:00:22 [MoniPlus2sExtension] The 'MoniPlus2sExtension' extension is started.
         if (logLine.Contains("[MoniPlus2sExtension]"))
         {
            return new MoniPlus2sExtension(this, logLine, AELogType.MoniPlus2sExtension);
         }

         return new AELine(this, logLine, AELogType.None);
      }
   }
}
