﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class BeeHDVideoControl : AWLine
   {
      private string className = "BeeHDVideoControl";

      public string ServerSigninState { get; private set; } = string.Empty;
      public string CallDevicesState { get; private set; } = string.Empty;
      public string VideoCallState { get; private set; } = string.Empty;
      public string VideoCallDetails { get; private set; } = string.Empty;


      public BeeHDVideoControl(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.BeeHDVideoControl) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[BeeHDVideoControl   ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //AcceptVideoCall
            //HoldVideoCall
            //ResumeVideoCall
            //StopVideoCall
            //Uninitialize

            //GetCallDevices: Successfully set AV devices.

            //Initialize: Initialized BEEHD Client. SDK version = 4.7.24.8 
            //Initialize: Video client initializing.

            //SetSelectedCamera: Attempting to set the selected camera HD Pro Webcam C920
            //SetSelectedCamera: BeeHD set camera to [HD Pro Webcam C920] at index [0]
            //SetSelectedMicrophone: Attempting to set the selected microphone [Headset Microphone (Plantronics Blackwire 3220 Series)]
            //SetSelectedMicrophone: BeeHD set microphone to [Headset Microphone (Plantronics Blackwire 3220 Series)] at index [0]
            //SetSelectedSpeaker: Attempting to set the selected speakers Headset Earphone (Plantronics Blackwire 3220 Series)
            //SetSelectedSpeaker: BeeHD set speaker to [Headset Earphone (Plantronics Blackwire 3220 Series)] at index [1]

            //SignInServer: SignURI = 192.168.20.144
            //SignInServer: SignURI = 

            //UpdateCurrentVideoSessionState: Update video session state: SessionStopped

            //Update video session state: Invalid
            //Update video session state: ServiceStarted

            //VideoClient_CallStateChanges: callHandle=276570400, prevCallState=RVV2OIP_CALLSTATECONNECTED, callState=RVV2OIP_CALLSTATEDROPPING, callStateReason=RVV2OIP_CALLSTATEREASON_IDLE, callType=RVV2OIP_CALLTYPEVIDEO, remoteCallerName=192.168.53.233, isOutgoing=False
            //VideoClient_OnHistoryRecordAdded=> kmcRequest=192.168.61.39@192.168.61.39, dateTime=1694623441, duration=33, callType=RVV2OIP_CALLTYPEVIDEO, callProtocol=V2OIP_PROTOCOLTYPE_SIP, isOutgoing=False, isMissedCall=False, isEncrypted=False
            //VideoClient_OnNewIncomingCall: callHandle=276570400
            //VideoClient_OnRemoteHold: callHandle=278854480, islocal=True
            //VideoClient_OnRemoteUnHold: callHandle=278854480, islocal=True
            //VideoClient_OnUserNotify: callHandle=0, val=2013, severity=SEVERITY_WARNING, userType=NOTIFIER_USER_TYPE_ALL, description=The Feature is Not Supported in the License, additionalInfo=McType MCPTT is not licensed, suggestedAction= 

            //VideoClient_OnUserNotify=> callHandle=0, val=2011, severity=NOTIFIER_SEVERITY_TYPE_INFO, userType=NOTIFIER_USER_TYPE_ALL, description=The License Validation Completed Successfully, additionalInfo=, suggestedAction= 
            //VideoClient_OnDivertNotification=> reason=RVV2OIP_CALL_DIVERT_REASON_NONE, reasonText=, retValue=True
            //VideoClient_OnHistoryRecordAdded=> kmcRequest=10.50.240.11@10.50.240.11, dateTime=1698932735, duration=131, callType=RVV2OIP_CALLTYPEVIDEO, callProtocol=V2OIP_PROTOCOLTYPE_SIP, isOutgoing=False, isMissedCall=False 

            //VideoClient_OnClientIdentityRegStateChanged=>identityID=Identity-Sip_1, identityProtocolType=V2OIP_PROTOCOLTYPE_SIP, identityHandle=381937848, identityRegState=RVV2OIP_IDENTITYREGSTATEIDLE, identityRegReason=RVV2OIP_IDENTITYREGREASONSERVERADDRESSNOTCONFIGURED

            //Video client initializing.

            //System.Runtime.InteropServices.COMException (0x8000FFFF): Creating an instance of the COM component with CLSID {0BC0A80C-C2CD-477A-B604-930D9331BB54} from the IClassFactory failed due to the following error: 8000ffff Catastrophic failure (Exception from HRESULT: 0x8000FFFF (E_UNEXPECTED)).

            string subtag = "AcceptVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               VideoCallState = "ACCEPT";
               IsRecognized = true;
            }

            subtag = "HoldVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               VideoCallState = "HOLD";
               IsRecognized = true;
            }
            
            subtag = "ResumeVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               VideoCallState = "RESUME";
               IsRecognized = true;
            }

            subtag = "Video client initializing.";
            if (subLogLine.StartsWith(subtag))
            {
               VideoCallState = "INITIALIZING";
               IsRecognized = true;
            }

            subtag = "StopVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               VideoCallState = "STOP";
               IsRecognized = true;
            }

            subtag = "Uninitialize";
            if (subLogLine.StartsWith(subtag))
            {
               VideoCallState = "UNINITIALIZE";
               IsRecognized = true;
            }

            Regex regex = new Regex("Initialize: (?<topic>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"INITIALIZE {m.Groups["topic"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Update video session state: (?<state>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"UPDATE SESSION STATE - {m.Groups["state"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("UpdateCurrentVideoSessionState: Update (?<sessiontype>.*): (?<newstate>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"UPDATE {m.Groups["sessiontype"].Value} {m.Groups["newstate"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_CallStateChanges: callHandle=(?<handle>.*), prevCallState=(?<prevstate>.*), callState=(?<newstate>.*), callStateReason=(?<reason>.*), callType=(?<calltype>.*), remoteCallerName=(?<remoteaddress>.*), isOutgoing=(?<isoutgoing>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = "CHANGE";
               StringBuilder sb = new StringBuilder();
               sb.Append($"CallHandle {m.Groups["handle"].Value},");
               sb.Append($"PreviousCallState {m.Groups["prevstate"].Value},");
               sb.Append($"NewCallState {m.Groups["newstate"].Value},");
               sb.Append($"Reason {m.Groups["reason"].Value},");
               sb.Append($"CallType {m.Groups["calltype"].Value},");
               sb.Append($"RemoteCaller {m.Groups["remoteaddress"].Value},");
               sb.Append($"CallDirection {(bool.Parse(m.Groups["isoutgoing"].Value) ? "OUTGOING" : "INCOMING")},");
               VideoCallDetails = sb.ToString();
               IsRecognized = true;
            }

            regex = new Regex("GetCallDevices: (?<topic>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               CallDevicesState = m.Groups["topic"].Value;
               IsRecognized = true;
            }

            regex = new Regex("SetSelected(?<device>.*): (?<topic>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               CallDevicesState = $"Selected {m.Groups["device"].Value}, Action {m.Groups["topic"].Value}";
               IsRecognized = true;
            }

            //[2023-12-22 11:32:43-103][3][BeeHDVideoControl   ]SignInServer: SignURI = 
            regex = new Regex("SignInServer => ");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ServerSigninState = $"SignInServer";
               IsRecognized = true;
            }

            regex = new Regex("SignInServer: SignURI = ");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ServerSigninState = $"SignInServer";
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnHistoryRecordAdded=> kmcRequest=(?<remoteaddress>.*), dateTime=(?<datetime>.*), duration=(?<duration>.*), callType=(?<calltype>.*), callProtocol=(?<protocol>.*), isOutgoing=(?<isoutgoing>.*), isMissedCall=(?<ismissedcall>.*), isEncrypted=(?<isencrypted>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = "HISTORY";
               StringBuilder sb = new StringBuilder();
               sb.Append($"DateTime {m.Groups["datetime"].Value},");
               sb.Append($"Duration {m.Groups["duration"].Value},");
               sb.Append($"CallType {m.Groups["calltype"].Value},");
               sb.Append($"CallProtocol {m.Groups["protocol"].Value},");
               sb.Append($"CallDirection {(bool.Parse(m.Groups["isoutgoing"].Value) ? "OUTGOING" : "INCOMING")},");
               sb.Append($"MissedCall {(bool.Parse(m.Groups["ismissedcall"].Value) ? "MISSED" : string.Empty)},");
               sb.Append($"Encrypted {(bool.Parse(m.Groups["isencrypted"].Value) ? "ENCRYPTED" : string.Empty)},");
               VideoCallDetails = sb.ToString();
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnNewIncomingCall: callHandle=(?<handle>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"NEW INCOMING CALL handle {m.Groups["handle"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnNewIncomingCall=> callHandle=(?<handle>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"NEW INCOMING CALL handle {m.Groups["handle"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnRemote(?<action>.*): callHandle=(?<handle>.*), islocal=(?<islocal>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"REMOTE-{m.Groups["action"].Value}, handle {m.Groups["handle"].Value}, {(bool.Parse(m.Groups["islocal"].Value) ? "LOCAL HOLD" : "REMOTE HOLD")}";
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnUserNotify: callHandle=(?<handle>.*), val=(?<value>.*), severity=(?<severity>.*), userType=(?<usertype>.*), description=(?<description>.*), additionalInfo=(?<additionalinfo>.*), suggestedAction=(?<suggestionaction>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"VIDEO EVENT";
               StringBuilder sb = new StringBuilder();
               sb.Append($"Severity {m.Groups["severity"].Value},");
               sb.Append($"Description {m.Groups["description"].Value},");
               if (!string.IsNullOrEmpty(m.Groups["additionalinfo"].Value))
               {
                  sb.Append($"AdditionalInfo {m.Groups["additionalinfo"].Value},");
               }

               if (!string.IsNullOrEmpty(m.Groups["suggestedaction"].Value))
               {
                  sb.Append($"SuggestedAction {m.Groups["suggestedaction"].Value},");
               }

               VideoCallDetails = sb.ToString();
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnClientIdentityRegStateChanged=>identityID=(?<id>.*), identityProtocolType=(?<protocol>.*), identityHandle=(?<handle>[0-9]*), identityRegState=(?<regstate>.*), identityRegReason=(?<regreason>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = "REGISTER";
               StringBuilder sb = new StringBuilder();
               sb.Append($"Identity {m.Groups["id"].Value},");
               sb.Append($"Protocol {m.Groups["protocol"].Value},");
               sb.Append($"Handle {m.Groups["handle"].Value},");
               sb.Append($"State {m.Groups["regstate"].Value},");
               sb.Append($"Reason {m.Groups["regreason"].Value},");
               VideoCallDetails = sb.ToString();
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnDivertNotification=> reason=(?<reason>.*), reasonText=(?<reasontext>.*), retValue=(?<retvalue>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = "DIVERT";
               StringBuilder sb = new StringBuilder();
               sb.Append($"Reason {m.Groups["reason"].Value} {m.Groups["reasontext"]},");
               sb.Append($"Retvalue {m.Groups["retvalue"].Value},");
               VideoCallDetails = sb.ToString();
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnHistoryRecordAdded=> kmcRequest=(?<uri>.*), dateTime=(?<time>.*), duration=(?<duration>.*), callType=(?<calltype>.*), callProtocol=(?<protocol>.*), isOutgoing=(?<outgoing>.*), isMissedCall=(?<missed>.*) ");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = "HISTORY";
               StringBuilder sb = new StringBuilder();
               sb.Append($"Uri {m.Groups["uri"].Value},");
               sb.Append($"Time {m.Groups["time"].Value},");
               sb.Append($"Duration {m.Groups["duration"].Value},");
               sb.Append($"Type {m.Groups["calltype"].Value},");
               sb.Append($"Protocol {m.Groups["protocol"].Value},");
               sb.Append($"Outgoing {m.Groups["outgoing"].Value},");
               sb.Append($"Missed {m.Groups["missed"].Value},");
               VideoCallDetails = sb.ToString();
               IsRecognized = true;
            }

            regex = new Regex("VideoClient_OnUserNotify=> callHandle=(?<handle>.*), val=(?<value>.*), severity=(?<severity>.*), userType=(?<usertype>.*), description=(?<description>.*), additionalInfo=(?<additionalinfo>.*), suggestedAction=(?<suggestionaction>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               VideoCallState = $"VIDEO EVENT";
               StringBuilder sb = new StringBuilder();
               sb.Append($"Severity {m.Groups["severity"].Value},");
               sb.Append($"Description {m.Groups["description"].Value},");
               if (!string.IsNullOrEmpty(m.Groups["additionalinfo"].Value))
               {
                  sb.Append($"AdditionalInfo {m.Groups["additionalinfo"].Value},");
               }

               if (!string.IsNullOrEmpty(m.Groups["suggestedaction"].Value))
               {
                  sb.Append($"SuggestedAction {m.Groups["suggestedaction"].Value},");
               }

               VideoCallDetails = sb.ToString();
               IsRecognized = true;
            }

            //System.Runtime.InteropServices.COMException (0x8000FFFF): Creating an instance of the COM component with CLSID {0BC0A80C-C2CD-477A-B604-930D9331BB54} from the IClassFactory failed due to the following error: 8000ffff Catastrophic failure (Exception from HRESULT: 0x8000FFFF (E_UNEXPECTED)).
            //   at NH.ActiveTeller.VideoControl.BeeHDVideoControl.SignInServer(VideoSignInArgs pSignInInfo)
            if (subLogLine.Contains("Exception") || subLogLine.StartsWith("   at "))
            {
               VideoCallState = "EXCEPTION";
               VideoCallDetails = subLogLine;
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
