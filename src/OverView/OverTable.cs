using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Contract;
using Impl;
using LogLineHandler;
using Excel = Microsoft.Office.Interop.Excel;

namespace OverView
{
   class OverTable : BaseTable
   {
      string FITSwitchOnUsNextStateNumbers = string.Empty;
      string FITSwitchForeignNextStateNumbers = string.Empty;
      string FITSwitchOtherNextStateNumbers = string.Empty;

      /// <summary>Resource row being assembled from a group of snapshot lines.</summary>
      private DataRow _pendingResourceRow = null;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public OverTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      #region StateWallClock Analysis Support

      /// <summary>
      /// Gets the Summary DataTable for analysis.
      /// </summary>
      /// <returns>The Summary DataTable, or null if not found.</returns>
      public DataTable GetSummaryTable()
      {
         if (dTableSet.Tables.Contains("Summary"))
            return dTableSet.Tables["Summary"];
         return null;
      }

      /// <summary>Accessor for the raw Resource snapshot table.</summary>
      public DataTable GetResourceTable()
      {
         if (dTableSet.Tables.Contains("Resource"))
            return dTableSet.Tables["Resource"];
         return null;
      }

      /// <summary>
      /// Creates and populates the StateWallClock table from analyzer results.
      /// </summary>
      /// <param name="analyzer">The analyzer containing results.</param>
      public void PopulateStateWallClockTable(StateWallClockAnalyzer analyzer)
      {
         // Create StateWallClock table if it doesn't exist
         if (!dTableSet.Tables.Contains("StateWallClock"))
         {
            DataTable wallClockTable = new DataTable("StateWallClock");
            wallClockTable.Columns.Add("Date", typeof(string));
            wallClockTable.Columns.Add("Period", typeof(string));
            wallClockTable.Columns.Add("State", typeof(string));
            wallClockTable.Columns.Add("Label", typeof(string));
            wallClockTable.Columns.Add("StartTime", typeof(string));
            wallClockTable.Columns.Add("EndTime", typeof(string));
            wallClockTable.Columns.Add("DurationSeconds", typeof(double));
            wallClockTable.Columns.Add("DurationFormatted", typeof(string));
            dTableSet.Tables.Add(wallClockTable);
         }

         DataTable table = dTableSet.Tables["StateWallClock"];
         table.Clear();

         analyzer.PopulateResultsTable(table);
      }

      /// <summary>
      /// Writes the StateWallClock worksheet with wall clock pie charts.
      /// </summary>
      /// <param name="results">List of period state segments from the analyzer.</param>
      public void WriteStateWallClockExcel(List<PeriodStateSegments> results)
      {
         if (results == null || results.Count == 0)
            return;

         string excelFileName = ctx.ExcelFileName;
         ctx.ConsoleWriteLogLine("WriteStateWallClockExcel: Adding wall clock charts to " + excelFileName);

         Excel.Application excelApp = new Excel.Application
         {
            Visible = false,
            DisplayAlerts = false
         };

         Excel.Workbook activeBook = excelApp.Workbooks.Open(excelFileName);

         // Add new worksheet
         Excel._Worksheet worksheet = (Excel._Worksheet)activeBook.Sheets.Add(After: activeBook.Sheets[activeBook.Sheets.Count]);
         worksheet.Name = "StateWallClock";

         // Write summary table first
         WriteStateWallClockSummaryTable(worksheet, results);

         // State colors (BGR format for Excel)
         var stateColors = new Dictionary<string, int>
      {
         { "InService", 0x00AA00 },      // Green
         { "OutOfService", 0x0066FF },   // Orange
         { "OffLine", 0x0000FF },        // Red
         { "PowerUp", 0xFF6600 },        // Blue
         { "Supervisor", 0xFF0099 },     // Purple
         { "Unknown", 0xC0C0C0 }         // Light Gray
      };

         // Chart size and position: K1 to O14 for first chart
         // row height 15, column width 8.43 (default)
         int chartWidth = 240;
         int chartHeight = 210;
         int chartLeft = 480;   // Column K start (approximately 10 columns * 48 points)
         int chartTop = 0;
         int chartHGap = 250;   // Horizontal gap between AM and PM charts
         int chartVGap = 220;   // Vertical gap between day rows

         int chartIndex = 0;

         var resultsByDate = results.GroupBy(r => r.Date).OrderBy(g => g.Key).ToList();

         foreach (var dateGroup in resultsByDate)
         {
            DateTime date = dateGroup.Key;

            foreach (var period in dateGroup.OrderBy(p => p.Period))
            {
               if (period.Segments.Count == 0)
                  continue;

               int rowPos = chartIndex / 2;
               int colPos = chartIndex % 2;
               int left = chartLeft + (colPos * chartHGap);
               int top = chartTop + (rowPos * chartVGap);

               var sortedSegments = period.Segments;  // Already in chronological order

               // Build arrays for chart data
               string[] labels = sortedSegments.Select(s => s.Label).ToArray();
               double[] values = sortedSegments.Select(s => s.DurationSeconds).ToArray();

               // Create chart
               Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
               Excel.ChartObject chartObject = chartObjects.Add(left, top, chartWidth, chartHeight);
               Excel.Chart chart = chartObject.Chart;
               chart.ChartType = Excel.XlChartType.xlPie;

               // Add series using arrays
               Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection(Type.Missing);
               Excel.Series series = seriesCollection.NewSeries();
               series.Values = values;
               series.XValues = labels;
               series.Name = $"{date:yyyy-MM-dd} {period.Period}";

               // Title only
               chart.HasTitle = true;
               chart.ChartTitle.Text = $"{date:yyyy-MM-dd} {period.Period}";
               chart.ChartTitle.Font.Size = 10;
               chart.ChartTitle.Font.Bold = true;

               // No legend - color key is in the data table
               chart.HasLegend = false;

               // No data labels
               series.HasDataLabels = false;

               // Apply colors based on state
               try
               {
                  for (int i = 0; i < sortedSegments.Count; i++)
                  {
                     string state = sortedSegments[i].State;
                     if (stateColors.TryGetValue(state, out int color))
                     {
                        Excel.Point point = (Excel.Point)series.Points(i + 1);
                        point.Interior.Color = color;
                     }
                  }
               }
               catch (Exception ex)
               {
                  ctx.ConsoleWriteLogLine($"Chart formatting error: {ex.Message}");
               }

               chartIndex++;
            }
         }

         // Add color legend in the data area
         WriteStateColorLegend(worksheet, stateColors);

         activeBook.Save();
         activeBook.Close();
         excelApp.Quit();

         ctx.ConsoleWriteLogLine("WriteStateWallClockExcel: Complete");
      }

