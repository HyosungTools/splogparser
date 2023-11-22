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
