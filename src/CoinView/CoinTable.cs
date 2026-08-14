using Contract;
using Impl;
using System;
using System.Data;
using LogLineHandler;

namespace CoinView
{
   /// <summary>
   /// Table for the Coin dispenser view. Reuses the same CDM XFS parse classes (WFSCDMSTATUS,
   /// WFSCDMDENOMINATION, WFSCDMCUINFO) that the cash dispenser uses - the wire format is identical -
   /// but only processes the WFS_*_COIN_* types the factory tagged, and writes to the Coin worksheets.
   /// MVP scope: status, dispense (with a dispensable flag derived from the result), and coin cash-unit
   /// counts.
   /// </summary>
   internal class CoinTable : BaseTable
   {
      public CoinTable(IContext ctx, string viewName) : base(ctx, viewName)
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
                  case LogLineHandler.XFSType.WFS_INF_COIN_STATUS:
                     WFS_INF_COIN_STATUS(spLogLine);
                     break;

                  case LogLineHandler.XFSType.WFS_CMD_COIN_DISPENSE:
                     WFS_CMD_COIN_DISPENSE(spLogLine);
                     break;

                  case LogLineHandler.XFSType.WFS_INF_COIN_CASH_UNIT_INFO:
                     WFS_INF_COIN_CASH_UNIT_INFO(spLogLine);
                     break;

                  case LogLineHandler.XFSType.WFS_SRVE_COIN_ITEMSTAKEN:
                     WFS_SRVE_COIN_ITEMSTAKEN(spLogLine);
                     break;

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CoinTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      // ---- status ----
      private void WFS_INF_COIN_STATUS(SPLine spLogLine)
      {
         if (!(spLogLine is WFSCDMSTATUS status)) return;
         try
         {
            DataRow row = dTableSet.Tables["CoinStatus"].Rows.Add();
            row["file"] = spLogLine.LogFile;
            row["time"] = spLogLine.Timestamp;
            row["error"] = spLogLine.HResult;
            row["status"] = status.fwDevice;
            row["dispenser"] = status.fwDispenser;
            row["shutter"] = status.fwShutter;
            row["posstatus"] = status.fwPositionStatus;
            row["position"] = status.wDevicePosition;
            if (status.lpszExtra != null)
            {
               row["errcode"] = status.lpszExtra.ErrCode;
               row["errmsg"] = status.lpszExtra.ErrMsg;
            }
            dTableSet.Tables["CoinStatus"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CoinTable.WFS_INF_COIN_STATUS EXCEPTION:" + e.Message);
         }
      }

      // ---- dispense (the coin cash-out; NotDispensable shows up as a non-zero result) ----
      private void WFS_CMD_COIN_DISPENSE(SPLine spLogLine)
      {
         try
         {
            DataRow row = dTableSet.Tables["CoinDispense"].Rows.Add();
            row["file"] = spLogLine.LogFile;
            row["time"] = spLogLine.Timestamp;
            row["error"] = spLogLine.HResult;
            row["dispensable"] = IsOk(spLogLine.HResult) ? "yes" : "NO (" + spLogLine.HResult + ")";

            if (spLogLine is WFSCDMDENOMINATION denom)
            {
               row["currency"] = denom.cCurrencyID;
               row["amount"] = denom.ulAmount;
               row["count"] = denom.usCount;
               row["values"] = denom.lpulValues != null ? string.Join(",", denom.lpulValues) : "";
            }
            dTableSet.Tables["CoinDispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CoinTable.WFS_CMD_COIN_DISPENSE EXCEPTION:" + e.Message);
         }
      }

      // ---- coin cash units (per-tube counts) ----
      private void WFS_INF_COIN_CASH_UNIT_INFO(SPLine spLogLine)
      {
         if (!(spLogLine is WFSCDMCUINFO cu)) return;
         try
         {
            int count = cu.lUnitCount > 0 ? cu.lUnitCount : 0;
            for (int i = 0; i < count; i++)
            {
               DataRow row = dTableSet.Tables["CoinUnit"].Rows.Add();
               row["file"] = spLogLine.LogFile;
               row["time"] = spLogLine.Timestamp;
               row["error"] = spLogLine.HResult;
               row["unit"] = At(cu.usNumbers, i);
               row["currency"] = At(cu.cCurrencyIDs, i);
               row["denom"] = At(cu.ulValues, i);
               row["status"] = At(cu.usStatuses, i);
               row["count"] = At(cu.ulCounts, i);
               row["dispensed"] = At(cu.ulDispensedCounts, i);
               row["reject"] = At(cu.ulRejectCounts, i);
               dTableSet.Tables["CoinUnit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CoinTable.WFS_INF_COIN_CASH_UNIT_INFO EXCEPTION:" + e.Message);
         }
      }

      // ---- items taken (customer took the coins) -> note on the dispense sheet ----
      private void WFS_SRVE_COIN_ITEMSTAKEN(SPLine spLogLine)
      {
         try
         {
            DataRow row = dTableSet.Tables["CoinDispense"].Rows.Add();
            row["file"] = spLogLine.LogFile;
            row["time"] = spLogLine.Timestamp;
            row["error"] = spLogLine.HResult;
            row["comment"] = "coins taken";
            dTableSet.Tables["CoinDispense"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CoinTable.WFS_SRVE_COIN_ITEMSTAKEN EXCEPTION:" + e.Message);
         }
      }

      // ---- helpers ----
      private static bool IsOk(string hResult)
      {
         return string.IsNullOrEmpty(hResult) || hResult == "0";
      }

      private static string At(string[] arr, int i)
      {
         return (arr != null && i >= 0 && i < arr.Length && arr[i] != null) ? arr[i] : "";
      }
   }
}
