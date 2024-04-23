using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SipSession
{
   public class SipSession: IDisposable
   {
      // all SIP messages share the same call-id
      private List<SipMessage> sipMessages = new List<SipMessage>();

      private MediaStream incomingAudioStream = new MediaStream(MediaStream.MediaType.Audio, MediaStream.StreamDirection.Incoming);
      private MediaStream outgoingAudioStream = new MediaStream(MediaStream.MediaType.Audio, MediaStream.StreamDirection.Outgoing);
      private MediaStream incomingVideoStream = new MediaStream(MediaStream.MediaType.Video, MediaStream.StreamDirection.Incoming);
      private MediaStream outgoingVideoStream = new MediaStream(MediaStream.MediaType.Video, MediaStream.StreamDirection.Outgoing);

      private MediaTimeSeries timeSeries = null;

      private SessionState state = SessionState.Idle;
      public bool isDuplicateSipMessage = false;

      public enum SessionState { Idle, Invite, WaitingACK, Connected, Disconnecting, Unknown };
      public enum SessionDirection { Incoming, Outgoing };


      // TODO: analysis based on SIP timer values depending on the type of message being sent:
      // initial values for UDP connections
      // timers D, H, I, J do not apply to TCP/STCP connections

      private static int remoteUnreachableTimeoutMsec = 32 * 1000;

      private static int timer1Msec = 500;             // Round-trip time (RTT) estimate
      private static int timer2Msec = 4000;            // Maximum retransmission interval for non-INVITE requests and INVITE responses
      private static int timer4Msec = 5000;            // Maximum duration that a message can remain in the network

      private static int timerAMsec = timer1Msec;      // INVITE request retransmission interval, for UDP only
      private static int timerBMsec = 64 * timer1Msec; // INVITE transaction timeout
      private static int timerCMsec = 180000;          // proxy INVITE transaction timeout (1xx provisional responses)

      private static int timerDMsec = 32 * 1000 + 1;   // Maximum time for response retransmissions  (0 sec not applicable for TCP)
      private static int timerEMsec = timer1Msec;      // Non-INVITE request retransmission interval, UDP only
      private static int timerFMsec = 64 * timer1Msec; // Maximum time for Non-INVITE transaction
      private static int timerGMsec = timer1Msec;      // INVITE response retransmission interval
      private static int timerHMsec = 64 * timer1Msec; // Maximum wait time for ACK receipt               (0 sec not applicable for TCP)
      private static int timerIMsec = timer4Msec;      // Maximum time for ACK retransmissions       (0 sec not applicable for TCP)
      private static int timerJMsec = 64 * timer1Msec; // Maximum time for retransmissions of non-INVITE requests  (0 sec not applicable for TCP)
      private static int timerKMsec = timer4Msec;      // Maximum time for response retransmissions

      private string sessionConnectedNote = string.Empty;
      private DateTime sessionStartTime = DateTime.MinValue;

      /// <summary>
      /// Instantiates a new instance of the SipSession class.
      /// </summary>
      public SipSession()
      {
         // Console.WriteLine("+SipSession()");
      }


      /// <summary>
      /// Instantiates a new instance of the SipSession class.
      /// </summary>
      /// <param name="msg">The message to add.</param>
      /// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
      public SipSession(SipMessage msg)
      {
         // Console.WriteLine($"+SipSession({msg.CallId})");

         AddFirstMessage(msg);
      }


      /// <summary>
      /// Destructor
      /// </summary>
      ~SipSession()
      {
         // Console.WriteLine("~SipSession");

         this.Dispose();
      }


      /// <summary>
      /// Free memory resources
      /// </summary>
      public void Dispose()
      {
         if (incomingAudioStream != null)
         {
            incomingAudioStream.Dispose();
            incomingAudioStream = null;
         }

         if (outgoingAudioStream != null)
         {
            outgoingAudioStream.Dispose();
            outgoingAudioStream = null;
         }

         if (incomingVideoStream != null)
         {
            incomingVideoStream.Dispose();
            incomingVideoStream = null;
         }

         if (outgoingVideoStream != null)
         {
            outgoingVideoStream.Dispose();
            outgoingVideoStream = null;
         }

         if (this.timeSeries != null)
         {
            timeSeries = null;
         }
      }


      /// <summary>
      /// Adds the first message to the list and establishes the call-id
      /// </summary>
      /// <param name="msg"></param>
      /// <exception cref="ArgumentNullException"></exception>
      public void AddFirstMessage(SipMessage msg)
      {
         if (msg == null)
         {
            throw new ArgumentNullException(nameof(msg));
         }

         this.AddSipMessageToList(msg);
         this.UpdateSessionState();

         this.timeSeries = new MediaTimeSeries(Convert.ToDateTime(msg.Timestamp));
      }


      /// <summary>
      /// Adds additional messages to the list.
      /// </summary>
      /// <param name="msg"></param>
      private void AddSipMessageToList(SipMessage msg)
      {
         // this.TimingAnalysis(msg);

         this.sipMessages.Add(msg);

         // update local media port variables

         if (msg.SdpSectionComplete && (msg.MessageType == "INVITE" || msg.MessageType == "OK"))
         {
            if (msg.Direction == SipMessage.MessageDirection.In)
            {
               // stream traffic outgoing to the Remote side
               this.outgoingAudioStream.EndpointAddress = msg.RemoteIpAddress.Replace(":5060", string.Empty);
               this.outgoingAudioStream.ControlPort = msg.AudioControlPort;
               this.outgoingAudioStream.Port = msg.AudioStreamingPort;

               this.outgoingVideoStream.EndpointAddress = msg.RemoteIpAddress.Replace(":5060", string.Empty);
               this.outgoingVideoStream.ControlPort = msg.VideoControlPort;
               this.outgoingVideoStream.Port = msg.VideoStreamingPort;
            }
            else if (msg.Direction == SipMessage.MessageDirection.Out)
            {
               // stream traffic incoming from the Remote side
               this.incomingAudioStream.EndpointAddress = msg.Contact.Replace("<sip:", string.Empty).Replace(">", string.Empty);
               this.incomingAudioStream.ControlPort = msg.AudioControlPort;
               this.incomingAudioStream.Port = msg.AudioStreamingPort;

               this.incomingVideoStream.EndpointAddress = msg.Contact.Replace("<sip:", string.Empty).Replace(">", string.Empty);
               this.incomingVideoStream.ControlPort = msg.VideoControlPort;
               this.incomingVideoStream.Port = msg.VideoStreamingPort;
            }
         }
      }


      /// <summary>
      /// Attempt to append a SIP message.
      /// </summary>
      /// <param name="msg">The message to add.</param>
      /// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
      /// <exception cref="InvalidOperationException">Thrown if the call-id of the message is empty or does not match the messages already in the list.</exception>
      public void AppendMessage(SipMessage msg)
      {
         if (msg == null)
         {
            throw new ArgumentNullException(nameof(msg));
         }

         if (msg.CallId == string.Empty)
         {
            throw new InvalidOperationException("The call-id is empty.");
         }

         if (msg.CallId != this.sipMessages[0].CallId)
         {
            throw new InvalidOperationException("The call-id is not valid for this session.");
         }

         // check for duplicates
         foreach (SipMessage listMsg in this.sipMessages)
         {
            if (msg.IsIdentical(listMsg))
            {
               msg.IsRetransmission = true;
               break;
            }
         }

         this.AddSipMessageToList(msg);
         this.UpdateSessionState();

         // https://www.tutorialspoint.com/session_initiation_protocol/session_initiation_protocol_basic_call_flow.htm

         // TODO: determination whether there is a NAT router or SIP appliance rewriting the headers

         // TODO: synchronization of the local and remote party message sequences - both to determine log file time
         //       offset and to know whether and when messages transmitted are received by the other party.  Without
         //       such, the only way to know is to use the INVITE responses.
      }


      /// <summary>
      /// Updates the session state by examining the SIP messages that have been received.
      /// </summary>
      private void UpdateSessionState()
      {
         // simple determination of the call state
         //   if first message is not an INVITE, assume starting in the middle of a call with unknown state
         //   if no messages or there is a BYE, assume Idle
         //   if contains at least one INVITE that was accepted with 200 OK and no BYE, assume connected

         this.state = SessionState.Idle;

         bool firstMessage = true;

         DateTime sessionConnected = DateTime.MinValue;
         DateTime sessionDisconnected = DateTime.MinValue;

         this.sessionConnectedNote = string.Empty;

         foreach (SipMessage sipMsg in this.sipMessages)
         {
            if (firstMessage && sipMsg.MessageType != "INVITE")
            {
               this.state = SessionState.Unknown;
            }

            if (sipMsg.MessageType == "BYE")
            {
               this.state = SessionState.Disconnecting;

               if (sessionDisconnected == DateTime.MinValue)
               {
                  sessionDisconnected = Convert.ToDateTime(sipMsg.Timestamp);

                  sipMsg.SipSessionNote = $"Total connected {(sessionDisconnected - sessionConnected).TotalSeconds} seconds.";
               }

               // is there a matching 200 OK?
               foreach (SipMessage respMsg in this.sipMessages)
               {
                  if (respMsg.MessageType == "OK" &&
                     respMsg.Cseq == sipMsg.Cseq)
                  {
                     this.state = SessionState.Idle;
                     break;
                  }
               }

               break;
            }

            if (sipMsg.MessageType == "INVITE")
            {
               this.state = SessionState.Invite;

               int cseq = sipMsg.Cseq;

               // is there a matching 200 OK and ACK response?

               foreach (SipMessage respMsg in this.sipMessages)
               {
                  if (respMsg.MessageType == "OK" &&
                     respMsg.Cseq == sipMsg.Cseq)
                  {
                     this.state = SessionState.Connected;

                     if (sessionConnected == DateTime.MinValue)
                     {
                        sessionConnected = Convert.ToDateTime(sipMsg.Timestamp);

                        this.sessionStartTime = sessionConnected;
                     }
                  }
               }
            }

            firstMessage = false;
         }
      }


      /// <summary>
      /// Checks whether an IP address in the description is a private IP address.
      /// </summary>
      /// <param name="contactDescription"></param>
      /// <returns>true if the address in the description is private IP address</returns>
      private bool IsPrivateIpAddress(string contactDescription)
      {
         // find and parse an IP address

         // \b(?<first>\d{1,3})\.(?<second>\d{1,3})\.(?<third>\d{1,3})\.(?<fourth>\d{1,3})\b

         Regex regex = new Regex("\\b(?<first>\\d{1,3})\\.(?<second>\\d{1,3})\\.(?<third>\\d{1,3})\\.(?<fourth>\\d{1,3})\\b");
         Match m = regex.Match(contactDescription);
         bool success = m.Success;
         if (success)
         {
            int first = int.Parse(m.Groups["first"].Value);
            int second = int.Parse(m.Groups["second"].Value);

            if (first == 10 ||
               (first == 192 && second == 168) ||
               (first == 172 && (second >= 16 && second <= 31)))
            {
               return true;
            }
         }

         return false;
      }


      /// <summary>
      /// Gets the call-id for the session.
      /// </summary>
      public string CallId
      {
         get { return this.sipMessages[0].CallId; }
      }


      /// <summary>
      /// Gets a value indicating whether the session has a call-id.
      /// </summary>
      public bool HasCallId
      {
         get
         {
            if (this.sipMessages.Count > 0 && this.sipMessages[0].CallId != string.Empty)
            {
               return true;
            }

            return false;
         }
      }


      /// <summary>
      /// Gets the state of the session.
      /// </summary>
      public SessionState State { get { return this.state; } }


      /// <summary>
      /// Gets a descriptive name associated with the port number given.
      /// </summary>
      /// <param name="direction">The direction of the stream.</param>
      /// <param name="port">A numeric string representing a UDP port number.</param>
      /// <returns>A description, or empty string.</returns>
      public string StreamingSessionPortName(MediaStream.StreamDirection direction, string port)
      {
         if (direction == MediaStream.StreamDirection.Incoming)
         {
            // audio and video use separate port ranges
            if (port == this.incomingAudioStream.ControlPort)
            {
               return $"{this.incomingAudioStream.StreamName}-control";
            }
            else if (port == this.incomingVideoStream.ControlPort)
            {
               return $"{this.incomingVideoStream.StreamName}-control";
            }
            else if (port == this.incomingAudioStream.Port)
            {
               return $"{this.incomingAudioStream.StreamName}";
            }
            else if (port == this.incomingVideoStream.Port)
            {
               return $"{this.incomingVideoStream.StreamName}";
            }
         }
         else
         {
            // audio and video use separate port ranges
            if (port == this.outgoingAudioStream.ControlPort)
            {
               return $"{this.outgoingAudioStream.StreamName}-control";
            }
            else if (port == this.outgoingVideoStream.ControlPort)
            {
               return $"{this.outgoingVideoStream.StreamName}-control";
            }
            else if (port == this.outgoingAudioStream.Port)
            {
               return $"{this.outgoingAudioStream.StreamName}";
            }
            else if (port == this.outgoingVideoStream.Port)
            {
               return $"{this.outgoingVideoStream.StreamName}";
            }
         }

         return $"{direction.ToString()}-{port}";
      }


      /// <summary>
      /// Records the number of packets lost and jitter.
      /// </summary>
      /// <param name="direction">stream direction</param>
      /// <param name="type">stream type</param>
      /// <param name="timestamp">timestamp</param>
      /// <param name="lostPackets">the number of packets lost</param>
      /// <param name="goodPackets">the number of packets received correctly</param>
      /// <param name="jitterMsec">jitter in Msec</param>
      public void RecordOtherPacketStatistics(MediaStream.StreamDirection direction, MediaStream.MediaType type, DateTime timestamp, long lostPackets, long goodPackets, int jitterMsec)
      {
         if (direction == MediaStream.StreamDirection.Incoming)
         {
            if (type == MediaStream.MediaType.Audio)
            {
               this.incomingAudioStream.LogDataTransferErrors(timestamp, lostPackets, goodPackets, jitterMsec);
            }
            else
            {
               this.incomingVideoStream.LogDataTransferErrors(timestamp, lostPackets, goodPackets, jitterMsec);
            }
         }
         else
         {
            if (type == MediaStream.MediaType.Audio)
            {
               this.outgoingAudioStream.LogDataTransferErrors(timestamp, lostPackets, goodPackets, jitterMsec);
            }
            else
            {
               this.outgoingVideoStream.LogDataTransferErrors(timestamp, lostPackets, goodPackets, jitterMsec);
            }
         }
      }


      /// <summary>
      /// Adds a log event to the list.
      /// </summary>
      /// <param name="direction"></param>
      /// <param name="type"></param>
      /// <param name="timestamp"></param>
      /// <param name="tag"></param>
      /// <param name="description"></param>
      /// <param name="count"></param>
      /// <exception cref="Exception"></exception>
      public void RecordStreamEvent(MediaStream.StreamDirection direction, MediaStream.MediaType type, DateTime timestamp, string tag, string description, long count)
      {
         MediaStreamEvent newEvent = null;

         if (direction == MediaStream.StreamDirection.Incoming)
         {
            if (type == MediaStream.MediaType.Audio)
            {
               newEvent = this.incomingAudioStream.LogStreamEvent(timestamp, tag, description, count);
            }
            else
            {
               newEvent = this.incomingVideoStream.LogStreamEvent(timestamp, tag, description, count);
            }
         }
         else
         {
            if (type == MediaStream.MediaType.Audio)
            {
               newEvent = this.outgoingAudioStream.LogStreamEvent(timestamp, tag, description, count);
            }
            else
            {
               newEvent = this.outgoingVideoStream.LogStreamEvent(timestamp, tag, description, count);
            }
         }

         // update the session time-series buckets
         try
         {
            // if timeSeries is null, nothing to do
            this.timeSeries?.UpdateTimeBucket(newEvent);
         }
         catch (Exception ex)
         {
            // should not occur
            throw new Exception("SipSession.RecordStreamEvent exception", ex);
         }

      }


      /// <summary>
      /// Summarizes the SIP session, data transfer, and errors.
      /// </summary>
      public StringBuilder Summary()
      {
         StringBuilder summary = new StringBuilder();

         if (!HasCallId || this.sipMessages.Count == 0)
         {
            //summary.AppendLine("empty session");
            return summary;
         }

         bool firstMessage = true;
         DateTime messageTime = DateTime.MinValue;
         DateTime previousMessageTime = DateTime.MinValue;

         bool invite2xxAckExpected = false;
         bool invite2xxAckReceived = false;

         bool isSDPOnly = true;
         DateTime startTime = DateTime.MinValue;
         DateTime inviteStart = DateTime.MinValue;
         DateTime proxyResponded = DateTime.MinValue;
         DateTime responderResponded = DateTime.MinValue;
         DateTime ringingStart = DateTime.MinValue;
         DateTime answeredStart = DateTime.MinValue;
         DateTime callConnected = DateTime.MinValue;
         DateTime reInviteStart = DateTime.MinValue;
         DateTime reInviteAccepted = DateTime.MinValue;
         DateTime byeStarted = DateTime.MinValue;
         DateTime callDisconnected = DateTime.MinValue;
         DateTime finalMessage = DateTime.MinValue;

         int largestInviteMessageSize = 0;
         int largestOkMessageSize = 0;
         string networkPacketTypeUsed = string.Empty;

         int inviteCseq = 0;
         SipMessage.MessageDirection inviteDirection = SipMessage.MessageDirection.Undefined;
         SipMessage.MessageDirection inviteResponderDirection = SipMessage.MessageDirection.Undefined;

         SipMessage.MessageDirection answerDirection = SipMessage.MessageDirection.Undefined;
         SipMessage.MessageDirection responderDirection = SipMessage.MessageDirection.Undefined;

         int reInviteCseq = 0;
         SipMessage.MessageDirection reInviteDirection = SipMessage.MessageDirection.Undefined;
         SipMessage.MessageDirection reInviteResponderDirection = SipMessage.MessageDirection.Undefined;

         int byeCseq = 0;
         SipMessage.MessageDirection byeDirection = SipMessage.MessageDirection.Undefined;
         SipMessage.MessageDirection byeResponderDirection = SipMessage.MessageDirection.Undefined;

         string invitingPartyDescription = string.Empty;
         string respondingPartyDescription = string.Empty;

         // standard line format <time> <msec offset>
         string formatTimespan = "mm\\:ss\\.fff";
         string formatDateTime = "yyMMdd HH:mm:ss.fff";
         string formatTime = "HH:mm:ss.fff";
         string errorString = "                              ERROR ";
         string infoString = "                              INFO    ";
         string warningString = "                              WARNING ";
         bool isShowMessageList = true;

         summary.AppendLine();
         summary.AppendLine();
         summary.Append("** SESSION **  ");

         // normal state transitions (from-to)
         //   INVITE to TRYING/RINGING/OK       Inviting
         //   INVITE to OK                      Accepted or Media Change
         //   OK to ACK                         Connected or Media Change accepted
         //   BYE to OK                         Disconnecting
         //   OK to ACK                         Terminated

         foreach (SipMessage msg in this.sipMessages)
         {
            messageTime = Convert.ToDateTime(msg.Timestamp);

            if (firstMessage)
            {
               networkPacketTypeUsed = msg.PacketType;

               previousMessageTime = messageTime;

               startTime = Convert.ToDateTime(msg.Timestamp);

               if (msg.MessageType != "INVITE")
               {
                  summary.AppendLine($"{warningString} Session starts with {msg.MessageType} expected INVITE");
               }

               summary.AppendLine($"Call-id {msg.CallId}.");

               if (isShowMessageList)
               {
                  // show the messages in the summary
                  summary.AppendLine($"--sipmessages--");
               }
            }

            if (isShowMessageList)
            {
               // show the messages in the summary
               summary.AppendLine($"{messageTime.ToString(formatDateTime)} {(messageTime - startTime).ToString(formatTimespan)} {msg.MessageType} {msg.Cseq} {msg.Direction.ToString()} {msg.Contact} {msg.SessionOriginator} ({msg.MessageSize} bytes)");
            }

            if (msg.Contact != string.Empty)
            {
               // older Bee version logs don't contain SIP headers, only SDP
               isSDPOnly = false;
            }

            // good case
            // all messages in this session have the same CallId value, no need to check it

            if (msg.MessageType == "INVITE")
            {
               largestInviteMessageSize = (msg.MessageSize > largestInviteMessageSize) ? msg.MessageSize : largestInviteMessageSize;
            }
            else if (msg.MessageType == "OK")
            {
               largestOkMessageSize = (msg.MessageSize > largestOkMessageSize) ? msg.MessageSize : largestOkMessageSize;
            }

            // INVITE - starts an incoming or outgoing call
            if (msg.MessageType == "INVITE" && inviteStart == DateTime.MinValue)
            {
               inviteStart = Convert.ToDateTime(msg.Timestamp);
               inviteDirection = msg.Direction;
               inviteCseq = msg.Cseq;

               inviteResponderDirection = (inviteDirection == SipMessage.MessageDirection.In) ? SipMessage.MessageDirection.Out : SipMessage.MessageDirection.In;
               invitingPartyDescription = $"{msg.Contact} {msg.SessionOriginator}";
            }

            // TRYING - response from intermediate proxy servers
            if (msg.MessageType == "TRYING" && msg.Direction == inviteResponderDirection)
            {
               proxyResponded = Convert.ToDateTime(msg.Timestamp);
            }

            // RINGING - response from the other party
            if (msg.MessageType == "RINGING" && msg.Direction == inviteResponderDirection && ringingStart == DateTime.MinValue)
            {
               if (msg.Cseq == inviteCseq)
               {
                  responderResponded = Convert.ToDateTime(msg.Timestamp);
                  responderDirection = msg.Direction;

                  ringingStart = Convert.ToDateTime(msg.Timestamp);
               }
            }

            // OK - accepting a BYE
            if (msg.MessageType == "OK" && msg.Direction == byeResponderDirection && callDisconnected == DateTime.MinValue)
            {
               callDisconnected = Convert.ToDateTime(msg.Timestamp);
            }

            // OK - accepting an INVITE
            if (msg.MessageType == "OK" && msg.Direction == inviteResponderDirection && answeredStart == DateTime.MinValue && callDisconnected == DateTime.MinValue)
            {
               if (msg.Cseq == inviteCseq)
               {
                  answeredStart = Convert.ToDateTime(msg.Timestamp);
                  answerDirection = msg.Direction;

                  respondingPartyDescription = $"{msg.Contact} {msg.SessionOriginator}";
               }

               if (msg.Direction == SipMessage.MessageDirection.Out)
               {
                  invite2xxAckExpected = true;
                  invite2xxAckReceived = false;
               }
            }

            // OK - accepting a re-INVITE
            if (msg.MessageType == "OK" && msg.Direction == reInviteResponderDirection && reInviteAccepted == DateTime.MinValue && callDisconnected == DateTime.MinValue)
            {
               if (msg.Cseq == reInviteCseq)
               {
                  reInviteAccepted = Convert.ToDateTime(msg.Timestamp);
               }

               if (msg.Direction == SipMessage.MessageDirection.Out)
               {
                  invite2xxAckExpected = true;
                  invite2xxAckReceived = false;
               }
            }

            // ACK response to an OK
            if (msg.MessageType == "ACK")
            {
               if (msg.Direction == SipMessage.MessageDirection.In)
               {
                  if (invite2xxAckExpected)
                  {
                     invite2xxAckReceived = true;
                  }
               }

               if (msg.Cseq == inviteCseq)
               {
                  callConnected = Convert.ToDateTime(msg.Timestamp);
               }

               else if (msg.Cseq == byeCseq)
               {
                  // BYE usually gets OK but not ACK
               }
            }

            if (msg.MessageType == "BYE" && byeStarted == DateTime.MinValue)
            {
               byeStarted = Convert.ToDateTime(msg.Timestamp);
               byeDirection = msg.Direction;
               byeResponderDirection = (byeDirection == SipMessage.MessageDirection.In) ? SipMessage.MessageDirection.Out : SipMessage.MessageDirection.In;
               byeCseq = msg.Cseq;
            }

            // re-INVITE
            if (msg.MessageType == "INVITE" && msg.Cseq != inviteCseq && callConnected != DateTime.MinValue && callDisconnected == DateTime.MinValue)
            {
               reInviteCseq = msg.Cseq;
               reInviteDirection = msg.Direction;
               reInviteResponderDirection = (reInviteDirection == SipMessage.MessageDirection.In) ? SipMessage.MessageDirection.Out : SipMessage.MessageDirection.In;
               reInviteStart = Convert.ToDateTime(msg.Timestamp);
               reInviteAccepted = DateTime.MinValue;
            }

            firstMessage = false;
            previousMessageTime = messageTime;
         }

         finalMessage = messageTime;


         // initial INVITE time
         // the remote party IP address
         // the session originator name or IP address
         // whether the call connected successfully
         // times of connect and disconnect
         // how long it took to connect
         // the call duration
         // whether BYE was received or sent
         // amount of stream data transferred in and out
         // whether any SIP signalling errors
         // error summary for each stream
         //  whether any outages (from-to time)


         summary.AppendLine($"--interpretation--");

         summary.AppendLine($"Largest INVITE: {largestInviteMessageSize} bytes");
         summary.AppendLine($"Largest OK: {largestInviteMessageSize} bytes");

         summary.Append($"{startTime.ToString(formatDateTime)} {(startTime - startTime).ToString(formatTimespan)} ");

         if (inviteDirection == SipMessage.MessageDirection.In)
         {
            summary.AppendLine($"[TELLER] Incoming call request was received at machine.");
         }
         else if (inviteDirection == SipMessage.MessageDirection.Out)
         {
            summary.AppendLine($"[ATM] Outgoing call request was sent to a remote machine.");
         }

         summary.AppendLine($"{infoString} Call-id {this.CallId}");
         summary.AppendLine($"{infoString} The Inviter {invitingPartyDescription}");
         summary.AppendLine($"{infoString} The Responder {respondingPartyDescription}");

         if (respondingPartyDescription != string.Empty && IsPrivateIpAddress(respondingPartyDescription))
         {
            summary.AppendLine($"{infoString} The Responder has a private IP address behind a NAT or VPN.");
         }

         if (isSDPOnly)
         {
            summary.AppendLine($"{warningString} only SDP was detected in the log, no standard SIP messages");
         }

         /*
         summary.Append($"{inviteStart.ToString(formatDateTime)} {(inviteStart - startTime).ToString(formatTimespan)} ");
         summary.AppendLine($"Call setup started");
         */

         if (proxyResponded != DateTime.MinValue)
         {
            summary.Append($"{proxyResponded.ToString(formatDateTime)} {(proxyResponded - startTime).ToString(formatTimespan)} ");
            summary.AppendLine($"There is a SIP Proxy Agent relaying messages to and from the remote machine.");
         }

         if (responderResponded == DateTime.MinValue)
         {
            summary.AppendLine($"{errorString} The machine did not respond to an INVITE.");
            summary.AppendLine("** Verify the INVITE arrived at the remote endpoint, that the remote side responded with 200 OK,");
            summary.AppendLine("   that 200 OK was received and ACK sent, verify the remote side received the ACK.");

            if (networkPacketTypeUsed.ToLower() == "udp")
            {
               summary.AppendLine("** When using UDP and large messages are not received:");
               summary.AppendLine("    - check firewall rules and network congestion (UDP packets can be dropped).");
               summary.AppendLine("    - check the entire network path supports packet fragmentation-reassembly");
               summary.AppendLine("      or the maximum MTU is big enough for the largest message.");
               summary.AppendLine("    - consider using TCP instead of UDP.");
            }
         }
         else
         {
            summary.Append($"{responderResponded.ToString(formatDateTime)} {(responderResponded - startTime).ToString(formatTimespan)} ");

            if (responderDirection == SipMessage.MessageDirection.Out)
            {
               summary.AppendLine($"Started RINGING and informed the remote machine.");
            }
            else
            {
               summary.AppendLine($"Remote machine responded that it is RINGING.");
            }

            summary.AppendLine($"{infoString} {respondingPartyDescription}");
         }

         /*
         if (ringingStart == DateTime.MinValue)
         {
            summary.AppendLine($"{errorString} There was no RINGING sent or received.");
         }
         else
         {
            summary.Append($"{ringingStart.ToString(formatDateTime)} {(ringingStart - startTime).ToString(formatTimespan)} ");
            summary.AppendLine($"Ringing started");
         }
         */

         if (answeredStart == DateTime.MinValue)
         {
            summary.AppendLine($"{errorString} The RING was not answered by the remote party.");
         }
         else
         {
            summary.Append($"{answeredStart.ToString(formatDateTime)} {(answeredStart - startTime).ToString(formatTimespan)} ");
            if (answerDirection == SipMessage.MessageDirection.In)
            {
               summary.AppendLine($"The remote party answered the ring and accepted the call.");
            }
            else
            {
               summary.AppendLine($"Accepted the call and informed the remote party to finalize establishing the connection.");
            }
         }

         if (callConnected == DateTime.MinValue)
         {
            summary.AppendLine($"{errorString} The call failed to transition to the connected state.");
         }
         else
         {
            summary.Append($"{callConnected.ToString(formatDateTime)} {(callConnected - startTime).ToString(formatTimespan)} ");
            summary.AppendLine($"Success (Connected) - local and remote parties are connected, audio and video can flow.");
         }

         if (invite2xxAckExpected && !invite2xxAckReceived)
         {
            // if the ACK does not arrive at the remote endpoint it might be due to an invalid private IP address supplied in the Contact: header.
            // To know whether the other endpoint received a 2xx response correctly, ACK is expected to be received before TimerH expires - if it
            // expires the endpoint sends BYE.

            summary.AppendLine($"{errorString} ACK was not received, the call should disconnect after TimerH expires in {timerHMsec} msec.");
            summary.AppendLine($"{errorString} Verify that the remote party received the correct public address in the Contact: header of the 200 OK response.");
         }

         if (byeStarted == DateTime.MinValue)
         {
            summary.AppendLine($"{errorString} The call ended abruptly/abnormally without a BYE.");
         }
         else
         {
            summary.Append($"{byeStarted.ToString(formatDateTime)} {(byeStarted - startTime).ToString(formatTimespan)} ");

            if (byeDirection == SipMessage.MessageDirection.In)
            {
               summary.AppendLine($"BYE was received, the remote party ended the call.");
            }
            else if (byeDirection == SipMessage.MessageDirection.Out)
            {
               summary.AppendLine($"Call is ended, sent BYE to inform the remote party.");
            }
         }

         if (callDisconnected == DateTime.MinValue)
         {
            summary.AppendLine($"{errorString} The call did not disconnect normally with BYE - message might have been lost or log ends in the middle of a call.");

            summary.Append($"{finalMessage.ToString(formatDateTime)} {(finalMessage - startTime).ToString(formatTimespan)} ");
            summary.AppendLine($"Assumed disconnected");
         }
         else
         {
            summary.Append($"{callDisconnected.ToString(formatDateTime)} {(callDisconnected - startTime).ToString(formatTimespan)} ");
            summary.AppendLine($"Disconnected normally with BYE.");

            summary.AppendLine($"{infoString} Total time connected {(callDisconnected - callConnected).ToString(formatTimespan)}");

            if ((callDisconnected - callConnected).TotalSeconds < 60)
            {
               summary.AppendLine($"{warningString} very short connection time - was there a problem?");

               if (invite2xxAckExpected && !invite2xxAckReceived)
               {
                  summary.AppendLine($"{warningString} ACK was not received when 200 OK was sent - is a NAT ALG router rewriting the Contact: address incorrectly?");
               }
            }
         }

         // analyze and summarize statistics

         // put in a list for convenience
         List<MediaStream> streams = new List<MediaStream>();
         streams.Add(this.incomingAudioStream);
         streams.Add(this.outgoingAudioStream);
         streams.Add(this.incomingVideoStream);
         streams.Add(this.outgoingVideoStream);

         summary.AppendLine($"--statistics--");

         foreach (MediaStream stream in streams)
         {
            summary.Append($"{stream.StreamName} ");

            if (stream.MediaStreamData.AverageTransferRateBps == 0)
            {
               // MediaLogOptions
               summary.AppendLine($" Transfer Rate statistics not available - enable MediaLogOptions|MediaSessionFilters = DEBUG");
            }
            else
            {
               summary.Append($" Transmitted from {stream.MediaStreamData.Timestamp_FirstTransfer.ToString(formatTime)}");
               summary.Append($" to {stream.MediaStreamData.Timestamp_LastTransfer.ToString(formatTime)}");
               summary.Append($" Bytes transferred  {stream.MediaStreamData.TotalBytesTransferred.ToString()}");
               summary.AppendLine($" Avg Transfer Rate  {stream.MediaStreamData.AverageTransferRateBps.ToString()} bytes/sec");
            }

            summary.AppendLine($" Good packets {stream.PacketsReceived}, lost {stream.PacketsLost}, average packet loss {stream.AveragePacketLoss.ToString("##0")}%, peak packet loss {stream.MediaStreamData.MovingAveragePacketLossPercentage.ToString("##0")}%");

            // gets a formatted list of events
            bool isSummarizeConsecutive = true;
            List<string> eventList = stream.MediaStreamEventDescriptions(isSummarizeConsecutive);

            if (eventList.Count > 0)
            {
               foreach (string @event in eventList)
               {
                  summary.AppendLine($"{new String(' ', 6)} {@event}");
               }

               eventList.Clear();
            }
         }

         // release the list
         streams.Clear();

         // optional CSV output to log
         bool includeCsvInLog = true;

         if (includeCsvInLog)
         {
            List<string> bucketList = this.timeSeries.TimeBucketsToCsv();

            if (bucketList.Count > 0)
            {
               summary.AppendLine("** CSV Time Buckets");

               foreach (string csvText in bucketList)
               {
                  summary.AppendLine($"{csvText}");
               }
            }

         }

         // hand waving
         // summary.AppendLine("** Consider adding application support for BeeHD MEDIA STATISTICS API stream reporting.");

         return summary;
      }


      /// <summary>
      /// Returns a table of the events summarized into N-second time buckets, or null if the table has not yet been constructed.
      /// </summary>
      public DataTable TimeSeriesTable
      {
         get
         {
            return this.timeSeries?.GetFinalizedTimeSeriesTable();
         }
      }

      /// <summary>
      /// The local UDP port for RTCP control of audio streaming.
      /// </summary>
      public string LocalAudioControlPort { get { return this.incomingAudioStream.ControlPort; } }

      /// <summary>
      /// The local UDP port to receive audio streaming.
      /// </summary>
      public string LocalAudioStreamingPort { get { return this.incomingAudioStream.Port; } }

      /// <summary>
      /// The local UDP port for RTCP control of video streaming.
      /// </summary>
      public string LocalVideoControlPort { get { return this.incomingVideoStream.ControlPort; } }

      /// <summary>
      /// The local UDP port to receive video streaming.
      /// </summary>
      public string LocalVideoStreamingPort { get { return this.incomingVideoStream.Port; } }

      /// <summary>
      /// The remote UDP port for RTCP control of audio streaming.
      /// </summary>
      public string RemoteAudioControlPort { get { return this.outgoingAudioStream.ControlPort; } }

      /// <summary>
      /// The remote UDP port to send audio streaming.
      /// </summary>
      public string RemoteAudioStreamingPort { get { return this.outgoingAudioStream.Port; } }

      /// <summary>
      /// The remote UDP port for RTCP control of video streaming.
      /// </summary>
      public string RemoteVideoControlPort { get { return this.outgoingVideoStream.ControlPort; } }

      /// <summary>
      /// The remote UDP port to send video streaming.
      /// </summary>
      public string RemoteVideoStreamingPort { get { return this.outgoingVideoStream.Port; } }


      /// <summary>
      /// Gets the local machine IP address.
      /// </summary>
      public string LocalEndpointAddress {  get {  return this.incomingAudioStream.EndpointAddress; } }

      /// <summary>
      /// Gets the remote endpoint IP address.
      /// </summary>
      public string RemoteEndpointAddress {  get {  return this.outgoingAudioStream.EndpointAddress; } }

      /// <summary>
      /// Set the value of a SIP timer.
      /// </summary>
      /// <param name="timerName">T1, T2, T4</param>
      /// <param name="valueMsec"></param>
      /// <exception cref="ArgumentException"></exception>
      public void SetTimerValue( string timerName, int valueMsec)
      {
         switch (timerName.ToUpper())
         {
            case "TIMER1":
            case "TIMER 1":
            case "T1":                                    // Round-trip time (RTT) estimate, used for retransmission timing
               if (valueMsec == 0)
               {
                  timer1Msec = remoteUnreachableTimeoutMsec / 64;
               }
               timer1Msec = valueMsec;
               break;

            case "TIMER2":                                // Maximum retransmission interval for non-INVITE requests and INVITE responses
            case "TIMER 2":
            case "T2":
               timer2Msec = valueMsec;
               break;

            case "TIMER4":                                // Maximum retransmission interval for non-INVITE requests and INVITE responses
            case "TIMER 4":
            case "T4":
               timer4Msec = valueMsec;
               break;

            case "TIMERC":                                // proxy INVITE transaction timeout (1xx provisional responses)
            case "TIMER C":
            case "TC":
               timerCMsec = valueMsec;
               break;

            default:
               throw new ArgumentException(nameof(timerName));
         }

         if (valueMsec <= 0)
         {
            throw new ArgumentException(nameof(valueMsec));
         }

         // recalculate timers whose values are dependent on other timers
         timerAMsec = timer1Msec;        // INVITE request retransmission interval, for UDP only
         timerBMsec = 64 * timer1Msec;   // INVITE transaction timeout
         timerEMsec = timer1Msec;        // Non-INVITE request retransmission interval, UDP only
         timerFMsec = 64 * timer1Msec;   // Maximum time for Non-INVITE transaction
         timerGMsec = timer1Msec;        // INVITE response retransmission interval
         timerKMsec = timer4Msec;        // Maximum time for response retransmissions

         if (this.sipMessages[0].PacketType.ToLower() == "udp")
         {
            timerHMsec = 64 * timer1Msec; // Maximum wait time for ACK receipt               (0 sec not applicable for TCP)
            timerIMsec = timer4Msec;      // Maximum wait time for ACK retransmissions       (0 sec not applicable for TCP)
            timerJMsec = 64 * timer1Msec; // Maximum wait time for retransmissions of non-INVITE requests  (0 sec not applicable for TCP)
         }
         else
         {
            timerHMsec = 0;
            timerIMsec = 0;
            timerJMsec = 0;
         }
      }

      /// <summary>
      /// A long timeout for declaring the remote party is unresponsive.
      /// </summary>
      public int RemoteUnreachableTimeoutMsec {  get {  return remoteUnreachableTimeoutMsec; } }

      /// <summary>
      /// Round-trip time (RTT) estimate, used for retransmission timing.
      /// </summary>
      public int RoundTripTimeMsec_Timer1 { get { return timer1Msec; } }

      /// <summary>
      /// Maximum retransmission interval for non-INVITE requests and INVITE responses
      /// </summary>
      public int MaxRetransmissionIntervalMsec_NonInviteRequest_InviteResponse { get { return timer2Msec; } }

      /// <summary>
      /// Maximum duration that a message can remain in the network
      /// </summary>
      public int MaxMessageDurationMsec { get { return timer4Msec; } }

      /// <summary>
      /// INVITE request retransmission interval, for UDP only
      /// </summary>
      public int InviteRequestRetransmissionIntervalMsec_Udp { get { return timerAMsec; } }

      /// <summary>
      /// INVITE transaction timeout
      /// </summary>
      public int InviteTransactionTimeoutMsec { get { return timerBMsec; } }

      /// <summary>
      /// proxy INVITE transaction timeout (1xx provisional responses)
      /// </summary>
      public int ProvisionalResponseTimeoutMsec {  get { return timerCMsec; } }

      /// <summary>
      /// Maximum time for response retransmissions  (0 sec not applicable for TCP)
      /// </summary>
      public int MaximumResponseRetransmissionWaitMsec_Udp { get { return timerDMsec; } }

      /// <summary>
      /// Non-INVITE request retransmission interval, UDP only
      /// </summary>
      public int NonInviteRequestRetransmissionIntervalMsec_Udp { get { return timerEMsec; } }

      /// <summary>
      /// Maximum time for Non-INVITE transaction
      /// </summary>
      public int NonInviteTransactionTimeoutMsec { get { return timerFMsec; } }

      /// <summary>
      /// INVITE response retransmission interval
      /// </summary>
      public int InviteResponseRetransmissionIntervalMsec_Udp { get { return timerGMsec; } }

      /// <summary>
      /// Maximum wait time for ACK receipt (not applicable for TCP)
      /// </summary>
      public int AckReceiptWaitTimeMsec_Udp { get { return timerHMsec; } }

      /// <summary>
      /// Maximum time for ACK retransmissions (not applicable for TCP)
      /// </summary>
      public int AckRetransmissionsTimeout_Udp { get { return timerIMsec; } }

      /// <summary>
      /// Maximum time for retransmissions of non-INVITE requests (not applicable for TCP)
      /// </summary>
      public int NonInviteRequestRetransmissionsTimeoutMsec_Udp {  get { return timerJMsec; } }

      /// <summary>
      /// Maximum time for response retransmissions
      /// </summary>
      public int MaximumResponseRetransmissionWaitMsec { get { return timerKMsec; } }
   }
}