      /// <summary>
      /// Writes a summary table showing state segments for all periods.
      /// </summary>
      private void WriteStateWallClockSummaryTable(Excel._Worksheet worksheet, List<PeriodStateSegments> results)
      {
         int startRow = 1;
         int startCol = 1;

         // Header row
         worksheet.Cells[startRow, startCol] = "Date";
         worksheet.Cells[startRow, startCol + 1] = "Period";
         worksheet.Cells[startRow, startCol + 2] = "Start";
         worksheet.Cells[startRow, startCol + 3] = "End";
         worksheet.Cells[startRow, startCol + 4] = "State";
         worksheet.Cells[startRow, startCol + 5] = "Duration";
         worksheet.Cells[startRow, startCol + 6] = "Percent";

         // Format header row
         Excel.Range headerRange = worksheet.Range[
             worksheet.Cells[startRow, startCol],
             worksheet.Cells[startRow, startCol + 6]
         ];
         headerRange.Font.Bold = true;
         headerRange.Interior.Color = 0xF0E8D5;

         // Data rows
         int dataRow = startRow + 1;
         double totalSeconds = 12 * 60 * 60; // 12 hours

         foreach (var period in results)
         {
            foreach (var segment in period.Segments)
            {
               worksheet.Cells[dataRow, startCol] = period.Date.ToString("yyyy-MM-dd");
               worksheet.Cells[dataRow, startCol + 1] = period.Period;
               worksheet.Cells[dataRow, startCol + 2] = segment.StartTime.ToString("HH:mm:ss");
               worksheet.Cells[dataRow, startCol + 3] = segment.EndTime.ToString("HH:mm:ss");
               worksheet.Cells[dataRow, startCol + 4] = segment.State;

               int hours = (int)segment.Duration.TotalHours;
               int mins = segment.Duration.Minutes;
               int secs = segment.Duration.Seconds;
               worksheet.Cells[dataRow, startCol + 5] = $"{hours:D2}:{mins:D2}:{secs:D2}";

               double percentage = (segment.DurationSeconds / totalSeconds) * 100;
               worksheet.Cells[dataRow, startCol + 6] = $"{percentage:F1}%";

               dataRow++;
            }
         }

         // Auto-fit columns
         Excel.Range allDataRange = worksheet.Range[
             worksheet.Cells[startRow, startCol],
             worksheet.Cells[dataRow - 1, startCol + 6]
         ];
         allDataRange.Columns.AutoFit();
      }

