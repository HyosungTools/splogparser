using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LogLineHandler
{
   public class Startup : AVLine
   {
      private string className = "Startup";


      public string StartupState { get; set; } = string.Empty;
      public string TimeState { get; set; } = string.Empty;
      public string ApiCall { get; set; } = string.Empty;
      public string AssetATM { get; set; } = string.Empty;
      public string ModeATM { get; set; } = string.Empty;
      public string Customer { get; set; } = string.Empty;
      public string Flowpoint { get; set; } = string.Empty;
      public string Image { get; set; } = string.Empty;
      public string Teller { get; set; } = string.Empty;
      public string Database { get; set; } = string.Empty;
      public string ConnectionSignalR { get; set; } = string.Empty;
      public string Scheduler { get; set; } = string.Empty;
      public string Exception { get; set; } = string.Empty;

public Startup(ILogFileHandler parent, string logLine, AVLogType awType = AVLogType.Startup) : base(parent, logLine, awType)
      {
      }

      protected override void Initialize()
      {
         base.Initialize();

         //ActiveTeller Server version 1.3.1.0 is starting
         //Looking in 'D:\Inetpub\ActiveTellerWebsite\ActiveTeller\bin\NH.ActiveTeller.Server.Extensions.*.dll' for extensions
         //Found server extension - CMCFlexCustomerDataExtension
         //The CMCFlexCustomerDataExtension server extension was found, but it is not enabled
         //Started the SymXchangeCustomerDataExtension server extension
         //TellerResourceManager is initializing
         //TellerRequestManager is initializing
         //ActiveWorkItemManager's maxActiveWorkItems has been set to 1000
         //Beginning to execute the Seed method
         //Removed 0 duplicate Clients records.
         //The Seed method has completed.
         //OperatingModeManager is initializing
         //OperatingModeManager is handling the OperatingMode.UpdateSchedule.Today job.

         //Schedule manager starting
         //Schedule manager status changed to accepting
         //Schedule manager processor thread starting
         //Schedule manager processor now accepting jobs
         //Schedule manager processor stopping
         //Schedule manager notification for a new work item available to process
         //Schedule manager processing job 1070 for ATM 1
         //Schedule manager processor now accepting jobs

         //The Scheduled mode for asset ID 1 is SelfService.
         //The Scheduled mode for asset ID 1 is SelfService.

         //Schedule watcher next job is due at '11/17/2023 6:30:00 PM'
         //Schedule watcher check schedule now event received
         //Schedule watcher OperatingMode.ChangeMode job 1070 for ID 1 is due to execute.
         //Schedule watcher terminating

         //Registering the CoreStatusManager.
         //No ICoreManagerExtension was found.
         //VideoRecordingManager is initializing
         //VideoRecordingManager is seeding pending jobs
         //VideoRecordingManager found no pending video jobs
         //ClearSchedule deleted 4 OperatingMode.ChangeMode jobs from 10/16/2023.
         //ScheduleModeChanges found a profile hours record 3 for profile 3.

         //API CALLS

         //Put - /activeteller/api/applicationstates/13
         //Get - /Activeteller/api/operatingmodes?AssetName=NM000563
         //Post - /ActiveTeller/api/assets
         //Delete - /ActiveTeller/api/devicestates?assetName=18OAKCRK02L&deviceId=2,4,8,A,B,F,J,K
         //GetUserPermissions - /ActiveTeller/api/permissions/14

         //ASSET AND CUSTOMER INTERACTION

         //The Scheduled mode for asset ID 13 is SelfService.
         //Searching for customer data for asset id 13 because it was not found in the cache
         //GetCustomerData: Elapsed=00:00:00.0000092
         //Received customer Id with no customer name. Try to look customer information up.
         //The customer data for asset id 13 and customer id '0009804832' was not found in the cache.
         //Found the customer data for asset id 7 and customer id '0000617518' in the cache.
         //Adding the customer data for asset id 7 to the cache
         //Customer data for asset id 7 was found in the cache
         //Removed the customer data for asset id 7 from the cache

         //ActiveWorkitemManager before handling work item 1070 for ATM 1.  Active work item count = 0
         //ActiveWorkItemManager after handling job 1070 for Asset 1.  Active work item count = 1
         //GetNextWorkItem found a work item in the Queue
         //Executing the OperatingMode.ChangeMode job for Asset 1.

         //The work item sequence was completed, so it has been removed from the ActiveWorkItemList.
         //Job 1070 for Asset 3 completed.  Active Work Item Count = 28
         //The WorkItemProcessor thread for Asset 3 is terminating.

         //CONNECTION - SIGNALR?

         //Connection 8d747bec-bb07-4bf8-9acf-3fc648348e60: OnReconnected event fired.
         //Connection 27a290ea-5383-4dc4-90d8-6998b17177a9: OnConnected event fired.
         //Connection 27a290ea-5383-4dc4-90d8-6998b17177a9: Connection was added to the connectionMap. Count=1
         //Connection 27a290ea-5383-4dc4-90d8-6998b17177a9: Connection was added to the assetMap for '14STUR01L'. Count=1
         //Connection 2f794894-3bb6-4843-bacd-61f37db77e27: Connection was removed from the connectionMap. Count=27
         //Connection 2f794894-3bb6-4843-bacd-61f37db77e27: Connection for asset '16CENT04D' was removed from the assetMap. Count=27
         //Connection 042b01d4-ce4c-4ec2-be6c-f8d50807aa1d: UpdateTellerSessionStatisticsSubscription was called with True
         //Connection 042b01d4-ce4c-4ec2-be6c-f8d50807aa1d: starting SendOpenTellerSessionRequests
         //Connection 042b01d4-ce4c-4ec2-be6c-f8d50807aa1d: finishing SendOpenTellerSessionRequests
         //Connection 042b01d4-ce4c-4ec2-be6c-f8d50807aa1d: Connection for client session '3880' was removed from the clientSessionMap. Count=0
         //Connection da2cbe85-2c10-4322-81fb-b9a6fbf28e7a: Connection was updated in the connectionMap. Count=30
         //Connection da2cbe85-2c10-4322-81fb-b9a6fbf28e7a: Connection was updated in the assetMap for '21PLEA03D'. Count=28
         //updateOperatingMode could not be invoked on asset 00HOME01L because we could not find its connection
         //updateAsset could not be invoked on asset 16CENT01L because we could not find its connection

         //PRINCIPAL?

         //PrincipalContext found for the domain in ConnectedServer DRDCV1.ecu.com.
         //Found UserPrincipal for ECU\ahall. User.Enabled = True

         //CLIENT SESSIONS

         //Client session 3880 subscribed to asset 21PLEA03D
         //Client session 3881 can handle the request from 16CENT06D

         //TELLER SESSIONS

         //ActiveTeller User ahall exists in the Users table.
         //UpdateSubscriptionForSelections: clientSessionId: 3880, old: 0, new: 29
         //SetTellerAvailability was called with available for client session 3880.
         //[BEFORE] RemotePool.FireResourceAvailable - tellerResource.Id = 3880; UnavailableResources.Count = 0; AssignedList.Count = 0; AvailableResources.Count = 0
         //[AFTER] RemotePool.FireResourceAvailable - tellerResource.Id = 3880; UnavailableResources.Count = 0; AssignedList.Count = 0; AvailableResources.Count = 1
         //TerminalProfileHoursPolicy.Find - Asset 13 is currently in terminal profile hours 1: returning RoutingRule.PREFER_BRANCH.
         //TellerRequestManager.HandleTellerSessionRequest is using RoutingRule.PREFER_BRANCH for tellerRequest from 21PLEA03D
         //Client session 3880 can handle the request from 21PLEA03D
         //Request for 14STUR03D requires teller control.
         //TellerRequestManager.HandleTellerSessionRequest handled tellerRequest {"Id":20055,"AssetName":"21PLEA03D","Timestamp":"2023-11-17T08:01:02.043053-06:00","CustomerId":"","CustomerName":"","FlowPoint":"Common-RequestAssistance","RequestContext":"HelpButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
         //TellerRequestManager.HandleTellerSessionRequest handled tellerRequest {"Id":20056,"AssetName":"16CENT06D","Timestamp":"2023-11-17T08:02:16.1789868-06:00","CustomerId":"","CustomerName":"","FlowPoint":"Common-RequestAssistance","RequestContext":"HelpButton","ApplicationState":"PostIdle","TransactionType":"","Language":"English","VoiceGuidance":false,"RoutingProfile":{"SupportedCallType":"BeeHD"}}
         //Returning 409 (Conflict): Teller session could not be created. Teller session request 20088 already received a response.
         //Updating teller transaction status to failed for teller session 19225 because the session ended while the transaction was still in progress.
         //RemoveTellerSessionRequestsForClient deleted TellerSessionRequestId=20616 for clientSessionId=3951
         //Removing 2 expired client session(s).

         //CHECKS

         //Updating check image 36893 with length 708032
         //Check image 36893 updated, preparing to notify observers
         //Check image 36934 content is null

         //EXCEPTIONS

         //Exception: System.Threading.ThreadAbortException: Thread was being aborted.   at System.Threading.WaitHandle.WaitMultiple(WaitHandle[] waitHandles, Int32 millisecondsTimeout, Boolean exitContext, Boolean WaitAll)   at System.Threading.WaitHandle.WaitAny(WaitHandle[] waitHandles, Int32 millisecondsTimeout, Boolean exitContext)   at NH.Scheduling.ScheduleWatcherBase.WatchSchedule()
         //A database update exception occurred while attempting to insert record. An error occurred while updating the entries. See the inner exception for details.
         //Inner exception: An error occurred while updating the entries. See the inner exception for details.
         //Encountered FormatException while attempting to delete RemoteDesktopSession 8861.
         //Unexpected exception attempting to delete record. An exception has been raised that is likely due to a transient failure. If you are connecting to a SQL Azure database consider using SqlAzureExecutionStrategy.

         // remove the timestamp
         string subLogLine = logLine.Substring(20);

         Regex regex = new Regex("ActiveTeller Server version (?<version>.*) is starting");
         Match m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Looking in (?<path>.*) for extensions");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Found server extension - (?<extension>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("The (?<extension>.*) server extension was found, but it is not enabled");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Started the (?<extension>.*) server extension");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("(?<manager>.*) is initializing");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Schedule manager processing job (?<id>.*) for ATM (?<asset>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            AssetATM = $"{m.Groups["asset"].Value} processing job {m.Groups["id"].Value}";
            IsRecognized = true;
            // no return - fall through to next check
         }

         regex = new Regex("Schedule manager (?<action>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            if (subLogLine.Contains("exception") || subLogLine.Contains("Exception"))
            {
               Exception = subLogLine;
            }
            else
            {
               Scheduler = subLogLine;
            }
            IsRecognized = true;
            return;
         }

         regex = new Regex("Schedule watcher OperatingMode.ChangeMode job (?<id>.*) for ID (?<asset>.*) is due to execute.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Scheduler = subLogLine;
            AssetATM = $"{m.Groups["asset"].Value} job {m.Groups["id"].Value} due to execute";
            IsRecognized = true;
            // no return - fall through to next check
         }

         regex = new Regex("Schedule watcher (?<action>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Scheduler = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("OperatingModeManager (?<action>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            ModeATM = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("VideoRecordingManager (?<action>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("ActiveWorkItemManager's (?<setting>.*) has been set to (?<value>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("ClearSchedule deleted (?<num>.*) (?<jobtype>.*) jobs from (?<date>.*).");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Scheduler = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("ScheduleModeChanges found a profile hours record (?<recnum>.*) for profile (?<profilenum>.*).");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            ModeATM = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("(?<manager>.*) is initializing");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Registering the (?<manager>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("No (?<extension>.*) was found.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Beginning to execute the (?<method>.*) method");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("The (?<method>.*) method has completed.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Removed (?<num>.*) duplicate Clients records.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         if (subLogLine.Contains("/api/"))
         {
            //Put - /activeteller/api/applicationstates/13
            //Post - /ActiveTeller/api/assetconfigurations
            //Get - /Activeteller/api/operatingmodes?AssetName=NM000563
            //Post - /ActiveTeller/api/assets
            //Delete - /ActiveTeller/api/devicestates?assetName=18OAKCRK02L&deviceId=2,4,8,A,B,F,J,K
            //GetUserPermissions - /ActiveTeller/api/permissions/14
            //Post - /ActiveTeller/api/auth/register

            ApiCall = subLogLine;
            IsRecognized = true;

            regex = new Regex("(?<api>.*)/applicationstates/(?<asset>[0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               AssetATM = m.Groups["asset"].Value;
            }

            regex = new Regex("(?<api>.*)/permissions/(?<asset>[0-9]*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               AssetATM = m.Groups["asset"].Value;
            }

            regex = new Regex("(?<api>.*)/operatingmodes\\?[Aa]ssetName=(?<asset>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               AssetATM = m.Groups["asset"].Value;
            }

            regex = new Regex("(?<api>.*)/devicestates\\?[Aa]ssetName=(?<asset>.*)&(?<params>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               AssetATM = m.Groups["asset"].Value;
            }

            return;
         }

         regex = new Regex("The Scheduled mode for asset ID (?<asset>.*) is (?<mode>.*).");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Scheduler = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            ModeATM = m.Groups["mode"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Searching for customer data for asset id (?<asset>.*) because it was not found in the cache");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            AssetATM = m.Groups["asset"].Value;
            Database = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("GetCustomerData: Elapsed=(?<timespan>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            StartupState = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Received customer Id with no customer name. Try to look customer information up.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("The customer data for asset id (?<asset>.*) and customer id \'(?<customer>.*)\' was not found in the cache.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            Customer = m.Groups["customer"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Found the customer data for asset id (?<asset>.*) and customer id \'(?<customer>.*)\' in the cache.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            Customer = m.Groups["customer"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Removed the customer data for asset id (?<asset>.*) from the cache");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Removing (?<asset>.*) expired client session\\(s\\).");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("ActiveWorkitemManager before handling work item (?<id>.*) for ATM (?<asset>.*).  Active work item count = (?<count>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("ActiveWorkItemManager after handling job (?<jobid>.*) for Asset (?<asset>.*).  Active work item count = (?<count>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("GetNextWorkItem found a work item in the Queue");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Executing the OperatingMode.ChangeMode job for Asset (?<asset>.*).");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            ModeATM = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("The work item sequence was completed, so it has been removed from the ActiveWorkItemList.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Job (?<jobid>.*) for Asset (?<asset>.*) completed.  Active Work Item Count = (?<count>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("The WorkItemProcessor thread for Asset (?<asset>.*) is terminating.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("(?<mode>.*) could not be invoked on asset (?<asset>.*) because we could not find its connection");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            ModeATM = $"{m.Groups["mode"].Value} failed";
            IsRecognized = true;
            return;
         }

         regex = new Regex("Adding the customer data for asset id (?<asset>.*) to the cache");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Customer data for asset id (?<asset>.*) was found in the cache");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         if (subLogLine.StartsWith("Connection "))
         {
            regex = new Regex("Connection (?<guid>.*): On(?<event>.*) event fired.");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value } {m.Groups["event"].Value}";
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): Connection was added to the connectionMap. Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} added";
               Database = subLogLine;
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): Connection was added to the clientSessionMap for clientSessionId (?<client>.*). Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} added";
               Database = subLogLine;
               Teller = m.Groups["client"].Value;
               IsRecognized = true;
               return;
            }
            regex = new Regex("Connection (?<guid>.*): Connection was added to the assetMap for \'(?<asset>.*)\'. Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} added";
               Database = subLogLine;
               AssetATM = m.Groups["asset"].Value;
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): Connection was updated in the (?<map>.*). Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} updated";
               Database = subLogLine;
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): Connection was updated in the (?<map>.*) for \'(?<asset>.*)\'. Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} updated";
               Database = subLogLine;
               AssetATM = m.Groups["asset"].Value;
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): Connection was removed from the (?<map>.*). Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} removed";
               Database = subLogLine;
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): Connection for asset \'(?<asset>.*)\' was removed from the (?<map>.*). Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} removed";
               Database = subLogLine;
               AssetATM = $"{m.Groups["asset"].Value} removed";
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): Connection for client session \'(?<session>.*)\' was removed from the (?<map>.*). Count=(?<count>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} removed";
               Database = subLogLine;
               Teller = $"{m.Groups["session"].Value} removed";
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): UpdateTellerSessionStatisticsSubscription was called with (?<vall>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} updated subscription";
               Teller = $"update statistics subscription";
               Database = subLogLine;
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): starting (?<requests>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} starting";
               Database = subLogLine;
               IsRecognized = true;
               return;
            }

            regex = new Regex("Connection (?<guid>.*): finishing (?<requests>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               ConnectionSignalR = $"{m.Groups["guid"].Value} finishing";
               Database = subLogLine;
               IsRecognized = true;
               return;
            }
         }

         regex = new Regex("PrincipalContext found for the domain in ConnectedServer (?<server>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Found UserPrincipal for ECU\\\\(?<user>.*). User.Enabled = (?<val>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            Teller = m.Groups["user"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("ActiveTeller User (?<user>.*) exists in the Users table.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            Teller = m.Groups["user"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("SetTellerAvailability was called with (?<state>.*) for client session (?<client>.*).");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            ConnectionSignalR = subLogLine;
            Teller = m.Groups["client"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("UpdateSubscriptionForSelections: clientSessionId: (?<client>.*), old: (?<old>.*), new: (?<new>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            ConnectionSignalR = subLogLine;
            Teller = m.Groups["client"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("\\[(?<when>.*)\\] (?<resource>.*) - tellerResource.Id = (?<id>.*); UnavailableResources.Count = (?<unavail>.*); AssignedList.Count = (?<assigned>.*); AvailableResources.Count = (?<avail>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Teller = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("TerminalProfileHoursPolicy.Find - Asset (?<asset>.*) is currently in terminal profile hours (?<hours>.*): returning (?<rule>.*).");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Teller = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("TellerRequestManager\\.(?<request>.*) is using RoutingRule\\.(?<rule>.*) for tellerRequest from (?<asset>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Teller = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("RemoveTellerSessionRequestsForClient deleted TellerSessionRequestId=(?<requestid>.*) for clientSessionId=(?<sessionid>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            AssetATM = $"{m.Groups["requestedid"].Value} request deleted";
            Teller = $"{m.Groups["sessionid"].Value}";
            IsRecognized = true;
            return;
         }

         regex = new Regex("Client session (?<session>.*) subscribed to asset (?<asset>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            ConnectionSignalR = subLogLine;
            Teller = $"{m.Groups["session"].Value} removed";
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Client session (?<session>.*) can handle the request from (?<asset>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            ConnectionSignalR = subLogLine;
            Teller = $"{m.Groups["session"].Value}";
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Request for (?<asset>.*) requires teller control.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Teller = subLogLine;
            AssetATM = m.Groups["asset"].Value;
            IsRecognized = true;
            return;
         }

         regex = new Regex("TellerRequestManager.HandleTellerSessionRequest handled tellerRequest (?<json>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;

            try
            {
               dynamic tellerSessionRequest = JsonConvert.DeserializeObject<ExpandoObject>((string) m.Groups["json"].Value, new ExpandoObjectConverter());

               AssetATM = $"{tellerSessionRequest.AssetName}";
               Customer = $"{tellerSessionRequest.CustomerId} {tellerSessionRequest.CustomerName}";
               //2023-11-20T08:42:40.2709727-06:00
               TimeState = $"{tellerSessionRequest.Timestamp.ToString(DateTimeFormatStringMsec)}";
               Flowpoint = $"{tellerSessionRequest.ApplicationState} {tellerSessionRequest.FlowPoint} {tellerSessionRequest.RequestContext} {tellerSessionRequest.TransactionType}";
            }
            catch (Exception ex)
            {
               throw new Exception($"AVLogLine.Startup: failed to deserialize tellerSessionRequest Json payload for log line '{subLogLine}'\n{ex}");
            }

            IsRecognized = true;
            return;
         }

         regex = new Regex("Updating check image (?<id>.*) with length (?<length>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Image = $"Updating check image {m.Groups["id"].Value} length {m.Groups["length"].Value}";
            IsRecognized = true;
            return;
         }

         regex = new Regex("Check image (?<id>.*) updated, preparing to notify observers");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Image = $"Updated check image {m.Groups["id"].Value}, notify observers";
            IsRecognized = true;
            return;
         }

         regex = new Regex("Check image (?<id>.*) content is null");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Image = $"Check image {m.Groups["id"].Value} content is null";
            IsRecognized = true;
         }

         if (subLogLine.Contains("exception") || subLogLine.Contains("Exception"))
         {
            Exception = subLogLine;

            regex = new Regex("Exception: (?<exception>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               return;
            }

            regex = new Regex("A database update exception occurred (?<description>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               return;
            }

            regex = new Regex("Inner exception: (?<description>.*)");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               return;
            }

            regex = new Regex("Encountered (?<exception>.*) while attempting to (?<action>.*) (?<object>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               return;
            }

            regex = new Regex("Unexpected exception attempting to (?<action>.*). (?<description>.*).");
            m = regex.Match(subLogLine);
            if (m.Success)
            {
               IsRecognized = true;
               return;
            }
         }

         regex = new Regex("Returning (?<retcode>.*) \\((?<retname>.*)\\): (?<description>.*)");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Teller = subLogLine;
            IsRecognized = true;
            return;
         }

         regex = new Regex("Updating teller transaction status to failed for teller session (?<id>.*) because the session ended while the transaction was still in progress.");
         m = regex.Match(subLogLine);
         if (m.Success)
         {
            Database = subLogLine;
            Teller = $"{m.Groups["id"].Value} update failed";

            IsRecognized = true;
            return;
         }

         if (!IsRecognized)
         {
           throw new Exception($"AVLogLine.{className}: did not recognize the log line '{subLogLine}'");
         }
      }
   }
}
