using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace IPMView_Flat
{
   /// <summary>
   /// Flat IPM (check / item processor) view for Diebold-Nixdorf machines. Two sheets:
   ///
   ///   Status - a device-health timeline. One row each time a status field changes value, showing the
   ///            full current snapshot (device / media / acceptor / stacker / ink / toner / shutter).
   ///   Bins   - the media bins: one row per bin (number, type, status, item count), from the parallel
   ///            GetBinType / GetBinStatus / GetBinCount arrays (final state).
   ///
   /// Fed by SPFlatDeviceLine (generic decoded record); filters to Device == "IPM".
   /// </summary>
   internal class IPMTable_Flat : BaseTable
   {
      private static readonly string[] StatusCols = { "device", "media", "acceptor", "stacker", "ink", "toner", "shutter" };

      private readonly Dictionary<string, string> _cur = new Dictionary<string, string>();
      private string[] _binType = new string[0];
      private string[] _binStatus = new string[0];
      private string[] _binCount = new string[0];

      public IPMTable_Flat(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            SPFlatDeviceLine dl = logLine as SPFlatDeviceLine;
            if (dl == null || dl.Device != "IPM")
            {
               return;
            }

            string m = dl.DeviceMethod;
            string p = dl.DevicePayload;

            switch (m)
            {
               case "Ctrl::GetDeviceStatus":   UpdateStatus(dl, "device",   LastBracket(p)); break;
               case "Ctrl::GetMediaStatus":    UpdateStatus(dl, "media",    LastBracket(p)); break;
               case "Ctrl::GetAcceptorStatus": UpdateStatus(dl, "acceptor", LastBracket(p)); break;
               case "Ctrl::GetStackerStatus":  UpdateStatus(dl, "stacker",  LastBracket(p)); break;
               case "Ctrl::GetInkStatus":      UpdateStatus(dl, "ink",      LastBracket(p)); break;
               case "Ctrl::GetTonerStatus":    UpdateStatus(dl, "toner",    LastBracket(p)); break;
               case "Ctrl::GetShutterStatus":  UpdateStatus(dl, "shutter",  LastBracket(p)); break;

               case "Ctrl::GetBinType":   _binType   = ParseArray(p); break;
               case "Ctrl::GetBinStatus": _binStatus = ParseArray(p); break;
               case "Ctrl::GetBinCount":  _binCount  = ParseArray(p); break;
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("IPMTable_Flat.ProcessRow EXCEPTION: " + e.Message);
         }
      }

      /// <summary>Emit a Status row only when a field changes; the row shows the full current snapshot.</summary>
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
            ctx.ConsoleWriteLogLine("IPMTable_Flat.UpdateStatus Exception: " + e.Message);
         }
      }

      /// <summary>Flush the media bins (final state) - one row per bin.</summary>
      public override void PostProcess()
      {
         try
         {
            int n = Math.Max(_binType.Length, Math.Max(_binStatus.Length, _binCount.Length));
            for (int i = 0; i < n; i++)
            {
               DataRow row = dTableSet.Tables["Bins"].Rows.Add();
               row["number"] = (i + 1).ToString();
               row["type"] = (i < _binType.Length) ? _binType[i] : "";
               row["status"] = (i < _binStatus.Length) ? _binStatus[i] : "";
               row["count"] = (i < _binCount.Length) ? _binCount[i] : "";
            }
            dTableSet.AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("IPMTable_Flat.PostProcess Exception: " + e.Message);
         }

         base.PostProcess();
      }

      /// <summary>Content of the LAST [..] group: "DeviceStatus[DEVONLINE]" -> "DEVONLINE";
      /// "ShutterStatus[0][CLOSED]" -> "CLOSED".</summary>
      private static string LastBracket(string s)
      {
         MatchCollection ms = Regex.Matches(s, @"\[([^\]]*)\]");
         return ms.Count > 0 ? ms[ms.Count - 1].Groups[1].Value.Trim() : "";
      }

      /// <summary>Paren-list: "BinType[(MEDIAIN)(RETRACT)]" -> { "MEDIAIN", "RETRACT" }.</summary>
      private static string[] ParseArray(string s)
      {
         return Regex.Matches(s, @"\(([^)]*)\)")
            .Cast<Match>()
            .Select(m => string.IsNullOrWhiteSpace(m.Groups[1].Value) ? "" : m.Groups[1].Value.Trim())
            .ToArray();
      }
   }
}
