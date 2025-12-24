using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OverView
{
   /// <summary>
   /// Represents a single time segment with a specific state.
   /// Used to create chronological pie slices for wall clock visualization.
   /// </summary>
   public class StateSegment
   {
      public DateTime StartTime { get; set; }
      public DateTime EndTime { get; set; }
      public string State { get; set; }
      public string Label { get; set; }  // Unique label for pie chart (e.g., "InService_1")

      public TimeSpan Duration => EndTime - StartTime;
      public double DurationSeconds => Duration.TotalSeconds;
   }

   /// <summary>
   /// Represents state segments for a 12-hour period (AM or PM) of a single day.
   /// Segments are in chronological order for wall clock visualization.
   /// </summary>
   public class PeriodStateSegments
   {
      public DateTime Date { get; set; }
      public string Period { get; set; } // "AM" or "PM"
      public List<StateSegment> Segments { get; set; }

      public PeriodStateSegments(DateTime date, string period)
      {
         Date = date;
         Period = period;
         Segments = new List<StateSegment>();
      }

      /// <summary>
      /// Add a segment and assign a unique label.
      /// </summary>
      public void AddSegment(DateTime start, DateTime end, string state)
      {
         if (start >= end)
            return;

         // Count existing segments with same state to create unique label
         int count = Segments.Count(s => s.State == state) + 1;

         Segments.Add(new StateSegment
         {
            StartTime = start,
            EndTime = end,
            State = state,
            Label = $"{state}_{count}"
         });
      }

      public TimeSpan TotalDuration => Segments.Aggregate(TimeSpan.Zero, (sum, s) => sum + s.Duration);
   }

   /// <summary>
   /// Analyzes OverSummary data to calculate ATM state segments for wall clock visualization.
   /// Produces chronologically ordered segments that map directly to pie chart slices.
   /// </summary>
   public class StateWallClockAnalyzer
   {
      private const string STATE_UNKNOWN = "Unknown";
      private const string STATE_POWERUP = "PowerUp";

      private List<PeriodStateSegments> _results;

      /// <summary>
      /// Analysis results - list of period segments for each day/period combination.
      /// </summary>
      public List<PeriodStateSegments> Results => _results;

      /// <summary>
      /// Analyze the Summary DataTable to calculate state segments per day and period.
      /// </summary>
      /// <param name="summaryTable">The Summary DataTable from OverTable</param>
      public void Analyze(DataTable summaryTable)
      {
         _results = new List<PeriodStateSegments>();

         if (summaryTable == null || summaryTable.Rows.Count == 0)
         {
            return;
         }

         // Extract all log entries with time, and those with mode values
         var logEntries = ExtractLogEntries(summaryTable);
         if (logEntries.Count == 0)
         {
            return;
         }

         // Find the full date range
         DateTime firstDate = logEntries.Min(e => e.Time.Date);
         DateTime lastDate = logEntries.Max(e => e.Time.Date);

         // Group entries by day for lookup
         var entriesByDay = logEntries.GroupBy(e => e.Time.Date)
                                       .ToDictionary(g => g.Key, g => g.OrderBy(e => e.Time).ToList());

         // Process EVERY day in the range, including days with no entries
         for (DateTime date = firstDate; date <= lastDate; date = date.AddDays(1))
         {
            var amPeriod = new PeriodStateSegments(date, "AM");
            var pmPeriod = new PeriodStateSegments(date, "PM");

            if (entriesByDay.TryGetValue(date, out var dayEntries))
            {
               // Day has log entries - process normally
               CalculateSegmentsForDay(dayEntries, date, amPeriod, pmPeriod);
            }
            else
            {
               // Day has NO log entries - entire day is Unknown
               DateTime dayStart = date;
               DateTime noon = date.AddHours(12);
               DateTime dayEnd = date.AddDays(1);

               amPeriod.AddSegment(dayStart, noon, STATE_UNKNOWN);
               pmPeriod.AddSegment(noon, dayEnd, STATE_UNKNOWN);
            }

            _results.Add(amPeriod);
            _results.Add(pmPeriod);
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

         foreach (var period in _results)
         {
            foreach (var segment in period.Segments)
            {
               DataRow row = resultsTable.NewRow();
               row["Date"] = period.Date.ToString("yyyy-MM-dd");
               row["Period"] = period.Period;
               row["State"] = segment.State;
               row["Label"] = segment.Label;
               row["StartTime"] = segment.StartTime.ToString("HH:mm:ss");
               row["EndTime"] = segment.EndTime.ToString("HH:mm:ss");
               row["DurationSeconds"] = segment.DurationSeconds;
               row["DurationFormatted"] = FormatDuration(segment.Duration);
               resultsTable.Rows.Add(row);
            }
         }

         resultsTable.AcceptChanges();
      }

      #region Private Methods

      private List<LogEntry> ExtractLogEntries(DataTable summaryTable)
      {
         var entries = new List<LogEntry>();

         if (!summaryTable.Columns.Contains("time"))
         {
            return entries;
         }

         bool hasModeColumn = summaryTable.Columns.Contains("mode");

         foreach (DataRow row in summaryTable.Rows)
         {
            if (row["time"] == DBNull.Value)
               continue;

            DateTime time;
            if (row["time"] is DateTime dt)
            {
               time = dt;
            }
            else if (!DateTime.TryParse(row["time"].ToString(), out time))
            {
               continue;
            }

            string mode = null;
            if (hasModeColumn && row["mode"] != DBNull.Value)
            {
               string modeValue = row["mode"].ToString().Trim();
               if (!string.IsNullOrEmpty(modeValue))
                  mode = modeValue;
            }

            entries.Add(new LogEntry { Time = time, Mode = mode });
         }

         return entries;
      }

      private void CalculateSegmentsForDay(List<LogEntry> dayEntries, DateTime date,
          PeriodStateSegments amPeriod, PeriodStateSegments pmPeriod)
      {
         DateTime dayStart = date;
         DateTime noon = date.AddHours(12);
         DateTime dayEnd = date.AddDays(1);

         DateTime currentTime = dayStart;
         string currentState = STATE_UNKNOWN;
         DateTime? lastLogEntryTime = null;

         foreach (var entry in dayEntries)
         {
            // Check if this is a PowerUp - if so, mark time since last log entry as Unknown
            if (entry.Mode == STATE_POWERUP && lastLogEntryTime.HasValue)
            {
               // Record segment from currentTime to lastLogEntryTime in currentState
               if (lastLogEntryTime.Value > currentTime)
               {
                  RecordSegment(currentTime, lastLogEntryTime.Value, currentState, noon, amPeriod, pmPeriod);
                  currentTime = lastLogEntryTime.Value;
               }

               // Record segment from lastLogEntryTime to PowerUp as Unknown
               RecordSegment(currentTime, entry.Time, STATE_UNKNOWN, noon, amPeriod, pmPeriod);
               currentTime = entry.Time;
               currentState = STATE_POWERUP;
            }
            else if (entry.Mode != null)
            {
               // Regular mode change - record segment in previous state
               if (entry.Time > currentTime)
               {
                  RecordSegment(currentTime, entry.Time, currentState, noon, amPeriod, pmPeriod);
                  currentTime = entry.Time;
               }
               currentState = entry.Mode;
            }

            // Track last log entry time (regardless of whether it has a mode)
            lastLogEntryTime = entry.Time;
         }

         // Handle end of day - from last log entry to midnight is Unknown
         if (lastLogEntryTime.HasValue && lastLogEntryTime.Value < dayEnd)
         {
            // First, record any remaining time in current state up to last log entry
            if (lastLogEntryTime.Value > currentTime)
            {
               RecordSegment(currentTime, lastLogEntryTime.Value, currentState, noon, amPeriod, pmPeriod);
               currentTime = lastLogEntryTime.Value;
            }

            // Then record Unknown from last log entry to end of day
            RecordSegment(currentTime, dayEnd, STATE_UNKNOWN, noon, amPeriod, pmPeriod);
         }
         else if (currentTime < dayEnd)
         {
            // No log entries at all
            RecordSegment(currentTime, dayEnd, STATE_UNKNOWN, noon, amPeriod, pmPeriod);
         }
      }

      private void RecordSegment(DateTime start, DateTime end, string state,
          DateTime noon, PeriodStateSegments amPeriod, PeriodStateSegments pmPeriod)
      {
         if (start >= end)
            return;

         // Split at noon if necessary
         if (start < noon && end <= noon)
         {
            // Entirely in AM
            amPeriod.AddSegment(start, end, state);
         }
         else if (start >= noon && end > noon)
         {
            // Entirely in PM
            pmPeriod.AddSegment(start, end, state);
         }
         else if (start < noon && end > noon)
         {
            // Spans noon - split it
            amPeriod.AddSegment(start, noon, state);
            pmPeriod.AddSegment(noon, end, state);
         }
      }

      private string FormatDuration(TimeSpan duration)
      {
         return $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
      }

      #endregion

      #region Helper Classes

      private class LogEntry
      {
         public DateTime Time { get; set; }
         public string Mode { get; set; }
      }

      #endregion
   }
}
