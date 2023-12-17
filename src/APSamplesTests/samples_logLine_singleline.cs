namespace Samples
{
   public static class samples_logLine_singleline
   {
      public const string SAMPLE_SINGLELINE_1 =
         @"[2023-03-30 18:08:32-948][3][][CardReader          ][EMV_TerminalRiskMgmt][NORMAL][CardReader Log] Call EMV_TerminalRiskMgmt()
[2023-03-30 18:08:32-952][3][][EMVDataHelper       ][CheckApplicationVersionNumber][NORMAL]Get TLV to check version : 8000048000
[2023-03-30 18:08:32-954][3][][EMVDataHelper       ][CheckApplicationVersionNumber][NORMAL]bit = 0
[2023-03-30 18:08:32-955][3][][EMVDataHelper       ][CheckApplicationVersionNumber][NORMAL]bitExpress = 0000
[2023-03-30 18:08:32-955][3][][EMVDataHelper       ][CheckApplicationVersionNumber][NORMAL]Version is same
[2023-03-30 18:08:32-955][3][][CardReader          ][EMV_ActionAnalysis  ][NORMAL][CardReader Log] Call EMV_ActionAnalysis()
[2023-03-30 18:08:33-248][3][][CardReader          ][EMV_ActionAnalysis  ][NORMAL][CardReader Log] Return EMV_ActionAnalysis() : 1
[2023-03-30 18:08:33-249][3][][EMVProcessing       ][GenerateARQC        ][NORMAL]EMV_ActionAnalysis Result :   
[2023-03-30 18:08:33-249][3][][EMVProcessing       ][UpdateEMVTagList    ][NORMAL]Call 'Device.CardReader.GetRequestedDataObjects()'
[2023-03-30 18:08:33-250][3][][EMVProcessing       ][UpdateEMVTagList    ][NORMAL]EMVState.SelectedAID                      : A0000000041010
[2023-03-30 18:08:33-250][3][][EMVTagDefinition    ][GetRequestTagList   ][NORMAL]pAID           :A0000000041010
[2023-03-30 18:08:33-250][3][][EMVTagDefinition    ][GetRequestTagList   ][NORMAL]_AID_INFO_FILE :D:\EMVIni\EMVAID.ini
";

      public const string SAMPLE_SINGLELINE_2 =
         @"[2023-03-30 18:08:33-254][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :38 tagValue    : A0000000041010
[2023-03-30 18:08:33-261][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 57
[2023-03-30 18:08:33-263][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :91 tagValue    : 546316XXXXXXXXXXXXXXXXXXXXXX0636
[2023-03-30 18:08:33-263][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 5A
[2023-03-30 18:08:33-265][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :19 tagValue    : 546316XXXXXX1538
[2023-03-30 18:08:33-265][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 5F2A
[2023-03-30 18:08:33-267][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :94 tagValue    : 0840
[2023-03-30 18:08:33-270][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 5F34
[2023-03-30 18:08:33-272][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :20 tagValue    : **
[2023-03-30 18:08:33-272][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 82
[2023-03-30 18:08:33-273][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :16 tagValue    : 3900
[2023-03-30 18:08:33-274][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 8C
[2023-03-30 18:08:33-278][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :29 tagValue    : 9F02069F03069F1A0295055F2A029A039C019F37049F35019F45029F4C089F34039F21039F7C14
[2023-03-30 18:08:33-279][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 95
[2023-03-30 18:08:33-281][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :88 tagValue    : 8000048000
[2023-03-30 18:08:33-281][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 9A
[2023-03-30 18:08:33-282][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :96 tagValue    : 230330
[2023-03-30 18:08:33-283][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tag : 9C
[2023-03-30 18:08:33-285][3][][EMVTagDefinition    ][GetTLV              ][NORMAL][CardReader Log] tagIndex :103 tagValue    : 01
";

      public const string SAMPLE_SINGLELINE_3 =
         @"[2023-03-30 18:42:38-597][3][][GuideLights         ][OnGuideLightChanged ][NORMAL]GuideLightUnit[PINPAD]
[2023-03-30 18:42:38-597][3][][GuideLights         ][OnGuideLightChanged ][NORMAL]State[OFF]
[2023-03-30 18:42:40-620][3][][ScreenDecoratorLocal][OnButtonClick       ][NORMAL]WPFButtonClicked | Type [MoniPlus2.WPF.Controls.Control.Lextant.ImageButton]
[2023-03-30 18:42:40-622][3][][ScreenDecoratorLocal][OnFunctionKeySelected][NORMAL]Raising FunctionKeySelected event with values FunctionKey[Denominations], PinInputData[], ResultData[].
[2023-03-30 18:42:40-623][3][][StandardKeyEntryFlowPoint][Screen_FunctionKeySelected][NORMAL]Received IScreenFramework.FunctionKeySelected event with values of FunctionKey[Denominations], PinInputData[], ResultData[].
[2023-03-30 18:42:40-623][3][][LocalScreenWindowEx ][StopUserInputTimer  ][NORMAL]StopUserInputTimer, stopped application pin timer with 30000
[2023-03-30 18:42:40-717][3][][Pinpad              ][OnReadDataCancelled ][NORMAL]m_NxPin.OnReadDataCancelled event received
[2023-03-30 18:42:40-719][3][][LocalScreenWindowEx ][StopUserInputTimer  ][NORMAL]StopUserInputTimer, stopped application pin timer with 30000
[2023-03-30 18:42:40-721][3][][LocalXmlHelper      ][ProcessExecuteNode  ][NORMAL]ProcessExecuteNode(): About to execute: Class: HelperFunctions, Method: FormatCurrency
[2023-03-30 18:42:40-722][3][][Utility             ][MapLangStringToTwoLetterLangCode][NORMAL]langCode return : 
[2023-03-30 18:42:40-723][3][][AbstractFlowPoint   ][DetermineNextFlowPoint][NORMAL]Next flowpoint has been determined:Withdrawal-SelectBillMix
[2023-03-30 18:42:40-723][3][][LocalScreenWindowEx ][StopUserInputTimer  ][NORMAL]StopUserInputTimer, stopped application pin timer with 30000
[2023-03-30 18:42:40-724][3][][HybridFlowEngine    ][GetFlowPoint        ][NORMAL]FlowPoint (Withdrawal-SelectBillMix) was found in the Flow xml file.
[2023-03-30 18:42:40-724][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]****************************************************************************
[2023-03-30 18:42:40-724][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: Withdrawal-SelectBillMix (StandardKeyEntryFlowPoint)
[2023-03-30 18:42:40-724][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]****************************************************************************
[2023-03-30 18:42:40-727][3][][ActiveTeller        ][SendToAgent         ][NORMAL]Sending message to Agent: ApplicationState
[2023-03-30 18:42:40-732][3][][SockAdapter         ][Send                ][NORMAL]Socket Send SocketCode=18187816 Socket Handle : 7116
";

      public const string SAMPLE_SINGLELINE_4 =
         @"[2023-03-30 18:42:40-749][3][][LocalXmlHelper      ][ProcessExecuteNode  ][NORMAL]ProcessExecuteNode(): About to execute: Class: HelperFunctions, Method: GetMaxWithdrawalAmount
[2023-03-30 18:42:40-750][3][][LocalXmlHelper      ][ProcessExecuteNode  ][NORMAL]ProcessExecuteNode(): About to execute: Class: HelperFunctions, Method: FormatCurrency
[2023-03-30 18:42:40-750][3][][Utility             ][MapLangStringToTwoLetterLangCode][NORMAL]langCode return : 
[2023-03-30 18:42:40-750][3][][StateFramework      ][GetTimerData        ][NORMAL]Timer [0] is founded:30000
[2023-03-30 18:42:40-752][3][][StateFramework      ][GetTimerData        ][NORMAL]Timer val:30000
[2023-03-30 18:42:40-753][3][][ScreenFramework     ][ShowScreenCore      ][NORMAL]ShowScreenCore pScreenNumber=[SelectBills] pEnableTouchKeys=[] pKeyInDispType=[None] pInputDisplayScreenNumber=[]
[2023-03-30 18:42:40-754][3][][LocalScreenWindowEx ][ProcessPageRequest  ][NORMAL]Starting to change the display to screen name [SelectBills].
[2023-03-30 18:42:40-755][3][][StandardKeyEntryFlowPoint][ReadKeyDataAsync    ][NORMAL]MaxLen=  0, Autoend True, ActiveKeys = CANCEL,CLEAR, TerminateKeys = CANCEL
[2023-03-30 18:42:40-756][3][][LocalScreenWindowEx ][StartUserInputTimer ][NORMAL]StartUserInputTimer, application pin timer started with 30000
[2023-03-30 18:42:40-763][3][][LocalScreenWindowEx ][<ProcessPageRequest>b__0][NORMAL][DispatcherOperation.Completed] for screen [SelectBills].
[2023-03-30 18:42:40-944][3][][LocalScreenWindowEx ][StartUserInputTimer ][NORMAL]StartUserInputTimer, application pin timer started with 30000
[2023-03-30 18:42:40-948][3][][LyncVideoControl    ][get_ActiveAudioDevice][NORMAL]AudioDeviceList[0] => Speakers (Realtek High Definition Audio)/Microphone (Realtek High Definition Audio), ActiveDevice => Speakers (Realtek High Definition Audio)/Microphone (Realtek High Definition Audio)
[2023-03-30 18:42:40-957][3][][LyncVideoControl    ][get_ActiveAudioDevice][NORMAL]AudioDeviceList[0] => Speakers (Realtek High Definition Audio)/Microphone (Realtek High Definition Audio), ActiveDevice => Speakers (Realtek High Definition Audio)/Microphone (Realtek High Definition Audio)
[2023-03-30 18:42:40-977][3][][LocalScreenWindowEx ][LogScreenUpdate     ][NORMAL]TransactionFrame.Navigated event was raised. FrameworkElement.Name[TransactionFrame] FrameworkElement.Visibility[Visible]. Class: LocalScreenWindow.
[2023-03-30 18:42:40-977][3][][LocalScreenWindowEx ][LogScreenUpdate     ][NORMAL]Navigated screen [SelectBills] Visibility property is [Visible]. Class: LocalScreenWindow.
[2023-03-30 18:42:41-014][3][][LocalScreenWindowEx ][AfterPageLoaded     ][NORMAL]CanBeToDisplay = True m_RootWindow.IsActive = True
[2023-03-30 18:42:41-015][3][][LocalScreenWindow   ][ActivateRootWindow  ][NORMAL]m_RootWindow.IsActive = True
[2023-03-30 18:42:41-015][3][][LocalScreenWindow   ][ActivateRootWindow  ][NORMAL]Screen Number Displayed = SelectBills
[2023-03-30 18:42:41-050][3][][errorMessageControl ][OnUnloaded          ][NORMAL]OnUnloaded is called.
";

      public const string SAMPLE_MEMORY_LINE =
         @"[2023-04-19 05:49:48-149][3][][Utility             ][MemoryWatchDog      ][NORMAL] UsedProcessMemory  : 496,041,984 Handle : 9900 NonPage : 215,752 Page : 1,450,088 Peek : 678,563,840 PeekVirtual : 1,570,131,968 PeekWorking : 661,291,008 Private : 496,041,984 GC Total Memory : 113,594,628";

      public const string SAMPLE_TYPETYPEINFO_LINE_A =
         @"[2023-04-19 06:45:28-790] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeA Currency:[USD] Value:[1] SPLCUIndex:[1] SPPCUIndex:[-1]";

      public const string SAMPLE_TYPETYPEINFO_LINE_B =
         @"[2023-04-19 06:45:28-790] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeB Currency:[USD] Value:[5] SPLCUIndex:[2] SPPCUIndex:[-1]";

      public const string SAMPLE_TYPETYPEINFO_LINE_C =
         @"[2023-04-19 06:45:28-791] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeC Currency:[USD] Value:[20] SPLCUIndex:[3] SPPCUIndex:[-1]";

      public const string SAMPLE_TYPETYPEINFO_LINE_D =
         @"[2023-04-19 06:45:28-791] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeD Currency:[USD] Value:[100] SPLCUIndex:[4] SPPCUIndex:[-1]";

      public const string SAMPLE_TYPETYPEINFO_LINE_E =
         @"[2023-04-19 06:45:28-791] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeE Currency:[] Value:[-1] SPLCUIndex:[-1] SPPCUIndex:[-1]";

      public const string SAMPLE_TYPETYPEINFO_LINE_F =
         @"[2023-04-19 06:45:28-791] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeF Currency:[] Value:[-1] SPLCUIndex:[-1] SPPCUIndex:[-1]";

      public const string SAMPLE_TYPETYPEINFO_LINE_G =
         @"[2023-04-19 06:45:28-791] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeG Currency:[] Value:[-1] SPLCUIndex:[-1] SPPCUIndex:[-1]";

      public const string SAMPLE_TYPETYPEINFO_LINE_H =
         @"[2023-04-19 06:45:28-792] [3] [] [CashDispenser] [SetupNoteTypeInfo   ][NORMAL]Set NoteTypeH Currency:[] Value:[-1] SPLCUIndex:[-1] SPPCUIndex:[-1]";

      public const string SAMPLE_BILLMIXLIST_1 =
         @"[2023-07-03 14:33:19-289][3][][HelperFunctions     ][SetBufferWithBillMix][NORMAL]Using BillMixList: 50~0|20~4|10~2|5~0";

      public const string SAMPLE_BILLMIXLIST_2 =
         @"[2023-07-03 15:32:57-565][3][][HelperFunctions     ][SetBufferWithBillMix][NORMAL]Using BillMixList: 50~0|20~7|10~0|5~0";

      public const string SAMPLE_BILLMIXLIST_3 =
         @"[2023-07-03 09:52:43-698][3][][HelperFunctions     ][SetBufferWithBillMix][NORMAL]Using BillMixList: 50~10|20~10|10~0|5~0";

      public const string SAMPLE_BILLMIXLIST_4 =
         @"[2023-07-03 12:31:53-487][3][][HelperFunctions     ][SetBufferWithBillMix][NORMAL]Using BillMixList: 50~0|20~1|10~0|5~0";

      public const string SAMPLE_BILLMIXLIST_5 =
         @"[2023-07-05 14:20:02-922][3][][HelperFunctions     ][SetBufferWithBillMix][NORMAL]Using BillMixList: 50~0|20~1|10~1|5~0";

      public const string SAMPLE_CASHDEPOT_LINE =
         @"  INFO [2023-07-24 22:05:09-397] [PowerUpMode.UpdateRMSMonitorLEDs] Current Mode: PowerUp";
   }
}
