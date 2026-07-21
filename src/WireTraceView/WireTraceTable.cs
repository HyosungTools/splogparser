using System;
using System.Collections.Generic;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace WireTraceView
{
   /// <summary>
   /// Readable view of the ATM &lt;-&gt; MoniView RMS wire protocol from TcpTrace_*.txt, envelope
   /// only (the payload is per-frame XOR-encrypted and is not decoded). Two sheets:
   ///
   ///   WireTrace   - one row per message, time-ordered: direction, ip, len, kind
   ///                 (DATA/ACK/EOT/NAK/ENQ), encIdx, frameOk, and a plain-English comment.
   ///   WireSummary - one row per ATM IP: frame/DATA/ACK/EOT counts, malformed count, handshake
   ///                 clean %, average frame size, first/last seen, and a health note.
   ///
   /// Handshake model (per legacy Ch.4): a DATA frame is "clean" once the server ACKs it (06). EOT
   /// (04) is a session close, not required per frame - so chatty pollers that ACK-without-EOT are
   /// still healthy. NAK (15) or a malformed frame are the real anomalies.
   /// </summary>
   class WireTraceTable : BaseTable
   {
      private const string WIRE = "WireTrace";
      private const string SUMMARY = "WireSummary";

      private class Agg
      {
         public string IP { get; set; }
         public string LastFile { get; set; }
         public string First { get; set; }
         public string Last { get; set; }
         public int Frames { get; set; }
         public int Data { get; set; }        // ATM->MV uploads only
         public int HostData { get; set; }    // MV->ATM data/command frames
         public int Ack { get; set; }
         public int Eot { get; set; }
         public int Nak { get; set; }
         public int Malformed { get; set; }
         public int Acked { get; set; }
         public int Unacked { get; set; }
         public long SizeSum { get; set; }
         public bool DataAwaitingAck { get; set; }

         public Agg()
         {
            IP = ""; LastFile = ""; First = ""; Last = "";
            Frames = 0; Data = 0; HostData = 0; Ack = 0; Eot = 0; Nak = 0; Malformed = 0;
            Acked = 0; Unacked = 0; SizeSum = 0; DataAwaitingAck = false;
         }
      }

      private readonly Dictionary<string, Agg> _byIp = new Dictionary<string, Agg>();

      public WireTraceTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is TcpLine tcp)
            {
               if (string.IsNullOrEmpty(tcp.IP))
               {
                  return;
               }

               AddWireRow(tcp);
               Fold(tcp);
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("WireTraceTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void AddWireRow(TcpLine tcp)
      {
         try
         {
            DataRow row = dTableSet.Tables[WIRE].Rows.Add();
            row["file"] = tcp.LogFile;
            row["time"] = tcp.Timestamp;
            row["direction"] = tcp.Direction;
            row["ip"] = tcp.IP;
            row["len"] = tcp.Length.ToString();
            row["kind"] = tcp.Kind;
            row["encidx"] = tcp.EncIndexHex;
            row["frameok"] = (tcp.MsgType == TcpMsgType.Data) ? (tcp.FrameOk ? "yes" : "NO") : "";
            row["comment"] = RowComment(tcp);
            dTableSet.Tables[WIRE].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WireTraceTable.AddWireRow Exception : " + e.Message);
         }
      }

      private string RowComment(TcpLine tcp)
      {
         switch (tcp.MsgType)
         {
            case TcpMsgType.Data:
               if (!tcp.FrameOk)
               {
                  return "Malformed/truncated frame (captured " + tcp.CapturedLen.ToString() + " of " + tcp.Length.ToString() + " bytes)";
               }
               return "Data frame, well-formed (encrypted payload, table " + tcp.EncIndexHex + ")";
            case TcpMsgType.Ack:
               return "Server acknowledged frame";
            case TcpMsgType.Eot:
               return "Transaction closed";
            case TcpMsgType.Nak:
               return "Server rejected frame (NAK)";
            case TcpMsgType.Enq:
               return "Enquiry / poll";
            default:
               return "Unknown control byte";
         }
      }

      private void Fold(TcpLine tcp)
      {
         Agg agg = GetAgg(tcp.IP);
         agg.LastFile = tcp.LogFile;
         UpdateSeen(agg, tcp.Timestamp);
         agg.Frames++;

         switch (tcp.MsgType)
         {
            case TcpMsgType.Data:
               if (!tcp.FrameOk)
               {
                  agg.Malformed++;
               }
               if (tcp.FromAtm)
               {
                  // ATM upload - the frame we track for "is the ATM being heard".
                  agg.Data++;
                  agg.SizeSum += tcp.Length;
                  if (agg.DataAwaitingAck)
                  {
                     // previous upload never got an ACK before this one
                     agg.Unacked++;
                  }
                  agg.DataAwaitingAck = true;
               }
               else
               {
                  // Host -> ATM data/command frame; closed by the ATM's EOT, not an ACK.
                  agg.HostData++;
               }
               break;

            case TcpMsgType.Ack:
               agg.Ack++;
               // Only a host ACK (MV->ATM) acknowledges an ATM upload.
               if (!tcp.FromAtm && agg.DataAwaitingAck)
               {
                  agg.Acked++;
                  agg.DataAwaitingAck = false;
               }
               break;

            case TcpMsgType.Eot:
               agg.Eot++;
               break;

            case TcpMsgType.Nak:
               agg.Nak++;
               break;

            default:
               break;
         }
      }

      public override void PostProcess()
      {
         try
         {
            foreach (KeyValuePair<string, Agg> pair in _byIp)
            {
               Agg agg = pair.Value;

               // A DATA still awaiting an ACK at end of capture is unacked.
               if (agg.DataAwaitingAck)
               {
                  agg.Unacked++;
               }

               int cleanPct = (agg.Data > 0) ? (int)((agg.Acked * 100L) / agg.Data) : 100;
               int avgSize = (agg.Data > 0) ? (int)(agg.SizeSum / agg.Data) : 0;

               DataRow row = dTableSet.Tables[SUMMARY].Rows.Add();
               row["file"] = agg.LastFile;
               row["time"] = agg.Last;
               row["ip"] = agg.IP;
               row["frames"] = agg.Frames.ToString();
               row["data"] = agg.Data.ToString();
               row["hostdata"] = agg.HostData.ToString();
               row["ack"] = agg.Ack.ToString();
               row["eot"] = agg.Eot.ToString();
               row["nak"] = agg.Nak.ToString();
               row["malformed"] = agg.Malformed.ToString();
               row["cleanpct"] = cleanPct.ToString();
               row["avgsize"] = avgSize.ToString();
               row["firstseen"] = agg.First;
               row["comment"] = SummaryComment(agg, cleanPct);
               dTableSet.Tables[SUMMARY].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WireTraceTable.PostProcess Exception : " + e.Message);
         }

         base.PostProcess();
      }

      private string SummaryComment(Agg agg, int cleanPct)
      {
         string hostNote = (agg.HostData > 0) ? (", " + agg.HostData.ToString() + " host frame(s)") : "";

         if (agg.Malformed > 0 || agg.Nak > 0)
         {
            return "Attention - " + agg.Malformed.ToString() + " malformed, " + agg.Nak.ToString() + " NAK" + hostNote;
         }

         if (agg.Unacked > 0)
         {
            return cleanPct.ToString() + "% of uploads acked - " + agg.Unacked.ToString() + " unacked" + hostNote;
         }

         return "Healthy - " + agg.Data.ToString() + " uploads all acked" + hostNote;
      }

      private Agg GetAgg(string ip)
      {
         Agg agg;
         if (!_byIp.TryGetValue(ip, out agg))
         {
            agg = new Agg();
            agg.IP = ip;
            _byIp[ip] = agg;
         }

         return agg;
      }

      private void UpdateSeen(Agg agg, string ts)
      {
         if (string.IsNullOrEmpty(ts))
         {
            return;
         }

         if (string.IsNullOrEmpty(agg.First) || string.CompareOrdinal(ts, agg.First) < 0)
         {
            agg.First = ts;
         }

         if (string.IsNullOrEmpty(agg.Last) || string.CompareOrdinal(ts, agg.Last) > 0)
         {
            agg.Last = ts;
         }
      }
   }
}
