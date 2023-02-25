using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impl
{
   public static class DataTableOps
   {
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
