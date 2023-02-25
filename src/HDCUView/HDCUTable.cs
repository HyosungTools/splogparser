using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;

namespace HDCUView
{
   /// <summary>
   /// class for processing loglines containing 'HCDUSensor::UpdateSensor'
   /// </summary>
   internal class HDCUTable : BaseTable
   {

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public HDCUTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;

      }

      /// <summary>
      /// Create a table with a given name, at the same time add columns. 
      /// </summary>
      /// <param name="tableName">name of the table to create</param>
      /// <returns></returns>
      protected override void InitDataTable(string tableName)
      {
         base.InitDataTable(tableName);
         AddColumn(tableName, "RjFull");
         AddColumn(tableName, "RjMiss");

         for (int i = 1; i <= 6; i++)
         {
            AddColumn(tableName, "C" + i.ToString() + "Miss");
            AddColumn(tableName, "C" + i.ToString() + "Emty");
            AddColumn(tableName, "C" + i.ToString() + "Low");
         }
         return;
      }

      /// <summary>
      /// Wrapper function to call Utilities.FindByMarker but also handle return error logging
      /// </summary>
      /// <param name="logLine">the current log line</param>
      /// <param name="mark">string to search for in the log line (start marker)</param>
      /// <param name="endMark"></param>
      /// <returns>substring of logline book-ended by mark and endMark, or error.</returns>
      private (bool found, string foundStr, string subLogLine) Find(string logLine, string mark, string endMark)
      {
         (bool found, string foundStr, string subLogLine) result;
         result = LogFind.FindByMarker(logLine, mark, endMark);
         if (!result.found)
         {
            // can't continue 
            ctx.ConsoleWriteLogLine("CashInTable.ProcessRow - Failed to find '" + mark + "'");
            return (false, string.Empty, logLine);
         }
         return (true, result.foundStr, result.subLogLine);
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            if (string.IsNullOrEmpty(logLine))
            {
               return;
            }

            // This log line is for us if it contains HCDUSensor::UpdateSensor
            if (!(logLine.Contains("HCDUSensor::UpdateSensor") &&
                  logLine.Contains("Shutter Open = [") &&
                  logLine.Contains("ITem Taken = [") &&
                  logLine.Contains("Stacker Empty = [") &&
                  logLine.Contains("Reject Full = [") &&
                  logLine.Contains("Carriage Home Position = [") &&
                  logLine.Contains("Cst#1 Missing = [") &&
                  logLine.Contains("Cst#2 Missing = [") &&
                  logLine.Contains("Cst#3 Missing = [")))
            {
               return;
            }

            base.ProcessRow(traceFile, logLine);

            (bool success, DataRow dataRow) newRow = NewRow(viewName);
            if (!newRow.success)
            {
               ctx.ConsoleWriteLogLine("Failed to add row to table '" + viewName + "'");
            }

            newRow.dataRow["file"] = _traceFile;
            newRow.dataRow["time"] = _logDate;

            string subLogLine = logLine;

            // Shutter Open = [0], Lock = [1], Close = [1]

            (bool found, string foundStr, string subLogLine) result;

            // Reject Full = [0], Missing = [0]

            result = Find(subLogLine, "Reject Full = [", "]");
            if (!result.found)
            {
               return;
            }
            subLogLine = result.subLogLine;

            newRow.dataRow["RjFull"] = result.foundStr;


            result = Find(subLogLine, "Missing = [", "]");
            if (!result.found)
            {
               return;
            }
            subLogLine = result.subLogLine;

            newRow.dataRow["RjMiss"] = result.foundStr;

            // Cst#1 Missing = [0], Empty = [0], Low = [0]

            for (int i = 1; i <= 6; i++)
            {
               result = Find(subLogLine, "Missing = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               newRow.dataRow["C" + i.ToString() + "Miss"] = result.foundStr;


               result = Find(subLogLine, "Empty = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               newRow.dataRow["C" + i.ToString() + "Emty"] = result.foundStr;

               result = Find(subLogLine, "Low = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               newRow.dataRow["C" + i.ToString() + "Low"] = result.foundStr;
            }

            AddRow(viewName, newRow.dataRow);

         }
         catch (Exception e)
         {
            ctx.LogWriteLine("HDCUTable.ProcessRow EXCEPTION:" + e.Message);
         }

         return;
      }
   }
}
