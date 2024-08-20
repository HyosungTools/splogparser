using System;
using System.Data;
using Contract;
using Impl;
using LogLineHandler;

namespace TCRView
{
   internal class TransTable : BaseTable
   {
      string TRANS;
      string TELLERID;
      string RESULT;
      string ERRORCODE;
      string USD0;
      string USD1;
      string USD2;
      string USD5;
      string USD10;
      string USD20;
      string USD50;
      string USD100;
      string AMOUNT;
      string BALANCE;

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public TransTable(IContext ctx, string viewName) : base(ctx, viewName)
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
            if (logLine is TCRLogLine tcrLogLine)
            {
               switch (tcrLogLine.tcrType)
               {
                  case TCRLogType.TCR_DEP_TELLERID:
                     {
                        RESET_TRANS(tcrLogLine, "DEP");
                        break;
                     }
                  case TCRLogType.TCR_WD_TELLERID:
                     {
                        RESET_TRANS(tcrLogLine, "WD");
                        break;
                     }
                  case TCRLogType.TCR_DEP_RESULT:
                  case TCRLogType.TCR_WD_RESULT:
                     {
                        if (tcrLogLine is TCRLogLineWithField tcrLogLineWithField)
                        {
                           RESULT = tcrLogLineWithField.field;
                        }
                        break;
                     }
                  case TCRLogType.TCR_DEP_ERRORCODE:
                  case TCRLogType.TCR_WD_ERRORCODE:
                     {
                        if (tcrLogLine is TCRLogLineWithField tcrLogLineWithField)
                        {
                           ERRORCODE = tcrLogLineWithField.field;
                        }
                        break;
                     }
                  case TCRLogType.TCR_DEP_CASHDEPOSITED:
                  case TCRLogType.TCR_WD_CASHDISPENSED:
                     {
                        if (tcrLogLine is TCRLogLineCash tcrLogLineCash)
                        {
                           USD0 = tcrLogLineCash.USD0;
                           USD1 = tcrLogLineCash.USD1;
                           USD2 = tcrLogLineCash.USD2;
                           USD5 = tcrLogLineCash.USD5;
                           USD10 = tcrLogLineCash.USD10;
                           USD20 = tcrLogLineCash.USD20;
                           USD50 = tcrLogLineCash.USD50;
                           USD100 = tcrLogLineCash.USD100;
                        }
                        break;
                     }
                  case TCRLogType.TCR_DEP_AMOUNT:
                  case TCRLogType.TCR_WD_AMOUNT:
                     {
                        if (tcrLogLine is TCRLogLineWithField tcrLogLineWithField)
                        {
                           AMOUNT = tcrLogLineWithField.field;
                        }
                        break;
                     }
                  case TCRLogType.TCR_DEP_BALANCE:
                  case TCRLogType.TCR_WD_BALANCE:
                     {

                        if (tcrLogLine is TCRLogLineWithField tcrLogLineWithField)
                        {
                           BALANCE = tcrLogLineWithField.field;
                        }

                        ADDROW(tcrLogLine);
                        break;
                     }

                  default:
                     break;
               }
            }
         }
         catch (Exception e)
         {
            ctx.LogWriteLine("TCRTable.ProcessRow EXCEPTION:" + e.Message);
         }
      }

      protected void RESET_TRANS(TCRLogLine tcrLogLine, string trans)
      {
         try
         {
            TRANS = trans;
            TELLERID = string.Empty;
            RESULT = string.Empty;
            ERRORCODE = string.Empty;
            USD0 = string.Empty;
            USD1 = string.Empty;
            USD2 = string.Empty;
            USD5 = string.Empty;
            USD10 = string.Empty;
            USD20 = string.Empty;
            USD50 = string.Empty;
            USD100 = string.Empty;
            AMOUNT = string.Empty;
            BALANCE = string.Empty;

            if (tcrLogLine is TCRLogLineWithField tcrLogLineWithField)
            {
               TELLERID = tcrLogLineWithField.field;
            }

         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("UPDATE_TCR Exception : " + e.Message);
         }

         return;
      }

      protected void ADDROW(TCRLogLine tcrLogLine)
      {
         try
         {
            try
            {
               DataRow dataRow = dTableSet.Tables["Trans"].Rows.Add();

               dataRow["file"] = tcrLogLine.LogFile;
               dataRow["time"] = tcrLogLine.Timestamp;
               dataRow["error"] = tcrLogLine.HResult;

               dataRow["trans"] = TRANS;
               dataRow["tellerid"] = TELLERID;
               dataRow["result"] = RESULT;
               dataRow["errcode"] = ERRORCODE;
               dataRow["USD0"] = USD0;
               dataRow["USD1"] = USD1;
               dataRow["USD2"] = USD2;
               dataRow["USD5"] = USD5;
               dataRow["USD10"] = USD10;
               dataRow["USD20"] = USD20;
               dataRow["USD50"] = USD50;
               dataRow["USD100"] = USD100;
               dataRow["amt"] = AMOUNT;
               dataRow["bal"] = BALANCE;

               dTableSet.Tables["Trans"].AcceptChanges();
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("UPDATE_SS Exception : " + e.Message);
            }


         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("TransTable.ADDROW : " + e.Message);
         }

         return;
      }
   }
}


