using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Contract;
using Impl;
using LogLineHandler;

namespace CDMView
{
   internal class PHYTable : BaseTable
   {
      private readonly Dictionary<string, string> _unitIdToTableName = new Dictionary<string, string>();

      private class SerialScratchEntry
      {
         public string LogFile { get; set; }
         public string Timestamp { get; set; }
         public string HResult { get; set; }
         public string CUnitID { get; set; }
         public string UsPStatus { get; set; }
         public string UlCount { get; set; }
         public string UlRejectCount { get; set; }
         public string UlDispensedCount { get; set; }
         public string UlPresentedCount { get; set; }
         public string UlRetractedCount { get; set; }
         public string Serial { get; set; }
      }

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
                  // CASH UNIT INFO CHANGED is delivering a lot of zeros when there should be data, so skipping for now until we can understand why
                  //case LogLineHandler.XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED:
                  //   {
                  //      base.ProcessRow(spLogLine);
                  //      WFS_SRVE_CDM_CASHUNITINFOCHANGED(spLogLine);
                  //      break;
                  //   }
                  case LogLineHandler.XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_CIM_CASHUNITINFOCHANGED(spLogLine);
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
            // S U M M A R Y
            DeleteRedundantRows("Summary");
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception processing Summary table - {0}", e.Message));
         }

