using System;
using System.Text;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads application logs 
   /// </summary>
   public class AWLogHandler : LogHandler, ILogFileHandler
   {
      public AWLogHandler(ICreateStreamReader createReader) : base(ParseType.AW, createReader)
      {
         LogExpression = "Workstation*.*";
         Name = "AWLogFileHandler";
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
            // - Software Name    = C:\Program Files (x86)\Nautilus Hyosung\ActiveTeller\Workstation\NH.ActiveTeller.Client.exe
            // - Version          = 1.3.0.0
            // - File Description = ActiveTeller Workstation
            //==========================================================================================================
            //[2023-09-13 10:02:12-399][3][Settings            ]UserSettings Path: C:\Users\lpina\AppData\Local\Nautilus_Hyosung\NH.ActiveTeller.Client.ex_Url_qnalorbjt4q0zmjb1ac1ptpbvvqeo5na\1.3.0.0\user.config

            char c = logFile[traceFilePos];
            traceFilePos++;

            if (c < 128)
            {
               builder.Append(c);

               // generally, '\n' means EOL
               if (c.Equals('\n'))
               {
                  // if the next char after '\n' is a '\t', '{', '(', '<', ' ', '-'  or letter, we are not at EOL
                  // handle empty line at the end of the file
                  char cNext = !EOF() ? logFile[traceFilePos] : '\n';
                  endOfLine = !(cNext == '\r' || cNext == '\t' || cNext == '(' || cNext == '{' || cNext == '<' || cNext == ' ' || cNext == '-' || char.IsLetter(cNext));

                  // if we are at EOL and the next char is a ')' or '}' add it
                  if (endOfLine)
                  {
                     if (cNext == ')' || cNext == '}')
                     {
                        builder.Append(cNext);
                     }
                  }
               }
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

         return new AWLine(this, logLine, AWLogType.None);
      }
   }
}
