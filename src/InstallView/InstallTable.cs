using System;
using System.Data;
using System.IO;
using Contract;
using Impl;
using LogLineHandler;
using Excel = Microsoft.Office.Interop.Excel;

namespace InstallView
{
   class InstallTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public InstallTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         return true; 

         string tableName = string.Empty;

         //// P R O G R A M S  T A B L E

         //tableName = "Programs";
         //ctx.ConsoleWriteLogLine(String.Format("{0} : WriteExcel", this.viewName));

         //// Remove duplicate rows - use distinctTable going forward
         //ctx.ConsoleWriteLogLine(String.Format("{0} : Remove Duplicate Rows", this.viewName));
         //DataTable distinctTable = dTableSet.Tables[tableName].DefaultView.ToTable(true);
         //distinctTable.TableName = tableName;

         //ctx.ConsoleWriteLogLine("DataTable: " + distinctTable.TableName);
         //ctx.ConsoleWriteLogLine("DataTable: " + distinctTable.TableName + " has " + distinctTable.Rows.Count.ToString() + " rows.");
         //if (distinctTable.Rows.Count == 0)
         //{
         //   // if there are no rows, dont create a worksheet
         //   return true;
         //}

         //string excelFileName = ctx.WorkFolder + "\\" + Path.GetFileNameWithoutExtension(ctx.ZipFileName) + ".xlsx";
         //Console.WriteLine("Write DataTable to Excel:" + excelFileName);

         //// create Excel 
         //ctx.ConsoleWriteLogLine("Create Excel..." + excelFileName);
         //Excel.Application excelApp = new Excel.Application
         //{
         //   Visible = false,
         //   DisplayAlerts = false
         //};

         //ctx.ConsoleWriteLogLine("Instantiate excel objects (application, workbook, worksheets)...");
         //Excel.Workbook activeBook;

         //if (File.Exists(excelFileName))
         //{
         //   // open existing
         //   ctx.ConsoleWriteLogLine("Opening existing workbook...");
         //   activeBook = excelApp.Workbooks.Open(excelFileName);
         //}
         //else
         //{
         //   // create new 
         //   ctx.ConsoleWriteLogLine("Creating new workbook...");
         //   activeBook = excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
         //}

         //try
         //{
         //   // instantiate excel objects (application, workbook, worksheets)
         //   ctx.ConsoleWriteLogLine("Instantiate Excel objects...");
         //   Excel._Worksheet activeSheet = (Excel._Worksheet)activeBook.Sheets.Add(Before: activeBook.Sheets[activeBook.Sheets.Count]);
         //   activeSheet.Activate();
         //   try
         //   {
         //      activeSheet.Name = distinctTable.TableName;
         //   }
         //   catch (Exception e)
         //   {
         //      ctx.ConsoleWriteLogLine("Exception in WriteExcelFile :" + e.Message);
         //      ctx.ConsoleWriteLogLine("Exception in WriteExcelFile did not like dTable.TableName : " + distinctTable.TableName);
         //      activeSheet.Name = distinctTable.TableName + RandomString(3);
         //      ctx.ConsoleWriteLogLine("Renamed to : " + activeSheet.Name);
         //   }

         //   // add column headers 
         //   ctx.ConsoleWriteLogLine("Add column headers...");

         //   int rowOffset = 1; // offset for derived data and links
         //   int colOffset = 1; // offset for derived data and links

         //   // add a first column (A1) for the XmlParam
         //   ctx.ConsoleWriteLogLine(String.Format("Add a first column for the XmlParams rowOffset = {0}, colOffset = {1}", rowOffset, colOffset));
         //   activeSheet.Cells[rowOffset, colOffset++] = "installed";
         //   activeSheet.Cells[rowOffset, colOffset++] = "name";
         //   activeSheet.Cells[rowOffset, colOffset++] = "version";
         //   activeSheet.Cells[rowOffset, colOffset++] = "comment";

         //   colOffset = 1;
         //   rowOffset++;

