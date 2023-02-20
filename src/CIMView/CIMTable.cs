using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;

namespace CIMView
{
   internal class CIMTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CIMTable(IContext ctx, string viewName) : base(ctx, viewName)
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
            (XFSType xfsType, string xfsLine) result = LogLine.IdentifyLine(logLine);
            switch (result.xfsType)
            {
               case XFSType.WFS_INF_CIM_STATUS:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_INF_CIM_STATUS");
                     break;
                  }
               case XFSType.WFS_INF_CIM_CASH_UNIT_INFO:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_INF_CIM_CASH_UNIT_INFO");
                     break;
                  }
               case XFSType.WFS_INF_CIM_CASH_IN_STATUS:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_INF_CIM_CASH_IN_STATUS");
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN_START:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_CMD_CIM_CASH_IN_START");
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_CMD_CIM_CASH_IN");
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN_END:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_CMD_CIM_CASH_IN_END");
                     break;
                  }
               case XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_CMD_CIM_CASH_IN_ROLLBACK");
                     break;
                  }
               case XFSType.WFS_CMD_CIM_RETRACT:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_CMD_CIM_RETRACT");
                     break;
                  }
               case XFSType.WFS_CMD_CIM_RESET:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_CMD_CIM_RESET");
                     break;
                  }
               case XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_USRE_CIM_CASHUNITTHRESHOLD");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_SRVE_CIM_CASHUNITINFOCHANGED");
                     break;
                  }
               case XFSType.WFS_SRVE_CIM_ITEMSTAKEN:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_SRVE_CIM_ITEMSTAKEN");
                     break;
                  }
               case XFSType.WFS_EXEE_CIM_INPUTREFUSE:
                  {
                     //ctx.ConsoleWriteLogLine("CIM : XFSType.WFS_EXEE_CIM_INPUTREFUSE");
                     break;
                  }
               default:
                  break;
            };
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("CIMTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }
   }
}
