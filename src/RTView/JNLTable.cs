using System;
using System.Collections.Generic;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace JNLView
{
   internal class JNLTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public JNLTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;
      }

      public override void PostProcess()
      {
         string tableName = string.Empty;

         base.PostProcess();
         return;
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         return base.WriteExcelFile();
      }


      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(ILogLine logLine)
      {
         try
         {
            if (logLine is RTLine jnlLogLine)
            {
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("JNLTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }
   }
}
