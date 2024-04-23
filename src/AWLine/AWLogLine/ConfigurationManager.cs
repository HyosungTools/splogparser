using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class ConfigurationManager : AWLine
   {
      private string className = "ConfigurationManager";

      public string FeatureList {  get; set; } = string.Empty;
      public string SettingsList { get; set; } = string.Empty;
      public string NetOpLicence { get; set; } = string.Empty;

      public ConfigurationManager(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.ConfigurationManager) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[ConfigurationManager]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //Setting NetOp license key
            //MoveNext: feature list [{"Name":"CustomerSearch","Options":null},{"Name":"TellerControl","Options":null},{"Name":"TellerInitiatedSession","Options":null},{"Name":"CustomerIdentification","Options":null},{"Name":"InvalidCheckEditing","Options":null},{"Name":"CheckHolds","Options":null},{"Name":"PeerToPeerVideo","Options":null},{"Name":"SavedImagesEnabled","Options":null},{"Name":"RemoteDesktop","Options":null}]
            //MoveNext: settings list [{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"False"},{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"},{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"},{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"False"},{"Name":"JournalHistory","Type":"System.Int32","Value":"90"},{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"},{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"},{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="},{"Name":"SavedImagesHistory","Type":"System.Int32","Value":"90"},{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"},{"Name":"SmtpFromEmail","Type":"System.String","Value":"noreply@domain.com"},{"Name":"SmtpPassword","Type":"System.Security.SecureString","Value":"Z7BKbZXrF+PMTVNvarcz4Q=="},{"Name":"SmtpPortNumber","Type":"System.String","Value":"587"},{"Name":"SmtpServer","Type":"System.String","Value":"127.0.0.1"},{"Name":"SmtpUsername","Type":"System.String","Value":"admin"},{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"},{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"},{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"60"},{"Name":"TwilioAccountId","Type":"System.Security.SecureString","Value":"KXwVYDO5FJ1CAQSc359+C1E9GzftP0uwtPzE6jsCui9VydQ4Sm9NKUMZ5rWxtGlG"},{"Name":"TwilioApiKey","Type":"System.Security.SecureString","Value":"XDL1Pih/5D3gOJTc+rRnFMF3+NJHPgoaQDv61cIk62++YER61nDveWllUgZgsS/5"},{"Name":"TwilioPhoneNumber","Type":"System.String","Value":"+19375551212"},{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"7"},{"Name":"VideoRecordingHistory","Type":"System.Int32","Value":"90"}]

            string subtag = "MoveNext: feature list ";
            if (subLogLine.StartsWith(subtag))
            {
               FeatureList = subLogLine.Substring(subtag.Length);
               IsRecognized = true;
            }

            subtag = "MoveNext: settings list ";
            if (subLogLine.StartsWith(subtag))
            {
               SettingsList = subLogLine.Substring(subtag.Length);
               IsRecognized = true;
            }

            subtag = "Setting NetOp license key";
            if (subLogLine.StartsWith(subtag))
            {
               NetOpLicence = "SET";
               IsRecognized = true;
            }

            Regex regex = new Regex("System settings changed WaitForAssign=(?<value>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingsList = $"WaitForAssign={m.Groups["value"].Value}";
               IsRecognized = true;
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