         //   for (int i = 0; i < distinctTable.Rows.Count; i++)
         //   {
         //      for (int j = 0; j < distinctTable.Columns.Count; j++)
         //      {
         //         activeSheet.Cells[rowOffset + i, colOffset + j] = distinctTable.Rows[i][j].ToString().Trim();
         //      }
         //   }

         //   // set format for date/time column to be readable
         //   Console.WriteLine("Set the formula for the date/time column...");
         //   Microsoft.Office.Interop.Excel.Range timeColumn = activeSheet.Range[activeSheet.Cells[2, 1], activeSheet.Cells[distinctTable.Rows.Count + 1, 1]];
         //   timeColumn.Cells.NumberFormat = "yyyy-mm-dd";
         //   timeColumn.Cells.ColumnWidth = 15;

         //   // set column widths on other columns
         //   Microsoft.Office.Interop.Excel.Range nextColumn = activeSheet.Range[activeSheet.Cells[2, 2], activeSheet.Cells[distinctTable.Rows.Count + 1, 2]];
         //   nextColumn.Cells.ColumnWidth = 52;
         //   nextColumn = activeSheet.Range[activeSheet.Cells[2, 3], activeSheet.Cells[distinctTable.Rows.Count + 1, 3]];
         //   nextColumn.Cells.ColumnWidth = 15;

         //   // enable autofilter for all cells
         //   ctx.ConsoleWriteLogLine("Enable autofilter for all cells...");
         //   Microsoft.Office.Interop.Excel.Range allCellRange = null;
         //   allCellRange = (Excel.Range)activeSheet.Range[activeSheet.Cells[1, 1], activeSheet.Cells[distinctTable.Rows.Count, distinctTable.Columns.Count]];
         //   allCellRange.AutoFilter(2, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

         //   // freeze the top row so that column headers are always visible when scrolling
         //   activeSheet.Application.ActiveWindow.SplitRow = 1;
         //   activeSheet.Application.ActiveWindow.FreezePanes = true;


         //   // P A C K A G E S  T A B L E

         //   tableName = "Packages";
         //   ctx.ConsoleWriteLogLine("Settings : WriteExcel");

         //   // Remove duplicate rows - use distinctTable going forward
         //   ctx.ConsoleWriteLogLine("Settings : Remove Duplicate Rows");
         //   distinctTable = dTableSet.Tables[tableName].DefaultView.ToTable(true);
         //   distinctTable.TableName = tableName;

         //   // instantiate excel objects (application, workbook, worksheets)
         //   ctx.ConsoleWriteLogLine("Instantiate Excel objects...");
         //   activeSheet = (Excel._Worksheet)activeBook.Sheets.Add(Before: activeBook.Sheets[activeBook.Sheets.Count]);
         //   activeSheet.Activate();
         //   try
         //   {
         //      activeSheet.Name = distinctTable.TableName;
         //   }
         //   catch (Exception e)
         //   {
         //      ctx.ConsoleWriteLogLine("Exception in WriteExcelFile :" + e.Message);
         //      ctx.ConsoleWriteLogLine("Exception in WriteExcelFile did not like dTable.TableName : " + distinctTable.TableName);
         //      activeSheet.Name = distinctTable.TableName + RandomString(3);
         //      ctx.ConsoleWriteLogLine("Renamed to : " + activeSheet.Name);
         //   }

         //   // add column headers 
         //   ctx.ConsoleWriteLogLine("Add column headers...");

         //   rowOffset = 1; // offset for derived data and links
         //   colOffset = 1; // offset for derived data and links

         //   // add a first column (A1) for the XmlParam
         //   ctx.ConsoleWriteLogLine(String.Format("Add a first column for the XmlParams rowOffset = {0}, colOffset = {1}", rowOffset, colOffset));
         //   activeSheet.Cells[rowOffset, colOffset++] = "installed";
         //   activeSheet.Cells[rowOffset, colOffset++] = "name";
         //   activeSheet.Cells[rowOffset, colOffset++] = "status";
         //   activeSheet.Cells[rowOffset, colOffset++] = "comment";

