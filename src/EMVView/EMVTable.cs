using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace EMVView
{
   class EMVTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public EMVTable(IContext ctx, string viewName) : base(ctx, viewName)
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
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("EMVTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }
   }
}
