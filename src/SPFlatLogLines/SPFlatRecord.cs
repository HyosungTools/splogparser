using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LogLineHandler
{
   /// <summary>
   /// Generic decoder for ONE Diebold-Nixdorf / Nextware "flat" nwlog record.
   ///
   /// A record is a run of length-prefixed ASCII fields: NNNN&lt;value&gt; where NNNN is a 4-digit
   /// decimal length and &lt;value&gt; is exactly that many characters. The record's own fields, in
   /// order, are:
   ///
   ///   [DEVICE(3) SOURCE("ACTIVEX")]  DATE("yyyy/MM/dd")  TIME("HH:MM SS.mmm")
   ///   CATEGORY  METHOD  PAYLOAD  [trailer: hResult sentinel + seq ...]
   ///
   /// CATEGORY is PROPERTY / INFORMATION / METHOD / EVENT / XFSAPI / ERROR. METHOD is a Ctrl::Xxx
   /// property/method or a service-handler symbol (CCdmService::HandleX,
   /// CLogicalUnit::TraceCIMCashUnitInfo, ...). PAYLOAD is Name[value] or Name[(v1)(v2)(v3)] arrays.
   /// TIME uses a space where the second ':' would be: "HH:MM SS.mmm".
   ///
   /// FRAMING - this decoder is robust to both framings, which is why it can be introduced before the
   /// handler is switched to device-anchoring:
   ///   * device-anchored  : record starts with 0003&lt;DEV&gt;0007ACTIVEX, then the date. DEVICE known.
   ///   * timestamp-framed  : record starts at the date; the 0003&lt;DEV&gt;0007ACTIVEX that appears LATER
   ///                         belongs to the NEXT record, so it is NOT this record's device -&gt; "?".
   /// The decoder therefore anchors on the record's OWN timestamp (always the first date in the line)
   /// and reads DEVICE only from an anchor that sits immediately BEFORE that date. CATEGORY/METHOD/
   /// PAYLOAD come from the fields after the timestamp and are correct in either framing.
   ///
   /// Standalone (no ILogFileHandler / ILogLine dependency) so it can be unit-tested in isolation and
   /// reused by SPFlatLine.Factory for routing.
   /// </summary>
   public class SPFlatRecord
   {
      /// <summary>Device tag: CDM, CIM, IPM, IDC, PIN, SPR, VDM, COD, DEP, JPR ... or "?" if the line
      /// is timestamp-framed (device tag not attributable without the handler reframing).</summary>
      public string Device { get; private set; } = "?";
      public string Source { get; private set; } = "";
      public string Date { get; private set; } = "";
      public string Time { get; private set; } = "";
      public string Category { get; private set; } = "";
      public string Method { get; private set; } = "";
      public string Payload { get; private set; } = "";

      /// <summary>The length-prefixed fields AFTER the timestamp, in order (CATEGORY, METHOD, PAYLOAD,
      /// then trailer fields). Diagnostic / for callers that need more than the named fields.</summary>
      public List<string> Fields { get; } = new List<string>();

      /// <summary>True once we decoded at least through the METHOD field.</summary>
      public bool Ok { get; private set; }

      // The record's own timestamp: date(10) + 4-digit length-prefix + time(12),
      // e.g. 2026/07/10 0012 10:15 01.445  ->  "2026/07/10001210:15 01.445".
      private static readonly Regex TimestampRegex =
         new Regex(@"\d{4}/\d{2}/\d{2}\d{4}\d{2}:\d{2} \d{2}\.\d{3}", RegexOptions.Compiled);

      // Device envelope that precedes the date in device-anchored framing: 0003<DEV>0007ACTIVEX.
      private static readonly Regex AnchorRegex =
         new Regex(@"0003([A-Z]{3})0007ACTIVEX", RegexOptions.Compiled);

      private static readonly Regex NormalTsRegex =
         new Regex(@"(\d{4})/(\d{2})/(\d{2}) (\d{2}):(\d{2}) (\d{2})\.(\d{3})", RegexOptions.Compiled);

      /// <summary>Decode one record substring. Never throws; Ok=false if there is no timestamp to
      /// anchor on or the record is truncated before the method.</summary>
      public static SPFlatRecord Decode(string record)
      {
         var r = new SPFlatRecord();
         if (string.IsNullOrEmpty(record))
         {
            return r;
         }

         Match ts = TimestampRegex.Match(record);
         if (!ts.Success)
         {
            return r;
         }

         // The matched timestamp text is date(10) + lenPrefix(4) + time(12). Split it directly so we
         // never depend on whether the date carries its own length prefix in this particular framing.
         string tsText = ts.Value;
         r.Date = tsText.Substring(0, 10);
         r.Time = (tsText.Length >= 14) ? tsText.Substring(14) : "";

         // TLV-walk the fields AFTER the timestamp: CATEGORY, METHOD, PAYLOAD, then trailer, until a
         // non-numeric length prefix (end of the ASCII record / the binary framing rendered as spaces).
         int p = ts.Index + ts.Length;
         while (p + 4 <= record.Length && r.Fields.Count < 10)
         {
            string lenStr = record.Substring(p, 4);
            if (!IsAllDigits(lenStr))
            {
               break;
            }
            int n = int.Parse(lenStr);
            p += 4;
            if (n == 0 || p + n > record.Length)
            {
               break;
            }
            r.Fields.Add(record.Substring(p, n));
            p += n;
         }

         r.Category = FieldAt(r, 0);
         r.Method = FieldAt(r, 1);
         r.Payload = FieldAt(r, 2);

         // Device: only an anchor that sits BEFORE this record's date is this record's device. Take the
         // last such anchor (closest to the date). Timestamp-framed lines have none -> Device stays "?".
         string pre = record.Substring(0, ts.Index);
         MatchCollection anchors = AnchorRegex.Matches(pre);
         if (anchors.Count > 0)
         {
            r.Device = anchors[anchors.Count - 1].Groups[1].Value;
            r.Source = "ACTIVEX";
         }

         r.Ok = r.Fields.Count >= 2;   // through METHOD
         return r;
      }

      /// <summary>Normal-form timestamp "yyyy-MM-dd HH:MM:SS.mmm" from Date + Time, or "" if unparseable.</summary>
      public string NormalTimestamp()
      {
         Match m = NormalTsRegex.Match(Date + " " + Time);
         if (!m.Success)
         {
            return "";
         }
         return m.Groups[1].Value + "-" + m.Groups[2].Value + "-" + m.Groups[3].Value + " " +
                m.Groups[4].Value + ":" + m.Groups[5].Value + ":" + m.Groups[6].Value + "." + m.Groups[7].Value;
      }

      private static string FieldAt(SPFlatRecord r, int i)
      {
         return (i < r.Fields.Count) ? r.Fields[i] : "";
      }

      private static bool IsAllDigits(string s)
      {
         if (s.Length == 0)
         {
            return false;
         }
         foreach (char c in s)
         {
            if (c < '0' || c > '9')
            {
               return false;
            }
         }
         return true;
      }
   }
}
