using System;
using System.Collections.Generic;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace SSView
{
   internal class SSTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public SSTable(IContext ctx, string viewName) : base(ctx, viewName)
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
            if (logLine is SSLine ssLogLine)
            {
               switch (ssLogLine.ssType)
               {
                  /* UPDATE_STATUS */

                  case SSLogType.EJ_Created:
                     {
                        base.ProcessRow(ssLogLine);
                        UPDATE_SS(ssLogLine, "created");
                        break;
                     }
                  case SSLogType.EJ_Discovered:
                     {
                        base.ProcessRow(ssLogLine);
                        UPDATE_SS(ssLogLine, "discovered");
                        break;
                     }
                  case SSLogType.EJ_ImportSucceeded:
                     {
                        base.ProcessRow(ssLogLine);
                        UPDATE_SS(ssLogLine, "imported");
                        break;
                     }
                  case SSLogType.EJ_UploadedReceived:
                     {
                        base.ProcessRow(ssLogLine);
                        UPDATE_SS(ssLogLine, "uploaded");
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CDMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }


      protected void UPDATE_SS(SSLine ssLogLine, string colName)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["SS"].Rows.Add();

            dataRow["file"] = ssLogLine.LogFile;
            dataRow["time"] = ssLogLine.Timestamp;
            dataRow["error"] = ssLogLine.HResult;

            dataRow["ej"] = ssLogLine.ejFileName;
            dataRow[colName] = "1";

            dTableSet.Tables["SS"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UPDATE_SS Exception : " + e.Message);
         }

         return;
      }
   }
}

