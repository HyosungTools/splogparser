using System;
using System.Text.RegularExpressions;
using Contract;
using static LogLineHandler.SignalRConnectionState;

namespace LogLineHandler
{
   public class ConnectionManagerAction : ATLine
   {
      public enum ConnectionManagerActionEnum
      {
         None,
         ManagerThreadStarting,
         AttemptingActiveTellerServerContact,
         ActiveTellerServerContacted,
         RegisteringClient,
         PreparingActiveTellerConnection,
         ConfiguringActiveTellerHubProxy,
         InitiatingActiveTellerConnection,
         RegisteringAssetUsingMAC,
         RegistrationException
      }

      public ConnectionManagerActionEnum state { get; set; }
      public string deviceId { get; set; }
      public string macAddress { get; set; }

      public ConnectionManagerAction(ILogFileHandler parent, string logLine, ATLogType atType = ATLogType.ConnectionManagerAction) : base(parent, logLine, atType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         state = ConnectionManagerActionEnum.None;

         /*
            2023-11-17 03:00:21 Attempting to contact the ActiveTeller server
            2023-11-17 03:00:21 Successfully contacted the ActiveTeller server

            2023-11-17 03:00:21 Connection manager thread starting
            2023-11-17 03:00:21 Connection manager registering client using device id 70-85-C2-18-7C-DA
            2023-11-17 03:00:22 Connection manager preparing the ActiveTeller connection
            2023-11-17 03:00:22 Connection manager configuring the ActiveTeller hub proxy
            2023-11-17 03:00:22 Connection manager initiating ActiveTeller connection
            2023-11-17 03:00:22 Connection manager registering asset using MAC Address 70-85-C2-18-7C-DA
            2023-11-17 09:24:30 Connection manager registration exception
         */

         int idx = logLine.IndexOf("Connection manager");
         if (idx != -1)
         {
            string subLogLine = logLine.Substring(idx + "Connection manager".Length + 1);

            if (subLogLine.StartsWith("thread starting"))
            {
               IsRecognized = true;
               state = ConnectionManagerActionEnum.ManagerThreadStarting;
            }

            else if (subLogLine.StartsWith("registering client using device id"))
            {
               IsRecognized = true;
               state = ConnectionManagerActionEnum.RegisteringClient;
               deviceId = subLogLine.Substring(subLogLine.LastIndexOf(" ") + 1);
            }

            else if (subLogLine.StartsWith("registering asset using MAC Address"))
            {
               IsRecognized = true;
               state = ConnectionManagerActionEnum.RegisteringAssetUsingMAC;
               macAddress = subLogLine.Substring(subLogLine.LastIndexOf(" ") + 1);
            }

            else if (subLogLine.StartsWith("registration exception"))
            {
               IsRecognized = true;
               state = ConnectionManagerActionEnum.RegistrationException;
            }

            else if (subLogLine.StartsWith("preparing the ActiveTeller connection"))
            {
               IsRecognized = true;
               state = ConnectionManagerActionEnum.PreparingActiveTellerConnection;
            }

            else if (subLogLine.StartsWith("configuring the ActiveTeller hub proxy"))
            {
               IsRecognized = true;
               state = ConnectionManagerActionEnum.ConfiguringActiveTellerHubProxy;
            }

            else if (subLogLine.StartsWith("initiating ActiveTeller connection"))
            {
               IsRecognized = true;
               state = ConnectionManagerActionEnum.InitiatingActiveTellerConnection;
            }
         }

         else if (logLine.Contains("Attempting to contact the ActiveTeller server"))
         {
            IsRecognized = true;
            state = ConnectionManagerActionEnum.AttemptingActiveTellerServerContact;
         }

         else if (logLine.Contains("Successfully contacted the ActiveTeller server"))
         {
            IsRecognized = true;
            state = ConnectionManagerActionEnum.ActiveTellerServerContacted;
         }

         if (!IsRecognized)
         {
            throw new Exception($"ATLogLine.ConnectionManagerAction: did not recognize the log line '{logLine}'");
         }
      }
   }
}
