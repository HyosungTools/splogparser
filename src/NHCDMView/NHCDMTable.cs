using Contract;
using Impl;
using System;
using System.Data;
using LogLineHandler;

namespace NHCDMView
{
   internal class NHCDMTable : BaseTable
   {
      public NHCDMTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         _zeroAsBlank = false;
      }

      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is SPLine spLogLine)
            {
               switch (spLogLine.xfsType)
               {
                  case LogLineHandler.XFSType.NHCDM_SETCASHUNITINFORESULT:
                     {
                        base.ProcessRow(spLogLine);
                        NHCDM_SETCASHUNITINFORESULT(spLogLine);
                        break;
                     }
                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("NHCDMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

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
            // C O M P R E S S  N H C D M - x  T A B L E S
            string[] columns = new string[] { "status", "serial", "current", "initial", "dispensed", "presented", "rejected", "retracted", "noterev", "calibration", "missingcheck" };
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.StartsWith("NHCDM-"))
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
               if (dTable.TableName.StartsWith("NHCDM-"))
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
            // R E N A M E  C A S S E T T E S
            DataRow[] allRows = dTableSet.Tables["Summary"].Select();
            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.StartsWith("NHCDM-"))
               {
                  string nhcdmNumber = dTable.TableName.Replace("NHCDM-", string.Empty);
                  int idx = int.Parse(nhcdmNumber) - 1;
                  if (idx < allRows.Length && !string.IsNullOrWhiteSpace(allRows[idx]["cstnumber"].ToString()))
                  {
                     string newName = allRows[idx]["cstnumber"].ToString().Trim();
                     ctx.ConsoleWriteLogLine(String.Format("Renaming {0} to {1}", dTable.TableName, newName));
                     dTable.TableName = newName;
                     dTable.AcceptChanges();
                  }
                  else
                  {
                     ctx.ConsoleWriteLogLine(String.Format("No Summary row found for {0}, leaving unchanged", dTable.TableName));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("Exception renaming NHCDM tables - {0}", e.Message));
         }

         return base.WriteExcelFile();
      }

      protected void NHCDM_SETCASHUNITINFORESULT(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is NHCDMSetCashUnitInfoResult cstInfo)
            {
               ctx.ConsoleWriteLogLine(String.Format("NHCDM_SETCASHUNITINFORESULT: CassetteCount={0} CstNumber[0]='{1}'",
                  cstInfo.CstNumber.Length, cstInfo.CstNumber[0]));

               // S U M M A R Y  — write once per cassette on first encounter
               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

               for (int i = 0; i < cstInfo.CstNumber.Length; i++)
               {
                  try
                  {
                     int rowIdx = i + 1;
                     if (rowIdx >= dataRows.Length)
                        continue;

                     dataRows[rowIdx]["file"] = cstInfo.LogFile;
                     dataRows[rowIdx]["time"] = cstInfo.Timestamp;
                     dataRows[rowIdx]["error"] = cstInfo.HResult;
                     dataRows[rowIdx]["cstnumber"] = cstInfo.CstNumber[i];
                     dataRows[rowIdx]["cstid"] = cstInfo.CstID[i];
                     dataRows[rowIdx]["currency"] = cstInfo.CurrencyID[i];
                     dataRows[rowIdx]["denom"] = cstInfo.Values[i];
                     dataRows[rowIdx]["noterev"] = cstInfo.NoteRevision[i];
                     dataRows[rowIdx]["calibration"] = cstInfo.Calibration[i];
                     dataRows[rowIdx]["missingcheck"] = cstInfo.MissingCheck[i];
                     dataRows[rowIdx]["initial"] = cstInfo.InitialCount[i];
                     dataRows[rowIdx]["serial"] = cstInfo.SerialNumber[i];
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("NHCDM_SETCASHUNITINFORESULT Summary Exception row {0}: {1}", i, e.Message));
                  }
               }

               dTableSet.Tables["Summary"].AcceptChanges();

               // N H C D M - x  T I M E  S E R I E S
               for (int i = 0; i < cstInfo.CstNumber.Length; i++)
               {
                  try
                  {
                     // Skip placeholder cassettes
                     if (cstInfo.CurrencyID[i] == "XXX" || string.IsNullOrEmpty(cstInfo.CstNumber[i]))
                        continue;

                     string tableName = "NHCDM-" + (i + 1).ToString();
                     DataTable dTable = dTableSet.Tables[tableName];
                     if (dTable == null)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("NHCDM_SETCASHUNITINFORESULT: table not found {0}", tableName));
                        continue;
                     }

                     DataRow newRow = dTable.NewRow();
                     newRow["file"] = cstInfo.LogFile;
                     newRow["time"] = cstInfo.Timestamp;
                     newRow["error"] = cstInfo.HResult;
                     newRow["status"] = cstInfo.Status[i];
                     newRow["serial"] = cstInfo.SerialNumber[i];
                     newRow["current"] = cstInfo.CurrentCount[i];
                     newRow["initial"] = cstInfo.InitialCount[i];
                     newRow["dispensed"] = cstInfo.DispenseCount[i];
                     newRow["presented"] = cstInfo.PresentCount[i];
                     newRow["rejected"] = cstInfo.RejectCount[i];
                     newRow["retracted"] = cstInfo.RetractCount[i];
                     newRow["noterev"] = cstInfo.NoteRevision[i];
                     newRow["calibration"] = cstInfo.Calibration[i];
                     newRow["missingcheck"] = cstInfo.MissingCheck[i];
                     dTable.Rows.Add(newRow);
                     dTable.AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("NHCDM_SETCASHUNITINFORESULT NHCDM-{0} Exception: {1}", i + 1, e.Message));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("NHCDM_SETCASHUNITINFORESULT Exception: " + e.Message);
         }
      }
   }
}
