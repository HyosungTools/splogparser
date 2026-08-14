using Contract;
using Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace TellerTxnView
{
   /// <summary>
   /// TellerTxnView is an ANALYZE-ONLY view. It parses no logs of its own; it runs in the Analyze
   /// phase and pulls in data tables that other views already wrote to WorkFolder\&lt;View&gt;.xml
   /// during PostProcess, then reconstructs one row per teller-assisted transaction.
   ///
   /// The VIEW declares exactly which tables it pulls in (see Sources) and loads them; the table
   /// class only reconstructs from the tables it is handed. Each source is guarded, so a view that
   /// wasn't parsed just leaves its columns blank.
   ///
   /// Shares the AE ParseType, so it is selected with -e * (which also runs AEView, whose XML this
   /// view depends on). For the dispense/disposition columns also parse: -a Over -s CDM,IPM.
   /// </summary>
   [Export(typeof(IView))]
   public class TellerTxnView : BaseView, IView
   {
      /// <summary>
      /// Built-in default cross-view dependencies, used when the external config
      /// (TellerTxnView.sources.xml) is absent or empty. Each entry is (source view name,
      /// table name inside that view's XML).
      /// </summary>
      private static readonly (string View, string Table)[] DefaultSources =
      {
         ("AEView",   "MoniPlus2sEvents"),   // required - the transaction
         ("AEView",   "NextwareEvents"),     // required - device Open/Close session faults
         ("OverView", "OverSummary"),        // optional - cash dispensed
         ("IPMView",  "IPMDeposit"),         // optional - check disposition
      };

      private TellerTxnTable _table;

      TellerTxnView() : base(ParseType.AE, "TellerTxnView") { }

      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         TellerTxnTable table = new TellerTxnTable(ctx, viewName);
         table.ReadXmlFile();
         return table;
      }

      /// <summary>Analyze-only: this view parses no log lines.</summary>
      public override void Process(IContext ctx)
      {
         ctx.LogWriteLine("TellerTxnView.Process: analyze-only view, nothing to process.");
      }

      public override void PreAnalyze(IContext ctx)
      {
         _table = new TellerTxnTable(ctx, viewName);
         _table.ReadXmlFile();   // loads this view's own (empty) TellerTransactions/TellerDevice schema
      }

      public override void Analyze(IContext ctx)
      {
         if (_table == null) return;
         Dictionary<string, DataTable> sourceTables = LoadSources(ctx);
         _table.Build(ctx, sourceTables);
      }

      public override void PostAnalyze(IContext ctx)
      {
         // Persist the reconstructed rows so the standard WriteExcel path picks them up.
         if (_table != null) _table.WriteXmlFile();
      }

      /// <summary>
      /// Load each declared source table from its view's WorkFolder XML. Reads each XML file once
      /// (grouping the table list by view), guards on file existence, and returns a detached copy of
      /// each table keyed by table name.
      /// </summary>
      private Dictionary<string, DataTable> LoadSources(IContext ctx)
      {
         var loaded = new Dictionary<string, DataTable>();

         foreach (var group in ResolveSources(ctx).GroupBy(s => s.View))
         {
            string path = ctx.WorkFolder + "\\" + group.Key + ".xml";
            if (!ctx.ioProvider.Exists(path))
            {
               ctx.ConsoleWriteLogLine($"TellerTxnView: {group.Key}.xml not found - skipping ({group.Key} was not parsed).");
               continue;
            }

            try
            {
               DataSet ds = new DataSet();
               ds.ReadXml(path);
               foreach (var src in group)
               {
                  if (ds.Tables.Contains(src.Table))
                  {
                     loaded[src.Table] = ds.Tables[src.Table].Copy();
                     ctx.LogWriteLine($"TellerTxnView: pulled in {group.Key}.{src.Table} ({loaded[src.Table].Rows.Count} rows).");
                  }
                  else
                  {
                     ctx.ConsoleWriteLogLine($"TellerTxnView: table {src.Table} not found in {group.Key}.xml.");
                  }
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine($"TellerTxnView: failed reading {group.Key}.xml: {e.Message}");
            }
         }

         return loaded;
      }

      /// <summary>
      /// Resolve the source-table list. Data-driven: if TellerTxnView.sources.xml is found (in the
      /// exe/current directory or the work folder) and lists at least one &lt;Source view="" table=""/&gt;,
      /// that list is used so the support team can change which tables feed the summary without a
      /// rebuild. Otherwise the built-in DefaultSources are used.
      /// </summary>
      private List<(string View, string Table)> ResolveSources(IContext ctx)
      {
         foreach (string dir in new[] { ctx.ioProvider.GetCurrentDirectory(), ctx.WorkFolder })
         {
            if (string.IsNullOrEmpty(dir)) continue;
            string path = dir + "\\TellerTxnView.sources.xml";
            if (!ctx.ioProvider.Exists(path)) continue;

            try
            {
               XDocument doc = XDocument.Load(path);
               var list = doc.Descendants("Source")
                  .Select(x => (View: (string)x.Attribute("view"), Table: (string)x.Attribute("table")))
                  .Where(s => !string.IsNullOrEmpty(s.View) && !string.IsNullOrEmpty(s.Table))
                  .ToList();

               if (list.Count > 0)
               {
                  ctx.ConsoleWriteLogLine($"TellerTxnView: loaded {list.Count} sources from config {path}.");
                  return list;
               }
            }
            catch (Exception e)
            {
               ctx.ConsoleWriteLogLine($"TellerTxnView: bad sources config {path}: {e.Message} - using defaults.");
            }
         }

         return DefaultSources.ToList();
      }
   }
}
