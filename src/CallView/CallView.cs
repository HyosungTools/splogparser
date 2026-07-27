using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CallView
{
   /// <summary>
   /// CallView is a *summary* view of the Active Teller Workstation log.
   ///
   /// Where AWView is a raw one-row-per-line dump of the Workstation log, CallView
   /// reconstructs teller call SESSIONS (a session spans many log lines) and emits a
   /// short, scannable ledger: one row per answered call with its disposition
   /// (Clean / DROPPED) and, for drops, the root cause. It shares the AW ParseType,
   /// so it reuses the existing AWLogHandler / AWLine machinery unchanged and is
   /// selected on the command line with:  -w Call  (or -w *).
   /// </summary>
   [Export(typeof(IView))]
   public class CallView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      CallView() : base(ParseType.AW, "CallView") { }

      /// <summary>
      /// Creates a CallTable instance.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>CallTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CallTable callTable = new CallTable(ctx, viewName);
         callTable.ReadXmlFile();
         return callTable;
      }
   }
}
