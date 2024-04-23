using Contract;

namespace LogLineHandler
{
   public class APLineField : APLine
   {
      public string field = string.Empty;

      public APLineField(ILogFileHandler parent, string logLine, APLogType apType,  string field = "") : base(parent, logLine, apType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         if (field != "")
            return; 

         string lookFor = string.Empty;
         int idx; 

         switch (this.apType)
         {
            case APLogType.APLOG_SETTINGS_CONFIG:
               {
                  lookFor = "Adding xml file: ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim();
                  }
                  break;
               }
            case APLogType.APLOG_CURRENTMODE:
               {
                  lookFor = "Current Mode: ";
                  idx = logLine.IndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim();
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
                     field = logLine.Substring(idx + lookFor.Length).Trim();
                  }
                  break;
               }
            case APLogType.APLOG_FLW_SWITCH_FIT:
               {
                  lookFor = "Next State is to be";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim();
                  }
                  break;
               }
            case APLogType.APLOG_PIN_ISPCI:
               {
                  lookFor = "This is ";
                  idx = logLine.LastIndexOf(lookFor);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + lookFor.Length).Trim();
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
                     char[] trimChars = { '[', ']', '.'};
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
                     char[] trimChars = { '[', ']', '.' };
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
                        field = field.Substring(0, idx - 1).Trim();
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
                        char[] trimChars = { '[', ']', '.' };
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
                        char[] trimChars = { '[', ']', '.' };
                        field = field.Substring(0, idx).Trim().Trim(trimChars);
                     }
                  }
                  break;
               }
            case APLogType.HelperFunctions_GetConfiguredBillMixList:
               {
                  string findMe = "ConfiguredBillMixList:";

                  idx = logLine.LastIndexOf(findMe);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + findMe.Length).Trim();
                  }
                  break;
               }
            case APLogType.HelperFunctions_GetFewestBillMixList:
               {
                  string findMe = "FewestBillMixList:";

                  idx = logLine.LastIndexOf(findMe);
                  if (idx != -1)
                  {
                     field = logLine.Substring(idx + findMe.Length).Trim();
                  }
                  break;
               }

         }
      }
   }
}
