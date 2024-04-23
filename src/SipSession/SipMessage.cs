using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SipSession
{

   /// <summary>
   /// A class to parse SIP messages.  It is not a complete SIP message parser, just handles the most common cases as seen in the
   /// RVBEEHD logs.
   /// </summary>
   public class SipMessage
   {
      private List<string> messageLines = new List<string>();
      private string timestamp = string.Empty;
      private string timeoutTimestamp = string.Empty;

      private SdpMessage sdpMessage;
      private MessageDirection direction = MessageDirection.Undefined;
      private bool retransmission = false;
      private string messageType = string.Empty;
      private string headerType = string.Empty;
      private string responseCode = string.Empty;
      private string contact = string.Empty;
      private int cseqValue = 0;
      private string callId = string.Empty;
      private string machineIpAddress = string.Empty;
      private string remoteIpAddress = string.Empty;
      private string sourceIpAddress = string.Empty;
      private string rportOption = string.Empty;
      private string packetType = string.Empty;
      private string transactionId = string.Empty;
      private int mimeContentLength = -1;
      private bool sipSectionComplete = false;
      private string note = string.Empty;
      private string expect = string.Empty;


      public enum MessageDirection { Undefined, In, Out };


      /// <summary>
      ///  INVITE sip:x.x.x.x SIP/2.0
      /// </summary>
      /// <param name="firstHeaderLine">normally an INVITE</param>
      /// <param name="direction">in or out</param>
      /// <param name="timestamp"></param>
      public SipMessage(string firstHeaderLine, string direction, string timestamp)
      {
         this.timestamp = timestamp;

         if (direction == "in" || direction == "received")
         {
            this.direction = MessageDirection.In;
         }
         else if (direction == "out" || direction == "sent")
         {
            this.direction = MessageDirection.Out;
         }

         // first line contains the message type
         //   INVITE sip:10.255.254.111 SIP/2.0
         //   180 RINGING SIP/2.0
         //   200 OK SIP/2.0
         //   ACK sip:10.255.254.111 SIP/2.0
         //   BYE sip:10.255.254.111 SIP/2.0
         //   BYE sip:192.168.53.235@192.168.53.235 SIP/2.0
         //   200 OK SIP/2.0
         //   INFO sip:10.100.17.129 SIP/2.0

         /*
               INFO sip:10.100.17.129 SIP/2.0
               From: "10.172.11.54"<sip:10.172.11.54@10.172.11.54>;tag=413970ab-16d17958-7bf7-28a14130-360bac0a-13c4-65015
               To: <sip:10.100.17.129>
               Call-ID: 3df8338f-52e0c535-7bf7-28aa0100-360bac0a-13c4-65015
               CSeq: 2 INFO
               Via: SIP/2.0/UDP 10.172.11.54:5060;rport;branch=z9hG4bK-6f089990-7c06-1e47ae2-24792300
               Max-Forwards: 70
               Supported: replaces,timer,100rel
               Agent: BEEHD 4.7.24
               Allow: INVITE, ACK, BYE, REFER, NOTIFY, INFO, CANCEL, UPDATE, PRACK
               User-Agent: Hyosung WIndows 4.7.24.0
               Contact: <sip:10.172.11.54@10.172.11.54>
               Content-Type: application/media_control+xml
               Content-Length: 168

               <?xml version="1.0" encoding="utf-8" ?> <media_control><vc_primitive><to_encoder><picture_fast_update></picture_fast_update></to_encoder></vc_primitive></media_control>
          */


         /*
            SIP/2.0 481 Call Leg/Transaction Does Not Exist
            From: "10.50.208.10"<sip:10.50.208.10@10.50.208.10>;tag=7ea44101-20673abf-bb9b-1f209130-ad0320a-13c4-65015
            To: <sip:10.20.40.106>;tag=5ed0f0ba-130e6a84-4e12e-1bb2b130-6a28140a-13c4-65015
            Call-ID: 62118d12-a26f333-bb9b-1b26fc58-ad0320a-13c4-65015
            CSeq: 1 CANCEL
            Contact: <sip:jsmith@0.0.0.0>
            Via: SIP/2.0/UDP 10.50.208.10:5060;rport=5060;branch=z9hG4bK-705a9bdc-bb9b-2dcd69b-1f17c148
            Supported: replaces,timer,100rel
            Agent: BEEHD 4.7.24
            Content-Length: 0
         */


         string[] parts = firstHeaderLine.Replace("SIP/2.0", string.Empty).Trim().Split(' ');

         // first line contains the message type
         //   INVITE sip:10.255.254.111
         //   180 RINGING
         //   200 OK
         //   ACK sip:10.255.254.111
         //   BYE sip:10.255.254.111


         // handle unexpected error responses.  1xx and 2xx are expected, so start with 3xx
         bool isErrorResponse = false;

          // a SIP error response (3xx, 4xx, 5xx)
         // 481 Call Leg/Transaction Does Not Exist

         int errorResponse = 0;

         if (int.TryParse(parts[0], out errorResponse))
         {
            isErrorResponse = errorResponse >= 300 && errorResponse < 600;

            // reassemble the parts to force parts.Length==2

            parts = new string[] { parts[0], firstHeaderLine.Replace($"SIP/2.0 {parts[0]} ", string.Empty) };
         }


         if (parts.Length == 2)
         {
            this.headerType = "SIP";

            if (parts[0] == "INVITE" || parts[0] == "ACK" || parts[0] == "BYE" || parts[0] == "INFO" || parts[0] == "OPTIONS" || parts[0] == "CANCEL")
            {
               this.messageType = parts[0].ToUpper();

               if (this.direction == MessageDirection.In)
               {
                  this.machineIpAddress = parts[1].Replace("sip:", string.Empty);
               }

               else
               {
                  this.remoteIpAddress = parts[1].Replace("sip:", string.Empty);
               }

               if (this.messageType == "INVITE")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received INVITE or re-INVITE";
                     this.expect = "respond with 180 RINGING and 200 OK";
                  }
                  else
                  {
                     this.note = "sent INVITE or re-INVITE";
                     this.expect = "expect 180 RINGING and 200 OK responses";
                  }
               }

               else if (this.messageType == "BYE")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received BYE";
                     this.expect = "respond with 200 OK and ACK, remote side may not send an ACK reply";
                  }
                  else
                  {
                     this.note = "sent BYE";
                     this.expect = "expect 200 OK";
                  }
               }

               else if (this.messageType == "ACK")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received ACK";
                     this.expect = "handshake complete";
                  }
                  else
                  {
                     this.note = "sent ACK";
                     this.expect = "handshake complete";
                  }
               }

               else if (this.messageType == "INFO")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received INFO";
                     this.expect = string.Empty;
                  }
                  else
                  {
                     this.note = "sent INFO";
                     this.expect = string.Empty;
                  }
               }

               else if (this.messageType == "OPTIONS")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received OPTIONS";
                     this.expect = string.Empty;
                  }
                  else
                  {
                     this.note = "sent OPTIONS";
                     this.expect = string.Empty;
                  }
               }

               else if (this.messageType == "CANCEL")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received CANCEL";
                     this.expect = string.Empty;
                  }
                  else
                  {
                     this.note = "sent CANCEL";
                     this.expect = string.Empty;
                  }
               }
            }

            else
            {
               this.responseCode = parts[0];
               this.messageType = parts[1].ToUpper();

               // 100 TRYING from a proxy server is not supported, if received it is supposed to stop INVITE retransmissions

               // https://www.tutorialspoint.com/session_initiation_protocol/session_initiation_protocol_basic_call_flow.htm

               if (this.responseCode == "180" && this.messageType == "RINGING")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received 180 RINGING from the remote party";
                     this.expect = "expect an additional 200 OK when the connection is accepted";
                  }
                  else
                  {
                     this.note = "sent 180 RINGING to the remote party";
                     this.expect = "send 200 OK when the connection is accepted";
                  }
               }

               else if (this.responseCode == "200" && this.messageType == "OK")
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = "received 200 OK from the remote party";
                     this.expect = "respond with ACK";
                  }
                  else
                  {
                     this.note = "sent 200 OK to the remote party";
                     this.expect = "expect ACK response";
                  }
               }

               else
               {
                  if (this.direction == MessageDirection.In)
                  {
                     this.note = $"received error {this.responseCode} from the remote party";
                  }
                  else
                  {
                     this.note = $"sent error {this.responseCode} to the remote party";
                  }
               }
            }

            // append the optional SipSessionNote
            if (this.SipSessionNote != null && this.SipSessionNote != string.Empty)
            {
               this.note = $"{this.note} [ {this.SipSessionNote} ]";
            }
         }

         else if (parts.Length > 5)
         {
            // must be an all-in-one SDP

            this.headerType = "SDP";
            this.messageType = "INVITE";
            this.note = "One-line SDP";
            this.expect = string.Empty;
         }
      }

      public void AppendHeader(string line, string timestamp)
      {
         this.messageLines.Add(line);

         /*
            From: "192.168.53.235"<sip:192.168.53.235@192.168.53.235>;tag=7563498c-4ada37ee-69e4-25b9fd60-eb35a8c0-13c4-65015
            To: <sip:10.255.254.111>
            Call-ID: 548fe579-4f8bf90e-69e4-2234c4e8-eb35a8c0-13c4-65015
            CSeq: 1 INVITE
            Via: SIP/2.0/UDP 192.168.53.235:5060;rport;branch=z9hG4bK-4dcebadf-69e4-19da4fb-25b100c0
            Max-Forwards: 70
            Supported: replaces,timer,100rel
            Agent: BEEHD 4.7.24
            Allow: INVITE, ACK, BYE, REFER, NOTIFY, INFO, CANCEL, UPDATE, PRACK
            User-Agent: Hyosung WIndows 4.7.24.0
            Contact: <sip:192.168.53.235@192.168.53.235>
            Session-Expires: 1800;refresher=uac
            Min-SE: 90
            Content-Type: application/sdp
            Content-Length: 877
            10/16/23 10:32:26.472 TRANSPORT   : INFO  - 
            v=0
            o=192.168.53.235 3893448817 3893448817 IN IP4 192.168.53.235
            s=-
            c=IN IP4 192.168.53.235
            b=AS:1920
            t=0 0
            m=audio 3278 RTP/AVP 9 103 101
            c=IN IP4 192.168.53.235
            b=AS:64
            a=ptime:20
            a=rtcp:3279 IN IP4 192.168.53.235
            a=rtpmap:9 G722/8000
            a=fmtp:9 bitrate=64000
            a=rtpmap:103 telephone-event/16000
            a=fmtp:103 0-15
            a=rtpmap:101 telephone-event/8000
            a=fmtp:101 0-15
            m=video 3280 RTP/AVP 102 115 116
            c=IN IP4 192.168.53.235
            b=TIAS:1920000
            a=rtcp:3281 IN IP4 192.168.53.235
            a=rtcp-fb:* nack pli
            a=rtcp-fb:* ccm fir
            a=rtcp-fb:* ccm tmmbr
            a=rtpmap:102 H264/90000
            a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270
            a=rtpmap:115 ulpfec/90000
            a=fmtp:115 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8
            a=rtpmap:116 avcfec/90000
            a=fmtp:116 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8
         */

         // identify lines of interest
         // SIP headers first, followed by MIME content descriptor, followed by SDP

         // item: value (SIP or MIME)
         // item-item: value
         // item=value  (SDP)
         Regex regex = new Regex("^(?<type>[\\w'-]+)(?<separator>[:=])(?<value>.*)$");
         Match m = regex.Match(line);
         if (m.Success)
         {
            string item = m.Groups["type"].Value;
            string lItem = item.ToLower();
            string separator = m.Groups["separator"].Value;
            string value = m.Groups["value"].Value.Trim();

            switch (separator)
            {
               case ":":
                  // SIP or MIME

                  if (lItem == "content-length")
                  {
                     this.headerType = "MIME";

                     // number of characters in the SDP payload including CRLF for each line
                     this.mimeContentLength = Convert.ToInt16(value);
                     if (this.mimeContentLength <= 0)
                     {
                        this.sipSectionComplete = true;
                     }
                  }
                  else if (lItem == "content-type")
                  {
                     this.headerType = "MIME";

                     // MIME section begins
                     // this header may be missing if Content-length: 0
                     this.sipSectionComplete = true;
                  }
                  else
                  {
                     this.headerType = "SIP";

                     switch (lItem)
                     {
                        // CSeq: nn tracking
                        //
                        // Message sequences are identified by a common value in the CSeq: field.  The sender starts
                        // with 1 and increments for each new sequence.  Responses from the receiver include a copy
                        // of the same CSeq: line.
                        //
                        // Each sequence contains new information, for example new SDP payloads or new fields in the SIP
                        // header.
                        //
                        // Messages that expect a response but don't get one before a timeout, are retransmitted.  The
                        // receiver typically will send the expected responses each time it receives a request.
                        //
                        // Typical sequences are:
                        //   INVITE - 180 RINGING - 200 OK - ACK
                        //   BYE - 200 OK - ACK
                        //
                        case "cseq":
                           // 1 ACK
                           // 1 INVITE
                           string[] parts = value.Trim().Split(' ');

                           this.cseqValue = int.Parse(parts[0]);
                           break;

                        // Call-ID: nn tracking
                        //  - every party to a conversationl has a unique call ID, generally a GUID
                        //  - the caller and the callee have their own unique call ID values
                        case "call-id":
                           this.callId = value;
                           break;

                        case "contact":
                           // Contact: <sip:192.168.53.235@192.168.53.235>
                           // ACK is sent to the address in Record-Route (inserted by intermediate proxies)
                           // or Contact (inserted by the remote endpoint itself) but which can be modified by intermediate proxies
                           // including NAT gateways (ALG) which are supposed to replace a private network IP address that the remote
                           // endpoint knows about, with the public IP address of a NAT router.
                           // https://blog.opensips.org/2017/02/22/troubleshooting-missing-ack-in-sip/
                           this.contact = value;
                           break;

                        case "via":
                           // callee send responses back to caller by determining the information in the Via header, which is created by the caller, when composing the initial requests, e.g: INVITE.
                           // This manner is standard, and defined in rfc3261, also works in the lab architechture. But in real life, when most of the endpoints lie behind a NAT, they would usually identify themselve with their private adresses, which are not usable in above standard compliment.
                           // by requesting a call to a public server outside of my network, that URI is absolutely not reachable by the server. Hence, responses never traverse back to the caller properly.
                           // You see the problem? Great, then we go to the solutions.
                           //
                           // “received” is a standard parameter in the Via header, which contains the actual source address from which the packet was received.
                           // “received” parameter is generated if the IP in Via header differs from the packet source address.
                           //
                           // “rport” aka response-port is actually an analogous to the “received” parameter, except “rport” contains a port, not an IP address, defined in rfc3581.
                           // When a UAC generates a request, it may contain an empty “rport” parameter in the Via header.
                           // When a UAS receives a request, it examines the topmost Via header for checking the existence of the “rport” parameter with no value, if there is one, it MUST set the value of the parameter to the source port of the request.
                           // Presence of “rport” parameter also overwrite the rule for generating “received” parameter: UAS must insert a “received” parameter containing the source IP address that the request came from, even if it is identical to the value specified in Via header.

                           // Via: SIP/2.0/UDP 10.47.40.1:5060;rport;branch=z9hG4bK-2bbcda0f-b0ba-2b25967-1c4d4148
                           // Via: SIP/2.0/UDP 10.47.40.1:5060;rport=5060;branch=z9hG4bK-2bbcda0f-b0ba-2b25967-1c4d4148

                           string[] subparts = value.Split(';');
                           regex = new Regex("^SIP/2.0/(?<packettype>.*) (?<address>.*:[0-9]*)$");
                           m = regex.Match(subparts[0]);
                           if (m.Success)
                           {
                              this.packetType = m.Groups["packettype"].Value;
                              this.remoteIpAddress = m.Groups["address"].Value;
                           }

                           foreach (string subpart in subparts)
                           {
                              if (subpart.StartsWith("rport"))
                              {
                                 this.rportOption = subpart;
                              }
                              else if (subpart.StartsWith("received"))
                              {
                                 this.sourceIpAddress = subpart.Substring(subpart.IndexOf("=") + 1);
                              }
                              else if (subpart.StartsWith("branch"))
                              {
                                 this.transactionId = subpart.Substring(subpart.IndexOf("=") + 1);
                              }
                           }
                           break;

                        case "from":
                           // From: "\"10.47.40.1\"<sip:10.47.40.1@10.47.40.1>;tag=14a2a93c-1a1d95de-b0ba-1e7b2130-1282f0a-13c4-65015"
                           regex = new Regex("^(?<name>.*)<sip:(?<address>.*)>;(?<tags>.*)$");
                           m = regex.Match(value);
                           if (m.Success)
                           {
                              this.remoteIpAddress = m.Groups["address"].Value;

                              string n = m.Groups["name"].Value;
                              string a = m.Groups["address"].Value;
                              string t = m.Groups["tags"].Value;
                           }

                           break;

                        default:
                           break;
                     }
                  }
                  break;

               case "=":

                  this.headerType = "SDP";

                  if (this.sdpMessage == null)
                  {
                     this.sdpMessage = new SdpMessage(line, this.mimeContentLength);

                     // this message should have a Call-Id by now.  If it doesn't then most likely no SIP content was logged
                     // and only SDP content.  Use the SDP Session Identifier as the Call-Id.

                     if (this.callId == string.Empty)
                     {
                        this.callId = "NotAvailable";

                        // similarly with local machineIpAddress and remoteIpAddress, message type, etc
                        if (this.direction == MessageDirection.Out)
                        {
                           this.machineIpAddress = this.sdpMessage.AudioIpAddress;
                        }

                        if (this.direction == MessageDirection.In)
                        {
                           this.remoteIpAddress = this.sdpMessage.AudioIpAddress;
                        }

                        this.messageType = "INVITE";   // or 200 OK
                     }
                  }
                  else
                  {
                     this.sdpMessage.AppendHeader(line);
                  }
                  break;

               default:
                  break;
            }
         }
      }

      
      /// <summary>
      /// A short summary of the message.
      /// </summary>
      public string ShortSummary
      {
         get
         {
            // msgtype N (Cseq value)
            // address --> address
            // in-out
            // machineaddress, remote address

            StringBuilder summary = new StringBuilder();

            if (this.responseCode != string.Empty)
            {
               summary.Append( this.responseCode + " " );
            }

            summary.Append((this.direction == MessageDirection.In ? "received" : "sent") + " ");
            summary.Append(this.messageType + " ");
            summary.Append(this.cseqValue.ToString() + " ");
            summary.Append(" to " + this.remoteIpAddress);

            return summary.ToString();
         }
      }

      /// <summary>
      /// A summary about the two endpoints and message direction.
      /// </summary>
      public string EndpointsSummary
      {
         get
         {
            string summary = this.machineIpAddress + " " +
               (this.direction == MessageDirection.In ? "<--" : "-->") + " " +
               this.remoteIpAddress + " (" + this.packetType + ")";
            return summary;
         }
      }

      /// <summary>
      /// Performs a comparison of the SIP message.
      /// </summary>
      /// <param name="msg">A message to compare with.</param>
      /// <returns>True if the messages are the same</returns>
      public bool IsIdentical(SipMessage msg)
      {
         if (this.MessageType == msg.MessageType && this.Cseq == msg.Cseq)
         {
            return true;
         }

         return false;
      }


      /// <summary>
      /// The header type for the most recently added line.
      /// </summary>
      public string LastHeaderType { get { return this.headerType; } }

      /// <summary>
      /// The message lines.
      /// </summary>
      public List<string> MessageLines {  get {  return messageLines; } }  
      
      /// <summary>
      /// Gets the size of the message in bytes.
      /// </summary>
      public int MessageSize
      {
         get
         {
            int size = 0;
            foreach (string msg in this.messageLines)
            {
               size += msg.Length + 2;   // include cr+lf
            }

            return size;
         }
      }

      /// <summary>
      /// The timestamp.
      /// </summary>
      public string Timestamp {  get { return this.timestamp; } }

      /// <summary>
      /// A note describing the message.
      /// </summary>
      public string Note { get { return this.note; } }

      /// <summary>
      /// A note describing the expected response.
      /// </summary>
      public string Expect { get { return this.expect; } }

      /// <summary>
      /// A note inserted externally by the SipSession, for example to describe the connection duration
      /// or how long it has been since the previous call ended.
      /// </summary>
      public string SipSessionNote { get; set;  }

      /// <summary>
      /// A value indicating whether the entire SIP message and SDP payload has been received correctly.
      /// </summary>
      public bool SipAndSdpPayloadComplete
      {
         get
         {
            // normal case, when SIP and SDP headers were all provided
            // or when there is no SDP payload when MIME Content-Length: 0
            if (this.sipSectionComplete)
            {
               if (this.SdpSectionComplete)
               {
                  return true;
               }
            }

            else if (this.SdpSectionComplete)
            {
               return true;
            }

            return false;
         }
      }

      /// <summary>
      /// The CSeq value.
      /// </summary>
      public int Cseq { get { return this.cseqValue; } }

      /// <summary>
      /// Gets or sets a value indicating whether the message is a retransmission.
      /// </summary>
      public bool IsRetransmission
      {
         get
         {
            return this.retransmission;
         }
         
         set
         {
            this.retransmission = value;
         }
      }

      /// <summary>
      /// The Call Id.
      /// </summary>
      public string CallId { get { return this.callId; } }

      /// <summary>
      /// The Transaction Id.
      /// </summary>
      /// <remarks>A transaction is a series of related messages with ascending CSeq values.</remarks>
      public string TransactionId { get { return this.transactionId; } }

      /// <summary>
      /// The contact address.
      /// </summary>
      /// <remarks>
      /// SCK is sent to the address in Record-Route (inserted by intermediate proxies)
      /// or Contact (inserted by the remote endpoint itself) but which can be modified by intermediate proxies
      /// including NAT gateways (ALG) which are supposed to replace a private network IP address that the remote
      /// endpoint knows about, with the public IP address of a NAT router.
      /// https://blog.opensips.org/2017/02/22/troubleshooting-missing-ack-in-sip/
      /// </remarks>
      public string Contact { get { return this.contact; } }

      /// <summary>
      /// The SIP response code if any (1xx, 2xx, 3xx, 4xx, 5xx).
      /// </summary>
      public string ResponseCode { get { return this.responseCode; } }

      /// <summary>
      /// The type of message.
      /// </summary>
      public string MessageType { get { return this.messageType; } }

      /// <summary>
      /// The source address from which a message packet was received.
      /// </summary>
      public string SourceIpAddress {  get { return this.sourceIpAddress; } } 

      /// <summary>
      /// The IP Address for the machine receiving messages.
      /// </summary>
      public string LocalIpAddress { get { return this.machineIpAddress; } }

      /// <summary>
      /// The IP address for the source or destination of messages.
      /// </summary>
      public string RemoteIpAddress { get { return this.remoteIpAddress; } }

      /// <summary>
      /// The type of network packet used for SIP messages.
      /// </summary>
      public string PacketType {  get { return this.packetType; } }

      /// <summary>
      /// The rport option used when routing SIP messages.
      /// </summary>
      public string RportOption {  get { return this.rportOption; } }

      /// <summary>
      /// A value indicating the direction of the message.
      /// </summary>
      public MessageDirection Direction { get { return this.direction; } }


      /// <summary>
      /// A value indicating whether all of the SDP headers have been received correctly.  If there are no SDP headers
      /// expected or if they were provided all-in-one-line, returns true.
      /// </summary>
      public bool SdpSectionComplete
      {
         get
         {
            if (this.sdpMessage != null)
            {
               return this.sdpMessage.AllSdpLinesReceived;
            }

            // if the SIP message doesn't have an SDP payload
            if (this.mimeContentLength == 0)
            {
               return true;
            }

            return false;
         }
      }

      /// <summary>
      /// The originator of the message, it might be an IP address or a session name from the o= line of the SDP headers.
      /// </summary>
      public string SessionOriginator
      { 
         get
         {
            if (this.sdpMessage == null)
            {
               return string.Empty;
            }

            return this.sdpMessage.SessionOriginator;
         }
      }

      /// <summary>
      /// A value indicating the requested or offered audio streaming state.
      /// </summary>
      public string AudioState
      {
         get
         {
            // if the SIP message doesn't have an SDP payload
            if (this.sdpMessage == null)
            {
               return string.Empty;
            }

            return this.sdpMessage.AudioStreamState.ToString();
         }
      }

      /// <summary>
      /// A value indicating the requested or offered video streaming state.
      /// </summary>
      public string VideoState
      {
         get 
         {
            // if the SIP message doesn't have an SDP payload
            if (this.sdpMessage == null)
            {
               return string.Empty;
            }

            return this.sdpMessage.VideoStreamState.ToString(); 
         }
      }

      public string AudioControlPort { get { return this.sdpMessage?.AudioControlPort; } }
      public string AudioStreamingPort { get { return this.sdpMessage?.AudioPort; } }
      public string VideoControlPort { get { return this.sdpMessage?.VideoControlPort; } }
      public string VideoStreamingPort { get { return this.sdpMessage?.VideoPort; } }
   }
}
