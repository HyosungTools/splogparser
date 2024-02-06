using Contract;
using Impl;
using LogFileHandler;
using LogLineHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using static System.Collections.Specialized.BitVector32;

namespace BHDView
{
   [Export(typeof(IView))]
   public class BHDView : BaseView, IView
   {
      /// <summary>
      /// Set this false once initialzation has completed
      /// </summary>
      bool IsInitializationPass = true;

      /// <summary>
      /// Constructor
      /// </summary>
      BHDView() : base(ParseType.BE, "BHDView") { }


      /// <summary>
      /// Creates an BeeHDTable instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>BeeHDTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         // this is called twice - once for initialization, the second time while calling WriteExcel

         if (IsInitializationPass)
         {
            // delete old XML and XSD files in the working folder - they might be left behind after a crash

            foreach (string sessionFile in ctx.ioProvider.GetFiles(ctx.WorkFolder, "BHDView_Session_*.xml"))
            {
               ctx.ConsoleWriteLogLine($"BHDView: removing old {sessionFile}");
               ctx.ioProvider.Delete(sessionFile);
            }

            foreach (string sessionFile in ctx.ioProvider.GetFiles(ctx.WorkFolder, "BHDView_Session_*.xsd"))
            {
               ctx.ConsoleWriteLogLine($"BHDView: removing old {sessionFile}");
               ctx.ioProvider.Delete(sessionFile);
            }

            IsInitializationPass = false;
         }

         BHDTable installTable = new BHDTable(ctx, viewName);
         installTable.ReadXmlFile();

         return installTable;
      }

      public override void Process(IContext ctx)
      {
         if (ctx.activeHandler == null)
         {
            ctx.ConsoleWriteLogLine("BHDView: active log handler is null.");
            return;
         }

         BELogHandler BElogHandler = (ctx.activeHandler as BELogHandler);

         // process all the log lines
         base.Process(ctx);

         // writes SIP session summary lines to the output log

         ctx.ConsoleWriteLogLine(BElogHandler.LogSummary());

         foreach (string summary in BElogHandler.SessionSummaries())
         {
            ctx.ConsoleWriteLogLine(summary);
         }
      }

      public override void PostProcess(IContext ctx)
      {
         // write the BHDView table(s) built out to file
         // bTable.WriteXmlFile();

         base.PostProcess(ctx);

         if (BELine.SipSessionTables() == null)
         {
            return;
         }

         // write additional XML files, one per SIP session
         try
         {
            foreach (System.Data.DataTable table in BELine.SipSessionTables())
            {
               // writes SIP Session timeline-buckets to XML, for output to Excel worksheets
               // the main XML is output below, after this loop
               //
               // BHDView_Session_0.xsd  (to _n)   - written only if there were SIP sessions
               // BHDView_Session_0.xml
               // BHDView.xsd
               // BHDView.xml

               if (table.Rows.Count == 0)
               {
                  continue;
               }

               // table name contains the day and time - it should be unique
               string outFileName = $"{ctx.WorkFolder}\\{viewName}_{table.TableName}";
               string outFile = string.Empty;

               try
               {
                  outFile = $"{outFileName}.xsd";
                  ctx.ConsoleWriteLogLine(String.Format("Write out SIP Session data schema to '{0}'", outFile));
                  table.WriteXmlSchema(outFile);

                  outFile = $"{outFileName}.xml";
                  ctx.ConsoleWriteLogLine(String.Format("Write out SIP Session data set to '{0}'", outFile));
                  table.WriteXml(outFile, XmlWriteMode.WriteSchema);
               }
               catch (InvalidOperationException ex)
               {
                  //  a column type in the DataRow being written/read implements IDynamicMetaObjectProvider 
                  // and does not implement IXmlSerializable.
                  ctx.ConsoleWriteLogLine($">>>EXCEPTION BeeHDView.PostProcess invalid operation writing {outFile} : " + ex.Message);
               }
               catch (Exception ex)
               {
                  // unknown exception 
                  ctx.ConsoleWriteLogLine($">>>EXCEPTION BeeHDView.PostProcess writing {outFile} : " + ex.Message);
               }
            }
         }
         catch (Exception ex)
         {
            // there are no session summaries
            ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDView.PostProcess Processing SessionSummaries : " + ex.Message);
         }
      }

      public override void WriteExcel(IContext ctx)
      {
         // write the BHDView.xml and all of the Session XML files
         base.WriteExcel(ctx);
      }

      public override void Cleanup(IContext ctx)
      {
         base.Cleanup(ctx);

         // delete the SIP session XML/XSD files
         foreach (System.Data.DataTable table in BELine.SipSessionTables())
         {
            try
            {
               // table name contains the day and time - it should be unique
               string outFileName = $"{ctx.WorkFolder}\\{viewName}_{table.TableName}";

               string outFileToDelete = $"{outFileName}.xsd";
               if (File.Exists(outFileToDelete))
               {
                  ctx.ConsoleWriteLogLine("Deleting file : " + outFileToDelete);
                  File.Delete(outFileToDelete);
               }

               outFileToDelete = $"{outFileName}.xml";
               if (File.Exists(outFileToDelete))
               {
                  ctx.ConsoleWriteLogLine("Deleting file : " + outFileToDelete);
                  File.Delete(outFileToDelete);
               }
            }
            catch (Exception ex)
            {
               // there are no session summaries
               ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDView.Cleanup Processing SipSessionTables : " + ex.Message);
            }
         }

         BELine.ReleaseSipSessionTables();
      }
   }
}

