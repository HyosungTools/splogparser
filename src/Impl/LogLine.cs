using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Impl
{
   public enum XFSType
   {
      /* Not an XFS line we are interested in */
      None,
      /* CDM */
      WFS_INF_CDM_STATUS,
      WFS_INF_CDM_CASH_UNIT_INFO,
      WFS_CMD_CDM_DISPENSE,
      WFS_CMD_CDM_PRESENT,
      WFS_CMD_CDM_REJECT,
      WFS_CMD_CMD_RETRACT,
      WFS_CMD_CDM_RESET,
      WFS_SRVE_CDM_CASHUNITINFOCHANGED,
      WFS_SRVE_CDM_ITEMSTAKEN,
      /* CIM */
      WFS_INF_CIM_STATUS,
      WFS_INF_CIM_CASH_UNIT_INFO,
      WFS_INF_CIM_CASH_IN_STATUS,
      WFS_CMD_CIM_CASH_IN_START,
      WFS_CMD_CIM_CASH_IN,
      WFS_CMD_CIM_CASH_IN_END,
      WFS_CMD_CIM_CASH_IN_ROLLBACK,
      WFS_CMD_CIM_RETRACT,
      WFS_CMD_CIM_RESET,
      WFS_USRE_CIM_CASHUNITTHRESHOLD,
      WFS_SRVE_CIM_CASHUNITINFOCHANGED,
      WFS_SRVE_CIM_ITEMSTAKEN,
      WFS_EXEE_CIM_INPUTREFUSE,
      WFS_SRVE_CIM_ITEMSPRESENTED,
      WFS_SRVE_CIM_ITEMSINSERTED,
      WFS_EXEE_CIM_NOTEERROR,
      WFS_SRVE_CIM_MEDIADETECTED,

      /* ERROR */
      Error
   }
   public static class LogLine
   {
      /// <summary>
      /// Given a nwlog log line (that can span multiple lines in the log file) read one line
      /// Returns a tuple where: 
      /// found - indicates whether a line was found
      /// oneLine - is the line, 
      /// subLogLine - is the rest of the logLine
      /// </summary>
      /// <param name="logLine">the twlog line (that can span multiple lines in the log file)</param>
      /// <returns>tuple</returns>
      public static (bool found, string oneLine, string subLogLine) ReadNextLine(string logLine)
      {
         string subLogLine = logLine;
         StreamReader streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(subLogLine)));
         string oneLine = streamReader.ReadLine();
         if (!(string.IsNullOrEmpty(oneLine) || string.IsNullOrWhiteSpace(oneLine)))
         {
            // we were able to read one line - for the next line trim any leftover \r\n from the found line
            subLogLine = subLogLine.Substring(oneLine.Length).TrimStart();
            return (true, oneLine, subLogLine);
         }
         // we were not able to read a line
         return (false, string.Empty, subLogLine);
      }

      /// <summary>
      /// Given a nwlog line (that can span mulitple lines), find theline that contains the marker. 
      /// Return a tuple where: 
      /// found - indicates a line with the marker was found
      /// oneLine - the line containing the marker
      /// subLogLine - the rest of the twlog line
      /// </summary>
      /// <param name="logLine">twlog line</param>
      /// <param name="marker">marker to search for</param>
      /// <returns></returns>
      public static (bool found, string oneLine, string subLogLine) FindLine(string logLine, string marker)
      {
         (bool found, string oneLine, string subLogLine) nextLine;
         nextLine.found = true;
         nextLine.oneLine = string.Empty;
         nextLine.subLogLine = logLine;
         while (nextLine.found == true && !nextLine.oneLine.Contains(marker))
         {
            nextLine = LogLine.ReadNextLine(nextLine.subLogLine);
         }
         if (nextLine.found)
         {
            return nextLine;
         }
         return (false, string.Empty, logLine);
      }

      public static XFSType IdentifyLine(string logLine)
      {
         XFSType returnType = XFSType.None;

         /* CDM */
         if (logLine.Contains("GETINFO[3") || logLine.Contains("EXECUTE[3") || logLine.Contains("SERVICE_EVENT[3"))
         {
            /* INFO */
            Regex infoStatus = new Regex("GETINFO.301.*WFS_GETINFO_COMPLETE");
            Regex infoCashUnit = new Regex("GETINFO.303.*WFS_GETINFO_COMPLETE");

            /* EXECUTE */
            Regex cmdDispense = new Regex("EXECUTE.302.*WFS_EXECUTE_COMPLETE");
            Regex cmdPresent = new Regex("EXECUTE.303.*WFS_EXECUTE_COMPLETE");
            Regex cmdReject = new Regex("EXECUTE.304.*WFS_EXECUTE_COMPLETE");
            Regex cmdRetract = new Regex("EXECUTE.305.*WFS_EXECUTE_COMPLETE");
            Regex cmdReset = new Regex("EXECUTE.321.*WFS_EXECUTE_COMPLETE");

            /* EVENTS */
            Regex evtCashUnitChange = new Regex("SERVICE_EVENT.304.*WFS_SERVICE_EVENT");
            Regex evtItemsTaken = new Regex("SERVICE_EVENT.309.*WFS_SERVICE_EVENT");

            /* Test for INFO */
            if (infoStatus.Match(logLine).Success) return XFSType.WFS_INF_CDM_STATUS;
            if (infoCashUnit.Match(logLine).Success) return XFSType.WFS_INF_CDM_CASH_UNIT_INFO;

            /* Test for EXECUTE */
            if (cmdDispense.Match(logLine).Success) return XFSType.WFS_CMD_CDM_DISPENSE;
            if (cmdPresent.Match(logLine).Success) return XFSType.WFS_CMD_CDM_PRESENT;
            if (cmdReject.Match(logLine).Success) return XFSType.WFS_CMD_CDM_REJECT;
            if (cmdRetract.Match(logLine).Success) return XFSType.WFS_CMD_CMD_RETRACT;
            if (cmdReset.Match(logLine).Success) return XFSType.WFS_CMD_CDM_RESET;

            /* Test for EVENTS */
            if (evtCashUnitChange.Match(logLine).Success) return XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED;
            if (evtItemsTaken.Match(logLine).Success) return XFSType.WFS_SRVE_CDM_ITEMSTAKEN;

            /* We should have matched something */
            return XFSType.Error;
         }

         /* CIM */
         if (logLine.Contains("GETINFO[13") || logLine.Contains("EXECUTE[13") || logLine.Contains("SERVICE_EVENT[13") ||
             logLine.Contains("USER_EVENT[13") || logLine.Contains("EXECUTE_EVENT[13"))
         {
            /* INFO */
            Regex infoStatus = new Regex("GETINFO.1301.*WFS_GETINFO_COMPLETE");
            Regex infoCashUnit = new Regex("GETINFO.1303.*WFS_GETINFO_COMPLETE");
            Regex infoCashInStatus = new Regex("GETINFO.1307.*WFS_GETINFO_COMPLETE");

            /* EXECUTE */
            Regex cmdCashInStart = new Regex("EXECUTE.1301.*WFS_EXECUTE_COMPLETE");
            Regex cmdCashIn = new Regex("EXECUTE.1302.*WFS_EXECUTE_COMPLETE");
            Regex cmdCashInEnd = new Regex("EXECUTE.1303.*WFS_EXECUTE_COMPLETE");
            Regex cmdCashInRollback = new Regex("EXECUTE.1304.*WFS_EXECUTE_COMPLETE");
            Regex cmdCashInRetract = new Regex("EXECUTE.1305.*WFS_EXECUTE_COMPLETE");
            Regex cmdCashInReset = new Regex("EXECUTE.1313.*WFS_EXECUTE_COMPLETE");

            /* Events */
            Regex evtCashUnitThreshold = new Regex("USER_EVENT.1303.*WFS_USER_EVENT");
            Regex evtCashUnitInfoChanged = new Regex("SERVICE_EVENT.1304.*WFS_SERVICE_EVENT");
            Regex evtItemsTaken = new Regex("SERVICE_EVENT.1307.*WFS_SERVICE_EVENT");
            Regex evtInputRefused = new Regex("EXECUTE_EVENT.1309.*WFS_EXECUTE_EVENT");
            Regex evtItemsPresented = new Regex("SERVICE_EVENT.1310.*WFS_SERVICE_EVENT");
            Regex evtItemsInserted = new Regex("SERVICE_EVENT.1311.*WFS_SERVICE_EVENT");
            Regex evtNoteError = new Regex("EXECUTE_EVENT.1312.*WFS_EXECUTE_EVENT");
            Regex evtMediaDetected = new Regex("SERVICE_EVENT.1314.*WFS_SERVICE_EVENT");

            /* Test for INFO */
            if (infoStatus.Match(logLine).Success) return XFSType.WFS_INF_CIM_STATUS;
            if (infoCashUnit.Match(logLine).Success) return XFSType.WFS_INF_CIM_CASH_UNIT_INFO;
            if (infoCashInStatus.Match(logLine).Success) return XFSType.WFS_INF_CIM_CASH_IN_STATUS;

            /* Test for EXECUTE */
            if (cmdCashInStart.Match(logLine).Success) return XFSType.WFS_CMD_CIM_CASH_IN_START;
            if (cmdCashIn.Match(logLine).Success) return XFSType.WFS_CMD_CIM_CASH_IN;
            if (cmdCashInEnd.Match(logLine).Success) return XFSType.WFS_CMD_CIM_CASH_IN_END;
            if (cmdCashInRollback.Match(logLine).Success) return XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK;
            if (cmdCashInRetract.Match(logLine).Success) return XFSType.WFS_CMD_CIM_RETRACT;
            if (cmdCashInReset.Match(logLine).Success) return XFSType.WFS_CMD_CIM_RESET;

            /* Test for EVENTS */
            if (evtCashUnitThreshold.Match(logLine).Success) return XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD;
            if (evtCashUnitInfoChanged.Match(logLine).Success) return XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED;
            if (evtItemsTaken.Match(logLine).Success) return XFSType.WFS_SRVE_CIM_ITEMSTAKEN;
            if (evtInputRefused.Match(logLine).Success) return XFSType.WFS_EXEE_CIM_INPUTREFUSE;
            if (evtItemsPresented.Match(logLine).Success) return XFSType.WFS_SRVE_CIM_ITEMSPRESENTED;
            if (evtItemsInserted.Match(logLine).Success) return XFSType.WFS_SRVE_CIM_ITEMSINSERTED;
            if (evtNoteError.Match(logLine).Success) return XFSType.WFS_EXEE_CIM_NOTEERROR;
            if (evtMediaDetected.Match(logLine).Success) return XFSType.WFS_SRVE_CIM_MEDIADETECTED;

            /* We should have matched something */
            return XFSType.Error;
         }

         return returnType;
      }
   }
}
