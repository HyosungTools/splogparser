using Contract;

namespace LogLineHandler
{
   public class APLineField : APLine
   {
      public string field = string.Empty;

      public APLineField(ILogFileHandler parent, string logLine, APLogType apType) : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         if (field != "")
            return; 

         string lookFor = string.Empty;
         char[] trimChars = { '[', ']', '.','\n','\r' };
         int idx; 

         switch (this.apType)
         {
            case APLogType.APLOG_SETTINGS_CONFIG:
               {
                  lookFor = "Adding xml file: ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_CURRENTMODE:
               {
                  lookFor = "Current Mode: ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_HOST:
               {
                  field = "disconnect";
                  lookFor = "Host Connected";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = "connect";
                  }
                  break;
               }
            case APLogType.APLOG_CARD_PAN:
            case APLogType.APLOG_DEVICE_FITNESS:
               {
                  lookFor = ":";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_CARD_READAVAILABLERAWDATA:
               {
                  lookFor = "Arg:";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  else
                  {
                     lookFor = "result is";
                     idx = logLine.LastIndexOf(lookFor);
                     if (idx != -1)
                     {
                        field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }
            case APLogType.APLOG_EMV_BUILD_CANDIDATE_LIST:
               {
                  lookFor = "Result =";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_EMV_CREATE_APPNAME_LIST:
               {
                  lookFor = "AppNameList  =";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_EMV_APP_SELECTED:
               {
                  lookFor = "]";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_EMV_PAN:
               {
                  lookFor = "Device.CardReader.PANData    :";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_EMV_OFFLINE_AUTH:
               {
                  lookFor = "Return EMV_OffDataAuth() :";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_FLW_SWITCH_FIT:
               {
                  lookFor = "Next State is to be";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_PIN_ISPCI:
               {
                  lookFor = "This is ";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_PIN_ISTR31:
               {
                  lookFor = "is supported";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = "TR31";
                  }
                  break;
               }
            case APLogType.APLOG_PIN_ISTR34:
               {
                  lookFor = "is supported";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = "TR34";
                  }
                  break;
               }
            case APLogType.APLOG_DISPLAYLOAD:
               {
                  lookFor = "for screen ";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_SCREENWINDOW:
               {
                  lookFor = "[";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_LOCALXMLHELPER_ABOUT_TO_EXECUTE:
               {
                  lookFor = "About to execute: Class: HelperFunctions, Method: ";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.APLOG_STATE_CREATED:
               {
                  lookFor = "State created: ";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length);
                     idx = field.IndexOf("(");
                     if (idx != -1)
                     {
                        field = field.Substring(0, idx - 1).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }
            case APLogType.APLOG_FUNCTIONKEY_SELECTED:
               {
                  lookFor = "Raising FunctionKeySelected event with values FunctionKey";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length);
                     idx = field.IndexOf(",");
                     if (idx != -1)
                     {
                        field = field.Substring(0, idx - 1).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }
            case APLogType.APLOG_FUNCTIONKEY_SELECTED2:
               {
                  lookFor = " The ";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length);
                     idx = field.IndexOf(" ");
                     if (idx != -1)
                     {
                        field = field.Substring(0, idx).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }
            case APLogType.HelperFunctions_GetConfiguredBillMixList:
               {
                  lookFor = "ConfiguredBillMixList:";

                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }
            case APLogType.HelperFunctions_GetFewestBillMixList:
               {
                  lookFor = "FewestBillMixList:";

                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim();
                  }
                  break;
               }
            case APLogType.APLOG_RFID_WAITCOMMANDCOMPLETE:
            case APLogType.APLOG_RFID_COMMAND_COMPLETE_ERROR:
            {
                  lookFor = "[";

                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
            }

            case APLogType.APLOG_ACCOUNT_ENTERED:
               {
                  lookFor = "=";

                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case APLogType.APLOG_OPERATOR_MENU:
               {
                  lookFor = ":";

                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars);
                  }
                  break;
               }

            case APLogType.APLOG_ERROR:
               {
                  lookFor = "]";

                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim().Trim(trimChars).Replace(']',',');
                  }
                  break;
               }
            case APLogType.APLOG_EXCEPTION:
               {
                  lookFor = "EXCEPTION MESSAGE :";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx).Trim().Trim(trimChars);
                     lookFor = "\n";
                     idx = field.IndexOf(lookFor);
                     if (idx != -1)
                     {
                        field = field.Substring(0, idx).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }
         }
      }
   }
}
