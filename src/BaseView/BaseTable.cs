﻿using Contract;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;


namespace Impl
{
   public class BaseTable
   {
      /// <summary>
      /// Constant for the max size of an Excel cell length
      /// </summary>
      protected const int SENSIBLE_EXCEL_CELL_DATA_LENGTH = 2048;
      /// <summary>
      /// Excel standard cell data length
      /// </summary>
      protected const int MAX_EXCEL_CELL_DATA_LENGTH = 32767;
      /// <summary>
      /// Excel maximum rows in a worksheet
      /// </summary>
      protected const int MAX_EXCEL_WORKSHEET_ROWS = 1048576;
      /// <summary>
      /// Log Extrator maximum rows in a worksheet.
      /// </summary>
      protected const int SENSIBLE_MAX_ROW = 100000;

      /// <summary>
      /// row id. Increments for each line processed. 
      /// </summary>
      protected int id;
      /// <summary>
      /// Data table that will become Excel worksheet. 
      /// </summary>
      protected DataSet dTableSet;
      /// <summary>
      /// Manages writing to the log file. 
      /// </summary>
      protected IContext ctx;
      /// <summary>
      /// Contains current session context. 
      /// </summary>
      protected string viewName;
      /// <summary>
      /// Flags if a zero in the worksheet should be shown a blank+
      /// </summary>
      protected bool _zeroAsBlank = false;

      /// <summary>
      /// Include the raw logline in the XML output
      /// </summary>
      public bool isOptionIncludePayload { get; set; } = true;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">The context for the instruction</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public BaseTable(IContext ctx, string viewName)
      {
         Console.WriteLine("BaseTable.constructor");

         dTableSet = new DataSet(viewName);

         id = 0;
         this.ctx = ctx;
         this.viewName = viewName;

         isOptionIncludePayload = this.ctx.opts.RawLogLine;

         Console.WriteLine("BaseTable.constructor complete");

      }

      /// <summary>
      /// Add/Initialize a named datatable to the data set. 
      /// At the same time add 2 columns - File and Time. 
      /// </summary>
      /// <returns>bool</returns>
      protected virtual void InitDataTable(string tableName)
      {
         ctx.ConsoleWriteLogLine("base.InitDataTable - adding table '" + tableName + "'");
         System.Data.DataTable dTable = dTableSet.Tables.Add();
         dTable.TableName = tableName;
         ctx.ConsoleWriteLogLine("base.InitDataTable - adding columns file and time to table '" + tableName + "'");
         AddColumn(tableName, "file");
         AddColumn(tableName, "time");
         return;
      }

      /// <summary>
      /// Add Column to a named Table
      /// </summary>
      /// <returns>void</returns>
      protected (bool success, DataColumn dataColumn) AddColumn(string tableName, string columnName)
      {
         int dTableIndex = dTableSet.Tables.IndexOf(tableName);
         if (dTableIndex < 0)
         {
            ctx.ConsoleWriteLogLine("Could not find table: " + tableName + " to add column '" + columnName + "'");
            return (false, null);
         }
         return (true, dTableSet.Tables[dTableIndex].Columns.Add(columnName, typeof(string)));
      }

      /// <summary>
      /// Create a Row for the named Table. 
      /// </summary>
      /// <param name="tableName">name of the table for the new row</param>
      /// <returns>tuple: success and dataRow</returns>
      protected (bool success, DataRow dataRow) NewRow(string tableName)
      {
         int dTableIndex = dTableSet.Tables.IndexOf(tableName);
         if (dTableIndex < 0)
         {
            ctx.ConsoleWriteLogLine("Could not find table '" + tableName + "' to create row.");
            return (false, null);
         }
         return (true, dTableSet.Tables[dTableIndex].NewRow());
      }

      /// <summary>
      /// Add a Row to the Table. The View populate the row, this method adds it to the table
      /// </summary>
      /// <param name="tableName">Name of the Table where the row should be added. </param>
      /// <param name="dataRow">Data Row to add to the table</param>
      /// <returns>true if successful, false otherwise</returns>
      protected bool AddRow(string tableName, DataRow dataRow)
      {
         int dTableIndex = dTableSet.Tables.IndexOf(tableName);
         if (dTableIndex < 0)
         {
            Console.WriteLine("Could not file table '" + tableName + "' to add row.");
            return false;
         }
         dTableSet.Tables[dTableIndex].Rows.Add(dataRow);
         return true;
      }

