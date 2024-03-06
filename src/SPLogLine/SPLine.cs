using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public enum XFSType
   {
      /* Not an XFS line we are interested in */
      None,
      /* 1 - PTR */
      WFS_INF_PTR_STATUS,

      /* 2 - IDC */
      WFS_INF_IDC_STATUS,
      WFS_INF_IDC_CAPABILITIES,
      WFS_INF_IDC_FORM_LIST,
      WFS_INF_IDC_QUERY_FORM,
      WFS_INF_IDC_QUERY_IFM_IDENTIFIER,

      WFS_CMD_IDC_READ_TRACK,
      WFS_CMD_IDC_WRITE_TRACK,
      WFS_CMD_IDC_EJECT_CARD,
      WFS_CMD_IDC_RETAIN_CARD,
      WFS_CMD_IDC_RESET_COUNT,
      WFS_CMD_IDC_SETKEY,
      WFS_CMD_IDC_READ_RAW_DATA,
      WFS_CMD_IDC_WRITE_RAW_DATA,
      WFS_CMD_IDC_CHIP_IO,
      WFS_CMD_IDC_RESET,
      WFS_CMD_IDC_CHIP_POWER,
      WFS_CMD_IDC_PARSE_DATA,
      WFS_CMD_IDC_SET_GUIDANCE_LIGHT,
      WFS_CMD_IDC_POWER_SAVE_CONTROL,
      WFS_CMD_IDC_PARK_CARD,

      WFS_EXEE_IDC_INVALIDTRACKDATA,
      WFS_EXEE_IDC_MEDIAINSERTED,
      WFS_SRVE_IDC_MEDIAREMOVED,
      WFS_EXEE_IDC_MEDIARETAINED,
      WFS_EXEE_IDC_INVALIDMEDIA,
      WFS_SRVE_IDC_CARDACTION,
      WFS_USRE_IDC_RETAINBINTHRESHOLD,
      WFS_SRVE_IDC_MEDIADETECTED,
      WFS_SRVE_IDC_RETAINBINREMOVED,
      WFS_SRVE_IDC_RETAINBININSERTED,
      WFS_EXEE_IDC_INSERTCARD,
      WFS_SRVE_IDC_DEVICEPOSITION,
      WFS_SRVE_IDC_POWER_SAVE_CHANGE,
      WFS_EXEE_IDC_TRACKDETECTED,

      /* 3 - CDM */
      WFS_INF_CDM_STATUS,
      WFS_INF_CDM_CASH_UNIT_INFO,
      WFS_INF_CDM_PRESENT_STATUS,
      WFS_CMD_CDM_DISPENSE,
      WFS_CMD_CDM_PRESENT,
      WFS_CMD_CDM_REJECT,
      WFS_CMD_CDM_RETRACT,
      WFS_CMD_CDM_RESET,
      WFS_CMD_CDM_STARTEX,
      WFS_CMD_CDM_ENDEX,
      WFS_SRVE_CDM_CASHUNITINFOCHANGED,
      WFS_SRVE_CDM_ITEMSTAKEN,

      /* 4 - PIN */
      WFS_INF_PIN_STATUS,
      WFS_INF_PIN_CAPABILITIES,
      WFS_INF_PIN_KEY_DETAIL,
      WFS_INF_PIN_FUNCKEY_DETAIL,
      WFS_INF_PIN_HSM_TDATA,
      WFS_INF_PIN_KEY_DETAIL_EX,
      WFS_INF_PIN_SECUREKEY_DETAIL,
      WFS_INF_PIN_QUERY_LOGICAL_HSM_DETAIL,
      WFS_INF_PIN_QUERY_PCIPTS_DEVICE_ID,

      WFS_CMD_PIN_CRYPT,
      WFS_CMD_PIN_IMPORT_KEY,
      WFS_CMD_PIN_GET_PIN,
      WFS_CMD_PIN_GET_PINBLOCK,
      WFS_CMD_PIN_GET_DATA,
      WFS_CMD_PIN_INITIALIZATION,
      WFS_CMD_PIN_LOCAL_DES,
      WFS_CMD_PIN_LOCAL_EUROCHEQUE,
      WFS_CMD_PIN_LOCAL_VISA,
      WFS_CMD_PIN_CREATE_OFFSET,
      WFS_CMD_PIN_DERIVE_KEY,
      WFS_CMD_PIN_PRESENT_IDC,
      WFS_CMD_PIN_LOCAL_BANKSYS,
      WFS_CMD_PIN_BANKSYS_IO,
      WFS_CMD_PIN_RESET,
      WFS_CMD_PIN_HSM_SET_TDATA,
      WFS_CMD_PIN_SECURE_MSG_SEND,
      WFS_CMD_PIN_SECURE_MSG_RECEIVE,
      WFS_CMD_PIN_GET_JOURNAL,
      WFS_CMD_PIN_IMPORT_KEY_EX,
      WFS_CMD_PIN_ENC_IO,
      WFS_CMD_PIN_HSM_INIT,
      WFS_CMD_PIN_IMPORT_RSA_PUBLIC_KEY,

      WFS_CMD_PIN_EXPORT_RSA_ISSUER_SIGNED_ITEM,
      WFS_CMD_PIN_IMPORT_RSA_SIGNED_DES_KEY,
      WFS_CMD_PIN_GENERATE_RSA_KEY_PAIR,
      WFS_CMD_PIN_EXPORT_RSA_EPP_SIGNED_ITEM,
      WFS_CMD_PIN_LOAD_CERTIFICATE,
      WFS_CMD_PIN_GET_CERTIFICATE,
      WFS_CMD_PIN_REPLACE_CERTIFICATE,
      WFS_CMD_PIN_START_KEY_EXCHANGE,
      WFS_CMD_PIN_IMPORT_RSA_ENCIPHERED_PKCS7_KEY,
      WFS_CMD_PIN_EMV_IMPORT_PUBLIC_KEY,
      WFS_CMD_PIN_DIGEST,
      WFS_CMD_PIN_SECUREKEY_ENTRY,
      WFS_CMD_PIN_GENERATE_KCV,
      WFS_CMD_PIN_SET_GUIDANCE_LIGHT,
      WFS_CMD_PIN_MAINTAIN_PIN,
      WFS_CMD_PIN_KEYPRESS_BEEP,
      WFS_CMD_PIN_SET_PINBLOCK_DATA,
      WFS_CMD_PIN_SET_LOGICAL_HSM,
      WFS_CMD_PIN_IMPORT_KEYBLOCK,
      WFS_CMD_PIN_POWER_SAVE_CONTROL,

      WFS_EXEE_PIN_KEY,
      WFS_SRVE_PIN_INITIALIZED,
      WFS_SRVE_PIN_ILLEGAL_KEY_ACCESS,
      WFS_SRVE_PIN_OPT_REQUIRED,
      WFS_SRVE_PIN_HSM_TDATA_CHANGED,
      WFS_SRVE_PIN_CERTIFICATE_CHANGE,
      WFS_SRVE_PIN_HSM_CHANGED,
      WFS_EXEE_PIN_ENTERDATA,
      WFS_SRVE_PIN_DEVICEPOSITION,
      WFS_SRVE_PIN_POWER_SAVE_CHANGE,



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
      WFS_CMD_CIM_STARTEX,
      WFS_CMD_CIM_ENDEX,
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
      WFS_CMD_IPM_PRINT_TEXT,
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

      /* WFPOpen() and WFPClose() */
      WFPOPEN,
      WFPCLOSE,

      WFS_SYSEVENT,

      /* ERROR */
      Error
   }

   public class SPLine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }
      public string HResult { get; set; }
      public XFSType xfsType { get; set; }

      public SPLine(ILogFileHandler parent, string logLine, XFSType xfsType) : base(parent, logLine)
      {
         this.xfsType = xfsType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();
         HResult = hResult();

         // truncate the logLine

      }

      protected virtual void Initialize(int lUnitCount = 1)
      {
         Timestamp = tsTimestamp();
         HResult = hResult();
      }

      protected override string tsTimestamp()
      {
         // the string from the log file, but return is in normal form
         // (replace '/' with '-' and the 2nd space with a ':')
         string logTime = "2022/01/01 00:00 00.000";

         // Example: tsTimestamp = [2023/02/01 21:03 11.557],
         Regex timeRegex = new Regex("tsTimestamp = \\[(.{23})\\]");
         Match mtch = timeRegex.Match(logLine);
         if (mtch.Success)
         {
            logTime = mtch.Groups[1].Value;
         }

         // replace / with -
         logTime = logTime.Replace('/', '-');

         // replace the 17th character (' ' with ':')
         char replacementChar = ':';

         if (logTime.Length >= 17)
         {
            logTime = logTime.Remove(16, 1).Insert(16, replacementChar.ToString());
         }

         return logTime;
      }

      protected override string hResult()
      {
         string hResult = "0";

         // Example: hResult = [0],
         Regex timeRegex = new Regex("hResult = \\[(.*)\\]");
         Match mtch = timeRegex.Match(logLine);
         if (mtch.Success)
         {
            hResult = mtch.Groups[1].Value.Trim();
         }

         return hResult == "0" ? "" : hResult;
      }
   }
}
