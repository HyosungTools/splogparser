using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCRSamples
{
    public class tcr_samples_general
    {
      public const string TCR_INSTALL =
@"==========================================================================================================
 - Machine Number      = TCR1
 - StandardTCRCode Version = 03.01.00.00
 - Application Version = 03.01.00.00
 - SDK Version         = 03.00.02.16
 - Model               = MS500
 - IP Address          = 127.0.0.1
 - Port Number         = 9998
 - Time Zone           = Central Standard Time
 - Nextware Version    = 030-23
 - EMV Kernel 5 Version  = 5.0.3.4 69632 bytes MWIOCX = 2, 1, 0, 1 
 -         TCR Version = SP[            ], EP[          ]
 -         SIU Version = SP[  V 04.02.46], EP[V 07.30.05]
 -         BRM Version = SP[  V 04.30.63], EP[V 01.03.11]
 - BRM BC MAIN Version = SP[            ], EP[V 01.01.65]
 -  BRM BC DB1 Version = SP[            ], EP[   V010014]
 -  BRM BC DB2 Version = SP[            ], EP[   T010014]
 -     BRM RBU Version = SP[            ], EP[V 01.03.11]
 -  BRM BCFPGA Version = SP[            ], EP[  00570303]
 - BRM SHUTTER Version = SP[            ], EP[          ]
==========================================================================================================
Installed Programs:
 - BRM30_BC_V010153, Installed: 03/15/2024, Version: 01.01.53
 - BRM30_DB_V010014, Installed: 03/15/2024, Version: 01.00.14
 - BRM30_DB57_CAD_V010001, Installed: 02/12/2021, Version: 01.00.01
 - BRM30_EP_V010243, Installed: 03/15/2024, Version: 01.02.43
 - BRM30_FPGA_V00570303, Installed: 03/15/2024, Version: 00.57.03.03
 - BRM30_SP_V043063_R1, Installed: 03/15/2024, Version: 04.30.63
 - BRM30_SP_V043063_US_REG_R1, Installed: 03/15/2024, Version: 04.30.63
 - BRM36_EP_V010103, Installed: 03/15/2024, Version: 01.01.03
 - CLS_COIN_DriverPackage_V010001, Installed: 03/15/2024, Version: 01.00.01
 - CLS_COIN_SP_V042030_US_R1, Installed: 03/15/2024, Version: 04.20.30
 - CLS_COIN_SP_V042030_US_REG_R1, Installed: 03/15/2024, Version: 04.20.30
 - Common_SP_V043121_CHANNEL_R1, Installed: 02/12/2021, Version: 04.31.21
 - CONTF_SP_V043007_R1, Installed: 02/12/2021, Version: 04.30.07
 - Intel(R) Chipset Device Software, Installed: , Version: 10.1.1.38
 - Intel(R) Processor Graphics, Installed: , Version: 10.18.10.5069
 - Intel(R) Sideband Fabric Device Driver, Installed: , Version: 604.10125.2655.573
 - Microsoft Visual C++ 2010  x86 Redistributable - 10.0.30319, Installed: 02/12/2021, Version: 10.0.30319
 - Microsoft Visual C++ 2013 Redistributable (x86) - 12.0.30501, Installed: , Version: 12.0.30501.0
 - Microsoft Visual C++ 2013 x86 Additional Runtime - 12.0.21005, Installed: 02/12/2021, Version: 12.0.21005
 - Microsoft Visual C++ 2013 x86 Minimum Runtime - 12.0.21005, Installed: 02/12/2021, Version: 12.0.21005
 - MP2s Compuflex TCR, Installed: 03/15/2024, Version: 01.01.00.00
 - MP2s Standard TCR, Installed: 02/12/2021, Version: 03.01.00.00
 - Nextware_V033023_S1, Installed: 02/12/2021, Version: 03.30.23
 - PNCG0_SP_V040246_Channel_US_REG_R1, Installed: 02/12/2021, Version: 04.02.46
 - PNCG0_SP_V040246_R1, Installed: 02/12/2021, Version: 04.02.46
 - PulseIR, Installed: 06/15/2018, Version: 1.2.05
 - Realtek Ethernet Controller All-In-One Windows Driver, Installed: 02/12/2021, Version: 10.3.723.2015
 - Realtek High Definition Audio Driver, Installed: 02/12/2021, Version: 6.0.1.7624
 - tpdrv, Installed: , Version: 
==========================================================================================================
Installed Packages:
 - , Installed: 2024-03-15 16:44:39, Status: Unknown
 - Compuflex-TCR-01.01.00.00, Installed: 2024-03-15 16:42:10, Status: Good
 - TCR-PlatformUpdate-01.00.03, Installed: 2024-03-15 16:41:50, Status: Good
 - Basic Media-V02.00.17.00, Installed: Fri 02/12/2021
==========================================================================================================
";

      public const string TCR_HOST2ATM =
@"[2024-05-01 16:11:32-293][0][][Log                 ][RecvProcAsync       ][RECV]HOST2ATM: 076202405011611333850000000002NH01TCR1Admin2    0603A300000000000000           
";
      public const string TCR_ATM2HOST =
@"[2024-05-01 16:11:32-447][0][][Log                 ][Send                ][SEND]ATM2HOST: 093202405011611324390000000002NH01TCR1Admin2    0603A300000000000000           False|True|051301
";
      public const string TCR_ON_UPDATE_SCREENDATA =
@"[2024-05-01 16:11:47-371][3][][TCRLocalScreenWindow][OnUpdateScreenData  ][NORMAL]Display has been updated for screen [901-Idle-Horizontal-MS500].
";
      public const string TCR_CHANGING_MODE =
@"[2024-05-01 16:13:50-613][3][][ATMTaskManager      ][ATMTaskSchedulerProc][NORMAL]---------------Trying to change mode to OutOfService-------------------
";
      public const string TCR_CHANGEMODE_FAILED =
@"[2024-05-01 16:12:13-756][3][][ATMTaskManager      ][ATMTaskSchedulerProc][NORMAL]---------------ERROR: SwitchATMMode() NextMode[MoniPlus2.FW.Mode.TCRInServiceMode].Ready Fails.
";
      public const string TCR_CURRENTMODE =
@"[2024-05-01 16:11:53-895][3][][ModeFramework       ][OnModeChangeEvent   ][NORMAL]MoniPlus2.FW.Mode.ModeFramework.m_CurrentModeName value has been set to [OffLine].
";

      public const string TCR_NEXTSTATE =
@"[2024-05-13 09:10:30-899][3][][LocalFlowEngine     ][CreateNextState     ][NORMAL]State created: Deposit-ConfirmResult (TCRCommonFlowPoint)
";


      public const string TCR_DEP_TELLERID =
@"[2024-05-15 12:22:20-785][3][][Deposit             ][TransactionEnd      ][NORMAL]TellerID=Admin2    
";
      public const string TCR_DEP_RESULT =
@"[2024-05-15 12:22:20-799][3][][Deposit             ][TransactionEnd      ][NORMAL]Result=SUCCESS
";
      public const string TCR_DEP_ERRORCODE =
@"[2024-05-15 12:22:20-806][3][][Deposit             ][TransactionEnd      ][NORMAL]ErrorCode=
";
      public const string TCR_DEP_CASHDEPOSITED =
@"[2024-05-15 12:22:20-820][3][][Deposit             ][TransactionEnd      ][NORMAL]CashDeposited=[USD,(1,2)][USD,(2,3)][USD,(5,4)][USD,(10,5)][USD,(20,6)][USD,(50,7)][USD,(100,8)][UNKNOWN,(0,1)]
";
      public const string TCR_DEP_AMOUNT =
@"[2024-05-15 12:22:20-835][3][][Deposit             ][TransactionEnd      ][NORMAL]Amount=[USD,16000]
";
      public const string TCR_DEP_BALANCE =
@"[2024-05-15 12:22:20-842][3][][Deposit             ][TransactionEnd      ][NORMAL]Balance=[USD,64506]
";


      public const string TCR_WD_TELLERID =
@"[2024-05-15 08:08:58-986][3][][Dispense            ][TransactionEnd      ][NORMAL]TellerID=Admin2    
";
      public const string TCR_WD_RESULT =
@"[2024-05-15 08:08:58-999][3][][Dispense            ][TransactionEnd      ][NORMAL]Result=SUCCESS
";
      public const string TCR_WD_ERRORCODE =
@"[2024-05-15 08:08:59-006][3][][Dispense            ][TransactionEnd      ][NORMAL]ErrorCode=
";
      public const string TCR_WD_CASHDISPENSED =
@"[2024-05-15 08:08:59-013][3][][Dispense            ][TransactionEnd      ][NORMAL]CashDispensed=[USD,(100,1)][USD,(5,10)][USD,(10,7)]
";
      public const string TCR_WD_AMOUNT =
@"[2024-05-15 08:08:59-083][3][][Dispense            ][TransactionEnd      ][NORMAL]Amount=[USD,220]
";
      public const string TCR_WD_BALANCE =
@"[2024-05-15 08:08:59-095][3][][Dispense            ][TransactionEnd      ][NORMAL]Balance=[USD,59015]
";


      public const string TCR_HOST_CMD =
@"[2024-05-29 07:46:53-615][3][][TCRInServiceMode    ][ProcessHostReceivedData][NORMAL]ProcessHostReceivedData: 040801, GetPhysicalUnitInformation
";
      public const string TCR_HOST_CMD_RESPONSE =
@"[2024-05-29 07:14:31-836][3][][ModeHelperFunction  ][ProcessSyncCommand  ][NORMAL]Send the result of the InquiryCommand: [0603A3], [False|False|051301]
";
   }
}
