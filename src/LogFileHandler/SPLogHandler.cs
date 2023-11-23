using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Contract;
using LogLineHandler;

namespace LogFileHandler
{
   /// <summary>
   /// Reads Trace files (.nwlog) one line at a time
   /// </summary>
   public class SPLogHandler : LogHandler, ILogFileHandler
   {
      /* 1 - PTR */
      /* INFO */
      static Regex WFS_INF_PTR_STATUS = new Regex("GETINFO.101.[0-9]+WFS_GETINFO_COMPLETE");

      /* 2 - IDC */
      /* INFO */
      static Regex WFS_INF_IDC_STATUS = new Regex("GETINFO.201.[0-9]+WFS_GETINFO_COMPLETE");

      /* 3 - CDM */
      /* INFO */
      static Regex WFS_INF_CDM_STATUS = new Regex("GETINFO.301.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_CDM_CASH_UNIT_INFO = new Regex("GETINFO.303.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_CDM_PRESENT_STATUS = new Regex("GETINFO.309.[0-9]+WFS_GETINFO_COMPLETE");

      /* EXECUTE */
      static Regex WFS_CMD_CDM_DISPENSE = new Regex("EXECUTE.302.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CDM_PRESENT = new Regex("EXECUTE.303.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CDM_REJECT = new Regex("EXECUTE.304.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CDM_RETRACT = new Regex("EXECUTE.305.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CDM_STARTEX = new Regex("EXECUTE.311.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CDM_ENDEX = new Regex("EXECUTE.312.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CDM_RESET = new Regex("EXECUTE.321.[0-9]+WFS_EXECUTE_COMPLETE");

      /* EVENTS */
      static Regex WFS_SRVE_CDM_CASHUNITINFOCHANGED = new Regex("SERVICE_EVENT.304.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_CDM_ITEMSTAKEN = new Regex("SERVICE_EVENT.309.[0-9]+WFS_SERVICE_EVENT");

      /* 4 - PIN */
      /* INFO */
      static Regex WFS_INF_PIN_STATUS = new Regex("GETINFO.401.[0-9]+WFS_GETINFO_COMPLETE");

      /* 5 - CHK */
      /* INFO */
      static Regex WFS_INF_CHK_STATUS = new Regex("GETINFO.501.[0-9]+WFS_GETINFO_COMPLETE");

      /* 6 - DEP */
      /* INFO */
      static Regex WFS_INF_DEP_STATUS = new Regex("GETINFO.601.[0-9]+WFS_GETINFO_COMPLETE");

      /* 7 - TTU */
      /* INFO */
      static Regex WFS_INF_TTU_STATUS = new Regex("GETINFO.701.[0-9]+WFS_GETINFO_COMPLETE");

      /* 8 - SIU */
      /* INFO */
      Regex WFS_INF_SIU_STATUS = new Regex("GETINFO.801.[0-9]+WFS_GETINFO_COMPLETE");

      /* 9 - VDM */
      /* INFO */
      static Regex WFS_INF_VDM_STATUS = new Regex("GETINFO.901.[0-9]+WFS_GETINFO_COMPLETE");

      /* 10 - CAM */
      /* INFO */
      static Regex WFS_INF_CAM_STATUS = new Regex("GETINFO.1001.[0-9]+WFS_GETINFO_COMPLETE");

      /* 11 - ALM */
      /* INFO */
      static Regex WFS_INF_ALM_STATUS = new Regex("GETINFO.1101.[0-9]+WFS_GETINFO_COMPLETE");

      /* 12 - CEU */
      /* INFO */
      static Regex WFS_INF_CEU_STATUS = new Regex("GETINFO.1201.[0-9]+WFS_GETINFO_COMPLETE");

      /* 13 - CIM */
      /* INFO */
      static Regex WFS_INF_CIM_STATUS = new Regex("GETINFO.1301.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_CIM_CASH_UNIT_INFO = new Regex("GETINFO.1303.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_CIM_CASH_IN_STATUS = new Regex("GETINFO.1307.[0-9]+WFS_GETINFO_COMPLETE");

      /* EXECUTE */
      static Regex WFS_CMD_CIM_CASH_IN_START = new Regex("EXECUTE.1301.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CIM_CASH_IN = new Regex("EXECUTE.1302.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CIM_CASH_IN_END = new Regex("EXECUTE.1303.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CIM_CASH_IN_ROLLBACK = new Regex("EXECUTE.1304.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CIM_RETRACT = new Regex("EXECUTE.1305.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CIM_START_EXCHANGE = new Regex("EXECUTE.1310.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CIM_END_EXCHANGE = new Regex("EXECUTE.1311.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_CIM_RESET = new Regex("EXECUTE.1313.[0-9]+WFS_EXECUTE_COMPLETE");

      /* Events */
      //Regex WFS_SRVE_CIM_SAFEDOOROPEN = new Regex("SERVICE_EVENT.1301.[0-9]+WFS_SERVICE_EVENT");
      //Regex WFS_SRVE_CIM_SAFEDOORCLOSED = new Regex("SERVICE_EVENT.1302.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_USRE_CIM_CASHUNITTHRESHOLD = new Regex("USER_EVENT.1303.[0-9]+WFS_USER_EVENT");
      static Regex WFS_SRVE_CIM_CASHUNITINFOCHANGED = new Regex("SERVICE_EVENT.1304.[0-9]+WFS_SERVICE_EVENT");
      //Regex WFS_EXEE_CIM_CASHUNITERROR = new Regex("EXECUTE_EVENT.1306.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_CIM_ITEMSTAKEN = new Regex("SERVICE_EVENT.1307.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_EXEE_CIM_INPUTREFUSE = new Regex("EXECUTE_EVENT.1309.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_CIM_ITEMSPRESENTED = new Regex("SERVICE_EVENT.1310.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_CIM_ITEMSINSERTED = new Regex("SERVICE_EVENT.1311.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_EXEE_CIM_NOTEERROR = new Regex("EXECUTE_EVENT.1312.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_CIM_MEDIADETECTED = new Regex("SERVICE_EVENT.1314.[0-9]+WFS_SERVICE_EVENT");

      /* 14 - CRD */
      /* INFO */
      static Regex WFS_INF_CRD_STATUS = new Regex("GETINFO.1401.[0-9]+WFS_GETINFO_COMPLETE");

      /* 15 - BCR */
      /* INFO */
      static Regex WFS_INF_BCR_STATUS = new Regex("GETINFO.1501.[0-9]+WFS_GETINFO_COMPLETE");

      /* 16 - IPM */
      /* INFO */
      static Regex WFS_INF_IPM_STATUS = new Regex("GETINFO.1601.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_IPM_MEDIA_BIN_INFO = new Regex("GETINFO.1604.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_IPM_TRANSACTION_STATUS = new Regex("GETINFO.1605.[0-9]+WFS_GETINFO_COMPLETE");

      /* EXECUTE */
      static Regex WFS_CMD_IPM_MEDIA_IN = new Regex("EXECUTE.1601.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IPM_MEDIA_IN_END = new Regex("EXECUTE.1602.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IPM_MEDIA_IN_ROLLBACK = new Regex("EXECUTE.1603.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IPM_PRESENT_MEDIA = new Regex("EXECUTE.1606.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IPM_RETRACT_MEDIA = new Regex("EXECUTE.1607.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IPM_PRINT_TEXT = new Regex("EXECUTE.1608.[0-9]+WFS_EXECUTE_COMPLETE");

      static Regex WFS_CMD_IPM_RESET = new Regex("EXECUTE.1610.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IPM_EXPEL_MEDIA = new Regex("EXECUTE.1614.[0-9]+WFS_EXECUTE_COMPLETE");

      /* Events */
      static Regex WFS_EXEE_IPM_MEDIAINSERTED = new Regex("EXECUTE_EVENT.1602.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_USRE_IPM_MEDIABINTHRESHOLD = new Regex("USER_EVENT.1603.[0-9]+WFS_USER_EVENT");
      static Regex WFS_SRVE_IPM_MEDIABININFOCHANGED = new Regex("SERVICE_EVENT.1604.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_EXEE_IPM_MEDIABINERROR = new Regex("EXECUTE_EVENT.1605.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_IPM_MEDIATAKEN = new Regex("SERVICE_EVENT.1606.[0-9]+WFS_SERVICE_EVENT");

      static Regex WFS_SRVE_IPM_MEDIADETECTED = new Regex("SERVICE_EVENT.1610.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_EXEE_IPM_MEDIAPRESENTED = new Regex("EXECUTE_EVENT.1611.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_EXEE_IPM_MEDIAREFUSED = new Regex("EXECUTE_EVENT.1612.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_EXEE_IPM_MEDIAREJECTED = new Regex("EXECUTE_EVENT.1615.[0-9]+WFS_EXECUTE_EVENT");

      static Regex WFPOpen = new Regex("(XFS_CMD[a-zA-Z0-9 ]*)(OPEN[a-zA-Z0-9 ]*)(hResult\\[(\\d+)\\] = WFPOpen)");
      static Regex WFPClose = new Regex("(XFS_CMD[a-zA-Z0-9 ]*)(CLOSE[a-zA-Z0-9 ]*)(hResult\\[(\\d+)\\] = WFPClose)");


      public string ParseType { get; }

      /// <summary>
      /// Constructor - reads the entire trace file into the traceFile array
      /// </summary>
      public SPLogHandler(ICreateStreamReader createReader) : base(createReader)
      {
         ParseType = "SP";
         LogExpression = "*.nwlog";
         Name = "SPLogFileHandler";
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
            char c = logFile[traceFilePos];
            if (c < 128)
            {
               builder.Append(c);

               // generally, '\n' means EOL
               if (c.Equals('\n'))
               {
                  // if the next char after '\n' is a '\t', '{', '(', '<', ' ', '-'  or letter, we are not at EOL
                  char cNext = logFile[traceFilePos + 1];
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
            traceFilePos++;
         }

         return builder.ToString();
      }

      private (bool success, string xfsLine) GenericMatch(Regex regEx, string logLine)
      {
         string matchLine = logLine;
         Match m = regEx.Match(matchLine);
         if (m.Success)
         {
            string subLogLine = logLine.Substring(m.Index + m.Length + 4);
            return (true, subLogLine);
         }

         return (false, logLine);
      }

      public ILogLine IdentifyLine(string logLine)
      {
         (bool success, string subLogLine) result;

         /* 2 - IDC */
         if (logLine.Contains("GETINFO[2") || logLine.Contains("EXECUTE[2") || logLine.Contains("SERVICE_EVENT[2"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_IDC_STATUS, logLine);
            if (result.success) return new WFSIDCSTATUS(this, result.subLogLine);
         }

         /* 3 - CDM */
         if (logLine.Contains("GETINFO[3") || logLine.Contains("EXECUTE[3") || logLine.Contains("SERVICE_EVENT[3"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CDM_STATUS, logLine);
            if (result.success) return new WFSCDMSTATUS(this, result.subLogLine);

            result = GenericMatch(WFS_INF_CDM_CASH_UNIT_INFO, logLine);
            if (result.success) return new WFSCDMCUINFO(this, result.subLogLine);

            result = GenericMatch(WFS_INF_CDM_PRESENT_STATUS, logLine);
            if (result.success) return new WFSCDMPRESENTSTATUS(this, result.subLogLine);

            /* Test for EXECUTE */
            result = GenericMatch(WFS_CMD_CDM_DISPENSE, logLine);
            if (result.success) return new WFSCDMDENOMINATION(this, result.subLogLine, XFSType.WFS_CMD_CDM_DISPENSE);

            result = GenericMatch(WFS_CMD_CDM_PRESENT, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CDM_PRESENT);

            result = GenericMatch(WFS_CMD_CDM_REJECT, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CDM_REJECT);

            result = GenericMatch(WFS_CMD_CDM_RETRACT, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CDM_RETRACT);

            result = GenericMatch(WFS_CMD_CDM_RESET, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CDM_RESET);

            result = GenericMatch(WFS_CMD_CDM_STARTEX, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CDM_STARTEX);

            result = GenericMatch(WFS_CMD_CDM_ENDEX, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CDM_ENDEX);

            /* Test for EVENTS */
            result = GenericMatch(WFS_SRVE_CDM_CASHUNITINFOCHANGED, logLine);
            if (result.success) return new WFSCDMCUINFO(this, result.subLogLine, XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED);

            result = GenericMatch(WFS_SRVE_CDM_ITEMSTAKEN, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_SRVE_CDM_ITEMSTAKEN);

            /* We should have matched something */
            return new SPLine(this, logLine, XFSType.Error);
         }

         /* 4 - PIN */
         if (logLine.Contains("GETINFO[4") || logLine.Contains("EXECUTE[4") || logLine.Contains("SERVICE_EVENT[4"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_PIN_STATUS, logLine);
            if (result.success) return new WFSPINSTATUS(this, result.subLogLine, XFSType.WFS_INF_PIN_STATUS);
         }

         /* 5 - CHK */
         if (logLine.Contains("GETINFO[5") || logLine.Contains("EXECUTE[5") || logLine.Contains("SERVICE_EVENT[5"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CHK_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_CHK_STATUS);
         }

         /* 6 - DEP */
         if (logLine.Contains("GETINFO[6") || logLine.Contains("EXECUTE[6") || logLine.Contains("SERVICE_EVENT[6"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_DEP_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_DEP_STATUS);
         }

         /* 7 - TTU */
         if (logLine.Contains("GETINFO[7") || logLine.Contains("EXECUTE[7") || logLine.Contains("SERVICE_EVENT[7"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_TTU_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_TTU_STATUS);
         }

         /* 8 - SIU */
         if (logLine.Contains("GETINFO[8") || logLine.Contains("EXECUTE[8") || logLine.Contains("SERVICE_EVENT[8"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_SIU_STATUS, logLine);
            if (result.success) return new WFSSIUSTATUS(this, result.subLogLine, XFSType.WFS_INF_SIU_STATUS);
         }

         /* 9 - VDM */
         if (logLine.Contains("GETINFO[9") || logLine.Contains("EXECUTE[9") || logLine.Contains("SERVICE_EVENT[9"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_VDM_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_VDM_STATUS);
         }

         /* 10 - CAM */
         if (logLine.Contains("GETINFO[10") || logLine.Contains("EXECUTE[10") || logLine.Contains("SERVICE_EVENT[10"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CAM_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_CAM_STATUS);
         }

         /* 11 - ALM */
         if (logLine.Contains("GETINFO[11") || logLine.Contains("EXECUTE[11") || logLine.Contains("SERVICE_EVENT[11"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_ALM_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_ALM_STATUS);
         }

         /* 12 - CEU */
         if (logLine.Contains("GETINFO[12") || logLine.Contains("EXECUTE[12") || logLine.Contains("SERVICE_EVENT[12"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CEU_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_CEU_STATUS);
         }

         /* 13 - CIM */
         if (logLine.Contains("GETINFO[13") || logLine.Contains("EXECUTE[13") || logLine.Contains("SERVICE_EVENT[13") ||
             logLine.Contains("USER_EVENT[13") || logLine.Contains("EXECUTE_EVENT[13"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CIM_STATUS, logLine);
            if (result.success) return new WFSCIMSTATUS(this, result.subLogLine, XFSType.WFS_INF_CIM_STATUS); 

            result = GenericMatch(WFS_INF_CIM_CASH_UNIT_INFO, logLine);
            if (result.success) return new WFSCIMCASHINFO(this, result.subLogLine, XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

            result = GenericMatch(WFS_INF_CIM_CASH_IN_STATUS, logLine);
            if (result.success) return new WFSCIMCASHINSTATUS(this, result.subLogLine, XFSType.WFS_INF_CIM_CASH_IN_STATUS);

            /* Test for EXECUTE */
            result = GenericMatch(WFS_CMD_CIM_CASH_IN_START, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CIM_CASH_IN_START);

            result = GenericMatch(WFS_CMD_CIM_CASH_IN, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CIM_CASH_IN);

            result = GenericMatch(WFS_CMD_CIM_CASH_IN_END, logLine);
            if (result.success) return new WFSCIMCASHINFO(this, logLine, XFSType.WFS_CMD_CIM_CASH_IN_END);

            result = GenericMatch(WFS_CMD_CIM_CASH_IN_ROLLBACK, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK);

            result = GenericMatch(WFS_CMD_CIM_RETRACT, logLine);
            if (result.success) return new WFSCIMCASHINFO(this, result.subLogLine, XFSType.WFS_CMD_CIM_RETRACT);

            result = GenericMatch(WFS_CMD_CIM_RESET, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CIM_RESET);

            result = GenericMatch(WFS_CMD_CIM_START_EXCHANGE, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CIM_STARTEX);

            result = GenericMatch(WFS_CMD_CIM_END_EXCHANGE, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_CIM_ENDEX);

            /* Test for EVENTS */
            result = GenericMatch(WFS_USRE_CIM_CASHUNITTHRESHOLD, logLine);
            if (result.success) return new WFSCIMCASHINFO(this, result.subLogLine, XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD);

            result = GenericMatch(WFS_SRVE_CIM_CASHUNITINFOCHANGED, logLine);
            if (result.success) return new WFSCIMCASHINFO(this, result.subLogLine, XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED);

            result = GenericMatch(WFS_SRVE_CIM_ITEMSTAKEN, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_SRVE_CIM_ITEMSTAKEN);

            result = GenericMatch(WFS_EXEE_CIM_INPUTREFUSE, logLine);
            if (result.success) return new WFSCIMINPUTREFUSE(this, result.subLogLine, XFSType.WFS_EXEE_CIM_INPUTREFUSE);

            result = GenericMatch(WFS_SRVE_CIM_ITEMSPRESENTED, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_SRVE_CIM_ITEMSPRESENTED);

            result = GenericMatch(WFS_SRVE_CIM_ITEMSINSERTED, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_SRVE_CIM_ITEMSINSERTED);

            result = GenericMatch(WFS_EXEE_CIM_NOTEERROR, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_EXEE_CIM_NOTEERROR);

            result = GenericMatch(WFS_SRVE_CIM_MEDIADETECTED, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_SRVE_CIM_MEDIADETECTED);

            /* We should have matched something */
            return new SPLine(this, logLine, XFSType.Error);
         }

         /* 14 - CRD */
         if (logLine.Contains("GETINFO[14") || logLine.Contains("EXECUTE[14") || logLine.Contains("SERVICE_EVENT[14"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CRD_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_CRD_STATUS);
         }

         /* 15 - BCR */
         if (logLine.Contains("GETINFO[15") || logLine.Contains("EXECUTE[15") || logLine.Contains("SERVICE_EVENT[15"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_BCR_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_BCR_STATUS);
         }

         /* 16 - IPM */
         if (logLine.Contains("GETINFO[16") || logLine.Contains("EXECUTE[16") || logLine.Contains("SERVICE_EVENT[16") ||
             logLine.Contains("USER_EVENT[16") || logLine.Contains("EXECUTE_EVENT[16"))
         {

            /* Test for INFO */
            result = GenericMatch(WFS_INF_IPM_STATUS, logLine);
            if (result.success) return new WFSIPMSTATUS(this, result.subLogLine, XFSType.WFS_INF_IPM_STATUS);

            result = GenericMatch(WFS_INF_IPM_MEDIA_BIN_INFO, logLine);
            if (result.success) return new WFSIPMMEDIABININFO(this, result.subLogLine, XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

            result = GenericMatch(WFS_INF_IPM_TRANSACTION_STATUS, logLine);
            if (result.success) return new WFSIPMTRANSSTATUS(this, result.subLogLine, XFSType.WFS_INF_IPM_TRANSACTION_STATUS);

            /* Test for EXECUTE */
            result = GenericMatch(WFS_CMD_IPM_MEDIA_IN, logLine);
            if (result.success) return new WFSIPMMEDIAIN(this, result.subLogLine, XFSType.WFS_CMD_IPM_MEDIA_IN);

            result = GenericMatch(WFS_CMD_IPM_MEDIA_IN_END, logLine);
            if (result.success) return new WFSIPMMEDIAINEND(this, result.subLogLine, XFSType.WFS_CMD_IPM_MEDIA_IN_END);

            result = GenericMatch(WFS_CMD_IPM_MEDIA_IN_ROLLBACK, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_IPM_MEDIA_IN_ROLLBACK);

            result = GenericMatch(WFS_CMD_IPM_PRESENT_MEDIA, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_IPM_PRESENT_MEDIA);

            result = GenericMatch(WFS_CMD_IPM_RETRACT_MEDIA, logLine);
            if (result.success) return new WFSIPMRETRACTMEDIAOUT(this, result.subLogLine, XFSType.WFS_CMD_IPM_RETRACT_MEDIA);

            result = GenericMatch(WFS_CMD_IPM_PRINT_TEXT, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_IPM_PRINT_TEXT);

            result = GenericMatch(WFS_CMD_IPM_RESET, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_IPM_RESET);

            result = GenericMatch(WFS_CMD_IPM_EXPEL_MEDIA, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_CMD_IPM_EXPEL_MEDIA);

            /* Test for EVENTS */
            result = GenericMatch(WFS_EXEE_IPM_MEDIAINSERTED, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAINSERTED);

            result = GenericMatch(WFS_USRE_IPM_MEDIABINTHRESHOLD, logLine);
            if (result.success) return new WFSIPMMEDIABININFO(this, result.subLogLine, XFSType.WFS_USRE_IPM_MEDIABINTHRESHOLD);

            result = GenericMatch(WFS_SRVE_IPM_MEDIABININFOCHANGED, logLine);
            if (result.success) return new WFSIPMMEDIABININFO(this, result.subLogLine, XFSType.WFS_SRVE_IPM_MEDIABININFOCHANGED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIABINERROR, logLine);
            if (result.success) return new WFSIPMEDIABINERROR(this, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIABINERROR);

            result = GenericMatch(WFS_SRVE_IPM_MEDIATAKEN, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_SRVE_IPM_MEDIATAKEN);

            result = GenericMatch(WFS_SRVE_IPM_MEDIADETECTED, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_SRVE_IPM_MEDIADETECTED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIAPRESENTED, logLine);
            if (result.success) return new SPLine(this, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAPRESENTED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIAREFUSED, logLine);
            if (result.success) return new WFSIPMMEDIAREFUSED(this, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAREFUSED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIAREJECTED, logLine);
            if (result.success) return new WFSIPMMEDIAREJECTED(this, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAREJECTED);

            /* We should have matched something */
            return new SPLine(this, logLine, XFSType.Error);
         }

         /* 1 - PTR */
         if (logLine.Contains("GETINFO[1") || logLine.Contains("EXECUTE[1") || logLine.Contains("SERVICE_EVENT[1"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_PTR_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(this, result.subLogLine, XFSType.WFS_INF_PTR_STATUS);
         }

         /* WFPOpen/WFPClose */
         if (logLine.Contains("WFPOpen") || logLine.Contains("WFPClose"))
         {
            result = GenericMatch(WFPOpen, logLine);
            if (result.success) return new WFPOPEN(this, result.subLogLine, XFSType.WFPOPEN);

            result = GenericMatch(WFPClose, logLine);
            if (result.success) return new WFPCLOSE(this, result.subLogLine, XFSType.WFPCLOSE);
         }

         /* SysEvent */
         if (logLine.Contains("lpbDescription"))
         {
            return new SPLine(this, logLine, XFSType.WFS_SYSEVENT);
         }

         return new SPLine(this, logLine, XFSType.None);
      }
   }
}

