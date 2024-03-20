using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace ATView
{
   class ATTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public ATTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         return base.WriteExcelFile();
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         if (logLine is LogLineHandler.SignalRConnectionState atLogLine)
         {
            try
            {
               switch (atLogLine.atType)
               {
                  case ATLogType.ActiveTellerConnectionState:
                     base.ProcessRow(atLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {atLogLine.atType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine("ATTable.ProcessRow.ConnectionState EXCEPTION:" + e.Message);
            }
            finally
            {
               AddSignalRConnectionEvent(atLogLine);
            }
         }

         if (logLine is LogLineHandler.ConnectionManagerAction atLogLine2)
         {
            try
            {
               switch (atLogLine2.atType)
               {
                  case ATLogType.ConnectionManagerAction:
                     base.ProcessRow(atLogLine2);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {atLogLine2.atType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"ATTable.ProcessRow.ConnectionManagerAction EXCEPTION: {e}");
            }
            finally
            {
               AddConnectionManagerAction(atLogLine2);
            }
         }

         if (logLine is LogLineHandler.AgentConfiguration acLogLine)
         {
            try
            {
               switch (acLogLine.atType)
               {
                  case ATLogType.AgentConfiguration:
                     base.ProcessRow(acLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {acLogLine.atType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine("ATTable.ProcessRow.AgentConfiguration EXCEPTION:" + e.Message);
            }
            finally
            {
               AddAgentConfigurationEvent(acLogLine);
            }
         }


         if (logLine is LogLineHandler.AgentHost ahLogLine)
         {
            try
            {
               switch (ahLogLine.atType)
               {
                  case ATLogType.AgentHost:
                     base.ProcessRow(ahLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {ahLogLine.atType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine("ATTable.ProcessRow.AgentHost EXCEPTION:" + e.Message);
            }
            finally
            {
               AddAgentHostEvent(ahLogLine);
            }
         }


         if (logLine is LogLineHandler.ServerRequests scLogLine)
         {
            try
            {
               switch (scLogLine.atType)
               {
                  case ATLogType.ServerRequest:
                     base.ProcessRow(scLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {scLogLine.atType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AETable.ProcessRow MoniPlus2sExtension EXCEPTION: {e}");
            }
            finally
            {
               AddServerHttpRequest(scLogLine);
            }
         }
      }

      protected void AddServerHttpRequest(LogLineHandler.ServerRequests logLine)
      {
         string tableName = "ServerHttpRequests";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            if (isOptionIncludePayload || !logLine.IsRecognized)
            {
               dataRow["Payload"] = logLine.logLine;
            }

            dataRow["RequestMethod"] = logLine.RequestMethod;
            dataRow["RequestUrl"] = logLine.RequestUrl;
            dataRow["RequestDomain"] = logLine.RequestDomain;
            dataRow["RequestPath"] = logLine.RequestPath;

            dataRow["RequestResult"] = logLine.RequestResult;
            dataRow["Operation"] = logLine.Operation;
            dataRow["ObjectType"] = logLine.ObjectType;
            dataRow["ObjectHandler"] = logLine.ObjectHandler;

            dataRow["RequestTime"] = logLine.RequestTimeUTC;

            dataRow["ClientSession"] = logLine.ClientSession;
            dataRow["AssetName"] = logLine.AssetName;
            dataRow["SessionId"] = logLine.SessionId;
            dataRow["TellerName"] = logLine.TellerName;
            dataRow["TellerId"] = logLine.TellerId;
            dataRow["TellerUri"] = logLine.TellerUri;
            dataRow["TaskName"] = logLine.TaskName;
            dataRow["EventName"] = logLine.EventName;
            dataRow["FlowPoint"] = logLine.FlowPoint;
            dataRow["CustomerId"] = logLine.CustomerId;
            dataRow["RequestName"] = logLine.RequestName;
            dataRow["RequestContext"] = logLine.RequestContext;
            dataRow["ApplicationState"] = logLine.ApplicationState;
            dataRow["TransactionType"] = logLine.TransactionType;

            dataRow["Payload"] = logLine.Payload;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }

         // also put it in the server communication summary
         tableName = "ServerCommunication";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            StringBuilder sb = new StringBuilder();
            sb.Append((!string.IsNullOrEmpty(logLine.RequestMethod) ? $"Method {logLine.RequestMethod}, " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.RequestUrl) ? $"URL {logLine.RequestUrl}, " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.RequestResult) ? $"Result {logLine.RequestResult}, " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.Operation) ? $"Operation {logLine.Operation}, " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.ObjectType) ? $"ObjectType {logLine.ObjectType}, " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.ObjectHandler) ? $"Handler {logLine.ObjectHandler}" : string.Empty));

            sb.Append((!string.IsNullOrEmpty(logLine.AssetName) ? logLine.AssetName : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.SessionId) ? logLine.SessionId : string.Empty));

            dataRow["ServerHttp"] = sb.ToString();

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }

      }

      protected void AddSignalRConnectionEvent(LogLineHandler.SignalRConnectionState logLine)
      {
         string tableName = "SignalRConnectionEvents";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;
            dataRow["state"] = logLine.ConnectionState.ToString();
            dataRow["url"] = (logLine.Url != null) ? logLine.Url : string.Empty;
            dataRow["guid"] = (logLine.ConnectionGuid != null) ? logLine.ConnectionGuid : string.Empty;
            dataRow["exception"] = (logLine.Exception != null) ? logLine.Exception : string.Empty;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }

         // also put it in the server communication summary
         tableName = "ServerCommunication";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            StringBuilder sb = new StringBuilder();
            sb.Append($"State {logLine.ConnectionState}, ");
            sb.Append((!string.IsNullOrEmpty(logLine.Url) ? $"URL {logLine.Url}, " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.Exception) ? $"Exception {logLine.Exception}, " : string.Empty));

            dataRow["ServerSignalR"] = sb.ToString();

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }
      }

      protected void AddConnectionManagerAction(LogLineHandler.ConnectionManagerAction logLine)
      {
         string tableName = "ConnectionManagerActions";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;
            dataRow["state"] = logLine.state.ToString();
            dataRow["deviceid"] = logLine.deviceId;
            dataRow["macaddress"] = logLine.macAddress;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }

         // also put it in the server communication summary
         tableName = "ServerCommunication";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            StringBuilder sb = new StringBuilder();
            sb.Append($"{logLine.state.ToString()}");
            dataRow["ConnectionManager"] = sb.ToString();

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }
      }

      protected void AddAgentConfigurationEvent(LogLineHandler.AgentConfiguration logLine)
      {
         string tableName = "AgentConfigurationEvents";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;
            dataRow["serverurl"] = (logLine.serverUrl != null) ? logLine.serverUrl.ToString() : string.Empty;
            dataRow["reconnectinterval"] = logLine.reconnectInterval;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }

         // also put it in the server communication summary
         tableName = "ServerCommunication";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            StringBuilder sb = new StringBuilder();
            sb.Append((!string.IsNullOrEmpty(logLine.serverUrl) ? $"URL {logLine.serverUrl}" : string.Empty));
            sb.Append((logLine.reconnectInterval > 0 ? $"Reconnect {logLine.reconnectInterval} sec" : string.Empty));
            dataRow["AgentConfiguration"] = (logLine.serverUrl != null) ? logLine.serverUrl.ToString() : string.Empty;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }
      }

      protected void AddAgentHostEvent(LogLineHandler.AgentHost logLine)
      {
         string tableName = "AgentHostEvents";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;
            dataRow["state"] = logLine.state.ToString();

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Add{tableName} Exception : " + e.Message);
         }
      }

   }
}
