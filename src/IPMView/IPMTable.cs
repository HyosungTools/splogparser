using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;

namespace IPMView
{
   internal class IPMTable : BaseTable
   {
      private bool have_seen_WFS_INF_IPM_MEDIA_BIN_INFO = false;

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
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            (XFSType xfsType, string xfsLine) result = IdentifyLines.XFSLine(logLine);
            switch (result.xfsType)
            {
               case XFSType.WFS_INF_IPM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_IPM_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_IPM_MEDIA_BIN_INFO:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_IPM_MEDIA_BIN_INFO(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_IPM_TRANSACTION_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_INF_IPM_TRANSACTION_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_IPM_MEDIA_IN:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_IPM_MEDIA_IN(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_IPM_MEDIA_IN_END:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_IPM_MEDIA_IN_END(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_IPM_MEDIA_IN_ROLLBACK:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_IPM_MEDIA_IN_ROLLBACK(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_IPM_PRESENT_MEDIA:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_IPM_PRESENT_MEDIA(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_IPM_RETRACT_MEDIA:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_IPM_RETRACT_MEDIA(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_IPM_RESET:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_IPM_RESET(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_CMD_IPM_EXPEL_MEDIA:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_CMD_IPM_EXPEL_MEDIA(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_EXEE_IPM_MEDIAINSERTED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_EXEE_IPM_MEDIAINSERTED(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_USRE_IPM_MEDIABINTHRESHOLD:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_USRE_IPM_MEDIABINTHRESHOLD(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_SRVE_IPM_MEDIABININFOCHANGED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_SRVE_IPM_MEDIABININFOCHANGED(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_EXEE_IPM_MEDIABINERROR:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_EXEE_IPM_MEDIABINERROR(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_SRVE_IPM_MEDIATAKEN:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_SRVE_IPM_MEDIATAKEN(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_SRVE_IPM_MEDIADETECTED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_SRVE_IPM_MEDIADETECTED(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_EXEE_IPM_MEDIAPRESENTED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_EXEE_IPM_MEDIAPRESENTED(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_EXEE_IPM_MEDIAREFUSED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_EXEE_IPM_MEDIAREFUSED(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_EXEE_IPM_MEDIAREJECTED:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_EXEE_IPM_MEDIAREJECTED(result.xfsLine);
                     break;
                  }

               default:
                  break;
            };
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
         try
         {
            // S T A T U S   T A B L E

            // sort the table by time, visit every row and delete rows that are unchanged from their predecessor
            ctx.ConsoleWriteLogLine("Compress the Status Table: sort by time, visit every row and delete rows that are unchanged from their predecessor");
            ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table start: rows before: {0}", dTableSet.Tables["Status"].Rows.Count));

            // the list of columns to compare
            string[] columns = new string[] { "error", "status", "acceptor", "media", "stacker" };
            (bool success, string message) result = _datatable_ops.DeleteUnchangedRowsInTable(dTableSet.Tables["Status"], "time ASC", columns);
            if (!result.success)
            {
               ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + result.message);
            }
            ctx.ConsoleWriteLogLine(String.Format("Compress the Status Table complete: rows after: {0}", dTableSet.Tables["Status"].Rows.Count));

            // add English to the Status Table
            string[,] colKeyMap = new string[4, 2]
            {
               {"status", "fwDevice" },
               {"acceptor", "fwAcceptor" },
               {"media", "wMedia" },
               {"stacker", "wStacker" }
            };

            for (int i = 0; i < 4; i++)
            {
               result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Status"], dTableSet.Tables["Messages"], colKeyMap[i, 0], colKeyMap[i, 1]);
            }

            // S U M M A R Y   T A B L E 

            try
            {
               // delete redundant lines from the Summary Table
               ctx.ConsoleWriteLogLine("Delete redundant lines from the Summary Table");
               DataRow[] dataRows = dTableSet.Tables["Summary"].Select();
               List<DataRow> deleteRows = new List<DataRow>();
               foreach (DataRow dataRow in dataRows)
               {
                  if (dataRow["file"].ToString().Trim() == string.Empty)
                  {
                     deleteRows.Add(dataRow);
                  }
               }

               foreach (DataRow dataRow in deleteRows)
               {
                  dataRow.Delete();
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("Failed to delete redundant lines from Summary Table : {0}", e.Message));
            }

            try
            {
               // Add English to Summary Table
               ctx.ConsoleWriteLogLine("Add English to Summary Table");
               string[,] summaryColMap = new string[2, 2]
               {
               {"type", "fwType" },
               {"mediatype", "wMediaType" }
               };

               for (int i = 0; i < 2; i++)
               {
                  result = _datatable_ops.AddEnglishToTable(ctx, dTableSet.Tables["Summary"], dTableSet.Tables["Messages"], summaryColMap[i, 0], summaryColMap[i, 1]);
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("Failed to add English to the Summary Table : {0}", e.Message));
            }


            // M E D I A  B I N   T A B L E
            try
            {
               ctx.ConsoleWriteLogLine("Compress the MediaBIn Tables: sort by time, visit every row and delete rows that are unchanged from their predecessor");

               // the list of columns to compare
               string[] cashUnitCols = new string[] { "error", "status", "count", "retract" };

               foreach (DataTable dTable in dTableSet.Tables)
               {
                  ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
                  if (dTable.TableName.StartsWith("MediaBin-"))
                  {
                     ctx.ConsoleWriteLogLine(String.Format("Compress the Table '{0}' rows before: {1}", dTable.TableName, dTable.Rows.Count));
                     (bool success, string message) cashUnitResult = _datatable_ops.DeleteUnchangedRowsInTable(dTable, "time ASC", cashUnitCols);
                     if (!cashUnitResult.success)
                     {
                        ctx.ConsoleWriteLogLine("Unexpected error during table compression : " + cashUnitResult.message);
                     }
                     ctx.ConsoleWriteLogLine(String.Format("Compress the Table '{0}' rows after: {1}", dTable.TableName, dTable.Rows.Count));
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("Failed to compress the MediaBin tables: {0}", e.Message));
            }

            // add English to MediaIn Tables
            string[,] cashUnitColMap = new string[1, 2]
            {
            {"status", "usStatus" }
            };

            foreach (DataTable dTable in dTableSet.Tables)
            {
               ctx.ConsoleWriteLogLine("Looking at table :" + dTable.TableName);
               if (dTable.TableName.StartsWith("MediaIn-"))
               {
                  for (int i = 0; i < 1; i++)
                  {
                     result = _datatable_ops.AddEnglishToTable(ctx, dTable, dTableSet.Tables["Messages"], cashUnitColMap[i, 0], cashUnitColMap[i, 1]);
                  }
               }
            }


            // D E P O S I T   T A B L E

            // add English to Deposit
            string[,] depositColMap = new string[4, 2]
            {
               {"trans","wMediaInTransaction" },
               {"status", "wStatus" },
               {"reason", "wFailure" },
               {"reason", "wReason" }
            };

            try
            {
               foreach (DataTable dTable in dTableSet.Tables)
               {
                  if (dTable.TableName.Equals("Deposit"))
                  {
                     ctx.ConsoleWriteLogLine(String.Format("Adding English to table '{0}'", dTable.TableName));
                     for (int i = 0; i < 4; i++)
                     {
                        result = _datatable_ops.AddEnglishToTable(ctx, dTable, dTableSet.Tables["Messages"], depositColMap[i, 0], depositColMap[i, 1]);
                     }
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("EXCEPTION adding English to to table:'{0}'", e.Message));
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


         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WriteExcelFile Exception : " + e.Message);
         }

         return base.WriteExcelFile();
      }

      protected (bool success, DataRow dataRow) FindMessages(string type, string code)
      {
         // Create an array for the key values to find.
         object[] findByKeys = new object[2];

         // Set the values of the keys to find.
         findByKeys[0] = type;
         findByKeys[1] = code;

         DataRow foundRow = dTableSet.Tables["Messages"].Rows.Find(findByKeys);
         if (foundRow != null)
         {
            return (true, foundRow);
         }
         else
         {
            return (false, null);
         }
      }

      protected void WFS_INF_IPM_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMSTATUS ipmStatus = new WFSIPMSTATUS(ctx);

            try
            {
               ipmStatus.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_CDM_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow dataRow = dTableSet.Tables["Status"].Rows.Add();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);

            dataRow["status"] = ipmStatus.fwDevice;
            dataRow["acceptor"] = ipmStatus.wAcceptor;
            dataRow["media"] = ipmStatus.wMedia;
            dataRow["stacker"] = ipmStatus.wStacker;

            dTableSet.Tables["Status"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IPM_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_INF_IPM_MEDIA_BIN_INFO(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_MEDIA_BIN_INFO tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMMEDIABININFO binInfo = new WFSIPMMEDIABININFO(ctx);

            try
            {
               binInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_MEDIA_BIN_INFO Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

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
                     dataRows[usBinNumber]["file"] = _traceFile;
                     dataRows[usBinNumber]["time"] = lpResult.tsTimestamp(xfsLine);
                     dataRows[usBinNumber]["error"] = lpResult.hResult(xfsLine);

                     dataRows[usBinNumber]["number"] = binInfo.usBinNumbers[i];
                     dataRows[usBinNumber]["type"] = binInfo.fwTypes[i];
                     dataRows[usBinNumber]["mediatype"] = binInfo.wMediaTypes[i];
                     dataRows[usBinNumber]["name"] = binInfo.lpstrBinIDs[i];
                     dataRows[usBinNumber]["maxitems"] = binInfo.ulMaximumItems[i];
                     dataRows[usBinNumber]["maxretracts"] = binInfo.ulMaximumRetractOperations[i];
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_MEDIA_BIN_INFO Summary Table Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
                  }
               }

               dTableSet.Tables["Summary"].AcceptChanges();
            }

            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_MEDIA_BIN_INFO cashInfo.lUnitCount '{0}' ", binInfo.lUnitCount));

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

                  mediaBinRow["file"] = _traceFile;
                  mediaBinRow["time"] = lpResult.tsTimestamp(xfsLine);
                  mediaBinRow["error"] = lpResult.hResult(xfsLine);

                  mediaBinRow["count"] = binInfo.ulCounts[i];
                  mediaBinRow["retract"] = binInfo.ulRetractOperations[i];
                  mediaBinRow["status"] = binInfo.usStatuses[i];

                  dTableSet.Tables[tableName].AcceptChanges();
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_MEDIA_BIN_INFO Cash Unit Table Exception {0}, {1}, {2}, {3}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message, i));
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IPM_MEDIA_BIN_INFO Exception : " + e.Message);
         }
      }

      protected void WFS_INF_IPM_TRANSACTION_STATUS(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_TRANSACTION_STATUS tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMTRANSSTATUS transInfo = new WFSIPMTRANSSTATUS(ctx);

            try
            {
               transInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_INF_IPM_TRANSACTION_STATUS Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

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
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_INF_IPM_TRANSACTION_STATUS Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_MEDIA_IN(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_MEDIA_IN tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMMEDIAIN mediaIn = new WFSIPMMEDIAIN(ctx);

            try
            {
               mediaIn.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_MEDIA_IN Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "ready";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_MEDIA_IN Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_MEDIA_IN_END(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_MEDIA_IN_END tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMMEDIAINEND mediaInEnd = new WFSIPMMEDIAINEND(ctx);

            try
            {
               mediaInEnd.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_MEDIA_IN_END Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "media in end";
            depRow["itemsreturned"] = mediaInEnd.usItemsReturned;
            depRow["totalrefsd"] = mediaInEnd.usItemsRefused;
            depRow["totlbnchrefsd"] = mediaInEnd.usBunchesRefused;

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_MEDIA_IN_END Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_MEDIA_IN_ROLLBACK(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_MEDIA_IN_ROLLBACK tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "rollback";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_MEDIA_IN_ROLLBACK Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_PRESENT_MEDIA(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_PRESENT_MEDIA tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "present media";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_PRESENT_MEDIA Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_RETRACT_MEDIA(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_RETRACT_MEDIA tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMRETRACTMEDIAOUT mediaRetract = new WFSIPMRETRACTMEDIAOUT(ctx);

            try
            {
               mediaRetract.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_RETRACT_MEDIA Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "retract";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_RETRACT_MEDIA Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_RESET(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_RESET tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "reset";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_RESET Exception : " + e.Message);
         }
      }

      protected void WFS_CMD_IPM_EXPEL_MEDIA(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_CMD_IPM_EXPEL_MEDIA tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "expel";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_CMD_IPM_EXPEL_MEDIA Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAINSERTED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_IPM_MEDIAINSERTED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "inserted";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAINSERTED Exception : " + e.Message);
         }
      }

      protected void WFS_USRE_IPM_MEDIABINTHRESHOLD(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_IPM_MEDIABINTHRESHOLD tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMMEDIABININFO binInfo = new WFSIPMMEDIABININFO(ctx);

            try
            {
               binInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_IPM_MEDIABINTHRESHOLD Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }


            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "threshold";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_USRE_IPM_MEDIABINTHRESHOLD Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_IPM_MEDIABININFOCHANGED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_IPM_MEDIABININFOCHANGED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMMEDIABININFO binInfo = new WFSIPMMEDIABININFO(ctx);

            try
            {
               binInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_IPM_MEDIABINTHRESHOLD Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "info changed";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_IPM_MEDIABININFOCHANGED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIABINERROR(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_IPM_MEDIABINERROR tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMEDIABINERROR binInfo = new WFSIPMEDIABINERROR(ctx);

            try
            {
               binInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_USRE_IPM_MEDIABINTHRESHOLD Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "error";
            depRow["reason"] = binInfo.wFailure; 

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIABINERROR Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_IPM_MEDIATAKEN(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_IPM_MEDIATAKEN tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "taken";

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_IPM_MEDIATAKEN Exception : " + e.Message);
         }
      }

      protected void WFS_SRVE_IPM_MEDIADETECTED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_SRVE_IPM_MEDIADETECTED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "detected";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_SRVE_IPM_MEDIADETECTED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAPRESENTED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_IPM_MEDIAPRESENTED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "presented";

            dTableSet.Tables["Deposit"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAPRESENTED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAREFUSED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_IPM_MEDIAREFUSED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMMEDIAREFUSED binInfo = new WFSIPMMEDIAREFUSED(ctx);

            try
            {
               binInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_IPM_MEDIAREFUSED Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "refused";
            depRow["reason"] = binInfo.wReason; 

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAREFUSED Exception : " + e.Message);
         }
      }

      protected void WFS_EXEE_IPM_MEDIAREJECTED(string xfsLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_IPM_MEDIAREJECTED tracefile '{0}' timestamp '{1}", _traceFile, lpResult.tsTimestamp(xfsLine)));

            WFSIPMMEDIAREJECTED binInfo = new WFSIPMMEDIAREJECTED(ctx);

            try
            {
               binInfo.Initialize(xfsLine);
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine(String.Format("WFS_EXEE_IPM_MEDIAREFUSED Assignment Exception {0}. {1}, {2}", _traceFile, lpResult.tsTimestamp(xfsLine), e.Message));
            }

            DataRow depRow = dTableSet.Tables["Deposit"].Rows.Add();

            depRow["file"] = _traceFile;
            depRow["time"] = lpResult.tsTimestamp(xfsLine);
            depRow["error"] = lpResult.hResult(xfsLine);

            depRow["trans"] = "rejected";
            depRow["reason"] = binInfo.wReason;

            dTableSet.Tables["Deposit"].AcceptChanges();

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("WFS_EXEE_IPM_MEDIAREJECTED Exception : " + e.Message);
         }
      }
   }
}