      /// <summary>The 32-bit user address-space ceiling in MB. 4096 assumes XTM is
      /// LargeAddressAware (its VM size exceeding 2 GB proves it is); use 2048 for a
      /// non-LAA build.</summary>
      private const double Win32AddressCeilingMB = 4096.0;

      /// <summary>
      /// Writes the ResourceWallClock worksheet: per day, a 96-slot (15-minute)
      /// grid of resource metrics with four line charts (memory, VM size, private
      /// bytes, handles) over a fixed 24-hour x-axis. VM and private charts carry a
      /// dashed reference line at the 32-bit address-space ceiling.
      /// </summary>
      public void WriteResourceWallClockExcel(List<ResourceDaySlots> results)
      {
         if (results == null || results.Count == 0)
            return;

         string excelFileName = ctx.ExcelFileName;
         ctx.ConsoleWriteLogLine("WriteResourceWallClockExcel: Adding resource charts to " + excelFileName);

         Excel.Application excelApp = new Excel.Application
         {
            Visible = false,
            DisplayAlerts = false
         };

         Excel.Workbook activeBook = excelApp.Workbooks.Open(excelFileName);

         try
         {
            Excel._Worksheet worksheet = (Excel._Worksheet)activeBook.Sheets.Add(After: activeBook.Sheets[activeBook.Sheets.Count]);
            worksheet.Name = "ResourceWallClock";

            string[] metricTitles = { "Memory (MB)", "VM Size (MB)", "Private Size (MB)", "Handles" };
            int[] metricColumns = { 2, 3, 4, 5 };
            int[] metricColors = { 0x00DD8A37, 0x00759E1D, 0x00004AE2, 0x001775BA };  // BGR

            // Rectangular charts: 2:1 (wider on x than y).
            double chartW = 480, chartH = 240, hGap = 500, vGap = 260;

            int currentRow = 1;

            foreach (ResourceDaySlots day in results)
            {
               int headerRow = currentRow;
               int dataFirst = headerRow + 1;
               int dataLast = headerRow + ResourceDaySlots.SlotCount;

               worksheet.Cells[headerRow, 1] = day.Date.ToString("yyyy-MM-dd");
               ((Excel.Range)worksheet.Cells[headerRow, 1]).Font.Bold = true;
               worksheet.Cells[headerRow, 2] = "Memory";
               worksheet.Cells[headerRow, 3] = "VM";
               worksheet.Cells[headerRow, 4] = "Private";
               worksheet.Cells[headerRow, 5] = "Handles";

               object[,] block = new object[ResourceDaySlots.SlotCount, 5];
               for (int slot = 0; slot < ResourceDaySlots.SlotCount; slot++)
               {
                  block[slot, 0] = ResourceDaySlots.SlotLabel(slot);
                  block[slot, 1] = day.Memory[slot].HasValue ? (object)day.Memory[slot].Value : null;
                  block[slot, 2] = day.VmSize[slot].HasValue ? (object)day.VmSize[slot].Value : null;
                  block[slot, 3] = day.Private[slot].HasValue ? (object)day.Private[slot].Value : null;
                  block[slot, 4] = day.Handles[slot].HasValue ? (object)day.Handles[slot].Value : null;
               }
               worksheet.Range[worksheet.Cells[dataFirst, 1], worksheet.Cells[dataLast, 5]].Value2 = block;

               Excel.Range anchor = (Excel.Range)worksheet.Cells[headerRow, 7];
               double baseLeft = (double)anchor.Left;
               double baseTop = (double)anchor.Top;

               Excel.Range xValues = worksheet.Range[worksheet.Cells[dataFirst, 1], worksheet.Cells[dataLast, 1]];
               Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);

               for (int m = 0; m < metricColumns.Length; m++)
               {
                  bool addWall = (m == 1 || m == 2);   // VM Size, Private Size

                  double left = baseLeft + ((m % 2) * hGap);
                  double top = baseTop + ((m / 2) * vGap);

                  Excel.ChartObject chartObject = chartObjects.Add(left, top, chartW, chartH);
                  Excel.Chart chart = chartObject.Chart;
                  chart.ChartType = Excel.XlChartType.xlLine;

                  Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection(Type.Missing);

                  Excel.Series series = seriesCollection.NewSeries();
                  series.XValues = xValues;
                  series.Values = worksheet.Range[worksheet.Cells[dataFirst, metricColumns[m]], worksheet.Cells[dataLast, metricColumns[m]]];
                  series.Name = metricTitles[m];
                  series.Border.Color = metricColors[m];
                  series.Border.Weight = Excel.XlBorderWeight.xlMedium;
                  series.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;

                  if (addWall)
                  {
                     double[] ceiling = new double[ResourceDaySlots.SlotCount];
                     for (int k = 0; k < ceiling.Length; k++)
                     {
                        ceiling[k] = Win32AddressCeilingMB;
                     }

                     Excel.Series wallSeries = seriesCollection.NewSeries();
                     wallSeries.XValues = xValues;
                     wallSeries.Values = ceiling;
                     wallSeries.Name = "32-bit limit";
                     wallSeries.Border.Color = 0x000000FF;   // red (BGR)
                     wallSeries.Border.LineStyle = Excel.XlLineStyle.xlDash;
                     wallSeries.MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone;
                  }

                  string title = day.Date.ToString("yyyy-MM-dd") + "  " + metricTitles[m];
                  if (addWall)
                  {
                     title += "  (red dash = 4 GB cap)";
                  }
                  chart.HasTitle = true;
                  chart.ChartTitle.Text = title;
                  chart.HasLegend = false;

                  Excel.Axis categoryAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                  categoryAxis.TickLabelSpacing = 8;
                  categoryAxis.TickLabels.Orientation = (Excel.XlTickLabelOrientation)45;

                  Excel.Axis valueAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                  valueAxis.MinimumScaleIsAuto = true;
                  valueAxis.MaximumScaleIsAuto = true;
               }

               currentRow = dataLast + 2;
            }
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine("WriteResourceWallClockExcel EXCEPTION: " + ex.Message);
         }
         finally
         {
            activeBook.Save();
            activeBook.Close();
            excelApp.Quit();
         }

