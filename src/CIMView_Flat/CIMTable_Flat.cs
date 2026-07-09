using Contract;
using System;
using System.Data;
using LogLineHandler;
using Impl;
using System.Linq;

namespace CIMView_Flat
{
   internal class CIMTable_Flat : BaseTable
   {
      // NoteID and NoteCount arrive on consecutive lines; buffer the ID so the
      // pair can be written as one 'notes accepted' row.
      private string _lastNoteID = string.Empty;

      // The SP polls status and escrow properties repeatedly, and denom/count lines
      // alternate, so 'compare with the previous row' never de-duplicates them.
      // Remember the last value per event and only add a row when the value changes.
      private System.Collections.Generic.Dictionary<string, string> _lastValueByEvent = new System.Collections.Generic.Dictionary<string, string>();

      public CIMTable_Flat(IContext ctx, string viewName) : base(ctx, viewName)
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
                  // C A S H  U N I T  /  C A S S E T T E  C O U N T S

                  case SPFlatType.CIM_CashUnitTrace:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashUnitTrace trace)
                        {
                           UpdateSummary(trace);
                           AddUnitRow(trace);
                        }
                        break;
                     }

                  // D E P O S I T  L I F E C Y C L E

                  case SPFlatType.CIM_StartCashIn:
                     {
                        base.ProcessRow(spFlatLine);
                        // each deposit re-baselines the polled status/escrow values
                        _lastValueByEvent.Clear();
                        AddDeposit(spFlatLine, "start cashin", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_SetCashInLimit:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDeposit(spFlatLine, "cashin limit", cashInLine.Value);
                        }
                        break;
                     }

                  case SPFlatType.CIM_HandleCashInStart:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "start result", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_AcceptCash:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "accept cash", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_ItemsInserted:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "items inserted", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_InputRefuse:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "input refused", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_HandleCashIn:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "accept result", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_NoteID:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           _lastNoteID = cashInLine.Value;
                        }
                        break;
                     }

                  case SPFlatType.CIM_NoteCount:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDeposit(spFlatLine, "notes accepted", String.Format("{0} x noteid {1}", cashInLine.Value, _lastNoteID));
                           _lastNoteID = string.Empty;
                        }
                        break;
                     }

                  case SPFlatType.CIM_CashInStatus:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDepositConditionally(spFlatLine, "cashin status", cashInLine.Value);
                        }
                        break;
                     }

                  case SPFlatType.CIM_CashInRefused:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDepositConditionally(spFlatLine, "refused count", cashInLine.Value);
                        }
                        break;
                     }

                  case SPFlatType.CIM_LastCashInStatus:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDepositConditionally(spFlatLine, "last status", cashInLine.Value);
                        }
                        break;
                     }

                  case SPFlatType.CIM_CashInStatusValue:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDepositConditionally(spFlatLine, String.Format("escrow[{0}] denom", cashInLine.Index), cashInLine.Value);
                        }
                        break;
                     }

                  case SPFlatType.CIM_CashInStatusItemCount:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDepositConditionally(spFlatLine, String.Format("escrow[{0}] count", cashInLine.Index), cashInLine.Value);
                        }
                        break;
                     }

                  case SPFlatType.CIM_StoreCash:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "store cash", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_HandleCashInEnd:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "cashin end", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_StoreCashComplete:
                     {
                        base.ProcessRow(spFlatLine);
                        if (spFlatLine is CIMCashInLine cashInLine)
                        {
                           AddDeposit(spFlatLine, "store complete", cashInLine.Value == "1" ? "ok" : "FAILED");
                        }
                        break;
                     }

                  case SPFlatType.CIM_RollbackCash:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "rollback", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_HandleRollback:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "rollback result", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_HandleRetract:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "retract result", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_HandleReset:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "reset result", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_HandleOpenShutter:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "open shutter", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_HandleCloseShutter:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "close shutter", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_ItemsTaken:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "items taken", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_CountsChanged:
                     {
                        base.ProcessRow(spFlatLine);
                        AddDeposit(spFlatLine, "counts changed", string.Empty);
                        break;
                     }

                  case SPFlatType.CIM_CashUnitInfo:
                     {
                        base.ProcessRow(spFlatLine);
                        // 150+ polls per day - only worth a row when it failed
                        if (!string.IsNullOrEmpty(spFlatLine.HResult))
                        {
                           AddDeposit(spFlatLine, "cashunit info", string.Empty);
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
            ctx.LogWriteLine("CIMTable_Flat.ProcessRow EXCEPTION: " + e.Message);
         }
      }

      private void UpdateSummary(CIMCashUnitTrace line)
      {
         try
         {
            DataRow[] dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = '{0}'", line.UnitIndex));
            if (dataRows.Length == 0)
            {
               return;
            }

            DataRow row = dataRows[0];
            row["file"] = line.LogFile;
            row["time"] = line.Timestamp;
            row["error"] = line.HResult;
            row["type"] = line.UnitType;
            row["id"] = line.UnitID;
            row["currency"] = line.CurrencyID;
            row["denom"] = String.Join(",", line.FieldList("ItemValue"));
            row["initial"] = line.InitialCount;
            row["total"] = line.TotalCount;
            row["cashin"] = line.CashInCount;
            row["dispensed"] = line.DispensedCount;
            row["presented"] = line.PresentedCount;
            row["retracted"] = line.RetractedCount;
            row["reject"] = line.RejectCount;
            row["status"] = line.Status;
            row["slot"] = String.Join(",", line.FieldList("PCUPositionName"));

            dTableSet.Tables["Summary"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", "UpdateSummary", line.LogFile, line.Timestamp, e.Message));
         }
      }

      private void AddUnitRow(CIMCashUnitTrace line)
      {
         try
         {
            // Unit-1 .. Unit-9 track counts over time; logical unit index is zero-based
            int unitNumber = line.UnitIndex + 1;
            if (unitNumber < 1 || unitNumber > 9)
            {
               return;
            }

            string tableName = "Unit-" + unitNumber.ToString();
            DataTable dTable = dTableSet.Tables[tableName];

            DataRow row = dTable.NewRow();
            row["file"] = line.LogFile;
            row["time"] = line.Timestamp;
            row["error"] = line.HResult;
            row["total"] = line.TotalCount;
            row["cashin"] = line.CashInCount;
            row["dispensed"] = line.DispensedCount;
            row["presented"] = line.PresentedCount;
            row["retracted"] = line.RetractedCount;
            row["reject"] = line.RejectCount;
            row["status"] = line.Status;
            dTable.Rows.Add(row);

            dTable.AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", "AddUnitRow", line.LogFile, line.Timestamp, e.Message));
         }
      }

      private void AddDeposit(SPFlatLine line, string eventName, string value)
      {
         try
         {
            DataRow row = dTableSet.Tables["Deposit"].Rows.Add();
            row["file"] = line.LogFile;
            row["time"] = line.Timestamp;
            row["error"] = line.HResult;
            row["event"] = eventName;
            row["value"] = value;
            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", "AddDeposit", line.LogFile, line.Timestamp, e.Message));
         }
      }

      private void AddDepositConditionally(SPFlatLine line, string eventName, string value)
      {
         try
         {
            // Skip the row if this event's value has not changed since it was last seen
            string lastValue;
            if (_lastValueByEvent.TryGetValue(eventName, out lastValue) && lastValue == value)
            {
               return;
            }

            _lastValueByEvent[eventName] = value;
            AddDeposit(line, eventName, value);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("{0} Table Exception {1}. {2}, {3}", "AddDepositConditionally", line.LogFile, line.Timestamp, e.Message));
         }
      }
   }
}
