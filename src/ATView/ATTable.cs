using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;
using System.Text;

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
                     {
                        base.ProcessRow(atLogLine);
                        AddSignalRConnectionEvent(atLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine("ATTable.ProcessRow.ConnectionState EXCEPTION:" + e.Message);
            }
         }

         if (logLine is LogLineHandler.ConnectionManagerAction atLogLine2)
         {
            try
            {
               switch (atLogLine2.atType)
               {
                  case ATLogType.ConnectionManagerAction:
                     {
                        base.ProcessRow(atLogLine2);
                        AddConnectionManagerAction(atLogLine2);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"ATTable.ProcessRow.ConnectionManagerAction EXCEPTION: {e}");
            }
         }

         if (logLine is LogLineHandler.AgentConfiguration acLogLine)
         {
            try
            {
               switch (acLogLine.atType)
               {
                  case ATLogType.AgentConfiguration:
                     {
                        base.ProcessRow(acLogLine);
                        AddAgentConfigurationEvent(acLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine("ATTable.ProcessRow.AgentConfiguration EXCEPTION:" + e.Message);
            }
         }


         if (logLine is LogLineHandler.AgentHost ahLogLine)
         {
            try
            {
               switch (ahLogLine.atType)
               {
                  case ATLogType.AgentHost:
                     {
                        base.ProcessRow(ahLogLine);
                        AddAgentHostEvent(ahLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine("ATTable.ProcessRow.AgentHost EXCEPTION:" + e.Message);
            }
         }


         if (logLine is LogLineHandler.ServerRequests scLogLine)
         {
            try
            {
               switch (scLogLine.atType)
               {
                  case ATLogType.ServerRequest:
                     {
                        base.ProcessRow(scLogLine);
                        AddServerHttpRequest(scLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AETable.ProcessRow MoniPlus2sExtension EXCEPTION: {e}");
            }
         }
      }

      private string TimestampToTableString(DateTime dt)
      {
         return (dt == DateTime.MinValue) || (dt == default(DateTime)) ? string.Empty : dt.ToString(Impl.LogLine.TimeFormatStringMsec);
      }


      protected void AddServerHttpRequest(LogLineHandler.ServerRequests logLine)
      {
         string tableName = "ServerHttpRequests";

         try
         {
            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            dataRow["RequestMethod"] = (logLine.RequestMethod != null) ? logLine.RequestMethod.ToString() : string.Empty;
            dataRow["RequestUrl"] = (logLine.RequestUrl != null) ? logLine.RequestUrl.ToString() : string.Empty;
            dataRow["RequestResult"] = (logLine.RequestResult != null) ? logLine.RequestResult.ToString() : string.Empty;

            dataRow["Operation"] = (logLine.Operation != null) ? logLine.Operation.ToString() : string.Empty;
            dataRow["ObjectType"] = (logLine.ObjectType != null) ? logLine.ObjectType.ToString() : string.Empty;
            dataRow["ObjectHandler"] = (logLine.ObjectHandler != null) ? logLine.ObjectHandler.ToString() : string.Empty;

            dataRow["RequestTime"] = (logLine.RequestTimeUTC != null) ? logLine.RequestTimeUTC : string.Empty;

            dataRow["ClientSession"] = logLine.ClientSession;
            dataRow["Terminal"] = (logLine.Terminal != null) ? logLine.Terminal.ToString() : string.Empty;
            dataRow["TellerName"] = (logLine.TellerName != null) ? logLine.TellerName.ToString() : string.Empty;
            dataRow["TellerId"] = (logLine.TellerId != null) ? logLine.TellerId.ToString() : string.Empty;
            dataRow["TellerUri"] = (logLine.TellerUri != null) ? logLine.TellerUri.ToString() : string.Empty;
            dataRow["TaskName"] = (logLine.TaskName != null) ? logLine.TaskName.ToString() : string.Empty;
            dataRow["EventName"] = (logLine.EventName != null) ? logLine.EventName.ToString() : string.Empty;
            dataRow["FlowPoint"] = (logLine.FlowPoint != null) ? logLine.FlowPoint.ToString() : string.Empty;
            dataRow["CustomerId"] = (logLine.CustomerId != null) ? logLine.CustomerId.ToString() : string.Empty;
            dataRow["RequestName"] = (logLine.RequestName != null) ? logLine.RequestName.ToString() : string.Empty;
            dataRow["RequestContext"] = (logLine.RequestContext != null) ? logLine.RequestContext.ToString() : string.Empty;
            dataRow["ApplicationState"] = (logLine.ApplicationState != null) ? logLine.ApplicationState.ToString() : string.Empty;
            dataRow["TransactionType"] = (logLine.TransactionType != null) ? logLine.TransactionType.ToString() : string.Empty;

            dataRow["Payload"] = (logLine.Payload != null) ? logLine.Payload.ToString() : string.Empty;

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
