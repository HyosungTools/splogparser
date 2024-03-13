using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace AVView
{
   class AVTable : BaseTable
   {
      /// <summary>
      /// Include the raw logline in the XML output
      /// </summary>
      public bool isOptionIncludePayload { get; set; } = false;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public AVTable(IContext ctx, string viewName) : base(ctx, viewName)
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
         if (logLine is LogLineHandler.Startup stLogLine)
         {
            try
            {
               switch (stLogLine.avType)
               {
                  case AVLogType.Startup:
                     base.ProcessRow(stLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {stLogLine.avType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AVTable.Startup Settings EXCEPTION: {e}");
            }
            finally
            {
               AddStartup(stLogLine);
            }
         }

         if (logLine is LogLineHandler.CustomerDataExtension ceLogLine)
         {
            try
            {
               switch (ceLogLine.avType)
               {
                  case AVLogType.SymXchangeCustomerDataExtension:
                     base.ProcessRow(ceLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {ceLogLine.avType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AVTable.CustomerDataExtension Settings EXCEPTION: {e}");
            }
            finally
            {
               AddCustomerDataExtension(ceLogLine);
            }
         }
      }


      private string ListOfLongToString(List<long> list)
      {
         StringBuilder sb = new StringBuilder();
         foreach (long l in list)
         {
            sb.Append(l.ToString());
         }

         return sb.ToString();
      }

      private string DictionaryStringStringToString(Dictionary<string,string> list)
      {
         string comma = string.Empty;

         StringBuilder sb = new StringBuilder();
         foreach (KeyValuePair<string,string> kvp in list)
         {
            sb.Append($"{comma}{kvp.Key}={kvp.Value}");
            comma = ",";
         }

         return sb.ToString();
      }

      protected void AddStartup(Startup logLine)
      {
         try
         {
            string tableName = "ATServer";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            if (isOptionIncludePayload || !logLine.IsRecognized)
            {
               dataRow["Payload"] = logLine.logLine;
            }

            dataRow["state"] = logLine.StartupState;
            dataRow["requesttime"] = logLine.TimeState;
            dataRow["api"] = logLine.ApiCall;
            dataRow["asset"] = logLine.AssetATM;
            dataRow["mode"] = logLine.ModeATM;
            dataRow["customer"] = logLine.Customer;
            dataRow["flowpoint"] = logLine.Flowpoint;
            dataRow["image"] = logLine.Image;
            dataRow["teller"] = logLine.Teller;
            dataRow["database"] = logLine.Database;
            dataRow["connection"] = logLine.ConnectionSignalR;
            dataRow["scheduler"] = logLine.Scheduler;
            dataRow["exception"] = logLine.Exception;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddStartup Exception : " + e.Message);
         }
      }

      protected void AddCustomerDataExtension(CustomerDataExtension logLine)
      {
         try
         {
            string tableName = "CustomerDataExtension";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            if (isOptionIncludePayload || !logLine.IsRecognized)
            {
               dataRow["Payload"] = logLine.logLine;
            }

            dataRow["state"] = logLine.ExtensionState;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddCustomerDataExtension Exception : " + e.Message);
         }
      }

      /*
      protected void AddNetOpExtensionEvent(LogLineHandler.NetOpExtension logLine)
      {
         try
         {
            string tableName = "NetOpEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;
            dataRow["ModelName"] = logLine.ModelName;
            dataRow["ConfigurationState"] = logLine.ConfigurationState;
            dataRow["RemoteDesktopServerState"] = logLine.RemoteDesktopServerState;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddNetOpExtensionEvent Exception : " + e.Message);
         }

         // also add to MoniPlus2sEvents table
         try
         {
            string tableName = "MoniPlus2sEvents";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            StringBuilder sb = new StringBuilder();
            sb.Append((!string.IsNullOrEmpty(logLine.ModelName) ? $"Model {logLine.ModelName} " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.ConfigurationState) ? $"{logLine.ConfigurationState} " : string.Empty));
            sb.Append((!string.IsNullOrEmpty(logLine.RemoteDesktopServerState) ? $"Server {logLine.RemoteDesktopServerState}" : string.Empty));

            dataRow["Netop"] = sb.ToString();

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddNetOpExtensionEvent Exception : " + e.Message);
         }
      }
*/

   }
}
