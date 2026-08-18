using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace WireTraceView
{
   [Export(typeof(IView))]
   public class WireTraceView : BaseView, IView
   {
      /// <summary>
      /// Constructor. Runs under ParseType.MV (sub-verb "WireTrace", e.g. --mv WireTrace or --mv *)
      /// but is fed by the TcpLogHandler, which matches TcpTrace_*.txt.
      /// </summary>
      WireTraceView() : base(ParseType.MV, "WireTraceView") { }

      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         WireTraceTable wireTable = new WireTraceTable(ctx, viewName);
         wireTable.ReadXmlFile();
         return wireTable;
      }

      /// <summary>
      /// WireTrace only consumes the TcpTrace_*.txt captures (TcpLogHandler). Skip the
      /// MoniViewServerLog*.txt files (MVLogHandler) - it discards every server-log line anyway.
      /// See BaseView.HandlesActiveHandler.
      /// </summary>
      protected override bool HandlesActiveHandler(ILogFileHandler handler)
      {
         return handler != null && handler.Name == "TcpLogFileHandler";
      }
   }
}
