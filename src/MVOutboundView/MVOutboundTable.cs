using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Contract;
using Impl;
using LogLineHandler;

namespace MVOutboundView
{
   /// <summary>
   /// One row per outbound (server-to-ATM) push job. MoniView has two outbound flavours and this
   /// view unifies them:
   ///
   ///   * Scheduled jobs - announced by "ScheduleLog: Starting job {jobid} for ATM {atmid}" and
   ///     then tracked through their state machine (Connecting -> Connected -> ... -> EOTCompleted,
   ///     with TodoJob/DoneJob step counters). Keyed by JobID.
   ///
   ///   * Connect failures - "OB_MultiStep Outbound Socket Connecting Failed! from IP {ip}" carry
   ///     NO JobID, only an IP. A failing scheduled job first emits a Connecting state line (which
   ///     carries both JobID and IP), so the failure is correlated back to its job via an IP->JobID
   ///     map. A failure whose IP matches no known job becomes a standalone "InstantFail" row so
   ///     nothing is dropped.
   ///
   /// Result is derived, not read: DoneJob >= TodoJob (or Status EOTCompleted) = Completed; a
   /// Connecting job matched by a connect-failure = Failed; anything else was still in flight when
   /// the capture ended = InProgress.
   ///
   /// Rows are folded across ProcessRow calls and flushed in PostProcess (before base writes XML).
   /// </summary>
   class MVOutboundTable : BaseTable
   {
      private const string TABLE_NAME = "Outbound";

      private class Job
      {
         public string JobId { get; set; }
         public string AtmId { get; set; }
         public string IP { get; set; }
         public string LastFile { get; set; }
         public string Scheduled { get; set; }
         public string Connected { get; set; }
         public string LastTime { get; set; }
         public string Status { get; set; }
         public int Todo { get; set; }
         public int Done { get; set; }
         public bool Failed { get; set; }
         public string FailComment { get; set; }

         public Job()
         {
            JobId = ""; AtmId = ""; IP = ""; LastFile = ""; Scheduled = ""; Connected = "";
            LastTime = ""; Status = ""; Todo = 0; Done = 0; Failed = false; FailComment = "";
         }
      }

      private class InstantFail
      {
         public string IP { get; set; }
         public string LastFile { get; set; }
         public string First { get; set; }
         public string Last { get; set; }
         public int Attempts { get; set; }
         public string Comment { get; set; }

         public InstantFail()
         {
            IP = ""; LastFile = ""; First = ""; Last = ""; Attempts = 0; Comment = "";
         }
      }

      private readonly Dictionary<string, Job> _jobs = new Dictionary<string, Job>();
      private readonly Dictionary<string, string> _ipToJob = new Dictionary<string, string>();
      private readonly Dictionary<string, InstantFail> _instantFails = new Dictionary<string, InstantFail>();

      private static readonly Regex StartingJobRegex = new Regex(@"Starting job (\d+) for ATM (\d+)");

