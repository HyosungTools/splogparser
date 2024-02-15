using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class DataFlowManager : AWLine
   {
      public Dictionary<string, string> SettingDict = new Dictionary<string, string>();


      private string className = "DataFlowManager";
      private bool isRecognized = false;


      public DataFlowManager(ILogFileHandler parent, string logLine, AWLogType awType = AWLogType.DataFlowManager) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         string tag = "[DataFlowManager     ]";

         int idx = logLine.IndexOf(tag);
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx+tag.Length);

            //Firing system settings changed event

            //No event handlers were found during attempt to fire ActiveTeller connection state changed event.

            //Teller session statistics update received

            //ActiveTeller connection state changed to Connected
            //ActiveTeller connection state changed to Registered

            // transaction review request 134370 for asset TX005020 added
            // transaction review request 134646 for asset TX005006 updated

            //Application state Identification update for asset TX005018 during  for mode Standard
            //Application state Identification update for asset TX005018 during CustomerIdentification for mode Standard
            //Application state Idle update for asset TX005018 during  for mode Standard
            //Application state InTransaction update for asset NM000562 during CheckDeposit for mode Standard
            //Application state InTransaction update for asset TX005006 during CashDeposit for mode Standard
            //Application state InTransaction update for asset TX005009 during CheckDeposit for mode Standard
            //Application state InTransaction update for asset TX005011 during  for mode Standard
            //Application state InTransaction update for asset TX005011 during CashPayment for mode Standard
            //Application state InTransaction update for asset TX005018 during  for mode Standard
            //Application state InTransaction update for asset TX005018 during CheckDeposit for mode Standard
            //Application state InTransaction update for asset TX005018 during CustomerIdentification for mode Standard
            //Application state InTransaction update for asset TX005018 during DepositTBD for mode Standard
            //Application state InTransaction update for asset TX005018 during PaymentTBD for mode Standard
            //Application state InTransaction update for asset TX005018 during PinAuthentication for mode Standard
            //Application state InTransaction update for asset TX005018 during TransferPayment for mode Standard
            //Application state InTransaction update for asset TX005018 during Withdrawal for mode Standard
            //Application state InTransaction update for asset TX005020 during Deposit for mode Standard
            //Application state InTransaction update for asset TX005022 during CheckDeposit for mode Standard
            //Application state MainMenu update for asset TX005018 during  for mode Standard

            //Assist request 135043 for asset TX005011 added

            //Check image api/checkimages/41923 available

            //ID Scan image api/IdImages/3377 available

            //Control session 134520 for asset TX005009 updated for  transaction
            //Control session 134647 for asset TX005006 deleted

            //Firing remote control session 134996 started for asset NM000564

            //Received AcceptCashCompleted event for DepositTask 8 for asset TX005006
            //Received CommitDepositCompleted event for CheckCashingTask 6 for asset NM000564
            //Received CommitDepositCompleted event for DepositTask 13 for asset TX005011
            //Received ConfigurationReceived event for ConfigurationQueryTask 6 for asset TX005006
            //Received CustomerInputReceived event for CustomerInputTask 7 for asset TX005006
            //Received IdScanCompleted event for ScanIdTask 2 for asset NM000564
            //Received ItemsInserted event for CheckCashingTask 4 for asset NM000564
            //Received ItemsInserted event for DepositTask 12 for asset TX005011
            //Received ItemsPresented event for CheckCashingTask 6 for asset NM000564
            //Received ItemsTaken event for CheckCashingTask 6 for asset NM000564
            //Received MediaReadCompleted event for CheckCashingTask 4 for asset NM000564
            //Received MediaReadCompleted event for DepositTask 17 for asset TX005014
            //Received TaskCompleted event for PrintReceiptTask 7 for asset NM000564

            //Sending transaction item approval: CheckIndex=0, TellerApproval=Approved, TellerAmount=1700000, Reason=

            //Teller session 21408 for asset TX005208 accepted by Diana.
            //Teller session 21497 for asset TX005011 created
            //Teller session request 27933 for asset TX005009 deleted
            //Teller session request 27934 for asset TX005007

            //Transaction item 0 Approved for amount 10000 with reason 


            string subtag = "Firing system settings changed event";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("EventFired", "System settings changed");
               isRecognized = true;
            }

            subtag = "Teller session statistics update received";
            if (subLogLine.StartsWith(subtag))
            {
               SettingDict.Add("EventFired", "Teller session statistic updated received");
               isRecognized = true;
            }

            Regex regex = new Regex("transaction review request (?<requestid>[\\-0-9]*) for asset (?<asset>\\w\\w[0-9]*) (?<event>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("RequestType", "TransactionReviewRequest");
               SettingDict.Add("RequestId", m.Groups["requestid"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               SettingDict.Add("Event", m.Groups["event"].Value);
               isRecognized = true;
            }

            regex = new Regex("ActiveTeller connection state changed to (?<newstate>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("ActiveTellerConnectionState", m.Groups["newstate"].Value);
               isRecognized = true;
            }

            regex = new Regex("No event handlers were found during attempt to fire (?<event>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("ErrorNoEventHandlersFound", m.Groups["event"].Value);
               isRecognized = true;
            }

            regex = new Regex("Application state (?<type>.*) update for asset (?<asset>\\w\\w[0-9]*) during (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("ActiveTellerConnectionState", m.Groups["type"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               SettingDict.Add("Event", m.Groups["action"].Value);
               isRecognized = true;
            }

            regex = new Regex("Assist request (?<requestid>.*) for asset (?<asset>\\w\\w[0-9]*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("AssistRequestEvent", m.Groups["action"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               SettingDict.Add("RequestId", m.Groups["requestid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Check image (?<uri>.*) available");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("CheckImageStatus", "AVAILABLE");
               SettingDict.Add("URI", m.Groups["uri"].Value);
               isRecognized = true;
            }

            regex = new Regex("ID Scan image (?<uri>.*) available");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("IDScanImageStatus", "AVAILABLE");
               SettingDict.Add("URI", m.Groups["uri"].Value);
               isRecognized = true;
            }

            regex = new Regex("Control session (?<sessionid>.*) for asset (?<asset>\\w\\w[0-9]*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("ControlSessionStatus", m.Groups["action"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               SettingDict.Add("SessionId", m.Groups["sessionid"].Value);
               isRecognized = true;
            }

            regex = new Regex("Firing remote control session (?<sessionid>.*) (?<action>.*) for asset (?<asset>\\w\\w[0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("RemoteControlSessionStatus", m.Groups["action"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               isRecognized = true;
            }

            regex = new Regex("Received (?<event>.*) event for (?<task>.*) (?<val>.*) for asset (?<asset>\\w\\w[0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TaskStatusEvent", m.Groups["event"].Value);
               SettingDict.Add("Task", m.Groups["task"].Value);
               SettingDict.Add("Value", m.Groups["val"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               isRecognized = true;
            }

            regex = new Regex("Sending transaction item approval: CheckIndex=(?<checkindex>.*), TellerApproval=(?<approval>.*), TellerAmount=(?<amount>.*), Reason=(?<reason>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TransactionItemStatus", "APPROVAL");
               SettingDict.Add("CheckIndex", m.Groups["checkindex"].Value);
               SettingDict.Add("TellerApproval", m.Groups["approval"].Value);
               SettingDict.Add("TellerAmount", m.Groups["amount"].Value);
               SettingDict.Add("Reason", m.Groups["reason"].Value);
               isRecognized = true;
            }

            regex = new Regex("Teller session (?<sessionid>[0-9]*) for asset (?<asset>\\w\\w[0-9]*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerSessionRequest", "TELLER SESSION REQUEST FOR ATM");
               SettingDict.Add("RequestId", m.Groups["sessionid"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               SettingDict.Add("Action", m.Groups["action"].Value);
               isRecognized = true;
            }

            regex = new Regex("Teller session request (?<id>[0-9]*) for asset (?<asset>\\w\\w[0-9]*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerSessionRequest", "TELLER SESSION REQUEST FOR ATM");
               SettingDict.Add("RequestId", m.Groups["id"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               SettingDict.Add("Action", m.Groups["action"].Value);
               isRecognized = true;
            }

            regex = new Regex("Teller session request (?<id>[0-9]*) for asset (?<asset>\\w\\w[0-9]*)$");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TellerSessionRequest", "TELLER SESSION REQUEST FOR ATM");
               SettingDict.Add("RequestId", m.Groups["id"].Value);
               SettingDict.Add("ATM", m.Groups["asset"].Value);
               isRecognized = true;
            }

            regex = new Regex("Transaction item (?<item>[0-9]*) (?<action>.*) for amount (?<amount>[0-9]*) with reason (?<reason>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               SettingDict.Add("TransactionItemStatusChange", m.Groups["action"].Value);
               SettingDict.Add("ItemNumber", m.Groups["item"].Value);
               SettingDict.Add("Amount", m.Groups["amount"].Value);
               SettingDict.Add("Reason", m.Groups["reason"].Value);
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
