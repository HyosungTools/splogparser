using System.Text.RegularExpressions;

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

   public static class IdentifyLines
   {
      /// <summary>
      /// Attempt to Match the RegEx and if successful, truncate the line to after the match. 
      /// Truncate so the remainder starts with lpResult
      /// </summary>
      /// <param name="regEx">Regular Expression to search for</param>
      /// <param name="logLine">Log Line to search</param>
      /// <returns>tuple: success (t/f) and log line </returns>
      private static (bool success, string xfsLine) GenericMatch(Regex regEx, string logLine)
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

      public static (XFSType xfsType, string xfsLine) XFSLine(string logLine)
      {
         (bool success, string xfsLine) result;

         /* CDM */
         if (logLine.Contains("GETINFO[3") || logLine.Contains("EXECUTE[3") || logLine.Contains("SERVICE_EVENT[3"))
         {
            /* INFO */
            Regex wfs_inf_cmd_status = new Regex("GETINFO.301.[0-9]+WFS_GETINFO_COMPLETE");
            Regex wfs_inf_cmd_cash_unit_info = new Regex("GETINFO.303.[0-9]+WFS_GETINFO_COMPLETE");

            /* EXECUTE */
            Regex wfs_cmd_cdm_dispense = new Regex("EXECUTE.302.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cdm_present = new Regex("EXECUTE.303.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cdm_reject = new Regex("EXECUTE.304.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cdm_retract = new Regex("EXECUTE.305.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cdm_reset = new Regex("EXECUTE.321.[0-9]+WFS_EXECUTE_COMPLETE");

            /* EVENTS */
            Regex wfs_srve_cdm_cashunitinfochanged = new Regex("SERVICE_EVENT.304.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_srve_cdm_itemstaken = new Regex("SERVICE_EVENT.309.[0-9]+WFS_SERVICE_EVENT");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_cmd_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CDM_STATUS, result.xfsLine);

            result = GenericMatch(wfs_inf_cmd_cash_unit_info, logLine);
            if (result.success) return (XFSType.WFS_INF_CDM_CASH_UNIT_INFO, result.xfsLine);

            /* Test for EXECUTE */
            result = GenericMatch(wfs_cmd_cdm_dispense, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_DISPENSE, result.xfsLine);

            result = GenericMatch(wfs_cmd_cdm_present, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_PRESENT, result.xfsLine);

            result = GenericMatch(wfs_cmd_cdm_reject, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_REJECT, result.xfsLine);

            result = GenericMatch(wfs_cmd_cdm_retract, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_RETRACT, result.xfsLine);

            result = GenericMatch(wfs_cmd_cdm_reset, logLine);
            if (result.success) return (XFSType.WFS_CMD_CDM_RESET, result.xfsLine);

            /* Test for EVENTS */
            result = GenericMatch(wfs_srve_cdm_cashunitinfochanged, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED, result.xfsLine);

            result = GenericMatch(wfs_srve_cdm_itemstaken, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CDM_ITEMSTAKEN, result.xfsLine);

            /* We should have matched something */
            return (XFSType.Error, string.Empty);
         }

         /* CIM */
         if (logLine.Contains("GETINFO[13") || logLine.Contains("EXECUTE[13") || logLine.Contains("SERVICE_EVENT[13") ||
             logLine.Contains("USER_EVENT[13") || logLine.Contains("EXECUTE_EVENT[13"))
         {
            /* INFO */
            Regex wfs_inf_cim_status = new Regex("GETINFO.1301.[0-9]+WFS_GETINFO_COMPLETE");
            Regex wfs_inf_cim_cash_unit_info = new Regex("GETINFO.1303.[0-9]+WFS_GETINFO_COMPLETE");
            Regex wfs_inf_cim_cash_in_status = new Regex("GETINFO.1307.[0-9]+WFS_GETINFO_COMPLETE");

            /* EXECUTE */
            Regex wfs_cmd_cim_cash_in_start = new Regex("EXECUTE.1301.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cim_cash_in = new Regex("EXECUTE.1302.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cim_cash_in_end = new Regex("EXECUTE.1303.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cim_cash_in_rollback = new Regex("EXECUTE.1304.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cim_retract = new Regex("EXECUTE.1305.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_cim_reset = new Regex("EXECUTE.1313.[0-9]+WFS_EXECUTE_COMPLETE");

            /* Events */
            Regex wfs_usre_cim_cashunitthreshold = new Regex("USER_EVENT.1303.[0-9]+WFS_USER_EVENT");
            Regex wfs_srve_cim_cashunitinfochanged = new Regex("SERVICE_EVENT.1304.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_srve_cim_itemstaken = new Regex("SERVICE_EVENT.1307.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_exee_cim_inputrefuse = new Regex("EXECUTE_EVENT.1309.[0-9]+WFS_EXECUTE_EVENT");
            Regex wfs_srve_cim_itemspresented = new Regex("SERVICE_EVENT.1310.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_srve_cim_itemsinserted = new Regex("SERVICE_EVENT.1311.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_exee_cim_noteerror = new Regex("EXECUTE_EVENT.1312.[0-9]+WFS_EXECUTE_EVENT");
            Regex wfs_srve_cim_mediadetected = new Regex("SERVICE_EVENT.1314.[0-9]+WFS_SERVICE_EVENT");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_cim_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CIM_STATUS, result.xfsLine);

            result = GenericMatch(wfs_inf_cim_cash_unit_info, logLine);
            if (result.success) return (XFSType.WFS_INF_CIM_CASH_UNIT_INFO, result.xfsLine);

            result = GenericMatch(wfs_inf_cim_cash_in_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CIM_CASH_IN_STATUS, result.xfsLine);

            /* Test for EXECUTE */
            result = GenericMatch(wfs_cmd_cim_cash_in_start, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN_START, result.xfsLine);

            result = GenericMatch(wfs_cmd_cim_cash_in, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN, result.xfsLine);

            result = GenericMatch(wfs_cmd_cim_cash_in_end, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN_END, result.xfsLine);

            result = GenericMatch(wfs_cmd_cim_cash_in_rollback, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK, result.xfsLine);

            result = GenericMatch(wfs_cmd_cim_retract, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_RETRACT, result.xfsLine);

            result = GenericMatch(wfs_cmd_cim_reset, logLine);
            if (result.success) return (XFSType.WFS_CMD_CIM_RESET, result.xfsLine);

            /* Test for EVENTS */
            result = GenericMatch(wfs_usre_cim_cashunitthreshold, logLine);
            if (result.success) return (XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD, result.xfsLine);

            result = GenericMatch(wfs_srve_cim_cashunitinfochanged, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED, result.xfsLine);

            result = GenericMatch(wfs_srve_cim_itemstaken, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_ITEMSTAKEN, result.xfsLine);

            result = GenericMatch(wfs_exee_cim_inputrefuse, logLine);
            if (result.success) return (XFSType.WFS_EXEE_CIM_INPUTREFUSE, result.xfsLine);

            result = GenericMatch(wfs_srve_cim_itemspresented, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_ITEMSPRESENTED, result.xfsLine);

            result = GenericMatch(wfs_srve_cim_itemsinserted, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_ITEMSINSERTED, result.xfsLine);

            result = GenericMatch(wfs_exee_cim_noteerror, logLine);
            if (result.success) return (XFSType.WFS_EXEE_CIM_NOTEERROR, result.xfsLine);

            result = GenericMatch(wfs_srve_cim_mediadetected, logLine);
            if (result.success) return (XFSType.WFS_SRVE_CIM_MEDIADETECTED, result.xfsLine);

            /* We should have matched something */
            return (XFSType.Error, string.Empty);
         }

         return (XFSType.None, logLine);
      }
   }
}
