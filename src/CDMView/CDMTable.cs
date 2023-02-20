using Contract;
using Impl;
using System;
using System.Data;

namespace CDMView
{

   internal class CDMTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CDMTable(IContext ctx, string viewName, string schemaName) : base(ctx, viewName, schemaName)
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
            (XFSType xfsType, string xfsLine) result = LogLine.IdentifyLine(logLine);

            switch (result.xfsType)
            {
               case XFSType.WFS_INF_CDM_STATUS:
                  {
                     base.ProcessRow(traceFile, logLine);
                     WFS_IN_CDM_STATUS(result.xfsLine);
                     break;
                  }
               case XFSType.WFS_INF_CDM_CASH_UNIT_INFO:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_INF_CDM_CASH_UNIT_INFO");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_DISPENSE:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_DISPENSE");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_PRESENT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_PRESENT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_REJECT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_REJECT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RETRACT:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RETRACT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RESET:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RESET");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_ITEMSTAKEN:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                  {
                     //ctx.ConsoleWriteLogLine("CDM XFSType.WFS_EXEE_CIM_INPUTREFUSE");
                     break;
                  }
               default: 
                     break;

            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CDMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }
      protected void WFS_IN_CDM_STATUS(string xfsLine)
      {
         try
         {
            DataTable statusTable = null;
            foreach (DataTable table in dTableSet.Tables)
            {
               if (table.TableName == "Status")
               {
                  statusTable = table;
               }
            }
            if (statusTable == null)
            {
               ctx.ConsoleWriteLogLine("ERROR : Could not find status table");
               return;
            }

            DataRow dataRow = statusTable.NewRow();

            dataRow["file"] = _traceFile;
            dataRow["time"] = lpResult.tsTimestamp(xfsLine);
            dataRow["error"] = lpResult.hResult(xfsLine);


            (bool success, string xfsMatch, string subLogLine) result;

            result = _wfs_cmd_status.fwDevice(xfsLine);
            dataRow["status"] = result.xfsMatch;

            result = _wfs_cmd_status.fwDispenser(result.subLogLine);
            dataRow["dispenser"] = result.xfsMatch;
 
            result = _wfs_cmd_status.fwIntermediateStacker(result.subLogLine);
            dataRow["intstack"] = result.xfsMatch;

            result = _wfs_cmd_status.fwShutter(result.subLogLine);
            dataRow["shutter"] = result.xfsMatch;

            result = _wfs_cmd_status.fwPositionStatus(result.subLogLine);
            dataRow["posstatus"] = result.xfsMatch;
 
            result = _wfs_cmd_status.fwTransport(result.subLogLine);
            dataRow["transport"] = result.xfsMatch;

            result = _wfs_cmd_status.fwTransportStatus(result.subLogLine);
            dataRow["transstat"] = result.xfsMatch;
 
            result = _wfs_cmd_status.wDevicePosition(result.subLogLine);
            dataRow["position"] = result.xfsMatch;

            statusTable.Rows.Add(dataRow);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("Exception : " + e.Message);
         }

         return; 
      }
   }
}
