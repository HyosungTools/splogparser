
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Contract;

namespace Impl
{
   public static class _datatable_ops
   {
      /// <summary>
      /// Given a data view of a table, sorted by time, starting at the bottom, 
      /// examine each row and if its unchanged from the previous, delete it. 
      /// </summary>
      /// <param name="dTable"></param>
      /// <param name="columns"></param>
      /// <returns></returns>
      public static (bool success, string message) DeleteUnchangedRowsInTable(DataTable dataTable, string sortCriteria, string[] columns)
      {
         // determine number of rows
         int rowCount = dataTable.Rows.Count;
         if (rowCount == 0)
         {
            return (true, string.Empty);
         }

         DataView dataView = new DataView(dataTable)
         {
            Sort = sortCriteria
         };

         List<DataRow> deleteRows = new List<DataRow>();
         for (int i = rowCount - 1; i > 0; i--)
         {
            int zeroCount = 0;
            DataRowView lastRow = dataView[i];
            DataRowView secondLastRow = dataView[i - 1];

            if (string.Compare(lastRow["time"].ToString(), secondLastRow["time"].ToString()) < 0)
            {
               // should never happen!
               return (false, String.Format("Last Row time '{0}' is less than second last row time '{1}", lastRow["time"].ToString(), secondLastRow["time"].ToString()));
            }

            foreach (string colName in columns)
            {
               if (lastRow[colName].ToString() == secondLastRow[colName].ToString())
               {
                  zeroCount++;
               }
            }
            if (zeroCount == columns.Length)
            {
               // if we zeroed out everything in the last row just delete it. 
               // delete this row
               deleteRows.Add(lastRow.Row);
            }
         }

         // delete the rows - You cannot modify a collection while you're iterating
         // on it using a foreach statement. The MS document suggests you can call delete within
         // the above loop but in testing you can't
         foreach (DataRow dataRow in deleteRows)
         {
            dataRow.Delete();
         }

         dataTable.AcceptChanges();

         return (true, string.Empty);
      }

      public static (bool success, string message) RemoveDuplicateRows(DataTable dataTable, string[] columns)
      {
         // determine number of rows
         int rowCount = dataTable.Rows.Count;
         if (rowCount == 0)
         {
            return (true, string.Empty);
         }

         // Use LINQ to group and select distinct rows based on specified columns
         DataTable distinctRows = dataTable.AsEnumerable()
             .GroupBy(r => string.Join(",", columns.Select(c => r[c])))
             .Select(g => g.First())
             .CopyToDataTable();

         // Clear the original DataTable
         dataTable.Clear();

         // Import the distinct rows back into the original DataTable
         foreach (DataRow row in distinctRows.Rows)
         {
            dataTable.ImportRow(row);
         }

         return (true, string.Empty);
      }

      /// <summary>
      /// Part of getting ready for Excel, convert numeric values to their English equivalent
      /// </summary>
      /// <param name="dataTable"></param>
      /// <param name="messageTable"></param>
      /// <returns></returns>
      public static (bool success, string message) AddEnglishToTable(IContext ctx, DataTable dataTable, DataTable messageTable, string column, string type)
      {
         // determine number of rows
         int rowCount = dataTable.Rows.Count;
         if (rowCount == 0)
         {
            ctx.ConsoleWriteLogLine(String.Format("DataTable '{0}' has no rows!", dataTable.TableName));
            return (true, string.Empty);
         }

         try
         {
            foreach (DataRow dataRow in dataTable.Rows)
            {
               if (string.IsNullOrEmpty(dataRow[column].ToString()))
                  continue;

               // Create an array for the key values to find.
               object[] findByKeys = new object[2];

               // Set the values of the keys to find.
               findByKeys[0] = type;
               findByKeys[1] = dataRow[column].ToString().Trim();

               DataRow foundRow = messageTable.Rows.Find(findByKeys);
               char[] trimChars = { ',' };
               if (foundRow != null)
               {
                  dataRow[column] = foundRow["brief"];
                  dataRow["comment"] = dataRow["comment"].ToString().TrimStart(trimChars) + "," + foundRow["description"];
               }
            }

            dataTable.AcceptChanges();
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION in Add English : {0}", e.Message));
         }

         return (true, string.Empty);

      }

      /// <summary>
      /// Determine string to add to a data column. 
      /// We only want to record change. 
      /// </summary>
      public static bool AddValueToRowIfChanged(DataRow newRow, DataRow oldRow, string index, string newValue)
      {
         bool dataHasChanged = true;

         // if the old Row is null, add. 
         if (oldRow == null)
         {
            newRow[index] = newValue;
         }
         // if the value is changing, add
         else if (oldRow[index].ToString() != newValue)
         {
            newRow[index] = newValue;
         }
         // otherwise the value is unchanged, set new row value to nothing
         else
         {
            newRow[index] = string.Empty;
            dataHasChanged = false;
         }
         return dataHasChanged;
      }

