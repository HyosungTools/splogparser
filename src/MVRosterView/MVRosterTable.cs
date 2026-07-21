using System;
using System.Collections.Generic;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace MVRosterView
{
   /// <summary>
   /// One row per distinct ATM (keyed by IP): the "how many machines, and which ones are
   /// misbehaving" rollup. Sessions are folded across ProcessRow calls into a per-IP aggregate
   /// and flushed to rows in PostProcess (before the base writes the XML).
   ///
   /// FIRST-CUT aggregation - deliberately simple so it can be validated against a live run:
   ///   sessions = count of inbound Connected state lines (JobID=0, Status=Connected)
   ///   ok       = count of EOTCompleted state lines
   ///   failed   = count of WARN/ERROR lines (with a classified Kind) attributable to this IP,
   ///              plus any ErrorOccurred state lines
   ///   lastResult / comment are derived from those counts.
   /// Lines that carry a terminal id but no IP (e.g. the bare "not yet registered" warning) are
   /// still flagged because their partner line - "Attempting to register ATM {tid} at {ip}" -
   /// does carry the IP and lands on the same aggregate.
   /// </summary>
   class MVRosterTable : BaseTable
   {
      private const string TABLE_NAME = "Roster";

      /// <summary>Per-machine running aggregate.</summary>
      private class Agg
      {
         public string LastFile { get; set; }
         public string IP { get; set; }
         public int ATMID { get; set; }
         public string Terminal { get; set; }
         public string FirstSeen { get; set; }
         public string LastSeen { get; set; }
         public int Sessions { get; set; }
         public int Ok { get; set; }
         public int Failed { get; set; }
         public string LastResult { get; set; }

         public Agg()
         {
            LastFile = "";
            IP = "";
            ATMID = 0;
            Terminal = "";
            FirstSeen = "";
            LastSeen = "";
            Sessions = 0;
            Ok = 0;
            Failed = 0;
            LastResult = "OK";
         }
      }

      private readonly Dictionary<string, Agg> _byIp = new Dictionary<string, Agg>();

      public MVRosterTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Fold one line into the per-IP aggregate. Rows are not emitted here - see PostProcess.
      /// </summary>
      /// <param name="logLine">the parsed log line</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is MVServerLine mvLine)
            {
               string ip = mvLine.IP;
               if (string.IsNullOrEmpty(ip))
               {
                  // No machine we can attribute this line to - skip for the roster.
                  return;
               }

               Agg agg = GetAgg(ip);
               agg.LastFile = mvLine.LogFile;
               UpdateSeen(agg, mvLine.Timestamp);

               if (mvLine.HasState)
               {
                  if (mvLine.ATMID != 0)
                  {
                     agg.ATMID = mvLine.ATMID;
                  }

                  if (mvLine.JobID == 0 && mvLine.Status == "Connected")
                  {
                     agg.Sessions++;
                  }

                  if (mvLine.Status == "EOTCompleted")
                  {
                     agg.Ok++;
                  }

                  if (mvLine.Status == "ErrorOccurred")
                  {
                     agg.Failed++;
                     agg.LastResult = "Error";
                  }
               }

               if (!string.IsNullOrEmpty(mvLine.TerminalId))
               {
                  agg.Terminal = mvLine.TerminalId;
               }

               if (!string.IsNullOrEmpty(mvLine.Kind))
               {
                  agg.LastResult = mvLine.Kind;
                  if (mvLine.Level == "WARN" || mvLine.Level == "ERROR")
                  {
                     agg.Failed++;
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("MVRosterTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Flush the per-IP aggregates to one row each. Runs before base.WriteXmlFile().
      /// </summary>
      public override void PostProcess()
      {
         try
         {
            foreach (KeyValuePair<string, Agg> pair in _byIp)
            {
               Agg agg = pair.Value;

               DataRow dataRow = dTableSet.Tables[TABLE_NAME].Rows.Add();

               dataRow["file"] = agg.LastFile;
               dataRow["time"] = agg.LastSeen;
               dataRow["atmid"] = (agg.ATMID == 0) ? "" : agg.ATMID.ToString();
               dataRow["terminal"] = agg.Terminal;
               dataRow["ip"] = agg.IP;
               dataRow["firstseen"] = agg.FirstSeen;
               dataRow["sessions"] = agg.Sessions.ToString();
               dataRow["ok"] = agg.Ok.ToString();
               dataRow["failed"] = agg.Failed.ToString();
               dataRow["lastresult"] = (agg.Failed == 0) ? "OK" : agg.LastResult;
               dataRow["comment"] = BuildComment(agg);

               dTableSet.Tables[TABLE_NAME].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("MVRosterTable.PostProcess Exception : " + e.Message);
         }

         base.PostProcess();
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

      private void UpdateSeen(Agg agg, string timestamp)
      {
         if (string.IsNullOrEmpty(timestamp))
         {
            return;
         }

         // Files are not processed in chronological order (e.g. a May capture can be read before
         // a March one), so compare rather than assume arrival order. The timestamp format is
         // fixed-width yyyy-MM-dd HH:mm:ss.ffff, so an ordinal string compare is chronological.
         if (string.IsNullOrEmpty(agg.FirstSeen) || string.CompareOrdinal(timestamp, agg.FirstSeen) < 0)
         {
            agg.FirstSeen = timestamp;
         }

         if (string.IsNullOrEmpty(agg.LastSeen) || string.CompareOrdinal(timestamp, agg.LastSeen) > 0)
         {
            agg.LastSeen = timestamp;
         }
      }

      private string BuildComment(Agg agg)
      {
         if (agg.Failed == 0)
         {
            return "Healthy - " + agg.Sessions.ToString() + " clean cycle(s)";
         }

         return "Problem - " + agg.Failed.ToString() + " failure(s), last result: " + agg.LastResult;
      }
   }
}
