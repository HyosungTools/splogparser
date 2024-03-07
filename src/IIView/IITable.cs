using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace IIView
{
   class IITable : BaseTable
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
      public IITable(IContext ctx, string viewName) : base(ctx, viewName)
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
         if (logLine is LogLineHandler.Startup stLogLine)
         {
            try
            {
               switch (stLogLine.avType)
               {
                  case IILogType.Startup:
                     base.ProcessRow(stLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {stLogLine.avType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"IITable.Startup Settings EXCEPTION: {e}");
            }
            finally
            {
               AddStartup(stLogLine);
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

      /*
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
            dataRow["time"] = logLine.TimeState;
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
      */
   }
}
