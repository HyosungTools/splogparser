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

         if (logLine is LogLineHandler.IISComment coLogLine)
         {
            try
            {
               switch (coLogLine.iiType)
               {
                  case IILogType.IISComment:
                     base.ProcessRow(coLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {coLogLine.iiType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"IITable.IISComment Settings EXCEPTION: {e}");
            }
            finally
            {
               AddIISComment(coLogLine);
            }
         }

         if (logLine is LogLineHandler.IISRequest rqLogLine)
         {
            try
            {
               switch (rqLogLine.iiType)
               {
                  case IILogType.IISRequest:
                     base.ProcessRow(rqLogLine);
                     break;

                  default:
                     throw new Exception($"Unhandled LogType {rqLogLine.iiType.ToString()}");
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"IITable.IISRequest Settings EXCEPTION: {e}");
            }
            finally
            {
               AddIISRequest(rqLogLine);
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


      protected void AddIISComment(IISComment logLine)
      {
         if (logLine.IgnoreThis)
         {
            return;
         }

         try
         {
            string tableName = "IISService";

            // add the TIME ADJUSTMENT column after the timestamp field
            long sheetRow = dTableSet.Tables[tableName].Rows.Count + 2;   // Convert.ToInt64(bhdLine.lineNumber) + 1;
            string timeAdjustmentFormula = string.Empty;

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["tracefile"] = logLine.LogFile;
            dataRow["linenumber"] = logLine.lineNumber;
            dataRow["time"] = logLine.Timestamp;
            dataRow["adjustedtime"] = timeAdjustmentFormula;
            dataRow["utctime"] = logLine.Timestamp;

            if (isOptionIncludePayload || !logLine.IsRecognized)
            {
               dataRow["Payload"] = logLine.logLine;
            }

            dataRow["exception"] = logLine.Exception;

            dataRow["IISApplication"] = $"{IISComment.IISApplicationName} {IISComment.IISVersion}";
            dataRow["IISCommentState"] = logLine.IISCommentState;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddIISComment Exception : " + e.Message);
         }

         if (logLine.EventTimestamp != string.Empty)
         {
            // record IIS restarts in the IISRequests tab
            AddIISRequest_ServerComment(logLine);
         }
      }

      protected void AddIISRequest_ServerComment(IISComment logLine)
      {
         try
         {
            string tableName = "IISServer";

            // add the TIME ADJUSTMENT column after the timestamp field
            long sheetRow = dTableSet.Tables[tableName].Rows.Count + 2;   // Convert.ToInt64(bhdLine.lineNumber) + 1;
            string timeAdjustmentFormula = string.Empty;

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["tracefile"] = logLine.LogFile;
            dataRow["linenumber"] = logLine.lineNumber;
            dataRow["time"] = logLine.EventTimestamp;                  // comments don't usually have a timestamp field, so use this one
            dataRow["adjustedtime"] = timeAdjustmentFormula;
            dataRow["utctime"] = logLine.Timestamp;

            if (isOptionIncludePayload || !logLine.IsRecognized)
            {
               dataRow["Payload"] = logLine.logLine;
            }

            dataRow["exception"] = logLine.Exception;

            dataRow["IISRequestState"] = $"{logLine.IISCommentState} {IISComment.IISApplicationName} {IISComment.IISVersion}";

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddIISRequest_ServerComment Exception : " + e.Message);
         }

      }

      protected void AddIISRequest(IISRequest logLine)
      {
         if (logLine.IgnoreThis)
         {
            return;
         }

         try
         {
            string tableName = "IISServer";

            // add the TIME ADJUSTMENT column after the timestamp field
            long sheetRow = dTableSet.Tables[tableName].Rows.Count + 2;   // Convert.ToInt64(bhdLine.lineNumber) + 1;
            string timeAdjustmentFormula = string.Empty;

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["tracefile"] = logLine.LogFile;
            dataRow["linenumber"] = logLine.lineNumber;
            dataRow["time"] = logLine.Timestamp;
            dataRow["adjustedtime"] = timeAdjustmentFormula;
            dataRow["utctime"] = logLine.Timestamp;

            if (isOptionIncludePayload || !logLine.IsRecognized)
            {
               dataRow["Payload"] = logLine.logLine;
            }

            dataRow["exception"] = logLine.Exception;

            dataRow["IISRequestState"] = logLine.IISRequestState;
            dataRow["RepeatCount"] = logLine.RepeatCount;
            dataRow["Server_IpAddress"] = logLine.Server_IpAddress;
            dataRow["Method"] = logLine.Method;
            dataRow["Uri"] = logLine.Uri;
            dataRow["Query"] = logLine.Query;
            dataRow["Port"] = logLine.Port;
            dataRow["Username"] = logLine.Username;
            dataRow["Client_IpAddress"] = logLine.Client_IpAddress;
            dataRow["UserAgent"] = logLine.UserAgent;
            dataRow["Referer"] = logLine.Referer;
            dataRow["HttpStatusCode"] = logLine.HttpStatusCode;
            dataRow["SubStatusCode"] = logLine.SubStatusCode;
            dataRow["HttpError"] = logLine.HttpError;
            dataRow["Win32StatusCode"] = logLine.Win32StatusCode;
            dataRow["Win32Error"] = logLine.Win32Error;
            dataRow["ElapsedMsec"] = logLine.TimeTakenMsec;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddIISRequest Exception : " + e.Message);
         }
      }
   }
}