         try
         {
            // M E R G E  C I M  S E R I A L  S C R A T C H P A D
            ctx.ConsoleWriteLogLine(String.Format("CIM serial scratchpad: {0} entries", dTableSet.Tables["Scratch"].Rows.Count));

            // get the unique cUnitIDs in the scratch table
            var cUnitIDs = dTableSet.Tables["Scratch"].AsEnumerable()
               .Select(r => r["cunitid"].ToString().Trim())
               .Distinct();

            foreach (string cUnitID in cUnitIDs)
            {
               // find the Phy-x table for this cUnitID via Summary
               DataRow[] summaryRows = dTableSet.Tables["Summary"].Select();
               string phyTableName = null;
               for (int i = 0; i < summaryRows.Length; i++)
               {
                  if (summaryRows[i]["unitid"].ToString().Trim() == cUnitID)
                  {
                     phyTableName = "Phy-" + (i + 1).ToString();
                     break;
                  }
               }

               if (phyTableName == null)
               {
                  ctx.ConsoleWriteLogLine(String.Format("CIM serial scratchpad: no Summary row for unitid={0}", cUnitID));
                  continue;
               }

               DataTable dTable = dTableSet.Tables[phyTableName];
               if (dTable == null)
               {
                  ctx.ConsoleWriteLogLine(String.Format("CIM serial scratchpad: table not found {0}", phyTableName));
                  continue;
               }

               // get scratch rows for this cUnitID sorted by time
               DataRow[] scratchRows = dTableSet.Tables["Scratch"].Select(
                  String.Format("cunitid = '{0}'", cUnitID), "time ASC");

               // get Phy-x rows sorted by time
               DataView phyView = new DataView(dTable) { Sort = "time ASC" };

               // for each scratch row, stamp serial onto Phy-x rows from that time forward
               for (int s = 0; s < scratchRows.Length; s++)
               {
                  string serial = scratchRows[s]["serial"].ToString().Trim();
                  string scratchTime = scratchRows[s]["time"].ToString().Trim();

                  for (int p = 0; p < phyView.Count; p++)
                  {
                     string phyTime = phyView[p]["time"].ToString().Trim();
                     if (string.Compare(phyTime, scratchTime) < 0) continue;
                     phyView[p]["serial"] = serial;
                  }
               }
               dTable.AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception merging CIM serial scratchpad - {0}", e.Message));
         }
         dTableSet.Tables["Scratch"].Clear();
         dTableSet.Tables.Remove("Scratch");

         try
         {
            // C O M P R E S S  P H Y - x  T A B L E S
            string[] columns = new string[] { "error", "status", "count", "reject", "dispensed", "presented", "retracted", "serial" };
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.StartsWith("Phy-"))
               {
                  tableName = dTable.TableName;
                  CompressTable(tableName, columns);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception compressing {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // A D D  E N G L I S H
            string[,] colKeyMap = new string[1, 2]
            {
               { "status", "usPStatus" }
            };
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.StartsWith("Phy-"))
               {
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception adding English to {0} table - {1}", tableName, e.Message));
         }

         try
         {
            // R E N A M E  P H Y S I C A L  C A S S E T T E S
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.StartsWith("Phy-"))
               {
                  string phyNumber = dTable.TableName.Replace("Phy-", string.Empty);
                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
                  int idx = int.Parse(phyNumber) - 1;
                  if (idx < dataRows.Length && !string.IsNullOrWhiteSpace(dataRows[idx]["unitid"].ToString()))
                  {
                     string newName = dataRows[idx]["unitid"].ToString().Trim();
                     ctx.ConsoleWriteLogLine(String.Format("Renaming {0} to {1}", dTable.TableName, newName));
                     dTable.TableName = newName;
                     dTable.AcceptChanges();
                  }
                  else
                  {
                     ctx.ConsoleWriteLogLine(String.Format("No Summary row found for Phy-{0}, leaving unchanged", phyNumber));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception renaming physical cassette tables - {0}", e.Message));
         }

         return base.WriteExcelFile();
      }


      protected void WFS_INF_CDM_CASH_UNIT_INFO(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSCDMCUINFO cashInfo)
            {
               // S U M M A R Y
               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

               for (int i = 0; i < cashInfo.listPhysical.Count; i++)
               {
                  var unit = cashInfo.listPhysical[i];
                  int phyNumber = i + 1;

                  try
                  {
                     dataRows[phyNumber]["file"] = spLogLine.LogFile;
                     dataRows[phyNumber]["time"] = spLogLine.Timestamp;
                     dataRows[phyNumber]["error"] = spLogLine.HResult;
                     dataRows[phyNumber]["position"] = unit.lpPhysicalPositionName;
                     dataRows[phyNumber]["unitid"] = unit.cUnitID;
                     dataRows[phyNumber]["initial"] = unit.ulInitialCount;
                     dataRows[phyNumber]["maximum"] = unit.ulMaximum;
                     dataRows[phyNumber]["hwsensor"] = unit.bHardwareSensor;
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO Summary Exception {0}, {1}, {2}, i={3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }

                  // P H Y - x  T I M E  S E R I E S
                  try
                  {
                     string tableName = "Phy-" + phyNumber.ToString();
                     _unitIdToTableName[unit.cUnitID] = tableName;
                     DataTable dTable = dTableSet.Tables[tableName];

                     if (dTable == null)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO: table not found {0}", tableName));
                        continue;
                     }

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
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO PHYTable Exception {0}, {1}, {2}, i={3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }

               // Map currency and denom from logical to physical using usNumPhysicalCUs
               int physIdx = 0;
               for (int l = 0; l < cashInfo.usNumPhysicalCUss.Length; l++)
               {
                  if (!int.TryParse(cashInfo.usNumPhysicalCUss[l].Trim(), out int numPhys))
                     continue;

                  string currency = l < cashInfo.cCurrencyIDs.Length ? cashInfo.cCurrencyIDs[l].Trim() : "";
                  string denom = l < cashInfo.ulValues.Length ? cashInfo.ulValues[l].Trim() : "";

                  for (int p = 0; p < numPhys; p++)
                  {
                     int rowIdx = physIdx + 1;
                     if (rowIdx < dataRows.Length)
                     {
                        try
                        {
                           dataRows[rowIdx]["currency"] = currency;
                           dataRows[rowIdx]["denom"] = denom;
                        }
                        catch (Exception e)
                        {
                           ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_CASH_UNIT_INFO currency/denom Exception row {0}: {1}", rowIdx, e.Message));
                        }
                     }
                     physIdx++;
                  }
               }

               dTableSet.Tables["Summary"].AcceptChanges();
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
               // S U M M A R Y
               try
               {
                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
                  for (int i = 0; i < cashInfo.listPhysical.Count; i++)
                  {
                     int phyNumber = i + 1;
                     dataRows[phyNumber]["file"] = spLogLine.LogFile;
                     dataRows[phyNumber]["time"] = spLogLine.Timestamp;
                     dataRows[phyNumber]["error"] = spLogLine.HResult;
                     dataRows[phyNumber]["position"] = cashInfo.listPhysical[i].lpPhysicalPositionName;
                     dataRows[phyNumber]["unitid"] = cashInfo.listPhysical[i].cUnitID;
                     dataRows[phyNumber]["initial"] = cashInfo.listPhysical[i].ulInitialCount;
                     dataRows[phyNumber]["maximum"] = cashInfo.listPhysical[i].ulMaximum;
                     dataRows[phyNumber]["hwsensor"] = cashInfo.listPhysical[i].bHardwareSensor;
                  }
                  dTableSet.Tables["Summary"].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine("WFS_SRVE_CDM_CASHUNITINFOCHANGED Update Summary Exception : " + e.Message);
               }

               // P H Y - x  T I M E  S E R I E S
               for (int i = 0; i < cashInfo.listPhysical.Count; i++)
               {
                  try
                  {
                     string cUnitID = cashInfo.listPhysical[i].cUnitID;
                     if (!_unitIdToTableName.TryGetValue(cUnitID, out string tableName))
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CDM_CASHUNITINFOCHANGED: no table mapping for unitid={0}", cUnitID));
                        continue;
                     }

                     DataTable dTable = dTableSet.Tables[tableName];
                     if (dTable == null)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CDM_CASHUNITINFOCHANGED: table not found {0}", tableName));
                        continue;
                     }

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

      protected void WFS_SRVE_CIM_CASHUNITINFOCHANGED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSCIMCASHINFO cashInfo)
            {
               for (int i = 0; i < cashInfo.listPhysical.Count; i++)
               {
                  try
                  {
                     string cUnitID = cashInfo.listPhysical[i].cUnitID;
                     if (!cUnitID.StartsWith("CST_")) continue;

                     string serial = ParseSerialNumber(cashInfo.listPhysical[i].lpszExtra);
                     if (string.IsNullOrEmpty(serial)) continue;

                     DataRow scratchRow = dTableSet.Tables["Scratch"].NewRow();
                     scratchRow["cunitid"] = cUnitID;
                     scratchRow["file"] = cashInfo.LogFile;
                     scratchRow["time"] = cashInfo.Timestamp;
                     scratchRow["error"] = cashInfo.HResult;
                     scratchRow["status"] = cashInfo.listPhysical[i].usPStatus;
                     scratchRow["serial"] = serial;
                     dTableSet.Tables["Scratch"].Rows.Add(scratchRow);
                     dTableSet.Tables["Scratch"].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_CIM_CASHUNITINFOCHANGED PHYTable Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_CIM_CASHUNITINFOCHANGED Exception : " + e.Message);
         }
      }

      protected static string ParseSerialNumber(string lpszExtra)
      {
         const string key = "SerialNumber=";
         int start = lpszExtra.IndexOf(key);
         if (start < 0) return string.Empty;
         start += key.Length;
         int end = lpszExtra.IndexOf(',', start);
         return end < 0 ? lpszExtra.Substring(start) : lpszExtra.Substring(start, end - start);
      }
   }
}