      public MVOutboundTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is MVServerLine mvLine)
            {
               // 1. Scheduled-job announcement (ScheduleLog).
               if (mvLine.Category == "ScheduleLog")
               {
                  Match sj = StartingJobRegex.Match(mvLine.Message);
                  if (sj.Success)
                  {
                     Job job = GetJob(sj.Groups[1].Value);
                     job.AtmId = sj.Groups[2].Value;
                     job.LastFile = mvLine.LogFile;
                     if (string.IsNullOrEmpty(job.Scheduled))
                     {
                        job.Scheduled = mvLine.Timestamp;
                     }
                  }

                  return;
               }

               // 2. Outbound state line (JobID != 0).
               if (mvLine.HasState && mvLine.JobID != 0)
               {
                  Job job = GetJob(mvLine.JobID.ToString());
                  job.LastFile = mvLine.LogFile;
                  if (mvLine.ATMID != 0)
                  {
                     job.AtmId = mvLine.ATMID.ToString();
                  }
                  job.IP = mvLine.IP;
                  job.Status = mvLine.Status;
                  job.Todo = mvLine.TodoJob;
                  job.Done = mvLine.DoneJob;
                  job.LastTime = mvLine.Timestamp;

                  if (mvLine.Status == "Connecting")
                  {
                     _ipToJob[mvLine.IP] = job.JobId;
                  }

                  if (mvLine.Status == "Connected" && string.IsNullOrEmpty(job.Connected))
                  {
                     job.Connected = mvLine.Timestamp;
                  }

                  return;
               }

               // 3. Outbound connect failure (WARN, carries an IP but no JobID).
               if (mvLine.Level == "WARN" && IsOutboundFailKind(mvLine.Kind) && !string.IsNullOrEmpty(mvLine.IP))
               {
                  string jobId;
                  if (_ipToJob.TryGetValue(mvLine.IP, out jobId) && _jobs.ContainsKey(jobId))
                  {
                     Job job = _jobs[jobId];
                     job.Failed = true;
                     job.FailComment = mvLine.Comment;
                     job.LastTime = mvLine.Timestamp;
                     job.LastFile = mvLine.LogFile;
                  }
                  else
                  {
                     InstantFail fail = GetInstantFail(mvLine.IP);
                     fail.LastFile = mvLine.LogFile;
                     fail.Attempts++;
                     fail.Comment = mvLine.Comment;
                     if (string.IsNullOrEmpty(fail.First))
                     {
                        fail.First = mvLine.Timestamp;
                     }
                     fail.Last = mvLine.Timestamp;
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("MVOutboundTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      public override void PostProcess()
      {
         try
         {
            foreach (KeyValuePair<string, Job> pair in _jobs)
            {
               Job job = pair.Value;
               DataRow row = dTableSet.Tables[TABLE_NAME].Rows.Add();
               row["file"] = job.LastFile;
               row["time"] = string.IsNullOrEmpty(job.LastTime) ? job.Scheduled : job.LastTime;
               row["jobid"] = job.JobId;
               row["atmid"] = job.AtmId;
               row["ip"] = job.IP;
               row["scheduled"] = job.Scheduled;
               row["connected"] = job.Connected;
               row["steps"] = job.Done.ToString() + "/" + job.Todo.ToString();
               row["status"] = job.Status;
               row["result"] = Result(job);
               row["comment"] = Comment(job);
               dTableSet.Tables[TABLE_NAME].AcceptChanges();
            }

            foreach (KeyValuePair<string, InstantFail> pair in _instantFails)
            {
               InstantFail fail = pair.Value;
               DataRow row = dTableSet.Tables[TABLE_NAME].Rows.Add();
               row["file"] = fail.LastFile;
               row["time"] = fail.Last;
               row["jobid"] = "";
               row["atmid"] = "";
               row["ip"] = fail.IP;
               row["scheduled"] = fail.First;
               row["connected"] = "";
               row["steps"] = "";
               row["status"] = "ConnectFailed";
               row["result"] = "Failed";
               row["comment"] = fail.Attempts.ToString() + " attempt(s) - " + fail.Comment;
               dTableSet.Tables[TABLE_NAME].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("MVOutboundTable.PostProcess Exception : " + e.Message);
         }

         base.PostProcess();
      }

      private bool IsOutboundFailKind(string kind)
      {
         if (string.IsNullOrEmpty(kind))
         {
            return false;
         }

         return kind == "OutboundFailed" || kind == "OutboundRefused" || kind == "OutboundTimeout";
      }

      private string Result(Job job)
      {
         if (job.Todo > 0 && job.Done >= job.Todo)
         {
            return "Completed";
         }

         if (job.Status == "EOTCompleted")
         {
            return "Completed";
         }

         if (job.Failed)
         {
            return "Failed";
         }

         if (string.IsNullOrEmpty(job.Status))
         {
            return "Scheduled";
         }

         return "InProgress";
      }

      private string Comment(Job job)
      {
         if (job.Failed)
         {
            return string.IsNullOrEmpty(job.FailComment) ? "Outbound connect failed" : job.FailComment;
         }

         if (job.Todo > 0 && job.Done >= job.Todo)
         {
            return "Pushed " + job.Todo.ToString() + " step(s) successfully";
         }

         if (string.IsNullOrEmpty(job.Status))
         {
            return "Scheduled - no connection attempt seen in this capture";
         }

         return "In flight at end of capture (last status: " + job.Status + ")";
      }

      private Job GetJob(string jobId)
      {
         Job job;
         if (!_jobs.TryGetValue(jobId, out job))
         {
            job = new Job();
            job.JobId = jobId;
            _jobs[jobId] = job;
         }

         return job;
      }

      private InstantFail GetInstantFail(string ip)
      {
         InstantFail fail;
         if (!_instantFails.TryGetValue(ip, out fail))
         {
            fail = new InstantFail();
            fail.IP = ip;
            _instantFails[ip] = fail;
         }

         return fail;
      }
   }
}