         ctx.ConsoleWriteLogLine("WriteResourceWallClockExcel: Complete");
      }

      /// <summary>
      /// Writes a color legend showing what each state color means.
      /// </summary>
      private void WriteStateColorLegend(Excel._Worksheet worksheet, Dictionary<string, int> stateColors)
      {
         int startRow = 1;
         int startCol = 9;  // Column I

         worksheet.Cells[startRow, startCol] = "State Colors";
         worksheet.Cells[startRow, startCol].Font.Bold = true;

         int row = startRow + 1;
         foreach (var kvp in stateColors.OrderBy(k => k.Key))
         {
            // Color swatch
            Excel.Range colorCell = worksheet.Cells[row, startCol];
            colorCell.Interior.Color = kvp.Value;
            colorCell.Value2 = "";

            // State name
            worksheet.Cells[row, startCol + 1] = kvp.Key;

            row++;
         }

         // Adjust column widths
         ((Excel.Range)worksheet.Columns[startCol, Type.Missing]).ColumnWidth = 3;
         ((Excel.Range)worksheet.Columns[startCol + 1, Type.Missing]).AutoFit();
      }

      /// <summary>
      /// Orders the analytical worksheets left-to-right where present:
      /// StateWallClock, then ResourceWallClock, then OverSummary. Missing sheets
      /// are skipped; any other sheets stay to the right of these.
      /// </summary>
      public void OrderWorksheets()
      {
         string excelFileName = ctx.ExcelFileName;

         Excel.Application excelApp = new Excel.Application
         {
            Visible = false,
            DisplayAlerts = false
         };
         Excel.Workbook activeBook = excelApp.Workbooks.Open(excelFileName);

         try
         {
            string[] desiredOrder = { "StateWallClock", "ResourceWallClock", "OverSummary" };
            Excel._Worksheet previous = null;

            foreach (string name in desiredOrder)
            {
               Excel._Worksheet sheet = null;
               foreach (Excel._Worksheet candidate in activeBook.Sheets)
               {
                  if (candidate.Name == name)
                  {
                     sheet = candidate;
                     break;
                  }
               }

               if (sheet == null)
               {
                  continue;
               }

               if (previous == null)
               {
                  sheet.Move(Before: activeBook.Sheets[1]);
               }
               else
               {
                  sheet.Move(After: previous);
               }
               previous = sheet;
            }
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine("OrderWorksheets EXCEPTION: " + ex.Message);
         }
         finally
         {
            activeBook.Save();
            activeBook.Close();
            excelApp.Quit();
         }
      }

