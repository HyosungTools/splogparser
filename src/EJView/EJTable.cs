using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace EJView
{
   class EJTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public EJTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         try
         {
            ctx.LogWriteLine("EJTable.WriteExcelFile");
            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.LogWriteLine(String.Format("EJTable.WriteExcelFile : Table Name - {0}", dTable.TableName));
               ctx.LogWriteLine(String.Format("EJTable.WriteExcelFile : Table Columns - {0}", dTable.Columns.Count));
               ctx.LogWriteLine(String.Format("EJTable.WriteExcelFile : Table Rows - {0}", dTable.Rows.Count));
            }

         }
         catch (Exception e)
         {
            ctx.LogWriteLine("EJTable.WriteExcelFile EXCEPTION:" + e.Message);
         }

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
            if (logLine is EJInsert ejLogLine)
            {
               switch (ejLogLine.apType)
               {
                  case APLogType.EJInsert:
                     {
                        base.ProcessRow(ejLogLine);
                        EJInsert(ejLogLine);
                        break;
                     }
                  default:
                     break;
               };
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("OveTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void EJInsert(EJInsert ejLogLine)
      {
         try
         {
            DataTable dataTable = null;
            string tableName = ejLogLine.ejTable;

            if (!dTableSet.Tables.Contains(tableName))
            {
               ctx.ConsoleWriteLogLine(String.Format("EJInsert creating table '{0}'", tableName));
               dataTable = dTableSet.Tables.Add(tableName);

               dataTable.Columns.Add("file", typeof(string));
               dataTable.Columns.Add("time", typeof(string));
               dataTable.Columns.Add("error", typeof(string));
            }
            else
            {
               dataTable = dTableSet.Tables[tableName];
            }

            foreach (string colName in ejLogLine.ejColumns)
            {
               if (!dataTable.Columns.Contains(colName))
               {
                  dataTable.Columns.Add(colName, typeof(string));
               }
            }

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = ejLogLine.LogFile;
            dataRow["time"] = ejLogLine.Timestamp;
            dataRow["error"] = string.Empty;

            for (int i = 0; i < ejLogLine.ejColumns.Length; i++)
            {
               dataRow[ejLogLine.ejColumns[i]] = ejLogLine.ejValues[i];
            }

            dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("EJInsert Exception : " + e.Message);
         }
      }
   }
}

