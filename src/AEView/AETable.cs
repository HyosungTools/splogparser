using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace AEView
{
   class AETable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public AETable(IContext ctx, string viewName) : base(ctx, viewName)
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
         if (logLine is LogLineHandler.ExtensionStarted aeLogLine)
         {
            try
            {
               switch (aeLogLine.aeType)
               {
                  case AELogType.ExtensionStarted:
                     {
                        base.ProcessRow(aeLogLine);
                        AddExtensionStartedEvent(aeLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AETable.ProcessRow ExtensionStarted EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.NetOpExtension noLogLine)
         {
            try
            {
               switch (noLogLine.aeType)
               {
                  case AELogType.NetOpExtension:
                     {
                        base.ProcessRow(noLogLine);
                        AddNetOpExtensionEvent(noLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AETable.ProcessRow NetOpExtension EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.NextwareExtension neLogLine)
         {
            try
            {
               switch (neLogLine.aeType)
               {
                  case AELogType.NextwareExtension:
                     {
                        base.ProcessRow(neLogLine);
                        AddNetwareExtensionEvent(neLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AETable.ProcessRow NextwareExtension EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.MoniPlus2sExtension mpLogLine)
         {
            try
            {
               switch (mpLogLine.aeType)
               {
                  case AELogType.MoniPlus2sExtension:
                     {
                        base.ProcessRow(mpLogLine);
                        AddMoniPlus2sExtensionEvent(mpLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AETable.ProcessRow MoniPlus2sExtension EXCEPTION: {e}");
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

      protected void AddNetwareExtensionEvent(LogLineHandler.NextwareExtension logLine)
      {
         try
         {
            string tableName = "NetwareEvents";

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
            dataRow["EncStatus"] = logLine.EncStatus;
            dataRow["DevicePositionStatus"] = logLine.DevicePositionStatus;
            dataRow["PowerSaveRecoveryTime"] = logLine.PowerSaveRecoveryTime;
            dataRow["LogicalServiceName"] = logLine.LogicalServiceName;
            dataRow["ExtraInformation"] = logLine.ExtraInformation;
            dataRow["SafeDoorStatus"] = logLine.SafeDoorStatus;
            dataRow["DispenserStatus"] = logLine.DispenserStatus;
            dataRow["IntermediateStackerStatus"] = logLine.IntermediateStackerStatus;
            dataRow["ShutterStatus"] = logLine.ShutterStatus;
            dataRow["PositionStatus"] = logLine.PositionStatus;
            dataRow["TransportStatus"] = logLine.TransportStatus;
            dataRow["TransportStatusStatus"] = logLine.TransportStatusStatus;
            dataRow["UnitCurrencyID"] = Impl.LogLine.ListToString(',', logLine.UnitCurrencyID);
            dataRow["UnitValue"] = Impl.LogLine.ListToString(',', logLine.UnitValue);
            dataRow["UnitStatus"] = Impl.LogLine.ListToString(',', logLine.UnitStatus);
            dataRow["UnitType"] = Impl.LogLine.ListToString(',', logLine.UnitType);
            dataRow["CabinetStatus"] = logLine.CabinetStatus;
            dataRow["SafeStatus"] = logLine.SafeStatus;
            dataRow["VandalShieldStatus"] = logLine.VandalShieldStatus;
            dataRow["MediaStatus"] = logLine.MediaStatus;
            dataRow["RetainBinStatus"] = logLine.RetainBinStatus;
            dataRow["SecurityStatus"] = logLine.SecurityStatus;
            dataRow["NumberOfCardsRetained"] = logLine.NumberOfCardsRetained;
            dataRow["ChipPowerStatus"] = logLine.ChipPowerStatus;
            dataRow["PaperStatus"] = Impl.LogLine.ListToString(',', logLine.PaperStatus);
            dataRow["Media_TonerStatus"] = logLine.Media_TonerStatus;
            dataRow["Media_InkStatus"] = logLine.Media_InkStatus;
            dataRow["LampStatus"] = logLine.LampStatus;
            dataRow["RetractBinStatus"] = logLine.RetainBinStatus;
            dataRow["MediaOnStacker"] = logLine.MediaOnStacker;
            dataRow["Media_DevicePositionStatus"] = logLine.Media_DevicePositionStatus;
            dataRow["CardUnitStatus"] = logLine.CardUnitStatus;
            dataRow["PinpadStatus"] = logLine.PinpadStatus;
            dataRow["NotesDispenserStatus"] = logLine.NotesDispenserStatus;
            dataRow["CoinDispenserStatus"] = logLine.CoinDispenserStatus;
            dataRow["ReceiptPrinterStatus"] = logLine.ReceiptPrinterStatus;
            dataRow["PassbookPrinterStatus"] = logLine.PassbookPrinterStatus;
            dataRow["EnvelopeDepositoryStatus"] = logLine.EnvelopeDepositoryStatus;
            dataRow["ChequeUnitStatus"] = logLine.ChequeUnitStatus;
            dataRow["BillAcceptorStatus"] = logLine.BillAcceptorStatus;
            dataRow["EnvelopeDispenserStatus"] = logLine.EnvelopeDispenserStatus;
            dataRow["DocumentPrinterStatus"] = logLine.DocumentPrinterStatus;
            dataRow["CoinAcceptorStatus"] = logLine.CoinAcceptorStatus;
            dataRow["ScannerStatus"] = logLine.ScannerStatus;
            dataRow["OperatorSwitchStatus"] = logLine.OperatorSwitchStatus;
            dataRow["TamperStatus"] = logLine.TamperStatus;
            dataRow["IntTamperStatus"] = logLine.IntTamperStatus;
            dataRow["SeismicStatus"] = logLine.SeismicStatus;
            dataRow["HeatStatus"] = logLine.HeatStatus;
            dataRow["ProximityStatus"] = logLine.ProximityStatus;
            dataRow["AmblightStatus"] = logLine.AmblightStatus;
            dataRow["EnhancedAudioStatus"] = logLine.EnhancedAudioStatus;
            dataRow["OpenCloseStatus"] = logLine.OpenCloseStatus;
            dataRow["FasciaLightStatus"] = logLine.FasciaLightStatus;
            dataRow["AudioStatus"] = logLine.AudioStatus;
            dataRow["HeatingStatus"] = logLine.HeatingStatus;
            dataRow["VolumeStatus"] = logLine.VolumeStatus;
            dataRow["UpsStatus"] = logLine.UpsStatus;
            dataRow["GreenLedStatus"] = logLine.GreenLedStatus;
            dataRow["AmberLedStatus"] = logLine.AmberLedStatus;
            dataRow["RedLedStatus"] = logLine.RedLedStatus;
            dataRow["AudibleAlarmStatus"] = logLine.AudibleAlarmStatus;
            dataRow["EnhancedAudioControlStatus"] = logLine.EnhancedAudioControlStatus;
            dataRow["CheckAcceptorStatus"] = logLine.CheckAcceptorStatus;
            dataRow["TonerStatus"] = logLine.TonerStatus;
            dataRow["InkStatus"] = logLine.InkStatus;
            dataRow["FrontImageScannerStatus"] = logLine.FrontImageScannerStatus;
            dataRow["BackImageScannerStatus"] = logLine.BackImageScannerStatus;
            dataRow["MICRReaderStatus"] = logLine.MICRReaderStatus;
            dataRow["StackerStatus"] = logLine.StackerStatus;
            dataRow["ReBuncherStatus"] = logLine.ReBuncherStatus;
            dataRow["MediaFeederStatus"] = logLine.MediaFeederStatus;
            dataRow["PositionStatus_Input"] = logLine.PositionStatus_Input;
            dataRow["PositionStatus_Output"] = logLine.PositionStatus_Output;
            dataRow["PositionStatus_Refused"] = logLine.PositionStatus_Refused;
            dataRow["ShutterStatus_Input"] = logLine.ShutterStatus_Input;
            dataRow["ShutterStatus_Output"] = logLine.ShutterStatus_Output;
            dataRow["ShutterStatus_Refused"] = logLine.ShutterStatus_Refused;
            dataRow["TransportStatus_Input"] = logLine.TransportStatus_Input;
            dataRow["TransportStatus_Output"] = logLine.TransportStatus_Output;
            dataRow["TransportStatus_Refused"] = logLine.TransportStatus_Refused;
            dataRow["TransportMediaStatus_Input"] = logLine.TransportMediaStatus_Input;
            dataRow["TransportMediaStatus_Output"] = logLine.TransportMediaStatus_Output;
            dataRow["TransportMediaStatus_Refused"] = logLine.TransportMediaStatus_Refused;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddNetwareExtensionEvent Exception : " + e.Message);
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
            ctx.ConsoleWriteLogLine("AddNetwareExtensionEvent Exception : " + e.Message);
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

   }
}