      #endregion

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is APLine apLogLine)
            {
               switch (apLogLine.apType)
               {
                  /* mode */
                  case APLogType.APLOG_CURRENTMODE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField apLineField = (APLineField)apLogLine;
                           APLINE(apLineField, "mode", apLineField.field);
                        }
                        break;
                     }

                  /* host */
                  case APLogType.APLOG_HOST:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField apLineField = (APLineField)apLogLine;
                           APLINE(apLineField, "host", apLineField.field);
                        }
                        break;
                     }

                  /* headset */

                  /* card */
                  case APLogType.APLOG_CARD_OPEN:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "open");
                        break;
                     }
                  case APLogType.APLOG_CARD_CLOSE:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "close");
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIAPRESENT:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "present");
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIANOTPRESENT:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "not present");
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIAINSERTED:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "inserted");
                        break;
                     }
                  case APLogType.APLOG_CARD_ONREADCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "read");
                        break;
                     }
                  case APLogType.APLOG_CARD_ONEJECTCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "ejected");
                        break;
                     }
                  case APLogType.APLOG_CARD_PAN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "card", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_TIMEOUT:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "card", "timeout");
                        break;
                     }

                  case APLogType.APLOG_TRANSACTION_TIMEOUT:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "state", "transaction timeout");
                        break;
                     }


                  /* rfid */
                  case APLogType.APLOG_RFID_DELETE:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "rfid", "delete data");
                        break;
                     }

                  case APLogType.APLOG_RFID_ACCEPTCANCELLED:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "rfid", "accept cancelled");
                        break;
                     }

                  case APLogType.APLOG_RFID_ONMEDIAINSERTED:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "rfid", "inserted");
                        break;
                     }

                  case APLogType.APLOG_RFID_ONMEDIAREMOVED:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "rfid", "removed");
                        break;
                     }

                  case APLogType.APLOG_RFID_TIMEREXPIRED:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "rfid", "timer expired");
                        break;
                     }

                  case APLogType.APLOG_RFID_ONMEDIAPRESENT:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "rfid", "present");
                        break;
                     }

                  case APLogType.APLOG_RFID_ONMEDIANOTPRESENT:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "rfid", "not present");
                        break;
                     }

                  case APLogType.APLOG_RFID_WAITCOMMANDCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "rfid", lineField.field);
                        }
                        break;
                     }

                  /* emv */

                  case APLogType.APLOG_EMV_INIT:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "emv", "init");
                        break;
                     }


                  case APLogType.APLOG_EMV_INITCHIP:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "emv", "init_chip");
                        break;
                     }

                  case APLogType.APLOG_EMV_BUILD_CANDIDATE_LIST:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE2(lineField, "emv", "build candidate list", "comment", lineField.field == "1" ? "success" : "failed");
                        }
                        break;
                     }

                  case APLogType.APLOG_EMV_CREATE_APPNAME_LIST:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE2(lineField, "emv", "create appname list", "comment", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_EMV_APP_SELECTED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE2(lineField, "emv", "app selected", "comment", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_EMV_PAN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "card", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_EMV_CURRENCY_TYPE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineEmvCurrencyType)
                        {
                           APLineEmvCurrencyType lineField = (APLineEmvCurrencyType)apLogLine;
                           APLINE(lineField, "emv", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_EMV_OFFLINE_AUTH:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE2(lineField, "emv", "offline auth", "comment", lineField.field == "1" ? "success" : "failed");
                        }
                        break;
                     }
                  case APLogType.APLOG_EMV_FAULT_SMART_CARDREADER:
                     {
                        base.ProcessRow(logLine);
                        APLINE(apLogLine, "emv", "smartcard fault");
                        break;
                     }


                  /* pin */
                  case APLogType.APLOG_PIN_OPEN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "open");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_CLOSE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "close");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_ISPCI:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "pin", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_ISTR31:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "pin", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_ISTR34:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "pin", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_KEYIMPORTED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "key_imported");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_RAND:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "rand_generated");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_PINBLOCK:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "pinblock");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_PINBLOCK_FAILED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "pinblock_failed");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_TIMEOUT:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "timeout");
                        }
                        break;
                     }
                  case APLogType.APLOG_PIN_READCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "pin", "read_complete");
                        }
                        break;
                     }

                  case APLogType.APLOG_PIN_EXECUTECOMMAND:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLinePinCommand pinCmd)
                        {
                           APLINE(pinCmd, "pin", pinCmd.field);
                        }
                        break;
                     }

                  case APLogType.APLOG_PIN_XFSCODE_ERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField lineField)
                        {
                           APLINE2(lineField, "pin", "ERROR", "error", lineField.field);
                        }
                        break;
                     }

                  case APLogType.APLOG_PIN_FATALERROR:
                     {
                        base.ProcessRow(logLine);
                        APLINE2(apLogLine, "pin", "FATAL ERROR", "error", "OnFatalError");
                        break;
                     }

                  /* device */

                  case APLogType.APLOG_CDM_ONLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cdm", "online");
                        }
                        break;
                     }

                  case APLogType.APLOG_CDM_OFFLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cdm", "offline");
                        }
                        break;
                     }

                  case APLogType.APLOG_CDM_ONHWERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cdm", "hwerror");
                        }
                        break;
                     }

                  case APLogType.APLOG_CDM_DEVERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cdm", "deverror");
                        }
                        break;
                     }

                  case APLogType.APLOG_CDM_ONOK:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cdm", "ok");
                        }
                        break;
                     }

                  case APLogType.APLOG_CIM_ONLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cim", "online");
                        }
                        break;
                     }

                  case APLogType.APLOG_CIM_OFFLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cim", "offline");
                        }
                        break;
                     }

                  case APLogType.APLOG_CIM_ONHWERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cim", "hwerror");
                        }
                        break;
                     }

                  case APLogType.APLOG_CIM_DEVERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cim", "deverror");
                        }
                        break;
                     }

                  case APLogType.APLOG_CIM_ONOK:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "cim", "ok");
                        }
                        break;
                     }

                  case APLogType.APLOG_IPM_ONLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "ipm", "online");
                        }
                        break;
                     }

                  case APLogType.APLOG_IPM_OFFLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "ipm", "offline");
                        }
                        break;
                     }

                  case APLogType.APLOG_IPM_ONHWERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "ipm", "hwerror");
                        }
                        break;
                     }

                  case APLogType.APLOG_IPM_DEVERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "ipm", "deverror");
                        }
                        break;
                     }

                  case APLogType.APLOG_IPM_ONOK:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "ipm", "ok");
                        }
                        break;
                     }

                  case APLogType.APLOG_MMA_ONLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "mma", "online");
                        }
                        break;
                     }

                  case APLogType.APLOG_MMA_OFFLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "mma", "offline");
                        }
                        break;
                     }

                  case APLogType.APLOG_MMA_ONHWERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "mma", "hwerror");
                        }
                        break;
                     }

                  case APLogType.APLOG_MMA_DEVERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "mma", "deverror");
                        }
                        break;
                     }

                  case APLogType.APLOG_MMA_ONOK:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "mma", "ok");
                        }
                        break;
                     }

                  case APLogType.APLOG_RCT_ONLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "rct", "online");
                        }
                        break;
                     }

                  case APLogType.APLOG_RCT_OFFLINE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "rct", "offline");
                        }
                        break;
                     }

                  case APLogType.APLOG_RCT_ONHWERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "rct", "hwerror");
                        }
                        break;
                     }

                  case APLogType.APLOG_RCT_DEVERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "rct", "deverror");
                        }
                        break;
                     }

                  case APLogType.APLOG_RCT_ONOK:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "rct", "ok");
                        }
                        break;
                     }


                  /* screen */
                  case APLogType.APLOG_DISPLAYLOAD:
                  case APLogType.APLOG_SCREENWINDOW:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "screen", lineField.field);
                        }
                        break;
                     }

                  case APLogType.APLOG_ADDKEY:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is AddKey)
                        {
                           AddKey addKey = (AddKey)apLogLine;
                           APLOG_ADDKEY(addKey);
                        }
                        break;
                     }

                  case APLogType.APLOG_FLW_SWITCH_FIT:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE_FLW_SWITCH_FIT(lineField);
                        }
                        break;
                     }

                  case APLogType.NDC_ATM2HOST11:
                  case APLogType.NDC_ATM2HOST12:
                  case APLogType.NDC_ATM2HOST22:
                  case APLogType.NDC_ATM2HOST23:
                  case APLogType.NDC_ATM2HOST51:
                  case APLogType.NDC_ATM2HOST61:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Atm2Host)
                        {
                           Atm2Host atm2Host = (Atm2Host)apLogLine;
                           APLINE2(atm2Host, "state", "ATM2HOST", "comment", atm2Host.english);
                        }
                        break;
                     }

                  case APLogType.NDC_HOST2ATM1:
                  case APLogType.NDC_HOST2ATM3:
                  case APLogType.NDC_HOST2ATM4:
                  case APLogType.NDC_HOST2ATM6:
                  case APLogType.NDC_HOST2ATM7:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Host2Atm)
                        {
                           Host2Atm host2Atm = (Host2Atm)apLogLine;
                           APLINE2(host2Atm, "state", "HOST2ATM", "comment", host2Atm.english);
                        }
                        break;
                     }

                  case APLogType.Core_DispensedAmount:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Core_DispensedAmount)
                        {
                           Core_DispensedAmount dispenseAmount = (Core_DispensedAmount)apLogLine;
                           APLINE2(dispenseAmount, "state", dispenseAmount.name, "comment", String.Format("Dispense Amount : ${0}", dispenseAmount.amount));
                        }
                        break;
                     }

                  case APLogType.Core_ProcessWithdrawalTransaction_Account:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Core_ProcessWithdrawalTransaction_Account)
                        {
                           Core_ProcessWithdrawalTransaction_Account transAccount = (Core_ProcessWithdrawalTransaction_Account)apLogLine;
                           APLINE2(transAccount, "state", transAccount.name, "comment", String.Format("Process WD Transaction Account : {0}", transAccount.account));
                        }
                        break;
                     }

                  case APLogType.Core_ProcessWithdrawalTransaction_Amount:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is Core_ProcessWithdrawalTransaction_Amount)
                        {
                           Core_ProcessWithdrawalTransaction_Amount transAccount = (Core_ProcessWithdrawalTransaction_Amount)apLogLine;
                           APLINE2(transAccount, "state", transAccount.name, "comment", String.Format("Process WD Transaction Amount : ${0}", transAccount.amount));
                        }
                        break;
                     }

                  /* function key */
                  case APLogType.APLOG_FUNCTIONKEY_SELECTED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "functionkey", lineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_FUNCTIONKEY_SELECTED2:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "functionkey", lineField.field);
                        }
                        break;
                     }

                  case APLogType.APLOG_KEYPRESS:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLINE(apLogLine, "functionkey", "keypress");
                        }
                        break;
                     }

                  /* Error */

                  case APLogType.APLOG_ERROR:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE2(lineField, "error", "error", "comment", lineField.field);
                        }
                        break;
                     }

                  case APLogType.APLOG_EXCEPTION:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE2(lineField, "error", "exception", "comment", lineField.field);
                        }
                        break;
                     }

                  /* cash dispenser */

                  case APLogType.CashDispenser_OnDenominateComplete:
                     {
                        base.ProcessRow(apLogLine);
                        APLINE(apLogLine, "dispensed", "denominated");
                        break;
                     }

                  case APLogType.CashDispenser_OnPresentComplete:
                     {
                        base.ProcessRow(apLogLine);
                        APLINE(apLogLine, "dispensed", "presented");
                        break;
                     }
                  case APLogType.CashDispenser_OnItemsTaken:
                     {
                        base.ProcessRow(apLogLine);
                        APLINE(apLogLine, "dispensed", "items taken");
                        break;
                     }

                  case APLogType.CashDispenser_UpdateTypeInfoToDispense:
                     {
                        base.ProcessRow(apLogLine);
                        if (apLogLine is CashDispenser_UpdateTypeInfoToDispense typeInfoToDispense)
                        {
                           APLINE(apLogLine, "dispensed", String.Format("${0}", typeInfoToDispense.dispenseAmount));
                        }
                        break;
                     }

                  /* Account */

                  case APLogType.APLOG_ACCOUNT_ENTERED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "account", lineField.field);
                        }
                        break;
                     }

                  /* operator menu */

                  case APLogType.APLOG_OPERATOR_MENU:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField lineField = (APLineField)apLogLine;
                           APLINE(lineField, "operatormenu", lineField.field);
                        }
                        break;
                     }

                  /* deposit */

                  case APLogType.APLOG_MEMORY:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is MemoryLine memoryLine)
                        {
                           APLOG_MEMORY(memoryLine);
                        }
                        break;
                     }

                  default:
                     break;
               }
               ;
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("OveTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void APLINE(APLine lineField, string columnName, string columnValue)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = lineField.LogFile;
            dataRow["time"] = lineField.Timestamp;
            dataRow["error"] = lineField.HResult;
            dataRow["TID"] = lineField.TID;

            dataRow["mode"] = string.Empty;
            dataRow["host"] = string.Empty;
            dataRow["card"] = string.Empty;
            dataRow["pin"] = string.Empty;
            dataRow["screen"] = string.Empty;
            dataRow["state"] = string.Empty;
            dataRow["functionkey"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dataRow[columnName] = columnValue;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLINE Exception : " + e.Message);
         }
      }

      protected void APLINE2(APLine lineField, string columnName, string columnValue, string columnName2, string columnValue2)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = lineField.LogFile;
            dataRow["time"] = lineField.Timestamp;
            dataRow["error"] = lineField.HResult;
            dataRow["TID"] = lineField.TID;

            dataRow["mode"] = string.Empty;
            dataRow["host"] = string.Empty;
            dataRow["card"] = string.Empty;
            dataRow["pin"] = string.Empty;
            dataRow["screen"] = string.Empty;
            dataRow["state"] = string.Empty;
            dataRow["functionkey"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dataRow[columnName] = columnValue;
            dataRow[columnName2] = columnValue2;

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLINE2 Exception : " + e.Message);
         }
      }

      protected void APLOG_ADDKEY(AddKey addKey)
      {
         try
         {
            if (addKey.keyName == "FITSwitchOnUsNextStateNumbers")
            {
               FITSwitchOnUsNextStateNumbers = addKey.value;
            }
            else if (addKey.keyName == "FITSwitchForeignNextStateNumbers")
            {
               FITSwitchForeignNextStateNumbers = addKey.value;
            }
            else if (addKey.keyName == "FITSwitchOtherNextStateNumbers")
            {
               FITSwitchOtherNextStateNumbers = addKey.value;
            }

            return;
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLOG_ADDKEY Exception : " + e.Message);
         }
      }

      protected void APLINE_FLW_SWITCH_FIT(APLineField lineField)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Summary"].Rows.Add();

            dataRow["file"] = lineField.LogFile;
            dataRow["time"] = lineField.Timestamp;
            dataRow["error"] = lineField.HResult;
            dataRow["TID"] = lineField.TID;

            dataRow["mode"] = string.Empty;
            dataRow["host"] = string.Empty;
            dataRow["card"] = string.Empty;
            dataRow["pin"] = string.Empty;
            dataRow["screen"] = string.Empty;
            dataRow["state"] = string.Empty;
            dataRow["functionkey"] = string.Empty;
            dataRow["comment"] = string.Empty;

            if (FITSwitchForeignNextStateNumbers.Contains(lineField.field))
            {
               dataRow["card"] = "foreign";
            }
            else if (FITSwitchOnUsNextStateNumbers.Contains(lineField.field))
            {
               dataRow["card"] = "on-us";
            }
            else if (FITSwitchOtherNextStateNumbers.Contains(lineField.field))
            {
               dataRow["card"] = "other";
            }
            else
            {
               dataRow["card"] = lineField.field;
            }

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLINE2 Exception : " + e.Message);
         }
      }

      protected void APLOG_MEMORY(MemoryLine memoryLine)
      {
         try
         {
            DataTable resourceTable = dTableSet.Tables["Resource"];

            // A 'Date Time' line always starts a new snapshot row. Also start fresh if there
            // is no pending row, or the pending row is stale (snapshot lines arrive within ms).
            bool startNewRow = (memoryLine.metric == MemoryMetric.DateTimeCount) || (_pendingResourceRow == null);

            if (!startNewRow)
            {
               DateTime pendingTime;
               DateTime lineTime;
               if (DateTime.TryParse((string)_pendingResourceRow["time"], out pendingTime) &&
                   DateTime.TryParse(memoryLine.Timestamp, out lineTime) &&
                   (lineTime - pendingTime).TotalSeconds > 5)
               {
                  startNewRow = true;
               }
            }

            if (startNewRow)
            {
               _pendingResourceRow = resourceTable.Rows.Add();
               _pendingResourceRow["file"] = memoryLine.LogFile;
               _pendingResourceRow["time"] = memoryLine.Timestamp;
               _pendingResourceRow["count"] = string.Empty;
               _pendingResourceRow["memoryMB"] = string.Empty;
               _pendingResourceRow["vmSizeMB"] = string.Empty;
               _pendingResourceRow["privateSizeMB"] = string.Empty;
               _pendingResourceRow["handles"] = string.Empty;
            }

            switch (memoryLine.metric)
            {
               case MemoryMetric.DateTimeCount:
                  _pendingResourceRow["count"] = memoryLine.value.ToString();
                  break;
               case MemoryMetric.Memory:
                  _pendingResourceRow["memoryMB"] = BytesToMB(memoryLine.value);
                  break;
               case MemoryMetric.VMSize:
                  _pendingResourceRow["vmSizeMB"] = BytesToMB(memoryLine.value);
                  break;
               case MemoryMetric.PrivateSize:
                  _pendingResourceRow["privateSizeMB"] = BytesToMB(memoryLine.value);
                  break;
               case MemoryMetric.HandleCount:
                  _pendingResourceRow["handles"] = memoryLine.value.ToString();
                  break;
            }

            resourceTable.AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLOG_MEMORY Exception : " + e.Message);
         }
      }

      private static string BytesToMB(long bytes)
      {
         double megabytes = bytes / (1024.0 * 1024.0);
         return megabytes.ToString("F1");
      }
   }
}