         //   colOffset = 1;
         //   rowOffset++;

         //   for (int i = 0; i < distinctTable.Rows.Count; i++)
         //   {
         //      for (int j = 0; j < distinctTable.Columns.Count; j++)
         //      {
         //         activeSheet.Cells[rowOffset + i, colOffset + j] = distinctTable.Rows[i][j].ToString().Trim();
         //      }
         //   }

         //   // set format for date/time column to be readable
         //   Console.WriteLine("Set the formula for the date/time column...");
         //   timeColumn = activeSheet.Range[activeSheet.Cells[2, 1], activeSheet.Cells[distinctTable.Rows.Count + 1, 1]];
         //   timeColumn.Cells.NumberFormat = "yyyy-mm-dd hh:mm:ss.000";
         //   timeColumn.Cells.ColumnWidth = 21;

         //   // set column widths on other columns
         //   nextColumn = activeSheet.Range[activeSheet.Cells[2, 2], activeSheet.Cells[distinctTable.Rows.Count + 1, 2]];
         //   nextColumn.Cells.ColumnWidth = 52;
         //   nextColumn = activeSheet.Range[activeSheet.Cells[2, 3], activeSheet.Cells[distinctTable.Rows.Count + 1, 3]];
         //   nextColumn.Cells.ColumnWidth = 15;

         //   // enable autofilter for all cells
         //   ctx.ConsoleWriteLogLine("Enable autofilter for all cells...");
         //   allCellRange = null;
         //   allCellRange = (Excel.Range)activeSheet.Range[activeSheet.Cells[1, 1], activeSheet.Cells[distinctTable.Rows.Count, distinctTable.Columns.Count]];
         //   allCellRange.AutoFilter(2, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

         //   // freeze the top row so that column headers are always visible when scrolling
         //   activeSheet.Application.ActiveWindow.SplitRow = 1;
         //   activeSheet.Application.ActiveWindow.FreezePanes = true;

         //   // save the file
         //   ctx.ConsoleWriteLogLine("Save the file : " + excelFileName);
         //   Excel.Workbook workBook = excelApp.ActiveWorkbook;
         //   workBook.SaveAs(excelFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
         //               Type.Missing, Type.Missing);
         //}
         //catch (Exception e)
         //{
         //   ctx.LogWriteLine(String.Format("{0}.WriteExcelFile EXCEPTION:", this.viewName) + e.Message);
         //}

         //// shutdown excel
         //ctx.ConsoleWriteLogLine("Shutdown Excel...");
         //activeBook.Close();
         //excelApp.Quit();

         //ctx.ConsoleWriteLogLine("Finished writing Excel.");
         //ctx.ConsoleWriteLogLine("Wrote Excel file: " + excelFileName);
         //return true;
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is MachineInfo machineInfo)
            {
               base.ProcessRow(logLine);
               APLOG_INSTALL(machineInfo);
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("InstallTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void APLOG_INSTALL(MachineInfo machineInfo)
      {
         try
         {
            // populate the programs table. 
            for (int i = 0; i < machineInfo.installedPrograms.GetLength(0); i++)
            {
               DataRow dataRow = dTableSet.Tables["Programs"].Rows.Add();

               dataRow["name"] = machineInfo.installedPrograms[i, 0];
               dataRow["installed"] = machineInfo.installedPrograms[i, 1];
               dataRow["version"] = machineInfo.installedPrograms[i, 2];

               dTableSet.Tables["Programs"].AcceptChanges();
            }

            // populate the packages table. 
            for (int i = 0; i < machineInfo.installedPackages.GetLength(0); i++)
            {
               DataRow dataRow = dTableSet.Tables["Packages"].Rows.Add();

               dataRow["name"] = machineInfo.installedPackages[i, 0];
               dataRow["installed"] = machineInfo.installedPackages[i, 1];
               dataRow["status"] = machineInfo.installedPackages[i, 2];

               dTableSet.Tables["Packages"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLOG_INSTALL Exception : " + e.Message);
         }
      }
   }
}
