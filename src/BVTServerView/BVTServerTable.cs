using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BVTServerView
{
   /// <summary>
   /// BVTServerTable reads the BlueVerse Teller server logs and emits two summary worksheets:
   ///   Allocation   - one row per teller session request: asset, routing rule, whether a teller
   ///                  was assigned, which teller, and the wait to assignment.
   ///   ServerFaults - server exceptions grouped by signature (digits / GUIDs / URIs normalized),
   ///                  with a count and first/last seen, so an exception storm reads as one line.
   /// State is accumulated across ProcessRow() and flushed in PostProcess().
   ///
   /// Note: this view emits only allocations and fault signatures - it never writes the raw
   /// settings payload, so the plaintext ClientCredentialsPassword that AVView surfaces does not
   /// reach this worksheet.
   /// </summary>
   class BVTServerTable : BaseTable
   {
      private class Req
      {
         public string Asset = "";
         public string Id = "";
         public string File = "";
         public DateTime Seen;
         public string Rule = "";
         public bool Assigned = false;
         public DateTime AssignedAt;
         public string Teller = "";
      }

      private class Fault
      {
         public int Count = 0;
         public DateTime First;
         public DateTime Last;
         public string File = "";
      }

      private readonly Dictionary<string, Req> _reqs = new Dictionary<string, Req>();
      private readonly List<string> _reqOrder = new List<string>();
      private readonly Dictionary<string, Fault> _faults = new Dictionary<string, Fault>();
      private readonly List<string> _faultOrder = new List<string>();
      private string _file = "";

      private static readonly string[] TS_FORMATS = { "yyyy-MM-dd HH:mm:ss" };
      private static readonly Regex reCan    = new Regex(@"CanHandleRequest - teller session request (?<r>\d+) from (?<asset>\w+).*routing rule = (?<rule>\w+)", RegexOptions.Compiled);
      private static readonly Regex reAssign = new Regex(@"client session (?<t>\d+) is assigned teller session request (?<r>\d+) from (?<asset>\w+)", RegexOptions.Compiled);
      private static readonly Regex reFault  = new Regex(@"[Ee]xception|error occurred|not supplied|customers were found|not removed|conflicted with", RegexOptions.Compiled);
      private static readonly Regex reGuid   = new Regex(@"[0-9a-fA-F]{8}-[0-9a-fA-F-]{20,}", RegexOptions.Compiled);
      private static readonly Regex reUri    = new Regex(@"http\S+", RegexOptions.Compiled);
      private static readonly Regex reNum    = new Regex(@"\d+", RegexOptions.Compiled);

      public BVTServerTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      public override bool WriteExcelFile()
      {
         return base.WriteExcelFile();
      }

      public override void ProcessRow(ILogLine logLine)
      {
         AVLine av = logLine as AVLine;
         if (av == null) return;

         string text = av.logLine ?? string.Empty;
         if (!string.IsNullOrEmpty(av.LogFile)) _file = av.LogFile;

         DateTime now;
         if (!(av.IsValidTimestamp &&
               DateTime.TryParseExact(av.Timestamp, TS_FORMATS, CultureInfo.InvariantCulture, DateTimeStyles.None, out now)))
            now = DateTime.MinValue;

         // teller session request seen (routing decision)
         Match mc = reCan.Match(text);
         if (mc.Success)
         {
            Req r = GetReq(mc.Groups["asset"].Value, mc.Groups["r"].Value, now);
            if (string.IsNullOrEmpty(r.Rule)) r.Rule = mc.Groups["rule"].Value;
            return;
         }

         // teller assigned to the request
         Match ma = reAssign.Match(text);
         if (ma.Success)
         {
            Req r = GetReq(ma.Groups["asset"].Value, ma.Groups["r"].Value, now);
            if (!r.Assigned) { r.Assigned = true; r.AssignedAt = now; r.Teller = ma.Groups["t"].Value; }
            return;
         }

         // server fault - accumulate by normalized signature
         if (reFault.IsMatch(text))
         {
            string sig = Signature(text);
            Fault f;
            if (!_faults.TryGetValue(sig, out f))
            {
               f = new Fault { First = now, Last = now, File = _file };
               _faults[sig] = f; _faultOrder.Add(sig);
            }
            f.Count++;
            if (now != DateTime.MinValue)
            {
               if (f.First == DateTime.MinValue || now < f.First) f.First = now;
               if (now > f.Last) f.Last = now;
            }
         }
      }

      public override void PostProcess()
      {
         foreach (string key in _reqOrder)
         {
            Req r = _reqs[key];
            DataRow row = dTableSet.Tables["Allocation"].Rows.Add();
            row["file"] = r.File;
            row["time"] = r.Seen.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            row["asset"] = r.Asset;
            row["request"] = r.Id;
            row["rule"] = r.Rule;
            row["assigned"] = r.Assigned ? "yes" : "NO - no teller assigned";
            row["teller"] = r.Teller;
            row["waitsec"] = r.Assigned ? ((int)Math.Max(0, (r.AssignedAt - r.Seen).TotalSeconds)).ToString(CultureInfo.InvariantCulture) : "";
         }
         dTableSet.Tables["Allocation"].AcceptChanges();

         foreach (string sig in _faultOrder)
         {
            Fault f = _faults[sig];
            DataRow row = dTableSet.Tables["ServerFaults"].Rows.Add();
            row["file"] = f.File;
            row["time"] = f.First.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            row["lastseen"] = f.Last.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            row["count"] = f.Count.ToString(CultureInfo.InvariantCulture);
            row["signature"] = sig;
         }
         dTableSet.Tables["ServerFaults"].AcceptChanges();

         ctx.ConsoleWriteLogLine(string.Format(
            "BVTServerView: {0} teller-session requests, {1} distinct fault signatures.",
            _reqOrder.Count, _faultOrder.Count));
      }

      private Req GetReq(string asset, string id, DateTime now)
      {
         string key = asset + "#" + id;
         Req r;
         if (!_reqs.TryGetValue(key, out r))
         {
            r = new Req { Asset = asset, Id = id, Seen = now, File = _file };
            _reqs[key] = r; _reqOrder.Add(key);
         }
         return r;
      }

      private static string Signature(string body)
      {
         string s = reGuid.Replace(body, "<guid>");
         s = reUri.Replace(s, "<uri>");
         s = reNum.Replace(s, "#");
         s = s.Trim();
         return s.Length > 140 ? s.Substring(0, 140) : s;
      }
   }
}
