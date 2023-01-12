using System;
using System.Collections.Generic;
using System.Data;
using Contract;
using Impl;
using static Impl.Utilities;

namespace CashIn
{
   class CashInTable : BaseTable
   {

      private bool _firstParse;
      private int _usCount;

      private List<string> _columnNames = new List<string>();
      private List<string> _currentList = new List<string>();

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CashInTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         this._columnNames = new List<string>();
         this._currentList = new List<string>();

         // for our view we want '0' to render as ' ' in the worksheet
         _zeroAsBlank = true;
         _firstParse = true;
         _usCount = 0; 

         InitDataTable();

      }

      /// <summary>
      /// Initialize the Database Table
      /// </summary>
      protected override void InitDataTable()
      {
         base.InitDataTable();
      }

      /// <summary>
      /// Wrapper function to call Utilities.FindByMarker but also handle error logging. 
      /// </summary>
      /// <param name="logLine">the current log line</param>
      /// <param name="mark">the string to search for in the log line</param>
      /// <param name="size">the size of the return string</param>
      /// <returns>string after marker (of length size) or error</returns>
      private (bool found, string foundStr, string subLogLine) Find(string logLine, string mark, int size)
      {
         (bool found, string foundStr, string subLogLine) result;
         result = LogFind.FindByMarker(logLine, mark, size);
         if (!result.found)
         {
            // can't continue 
            ctx.ConsoleWriteLogLine("CashInTable.ProcessRow -  Failed to find '" + mark + "'");
            return (false, string.Empty, logLine);
         }
         return (true, result.foundStr, result.subLogLine);
      }

      /// <summary>
      /// Wrapper function to call Utilities.FindByMarker but also handle return error logging
      /// </summary>
      /// <param name="logLine">the current log line</param>
      /// <param name="mark">string to search for in the log line (start marker)</param>
      /// <param name="endMark"></param>
      /// <returns>substring of logline book-ended by mark and endMark, or error.</returns>
      private (bool found, string foundStr, string subLogLine) Find(string logLine, string mark, string endMark)
      {
         (bool found, string foundStr, string subLogLine) result;
         result = LogFind.FindByMarker(logLine, mark, endMark);
         if (!result.found)
         {
            // can't continue 
            ctx.ConsoleWriteLogLine("CashInTable.ProcessRow - Failed to find '" + mark + "'"); 
            return (false, string.Empty, logLine);
         }
         return (true, result.foundStr, result.subLogLine); 
      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(string traceFile, string logLine)
      {
         try
         {
            if (string.IsNullOrEmpty(logLine))
            {
               return;
            }

            // This log line is for us if it contains all these markers
            if (!(logLine.Contains("u.dwCommandCode = [1303]") && 
                  logLine.Contains("lppCashIn =")))
            {
               return;
            }

            base.ProcessRow(traceFile, logLine);

            string subLogLine = logLine;
            List<string> columnNames = new List<string>();
            List<string> currentList = new List<string>();



            (bool found, string foundStr, string subLogLine) result;

            // search for 'usCount', the number of structures to process
            result = Find(subLogLine, "usCount = [", "]");
            if (!result.found)
            {
               return;
            }
            subLogLine = result.subLogLine; 

            if (_firstParse)
            {
               _usCount = int.Parse(result.foundStr);
            }
            else
            {
               // expect for all log lines usCounts are the same
               if (_usCount != int.Parse(result.foundStr))
               {
                  ctx.ConsoleWriteLogLine(string.Format("unexpected usCount {0} does not equal usCount in logline {1}, ignoring this line.", _usCount, int.Parse(result.foundStr)));
                  return; 
               }
            }
 
            for (int i = 0; i < _usCount; i++)
            {
               // find 'usNumber = [' this is the start of the struct,
               result = Find(subLogLine, "usNumber = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               // find the 'fwType = [' 
               result = Find(subLogLine, "fwType = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               // based on the type and index, create a column prefix. 
               int fwType = int.Parse(result.foundStr);
               string colPrefix = string.Format("{0}.{1}", Utilities.CashInTypeName(fwType), i);

               // find ulCashInCount
               result = Find(subLogLine, "ulCashInCount = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               // create a column title: prefix + 'CashInCnt', also save off the value
               columnNames.Add(string.Format("{0}.CashInCnt", colPrefix));
               currentList.Add(result.foundStr);

               // find ulCount
               result = Find(subLogLine, "ulCount = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               // create a column title: prefix + 'Cnt', also save off the value
               columnNames.Add(string.Format("{0}.Cnt", colPrefix));
               currentList.Add(result.foundStr);

               // find ulMaximum
               result = Find(subLogLine, "ulMaximum = [", "]");
               if (!result.found)
               {
                  return;
               }
               subLogLine = result.subLogLine;

               // create a column title: prefix + 'Cnt', also save off the value
               columnNames.Add(string.Format("{0}.Max", colPrefix));
               currentList.Add(result.foundStr);
            }

            // if this is the first-time parse, add the columns to the table. 
            if (_firstParse)
            {
               // add the columns to the table
               string[] columnNamesArray = columnNames.ToArray();
               try
               {
                  foreach (string colName in columnNamesArray)
                  {
                     ctx.ConsoleWriteLogLine("add column name to the table: " + colName);
                     this.dTable.Columns.Add(colName, typeof(string));
                  }
               }
               catch (Exception e)
               {
                  ctx.ConsoleWriteLogLine("Exception Setting Columns Up: " + e.Message);
                  return; 
               }

               // as part of a first-pass parse, set up _colunNames and re-size _currentList to 
               // be a same sized list of empty strings. 
               _columnNames = columnNames;

               for (int i = 0; i < _columnNames.Count; i++)
               {
                  _currentList.Add(string.Empty); 
               }

               _firstParse = false;
            }

            // the column names generated should be identical to the previous. If that is not the case, error out. 
            if (!Utilities.ListsAreEqual(this._columnNames, columnNames))
            {
               ctx.ConsoleWriteLogLine(string.Format("Unexpected Error. _columnNames ({0}) does not equal columnNames ({1})",
                                                      this._columnNames.ToString(), columnNames.ToString()));
               return; 
            }

            // the number of values generated should match the number of column names. If this is not the case, error out. 
            if (columnNames.Count != currentList.Count)
            {
               ctx.ConsoleWriteLogLine(string.Format("Unexpected Error. columnNames size ({0}) does not equal values list size ({1})",
                                                      this._columnNames.Count, currentList.Count));
               return;
            }

            if (!Utilities.ListsAreEqual(this._currentList, currentList))
            {
               // add the values as a first row
               DataRow dataRow = dTable.NewRow();

               dataRow["File"] = _traceFile; 
               dataRow["Time"] = _logDate;

               string[] _columnNamesArray = _columnNames.ToArray(); 
               string[] _currentValues = _currentList.ToArray();
               string[] currentValues = currentList.ToArray(); 

               for (int i = 0; i < _currentValues.Length; i++)
               {
                  if (_currentValues[i] == currentValues[i])
                  {
                     dataRow[_columnNamesArray[i]] = string.Empty;
                  }
                  else
                  {
                     dataRow[_columnNamesArray[i]] = currentValues[i];
                  }
               }

               dTable.Rows.Add(dataRow);
            }

            _columnNames = columnNames;
            _currentList = currentList; 

         }
         catch (Exception e)
         {
            Console.WriteLine("Exception : " + e.Message);
         }
      }
   }
}

