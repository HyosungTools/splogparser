namespace Samples
{
   public static class samples_cashdepot_loglines
   {
      public const string SAMPLE_CASHDEPOT_MULTILINE_1 =
 @"  INFO [2023-07-24 22:07:41-392] [OutOfServiceMode.IgnoreOperatorSwitchPosition] Returning a value of [True].
  WARN [2023-07-24 22:07:41-392] [OutOfServiceMode.IsSupervisorTimeOut] Returning a value of [False].
 ERROR [2023-07-24 22:07:41-392] [OutOfServiceMode.IgnoreCabinetDoorStatus] NHApplication.Mode.SupportSupervisorWithCabinetDoor value is [False].
  INFO [2023-07-24 22:07:41-392] [OutOfServiceMode.IgnoreCabinetDoorStatus] Returning a value of [True].
  INFO [2023-07-24 22:07:41-392] [ATMTaskManager.ATMTaskSchedulerProc] ---------------Previous Mode is set as OffLine
  INFO [2023-07-24 22:07:41-392] [ATMTaskManager.ATMTaskSchedulerProc] CurrentMode: MoniPlus2.FW.Mode.OffLineMode, Activated: True
  INFO [2023-07-24 22:07:41-392] [OffLineMode.Stop] Stop mode OffLine
  INFO [2023-07-24 22:07:41-392] [OffLineMode.UpdateRMSMonitorLEDs] UpdateRMSMonitorLEDs is called. pLEDStatus: OFF
  INFO [2023-07-24 22:07:41-392] [OffLineMode.UpdateRMSMonitorLEDs] Current Mode: OffLine
  INFO [2023-07-24 22:07:41-392] [ATMTaskManager.ATMTaskSchedulerProc] ---------------Enter New Mode-------------------
  INFO [2023-07-24 22:07:41-392] [ATMTaskManager.ATMTaskSchedulerProc] OffLine Mode ----> OutOfService Mode
  INFO [2023-07-24 22:07:41-392] [ATMTaskManager.ATMTaskSchedulerProc] ------------------------------------------------
  INFO [2023-07-24 22:07:41-424] [Auxiliaries.OnRMSMonitorChanged] m_NxAux.OnRMSMonitorChanged event received
  INFO [2023-07-24 22:07:41-424] [Auxiliaries.RaiseDeviceUnSolEvent] FireDeviceUnsolEvent(RemoteStatusMonitorChanged, AUX_RSM_CHANGED)
  INFO [2023-07-24 22:07:41-506] [OutOfServiceMode.RaiseModeChangedEvent] RaiseModeChangedEvent
  INFO [2023-07-24 22:07:41-506] [ModeFramework.OnModeChangeEvent] MoniPlus2.FW.Mode.ModeFramework.m_CurrentModeName value has been set to [OutOfService].
  INFO [2023-07-24 22:07:41-506] [LocalScreenWindowEx.StopUserInputTimer] StopUserInputTimer, stopped application pin timer with 3600000
  INFO [2023-07-24 22:07:41-602] [Pinpad.OnReadDataCancelled] m_NxPin.OnReadDataCancelled event received
  INFO [2023-07-24 22:07:41-618] [SoftwareManagement.ModeFramework_ModeChangedEvent] Call ReleaseLock since nextmode is not supervisor
  INFO [2023-07-24 22:07:41-618] [NHManagement.StartService] MoniManager command release ModeChangedWaitHandle
  INFO [2023-07-24 22:07:41-618] [ATMStatusMonitor.GetDeviceStatusFullCode] Parameter pShortName:IDC
  INFO [2023-07-24 22:07:41-618] [ATMStatusMonitor.GetDeviceStatusFullCode] Atom Skimmer statusCode :00
  INFO [2023-07-24 22:07:41-618] [ATMStatusMonitor.GetDeviceStatusFullCode] Parameter pShortName:SPR
  INFO [2023-07-24 22:07:41-618] [ATMStatusMonitor.GetDeviceStatusFullCode] Parameter pShortName:SNS
  INFO [2023-07-24 22:07:41-618] [ATMStatusMonitor.GetDeviceStatusFullCode] Parameter pShortName:PIN";

      public const string SAMPLE_CASHDEPOT_INSTALLLINES_1 =
@"==========================================================================================================
 - Machine Number           = RC002379
 - Application Version      = 02.01.15.00
 - SDK Version              = 03.00.02.15
 - GSDK Version             = 01.02.00.00
 - Custom Version           = 01.00.05.00
 - Custom Project Name      = Cash Depot
 - Model                    = CAJERA
 - IP Address               = cdssl.cashdepotplus.com
 - Port Number              = 8460
 - Time Zone                = Central Standard Time
 - Nextware Version         = 030-39
 - EMV Kernel 7 Version     = 7.1.2.6 191760 bytes MWIOCX = 2, 1, 0, 1 
 -         PIN Version = SP[  V 04.22.13], EP[V 15.00.07]
 -         CDM Version = SP[            ], EP[          ]
 -         SIU Version = SP[  V 04.21.13], EP[V 09.00.17]
 -         IDC Version = SP[  V 04.30.39], EP[  5293-01I]
 -         SPR Version = SP[  V 04.30.87], EP[V 34.00.02]
==========================================================================================================
Installed Programs:
 - A2iA CheckReader V9.0, Installed: 01/23/2023, Version: 9.0
 - BRM_MERGED_SP_Pack_V010003_V042402_R1, Installed: 03/02/2023, Version: 04.24.02
 - BRM_MERGED_SP_V042402_US_REG_R1, Installed: 03/02/2023, Version: 04.24.02
 - BRM150_EP_V010009, Installed: 03/02/2023, Version: 01.00.09
 - BRM1xx_BC_V030045, Installed: 03/07/2023, Version: 03.00.45
 - BRM1xx_DB_V010019, Installed: 03/07/2023, Version: 01.00.19
 - Intel(R) Chipset Device Software, Installed: , Version: 10.1.18243.8188
 - Intel(R) Processor Graphics, Installed: , Version: 26.20.100.7986
 - Intel(R) Trusted Connect Service Client x86, Installed: 07/27/2022, Version: 1.60.155.0
 - Intel(R) Trusted Connect Services Client, Installed: , Version: 1.60.155.0
 - Microsoft VC++ redistributables repacked., Installed: 07/27/2022, Version: 12.0.0.0
 - Microsoft Visual C++ 2013 Redistributable (x86) - 12.0.30501, Installed: , Version: 12.0.30501.0
 - Microsoft Visual C++ 2013 x86 Additional Runtime - 12.0.21005, Installed: 07/26/2022, Version: 12.0.21005
 - Microsoft Visual C++ 2013 x86 Minimum Runtime - 12.0.21005, Installed: 07/26/2022, Version: 12.0.21005
 - Microsoft Visual C++ 2017 Redistributable (x86) - 14.16.27027, Installed: , Version: 14.16.27027.1
 - Microsoft Visual C++ 2017 X86 Additional Runtime - 14.16.27024, Installed: 12/15/2022, Version: 14.16.27024
 - Microsoft Visual C++ 2017 X86 Minimum Runtime - 14.16.27024, Installed: 12/15/2022, Version: 14.16.27024
 - MP2s CashDepot NBS, Installed: 03/08/2023, Version: 1.0.0.0
 - MP2s HIP NBS, Installed: 12/15/2022, Version: 02.00.04.00
 - MP2s Standard NBS, Installed: 03/02/2023, Version: 02.01.15.00
 - Nextware_V033039, Installed: 03/07/2023, Version: 03.30.39
 - Realtek High Definition Audio Driver, Installed: 07/27/2022, Version: 6.0.8838.1
 - VDM_Core_V010455_S1, Installed: 03/07/2023, Version: 01.04.55
 - VDM_Data_V010455_S1, Installed: 03/07/2023, Version: 01.04.55
==========================================================================================================
Installed Packages:
==========================================================================================================

  INFO [2023-07-24 00:19:49-550] [Archiving.CreateArchive] CreateArchive(): About to run, runNow=False
  INFO [2023-07-24 00:19:49-550] [Management.CopyLogFile] Parameter pStartDateTime: 7/23/2023 12:19:49 AM
  INFO [2023-07-24 00:19:49-550] [Management.CopyLogFile] Parameter pEndDateTime  : 7/23/2023 12:19:49 AM
  INFO [2023-07-24 00:19:49-551] [Management.CopyLogFile] Console Application Arguments = -oASEVRDT -s20230723 -e20230723 -pC:\nhtemp\
  INFO [2023-07-24 00:19:50-416] [Management.CopyLogFile] Console Application finished Result = 0
  INFO [2023-07-24 00:19:50-417] [Management.CopyMultipleApplicationFiles] Application Config files Copy to USB or CD";

      public const string SAMPLE_CASHDEPOT_MULTILINE_2 =
         @"  INFO [2023-07-24 20:44:41-347] [TransactionRequestCondition.SetExtendTransactionFlag] Parameter pFields:[0]=003,Parameter pFields:[1]=000,Parameter pFields:[2]=000,Parameter pFields:[3]=000,Parameter pFields:[4]=000,Parameter pFields:[5]=000,Parameter pFields:[6]=001,Parameter pFields:[7]=052,
  INFO [2023-07-24 20:44:41-347] [MessageFramework.MakeTransactionRequestMessage] Parameter pCondition:Track1:False
Track2:True
Track3:False
PinBuffer:True
OPCode:True
Amount:True
BufferB:True
BufferC:True
Optional Data Field U:False
Optional Data Field V:False
Optional Data Field Z:False
SendCashAcceptor:False
OptionalSendCountsOfGoodAndSuspect:False
OptionalSendCountsOfSuspect:False
OptionalSendCountsOfCounterfeit:False
SendChequeProcessor:False
SendBunchChequeDeposit:False

  INFO [2023-07-24 20:44:41-347] [MessageFramework.MakeMessage] Parameter pCmd:TRANSACT_INTERACTIVE_RESP
  INFO [2023-07-24 20:44:41-347] [MessageFramework.MakeMessage] Parameter pExt:NONE
  INFO [2023-07-24 20:44:41-347] [MessageFramework.NeedMacData] Parameter pCmd:TRANSACT_INTERACTIVE_RESP
  INFO [2023-07-24 20:44:41-348] [BuilderNDCMessage.MakeMessage] Parameter pDeviceStatus:Fitness:[]MCode:[]TCode:[]StatusDDC:[]";
   }
}
