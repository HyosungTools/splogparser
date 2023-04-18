using System.Text.RegularExpressions;

namespace Impl
{
   public enum XFSType
   {
      /* Not an XFS line we are interested in */
      None,
      /* 1 - PTR */
      WFS_INF_PTR_STATUS,
      /* 2 - IDC */
      WFS_INF_IDC_STATUS,
      /* 3 - CDM */
      WFS_INF_CDM_STATUS,
      WFS_INF_CDM_CASH_UNIT_INFO,
      WFS_INF_CDM_PRESENT_STATUS,
      WFS_CMD_CDM_DISPENSE,
      WFS_CMD_CDM_PRESENT,
      WFS_CMD_CDM_REJECT,
      WFS_CMD_CDM_RETRACT,
      WFS_CMD_CDM_RESET,
      WFS_SRVE_CDM_CASHUNITINFOCHANGED,
      WFS_SRVE_CDM_ITEMSTAKEN,
      /* 4 - PIN */
      WFS_INF_PIN_STATUS,
      /* 5 - CHK */
      WFS_INF_CHK_STATUS,
      /* 6 - DEP */
      WFS_INF_DEP_STATUS,
      /* 7 - TTU */
      WFS_INF_TTU_STATUS,
      /* 8 - SIU */
      WFS_INF_SIU_STATUS,
      /* 9 - VDM */
      WFS_INF_VDM_STATUS,
      /* 10 - CAM */
      WFS_INF_CAM_STATUS,
      /* 11 - ALM */
      WFS_INF_ALM_STATUS,
      /* 12 - CEU */
      WFS_INF_CEU_STATUS,
      /* 13 - CIM */
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
      /* 14 - CRD */
      WFS_INF_CRD_STATUS,
      /* 15 - BCR */
      WFS_INF_BCR_STATUS,
      /* 16 - IPM */
      WFS_INF_IPM_STATUS,
      WFS_INF_IPM_MEDIA_BIN_INFO, 
      WFS_INF_IPM_TRANSACTION_STATUS,
      WFS_CMD_IPM_MEDIA_IN,
      WFS_CMD_IPM_MEDIA_IN_END, 
      WFS_CMD_IPM_MEDIA_IN_ROLLBACK, 
      WFS_CMD_IPM_PRESENT_MEDIA,
      WFS_CMD_IPM_RETRACT_MEDIA,
      WFS_CMD_IPM_RESET, 
      WFS_CMD_IPM_EXPEL_MEDIA,  
      WFS_EXEE_IPM_MEDIAINSERTED, 
      WFS_USRE_IPM_MEDIABINTHRESHOLD, 
      WFS_SRVE_IPM_MEDIABININFOCHANGED, 
      WFS_EXEE_IPM_MEDIABINERROR, 
      WFS_SRVE_IPM_MEDIATAKEN, 
      WFS_SRVE_IPM_MEDIADETECTED, 
      WFS_EXEE_IPM_MEDIAPRESENTED, 
      WFS_EXEE_IPM_MEDIAREFUSED,  
      WFS_EXEE_IPM_MEDIAREJECTED, 

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

