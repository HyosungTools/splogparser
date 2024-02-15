using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace AWView
{
   class AWTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public AWTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         return base.WriteExcelFile();
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         /*
          * 
[2023-09-13 10:02:12-399][3][Settings            ]UserSettings Path: C:\Users\lpina\AppData\Local\Nautilus_Hyosung\NH.ActiveTeller.Client.ex_Url_qnalorbjt4q0zmjb1ac1ptpbvvqeo5na\1.3.0.0\user.config
[2023-09-13 10:02:20-046][2][StringResourceManager]There is no StringResource\ResourceDictionary.xaml
[2023-09-13 10:02:20-816][3][ConfigurationManager]MoveNext: settings list [{"Name":"AutoAssignRemoteTeller","Type":"System.Boolean","Value":"False"},{"Name":"DefaultTerminalFeatureSet","Type":"System.Int32","Value":"1"},{"Name":"DefaultTerminalProfile","Type":"System.Int32","Value":"1"},{"Name":"ForceSkypeSignIn","Type":"System.Boolean","Value":"False"},{"Name":"JournalHistory","Type":"System.Int32","Value":"90"},{"Name":"NetOpLicenseKey","Type":"System.String","Value":"*AAC54RBFBJKLGEFBCHB2W2NX6KHG8PNCE9ER6SVT4V6KBW43SUEMXPNW6E5KGZNLK58JVJC74KVZC2S8OHS7SN8P3XSLJLCESRL4CB4EBCDEJZDHJAWKIZ4#"},{"Name":"RemoteDesktopAvailability","Type":"DropDownList","Value":"Always"},{"Name":"RemoteDesktopPassword","Type":"System.Security.SecureString","Value":"xnXpgq6Zgi5XLqZd1XUgwA=="},{"Name":"SavedImagesHistory","Type":"System.Int32","Value":"90"},{"Name":"SingleSignOn","Type":"System.Boolean","Value":"False"},{"Name":"SmtpFromEmail","Type":"System.String","Value":"noreply@domain.com"},{"Name":"SmtpPassword","Type":"System.Security.SecureString","Value":"Z7BKbZXrF+PMTVNvarcz4Q=="},{"Name":"SmtpPortNumber","Type":"System.String","Value":"587"},{"Name":"SmtpServer","Type":"System.String","Value":"127.0.0.1"},{"Name":"SmtpUsername","Type":"System.String","Value":"admin"},{"Name":"TraceLogHistory","Type":"System.Int32","Value":"120"},{"Name":"TransactionResultHistory","Type":"System.Int32","Value":"30"},{"Name":"TransactionTypeHistory","Type":"System.Int32","Value":"60"},{"Name":"TwilioAccountId","Type":"System.Security.SecureString","Value":"KXwVYDO5FJ1CAQSc359+C1E9GzftP0uwtPzE6jsCui9VydQ4Sm9NKUMZ5rWxtGlG"},{"Name":"TwilioApiKey","Type":"System.Security.SecureString","Value":"XDL1Pih/5D3gOJTc+rRnFMF3+NJHPgoaQDv61cIk62++YER61nDveWllUgZgsS/5"},{"Name":"TwilioPhoneNumber","Type":"System.String","Value":"+19375551212"},{"Name":"UploadedFileHistory","Type":"System.Int32","Value":"7"},{"Name":"VideoRecordingHistory","Type":"System.Int32","Value":"90"}]
[2023-09-13 10:02:21-717][3][BeeHDVideoControl   ]Initialize: Video client initializing.
[2023-09-13 10:02:21-729][3][VideoManager        ]Attempting to sign in to BeeHd video: uri=, user name=lpina
[2023-09-13 10:04:05-435][3][SignInManager       ]ActiveTeller sign-in received a Success response.
[2023-09-13 10:04:05-439][3][PermissionsManager  ]Retrieving the user's permissions
[2023-09-13 10:04:07-923][3][MainWindow          ]PeerToPeer_SignIn. SignIn URI = 192.168.20.144
[2023-09-13 10:04:15-583][3][IdleEmpty           ]---------PROCESS INFORMATION----------------
[2023-09-13 10:04:16-087][3][ConnectionManager   ]ActiveTeller connection state change Connecting
[2023-09-13 10:04:16-170][3][DataFlowManager     ]ActiveTeller connection state changed to Connected
[2023-09-13 10:05:14-830][3][DeviceFactory       ]Bill dispenser device state  {"SafeDoorStatus":"1","DispenserStatus":"0","IntermediateStackerStatus":"0","ShutterStatus":"0","PositionStatus":"0","TransportStatus":"0","TransportStatusStatus":"0","DevicePositionStatus":"0","PowerSaveRecoveryTime":0,"UnitCurrencyID":["   ","   ","USD","USD","USD","USD"],"UnitValue":[0,0,1,5,20,100],"UnitStatus":["0","0","0","0","0","0"],"UnitCount":[0,55,1327,527,1994,1001],"UnitType":["RETRACTCASSETTE","REJECTCASSETTE","RECYCLING","RECYCLING","RECYCLING","RECYCLING"],"LogicalServiceName":"","ExtraInformation":""}
         */

         if (logLine is LogLineHandler.Settings seLogLine)
         {
            try
            {
               switch (seLogLine.awType)
               {
                  case AWLogType.Settings:
                     {
                        base.ProcessRow(seLogLine);
                        AddSettings(seLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow Settings EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.StringResourceManager srLogLine)
         {
            try
            {
               switch (srLogLine.awType)
               {
                  case AWLogType.StringResourceManager:
                     {
                        base.ProcessRow(srLogLine);
                        AddSettings(srLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow StringResourceManager EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.ConfigurationManager cmLogLine)
         {
            try
            {
               switch (cmLogLine.awType)
               {
                  case AWLogType.ConfigurationManager:
                     {
                        base.ProcessRow(cmLogLine);
                        AddSettings(cmLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow ConfigurationManager EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.BeeHDVideoControl beLogLine)
         {
            try
            {
               switch (beLogLine.awType)
               {
                  case AWLogType.BeeHDVideoControl:
                     {
                        base.ProcessRow(beLogLine);
                        AddSettings(beLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow BeeHDVideoControl EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.VideoManager vmLogLine)
         {
            try
            {
               switch (vmLogLine.awType)
               {
                  case AWLogType.VideoManager:
                     {
                        base.ProcessRow(vmLogLine);
                        AddSettings(vmLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow VideoManager EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.SignInManager siLogLine)
         {
            try
            {
               switch (siLogLine.awType)
               {
                  case AWLogType.SignInManager:
                     {
                        base.ProcessRow(siLogLine);
                        AddSettings(siLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow SignInManager EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.PermissionsManager pmLogLine)
         {
            try
            {
               switch (pmLogLine.awType)
               {
                  case AWLogType.PermissionsManager:
                     {
                        base.ProcessRow(pmLogLine);
                        AddSettings(pmLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow PermissionsManager EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.MainWindow mwLogLine)
         {
            try
            {
               switch (mwLogLine.awType)
               {
                  case AWLogType.MainWindow:
                     {
                        base.ProcessRow(mwLogLine);
                        AddSettings(mwLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow MainWindow EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.IdleEmpty ieLogLine)
         {
            try
            {
               switch (ieLogLine.awType)
               {
                  case AWLogType.IdleEmpty:
                     {
                        base.ProcessRow(ieLogLine);
                        AddSettings(ieLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow IdleEmpty EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.ConnectionManager cm2LogLine)
         {
            try
            {
               switch (cm2LogLine.awType)
               {
                  case AWLogType.ConnectionManager:
                     {
                        base.ProcessRow(cm2LogLine);
                        AddSettings(cm2LogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow ConnectionManager EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.DataFlowManager dfmLogLine)
         {
            try
            {
               switch (dfmLogLine.awType)
               {
                  case AWLogType.DataFlowManager:
                     {
                        base.ProcessRow(dfmLogLine);
                        AddSettings(dfmLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow DataFlowManager EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.DeviceFactory dfLogLine)
         {
            try
            {
               switch (dfLogLine.awType)
               {
                  case AWLogType.DeviceFactory:
                     {
                        base.ProcessRow(dfLogLine);
                        AddSettings(dfLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTable.ProcessRow DeviceFactory EXCEPTION: {e}");
            }
         }
      }


      private string ListOfLongToString(List<long> list)
      {
         StringBuilder sb = new StringBuilder();
         foreach (long l in list)
         {
            sb.Append(l.ToString());
         }

         return sb.ToString();
      }

      private string DictionaryStringStringToString(Dictionary<string,string> list)
      {
         string comma = string.Empty;

         StringBuilder sb = new StringBuilder();
         foreach (KeyValuePair<string,string> kvp in list)
         {
            sb.Append($"{comma}{kvp.Key}={kvp.Value}");
            comma = ",";
         }

         return sb.ToString();
      }

      protected void AddSettings(AWLine logLine)
      {
         try
         {
            string tableName = "Workstation";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            dataRow["Payload"] = logLine.logLine;

            switch (logLine.awType)
            {
               case AWLogType.Settings:
                  Settings seLine = logLine as Settings;
                  dataRow["Settings"] = DictionaryStringStringToString(seLine.SettingDict);
                  break;

               case AWLogType.StringResourceManager:
                  StringResourceManager srLine = logLine as StringResourceManager;
                  dataRow["Settings"] = DictionaryStringStringToString(srLine.SettingDict);
                  break;

               case AWLogType.ConfigurationManager:
                  ConfigurationManager cfLine = logLine as ConfigurationManager;
                  dataRow["Settings"] = $"features, {cfLine.FeatureList}, settings {cfLine.SettingsList}, NetOpLicense {cfLine.NetOpLicence}";
                  break;

               case AWLogType.BeeHDVideoControl:
                  BeeHDVideoControl beLine = logLine as BeeHDVideoControl;
                  dataRow["ServerSigninState"] = beLine.ServerSigninState;
                  dataRow["CallDevicesState"] = beLine.CallDevicesState;
                  dataRow["VideoCallState"] = beLine.VideoCallState;
                  dataRow["VideoCallDetails"] = beLine.VideoCallDetails;
                  break;

               case AWLogType.VideoManager:
                  VideoManager vmLine = logLine as VideoManager;
                  dataRow["VideoEngineState"] = $"{vmLine.VideoEngineState} user {vmLine.User}, uri {vmLine.Uri}";
                  break;

               case AWLogType.SignInManager:
                  SignInManager siLine = logLine as SignInManager;
                  dataRow["SignInState"] = $"{siLine.SignInState} user {siLine.User}, branch {siLine.Branch}, uri {siLine.Uri}";
                  dataRow["Teller"] = siLine.User;
                  break;

               case AWLogType.PermissionsManager:
                  PermissionsManager pmLine = logLine as PermissionsManager;
                  dataRow["Permissions"] = pmLine.State;
                  break;

               case AWLogType.MainWindow:
                  MainWindow mwLine = logLine as MainWindow;
                  dataRow["ActiveTellerState"] = mwLine.ActiveTellerState;
                  dataRow["VideoSessionState"] = mwLine.VideoSessionState;
                  dataRow["Asset"] = mwLine.Asset;
                  break;

               case AWLogType.IdleEmpty:
                  IdleEmpty ieLine = logLine as IdleEmpty;
                  dataRow["ProcessStats"] = $"Memory {ieLine.Memory}, VMSize {ieLine.VMSize}, PrivateSize {ieLine.PrivateSize}, Handles {ieLine.HandleCount}";
                  break;

               case AWLogType.ConnectionManager:
                  ConnectionManager cmLine = logLine as ConnectionManager;
                  dataRow["SignalRConnectionState"] = cmLine.SignalRConnectionState;
                  dataRow["ActiveTellerConnectionState"] = cmLine.ActiveTellerConnectionState;
                  dataRow["RemoteControlSessionState"] = cmLine.RemoteControlSessionState;
                  dataRow["TellerAvailability"] = cmLine.TellerAvailability;
                  dataRow["TellerAssistSessionState"] = cmLine.TellerAssistSessionState;
                  dataRow["ServerConnectionState"] = cmLine.ServerConnectionState;
                  dataRow["HttpImageRetrievalState"] = cmLine.HttpImageRetrievalState;
                  dataRow["HttpServerRequest"] = cmLine.HttpServerRequest;
                  break;

               case AWLogType.DataFlowManager:
                  DataFlowManager dfmLine = logLine as DataFlowManager;
                  dataRow["Event"] = dfmLine.Event;
                  dataRow["Asset"] = dfmLine.Asset;
                  dataRow["ActiveTellerConnectionState"] = dfmLine.ActiveTellerConnectionState;
                  dataRow["AssistRequestEvent"] = dfmLine.AssistRequestEvent;
                  dataRow["CheckImageStatus"] = dfmLine.CheckImageStatus;
                  dataRow["IDScanImageStatus"] = dfmLine.IDScanImageStatus;
                  dataRow["ControlSessionStatus"] = dfmLine.ControlSessionStatus;
                  dataRow["RemoteControlSessionState"] = dfmLine.RemoteControlSessionState;
                  dataRow["TaskStatusEvent"] = dfmLine.TaskStatusEvent;
                  dataRow["TransactionItemStatus"] = dfmLine.TransactionItemStatus;
                  dataRow["TellerSessionRequest"] = dfmLine.TellerSessionRequest;
                  dataRow["TransactionItemStatusChange"] = dfmLine.TransactionItemStatusChange;
                  dataRow["TransactionReviewRequest"] = dfmLine.TransactionReviewRequest;
                  break;

               case AWLogType.DeviceFactory:
                  DeviceFactory dfLine = logLine as DeviceFactory;
                  dataRow["Settings"] = DictionaryStringStringToString(dfLine.SettingDict);
                  break;

               case AWLogType.Error:
               case AWLogType.None:
               default:
                  break;
            }

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddSettings Exception : " + e.Message);
         }
      }

      /*
      protected void AddNetOpExtensionEvent(LogLineHandler.NetOpExtension logLine)
      {
         try
         {
            string tableName = "NetOpEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;
            dataRow["ModelName"] = logLine.ModelName;
            dataRow["ConfigurationState"] = logLine.ConfigurationState;
            dataRow["RemoteDesktopServerState"] = logLine.RemoteDesktopServerState;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddNetOpExtensionEvent Exception : " + e.Message);
         }

         // also add to MoniPlus2sEvents table
         try
         {
            string tableName = "MoniPlus2sEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            StringBuilder sb = new StringBuilder();
            sb.Append((!string.IsNullOrEmpty(logLine.ModelName) ? $"Model {logLine.ModelName} " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.ConfigurationState) ? $"{logLine.ConfigurationState} " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.RemoteDesktopServerState) ? $"Server {logLine.RemoteDesktopServerState}" : string.Empty));

            dataRow["Netop"] = sb.ToString();

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddNetOpExtensionEvent Exception : " + e.Message);
         }
      }
*/

   }
}
