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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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
                        // AddExtensionStartedEvent(aeLogLine);
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

         if (logLine is LogLineHandler.DataFlowManager dfLogLine)
         {
            try
            {
               switch (dfLogLine.awType)
               {
                  case AWLogType.DataFlowManager:
                     {
                        base.ProcessRow(dfLogLine);
                        // AddExtensionStartedEvent(aeLogLine);
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

         if (logLine is LogLineHandler.DeviceFactory df2LogLine)
         {
            try
            {
               switch (df2LogLine.awType)
               {
                  case AWLogType.DeviceFactory:
                     {
                        base.ProcessRow(df2LogLine);
                        // AddExtensionStartedEvent(aeLogLine);
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

/*
      protected void AddExtensionStartedEvent(LogLineHandler.ExtensionStarted logLine)
      {
         try
         {
            string tableName = "ExtensionStartedEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;
            dataRow["name"] = logLine.extensionName;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddExtensionStartedEvent Exception : " + e.Message);
         }
      }

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

      protected void AddNextwareExtensionEvent(LogLineHandler.NextwareExtension logLine)
      {
         try
         {
            string tableName = "NextwareEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            dataRow["MonitoringDeviceChanges"] = logLine.MonitoringDeviceChanges;
            dataRow["MonitoringDeviceName"] = logLine.MonitoringDeviceName;
            dataRow["MonitoringElapsed"] = logLine.MonitoringElapsed;

            dataRow["Id"] = logLine.Id;
            dataRow["MacAddress"] = logLine.MacAddress;
            dataRow["DeviceId"] = logLine.DeviceId;
            dataRow["DeviceClass"] = logLine.DeviceClass;
            dataRow["DisplayName"] = logLine.DisplayName;
            dataRow["Status"] = logLine.Status;
            dataRow["AssetName"] = logLine.AssetName;
            dataRow["DeviceMediaStatus"] = logLine.DeviceMediaStatus;
            dataRow["Timestamp"] = logLine.DeviceStateTimestampUTC;
            dataRow["StatusDeviceName"] = logLine.StatusDeviceName;
            dataRow["DeviceStatus"] = logLine.DeviceStatus;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddNextwareExtensionEvent Exception : " + e.Message);
         }

         // also add to MoniPlus2sEvents table
         try
         {
            string tableName = "MoniPlus2sEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            StringBuilder sb = new StringBuilder();
            sb.Append((!string.IsNullOrEmpty(logLine.MonitoringDeviceChanges) ? $"Event {logLine.MonitoringDeviceChanges}, " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.MonitoringDeviceName) ? $"Device {logLine.MonitoringDeviceName}," : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.StatusDeviceName) ? $"Status {logLine.StatusDeviceName}" : string.Empty));

            dataRow["Netware"] = sb.ToString();

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddNextwareExtensionEvent Exception : " + e.Message);
         }
      }


      protected void AddMoniPlus2sExtensionEvent(LogLineHandler.MoniPlus2sExtension logLine)
      {
         try
         {
            string tableName = "MoniPlus2sEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            dataRow["RemoteTellerActive"] = MoniPlus2sExtension._RemoteTellerActive ? "Active" : string.Empty;

            dataRow["CustomerId"] = logLine.CustomerId;

            // general
            dataRow["ApplicationAvailability"] = logLine.ApplicationAvailability;

            dataRow["FlowTimestamp"] = logLine.FlowTimestampUTC;

            dataRow["FlowPoint"] = logLine.FlowPoint;
            dataRow["TransactionType"] = logLine.TransactionType;
            dataRow["Language"] = logLine.Language;
            dataRow["VoiceGuidance"] = logLine.VoiceGuidance ? "true" : "false";
            dataRow["RequestContext"] = logLine.RequestContext;


            // operating mode and state
            dataRow["OperatingMode"] = logLine.OperatingMode;
            dataRow["State"] = logLine.State;
            dataRow["ApplicationState"] = logLine.ApplicationState;


            // hardware devices
            dataRow["AssetName"] = logLine.AssetName;
            dataRow["EnabledDeviceList"] = logLine.EnabledDeviceList;


            // network
            dataRow["IpAddress"] = logLine.IpAddress;
            dataRow["MacAddress"] = logLine.MacAddress;


            // physical assets and status
            dataRow["Manufacturer"] = logLine.Manufacturer;
            dataRow["Model"] = logLine.Model;
            dataRow["Name"] = logLine.Name;

            StringBuilder sb = new StringBuilder();
            foreach (MoniPlus2sExtension.AssetCapabilities cap in logLine.Capabilities)
            {
               sb.Append($"{cap.ToString()};");
            }
            dataRow["Capabilities"] = sb.ToString();
            sb = null;

            dataRow["Status"] = logLine.Status;
            dataRow["StatusChangedTime"] = logLine.StatusChangedTime;
            dataRow["StatusReceivedTime"] = logLine.StatusReceivedTime;


            // operating mode
            dataRow["OperatingMode_ModeType"] = logLine.OperatingMode_ModeType;
            dataRow["OperatingMode_ModeName"] = logLine.OperatingMode_ModeName;
            dataRow["OperatingMode_CoreStatus"] = logLine.OperatingMode_CoreStatus;
            dataRow["OperatingMode_CoreProperties"] = logLine.OperatingMode_CoreProperties;


            // video conference method
            dataRow["SupportedCallType"] = logLine.SupportedCallType;
            dataRow["CallRouting_Summary"] = logLine.CallRouting_Summary;


            // Active Teller (workstation)
            dataRow["TellerId"] = logLine.TellerId;
            dataRow["TellerName"] = logLine.TellerName;
            dataRow["TellerVideoConferenceUri"] = logLine.TellerVideoConferenceUri;
            dataRow["TellerInfo_Summary"] = logLine.TellerInfo_Summary;


            // teller session request
            dataRow["TellerSessionRequest_Timestamp"] = logLine.TellerSessionRequest_TimestampUTC;


            // remote control session
            dataRow["RemoteControlSession_StartTime"] = logLine.RemoteControlSession_StartTimeUTC;
            dataRow["RemoteControlSession_TellerSessionRequestTimestamp"] = logLine.RemoteControlSession_TellerSessionRequestTimestampUTC;


            // remote control task
            dataRow["RemoteControl_TaskName"] = logLine.RemoteControl_TaskName;
            dataRow["RemoteControl_EventName"] = logLine.RemoteControl_EventName;
            dataRow["RemoteControl_AssetName"] = logLine.RemoteControl_AssetName;
            dataRow["RemoteControlTask_EventData_Name"] = logLine.RemoteControlTask_EventData_Name;
            dataRow["RemoteControlTask_EventData_TellerId"] = logLine.RemoteControlTask_EventData_TellerId;
            dataRow["RemoteControlTask_EventData_DateTime"] = logLine.RemoteControlTask_EventData_DateTimeUTC;
            dataRow["RemoteControlTask_EventData_TaskTimeout"] = logLine.RemoteControlTask_EventData_TaskTimeout;


            // accounts
            dataRow["Accounts_Action"] = ListOfLongToString(logLine.Accounts_Action);
            dataRow["Accounts_AccountType"] = string.Join(",", logLine.Accounts_AccountType);
            dataRow["Accounts_Amount"] = string.Join(",", logLine.Accounts_Amount);
            dataRow["Accounts_Summary"] = string.Join(",", logLine.Accounts_Summary);


            // transactions
            dataRow["TransactionDetail_ApproverId"] = string.Join(",", logLine.TransactionDetail_ApproverId);
            dataRow["TransactionDetail_Summary"] = string.Join(",", logLine.TransactionDetail_Summary);
            dataRow["TransactionData_Summary"] = string.Join(",", logLine.TransactionData_Summary);
            dataRow["Transaction_Warnings_Summary"] = string.Join(",", logLine.Transaction_Warnings_Summary);
            dataRow["TransactionOtherAmounts_Summary"] = string.Join(",", logLine.TransactionOtherAmounts_Summary);


            // checks
            dataRow["Checks_Amount"] = string.Join(",", logLine.Checks_Amount);
            dataRow["Checks_AcceptStatus"] = string.Join(",", logLine.Checks_AcceptStatus);
            dataRow["Checks_AmountRead"] = string.Join(",", logLine.Checks_AmountRead);
            dataRow["Checks_AmountScore"] = ListOfLongToString(logLine.Checks_AmountScore);
            dataRow["Checks_BackImageRelativeUri"] = string.Join(",", logLine.Checks_BackImageRelativeUri);
            dataRow["Checks_CheckDateRead"] = string.Join(",", logLine.Checks_CheckDateRead);
            dataRow["Checks_CheckDateScore"] = ListOfLongToString(logLine.Checks_CheckDateScore);
            dataRow["Checks_CheckIndex"] = ListOfLongToString(logLine.Checks_CheckIndex);
            dataRow["Checks_FrontImageRelativeUri"] = string.Join(",", logLine.Checks_FrontImageRelativeUri);
            dataRow["Checks_ImageBack"] = string.Join(",", logLine.Checks_ImageBack);
            dataRow["Checks_ImageFront"] = string.Join(",", logLine.Checks_ImageFront);
            dataRow["Checks_InvalidReason"] = string.Join(",", logLine.Checks_InvalidReason);
            dataRow["Checks_Summary"] = string.Join(",", logLine.Checks_Summary);


            // cash
            dataRow["CashDetails_Amount"] = string.Join(",", logLine.CashDetails_Amount);
            dataRow["CashDetails_CashTransactionType"] = ListOfLongToString(logLine.CashDetails_CashTransactionType);
            dataRow["CashDetails_Currency"] = string.Join(",", logLine.CashDetails_Currency);
            dataRow["CashDetails_Summary"] = string.Join(",", logLine.CashDetails_Summary);


            // currency
            dataRow["CurrencyItems_Value"] = ListOfLongToString(logLine.CurrencyItems_Value);
            dataRow["CurrencyItems_Quantity"] = ListOfLongToString(logLine.CurrencyItems_Quantity);
            dataRow["CurrencyItems_MediaType"] = ListOfLongToString(logLine.CurrencyItems_MediaType);
            dataRow["CurrencyItems_Summary"] = string.Join(",", logLine.CurrencyItems_Summary);


            // id scans
            dataRow["IdScans_Summary"] = string.Join(",", logLine.IdScans_Summary);


            // reviews
            dataRow["Review_ReasonForReview"] = ListOfLongToString(logLine.Review_ReasonForReview);
            dataRow["Review_TellerAmount"] = string.Join(",", logLine.Review_TellerAmount);
            dataRow["Review_TellerApproval"] = ListOfLongToString(logLine.Review_TellerApproval);
            dataRow["Review_Reason"] = string.Join(",", logLine.Review_Reason);
            dataRow["Review_Summary"] = string.Join(",", logLine.Review_Summary);

            // application (server) comms - RESTful requests
            dataRow["CommunicationResult_Comment"] = logLine.CommunicationResult_Comment;
            dataRow["RestResource"] = logLine.RestResource;
            dataRow["MessageBody"] = logLine.MessageBody;
            dataRow["HttpRequest"] = logLine.HttpRequest;
            dataRow["ApplicationConnectionState"] = logLine.ApplicationConnectionState;


            // internal IDs
            dataRow["Asset_Id"] = logLine.Asset_Id;
            dataRow["AssetState_Id"] = logLine.AssetState_Id;

            dataRow["ApplicationState_Id"] = logLine.ApplicationState_Id;

            dataRow["TransactionDetail_Id"] = logLine.TransactionDetail_Id;
            dataRow["TransactionDetail_TellerSessionActivityId"] = logLine.TransactionDetail_TellerSessionActivityId;

            dataRow["SessionRequest_Id"] = logLine.SessionRequest_Id;

            dataRow["RemoteControlSession_Id"] = logLine.RemoteControlSession_Id;
            dataRow["RemoteControlSession_TellerSessionRequestId"] = logLine.RemoteControlSession_TellerSessionRequestId;

            dataRow["RemoteControl_TaskId"] = logLine.RemoteControl_TaskId;

            dataRow["TellerSession_Id"] = logLine.TellerSession_Id;
            dataRow["TellerSessionRequest_Id"] = logLine.TellerSessionRequest_Id;

            dataRow["TellerInfo_ClientSessionId"] = logLine.TellerInfo_ClientSessionId;

            dataRow["CashDetails_Id"] = ListOfLongToString(logLine.CashDetails_Id);

            dataRow["CurrencyItems_Id"] = ListOfLongToString(logLine.CurrencyItems_Id);
            dataRow["CurrencyItems_CashDetailId"] = ListOfLongToString(logLine.CurrencyItems_CashDetailId);

            dataRow["Checks_Id"] = ListOfLongToString(logLine.Checks_Id);
            dataRow["Checks_TransactionDetailId"] = ListOfLongToString(logLine.Checks_TransactionDetailId);

            dataRow["Accounts_Id"] = ListOfLongToString(logLine.Accounts_Id);
            dataRow["Accounts_TransactionDetailId"] = ListOfLongToString(logLine.Accounts_TransactionDetailId);

            dataRow["Review_Id"] = ListOfLongToString(logLine.Review_Id);
            dataRow["ReviewRequest_Id"] = logLine.ReviewRequest_Id;
            dataRow["Review_TransactionItemId"] = ListOfLongToString(logLine.Review_TransactionItemId);
            dataRow["Review_ItemReviewId"] = ListOfLongToString(logLine.Review_ItemReviewId);

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddMoniPlus2sExtensionEvent Exception : " + e.Message);
         }
      }
*/

   }
}
