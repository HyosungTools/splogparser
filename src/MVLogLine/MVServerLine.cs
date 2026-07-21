using System;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   /// <summary>
   /// Classification of a MoniView server-log line. These names are the switch labels the MV
   /// view Tables use (mirrors how CDMTable switches on SPLine.xfsType). Level (INFO/WARN/ERROR)
   /// is orthogonal to this - a WARN can be a State line, a Register line, etc.
   /// </summary>
   public enum MVLogType
   {
      /* Not a MoniView line we recognize */
      None,

      /* State-machine line: ATMID=..,IP=..,JobID=..,Status=..,Flag=..,TodoJob=..,DoneJob=.. */
      State,

      /* Socket narration: OnSockEventReceived / OutboundProc / InboundProc received <evt> */
      SocketEvent,

      /* InboundProc`1 ATM Connected: <ip> */
      InboundConnected,

      /* CloseJobProcess removed key <guid> ... for <ip> */
      CloseJob,

      /* Registration attempts and 'not yet registered' warnings */
      Register,

      /* RmsBizRule - Retail : inbound data being written to the DB */
      BizRule,

      /* ScheduleLog : Starting job / completed / next due */
      Schedule,

      /* MoniViewServerService : service-level message */
      ServiceError,

      /* RMSAlert */
      Alert,

      /* Recognized grammar but none of the above */
      Other
   }

   /// <summary>
   /// One logical line from a MoniViewServerLog*.txt file.
   /// Grammar: timestamp|threadId|level|category|message
   /// Continuation lines (SQL text, stack traces) are folded onto the end by MVLogHandler and
   /// surfaced here in Detail; Message holds only the first physical line's payload.
   /// </summary>
   public class MVServerLine : LogLine, ILogLine
   {
      public MVLogType mvType { get; set; }

      // ILogLine implementations (mirrors APLine)
      public string Timestamp { get; set; }
      public string HResult { get; set; }

      // grammar fields
      public string ThreadId { get; set; }
      public string Level { get; set; }
      public string Category { get; set; }
      public string Message { get; set; }
      public string Detail { get; set; }

      // state-line fields (HasState is true only when the ATMID=..,IP=.. block is present)
      public bool HasState { get; set; }
      public int ATMID { get; set; }
      public string IP { get; set; }
      public long JobID { get; set; }
      public string Status { get; set; }
      public int Flag { get; set; }
      public int TodoJob { get; set; }
      public int DoneJob { get; set; }

      // resolved during a session (only appears on register / not-registered lines)
      public string TerminalId { get; set; }

      // plain-English classification from the comment cookbook
      public string Kind { get; set; }
      public string Comment { get; set; }

      private static readonly Regex StateRegex =
         new Regex(@"ATMID=(-?\d+),IP=([\d\.]+),JobID=(\d+),Status=(\w+),Flag=(-?\d+),TodoJob=(\d+),DoneJob=(\d+)");

      private static readonly Regex TidQuotedRegex = new Regex(@"Terminal ID = '([^']+)'");
      private static readonly Regex TidRegisterRegex = new Regex(@"Attempting to register ATM (\S+) at");
      private static readonly Regex TimeRegex = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d+");
      private const string IpPattern = @"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";

      // ATM-addressing contexts only. A blanket IPv4 match wrongly grabs config VALUES such as
      // DNS / subnet mask from RmsBizRule "... with value '8.8.8.8'" lines. Tried in order.
      private static readonly Regex[] AtmIpRegexes = new Regex[]
      {
         new Regex(@"Connected:\s*" + IpPattern),
         new Regex(@"from IP\s+" + IpPattern),
         new Regex(@"\bat\s+" + IpPattern),
         new Regex(@"\bfor\s+" + IpPattern),
         new Regex(IpPattern + @":5555")
      };

      public MVServerLine(ILogFileHandler parent, string logLine, MVLogType mvType) : base(parent, logLine)
      {
         this.mvType = mvType;
         Initialize();
      }

      protected virtual void Initialize()
      {
         ThreadId = "";
         Level = "";
         Category = "";
         Message = "";
         Detail = "";
         IP = "";
         Status = "";
         TerminalId = "";
         Kind = "";
         Comment = "";

         Timestamp = tsTimestamp();
         IsValidTimestamp = bCheckValidTimestamp(Timestamp);
         HResult = hResult();

         ParseFields();
      }

      protected override string hResult()
      {
         return "";
      }

      protected override string tsTimestamp()
      {
         string timestamp = LogLine.DefaultTimestamp;

         Match m = TimeRegex.Match(logLine);
         if (m.Success)
         {
            // MoniView already emits 'yyyy-MM-dd HH:mm:ss.ffff', which DateTime.Parse accepts.
            timestamp = m.Groups[0].Value;
         }

         return timestamp;
      }

      /// <summary>
      /// Split the pipe grammar, peel the continuation tail into Detail, extract the state-line
      /// fields and terminal id, and look up the plain-English Kind/Comment from the cookbook.
      /// </summary>
      protected void ParseFields()
      {
         // The handler may have folded continuation lines onto the end, separated by '\n'.
         // Parse the grammar off the first physical line; keep the remainder as Detail.
         string firstLine = logLine;
         int nl = logLine.IndexOf('\n');
         if (nl > -1)
         {
            firstLine = logLine.Substring(0, nl);
            Detail = logLine.Substring(nl + 1).Trim();
         }

         // ts|thread|level|category|message  (message may itself contain '|', so cap the split at 5)
         string[] parts = firstLine.Split(new char[] { '|' }, 5);
         if (parts.Length < 5)
         {
            mvType = MVLogType.None;
            return;
         }

         ThreadId = parts[1].Trim();
         Level = parts[2].Trim();
         Category = parts[3].Trim();
         Message = parts[4].Trim();

         Match sm = StateRegex.Match(Message);
         if (sm.Success)
         {
            HasState = true;
            ATMID = SafeInt(sm.Groups[1].Value);
            IP = sm.Groups[2].Value;
            JobID = SafeLong(sm.Groups[3].Value);
            Status = sm.Groups[4].Value;
            Flag = SafeInt(sm.Groups[5].Value);
            TodoJob = SafeInt(sm.Groups[6].Value);
            DoneJob = SafeInt(sm.Groups[7].Value);
         }

         Match tq = TidQuotedRegex.Match(Message);
         if (tq.Success)
         {
            TerminalId = tq.Groups[1].Value;
         }
         else
         {
            Match tr = TidRegisterRegex.Match(Message);
            if (tr.Success)
            {
               TerminalId = tr.Groups[1].Value;
            }
         }

         // When this is not a state line, lift the ATM IP only from an ATM-addressing context
         // (Connected: x / from IP x / at x / for x / x:5555). This deliberately ignores IPv4
         // values that appear as configuration data on RmsBizRule "with value '...'" lines.
         if (IP == "")
         {
            for (int i = 0; i < AtmIpRegexes.Length; i++)
            {
               Match ipm = AtmIpRegexes[i].Match(Message);
               if (ipm.Success)
               {
                  IP = ipm.Groups[1].Value;
                  break;
               }
            }
         }

         MVClassifier.Result r = MVClassifier.Classify(Level, Message);
         Kind = r.Kind;
         Comment = r.Comment;
      }

      private int SafeInt(string s)
      {
         int v = 0;
         int.TryParse(s, out v);
         return v;
      }

      private long SafeLong(string s)
      {
         long v = 0;
         long.TryParse(s, out v);
         return v;
      }

      /// <summary>True when this state line represents an outbound (server-to-ATM) job.</summary>
      public bool IsOutbound
      {
         get
         {
            return HasState && JobID != 0;
         }
      }

      /// <summary>
      /// Session correlation key. Inbound sessions (ATM dials in, JobID=0) fold by IP; outbound
      /// jobs (server pushes, JobID!=0) fold by JobID. Used by the roster/session view Tables.
      /// </summary>
      public string SessionKey
      {
         get
         {
            if (HasState && JobID != 0)
            {
               return "OUT:" + JobID.ToString();
            }

            return "IN:" + IP;
         }
      }

      /// <summary>
      /// Returns an MVServerLine for any well-formed MoniView line, or null for blank/garbage
      /// (matches APLine.Factory returning null for lines it does not care about).
      /// </summary>
      public static ILogLine Factory(ILogFileHandler logFileHandler, string logLine)
      {
         if (string.IsNullOrEmpty(logLine))
         {
            return null;
         }

         string firstLine = logLine;
         int nl = logLine.IndexOf('\n');
         if (nl > -1)
         {
            firstLine = logLine.Substring(0, nl);
         }

         string[] parts = firstLine.Split(new char[] { '|' }, 5);
         if (parts.Length < 5)
         {
            return null;
         }

         string category = parts[3].Trim();
         string message = parts[4];

         MVLogType mvType = ClassifyType(category, message);
         return new MVServerLine(logFileHandler, logLine, mvType);
      }

      /// <summary>Coarse semantic type from message shape + category.</summary>
      private static MVLogType ClassifyType(string category, string message)
      {
         if (StateRegex.IsMatch(message))
         {
            return MVLogType.State;
         }

         if (message.Contains("ATM Connected:"))
         {
            return MVLogType.InboundConnected;
         }

         if (message.Contains("CloseJobProcess removed key"))
         {
            return MVLogType.CloseJob;
         }

         if (message.Contains("Attempting to register ATM") || message.Contains("has not yet been registered"))
         {
            return MVLogType.Register;
         }

         if (message.Contains("OnSockEventReceived") || message.Contains("OutboundProc") || message.Contains("InboundProc"))
         {
            return MVLogType.SocketEvent;
         }

         switch (category)
         {
            case "ScheduleLog":
               return MVLogType.Schedule;
            case "RmsBizRule - Retail":
               return MVLogType.BizRule;
            case "MoniViewServerService":
               return MVLogType.ServiceError;
            case "RMSAlert":
               return MVLogType.Alert;
            default:
               return MVLogType.Other;
         }
      }
   }
}
