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
      /// Holds a list of Positions 
      private List<string> listPositions;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public HDCUTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;

         this.listPositions = new List<string>(new string[] { "Taken", "Stacker", "Output", "Transpt" });

         InitDataTable();
      }

      protected override void InitDataTable()
      {
         base.InitDataTable();

         this.dTable.Columns.Add("ShtrOpn", typeof(string));
         foreach (string strPosition in this.listPositions)
         {
            this.dTable.Columns.Add(strPosition, typeof(string));
         }

         this.dTable.Columns.Add("RjFull", typeof(string));
         this.dTable.Columns.Add("RjMiss", typeof(string));
         this.dTable.Columns.Add("UnDock", typeof(string));

         for (int i = 1; i <= 6; i++)
         {
            this.dTable.Columns.Add("C" + i.ToString() + "Miss", typeof(string));
            this.dTable.Columns.Add("C" + i.ToString() + "Emty", typeof(string));
            this.dTable.Columns.Add("C" + i.ToString() + "Low", typeof(string));
         }
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
            if (!(logLine.Contains("HCDUSensor::UpdateSensor") && logLine.Length > 1000))
            {
               return;
            }

            base.ProcessRow(traceFile, logLine);

            DataRow dataRow = dTable.NewRow();

            dataRow["File"] = _traceFile;
            dataRow["Time"] = _logDate; 

            string subLogLine = logLine;

            // Shutter Open = [0], Lock = [1], Close = [1]

            (bool found, string foundStr, string subLogLine) result;

            result = LogFind.FindByMarker(subLogLine, "Shutter Open = [", 1);
            dataRow["ShtrOpn"] = result.foundStr;
            subLogLine = result.subLogLine;

            // ITem Taken = [1]
            // Stacker Empty = [1], Output Position Empty = [1], Transport Empty = [1]

            result = LogFind.FindByMarker(subLogLine, "ITem Taken = [", 1);
            dataRow["Taken"] = Utilities.Flip(result.foundStr);
            subLogLine = result.subLogLine;

            result = LogFind.FindByMarker(subLogLine, "Stacker Empty = [", 1);
            dataRow["Stacker"] = Utilities.Flip(result.foundStr);
            subLogLine = result.subLogLine;

            result = LogFind.FindByMarker(subLogLine, "Output Position Empty = [", 1);
            dataRow["Output"] = Utilities.Flip(result.foundStr);
            subLogLine = result.subLogLine;

            result = LogFind.FindByMarker(subLogLine, "Transport Empty = [", 1);
            dataRow["Transpt"] = Utilities.Flip(result.foundStr);
            subLogLine = result.subLogLine;

            // Reject Full = [0], Missing = [0]

            result = LogFind.FindByMarker(subLogLine, "Reject Full = [", 1);
            dataRow["RjFull"] = result.foundStr;
            subLogLine = result.subLogLine;

            result = LogFind.FindByMarker(subLogLine, "Missing = [", 1);
            dataRow["RjMiss"] = result.foundStr;
            subLogLine = result.subLogLine;

            result = LogFind.FindByMarker(subLogLine, "CDU Dock Position = [", 1);
            dataRow["UnDock"] = Utilities.Flip(result.foundStr);
            subLogLine = result.subLogLine;

            // Cst#1 Missing = [0], Empty = [0], Low = [0]

            for (int i = 1; i <= 6; i++)
            {
               result = LogFind.FindByMarker(subLogLine, "Missing = [", 1);
               dataRow["C" + i.ToString() + "Miss"] = result.foundStr;
               subLogLine = result.subLogLine;

               result = LogFind.FindByMarker(subLogLine, "Empty = [", 1);
               dataRow["C" + i.ToString() + "Emty"] = result.foundStr;
               subLogLine = result.subLogLine;

               result = LogFind.FindByMarker(subLogLine, "Low = [", 1);
               dataRow["C" + i.ToString() + "Low"] = result.foundStr;
               subLogLine = result.subLogLine;
            }

            dTable.Rows.Add(dataRow);

         }
         catch (Exception e)
         {
            ctx.LogWriteLine("HDCUTable.ProcessRow EXCEPTION:" + e.Message);
         }

         return;
      }
   }
}
