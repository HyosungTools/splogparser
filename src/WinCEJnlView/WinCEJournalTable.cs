using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Contract;
using Impl;
using LogLineHandler;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinCEJournalView
{
   /// <summary>
   /// Table class for WinCE electronic journal data.
   /// Processes WinCEJournalLine objects into DataTable rows matching EJViewer output format.
   /// </summary>
   internal class WinCEJournalTable : BaseTable
   {
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="ctx">Application context</param>
      /// <param name="viewName">Name of the parent view</param>
      public WinCEJournalTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process a log line into a table row.
      /// Maps WinCEJournalLine properties to columns matching EJViewer format.
      /// </summary>
      /// <param name="logLine">The log line to process</param>
      public override void ProcessRow(ILogLine logLine)
      {
         if (!(logLine is WinCEJournalLine journalLine))
         {
            return;
         }

         try
         {
            // Always add to Summary
            AddSummaryRow(journalLine);

            // Add to detail worksheets based on type
            if (journalLine.journalType == WinCEJournalType.Transaction)
            {
               AddTransactionDetailsRow(journalLine);
            }
            else if (journalLine.journalType == WinCEJournalType.EMV)
            {
               AddEMVDetailsRow(journalLine);
            }
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine($"WinCEJournalTable.ProcessRow exception: {ex.Message}");
         }
      }

      /// <summary>
      /// Prep the tables for Excel - add English descriptions and run analysis
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         string tableName = string.Empty;

         try
         {
            // S U M M A R Y   T A B L E
            tableName = "Summary";

            // Add English descriptions to Summary table
            AddEnglishToSummaryTable();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Exception processing the {tableName} table - {e.Message}");
         }

         try
         {
            // T R A N S A C T I O N   D E T A I L S   T A B L E
            tableName = "TransactionDetails";

            // Add English descriptions to TransactionDetails table
            AddEnglishToTransactionDetailsTable();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Exception processing the {tableName} table - {e.Message}");
         }

         // Write base Excel file first
         bool result = base.WriteExcelFile();

         // Now add the CashDispense analysis charts
         try
         {
            tableName = "CashDispenseByHour";

            // Run the analysis
            CashDispenseAnalyzer analyzer = new CashDispenseAnalyzer();
            DataTable transactionDetails = GetTransactionDetailsTable();
            
            ctx.ConsoleWriteLogLine($"CashDispense: TransactionDetails table exists: {transactionDetails != null}");
            ctx.ConsoleWriteLogLine($"CashDispense: TransactionDetails rows: {transactionDetails?.Rows.Count ?? 0}");

            if (transactionDetails != null && transactionDetails.Rows.Count > 0)
            {
               // Log a sample dispensedamt value
               if (transactionDetails.Rows.Count > 0 && transactionDetails.Columns.Contains("dispensedamt"))
               {
                  ctx.ConsoleWriteLogLine($"CashDispense: Sample dispensedamt value: '{transactionDetails.Rows[0]["dispensedamt"]}'");
               }

               analyzer.Analyze(transactionDetails);

               ctx.ConsoleWriteLogLine($"CashDispense: Analyzer results count: {analyzer.Results?.Count ?? 0}");

               if (analyzer.Results != null && analyzer.Results.Count > 0)
               {
                  // Populate the data table
                  PopulateCashDispenseTable(analyzer);

                  // Write the Excel charts
                  WriteCashDispenseExcel(analyzer.Results);
               }
               else
               {
                  ctx.ConsoleWriteLogLine("CashDispense: No results from analyzer");
               }
            }
            else
            {
               ctx.ConsoleWriteLogLine("CashDispense: TransactionDetails table empty or missing");
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine($"Exception processing the {tableName} table - {e.Message}");
         }

         return result;
      }

      #region Row Addition Methods

      /// <summary>
      /// Add row to Summary worksheet
      /// </summary>
      private void AddSummaryRow(WinCEJournalLine journalLine)
      {
         DataRow dataRow = dTableSet.Tables["Summary"].NewRow();

         dataRow["file"] = journalLine.LogFile;
         dataRow["time"] = FormatStackDateTime(journalLine.Timestamp);
         dataRow["stacknum"] = journalLine.StackNum;
         dataRow["type"] = journalLine.FullType;
         dataRow["typedesc"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["trantype"] = journalLine.TranType;
         dataRow["trantypedesc"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["sequence"] = journalLine.TranSequence;
         dataRow["description"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["requestedamt"] = journalLine.RequestedAmount;
         dataRow["dispensedamt"] = journalLine.DispensedAmount;
         dataRow["surchargeamt"] = journalLine.SurchargeAmount;
         dataRow["errorcode"] = journalLine.ErrorCode;
         dataRow["trandatetime"] = journalLine.TranDateTime;

         dTableSet.Tables["Summary"].Rows.Add(dataRow);
      }

      /// <summary>
      /// Add row to TransactionDetails worksheet
      /// </summary>
      private void AddTransactionDetailsRow(WinCEJournalLine journalLine)
      {
         if (!dTableSet.Tables.Contains("TransactionDetails"))
            return;

         DataRow dataRow = dTableSet.Tables["TransactionDetails"].NewRow();

         dataRow["file"] = journalLine.LogFile;
         dataRow["time"] = FormatStackDateTime(journalLine.Timestamp);
         dataRow["stacknum"] = journalLine.StackNum;
         dataRow["terminalid"] = journalLine.TerminalID;
         dataRow["transno"] = journalLine.TranSequence;
         dataRow["trantype"] = journalLine.TranType;
         dataRow["trantypedesc"] = string.Empty;  // Populated in WriteExcelFile
         dataRow["fromaccount"] = journalLine.FromAccount;
         dataRow["toaccount"] = journalLine.ToAccount;
         dataRow["bankcode"] = journalLine.BankCode;
         dataRow["hostdate"] = journalLine.HostDate;
         dataRow["hosttime"] = journalLine.HostTime;
         dataRow["availablebalance"] = journalLine.AvailableBalance;
         dataRow["retrievalno"] = journalLine.RetrievalNo;
         dataRow["authorizationno"] = journalLine.AuthorizationNo;
         dataRow["settledate"] = journalLine.SettleDate;
         dataRow["surchargeamt"] = journalLine.SurchargeAmount;
         dataRow["requestedamt"] = journalLine.RequestedAmount;
         dataRow["dispensedamt"] = journalLine.DispensedAmount;
         dataRow["ledgerbalance"] = journalLine.LedgerBalance;
         dataRow["procedurecount"] = journalLine.ProcedureCount;
         dataRow["transactionresult"] = journalLine.TransactionResult;
         dataRow["errorcode"] = journalLine.ErrorCode;
         dataRow["carddata"] = journalLine.CardData;
         dataRow["noncashvalue"] = journalLine.NonCashValue;
         dataRow["noncashtype"] = journalLine.NonCashType;

         dTableSet.Tables["TransactionDetails"].Rows.Add(dataRow);
      }

      /// <summary>
      /// Add row to EMVDetails worksheet
      /// </summary>
      private void AddEMVDetailsRow(WinCEJournalLine journalLine)
      {
         if (!dTableSet.Tables.Contains("EMVDetails"))
            return;

         DataRow dataRow = dTableSet.Tables["EMVDetails"].NewRow();

         dataRow["file"] = journalLine.LogFile;
         dataRow["time"] = FormatStackDateTime(journalLine.Timestamp);
         dataRow["stacknum"] = journalLine.StackNum;
         dataRow["terminalid"] = journalLine.TerminalID;
         dataRow["transno"] = journalLine.TranSequence;
         dataRow["authorizationno"] = journalLine.AuthorizationNo;
         dataRow["aid"] = journalLine.AID;
         dataRow["appname"] = journalLine.AppName;
         dataRow["arqc"] = journalLine.ARQC;
         dataRow["arpc"] = journalLine.ARPC;
         dataRow["servicecode"] = journalLine.ServiceCode;
         dataRow["terminalcapability"] = journalLine.TerminalCapability;
         dataRow["posentrymode"] = journalLine.POSEntryMode;
         dataRow["tvr"] = journalLine.TVR;
         dataRow["issueractioncode"] = journalLine.IssuerActionCode;
         dataRow["rawdata"] = journalLine.EMVRawData;

         dTableSet.Tables["EMVDetails"].Rows.Add(dataRow);
      }

      #endregion

      #region English Description Methods

      /// <summary>
      /// Add English descriptions to Summary table columns
      /// </summary>
      private void AddEnglishToSummaryTable()
      {
         if (!dTableSet.Tables.Contains("Summary"))
            return;

         DataTable summaryTable = dTableSet.Tables["Summary"];

         foreach (DataRow row in summaryTable.Rows)
         {
            try
            {
               // typedesc: Extract main type (first char) from type column and look up
               string fullType = row["type"]?.ToString() ?? string.Empty;
               if (!string.IsNullOrEmpty(fullType) && fullType.Length > 0)
               {
                  string mainType = fullType.Substring(0, 1);
                  row["typedesc"] = LookupMessage("MainType", mainType);
               }

               // trantypedesc: Look up trantype code
               string tranType = row["trantype"]?.ToString() ?? string.Empty;
               if (!string.IsNullOrEmpty(tranType))
               {
                  row["trantypedesc"] = LookupMessage("TranType", tranType);
               }

               // description: Look up KindCode (extract from type column after underscore)
               if (!string.IsNullOrEmpty(fullType) && fullType.Contains("_"))
               {
                  string kindCode = fullType.Substring(fullType.IndexOf('_') + 1);
                  row["description"] = LookupMessage("KindCode", kindCode);
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine($"AddEnglishToSummaryTable row exception: {ex.Message}");
            }
         }

         summaryTable.AcceptChanges();
      }

      /// <summary>
      /// Add English descriptions to TransactionDetails table columns
      /// </summary>
      private void AddEnglishToTransactionDetailsTable()
      {
         if (!dTableSet.Tables.Contains("TransactionDetails"))
            return;

         DataTable detailsTable = dTableSet.Tables["TransactionDetails"];

         foreach (DataRow row in detailsTable.Rows)
         {
            try
            {
               // trantypedesc: Look up trantype code
               string tranType = row["trantype"]?.ToString() ?? string.Empty;
               if (!string.IsNullOrEmpty(tranType))
               {
                  row["trantypedesc"] = LookupMessage("TranType", tranType);
               }
            }
            catch (Exception ex)
            {
               ctx.ConsoleWriteLogLine($"AddEnglishToTransactionDetailsTable row exception: {ex.Message}");
            }
         }

         detailsTable.AcceptChanges();
      }

      #endregion

      #region Utility Methods

      /// <summary>
      /// Format timestamp to MM/DD/YYYY HH:MM:SS format
      /// </summary>
      private string FormatStackDateTime(string timestamp)
      {
         if (string.IsNullOrEmpty(timestamp) || timestamp.Length < 19)
            return timestamp;

         try
         {
            // Input: 2024-11-05 00:12:06.000
            // Output: 11/05/2024 00:12:06
            string year = timestamp.Substring(0, 4);
            string month = timestamp.Substring(5, 2);
            string day = timestamp.Substring(8, 2);
            string time = timestamp.Substring(11, 8);

            return $"{month}/{day}/{year} {time}";
         }
         catch
         {
            return timestamp;
         }
      }

      /// <summary>
      /// Generic lookup in Messages table by type and code
      /// </summary>
      private string LookupMessage(string messageType, string code)
      {
         if (string.IsNullOrEmpty(code))
            return string.Empty;

         try
         {
            if (dTableSet.Tables.Contains("Messages"))
            {
               DataTable messagesTable = dTableSet.Tables["Messages"];
               foreach (DataRow row in messagesTable.Rows)
               {
                  string rowType = row["type"]?.ToString();
                  string rowCode = row["code"]?.ToString();
                  if (rowType == messageType && rowCode == code)
                  {
                     return row["description"]?.ToString() ?? string.Empty;
                  }
               }
            }
         }
         catch
         {
            // Return empty on lookup failure
         }

         return string.Empty;
      }

      #endregion

      #region CashDispense Analysis Support

      /// <summary>
      /// Gets the TransactionDetails DataTable for analysis.
      /// </summary>
      /// <returns>The TransactionDetails DataTable, or null if not found.</returns>
      public DataTable GetTransactionDetailsTable()
      {
         if (dTableSet.Tables.Contains("TransactionDetails"))
            return dTableSet.Tables["TransactionDetails"];
         return null;
      }

      /// <summary>
      /// Creates and populates the CashDispenseByHour table from analyzer results.
      /// </summary>
      /// <param name="analyzer">The analyzer containing results.</param>
      public void PopulateCashDispenseTable(CashDispenseAnalyzer analyzer)
      {
         // Create CashDispenseByHour table if it doesn't exist
         if (!dTableSet.Tables.Contains("CashDispenseByHour"))
         {
            DataTable dispenseTable = new DataTable("CashDispenseByHour");
            dispenseTable.Columns.Add("file", typeof(string));
            dispenseTable.Columns.Add("time", typeof(string));
            dispenseTable.Columns.Add("Date", typeof(string));
            dispenseTable.Columns.Add("Hour", typeof(string));
            dispenseTable.Columns.Add("HourLabel", typeof(string));
            dispenseTable.Columns.Add("HourlyAmount", typeof(string));
            dispenseTable.Columns.Add("CumulativeAmount", typeof(string));
            dispenseTable.Columns.Add("TransactionCount", typeof(string));
            dTableSet.Tables.Add(dispenseTable);
         }

         DataTable table = dTableSet.Tables["CashDispenseByHour"];
         table.Clear();

         analyzer.PopulateResultsTable(table);
      }

      /// <summary>
      /// Writes the CashDispenseByHour worksheet with cumulative bar charts.
      /// </summary>
      /// <param name="results">List of daily dispense data from the analyzer.</param>
      public void WriteCashDispenseExcel(List<DailyDispenseData> results)
      {
         if (results == null || results.Count == 0)
            return;

         string excelFileName = ctx.ExcelFileName;
         ctx.ConsoleWriteLogLine("WriteCashDispenseExcel: Adding dispense charts to " + excelFileName);

         Excel.Application excelApp = null;
         Excel.Workbook activeBook = null;

         try
         {
            excelApp = new Excel.Application
            {
               Visible = false,
               DisplayAlerts = false
            };

            activeBook = excelApp.Workbooks.Open(excelFileName);

            // Add new worksheet for charts
            Excel.Worksheet worksheet = (Excel.Worksheet)activeBook.Sheets.Add(After: activeBook.Sheets[activeBook.Sheets.Count]);
            worksheet.Name = "CashDispenseByHour";

            // Write summary table first
            int tableEndRow = WriteCashDispenseSummaryTable(worksheet, results);

            // Chart size and position
            int chartWidth = 500;
            int chartHeight = 300;
            int chartLeft = 480;   // Start to the right of the data table
            int chartTop = 0;
            int chartVGap = 320;   // Vertical gap between charts

            int chartIndex = 0;

            foreach (var daily in results)
            {
               if (daily.HourlyData.Count == 0)
                  continue;

               int top = chartTop + (chartIndex * chartVGap);

               // Build arrays for chart data
               string[] labels = daily.HourlyData.Select(h => h.HourLabel).ToArray();
               decimal[] values = daily.HourlyData.Select(h => h.CumulativeAmount).ToArray();

               // Convert to double array for Excel
               double[] doubleValues = values.Select(v => (double)v).ToArray();

               // Create chart
               Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
               Excel.ChartObject chartObject = chartObjects.Add(chartLeft, top, chartWidth, chartHeight);
               Excel.Chart chart = chartObject.Chart;
               chart.ChartType = Excel.XlChartType.xlColumnClustered;

               // Add series using arrays
               Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection(Type.Missing);
               Excel.Series series = seriesCollection.NewSeries();
               series.Values = doubleValues;
               series.XValues = labels;
               series.Name = "Cumulative Dispensed";

               // Format series - green bars
               // Note: Advanced formatting like series.Format.Fill requires Office core reference
               // Using simpler approach that works without it

               // Title
               chart.HasTitle = true;
               chart.ChartTitle.Text = $"Cash Dispensed - {daily.Date:yyyy-MM-dd} (Total: ${daily.TotalDispensed:N2})";
               chart.ChartTitle.Font.Size = 12;
               chart.ChartTitle.Font.Bold = true;

               // Axis titles
               Excel.Axis xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory);
               xAxis.HasTitle = true;
               xAxis.AxisTitle.Text = "Hour of Day";

               Excel.Axis yAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue);
               yAxis.HasTitle = true;
               yAxis.AxisTitle.Text = "Cumulative Amount ($)";
               yAxis.TickLabels.NumberFormat = "$#,##0";

               // No legend needed for single series
               chart.HasLegend = false;

               // Add data labels showing values
               series.HasDataLabels = true;
               series.DataLabels().NumberFormat = "$#,##0";
               series.DataLabels().Font.Size = 8;
               series.DataLabels().Position = Excel.XlDataLabelPosition.xlLabelPositionOutsideEnd;

               chartIndex++;
            }

            activeBook.Save();
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine($"WriteCashDispenseExcel Exception: {ex.Message}");
         }
         finally
         {
            if (activeBook != null)
            {
               activeBook.Close();
               System.Runtime.InteropServices.Marshal.ReleaseComObject(activeBook);
            }
            if (excelApp != null)
            {
               excelApp.Quit();
               System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
         }

         ctx.ConsoleWriteLogLine("WriteCashDispenseExcel: Complete");
      }

      /// <summary>
      /// Writes a summary table showing hourly dispense data for all days.
      /// </summary>
      /// <returns>The last row used by the table.</returns>
      private int WriteCashDispenseSummaryTable(Excel.Worksheet worksheet, List<DailyDispenseData> results)
      {
         int startRow = 1;
         int startCol = 1;

         // Header row
         worksheet.Cells[startRow, startCol] = "Date";
         worksheet.Cells[startRow, startCol + 1] = "Hour";
         worksheet.Cells[startRow, startCol + 2] = "Hourly Amount";
         worksheet.Cells[startRow, startCol + 3] = "Cumulative";
         worksheet.Cells[startRow, startCol + 4] = "Transactions";

         // Format header row
         Excel.Range headerRange = worksheet.Range[
             worksheet.Cells[startRow, startCol],
             worksheet.Cells[startRow, startCol + 4]
         ];
         headerRange.Font.Bold = true;
         headerRange.Interior.Color = 0xF0E8D5;

         // Data rows
         int dataRow = startRow + 1;

         foreach (var daily in results)
         {
            foreach (var hourly in daily.HourlyData)
            {
               worksheet.Cells[dataRow, startCol] = daily.Date.ToString("yyyy-MM-dd");
               worksheet.Cells[dataRow, startCol + 1] = hourly.HourLabel;
               worksheet.Cells[dataRow, startCol + 2] = hourly.HourlyAmount;
               worksheet.Cells[dataRow, startCol + 3] = hourly.CumulativeAmount;
               worksheet.Cells[dataRow, startCol + 4] = hourly.TransactionCount;

               // Format currency columns
               ((Excel.Range)worksheet.Cells[dataRow, startCol + 2]).NumberFormat = "$#,##0.00";
               ((Excel.Range)worksheet.Cells[dataRow, startCol + 3]).NumberFormat = "$#,##0.00";

               dataRow++;
            }

            // Add a blank row between days for readability
            dataRow++;
         }

         // Auto-fit columns
         Excel.Range allDataRange = worksheet.Range[
             worksheet.Cells[startRow, startCol],
             worksheet.Cells[dataRow - 1, startCol + 4]
         ];
         allDataRange.Columns.AutoFit();

         return dataRow;
      }

      #endregion
   }
}
