using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSamples
{
   public static class samples_apfilereader
   {
      public const string multiLine =
@"[2023-12-07 20:10:17-047][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]tag value from TAGID.ini file :[37]
[2023-12-07 20:10:17-048][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]tag value from m_AxEMV.EMV_GetVal :[].
[2023-12-07 20:10:17-049][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]Tag ID 9F45 (37) value is empty
[2023-12-07 20:10:17-049][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]tag :[9F4C]
[2023-12-07 20:10:17-050][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]tag value from TAGID.ini file :[44]
[2023-12-07 20:10:17-051][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]tag value from m_AxEMV.EMV_GetVal :[].
[2023-12-07 20:10:17-051][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]Tag ID 9F4C (44) value is empty
[2023-12-07 20:10:17-051][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]tag :[9F34] is duplicate,discard
[2023-12-07 20:10:17-052][3][][EMVTagDefinition    ][AppendTLVOfCDOL     ][NORMAL]tag :[9F21]
";

      public const string multiLine_2 =
@"[2023-12-07 20:10:17-128][3][][NDCTransactionRequestState][ExecuteState        ][NORMAL]Executing the SendTransactionMessage method.
[2023-12-07 20:10:17-197][3][][ManagementFramework ][SendMessage         ][NORMAL]Parameter pRecordId:1010
[2023-12-07 20:10:17-198][3][][ManagementFramework ][SendMessage         ][NORMAL]Parameter pData    :IA    AA
[2023-12-07 20:10:17-199][3][][MessageFramework    ][MakeTransactionRequestMessage][NORMAL]Parameter pCondition:Track1:False
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

[2023-12-07 20:10:17-199][3][][MessageFramework    ][MakeMessage         ][NORMAL]Parameter pCmd:TRANSACT_REQUEST
[2023-12-07 20:10:17-200][3][][MessageFramework    ][MakeMessage         ][NORMAL]Parameter pExt:NONE
[2023-12-07 20:10:17-200][3][][MessageFramework    ][NeedMacData         ][NORMAL]Parameter pCmd:TRANSACT_REQUEST
[2023-12-07 20:10:17-201][3][][BuilderNDCMessage   ][MakeMessage         ][NORMAL]Parameter pDeviceStatus:Fitness:[]MCode:[]TCode:[]StatusDDC:[]
[2023-12-07 20:10:17-201][3][][BuilderNDCMessage   ][MakeMessage         ][NORMAL]Parameter pCmd:TRANSACT_REQUEST
";

      public const string multiLine_3 =
@"[2023-12-07 23:58:07-844][3][][SupplyStatus        ][GetPaperStatus      ][NORMAL]Paper status:0
[2023-12-07 23:58:07-881][3][][SupplyCountInformation][AddFrame            ][NORMAL]Duplicated logical index was founded in CDMFrameStatus
[2023-12-07 23:58:07-897][3][][LocalXmlHelper      ][LoadConfigurationFiles][NORMAL]Finished loading xml: C:\Hyosung\MoniPlus2S\ConfigApplication\Application\OutgoingMessage.xml
[2023-12-07 23:58:07-899][3][][CommunicationFramework][TransmitData        ][NORMAL]MAC TVN CHECK:
[2023-12-07 23:58:07-900][0][][Log                 ][Send                ][SEND]ATM2HOST: 22177F1117700000000000000000000000000000000000000167F001D01020580000000C7000000010200007F7F000000000001000000000001000000000000001111000000011110000000100001011111
[2023-12-07 23:58:07-900][3][][SockAdapter         ][Send                ][NORMAL]Socket Send SocketCode=16940691 Socket Handle : 37732
[2023-12-07 23:58:07-901][3][][SockAdapter         ][Send                ][NORMAL]remoteIP = 10.204.97.46
[2023-12-07 23:58:07-911][3][][SockAdapter         ][Send                ][NORMAL]send 173bytes
[2023-12-07 23:58:07-912][3][][TcpComm             ][OnReceivedSockEvent ][NORMAL]Socket event arrived: SendComplete
";
   }
}