      public bool WriteXmlFile()
      {
         string outFile;
         try
         {
            outFile = ctx.WorkFolder + "\\" + viewName + ".xsd";
            ctx.ConsoleWriteLogLine(String.Format("Write out data schema to '{0}'", outFile));
            dTableSet.WriteXmlSchema(outFile);

            outFile = ctx.WorkFolder + "\\" + viewName + ".xml";
            ctx.ConsoleWriteLogLine(String.Format("Write out data set to '{0}'", outFile));
            dTableSet.WriteXml(outFile, XmlWriteMode.WriteSchema);
         }
         catch (InvalidOperationException ex)
         {
            //  a column type in the DataRow being written/read implements IDynamicMetaObjectProvider 
            // and does not implement IXmlSerializable.
            ctx.ConsoleWriteLogLine("Exception (InvalidOperationException): " + ex.Message);
            return false;
         }
         catch (Exception ex)
         {
            // unknown exception 
            ctx.ConsoleWriteLogLine("Exception: " + ex.Message);
            return false;
         }
         ctx.ConsoleWriteLogLine("Wrote XML file: " + outFile);
         return true;
      }

      /// <summary>
      /// Reads the DataTable in from an Xml file. 
      /// The xd/xml can exist in two places. On start up they are in the distribution folder of 
      /// the application and they just have seed data. During parsing, the application is broken 
      /// into two steps: 
      /// a. the parse log file step
      /// b. the write excel step. 
      /// In between these steps the XML files (all loaded with data) are stored in the working folder. 
      /// </summary>
      /// <returns>true if the read is successful, false otherwise. </returns>      
      public bool ReadXmlFile()
      {
         try
         {
            // use the working files for the WRITE EXCEL pass
            if (ctx.ioProvider.Exists(ctx.WorkFolder + "\\" + viewName + ".xsd"))
            {
               // Load the View xsd and xml
               dTableSet.ReadXmlSchema(ctx.WorkFolder + "\\" + viewName + ".xsd");
               dTableSet.ReadXml(ctx.WorkFolder + "\\" + viewName + ".xml");
            }
            else
            {
               // if no working files, use the distribution
               // Load the BaseView and ViewName Xsd and Xml files
               dTableSet.ReadXmlSchema(ctx.ioProvider.GetCurrentDirectory() + "\\BaseView.xsd");
               dTableSet.ReadXml(ctx.ioProvider.GetCurrentDirectory() + "\\BaseView.xml");

               dTableSet.ReadXmlSchema(ctx.ioProvider.GetCurrentDirectory() + "\\" + viewName + ".xsd");
               dTableSet.ReadXml(ctx.ioProvider.GetCurrentDirectory() + "\\" + viewName + ".xml");

            }

            // The Tables we now have available to us
            foreach (System.Data.DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine(String.Format("BaseTable ReadXmlFile loaded Table : {0} with {1} rows", dTable.TableName, dTable.Rows.Count));
            }
         }
         catch (InvalidOperationException ex)
         {
            //  a column type in the DataRow being written/read implements IDynamicMetaObjectProvider 
            // and does not implement IXmlSerializable.
            ctx.ConsoleWriteLogLine("Exception (InvalidOperationException): " + ex.Message);
            return false;
         }
         catch (Exception ex)
         {
            // unknown exception 
            ctx.ConsoleWriteLogLine("Exception: " + ex.Message);
            return false;
         }
         return true;
      }

      public bool CleanupXMLFile()
      {
         try
         {

            string strInFile = ctx.WorkFolder + "\\" + viewName + ".xsd";
            if (File.Exists(strInFile))
            {
               ctx.ConsoleWriteLogLine("Deleting file : " + strInFile);
               File.Delete(strInFile);
            }
            strInFile = ctx.WorkFolder + "\\" + viewName + ".xml";
            if (File.Exists(strInFile))
            {
               ctx.ConsoleWriteLogLine("Deleting file : " + strInFile);
               File.Delete(strInFile);
            }
         }
         catch (InvalidOperationException ex)
         {
            //  a column type in the DataRow being written/read implements IDynamicMetaObjectProvider 
            // and does not implement IXmlSerializable.
            ctx.ConsoleWriteLogLine("Exception (InvalidOperationException): " + ex.Message);
            return false;
         }
         catch (Exception ex)
         {
            // unknown exception 
            ctx.ConsoleWriteLogLine("Exception: " + ex.Message);
            return false;
         }
         return true;
      }

