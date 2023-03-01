
using System;
using System.Collections.Generic;
using System.Data;


namespace Impl
{
   public static class DataTableOps
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

         DataView dataView = new DataView(dataTable);
         dataView.Sort = sortCriteria;

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
                  lastRow[colName] = string.Empty;
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
   }
}