      public static (bool success, string message) AddMoneyToTable(IContext ctx, DataTable dataTable, DataTable messageTable)
      {
         // determine number of rows
         int rowCount = dataTable.Rows.Count;
         if (rowCount == 0)
         {
            ctx.ConsoleWriteLogLine(String.Format("DataTable '{0}' has no rows!", dataTable.TableName));
            return (true, string.Empty);
         }

         foreach (DataRow dataRow in dataTable.Rows)
         {
            string[] moneyColumns = new string[] { "USD0", "USD1", "USD2", "USD5", "USD10", "USD20", "USD50", "USD100" };

            foreach (string moneyColumn in moneyColumns)
            {
               // Create an array for the key values to find.
               object[] findByKeys = new object[2];

               // Set the values of the keys to find (e.g. {USD5, 5})
               findByKeys[0] = "noteType";
               findByKeys[1] = moneyColumn.Replace("USD", string.Empty);

               DataRow foundRow = messageTable.Rows.Find(findByKeys);
               if (foundRow == null)
               {
                  ctx.ConsoleWriteLogLine(String.Format("Failed to find entry for key '{0}' and '{1}'", findByKeys[0], findByKeys[1]));
                  continue;
               }

               // the descripton field has the list of N columns (e.g. N7,N12,N17)
               string[] nColumns = foundRow["description"].ToString().Split(',');

               // sum the N columns and store the result in the USDx column - rinse repeat
               int total = 0;
               foreach (string nColumn in nColumns)
               {
                  // skip if null or empty
                  if (string.IsNullOrEmpty(dataRow[nColumn].ToString()))
                  {
                     continue;
                  }

                  try
                  {
                     total += int.Parse(dataRow[nColumn].ToString());
                  }
                  catch (Exception e)
                  {
                     ctx.ConsoleWriteLogLine(String.Format("EXCEPTION in AddMoneyToTable : {0}", e.Message));
                  }
               }

               if (total > 0)
               {
                  dataRow[moneyColumn] = total.ToString();
               }
            }
         }

         dataTable.AcceptChanges();

         return (true, string.Empty);

      }

      public static (bool success, string message) AddAmountToTable(IContext ctx, DataTable dataTable)
      {
         // determine number of rows
         int rowCount = dataTable.Rows.Count;
         if (rowCount == 0)
         {
            ctx.ConsoleWriteLogLine(String.Format("DataTable '{0}' has no rows!", dataTable.TableName));
            return (true, string.Empty);
         }

         try
         {
            foreach (DataRow dataRow in dataTable.Rows)
            {
               // only set the amount for 'End' deposits
               if (!dataRow["position"].ToString().Equals("End"))
                  continue;

               string[] moneyColumns = new string[] { "USD0", "USD1", "USD2", "USD5", "USD10", "USD20", "USD50", "USD100" };

               int total = 0;
               foreach (string moneyColumn in moneyColumns)
               {
                  if (string.IsNullOrEmpty(dataRow[moneyColumn].ToString()))
                     continue;

                  total += int.Parse(dataRow[moneyColumn].ToString()) * int.Parse(moneyColumn.Replace("USD", string.Empty));
               }
               dataRow["amount"] = total.ToString();
            }
         }
         catch (Exception e)
         {
            ctx.ConsoleWriteLogLine(String.Format("EXCEPTION Setting amount deposited: {0}", e.Message));
         }

         dataTable.AcceptChanges();

         return (true, string.Empty);

      }

      public static DataTable MergeTable(DataTable originalDt)
      {
         // Clone the original DataTable structure for the result
         DataTable mergedDt = originalDt.Clone();

         // Group rows by 'file' and 'time' (use anonymous type for key)
         var groups = originalDt.AsEnumerable()
             .GroupBy(row => new
             {
                File = row["file"],
                Time = row["time"]
             });

         // Iterate through each group and merge values into a new row
         foreach (var group in groups)
         {
            DataRow newRow = mergedDt.NewRow();
            newRow["file"] = group.Key.File;
            newRow["time"] = group.Key.Time;

            // Combine values from the group, overwriting only if non-null
            // Assumes no conflicts; takes the last non-null value per column
            foreach (var row in group)
            {
               if (!row.IsNull("error") && row["error"] != DBNull.Value)
               {
                  newRow["error"] = row["error"];
               }
               if (!row.IsNull("status") && row["status"] != DBNull.Value)
               {
                  newRow["status"] = row["status"];
               }
               if (!row.IsNull("count") && row["count"] != DBNull.Value)
               {
                  newRow["count"] = row["count"];
               }
               if (!row.IsNull("reject") && row["reject"] != DBNull.Value)
               {
                  newRow["reject"] = row["reject"];
               }
               if (!row.IsNull("comment") && row["comment"] != DBNull.Value)
               {
                  newRow["comment"] = row["comment"];
               }
            }

            mergedDt.Rows.Add(newRow);
         }

         return mergedDt;
      }
   }
}
