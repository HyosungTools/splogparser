using Contract;
using System;
using System.Data;
using System.IO;
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
      /// trace file of the log line
      /// </summary>
      protected string _traceFile = string.Empty;
      /// <summary>
      /// grab the date/time from a logline
      /// </summary>
      protected string _logDate = string.Empty;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">The context for the instruction</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public BaseTable(IContext ctx, string viewName)
      {
         Console.WriteLine("BaseTable.constructor");

         dTableSet = new DataSet(viewName);
         if (ctx.ioProvider.Exists(ctx.ioProvider.GetCurrentDirectory() + "\\" + viewName + ".xsd"))
         {
            ctx.ConsoleWriteLogLine("Schema files exists, loading : " + ctx.ioProvider.GetCurrentDirectory() + "\\" + viewName + ".xsd");
            dTableSet.ReadXmlSchema(ctx.ioProvider.GetCurrentDirectory() + "\\" + viewName + ".xsd");
         }

         id = 0;
         this.ctx = ctx;
         this.viewName = viewName;

         Console.WriteLine("BaseTable.constructor complete");

      }

      /// <summary>
      /// Add/Initialize a named datatable to the data set. 
      /// At the same time add 2 coumns - File and Time. 
      /// </summary>
      /// <returns>bool</returns>
      protected virtual void InitDataTable(string tableName)
      {
         ctx.ConsoleWriteLogLine("base.InitDataTable - adding table '" + tableName + "'");
         DataTable dTable = dTableSet.Tables.Add();
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
         string outFile = ctx.WorkFolder + "\\" + viewName + ".xml";
         try
         {
            ctx.ConsoleWriteLogLine("Write out data set to " + outFile);
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
      /// </summary>
      /// <returns>true if the read is successful, false otherwise. </returns>
      public bool ReadXmlFile()
      {
         // if the work folder xml file exists, load it
         string strInFile = ctx.WorkFolder + "\\" + viewName + ".xml";
         if (!ctx.ioProvider.Exists(strInFile))
         {
            // otherwise fall back to the default xml file, if it exists, load it
            strInFile = ctx.ioProvider.GetCurrentDirectory() + "\\" + viewName + ".xml";
         }

         try
         {
            // if the XML file exists, load it
            if (ctx.ioProvider.Exists(strInFile))
            {
               dTableSet.ReadXml(strInFile);
            }
            else
            {
               // if there is no XML file, initialize the db Table in code
               ctx.ConsoleWriteLogLine("base.ReadXMLFile - Calling InitDataTable (" + viewName + ")");
               InitDataTable(viewName);
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
         ctx.ConsoleWriteLogLine("Read XML file: " + strInFile);
         return true;
      }

      public bool CleanupXMLFile()
      {
         string strInFile = ctx.WorkFolder + "\\" + viewName + ".xml";

         if (File.Exists(strInFile))
         {
            ctx.ConsoleWriteLogLine("Deleting file : " + strInFile);
            File.Delete(strInFile);
            return true;
         }
         return false;
      }

      /// <summary>
      /// Process a log line. 
      /// </summary>
      public virtual void ProcessRow(string traceFile, string logLine)
      {
         _traceFile = traceFile;
         _logDate = LogTime.GetTimeFromLogLine(logLine);
         return;
      }

      /// <summary>
      /// Write the DataTable out to an Excel worksheet. This is the heavy lift. Most of the write to Excel worksheets 
      /// happens here. 
      /// </summary>
      /// <returns>True if the write is successful, false otherwise.</returns>
      public virtual bool WriteExcelFile()
      {
         string excelFileName = ctx.WorkFolder + "\\" + Path.GetFileNameWithoutExtension(ctx.ZipFileName) + ".xlsx";
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
         foreach (DataTable dTable in dTableSet.Tables)
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
            activeSheet.Name = dTable.TableName;

            // add column headers -----------------------------------------------------
            ctx.ConsoleWriteLogLine("Add column headers...");

            int rowOffset = 1; // offset for derived data and links
            int colOffset = 1; // offset for derived data and links

            // add columns based on the data table
            int colCount = dTable.Columns.Count;
            for (int colNum = 0; colNum < colCount; colNum++)
            {
               ctx.ConsoleWriteLogLine("Adding column header: " + dTable.Columns[colNum].ToString());
               activeSheet.Cells[rowOffset, colOffset + colNum] = dTable.Columns[colNum].ToString();
            }

            // finished with the title row, now point to the next row
            rowOffset++;

            // remove duplicate rows
            DataTable distinctTable = dTable.DefaultView.ToTable( /*distinct*/ true);
            ctx.ConsoleWriteLogLine("DataTable: " + dTable.TableName + " has " + distinctTable.Rows.Count.ToString() + " distinct rows.");

            // create a view
            DataView dataView = new DataView(distinctTable);

            // sort by time
            dataView.Sort = "Time ASC";

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
                  Console.WriteLine("Set the formula for the date/time column...");
                  Microsoft.Office.Interop.Excel.Range timeColumn = activeSheet.Range[activeSheet.Cells[rowOffset, 1], activeSheet.Cells[dataView.Count + 1, 1]];
                  timeColumn.Cells.NumberFormat = "YYYY-MM-ddTHH:mm:ss.000";
                  timeColumn.Cells.ColumnWidth = 20;

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

                  // freeze the top row so that column headers are always visible when scrolling
                  activeSheet.Application.ActiveWindow.SplitRow = 1;
                  activeSheet.Application.ActiveWindow.FreezePanes = true;

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


