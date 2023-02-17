using Contract;
using Impl;
using System;
using System.Collections.Generic;
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
      public CDMTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;

         InitDataTable(viewName);
      }

      /// <summary>
      /// Create a table with a given name, at the same time add columns. 
      /// </summary>
      /// <param name="tableName">name of the table to create</param>
      /// <returns></returns>
      protected override bool InitDataTable(string tableName)
      {
         return true;
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            XFSType xfsType = LogLine.IdentifyLine(logLine);
            switch (xfsType)
            {
               case XFSType.WFS_INF_CDM_STATUS:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_INF_CDM_STATUS");
                     break;
                  }
               case XFSType.WFS_INF_CDM_CASH_UNIT_INFO:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_INF_CDM_CASH_UNIT_INFO");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_DISPENSE:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_DISPENSE");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_PRESENT:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_PRESENT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_REJECT:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_REJECT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RETRACT:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RETRACT");
                     break;
                  }
               case XFSType.WFS_CMD_CDM_RESET:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_CMD_CDM_RESET");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CDM_ITEMSTAKEN:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CDM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_SRVE_CIM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                  {
                     ctx.ConsoleWriteLogLine("CDM XFSType.WFS_EXEE_CIM_INPUTREFUSE");
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
   }
}
