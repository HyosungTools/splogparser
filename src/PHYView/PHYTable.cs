using Contract;
using Impl;
using System;
using System.Data;
using LogLineHandler;
using System.Linq;

namespace CDMView
{
   internal class PHYTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public PHYTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = false;
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
                  case LogLineHandler.XFSType.WFS_INF_CDM_CASH_UNIT_INFO:
                  {
                        base.ProcessRow(spLogLine);
                        WFS_INF_CDM_CASH_UNIT_INFO(spLogLine);
                        break;
                     }
                  default:
                     break;

               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("PHYTable.ProcessRow EXCEPTION:" + e.Message);
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
            // P H Y S I C A L  T A B L E S

            string[] columns = new string[] { "error", "status", "count", "reject", "dispensed", "presented", "retracted" };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if ( !dTable.TableName.StartsWith("Phy") && !dTable.TableName.Equals("Messages") )
               {
                  // COMPRESS
                  tableName = dTable.TableName;
                  CompressTable(tableName, columns);
               }
            }

            // ADD ENGLISH 
            string[,] colKeyMap = new string[1, 2]
            {
               {"status", "usPStatus" }
            };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("Phy-"))
               {
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(message: String.Format("Exception processing the {0} table - {1}", tableName, e.Message));
         }

         return base.WriteExcelFile();
      }


      protected void WFS_INF_CDM_CASH_UNIT_INFO(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSCDMCUINFO cashInfo)
            {
               foreach (var unit in cashInfo.listPhysical)
               {
                  try
                  {
                     string tableName = unit.cUnitID; // Use physical cUnitID (e.g., CST_B, CST_D)
                     if (string.IsNullOrEmpty(tableName))
                     {
                        ctx.ConsoleWriteLogLine(String.Format("Skipping PhysicalCU: Missing cUnitID {0}, {1}", spLogLine.LogFile, spLogLine.Timestamp));
                        continue;
                     }
                     DataTable dTable = dTableSet.Tables[tableName];
                     if (dTable == null)
                     {
                        // Find the first Phy-? table
                        DataTable phyTable = null;
                        string oldName = null;
                        for (int k = 1; k <= 9; k++)
                        {
                           string phyName = $"Phy-{k}";
                           if (dTableSet.Tables.Contains(phyName))
                           {
                              phyTable = dTableSet.Tables[phyName];
                              oldName = phyName;
                              break;
                           }
                        }
                        if (phyTable == null)
                        {
                           ctx.ConsoleWriteLogLine("No Phy-? table found to rename for LogicalCUnitID=" + tableName);
                           continue;
                        }

                        // Check for duplicate table name
                        if (dTableSet.Tables.Contains(tableName))
                        {
                           ctx.ConsoleWriteLogLine($"Table {tableName} already exists, using existing table");
                           dTable = dTableSet.Tables[tableName];
                        }
                        else
                        {
                           // Rename by setting TableName directly
                           ctx.ConsoleWriteLogLine($"Renaming {oldName} to {tableName}");
                           phyTable.TableName = tableName;
                           dTable = phyTable;
                        }
                     }

                     dTable.AcceptChanges();

                     // Search for a row with matching file and time
                     DataRow existingRow = dTable.AsEnumerable()
                         .FirstOrDefault(row => row.Field<string>("file") == cashInfo.LogFile &&
                                                row.Field<string>("time") == cashInfo.Timestamp);

                     // Add new row
                     DataRow newRow = dTable.NewRow();
                     newRow["file"] = cashInfo.LogFile;
                     newRow["time"] = cashInfo.Timestamp;
                     newRow["error"] = cashInfo.HResult;
                     newRow["status"] = unit.usPStatus;
                     newRow["count"] = unit.ulCount;
                     newRow["reject"] = unit.ulRejectCount;
                     newRow["dispensed"] = unit.ulDispensedCount;
                     newRow["presented"] = unit.ulPresentedCount;
                     newRow["retracted"] = unit.ulRetractedCount;
                     dTable.Rows.Add(newRow);
                     dTable.AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO PHYTable Exception {0}, {1}, {2}, LogicalCUnitID={3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, unit.cUnitID));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_CDM_CASH_UNIT_INFO End of Function Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_CDM_CASHUNITINFOCHANGED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCDMCUINFO cashInfo)
            {
               for (int i = 0; i < cashInfo.listPhysical.Count; i++)
               {
                  try
                  {
                     string tableName = "Phy-" + (i + 1).ToString();
                     DataTable dTable = dTableSet.Tables[tableName];

                     // Search for a row with matching file and time
                     DataRow existingRow = dTable.AsEnumerable()
                         .FirstOrDefault(row => row.Field<string>("file") == cashInfo.LogFile &&
                                                row.Field<string>("time") == cashInfo.Timestamp);


                     // Add new row
                     DataRow newRow = dTable.NewRow();
                     newRow["file"] = cashInfo.LogFile;
                     newRow["time"] = cashInfo.Timestamp;
                     newRow["error"] = cashInfo.HResult;
                     newRow["status"] = cashInfo.listPhysical[i].usPStatus;
                     newRow["count"] = cashInfo.listPhysical[i].ulCount;
                     newRow["reject"] = cashInfo.listPhysical[i].ulRejectCount;
                     newRow["dispensed"] = cashInfo.listPhysical[i].ulDispensedCount;
                     newRow["presented"] = cashInfo.listPhysical[i].ulPresentedCount;
                     newRow["retracted"] = cashInfo.listPhysical[i].ulRetractedCount;

                     dTable.Rows.Add(newRow);

                     dTable.AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CDM_CASHUNITINFOCHANGED PHYTable Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_CASHUNITINFOCHANGED Exception : " + e.Message);
         }
      }
      protected void WFS_USRE_CDM_CASHUNITTHRESHOLD(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCDMCUINFO cashInfo)
            {
               for (int i = 0; i < cashInfo.listPhysical.Count; i++)
               {
                  try
                  {
                     string tableName = "Phy-" + (i + 1).ToString();
                     DataTable dTable = dTableSet.Tables[tableName];

                     // Search for a row with matching file and time
                     DataRow existingRow = dTable.AsEnumerable()
                         .FirstOrDefault(row => row.Field<string>("file") == cashInfo.LogFile &&
                                                row.Field<string>("time") == cashInfo.Timestamp);

                     // Add new row
                     DataRow newRow = dTable.NewRow();
                     newRow["file"] = cashInfo.LogFile;
                     newRow["time"] = cashInfo.Timestamp;
                     newRow["error"] = cashInfo.HResult;
                     newRow["status"] = cashInfo.listPhysical[i].usPStatus;
                     newRow["count"] = cashInfo.listPhysical[i].ulCount;
                     newRow["reject"] = cashInfo.listPhysical[i].ulRejectCount;
                     newRow["dispensed"] = cashInfo.listPhysical[i].ulDispensedCount;
                     newRow["presented"] = cashInfo.listPhysical[i].ulPresentedCount;
                     newRow["retracted"] = cashInfo.listPhysical[i].ulRetractedCount;

                     dTable.Rows.Add(newRow);

                     dTable.AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_CDM_CASHUNITTHRESHOLD PHYTable Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_CDM_CASHUNITTHRESHOLD Exception : " + e.Message);
         }
      }
   }
}
