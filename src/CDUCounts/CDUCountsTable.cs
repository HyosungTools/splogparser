using System;
using System.Collections.Generic;
using System.Data;
using Contract;
using Impl;
using static Impl.Utilities;

namespace CDUCountsView
{
   /// <summary>
   /// Convert this table from the log files: 
   /// 
   /// cUnitID          LCU00  LCU01   LCU02
   /// cCurrencyID             USD     USD
   /// ulValues         0		 5		   20
   /// 
   /// Into this:
   /// Init Cur    USD5Init Cur   USD20Init Cur
   /// 
   /// </summary>
   class CDUCountsTable : BaseTable
   {
      bool firstParse = true;
      List<string> _columnNames = null;
      List<string> _initialList = null;
      List<string> _currentList = null; 

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <param name="viewName">The (unique) name of the view being created.</param>
      public CDUCountsTable(IContext ctx, string viewName) : base(ctx, viewName)
      {
         this._columnNames = new List<string>();
         this._initialList = new List<string>();
         this._currentList = new List<string>(); 

         // for our view we want '0' to render as ' ' in the worksheet
         zeroAsBlank = true;

         InitDataTable();

      }

      protected override void InitDataTable()
      {
         base.InitDataTable();

      }

      /// <summary>
      /// Process one line from the merged log file. 
      /// </summary>
      /// <param name="logLine">logline from the file</param>
      public override void ProcessRow(string logLine)
      {
         try
         {
            if (string.IsNullOrEmpty(logLine))
            {
               return;
            }

            if (logLine.Contains("ulCashInCount") ||
                logLine.Contains("cSerialNum"))
            {
               return; 
            }

            // This log line is for us if it contains all these markers
            if (!( logLine.Contains("lpResult =") &&
                   logLine.Contains("usTellerID=") &&
                   logLine.Contains("usCount=") &&
                   logLine.Contains("lppList->") &&
                   logLine.Contains("cCurrencyID") &&
                   logLine.Contains("ulValues") &&
                   logLine.Contains("ulInitialCount") &&
                   logLine.Contains("ulCount")))
            {
               return;
            }

            // found one for us
            string subLogLine = logLine;
            //(bool found, string foundStr, string subLogLine) result;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            // isolate the logDate
            logDate = Utilities.GetTimeFromLogLine(logLine);

            if (firstParse)
            {
               // For this class we determine the columns by examining the first log line we encounter
               // look for lines like these:
               //
               // cCurrencyID		   		USD		USD
               // ulValues          0     5        20
               //
               // You now know the logical units. 
               // To add columns to the table. In this case you will add add columns
               // Reject(Initial), Reject(Current), USD5(Initial), USD5(Current), USD20(Initial), USD20(Current). 

               // continue to read lines until you find the cCurrencyID line
               (bool found, string oneLine, string subLogLine) currencyLine = Utilities.FindLine(subLogLine, "cCurrencyID");
               if (!currencyLine.found)
               {
                  // can't continue 
                  ctx.ConsoleWriteLogLine("CDUCountsTable.ProcessRow - Failed to find cCurrencyID: " + logLine.Substring(0, 20));
                  return;
               }

               string currencyRow = currencyLine.oneLine;

               // continue to read lines until you find the ulValues line
               (bool found, string oneLine, string subLogLine) valueLine = Utilities.FindLine(subLogLine, "ulValues");
               if (!valueLine.found)
               {
                  // can't continue 
                  ctx.ConsoleWriteLogLine("CDUCountsTable.ProcessRow - Failed to find ulValues: " + logLine.Substring(0, 20));
                  return;
               }

               string valuesRow = valueLine.oneLine;

               // now create the columns in the table
               List<string> currencyList = new List<string>(currencyRow.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));
               List<string> valuesList = new List<string>(valuesRow.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));

               // trim cCurrencyID and ulValues from the lists
               currencyList.Remove("cCurrencyID");
               valuesList.Remove("ulValues");

               // the currency list should be the same size as the values list, if not, insert until its the same size
               ctx.ConsoleWriteLogLine(string.Format(@"currencyList: {0} valuesList:{1}", currencyList.Count, valuesList.Count));
               int rjCount = 0;
               while (currencyList.Count < valuesList.Count)
               {
                  currencyList.Insert(0, string.Format("Rjct{0}",++rjCount));
                  ctx.ConsoleWriteLogLine(string.Format(@"Adding the column name: Rjct{0}", rjCount));
               }

               string[] currencyArray = currencyList.ToArray();
               string[] valuesArray = valuesList.ToArray();
               string colName; 

               // now add the columns to the table - we want columns for Initial and Current because that's
               // the data we will be reporting (see next block below)
               for (int i = 0; i < currencyArray.Length; i++)
               {
                  colName = currencyArray[i] + (valuesArray[i] == "0" ? " " : valuesArray[i]) + "(Initial)";
                  this.dTable.Columns.Add(colName, typeof(string));
                  _columnNames.Add(colName); 

                  colName = currencyArray[i] + (valuesArray[i] == "0" ? " " : valuesArray[i]) + "(Current)";
                  this.dTable.Columns.Add(colName, typeof(string));
                  _columnNames.Add(colName);

                  _initialList.Add(string.Empty);
                  _currentList.Add(string.Empty);
               }

               firstParse = false;
            }

