using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipSession
{
   internal class SdpMessage
   {
      private enum SectionNames { general, audio, video };

      private List<string> messageLines = new List<string>();
      private int targetContentLength = 0;
      private int contentLength = 0;

      SectionNames section = SectionNames.general;
      private string audioIpAddress = string.Empty;
      private string audioPort = string.Empty;
      private string audioControlPort = string.Empty;
      private StreamState audioStreamState = StreamState.none;

      private string videoIpAddress = string.Empty;
      private string videoPort = string.Empty;
      private string videoControlPort = string.Empty;
      private StreamState videoStreamState = StreamState.none;

      private string origin = string.Empty;
      private string sessionId = string.Empty;
      private string sessionVersion = string.Empty;

      public enum StreamState { none, sendonly, recvonly, sendrecv, other };

      /// <summary>
      ///  INVITE sip:x.x.x.x SIP/2.0
      /// </summary>
      /// <param name="firstHeaderLine">normally an INVITE</param>
      /// <param name="direction">in or out</param>
      /// <param name="timestamp"></param>
      public SdpMessage(string firstHeaderLine, int contentLength)
      {
         this.targetContentLength = contentLength;
         this.messageLines.Add(firstHeaderLine);

         // detect whether there are multiple parameters defined, SDP all-on-one-line.  There will be two (eg: a=value) for single parameter on a line, or as many as 25 or more
         // regular SDP has a limited number, eg:
         //a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270
         //a=fmtp:115 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8
         //a=fmtp:116 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8
         string[] tokens = firstHeaderLine.Split(new char[] { '=' });

         if (tokens.Length > 10)
         {
            if (this.targetContentLength == 0)
            {
               this.targetContentLength = firstHeaderLine.Length;
            }

            this.contentLength = firstHeaderLine.Length;

            this.ProcessAllOnOneLine(firstHeaderLine);
         }
         else
         {
            // includes crlf which ends each line of the message payload
            this.contentLength = firstHeaderLine.Length + 2;
            this.ProcessLine(firstHeaderLine);
         }
      }

      public void AppendHeader(string nextHeaderLine)
      {
         string[] tokens = nextHeaderLine.Split(new char[] { '=' });

         // all-in-one SDP has a lot of = delimiters, perhaps 25 but certainly more than 10
         // regular SDP has a limited number, eg:
         //a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270
         //a=fmtp:115 protectedPayload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8
         //a=fmtp:116 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8

         if (tokens.Length > 10)
         {
            this.contentLength = nextHeaderLine.Length;

            this.ProcessAllOnOneLine(nextHeaderLine);

            if (this.targetContentLength < 0)
            {
               this.targetContentLength = this.contentLength;
            }
         }

         else
         {
            // try to fix an obvious corruption like this seen in one log, no other examples have been identified:
            //a=ptime:20a=sendrecv

            if (nextHeaderLine == "a=ptime:20a=sendrecv")
            {
               // a general algorithm that could be adapted, if other instances are found

               string testLine = nextHeaderLine;
               int idx = testLine.IndexOf("a=");
               int previdx = 0;
               List<string> subLines = new List<string>();

               while (idx != -1)
               {
                  if (idx > 0)
                  {
                     subLines.Add(testLine.Substring(previdx, (idx-previdx)));
                  }

                  previdx = idx;

                  idx = testLine.IndexOf("a=", idx+1);
               }

               if (previdx < testLine.Length)
               {
                  subLines.Add(testLine.Substring(previdx));
               }

               foreach (string kludge in subLines)
               {
                  this.messageLines.Add(kludge);
                  this.contentLength += kludge.Length + 2;
                  this.ProcessLine(kludge);
               }
            }

            else
            {
               // normal processing

               this.messageLines.Add(nextHeaderLine);
               this.contentLength += nextHeaderLine.Length + 2;
               this.ProcessLine(nextHeaderLine);
            }
         }
      }

      /// <summary>
      /// process SDP which is all on one line
      /// </summary>
      /// <param name="line"></param>
      /// <remarks>some versions of rvbeehd.log deliver the all-in-one SDP on multiple lines without necessarily playing nice and splitting
      /// the SDP payload on x=value boundaries!  This can cause exceptions in ProcessLine() when the size of the subTokens array is smaller
      /// than expected.   Example:                       this.audioPort = subparts[1];  but subparts.Length=1</remarks>
      private void ProcessAllOnOneLine( string line )
      {
         // v=0o=192.168.4.24 3509267301 3509267301 IN IP4 192.168.4.24s=-c=IN IP4 192.168.4.24b=AS:768t=0 0m=audio 3270 RTP/AVP 9 103 101b=AS:64a=rtpmap:9 G722/8000a=fmtp:9 bitrate=64000a=rtpmap:103 telephone-event/16000a=fmtp:103 0-15a=rtpmap:101 telephone-event/8000a=fmtp:101 0-15a=ptime:20m=video 3280 RTP/AVP 102 111 114b=TIAS:768000a=content:maina=rtpmap:102 H264/90000a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270a=rtpmap:111 ulpfec/90000a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgoa=rtpmap:114 avcfec/90000a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=0a=rtcp-fb:* nack plia=rtcp-fb:* ccm tmmbra=rtcp-fb:* ccm fir

         // find all the delimiters
         int start = line.IndexOf('=') - 1;     // points to the first parameter name/letter
         int length = (start >= 0) ? 2 : 0;          // accumulated length of the parameter

         List<string> sdpParams = new List<string>();

         if (length > 0)
         {
            for (int i = start + 2; i <= line.Length; i++)
            {
               if (i == line.Length)
               {
                  // end of string
                  string sdpParam = line.Substring(start);

                  sdpParams.Add(sdpParam);
               }
               else
               {
                  if (line[i] == '=')
                  {
                     length = i - start - 1;

                     string sdpParam = line.Substring(start, length);

                     if (sdpParam.Contains("fmtp"))
                     {
                        // contains subparameters possibly including = delimiter.  Continue until the next a=
                        // a=fmtp:103 0-15
                        // a=fmtp:9 bitrate=64000
                        // a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270
                        // a=fmtp:111 protectedPayload=102;RvXorAlgo;RvRsAlgo
                        // a=fmtp:114 protectedPayload=102;AllowMultiFrameGroups=0

                        if (line[i - 1] == 'a' || line[i-1] == 'm')
                        {
                           sdpParams.Add(sdpParam);

                           start = i - 1;
                           length = 2;
                        }
                     }
                     else
                     {
                        sdpParams.Add(sdpParam);

                        start = i - 1;
                        length = 2;
                     }
                  }
               }
            }
         }

         foreach (string param in sdpParams)
         {
            this.ProcessLine(param);
         }
      }

      private void ProcessLine( string line )
      {
         /*
           v=0
           o=192.168.53.235 3893448817 3893448817 IN IP4 192.168.53.235
    ..or..         o=dsantoyo 3374776509 3374776509 IN IP4 10.255.254.111
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

         // endpoint identification section, followed by (m=audio) section, followed by (m=video) section

         // NOTE: audio headers may be received prior to the m=audio line, perhaps it should be default?

         if (line.StartsWith("m=audio"))
         {
            section = SectionNames.audio;
         }
         else if (line.StartsWith("m=video"))
         {
            section = SectionNames.video;
         }

         // o=192.168.53.235 3893448817 3893448817 IN IP4 192.168.53.235
         // m=audio 3278 RTP/AVP 9 103 101
         // a=rtcp:3279 IN IP4 192.168.53.235
         // c=IN IP4 192.168.53.235
         // m=video 3280 RTP/AVP 102 115 116
         // c=IN IP4 192.168.53.235
         // a=rtcp:3281 IN IP4 192.168.53.235

         // anomaly - log lines can be corrupted by inserted log lines, messing up the token parsing!!
         /*
          * Note the hanging m=   !!!
          * 
          * RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - v=0
o=dakota 3248873059 3248873059 IN IP4 10.174.2.3
s=-
c=IN IP4 10.174.2.3
b=AS:1920
t=0 0
m=audio 3278 RTP/AVP 9 101
c=IN IP4 10.174.2.3
b=AS:64
a=ptime:20
a=rtcp:3279 IN IP4 10.174.2.3
a=rtpmap:9 G722/8000
a=fmtp:9 bitrate=64000
a=rtpmap:101 telephone-event/8000
a=fmtp:101 0-15
m=
RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - video 3270 RTP/AVP 102 115 116
c=IN IP4 10.174.2.3
b=TIAS:1920000
a=rtcp:3271 IN IP4 10.174.2.3
a=rtcp-fb:* nack pli
a=rtcp-fb:* ccm fir
a=rtcp-fb:* ccm tmmbr
a=rtpmap:102 H264/90000
a=fmtp:102 profile-level-id=42E01F;max-rcmd-nalu-size=1270
a=rtpmap:115 ulpfec/90000
a=fmtp:115 protectedPa
RV_BEEHD           01/05/24 12:21:35.453 VE_CALL     : DEBUG - yload=102;RvXorAlgo;RvRsAlgo;MaxRsWindowSize=48;MaxRsFecPackets=8
a=rtpmap:116 avcfec/90000
a=fmtp:116 protectedPayload=102;AllowMultiFrameGroups=1;MaxRsWindowSize=48;MaxRsFecPackets=8
a=content:main
          */

         string[] parts = line.Split('=');
         string[] subparts = parts[1].Split(' ');

         switch (section)
         {
            case SectionNames.general:

               switch (parts[0])
               {
                  case "o":
                  case "origin":
                     this.origin = subparts[0];
                     this.sessionId = subparts[1];
                     this.sessionVersion = subparts[2];
                     break;

                  case "c":
                  case "connection":
                     this.audioIpAddress = subparts[2];
                     this.videoIpAddress = subparts[2];
                     break;
               }
               break;

            case SectionNames.audio:
               switch (parts[0])
               {
                  case "c":
                  case "connection":
                     this.audioIpAddress = subparts[2];
                     break;

                  case "m":
                  case "media":
                     this.audioPort = subparts[1];
                     this.audioControlPort = (Convert.ToInt16(this.audioPort) + 1).ToString();   // normally
                     break;

                  case "a":
                  case "attribute":
                     if (subparts[0].StartsWith("rtcp:"))
                     {
                        this.audioControlPort = subparts[0].Replace("rtcp:", string.Empty);
                     }
                     else if (subparts[0] == "recvonly")
                     {
                        this.audioStreamState = StreamState.recvonly;
                     }
                     else if (subparts[0] == "sendonly")
                     {
                        this.audioStreamState = StreamState.sendonly;
                     }
                     else if (subparts[0] == "sendrecv")
                     {
                        this.audioStreamState |= StreamState.sendrecv;
                     }
                     break;

                  default:
                     break;
               }
               break;

            case SectionNames.video:
               switch (parts[0])
               {
                  case "c":
                  case "connection":
                     this.videoIpAddress = subparts[2];
                     break;

                  case "m":
                  case "media":
                     this.videoPort = subparts[1];
                     this.videoControlPort = (Convert.ToInt16(this.videoPort) + 1).ToString();   // normally
                     break;

                  case "a":
                  case "attribute":
                     if (subparts[0].StartsWith("rtcp:"))
                     {
                        this.videoControlPort = subparts[0].Replace("rtcp:", string.Empty);
                     }
                     else if (subparts[0] == "recvonly")
                     {
                        this.videoStreamState = StreamState.recvonly;
                     }
                     else if (subparts[0] == "sendonly")
                     {
                        this.videoStreamState = StreamState.sendonly;
                     }
                     else if (subparts[0] == "sendrecv")
                     {
                        this.videoStreamState |= StreamState.sendrecv;
                     }
                     break;

                  default:
                     break;
               }
               break;
         }
      }



      /// <summary>
      /// A value indicating whether all of the the SDP header lines have been received.
      /// </summary>
      public bool AllSdpLinesReceived
      {
         get
         {
            if (this.targetContentLength > 0)
            {
               return this.contentLength == this.targetContentLength;
            }

            return false;
         }
      }

      /// <summary>
      /// The audio stream IP address.
      /// </summary>
      public string AudioIpAddress { get { return this.audioIpAddress; }}

      /// <summary>
      /// The audio stream port number.
      /// </summary>
      public string AudioPort { get { return this.audioPort; } }

      /// <summary>
      /// The audio stream RTCP control port number.
      /// </summary>
      public string AudioControlPort {  get { return this.audioControlPort; } }

      /// <summary>
      /// The audio stream state in the media offer.
      /// </summary>
      public StreamState AudioStreamState {  get {  return this.audioStreamState; } }

      /// <summary>
      /// The video stream IP address.
      /// </summary>
      public string VideoIpAddress { get { return this.videoIpAddress; } }

      /// <summary>
      /// The video stream port number.
      /// </summary>
      public string VideoPort { get { return this.videoPort; } }

      /// <summary>
      /// the video stream RTCP control port number.
      /// </summary>
      public string VideoControlPort { get { return this.videoControlPort; } }

      /// <summary>
      /// The video stream state in the media offer.
      /// </summary>
      public StreamState VideoStreamState { get { return this.videoStreamState; } }

      /// <summary>
      /// The stream origin IP Address or session name.
      /// </summary>
      public string SessionOriginator { get { return this.origin; } }

      /// <summary>
      /// The media session's stream identifier.
      /// </summary>
      public string SessionId { get { return this.sessionId; } }

      /// <summary>
      /// The media session version.
      /// </summary>
      public string SessionVersion { get { return this.sessionVersion; } }

   }
}
