using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Data;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace BHDView
{
   class BHDTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public BHDTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         // return base.WriteExcelFile();

         string excelFileName = ctx.ExcelFileName;
         Console.WriteLine("Write DataTable to Excel:" + excelFileName);

         bool isTimeAdjustmentWorksheetWritten = false;

         // create Excel 
         // ctx.ConsoleWriteLogLine("Create Excel..." + excelFileName);

         Excel.Application excelApp = new Excel.Application
         {
            Visible = false,
            DisplayAlerts = false
         };

         // ctx.ConsoleWriteLogLine("Instantiate excel objects (application, workbook, worksheets)...");

         Excel.Workbook activeBook;

         if (File.Exists(excelFileName))
         {
            // open existing
            ctx.ConsoleWriteLogLine("Opening existing workbook...");
            activeBook = excelApp.Workbooks.Open(excelFileName);
         }
         else
         {
            // create a new workbook
            ctx.ConsoleWriteLogLine("Creating new workbook...");
            activeBook = excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
         }

         // include the SipSessionTables in the Excel workbook
         foreach (System.Data.DataTable table in BELine.SipSessionTables())
         {
            if (table.Rows.Count > 1 && table.Rows[0][0].ToString().Contains("OPTIONS"))
            {
               // OPTIONS sessions consist only of 1-2 messages, are very short, and often start in the same hh_mm as
               // a real session thus the table has the same name as a real session of interest.
               ctx.ConsoleWriteLogLine($"Ignored OPTIONS session {table.TableName}");
               continue;
            }

            try
            {
               dTableSet.Tables.Add(table);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(">>>EXCEPTION BHDTable.WriteExcelFile : " + e.Message);
               ctx.ConsoleWriteLogLine("Table not included in Excel output.");
            }
         }


         // outer try-catch with a finally clause to clean up Excel properly
         try
         {
            // for each table in the dataset
            // For each table in the DataSet, print the row values.
            foreach (System.Data.DataTable dTable in dTableSet.Tables)
            {
               // ctx.ConsoleWriteLogLine("BaseTable.WriteExcelFile - DataTable: " + dTable.TableName);

               ctx.ConsoleWriteLogLine("DataTable: " + dTable.TableName + " has " + dTable.Rows.Count.ToString() + " rows.");
               if (dTable.Rows.Count == 0)
               {
                  continue;
               }

               // instantiate excel objects (application, workbook, worksheets)
               // ctx.ConsoleWriteLogLine("Instantiate Excel objects...");

               if (!isTimeAdjustmentWorksheetWritten)
               {
                  // TIME ADJUSTMENT WORKSHEET
                  //
                  //   Terminal, Server and Teller logs will seldom be time-synchronized.  To analyze two spreadsheets
                  //   using timestamps, one of them requires adjustment.
                  //
                  //   1.  identify log lines in each of the spreadsheets where you know that one side sent a UDP message
                  //       and the other spreadsheet received it.  Note the two timestamps.
                  //
                  //   2.  on one of the spreadsheets
                  //
                  //       a. insert a column AdjustedTime to the right of the timestamp column, it should be in the same
                  //          time format as the timestamp column.
                  //
                  //       b. on a separate worksheet (name it Adjustments if you wish), enter these cells and formulas:
                  //
                  //           A        B         C
                  //         1   Hrs        0         =(B1*60*60)/86400
                  //         2   Min        4         =(B2*60)/86400
                  //         3   Sec       31         =B3/86400
                  //         4   Msec     305         =(B4/1000)/86400
                  //         5            ADJUSTMENT  =SUM(C1:C4)
                  //
                  //       c. define a name ADJUSTMENT representing the cell C5
                  //
                  //       d. in the AdjustedTime column beside the timestamp column that you chose as a reference
                  //          (for example C21907), insert this adjustment formula:
                  //
                  //               =C21907-ADJUSTMENT
                  //
                  //       e. an adjusted timestamp is displayed in the cell
                  //
                  //       f. back on the Adjustments worksheet, enter values for Hrs/Min/Sec/Msec that cause the
                  //          adjusted timestamp to be similar to the other worksheet
                  //
                  //       g. copy the adjustment formula from step (d) into all the other cells of the AdjustedTime
                  //          column.  You can select the entire column and paste all at once without selecting a cell range.

                  // ctx.ConsoleWriteLogLine("Add TimeAdjustment worksheet...");

                  Excel._Worksheet timeAdjustmentSheet = (Excel._Worksheet)activeBook.Sheets.Add(Before: activeBook.Sheets[activeBook.Sheets.Count]);
                  timeAdjustmentSheet.Activate();
                  try
                  {
                     timeAdjustmentSheet.Name = "TimeAdjustment";
                     isTimeAdjustmentWorksheetWritten = true;
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(">>>EXCEPTION BHDTable.WriteExcelFile : " + e.Message);
                     ctx.ConsoleWriteLogLine("ExcelFile did not like the name 'TimeAdjustment'");
                     timeAdjustmentSheet.Name = "TimeAdjustment" + RandomString(3);
                     ctx.ConsoleWriteLogLine("Renamed to : " + timeAdjustmentSheet.Name);
                  }

                  // quick calculation
                  timeAdjustmentSheet.Cells[1, 5] = "Paste other workbook time";                       // [row, column]
                  timeAdjustmentSheet.Cells[2, 5] = "Paste this workbook time";

                  timeAdjustmentSheet.Cells[1, 6] = "2023-01-01 01:23:04.567";
                  timeAdjustmentSheet.Cells[2, 6] = "2023-01-01 01:23:04.567";

                  timeAdjustmentSheet.Cells[3, 5] = "TIMEADJUSTMENT";
                  timeAdjustmentSheet.Cells[3, 6] = "=F1-F2";

                  // add a Defined Name for the adjustment total
                  activeBook.Names.Add("TIMEADJUSTMENT", timeAdjustmentSheet.Cells[3, 6]);

                  Range timeAdjustmentColumns = timeAdjustmentSheet.Range[timeAdjustmentSheet.Cells[1, 6], timeAdjustmentSheet.Cells[2, 6]];
                  timeAdjustmentColumns.Cells.NumberFormat = "yyyy-mm-dd hh:mm:ss.000";
                  timeAdjustmentColumns.Cells.ColumnWidth = 21;

                  timeAdjustmentColumns = timeAdjustmentSheet.Range[timeAdjustmentSheet.Cells[3, 6], timeAdjustmentSheet.Cells[6, 6]];
                  timeAdjustmentColumns.Cells.NumberFormat = "0.000000";
                  timeAdjustmentColumns.Cells.ColumnWidth = 21;

                  timeAdjustmentColumns = timeAdjustmentSheet.Range[timeAdjustmentSheet.Cells[3, 5], timeAdjustmentSheet.Cells[3, 5]];
                  timeAdjustmentColumns.Cells.ColumnWidth = 35;


                  // manual calculation
                  timeAdjustmentSheet.Cells[1, 1] = "Hrs";                       // [row, column]
                  timeAdjustmentSheet.Cells[2, 1] = "Min";
                  timeAdjustmentSheet.Cells[3, 1] = "Sec";
                  timeAdjustmentSheet.Cells[4, 1] = "Msec";

                  timeAdjustmentSheet.Cells[1, 2] = "0";
                  timeAdjustmentSheet.Cells[2, 2] = "0";
                  timeAdjustmentSheet.Cells[3, 2] = "0";
                  timeAdjustmentSheet.Cells[4, 2] = "0";

                  timeAdjustmentSheet.Cells[1, 3] = "=(B1*60*60)/86400";
                  timeAdjustmentSheet.Cells[2, 3] = "=(B2*60)/86400";
                  timeAdjustmentSheet.Cells[3, 3] = "=B3/86400";
                  timeAdjustmentSheet.Cells[4, 3] = "=(B4/1000)/86400";

                  timeAdjustmentSheet.Cells[5, 2] = "copy to TIMEADJUSTMENT";
                  timeAdjustmentSheet.Cells[5, 3] = "=SUM(C1:C4)";

                  // add a Defined Name for the adjustment total
                  activeBook.Names.Add("MANUALTIMEADJUSTMENT", timeAdjustmentSheet.Cells[5, 3]);

                  timeAdjustmentColumns = timeAdjustmentSheet.Range[timeAdjustmentSheet.Cells[5, 2], timeAdjustmentSheet.Cells[5, 2]];
                  timeAdjustmentColumns.Cells.ColumnWidth = 20;
               }

               // identify column offsets needed later

               int timeColumn = -1;
               int adjustedTimeColumn = -1;
               int traceFileColumn = -1;
               int bucketOffsetColumn = -1;
               int summaryTextColumn = -1;

               // identify columns that will need to be adjusted later
               int colCount = dTable.Columns.Count;
               for (int colNum = 0; colNum < colCount; colNum++)
               {
                  if (dTable.Columns[colNum].ToString().Equals("time", StringComparison.InvariantCultureIgnoreCase))
                  {
                     timeColumn = colNum + 1;
                  }

                  if (dTable.Columns[colNum].ToString().Equals("adjustedtime", StringComparison.InvariantCultureIgnoreCase))
                  {
                     adjustedTimeColumn = colNum + 1;
                  }

                  if (dTable.Columns[colNum].ToString().Equals("file", StringComparison.InvariantCultureIgnoreCase))
                  {
                     traceFileColumn = colNum + 1;
                  }

                  if (dTable.Columns[colNum].ToString().Equals("bucketoffset", StringComparison.InvariantCultureIgnoreCase))
                  {
                     bucketOffsetColumn = colNum + 1;
                  }

                  if (dTable.Columns[colNum].ToString().Equals("summary", StringComparison.InvariantCultureIgnoreCase))
                  {
                     summaryTextColumn = colNum + 1;
                  }
               }

               // create a distinct table from the default view, with option to remove duplicate rows
               // for BHD ATLOGPARSER - DON'T REMOVE DUPLICATE ROWS!  
               System.Data.DataTable distinctTable = dTable.DefaultView.ToTable(); // /*distinct*/ true);

               // ctx.ConsoleWriteLogLine("DataTable: " + dTable.TableName + " has " + distinctTable.Rows.Count.ToString() + " distinct rows.");

               // create a view
               DataView dataView = new DataView(distinctTable);

               // TableName == "Messages"
               if (dTable.TableName == "Messages" && traceFileColumn >= 0 && timeColumn >= 0)
               {
                  // sort by filename and time
                  dataView.Sort = "File, Time ASC";
               }

               // create a 2D array of the cell contents, we have to massage the data because there
               // are some things Excel does not like

               // ctx.ConsoleWriteLogLine("Create a 2D array of the cell contents..." + excelFileName);
               // ctx.ConsoleWriteLogLine($"There are {dataView.Count} data rows total.");

               // avoid OutOfMemory exception by splitting very large views into 2 or more parts, each part goes to a separate worksheet.  This works, but
               // OutOfMemory can still occur if the number of rows written to a worksheet is too large.

               // TODO find a better way than using a constant, to determine when OutOfMemory could occur.  OutOfMemory refers to a [large] data structure
               //      not being able to find enough contiguous memory space.  Added <gcAllowVeryLargeObjects enabled="true" /> to app.config but it doesn't
               //      help.

               int worksheetRowsMax = 100000;
               int numWorksheets = (int)(dataView.Count / worksheetRowsMax) + 1;

               int dataViewRowIndex = 0;
               int remainingViewRowsCount = dataView.Count;

               int sheetStartRow = 1;

               for (int worksheetNumber = 0; worksheetNumber < numWorksheets; worksheetNumber++)
               {
                  // allocate space on the heap for the rows intended for the current worksheet
                  int worksheetRowsToProcess = (remainingViewRowsCount <= worksheetRowsMax) ? remainingViewRowsCount : worksheetRowsMax;
                  remainingViewRowsCount -= worksheetRowsToProcess;

                  ctx.ConsoleWriteLogLine($"Worksheet #{worksheetNumber} writing rows {sheetStartRow}-{sheetStartRow + worksheetRowsToProcess - 1}.");
                  sheetStartRow += worksheetRowsToProcess;

                  // copy the view to a worksheetData array, then write the array to a new Excel worksheet

                  object[,] worksheetData = null;

                  try
                  {
                     try
                     {
                        // release this object when no longer needed!
                        worksheetData = new object[worksheetRowsToProcess, colCount];
                     }
                     catch (OutOfMemoryException ex)
                     {
                        ctx.ConsoleWriteLogLine($">>>EXCEPTION BHDTable.WriteExcelFile trying to allocate a new object for worksheetData {worksheetRowsToProcess} rows : {ex.Message}");
                     }

                     // row 1 is reserved for the column headers
                     // row 2 is reserved for charts
                     // row 3 and below are for data
                     int worksheetHeaderRow = 1;
                     int worksheetChartRow = 2;
                     int worksheetDataStartRow = 3;

                     // 1-based row number used to update formula cell references
                     int worksheetRowNumber = worksheetDataStartRow - 1;

                     // ctx.ConsoleWriteLogLine($"---Copy input rows from XML to output array for Excel.");

                     while (worksheetRowsToProcess > 0)
                     {
                        try
                        {
                           DataRow dataViewRow = dataView.Table.Rows[dataViewRowIndex++];
                           worksheetRowNumber++;

                           // copy columns from the dataView to the data row that will be written to the worksheet
                           for (int colIndex = 0; colIndex < dataViewRow.Table.Columns.Count; colIndex++)
                           {
                              if (dataView.Table.Columns[colIndex].ColumnName == "bucketoffset")
                              {

                              }

                              if (!Convert.IsDBNull(dataViewRow[colIndex]))
                              {
                                 // unless we know the data is all numbers, or field containing a formula,
                                 // escape values starting with = by replacing them with '=
                                 string val = dataViewRow[colIndex].ToString();

                                 if ((colIndex + 1) == adjustedTimeColumn && timeColumn >= 1)
                                 {
                                    if (val.Contains("rowcol+TIMEADJUSTMENT"))
                                    {
                                       // columns are lettered beginning with "A", rows are numbered beginning with 1
                                       char timeColumnLetter = (char)((int)'A' + (timeColumn - 1));

                                       // rowcol with the actual cell coordinate
                                       val = val.Replace("rowcol", $"{timeColumnLetter}{worksheetRowNumber}");
                                    }
                                 }
                                 else if (val.StartsWith("="))
                                 {
                                    // escape data beginning with '='
                                    val = "'" + val;
                                 }

                                 // dimensions are 0-indexed
                                 worksheetData[worksheetRowNumber - worksheetDataStartRow, colIndex] = val;
                              }
                              else
                              {
                                 // replace DBNull
                                 worksheetData[worksheetRowNumber - worksheetDataStartRow, colIndex] = "";
                              }
                           }
                        }
                        catch (Exception ex)
                        {
                           ctx.ConsoleWriteLogLine($">>>EXCEPTION BHDTable.WriteExcelFile retrieving row {dataViewRowIndex - 1} : " + ex.Message);
                           throw;
                        }

                        worksheetRowsToProcess--;
                     }

                     // copy the worksheetData to a new Excel worksheet
                     _Worksheet worksheetSheet = (_Worksheet)activeBook.Sheets.Add(After: activeBook.Sheets[activeBook.Sheets.Count]);
                     worksheetSheet.Activate();

                     // give the worksheet a unique name
                     string worksheetName = (worksheetNumber == 0) ? $"{dTable.TableName}" : $"{dTable.TableName}_{worksheetNumber}";

                     try
                     {
                        ctx.ConsoleWriteLogLine($"---Set the worksheet name to {worksheetName}.");
                        worksheetSheet.Name = worksheetName;
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(">>>EXCEPTION BHDTable.WriteExcelFile assigning worksheet name : " + e.Message);
                        ctx.ConsoleWriteLogLine("Excel did not like the worksheet Name : " + worksheetName);
                        worksheetSheet.Name = worksheetName + RandomString(3);
                        ctx.ConsoleWriteLogLine("Renamed to : " + worksheetSheet.Name);
                     }

                     // copy the column names to the header line in row 1 of the worksheet
                     // Row,col are 1-based and set the destination of the data in the worksheet.

                     int numberOfRows = worksheetData.GetUpperBound(0);
                     int numberOfCols = worksheetData.GetUpperBound(1);

                     int worksheetDataEndRow = worksheetDataStartRow + numberOfRows;

                     // put in it's own scope - headerRowRange and worksheetRange are not needed again.  Range is not IDisposable() so can't use using() {}
                     {
                        // ctx.ConsoleWriteLogLine("---Set the column values for the header row");

                        Range headerRowRange = worksheetSheet.Range[worksheetSheet.Cells[worksheetHeaderRow, 1], worksheetSheet.Cells[worksheetHeaderRow, numberOfCols]];
                        for (int colNum = 1; colNum <= colCount; colNum++)
                        {
                           headerRowRange.Cells[1, colNum] = dTable.Columns[colNum - 1].ToString();
                        }

                        // set worksheet column formats
                        if (timeColumn >= 1)
                        {
                           // ctx.ConsoleWriteLogLine("---Set the format for the time column");

                           Range timeColumns = worksheetSheet.Range[
                              worksheetSheet.Cells[worksheetDataStartRow, timeColumn],
                              worksheetSheet.Cells[worksheetDataEndRow, timeColumn]];
                           timeColumns.Cells.NumberFormat = "yyyy-mm-dd hh:mm:ss.000";
                           timeColumns.Cells.ColumnWidth = 21;
                        }

                        if (adjustedTimeColumn >= 1)
                        {
                           // ctx.ConsoleWriteLogLine("---Set the format for the adjustedtime column");

                           Range timeColumns = worksheetSheet.Range[
                              worksheetSheet.Cells[worksheetDataStartRow, adjustedTimeColumn],
                              worksheetSheet.Cells[worksheetDataEndRow, adjustedTimeColumn]];
                           timeColumns.Cells.NumberFormat = "yyyy-mm-dd hh:mm:ss.000";
                           timeColumns.Cells.ColumnWidth = 21;
                        }

                        if (bucketOffsetColumn >= 1)
                        {
                           // ctx.ConsoleWriteLogLine("---Set the format for the bucketoffset column");

                           Range timeColumns = worksheetSheet.Range[
                              worksheetSheet.Cells[worksheetDataStartRow, bucketOffsetColumn],
                              worksheetSheet.Cells[worksheetDataEndRow, bucketOffsetColumn]];

                           // text value is already correct - adding this NumberFormat causes the minutes to be lost!  (01:05 is rendered as 00:05)
                           // timeColumns.Cells.NumberFormat = "mm:ss";

                           timeColumns.Cells.ColumnWidth = 21;
                        }

                        if (summaryTextColumn >= 1)
                        {
                           // ctx.ConsoleWriteLogLine("---Set the format for the summary column");

                           Range summaryTextColumnsRange = worksheetSheet.Range[
                              worksheetSheet.Cells[worksheetDataStartRow, summaryTextColumn],
                              worksheetSheet.Cells[worksheetDataEndRow, summaryTextColumn]];

                           summaryTextColumnsRange.Cells.ColumnWidth = 200;

                           //timeAdjustmentColumns = timeAdjustmentSheet.Range[timeAdjustmentSheet.Cells[3, 5], timeAdjustmentSheet.Cells[3, 5]];
                           //timeAdjustmentColumns.Cells.ColumnWidth = 35;
                        }

                        // make an Excel range the same size as the data to be copied
                        Range worksheetRange = worksheetSheet.Range[worksheetSheet.Cells[worksheetDataStartRow, 1], worksheetSheet.Cells[worksheetDataEndRow, numberOfCols]];

                        // add the worksheetData to the worksheet
                        try
                        {
                           worksheetRange.Value2 = worksheetData;   // all the rows
                        }
                        catch (OutOfMemoryException ex)
                        {
                           ctx.ConsoleWriteLogLine($">>>EXCEPTION BHDTable.WriteExcelFile copying {numberOfRows} rows of data to worksheet {worksheetSheet.Name} : " + ex.Message);

                           // tried saving Excel and assigning Value2=worksheetData again, but it just threw another out-of-memory exception
                           // if this occurs on the Nth Message_ worksheet, N+1, N+2 etc Message_ worksheets also fail.  But adding the much smaller Session_ worksheets still succeed.
                           // TODO - try closing and re-opening Excel, to add a new worksheet that way?
                           throw;
                        }

                        worksheetRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                        // enable autofilter for all cells
                        // ctx.ConsoleWriteLogLine("---Enable autofilter for all data cells...");

                        worksheetRange.AutoFilter(worksheetDataStartRow, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                     }

                     // ctx.ConsoleWriteLogLine("---Set split row beginning at the column header.");

                     // freeze the top row so that column headers are always visible when scrolling
                     worksheetSheet.Application.ActiveWindow.SplitRow = 1;
                     worksheetSheet.Application.ActiveWindow.FreezePanes = true;

                     if (worksheetName.StartsWith("Session") && bucketOffsetColumn >= 1)
                     {
                        // see MediaStream.InitializeTimeBuckets to get the range of columnn numbers for plotting
                        int chartColumnStart = 24;
                        int chartColumnEnd = 59;

                        // ctx.ConsoleWriteLogLine($"Add charts for the session bucket columns {chartColumnStart} to {chartColumnEnd}");

                        // Add chart in the space occupied by the worksheetChartRow.  The chart really isn't in the cell, it is just in the raster area overlapping the cell.
                        // Make that row higher to accomodate the chart.

                        int chartHeightPixels = 250;
                        int chartWidthPixels = 30;

                        Range chartRowRange = worksheetSheet.Range[worksheetSheet.Cells[worksheetChartRow, 1], worksheetSheet.Cells[worksheetChartRow, numberOfCols]];

                        // size the chart row and columns to a reasonable size to view the charts
                        chartRowRange.EntireRow.RowHeight = chartHeightPixels;
                        chartRowRange.EntireRow.ColumnWidth = chartWidthPixels;

                        ChartObjects charts = worksheetSheet.ChartObjects();

                        // place a chart in the space occupied by the top cell for each column that needs one
                        for (int col = chartColumnStart; col <= chartColumnEnd; col++)
                        {
                           // size the chart to exactly overlay the cell
                           ChartObject chartObject = charts.Add(chartRowRange.Cells[1, col].Left, chartRowRange.Cells[1, col].Top, chartRowRange.Cells[1, col].Width, chartRowRange.Cells[1, col].Height);
                           Chart chart = chartObject.Chart;
                           chart.ChartType = Excel.XlChartType.xlLine;
                           chart.HasTitle = true;
                           chart.ChartTitle.Font.Size = 10;
                           chart.ChartTitle.Font.Bold = false;
                           chart.HasLegend = false;

                           // data range to chart
                           Range chartColumnDataRange = worksheetSheet.Range[worksheetSheet.Cells[worksheetDataStartRow, col], worksheetSheet.Cells[worksheetDataEndRow, col]];

                           chart.SetSourceData(chartColumnDataRange, Excel.XlRowCol.xlColumns);
                           chart.ChartWizard(Source: chartColumnDataRange,
                               Title: worksheetSheet.Cells[worksheetHeaderRow, col],
                               CategoryTitle: "Time",
                               ValueTitle: string.Empty);

                           // Set chart X- and Y-axis ranges using the data in the worksheet column.
                           Range bucketOffsetXAxisRange = worksheetSheet.Range[worksheetSheet.Cells[worksheetDataStartRow, bucketOffsetColumn], worksheetSheet.Cells[worksheetDataEndRow, bucketOffsetColumn]];

                           Excel.Axis xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                           xAxis.CategoryNames = bucketOffsetXAxisRange;
                           xAxis.AxisTitle.Font.Size = 10;
                           xAxis.AxisTitle.Font.Bold = false;
                        }

                        // reduce non-chart columns to make it faster to look at the Session plots at a glance
                        // 1. hide columns that don't have timestamps or plots
                        int hideColumnStart = adjustedTimeColumn + 1;   // E
                        int hideColumnEnd = chartColumnStart - 1;    // V

                        Range hideColumnRange = worksheetSheet.Range[worksheetSheet.Cells[worksheetDataStartRow, hideColumnStart], worksheetSheet.Cells[worksheetDataEndRow, hideColumnEnd]];
                        hideColumnRange.EntireColumn.Hidden = true;

                        // 2. reduce column widths for the columns 2-3 (rownumber, bucketoffset)
                       Range widthColumnRange = worksheetSheet.Range[worksheetSheet.Cells[worksheetDataStartRow, 2], worksheetSheet.Cells[worksheetDataEndRow, 3]];
                        widthColumnRange.EntireColumn.ColumnWidth = 10;
                     }

                     // TODO: user request to color rows based on some criteria
                  }
                  catch (Exception ex)
                  {
                     ctx.ConsoleWriteLogLine(">>>EXCEPTION BHDTable.WriteExcelFile : " + ex.Message);
                  }

                  // free worksheetData memory
                  worksheetData = null;

                  // force garbage collector to run
                  // GC.Collect();
               }

               // saving the file after each table has been processed might be unnecessary
               ctx.ConsoleWriteLogLine("Save the file as : " + excelFileName);
               excelApp.ActiveWorkbook.SaveAs(excelFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                           Type.Missing, Type.Missing);

               // free memory resources
               dataView.Dispose();
               distinctTable.Dispose();

               dataView = null;
               distinctTable = null;
            }
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine($">>>EXCEPTION BHDTable.WriteExcelFile - outer catch before closing Excel : {ex.Message}");
            throw ex;
         }
         finally
         {
            if (!excelApp.ActiveWorkbook.Saved)
            {
               ctx.ConsoleWriteLogLine("Saving Excel file : " + excelFileName);

               excelApp.ActiveWorkbook.SaveAs(excelFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                           Type.Missing, Type.Missing);
            }

            ctx.ConsoleWriteLogLine("Shutting down Excel .");

            activeBook.Close();
            activeBook = null;

            excelApp.Quit();
            while (excelApp.Quitting)
            {
               ctx.ConsoleWriteLogLine(".");
            }

            ctx.ConsoleWriteLogLine(string.Empty);

            excelApp = null;
         }

         return true;
      }


      /*
      /// <summary>
      /// Instruct the view to process this line 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void PreProcess(IContext ctx);

      /// <summary>
      /// Instruct the view to process this line 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void Process(IContext ctx);

      /// <summary>
      /// Instruct the View TimeSeries processing is over. 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void PostProcess(IContext ctx);

      /// <summary>
      /// Instruction to the view to write out its datatable to Excel. 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void WriteExcel(IContext ctx);

      /// <summary>
      /// Instruction to the view to cleanup. 
      /// </summary>
      /// <param name="ctx">Context for the command</param>
      void Cleanup(IContext ctx);
      */



      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is BELine bhdLine)
            {
               // process reads in all the log lines and produces SIP Session Summaries in BHDView_Session_*.xml/xsd
               base.ProcessRow(bhdLine);

               // BHDLogLine writes to the BHDView.xml and can become very large, running out of memory
               if (ctx.opts.BEViews.ToLower().Contains("beehdmessages"))
               {
                  BHDLogLine(bhdLine);
               }
            }
         }
         catch (System.OutOfMemoryException e)
         {
            ctx.LogWriteLine(">>>EXCEPTION BHDTable.ProcessRow : " + e.Message);
            throw;
         }
         catch (Exception e)
         {
            ctx.LogWriteLine(">>>EXCEPTION BHDTable.ProcessRow : " + e.Message);
         }
      }

      protected void BHDLogLine(BELine bhdLine)
      {
         try
         {
            // after changing BHDView.xsd, Clean the solution and rebuild to distribute the .xsd to the /dist folder

            // columns in the same order as the Messages table in BHDView.xsd

            // this gets very slow when the table gets very large > 10000
            // pre-allocate the table rows???
            // when gets too big, export to Excel and clear the table?
            // filter what is written to the table?

            string tableName = "BeeHDMessages";

            // add the TIME ADJUSTMENT column after the timestamp field
            long sheetRow = dTableSet.Tables[tableName].Rows.Count + 2;   // Convert.ToInt64(bhdLine.lineNumber) + 1;
            string timeAdjustmentFormula = "=rowcol+TIMEADJUSTMENT";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["file"] = bhdLine.LogFile;
            dataRow["linenumber"] = bhdLine.lineNumber;
            dataRow["time"] = bhdLine.Timestamp;
            dataRow["adjustedtime"] = timeAdjustmentFormula;

            // TODO - don't include IsRecognized failures because BELine is set up already with 'interesting' and 'non-interesting' tags.  Might
            //        need to adjust later, if there truly is an interesting pattern but the regex fails for a new type of log line.
            if (isOptionIncludePayload) // || !bhdLine.IsRecognized)
            {
               dataRow["Payload"] = bhdLine.logLine;
            }

            dataRow["source"] = bhdLine.source;
            dataRow["interest"] = bhdLine.interestingTag;
            dataRow["level"] = bhdLine.logLevel;
            dataRow["head"] = bhdLine.headTag;
            dataRow["thread"] = bhdLine.threadId;
            dataRow["class"] = bhdLine.className;
            dataRow["method"] = bhdLine.methodName;
            dataRow["machinetype"] = bhdLine.machineType;
            dataRow["direction"] = bhdLine.direction;
            dataRow["protocol"] = bhdLine.protocol;
            dataRow["analysis"] = bhdLine.analysis;
            dataRow["endpoints"] = bhdLine.endPoints;
            dataRow["callstate"] = bhdLine.callState.ToString();
            dataRow["audstate"] = bhdLine.audioState.ToString();
            dataRow["vidstate"] = bhdLine.videoState.ToString();
            dataRow["devstate"] = bhdLine.deviceStateChange;
            dataRow["spkstate"] = bhdLine.speakerState;
            dataRow["micstate"] = bhdLine.microphoneState;
            dataRow["camstate"] = bhdLine.cameraState;
            dataRow["audtxstate"] = bhdLine.audioTransmitterState;
            dataRow["audrxstate"] = bhdLine.audioReceiverState;
            dataRow["vidtxstate"] = bhdLine.videoTransmitterState;
            dataRow["vidrxstate"] = bhdLine.videoReceiverState;
            dataRow["timedelta"] = bhdLine.timeSinceLastMessage;
            dataRow["msgheader"] = bhdLine.msgHeader;
            dataRow["cseqheader"] = bhdLine.cseqHeader;
            dataRow["callidheader"] = bhdLine.callIdHeader;
            dataRow["transidheader"] = bhdLine.transactionId;
            dataRow["contact"] = bhdLine.contact;
            dataRow["originator"] = bhdLine.sessionOriginator;
            dataRow["rportoption"] = bhdLine.rportOption;
            dataRow["msgnote"] = bhdLine.msgNote;
            dataRow["expectnote"] = bhdLine.msgExpectNote;
            dataRow["sumnote"] = bhdLine.summaryNote;
            dataRow["audsent"] = bhdLine.audioSent.ToString();
            dataRow["audreceived"] = bhdLine.audioReceived.ToString();
            dataRow["audlost"] = bhdLine.audioLost;
            dataRow["audjitter"] = bhdLine.audioJitter;
            dataRow["vidsent"] = bhdLine.videoSent.ToString();
            dataRow["vidreceived"] = bhdLine.videoReceived.ToString();
            dataRow["vidlost"] = bhdLine.videoLost;
            dataRow["vidjitter"] = bhdLine.videoJitter;
            dataRow["micdatacount"] = bhdLine.microphoneDataCount;
            dataRow["camdatacount"] = bhdLine.cameraDataCount;

            // dTableSet.Tables[tableName].AcceptChanges();


            /*
            DataRow newrow = dTableSet.Tables["BeeHDMessages"].Rows.Add(
               bhdLine.LogFile,
               bhdLine.lineNumber,
               bhdLine.Timestamp,
               timeAdjustmentFormula,    // see BaseTable.cs
               isOptionIncludePayload || !bhdLine.IsRecognized ? bhdLine.payLoad : string.Empty,
               bhdLine.source,
               bhdLine.interestingTag,
               bhdLine.logLevel,
               bhdLine.headTag,
               bhdLine.threadId,
               bhdLine.className,
               bhdLine.methodName,
               bhdLine.direction,
               bhdLine.protocol,
               bhdLine.analysis,
               bhdLine.endPoints,
               bhdLine.callState.ToString(),
               bhdLine.audioState.ToString(),
               bhdLine.videoState.ToString(),
               bhdLine.deviceStateChange,
               bhdLine.speakerState,
               bhdLine.microphoneState,
               bhdLine.cameraState,
               bhdLine.audioTransmitterState,
               bhdLine.audioReceiverState,
               bhdLine.videoTransmitterState,
               bhdLine.videoReceiverState,
               bhdLine.timeSinceLastMessage,
               bhdLine.msgHeader,
               bhdLine.cseqHeader,
               bhdLine.callIdHeader,
               bhdLine.transactionId,
               bhdLine.contact,
               bhdLine.sessionOriginator,
               bhdLine.rportOption,
               bhdLine.msgNote,
               bhdLine.msgExpectNote,
               bhdLine.summaryNote,
               bhdLine.audioSent.ToString(),
               bhdLine.audioReceived.ToString(),
               bhdLine.audioLost,
               bhdLine.audioJitter,
               bhdLine.videoSent.ToString(),
               bhdLine.videoReceived.ToString(),
               bhdLine.videoLost,
               bhdLine.videoJitter,
               bhdLine.microphoneDataCount,
               bhdLine.cameraDataCount
            );
            */
         }
         catch (NullReferenceException e)
         {
            ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDTable.BHDLogLine : " + e.Message);
            ctx.ConsoleWriteLogLine("Check that BHDView.xsd exists and contains a valid \"Message\" table definition.");
            throw;
         }
         catch (System.OutOfMemoryException e)
         {
            ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDTable.BHDLogLine : " + e.Message);
            ctx.ConsoleWriteLogLine("Too many lines to process - split the input logs into smaller files or process one file at a time.");
            throw;
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDTable.BHDLogLine : " + e.Message);
         }
      }

   }
}


