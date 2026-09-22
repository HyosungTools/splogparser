using Contract;
using Impl;
using System;
using System.ComponentModel.Composition;

namespace CashTrailView
{
   /// <summary>
   /// CashTrailView - the "where's my money" view.
   ///
   /// Analysis-only. Parses no log files. In the Analyze phase it reads the XML written by
   /// CDMView, CIMView and OverView in PostProcess, and cross-references them to answer three
   /// questions a support analyst asks on every cash-balancing complaint:
   ///
   ///   CashBalance - did the money balance for this settlement period, and if not, by how much?
   ///   CashTrail   - where did each dollar go, one row per money event?
   ///   CashBins    - what is in the reject/retract bins and WHERE DID IT COME FROM?
   ///
   /// The last one is the point. On a combined reject/retract cash unit the same physical bin is
   /// reported by both the CDM and the CIM, so dispenser cash shows up on a CIM worksheet and
   /// reads as deposit money. See NHSWS-18867.
   /// </summary>
   [Export(typeof(IView))]
   public class CashTrailView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      CashTrailView() : base(ParseType.SP, "CashTrailView") { }

      /// <summary>
      /// Creates a CashTrail Table instance.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>new CashTrailTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CashTrailTable table = new CashTrailTable(ctx, viewName);
         table.ReadXmlFile();
         return table;
      }

      /// <summary>
      /// No log file parsing - this view is analysis-only.
      /// Override Process to prevent BaseView from iterating SP log files.
      /// </summary>
      public override void Process(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("------------------------------------------------");
         ctx.ConsoleWriteLogLine("Process: " + Name + " (analysis-only, skipping)");
      }

      /// <summary>
      /// No-op - nothing to serialize after the parse phase.
      /// </summary>
      public override void PostProcess(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("------------------------------------------------");
         ctx.ConsoleWriteLogLine("PostProcess: " + Name + " (analysis-only, skipping)");
      }

      /// <summary>
      /// The main entry point. Read CDMView, CIMView and OverView XML, build the cash trail,
      /// write XML for WriteExcel.
      /// </summary>
      public override void Analyze(IContext ctx)
      {
         ctx.ConsoleWriteLogLine("------------------------------------------------");
         ctx.ConsoleWriteLogLine("Analyze: " + Name);

         try
         {
            CashTrailTable table = new CashTrailTable(ctx, viewName);
            table.BuildSummary();
            table.WriteXmlFile();
         }
         catch (Exception ex)
         {
            ctx.ConsoleWriteLogLine($"EXCEPTION in {Name}.Analyze: {ex.Message}");
            ctx.ConsoleWriteLogLine(ex.StackTrace);
         }
      }
   }
}
