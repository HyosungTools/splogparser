using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace PINView_Flat
{
   /// <summary>
   /// Flat PIN (encrypting PIN pad) view for Diebold-Nixdorf machines. Three sheets:
   ///
   ///   Status - PIN-pad health timeline (device).
   ///   Keys   - the loaded encryption keys: name / use / loaded, from the parallel GetKeyName /
   ///            GetKeyUse / GetKeyLoaded arrays (final state). The gem for crypto diagnostics.
   ///   Ops    - PIN operations: ReadData / CancelReadData and CPINService::HandleXFSResult results.
   ///
   /// Fed by SPFlatDeviceLine; filters to Device == "PIN".
   /// </summary>
   internal class PINTable_Flat : BaseTable
   {
      private readonly Dictionary<string, string> _cur = new Dictionary<string, string>();
      private string[] _keyName = new string[0];
      private string[] _keyUse = new string[0];
      private string[] _keyLoaded = new string[0];

      public PINTable_Flat(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            SPFlatDeviceLine dl = logLine as SPFlatDeviceLine;
            if (dl == null || dl.Device != "PIN")
            {
               return;
            }

            string m = dl.DeviceMethod;
            string p = dl.DevicePayload;

            switch (m)
            {
               case "Ctrl::GetDeviceStatus": UpdateStatus(dl, "device", LastBracket(p)); break;

               case "CPINService::GetKeyName":   _keyName   = ParseArray(p); break;
               case "CPINService::GetKeyUse":    _keyUse    = ParseArray(p); break;
               case "CPINService::GetKeyLoaded": _keyLoaded = ParseArray(p); break;

               case "Ctrl::ReadData":       AddOp(dl, "read data", ""); break;
               case "Ctrl::CancelReadData": AddOp(dl, "cancel read", ""); break;

               default:
                  if (m.IndexOf("HandleXFSResult", StringComparison.OrdinalIgnoreCase) >= 0 && p.Contains("FireXFSEvent"))
                  {
                     Match c = Regex.Match(p, @"dwCommandCode=(-?\d+)");
                     Match h = Regex.Match(p, @"hResult=(-?\d+)");
                     string result = (h.Success && h.Groups[1].Value != "0") ? h.Groups[1].Value : "";
                     AddOp(dl, c.Success ? CommandName(c.Groups[1].Value) : "xfs result", result);
                  }
                  break;
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("PINTable_Flat.ProcessRow EXCEPTION: " + e.Message);
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
            row["device"] = _cur.ContainsKey("device") ? _cur["device"] : "";
            row["comment"] = col + " -> " + val;
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("PINTable_Flat.UpdateStatus Exception: " + e.Message);
         }
      }

      private void AddOp(SPFlatDeviceLine dl, string op, string result)
      {
         try
         {
            DataRow row = dTableSet.Tables["Ops"].Rows.Add();
            row["file"] = dl.LogFile;
            row["time"] = dl.Timestamp;
            row["error"] = string.IsNullOrEmpty(result) ? dl.HResult : result;
            row["op"] = op;
            row["comment"] = "";
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("PINTable_Flat.AddOp Exception: " + e.Message);
         }
      }

      /// <summary>Flush the loaded keys (final state) - one row per key.</summary>
      public override void PostProcess()
      {
         try
         {
            int n = Math.Max(_keyName.Length, Math.Max(_keyUse.Length, _keyLoaded.Length));
            for (int i = 0; i < n; i++)
            {
               DataRow row = dTableSet.Tables["Keys"].Rows.Add();
               row["number"] = (i + 1).ToString();
               row["name"] = (i < _keyName.Length) ? _keyName[i] : "";
               row["use"] = (i < _keyUse.Length) ? _keyUse[i] : "";
               // GetKeyLoaded is often a single (TRUE) flag for the whole set; repeat it per row if so.
               row["loaded"] = (i < _keyLoaded.Length) ? _keyLoaded[i]
                             : (_keyLoaded.Length == 1 ? _keyLoaded[0] : "");
            }
            dTableSet.AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("PINTable_Flat.PostProcess Exception: " + e.Message);
         }

         base.PostProcess();
      }

      /// <summary>Name an XFS command code from the Messages lookup (type "dwCommandCode"); falls back
      /// to "command N". Data-driven - add codes to PINView_Flat.xml, no recompile.</summary>
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

      private static string LastBracket(string s)
      {
         MatchCollection ms = Regex.Matches(s, @"\[([^\]]*)\]");
         return ms.Count > 0 ? ms[ms.Count - 1].Groups[1].Value.Trim() : "";
      }

      private static string[] ParseArray(string s)
      {
         return Regex.Matches(s, @"\(([^)]*)\)")
            .Cast<Match>()
            .Select(m => string.IsNullOrWhiteSpace(m.Groups[1].Value) ? "" : m.Groups[1].Value.Trim())
            .ToArray();
      }
   }
}
