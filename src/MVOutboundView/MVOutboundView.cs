using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace MVOutboundView
{
   [Export(typeof(IView))]
   public class MVOutboundView : BaseView, IView
   {
      /// <summary>
      /// Constructor. ParseType.MV routes this view to the MoniView server-log handler.
      /// </summary>
      MVOutboundView() : base(ParseType.MV, "MVOutboundView") { }

      /// <summary>
      /// Creates an MVOutboundTable instance and loads its seed XML/XSD.
      /// </summary>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         MVOutboundTable outboundTable = new MVOutboundTable(ctx, viewName);
         outboundTable.ReadXmlFile();
         return outboundTable;
      }
   }
}