            // Search for these two lines in the log line: 
            //
            // ulInitialCount    0     2000     6000
            // ulCount           0     1986     5131
            //
            // Use these values to populate the row of the table. 
            (bool found, string oneLine, string subLogLine) initialLine = Utilities.FindLine(subLogLine, "ulInitialCount");
            if (!initialLine.found)
            {
               // can't continue 
               ctx.ConsoleWriteLogLine("CDUCountsTable.ProcessRow - Failed to find ulInitialCount: " + logLine.Substring(0, 20));
               return;
            }

            string initialRow = initialLine.oneLine;

            // continue to read lines until you find the ulValues line
            (bool found, string oneLine, string subLogLine) currentLine = Utilities.FindLine(subLogLine, "ulCount");
            if (!currentLine.found)
            {
               // can't continue 
               ctx.ConsoleWriteLogLine("CDUCountsTable.ProcessRow - Failed to find ulCount: " + logLine.Substring(0, 20));
               return;
            }

            string currentRow = currentLine.oneLine;

            // now create the columns in the table
            List<string> initialList = new List<string>(initialRow.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));
            List<string> currentList = new List<string>(currentRow.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));

            // trim cCurrencyID and ulValues from the lists
            initialList.Remove("ulInitialCount");
            currentList.Remove("ulCount");

            ctx.ConsoleWriteLogLine(
               string.Format(@"_initialList:{0} _currentList:{1} initialList:{2} currentList:{3} _columnNames:{4} ",
                               _initialList.Count, _currentList.Count, initialList.Count, currentList.Count, _columnNames.Count)
               ); 

            //
            if (this._initialList.Count > initialList.Count || this._currentList.Count > currentList.Count)
            {
               // something went wrong, throw this out
               ctx.ConsoleWriteLogLine("Unexpected values");
               return;
            }

            // if the values are identical to the previously stored values, dont bother writing. 
            bool listsAreEqual = this._initialList.Count == initialList.Count && 
                                 this._currentList.Count == currentList.Count;

            if (listsAreEqual)
            {
               for (int i = 0; i < initialList.Count && listsAreEqual; i++)
               {
                  listsAreEqual = listsAreEqual && this._initialList[i] == initialList[i] &&
                                                   this._currentList[i] == currentList[i];
               }
            }

            if (!listsAreEqual)
            {
               string[] _initialArray = this._initialList.ToArray();
               string[] _currentArray = this._currentList.ToArray(); 

               string[] initialArray = initialList.ToArray();
               string[] currentArray = currentList.ToArray();

               DataRow dataRow = dTable.NewRow();

               dataRow["Time"] = logDate;

               // now add the columns to the table - but if the previous value is the same, write and empty string
               // for example, only report Initial Value once because it never changes. 
               string[] columnNameArray = _columnNames.ToArray();
               int columnNameArrayCount = 0;
               for (int i = 0; i < initialArray.Length; i++)
               {
                  if (_initialArray[i] == initialArray[i])
                  {
                     dataRow[columnNameArray[columnNameArrayCount++]] = string.Empty;
                  }
                  else
                  {
                     dataRow[columnNameArray[columnNameArrayCount++]] = initialArray[i];

                  }

                  if (_currentArray[i] == currentArray[i])
                  {
                     dataRow[columnNameArray[columnNameArrayCount++]] = string.Empty;

                  }
                  else
                  {
                     dataRow[columnNameArray[columnNameArrayCount++]] = currentArray[i];

                  }
               }

               dTable.Rows.Add(dataRow);

               // store off these values so we can compare next time. 
               this._initialList = initialList;
               this._currentList = currentList; 
            }
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception : " + e.Message);
         }
      }
   }
}
