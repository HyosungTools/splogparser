using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace TCRView
{
   internal class TCRTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public TCRTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;
      }

      public override void PostProcess()
      {
         string tableName = string.Empty;

         base.PostProcess();
         return;
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
         try
         {
            if (logLine is TCRLogLine tcrLogLine)
            {
               switch (tcrLogLine.tcrType)
               {
                  //case TCRLogType.TCR_ATM2HOST:
                  //   {
                  //      base.ProcessRow(logLine);
                  //      TCRLINE(tcrLogLine, "comms", "ATM2HOST");
                  //      break;
                  //   }

                  case TCRLogType.TCR_ATM_SENDMESSAGE_SYNC:
                     {
                        if (tcrLogLine is TCRLogLineWithField)
                        {
                           TCRLogLineWithField tcrLogLineWithField = (TCRLogLineWithField)tcrLogLine;
                           base.ProcessRow(logLine);
                           TCRLINE(tcrLogLine, "comms", "ATM2HOST: " + tcrLogLineWithField.field);
                        }
                        break;
                     }

                  //case TCRLogType.TCR_HOST2ATM:
                  //   {
                  //      base.ProcessRow(logLine);
                  //      TCRLINE(tcrLogLine, "comms", "HOST2ATM");
                  //      break;
                  //   }
                  case TCRLogType.TCR_HOST_RECEIVED_DATA:
                     {
                        if (tcrLogLine is TCRLogLineWithField)
                        {
                           TCRLogLineWithField tcrLogLineWithField = (TCRLogLineWithField)tcrLogLine;
                           base.ProcessRow(logLine);
                           TCRLINE(tcrLogLine, "comms", "HOST2ATM: " + tcrLogLineWithField.field);
                        }
                        break;
                     }
                  case TCRLogType.TCR_ON_UPDATE_SCREENDATA:
                     {
                        if (tcrLogLine is TCRLogLineWithField)
                        {
                           TCRLogLineWithField tcrLogLineWithField = (TCRLogLineWithField)tcrLogLine;
                           base.ProcessRow(logLine);
                           TCRLINE(tcrLogLine, "screen", tcrLogLineWithField.field);
                        }
                        break;
                     }
                  case TCRLogType.TCR_CHANGING_MODE:
                  case TCRLogType.TCR_CHANGEMODE_FAILED:
                  case TCRLogType.TCR_CURRENTMODE:
                     {
                        if (tcrLogLine is TCRLogLineWithField)
                        {
                           TCRLogLineWithField tcrLogLineWithField = (TCRLogLineWithField)tcrLogLine;
                           base.ProcessRow(logLine);
                           TCRLINE(tcrLogLine, "mode", tcrLogLineWithField.field);
                        }
                        break;
                     }
                  case TCRLogType.TCR_NEXTSTATE:
                     {
                        if (tcrLogLine is TCRLogLineWithField)
                        {
                           TCRLogLineWithField tcrLogLineWithField = (TCRLogLineWithField)tcrLogLine;
                           base.ProcessRow(logLine);
                           TCRLINE(tcrLogLine, "state", tcrLogLineWithField.field);
                        }
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("TCRTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void TCRLINE(TCRLogLine tcrLine, string columnName, string columnValue)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["TCRSummary"].Rows.Add();

            dataRow["file"] = tcrLine.LogFile;
            dataRow["time"] = tcrLine.Timestamp;
            dataRow["error"] = tcrLine.HResult;

            dataRow["mode"] = string.Empty;
            dataRow["host"] = string.Empty;
            dataRow["screen"] = string.Empty;
            dataRow["state"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dataRow[columnName] = columnValue;

            dTableSet.Tables["TCRSummary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("TCRLINE Exception : " + e.Message);
         }
      }
   }
}

