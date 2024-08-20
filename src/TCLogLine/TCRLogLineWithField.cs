using Contract;

namespace LogLineHandler
{
   public class TCRLogLineWithField : TCRLogLine
   {
      public string field = string.Empty;

      public TCRLogLineWithField(ILogFileHandler parent, string logLine, TCRLogType tcrType, string field = "") : base(parent, logLine, tcrType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         if (field != "")
            return;

         field = "";

         string lookFor = string.Empty;
         int idx;
         char[] trimChars = { '[', ']', '.', '-', ',', ' ' };

         switch (this.tcrType)
         {
            case TCRLogType.TCR_ON_UPDATE_SCREENDATA:
               {
                  lookFor = "Display has been updated for screen ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case TCRLogType.TCR_CHANGING_MODE:
               {
                  lookFor = "---------------Trying to change mode to ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case TCRLogType.TCR_CURRENTMODE:
               {
                  lookFor = "MoniPlus2.FW.Mode.ModeFramework.m_CurrentModeName value has been set to ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }


            case TCRLogType.TCR_NEXTSTATE:
               {
                  lookFor = "State created: ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     field = field.Replace("(TCRCommonFlowPoint)", "")
                                  .Replace("(TCRDeviceControlFlowPoint)", "")
                                  .Replace("(TCRCommonFlowPoint)", "")
                                  .Replace("()", "")
                                  .Replace("(StandardFlowPoint)", "").Trim(trimChars);
                     if (field == "000")
                     {
                        field = "Idle";
                     }
                  }
                  break;
               }


            case TCRLogType.TCR_DEP_TELLERID:
            case TCRLogType.TCR_WD_TELLERID:
               {
                  lookFor = "TellerID=";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case TCRLogType.TCR_DEP_RESULT:
            case TCRLogType.TCR_WD_RESULT:
               {
                  lookFor = "Result=";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case TCRLogType.TCR_DEP_ERRORCODE:
            case TCRLogType.TCR_WD_ERRORCODE:
               {
                  lookFor = "ErrorCode=";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case TCRLogType.TCR_DEP_AMOUNT:
            case TCRLogType.TCR_WD_AMOUNT:
               {
                  lookFor = "Amount=[USD";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case TCRLogType.TCR_DEP_BALANCE:
            case TCRLogType.TCR_WD_BALANCE:
               {
                  lookFor = "Balance=[USD";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case TCRLogType.TCR_HOST_CMD:
               {
                  lookFor = "ProcessHostReceivedData:";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     lookFor = ",";
                     idx = field.IndexOf(lookFor);
                     if (idx != -1)
                     {
                        field = field.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }

            case TCRLogType.TCR_HOST_CMD_RESPONSE:
               {
                  lookFor = "Send the result of the InquiryCommand:";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     lookFor = ",";
                     idx = field.IndexOf(lookFor);
                     if (idx != -1)
                     {
                        field = field.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }

            case TCRLogType.TCR_HOST_RECEIVED_DATA:
               {
                  lookFor = "ProcessHostReceivedData:";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     lookFor = ",";
                     idx = field.IndexOf(lookFor);
                     if (idx != -1)
                     {
                        field = field.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }

            case TCRLogType.TCR_ATM_SENDMESSAGE_SYNC:
               {
                  lookFor = "[";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars).Trim(']');
                  }
                  break;
               }
         }
      }
   }
}
