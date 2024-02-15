using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class ConnectionManager : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "ConnectionManager";
      private bool isRecognized = false;


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
               SettingDict.Add("SignalRConnectionState", "REGISTERED");
               isRecognized = true;
            }

            subtag = "ActiveTeller connection state change Connecting";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("SignalRConnectionState", "CONNECTING");
               isRecognized = true;
            }

            subtag = "Setting teller availability to Busy";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("TellerAvailabilityStatus", "BUSY");
               isRecognized = true;
            }

            subtag = "ActiveTeller connection thread is stopping";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("SignalRConnectionState", "CONNECTION THREAD IS STOPPING");
               isRecognized = true;
            }

            subtag = "ActiveTeller connection thread has completed.";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("SignalRConnectionState", "CONNECTION THREAD COMPLETED");
               isRecognized = true;
            }

            subtag = "ActiveTeller connection thread has started.";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("SignalRConnectionState", "CONNECTION THREAD STARTED");
               isRecognized = true;
            }

            subtag = "Attempting to start the ActiveTeller connection";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("SignalRConnectionState", "CONNECTION THREAD STARTING");
               isRecognized = true;
            }

            subtag = "Request connection...";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("SignalRConnectionState", "REQUEST CONNECTION");
               isRecognized = true;
            }

            subtag = "Setting teller availability to Available";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("TellerAvailabilityStatus", "AVAILABLE");
               isRecognized = true;
            }

            Regex regex = new Regex("ActiveTeller connection (?<guid>.*) connected to (?<url>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("SignalRConnectionState", "CONNECTED");
               SettingDict.Add("ConnectionGUID", m.Groups["guid"].Value);
               SettingDict.Add("URL", m.Groups["url"].Value);
               isRecognized = true;
            }

            regex = new Regex("Thread Run has started. Requesting connection to (?<url>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("SignalRConnectionState", "CONNECTION REQUESTED");
               SettingDict.Add("URL", m.Groups["url"].Value);
               isRecognized = true;
            }

            regex = new Regex("Registering the connection to the server for client session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("SignalRConnectionState", "REGISTERING SERVER CONNECTION");
               SettingDict.Add("SessionId", m.Groups["sessionid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Sending teller session for request (?<requestid>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerAssistSessionState", "TELLER SESSION SENT FOR REQUEST");
               SettingDict.Add("RequestId", m.Groups["requestid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Sending remote control session for teller session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerAssistSessionState", "REMOTE CONTROL SESSION SENT");
               SettingDict.Add("SessionId", m.Groups["sessionid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Sending assist session for teller session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerAssistSessionState", "ASSIST SESSION SENT");
               SettingDict.Add("SessionId", m.Groups["sessionid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Sending approval response for teller session (?<sessionid>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerAssistSessionState", "APPROVAL RESPONSE SENT");
               SettingDict.Add("SessionId", m.Groups["sessionid"].Value);
               isRecognized = true;
            }

            regex = new Regex("ActiveTeller connection (?<guid>.*) disconnected");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("SignalRConnectionState", "DISCONNECTED");
               SettingDict.Add("ConnectionGUID", m.Groups["guid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Remote control session (?<sessionid>.*) created");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("RemoteControlSessionState", "CREATED");
               SettingDict.Add("SessionId", m.Groups["sessionid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Deleting remote control session (?<sessionid>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("RemoteControlSessionState", "DELETING");
               SettingDict.Add("SessionId", m.Groups["sessionid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Failed retrieving image from uri (?<uri>.*) with status (?<status>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("HttpImageRetrievalState", "FAILED TO RETRIEVE IMAGE");
               SettingDict.Add("URI", m.Groups["uri"].Value);
               SettingDict.Add("Status", m.Groups["status"].Value);
               isRecognized = true;
            }

            regex = new Regex("Retrieving image with uri (?<uri>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("HttpImageRetrievalState", "RETRIEVING IMAGE");
               SettingDict.Add("URI", m.Groups["uri"].Value);
               isRecognized = true;
            }

            regex = new Regex("Request teller activities for uri (?<uri>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("HttpServerRequest", "REQUEST TELLER ACTIVITIES");
               SettingDict.Add("URI", m.Groups["uri"].Value);
               isRecognized = true;
            }

            regex = new Regex("Update teller session statistics: Pending=(?<pending>.*) Current=(?<current>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerAssistSessionState", "UPDATE TELLER STATISTICS");
               SettingDict.Add("Pending", m.Groups["pending"].Value);
               SettingDict.Add("Current", m.Groups["current"].Value);
               isRecognized = true;
            }

            regex = new Regex("Updating teller session statistics subscription: (?<state>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerAssistSessionState", "UPDATE TELLER STATISTICS SUBSCRIPTION");
               SettingDict.Add("SubscriptionState", m.Groups["state"].Value);
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
