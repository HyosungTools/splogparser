using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace OverView
{
   /// <summary>
   /// One day's resource metrics bucketed into 96 fifteen-minute slots
   /// (00:00 .. 23:45). A null slot means "no data yet" — slots before the first
   /// sample of the day stay null (a gap on the chart); from the first sample
   /// onward each metric is carried forward until the next sample.
   /// </summary>
   public class ResourceDaySlots
   {
      public const int SlotCount = 96;   // 24h * 4 slots/hour

      public DateTime Date { get; set; }
      public double?[] Memory { get; } = new double?[SlotCount];
      public double?[] VmSize { get; } = new double?[SlotCount];
      public double?[] Private { get; } = new double?[SlotCount];
      public double?[] Handles { get; } = new double?[SlotCount];

      /// <summary>Slot label, e.g. "13:15".</summary>
      public static string SlotLabel(int slot)
      {
         return TimeSpan.FromMinutes(slot * 15).ToString(@"hh\:mm");
      }
   }

   /// <summary>
   /// Buckets the raw Resource snapshot rows into per-day, 96-slot grids: each
   /// sample is dropped into its NEAREST 15-minute slot, then values are filled
   /// forward until the next sample is found. Mirrors the per-day grouping that
   /// StateWallClockAnalyzer uses so the two wall-clock views behave the same.
   /// </summary>
   public class ResourceWallClockAnalyzer
   {
      public List<ResourceDaySlots> Results { get; private set; } = new List<ResourceDaySlots>();

      public void Analyze(DataTable resourceTable)
      {
         Results = new List<ResourceDaySlots>();
         if (resourceTable == null || resourceTable.Rows.Count == 0)
         {
            return;
         }

         // Parse rows, skipping anything without a usable timestamp.
         List<Sample> samples = new List<Sample>();
         foreach (DataRow row in resourceTable.Rows)
         {
            DateTime ts;
            if (!TryParseTime(row, out ts))
            {
               continue;
            }

            Sample s = new Sample();
            s.Time = ts;
            s.Memory = ParseDouble(row, "memoryMB");
            s.VmSize = ParseDouble(row, "vmSizeMB");
            s.Private = ParseDouble(row, "privateSizeMB");
            s.Handles = ParseDouble(row, "handles");
            samples.Add(s);
         }

         if (samples.Count == 0)
         {
            return;
         }

         // One grid per calendar day, in date order.
         foreach (IGrouping<DateTime, Sample> dayGroup in samples
                     .GroupBy(s => s.Time.Date)
                     .OrderBy(g => g.Key))
         {
            ResourceDaySlots day = new ResourceDaySlots();
            day.Date = dayGroup.Key;

            // Drop each sample into its nearest slot; a later sample in the same
            // slot overwrites an earlier one.
            foreach (Sample s in dayGroup.OrderBy(s => s.Time))
            {
               int slot = NearestSlot(s.Time);
               if (s.Memory.HasValue) { day.Memory[slot] = s.Memory; }
               if (s.VmSize.HasValue) { day.VmSize[slot] = s.VmSize; }
               if (s.Private.HasValue) { day.Private[slot] = s.Private; }
               if (s.Handles.HasValue) { day.Handles[slot] = s.Handles; }
            }

            FillForward(day.Memory);
            FillForward(day.VmSize);
            FillForward(day.Private);
            FillForward(day.Handles);

            Results.Add(day);
         }
      }

      /// <summary>Nearest 15-minute slot (0..95) for a time of day.</summary>
      private static int NearestSlot(DateTime t)
      {
         double minutes = t.TimeOfDay.TotalMinutes;
         int slot = (int)Math.Round(minutes / 15.0, MidpointRounding.AwayFromZero);
         if (slot < 0)
         {
            slot = 0;
         }
         if (slot > ResourceDaySlots.SlotCount - 1)
         {
            slot = ResourceDaySlots.SlotCount - 1;
         }
         return slot;
      }

      /// <summary>Carry the last non-null value forward into later empty slots.</summary>
      private static void FillForward(double?[] series)
      {
         double? last = null;
         for (int i = 0; i < series.Length; i++)
         {
            if (series[i].HasValue)
            {
               last = series[i];
            }
            else if (last.HasValue)
            {
               series[i] = last;
            }
         }
      }

      private static bool TryParseTime(DataRow row, out DateTime ts)
      {
         ts = DateTime.MinValue;
         if (!row.Table.Columns.Contains("time"))
         {
            return false;
         }

         string raw = row["time"] as string;
         if (string.IsNullOrWhiteSpace(raw))
         {
            return false;
         }

         // Resource 'time' is the APLine timestamp form: "yyyy-MM-dd HH:mm:ss.fff"
         return DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out ts);
      }

      private static double? ParseDouble(DataRow row, string column)
      {
         if (!row.Table.Columns.Contains(column))
         {
            return null;
         }

         string raw = row[column] as string;
         if (string.IsNullOrWhiteSpace(raw))
         {
            return null;
         }

         double value;
         if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
         {
            return value;
         }
         return null;
      }

      private class Sample
      {
         public DateTime Time;
         public double? Memory;
         public double? VmSize;
         public double? Private;
         public double? Handles;
      }
   }
}
