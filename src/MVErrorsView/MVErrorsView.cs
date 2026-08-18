using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace MVErrorsView
{
   [Export(typeof(IView))]
   public class MVErrorsView : BaseView, IView
   {
      /// <summary>
      /// Constructor. ParseType.MV routes this view to the MoniView server-log handler.
      /// </summary>
      MVErrorsView() : base(ParseType.MV, "MVErrorsView") { }

      /// <summary>
      /// Creates an MVErrorsTable instance and loads its seed XML/XSD.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>MVErrorsTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         MVErrorsTable errorsTable = new MVErrorsTable(ctx, viewName);
         errorsTable.ReadXmlFile();
         return errorsTable;
      }

      /// <summary>
      /// Server-log view: only consume MoniViewServerLog*.txt (MVLogHandler). Skip the
      /// TcpTrace_*.txt captures (TcpLogHandler) - a fleet capture has thousands and every line
      /// would be discarded in ProcessRow. See BaseView.HandlesActiveHandler.
      /// </summary>
      protected override bool HandlesActiveHandler(ILogFileHandler handler)
      {
         return handler != null && handler.Name == "MVLogFileHandler";
      }
   }
}
