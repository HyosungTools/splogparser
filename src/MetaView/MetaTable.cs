using Contract;
using Impl;
using System;
using System.Data;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

namespace MetaView
{
   class MetaTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public MetaTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Reads the DataTables into a DataSet, using all the available View Xml files in the working folder. 
      /// </summary>
      /// <returns>true if the read is successful, false otherwise. </returns>      
      public DataSet ReadXmlFilesForQuery()
      {
         DataSet combinedDS = new DataSet();

         string processingFile = string.Empty;

         // reads the MetaView.xsd and adds its tables to the return value DataSet
         // base.ReadXmlFile();

         try
         {
            // Initialize step - normally no files (unless a crash left .xsd/.xml in the working directory)
            // Analyze, WriteExcel steps - there are .xsd/.xml created this and other Views
            // CleanUp step - there may be other .xsd/.xml created by other views, but at minimum there will be one created by this view
            string[] filenames = ctx.ioProvider.GetFiles(ctx.WorkFolder, "*.xsd", false);

            foreach (string xsdfile in filenames)
            {
               if (xsdfile.Contains(viewName))
               {
                  continue;
               }

               string xmlfile = xsdfile.Replace(".xsd", ".xml");

               ctx.ConsoleWriteLogLine(String.Format("MetaTable loading Table : {0} from {1}", ctx.ioProvider.GetFileName(xsdfile), ctx.ioProvider.GetFileName(xmlfile)));

               processingFile = xsdfile;
               combinedDS.ReadXmlSchema(xsdfile);

               processingFile = xmlfile;

               // Each of the Xml files has a <Messages> table with a constraint requiring unique values.  The table is written by BaseTable.AddEnglishToTable().
               // Simply adding the new tables into the dTableSet which already contains a Messages table throws a constraint exception due the duplicates in
               // the second incoming Messages table.  Merge instead of directly adding.
               DataSet nextXml = new DataSet();
               nextXml.ReadXml(xmlfile);

               ctx.ConsoleWriteLogLine(String.Format("Merging Table : {0}", ctx.ioProvider.GetFileNameWithoutExtension(xmlfile)));
               combinedDS.Merge(nextXml);
            }

            // The Tables we now have available to us
            foreach (System.Data.DataTable dTable in combinedDS.Tables)
            {
               ctx.ConsoleWriteLogLine(String.Format("MetaTable loaded Table : {0} with {1} rows", dTable.TableName, dTable.Rows.Count));
            }
         }
         catch (InvalidOperationException ex)
         {
            //  a column type in the DataRow being written/read implements IDynamicMetaObjectProvider 
            // and does not implement IXmlSerializable.
            ctx.ConsoleWriteLogLine("Exception (InvalidOperationException): " + ex.Message);
         }
         catch (Exception ex)
         {
            // unknown exception 
            ctx.ConsoleWriteLogLine($"MetaTable.ReadXmlFilesForQuery EXCEPTION: processing file {processingFile}, {ex.Message}");
         }

         return combinedDS;
      }

