using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class ConnectionManager : AWLine
   {
      private string className = "ConnectionManager";
      private bool isRecognized = false;

      public string SignalRConnectionState { get; set; } = string.Empty;
      public string ActiveTellerConnectionState { get; set; } = string.Empty;
      public string RemoteControlSessionState { get; set; } = string.Empty;
      public string TellerAvailability { get; set; } = string.Empty;
      public string TellerAssistSessionState { get; set; } = string.Empty;
      public string ServerConnectionState { get; set; } = string.Empty;
      public string HttpImageRetrievalState { get; set; } = string.Empty;
      public string HttpServerRequest { get; set; } = string.Empty;


      public ConnectionManager(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.ConnectionManager) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[ConnectionManager   ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //ActiveTeller connection 25bc99ad-403b-4e63-8ab7-3afe0a942034 connected to http://V00483107/activeteller/SignalR/signalr/
            //ActiveTeller connection 25bc99ad-403b-4e63-8ab7-3afe0a942034 disconnected

            //ActiveTeller connection registered
            //ActiveTeller connection state change Connecting
            //ActiveTeller connection thread has completed.
            //ActiveTeller connection thread has started.

            //Attempting to start the ActiveTeller connection

            //Request connection...

            //Registering the connection to the server for client session 3537.

            //Setting teller availability to Available

            //Thread Run has started. Requesting connection to http://V00483107/activeteller/SignalR

            //Remote control session 134378 created
            //Deleting remote control session 134647

            //Failed retrieving image from uri api/checkimages/41950 with status Gone

            //Retrieving image with uri api/IdImages/3376

            //Sending approval response for teller session 21409
            //Sending assist session for teller session 21497
            //Sending remote control session for teller session 21413
            //Sending teller session for request 28044

            //Request teller activities for uri TellerActivities?userid=21&start=2023-09-13T00:00:00.000-06:00&end=2023-09-13T23:59:59.000-06:00

            //Update teller session statistics: Pending=0 Current=0
            //Updating teller session statistics subscription: enabled

            string subtag = "ActiveTeller connection registered";
            if (subLogLine.StartsWith(subtag))
            {
               SignalRConnectionState = "REGISTERED";
               isRecognized = true;
            }

            subtag = "ActiveTeller connection state change Connecting";
            if (subLogLine.StartsWith(subtag))
            {
               SignalRConnectionState = "CONNECTING";
               isRecognized = true;
            }

            subtag = "Setting teller availability to Busy";
            if (subLogLine.StartsWith(subtag))
            {
               TellerAvailability = "BUSY";
               isRecognized = true;
            }

            subtag = "ActiveTeller connection thread is stopping";
            if (subLogLine.StartsWith(subtag))
            {
               SignalRConnectionState = "STOPPING";
               isRecognized = true;
            }

            subtag = "ActiveTeller connection thread has completed.";
            if (subLogLine.StartsWith(subtag))
            {
               SignalRConnectionState = "COMPLETED";
               isRecognized = true;
            }

            subtag = "ActiveTeller connection thread has started.";
            if (subLogLine.StartsWith(subtag))
            {
               SignalRConnectionState = "STARTED";
               isRecognized = true;
            }

            subtag = "Attempting to start the ActiveTeller connection";
            if (subLogLine.StartsWith(subtag))
            {
               SignalRConnectionState = "STARTING";
               isRecognized = true;
            }

            subtag = "Request connection...";
            if (subLogLine.StartsWith(subtag))
            {
               SignalRConnectionState = "REQUEST CONNECTION";
               isRecognized = true;
            }

            subtag = "Setting teller availability to Available";
            if (subLogLine.StartsWith(subtag))
            {
               TellerAvailability = "AVAILABLE";
               isRecognized = true;
            }

            Regex regex = new Regex("ActiveTeller connection (?<guid>.*) connected to (?<uri>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SignalRConnectionState = $"CONNECTED guid {m.Groups["guid"].Value} to {m.Groups["uri"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Thread Run has started. Requesting connection to (?<uri>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SignalRConnectionState = $"CONNECTION REQUESTED to {m.Groups["uri"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Registering the connection to the server for client session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ServerConnectionState = $"REGISTERING CLIENT SESSION id {m.Groups["sessionid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Sending teller session for request (?<requestid>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerAssistSessionState = $"TELLER SESSION REQUESTED id {m.Groups["requestid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Sending remote control session for teller session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerAssistSessionState = $"REMOTE CONTROL SESSION REQUESTED id {m.Groups["sessionid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Sending assist session for teller session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerAssistSessionState = $"ASSIST SESSION REQUESTED id {m.Groups["sessionid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Sending approval response for teller session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerAssistSessionState = $"APPROVAL SENT FOR SESSION id {m.Groups["sessionid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("ActiveTeller connection (?<guid>.*) disconnected");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SignalRConnectionState = $"DISCONNECTED guid {m.Groups["guid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Remote control session (?<sessionid>.*) created");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               RemoteControlSessionState = $"CREATED SESSION id {m.Groups["sessionid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Deleting remote control session (?<sessionid>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               RemoteControlSessionState = $"DELETING SESSION id {m.Groups["sessionid"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Failed retrieving image from uri (?<uri>.*) with status (?<status>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               HttpImageRetrievalState = $"FAILED uri {m.Groups["uri"].Value}, status {m.Groups["status"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Retrieving image with uri (?<uri>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               HttpImageRetrievalState = $"RETRIEVING uri {m.Groups["uri"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Request teller activities for uri (?<uri>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               HttpServerRequest = $"GET TELLER STATES {m.Groups["uri"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Update teller session statistics: Pending=(?<pending>.*) Current=(?<current>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerAssistSessionState = $"ASSIST STATISTICS UPDATED Pending {m.Groups["pending"].Value}, Current {m.Groups["current"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Updating teller session statistics subscription: (?<state>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerAssistSessionState = $"TELLER ASSIST STATE {m.Groups["state"].Value}";
               isRecognized = true;
            }
         }

         if (!isRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
