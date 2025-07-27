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
                        AddInformation(spFlatLine, "denominate");
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
   }
}
