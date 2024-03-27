using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Contract;

namespace LogLineHandler
{
   public class DataFlowManager : AWLine
   {
      private string className = "DataFlowManager";

      public string Event { get; private set; } = string.Empty;
      public string ActiveTellerConnectionState { get; private set; } = string.Empty;
      public string Asset { get; private set; } = string.Empty;
      public string AssistRequestEvent { get; private set; } = string.Empty;
      public string CheckImageStatus { get; private set; } = string.Empty;
      public string IDScanImageStatus { get; private set; } = string.Empty;
      public string ControlSessionStatus { get; private set; } = string.Empty;
      public string RemoteControlSessionState { get; private set; } = string.Empty;
      public string TaskStatusEvent { get; private set; } = string.Empty;
      public string TransactionItemStatus { get; private set; } = string.Empty;
      public string TellerSessionRequest { get; private set; } = string.Empty;
      public string SessionRequestId { get; private set; } = string.Empty;
      public string TransactionItemStatusChange { get; private set; } = string.Empty;
      public string TransactionReviewRequest { get; private set; } = string.Empty;



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

            //Customer review request 222810 for asset 12PROS02L added

            //Customer search started
            //Customer search returned 6 customers

            //Doors device state 0 for asset 16CENT01L

            //ID Scan image api/IdImages/3377 available

            //Item Processing Module device state 4 for asset 16CENT01L

            //Control session 134520 for asset TX005009 updated for  transaction
            //Control session 134647 for asset TX005006 deleted

            //Firing remote control session 134996 started for asset NM000564
            //Firing DeviceStateChanged event

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
            //Teller session request 23580 for asset 16CENT04D

            //Transaction item 0 Approved for amount 10000 with reason 

            string subtag = "Firing system settings changed event";
            if (subLogLine.StartsWith(subtag))
            {
               Event = "System settings changed";
               IsRecognized = true;
            }

            subtag = "Firing DeviceStateChanged event";
            if (subLogLine.StartsWith(subtag))
            {
               Event = "Device state changed";
               IsRecognized = true;
            }

            subtag = "Teller session statistics update received";
            if (subLogLine.StartsWith(subtag))
            {
               Event = "Teller session statistic updated received";
               IsRecognized = true;
            }

            Regex regex = new Regex("transaction review request (?<requestid>[\\-0-9]*) for asset (?<asset>.*) (?<event>.*)");
            Match m = regex.Match(subLogLine);
            if (m.Success)
            {
               TransactionReviewRequest = $"request id {m.Groups["requestid"].Value}, for ATM {m.Groups["asset"].Value}, event {m.Groups["event"].Value}";
               Asset = m.Groups["asset"].Value;
               IsRecognized = true;
            }

            regex = new Regex("ActiveTeller connection state changed to (?<newstate>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerConnectionState = m.Groups["newstate"].Value;
               IsRecognized = true;
            }

            regex = new Regex("No event handlers were found during attempt to fire (?<event>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               Event = $"No handler found for event {m.Groups["event"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Application state (?<type>.*) update for asset (?<asset>.*) during (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ActiveTellerConnectionState = $"{m.Groups["type"].Value} updated for ATM {m.Groups["asset"].Value} during {m.Groups["action"].Value}";
               Asset = m.Groups["asset"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Assist request (?<requestid>.*) for asset (?<asset>.*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               AssistRequestEvent = $"{m.Groups["action"].Value} for ATM {m.Groups["asset"].Value}, request id {m.Groups["requestid"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Check image (?<uri>.*) available");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               CheckImageStatus = $"AVAILABLE uri {m.Groups["uri"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Customer review request (?<num>.*)for asset (?<asset>.*) added");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TransactionReviewRequest = $"CUSTOMER REVIEW REQUEST {m.Groups["num"].Value} for ATM {m.Groups["asset"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("ID Scan image (?<uri>.*) available");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               IDScanImageStatus = $"AVAILABLE uri {m.Groups["uri"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Item Processing Module device state (?<state>[0-9]*) for asset (?<asset>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TransactionItemStatus = $"ITEM PROCESSING DEVICE STATE {m.Groups["state"].Value} for ATM {m.Groups["asset"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Control session (?<sessionid>.*) for asset (?<asset>.*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ControlSessionStatus = $"{m.Groups["action"].Value} for ATM {m.Groups["asset"].Value}, session id {m.Groups["sessionid"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Firing remote control session (?<sessionid>.*) (?<action>.*) for asset (?<asset>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               RemoteControlSessionState = $"{m.Groups["action"].Value} for ATM {m.Groups["asset"].Value}, session id {m.Groups["sessionid"].Value}";
               Asset = m.Groups["asset"].Value;
               IsRecognized = true;
            }

            regex = new Regex("(?<device>.*) device state (?<state>.*) for asset (?<asset>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               RemoteControlSessionState = $"{m.Groups["device"].Value} device state {m.Groups["state"].Value} for ATM {m.Groups["asset"].Value}";
               Asset = m.Groups["asset"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Received (?<event>.*) event for (?<task>.*) (?<val>.*) for asset (?<asset>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TaskStatusEvent = $"{m.Groups["event"].Value} for ATM {m.Groups["asset"].Value}, Task {m.Groups["task"].Value}, Value {m.Groups["val"].Value}";
               Asset = m.Groups["asset"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Sending transaction item approval: CheckIndex=(?<checkindex>.*), TellerApproval=(?<approval>.*), TellerAmount=(?<amount>.*), Reason=(?<reason>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TransactionItemStatus = $"APPROVAL CheckIndex {m.Groups["checkindex"].Value}, TellerApproval {m.Groups["approval"].Value}, TellerAmount {m.Groups["amount"].Value}, Reason {m.Groups["reason"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Teller session (?<sessionid>[0-9]*) for asset (?<asset>.*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerSessionRequest = $"TELLER SESSION session id {m.Groups["sessionid"].Value}, for ATM {m.Groups["asset"].Value}, action {m.Groups["action"].Value}";
               SessionRequestId = m.Groups["sessionid"].Value;
               Asset = m.Groups["asset"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Teller session request (?<id>[0-9]*) for asset (?<asset>.*) (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerSessionRequest = $"TELLER SESSION REQUEST id {m.Groups["sessionid"].Value}, for ATM {m.Groups["asset"].Value}, action {m.Groups["action"].Value}";
               Asset = m.Groups["asset"].Value;
               SessionRequestId = m.Groups["sessionid"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Teller session request (?<id>[0-9]*) for asset (?<asset>.*)$");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TellerSessionRequest = $"TELLER SESSION REQUEST id {m.Groups["sessionid"].Value}, for ATM {m.Groups["asset"].Value}";
               Asset = m.Groups["asset"].Value;
               SessionRequestId = m.Groups["sessionid"].Value;
               IsRecognized = true;
            }

            regex = new Regex("Transaction item (?<item>[0-9]*) (?<action>.*) for amount (?<amount>[0-9]*) with reason (?<reason>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               TransactionItemStatusChange = $"{m.Groups["action"].Value} item-number {m.Groups["item"].Value}, amount {m.Groups["amount"].Value}, reason {m.Groups["reason"].Value}";
               IsRecognized = true;
            }

            regex = new Regex("Customer search (?<action>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               Event = $"Customer search {m.Groups["action"].Value}";
               IsRecognized = true;
            }
         }

         if (!IsRecognized)
         {
            throw new Exception($"AWLogLine.{className}: did not recognize the log line '{logLine}'");
         }
      }
   }
}