         /* 1 - PTR */
         if (logLine.Contains("GETINFO[1") || logLine.Contains("EXECUTE[1") || logLine.Contains("SERVICE_EVENT[1"))
         {
            /* INFO */
            Regex wfs_inf_ptr_status = new Regex("GETINFO.101.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_ptr_status, logLine);
            if (result.success) return (XFSType.WFS_INF_PTR_STATUS, result.xfsLine);

         }

         /* 2 - IDC */
         if (logLine.Contains("GETINFO[2") || logLine.Contains("EXECUTE[2") || logLine.Contains("SERVICE_EVENT[2"))
         {
            /* INFO */
            Regex wfs_inf_idc_status = new Regex("GETINFO.201.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_idc_status, logLine);
            if (result.success) return (XFSType.WFS_INF_IDC_STATUS, result.xfsLine);
         }

         /* 3 - CDM */
         if (logLine.Contains("GETINFO[3") || logLine.Contains("EXECUTE[3") || logLine.Contains("SERVICE_EVENT[3"))
         {
            /* INFO */
            Regex wfs_inf_cdm_status = new Regex("GETINFO.301.[0-9]+WFS_GETINFO_COMPLETE");
            Regex wfs_inf_cdm_cash_unit_info = new Regex("GETINFO.303.[0-9]+WFS_GETINFO_COMPLETE");
            Regex WFS_INF_CDM_PRESENT_STATUS = new Regex("GETINFO.309.[0-9]+WFS_GETINFO_COMPLETE");

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
            result = GenericMatch(wfs_inf_cdm_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CDM_STATUS, result.xfsLine);

            result = GenericMatch(wfs_inf_cdm_cash_unit_info, logLine);
            if (result.success) return (XFSType.WFS_INF_CDM_CASH_UNIT_INFO, result.xfsLine);

            result = GenericMatch(WFS_INF_CDM_PRESENT_STATUS, logLine);
            if (result.success) return (XFSType.WFS_INF_CDM_PRESENT_STATUS, result.xfsLine);

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

         /* 4 - PIN */
         if (logLine.Contains("GETINFO[4") || logLine.Contains("EXECUTE[4") || logLine.Contains("SERVICE_EVENT[4"))
         {
            /* INFO */
            Regex wfs_inf_pin_status = new Regex("GETINFO.401.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_pin_status, logLine);
            if (result.success) return (XFSType.WFS_INF_PIN_STATUS, result.xfsLine);
         }

         /* 5 - CHK */
         if (logLine.Contains("GETINFO[5") || logLine.Contains("EXECUTE[5") || logLine.Contains("SERVICE_EVENT[5"))
         {
            /* INFO */
            Regex wfs_inf_chk_status = new Regex("GETINFO.501.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_chk_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CHK_STATUS, result.xfsLine);
         }
         /* 6 - DEP */
         if (logLine.Contains("GETINFO[6") || logLine.Contains("EXECUTE[6") || logLine.Contains("SERVICE_EVENT[6"))
         {
            /* INFO */
            Regex wfs_inf_dep_status = new Regex("GETINFO.601.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_dep_status, logLine);
            if (result.success) return (XFSType.WFS_INF_DEP_STATUS, result.xfsLine);
         }

         /* 7 - TTU */
         if (logLine.Contains("GETINFO[7") || logLine.Contains("EXECUTE[7") || logLine.Contains("SERVICE_EVENT[7"))
         {
            /* INFO */
            Regex wfs_inf_ttu_status = new Regex("GETINFO.701.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_ttu_status, logLine);
            if (result.success) return (XFSType.WFS_INF_TTU_STATUS, result.xfsLine);
         }

         /* 8 - SIU */
         if (logLine.Contains("GETINFO[8") || logLine.Contains("EXECUTE[8") || logLine.Contains("SERVICE_EVENT[8"))
         {
            /* INFO */
            Regex wfs_inf_siu_status = new Regex("GETINFO.801.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_siu_status, logLine);
            if (result.success) return (XFSType.WFS_INF_SIU_STATUS, result.xfsLine);
         }

         /* 9 - VDM */
         if (logLine.Contains("GETINFO[9") || logLine.Contains("EXECUTE[9") || logLine.Contains("SERVICE_EVENT[9"))
         {
            /* INFO */
            Regex wfs_inf_vdm_status = new Regex("GETINFO.901.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_vdm_status, logLine);
            if (result.success) return (XFSType.WFS_INF_VDM_STATUS, result.xfsLine);
         }

         /* 10 - CAM */
         if (logLine.Contains("GETINFO[10") || logLine.Contains("EXECUTE[10") || logLine.Contains("SERVICE_EVENT[10"))
         {
            /* INFO */
            Regex wfs_inf_cam_status = new Regex("GETINFO.1001.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_cam_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CAM_STATUS, result.xfsLine);
         }

         /* 11 - ALM */
         if (logLine.Contains("GETINFO[11") || logLine.Contains("EXECUTE[11") || logLine.Contains("SERVICE_EVENT[11"))
         {
            /* INFO */
            Regex wfs_inf_alm_status = new Regex("GETINFO.1101.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_alm_status, logLine);
            if (result.success) return (XFSType.WFS_INF_ALM_STATUS, result.xfsLine);
         }

         /* 12 - CEU */
         if (logLine.Contains("GETINFO[12") || logLine.Contains("EXECUTE[12") || logLine.Contains("SERVICE_EVENT[12"))
         {
            /* INFO */
            Regex wfs_inf_ceu_status = new Regex("GETINFO.1201.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_ceu_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CEU_STATUS, result.xfsLine);
         }

         /* 13 - CIM */
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

         /* 14 - CRD */
         if (logLine.Contains("GETINFO[14") || logLine.Contains("EXECUTE[14") || logLine.Contains("SERVICE_EVENT[14"))
         {
            /* INFO */
            Regex wfs_inf_crd_status = new Regex("GETINFO.1401.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_crd_status, logLine);
            if (result.success) return (XFSType.WFS_INF_CRD_STATUS, result.xfsLine);
         }

         /* 15 - BCR */
         if (logLine.Contains("GETINFO[15") || logLine.Contains("EXECUTE[15") || logLine.Contains("SERVICE_EVENT[15"))
         {
            /* INFO */
            Regex wfs_inf_bcr_status = new Regex("GETINFO.1501.[0-9]+WFS_GETINFO_COMPLETE");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_bcr_status, logLine);
            if (result.success) return (XFSType.WFS_INF_BCR_STATUS, result.xfsLine);
         }

         /* 16 - IPM */
         if (logLine.Contains("GETINFO[16") || logLine.Contains("EXECUTE[16") || logLine.Contains("SERVICE_EVENT[16") ||
             logLine.Contains("USER_EVENT[16") || logLine.Contains("EXECUTE_EVENT[16"))
         {
            /* INFO */
            Regex wfs_inf_ipm_status = new Regex("GETINFO.1601.[0-9]+WFS_GETINFO_COMPLETE");
            Regex wfs_inf_ipm_media_bin_info = new Regex("GETINFO.1604.[0-9]+WFS_GETINFO_COMPLETE");
            Regex wfs_inf_ipm_transaction_status = new Regex("GETINFO.1605.[0-9]+WFS_GETINFO_COMPLETE");

            /* EXECUTE */
            Regex wfs_cmd_ipm_media_in = new Regex("EXECUTE.1601.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_ipm_media_in_end = new Regex("EXECUTE.1602.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_ipm_media_in_rollback = new Regex("EXECUTE.1603.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_ipm_present_media = new Regex("EXECUTE.1606.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_ipm_retract_media = new Regex("EXECUTE.1607.[0-9]+WFS_EXECUTE_COMPLETE");

            Regex wfs_cmd_ipm_reset = new Regex("EXECUTE.1610.[0-9]+WFS_EXECUTE_COMPLETE");
            Regex wfs_cmd_ipm_expel_media = new Regex("EXECUTE.1614.[0-9]+WFS_EXECUTE_COMPLETE");

            /* Events */
            Regex wfs_exee_ipm_mediainserted = new Regex("SERVICE_EVENT.1602.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_usre_ipm_mediabinthreshold = new Regex("SERVICE_EVENT.1603.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_srve_ipm_mediabininfochanged = new Regex("EXECUTE_EVENT.1604.[0-9]+WFS_EXECUTE_EVENT");
            Regex wfs_exee_ipm_mediabinerror = new Regex("SERVICE_EVENT.1605.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_srve_ipm_mediataken = new Regex("SERVICE_EVENT.1606.[0-9]+WFS_SERVICE_EVENT");

            Regex wfs_srve_ipm_mediadetected = new Regex("EXECUTE_EVENT.1610.[0-9]+WFS_EXECUTE_EVENT");
            Regex wfs_exee_ipm_mediapresented = new Regex("SERVICE_EVENT.1611.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_exee_ipm_mediarefused = new Regex("SERVICE_EVENT.1612.[0-9]+WFS_SERVICE_EVENT");
            Regex wfs_exee_ipm_mediarejected = new Regex("SERVICE_EVENT.1615.[0-9]+WFS_SERVICE_EVENT");

            /* Test for INFO */
            result = GenericMatch(wfs_inf_ipm_status, logLine);
            if (result.success) return (XFSType.WFS_INF_IPM_STATUS, result.xfsLine);

            result = GenericMatch(wfs_inf_ipm_media_bin_info, logLine);
            if (result.success) return (XFSType.WFS_INF_IPM_MEDIA_BIN_INFO, result.xfsLine);

            result = GenericMatch(wfs_inf_ipm_transaction_status, logLine);
            if (result.success) return (XFSType.WFS_INF_IPM_TRANSACTION_STATUS, result.xfsLine);

            /* Test for EXECUTE */
            result = GenericMatch(wfs_cmd_ipm_media_in, logLine);
            if (result.success) return (XFSType.WFS_CMD_IPM_MEDIA_IN, result.xfsLine);

            result = GenericMatch(wfs_cmd_ipm_media_in_end, logLine);
            if (result.success) return (XFSType.WFS_CMD_IPM_MEDIA_IN_END, result.xfsLine);

            result = GenericMatch(wfs_cmd_ipm_media_in_rollback, logLine);
            if (result.success) return (XFSType.WFS_CMD_IPM_MEDIA_IN_ROLLBACK, result.xfsLine);

            result = GenericMatch(wfs_cmd_ipm_present_media, logLine);
            if (result.success) return (XFSType.WFS_CMD_IPM_PRESENT_MEDIA, result.xfsLine);

            result = GenericMatch(wfs_cmd_ipm_retract_media, logLine);
            if (result.success) return (XFSType.WFS_CMD_IPM_RETRACT_MEDIA, result.xfsLine);

            result = GenericMatch(wfs_cmd_ipm_reset, logLine);
            if (result.success) return (XFSType.WFS_CMD_IPM_RESET, result.xfsLine);

            result = GenericMatch(wfs_cmd_ipm_expel_media, logLine);
            if (result.success) return (XFSType.WFS_CMD_IPM_EXPEL_MEDIA, result.xfsLine);

            /* Test for EVENTS */
            result = GenericMatch(wfs_exee_ipm_mediainserted, logLine);
            if (result.success) return (XFSType.WFS_EXEE_IPM_MEDIAINSERTED, result.xfsLine);

            result = GenericMatch(wfs_usre_ipm_mediabinthreshold, logLine);
            if (result.success) return (XFSType.WFS_USRE_IPM_MEDIABINTHRESHOLD, result.xfsLine);

            result = GenericMatch(wfs_srve_ipm_mediabininfochanged, logLine);
            if (result.success) return (XFSType.WFS_SRVE_IPM_MEDIABININFOCHANGED, result.xfsLine);

            result = GenericMatch(wfs_exee_ipm_mediabinerror, logLine);
            if (result.success) return (XFSType.WFS_EXEE_IPM_MEDIABINERROR, result.xfsLine);

            result = GenericMatch(wfs_srve_ipm_mediataken, logLine);
            if (result.success) return (XFSType.WFS_SRVE_IPM_MEDIATAKEN, result.xfsLine);

            result = GenericMatch(wfs_srve_ipm_mediadetected, logLine);
            if (result.success) return (XFSType.WFS_SRVE_IPM_MEDIADETECTED, result.xfsLine);

            result = GenericMatch(wfs_exee_ipm_mediapresented, logLine);
            if (result.success) return (XFSType.WFS_EXEE_IPM_MEDIAPRESENTED, result.xfsLine);

            result = GenericMatch(wfs_exee_ipm_mediarefused, logLine);
            if (result.success) return (XFSType.WFS_EXEE_IPM_MEDIAREFUSED, result.xfsLine);

            result = GenericMatch(wfs_exee_ipm_mediarejected, logLine);
            if (result.success) return (XFSType.WFS_EXEE_IPM_MEDIAREJECTED, result.xfsLine);


            /* We should have matched something */
            return (XFSType.Error, string.Empty);
         }

         return (XFSType.None, logLine);
      }
   }
}
