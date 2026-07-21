using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>Frame kind of a TCP-trace message, from its first byte / control code.</summary>
   public enum TcpMsgType
   {
      None,
      Data,   /* 0x02 STX - an ATM data telegram (encrypted payload) */
      Ack,    /* 0x06 */
      Eot,    /* 0x04 */
      Nak,    /* 0x15 */
      Enq,    /* 0x05 */
      Unknown
   }

   /// <summary>
   /// One message block from a TcpTrace_*.txt capture. This is the on-the-wire RMS protocol
   /// (legacy docs Ch.4 Communications / Ch.5 Message Format), not an application log.
   ///
   /// Frame (Len &gt; 1):  02 STX | 2-byte LE length | encIdx | ..encrypted payload.. | 03 ETX | BCC
   ///   Len = 1(STX) + 2(len) + payload + 1(ETX) + 1(BCC).  byte[3] = encryption-table index.
   /// Control (Len == 1): 06 ACK, 04 EOT, 15 NAK, 05 ENQ.
   ///
   /// We read only the ENVELOPE (timestamp, direction, IP, length, encIdx, control byte, frame
   /// well-formedness). The payload is XOR-encrypted per frame by the table named in encIdx and
   /// cannot be decoded without the XOR_TABLE set - so it is intentionally left untouched.
   /// </summary>
   public class TcpLine : LogLine, ILogLine
   {
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      public string Direction { get; set; }   // "ATM->MV" or "MV->ATM"
      public bool FromAtm { get; set; }
      public string IP { get; set; }
      public int Length { get; set; }          // declared Len from the header
      public int CapturedLen { get; set; }     // bytes actually recovered from the dump
      public int FirstByte { get; set; }
      public int EncIndex { get; set; }         // byte[3] on DATA frames, else -1
      public bool FrameOk { get; set; }
      public TcpMsgType MsgType { get; set; }

      private static readonly Regex TsRegex = new Regex(@"\[(\d{4})(\d{2})(\d{2})-(\d{2}:\d{2}:\d{2})\+(\d+)\]");
      private static readonly Regex HdrRegex = new Regex(@"(ATM --> MoniView|MoniView --> ATM)\s+IP Address : ([\d\.]+)\s+Len : (\d+)");

      public TcpLine(ILogFileHandler parent, string logLine) : base(parent, logLine)
      {
         Initialize();
      }

      protected virtual void Initialize()
      {
         Direction = "";
         FromAtm = false;
         IP = "";
         Length = 0;
         CapturedLen = 0;
         FirstByte = -1;
         EncIndex = -1;
         FrameOk = false;
         MsgType = TcpMsgType.None;

         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();

         ParseFrame();
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         string timestamp = LogLine.DefaultTimestamp;

         Match m = TsRegex.Match(logLine);
         if (m.Success)
         {
            // [20260707-04:00:28+754] -> 2026-07-07 04:00:28.754
            timestamp = String.Format("{0}-{1}-{2} {3}.{4}",
               m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value, m.Groups[5].Value);
         }

         return timestamp;
      }

      protected void ParseFrame()
      {
         Match h = HdrRegex.Match(logLine);
         if (!h.Success)
         {
            return;
         }

         FromAtm = h.Groups[1].Value.StartsWith("ATM");
         Direction = FromAtm ? "ATM->MV" : "MV->ATM";
         IP = h.Groups[2].Value;

         int len = 0;
         int.TryParse(h.Groups[3].Value, out len);
         Length = len;

         List<int> bytes = ExtractFrameBytes();
         CapturedLen = bytes.Count;
         if (bytes.Count > 0)
         {
            FirstByte = bytes[0];
         }

         MsgType = ClassifyType(FirstByte);

         if (MsgType == TcpMsgType.Data)
         {
            if (bytes.Count > 3)
            {
               EncIndex = bytes[3];
            }
            // Well-formed: STX first, ETX second-to-last, and captured length matches the header.
            FrameOk = (bytes.Count == Length) && (bytes.Count >= 2) && (bytes[0] == 0x02) && (bytes[bytes.Count - 2] == 0x03);
         }
         else
         {
            // Control message: just needs to match its declared length.
            FrameOk = (bytes.Count == Length);
         }
      }

      /// <summary>
      /// Recover the frame bytes from the hex-dump rows. Reads space-separated tokens left to
      /// right, skips the literal '-' column separator, accepts only ^[0-9A-Fa-f]{2}$, and BREAKS
      /// on the first non-hex token - that token is the start of the ASCII gutter, which can itself
      /// contain hex-looking pairs. (Legacy-doc parsing rule.)
      /// </summary>
      protected List<int> ExtractFrameBytes()
      {
         List<int> bytes = new List<int>();

         string[] lines = logLine.Split('\n');
         foreach (string raw in lines)
         {
            string line = raw.TrimEnd('\r');
            if (line.Length < 2 || !IsHexByte(line.Substring(0, 2)))
            {
               // timestamp line, header line, or blank - not a hex row
               continue;
            }

            string[] tokens = line.Split(' ');
            foreach (string tok in tokens)
            {
               if (tok.Length == 0 || tok == "-")
               {
                  continue;
               }

               if (IsHexByte(tok))
               {
                  bytes.Add(Convert.ToInt32(tok, 16));
               }
               else
               {
                  // first non-hex token = ASCII gutter; stop reading this row
                  break;
               }
            }
         }

         return bytes;
      }

      private bool IsHexByte(string s)
      {
         if (s.Length != 2)
         {
            return false;
         }

         return Uri.IsHexDigit(s[0]) && Uri.IsHexDigit(s[1]);
      }

      private TcpMsgType ClassifyType(int firstByte)
      {
         switch (firstByte)
         {
            case 0x02:
               return TcpMsgType.Data;
            case 0x06:
               return TcpMsgType.Ack;
            case 0x04:
               return TcpMsgType.Eot;
            case 0x15:
               return TcpMsgType.Nak;
            case 0x05:
               return TcpMsgType.Enq;
            default:
               return TcpMsgType.Unknown;
         }
      }

      /// <summary>Upper-case token for the view: DATA / ACK / EOT / NAK / ENQ / UNKNOWN.</summary>
      public string Kind
      {
         get
         {
            switch (MsgType)
            {
               case TcpMsgType.Data:
                  return "DATA";
               case TcpMsgType.Ack:
                  return "ACK";
               case TcpMsgType.Eot:
                  return "EOT";
               case TcpMsgType.Nak:
                  return "NAK";
               case TcpMsgType.Enq:
                  return "ENQ";
               default:
                  return "UNKNOWN";
            }
         }
      }

      /// <summary>encIdx as a hex string for DATA frames, empty otherwise.</summary>
      public string EncIndexHex
      {
         get
         {
            if (EncIndex < 0)
            {
               return "";
            }

            return "0x" + EncIndex.ToString("X2");
         }
      }

      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (string.IsNullOrEmpty(logLine))
         {
            return null;
         }

         if (!HdrRegex.IsMatch(logLine))
         {
            return null;
         }

         return new TcpLine(logFileHandler, logLine);
      }
   }
}
