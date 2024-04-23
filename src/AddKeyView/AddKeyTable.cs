using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;

namespace AddKeyView
{
   class AddKeyTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public AddKeyTable(IContext ctx, string viewName) : base(ctx, viewName)
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
         try
         {
            if (logLine is AddKey akLogLine)
            {
               switch (akLogLine.apType)
               {
                  case APLogType.APLOG_ADDKEY:
                     {
                        base.ProcessRow(akLogLine);
                        AddKey(akLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("AddKey.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void AddKey(AddKey akLogLine)
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
