using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace IDCView_Flat
{
   /// <summary>
   /// Flat IDC (motorized card reader) view for Diebold-Nixdorf machines. Two sheets:
   ///
   ///   Status - card-reader health timeline: device / media / type (a row when a value changes).
   ///   Cards  - card-operation log from CIDCService::HandleXFSResult (FireXFSEvent [dwCommandCode,
   ///            hResult]) and the ChipIO events - what the reader did and whether it faulted.
   ///
   /// Fed by SPFlatDeviceLine; filters to Device == "IDC".
   /// </summary>
   internal class IDCTable_Flat : BaseTable
   {
      private static readonly string[] StatusCols = { "device", "media", "type" };
      private readonly Dictionary<string, string> _cur = new Dictionary<string, string>();

      public IDCTable_Flat(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            SPFlatDeviceLine dl = logLine as SPFlatDeviceLine;
            if (dl == null || dl.Device != "IDC")
            {
               return;
            }

            string m = dl.DeviceMethod;
            string p = dl.DevicePayload;

            switch (m)
            {
               case "Ctrl::GetDeviceStatus": UpdateStatus(dl, "device", StatVal(p)); break;
               case "Ctrl::GetMediaStatus":  UpdateStatus(dl, "media",  StatVal(p)); break;
               case "Ctrl::GetDeviceType":   UpdateStatus(dl, "type",   StatVal(p)); break;
               default:
                  if (m.IndexOf("HandleXFSResult", StringComparison.OrdinalIgnoreCase) >= 0 && p.Contains("FireXFSEvent"))
                  {
                     AddCardOp(dl, p);
                  }
                  else if (m == "Ctrl::ChipIO" && p.Contains("Invoked"))
                  {
                     AddChipIO(dl, p);
                  }
                  break;
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("IDCTable_Flat.ProcessRow EXCEPTION: " + e.Message);
         }
      }

      private void UpdateStatus(SPFlatDeviceLine dl, string col, string val)
      {
         string old;
         if (_cur.TryGetValue(col, out old) && old == val)
         {
            return;
         }
         _cur[col] = val;

         try
         {
            DataRow row = dTableSet.Tables["Status"].Rows.Add();
            row["file"] = dl.LogFile;
            row["time"] = dl.Timestamp;
            row["error"] = dl.HResult;
            foreach (string c in StatusCols)
            {
               string v;
               row[c] = _cur.TryGetValue(c, out v) ? v : "";
            }
            row["comment"] = col + " -> " + val;
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("IDCTable_Flat.UpdateStatus Exception: " + e.Message);
         }
      }

      private void AddCardOp(SPFlatDeviceLine dl, string payload)
      {
         try
         {
            Match c = Regex.Match(payload, @"dwCommandCode=(-?\d+)");
            Match h = Regex.Match(payload, @"hResult=(-?\d+)");
            string result = (h.Success && h.Groups[1].Value != "0") ? h.Groups[1].Value : "";

            DataRow row = dTableSet.Tables["Cards"].Rows.Add();
            row["file"] = dl.LogFile;
            row["time"] = dl.Timestamp;
            row["error"] = result;
            row["op"] = c.Success ? CommandName(c.Groups[1].Value) : "xfs result";
            row["comment"] = "";
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("IDCTable_Flat.AddCardOp Exception: " + e.Message);
         }
      }

      private void AddChipIO(SPFlatDeviceLine dl, string payload)
      {
         try
         {
            Match t = Regex.Match(payload, @"Token\[([^\]]*)\]");
            DataRow row = dTableSet.Tables["Cards"].Rows.Add();
            row["file"] = dl.LogFile;
            row["time"] = dl.Timestamp;
            row["error"] = dl.HResult;
            row["op"] = "chip io";
            row["comment"] = t.Success ? ("token " + t.Groups[1].Value) : "";
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("IDCTable_Flat.AddChipIO Exception: " + e.Message);
         }
      }

      public override void PostProcess()
      {
         try { dTableSet.AcceptChanges(); }
         catch (Exception e) { ctx.ConsoleWriteLogLine("IDCTable_Flat.PostProcess Exception: " + e.Message); }
         base.PostProcess();
      }

      /// <summary>Name an XFS command code from the Messages lookup (type "dwCommandCode"); falls back
      /// to "command N". Data-driven - add codes to IDCView_Flat.xml, no recompile. Uses Select (not
      /// FindMessages) so the Messages table needs no primary key.</summary>
      private string CommandName(string code)
      {
         try
         {
            DataRow[] r = dTableSet.Tables["Messages"].Select("type = 'dwCommandCode' AND code = '" + code + "'");
            if (r.Length > 0 && r[0]["brief"] != null && r[0]["brief"] != System.DBNull.Value)
            {
               string name = r[0]["brief"].ToString();
               if (!string.IsNullOrEmpty(name))
               {
                  return name;
               }
            }
         }
         catch (Exception)
         {
         }
         return "command " + code;
      }

      /// <summary>Extract a status value from either "Name[VALUE]" / "Name[i][VALUE]" or "Name = VALUE".</summary>
      private static string StatVal(string s)
      {
         if (s.IndexOf('[') >= 0)
         {
            MatchCollection ms = Regex.Matches(s, @"\[([^\]]*)\]");
            return ms.Count > 0 ? ms[ms.Count - 1].Groups[1].Value.Trim() : "";
         }
         int eq = s.LastIndexOf('=');
         return (eq >= 0) ? s.Substring(eq + 1).Trim() : s.Trim();
      }
   }
}
