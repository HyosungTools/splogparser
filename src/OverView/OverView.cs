using Contract;
using Impl;
using System.ComponentModel.Composition;
using System.Data;

namespace OverView
{
   [Export(typeof(IView))]
   public class OverView : BaseView, IView
   {
      /// <summary>
      /// The analyzer for calculating ATM state segments.
      /// </summary>
      private StateWallClockAnalyzer _wallClockAnalyzer;

      /// <summary>
      /// Reference to the OverTable for analysis phase.
      /// </summary>
      private OverTable _analyzeTable;

      /// <summary>
      /// Constructor
      /// </summary>
      OverView() : base(ParseType.AP, "OverView") { }

      /// <summary>
      /// Creates an Over Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new overview table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         OverTable overTable = new OverTable(ctx, viewName);
         overTable.ReadXmlFile();
         return overTable;
      }

      /// <summary>
      /// PreAnalyze: Load the Summary data and prepare for analysis.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      public override void PreAnalyze(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("PreAnalyze: " + viewName);

         // Create table instance and load data from XML
         _analyzeTable = new OverTable(ctx, viewName);
         _analyzeTable.ReadXmlFile();

         // Initialize the analyzer
         _wallClockAnalyzer = new StateWallClockAnalyzer();

         ctx.LogWriteLine("PreAnalyze: Data loaded for analysis");
      }

      /// <summary>
      /// Analyze: Calculate ATM state segments for wall clock visualization.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      public override void Analyze(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("Analyze: " + viewName);

         if (_analyzeTable == null || _wallClockAnalyzer == null)
         {
            ctx.LogWriteLine("Analyze: No data available for analysis");
            return;
         }

         // Get the Summary table from the DataSet
         DataTable summaryTable = _analyzeTable.GetSummaryTable();
         if (summaryTable == null)
         {
            ctx.LogWriteLine("Analyze: Summary table not found");
            return;
         }

         ctx.LogWriteLine($"Analyze: Processing {summaryTable.Rows.Count} rows from Summary table");

         // Run the analysis
         _wallClockAnalyzer.Analyze(summaryTable);

         if (_wallClockAnalyzer.Results != null)
         {
            ctx.LogWriteLine($"Analyze: Generated {_wallClockAnalyzer.Results.Count} period results");

            // Log the results
            foreach (var period in _wallClockAnalyzer.Results)
            {
               ctx.LogWriteLine($"  {period.Date:yyyy-MM-dd} {period.Period}: {period.Segments.Count} segments");
               foreach (var segment in period.Segments)
               {
                  ctx.LogWriteLine($"    {segment.StartTime:HH:mm:ss}-{segment.EndTime:HH:mm:ss} {segment.State} ({segment.Duration:hh\\:mm\\:ss})");
               }
            }
         }
      }

      /// <summary>
      /// PostAnalyze: Store results for later use in WriteExcel.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      public override void PostAnalyze(IContext ctx)
      {
         ctx.LogWriteLine("------------------------------------------------");
         ctx.LogWriteLine("PostAnalyze: " + viewName);

         if (_wallClockAnalyzer?.Results == null || _wallClockAnalyzer.Results.Count == 0)
         {
            ctx.LogWriteLine("PostAnalyze: No analysis results to save");
            return;
         }

         // Populate the StateWallClock table in the OverTable
         _analyzeTable.PopulateStateWallClockTable(_wallClockAnalyzer);

         ctx.LogWriteLine("PostAnalyze: StateWallClock data prepared");
      }

      /// <summary>
      /// WriteExcel: Write worksheets including StateWallClock with pie charts.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      public override void WriteExcel(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("------------------------------------------------");
         ctx.ConsoleWriteLogLine("WriteExcel: " + viewName);

         // Create a Table Instance and load up the xml file
         OverTable overTable = new OverTable(ctx, viewName);
         overTable.ReadXmlFile();

         // Write the standard Summary worksheet
         overTable.WriteExcelFile();

         // If we have analysis results, write the StateWallClock worksheet with charts
         if (_wallClockAnalyzer?.Results != null && _wallClockAnalyzer.Results.Count > 0)
         {
            ctx.ConsoleWriteLogLine("WriteExcel: Writing StateWallClock worksheet with wall clock charts");
            overTable.WriteStateWallClockExcel(_wallClockAnalyzer.Results);
         }

         ctx.ConsoleWriteLogLine("End WriteExcel: " + viewName);
      }
   }
}
