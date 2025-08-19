using System;
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
      /* 1 - PTR */
      /* INFO */
      static Regex WFS_INF_PTR_STATUS = new Regex("GETINFO.101.[0-9]+WFS_GETINFO_COMPLETE");

      /* 2 - IDC */
      /* INFO */
      static Regex WFS_INF_IDC_STATUS = new Regex("GETINFO.201.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_IDC_CAPABILITIES = new Regex("GETINFO.202.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_IDC_FORM_LIST = new Regex("GETINFO.203.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_IDC_QUERY_FORM = new Regex("GETINFO.204.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_IDC_QUERY_IFM_IDENTIFIER = new Regex("GETINFO.205.[0-9]+WFS_GETINFO_COMPLETE");

      /* EXECUTE */
      static Regex WFS_CMD_IDC_READ_TRACK = new Regex("EXECUTE.201.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_WRITE_TRACK = new Regex("EXECUTE.202.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_EJECT_CARD = new Regex("EXECUTE.203.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_RETAIN_CARD = new Regex("EXECUTE.204.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_RESET_COUNT = new Regex("EXECUTE.205.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_SETKEY = new Regex("EXECUTE.206.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_READ_RAW_DATA = new Regex("EXECUTE.207.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_WRITE_RAW_DATA = new Regex("EXECUTE.208.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_CHIP_IO = new Regex("EXECUTE.209.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_RESET = new Regex("EXECUTE.210.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_CHIP_POWER = new Regex("EXECUTE.211.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_PARSE_DATA = new Regex("EXECUTE.212.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_SET_GUIDANCE_LIGHT = new Regex("EXECUTE.213.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_POWER_SAVE_CONTROL = new Regex("EXECUTE.214.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_IDC_PARK_CARD = new Regex("EXECUTE.215.[0-9]+WFS_EXECUTE_COMPLETE");


      static Regex WFS_EXEE_IDC_INVALIDTRACKDATA = new Regex("EXECUTE_EVENT.201.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_EXEE_IDC_MEDIAINSERTED = new Regex("EXECUTE_EVENT.203.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_IDC_MEDIAREMOVED = new Regex("SERVICE_EVENT.204.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_IDC_CARDACTION = new Regex("SERVICE_EVENT.205.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_USRE_IDC_RETAINBINTHRESHOLD = new Regex("USER_EVENT.206.[0-9]+WFS_USER_EVENT");
      static Regex WFS_EXEE_IDC_INVALIDMEDIA = new Regex("EXECUTE_EVENT.207.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_EXEE_IDC_MEDIARETAINED = new Regex("EXECUTE_EVENT.208.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_IDC_MEDIADETECTED = new Regex("SERVICE_EVENT.209.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_IDC_RETAINBININSERTED = new Regex("SERVICE_EVENT.210.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_IDC_RETAINBINREMOVED = new Regex("SERVICE_EVENT.211.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_EXEE_IDC_INSERTCARD = new Regex("EXECUTE_EVENT.212.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_IDC_DEVICEPOSITION = new Regex("SERVICE_EVENT.213.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_IDC_POWER_SAVE_CHANGE = new Regex("SERVICE_EVENT.214.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_EXEE_IDC_TRACKDETECTED = new Regex("EXECUTE_EVENT.215.[0-9]+WFS_EXECUTE_EVENT");


      /* 3 - CDM  CASH */
      /* INFO */
      static readonly Regex WFS_INF_CDM_STATUS = new Regex(@"CashDispenser.*GETINFO.301.[0-9]+WFS_GETINFO_COMPLETE", RegexOptions.Compiled);
      static readonly Regex WFS_INF_CDM_CASH_UNIT_INFO = new Regex(@"CashDispenser.*GETINFO\[303\][0-9]+WFS_GETINFO_COMPLETE", RegexOptions.Compiled);
      static readonly Regex WFS_INF_CDM_PRESENT_STATUS = new Regex(@"CashDispenser.*GETINFO.309.[0-9]+WFS_GETINFO_COMPLETE", RegexOptions.Compiled);

      /* EXECUTE */
      static Regex WFS_CMD_CDM_DISPENSE = new Regex(@"CashDispenser.*EXECUTE.302.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_PRESENT = new Regex(@"CashDispenser.*EXECUTE.303.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_REJECT = new Regex(@"CashDispenser.*EXECUTE.304.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_RETRACT = new Regex(@"CashDispenser.*EXECUTE.305.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_STARTEX = new Regex(@"CashDispenser.*EXECUTE.311.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_ENDEX = new Regex(@"CashDispenser.*EXECUTE.312.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_RESET = new Regex(@"CashDispenser.*EXECUTE.321.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);

      /* EVENTS */
      static Regex WFS_SRVE_CDM_CASHUNITINFOCHANGED = new Regex(@"CashDispenser.*SERVICE_EVENT.304.[0-9]+WFS_SERVICE_EVENT", RegexOptions.Compiled);
      static Regex WFS_SRVE_CDM_ITEMSTAKEN = new Regex(@"CashDispenser.*SERVICE_EVENT.309.[0-9]+WFS_SERVICE_EVENT", RegexOptions.Compiled);


      /* 3 - CDM  COIN */
      /* INFO */
      static readonly Regex WFS_INF_CDM_STATUS_COIN = new Regex(@"CoinDispenser.*GETINFO.301.[0-9]+WFS_GETINFO_COMPLETE", RegexOptions.Compiled);
      static readonly Regex WFS_INF_CDM_CASH_UNIT_INFO_COIN = new Regex(@"CoinDispenser.*GETINFO\[303\][0-9]+WFS_GETINFO_COMPLETE", RegexOptions.Compiled);
      static readonly Regex WFS_INF_CDM_PRESENT_STATUS_COIN = new Regex(@"CoinDispenser.*GETINFO.309.[0-9]+WFS_GETINFO_COMPLETE", RegexOptions.Compiled);

      /* EXECUTE */
      static Regex WFS_CMD_CDM_DISPENSE_COIN = new Regex(@"CoinDispenser.*EXECUTE.302.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_PRESENT_COIN = new Regex(@"CoinDispenser.*EXECUTE.303.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_REJECT_COIN = new Regex(@"CoinDispenser.*EXECUTE.304.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_RETRACT_COIN = new Regex(@"CoinDispenser.*EXECUTE.305.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_STARTEX_COIN = new Regex(@"Coin.*EXECUTE.311.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_ENDEX_COIN = new Regex(@"CoinDispenser.*EXECUTE.312.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);
      static Regex WFS_CMD_CDM_RESET_COIN = new Regex(@"CoinDispenser.*EXECUTE.321.[0-9]+WFS_EXECUTE_COMPLETE", RegexOptions.Compiled);

      /* EVENTS */
      static Regex WFS_SRVE_CDM_CASHUNITINFOCHANGED_COIN = new Regex(@"CoinDispenser.*SERVICE_EVENT.304.[0-9]+WFS_SERVICE_EVENT.*Coin", RegexOptions.Compiled);
      static Regex WFS_SRVE_CDM_ITEMSTAKEN_COIN = new Regex(@"CoinDispenser.*SERVICE_EVENT.309.[0-9]+WFS_SERVICE_EVENT.*Coin", RegexOptions.Compiled);


      /* 4 - PIN */
      /* INFO */
      static Regex WFS_INF_PIN_STATUS = new Regex("GETINFO.401.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_CAPABILITIES = new Regex("GETINFO.402.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_KEY_DETAIL = new Regex("GETINFO.404.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_FUNCKEY_DETAIL = new Regex("GETINFO.405.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_HSM_TDATA = new Regex("GETINFO.406.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_KEY_DETAIL_EX = new Regex("GETINFO.407.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_SECUREKEY_DETAIL = new Regex("GETINFO.408.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_QUERY_LOGICAL_HSM_DETAIL = new Regex("GETINFO.409.[0-9]+WFS_GETINFO_COMPLETE");
      static Regex WFS_INF_PIN_QUERY_PCIPTS_DEVICE_ID = new Regex("GETINFO.410.[0-9]+WFS_GETINFO_COMPLETE");

      static Regex WFS_CMD_PIN_CRYPT = new Regex("EXECUTE.401.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_IMPORT_KEY = new Regex("EXECUTE.403.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_GET_PIN = new Regex("EXECUTE.405.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_GET_PINBLOCK = new Regex("EXECUTE.407.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_GET_DATA = new Regex("EXECUTE.408.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_INITIALIZATION = new Regex("EXECUTE.409.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_LOCAL_DES = new Regex("EXECUTE.410.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_LOCAL_EUROCHEQUE = new Regex("EXECUTE.411.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_LOCAL_VISA = new Regex("EXECUTE.412.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_CREATE_OFFSET = new Regex("EXECUTE.413.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_DERIVE_KEY = new Regex("EXECUTE.414.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_PRESENT_IDC = new Regex("EXECUTE.415.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_LOCAL_BANKSYS = new Regex("EXECUTE.416.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_BANKSYS_IO = new Regex("EXECUTE.417.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_RESET = new Regex("EXECUTE.418.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_HSM_SET_TDATA = new Regex("EXECUTE.419.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_SECURE_MSG_SEND = new Regex("EXECUTE.420.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_SECURE_MSG_RECEIVE = new Regex("EXECUTE.421.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_GET_JOURNAL = new Regex("EXECUTE.422.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_IMPORT_KEY_EX = new Regex("EXECUTE.423.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_ENC_IO = new Regex("EXECUTE.424.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_HSM_INIT = new Regex("EXECUTE.425.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_IMPORT_RSA_PUBLIC_KEY = new Regex("EXECUTE.426.[0-9]+WFS_EXECUTE_COMPLETE");

      static Regex WFS_CMD_PIN_EXPORT_RSA_ISSUER_SIGNED_ITEM = new Regex("EXECUTE.427.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_IMPORT_RSA_SIGNED_DES_KEY = new Regex("EXECUTE.428.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_GENERATE_RSA_KEY_PAIR = new Regex("EXECUTE.429.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_EXPORT_RSA_EPP_SIGNED_ITEM = new Regex("EXECUTE.430.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_LOAD_CERTIFICATE = new Regex("EXECUTE.431.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_GET_CERTIFICATE = new Regex("EXECUTE.432.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_REPLACE_CERTIFICATE = new Regex("EXECUTE.433.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_START_KEY_EXCHANGE = new Regex("EXECUTE.434.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_IMPORT_RSA_ENCIPHERED_PKCS7_KEY = new Regex("EXECUTE.435.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_EMV_IMPORT_PUBLIC_KEY = new Regex("EXECUTE.436.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_DIGEST = new Regex("EXECUTE.437.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_SECUREKEY_ENTRY = new Regex("EXECUTE.438.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_GENERATE_KCV = new Regex("EXECUTE.439.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_SET_GUIDANCE_LIGHT = new Regex("EXECUTE.441.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_MAINTAIN_PIN = new Regex("EXECUTE.442.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_KEYPRESS_BEEP = new Regex("EXECUTE.443.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_SET_PINBLOCK_DATA = new Regex("EXECUTE.444.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_SET_LOGICAL_HSM = new Regex("EXECUTE.445.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_IMPORT_KEYBLOCK = new Regex("EXECUTE.446.[0-9]+WFS_EXECUTE_COMPLETE");
      static Regex WFS_CMD_PIN_POWER_SAVE_CONTROL = new Regex("EXECUTE.447.[0-9]+WFS_EXECUTE_COMPLETE");


      static Regex WFS_EXEE_PIN_KEY = new Regex("EXECUTE_EVENT.401.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_PIN_INITIALIZED = new Regex("SERVICE_EVENT.402.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_PIN_ILLEGAL_KEY_ACCESS = new Regex("SERVICE_EVENT.403.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_PIN_OPT_REQUIRED = new Regex("SERVICE_EVENT.404.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_PIN_HSM_TDATA_CHANGED = new Regex("SERVICE_EVENT.405.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_PIN_CERTIFICATE_CHANGE = new Regex("SERVICE_EVENT.406.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_PIN_HSM_CHANGED = new Regex("SERVICE_EVENT.407.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_EXEE_PIN_ENTERDATA = new Regex("EXECUTE_EVENT.408.[0-9]+WFS_EXECUTE_EVENT");
      static Regex WFS_SRVE_PIN_DEVICEPOSITION = new Regex("SERVICE_EVENT.409.[0-9]+WFS_SERVICE_EVENT");
      static Regex WFS_SRVE_PIN_POWER_SAVE_CHANGE = new Regex("SERVICE_EVENT.410.[0-9]+WFS_SERVICE_EVENT");


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
      static Regex WFS_INF_SIU_STATUS = new Regex("GETINFO.801.[0-9]+WFS_GETINFO_COMPLETE");

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

      // implementations of the ILogLine interface
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
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      protected virtual void Initialize(int lUnitCount = 1)
      {
         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
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

      private static bool IsMyLine(string logLine, string myDigit)
      {
         string getInfo = "GETINFO[" + myDigit;
         string execute = "EXECUTE[" + myDigit;
         string serviceEvent = "SERVICE_EVENT[" + myDigit;
         string executeEvent = "EXECUTE_EVENT[" + myDigit;
         string userEvent = "USER_EVENT[" + myDigit;

         return logLine.Contains(getInfo) || logLine.Contains(execute) || logLine.Contains(serviceEvent) || logLine.Contains(executeEvent) || logLine.Contains(userEvent);
      }

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

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         (bool success, string subLogLine) result;

         /* 2 - IDC */
         if (IsMyLine(logLine, "2"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_IDC_STATUS, logLine);
            if (result.success) return new WFSIDCSTATUS(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_INF_IDC_CAPABILITIES, logLine);
            if (result.success) return new WFSIDCCAPABILITIES(logFileHandler, result.subLogLine);

            /* Test for EXEC */
            result = GenericMatch(WFS_CMD_IDC_READ_RAW_DATA, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IDC_READ_RAW_DATA);

            result = GenericMatch(WFS_CMD_IDC_CHIP_IO, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IDC_CHIP_IO);

            result = GenericMatch(WFS_CMD_IDC_CHIP_POWER, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IDC_CHIP_POWER);

            result = GenericMatch(WFS_CMD_IDC_CHIP_POWER, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IDC_CHIP_POWER);

            result = GenericMatch(WFS_EXEE_IDC_INVALIDTRACKDATA, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IDC_INVALIDTRACKDATA);

            result = GenericMatch(WFS_EXEE_IDC_MEDIAINSERTED, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IDC_MEDIAINSERTED);

            result = GenericMatch(WFS_SRVE_IDC_MEDIAREMOVED, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_IDC_MEDIAREMOVED);

            result = GenericMatch(WFS_USRE_IDC_RETAINBINTHRESHOLD, logLine);
            if (result.success) return new WFSIDCRETAINBINTHRESHOLD(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_EXEE_IDC_INVALIDMEDIA, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IDC_INVALIDMEDIA);

            result = GenericMatch(WFS_EXEE_IDC_MEDIARETAINED, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IDC_MEDIARETAINED);

            result = GenericMatch(WFS_SRVE_IDC_MEDIADETECTED, logLine);
            if (result.success) return new WFSIDCMEDIADETECTED(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_SRVE_IDC_RETAINBININSERTED, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_IDC_RETAINBININSERTED);

            result = GenericMatch(WFS_SRVE_IDC_RETAINBINREMOVED, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_IDC_RETAINBINREMOVED);

            result = GenericMatch(WFS_EXEE_IDC_INSERTCARD, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IDC_INSERTCARD);

            result = GenericMatch(WFS_SRVE_IDC_DEVICEPOSITION, logLine);
            if (result.success) return new WFSIDCDEVICEPOSITION(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_SRVE_IDC_POWER_SAVE_CHANGE, logLine);
            if (result.success) return new WFSIDCPOWERSAVECHANGE(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_EXEE_IDC_TRACKDETECTED, logLine);
            if (result.success) return new WFSIDCPOWERSAVECHANGE(logFileHandler, result.subLogLine);

         }

         /* 3 - CDM */
         if (IsMyLine(logLine, "3"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CDM_STATUS, logLine);
            if (result.success) return new WFSCDMSTATUS(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_INF_CDM_CASH_UNIT_INFO, logLine);
            if (result.success) return new WFSCDMCUINFO(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_INF_CDM_PRESENT_STATUS, logLine);
            if (result.success) return new WFSCDMPRESENTSTATUS(logFileHandler, result.subLogLine);

            /* Test for EXECUTE */
            result = GenericMatch(WFS_CMD_CDM_DISPENSE, logLine);
            if (result.success) return new WFSCDMDENOMINATION(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CDM_DISPENSE);

            result = GenericMatch(WFS_CMD_CDM_PRESENT, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CDM_PRESENT);

            result = GenericMatch(WFS_CMD_CDM_REJECT, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CDM_REJECT);

            result = GenericMatch(WFS_CMD_CDM_RETRACT, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CDM_RETRACT);

            result = GenericMatch(WFS_CMD_CDM_RESET, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CDM_RESET);

            result = GenericMatch(WFS_CMD_CDM_STARTEX, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CDM_STARTEX);

            result = GenericMatch(WFS_CMD_CDM_ENDEX, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CDM_ENDEX);

            /* Test for EVENTS */
            result = GenericMatch(WFS_SRVE_CDM_CASHUNITINFOCHANGED, logLine);
            if (result.success) return new WFSCDMCUINFO(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED);

            result = GenericMatch(WFS_SRVE_CDM_ITEMSTAKEN, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_CDM_ITEMSTAKEN);

            /* We should have matched something */
            return new SPLine(logFileHandler, logLine, XFSType.Error);
         }

         /* 4 - PIN */
         if (IsMyLine(logLine, "4"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_PIN_STATUS, logLine);
            if (result.success) return new WFSPINSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_PIN_STATUS);

            result = GenericMatch(WFS_CMD_PIN_GET_PIN, logLine);
            if (result.success) return new WFSPINGETPIN(logFileHandler, result.subLogLine);

            result = GenericMatch(WFS_CMD_PIN_GET_PINBLOCK, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_PIN_GET_PINBLOCK);

            result = GenericMatch(WFS_CMD_PIN_GET_DATA, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_PIN_GET_DATA);

            result = GenericMatch(WFS_CMD_PIN_RESET, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_PIN_RESET);

            result = GenericMatch(WFS_EXEE_PIN_KEY, logLine);
            if (result.success) return new WFSPINKEY(logFileHandler, result.subLogLine);




         }

         /* 5 - CHK */
         if (IsMyLine(logLine, "5"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CHK_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_CHK_STATUS);
         }

         /* 6 - DEP */
         if (IsMyLine(logLine, "6"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_DEP_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_DEP_STATUS);
         }

         /* 7 - TTU */
         if (IsMyLine(logLine, "7"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_TTU_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_TTU_STATUS);
         }

         /* 8 - SIU */
         if (IsMyLine(logLine, "8"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_SIU_STATUS, logLine);
            if (result.success) return new WFSSIUSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_SIU_STATUS);
         }

         /* 9 - VDM */
         if (IsMyLine(logLine, "9"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_VDM_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_VDM_STATUS);
         }

         /* 10 - CAM */
         if (IsMyLine(logLine, "10"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CAM_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_CAM_STATUS);
         }

         /* 11 - ALM */
         if (IsMyLine(logLine, "11"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_ALM_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_ALM_STATUS);
         }

         /* 12 - CEU */
         if (IsMyLine(logLine, "12"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CEU_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_CEU_STATUS);
         }

         /* 13 - CIM */
         if (IsMyLine(logLine, "13"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CIM_STATUS, logLine);
            if (result.success) return new WFSCIMSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_CIM_STATUS);

            result = GenericMatch(WFS_INF_CIM_CASH_UNIT_INFO, logLine);
            if (result.success) return new WFSCIMCASHINFO(logFileHandler, result.subLogLine, XFSType.WFS_INF_CIM_CASH_UNIT_INFO);

            result = GenericMatch(WFS_INF_CIM_CASH_IN_STATUS, logLine);
            if (result.success) return new WFSCIMCASHINSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_CIM_CASH_IN_STATUS);

            /* Test for EXECUTE */
            result = GenericMatch(WFS_CMD_CIM_CASH_IN_START, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CIM_CASH_IN_START);

            result = GenericMatch(WFS_CMD_CIM_CASH_IN, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CIM_CASH_IN);

            result = GenericMatch(WFS_CMD_CIM_CASH_IN_END, logLine);
            if (result.success) return new WFSCIMCASHINFO(logFileHandler, logLine, XFSType.WFS_CMD_CIM_CASH_IN_END);

            result = GenericMatch(WFS_CMD_CIM_CASH_IN_ROLLBACK, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK);

            result = GenericMatch(WFS_CMD_CIM_RETRACT, logLine);
            if (result.success) return new WFSCIMCASHINFO(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CIM_RETRACT);

            result = GenericMatch(WFS_CMD_CIM_RESET, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CIM_RESET);

            result = GenericMatch(WFS_CMD_CIM_START_EXCHANGE, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CIM_STARTEX);

            result = GenericMatch(WFS_CMD_CIM_END_EXCHANGE, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_CIM_ENDEX);

            /* Test for EVENTS */
            result = GenericMatch(WFS_USRE_CIM_CASHUNITTHRESHOLD, logLine);
            if (result.success) return new WFSCIMCASHINFO(logFileHandler, result.subLogLine, XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD);

            result = GenericMatch(WFS_SRVE_CIM_CASHUNITINFOCHANGED, logLine);
            if (result.success) return new WFSCIMCASHINFO(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED);

            result = GenericMatch(WFS_SRVE_CIM_ITEMSTAKEN, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_CIM_ITEMSTAKEN);

            result = GenericMatch(WFS_EXEE_CIM_INPUTREFUSE, logLine);
            if (result.success) return new WFSCIMINPUTREFUSE(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_CIM_INPUTREFUSE);

            result = GenericMatch(WFS_SRVE_CIM_ITEMSPRESENTED, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_CIM_ITEMSPRESENTED);

            result = GenericMatch(WFS_SRVE_CIM_ITEMSINSERTED, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_CIM_ITEMSINSERTED);

            result = GenericMatch(WFS_EXEE_CIM_NOTEERROR, logLine);
            if (result.success) return new WFSCIMNOTEERROR(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_CIM_NOTEERROR);

            result = GenericMatch(WFS_SRVE_CIM_MEDIADETECTED, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_CIM_MEDIADETECTED);

            /* We should have matched something */
            return new SPLine(logFileHandler, logLine, XFSType.Error);
         }

         /* 14 - CRD */
         if (IsMyLine(logLine, "14"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_CRD_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_CRD_STATUS);
         }

         /* 15 - BCR */
         if (IsMyLine(logLine, "15"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_BCR_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_BCR_STATUS);
         }

         /* 16 - IPM */
         if (IsMyLine(logLine, "16"))
         {

            /* Test for INFO */
            result = GenericMatch(WFS_INF_IPM_STATUS, logLine);
            if (result.success) return new WFSIPMSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_IPM_STATUS);

            result = GenericMatch(WFS_INF_IPM_MEDIA_BIN_INFO, logLine);
            if (result.success) return new WFSIPMMEDIABININFO(logFileHandler, result.subLogLine, XFSType.WFS_INF_IPM_MEDIA_BIN_INFO);

            result = GenericMatch(WFS_INF_IPM_TRANSACTION_STATUS, logLine);
            if (result.success) return new WFSIPMTRANSSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_IPM_TRANSACTION_STATUS);

            /* Test for EXECUTE */
            result = GenericMatch(WFS_CMD_IPM_MEDIA_IN, logLine);
            if (result.success) return new WFSIPMMEDIAIN(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_MEDIA_IN);

            result = GenericMatch(WFS_CMD_IPM_MEDIA_IN_END, logLine);
            if (result.success) return new WFSIPMMEDIAINEND(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_MEDIA_IN_END);

            result = GenericMatch(WFS_CMD_IPM_MEDIA_IN_ROLLBACK, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_MEDIA_IN_ROLLBACK);

            result = GenericMatch(WFS_CMD_IPM_PRESENT_MEDIA, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_PRESENT_MEDIA);

            result = GenericMatch(WFS_CMD_IPM_RETRACT_MEDIA, logLine);
            if (result.success) return new WFSIPMRETRACTMEDIAOUT(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_RETRACT_MEDIA);

            result = GenericMatch(WFS_CMD_IPM_PRINT_TEXT, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_PRINT_TEXT);

            result = GenericMatch(WFS_CMD_IPM_RESET, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_RESET);

            result = GenericMatch(WFS_CMD_IPM_EXPEL_MEDIA, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_CMD_IPM_EXPEL_MEDIA);

            /* Test for EVENTS */
            result = GenericMatch(WFS_EXEE_IPM_MEDIAINSERTED, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAINSERTED);

            result = GenericMatch(WFS_USRE_IPM_MEDIABINTHRESHOLD, logLine);
            if (result.success) return new WFSIPMMEDIABININFO(logFileHandler, result.subLogLine, XFSType.WFS_USRE_IPM_MEDIABINTHRESHOLD);

            result = GenericMatch(WFS_SRVE_IPM_MEDIABININFOCHANGED, logLine);
            if (result.success) return new WFSIPMMEDIABININFO(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_IPM_MEDIABININFOCHANGED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIABINERROR, logLine);
            if (result.success) return new WFSIPMEDIABINERROR(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIABINERROR);

            result = GenericMatch(WFS_SRVE_IPM_MEDIATAKEN, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_IPM_MEDIATAKEN);

            result = GenericMatch(WFS_SRVE_IPM_MEDIADETECTED, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_SRVE_IPM_MEDIADETECTED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIAPRESENTED, logLine);
            if (result.success) return new SPLine(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAPRESENTED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIAREFUSED, logLine);
            if (result.success) return new WFSIPMMEDIAREFUSED(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAREFUSED);

            result = GenericMatch(WFS_EXEE_IPM_MEDIAREJECTED, logLine);
            if (result.success) return new WFSIPMMEDIAREJECTED(logFileHandler, result.subLogLine, XFSType.WFS_EXEE_IPM_MEDIAREJECTED);

            /* We should have matched something */
            return new SPLine(logFileHandler, logLine, XFSType.Error);
         }

         /* 1 - PTR */
         /* this needs to go here so we match '10', '11', '12', etc first */
         if (IsMyLine(logLine, "1"))
         {
            /* Test for INFO */
            result = GenericMatch(WFS_INF_PTR_STATUS, logLine);
            if (result.success) return new WFSDEVSTATUS(logFileHandler, result.subLogLine, XFSType.WFS_INF_PTR_STATUS);
         }

         /* WFPOpen/WFPClose */
         if (logLine.Contains("WFPOpen") || logLine.Contains("WFPClose"))
         {
            result = GenericMatch(WFPOpen, logLine);
            // we have to pass in logLine, not result.subLogLine, because there's no timestamp in the payload, 
            // we need to parse it out of the leading part of the message
            if (result.success) return new WFPOPEN(logFileHandler, logLine, XFSType.WFPOPEN);

            result = GenericMatch(WFPClose, logLine);
            // we have to pass in logLine, not result.subLogLine, because there's no timestamp in the payload, 
            // we need to parse it out of the leading part of the message
            if (result.success) return new WFPCLOSE(logFileHandler, logLine, XFSType.WFPCLOSE);
         }

         /* SysEvent */
         if (logLine.Contains("lpbDescription"))
         {
            return new SPLine(logFileHandler, logLine, XFSType.WFS_SYSEVENT);
         }

         return new SPLine(logFileHandler, logLine, XFSType.None);

      }
   }
   }
