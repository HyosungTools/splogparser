using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class MainWindow : AWLine
   {
      private string className = "MainWindow";

      public string ActiveTellerState { get; private set; } = string.Empty;
      public string Asset { get; private set; } = string.Empty;
      public string VideoSessionState { get; private set; } = string.Empty;

      private bool isRecognized = false;


      public MainWindow(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.MainWindow) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[MainWindow          ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //A new call event from the video control arrived. CanAcceptVideoCall=True, VideoSessonState=SessionConnecting

            //Assistance request information for 

            //BtnEndConference was clicked

            //Disconnected from ActiveTeller
            //Disconnecting from ActiveTeller

            //Done adding teller session for asset TX005022 for teller session request 27950

            //Finished and unloaded Report
            //Finished and unloaded TransactionReview

            //HandleTellerSesssionChanged(Deleted), VideoSessionState=SessionConnecting

            //Loading...

            //MainWindow is closing

            //Moving teller video window to position 3

            //OnInSessionApplicationStateChanged: FlowPoint=
            //OnInSessionApplicationStateChanged: FlowPoint=Common-BufferReceipt

            //PeerToPeer_SignIn. SignIn URI = 192.168.20.144

            //Received RemoteControlSessionChanged event.

            //Remote desktop connected successfully to the asset

            //Signed out from ActiveTeller

            //Starting the ActiveTeller connection because the user is allowed to assist customers

            //Teller session added for asset TX005022 for teller session request 27950

            //Teller session request deleted for teller session request 27945

            //Teller session requested for asset TX005208 for teller session request 28038. IsAcceptable=True. 

            //TransactionReview request information for CashDeposit
            //TransactionReview request information for CheckDeposit

            //Window_Loaded complete

            //Remote desktop encountered an error while connecting to the asset.



            string subtag = "Loading...";
            if (subLogLine.StartsWith(subtag))
            {
               ActiveTellerState = "WINDOW LOADING";
               isRecognized = true;
            }

            subtag = "Window_Loaded complete";
            if (subLogLine.StartsWith(subtag))
            {
               ActiveTellerState = "WINDOW LOAD COMPLETE";
               isRecognized = true;
            }

            subtag = "Remote desktop connected successfully to the asset";
            if (subLogLine.StartsWith(subtag))
            {
               ActiveTellerState = "REMOTE DESKTOP CONNECTED TO THE ATM";
               isRecognized = true;
            }

            subtag = "Starting the ActiveTeller connection because the user is allowed to assist customers";
            if (subLogLine.StartsWith(subtag))
            {
               ActiveTellerState = "STARTING, CAN ASSIST CUSTOMERS";
               isRecognized = true;
            }

            subtag = "Remote desktop encountered an error while connecting to the asset.";
            if (subLogLine.StartsWith(subtag))
            {
               ActiveTellerState = "ERROR, FAILED TO CONNECT TO THE ATM";
               isRecognized = true;
            }

            subtag = "Remote desktop encountered an error while connected to the asset.";
            if (subLogLine.StartsWith(subtag))
            {
               ActiveTellerState = "ERROR, FAILED WHILE CONNECTED TO THE ATM";
               isRecognized = true;
            }

            Regex regex = new Regex("A new call event from the video control arrived. CanAcceptVideoCall=(?<bool>.*), VideoSessonState=(?<state>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"NEW VIDEO CALL, {(bool.Parse(m.Groups["bool"].Value) ? "CAN ACCEPT" : "CANNOT ACCEPT")}";
               VideoSessionState = m.Groups["state"].Value;
               isRecognized = true;
            }

            regex = new Regex("Assistance request information for[ ](?<name>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"ASSISTANCE REQUEST INFORMATION for {m.Groups["name"].Value}" ;
               isRecognized = true;
            }

            regex = new Regex("(?<name>.*) was clicked");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"{m.Groups["name"].Value} BUTTON CLICKED";
               isRecognized = true;
            }

            regex = new Regex("(?<name>.*) is closing");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"{m.Groups["name"].Value} WINDOW CLOSING";
               isRecognized = true;
            }

            regex = new Regex("(?<state>.*) from ActiveTeller");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"STATE CHANGE, {m.Groups["state"].Value} from ActiveTeller";
               isRecognized = true;
            }

            regex = new Regex("Done adding teller session for asset (?<asset>\\w\\w[0-9]*) for teller session request (?<id>[\\-0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"ADDED SESSION for ATM {m.Groups["asset"].Value}, session request id {m.Groups["id"].Value}";
               Asset = m.Groups["asset"].Value;
               isRecognized = true;
            }

            regex = new Regex("Teller session added for asset (?<asset>\\w\\w[0-9]*) for teller session request (?<id>[\\-0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"ADDING SESSION for ATM {m.Groups["asset"].Value}, session request id {m.Groups["id"].Value}";
               Asset = m.Groups["asset"].Value;
               isRecognized = true;
            }

            regex = new Regex("Teller session requested for asset (?<asset>\\w\\w[0-9]*) for teller session request (?<id>[\\-0-9]*). IsAcceptable=(?<bool>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"SESSION REQUESTED for ATM {m.Groups["asset"].Value}, session request id {m.Groups["id"].Value}.  IsAcceptable={m.Groups["bool"].Value}";
               Asset = m.Groups["asset"].Value;
               isRecognized = true;
            }

            regex = new Regex("Teller session request deleted for teller session request (?<id>[\\-0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"SESSION REQUEST DELETED, session request id {m.Groups["id"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Finished and unloaded (?<name>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"FINISHED, UNLOADED {m.Groups["name"].Value}";
               isRecognized = true;
            }

            regex = new Regex("HandleTellerSesssionChanged\\((?<event>.*)\\), VideoSessionState=(?<state>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"SESSION CHANGED, {m.Groups["event"].Value}";
               VideoSessionState = m.Groups["state"].Value;
               isRecognized = true;
            }

            regex = new Regex("Moving teller video window to position (?<num>[\\-0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"MOVING TELLER VIDEO WINDOW to position {m.Groups["num"].Value}";
               isRecognized = true;
            }

            regex = new Regex("OnInSessionApplicationStateChanged: FlowPoint=(?<name>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"SESSION APPLICATION FLOWPOINT CHANGED, {m.Groups["name"].Value}";
               isRecognized = true;
            }

            regex = new Regex("PeerToPeer_SignIn. SignIn URI = (?<uri>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"PEER TO PEER SIGNIN to URI {m.Groups["uri"].Value}";
               isRecognized = true;
            }

            regex = new Regex("Received (?<event>.*) event.");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"RECEIVED EVENT, {m.Groups["event"].Value}";
               isRecognized = true;
            }

            regex = new Regex("TransactionReview request information for (?<name>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerState = $"TRANSACTION REVIEW REQUEST, {m.Groups["name"].Value}";
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
