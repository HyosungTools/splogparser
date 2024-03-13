#undef USE_OLDCODE


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq.Expressions;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Contract;
using SipSession;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace LogLineHandler
{
   public class BELine : LogLine, ILogLine
   {

      /// <summary>
      /// return the summaries to the BHDLogHandler
      /// </summary>
      /// <returns></returns>
      public static List<string> SipSessionSummaries()
      {
         List<string> summaries = new List<string>();

         foreach (SipSession.SipSession session in _sipSessions)
         {
            summaries.Add(session.Summary().ToString());
         }

         return summaries;
      }

      /// <summary>
      /// returns a list of tables that describe SIP sessions
      /// </summary>
      /// <returns></returns>
      public static List<DataTable> SipSessionTables()
      {
         List<DataTable> dataTables = new List<DataTable>();

         if (_sipSessions != null)
         {
            foreach (SipSession.SipSession session in _sipSessions)
            {
               if (session.TimeSeriesTable != null)
               {
                  // merge the lines of the SessionSummary into the TimeSeriesTable 'summary' column
                  string[] summaryLines = session.Summary().ToString().Split( new char[] { '\r', '\n' });
                  int summaryIndex = 0;
                  bool isCSV = false;

                  foreach (DataRow row in session.TimeSeriesTable.Rows)
                  {
                     // find the next non-empty summary line
                     while( summaryIndex < summaryLines.Length && string.IsNullOrWhiteSpace(summaryLines[summaryIndex]))
                     {
                        summaryIndex++;
                     }

                     if (summaryIndex < summaryLines.Length)
                     {
                        if (summaryLines[summaryIndex].StartsWith("** CSV Time Buckets"))
                        {
                           // exclude the CSV Time Bucket lines
                           isCSV = true;
                        }

                        if (!isCSV)
                        {
                           row["summary"] = summaryLines[summaryIndex];
                        }

                        summaryIndex++;
                     }

                     row.AcceptChanges();
                  }

                  dataTables.Add(session.TimeSeriesTable);
               }
            }
         }

         return dataTables;
      }


      /// <summary>
      /// Release the memory occupied by SipSession objects
      /// </summary>
      public static void ReleaseSipSessionTables()
      {
         if (_sipSessions == null)
         {
            return;
         }

         foreach (SipSession.SipSession session in _sipSessions)
         {
            session.Dispose();
            // leave the session in the list, otherwise the foreach(..) indexing breaks
         }

         _sipSessions.Clear();
      }

      private static string _logFileVersion = string.Empty;
      private static string _logFileProduct = string.Empty;
      private static bool _interestingLogLine = false;


      private static List<SipSession.SipSession> _sipSessions = new List<SipSession.SipSession>();

      private static SipSession.SipSession _activeSipSession;
      private static SipSession.SipMessage _sipMessage;
      private static bool _isMultilineSipMessageProcessedFlag = false;     // used to detect message completion at the end of the loop

      private enum CallState { Idle, Connecting, Connected, ConnectedOfferMedia, Disconnecting };

      // statics hold the context of a multi-line SIP/SDP message
      private static string _sipMessageDirection = string.Empty;
      private static string _sipMessageProtocol = string.Empty;
      private static string _timeOfLastMessage = string.Empty;
      private static string _timeSinceLastMessage = string.Empty;

      private static string _sipMessageTypeHeader = string.Empty;
      private static string _sipCSeqHeader = string.Empty;
      private static string _sipCallIdHeader = string.Empty;
      private static string _sipTransactionIdHeader = string.Empty;
      private static string _sipContactHeader = string.Empty;
      private static string _sipSessionOriginatorHeader = string.Empty;
      private static string _sipRportOption = string.Empty;
      private static string _sipCallState = "idle";
      private static string _sipAudioState = string.Empty;
      private static string _sipVideoState = string.Empty;

      private static string _speakerState = string.Empty;
      private static string _microphoneState = string.Empty;
      private static string _cameraState = string.Empty;
      private static string _audioTransmitterState = string.Empty;
      private static string _audioReceiverState = string.Empty;
      private static string _videoTransmitterState = string.Empty;
      private static string _videoReceiverState = string.Empty;

      private static string _speakerDevice = string.Empty;
      private static string _microphoneDevice = string.Empty;
      private static string _cameraDevice = string.Empty;

      private static int _speakerVolume;

      private static long _lastIncomingAudioSeqReported = 0;


      // interesting tags in the BeeHD log
      private static string[] interestingTags = new string[] {
         "CH_MAN",      // hardware devices (microphone, camera)
         "CALL",        // call engine state tracking
         "MEDIA",       // media streams
         "RTCP",        // streaming errors
         "TRANSPORT",   // SIP messages
         "AUDIO_TR",    // audio transmitter
         "AUDIO_RE",    // audio rendered
         "VE_CALL",     // older versions have SDP but no SIP
         "VE_MGR",      // not very interesting except the product version
         "VIDEO_TR",    // video transmitter
         "VIDEO_RE"     // video renderer
      };

      private static string[] uninterestingTags = new string[] {
         "EMA",
         "FW_MEMORY",
         "INTERF",
         "HTTP_MGR",
         "MESSAGE",
         "REG_CLIENT",
         "RESOLVER",
         "SECURITY",
         "SOCKET",      // setting socket type
         "THREAD",
         "VE_IDENTITY", // associated with Registering
         "VE_IDMC",
         "VE_MESSAGE",
         "VE_MUTEX",
         "VE_USERAPP",
         "VE_RULES",
         "XDMCORE",
         "XDMHTTP"
      };

      private static string[] MicrophoneDeviceStrings = new string[] {
         "mic"
      };

      private static string[] SpeakerDeviceStrings = new string[] {
         "speak",
         "ear"
      };

      private static string[] CameraDeviceStrings = new string[] {
         "cam"
      };

      private bool success = false;

      // implementations of the ILogLine interface
      public string Timestamp { get; set; }
      public string HResult { get; set; }


      public bool isLogSipMessage = false;
      public string lineNumber = string.Empty;
      public string headTag = string.Empty;
      public string interestingTag = string.Empty;
      public string threadId = string.Empty;
      public string className = string.Empty;
      public string methodName = string.Empty;
      public string source = string.Empty;
      public string logLevel = string.Empty;
      public string payLoad = string.Empty;
      public string direction = string.Empty;
      public string protocol = string.Empty;
      public string callState = string.Empty;
      public string audioState = string.Empty;
      public string videoState = string.Empty;
      public string audioTransmitterState = string.Empty;
      public string audioReceiverState = string.Empty;
      public string videoTransmitterState = string.Empty;
      public string videoReceiverState = string.Empty;
      public string deviceStateChange = string.Empty;
      public string timeSinceLastMessage = string.Empty;
      public string msgHeader = string.Empty;
      public string callIdHeader = string.Empty;
      public string transactionId = string.Empty;
      public string contact = string.Empty;
      public string sessionOriginator = string.Empty;
      public string rportOption = string.Empty;
      public string cseqHeader = string.Empty;
      public string endPoints = string.Empty;
      public string msgNote = string.Empty;
      public string msgExpectNote = string.Empty;
      public string summaryNote = string.Empty;
      public string analysis = string.Empty;

      public double audioSent = 0;
      public double audioReceived = 0;
      public int audioLost = 0;
      public int audioJitter = 0;
      public double videoSent = 0;
      public double videoReceived = 0;
      public int videoLost = 0;
      public int videoJitter = 0;

      public string speakerState = string.Empty;
      public string microphoneState = string.Empty;
      public string cameraState = string.Empty;
      public int microphoneDataCount = 0;
      public int cameraDataCount = 0;


      /// <summary>
      /// NOTE: see rdbeehd.ini
      /// </summary>
      /// <param name="parent"></param>
      /// <param name="logLine">the log text</param>
      public BELine(ILogFileHandler parent, string logLine) : base(parent, logLine)
      {
         // reset all the statics for a new file .. do not reset if the files logically follow one another in time sequence
         //   if all files are from the same terminal or teller and are in a roll-over sequence, make sure the filenames are in
         //    order of ascending time and don't reset the statics between
         //   if files are one from terminal and one from teller, need to reset
         bool resetStaticsForNewFile = false;

         if (parent.LineNumber == 1)
         {
            if (resetStaticsForNewFile)
            {
               _sipSessions = new List<SipSession.SipSession>();
               _sipSessions.Add(new SipSession.SipSession());      // start with a placeholder session which lacks a call-id
               _sipMessage = null;
               _isMultilineSipMessageProcessedFlag = false;
               _sipMessageDirection = string.Empty;
               _sipMessageProtocol = string.Empty;
               _timeOfLastMessage = string.Empty;
               _timeSinceLastMessage = string.Empty;

               _sipMessageTypeHeader = string.Empty;
               _sipCSeqHeader = string.Empty;
               _sipCallIdHeader = string.Empty;
               _sipTransactionIdHeader = string.Empty;
               _sipContactHeader = string.Empty;
               _sipSessionOriginatorHeader = string.Empty;
               _sipRportOption = string.Empty;
               _sipCallState = "idle";
               _sipAudioState = string.Empty;
               _sipVideoState = string.Empty;
               _microphoneState = string.Empty;
               _speakerState = string.Empty;
               _speakerVolume = 0;
               _cameraState = string.Empty;
               _audioTransmitterState = string.Empty;
               _audioReceiverState = string.Empty;
               _videoTransmitterState = string.Empty;
               _videoReceiverState = string.Empty;
            }

            if (_activeSipSession == null)
            {
               if (_sipSessions.Count == 0)
               {
                  _sipSessions.Add(new SipSession.SipSession());      // start with a placeholder session which lacks a call-id
                  _activeSipSession = _sipSessions[0];
               }
            }
         }

         Initialize();
         if (IgnoreThisLine)
         {
            return;
         }

         lineNumber = parent.LineNumber.ToString();

         // remove all \r and \n from the line
         logLine = logLine.Replace("\r", "").Replace("\n", "");

         // parse the log line as part of initialization

         // (?<tag>.{18}) (?<time>.{21}) (?<source>.{11}) : (?<level>.{5}) - ((?<thread>\[[0-9A-F]{8})] ){0,1}(?<rest>.*)
         // (?<tag>.{18}) (?<time>.{21}) (?<source>.{11}) : (?<level>.{5}) - ((?<thread>\[[0-9A-F]{8})] ){0,1}(?<class>.*::){0,1}(?<method2>.* - ){0,1}(?<method>.*:){0,1}(?<rest>.*)
         // (?<tag>.{18}) (?<time>.{21}) (?<source>.{11}) : (?<level>.{5}) - ((?<thread>\[[0-9A-F]{8})] ){0,1}(?<class>.*::){0,1}(?<method1>.* - ){0,1}(?<method2>.*: ){0,1}(?<rest>.*)
         //
         // BEEHD              10/04/23 08:21:35.061 TRANSPORT   : INFO  - TransportUdpEventCallback - sock 13296: notification of network events
         // RV_BEEHD           10/04/23 08:00:58.746 CH_MAN      : INFO  - [0B96E71C] CPALAudioDevice::SetCurrentVolume: this=100CB9AC set volume after linearMapping (50)
         // [1527E13C] CPALDeviceBase: device Speakers (2- Logi USB Headset) m_RefCount = 1 usage = 1 was constructed (150B3254)

         // 5 Speakers (2- Log 10/04/23 08:01:17.736 CH_MAN      : DEBUG - [0B96E71C] CPALAudioOutputDevice::SpkDeliverBufferLoopback: delta time = 80604768 procs time = 0


         // NEWER VERSIONS
         // ==============
         // RV_BEEHD           11/13/23 04:53:34.953 VE_MGR      : DEBUG - v2oipClientReadLicense pClient=1C330020, verify ver.=99 exact=0, where product version=4.7.24

         // OLDER VERSIONS
         // ==============
         //
         // BeeHD product versions - log messages are not the same, in particular 4.3 does not have the SIP/SDP messages listed in the same way as version 4.7:
         //
         // RV_BEEHD           11/13/23 13:43:24.758 VE_MGR      : DEBUG - v2oipClientReadLicense pClient=2192F020, verify ver.=99 exact=0, where product version=4.3.44.4

         // no SIP headers in the log, but there is this ... call state including callid
         // VE_MGR                                                 DEBUG   v2oipClientEventsLogAddEvent - added new event to log (4001) with additional info: 		<name>CallStateChanged</name>		<callid>342170984</callid>		<displayName>192.168.5.130</displayName>		<state>DROPPING</state>		<reason>REMOTE DISCONNECT</reason>

         try
         {
            Regex regex = new Regex("(?<tag>.{18}) (?<time>\\d{2}/\\d{2}/\\d{2} \\d{2}:\\d{2}:\\d{2}\\.\\d{3}) (?<source>.{11}) : (?<level>.{5}) - (?<rest>.*)");
            Match m = regex.Match(logLine);
            success = m.Success;
            if (success)
            {
               headTag = m.Groups["tag"].Value.Trim();
               Timestamp = m.Groups["time"].Value;    // "11/13/23 07:34:23.011"

               source = m.Groups["source"].Value.Trim();
               logLevel = m.Groups["level"].Value.Trim();
               payLoad = m.Groups["rest"].Value.Trim();
               _interestingLogLine = false;
               interestingTag = "no";

               // extract threadid, classname, methodname
               string[] payLoadParts = payLoad.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
               int partIndex = 0;
               if (payLoadParts.Length > 0)
               {
                  string part = payLoadParts[partIndex];

                  // [threadid]?
                  if (part.Length > 8 && part.StartsWith("[") && part.EndsWith("]"))
                  {
                     threadId = part.Substring(1, 8);

                     // special case for
                     // "BEEHD              10/04/23 08:00:55.036 XDMCORE     : INFO  - [===========================================================]"
                     if (threadId == "========")
                     {
                        threadId = string.Empty;
                     }
                     partIndex++;
                  }

                  // TODO: not catching the method for..
                  // CallLegChangeState - Call-leg 0x2241F300 state changed: Offering->Disconnected, (reason = Local Reject)
                  if (partIndex < payLoadParts.Length)
                  {
                     // [classname::[method]]
                     string[] subParts = payLoadParts[partIndex].Trim().Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                     if (subParts.Length == 2)
                     {
                        className = subParts[0];
                        methodName = subParts[1].Replace(":", string.Empty);

                        // special handling for 'xxx(len=9999)'
                        if (methodName.Contains("(len="))
                        {
                           methodName = methodName.Substring(0, methodName.IndexOf("(len="));
                        }
                     }
                     else if (subParts.Length == 1)
                     {
                        // clientunlock()
                        if (subParts[0].EndsWith("()"))
                        {
                           methodName = subParts[0];
                        }
                        else
                        {
                           // method:
                           if (subParts[0].EndsWith(":"))
                           {
                              // avoid recognizing SIP headers as MethodNames
                              //    From: "192.168.60.236"<sip:192.168.60.236@192.168.60.236>;tag=6137e59a-5da4151b-865c-2579b950-ec3ca8c0-13c4-65015

                              if (logLine.Contains(" -    "))
                              {
                              }
                              else
                              {
                                 methodName = subParts[0].Replace(":", string.Empty);
                              }
                           }
                           else if (payLoadParts.Length > 2 && payLoadParts[1] == "-")
                           {
                              methodName = payLoadParts[0];
                           }
                        }
                     }
                  }
               }
            }

            // for speed only consider interesting tags
            bool considerAllSourceTags = false;

            if (considerAllSourceTags || Array.Find(interestingTags, element => element == source) == source)
            {
               _interestingLogLine = true;

               // start using flag - TODO set it in each of the regex below
               IsRecognized = true;

               interestingTag = "yes";

               // analysis based on source name ..
               if (source.StartsWith("AUDIO"))
               {
                  analysis = "audio";
                  direction = source.EndsWith("RE") ? "received" : "sent";
               }

               else if (source.StartsWith("VIDEO"))
               {
                  analysis = "video";
                  direction = source.EndsWith("RE") ? "received" : "sent";
               }

               else if (source == "CALL")
               {
                  // call connection state
                  // RvSipCallLegAccept - Accepting call-leg 0x146C5B50
                  // CallLegChangeState - Call-leg 0x23377BB8 state changed: Accepted->Connected, (reason = Remote Ack)

                  /*
                   * CallLegChangeState - Call-leg 0x1CF78A68 state changed: Idle->Inviting, (reason = Local Inviting)
                   * CallLegChangeState - Call-leg 0x23377BB8 state changed: Idle->Offering, (reason = Remote Inviting)
                   * CallLegChangeState - Call-leg 0x1CF78A68 state changed: Inviting->Proceeding, (reason = Remote Provisional Response)
                   * CallLegChangeState - Call-leg 0x23377BB8 state changed: Offering->Accepted, (reason = Local Accepted)
                   * CallLegChangeState - Call-leg 0x23377BB8 state changed: Accepted->Connected, (reason = Remote Ack)
                   * CallLegChangeState - Call-leg 0x1CF78A68 state changed: Proceeding->RemoteAccepted, (reason = Remote Accepted)
                   * CallLegChangeState - Call-leg 0x1CF78A68 state changed: RemoteAccepted->Connected, (reason = Ack Sent)
                   * CallLegChangeState - Call-leg 0x1CF78A68 state changed: Connected->Disconnected, (reason = Remote Disconnected)
                   * CallLegChangeState - Call-leg 0x23377BB8 state changed: Connected->Disconnecting, (reason = Local Disconnecting)
                   * CallLegChangeState - Call-leg 0x23377BB8 state changed: Disconnecting->Disconnected, (reason = Disconnected)
                   * CallLegChangeState - Call-leg 0x23377BB8 state changed: Disconnected->Terminated, (reason = Call Terminated)
                   * CallLegChangeState - Call-leg 0x1CF78A68 state changed: Disconnected->Terminated, (reason = Call Terminated)
                   */

                  if (methodName == "CallLegChangeState")
                  {
                     string stateChangeInfo = payLoad.Substring(payLoad.IndexOf(methodName) + methodName.Length + 3).ToLower();

                     regex = new Regex("call-leg 0x(?<thread>[0-9A-Fa-f]{8}) state changed: (?<old>.*)->(?<new>.*), \\(reason = (?<reason>.*)\\)");
                     m = regex.Match(stateChangeInfo);
                     success = m.Success;
                     if (success)
                     {
                        _sipCallState = m.Groups["new"].Value;
                     }
                  }
               }

               else if (source == "CH_MAN")
               {
                  if (className == "CWasapiDevicesManager")
                  {
                     if (methodName == "OpenInputDevice" || methodName == "OpenOutputDevice")
                     {
                        string deviceInfo = payLoad.Substring(payLoad.IndexOf("Device ") + 7).ToLower();

                        // Microphone (2- Logi USB Headset) already opened - returning NULL
                        // Speakers (2- Logi USB Headset) opened
                        regex = new Regex("(?<device>\\w*) \\((?<num>[0-9])- (?<name>.*)\\) (?<rest>.*)");
                        m = regex.Match(deviceInfo);
                        success = m.Success;
                        if (success)
                        {
                           string deviceType = m.Groups["device"].Value;
                           int deviceNumber = int.Parse(m.Groups["num"].Value);
                           string deviceName = m.Groups["name"].Value;

                           deviceStateChange = $"{m.Groups["device"].Value} ({m.Groups["num"].Value}- {m.Groups["name"].Value})";

                           if (deviceInfo.Contains("mic"))
                           {
                              if (_microphoneState != "open")
                              {
                                 _microphoneState = "open";
                                 _microphoneDevice = m.Groups["device"].Value;

                                 analysis = "microphone-open";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                           else if (deviceInfo.Contains("speak") || deviceInfo.Contains("ear"))
                           {
                              if (_speakerState != "open")
                              {
                                 _speakerState = "open";
                                 _speakerDevice = m.Groups["device"].Value;

                                 analysis = "speaker-open";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                           else if (deviceInfo.Contains("cam"))
                           {
                              if (_cameraState != "open")
                              {
                                 _cameraState = "open";
                                 _cameraDevice = m.Groups["device"].Value;

                                 analysis = "camera-open";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                        }
                     }
                     // devices opened during the INVITE
                     // [0B96E71C] CWasapiDevicesManager::OpenInputDevice: Device Microphone (2- Logi USB Headset) already opened - returning NULL
                     // [0B96E71C] CWasapiDevicesManager::OpenInputDevice: Device Microphone (4- C922 Pro Stream Webcam) already opened - returning NULL
                     // [1527E13C] CWasapiDevicesManager::OpenOutputDevice: Device Speakers (2- Logi USB Headset) opened
                     // [0B96E71C] CWasapiDevicesManager::OpenOutputDevice: Device Speakers (2- Logi USB Headset) already opened

                  }
                  else if (className == "CSystemDevicesManager")
                  {
                     if (methodName == "PALSetSystemCurrentDevice")
                     {
                        // [0FB26EFC] CSystemDevicesManager::PALSetSystemCurrentDevice: device: Speakers (Realtek High Definition Audio), pDevice=1D8DD570, iDeviceIndex=0
                        // [0FB26EFC] CSystemDevicesManager::PALSetSystemCurrentDevice: device: Microphone (Realtek High Definition Audio), pDevice=0FE286E8, iDeviceIndex=0
                        // [0FB26EFC] CSystemDevicesManager::PALSetSystemCurrentDevice: device: NH_Web CAMERA1, pDevice=1E6992B0, iDeviceIndex=0
                        // [1527E13C] CSystemDevicesManager::PALSetSystemCurrentDevice: device: Microphone (2- Logi USB Headset), pDevice=1490A1B0, iDeviceIndex=1
                        // [1527E13C] CSystemDevicesManager::PALSetSystemCurrentDevice: device: Speakers (2- Logi USB Headset), pDevice=15067680, iDeviceIndex=0
                        // [1527E13C] CSystemDevicesManager::PALSetSystemCurrentDevice: device: Intel(R) Iris(R) Xe Graphics, pDevice=1026D688, iDeviceIndex=0
                        // [1527E13C] CSystemDevicesManager::PALSetSystemCurrentDevice: device: c922 Pro Stream Webcam, pDevice=12ADB2B8, iDeviceIndex=-1
                        // [2321AFE4] CSystemDevicesManager::PALSetSystemCurrentDevice: device: NH_Web CAMERA1, pDevice=3D77A2B0, iDeviceIndex=-1

                        string deviceInfo = payLoad.Substring(payLoad.IndexOf("device: ") + 8).ToLower();

                        // (?<device>.*), pDevice=(?<handle>.*), iDeviceIndex=(?<index>.*)
                        // Microphone (2- Logi USB Headset), pDevice=1490A1B0, iDeviceIndex=1
                        // Speakers (2 - Logi USB Headset), pDevice=15067680, iDeviceIndex=0
                        // Intel(R) Iris(R) Xe Graphics, pDevice=1026D688, iDeviceIndex=0
                        // c922 Pro Stream Webcam, pDevice=12ADB2B8, iDeviceIndex=-1
                        regex = new Regex("(?<device>.*), pdevice=(?<handle>.*), ideviceindex=(?<index>.*)");
                        m = regex.Match(deviceInfo);
                        success = m.Success;
                        if (success)
                        {
                           deviceStateChange = $"set ({m.Groups["index"].Value}- {m.Groups["device"].Value}) {m.Groups["handle"].Value}";

                           // TODO - a better way to parse out the device name
                           string deviceType = m.Groups["device"].Value;
                           int deviceNumber = int.Parse(m.Groups["index"].Value);
                           string deviceName = m.Groups["device"].Value;

                           if (deviceInfo.Contains("mic"))
                           {
                              if (_microphoneState != "on")
                              {
                                 _microphoneState = "on";
                                 _microphoneDevice = m.Groups["device"].Value;

                                 analysis = "microphone-on";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                           else if (deviceInfo.Contains("speak") || deviceInfo.Contains("ear"))
                           {
                              if (_speakerState != "on")
                              {
                                 _speakerState = "on";
                                 _speakerDevice = m.Groups["device"].Value;

                                 analysis = "speaker-on";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                           else if (deviceInfo.Contains("cam"))
                           {
                              if (_cameraState != "on")
                              {
                                 _cameraState = "on";
                                 _cameraDevice = m.Groups["device"].Value;

                                 analysis = "camera-on";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                        }
                     }
                     else if (methodName == "ReleaseDevice")
                     {
                        string deviceInfo = payLoad.Substring(payLoad.IndexOf("device ") + 7).ToLower();

                        // [2321AFE4] CSystemDevicesManager::ReleaseDevice: Going to release device Microphone (Realtek High Definition Audio) 0FE286E8
                        // [2321AFE4] CSystemDevicesManager::ReleaseDevice: Going to release device Speakers (Realtek High Definition Audio) 230D7288
                        // [2321AFE4] CSystemDevicesManager::ReleaseDevice: Going to release device NH_Web CAMERA1 3D77A2B0

                        // Microphone (2- Logi USB Headset) 1490A1B0
                        // c922 Pro Stream Webcam 12ADB2B8
                        // Intel(R) Iris(R) Xe Graphics 1026D688
                        // c922 Pro Stream Webcam 12ADB2B8
                        // 3mp uvc imager 1e6992b0  (ignored)
                        regex = new Regex("(?<device>.*) (?<handle>.*)");
                        m = regex.Match(deviceInfo);
                        success = m.Success;
                        if (success)
                        {
                           string deviceType = m.Groups["device"].Value;
                           string deviceHandle = m.Groups["handle"].Value;
                           //string deviceName = m.Groups["device"].Value;

                           deviceStateChange = $"({m.Groups["device"].Value}) {m.Groups["handle"].Value}";

                           if (deviceInfo.Contains("mic"))
                           {
                              if (_microphoneState != "closed")
                              {
                                 _microphoneState = "closed";

                                 analysis = "microphone-closed";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                           else if (deviceInfo.Contains("speak") || deviceInfo.Contains("ear"))
                           {
                              if (_speakerState != "closed")
                              {
                                 _speakerState = "closed";

                                 analysis = "speaker-closed";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }
                           else if (deviceInfo.Contains("cam"))
                           {
                              if (_cameraState != "closed")
                              {
                                 _cameraState = "closed";

                                 analysis = "camera-closed";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, deviceStateChange, 1);
                              }
                           }

                        }
                     }
                     // devices closed during IDLE
                     // [1527E13C] CSystemDevicesManager::ReleaseDevice: Going to release device Microphone (2- Logi USB Headset) 1490A1B0
                     // [1527E13C] CSystemDevicesManager::ReleaseDevice: Going to release device Speakers (2- Logi USB Headset) 15067680
                     // [1527E13C] CSystemDevicesManager::ReleaseDevice: Going to release device c922 Pro Stream Webcam 12ADB2B8
                     // [1527E13C] CSystemDevicesManager::ReleaseDevice: Going to release device Intel(R) Iris(R) Xe Graphics 1026D688

                     // devices added during INVITE or initialization  (MANY TIMES!!!!)
                     // [0B96E71C] CSystemDevicesManager::PALSetSystemCurrentDevice: device: Microphone (4- C922 Pro Stream Webcam) Released
                     // [0B96E71C] CSystemDevicesManager::PALSetSystemCurrentDevice: device: Integrated Webcam Released

                     // [1527E13C] CSystemDevicesManager::SetCurrentAudioMicDevice: microphone device. pDevice = 1490A1B0 iDeviceIndex=1
                  }

                  else if (className == "CWINVideoInputDevice")
                  {
                     if (methodName == "_SetResolution")
                     {
                        // CH_MAN [0FB26EFC] CWINVideoInputDevice::_SetResolution: bSetCameraOnlyOnce = 0 set camera Res to 1280 x 720  Current is 0 x 0
                        string paramString = payLoad.Substring(payLoad.IndexOf("set camera Res to ") + 18);
                        regex = new Regex("(?<width>[0-9]*) x (?<height>[0-9]*) ");
                        m = regex.Match(paramString);
                        success = m.Success;
                        if (success)
                        {
                           int width = int.Parse(m.Groups["width"].Value);
                           int height = int.Parse(m.Groups["height"].Value);

                           msgNote = $"{width}x{height} pixels";

                           analysis = "camera-set-resolution";
                           _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                        }
                     }
                  }

                  /* CPALVideoInputDevice::MatchOptimalNativeResolution ... inspection of video resolutions offered by a video device
                   * 
                  else if (className == "CPALVideoInputDevice")
                  {
                     if (methodName == "MatchOptimalNativeResolution")
                     {
                        // CH_MAN [0FB26EFC] CPALVideoInputDevice::MatchOptimalNativeResolution: m_iActiveDeviceStream=0 req 640x480 i=5 curr 1280x720 FPS=5 req FPS = 30
                        string paramString = payLoad.Substring(payLoad.IndexOf("m_iActiveDeviceStream=0 req") + 28);
                        regex = new Regex("(?<width>[0-9]*)x(?<height>[0-9]*) .* req FPS = (?<requestedfps>[0-9]*)");
                        m = regex.Match(paramString);
                        success = m.Success;
                        if (success)
                        {
                           int width = int.Parse(m.Groups["width"].Value);
                           int height = int.Parse(m.Groups["height"].Value);
                           int requestedFps = int.Parse(m.Groups["requestedfps"].Value);

                           analysis = $"camera-match-resolution-{width}x{height}pixels-{requestedFps}fps";
                           _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, 1);
                        }

                     }
                  }
                  */

                  else if (className == "CVideoTransmitterChannel")
                  {
                     if (methodName == "UpdateCameraResolution")
                     {
                        // CH_MAN [2321AFE4] CVideoTransmitterChannel::UpdateCameraResolution: frameRateToUse=15, resolution=7, bitrateToUse=1856000, bLandscapeEn=1, bPortraitEn=1 scaled 640x480
                        string paramString = payLoad.Substring(payLoad.IndexOf("UpdateCameraResolution: ") + 24);
                        regex = new Regex("frameRateToUse=(?<framerate>[0-9]*), resolution=(?<resolution>[0-9]*), bitrateToUse=(?<bitrate>[0-9]*), bLandscapeEn=(?<landscapeen>[0-9]*), bPortraitEn=(?<portraiten>[0-9]*) (?<scaled>\\w*) (?<width>[0-9]*)x(?<height>[0-9]*)");
                        m = regex.Match(paramString);
                        success = m.Success;
                        if (success)
                        {
                           int frameRate = int.Parse(m.Groups["framerate"].Value);
                           int resolution = int.Parse(m.Groups["resolution"].Value);
                           int maxBitRate = int.Parse(m.Groups["bitrate"].Value);
                           int width = int.Parse(m.Groups["width"].Value);
                           int height = int.Parse(m.Groups["height"].Value);

                           msgNote = $"framerate {frameRate} fps, maxbitrate {maxBitRate} bps, {width}x{height} pixels";

                           analysis = "video-set-parameters";
                           _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);

                           // uncompressed frame size = width * height bytes  (1920x1080 = 2073600).  Each pixel has an 8-bit colour value.
                           // transmitted frames/second = bitrate / frame size  (
                        }
                     }
                  }

                  else if (className == "CVolume")
                  {
                     if (methodName == "SetCurrentVolume")
                     {
                        // [0FB26EFC] CVolume::SetCurrentVolume: this=0FDB2844 set volume (63198)

                        string paramString = payLoad.Substring(payLoad.IndexOf("set volume"));
                        regex = new Regex("set volume \\((?<volume>[0-9]*)\\)");
                        m = regex.Match(paramString);
                        success = m.Success;
                        if (success)
                        {
                           double previousSpeakerVolumePercent = (double)_speakerVolume / 65535;

                           int speakerVolume = int.Parse(m.Groups["volume"].Value);
                           double speakerVolumePercent = (double)speakerVolume / 65535 * 100;

                           double alertLow = 20;
                           double alertHigh = 80;

                           msgNote = $"{Math.Round(speakerVolumePercent)}% of max";

                           // range is 0-65535, 0-100%
                           // report initial level, and when volume transitions between low-normal-high

                           if (previousSpeakerVolumePercent >= alertLow && previousSpeakerVolumePercent <= alertHigh)
                           {
                              if (speakerVolumePercent < alertLow)
                              {
                                 analysis = "audio-volume-low";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                              else if (speakerVolumePercent > alertHigh)
                              {
                                 analysis = "audio-volume-high";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }

                           else if (previousSpeakerVolumePercent < alertLow)
                           {
                              if (speakerVolumePercent >= alertLow && speakerVolumePercent < alertHigh)
                              {
                                 analysis = "audio-volume-normal";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                              else if (speakerVolumePercent > alertHigh)
                              {
                                 analysis = "audio-volume-high";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }

                           else
                           {
                              if (speakerVolumePercent >= alertLow && speakerVolumePercent < alertHigh)
                              {
                                 analysis = "audio-volume-normal";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                              else if (speakerVolumePercent < alertLow)
                              {
                                 analysis = "audio-volume-low";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }

                           _speakerVolume = speakerVolume;
                        }
                     }
                  }
               }

               else if (source == "VE_MGR")
               {
                  // RV_BEEHD           11/13/23 04:53:34.953 VE_MGR      : DEBUG - v2oipClientReadLicense pClient=1C330020, verify ver.=99 exact=0, where product version=4.7.24

                  if (payLoad.StartsWith("v2oipClientReadLicense"))
                  {
                     string versionInfo = payLoad.Substring(payLoad.IndexOf("v2oipClientReadLicense") + "v2oipClientReadLicense".Length + 1).ToLower();

                     regex = new Regex("pclient=(?<clientid>[0-9A-Fa-f]{8}), verify ver.=(?<ver>[0-9]*) exact=(?<exact>[0-9]*), where product version=(?<version>.*)");
                     m = regex.Match(versionInfo);
                     success = m.Success;
                     if (success)
                     {
                        _logFileVersion = m.Groups["version"].Value;
                     }
                  }

                  else if (payLoad.StartsWith("License param: CustomerProduct="))
                  {
                     // License param: CustomerProduct=Desktop1

                     _logFileProduct = payLoad.Substring(payLoad.IndexOf("=") + 1);
                  }

                  else if (methodName == "v2oipClientEventsLogAddEvent")
                  {
                     // log version 4.3 - not present in 4.7 logs
                     // VE_MGR                                                 DEBUG   v2oipClientEventsLogAddEvent - added new event to log (4001) with additional info: 		<name>CallStateChanged</name>		<callid>342170984</callid>		<displayName>192.168.5.130</displayName>		<state>DROPPING</state>		<reason>REMOTE DISCONNECT</reason>
                     //                                                                v2oipClientEventsLogAddEvent - added new event to log (2001) with additional info:              <notificationCode>2011</notificationCode>               <severity>Info</severity>               <description>The License Validation Completed Successfully</description>
                     // use this to synthesize some of the missing SIP information

                     if (payLoad.Contains("CallStateChanged"))
                     {
                        try
                        {
                           string xmlData = "<sipdata>" + payLoad.Substring(payLoad.IndexOf("<name>")) + "</sipdata>";
                           StringReader reader = new StringReader(xmlData);
                           DataSet dataSet = new DataSet();

                           dataSet.ReadXml(reader);

                           if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Rows.Count == 1)
                           {
                              DataTable table = dataSet.Tables[0];
                              DataRow row = dataSet.Tables[0].Rows[0];
                              DataColumn col = table.Columns[0];

                              string name = row[table.Columns["Name"].Ordinal].ToString();                // CallStateChanged
                              string callId = row[table.Columns["CallId"].Ordinal].ToString();
                              string displayName = row[table.Columns["DisplayName"].Ordinal].ToString();
                              string state = row[table.Columns["State"].Ordinal].ToString();
                              string reason = row[table.Columns["Reason"].Ordinal].ToString();

                              // the side initiating INVITE
                              // INVITING, IDLE
                              // PROCEEDING, IDLE
                              // CONNECTED, IDLE
                              //
                              // DROPPING, REMOTE DISCONNECT
                              // DISCONNECTED, REMOTE DISCONNECT
                              //
                              // DROPPING, LOCAL DISCONNECT
                              // DISCONNECTED, LOCAL DISCONNECT

                              //
                              // the side responding to INVITE (TBD)

                              _sipCallIdHeader = callId;
                              _sipContactHeader = displayName;
                              _sipCallState = state;

                              switch (state)
                              {
                                 case "INVITING": _sipMessageTypeHeader = "INVITE"; break;
                                 case "PROCEEDING": _sipMessageTypeHeader = "INVITE"; break;
                                 case "CONNECTED": _sipMessageTypeHeader = "OK"; break;
                                 case "DROPPING": _sipMessageTypeHeader = "BYE"; break;
                                 case "DISCONNECTED": _sipMessageTypeHeader = "IDLE"; break;
                                 default: _sipMessageTypeHeader = state; break;
                              }

                              msgNote = reason;
                              protocol = "SIP";


                              // from a 4.7 log:
                              // CALL    CallLegChangeState - Call - leg 0x1CF78A68 state changed: Idle->Inviting, (reason = Local Inviting)
                              // CallLegChangeState - Call - leg 0x1CF78A68 state changed: Idle->Inviting, (reason = Local Inviting)
                              // CallLegChangeState - Call - leg 0x23377BB8 state changed: Idle->Offering, (reason = Remote Inviting)
                              // CallLegChangeState - Call - leg 0x1CF78A68 state changed: Inviting->Proceeding, (reason = Remote Provisional Response)
                              // CallLegChangeState - Call - leg 0x23377BB8 state changed: Offering->Accepted, (reason = Local Accepted)
                              // CallLegChangeState - Call - leg 0x23377BB8 state changed: Accepted->Connected, (reason = Remote Ack)
                              // CallLegChangeState - Call - leg 0x1CF78A68 state changed: Proceeding->RemoteAccepted, (reason = Remote Accepted)
                              // CallLegChangeState - Call - leg 0x1CF78A68 state changed: RemoteAccepted->Connected, (reason = Ack Sent)
                              // CallLegChangeState - Call - leg 0x1CF78A68 state changed: Connected->Disconnected, (reason = Remote Disconnected)
                              // CallLegChangeState - Call - leg 0x23377BB8 state changed: Connected->Disconnecting, (reason = Local Disconnecting)
                              // CallLegChangeState - Call - leg 0x23377BB8 state changed: Disconnecting->Disconnected, (reason = Disconnected)
                              // CallLegChangeState - Call - leg 0x23377BB8 state changed: Disconnected->Terminated, (reason = Call Terminated)
                              // CallLegChangeState - Call - leg 0x1CF78A68 state changed: Disconnected->Terminated, (reason = Call Terminated)
                           }
                        }
                        catch (Exception ex)
                        {
                           // bad XML
                           Console.WriteLine(">>>EXCEPTION BELine constructor bad XML : " + ex.Message);
                           Console.WriteLine($"Line {lineNumber} {logLine}");
                        }
                     }
                  }
               }

               else if (source == "VE_CALL")
               {
                  // SIP message analysis .. _logFileVersion 4.3

                  // no SIP headers are provided, only this for terminal's local side:
                  // VE_CALL     : DEBUG - CVECallSipBase::callSipBuildContactAndFromHeaderStr() - Call 14507ED8 - (2)Prepared From header string: From:"192.168.4.24"<sip:192.168.4.24@192.168.4.24>
                  // TRANSPORT   : DEBUG - RvSockTranspConstruct: 254A30F8: sock=39596 was bound to 192.168.4.24:3270
                  // TRANSPORT   : DEBUG - RvSockTranspConstruct: 254A2978: sock=40356 was bound to 192.168.4.24:3271
                  //
                  // for terminal's remote side:
                  // TRANSPORT   : DEBUG - TransportConnectionInit - initialized a new CLIENT Connection 0x2499A3E0 - local address=192.168.4.24:5060,Remote Address=192.168.5.100:5060
                  // TRANSPORT   : DEBUG - RvSockTranspReceiveBuffer: 254A2978(sock=40356): buff=331CEF54,received=48, addr=192.168.5.100:3271
                  // TRANSPORT   : DEBUG - RvSockTranspReceiveBuffer: 254A2978(sock=40356): buff=331CEF54,received=48, addr=192.168.5.100:3271
                  //
                  // VE_CALL     : DEBUG - callSipHandleSdpAnswer(): Checking remote media: remoteIndex=1, remotePort=3280, remoteMediaDescr=39D9C968, remoteMediaType=1, localIndex=1
                  // VE_CALL     : DEBUG - callSipHandleSdpAnswer(): Checking remote media: remoteIndex=0, remotePort=3270, remoteMediaDescr=39D9D238, remoteMediaType=0, localIndex=0


                  /*
                   * BEEHD              11/13/23 13:46:55.646 VE_CALL     : DEBUG - CVECallSip::callSipHandleIncomingSdp - ******* Incoming SDP: *******:
                     v=0
                     o=192.168.5.100 2503654122 2503654122 IN IP4 192.168.5.100
                     s=-
                     c=IN IP4 192.168.5.100
                     b=AS:768
                     t=0 0
                     m=audio 3270 RTP/AVP 9 101
                     c=IN IP4 192.168.5.100
                     b=AS:64
                     a=ptime:20
                     a=rtpmap:9 G722/8000
                     a=fmtp:9 bitrate=64000
                     a=rtpmap:101 telephone-event/8000
                     a=fmtp:101 0-15
                     m=video 3280 RTP/AVP 102 111 114
                     c=IN IP4 192.168.5.100
                     b=TIAS:768000
                     a=rtcp-fb:* nack pli
                     a=rtcp-fb:* ccm fir
                     a=rtcp-fb:* ccm tmmbr
                     a=rtpmap:102 H264/90000
                     a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270
                     a=rtpmap:111 ulpfec/90000
                     a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8
                     a=rtpmap:114 avcfec/90000
                     a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8
                     a=content:main

                  (OUTGOING)
                     RV_BEEHD           11/13/23 13:46:44.179 VE_CALL     : DEBUG - CVECallSip::callSipCallLegMsgToSendEvHandler - (INVITE) m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:
                     v=0
                     o=192.168.4.24 3509267301 3509267301 IN IP4 192.168.4.24
                     s=-
                     c=IN IP4 192.168.4.24
                     b=AS:768
                     t=0 0
                     m=audio 3270 RTP/AVP 9 103 101
                     b=AS:64
                     a=rtpmap:9 G722/8000
                     a=fmtp:9 bitrate=64000
                     a=rtpmap:103 telephone-event/16000
                     a=fmtp:103 0-15
                     a=rtpmap:101 telephone-event/8000
                     a=fmtp:101 0-15
                     a=ptime:20
                     m=video 3280 RTP/AVP 102 111 114
                     b=TIAS:768000
                     a=content:main
                     a=rtpmap:102 H264/90000
                     a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270
                     a=rtpmap:111 ulpfec/90000
                     a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgo
                     a=rtpmap:114 avcfec/90000
                     a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=0
                     a=rtcp-fb:* nack pli
                     a=rtcp-fb:* ccm tmmbr
                     a=rtcp-fb:* ccm fir
                   */

                  if (_sipMessage != null && !_sipMessage.SdpSectionComplete && className == string.Empty && methodName == string.Empty)
                  {
                     // continue adding the SDP to the message
                     _sipMessage.AppendHeader(payLoad, Timestamp);             // only the SDP will be logged for this message

                     if (_activeSipSession != null)
                     {
                     }
                  }

                  else if (className == "CVECallSip")
                  {
                     if (methodName == "callSipHandleIncomingSdp")
                     {
                        // payload all on oneline
                        // incoming SDP
                        // "CVECallSip::callSipHandleIncomingSdp - ******* Incoming SDP: *******:v=0o=192.168.5.100 2503654122 2503654122 IN IP4 192.168.5.100s=-c=IN IP4 192.168.5.100b=AS:768t=0 0m=audio 3270 RTP/AVP 9 101c=IN IP4 192.168.5.100b=AS:64a=ptime:20a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15m=video 3280 RTP/AVP 102 111 114c=IN IP4 192.168.5.100b=TIAS:768000a=rtcp-fb:* nack plia=rtcp-fb:* ccm fira=rtcp-fb:* ccm tmmbra=rtpmap:102 H264/90000a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270a=rtpmap:111 ulpfec/90000a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8a=rtpmap:114 avcfec/90000a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8a=content:main"
                        // "CVECallSip::callSipHandleIncomingSdp - ******* Incoming SDP: *******:v=0o=192.168.5.130 2658196969 2658196969 IN IP4 192.168.5.130s=-c=IN IP4 192.168.5.130b=AS:768t=0 0m=audio 3270 RTP/AVP 9 101c=IN IP4 192.168.5.130b=AS:64a=ptime:20a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15m=video 3280 RTP/AVP 102 111 114c=IN IP4 192.168.5.130b=TIAS:768000a=rtcp-fb:* nack plia=rtcp-fb:* ccm fira=rtcp-fb:* ccm tmmbra=rtpmap:102 H264/90000a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270a=rtpmap:111 ulpfec/90000a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8a=rtpmap:114 avcfec/90000a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8a=content:main"
                        // "CVECallSip::callSipHandleIncomingSdp - ******* Incoming SDP: *******:v=0o=192.168.5.130 2766252654 2766252654 IN IP4 192.168.5.130s=-c=IN IP4 192.168.5.130b=AS:768t=0 0m=audio 3270 RTP/AVP 9 101c=IN IP4 192.168.5.130b=AS:64a=ptime:20a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15m=video 3280 RTP/AVP 102 111 114c=IN IP4 192.168.5.130b=TIAS:768000a=rtcp-fb:* nack plia=rtcp-fb:* ccm fira=rtcp-fb:* ccm tmmbra=rtpmap:102 H264/90000a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270a=rtpmap:111 ulpfec/90000a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8a=rtpmap:114 avcfec/90000a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8a=content:main"


                        /* ====================================================================================================================================
                         * TODO: update SdpMessage to accumulate log line until the SDP section is complete, THEN parse the individual parameters.  This is likely only
                         *        possible if the content-length is known.  Or if the last line of SDP is always predictable.  Or if (while processing SDP lines) a non-SDP line 
                         *        is encountered to trigger processing of the _sipMessage being collected.
                         * ====================================================================================================================================
                         *  
                           >>>EXCEPTION BHDLine constructor : Processing logline : Index was outside the bounds of the array.
                           3003 RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - v=0o=dakota 3248873059 3248873059 IN IP4 10.174.2.3s=-c=IN IP4 10.174.2.3b=AS:1920t=0 0m=audio 3278 RTP/AVP 9 101c=IN IP4 10.174.2.3b=AS:64a=ptime:20a=rtcp:3279 IN IP4 10.174.2.3a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15m=
                           >>>EXCEPTION BHDLine constructor : Processing logline : Index was outside the bounds of the array.
                           3007 RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - v=0o=dakota 3248873059 3248873059 IN IP4 10.174.2.3s=-c=IN IP4 10.174.2.3b=AS:1920t=0 0m=audio 3278 RTP/AVP 9 101c=IN IP4 10.174.2.3b=AS:64a=ptime:20a=rtcp:3279 IN IP4 10.174.2.3a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15m=

                           Another anomaly - BEEHD log lines butt in!!!

                           a=rtpmap:115 ulpfec/90000
                           a=fmtp:115 protectedPa
                  RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - yload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8
                           a=rtpmap:116 avcfec/90000
                           a=fmtp:116 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8
                           a=content:main

                  RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - CVECallSip::callUpdateIPAddressInSDP(len=6143) pSdpMsg after updating local address:
                  RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - v=0
                           o=dakota 3248873059 3248873059 IN IP4 10.174.2.3
                           s=-
                           c=IN IP4 10.174.2.3
                        */


                        /* payload on multiple lines
                         * CVECallSip::callSipHandleIncomingSdp(len=6143) ******* Incoming SDP: *******:
                           v=0
                           o=10.175.11.74 2519960358 2519960358 IN IP4 10.175.11.74
                           s=-
                           c=IN IP4 10.175.11.74
                           b=AS:1920
                           t=0 0
                           m=audio 3270 RTP/AVP 9 103 101
                           c=IN IP4 10.175.11.74
                           b=AS:64
                           a=ptime:20
                           a=rtcp:3271 IN IP4 10.175.11.74
                           a=rtpmap:9 G722/8000
                           a=fmtp:9 bitrate=64000
                           a=rtpmap:103 telephone-event/16000

                           a=fmtp:103 0-15
                           a=rtpmap:101 telephone-event/8000
                           a=fmtp:101 0-15
                           m=video 3278 RTP/AVP 102 115 116
                           c=IN IP4 10.175.11.74
                           b=TIAS:1920000
                           a=rtcp:3279 IN IP4 10.175.11.74
                           a=rtcp-fb:* nack pli
                           a=rtcp-fb:* ccm fir
                           a=rtcp-fb:* ccm tmmbr
                           a=rtpmap:102 H264/90000
                           a=fmtp:102 profile-level-id=42E01F
                           BEEHD              01/05/24 12:21:30.170 VE_CALL     : DEBUG - ;max-rcmd-nalu-size=1270
                           a=rtpmap:115 ulpfec/90000
                           a=fmtp:115 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8
                           a=rtpmap:116 avcfec/90000
                           a=fmtp:116 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8
                         */

                        if (payLoad.Contains("Incoming SDP:"))
                        {
                           //string sdpString = payLoad.Substring("CVECallSip::callSipHandleIncomingSdp - ******* Incoming SDP: *******:".Length);
                           
                           string sdpString = payLoad.Substring(payLoad.IndexOf("*******:") + 8);

                           if (sdpString == string.Empty)
                           {
                              // multi-line, expect the SDP on following lines.

                              int sipMessageLength = 0;
                              if (payLoad.Contains("(len=") && payLoad.Contains(")"))
                              {
                                 regex = new Regex(".*len=(?<length>[0-9]*).*");
                                 m = regex.Match(payLoad);
                                 success = m.Success;
                                 if (success)
                                 {
                                    sipMessageLength = int.Parse(m.Groups["length"].Value);

                                    if (sipMessageLength > 1600)
                                    {
                                       // typically the largest length is around 1500 bytes.  If 6000+ bytes, WTF is that???
                                    }
                                 }

                                 /*
                                    Content-Type: application/sdp      (MIME)
                                    Content-Length: nnn 
                                 */

                                 // sipMessageLength includes the SDP payload, whose length is not known but typically around 700-800 bytes.
                                 // specify a negative length to indicate the content-length should be calculated as the SDP payload is added
                                 sdpString = "Content-Length: -1";
                              }
                           }

                           if (_sipMessage == null)
                           {
                              // old logs don't have any SIP message, so simulate one so that media packets can be logged
                              isLogSipMessage = true;

                              _sipMessageDirection = "received";

                              // time since the previous SIP message
                              DateTime now = Convert.ToDateTime(Timestamp);
                              DateTime then = now;
                              if (_timeOfLastMessage != string.Empty)
                              {
                                 then = Convert.ToDateTime(_timeOfLastMessage);
                              }

                              TimeSpan interval = now - then;

                              _timeSinceLastMessage = $"{interval}";
                              _timeOfLastMessage = Timestamp;

                              // start a new message, leaving off the <-- or -->
                              _sipMessage = new SipMessage(sdpString, "in", Timestamp);   // nothing will be logged as a SIP message

                              if (sdpString.StartsWith("Content-Length:"))
                              {
                                 _sipMessage.AppendHeader("Content-Type: application/sdp", Timestamp);
                              }

                              _sipMessage.AppendHeader(sdpString, Timestamp);             // only the SDP will be logged for this message

                              _isMultilineSipMessageProcessedFlag = false;

                              _sipMessageProtocol = "SDP";
                              _sipMessageTypeHeader = "INVITE";
                           }

                           else
                           {
                              _sipMessage.AppendHeader(sdpString, Timestamp);             // only the SDP will be logged for this message

                              _isMultilineSipMessageProcessedFlag = false;

                              _sipMessageProtocol = "SDP";
                              _sipMessageTypeHeader = "INVITE";
                           }
                        }
                     }

                     else if (methodName == "callSipCallLegMsgToSendEvHandler")
                     {
                        // outgoing SDP
                        // "CVECallSip::callSipCallLegMsgToSendEvHandler - (INVITE) m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:v=0o=192.168.4.24 3509267301 3509267301 IN IP4 192.168.4.24s=-c=IN IP4 192.168.4.24b=AS:768t=0 0m=audio 3270 RTP/AVP 9 103 101b=AS:64a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:103 telephone-event/16000a=fmtp:103 0-15a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15a=ptime:20m=video 3280 RTP/AVP 102 111 114b=TIAS:768000a=content:maina=rtpmap:102 H264/90000a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270a=rtpmap:111 ulpfec/90000a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgoa=rtpmap:114 avcfec/90000a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=0a=rtcp-fb:* nack plia=rtcp-fb:* ccm tmmbra=rtcp-fb:* ccm fir"
                        // "CVECallSip::callSipCallLegMsgToSendEvHandler - (INVITE) m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:v=0o=192.168.4.24 3251165439 3251165439 IN IP4 192.168.4.24s=-c=IN IP4 192.168.4.24b=AS:768t=0 0m=audio 3270 RTP/AVP 9 103 101b=AS:64a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:103 telephone-event/16000a=fmtp:103 0-15a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15a=ptime:20m=video 3280 RTP/AVP 102 111 114b=TIAS:768000a=content:maina=rtpmap:102 H264/90000a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270a=rtpmap:111 ulpfec/90000a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgoa=rtpmap:114 avcfec/90000a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=0a=rtcp-fb:* nack plia=rtcp-fb:* ccm tmmbra=rtcp-fb:* ccm fir"
                        // "CVECallSip::callSipCallLegMsgToSendEvHandler - (INVITE) m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:v=0o=192.168.4.24 1222395596 1222395596 IN IP4 192.168.4.24s=-c=IN IP4 192.168.4.24b=AS:768t=0 0m=audio 3270 RTP/AVP 9 103 101b=AS:64a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:103 telephone-event/16000a=fmtp:103 0-15a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15a=ptime:20m=video 3280 RTP/AVP 102 111 114b=TIAS:768000a=content:maina=rtpmap:102 H264/90000a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270a=rtpmap:111 ulpfec/90000a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgoa=rtpmap:114 avcfec/90000a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=0a=rtcp-fb:* nack plia=rtcp-fb:* ccm tmmbra=rtcp-fb:* ccm fir"

                        // CVECallSip::callSipCallLegMsgToSendEvHandler - (INVITE) m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:v=0o=192.168.4.24 3251165439 3251165439 IN IP4 192.168.4.24s=-c=IN IP4 192.168.4.24b=AS:768t=0 0m=audio 3270 RTP/AVP 9 103 101b=AS:64a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:103 telephone-event/16000a=fmtp:103 0-15a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15a=ptime:20m=video 3280 RTP/AVP 102 111 114b=TIAS:768000a=content:maina=rtpmap:102 
                        // CVECallSip::callSipCallLegMsgToSendEvHandler(len=6143) (non-INVITE) m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:
                        if (payLoad.Contains("(INVITE)") || payLoad.Contains("(non-INVITE)"))
                        {
                           string sdpString = string.Empty;

                           if (payLoad.Contains("m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:"))
                           {
                              sdpString = payLoad.Substring(payLoad.IndexOf("m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:") + "m_mediaControl.pCurrentLocalMedia before callSipSetSdpInMsgBody:".Length);
                           }

                           if (sdpString == string.Empty)
                           {
                              // multi-line, expect the SDP on following lines.

                              int sdpLength = 0;
                              if (payLoad.Contains("(len=") && payLoad.Contains(")"))
                              {
                                 regex = new Regex(".*len=(?<length>[0-9]*).*");
                                 m = regex.Match(payLoad);
                                 success = m.Success;
                                 if (success)
                                 {
                                    sdpLength = int.Parse(m.Groups["length"].Value);

                                    /*
                                       Content-Type: application/sdp      (MIME)
                                       Content-Length: nnn 
                                    */

                                    sdpString = "Content-Length: " + sdpLength.ToString();
                                 }
                              }
                           }

                           if (_sipMessage == null)
                           {
                              // old logs don't have any SIP message, so simulate one so that media packets can be logged
                              isLogSipMessage = true;

                              _sipMessageDirection = "sent";

                              // time since the previous SIP message
                              DateTime now = Convert.ToDateTime(Timestamp);
                              DateTime then = now;
                              if (_timeOfLastMessage != string.Empty)
                              {
                                 then = Convert.ToDateTime(_timeOfLastMessage);
                              }

                              TimeSpan interval = now - then;

                              _timeSinceLastMessage = $"{interval}";
                              _timeOfLastMessage = Timestamp;

                              // start a new message, leaving off the <-- or -->
                              _sipMessage = new SipMessage(sdpString, "out", Timestamp);  // nothing will be logged as a SIP message

                              if (sdpString.StartsWith("Content-Length:"))
                              {
                                 _sipMessage.AppendHeader("Content-Type: application/sdp", Timestamp);
                              }

                              _sipMessage.AppendHeader(sdpString, Timestamp);             // only the SDP will be logged for this message

                              _isMultilineSipMessageProcessedFlag = false;

                              _sipMessageProtocol = "SDP";
                              _sipMessageTypeHeader = "INVITE";
                           }

                           else
                           {
                              _sipMessage.AppendHeader(sdpString, Timestamp);             // only the SDP will be logged for this message

                              _isMultilineSipMessageProcessedFlag = false;

                              _sipMessageProtocol = "SDP";
                              _sipMessageTypeHeader = "INVITE";
                           }
                        }
                     }

                     else if (methodName == "callUpdateIPAddressInSDP")
                     {
                        /*
                         * CVECallSip::callUpdateIPAddressInSDP(len=6143) pSdpMsg after updating local address:
                         */
                     }
                  }

                  else if (className == "VECall")
                  {
                     if (methodName == "staticMediaStreamResolutionChangeEvHandler")
                     {
                        // VE_CALL CVECall::staticMediaStreamResolutionChangeEvHandler - Call 0FCA5DB8 - Handle resolution change, pStream(0FCEB218), layoutElem(1), newWidth(800), newHeight(600)
                        string paramString = payLoad.Substring(payLoad.IndexOf("newWidth"));
                        regex = new Regex("newWidth\\((?<width>[0-9]*)\\), newHeight\\((?<height>[0-9]*)\\)");
                        m = regex.Match(paramString);
                        success = m.Success;
                        if (success)
                        {
                           int width = int.Parse(m.Groups["width"].Value);
                           int height = int.Parse(m.Groups["height"].Value);

                           msgNote = $"{width}x{height} pixels";

                           analysis = "video-resolution-changed";
                           _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                        }
                     }
                  }
               }

               else if (source == "MEDIA")
               {
                  // RTP media states
                  // [1527E13C] CMediaStream::ChangeState: VIDEO Transmitter stream - change state DeactivateRtpAlive to Active
                  // [38F88214] CMediaStream::ChangeState: AUDIO Transmitter stream - change state Initiated to Active
                  // [38F88214] CMediaStream::ChangeState: VIDEO Transmitter stream - change state Active to DeactivateRtpAlive

                  // [2273C1BC] CMediaStream::ChangeState: AUDIO Receiver stream - change state Initiated to Active
                  // [2273C1BC] CMediaStream::ChangeState: VIDEO Receiver stream - change state Initiated to Active

                  // [1527E13C] CMediaStream::_SetParamsInVideoTransmitterChan: Stream 14D967A4 - Transmitter channel remote address is 192.168.57.239:3272

                  // [1527E13C] CMediaEngine::CreateSession: #########################     NEW MEDIA SESSION 1B284FCC  (mediaEngine 1527E13C)  #########################
                  // [2AF462DC] ~CMediaEngine: Engine was destroyed (rv=0)

                  if (className == "CMediaStream")
                  {
                     if (methodName == "ChangeState")
                     {
                        string streamInfo = payLoad.Substring(payLoad.IndexOf("ChangeState: ") + 13).ToLower();

                        // VIDEO Transmitter stream - change state DeactivateRtpAlive to Active
                        // AUDIO Receiver stream - change state Initiated to Active
                        regex = new Regex("(?<stream>.*) (?<type>.*) stream - change state (?<from>.*) to (?<to>.*)");
                        m = regex.Match(streamInfo);
                        success = m.Success;
                        if (success)
                        {
                           if (m.Groups["type"].Value == "transmitter")
                           {
                              if (m.Groups["stream"].Value == "audio")
                              {
                                 _audioTransmitterState = m.Groups["to"].Value;
                              }
                              else if (m.Groups["stream"].Value == "video")
                              {
                                 _videoTransmitterState = m.Groups["to"].Value;
                              }
                           }
                           else if (m.Groups["type"].Value == "receiver")
                           {
                              if (m.Groups["stream"].Value == "audio")
                              {
                                 _audioReceiverState = m.Groups["to"].Value;
                              }
                              else if (m.Groups["stream"].Value == "video")
                              {
                                 _videoReceiverState = m.Groups["to"].Value;
                              }
                           }

                        }
                     }
                  }

                  else if (methodName == "~CMediaEngine")
                  {
                     // the penultimate end of a call

                     // [2AF462DC] ~CMediaEngine: Engine was destroyed (rv=0)

                     if (payLoad.Contains("Engine was destroyed"))
                     {
                        if (_activeSipSession != null)
                        {
                           _sipSessions.Add(new SipSession.SipSession());      // new placeholder session which lacks a call-id
                           _activeSipSession = _sipSessions[_sipSessions.Count-1];
                        }
                     }
                  }
               }

               else if (source == "TRANSPORT")
               {
                  // incoming and outgoing UDP traffic available at DEBUG level ..
                  if (payLoad.Contains("Sending UDP message"))
                  {
                     protocol = "UDP";
                     direction = "sent";
                     msgNote = payLoad;
                  }
                  else if (payLoad.Contains("SipTransportUdpSendMessage"))
                  {
                     protocol = "UDP";
                     direction = "sent";
                     msgNote = payLoad;
                  }
                  else if (payLoad.Contains("Recv new UDP message"))
                  {
                     protocol = "UDP";
                     direction = "received";
                     msgNote = payLoad;
                  }
                  else if (payLoad.Contains("TransportUdpEventCallback"))
                  {
                     protocol = "UDP";
                     direction = "received";
                     msgNote = payLoad;
                  }
                  else if (payLoad.Contains("Sending TCP message"))
                  {
                     protocol = "TCP";
                     direction = "sent";
                     msgNote = payLoad;
                  }
                  else if (payLoad.Contains("Recv new TCP message"))
                  {
                     protocol = "TCP";
                     direction = "received";
                     msgNote = payLoad;
                  }

                  #region SIP ANALYSIS
                  else
                  {
                     // SIP message analysis .. _logFileVersion 4.7

                     // SIP and SDP recognizer
                     // <-- SIP/2.0 sip header                 or --> sip header SIP/2.0
                     //     sip header
                     //     CSeq: n header
                     //     sip header
                     //     Content-Type: application/sdp      (MIME)
                     //     Content-Length: nnn                (MIME)
                     //     (empty line)
                     //     sdp header
                     //     sdp header
                     // xxx

                     if ((logLine.Contains("<-- ") && logLine.EndsWith(" SIP/2.0")) ||
                         (logLine.Contains("--> ") && logLine.EndsWith(" SIP/2.0")) ||
                         logLine.Contains("--> SIP/2.0") ||
                         logLine.Contains("<-- SIP/2.0"))
                     {
                        isLogSipMessage = true;

                        _sipMessageDirection = logLine.Contains("<-- ") ? "received" : "sent";

                        // time since the previous SIP message
                        DateTime now = Convert.ToDateTime(Timestamp);
                        DateTime then = now;
                        if (_timeOfLastMessage != string.Empty)
                        {
                           then = Convert.ToDateTime(_timeOfLastMessage);
                        }

                        TimeSpan interval = now - then;

                        _timeSinceLastMessage = $"{interval}";
                        _timeOfLastMessage = Timestamp;

                        if (_sipMessage != null)
                        {
                           throw new Exception($"starting a new SIP message but the previous one is not finished yet.");
                        }

                        // start a new message, leaving off the <-- or -->
                        _sipMessage = new SipMessage(payLoad.Substring(4), _sipMessageDirection, Timestamp);
                        _isMultilineSipMessageProcessedFlag = false;

                        _sipMessageProtocol = _sipMessage.LastHeaderType;
                        _sipMessageTypeHeader = _sipMessage.MessageType;

                        /*
                        // normally call state is for a session, but as this message has only just begun
                        // arriving we need to set call state early if the message is an INVITE
                        // so that log lines can be explored at the outset of a call

                        if (_sipCallState == string.Empty && _sipMessage.MessageType == "INVITE")
                        {
                           // _sipCallState = "Invite";

                           if (_sipMessage.CallId != _sipCallIdHeader)
                           {
                              // fresh INVITE should not be tagged with an old _sipCallIdHeader value
                              // if the call id is not known yet, it will be empty string

                              _sipCallIdHeader = _sipMessage.CallId;
                           }
                        }
                        */
                     }

                     else if (logLine.Contains(" -    ") && _sipMessageProtocol != string.Empty)
                     {
                        isLogSipMessage = true;

                        if (_sipMessage != null)
                        {
                        _sipMessage.AppendHeader(payLoad, Timestamp);

                        if (payLoad.Contains("fmtp:116"))
                        {
                           // last line of an INVITE
                        }

                        _sipMessageProtocol = _sipMessage.LastHeaderType;
                        _sipCSeqHeader = (_sipMessage.Cseq > 0) ? _sipMessage.Cseq.ToString() : string.Empty;
                        _sipCallIdHeader = _sipMessage.CallId;
                        _sipTransactionIdHeader = _sipMessage.TransactionId;
                        _sipContactHeader = _sipMessage.Contact;
                        _sipSessionOriginatorHeader = _sipMessage.SessionOriginator;
                        _sipRportOption = _sipMessage.RportOption;
                        _sipAudioState = _sipMessage.AudioState;
                        _sipVideoState = _sipMessage.VideoState;

                        int sessionNumber = _sipSessions.Count;
                        if (sessionNumber > 0)
                        {
                           // get call state from the active session (the most recently added one)
                           // _sipCallState = _sipSessions[sessionNumber - 1].State.ToString();
                        }
                        else
                        {
                           // carry over the callstate from previous logline
                        }
                     }

                        else
                        {
                           // normally it would be a bug, that a SIP-type message line is received but there is no
                           // _sipMessage to store it into.  But in the case of an OPTIONS message, sometimes there is
                           // an extra line after the SIP message body has deemed to have been finished by SdpMessage due
                           // to Content-Length: 0, and already finalized below (where _sipMessage is set to null).

                           if (logLine.Contains("User-Agent: ") && _sipMessageTypeHeader == "OPTIONS")
                           {
                              // INFO  -     Accept: application/sdp
                              // INFO  -     Content-Length: 0
                              // INFO  -     User-Agent: ${jndi:ldap://localhost#Greenbone-VA-2:13532/a}

                              // TODO - ignore it for now, but maybe later try to locate the previous _sipMessage object
                              //        and .AppendHeader() to include this line
                           }
                        }
                     }
                  }
                  #endregion SIP ANALYSIS

                  if (isLogSipMessage)
                  {
                     protocol = _sipMessageProtocol;
                     direction = _sipMessageDirection;
                     msgHeader = _sipMessageTypeHeader;
                     callIdHeader = _sipCallIdHeader;
                     cseqHeader = _sipCSeqHeader;
                     transactionId = _sipTransactionIdHeader;
                     contact = _sipContactHeader;
                     sessionOriginator = _sipSessionOriginatorHeader;
                     rportOption = _sipRportOption;
                     audioState = _sipAudioState;
                     videoState = _sipVideoState;
                  }
               }

               // common labels for all lines during a call ..
               callIdHeader = _sipCallIdHeader;
               audioState = _sipAudioState;
               videoState = _sipVideoState;
               microphoneState = _microphoneState;
               speakerState = _speakerState;
               cameraState = _cameraState;
               audioTransmitterState = _audioTransmitterState;
               audioReceiverState = _audioReceiverState;
               videoTransmitterState = _videoTransmitterState;
               videoReceiverState = _videoReceiverState;


               // analysis based on class and method name ..
               if (!isLogSipMessage && className != string.Empty || methodName != string.Empty)
               {
                  // granular audio-video analysis
                  switch (source)
                  {
                     case "CH_MAN":

                        if (className == "CWINVideoInputDevice")
                        {
                           // CH_MAN, level=EXCEP
                           // [12FA4A04] CWINVideoInputDevice::CopyDeviceFrame: frame size 4147200 is bigger than allocated buffers. Res=<1920,1080>, format=9. Going to crop the data
                           /*
>>>EXCEPTION BHDLine constructor : Processing logline : SipSession.RecordStreamEvent exception
10330 94 CAudioReceiverN 01/16/24 15:55:52.798 CH_MAN      : INFO  - [0B22E9AC] CVolume::SetCurrentVolume: this=128D9A44 set volume (65282)
                            */

                           if (methodName == "CopyDeviceFrame")
                           {
                              string paramString = payLoad.Substring(payLoad.IndexOf("frame size ") + 11);
                              regex = new Regex("(?<size>[0-9]*) .* Res=<(?<width>[0-9]*),(?<height>[0-9]*)>,");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 long frameSize = int.Parse(m.Groups["size"].Value);
                                 int width = int.Parse(m.Groups["width"].Value);
                                 int height = int.Parse(m.Groups["height"].Value);

                                 long maxExpected = width * height;

                                 msgNote = $"size {frameSize}, max {maxExpected} bytes";

                                 analysis = "video-frame-cropped";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CVideoDirect3DDevice")
                        {
                           if (methodName == "RawConvertFormatAndBlit_YUY2")
                           {
                              // ERROR
                              // CVideoDirect3DDevice::RawConvertFormatAndBlit_YUY2: Error while locking yuy2 surface.(80004005=UNKNOWN ERROR)

                              msgNote = "Direct3D screen update error";

                              analysis = "display-update-error";
                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }
                        }

                        break;

                     // Audio Receiver
                     case "AUDIO_RE":
                        // CAudioReceiverChannel	RtpMediaReceivedCb
                        // CAudioDecoderNode	ActivateCodec
                        // CCompressedVideoAggFrag::HandlePacketLoss: expected = 13822 received = 13823
                        // CMainAudioReceiverChannel	StopChannelMediaExport
                        // CAudioReceiverNode NodeStop

                        // 166929DC] CAudioDecoder:Reset: -->
                        // 166929DC] CAudioDecoderNode::ActivateCodec: codec G722 (bitrate 64000) was started successfully
                        // 166929DC] CAudioDecoderNode::HandlePacketLoss: m_lastSeqNum =29961 pRtpTag->ui16SequenceNumber=29963 packetsToConceal=1
                        // 166929DC] CAudioDecoderNode::ProcessBuffer: HandlePacketLoss done for one packet m_lastSeqNum=29962 m_plcCount=0 m_LastPackType=3
                        // 166929DC] CAudioDecoderNode::ProcessBuffer: m_LastPackType = 3 m_plcCount=0, m_cngCount=0 FillCount 0 JitterFillinMS=0 m_lastSeqNum=29955
                        // 2321AFE4] CAudioInformation::GetAudioCodecSpecificValues: Calculated params: encodedFrameSize=80 decodedFrameSize=320 frameDuration=10 sampleRate=16000

                        // 2321AFE4] CRTPSourceHandler::SetMaxPacketSize: new packetSize=1500

                        if (className == "CRTPSessionHandler")
                        {
                           if (methodName == "SetRemoteNetworkRtcpParameters")
                           {
                              // [38F88214] CRTPSessionHandler::SetRemoteNetworkRtcpParameters: 39860FEC Set RTCP Remote Address 10.206.20.47:3279. bDisableRtcp=1

                              analysis = "audio-receiver-local-controlport";
                           }

                           else if (methodName == "Initialize")
                           {
                              // [38F88214] CRTPSessionHandler::Initialize: 39860FEC - RTP listening port:3278

                              analysis = "audio-receiver-local-mediaport";
                           }
                        }

                        else if (className == "CAudioReceiverChannel")
                        {
                           if (methodName == "RtpMediaReceivedCb")
                           {
                              if (payLoad.Contains("Start processing audio"))
                              {
                                 // [2AF462DC] CAudioReceiverChannel::RtpMediaReceivedCb: Found Codec G722. Start processing audio media!!!

                                 analysis = "audio-receiver-startprocessing";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                              else if (payLoad.Contains("Set Codec"))
                              {
                                 // [2AF462DC] CAudioReceiverChannel::RtpMediaReceivedCb: Set Codec G722 (bitrate = 64000) to all nodes,m_decodedFrameSize=320

                                 analysis = "audio-receiver-codec";

                                 string paramString = payLoad.Substring(payLoad.IndexOf("Set Codec") + 10);
                                 regex = new Regex("(?<codec>\\w*) \\(bitrate = (?<bitrate>[0-9]*)\\) to all nodes,m_decodedFrameSize=(?<decodedframesize>[0-9]*)");
                                 m = regex.Match(paramString);
                                 success = m.Success;
                                 if (success)
                                 {
                                    string codecName = m.Groups["codec"].Value;
                                    int bitRate = int.Parse(m.Groups["bitrate"].Value);
                                    int decodedFrameSize = int.Parse(m.Groups["decodedframesize"].Value);

                                    msgNote = $"{codecName}, bitrate {bitRate} bps, framesize {decodedFrameSize} bytes";

                                    _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                                 }
                              }
                           }
                        }

                        else if (className == "CAudioDecoderNode")
                        {
                           if (methodName == "HandlePacketLoss")
                           {
                              // 166929DC] CAudioDecoderNode::HandlePacketLoss: m_lastSeqNum =29961 pRtpTag->ui16SequenceNumber=29963 packetsToConceal=1

                              protocol = "RTP-loss";

                              // see also CAudioDecoderNode::ProcessBuffer
                              analysis = "audio-receiver-quality";

                              string paramString = payLoad.Substring(payLoad.IndexOf("m_lastSeqNum") + 14);

                              regex = new Regex("(?<lastseqnum>[0-9]*) .* packetsToConceal=(?<packetslost>[0-9.]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 long lastSeqNumber = long.Parse(m.Groups["lastseqnum"].Value);
                                 long lostPackets = Convert.ToInt64(m.Groups["packetslost"].Value);

                                 msgNote = $"Audio QOS - lost {lostPackets}, LastSeq# {lastSeqNumber}";

                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                                 _activeSipSession?.RecordOtherPacketStatistics(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), lostPackets, 0, 0);
                              }
                           }

                           /* same information already covered by CAudioDecoderNode::HandlePacketLoss
                            * 
                           else if (methodName == "ProcessBuffer")
                           {
                              protocol = "RTP";
                              analysis = "audio-report";

                              // CAudioDecoderNode::ProcessBuffer: m_LastPackType = 3 m_plcCount=0, m_cngCount=0 FillCount 0 JitterFillinMS=0 m_lastSeqNum=29955

                              string paramString = payLoad.Substring(payLoad.IndexOf("FillCount"));

                              regex = new Regex("FillCount (?<fillcount>[0-9]*) JitterFillinMS=(?<jitter>[0-9]*) m_lastSeqNum=(?<lastseqnum>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 long lastSeqNumber = long.Parse(m.Groups["lastseqnum"].Value);
                                 int fillCount = int.Parse(m.Groups["fillcount"].Value);
                                 int jitterMs = int.Parse(m.Groups["jitter"].Value);

                                 analysis += $"-seq-{lastSeqNumber}-lost-{fillCount}-jitter-{jitterMs}msec";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, 1);
                              }
                           }
                           */
                        }

                        else if (className == "CAudioInformation")
                        {
                           if (methodName == "GetAudioCodecSpecificValues")
                           {
                              // 2321AFE4] CAudioInformation::GetAudioCodecSpecificValues: Calculated params: encodedFrameSize=80 decodedFrameSize=320 frameDuration=10 sampleRate=16000
                              string paramString = payLoad.Substring(payLoad.IndexOf("Calculated params: ") + 19);
                              regex = new Regex("encodedFrameSize=(?<encodedframesize>[0-9]*) decodedFrameSize=(?<decodedframesize>[0-9.]*) frameDuration=(?<frameduration>[0-9.]*) sampleRate=(?<samplerate>[0-9.]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int encodedFrameSize = int.Parse(m.Groups["encodedframesize"].Value);
                                 int decodedFrameSize = int.Parse(m.Groups["decodedframesize"].Value);
                                 int frameDuration = int.Parse(m.Groups["frameduration"].Value);
                                 int sampleRate = int.Parse(m.Groups["samplerate"].Value);

                                 double expectedFramesPerSec = 1000 / frameDuration;

                                 analysis = "audio-codec-frame";
                                 msgNote = $"size {decodedFrameSize} bytes, {frameDuration} msec, samplerate {sampleRate} bps";

                                 // don't need to log this event
                                 //_activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CRTPSourceHandler")
                        {
                           if (methodName == "SetMaxPacketSize")
                           {
                              // [2321AFE4] CRTPSourceHandler::SetMaxPacketSize: new packetSize= 1500

                              string paramString = payLoad.Substring(payLoad.IndexOf("new") + 4);
                              regex = new Regex("packetSize=(?<maxpacketsize>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int maxPacketSize = int.Parse(m.Groups["maxpacketsize"].Value);

                                 analysis = "audio-max-packetsize";
                                 msgNote = $"{maxPacketSize} bytes";

                                 // don't need to log this event
                                 // _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        break;

                     // Audio Transmitter
                     case "AUDIO_TR":
                        if (className == "CRTPSessionHandler")
                        {
                           if (methodName == "SetRemoteNetworkRtcpParameters")
                           {
                              // [38F88214] CRTPSessionHandler::SetRemoteNetworkRtcpParameters: 39860FEC Set RTCP Remote Address 10.206.20.47:3279. bDisableRtcp=1

                              analysis = "audio-transmitter-remote-controlport";
                           }

                           else if (methodName == "Initialize")
                           {
                              // [38F88214] CRTPSessionHandler::Initialize: 39860FEC - RTP listening port:3278

                              analysis = "audio-transmitter-remote-mediaport";
                           }
                        }

                        else if (className == "CRTPTransmitterNode")
                        {
                           // [38F88214] CRTPTransmitterNode::NodeStart: node 2F5A8504 was started

                           analysis = string.Empty;

                           switch (methodName)
                           {
                              //case "NodeInit": analysis = "video-transmitter-init"; break;
                              case "NodeStart": analysis = "video-transmitter-start"; break;
                              case "NodePause": analysis = "video-transmitter-pause"; break;
                              case "NodeStop": analysis = "video-transmitter-stop"; break;
                              //case "NodeTerminate": analysis = "video-transmitter-terminate"; break;
                              //default: analysis += methodName; break;
                           }

                           if (analysis != string.Empty)
                           {
                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }
                        }

                        else if (className == "CAudioTransmitterChannel")
                        {
                           if (methodName == "SetCodecParameters")
                           {
                              // [2321AFE4] CAudioTransmitterChannel::SetCodecParameters: G722, bitrate=64000
                              string paramString = payLoad;
                              regex = new Regex(".* bitrate=(?<bitrate>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 long bitRate = long.Parse(m.Groups["bitrate"].Value);

                                 analysis = "audio-transmitter-set-codec-bitrate";
                                 msgNote = $"{bitRate} bps";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CAudioInformation")
                        {
                           if (methodName == "GetAudioCodecSpecificValues")
                           {
                              // [2321AFE4] CAudioInformation::GetAudioCodecSpecificValues: Calculated params: encodedFrameSize=80 decodedFrameSize=320 frameDuration=10 sampleRate=16000

                              string paramString = payLoad.Substring(payLoad.IndexOf("Calculated params: ") + 19);
                              regex = new Regex("encodedFrameSize=(?<encodedframesize>[0-9]*) decodedFrameSize=(?<decodedframesize>[0-9.]*) frameDuration=(?<frameduration>[0-9.]*) sampleRate=(?<samplerate>[0-9.]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int encodedFrameSize = int.Parse(m.Groups["encodedframesize"].Value);
                                 int decodedFrameSize = int.Parse(m.Groups["decodedframesize"].Value);
                                 int frameDuration = int.Parse(m.Groups["frameduration"].Value);
                                 int sampleRate = int.Parse(m.Groups["samplerate"].Value);

                                 analysis = "audio-get-codec-parameters";
                                 msgNote = $"frame {decodedFrameSize} bytes, duration {frameDuration} msec, samplerate {sampleRate} bps";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CAudioReceiverNode")
                        {
                           // yes, this is in AUDIO_TR not AUDIO_RE !!

                           // [38F88214] CAudioReceiverNode::NodeStart: Mic node 396B2A5C was Start!!!
                           // [38F88214] CAudioReceiverNode::NodeStop: Mic node thread  396B2A5C was stopped RetVal=0

                           // [2AF462DC] CRTPReceiverNode::NodeStop: stopping node 2383C904 ...

                           analysis = string.Empty;

                           switch (methodName)
                           {
                              //case "NodeInit": analysis = "audio-receiver-init"; break;
                              case "NodeStart": analysis = "audio-receiver-start"; break;
                              case "NodePause": analysis = "audio-receiver-pause"; break;
                              case "NodeStop": analysis = "audio-receiver-stop"; break;
                                 //case "NodeTerminate": analysis = "audio-receiver-terminate"; break;
                                 //default: analysis += methodName; break;
                           }

                           if (analysis != string.Empty)
                           {
                              // in AUDIO_TR but the log is referring to AUDIO_RE?
                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }
                        }

                        break;

                     // Video Receiver
                     case "VIDEO_RE":
                        if (className == "CCompressedVideoAggFrag")
                        {
                           if (methodName == "HandlePacketLoss")
                           {
                              protocol = "RTP-loss";

                              string paramString = payLoad.Substring(payLoad.IndexOf(",buffLen") + 1);
                              regex = new Regex("expected ?= ?(?<lownum>[0-9]*) ?received ?= ?(?<hinum>[0-9.]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 videoLost = int.Parse(m.Groups["hinum"].Value) - int.Parse(m.Groups["lownum"].Value);

                                 if (videoLost > 0)
                                 {
                                    protocol = "RTP";

                                    analysis = "video-quality";
                                    msgNote = $"Video QOS - lost {videoLost} packets, LastSeq# {int.Parse(m.Groups["hinum"].Value)}";

                                    // TODO - how to determine # of good video packets received?  Without having to keep track of 'hinum' values?
                                    //      - it seems like only Video QOS with 'lost' is reported containing the 'hinum', so if there are no lost video frames then won't get hinum reports either??

                                    long videoGood = 0;      // see videoReceived
                                    int videoJitterMS = 0;   // see videoJitter

                                    _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                                    _activeSipSession?.RecordOtherPacketStatistics(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), videoLost, videoGood, videoJitterMS);
                                 }
                                 else
                                 {
                                    analysis = "video-quality-ok";
                                    _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                                 }

                                 /* 
                                    // is there a source of video jitter?
                                    public int videoJitter = 0;
                                 */
                              }
                           }
                        }

                        else if (className == "CRTPSessionHandler")
                        {
                           if (methodName == "Initialize")
                           {
                              // [2AF462DC] CRTPSessionHandler::Initialize: 12BBB20C - RTCP listening port:3281

                              analysis = "video-receiver-initialize";
                           }
                        }

                        else if (className == "CRTPReceiverNode")
                        {
                           // [2AF462DC] CRTPReceiverNode::NodeStop: stopping node 2BEBEE0C ...

                           analysis = string.Empty;
                           switch (methodName)
                           {
                              //case "NodeInit": analysis = "video-receiver-init"; break;
                              case "NodeStart": analysis = "video-receiver-start"; break;
                              case "NodePause": analysis = "video-receiver-pause"; break;
                              case "NodeStop": analysis = "video-receiver-stop"; break;
                                 //case "NodeTerminate": analysis = "video-receiver-terminate"; break;
                           }

                           if (analysis != string.Empty)
                           {
                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }
                        }

                        else if (className == "CVideoReceiverChannel")
                        {
                           if (methodName == "SetMTUSize")
                           {
                              // [2321AFE4] CVideoReceiverChannel::SetMTUSize: Channel 22C407BC - Set MTU size 1360

                              string paramString = payLoad.Substring(payLoad.IndexOf("Set MTU size ") + 13);
                              regex = new Regex("(?<mtusize>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int mtuSize = int.Parse(m.Groups["mtusize"].Value);

                                 analysis = "video-set-MTU";
                                 msgNote = $"{mtuSize} bytes";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CRTPSourceHandler")
                        {
                           if (methodName == "SetMaxPacketSize")
                           {
                              // [2321AFE4] CRTPSourceHandler::SetMaxPacketSize: new packetSize=1905

                              string paramString = payLoad.Substring(payLoad.IndexOf("new ") + 4);
                              regex = new Regex("packetSize=(?<packetsize>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int packetSize = int.Parse(m.Groups["packetsize"].Value);

                                 analysis = "video-set-packetsize";
                                 msgNote = $"{packetSize} bytes";

                                 // don't need to log this event
                                 // _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CVidDecWrapper")
                        {
                           // VIDEO_RE, level=WARN
                           // [2B2FE894] CVidDecWrapper::ProcessFrame: DecodeFrame ret=20 m_bIsOutOfSyncRecognitionEn=1

                           if (methodName == "ProcessFrame" && payLoad.Contains("m_bIsOutOfSyncRecognitionEn"))
                           {
                              protocol = "RTP";

                              analysis = "video-receive-frame-sync-warning";
                              msgNote = "Video QOS - frame lost";

                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }

                           else if (methodName == "ProcessFrame")
                           {
                              analysis = "video-receive-frame-process";
                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }
                        }

                        else if (className == "CVidDecInFrameHandler")
                        {
                           // NOTE: if RvSockTranspReceiveBuffer lines containing packet counts are not available, these might be the only indication of video packet or frame loss

                           // ERROR
                           // [2B2FE894] CVidDecInFrameHandler::IsLossInsideCurrentFrame:  IsPacketLostInFrameMiddle m_ui16AssembledFramesRunningIndex=2105
                           // [2BA7620C] CVidDecInFrameHandler::FillAssembledFrameMediaTag:  IsPotentialFullFrameLossBeforeCurrentFrame m_ui16AssembledFramesRunningIndex=411

                           if (methodName == "IsLossInsideCurrentFrame")
                           {
                              protocol = "RTP";

                              msgNote = "Video QOS - internal frame lost";
                              analysis = "video-receiver-quality";    // internal to a frame

                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }

                           else if (methodName == "FillAssembledFrameMediaTag")
                           {
                              protocol = "RTP";

                              msgNote = "Video QOS - previous frame lost";
                              analysis = "video-receiver-quality";     // previous frame lost

                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }
                        }

                        else if (className == "CQoSVideoRcvController")
                        {
                           // [2273C1BC] CQoSVideoRcvController::IncomingRtpRtcpStarted: ============== Incoming Media started! (bRtcp=0) ================  (terminal)
                           // [2B2FE894] CQoSVideoRcvController::IncomingRtpRtcpStarted: ============== Incoming Media started! (bRtcp=1) ================  (teller)
                           // [2AF462DC] CQoSVideoRcvController::SendIntraRequest: activating PLI...

                           // Picture Loss Indication
                           if (methodName == "SendIntraRequest" && payLoad.Contains("activating PLI"))
                           {
                              analysis = "video-qos-activate-PLI";
                           }
                        }

                        else if (className == "CQosNetSenseControl")
                        {
                           if (methodName == "SetBandwidth")
                           {
                              // [2321AFE4] CQosNetSenseControl::SetBandwidth: Update the net-sense of max-bitrate 1856000 (previous is 0)

                              string paramString = payLoad.Substring(payLoad.IndexOf("max-bitrate ") + 12);
                              regex = new Regex("(?<newbitrate>[0-9]*) \\(previous is (?<prevbitrate>[0-9]*)\\)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int newBitRate = int.Parse(m.Groups["newbitrate"].Value);
                                 int prevBitRate = int.Parse(m.Groups["prevbitrate"].Value);

                                 analysis = "video-set-bitrate";
                                 msgNote = $"{newBitRate} bps";

                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CRTCPHandler")
                        {
                           // related to frame loss handling - PLI Picture Loss Indication (requests an I-Frame)

                           // for a gray-screen ticket, teller reported gray screen from the ATM side while ATM was receiving
                           //  video normally

                           // VIDEO_RE and VIDEO_TR  both report this
                           // [38F88214] CRTCPHandler::RtcpFbPLIHandler: <--RTCP_FB: PLI received

                           if (methodName == "RtcpFbPLIHandler")
                           {
                              // received a request for an I-Frame
                              analysis = "video-qos-PLI-IFrame-requested";
                           }

                           else if (methodName == "SendFbPLI")
                           {
                              // [2AF462DC] CRTCPHandler::SendFbPLI: -->RTCP_FB: PLI request

                              analysis = "video-qos-PLI-IFrame-request-acknowledged";
                           }

                           else if (methodName == "SendIntraRequest" && payLoad.Contains("activating PLI"))
                           {
                              // VIDEO_RE, INFO
                              // [2AF462DC] CQoSVideoRcvController::SendIntraRequest: activating PLI...

                              analysis = "video-qos-PLI-activation-requested";
                           }
                        }

                        break;

                     // Video Transmitter
                     case "VIDEO_TR":
                        if (className == "CVideoDisplayTransmitterNode")
                        {
                           // [38F88214] CRTPTransmitterNode::NodeStart: node 2F5A8504 was started

                           analysis = string.Empty;
                           switch (methodName)
                           {
                              //case "NodeInit": analysis = "video-transmitter-init"; break;
                              case "NodeStart": analysis = "video-transmitter-start"; break;
                              case "NodePause": analysis = "video-transmitter-pause"; break;
                              case "NodeStop": analysis = "video-transmitter-stop"; break;
                                 //case "NodeTerminate": analysis = "video-transmitter-terminate"; break;
                           }

                           if (analysis != string.Empty)
                           {
                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }
                        }

                        else if (className == "CRTCPHandler")
                        {
                           // is this related to frame loss detected by the remote party?

                           // VIDEO_RE and VIDEO_TR  both report this
                           // [38F88214] CRTCPHandler::RtcpFbPLIHandler: <--RTCP_FB: PLI received

                           if (methodName == "RtcpFbPLIHandler" && payLoad.Contains("<--RTCP_FB: PLI received"))
                           {
                              analysis = "video-Fb_PLI-reported";
                           }
                        }

                        else if (className == "CVideoTransmitterChannel")
                        {
                           // [2321AFE4] CVideoTransmitterChannel::SetMTUSize: Channel 3F9B26BC - Set MTU size 1360

                           if (payLoad.Contains("Set MTU size"))
                           {
                              string paramString = payLoad.Substring(payLoad.IndexOf("Set MTU size ") + 13);
                              regex = new Regex("(?<mtusize>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int mtuSize = int.Parse(m.Groups["mtusize"].Value);

                                 analysis = "video-set-MTU";
                                 msgNote = $"{mtuSize} bytes";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }

                           else if (methodName == "UpdateCameraResolution")
                           {
                              // [2321AFE4] CVideoTransmitterChannel::UpdateCameraResolution: frameRateToUse=15, resolution=7, bitrateToUse=1920000, bLandscapeEn=1, bPortraitEn=1 scaled 640x480

                              string paramString = payLoad.Substring(payLoad.IndexOf("frameRateToUse=") + 15);
                              regex = new Regex("(?<framerate>[0-9]*), resolution=(?<resolution>[0-9]*), bitrateToUse=(?<bitrate>[0-9]*), bLandscapeEn=(?<landscapeen>[0-9]*), bPortraitEn=(?<portraiten>[0-9]*) (?<scaled>\\w*) (?<width>[0-9]*)x(?<height>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int frameRate = int.Parse(m.Groups["framerate"].Value);
                                 int useBitRate = int.Parse(m.Groups["bitrate"].Value);
                                 int width = int.Parse(m.Groups["width"].Value);
                                 int height = int.Parse(m.Groups["height"].Value);

                                 analysis = "video-set-resolution";
                                 msgNote = $"framerate {frameRate} fps, bitrate {useBitRate} bps, {width}x{height} pixels";

                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CCameraReceiverNode")
                        {
                           if (methodName == "SetFrameResolutionAndMaxCaptureFrameRate")
                           {
                              // VIDEO_TR [2321AFE4] CCameraReceiverNode::SetFrameResolutionAndMaxCaptureFrameRate: width=640, height=480, frame rate=15, bLandscapeEn=1, bPortraitEn=1
                              string paramString = payLoad.Substring(payLoad.IndexOf("SetFrameResolutionAndMaxCaptureFrameRate: ") + 42);
                              regex = new Regex("width=(?<width>[0-9]*), height=(?<height>[0-9]*), frame rate=(?<framerate>[0-9]*), bLandscapeEn=(?<landscapeen>[0-9]*), bPortraitEn=(?<portraiten>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int frameRate = int.Parse(m.Groups["framerate"].Value);
                                 int width = int.Parse(m.Groups["width"].Value);
                                 int height = int.Parse(m.Groups["height"].Value);

                                 analysis = "camera-set-capture-resolution";
                                 msgNote = $"{width}x{height} pixels, framerate {frameRate} fps";

                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "CQoSVideoTxChannelController")
                        {
                           if (methodName == "ModifyBandwidth")
                           {
                              // [2321AFE4] CQoSVideoTxChannelController::ModifyBandwidth: New bw value 1856000
                              // [0AE4DBDC] CQoSVideoTxChannelController::ModifyBandwidth: reduce fecBitrate 148480 ==> video new bitrate = 1707520

                              string paramString = payLoad.Substring(payLoad.IndexOf("New bw value ") + 13);

                              if (!payLoad.Contains("New bw value ") && payLoad.Contains("new bitrate = "))
                              {
                                 paramString = payLoad.Substring(payLoad.IndexOf("new bitrate = ") + 14);
                              }

                              regex = new Regex("(?<bandwidth>[0-9]*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 int bandwidth = int.Parse(m.Groups["bandwidth"].Value);

                                 analysis = "video-modify-bandwidth";
                                 msgNote = $"{bandwidth} bps";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }

                           else if (methodName == "SetVideoMode")
                           {
                              // [2321AFE4] CQoSVideoTxChannelController::SetVideoMode: Video Mode = SHARPNESS

                              string paramString = payLoad.Substring(payLoad.IndexOf("Video Mode = ") + 13);
                              regex = new Regex("(?<mode>\\w*)");
                              m = regex.Match(paramString);
                              success = m.Success;
                              if (success)
                              {
                                 string modeName = m.Groups["mode"].Value;

                                 analysis = "video-set-mode";
                                 msgNote = $"{modeName}";
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        else if (className == "QosVideoTxBitrateDecisionsTable")
                        {
                           if (methodName == "GetH264BestDecision")
                           {
                              // [2321AFE4] QosVideoTxBitrateDecisionsTable::GetH264BestDecision: maxBitrate=1920000 maxMbps=108000 maxFs=3600 maxFr=30
                              /*
                               * The following are examples of maxMBPS calculation:
                                   720p resolution dimensions: 1280x720 pixels = 921600 pixels
                                   CIF resolution dimensions: 352x288 pixels = 101376 pixels
                                   Size of a macroblock: 16x16 pixels = 256 pixels
                                   Number of macroblocks in 720p resolution: 921600/256 = 3600 macroblocks per frame
                                   For 30 frames per second: 3600x30 = 108000 macroblocks per second
                                   Number of macroblocks in CIF resolution: 101376/256 = 396 macroblocks per frame
                                   For 30 frames per second: 396x30 = 11880 macroblocks per second
                               */

                              if (payLoad.Contains("maxBitrate"))
                              {
                                 string paramString = payLoad.Substring(payLoad.IndexOf("maxBitrate"));
                                 regex = new Regex("maxBitrate=(?<bitrate>[0-9]*) maxMbps=(?<mbps>[0-9]*) maxFs=(?<fs>[0-9]*) maxFr=(?<fr>[0-9]*)");
                                 m = regex.Match(paramString);
                                 success = m.Success;
                                 if (success)
                                 {
                                    long bitrate = long.Parse(m.Groups["bitrate"].Value);         // bitrate on the wire
                                    long macroBlocksPerSec = long.Parse(m.Groups["mbps"].Value);  // 256-byte macroblocks/sec
                                    int fs = int.Parse(m.Groups["fs"].Value);                   // macroblocks per frame
                                    int fr = int.Parse(m.Groups["fr"].Value);                   // max frame rate frames/sec (fps)

                                    analysis = "video-get-H264-parameters";
                                    msgNote = $"bitrate {bitrate} bps, maxMacroblocks/sec {macroBlocksPerSec}, macroBlocks/frame {fs}, maxframes/sec {fr}";
                                    _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                                 }
                              }
                           }
                        }

                        else if (className == "")
                        {
                           if (methodName == "CRTPRendererHandler")
                           {
                              // [2321AFE4] CRTPRendererHandler: Set Codec Type: H264 Payload: 102

                              if (payLoad.Contains("Set Codec Type"))
                              {
                                 string paramString = payLoad.Substring(payLoad.IndexOf("Type: ") + 6);
                                 regex = new Regex("(?<codec>\\w*) ");
                                 m = regex.Match(paramString);
                                 success = m.Success;
                                 if (success)
                                 {
                                    string codecName = m.Groups["codec"].Value;

                                    analysis = "video-set-codec";
                                    msgNote = $"{codecName}";
                                    _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                                 }
                              }
                           }
                        }

                        break;

                     // UDP transport
                     case "TRANSPORT":
                        // analysis = "transport";

                        /* already flagged protocol=RTP, analysis=transport
                        if (className == string.Empty && methodName == "RvSockTranspReceiveBuffer")
                        {
                           analysis = "audio-received-buffer";
                        }
                        else if (className == string.Empty && methodName == "RvSockTranspSendBuffer")
                        {
                           analysis = "audio-transmit-buffer";
                        }
                        */
                        // RvSockTranspReceiveBuffer: 164A0160(sock=33284): buff=1A8A0050,received=1262, addr=192.168.56.236:3286
                        // RvSockTranspSendBuffer: 1649F300(sock=33348): buff=167D4F38,buffLen=172,addr=192.168.56.236:3278
                        break;
                  }

                  // identify lines with RTP transmit and receive
                  // the 'source' name is TRANSPORT, when sending the tag varies eg: 91 RTPSender, for receiving eg: 87 RTPDuplex

                  switch (this.methodName)
                  {
                     case "RvSockTranspSendBuffer":
                        // the tag varies when sending eg: 91 RTPSender
                        // RvSockTranspSendBuffer: 0C70EED8(sock=33036): buff=1C628848,buffLen=1264,addr=192.168.53.235:3280
                        // RvSockTranspSendBuffer: 0EEC22C8(sock=33756): buff=53D6B560,buffLen=1,addr=192.168.20.90:3272   - sudden switch to local address, always buffLen=1

                        string paramString = payLoad.Substring(payLoad.IndexOf(",buffLen") + 1);
                        regex = new Regex("buffLen=(?<buffLen>[0-9]*),addr=(?<addr>[0-9.]*):(?<port>[0-9]*)");
                        m = regex.Match(paramString);
                        success = m.Success;
                        if (success)
                        {
                           int port = int.Parse(m.Groups["port"].Value);
                           string ipAddress = m.Groups["addr"].Value;

                           // validate the IP Address and Port#, to avoid misinterpreting the log
                           if (ipAddress != _activeSipSession?.RemoteEndpointAddress)
                           {

                           }

                           // if _activeSipSession is null, there was no SIP session detected yet in the log
                           string entity = _activeSipSession?.StreamingSessionPortName(MediaStream.StreamDirection.Outgoing, m.Groups["port"].Value) ?? string.Empty;

                           // entity = _activeSipSession?.StreamingSessionPortName(MediaStream.StreamDirection.Incoming, m.Groups["port"].Value) ?? string.Empty;

                           string details = $"Sent {m.Groups["buffLen"].Value} bytes to {m.Groups["addr"].Value}:{m.Groups["port"].Value} ({entity})";
                           msgNote = details;
                           protocol = "RTP";

                           if (_activeSipSession != null)
                           {

                           }

                           // map the port number to audio-video
                           if (!entity.Contains("control") && m.Groups["buffLen"].Value != "0")
                           {
                              if (entity.Contains("Audio"))
                              {
                                 analysis = "audio-sent";
                                 audioSent = long.Parse(m.Groups["buffLen"].Value);
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);

                                 // OLD
                                 /*
                                 // convert # bytes sent to bytes/sec transmission rate for ease of graphing
                                 double movingAvg;

                                 _activeSipSession?.RecordBytesStreamed(
                                    MediaStream.StreamDirection.Outgoing,
                                    MediaStream.MediaType.Audio,
                                    Convert.ToDateTime(Timestamp),
                                    long.Parse(m.Groups["buffLen"].Value),
                                    out movingAvg);

                                 _activeSipSession?.GetAllStreamMovingAverages(out audioReceived, out audioSent, out videoReceived, out videoSent);
                                 */

                              }
                              else if (entity.Contains("Video"))
                              {
                                 analysis = "video-sent";
                                 videoSent = long.Parse(m.Groups["buffLen"].Value);
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Outgoing, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);

                                 // OLD
                                 /*
                                 // convert # bytes sent to bytes/sec transmission rate for ease of graphing
                                 double movingAvg;

                                 _activeSipSession?.RecordBytesStreamed(
                                    MediaStream.StreamDirection.Outgoing,
                                    MediaStream.MediaType.Video,
                                    Convert.ToDateTime(Timestamp),
                                    long.Parse(m.Groups["buffLen"].Value),
                                    out movingAvg);

                                 _activeSipSession?.GetAllStreamMovingAverages(out audioReceived, out audioSent, out videoReceived, out videoSent);
                                 */

                              }
                           }

                           direction = "sent to " + entity;
                        }
                        break;

                     case "RvSockTranspReceiveBuffer":
                        // the tag varies when receiving eg: 87 RTPDuplex
                        // RvSockTranspReceiveBuffer: 0C70F5B8(sock=33956): buff=1B06A850,received=172,addr=192.168.53.235:3278
                        // RvSockTranspReceiveBuffer: 00BC3790(sock=4128): buff=16F82CB8,received=172, addr=10.31.7.24:3278

                        // what does this indicate?
                        // "RvSockTranspReceiveBuffer: 10248478(sock=34392): buff=14B819D8,received=0, addr=:0"

                        // not
                        // "RvSockTranspReceiveBuffer: 10248478(sock=34392): buff=14B812D8, buffLen=1520"

                        if (payLoad.Contains("received"))
                        {
                           paramString = payLoad.Substring(payLoad.IndexOf(",received") + 1);
                           regex = new Regex("received=(?<received>[0-9]*), addr=(?<addr>[0-9.]*):(?<port>[0-9]*)");

                           m = regex.Match(payLoad);
                           success = m.Success;
                           if (success)
                           {
                              int port = int.Parse(m.Groups["port"].Value);
                              string ipAddress = m.Groups["addr"].Value;

                              if (port != 0)
                              {
                                 // incoming receive is FROM THE remote side's IP address and port, rather than ON the local port
                                 // NOTE: this might vary by BeeHD version?  It is true for Version 4.7.24.8

                                 // if _activeSipSession is null, there was no SIP session detected yet in the log
                                 string entity = _activeSipSession?.StreamingSessionPortName(MediaStream.StreamDirection.Outgoing, m.Groups["port"].Value) ?? string.Empty;

                                 if (!entity.StartsWith("Audio") && !entity.StartsWith("Video"))
                                 {
                                    entity = "unknownstream-" + entity;
                                 }

                                 string details = $"Received {m.Groups["received"].Value} bytes from {m.Groups["addr"].Value}:{m.Groups["port"].Value} ({entity})";
                                 msgNote = details;

                                 if (Convert.ToInt16(m.Groups["received"].Value) != 0)
                                 {
                                    protocol = "RTP";

                                    // map the port number to audio-video

                                    if (!entity.Contains("control") && m.Groups["received"].Value != "0")
                                    {
                                       if (entity.Contains("Audio"))
                                       {
                                          analysis = "audio-received";
                                          audioReceived = long.Parse(m.Groups["received"].Value);
                                          _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);

                                          // OLD
                                          /*
                                          // convert to bytes/sec transmission rate for ease of graphing
                                          double movingAvg;

                                          // convert # bytes sent to bytes/sec transmission rate for ease of graphing
                                          _activeSipSession?.RecordBytesStreamed(
                                             MediaStream.StreamDirection.Incoming,
                                             MediaStream.MediaType.Audio,
                                             Convert.ToDateTime(Timestamp),
                                             long.Parse(m.Groups["received"].Value),
                                             out movingAvg);

                                          _activeSipSession?.GetAllStreamMovingAverages(out audioReceived, out audioSent, out videoReceived, out videoSent);
                                          */
                                       }
                                       else if (entity.Contains("Video"))
                                       {
                                          analysis = "video-received";
                                          videoReceived = long.Parse(m.Groups["received"].Value);
                                          _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Video, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);

                                          // OLD
                                          /*
                                          // convert to bytes/sec transmission rate for ease of graphing
                                          double movingAvg;

                                          _activeSipSession?.RecordBytesStreamed(
                                             MediaStream.StreamDirection.Incoming,
                                             MediaStream.MediaType.Video,
                                             Convert.ToDateTime(Timestamp),
                                             long.Parse(m.Groups["received"].Value),
                                             out movingAvg);

                                          _activeSipSession?.GetAllStreamMovingAverages(out audioReceived, out audioSent, out videoReceived, out videoSent);
                                          */
                                       }
                                    }
                                 }

                                 direction = "received from " + entity;
                              }
                           }
                        }
                        break;

                     case "ProcessBuffer":
                        // AUDIO_RE
                        // CAudioDecoderNode::ProcessBuffer: m_LastPackType=3 m_plcCount=0, m_cngCount=0 FillCount 8 JitterFillinMS=160 m_lastSeqNum=12505
                        // "m_LastPackType = 4 m_plcCount=1, m_cngCount=0 FillCount 3 JitterFillinMS=40 m_lastSeqNum=11189"

                        // paramString = payLoad.Substring(payLoad.IndexOf(",received") + 1);
                        regex = new Regex("FillCount (?<fillcount>[0-9])* JitterFillinMS=(?<fillMS>[0-9.]*) m_lastSeqNum=(?<lastseqnum>[0-9]*)$");

                        m = regex.Match(payLoad);
                        success = m.Success;
                        if (success)
                        {
                           // estimate the number of audio packets successfully processed since the most recent loss report

                           long lastAudioSeqReceived = Convert.ToInt64(m.Groups["lastseqnum"].Value);
                           long goodAudioSeqReceived = 0;

                           if (_lastIncomingAudioSeqReported > 0)
                           {
                              goodAudioSeqReceived = lastAudioSeqReceived - _lastIncomingAudioSeqReported;
                           }
                           else
                           {
                              goodAudioSeqReceived = lastAudioSeqReceived;
                           }

                           if (goodAudioSeqReceived < 0)
                           {
                              analysis = "audio-received-outoforder";
                              msgNote = $"received packet out of order? LastSeq# {m.Groups["lastseqnum"].Value}";
                              _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                           }

                           else
                           {
                              _lastIncomingAudioSeqReported = lastAudioSeqReceived;

                              long lostPackets = Convert.ToInt64(m.Groups["fillcount"].Value);
                              long countPackets = lostPackets + goodAudioSeqReceived;

                              double percentLost = Math.Round((double) lostPackets / countPackets * 100, 2);


                              msgNote = $"Audio QOS - lost {lostPackets} out of {countPackets} ({percentLost}%), LastSeq# {m.Groups["lastseqnum"].Value}, Good {goodAudioSeqReceived}, jitter {m.Groups["fillMS"].Value} msec.";
                              protocol = "RTP";

                              // jitter from video not available, but lost is

                              audioLost = int.Parse(m.Groups["fillcount"].Value);
                              audioJitter = int.Parse(m.Groups["fillMS"].Value);

                              if (audioLost > 0)
                              {
                                 // see also CAudioDecoderNode::HandlePacketLoss
                                 analysis = "audio-quality";

                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                                 _activeSipSession?.RecordOtherPacketStatistics(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), audioLost, goodAudioSeqReceived, audioJitter);
                              }
                              else
                              {
                                 analysis = "audio-receiver-ok";
                                 msgNote = string.Empty;
                                 _activeSipSession?.RecordStreamEvent(MediaStream.StreamDirection.Incoming, MediaStream.MediaType.Audio, Convert.ToDateTime(Timestamp), analysis, msgNote, 1);
                              }
                           }
                        }

                        break;
                  }
               }
            }

            // output values that persist between SIP message lines ..
            callState = _sipCallState;

            if (_sipMessage != null)
            {
               if (_sipMessage.SipAndSdpPayloadComplete && !_isMultilineSipMessageProcessedFlag)
               {
                  // once the message is complete, output the analysis
                  _isMultilineSipMessageProcessedFlag = true;

                  analysis = "SIPSUMMARY";
                  protocol = "SIP";
                  endPoints = _sipMessage.EndpointsSummary;
                  msgNote = _sipMessage.Note;
                  msgExpectNote = _sipMessage.Expect;
                  summaryNote = _sipMessage.ShortSummary;
                  timeSinceLastMessage = _timeSinceLastMessage;

                  // find the call-id in the list of SipSessions and add the message,
                  // or create a new session to hold the message

                  bool messageAdded = false;
                  foreach (SipSession.SipSession sipSession in _sipSessions)
                  {
                     if (sipSession.HasCallId)
                     {
                        try
                        {
                           // succeeds only if the Session's call-id matches the Message call-id
                           sipSession.AppendMessage(_sipMessage);
                           messageAdded = true;

                           _activeSipSession = sipSession;

                           // _sipCallState = sipSession.State.ToString();

                           if (_sipMessage.IsRetransmission)
                           {
                              msgNote = "(retransmission) " + msgNote;
                           }
                        }
                        catch (Exception e)
                        {
                           // not necessarily an error - do not throw.  This might be the first message or the session's Call-Id doesn't match

                           if (e.Message != "The call-id is not valid for this session.")
                           {
                              throw e;
                           }
                        }
                     }
                  }

                  if (!messageAdded)
                  {
                     try
                     {
                        if (!_activeSipSession.HasCallId)
                        {
                           // this message is the first one, and will establish the call-id
                           _activeSipSession.AddFirstMessage(_sipMessage);
                        }
                        else
                        {
                           // starting a new session with an initial message
                           SipSession.SipSession newSipSession = new SipSession.SipSession(_sipMessage);
                           _sipSessions.Add(newSipSession);

                           _activeSipSession = newSipSession;
                        }

                        messageAdded = true;
                        // _sipCallState = _activeSipSession.State.ToString();
                     }
                     catch (Exception e)
                     {
                        // this will be an error
                        throw e;
                     }
                  }

                  if (!messageAdded)
                  {
                     // this will be an error
                  }

                  // no longer needed?
                  _sipMessage = null;
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine(">>>EXCEPTION BELine constructor : Processing logline : " + ex.Message);
            if (ex.InnerException != null)
            {
               Console.WriteLine(">>>INNER EXCEPTION : " + ex.InnerException.Message);
            }
            Console.WriteLine($"{lineNumber} {logLine}");
         }
      }


      public string LogFileVersion { get { return _logFileVersion; } }

      public string LogFileProductName { get { return _logFileProduct; } }

      public bool InterestingLogLine { get { return _interestingLogLine;  } }



      protected virtual void Initialize()
      {
         Timestamp = tsTimestamp();  // "11/13/23 07:34:23.011"
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();
      }

      protected override string hResult()
      {
         return "";
      }


      protected override string tsTimestamp()
      {
         // set timeStamp to a default time
         string timeStamp = DateTime.MinValue.ToString("MM/dd/yy hh:mm:ss.fff");

         // search for timestamp in the log line
         Regex timeRegex = new Regex(@"^(?<pre>.*) (?<stamp>\d{2}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}\.\d{3})");
         Match m = timeRegex.Match(logLine);
         if (m.Success)
         {
            timeStamp = m.Groups["stamp"].Value;
         }

         return timeStamp;
      }
   }
}
