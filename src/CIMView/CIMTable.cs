using System;
using System.Data;
using System.Dynamic;
using Contract;
using Impl;
using LogLineHandler;
using System.Collections.Generic;

namespace CIMView
{
   internal class CIMTable : BaseTable
   {
      private readonly bool have_seen_WFS_INF_CIM_CASH_UNIT_INFO = false;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CIMTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;

      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is SPLine spLogLine)
            {
               switch (spLogLine.xfsType)
               {
                  case LogLineHandler.XFSType.WFS_INF_CIM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CIM_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CIM_CASH_UNIT_INFO:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CIM_CASH_UNIT_INFO(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_CIM_CASH_IN_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CIM_CASH_IN_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN_START:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN_START(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN_END:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN_END(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_CASH_IN_ROLLBACK(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_RETRACT:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_RETRACT(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_RESET:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_RESET(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_STARTEX:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_STARTEX(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_CIM_ENDEX:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_CIM_ENDEX(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_USRE_CIM_CASHUNITTHRESHOLD(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_CIM_CASHUNITINFOCHANGED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_CIM_ITEMSTAKEN(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_CIM_INPUTREFUSE(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_CIM_NOTEERROR:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_CIM_NOTEERROR(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CIM_ITEMSPRESENTED:
                     {
                        base.ProcessRow(spLogLine);
                        CIM_UPDATE_POSITION(spLogLine, "presented", "WFS_SRVE_CIM_ITEMSPRESENTED");
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_CIM_ITEMSINSERTED:
                     {
                        base.ProcessRow(spLogLine);
                        CIM_UPDATE_POSITION(spLogLine, "inserted", "WFS_SRVE_CIM_ITEMSINSERTED");
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_IPM_MEDIADETECTED:
                     {
                        base.ProcessRow(spLogLine);
                        CIM_UPDATE_POSITION(spLogLine, "detected", "WFS_SRVE_IPM_MEDIADETECTED");
                        break;
                     }
                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CIMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         string tableName = string.Empty;

         try
         {
            // S T A T U S   T A B L E

            tableName = "Status";

            // COMPRESS
            string[] columns = new string[] { "error", "status", "safedoor", "acceptor", "intstack", "stackitems" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            string[,] colKeyMap = new string[5, 2]
            {
               {"status", "fwDevice" },
               {"safedoor", "fwSafeDoor;"},
               {"acceptor", "fwAcceptor" },
               {"intstack", "fwIntermediateStacker" },
               {"stackitems", "fwStackerItems" }
            };
            AddEnglishToTable(tableName, colKeyMap);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         try
         {
            // S U M M A R Y   T A B L E 

            tableName = "Summary";

            // COMPRESS
            DeleteRedundantRows(tableName);

            // ADD ENGLISH
            string[,] colKeyMap = new string[1, 2]
            {
               {"type", "fwType" }
            };
            AddEnglishToTable(tableName, colKeyMap);

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         try
         {
            // C A S H  I N   T A B L E

            ctx.ConsoleWriteLogLine("Compress the CashIn Tables: sort by time, visit every row and delete rows that are unchanged from their predecessor");

            // the list of columns to compare
            string[] cashUnitCols = new string[] { "error", "status", "cashin", "count" };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashIn-"))
               {
                  // COMPRESS
                  tableName = dTable.TableName;
                  CompressTable(tableName, cashUnitCols);
               }
            }

            // ADD ENGLISH
            string[,] colKeyMap = new string[1, 2]
            {
               {"status", "usStatus" }
            };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashIn-"))
               {
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         try
         {
            // D E P O S I T   T A B L E

            tableName = "Deposit";

            //// COMPRESS
            string[] columns = new string[] { "error", "errcode", "position", "status", "refused", "amount", "N0","N1","N2","N3","N4","N5","N6","N7","N8","N9","N10","N11","N12","N13","N14","N15","N16","N17","N18","N19","N20" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            string[,] colKeyMap = new string[3, 2]
            {
               {"errcode", "errcode" },
               {"position", "position" },
               {"status", "wStatus" }
            };


            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.Equals("Deposit"))
               {
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }


         // A D D   M O N E Y   T O   T A B L E S

         try
         {
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.StartsWith("CashIn-") || dTable.TableName.Equals("Deposit"))
               {
                  ctx.ConsoleWriteLogLine("Adding money to table :" + dTable.TableName);
                  _datatable_ops.AddMoneyToTable(ctx, dTable, dTableSet.Tables["Messages"]);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception ADD MONEY TO TABLES: " + e.Message);

         }


         // A D D   A M O U N T   T O   D E P O S I T   T A B L E

         try
         {
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.Equals("Deposit"))
               {
                  ctx.ConsoleWriteLogLine("Adding money to table :" + dTable.TableName);
                  _datatable_ops.AddAmountToTable(ctx, dTable);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception ADD MONEY TO TABLES: " + e.Message);

         }

         // E N H A N C E   C A S H I N   T A B L E S
         //
         // Add exchange summary rows and balance-sheet rows below "missing" events.
         // Must run AFTER AddMoneyToTable (USD columns populated)
         // and BEFORE rename (CashIn-* names still intact).

         try
         {
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.StartsWith("CashIn-"))
               {
                  ctx.ConsoleWriteLogLine("Enhancing CashIn table: " + dTable.TableName);
                  EnhanceCashInTable(dTable);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception ENHANCE CASHIN TABLES: " + e.Message);
         }


         // RENAME CASHIN TABLES - DO THIS LAST

         try
         {
            // rename the cash units
            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Rename the Cash Units : Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("CashIn-"))
               {
                  string cashUnitNumber = dTable.TableName.Replace("CashIn-", string.Empty);
                  ctx.ConsoleWriteLogLine("cashUnitNumber :" + cashUnitNumber);

                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = {0}", cashUnitNumber));
                  if (dataRows.Length == 1)
                  {
                     if (dataRows[0]["denom"].ToString().Trim() == "0")
                     {
                        ctx.ConsoleWriteLogLine("denom == 0");
                        // if currency is "" use the type (e.g. RETRACT, REJECT
                        ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["type"].ToString()));
                        dTable.TableName = dataRows[0]["type"].ToString();
                        dTable.AcceptChanges();
                     }
                     else
                     {
                        // otherwise combine the currency and denomination
                        ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString()));
                        dTable.TableName = dataRows[0]["currency"].ToString() + dataRows[0]["denom"].ToString();
                        dTable.AcceptChanges();
                     }
                  }
               }
            }

            // TODO - delete any CashUnit column named LUx
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception : " + e.Message);
         }

         return base.WriteExcelFile();
      }

      /// <summary>
      /// Denomination values for USD columns. Column name -> dollar value per note.
      /// </summary>
      private static readonly (string column, int value)[] USDDenominations = new (string, int)[]
      {
         ("USD1",   1),
         ("USD2",   2),
         ("USD5",   5),
         ("USD10",  10),
         ("USD20",  20),
         ("USD50",  50),
         ("USD100", 100)
      };

      /// <summary>
      /// Safely parse a DataRow column value as an integer. Returns 0 for null, empty, or non-numeric.
      /// </summary>
      private static int SafeGetInt(DataRow row, string column)
      {
         try
         {
            object val = row[column];
            if (val == null || val == DBNull.Value)
               return 0;

            string s = val.ToString().Trim();
            if (string.IsNullOrEmpty(s))
               return 0;

            if (int.TryParse(s, out int result))
               return result;

            return 0;
         }
         catch
         {
            return 0;
         }
      }

      /// <summary>
      /// Calculate total dollar value from USD note-count columns in a DataRow.
      /// </summary>
      private static int CalcDollarValue(DataRow row)
      {
         int total = 0;
         foreach (var (column, value) in USDDenominations)
         {
            total += SafeGetInt(row, column) * value;
         }
         return total;
      }

      /// <summary>
      /// Test whether a row's status indicates "missing" (after AddEnglish translation).
      /// Checks for both the English text and the raw XFS code.
      /// </summary>
      private static bool IsMissing(DataRow row)
      {
         string status = row["status"]?.ToString()?.Trim() ?? "";
         return status.Equals("missing", StringComparison.OrdinalIgnoreCase) ||
                status == "4";   // WFS_CIM_STATCUMISSING
      }

      /// <summary>
      /// Enhance a CashIn-* table with exchange summary rows, balance-sheet
      /// rows below each "missing" event, and computed delta rows where the
      /// three-row pattern is absent.
      /// </summary>
      private void EnhanceCashInTable(DataTable table)
      {
         if (table.Rows.Count < 2)
            return;

         int rowCount = table.Rows.Count;

         // -----------------------------------------------------------------
         // Pass 1: Classify every row
         // -----------------------------------------------------------------

         string[] classification = new string[rowCount];

         for (int i = 0; i < rowCount; i++)
         {
            DataRow row = table.Rows[i];
            int cashin = SafeGetInt(row, "cashin");
            int prevCashin = i > 0 ? SafeGetInt(table.Rows[i - 1], "cashin") : 0;
            int nextCashin = i < rowCount - 1 ? SafeGetInt(table.Rows[i + 1], "cashin") : 0;

            if (IsMissing(row))
            {
               classification[i] = "missing";
            }
            else if (i > 0 && i < rowCount - 1 &&
                     cashin < prevCashin && nextCashin >= prevCashin)
            {
               // Delta: small value sandwiched between two higher values
               classification[i] = "delta";
            }
            else if (i > 0 && cashin <= 1 && prevCashin > 5)
            {
               // Exchange reset: cashin drops to 0 or 1 from a significant value
               classification[i] = "exchange";
            }
            else
            {
               classification[i] = "cumulative";
            }
         }

         // -----------------------------------------------------------------
         // Pass 2: Collect insertion points
         // -----------------------------------------------------------------

         var insertions = new List<(int afterIndex, List<Dictionary<string, string>> newRows)>();

         DataRow peakRow = null;
         int peakCashin = 0;
         int exchangeNumber = 0;
         DataRow lastPeakRef = null;
         DataRow prevCumulative = null;

         for (int i = 0; i < rowCount; i++)
         {
            DataRow row = table.Rows[i];
            int cashin = SafeGetInt(row, "cashin");

            // Track peak cumulative for current exchange period
            if (classification[i] == "cumulative" && cashin > peakCashin)
            {
               peakRow = row;
               peakCashin = cashin;
            }

            // ----- Missing delta detection -----
            // If this is a cumulative and the previous cumulative exists with
            // no delta row between them, insert a computed delta.
            if (classification[i] == "cumulative" && prevCumulative != null)
            {
               int prevCI = SafeGetInt(prevCumulative, "cashin");

               if (cashin > prevCI)
               {
                  // Check if any row between prevCumulative and this one is a delta
                  bool hasDeltaBetween = false;
                  for (int k = i - 1; k >= 0; k--)
                  {
                     if (table.Rows[k] == prevCumulative)
                        break;
                     if (classification[k] == "delta")
                     {
                        hasDeltaBetween = true;
                        break;
                     }
                  }

                  if (!hasDeltaBetween)
                  {
                     var deltaValues = BuildComputedDeltaValues(prevCumulative, row);
                     if (deltaValues != null)
                     {
                        // Insert before this cumulative row
                        insertions.Add((i - 1, new List<Dictionary<string, string>> { deltaValues }));
                     }
                  }
               }
            }

            // ----- Exchange boundary -----
            if (classification[i] == "exchange")
            {
               if (peakRow != null && peakCashin > 0)
               {
                  exchangeNumber++;
                  var summary = BuildExchangeSummaryValues(peakRow, exchangeNumber);
                  insertions.Add((i - 1, new List<Dictionary<string, string>> { summary }));
                  lastPeakRef = peakRow;
               }
               peakRow = null;
               peakCashin = 0;
               prevCumulative = null;
            }

            // ----- Missing event: insert balance-sheet rows after -----
            if (classification[i] == "missing")
            {
               bool isRogue = cashin > 0;
               DataRow refRow = isRogue ? row : lastPeakRef;

               if (refRow != null)
               {
                  var balanceRows = BuildBalanceSheetValues(row, refRow, isRogue);
                  insertions.Add((i, balanceRows));
               }

               // Flag rogue in the comment column of the missing row itself
               if (isRogue)
               {
                  string existing = row["comment"]?.ToString() ?? "";
                  row["comment"] = existing + " WARNING: Counts not reset";
               }
            }

            // Track previous cumulative
            if (classification[i] == "cumulative")
            {
               prevCumulative = row;
            }
            else if (classification[i] == "exchange")
            {
               prevCumulative = null;
            }
         }

         // Final exchange summary
         if (peakRow != null && peakCashin > 0)
         {
            exchangeNumber++;
            var summary = BuildExchangeSummaryValues(peakRow, exchangeNumber);
            insertions.Add((rowCount - 1, new List<Dictionary<string, string>> { summary }));
         }

         // -----------------------------------------------------------------
         // Pass 3: Insert rows in reverse order (preserves indices)
         // -----------------------------------------------------------------

         insertions.Sort((a, b) => b.afterIndex.CompareTo(a.afterIndex));

         foreach (var (afterIndex, newRows) in insertions)
         {
            for (int j = 0; j < newRows.Count; j++)
            {
               DataRow newRow = table.NewRow();
               foreach (var kvp in newRows[j])
               {
                  if (table.Columns.Contains(kvp.Key))
                  {
                     newRow[kvp.Key] = kvp.Value;
                  }
               }
               table.Rows.InsertAt(newRow, afterIndex + 1 + j);
            }
         }

         table.AcceptChanges();

         ctx.ConsoleWriteLogLine(String.Format("EnhanceCashInTable '{0}': {1} exchanges, {2} insertions",
            table.TableName, exchangeNumber, insertions.Count));
      }

      /// <summary>
      /// Build column values for an Exchange Summary row.
      /// </summary>
      private Dictionary<string, string> BuildExchangeSummaryValues(DataRow peakRow, int exchangeNumber)
      {
         var values = new Dictionary<string, string>();

         values["file"] = String.Format("Exchange {0}", exchangeNumber);
         values["time"] = peakRow["time"]?.ToString() ?? "";
         values["cashin"] = peakRow["cashin"]?.ToString() ?? "";

         foreach (var (column, _) in USDDenominations)
         {
            int count = SafeGetInt(peakRow, column);
            if (count > 0)
            {
               values[column] = count.ToString();
            }
         }

         int totalDollars = CalcDollarValue(peakRow);
         values["comment"] = String.Format("${0:N0}", totalDollars);

         return values;
      }

      /// <summary>
      /// Build two balance-sheet rows for insertion below a "missing" event.
      ///
      /// Row 1 (SUBTOTAL): each USD column = note_count x denomination_value
      ///   error = "SUBTOTAL" (marker for Excel formatting: $ format, line top/bottom)
      ///
      /// Row 2 (TOTAL): dollar sum in USD100 column position
      ///   error = "TOTAL" (marker for Excel formatting: $ format, double line below)
      ///
      /// Both rows carry the missing row's file and time for sort ordering.
      /// </summary>
      private List<Dictionary<string, string>> BuildBalanceSheetValues(DataRow missingRow, DataRow refRow, bool isRogue)
      {
         var subtotalRow = new Dictionary<string, string>();
         var totalRow = new Dictionary<string, string>();

         // Anchor to the missing row's timestamp for sort ordering
         string file = missingRow["file"]?.ToString() ?? "";
         string time = missingRow["time"]?.ToString() ?? "";

         subtotalRow["file"] = file;
         subtotalRow["time"] = time;
         subtotalRow["error"] = "SUBTOTAL";

         totalRow["file"] = file;
         totalRow["time"] = time;
         totalRow["error"] = "TOTAL";

         int totalDollars = 0;

         foreach (var (column, denomValue) in USDDenominations)
         {
            int noteCount = SafeGetInt(refRow, column);
            if (noteCount > 0)
            {
               int dollars = noteCount * denomValue;
               subtotalRow[column] = dollars.ToString();
               totalDollars += dollars;
            }
         }

         totalRow["USD100"] = String.Format("${0:N0}", totalDollars);

         subtotalRow["comment"] = isRogue ? "WARNING: Counts not reset" : "$ per denomination";
         totalRow["comment"] = String.Format("Total ${0:N0}", totalDollars);

         return new List<Dictionary<string, string>> { subtotalRow, totalRow };
      }

      /// <summary>
      /// All USD columns including USD0 (unrecognized notes). Used for delta
      /// computation where we need to track every note, not just denominated ones.
      /// </summary>
      private static readonly string[] AllUSDColumns = new string[]
      {
         "USD0", "USD1", "USD2", "USD5", "USD10", "USD20", "USD50", "USD100"
      };

      /// <summary>
      /// Build a computed delta row for insertion between two consecutive
      /// cumulative rows that have no natural delta row between them.
      ///
      /// Shows the difference per denomination (cashin, count, all USD columns
      /// including USD0 for unrecognized notes).
      /// Uses the target (later) row's file and time for sort ordering.
      /// Returns null if the delta is zero across all columns.
      /// </summary>
      private Dictionary<string, string> BuildComputedDeltaValues(DataRow prevRow, DataRow currRow)
      {
         var values = new Dictionary<string, string>();
         bool hasDelta = false;

         // Anchor to the current row's timestamp
         values["file"] = currRow["file"]?.ToString() ?? "";
         values["time"] = currRow["time"]?.ToString() ?? "";
         values["status"] = currRow["status"]?.ToString() ?? "";

         // Cashin delta
         int deltaCashin = SafeGetInt(currRow, "cashin") - SafeGetInt(prevRow, "cashin");
         if (deltaCashin > 0)
         {
            values["cashin"] = deltaCashin.ToString();
            hasDelta = true;
         }

         // Count delta
         int deltaCount = SafeGetInt(currRow, "count") - SafeGetInt(prevRow, "count");
         if (deltaCount > 0)
         {
            values["count"] = deltaCount.ToString();
         }

         // All denomination deltas including USD0
         foreach (string column in AllUSDColumns)
         {
            int delta = SafeGetInt(currRow, column) - SafeGetInt(prevRow, column);
            if (delta > 0)
            {
               values[column] = delta.ToString();
               hasDelta = true;
            }
         }

         return hasDelta ? values : null;
      }

      protected void WFS_INF_CIM_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMSTATUS cimStatus)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               dataRow["status"] = cimStatus.fwDevice;
               dataRow["safedoor"] = cimStatus.fwSafeDoor;
               dataRow["acceptor"] = cimStatus.fwAcceptor;
               dataRow["intstack"] = cimStatus.fwIntStacker;
               dataRow["stackitems"] = cimStatus.fwStackerItems;

               dTableSet.Tables["Status"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CIM_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_INF_CIM_CASH_UNIT_INFO(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               if (!have_seen_WFS_INF_CIM_CASH_UNIT_INFO)
               {
                  try
                  {
                     // First time seeing CASH_UNIT_INFO, populate the Summary Table
                     // have_seen_WFS_INF_CIM_CASH_UNIT_INFO = true;

                     DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

                     // for each row, set the tracefile, timestamp and hresult
                     for (int i = 0; i < cashInfo.lUnitCount; i++)
                     {
                        try
                        {
                           // Now use the usNumbers to create and populate a row in the CashUnit-x table
                           int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                           if (usNumber < 1)
                           {
                              // We have to check because some log lines are truncated (i.e. "more data")
                              // and produce bad results
                              ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                              continue;
                           }

                           dataRows[usNumber]["file"] = spLogLine.LogFile;
                           dataRows[usNumber]["time"] = spLogLine.Timestamp;
                           dataRows[usNumber]["error"] = spLogLine.HResult;

                           dataRows[usNumber]["type"] = cashInfo.fwTypes[i];
                           dataRows[usNumber]["name"] = cashInfo.cUnitIDs[i];
                           dataRows[usNumber]["currency"] = cashInfo.cCurrencyIDs[i];
                           dataRows[usNumber]["denom"] = cashInfo.ulValues[i];
                           dataRows[usNumber]["max"] = cashInfo.ulMaximums[i];
                        }
                        catch (Exception e)
                        {
                           ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO Summary Table Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                        }
                     }

                     dTableSet.Tables["Summary"].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_UNIT_INFO First Time Assignment Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                  }
               }

               for (int i = 0; i < cashInfo.lUnitCount; i++)
               {
                  try
                  {
                     // Now use the usNumbers to create and populate a row in the CashUnit-x table
                     int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                     if (usNumber < 1)
                     {
                        // We have to check because some log lines are truncated (i.e. "more data")
                        // and produce bad results
                        ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                        continue;
                     }

                     string tableName = "CashIn-" + usNumber.ToString();
                     DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                     cashUnitRow["file"] = spLogLine.LogFile;
                     cashUnitRow["time"] = spLogLine.Timestamp;
                     cashUnitRow["error"] = spLogLine.HResult;

                     cashUnitRow["status"] = cashInfo.usStatuses[i];
                     cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                     cashUnitRow["count"] = cashInfo.ulCounts[i];

                     try
                     {
                        if (cashInfo.noteNumbers != null)
                        {
                           for (int j = 0; j < 20; j++)
                           {
                              if (!string.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                              {
                                 string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                                 cashUnitRow["N" + noteNum[0]] = noteNum[1];
                              }
                           }
                        }
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO CashIn Setting Notes Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                     }

                     dTableSet.Tables[tableName].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO CashIn Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
         }
      }

      protected void WFS_INF_CIM_CASH_IN_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINSTATUS cashIn)
            {

               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["position"] = cashIn.wStatus == "0" ? "deposited" : "";
               dataRow["status"] = cashIn.wStatus;
               dataRow["refused"] = cashIn.usNumOfRefused;

               try
               {
                  if (cashIn.noteNumbers != null)
                  {
                     for (int j = 0; j < 20; j++)
                     {
                        if (!string.IsNullOrEmpty(cashIn.noteNumbers[0, j]) && cashIn.noteNumbers[0, j].Contains(":"))
                        {
                           string[] noteNum = cashIn.noteNumbers[0, j].Split(':');
                           dataRow["N" + noteNum[0]] = noteNum[1];
                        }
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CIM_CASH_IN_STATUS CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CIM_CASH_IN_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_START(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "start";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_START Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN(SPLine spLogLine)
      {
         try
         {
            // access the values
            (bool success, string xfsMatch, string subLogLine) result = LogLineHandler.WFSCIMCASHINSTATUS.usNumOfRefusedFromList(spLogLine.logLine);
            string[,] noteNumberList = LogLineHandler.WFSCIMNOTENUMBERLIST.NoteNumberListFromList(spLogLine.logLine);

            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            try
            {
               if (noteNumberList != null)
               {
                  for (int j = 0; j < 20; j++)
                  {
                     if (!string.IsNullOrEmpty(noteNumberList[0, j]) && noteNumberList[0, j].Contains(":"))
                     {
                        string[] noteNum = noteNumberList[0, j].Split(':');
                        dataRow["N" + noteNum[0]] = noteNum[1];
                     }
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
            }

            // position
            dataRow["position"] = "cash in";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_END(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSCIMCASHINFO cashInfo)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["position"] = "end";
               dataRow["refused"] = "";

               try
               {
                  if (spLogLine.HResult == "0" && cashInfo.lUnitCount > 0)
                  {
                     if (cashInfo.noteNumbers != null)
                     {
                        for (int j = 0; j < 20; j++)
                        {
                           if (!string.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                           {
                              string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                              dataRow["N" + noteNum[0]] = noteNum[1];
                           }
                        }
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }

               dTableSet.Tables["Deposit"].AcceptChanges();

               if (spLogLine.HResult == "0" && cashInfo.lUnitCount > 0)
               {
                  for (int i = 0; i < cashInfo.lUnitCount; i++)
                  {
                     try
                     {
                        // Now use the usNumbers to create and populate a row in the CashUnit-x table
                        int usNumber = int.Parse(cashInfo.usNumbers[i].Trim());
                        if (usNumber < 1)
                        {
                           // We have to check because some log lines are truncated (i.e. "more data")
                           // and produce bad results
                           ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                           continue;
                        }

                        string tableName = "CashIn-" + usNumber.ToString();
                        DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                        cashUnitRow["file"] = spLogLine.LogFile;
                        cashUnitRow["time"] = spLogLine.Timestamp;
                        cashUnitRow["error"] = spLogLine.HResult;

                        cashUnitRow["status"] = cashInfo.usStatuses[i];
                        cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                        cashUnitRow["count"] = cashInfo.ulCounts[i];

                        try
                        {
                           if (cashInfo.noteNumbers != null)
                           {
                              for (int j = 0; j < 20; j++)
                              {
                                 if (!string.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                                 {
                                    string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                                    cashUnitRow["N" + noteNum[0]] = noteNum[1];
                                 }
                              }
                           }
                        }
                        catch (Exception e)
                        {
                           ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Setting Notes Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                        }

                        dTableSet.Tables[tableName].AcceptChanges();
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_CASH_IN_END CashIn Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                     }
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_CASH_IN_ROLLBACK(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "rollback";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_ROLLBACK Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_RETRACT(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["position"] = "retract";
               dataRow["refused"] = "";

               try
               {
                  if (cashInfo.noteNumbers != null)
                  {
                     for (int j = 0; j < 20; j++)
                     {
                        if (!string.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                        {
                           string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                           dataRow["N" + noteNum[0]] = noteNum[1];
                        }
                     }
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }

               dTableSet.Tables["Deposit"].AcceptChanges();

               for (int i = 0; i < cashInfo.lUnitCount; i++)
               {
                  try
                  {
                     // Now use the usNumbers to create and populate a row in the CashUnit-x table
                     if (int.Parse(cashInfo.usNumbers[i].Trim()) < 1)
                     {
                        // We have to check because some log lines are truncated (i.e. "more data")
                        // and produce bad results
                        ctx.ConsoleWriteLogLine("usNumbers[i] < 1, continue");
                        continue;
                     }

                     if (cashInfo.ulCounts[i] == "0" && cashInfo.ulCashInCounts[i] == "0" && cashInfo.usStatuses[i] == "0")
                     {
                        // again truncated log lines result in garbage output
                        ctx.ConsoleWriteLogLine("usNumbers[i] == 0, continue");
                        continue;
                     }

                     string tableName = "CashIn-" + cashInfo.usNumbers[i].Trim();
                     DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                     cashUnitRow["file"] = spLogLine.LogFile;
                     cashUnitRow["time"] = spLogLine.Timestamp;
                     cashUnitRow["error"] = spLogLine.HResult;

                     cashUnitRow["status"] = cashInfo.usStatuses[i];
                     cashUnitRow["cashin"] = cashInfo.ulCashInCounts[i];
                     cashUnitRow["count"] = cashInfo.ulCounts[i];

                     try
                     {
                        if (cashInfo.noteNumbers != null)
                        {
                           for (int j = 0; j < 20; j++)
                           {
                              if (!string.IsNullOrEmpty(cashInfo.noteNumbers[i, j]) && cashInfo.noteNumbers[i, j].Contains(":"))
                              {
                                 string[] noteNum = cashInfo.noteNumbers[i, j].Split(':');
                                 cashUnitRow["N" + noteNum[0]] = noteNum[1];
                              }
                           }
                        }
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Setting Notes Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                     }

                     dTableSet.Tables["CashIn-" + cashInfo.usNumbers[i]].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_CIM_RETRACT CashIn Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_CASH_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_RESET(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "reset";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_RESET Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_CIM_STARTEX(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "startex";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_STARTEX Exception : " + e.Message);
         }
      }
      protected void WFS_CMD_CIM_ENDEX(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "endex";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_CIM_ENDEX Exception : " + e.Message);
         }
      }
      protected void WFS_USRE_CIM_CASHUNITTHRESHOLD(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
               int i = int.Parse(cashInfo.usNumbers[0]);

               dataRows[i]["file"] = spLogLine.LogFile;
               dataRows[i]["time"] = spLogLine.Timestamp;
               dataRows[i]["error"] = spLogLine.HResult;

               dataRows[i]["type"] = cashInfo.fwTypes[0];
               dataRows[i]["name"] = cashInfo.cUnitIDs[0];
               dataRows[i]["currency"] = cashInfo.cCurrencyIDs[0];
               dataRows[i]["denom"] = cashInfo.ulValues[0];
               dataRows[i]["max"] = cashInfo.ulMaximums[0];

            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_CIM_CASHUNITTHRESHOLD Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CIM_CASHUNITINFOCHANGED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               try
               {
                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
                  int i = int.Parse(cashInfo.usNumbers[0]);

                  dataRows[i]["file"] = spLogLine.LogFile;
                  dataRows[i]["time"] = spLogLine.Timestamp;
                  dataRows[i]["error"] = spLogLine.HResult;

                  dataRows[i]["type"] = cashInfo.fwTypes[0];
                  dataRows[i]["name"] = cashInfo.cUnitIDs[0];
                  dataRows[i]["currency"] = cashInfo.cCurrencyIDs[0];
                  dataRows[i]["denom"] = cashInfo.ulValues[0];
                  dataRows[i]["max"] = cashInfo.ulMaximums[0];

                  dTableSet.Tables["Summary"].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED Updating Summary Table Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }

               try
               {
                  string tableName = "CashIn-" + cashInfo.usNumbers[0];
                  DataRow cashUnitRow = dTableSet.Tables[tableName].Rows.Add();

                  cashUnitRow["file"] = spLogLine.LogFile;
                  cashUnitRow["time"] = spLogLine.Timestamp;
                  cashUnitRow["error"] = spLogLine.HResult;

                  cashUnitRow["status"] = cashInfo.usStatuses[0];
                  cashUnitRow["cashin"] = cashInfo.ulCashInCounts[0];
                  cashUnitRow["count"] = cashInfo.ulCounts[0];

                  try
                  {
                     if (cashInfo.noteNumbers != null)
                     {
                        for (int j = 0; j < 20; j++)
                        {
                           if (!string.IsNullOrEmpty(cashInfo.noteNumbers[0, j]) && cashInfo.noteNumbers[0, j].Contains(":"))
                           {
                              string[] noteNum = cashInfo.noteNumbers[0, j].Split(':');
                              cashUnitRow["N" + noteNum[0]] = noteNum[1];
                           }
                        }
                     }
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED CashIn Setting Notes Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                  }


                  dTableSet.Tables["CashIn-" + cashInfo.usNumbers[0]].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED Updating Summary Table Exception {0}, {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CIM_CASHUNITINFOCHANGED Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CIM_ITEMSTAKEN(SPLine spLogLine)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = "items taken";
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CIM_ITEMSTAKEN Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_CIM_INPUTREFUSE(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMINPUTREFUSE cimRefused)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // error code
               dataRow["errcode"] = cimRefused.errorCode;

               // position
               dataRow["position"] = String.Format("input refused-{0}", cimRefused.usReason);
               dataRow["refused"] = "";

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_CIM_INPUTREFUSE Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_CIM_NOTEERROR(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMNOTEERROR cimRefused)
            {
               // add new row
               DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               // position
               dataRow["position"] = String.Format("note error-{0}", cimRefused.usReason);
               dataRow["refused"] = "";

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_CIM_INPUTREFUSE Exception : " + e.Message);
         }
      }
      protected void CIM_UPDATE_POSITION(SPLine spLogLine, string positionValue, string xfsString)
      {
         try
         {
            // add new row
            DataRow dataRow = dTableSet.Tables["Deposit"].Rows.Add();

            dataRow["file"] = spLogLine.LogFile;
            dataRow["time"] = spLogLine.Timestamp;
            dataRow["error"] = spLogLine.HResult;

            // position
            dataRow["position"] = positionValue;
            dataRow["refused"] = "";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(xfsString + " Exception : " + e.Message);
         }
      }
   }
}
