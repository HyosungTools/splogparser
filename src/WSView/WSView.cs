using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace WSView
{
   [Export(typeof(IView))]
   public class WSView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      WSView() : base(ParseType.AP, "WSView") { }

      /// <summary>
      /// Creates a WS Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new WS table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         WSTable wsTable = new WSTable(ctx, viewName);
         wsTable.ReadXmlFile();
         return wsTable;
      }
   }
}