      /*
      protected void MetaLogLine(MetaLine bhdLine)
      {
         try
         {
            // after changing MetaView.xsd, Clean the solution and rebuild to distribute the .xsd to the /dist folder

            // columns in the same order as the Messages table in MetaView.xsd

            // this gets very slow when the table gets very large > 10000
            // pre-allocate the table rows???
            // when gets too big, export to Excel and clear the table?
            // filter what is written to the table?

            string tableName = "BeeHDMessages";

            // add the TIME ADJUSTMENT column after the timestamp field
            long sheetRow = dTableSet.Tables[tableName].Rows.Count + 2;   // Convert.ToInt64(bhdLine.lineNumber) + 1;
            string timeAdjustmentFormula = "=rowcol+TIMEADJUSTMENT";

            DataRow dataRow = dTableSet.Tables[tableName].Rows.Add();

            dataRow["tracefile"] = bhdLine.LogFile;
            dataRow["linenumber"] = bhdLine.lineNumber;
            dataRow["time"] = bhdLine.Timestamp;
            dataRow["adjustedtime"] = timeAdjustmentFormula;

            // TODO - don't include IsRecognized failures because MetaLine is set up already with 'interesting' and 'non-interesting' tags.  Might
            //        need to adjust later, if there truly is an interesting pattern but the regex fails for a new type of log line.
            if (isOptionIncludePayload) // || !bhdLine.IsRecognized)
            {
               dataRow["Payload"] = bhdLine.logLine;
            }

            dataRow["source"] = bhdLine.source;
            dataRow["interest"] = bhdLine.interestingTag;
            dataRow["level"] = bhdLine.logLevel;
            dataRow["head"] = bhdLine.headTag;
            dataRow["thread"] = bhdLine.threadId;
            dataRow["class"] = bhdLine.className;
            dataRow["method"] = bhdLine.methodName;
            dataRow["direction"] = bhdLine.direction;
            dataRow["protocol"] = bhdLine.protocol;
            dataRow["analysis"] = bhdLine.analysis;
            dataRow["endpoints"] = bhdLine.endPoints;
            dataRow["callstate"] = bhdLine.callState.ToString();
            dataRow["audstate"] = bhdLine.audioState.ToString();
            dataRow["vidstate"] = bhdLine.videoState.ToString();
            dataRow["devstate"] = bhdLine.deviceStateChange;
            dataRow["spkstate"] = bhdLine.speakerState;
            dataRow["micstate"] = bhdLine.microphoneState;
            dataRow["camstate"] = bhdLine.cameraState;
            dataRow["audtxstate"] = bhdLine.audioTransmitterState;
            dataRow["audrxstate"] = bhdLine.audioReceiverState;
            dataRow["vidtxstate"] = bhdLine.videoTransmitterState;
            dataRow["vidrxstate"] = bhdLine.videoReceiverState;
            dataRow["timedelta"] = bhdLine.timeSinceLastMessage;
            dataRow["msgheader"] = bhdLine.msgHeader;
            dataRow["cseqheader"] = bhdLine.cseqHeader;
            dataRow["callidheader"] = bhdLine.callIdHeader;
            dataRow["transidheader"] = bhdLine.transactionId;
            dataRow["contact"] = bhdLine.contact;
            dataRow["originator"] = bhdLine.sessionOriginator;
            dataRow["rportoption"] = bhdLine.rportOption;
            dataRow["msgnote"] = bhdLine.msgNote;
            dataRow["expectnote"] = bhdLine.msgExpectNote;
            dataRow["sumnote"] = bhdLine.summaryNote;
            dataRow["audsent"] = bhdLine.audioSent.ToString();
            dataRow["audreceived"] = bhdLine.audioReceived.ToString();
            dataRow["audlost"] = bhdLine.audioLost;
            dataRow["audjitter"] = bhdLine.audioJitter;
            dataRow["vidsent"] = bhdLine.videoSent.ToString();
            dataRow["vidreceived"] = bhdLine.videoReceived.ToString();
            dataRow["vidlost"] = bhdLine.videoLost;
            dataRow["vidjitter"] = bhdLine.videoJitter;
            dataRow["micdatacount"] = bhdLine.microphoneDataCount;
            dataRow["camdatacount"] = bhdLine.cameraDataCount;

            // dTableSet.Tables[tableName].AcceptChanges();
         }
         catch (NullReferenceException e)
         {
            ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDTable.MetaLogLine : " + e.Message);
            ctx.ConsoleWriteLogLine("Check that MetaView.xsd exists and contains a valid \"Message\" table definition.");
            throw;
         }
         catch (System.OutOfMemoryException e)
         {
            ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDTable.MetaLogLine : " + e.Message);
            ctx.ConsoleWriteLogLine("Too many lines to process - split the input logs into smaller files or process one file at a time.");
            throw;
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(">>>EXCEPTION BeeHDTable.MetaLogLine : " + e.Message);
         }
      }
      */

   }
}


