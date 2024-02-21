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
         /*
         if (logLine is LogLineHandler.Settings seLogLine)
         {
            try
            {
               switch (seLogLine.avType)
               {
                  case AVLogType.Settings:
                     base.ProcessRow(seLogLine);
                     break;

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AVTable.ProcessRow Settings EXCEPTION: {e}");
            }
            finally
            {
               AddSettings(seLogLine);
            }
         }
         */
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

      protected void AddSettings(AVLine logLine)
      {
         try
         {
            string tableName = "Server";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = logLine.LogFile;
            dataRow["time"] = logLine.Timestamp;

            if (isOptionIncludePayload || !logLine.IsRecognized)
            {
               dataRow["Payload"] = logLine.logLine;
            }

            switch (logLine.avType)
            {
               case AVLogType.TBD:
                  /*
                  Settings seLine = logLine as Settings;
                  dataRow["Settings"] = DictionaryStringStringToString(seLine.SettingDict);
                  */
                  break;

               case AVLogType.Error:
               case AVLogType.None:
               default:
                  break;
            }

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddSettings Exception : " + e.Message);
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