      private void _AddEnglishToTable(string tableName, string[,] colKeyMap)
      {
         // Check if the table exists in the DataSet
         bool tableExists = false;
         foreach (System.Data.DataTable table in dTableSet.Tables)
         {
            if (table.TableName == tableName)
            {
               tableExists = true;
               break;
            }
         }

         if (tableExists)
         {
            // add English to the table based on the colKeyMap
            (bool success, string message) result = (false, string.Empty);
            for (int i = 0; i < colKeyMap.GetLength(0); i++)
            {
               result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables[tableName], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
               if (!result.success)
               {
                  ctx.ConsoleWriteLogLine(String.Format("Error in AddEnglishToTable : {0}", result.message));
               }
            }
         }
      }

      protected virtual void AddEnglishToTable(string tableName, string[,] colKeyMap)
      {
         // Add English to the columns listed in colKeyMap
         if (!string.IsNullOrEmpty(tableName) && colKeyMap != null)
            _AddEnglishToTable(tableName, colKeyMap);

         // Add English to the error column
         if (!string.IsNullOrEmpty(tableName))
         {
            string[,] errKeyMap = new string[1, 2] { { "error", "error" } };

            _AddEnglishToTable(tableName, errKeyMap);
         }
      }

      protected virtual void CompressTable(string tableName, string[] columns, string order = "time ASC")
      {
         ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table start: rows before: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));
         (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables[tableName], order, columns);
         ctx.ConsoleWriteLogLine(String.Format("Compress the {0} Table complete: rows after: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));

         if (!result.success)
         {
            ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
         }
      }

      protected virtual void RemoveDuplicateRows(string tableName, string[] columns)
      {
         ctx.ConsoleWriteLogLine(String.Format("Remove Duplicate Rows from {0}, RowCount before {1}", tableName, dTableSet.Tables[tableName].Rows.Count));
         (bool success, string message) result = _datatable_ops.RemoveDuplicateRows(dTableSet.Tables[tableName], columns);
         ctx.ConsoleWriteLogLine(String.Format("Remove Duplicate Rows from {0}, RowCount after {1}", tableName, dTableSet.Tables[tableName].Rows.Count));

         if (!result.success)
         {
            ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
         }
      }

      protected virtual void DeleteRedundantRows(string tableName, string colName = "file", string critera = "")
      {
         ctx.ConsoleWriteLogLine(String.Format("Delete redundant rows from the {0} Table start: rows before: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));
         DataRow[] dataRows = dTableSet.Tables[tableName].Select();
         List<DataRow> deleteRows = new List<DataRow>();
         foreach (DataRow dataRow in dataRows)
         {
            if (dataRow[colName].ToString().Trim() == critera)
            {
               deleteRows.Add(dataRow);
            }
         }

         foreach (DataRow dataRow in deleteRows)
         {
            dataRow.Delete();
         }
         ctx.ConsoleWriteLogLine(String.Format("Delete redundant rows from the {0} Table start: rows complete: {1}", tableName, dTableSet.Tables[tableName].Rows.Count));
      }

      /// <summary>
      /// PreProcess - steps not related to time series
      /// </summary>
      /// <param name="ctx"></param>
      public virtual void PreProcess(IContext ctx)
      {
         return;
      }

      /// <summary>
      /// PreProcess - steps not related to time series
      /// </summary>
      /// <param name="ctx"></param>
      public virtual void PostProcess()
      {
         return;
      }

      /// <summary>
      /// Process a log line. 
      /// </summary>
      public virtual void ProcessRow(ILogLine logLine)
      {
         return;
      }

      /// <summary>
      /// PreAnalyze - steps not related to time series
      /// </summary>
      /// <param name="ctx"></param>
      public virtual void PreAnalyze(IContext ctx)
      {
         return;
      }

      /// <summary>
      /// PostAnalyze - steps not related to time series
      /// </summary>
      /// <param name="ctx"></param>
      public virtual void PostAnalyze(IContext ctx)
      {
         return;
      }

      /// <summary>
      /// Analyze - steps not related to time series
      /// </summary>
      /// <param name="ctx"></param>
      public virtual void Analyze(IContext ctx)
      {
         return;
      }

      protected (bool success, DataRow dataRow) FindMessages(string type, string code)
      {
         // Create an array for the key values to find.
         object[] findByKeys = new object[2];

         // Set the values of the keys to find.
         findByKeys[0] = type;
         findByKeys[1] = code;

         DataRow foundRow = dTableSet.Tables["Messages"].Rows.Find(findByKeys);
         if (foundRow != null)
         {
            return (true, foundRow);
         }
         else
         {
            return (false, null);
         }
      }

      private static readonly Random random = new Random();

      public static string RandomString(int length)
      {
         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
         return new string(Enumerable.Repeat(chars, length)
             .Select(s => s[random.Next(s.Length)]).ToArray());
      }

      /// <summary>
      /// Write the DataTable out to an Excel worksheet. This is the heavy lift. Most of the write to Excel worksheets 
      /// happens here. 
      /// </summary>
      /// <returns>True if the write is successful, false otherwise.</returns>
      public virtual bool WriteExcelFile()
      {
         // Rename any datatable whose name will collidate with another
         foreach (System.Data.DataTable dTable in dTableSet.Tables)
         {
            if (dTable.TableName.Equals("Status") ||
                dTable.TableName.Equals("Summary") ||
                dTable.TableName.Equals("Deposit") ||
                dTable.TableName.Equals("reject") ||
                dTable.TableName.Equals("reject") ||
                dTable.TableName.Equals("retract") ||
                dTable.TableName.Equals("retain") ||
                dTable.TableName.StartsWith("USD"))
            {
               string tablePrefix = viewName.Replace("View", "");
               ctx.ConsoleWriteLogLine(String.Format("Rename DataTable from {0} to {1}", dTable.TableName, tablePrefix + dTable.TableName));
               dTable.TableName = tablePrefix + dTable.TableName;
            }
         }


         string excelFileName = ctx.ExcelFileName;
         Console.WriteLine("Write DataTable to Excel:" + excelFileName);

         // create Excel 
         ctx.ConsoleWriteLogLine("Create Excel..." + excelFileName);
         Excel.Application excelApp = new Excel.Application
         {
            Visible = false,
            DisplayAlerts = false
         };

         ctx.ConsoleWriteLogLine("Instantiate excel objects (application, workbook, worksheets)...");
         Excel.Workbook activeBook;

         if (File.Exists(excelFileName))
         {
            // open existing
            ctx.ConsoleWriteLogLine("Opening existing workbook...");
            activeBook = excelApp.Workbooks.Open(excelFileName);
         }
         else
         {
            // create new 
            ctx.ConsoleWriteLogLine("Creating new workbook...");
            activeBook = excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
         }

         // for each table in the dataset
         // For each table in the DataSet, print the row values.
         foreach (System.Data.DataTable dTable in dTableSet.Tables)
         {
            ctx.ConsoleWriteLogLine("DataTable: " + dTable.TableName);
            ctx.ConsoleWriteLogLine("DataTable: " + dTable.TableName + " has " + dTable.Rows.Count.ToString() + " rows.");
            if (dTable.Rows.Count == 0)
            {
               continue;
            }

            if (dTable.TableName == "Messages")
            {
               continue;
            }

            // instantiate excel objects (application, workbook, worksheets)
            ctx.ConsoleWriteLogLine("Instantiate Excel objects...");
            Excel._Worksheet activeSheet = (Excel._Worksheet)activeBook.Sheets.Add(Before: activeBook.Sheets[activeBook.Sheets.Count]);
            activeSheet.Activate();
            try
            {
               activeSheet.Name = dTable.TableName;
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("Exception in WriteExcelFile :" + e.Message);
               ctx.ConsoleWriteLogLine("Exception in WriteExcelFile did not like dTable.TableName : " + dTable.TableName);
               activeSheet.Name = dTable.TableName + RandomString(3);
               ctx.ConsoleWriteLogLine("Renamed to : " + activeSheet.Name);
            }

            // add column headers -----------------------------------------------------
            ctx.ConsoleWriteLogLine("Add column headers...");

            int rowOffset = 1; // offset for derived data and links
            int colOffset = 1; // offset for derived data and links

            List<int> timeColumns = new List<int>();

            // add columns based on the data table
            int colCount = dTable.Columns.Count;
            for (int colNum = 0; colNum < colCount; colNum++)
            {
               ctx.ConsoleWriteLogLine("Adding column header: " + dTable.Columns[colNum].ToString());
               activeSheet.Cells[rowOffset, colOffset + colNum] = dTable.Columns[colNum].ToString();

               string colName = dTable.Columns[colNum].ToString().ToLower();
               if (colName.Contains("time") && !colName.Contains("timeout"))
               {
                  timeColumns.Add(colNum);
               }
            }

            // finished with the title row, now point to the next row
            rowOffset++;

            // remove duplicate rows
            System.Data.DataTable distinctTable = dTable.DefaultView.ToTable( /*distinct*/ true);
            ctx.ConsoleWriteLogLine("DataTable: " + dTable.TableName + " has " + distinctTable.Rows.Count.ToString() + " distinct rows.");

            // create a view
            DataView dataView = new DataView(distinctTable)
            {

               // sort by time
               Sort = "Time ASC"
            };

            // create a 2D array of the cell contents, we have to massage the data because there
            // are some things Excel does not like

            ctx.ConsoleWriteLogLine("Create a 2D array of the cell contents..." + excelFileName);
            int rowIndex = 0;
            object[,] rowData = new object[dataView.Count, colCount];
            foreach (DataRowView dataRow in dataView)
            {
               for (int colIndex = 0; colIndex < colCount; colIndex++)
               {
                  if (!Convert.IsDBNull(dataRow[colIndex]))
                  {
                     // unless we know the data is all numberrs (e.g. Dispense) we need to check for values 
                     // starting with = and replace them with '= or else this procedure blows up
                     string val = dataRow[colIndex].ToString();
                     if (val.StartsWith("="))
                     {
                        val = "'" + val;
                     }
                     rowData[rowIndex, colIndex] = val;
                  }
                  else
                  {
                     rowData[rowIndex, colIndex] = "";
                  }
               }
               rowIndex++;
            }

            if (rowIndex > 0)
            {

               try
               {
                  // set format for date/time column to be readable

                  foreach (int timeColumn in timeColumns)
                  {
                     Console.WriteLine($"Set the formula for the date/time column '{dTable.Columns[timeColumn].ToString()}'...");
                     Microsoft.Office.Interop.Excel.Range timeColumnRange = activeSheet.Range[activeSheet.Cells[rowOffset, timeColumn+1], activeSheet.Cells[dataView.Count + 1, timeColumn+1]];
                     timeColumnRange.Cells.NumberFormat = "yyyy-mm-dd hh:mm:ss.000";
                     timeColumnRange.Cells.ColumnWidth = 21;
                  }

                  // copy the 2D data array into the excel worksheet starting at rowOffset, colOffset
                  Microsoft.Office.Interop.Excel.Range dispenseRange = activeSheet.Range[activeSheet.Cells[rowOffset, colOffset], activeSheet.Cells[dataView.Count + rowOffset - 1, colCount + colOffset - 1]];
                  dispenseRange.Value2 = rowData;
                  dispenseRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                  if (_zeroAsBlank)
                  {
                     // blank out any zeros in the range for readability
                     Excel.FormatCondition fc = (Excel.FormatCondition)dispenseRange.FormatConditions.Add(
                        Type: Excel.XlFormatConditionType.xlCellValue,
                        Operator: Excel.XlFormatConditionOperator.xlEqual,
                        Formula1: "=0");

                     fc.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                  }


                  // enable autofilter for all cells
                  ctx.ConsoleWriteLogLine("Enable autofilter for all cells...");
                  Microsoft.Office.Interop.Excel.Range allCellRange = null;
                  allCellRange = (Excel.Range)activeSheet.Range[activeSheet.Cells[1, 1], activeSheet.Cells[dataView.Count, colCount + 2]];
                  allCellRange.AutoFilter(2, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                  // disable text wrapping
                  allCellRange.WrapText = false; 

                  // freeze the top row so that column headers are always visible when scrolling
                  activeSheet.Application.ActiveWindow.SplitRow = 1;
                  activeSheet.Application.ActiveWindow.FreezePanes = true;

                  // add conditional formatting to highlight Exceptions
                  ctx.ConsoleWriteLogLine("Add format condition to highlight cells containing 'Exception'...");
                  FormatCondition format = (FormatCondition)(allCellRange.FormatConditions.Add(XlFormatConditionType.xlTextString,
                                                   Type.Missing, Type.Missing, Type.Missing,
                                                   "Exception",
                                                   XlContainsOperator.xlContains,   //XlFormatConditionOperator.xlEqual,
                                                   Type.Missing, Type.Missing)) ;
                  format.Font.Bold = true;
                  format.Font.Color = 0x000000FF;

               }
               catch (Exception ex)
               {
                  ctx.ConsoleWriteLogLine("EXCEPTION:" + ex.Message);
               }
            }
         }

         // save the file
         ctx.ConsoleWriteLogLine("Save the file : " + excelFileName);
         Excel.Workbook workBook = excelApp.ActiveWorkbook;
         workBook.SaveAs(excelFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                     Type.Missing, Type.Missing);

         // shutdown excel
         ctx.ConsoleWriteLogLine("Shutdown Excel...");
         activeBook.Close();
         excelApp.Quit();

         ctx.ConsoleWriteLogLine("Finished writing Excel.");
         ctx.ConsoleWriteLogLine("Wrote Excel file: " + excelFileName);
         return true;
      }
   }
}


