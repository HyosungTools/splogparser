using Contract;
using System;
using System.Data;
using LogLineHandler;
using Impl;
using System.Linq;

namespace CDMView_Flat
{
   internal class CDMTable_Flat : BaseTable
   {
      public CDMTable_Flat(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = false;
      }

      public override bool WriteExcelFile()
      {
         string tableName = string.Empty;

         try
         {
            // S U M M A R Y  T A B L E


            // COMPRESS
            DeleteRedundantRows("Summary");
            DeleteRedundantRows("PhySummary");

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         return base.WriteExcelFile();
      }

      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is SPFlatLine spFlatLine)
            {
               switch (spFlatLine.flatType)
               {
                  case SPFlatType.CDM_DenominateInvoked:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMDenominateLine denominateLine)
                        {
                           AddRowConditionally(dTableSet.Tables["Dispense"], denominateLine, "denominate", denominateLine.Amount.ToString());
                        }
                        break;
                     }
                  case SPFlatType.CDM_HandleDenominate:
                     {
                        base.ProcessRow(spFlatLine);
                        // (removed AddInformation(spFlatLine, "denominate") - it added a blank
                        //  duplicate row per denominate, doubling the sheet. The invoke row remains.)
                        break;
                     }


                  case SPFlatType.CDM_DispenseInvoked:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMDispenseLine dispenseLine)
                        {
                           AddDispenseRow(dispenseLine);
                        }
                        break;
                     }
                  case SPFlatType.CDM_HandleDispense:
                     {
                        base.ProcessRow(spFlatLine);
                        AddInformation(spFlatLine, "dispense");
                        break;
                     }

                  case SPFlatType.CDM_PresentInvoked:
                     {
                        base.ProcessRow(spFlatLine);
                        AddInformation(spFlatLine, "present");
                        break;
                     }

                  case SPFlatType.CDM_HandlePresent:
                     {
                        base.ProcessRow(spFlatLine);
                        AddInformation(spFlatLine, "present");
                        break;
                     }

                  case SPFlatType.CDM_HandleItemsTaken:
                     {
                        base.ProcessRow(spFlatLine);
                        AddInformation(spFlatLine, "items taken");
                        break;
                     }


                  // L O G I C A L  U N I T

                  case SPFlatType.CDM_UnitIDs:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("Summary", unitList, "id");
                        }
                        break;
                     }

                  case SPFlatType.CDM_UnitTypes:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("Summary", unitList, "type");
                        }
                        break;
                     }
                  case SPFlatType.CDM_UnitCurrencies:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("Summary", unitList, "currency");
                        }
                        break;
                     }
                  case SPFlatType.CDM_UnitValues:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("Summary", unitList, "denom");
                        }
                        break;
                     }

                  // P H Y S I C A L  U N I T

                  case SPFlatType.CDM_PhysicalUnitNumbers:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("PhySummary", unitList, "number");
                        }
                        break;
                     }

                  case SPFlatType.CDM_PhysicalIDs:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("PhySummary", unitList, "id");
                        }
                        break;
                     }

                  case SPFlatType.CDM_PhysicalPositionNames:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("PhySummary", unitList, "name");
                        }
                        break;
                     }

                  case SPFlatType.CDM_PhysicalInitialCounts:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddSummary("PhySummary", unitList, "initial");
                        }
                        break;
                     }



                  case SPFlatType.CDM_PhysicalStatuses:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddPhysicalStatusRow(unitList, "status");
                        }
                        break;
                     }

                  case SPFlatType.CDM_PhysicalCounts:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddPhysicalStatusRow(unitList, "count");
                        }
                        break;
                     }

                  case SPFlatType.CDM_PhysicalRejectCounts:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMUnitList unitList)
                        {
                           AddPhysicalStatusRow(unitList, "reject");
                        }
                        break;
                     }

                  case SPFlatType.CDM_CashUnitTrace:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMCashUnitTrace trace)
                        {
                           AddSummaryFromTrace(trace);
                        }
                        break;
                     }

                  case SPFlatType.CDM_XFSResult:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CDMXFSResultLine xfs)
                        {
                           AddOperationRow(xfs);
                        }
                        break;
                     }

                  default:
                     break;
               };
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CDMTable_Flat.ProcessRow EXCEPTION: " + e.Message);
         }
      }

      private void AddSummary(string tableName, CDMUnitList line, string column)
      {
         DataRow[] dataRows = dTableSet.Tables[tableName].Select();

         // for each row, set the tracefile, timestamp and hresult
         for (int i = 0; i < line.unitList.Length; i++)
         {
            try
            {
               dataRows[i]["file"] = line.LogFile;
               dataRows[i]["time"] = line.Timestamp;
               dataRows[i]["error"] = line.HResult;
               dataRows[i][column] = line.unitList[i];
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", tableName, line.LogFile, line.Timestamp, e.Message));
            }

         }
         dTableSet.Tables[tableName].AcceptChanges();
      }

      private void AddPhysicalStatusRow(CDMUnitList line, string column)
      {
         for (int i = 0; i < line.unitList.Length; i++)
         {
            try
            {
               string tableName = "Phy-" + (i + 1).ToString();
               DataTable dTable = dTableSet.Tables[tableName];

               // Search for a row with matching file and time
               DataRow existingRow = dTable.AsEnumerable()
                   .FirstOrDefault(row => row.Field<string>("file") == line.LogFile &&
                                          row.Field<string>("time") == line.Timestamp);

               if (existingRow != null)
               {
                  // Update existing row with non-null values
                  existingRow["error"] = line.HResult ?? existingRow["error"];
                  existingRow[column] = line.unitList[i].ToString() ?? existingRow[column];
               }
               else
               {
                  // Add new row
                  DataRow newRow = dTable.NewRow();
                  newRow["file"] = line.LogFile;
                  newRow["time"] = line.Timestamp;
                  newRow["error"] = line.HResult;
                  newRow[column] = line.unitList[i].ToString() ?? string.Empty;
                  dTable.Rows.Add(newRow);
               }

               dTable.AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}",
                   "AddPhysicalStatusRow", line.LogFile, line.Timestamp, e.Message));
            }
         }
      }

      private void AddDispenseRow(CDMDispenseLine line)
      {
         try
         {
            DataRow row = dTableSet.Tables["Dispense"].Rows.Add();
            row["file"] = line.LogFile;
            row["time"] = line.Timestamp;
            row["error"] = line.HResult;
            row["position"] = "dispense";

            // total amount if available
            row["amount"] = line.Amount.ToString() ?? string.Empty;

            // cassette note counts: lay PayloadValues into LU1..LU9
            for (int i = 0; i < line.NoteCounts.Length; i++)
            {
               string columnName = "LU" + (i + 1);
               row[columnName] = line.NoteCounts[i] == 0 ? string.Empty : line.NoteCounts[i].ToString();
            }

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", "AddDispenseRow", line.LogFile, line.Timestamp, e.Message));
         }
      }

      private void AddDenominateRow(CDMDenominateLine line)
      {
         try
         {
            DataRow row = dTableSet.Tables["Dispense"].Rows.Add();
            row["file"] = line.LogFile;
            row["time"] = line.Timestamp;
            row["error"] = line.HResult;
            row["position"] = "denominate";

            // total amount if available
            row["amount"] = line.Amount.ToString() ?? string.Empty;

            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", "AddDenominateRow", line.LogFile, line.Timestamp, e.Message));
         }
      }

      private void AddInformation(SPFlatLine line, string position)
      {
         try
         {
            DataRow row = dTableSet.Tables["Dispense"].Rows.Add();
            row["file"] = line.LogFile;
            row["time"] = line.Timestamp;
            row["error"] = line.HResult;
            row["position"] = position;
            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", "AddInformation", line.LogFile, line.Timestamp, e.Message));
         }
      }

      private void AddRowConditionally(DataTable dTable, CDMDenominateLine line, string position, string amount)
      {
         try
         {
            // Check if the last row has the same position and amount
            if (dTable.Rows.Count > 0)
            {
               DataRow lastRow = dTable.Rows[dTable.Rows.Count - 1];
               string lastPosition = lastRow.Field<string>("position");
               string lastAmount = lastRow.Field<string>("amount");

               if (lastPosition == position && lastAmount == amount)
               {
                  // Skip adding the row if position and amount match
                  return;
               }
            }

            // Add new row
            DataRow row = dTable.NewRow();
            row["file"] = line.LogFile;
            row["time"] = line.Timestamp;
            row["error"] = line.HResult;
            row["position"] = position;
            row["amount"] = amount; // Include amount in the new row
            dTable.Rows.Add(row);
            dTable.AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}",
                "AddRowConditionally", line.LogFile, line.Timestamp, e.Message));
         }
      }

      // DN consolidated CDM cash-unit dump -> the 'Summary' worksheet. One row per logical unit, laid
      // into the seed rows by index. UnitValue is the per-cassette denomination. Column-defensive + logs
      // a one-line diagnostic so Trace.log shows whether it ran and how many rows it wrote.
      private void AddSummaryFromTrace(CDMCashUnitTrace trace)
      {
         try
         {
            DataTable summary = dTableSet.Tables["Summary"];
            if (summary == null)
            {
               ctx.ConsoleWriteLogLine("AddSummaryFromTrace: no 'Summary' table!");
               return;
            }

            DataRow[] rows = summary.Select();
            ctx.ConsoleWriteLogLine(String.Format(
               "AddSummaryFromTrace: trace.Count={0}, seed rows={1}, cols=[{2}]",
               trace.Count, rows.Length,
               String.Join(",", summary.Columns.Cast<DataColumn>().Select(c => c.ColumnName))));

            int written = 0;
            for (int i = 0; i < trace.Count && i < rows.Length; i++)
            {
               Set(rows[i], "file", trace.LogFile);
               Set(rows[i], "time", trace.Timestamp);
               Set(rows[i], "error", trace.HResult);
               Set(rows[i], "number", trace.At("UnitNumber", i));   // CIM-style key column, if present
               Set(rows[i], "id", trace.At("UnitID", i));
               Set(rows[i], "type", trace.At("UnitType", i));
               Set(rows[i], "name", trace.At("UnitType", i));       // XML-seed column, if present
               Set(rows[i], "currency", trace.At("UnitCurrencyID", i));
               Set(rows[i], "denom", trace.At("UnitValue", i));     // <-- the denomination
               Set(rows[i], "initial", trace.At("UnitInitialCount", i));
               Set(rows[i], "comment", "count " + trace.At("UnitCount", i) + ", " + trace.At("UnitStatus", i));
               written++;
            }
            summary.AcceptChanges();
            ctx.ConsoleWriteLogLine("AddSummaryFromTrace: wrote " + written + " summary row(s)");
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("CDMTable_Flat.AddSummaryFromTrace Exception: " + e.Message);
         }
      }

      // Write a column only if it exists on the row's table (schema differs between XSD and seed XML).
      private void Set(DataRow row, string column, object value)
      {
         if (row.Table.Columns.Contains(column))
         {
            row[column] = value;
         }
      }
      // DN dispense-lifecycle event (CCDMService::HandleXFSResult / FireXFSEvent). The command number
      // is named from the Messages lookup (type "dwCommandCode") - team-maintained data, not hardcoded.
      // A row is written when the command is known (has a Messages entry) OR it faulted, so real
      // operations and every fault are visible while un-mapped successful chatter stays out.
      private void AddOperationRow(CDMXFSResultLine xfs)
      {
         try
         {
            // Denominate is already covered by the Ctrl::Denominate invoke row (with the amount).
            if (xfs.CommandCode == 301)
            {
               return;
            }

            (bool found, DataRow msg) = FindMessages("dwCommandCode", xfs.CommandCode.ToString());
            bool failed = !string.IsNullOrEmpty(xfs.HResult);

            if (!found && !failed)
            {
               return;   // un-mapped and succeeded -> internal chatter, skip
            }

            string name = found ? msg["brief"].ToString() : ("command " + xfs.CommandCode);

            DataRow row = dTableSet.Tables["Dispense"].Rows.Add();
            row["file"] = xfs.LogFile;
            row["time"] = xfs.Timestamp;
            row["error"] = xfs.HResult;
            row["position"] = name;
            dTableSet.Tables["Dispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("CDMTable_Flat.AddOperationRow Exception: " + e.Message);
         }
      }

   }
}
