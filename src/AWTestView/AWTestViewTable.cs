using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;

namespace AWTestView
{
   class AWTestViewTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public AWTestViewTable(IContext ctx, string viewName) : base(ctx, viewName)
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
         if (logLine is AWAddKey awLogLine)
         {
            try
            {
               switch (awLogLine.awType)
               {
                  case AWLogType.AddKey:
                     {
                        base.ProcessRow(awLogLine);
                        AddKey(awLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
            catch (Exception e)
            {
               ctx.LogWriteLine($"AWTestViewTable.ProcessRow EXCEPTION: {e}");
            }
         }

         else
         {
            // logLine does not have a predefined class - this just prints the classname, we want the original log line
            // so we need to make a class for it
            ctx.LogWriteLine(logLine.ToString());
         }
      }

      protected void AddKey(AWAddKey akLogLine)
      {
         try
         {
            string tableName = "Keys";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = akLogLine.LogFile;
            dataRow["time"] = akLogLine.Timestamp;
            dataRow["error"] = string.Empty;

            // set the value
            dataRow["table"] = akLogLine.tableName;
            dataRow["key"] = akLogLine.keyName;
            dataRow["value"] = akLogLine.value;

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("AddKey Exception : " + e.Message);
         }
      }
   }
}
