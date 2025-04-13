using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace CardView
{
   class CardTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CardTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is APLine apLogLine)
            {
               switch (apLogLine.apType)
               {
                  /* mode */
                  case APLogType.APLOG_CARD_OPEN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "status", "open");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_CLOSE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "status", "close");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIAPRESENT:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "media", "present");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIANOTPRESENT:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "media", "not present");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIAINSERTED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "media", "inserted");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_READAVAILABLERAWDATA:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField apLineField = (APLineField)apLogLine;
                           APLINE(apLineField, "readraw", apLineField.field);
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONREADCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "readraw", "read complete");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONEJECTCOMPLETE:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "media", "ejected");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_ONMEDIAREMOVED:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLine)
                        {
                           APLine apLine = (APLine)apLogLine;
                           APLINE(apLine, "media", "removed");
                        }
                        break;
                     }
                  case APLogType.APLOG_CARD_PAN:
                     {
                        base.ProcessRow(logLine);
                        if (apLogLine is APLineField)
                        {
                           APLineField apLineField = (APLineField)apLogLine;
                           APLINE(apLineField, "pan", apLineField.field);
                        }
                        break;
                     }
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CardTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void APLINE(APLine lineField, string columnName, string columnValue)
      {
         try
         {
            DataRow dataRow = dTableSet.Tables["Card"].Rows.Add();

            dataRow["file"] = lineField.LogFile;
            dataRow["time"] = lineField.Timestamp;
            dataRow["error"] = lineField.HResult;

            dataRow["status"] = string.Empty;
            dataRow["media"] = string.Empty;
            dataRow["readraw"] = string.Empty;
            dataRow["comment"] = string.Empty;

            dataRow[columnName] = columnValue;

            dTableSet.Tables["Card"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("APLINE Exception : " + e.Message);
         }
      }
   }
}
