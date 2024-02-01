using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class APLogHandler : LogHandler, ILogFileHandler
   {
      public APLogHandler(ICreateStreamReader createReader) : base(ParseType.AP, createReader)
      {
         LogExpression = "APLog*.*";
         Name = "APLogFileHandler";
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
            //==========================================================================================================
            // - Machine Number           = A036201
            //==========================================================================================================
            //Installed Programs:
            // - A2iA CheckReader V9.0, Installed: 07/26/2023, Version: 9.0
            //==========================================================================================================
            //Installed Packages:
            // - Basic Media-V02.01.02.00, Installed: Thu 05/19/2022
            //==========================================================================================================
            //[2023-11-16 00:00:29-315][3][][BeehdControl        ][<BeehdClient_OnUserNotify>b__0][NORMAL]BeehdClient_OnUserNotify=>callHandle=0, val=4001, severity=SEVERITY_INFO, userType=NOTIFIER_USER_TYPE_ALL,description=,additionalInfo=, suggestedAction= 
            //[2023-11-16 00:16:13-749][3][][Archiving           ][CreateArchive       ][NORMAL]CreateArchive(): About to run, runNow=False
            //[2023-11-16 00:16:13-752][3][][Management          ][CopyLogFile         ][NORMAL]Parameter pStartDateTime: 11/15/2023 12:16:13 AM

            char c = logFile[traceFilePos];
            if (c < 128)
            {
               builder.Append(c);

               // generally, '\n' means EOL
               if (c.Equals('\n'))
               {
                  try
                  {
                     // OLD: if the next char after '\n' is a '['  we are at EOL
                     char cNext = logFile[traceFilePos + 1];
                     endOfLine = (cNext == '[');

                     if (!endOfLine)
                     {
                        // NEW: look for '  INFO [' or '  WARN [' or ' ERROR ['
                        char c1 = logFile[traceFilePos + 1];
                        char c2 = logFile[traceFilePos + 2];
                        char c3 = logFile[traceFilePos + 3];
                        char c4 = logFile[traceFilePos + 4];
                        char c5 = logFile[traceFilePos + 5];
                        char c6 = logFile[traceFilePos + 6];
                        char c7 = logFile[traceFilePos + 7];
                        char c8 = logFile[traceFilePos + 8];

                        endOfLine =
                           (
                              ((c1 == ' ') && (c2 == ' ') && (c3 == 'I') && (c4 == 'N') && (c5 == 'F') && (c6 == 'O') && (c7 == ' ') && (c8 == '[')) ||
                              ((c1 == ' ') && (c2 == ' ') && (c3 == 'W') && (c4 == 'A') && (c5 == 'R') && (c6 == 'N') && (c7 == ' ') && (c8 == '[')) ||
                              ((c1 == ' ') && (c2 == 'E') && (c3 == 'R') && (c4 == 'R') && (c5 == 'O') && (c6 == 'R') && (c7 == ' ') && (c8 == '['))
                           );
                     }
                  }
                  catch (Exception e)
                  {
                     Console.WriteLine(String.Format("Exception {0}", e.Message));
                     endOfLine = true;
                  }
               }
            }
            traceFilePos++;
         }

         return builder.ToString();
      }

      public ILogLine IdentifyLine(string logLine)
      {
         // (bool success, string apLogLine) result;

         /* APLOG_INSTALL */
         if (logLine.StartsWith("=======") && logLine.EndsWith("========\r\n"))
            return new APLine(this, logLine, APLogType.APLOG_INSTALL);

         /* AddKey */
         if (logLine.Contains("[AbstractConfigHandler") && logLine.Contains("AddMoniplusData") && logLine.Contains("Add Key="))
            return new AddKey(this, logLine);

         /* CASHDISP */
         if (logLine.Contains("[CashDispenser"))
         {
            ILogLine iLine = CashDispenser.Factory(this, logLine);
            if (iLine != null) return iLine;
         }

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
         if (logLine.Contains("WebServiceRequestFlowPoint"))
         {
            ILogLine iLine = Core.Factory(this, logLine);
            if (iLine != null) return iLine;
         }

         /* EJ */
         if (logLine.Contains("INSERT INTO "))
            return new EJInsert(this, logLine);

         /* HELPER FUNCTIONS */
         if (logLine.Contains("[HelperFunctions"))
         {
            ILogLine iLine = HelperFunctions.Factory(this, logLine);
            if (iLine != null) return iLine;
         }

         return new APLine(this, logLine, APLogType.None);
      }
   }
}
