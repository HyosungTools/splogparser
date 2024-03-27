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
      /// Gets a DataSet containing all view Xml files in the working folder. 
      /// </summary>
      /// <returns>A dataset containing tables.</returns>      
      public DataSet ReadXmlFilesForQuery()
      {
         DataSet combinedDS = new DataSet();

         string processingFile = string.Empty;

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
   }
}


