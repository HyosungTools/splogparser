namespace Samples
{
   public static class samples_logLine_eof
   {
      public const string SAMPLES_EOF_1 =
         @"[2023-03-30 23:14:58-436][3][][GuideLights         ][WaitCommandComplete ][NORMAL]WaitCommandComplete returns [ERROR]
[2023-03-30 23:15:06-008][3][][GuideLights         ][OnGuideLightChanged ][NORMAL]m_NxGuideLights.OnGuideLightChanged event received
[2023-03-30 23:15:06-010][3][][GuideLights         ][OnGuideLightChanged ][NORMAL]GuideLightUnit[CARDUNIT]
[2023-03-30 23:15:06-011][3][][GuideLights         ][OnGuideLightChanged ][NORMAL]State[MEDIUM]
";

      public const string SAMPLES_EOF_2 =
         @"[2023-03-30 14:56:57-381][3][][CashDispenser       ][WriteParameters     ][NORMAL]WaitCommandComplete: [330000],[1]
[2023-03-30 14:56:57-384][3][][CashDispenser       ][OnDenominateComplete][NORMAL]m_NxCashDispenser.OnDenominateComplete event received
[2023-03-30 14:56:57-392][3][][CashDispenser       ][WaitCommandComplete ][NORMAL]WaitCommandComplete returns [OK]
[2023-03-30 14:56:57-395][3][][CashDispenser       ][ExecuteDeviceCommand][NORMAL]m_NxCashDispenser.Denominate result is DenominateComplete
";

      public const string SAMPLES_EOF_3 =
         @"[2023-03-30 18:08:32-861][3][][LyncVideoControl    ][get_ActiveAudioDevice][NORMAL]AudioDeviceList[0] => Speakers (Realtek High Definition Audio)/Microphone (Realtek High Definition Audio), ActiveDevice => Speakers (Realtek High Definition Audio)/Microphone (Realtek High Definition Audio)
[2023-03-30 18:08:32-862][3][][EMVProcessing       ][GenerateARQC        ][NORMAL]GenerateARQC
[2023-03-30 18:08:32-863][3][][CardReader          ][EMV_OffDataAuth     ][NORMAL][CardReader Log] Call EMV_OffDataAuth()
[2023-03-30 18:08:32-865][3][][CardReader          ][EMV_OffDataAuth     ][NORMAL][CardReader Log] Return EMV_OffDataAuth() : 1
[2023-03-30 18:08:32-868][3][][CardReader          ][EMV_ProcRestriction ][NORMAL][CardReader Log] Call EMV_ProcRestriction()
[2023-03-30 18:08:32-869][3][][CardReader          ][SetMinimumAppVersionNumber][NORMAL]Return without processing. KernelType7 is supported. 
[2023-03-30 18:08:32-876][3][][LocalScreenWindowEx ][LogScreenUpdate     ][NORMAL]TransactionFrame.Navigated event was raised. FrameworkElement.Name[TransactionFrame] FrameworkElement.Visibility[Visible]. Class: LocalScreenWindow.
";

      public const string SAMPLES_EOF_4 =
         @"[2023-03-30 11:15:54-701][3][][HybridFlowEngine    ][GetFlowPoint        ][NORMAL]FlowPoint (Common-BufferReceiptWithImages) was found in the Flow xml file.
[2023-03-30 11:15:54-702][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]****************************************************************************
[2023-03-30 11:15:54-702][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]State created: Common-BufferReceiptWithImages (ReceiptPrintFlowPoint)
[2023-03-30 11:15:54-703][3][][HybridFlowEngine    ][CreateNextState     ][NORMAL]****************************************************************************
[2023-03-30 11:15:54-705][3][][ActiveTeller        ][SendToAgent         ][NORMAL]Sending message to Agent: ApplicationState
";
   }
}
