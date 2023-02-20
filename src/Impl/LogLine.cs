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
      WFS_CMD_CDM_RETRACT,
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

      /// <summary>
      /// Attempt to Match the RegEx and if successful, truncate the line to after the match. 
      /// Truncate so the remainder starts with lpResult
      /// </summary>
      /// <param name="regEx">Regular Expression to search for</param>
      /// <param name="logLine">Log Line to search</param>
      /// <returns>tuple: success (t/f) and log line </returns>
      private static (bool success, string xfsLine)  GenericMatch(Regex regEx, string logLine)
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

      public static (XFSType xfsType, string xfsLine) IdentifyLine(string logLine)
      {
         (bool success, string xfsLine) result; 

         /* CDM */
         if (logLine.Contains("GETINFO[3") || logLine.Contains("EXECUTE[3") || logLine.Contains("SERVICE_EVENT[3"))
         {
            /* INFO */
            Regex infoStatus = new Regex("GETINFO.301.[0-9]+WFS_GETINFO_COMPLETE");
            Regex infoCashUnit = new Regex("GETINFO.303.[0-9]+WFS_GETINFO_COMPLETE");

            /* EXECUTE */
            Regex cmdDispense = new Regex("EXECUTE.302.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdPresent = new Regex("EXECUTE.303.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdReject = new Regex("EXECUTE.304.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdRetract = new Regex("EXECUTE.305.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdReset = new Regex("EXECUTE.321.[0-9]+WFS_EXECUTE_COMPLETE");

            /* EVENTS */
            Regex evtCashUnitChange = new Regex("SERVICE_EVENT.304.[0-9]+WFS_SERVICE_EVENT");
            Regex evtItemsTaken = new Regex("SERVICE_EVENT.309.[0-9]+WFS_SERVICE_EVENT");

            /* Test for INFO */
            result = GenericMatch(infoStatus, logLine);
            if (result.success) return (XFSType.WFS_INF_CDM_STATUS, result.xfsLine);

            result = GenericMatch(infoCashUnit, logLine);
            if (result.success) return (XFSType.WFS_INF_CDM_CASH_UNIT_INFO, result.xfsLine);

            /* Test for EXECUTE */
            result = GenericMatch(cmdDispense, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_DISPENSE, result.xfsLine);

            result = GenericMatch(cmdPresent, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_PRESENT, result.xfsLine);

            result = GenericMatch(cmdReject, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_REJECT, result.xfsLine);

            result = GenericMatch(cmdRetract, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_RETRACT, result.xfsLine);

            result = GenericMatch(cmdReset, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_RESET, result.xfsLine);

            /* Test for EVENTS */
            result = GenericMatch(evtCashUnitChange, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED, result.xfsLine);

            result = GenericMatch(evtItemsTaken, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CDM_ITEMSTAKEN, result.xfsLine);

            /* We should have matched something */
            return (XFSType.Error, string.Empty);
         }

         /* CIM */
         if (logLine.Contains("GETINFO[13") || logLine.Contains("EXECUTE[13") || logLine.Contains("SERVICE_EVENT[13") ||
             logLine.Contains("USER_EVENT[13") || logLine.Contains("EXECUTE_EVENT[13"))
         {
            /* INFO */
            Regex infoStatus = new Regex("GETINFO.1301.[0-9]+WFS_GETINFO_COMPLETE");
            Regex infoCashUnit = new Regex("GETINFO.1303.[0-9]+WFS_GETINFO_COMPLETE");
            Regex infoCashInStatus = new Regex("GETINFO.1307.[0-9]+WFS_GETINFO_COMPLETE");

            /* EXECUTE */
            Regex cmdCashInStart = new Regex("EXECUTE.1301.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdCashIn = new Regex("EXECUTE.1302.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdCashInEnd = new Regex("EXECUTE.1303.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdCashInRollback = new Regex("EXECUTE.1304.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdCashInRetract = new Regex("EXECUTE.1305.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex cmdCashInReset = new Regex("EXECUTE.1313.[0-9]+WFS_EXECUTE_COMPLETE");

            /* Events */
            Regex evtCashUnitThreshold = new Regex("USER_EVENT.1303.[0-9]+WFS_USER_EVENT");
            Regex evtCashUnitInfoChanged = new Regex("SERVICE_EVENT.1304.[0-9]+WFS_SERVICE_EVENT");
            Regex evtItemsTaken = new Regex("SERVICE_EVENT.1307.[0-9]+WFS_SERVICE_EVENT");
            Regex evtInputRefused = new Regex("EXECUTE_EVENT.1309.[0-9]+WFS_EXECUTE_EVENT");
            Regex evtItemsPresented = new Regex("SERVICE_EVENT.1310.[0-9]+WFS_SERVICE_EVENT");
            Regex evtItemsInserted = new Regex("SERVICE_EVENT.1311.[0-9]+WFS_SERVICE_EVENT");
            Regex evtNoteError = new Regex("EXECUTE_EVENT.1312.[0-9]+WFS_EXECUTE_EVENT");
            Regex evtMediaDetected = new Regex("SERVICE_EVENT.1314.[0-9]+WFS_SERVICE_EVENT");

            /* Test for INFO */
            result = GenericMatch(infoStatus, logLine);
            if (result.success) return (XFSType.WFS_INF_CIM_STATUS, result.xfsLine);

            result = GenericMatch(infoCashUnit, logLine);
            if (result.success) return (XFSType.WFS_INF_CIM_CASH_UNIT_INFO, result.xfsLine);

            result = GenericMatch(infoCashInStatus, logLine);
            if (result.success) return (XFSType.WFS_INF_CIM_CASH_IN_STATUS, result.xfsLine);

            /* Test for EXECUTE */
            result = GenericMatch(cmdCashInStart, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN_START, result.xfsLine);

            result = GenericMatch(cmdCashIn, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN, result.xfsLine);

            result = GenericMatch(cmdCashInEnd, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN_END, result.xfsLine);

            result = GenericMatch(cmdCashInRollback, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK, result.xfsLine);

            result = GenericMatch(cmdCashInRetract, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_RETRACT, result.xfsLine);

            result = GenericMatch(cmdCashInReset, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_RESET, result.xfsLine);

            /* Test for EVENTS */
            result = GenericMatch(evtCashUnitThreshold, logLine);
            if (result.success) return (XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD, result.xfsLine);

            result = GenericMatch(evtCashUnitInfoChanged, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED, result.xfsLine);

            result = GenericMatch(evtItemsTaken, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_ITEMSTAKEN, result.xfsLine);

            result = GenericMatch(evtInputRefused, logLine);
            if (result.success) return (XFSType.WFS_EXEE_CIM_INPUTREFUSE, result.xfsLine);

            result = GenericMatch(evtItemsPresented, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_ITEMSPRESENTED, result.xfsLine);

            result = GenericMatch(evtItemsInserted, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_ITEMSINSERTED, result.xfsLine);

            result = GenericMatch(evtNoteError, logLine);
            if (result.success) return (XFSType.WFS_EXEE_CIM_NOTEERROR, result.xfsLine);

            result = GenericMatch(evtMediaDetected, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_MEDIADETECTED, result.xfsLine);

            /* We should have matched something */
            return (XFSType.Error, string.Empty);
         }

         return (XFSType.None, logLine);
      }
   }
}
