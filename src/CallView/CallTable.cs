using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CallView
{
   /// <summary>
   /// CallTable reconstructs teller call SESSIONS from the Active Teller Workstation log
   /// (a single call spans many log lines) and emits two worksheets:
   ///
   ///   CallLedger   - one row per answered call: start, end, duration, asset, teller,
   ///                  assisted transaction, teller approvals, disposition, root cause.
   ///   DroppedCalls - a short correlated timeline for each dropped call (the "why").
   ///
   /// Unlike AWTable (a raw one-row-per-line dump) this table accumulates state across
   /// ProcessRow() calls and flushes rows in PostProcess(). It reads the raw log text so
   /// it can also see continuation lines (e.g. the unhandled-exception stack trace, which
   /// the handler returns as an unrecognized AWLine).
   /// </summary>
   class CallTable : BaseTable
   {
      /// <summary> An in-progress / completed teller call session. </summary>
      private class Session
      {
         public string Req = "";
         public string Asset = "";
         public string SessId = "";
         public string File = "";
         public DateTime Start;
         public DateTime End;
         public DateTime Connected;
         public bool HasConnected = false;
         public bool Created = false;
         public bool Answered = false;
         public bool Closed = false;
         public bool Dropped = false;
         public string RootCause = "";
         public string CrashFile = "";
         public readonly List<string> TxTypes = new List<string>();
         public readonly List<string> Approvals = new List<string>();
         public readonly List<string[]> Trail = new List<string[]>();   // { time, event, detail }
      }

      private readonly List<Session> _order = new List<Session>();
      private readonly Dictionary<string, Session> _byReq = new Dictionary<string, Session>();
      private string _teller = "";
      private string _file = "";
      private DateTime _lastTs = DateTime.MinValue;
      private bool _haveTs = false;

      // ---- line signatures (matched against the raw Workstation log text) ----
      // AWLine.tsTimestamp() rewrites "[2026-07-10 11:02:20-782]" to "2026.07.10 11:02:20.782"
      // (Replace("-",".") hits the date separators too), so the parse format uses dots, not dashes.
      private static readonly string[] TS_FORMATS = { "yyyy.MM.dd HH:mm:ss.fff", "yyyy.MM.dd HH:mm:ss" };
      private static readonly Regex reSignIn   = new Regex(@"user (?<u>[^ ]+) is signing in", RegexOptions.Compiled);
      private static readonly Regex reReqWord  = new Regex(@"session requested for asset (?<asset>\w+) for teller session request (?<id>\d+)", RegexOptions.Compiled);
      private static readonly Regex reReq      = new Regex(@"session request (?<id>\d+) for asset (?<asset>\w+)(?<del> deleted)?", RegexOptions.Compiled);
      private static readonly Regex reCreated  = new Regex(@"Teller session (?<sid>\d+) for asset (?<asset>\w+) created", RegexOptions.Compiled);
      private static readonly Regex reDelByReq = new Regex(@"session request deleted for teller session request (?<id>\d+)", RegexOptions.Compiled);
      private static readonly Regex reApproval = new Regex(@"TellerApproval=(?<a>\w+), TellerAmount=(?<amt>\d+)", RegexOptions.Compiled);
      private static readonly Regex reInTx     = new Regex(@"InTransaction update for asset (?<asset>\w+) during (?<tx>\w+)", RegexOptions.Compiled);
      private static readonly Regex reTaskEvt  = new Regex(@"Received (?<event>\w+) event for (?<task>\w+) \d+ for asset", RegexOptions.Compiled);
      private static readonly Regex reCrashMp4 = new Regex(@"file '(?<f>[^']+\.mp4)'", RegexOptions.Compiled);

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CallTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Prep the tables for Excel.
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         return base.WriteExcelFile();
      }

      /// <summary>
      /// Process one line from the merged Workstation log file. Accumulates session state;
      /// nothing is written to the tables until PostProcess().
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         AWLine aw = logLine as AWLine;
         if (aw == null) return;

         string text = aw.logLine ?? string.Empty;
         if (!string.IsNullOrEmpty(aw.LogFile)) _file = aw.LogFile;

         DateTime parsed;
         if (aw.IsValidTimestamp &&
             DateTime.TryParseExact(aw.Timestamp, TS_FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
         {
            _lastTs = parsed;
            _haveTs = true;
         }
         DateTime now = _haveTs ? _lastTs : DateTime.MinValue;

         // teller identity (from the sign-in line)
         Match m = reSignIn.Match(text);
         if (m.Success) { _teller = m.Groups["u"].Value; return; }

         // FATAL: client crash - VideoManager deletes an active recording whose .mp4 is
         // still locked. This text arrives on an unrecognized continuation line (no timestamp),
         // so it is attributed to the currently open call using the last timestamp seen.
         if (text.IndexOf("being used by another process", StringComparison.OrdinalIgnoreCase) >= 0)
         {
            Session cur = CurrentCall();
            if (cur != null)
            {
               cur.Dropped = true;
               cur.End = now;
               Match fm = reCrashMp4.Match(text);
               cur.CrashFile = fm.Success ? fm.Groups["f"].Value : string.Empty;
               cur.RootCause = "Client crash: VideoManager reseed timer tried to delete the active recording folder while the .mp4 was still locked, throwing an unhandled exception that aborted the ActiveTeller connection and dropped the call.";
               AddTrail(cur, now, "FATAL file lock", Trunc(text, 160));
            }
            return;
         }

         // teller session created (this request became a real call)
         m = reCreated.Match(text);
         if (m.Success)
         {
            Session s = LastOpenForAsset(m.Groups["asset"].Value);
            if (s != null) { s.Created = true; s.SessId = m.Groups["sid"].Value; s.End = now; AddTrail(s, now, "call created", "teller session " + m.Groups["sid"].Value); }
            return;
         }

         // teller answered the video call
         if (text.IndexOf("AcceptVideoCall", StringComparison.Ordinal) >= 0)
         {
            Session s = LastCreatedUnanswered();
            if (s != null)
            {
               s.Answered = true; s.Connected = now; s.HasConnected = true; s.End = now;
               AddTrail(s, now, "answered", "teller accepted the video call");
            }
            return;
         }

         // teller took remote control of the ATM
         if (text.IndexOf("remote control session", StringComparison.OrdinalIgnoreCase) >= 0 &&
             text.IndexOf("started for asset", StringComparison.OrdinalIgnoreCase) >= 0)
         {
            Session s = CurrentCall();
            if (s != null) { s.End = now; AddTrail(s, now, "remote control", "teller took control of the ATM"); }
            return;
         }

         // teller approval of a transaction item (e.g. modified / approved amount)
         m = reApproval.Match(text);
         if (m.Success)
         {
            Session s = CurrentCall();
            if (s != null)
            {
               decimal amt;
               decimal.TryParse(m.Groups["amt"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out amt);
               string a = m.Groups["a"].Value + " $" + (amt / 100m).ToString("N2", CultureInfo.InvariantCulture);
               s.Approvals.Add(a);
               s.End = now;
               AddTrail(s, now, "approval", a);
            }
            return;
         }

         // transaction type in progress (Withdrawal / Deposit / CheckDeposit / ...)
         m = reInTx.Match(text);
         if (m.Success)
         {
            string tx = m.Groups["tx"].Value;
            Session s = CurrentCall();
            if (s != null && !string.IsNullOrEmpty(tx) && !s.TxTypes.Contains(tx)) { s.TxTypes.Add(tx); s.End = now; AddTrail(s, now, "transaction", tx); }
            return;
         }

         // device / transaction progress during a money task (deposit / withdrawal / check / ID scan)
         m = reTaskEvt.Match(text);
         if (m.Success)
         {
            string task = m.Groups["task"].Value;
            if (task.IndexOf("Deposit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                task.IndexOf("CheckCashing", StringComparison.OrdinalIgnoreCase) >= 0 ||
                task.IndexOf("Withdrawal", StringComparison.OrdinalIgnoreCase) >= 0 ||
                task.IndexOf("ScanId", StringComparison.OrdinalIgnoreCase) >= 0)
            {
               Session s = CurrentCall();
               if (s != null) { s.End = now; AddTrail(s, now, "device", m.Groups["event"].Value + " (" + task + ")"); }
            }
            return;
         }

         // client tearing down its server connection - the drop cascade
         if (text.IndexOf("Disconnecting from ActiveTeller", StringComparison.Ordinal) >= 0)
         {
            Session s = CurrentCall();
            if (s != null) { s.End = now; AddTrail(s, now, "disconnecting", "client tearing down the ActiveTeller connection"); }
            return;
         }
         if (text.IndexOf("connection thread encountered an exception", StringComparison.Ordinal) >= 0)
         {
            Session s = CurrentCall();
            if (s != null) AddTrail(s, now, "connection aborted", "ActiveTeller / SignalR connection thread aborted");
            return;
         }

         // "session request deleted for teller session request N"  (end of the request/call)
         m = reDelByReq.Match(text);
         if (m.Success) { CloseReq(m.Groups["id"].Value, now); return; }

         // "session request N for asset X [deleted]"  (open, or delete)
         m = reReq.Match(text);
         if (m.Success)
         {
            if (m.Groups["del"].Success) CloseReq(m.Groups["id"].Value, now);
            else OpenReq(m.Groups["id"].Value, m.Groups["asset"].Value, now);
            return;
         }

         // "session requested for asset X for teller session request N"
         m = reReqWord.Match(text);
         if (m.Success) { OpenReq(m.Groups["id"].Value, m.Groups["asset"].Value, now); return; }
      }

      /// <summary>
      /// Flush accumulated call sessions into the CallLedger and DroppedCalls tables.
      /// Called by BaseView.PostProcess before the tables are persisted to XML.
      /// </summary>
      public override void PostProcess()
      {
         int clean = 0, dropped = 0, notAnswered = 0, requestOnly = 0;

         foreach (Session s in _order)
         {
            if (!s.Created) { requestOnly++; continue; }

            string disposition;
            if (s.Dropped) { disposition = "DROPPED - client crash"; dropped++; }
            else if (s.Answered) { disposition = "Clean"; clean++; }
            else { disposition = "Created, not answered"; notAnswered++; }

            DateTime startRef = s.HasConnected ? s.Connected : s.Start;
            int dur = (int)Math.Max(0, (s.End - startRef).TotalSeconds);

            DataRow row = dTableSet.Tables["CallLedger"].Rows.Add();
            row["file"] = string.IsNullOrEmpty(s.File) ? _file : s.File;
            row["time"] = s.Start.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            row["endtime"] = s.End.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            row["duration"] = dur.ToString(CultureInfo.InvariantCulture);
            row["asset"] = s.Asset;
            row["teller"] = _teller;
            row["transaction"] = s.TxTypes.Count > 0 ? string.Join(", ", s.TxTypes) : "-";
            row["approvals"] = s.Approvals.Count > 0 ? string.Join("; ", s.Approvals) : "";
            row["disposition"] = disposition;
            row["rootcause"] = s.RootCause;

            if (s.Dropped)
            {
               foreach (string[] t in s.Trail)
               {
                  DataRow d = dTableSet.Tables["DroppedCalls"].Rows.Add();
                  d["file"] = string.IsNullOrEmpty(s.File) ? _file : s.File;
                  d["time"] = t[0];
                  d["asset"] = s.Asset;
                  d["teller"] = _teller;
                  d["event"] = t[1];
                  d["detail"] = t[2];
               }
            }
         }

         dTableSet.Tables["CallLedger"].AcceptChanges();
         dTableSet.Tables["DroppedCalls"].AcceptChanges();

         ctx.ConsoleWriteLogLine(string.Format(
            "CallView: {0} answered calls ({1} clean, {2} DROPPED, {3} created-not-answered); {4} session requests never answered.",
            clean + dropped + notAnswered, clean, dropped, notAnswered, requestOnly));
      }

      // ---------------- session bookkeeping helpers ----------------

      private void OpenReq(string id, string asset, DateTime ts)
      {
         Session s;
         if (!_byReq.TryGetValue(id, out s))
         {
            s = new Session { Req = id, Asset = asset, Start = ts, End = ts, File = _file };
            _byReq[id] = s;
            _order.Add(s);
         }
         else if (string.IsNullOrEmpty(s.Asset))
         {
            s.Asset = asset;
         }
      }

      private void CloseReq(string id, DateTime ts)
      {
         Session s;
         if (_byReq.TryGetValue(id, out s))
         {
            if (!s.Dropped) s.End = ts;
            s.Closed = true;
         }
      }

      /// <summary> The most recently opened call that has connected and not yet closed. </summary>
      private Session CurrentCall()
      {
         for (int i = _order.Count - 1; i >= 0; i--)
            if (_order[i].Created && !_order[i].Closed) return _order[i];
         return null;
      }

      private Session LastCreatedUnanswered()
      {
         for (int i = _order.Count - 1; i >= 0; i--)
            if (_order[i].Created && !_order[i].Answered && !_order[i].Closed) return _order[i];
         return null;
      }

      private Session LastOpenForAsset(string asset)
      {
         for (int i = _order.Count - 1; i >= 0; i--)
            if (!_order[i].Created && !_order[i].Closed && _order[i].Asset == asset) return _order[i];
         return null;
      }

      private static void AddTrail(Session s, DateTime ts, string ev, string detail)
      {
         s.Trail.Add(new string[] { ts.ToString("HH:mm:ss", CultureInfo.InvariantCulture), ev, detail });
         if (s.Trail.Count > 30) s.Trail.RemoveAt(0);
      }

      private static string Trunc(string s, int n)
      {
         if (string.IsNullOrEmpty(s)) return "";
         return s.Length <= n ? s : s.Substring(0, n);
      }
   }
}
