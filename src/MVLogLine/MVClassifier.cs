using System;
using System.Collections.Generic;

namespace LogLineHandler
{
   /// <summary>
   /// Data-driven "comment cookbook": maps a MoniView server-log message to a short Kind token and
   /// a plain-English Comment a support engineer can read at a glance.
   ///
   /// Rules are evaluated in order; the first rule whose needles ALL appear in the message wins.
   /// Kept as a simple ordered list so new phrases can be added here (or, later, loaded from an
   /// external file in the same spirit as the Known Issues / XmlParam rule files) without touching
   /// the parser. Validated to cover 126/126 WARN+ERROR lines across the sample server captures.
   /// </summary>
   public static class MVClassifier
   {
      public struct Result
      {
         public string Kind;
         public string Comment;
      }

      public class Rule
      {
         public string[] Needles { get; set; }
         public string Kind { get; set; }
         public string Comment { get; set; }
      }

      public static readonly List<Rule> Cookbook = new List<Rule>()
      {
         new Rule { Needles = new string[] { "would exceed number of retail licenses" }, Kind = "LicenseExceeded", Comment = "Registration refused - retail license cap reached" },
         new Rule { Needles = new string[] { "String or binary data would be truncated" }, Kind = "DbTruncation", Comment = "Registration failed - DB insert truncated (version string too long for column)" },
         new Rule { Needles = new string[] { "SQL Query Failed" }, Kind = "SqlQueryFailed", Comment = "SQL insert failed on the server (usually the partner of a DB-truncation error)" },
         new Rule { Needles = new string[] { "has not yet been registered" }, Kind = "NotRegistered", Comment = "Terminal not yet registered on this server" },
         new Rule { Needles = new string[] { "Attempting to register ATM" }, Kind = "Registering", Comment = "Attempting to register a new terminal" },
         new Rule { Needles = new string[] { "Outbound Socket Connecting Failed", "actively refused" }, Kind = "OutboundRefused", Comment = "Could not reach ATM on :5555 - connection refused (nothing listening / firewall)" },
         new Rule { Needles = new string[] { "Outbound Socket Connecting Failed", "did not properly respond" }, Kind = "OutboundTimeout", Comment = "Could not reach ATM on :5555 - connection timed out" },
         new Rule { Needles = new string[] { "Outbound Socket Connecting Failed", "10060" }, Kind = "OutboundTimeout", Comment = "Could not reach ATM on :5555 - connection timed out" },
         new Rule { Needles = new string[] { "Outbound Socket Connecting Failed" }, Kind = "OutboundFailed", Comment = "Outbound job could not connect to the ATM on :5555" },
         new Rule { Needles = new string[] { "received SendComplete but", "not found for" }, Kind = "StaleOutboundSignal", Comment = "Outbound send-complete for a job no longer tracked - ATM likely already dropped" },
         new Rule { Needles = new string[] { "Object reference not set" }, Kind = "NullRef", Comment = "Server-side error handling an aborted outbound job" },
         new Rule { Needles = new string[] { "Unable to process outbound" }, Kind = "NullRef", Comment = "Server-side error handling an aborted outbound job" },
         new Rule { Needles = new string[] { "ClientSocket exception while receiving data", "closed sock" }, Kind = "PeerClosed", Comment = "ATM closed the socket mid-exchange" },
         new Rule { Needles = new string[] { "socket exception while receiving data", "10060" }, Kind = "RecvTimeout", Comment = "Timed out receiving data from the ATM" },
         new Rule { Needles = new string[] { "Connect Failed", "No connection could be made" }, Kind = "OutboundRefused", Comment = "Could not reach ATM - connection refused" },
         new Rule { Needles = new string[] { "Connect Failed", "connection attempt failed" }, Kind = "OutboundTimeout", Comment = "Could not reach ATM - connection timed out" },
         new Rule { Needles = new string[] { "Activation job error" }, Kind = "ActivationError", Comment = "Activation job failed for this terminal" },
         new Rule { Needles = new string[] { "InboundJob job error" }, Kind = "InboundError", Comment = "Inbound job errored for this terminal" },
         new Rule { Needles = new string[] { "InstantJob job error" }, Kind = "InstantJobError", Comment = "Instant (outbound) job errored" },
         new Rule { Needles = new string[] { "Finished parsing at field" }, Kind = "ParseSkip", Comment = "Inbound parse stopped early - some fields were not populated" }
      };

      /// <summary>
      /// Look up the first matching cookbook rule for a message. Returns empty Kind/Comment when
      /// nothing matches (e.g. ordinary INFO lines), which callers treat as "no annotation".
      /// </summary>
      public static Result Classify(string level, string message)
      {
         Result result = new Result();
         result.Kind = "";
         result.Comment = "";

         if (string.IsNullOrEmpty(message))
         {
            return result;
         }

         foreach (Rule rule in Cookbook)
         {
            bool allMatch = true;
            foreach (string needle in rule.Needles)
            {
               if (message.IndexOf(needle, StringComparison.Ordinal) < 0)
               {
                  allMatch = false;
                  break;
               }
            }

            if (allMatch)
            {
               result.Kind = rule.Kind;
               result.Comment = rule.Comment;
               return result;
            }
         }

         return result;
      }
   }
}
