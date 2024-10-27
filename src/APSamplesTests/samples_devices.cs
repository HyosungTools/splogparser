using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSamples
{
   public class samples_devices
   {
      /* CASH DISPENSER */


      public const string CashDisp_Open =
@"[2023-08-15 08:21:19-249][3][][CashDispenser       ][Open                ][NORMAL]CDM NumberOfPhysicalUnits is 6
";

      /* STATUS */

      /* CDM */

      public const string APLOG_CDM_ONLINE =
@"[2023-10-31 20:48:45-499][3][][CashDispenser       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVONLINE)
";
      public const string APLOG_CDM_OFFLINE =
@"[2023-11-01 08:22:59-387][3][][CashDispenser       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVOFFLINE)
";
      public const string APLOG_CDM_ONHWERROR =
@"[2023-10-31 20:48:13-399][3][][CashDispenser       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVHWERROR)
";
      public const string APLOG_CDM_DEVERROR =
@"[2023-06-07 11:39:02-264][3][][CashDispenser       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_ERROR)
";
      public const string APLOG_CDM_ONOK =
@"[2023-11-01 08:10:45-642][3][][CashDispenser       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_OK)
";
      /* CIM */

      public const string APLOG_CIM_ONLINE =
@"[2023-10-31 20:48:45-499][3][][CashAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVONLINE)
";
      public const string APLOG_CIM_OFFLINE =
@"[2023-11-01 08:22:59-387][3][][CashAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVOFFLINE)
";
      public const string APLOG_CIM_ONHWERROR =
@"[2023-10-31 20:48:13-399][3][][CashAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVHWERROR)";

      public const string APLOG_CIM_DEVERROR =
@"[2023-06-07 11:39:02-264][3][][CashAcceptor       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_ERROR)
";
      public const string APLOG_CIM_ONOK =
@"[2023-11-01 08:10:45-642][3][][CashAcceptor       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_OK)
";

      /* IPM */

      public const string APLOG_IPM_ONLINE =
@"[2023-10-31 20:48:45-499][3][][BundleCheckAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVONLINE)
";
      public const string APLOG_IPM_OFFLINE =
@"[2023-11-01 08:22:59-387][3][][BundleCheckAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVOFFLINE)
";
      public const string APLOG_IPM_ONHWERROR =
@"[2023-10-31 20:48:13-399][3][][BundleCheckAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVHWERROR)
";
      public const string APLOG_IPM_DEVERROR =
@"[2023-06-07 11:39:02-264][3][][BundleCheckAcceptor       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_ERROR)
";
      public const string APLOG_IPM_ONOK =
@"[2023-11-01 08:10:45-642][3][][BundleCheckAcceptor       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_OK)
";

      /* MMA */

      public const string APLOG_MMA_ONLINE =
@"[2023-10-31 20:48:45-499][3][][MixedMediaAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVONLINE)
";
      public const string APLOG_MMA_OFFLINE =
@"[2023-11-01 08:22:59-387][3][][MixedMediaAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVOFFLINE)
";
      public const string APLOG_MMA_ONHWERROR =
@"[2023-10-31 20:48:13-399][3][][MixedMediaAcceptor       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVHWERROR)
";
      public const string APLOG_MMA_DEVERROR =
@"[2023-06-07 11:39:02-264][3][][MixedMediaAcceptor       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_ERROR)
";
      public const string APLOG_MMA_ONOK =
@"[2023-11-01 08:10:45-642][3][][MixedMediaAcceptor       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_OK)
";

      /* Receipt Printer */

      public const string APLOG_RCT_ONLINE =
@"[2023-10-31 20:48:45-499][3][][ReceiptPrinter       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVONLINE)
";
      public const string APLOG_RCT_OFFLINE =
@"[2023-11-01 08:22:59-387][3][][ReceiptPrinter       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVOFFLINE)
";
      public const string APLOG_RCT_ONHWERROR =
@"[2023-10-31 20:48:13-399][3][][ReceiptPrinter       ][OnDeviceStatusChanged][NORMAL]m_NxCashDispenser.OnDeviceStatusChanged event received(DEVHWERROR)
";
      public const string APLOG_RCT_DEVERROR =
@"[2023-06-07 11:39:02-264][3][][ReceiptPrinter       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_ERROR)
";
      public const string APLOG_RCT_ONOK =
@"[2023-11-01 08:10:45-642][3][][ReceiptPrinter       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_OK)
";

      /* position status */

      public const string CashDispenser_NotInPosition =
@"[2023-06-07 11:39:01-910][3][][CashDispenser       ][OnPositionStatusChanged][NORMAL]m_NxCashDispenser.OnPositionnStatusChanged event received,NOTINPOSITION
";
      public const string CashDispenser_InPosition =
@"[2023-06-07 11:39:02-381][3][][CashDispenser       ][OnPositionStatusChanged][NORMAL]m_NxCashDispenser.OnPositionnStatusChanged event received,INPOSITION
";


      /* dispense */

      public const string CashDispenser_OnNoDispense =
@"[2023-11-01 08:09:37-043][3][][CashDispenser       ][OnDispenserStatusChanged][NORMAL]m_NxCashDispenser.OnDispenserStatusChanged event received,NODISPENSE
";
      public const string CashDispenser_OnDispenserOK =
@"[2023-11-01 08:34:40-912][3][][CashDispenser       ][OnDispenserStatusChanged][NORMAL]m_NxCashDispenser.OnDispenserStatusChanged event received,OK
";
      public const string CashDispenser_DeGrade =
@"[2023-06-07 11:39:15-516][3][][CashDispenser       ][OnDispenserStatusChanged][NORMAL]m_NxCashDispenser.OnDispenserStatusChanged event received,DEGRADED
";



      /* status - shutter, position, stacker, transport */

      public const string CashDispenser_OnShutterOpen =
@"[2023-10-31 20:57:36-498][3][][CashDispenser       ][OnShutterStatusChanged][NORMAL]m_NxCashDispenser.OnShutterStatusChanged event received,OPEN
";
      public const string CashDispenser_OnShutterClosed =
@"[2023-10-31 20:59:26-416][3][][CashDispenser       ][OnShutterStatusChanged][NORMAL]m_NxCashDispenser.OnShutterStatusChanged event received,CLOSED
";
      public const string CashDispenser_OnStackerNotEmpty =
@"[2023-10-31 20:59:19-122][3][][CashDispenser       ][OnStackerStatusChanged][NORMAL]m_NxCashDispenser.OnStackerStatusChanged event received,NOTEMPTY
";
      public const string CashDispenser_OnStackerEmpty =
@"[2023-10-31 20:59:23-289][3][][CashDispenser       ][OnStackerStatusChanged][NORMAL]m_NxCashDispenser.OnStackerStatusChanged event received,EMPTY
";
      public const string CashDispenser_OnTransportNotEmpty =
@"[2023-10-31 20:48:13-618][3][][CashDispenser       ][OnTransportStatusChanged][NORMAL]m_NxCashDispenser.OnTransportStatusChanged event received,NOTEMPTY
";
      public const string CashDispenser_OnTransportEmpty =
@"[2023-10-31 20:48:45-613][3][][CashDispenser       ][OnTransportStatusChanged][NORMAL]m_NxCashDispenser.OnTransportStatusChanged event received,EMPTY
";
      public const string CashDispenser_OnPositionNotEmpty =
@"[2023-10-31 07:54:49-085][3][][CashDispenser       ][OnPositionStatusChanged][NORMAL]m_NxCashDispenser.OnPositionnStatusChanged event received,NOTEMPTY
";
      public const string CashDispenser_OnPositionEmpty =
@"[2023-10-31 05:54:06-326][3][][CashDispenser       ][OnPositionStatusChanged][NORMAL]m_NxCashDispenser.OnPositionnStatusChanged event received,EMPTY
";
      public const string CashDispenser_OnCashUnitChanged =
@"[2023-10-31 20:48:46-523][3][][CashDispenser       ][OnCashUnitChanged   ][NORMAL]m_NxCashDispenser.OnCashUnitChanged event received,1
";


      /* SUMMARY */

      /* summary - set up */

      public const string CashDispenser_SetupCSTList =
@"[2023-10-31 20:57:21-590][3][][CashDispenser       ][SetupCSTListInHostTypeInfo][NORMAL]1:1 matching A Add CassetteIdxList 2
";
      public const string CashDispenser_SetupNoteType =
@"[2023-10-31 20:57:21-594][3][][CashDispenser       ][SetupNoteTypeInfo   ][NORMAL]Set NoteTypeA Currency:[USD] Value:[1] SPLCUIndex:[2] SPPCUIndex:[-1]
";


      /* DISPENSE */

      public const string CashDispenser_OnDenominateComplete =
@"[2023-10-31 20:57:17-067][3][][CashDispenser       ][OnDenominateComplete][NORMAL]m_NxCashDispenser.OnDenominateComplete event received
";

      public const string CashDispenser_OnDeviceOK =
@"[2023-10-31 20:48:45-606][3][][CashDispenser       ][RaiseDeviceUnSolEvent][NORMAL]FireDeviceUnsolEvent(DeviceStatusChanged, DEVICE_OK)
";
      public const string CashDispenser_ExecDispense =
@"[2023-10-31 20:57:21-724][3][][CashDispenser       ][ExecDispense_NDCDDC_LCU][NORMAL]Host amount is 20
";

      public const string CashDispenser_DispenseSyncAsync =
@"[2023-10-31 20:57:21-739][3][][CashDispenser       ][DispenseSyncAsync   ][NORMAL]MixAlgo=0, Currency=USD, Amount=0, Disp=0 0 0 0 1 0 
";
      public const string CashDispenser_OnDispenseComplete =
@"[2023-10-31 20:57:33-721][3][][CashDispenser       ][OnDispenseComplete  ][NORMAL]m_NxCashDispenser.OnDispenseComplete event received
";
      public const string CashDispenser_OnPresentComplete =
@"[2023-10-31 20:57:36-543][3][][CashDispenser       ][OnPresentComplete   ][NORMAL]m_NxCashDispenser.OnPresentComplete event received
";
      public const string CashDispenser_OnRetractComplete =
@"[2023-06-07 13:05:17-304][3][][CashDispenser       ][OnRetractComplete   ][NORMAL]m_NxCashDispenser.OnRetractComplete event received'
";
      public const string CashDispenser_OnItemsTaken =
@"[2023-10-31 20:59:26-419][3][][CashDispenser       ][OnItemsTaken        ][NORMAL]m_NxCashDispenser.OnItemsTaken event received
";
      public const string CashDispenser_GetLCULastDispensedCount =
@"[2023-10-31 20:57:36-555][3][][CashDispenser       ][GetLCULastDispensedCount][NORMAL]Last Dispensed Count A = 0
";

      /*
      [2023-08-15 08:20:33-940][3][][CashDispenser       ][UpdateRegistryValueForBRM][NORMAL]BRMConvPCU2LCU Before : 15
      [2023-08-15 08:20:34-518][3][][CashDispenser       ][UpdateRegistryValueForBRM][NORMAL]BRMConvPCU2LCU After : 15
      [2023-08-15 08:20:34-518][3][][CashDispenser       ][UpdateRegistryValueForBRM][NORMAL]SupportNoteCounting : True




      */

   }
}
