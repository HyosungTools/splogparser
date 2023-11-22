using Contract;
using Impl;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.Data;

namespace IPMView
{
   internal class IPMTable : BaseTable
   {
      private readonly bool have_seen_WFS_INF_IPM_MEDIA_BIN_INFO = false;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public IPMTable(IContext ctx, string viewName) : base(ctx, viewName)
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
                  case LogLineHandler.XFSType.WFS_INF_IPM_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_IPM_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_IPM_MEDIA_BIN_INFO:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_IPM_MEDIA_BIN_INFO(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_INF_IPM_TRANSACTION_STATUS:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_INF_IPM_TRANSACTION_STATUS(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_MEDIA_IN:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_MEDIA_IN(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_MEDIA_IN_END:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_MEDIA_IN_END(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_MEDIA_IN_ROLLBACK:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_MEDIA_IN_ROLLBACK(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_PRESENT_MEDIA:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_PRESENT_MEDIA(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_RETRACT_MEDIA:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_RETRACT_MEDIA(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_RESET:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_RESET(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_PRINT_TEXT:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_PRINT_TEXT(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_CMD_IPM_EXPEL_MEDIA:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_CMD_IPM_EXPEL_MEDIA(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_IPM_MEDIAINSERTED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_IPM_MEDIAINSERTED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_USRE_IPM_MEDIABINTHRESHOLD:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_USRE_IPM_MEDIABINTHRESHOLD(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_IPM_MEDIABININFOCHANGED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_IPM_MEDIABININFOCHANGED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_IPM_MEDIABINERROR:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_IPM_MEDIABINERROR(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_IPM_MEDIATAKEN:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_IPM_MEDIATAKEN(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_SRVE_IPM_MEDIADETECTED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_SRVE_IPM_MEDIADETECTED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_IPM_MEDIAPRESENTED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_IPM_MEDIAPRESENTED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_IPM_MEDIAREFUSED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_IPM_MEDIAREFUSED(spLogLine);
                        break;
                     }
                  case LogLineHandler.XFSType.WFS_EXEE_IPM_MEDIAREJECTED:
                     {
                        base.ProcessRow(spLogLine);
                        WFS_EXEE_IPM_MEDIAREJECTED(spLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("IPMTable.ProcessRow EXCEPTION:" + e.Message);
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
            string[] columns = new string[] { "error", "status", "acceptor", "media", "stacker" };
            CompressTable(tableName, columns);

            // ADD ENGLISH
            string[,] colKeyMap = new string[4, 2]
            {
               {"status", "fwDevice" },
               {"acceptor", "fwAcceptor" },
               {"media", "wMedia" },
               {"stacker", "wStacker" }
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
            string[,] colKeyMap = new string[2, 2]
            {
                  {"type", "fwType" },
                  {"mediatype", "wMediaType" }
            };
            AddEnglishToTable(tableName, colKeyMap);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

         try
         {
            // M E D I A  B I N   T A B L E

            ctx.ConsoleWriteLogLine("Compress the MediaBIn Tables: sort by time, visit every row and delete rows that are unchanged from their predecessor");

            // the list of columns to compare
            string[] cashUnitCols = new string[] { "error", "status", "count", "retract" };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("MediaBin-"))
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
               if (dTable.TableName.StartsWith("MediaIn-"))
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

            string[,] colKeyMap = new string[4, 2]
            {
               {"trans","wMediaInTransaction" },
               {"status", "wStatus" },
               {"reason", "wFailure" },
               {"reason", "wReason" }
            };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               if (dTable.TableName.Equals("Deposit"))
               {
                  // ADD ENGLISH
                  tableName = dTable.TableName;
                  AddEnglishToTable(tableName, colKeyMap);
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }


         // RENAME MEDIABIN TABLES - DO THIS LAST

         try
         {
            // rename the MediaBin units
            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Rename the MediaBins : Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("MediaBin-"))
               {
                  string unitNumber = dTable.TableName.Replace("MediaBin-", string.Empty);
                  ctx.ConsoleWriteLogLine("unitNumber :" + unitNumber);

                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select(String.Format("number = {0}", unitNumber));
                  if (dataRows.Length == 1)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("Changing table name from '{0}' to '{1}'", dTable.TableName, dataRows[0]["name"].ToString()));
                     dTable.TableName = dataRows[0]["name"].ToString();
                     dTable.AcceptChanges();
                  }
               }
            }

            // TODO - delete any MediaBin column named LUx

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception : " + e.Message);
         }

         return base.WriteExcelFile();
      }

      protected void WFS_INF_IPM_STATUS(SPLine spLogLine)
      {
         try
         {
            //ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_STATUS tracefile '{0}' timestamp '{1}", spLogLine.LogFile, spLogLine.Timestamp));

            if (spLogLine is WFSIPMSTATUS ipmStatus)
            {
               DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

               dataRow["file"] = spLogLine.LogFile;
               dataRow["time"] = spLogLine.Timestamp;
               dataRow["error"] = spLogLine.HResult;

               dataRow["status"] = ipmStatus.fwDevice;
               dataRow["acceptor"] = ipmStatus.wAcceptor;
               dataRow["media"] = ipmStatus.wMedia;
               dataRow["stacker"] = ipmStatus.wStacker;

               dTableSet.Tables["Status"].AcceptChanges();
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IPM_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_INF_IPM_MEDIA_BIN_INFO(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMMEDIABININFO binInfo)
            {
               if (!have_seen_WFS_INF_IPM_MEDIA_BIN_INFO)
               {
                  //// First time seeing MEDIA_BIN_INFO, populate the Summary Table
                  //have_seen_WFS_INF_IPM_MEDIA_BIN_INFO = true;

                  DataRow[] dataRows = dTableSet.Tables["Summary"].Select();

                  // for each row, set the tracefile, timestamp and hresult
                  for (int i = 0; i < binInfo.lUnitCount; i++)
                  {
                     // Now use the usNumbers to create and populate a row in the CashUnit-x table
                     int usBinNumber = int.Parse(binInfo.usBinNumbers[i].Trim());
                     if (usBinNumber < 1)
                     {
                        // We have to check because some log lines are truncated (i.e. "more data")
                        // and produce bad results
                        continue;
                     }

                     try
                     {
                        dataRows[usBinNumber]["file"] = spLogLine.LogFile;
                        dataRows[usBinNumber]["time"] = spLogLine.Timestamp;
                        dataRows[usBinNumber]["error"] = spLogLine.HResult;

                        dataRows[usBinNumber]["number"] = binInfo.usBinNumbers[i];
                        dataRows[usBinNumber]["type"] = binInfo.fwTypes[i];
                        dataRows[usBinNumber]["mediatype"] = binInfo.wMediaTypes[i];
                        dataRows[usBinNumber]["name"] = binInfo.lpstrBinIDs[i];
                        dataRows[usBinNumber]["maxitems"] = binInfo.ulMaximumItems[i];
                        dataRows[usBinNumber]["maxretracts"] = binInfo.ulMaximumRetractOperations[i];
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_MEDIA_BIN_INFO Summary Table Exception {0}. {1}, {2}", spLogLine.LogFile, spLogLine.Timestamp, e.Message));
                     }
                  }

                  dTableSet.Tables["Summary"].AcceptChanges();
               }

               for (int i = 0; i < binInfo.lUnitCount; i++)
               {
                  try
                  {
                     // Now use the usNumbers to create and populate a row in the MediaBin-x table
                     if (int.Parse(binInfo.usBinNumbers[i].Trim()) < 1)
                     {
                        // We have to check because some log lines are truncated (i.e. "more data")
                        // and produce bad results
                        continue;
                     }

                     if (binInfo.ulCounts[i] == "0" && binInfo.ulRetractOperations[i] == "0")
                     {
                        // again truncated log lines result in garbage output
                        continue;
                     }

                     string tableName = "MediaBin-" + binInfo.usBinNumbers[i].Trim();
                     DataRow mediaBinRow = dTableSet.Tables[tableName].Rows.Add();

                     mediaBinRow["file"] = spLogLine.LogFile;
                     mediaBinRow["time"] = spLogLine.Timestamp;
                     mediaBinRow["error"] = spLogLine.HResult;

                     mediaBinRow["count"] = binInfo.ulCounts[i];
                     mediaBinRow["retract"] = binInfo.ulRetractOperations[i];
                     mediaBinRow["status"] = binInfo.usStatuses[i];

                     dTableSet.Tables[tableName].AcceptChanges();
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_MEDIA_BIN_INFO Cash Unit Table Exception {0}, {1}, {2}, {3}", spLogLine.LogFile, spLogLine.Timestamp, e.Message, i));
                  }
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IPM_MEDIA_BIN_INFO Exception : " + e.Message);
         }
      }

      protected void WFS_INF_IPM_TRANSACTION_STATUS(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMTRANSSTATUS transInfo)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = transInfo.wMediaInTransaction;
               depRow["onstacker"] = transInfo.usMediaOnStacker;
               depRow["lasttotal"] = transInfo.usLastMediaInTotal;
               depRow["accepted"] = transInfo.usLastMediaAddedToStacker;
               depRow["totalitems"] = transInfo.usTotalItems;
               depRow["totalrefsd"] = transInfo.usTotalItemsRefused;
               depRow["totlbnchrefsd"] = transInfo.usTotalBunchesRefused;
               depRow["comment"] = transInfo.lpszExtra;

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IPM_TRANSACTION_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_MEDIA_IN(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSIPMMEDIAIN mediaIn)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "ready";

               dTableSet.Tables["Deposit"].AcceptChanges();
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_MEDIA_IN Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_MEDIA_IN_END(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMMEDIAINEND mediaInEnd)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "media in end";
               depRow["itemsreturned"] = mediaInEnd.usItemsReturned;
               depRow["totalrefsd"] = mediaInEnd.usItemsRefused;
               depRow["totlbnchrefsd"] = mediaInEnd.usBunchesRefused;

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_MEDIA_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_MEDIA_IN_ROLLBACK(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "rollback";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_MEDIA_IN_ROLLBACK Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_PRESENT_MEDIA(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "present media";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_PRESENT_MEDIA Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_RETRACT_MEDIA(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMRETRACTMEDIAOUT mediaRetract)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "retract";

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_RETRACT_MEDIA Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_RESET(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "reset";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_RESET Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_PRINT_TEXT(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "endorse";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_PRINT_TEXT Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_EXPEL_MEDIA(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "expel";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_EXPEL_MEDIA Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAINSERTED(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "inserted";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAINSERTED Exception : " + e.Message);
         }
      }

      protected void WFS_USRE_IPM_MEDIABINTHRESHOLD(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMMEDIABININFO binInfo)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "threshold";

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_IPM_MEDIABINTHRESHOLD Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_IPM_MEDIABININFOCHANGED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMMEDIABININFO binInfo)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "info changed";

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_IPM_MEDIABININFOCHANGED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIABINERROR(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMEDIABINERROR binInfo)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "error";
               depRow["reason"] = binInfo.wFailure;

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIABINERROR Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_IPM_MEDIATAKEN(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "taken";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_IPM_MEDIATAKEN Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_IPM_MEDIADETECTED(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "detected";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_IPM_MEDIADETECTED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAPRESENTED(SPLine spLogLine)
      {
         try
         {
            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = spLogLine.LogFile;
            depRow["time"] = spLogLine.Timestamp;
            depRow["error"] = spLogLine.HResult;

            depRow["trans"] = "presented";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAPRESENTED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAREFUSED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is LogLineHandler.WFSIPMMEDIAREFUSED binInfo)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "refused";
               depRow["reason"] = binInfo.wReason;

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAREFUSED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAREJECTED(SPLine spLogLine)
      {
         try
         {
            if (spLogLine is WFSIPMMEDIAREJECTED binInfo)
            {
               DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

               depRow["file"] = spLogLine.LogFile;
               depRow["time"] = spLogLine.Timestamp;
               depRow["error"] = spLogLine.HResult;

               depRow["trans"] = "rejected";
               depRow["reason"] = binInfo.wReason;

               dTableSet.Tables["Deposit"].AcceptChanges();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAREJECTED Exception : " + e.Message);
         }
      }
   }
}

