using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace XmlParamsView
{
   class XmlParamTable : BaseTable
   {
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public XmlParamTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
      }

      /// <summary>
      /// Prep the tables for Excel 
      /// </summary>
      /// <returns>true if the write was successful</returns>
      public override bool WriteExcelFile()
      {
         string tableName = string.Empty;

         // X M L P A R A M  T A B L E

         tableName = "XmlParam";
         ctx.ConsoleWriteLogLine(String.Format("{0} : WriteExcel", this.viewName));

         // Remove duplicate rows - use distinctTable going forward
         ctx.ConsoleWriteLogLine(String.Format("{0} : Remove Duplicate Rows", this.viewName));
         DataTable distinctTable = dTableSet.Tables[tableName].DefaultView.ToTable(true);
         distinctTable.TableName = tableName;

         ctx.ConsoleWriteLogLine("DataTable: " + distinctTable.TableName);
         ctx.ConsoleWriteLogLine("DataTable: " + distinctTable.TableName + " has " + distinctTable.Rows.Count.ToString() + " rows.");
         if (distinctTable.Rows.Count == 0)
         {
            // if there are no rows, dont create a worksheet
            return true;
         }

         string excelFileName = ctx.WorkFolder + "\\" + Path.GetFileNameWithoutExtension(ctx.ZipFileName) + ctx.opts.Suffix() + ".xlsx";
         Console.WriteLine("Write DataTable to Excel:" + excelFileName);

         // create Excel 
         ctx.ConsoleWriteLogLine("Create Excel..." + excelFileName);
         Excel.Application excelApp = new Excel.Application
         {
            Visible = false,
            DisplayAlerts = false
         };

         ctx.ConsoleWriteLogLine("Instantiate excel objects (application, workbook, worksheets)...");
         Excel.Workbook activeBook;

         if (File.Exists(excelFileName))
         {
            // open existing
            ctx.ConsoleWriteLogLine("Opening existing workbook...");
            activeBook = excelApp.Workbooks.Open(excelFileName);
         }
         else
         {
            // create new 
            ctx.ConsoleWriteLogLine("Creating new workbook...");
            activeBook = excelApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
         }

         try
         {
            // instantiate excel objects (application, workbook, worksheets)
            ctx.ConsoleWriteLogLine("Instantiate Excel objects...");
            Excel._Worksheet activeSheet = (Excel._Worksheet)activeBook.Sheets.Add(Before: activeBook.Sheets[activeBook.Sheets.Count]);
            activeSheet.Activate();
            try
            {
               activeSheet.Name = distinctTable.TableName;
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine("Exception in WriteExcelFile :" + e.Message);
               ctx.ConsoleWriteLogLine("Exception in WriteExcelFile did not like dTable.TableName : " + distinctTable.TableName);
               activeSheet.Name = distinctTable.TableName + RandomString(3);
               ctx.ConsoleWriteLogLine("Renamed to : " + activeSheet.Name);
            }

            // add column headers 
            ctx.ConsoleWriteLogLine("XmlParam Add column headers...");

            int rowOffset = 1; // offset for derived data and links
            int colOffset = 1; // offset for derived data and links

            // add a first column (A1) for the XmlParam
            ctx.ConsoleWriteLogLine(String.Format("XmlParam Add a first column for the XmlParams rowOffset = {0}, colOffset = {1}", rowOffset, colOffset));
            activeSheet.Cells[rowOffset, colOffset] = "XmlParam";

            colOffset++;

            // add a column (B1, C1, D1, ...) for each row of the data table - these are the xml files in load order. 
            ctx.ConsoleWriteLogLine(String.Format("Add a column for each row of the data table rowOffset = {0}, colOffset = {1}", rowOffset, colOffset));
            for (int i = 0; i < distinctTable.Rows.Count; i++)
            {
               ctx.ConsoleWriteLogLine("XmlParam Adding column header: " + distinctTable.Rows[i][0].ToString());
               activeSheet.Cells[1, colOffset + i] = distinctTable.Rows[i][0].ToString();
               activeSheet.Cells[1, colOffset + i].Orientation = 90;
               activeSheet.Cells[1, colOffset + i].ColumnWidth = 2.75;
            }

            rowOffset++;

            // fill in column 1 with the XmlParam values
            // we need to do 'dTable.Columns.Count - 1' and 'dTable.Columns[i + 1]' to skip over 'xpath'
            ctx.ConsoleWriteLogLine(String.Format("Fill in column 1 with the XmlParam values rowOffset = {0}, colOffset = {1}", rowOffset, colOffset));
            for (int i = 0; i < distinctTable.Columns.Count - 1; i++)
            {
               activeSheet.Cells[rowOffset + i, 1] = distinctTable.Columns[i + 1].ColumnName;
            }

            // copy/tranpose the data from dTable to the active sheet
            ctx.ConsoleWriteLogLine(String.Format("copy/tranpose the data from dTable to the active sheet rowOffset = {0}, colOffset = {1}", rowOffset, colOffset));
            ctx.ConsoleWriteLogLine(String.Format("dTable.Columns.Count = {0}, dTable.Rows.Count = {1}", distinctTable.Columns.Count, distinctTable.Rows.Count));

            for (int i = 0; i < distinctTable.Columns.Count - 1; i++)
            {
               for (int j = 0; j < distinctTable.Rows.Count; j++)
               {
                  // copying into Excel takes time. Lets see if we have to do it first. 
                  if (!string.IsNullOrEmpty(distinctTable.Rows[j][i + 1].ToString().Trim()))
                     activeSheet.Cells[rowOffset + i, colOffset + j] = distinctTable.Rows[j][i + 1].ToString().Trim();
               }
            }

            // freeze the top row so that column headers are always visible when scrolling
            activeSheet.Rows[1].RowHeight = 180;
            activeSheet.Application.ActiveWindow.SplitRow = 1;
            activeSheet.Application.ActiveWindow.FreezePanes = true;


            // save the file
            ctx.ConsoleWriteLogLine("Save the file : " + excelFileName);
            Excel.Workbook workBook = excelApp.ActiveWorkbook;
            workBook.SaveAs(excelFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);

         }
         catch (Exception e)
         {
            ctx.LogWriteLine(String.Format("{0}.WriteExcelFile EXCEPTION:", this.viewName) + e.Message);
         }

         // shutdown excel
         ctx.ConsoleWriteLogLine("Shutdown Excel...");
         activeBook.Close();
         excelApp.Quit();

         ctx.ConsoleWriteLogLine("Finished writing Excel.");
         ctx.ConsoleWriteLogLine("Wrote Excel file: " + excelFileName);
         return true;
      }

      /// <summary>
      /// Run prior to the time series processing. 
      /// </summary>
      /// <param name="ctx"></param>
      public override void PreProcess(IContext ctx)
      {
         try
         {
            // Convert to a Run Once routine
            // Visit each of these folders and load each xml file looking for XmlParams

            string[] applicationFolders =
            {
               @"Config\Application",
               @"ConfigModel\Application",
               @"ConfigApplication\Application",
               @"ConfigNetwork\Application",
               @"ConfigCore\Application",
               @"ConfigCustomer\Application",
               @"ConfigDevelopment\Application",
               @"ConfigDownload\Application",
               @"ConfigRuntime\Application"
            };

            // sometimes logs contain multiple zip files so find the first [AP] folder
            string directory = ctx.WorkFolder + "\\" + ctx.SubFolder;
            string appDirectory = "[AP]";

            string[] subdirectories = Directory.GetDirectories(directory, appDirectory, SearchOption.AllDirectories);

            if (subdirectories.Length == 0)
            {
               ctx.ConsoleWriteLogLine(String.Format("XMLPARAM_SETTINGS_CONFIG could not find a [AP] subfolder in '{0}'", ctx.WorkFolder + "\\" + ctx.SubFolder));
               return;
            }

            // We now have an [AP] folder
            string apFolder = subdirectories[0];

            ctx.ConsoleWriteLogLine(String.Format("XMLPARAM_SETTINGS_CONFIG found [AP] subfolder '{0}'", apFolder));

            // For each subdirectory, iterate over each Xml file and load up XmlParams
            foreach (string applFolder in applicationFolders)
            {
               // does the folder exist? 
               if (!ctx.ioProvider.DirExists(apFolder + "\\" + applFolder))
               {
                  ctx.ConsoleWriteLogLine(String.Format("XMLPARAM_SETTINGS_CONFIG folder does not exist : {0}", apFolder + "\\" + applFolder));
                  continue;
               }

               // folder exists
               ctx.ConsoleWriteLogLine(String.Format("XMLPARAM_SETTINGS_CONFIG Folder exist '{0}'", apFolder + "\\" + applFolder));

               // Process each Xml file in the folder
               string[] xmlFiles = ctx.ioProvider.GetFiles(apFolder + "\\" + applFolder, "*.xml");
               ctx.ConsoleWriteLogLine(String.Format("XMLPARAM_SETTINGS_CONFIG found {0} xml files in '{1}'", xmlFiles.Length, apFolder + "\\" + applFolder));
               foreach (string xmlFile in xmlFiles)
               {

                  // We have the xml config file. Create an instance of XmlDocument and load the file. 
                  XmlDocument xmlDoc = new XmlDocument();

                  ctx.ConsoleWriteLogLine(String.Format("XMLPARAM_SETTINGS_CONFIG : Load file '{0}'", xmlFile));
                  xmlDoc.Load(xmlFile);

                  XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                  nsmgr.AddNamespace("ns", "http://www.nh.com/Config");

                  // Select the XmlParam node
                  XmlNodeList xmlParamNodes = xmlDoc.SelectNodes("//ns:XmlParam", nsmgr);

                  List<string> keys = new List<string>();
                  List<string> values = new List<string>();

                  // Record the found elements
                  foreach (XmlNode xmlParamNode in xmlParamNodes)
                  {
                     try
                     {
                        string ancestorName = string.Empty;

                        // Find the first ancestor with the namespace
                        XmlNode ancestorNode = xmlParamNode.SelectSingleNode("ancestor::ns:XmlSection", nsmgr);
                        if (ancestorNode != null)
                        {
                           ancestorName = ancestorNode.Attributes[0].InnerText;
                        }

                        // Extract and log the Key and Value attributes
                        // Attach the ancestor name to add context to the variable
                        string key = ancestorName + "." + xmlParamNode.Attributes["Key"].Value;
                        string value = xmlParamNode.Attributes["Value"].Value;

                        // If the column doesnt exist create it now. We only want one occurrance of the 
                        // key (XmlParam) but possibly multiple values - representing overrides
                        if (!dTableSet.Tables["XmlParam"].Columns.Contains(key))
                        {
                           dTableSet.Tables["XmlParam"].Columns.Add(key);
                        }

                        // save key and value for later
                        if (!string.IsNullOrEmpty(key) && key.Length > 0)
                        {
                           keys.Add(key);
                           values.Add(value);
                        }
                     }
                     catch (Exception e)
                     {
                        ctx.ConsoleWriteLogLine("XMLPARAM_SETTINGS_CONFIG Exception : " + e.Message);
                     }
                  }

                  // Convert list of values to an array
                  string[] valueArray = values.ToArray();
                  int i = 0;

                  // create the new row and add the values
                  ctx.ConsoleWriteLogLine(String.Format("XMLPARAM_SETTINGS_CONFIG : Add Row for : {0}", xmlFile));
                  DataRow dataRow = dTableSet.Tables["XmlParam"].Rows.Add();

                  // Use the applFolder and xmlFile for the xpath
                  dataRow["xpath"] = xmlFile.Replace(apFolder, "");
                  foreach (string key in keys)
                  {
                     dataRow[key] = valueArray[i++];
                  }

                  dTableSet.Tables["XmlParam"].AcceptChanges();
               }
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine("XMLPARAM_SETTINGS_CONFIG Exception : " + e.Message);
         }
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public virtual void Process(ILogFileHandler logFileHandler)
      {
         return;
      }
   }
}

