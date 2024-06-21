using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace A2View
{
   internal class A2Table : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public A2Table(IContext ctx, string viewName) : base(ctx, viewName)
      {
         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = false;
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
         string tableName = string.Empty;

         try
         {
            // A 2   T A B L E

            tableName = "A2";

            // COMPRESS
            string[] columns = new string[] { "time", "error", "nodate", "nopayee", "nocar", "nolar", "nosign", "nocodeline", "noendorse", "amt", "amtscore", "codeline", "codelinescore",
                                              "date", "datescore", "sig", "sigscore", "lar", "larscore", "car", "carscore" };
            RemoveDuplicateRows(tableName, columns);
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION processing table '{0}' error '{1}'", tableName, e.Message));
         }

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
            if (logLine is A2iALine a2LogLine)
            {
               UPDATE_A2iA(a2LogLine);
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("A2Table.ProcessRow EXCEPTION:" + e.Message);
         }
      }


      protected void UPDATE_A2iA(A2iALine a2LogLine)
      {
         try
         {
            ctx.ConsoleWriteLogLine("UPDATE_A2iA");

            DataRow dataRow = dTableSet.Tables["A2"].Rows.Add();

            dataRow["file"] = a2LogLine.LogFile;
            dataRow["time"] = a2LogLine.Timestamp;
            dataRow["error"] = a2LogLine.HResult;

            dataRow["nodate"] = a2LogLine.noDate;
            dataRow["nopayee"] = a2LogLine.noPayeeName;
            dataRow["nocar"] = a2LogLine.noCAR;
            dataRow["nolar"] = a2LogLine.noLAR;
            dataRow["nosign"] = a2LogLine.noSignature;
            dataRow["nocodeline"] = a2LogLine.noCodeline;
            dataRow["noendorse"] = a2LogLine.noPayeeEndorsement;
            dataRow["amt"] = a2LogLine.amount;
            dataRow["amtscore"] = a2LogLine.amountScore;
            dataRow["codeline"] = a2LogLine.codeLine;
            dataRow["codelinescore"] = a2LogLine.codeLineScore;
            dataRow["date"] = a2LogLine.ocrDate;
            dataRow["datescore"] = a2LogLine.ocrDateScore;
            dataRow["sig"] = a2LogLine.signature;
            dataRow["sigscore"] = a2LogLine.signatureScore;
            dataRow["lar"] = a2LogLine.amountLAR;
            dataRow["larscore"] = a2LogLine.amountLARScore;
            dataRow["car"] = a2LogLine.amountCAR;
            dataRow["carscore"] = a2LogLine.amountCARScore;

            dTableSet.Tables["A2"].AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UPDATE_A2iA Exception : " + e.Message);
         }

         return;
      }
   }
}

