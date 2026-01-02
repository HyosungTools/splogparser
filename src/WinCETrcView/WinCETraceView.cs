using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace WinCETraceView
{
   /// <summary>
   /// View for WinCE trace log files (*.log).
   /// Displays parsed trace entries with type, code, and message.
   /// </summary>
   [Export(typeof(IView))]
   public class WinCETraceView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      WinCETraceView() : base(ParseType.WinCE, "WinCETraceView") { }

      /// <summary>
      /// Creates a WinCETraceTable instance.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>New WinCETraceTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         WinCETraceTable traceTable = new WinCETraceTable(ctx, viewName);
         traceTable.ReadXmlFile();
         return traceTable;
      }
   }
}
