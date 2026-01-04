using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WinCEJournalView
{
   /// <summary>
   /// Represents hourly dispense data for a single day.
   /// </summary>
   public class HourlyDispense
   {
      public DateTime Date { get; set; }
      public int Hour { get; set; }
      public decimal HourlyAmount { get; set; }
      public decimal CumulativeAmount { get; set; }
      public int TransactionCount { get; set; }

      public string HourLabel => $"{Hour:D2}:00";
   }

   /// <summary>
   /// Represents a full day's worth of hourly dispense data.
   /// </summary>
   public class DailyDispenseData
   {
      public DateTime Date { get; set; }
      public List<HourlyDispense> HourlyData { get; set; }
      public decimal TotalDispensed { get; set; }
      public int TotalTransactions { get; set; }

      public DailyDispenseData(DateTime date)
      {
         Date = date;
         HourlyData = new List<HourlyDispense>();
      }
   }

   /// <summary>
   /// Analyzes TransactionDetails data to calculate cash dispenses by hour.
   /// Produces cumulative hourly totals for bar chart visualization.
   /// </summary>
   public class CashDispenseAnalyzer
   {
      private List<DailyDispenseData> _results;

      /// <summary>
      /// Analysis results - list of daily dispense data.
      /// </summary>
      public List<DailyDispenseData> Results => _results;

      /// <summary>
      /// Analyze the TransactionDetails DataTable to calculate hourly dispense totals.
      /// </summary>
      /// <param name="transactionDetailsTable">The TransactionDetails DataTable</param>
      public void Analyze(DataTable transactionDetailsTable)
      {
         _results = new List<DailyDispenseData>();

         if (transactionDetailsTable == null || transactionDetailsTable.Rows.Count == 0)
         {
            return;
         }

         // Extract all transactions with time and dispensed amount
         var transactions = ExtractTransactions(transactionDetailsTable);
         if (transactions.Count == 0)
         {
            return;
         }

         // Group by date
         var transactionsByDate = transactions
            .GroupBy(t => t.Time.Date)
            .OrderBy(g => g.Key);

         foreach (var dateGroup in transactionsByDate)
         {
            DateTime date = dateGroup.Key;
            var dailyData = new DailyDispenseData(date);

            // Initialize all 24 hours
            var hourlyAmounts = new decimal[24];
            var hourlyCounts = new int[24];

            // Sum dispenses for each hour
            foreach (var transaction in dateGroup)
            {
               int hour = transaction.Time.Hour;
               hourlyAmounts[hour] += transaction.DispensedAmount;
               hourlyCounts[hour]++;
            }

            // Build cumulative totals
            decimal cumulative = 0;
            for (int hour = 0; hour < 24; hour++)
            {
               cumulative += hourlyAmounts[hour];

               // Only add hours that have activity or are after first activity
               if (hourlyAmounts[hour] > 0 || cumulative > 0)
               {
                  dailyData.HourlyData.Add(new HourlyDispense
                  {
                     Date = date,
                     Hour = hour,
                     HourlyAmount = hourlyAmounts[hour],
                     CumulativeAmount = cumulative,
                     TransactionCount = hourlyCounts[hour]
                  });
               }
            }

            dailyData.TotalDispensed = cumulative;
            dailyData.TotalTransactions = dateGroup.Count();

            _results.Add(dailyData);
         }
      }

      /// <summary>
      /// Populate a DataTable with the analysis results (for XML output).
      /// </summary>
      /// <param name="resultsTable">Target DataTable</param>
      public void PopulateResultsTable(DataTable resultsTable)
      {
         if (_results == null || resultsTable == null)
            return;

         foreach (var daily in _results)
         {
            foreach (var hourly in daily.HourlyData)
            {
               DataRow row = resultsTable.NewRow();
               row["file"] = string.Empty;
               row["time"] = daily.Date.AddHours(hourly.Hour).ToString("MM/dd/yyyy HH:mm:ss");
               row["Date"] = daily.Date.ToString("yyyy-MM-dd");
               row["Hour"] = hourly.Hour.ToString();
               row["HourLabel"] = hourly.HourLabel;
               row["HourlyAmount"] = hourly.HourlyAmount.ToString("F2");
               row["CumulativeAmount"] = hourly.CumulativeAmount.ToString("F2");
               row["TransactionCount"] = hourly.TransactionCount.ToString();
               resultsTable.Rows.Add(row);
            }
         }

         resultsTable.AcceptChanges();
      }

      #region Private Methods

      private List<DispenseTransaction> ExtractTransactions(DataTable transactionDetailsTable)
      {
         var transactions = new List<DispenseTransaction>();

         if (!transactionDetailsTable.Columns.Contains("time") ||
             !transactionDetailsTable.Columns.Contains("dispensedamt"))
         {
            return transactions;
         }

         foreach (DataRow row in transactionDetailsTable.Rows)
         {
            if (row["time"] == DBNull.Value || row["dispensedamt"] == DBNull.Value)
               continue;

            // Parse time
            DateTime time;
            string timeStr = row["time"].ToString();
            if (!DateTime.TryParse(timeStr, out time))
               continue;

            // Parse dispensed amount (format: "$20.00")
            string amountStr = row["dispensedamt"].ToString();
            decimal amount = ParseAmount(amountStr);

            if (amount > 0)
            {
               transactions.Add(new DispenseTransaction
               {
                  Time = time,
                  DispensedAmount = amount
               });
            }
         }

         return transactions;
      }

      /// <summary>
      /// Parse amount from string format "$20.00" to decimal.
      /// </summary>
      private decimal ParseAmount(string amountStr)
      {
         if (string.IsNullOrEmpty(amountStr))
            return 0;

         // Remove $ and any commas
         string cleaned = amountStr.Replace("$", "").Replace(",", "").Trim();

         if (decimal.TryParse(cleaned, out decimal amount))
            return amount;

         return 0;
      }

      #endregion

      #region Helper Classes

      private class DispenseTransaction
      {
         public DateTime Time { get; set; }
         public decimal DispensedAmount { get; set; }
      }

      #endregion
   }
}
