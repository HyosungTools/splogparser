using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class BeeHDVideoControl : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "BeeHDVideoControl";
      private bool isRecognized = false;


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

            //UpdateCurrentVideoSessionState: Update video session state: SessionStopped

            //VideoClient_CallStateChanges: callHandle=276570400, prevCallState=RVV2OIP_CALLSTATECONNECTED, callState=RVV2OIP_CALLSTATEDROPPING, callStateReason=RVV2OIP_CALLSTATEREASON_IDLE, callType=RVV2OIP_CALLTYPEVIDEO, remoteCallerName=192.168.53.233, isOutgoing=False
            //VideoClient_OnHistoryRecordAdded=> kmcRequest=192.168.61.39@192.168.61.39, dateTime=1694623441, duration=33, callType=RVV2OIP_CALLTYPEVIDEO, callProtocol=V2OIP_PROTOCOLTYPE_SIP, isOutgoing=False, isMissedCall=False, isEncrypted=False
            //VideoClient_OnNewIncomingCall: callHandle=276570400
            //VideoClient_OnRemoteHold: callHandle=278854480, islocal=True
            //VideoClient_OnRemoteUnHold: callHandle=278854480, islocal=True
            //VideoClient_OnUserNotify: callHandle=0, val=2013, severity=SEVERITY_WARNING, userType=NOTIFIER_USER_TYPE_ALL, description=The Feature is Not Supported in the License, additionalInfo=McType MCPTT is not licensed, suggestedAction= 

            string subtag = "AcceptVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("VideoCallState", "ACCEPT");
               isRecognized = true;
            }

            subtag = "HoldVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("VideoCallState", "HOLD");
               isRecognized = true;
            }

            subtag = "ResumeVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("VideoCallState", "RESUME");
               isRecognized = true;
            }

            subtag = "StopVideoCall";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("VideoCallState", "STOP");
               isRecognized = true;
            }

            subtag = "Uninitialize";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("VideoCallState", "UNINITIALIZE");
               isRecognized = true;
            }

            Regex regex = new Regex("Initialize: (?<topic>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("VideoCallState", m.Groups["topic"].Value);
               isRecognized = true;
            }

            regex = new Regex("UpdateCurrentVideoSessionState: Update (?<sessiontype>.*): (?<newstate>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("VideoCallState", "UPDATE");
               SettingDict.Add("SessionType", m.Groups["sessiontype"].Value);
               SettingDict.Add("VideoSessionState", m.Groups["newstate"].Value);
               isRecognized = true;
            }

            regex = new Regex("VideoClient_CallStateChanges: callHandle=(?<handle>.*), prevCallState=(?<prevstate>.*), callState=(?<newstate>.*), callStateReason=(?<reason>.*), callType=(?<calltype>.*), remoteCallerName=(?<remoteaddress>.*), isOutgoing=(?<isoutgoing>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("VideoCallState", "CHANGE");
               SettingDict.Add("CallHandle", m.Groups["handle"].Value);
               SettingDict.Add("PreviousCallState", m.Groups["prevstate"].Value);
               SettingDict.Add("NewCallState", m.Groups["newstate"].Value);
               SettingDict.Add("Reason", m.Groups["reason"].Value);
               SettingDict.Add("CallType", m.Groups["calltype"].Value);
               SettingDict.Add("RemoteCaller", m.Groups["remoteaddress"].Value);
               SettingDict.Add("CallDirection", (bool.Parse(m.Groups["isoutgoing"].Value) ? "OUTGOING" : "INCOMING"));
               isRecognized = true;
            }

            regex = new Regex("GetCallDevices: (?<topic>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("CallDevicesState", m.Groups["topic"].Value);
               isRecognized = true;
            }

            regex = new Regex("SetSelected(?<device>.*): (?<topic>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("SelectedDevice", m.Groups["device"].Value);
               SettingDict.Add("DeviceAction", m.Groups["topic"].Value);
               isRecognized = true;
            }

            regex = new Regex("SignInServer: SignURI = (?<uri>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("ServerSignInURI", m.Groups["uri"].Value);
               isRecognized = true;
            }

            regex = new Regex("VideoClient_OnHistoryRecordAdded=> kmcRequest=(?<remoteaddress>.*), dateTime=(?<datetime>.*), duration=(?<duration>.*), callType=(?<calltype>.*), callProtocol=(?<protocol>.*), isOutgoing=(?<isoutgoing>.*), isMissedCall=(?<ismissedcall>.*), isEncrypted=(?<isencrypted>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               //kmcRequest=(?<remote>.*), dateTime=(?<datetime>.*), duration=(?<duration>.*), callType=(?<calltype>.*), callProtocol=(?<protocol>.*), isOutgoing=(?<isoutgoing>.*), isMissedCall=(?<ismissedcall>.*), isEncrypted=(?<isencrypted>.*)");
               SettingDict.Add("VideoCallState", "HISTORY");
               SettingDict.Add("RemoteCaller", m.Groups["remoteaddress"].Value);
               SettingDict.Add("DateTime", m.Groups["datetime"].Value);
               SettingDict.Add("Duration", m.Groups["duration"].Value);
               SettingDict.Add("CallType", m.Groups["calltype"].Value);
               SettingDict.Add("CallProtocol", m.Groups["protocol"].Value);
               SettingDict.Add("CallDirection", (bool.Parse(m.Groups["isoutgoing"].Value) ? "OUTGOING" : "INCOMING"));
               SettingDict.Add("MissedCall", (bool.Parse(m.Groups["ismissedcall"].Value) ? "MISSED" : string.Empty));
               SettingDict.Add("Encrypted", (bool.Parse(m.Groups["isencrypted"].Value) ? "ENCRYPTED" : string.Empty));
               isRecognized = true;
            }

            regex = new Regex("VideoClient_OnNewIncomingCall: callHandle=(?<handle>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("VideoCallState", "NEW INCOMING CALL");
               SettingDict.Add("CallHandle", m.Groups["handle"].Value);
               isRecognized = true;
            }

            regex = new Regex("VideoClient_OnRemote(?<action>.*): callHandle=(?<handle>.*), islocal=(?<islocal>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("VideoCallState", $"REMOTE-{m.Groups["action"].Value}");
               SettingDict.Add("CallHandle", m.Groups["handle"].Value);
               SettingDict.Add("Local", (bool.Parse(m.Groups["islocal"].Value) ? "LOCAL HOLD" : "REMOTE HOLD"));
               isRecognized = true;
            }

            regex = new Regex("VideoClient_OnUserNotify: callHandle=(?<handle>.*), val=(?<value>.*), severity=(?<severity>.*), userType=(?<usertype>.*), description=(?<description>.*), additionalInfo=(?<additionalinfo>.*), suggestedAction=(?<suggestionaction>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("VideoCallState", $"USER NOTIFY");
               SettingDict.Add("CallHandle", m.Groups["handle"].Value);
               SettingDict.Add("Value", m.Groups["value"].Value);
               SettingDict.Add("Severity", m.Groups["severity"].Value);
               SettingDict.Add("UserType", m.Groups["usertype"].Value);
               SettingDict.Add("Description", m.Groups["description"].Value);
               SettingDict.Add("AdditionalInfo", m.Groups["additionalinfo"].Value);
               SettingDict.Add("SuggestedAction", m.Groups["suggestedaction"].Value);
               isRecognized = true;
            }

         }

         if (!isRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
