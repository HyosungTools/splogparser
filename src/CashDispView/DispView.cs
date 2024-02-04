using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CashDispView
{
   [Export(typeof(IView))]
   public class DispView : BaseView, IView
   {
      DispView() : base(ParseType.AP, "DispView") { }

      /// <summary>
      /// Creates an CD Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         DispTable ejTable = new DispTable(ctx, viewName);
         ejTable.ReadXmlFile();
         return ejTable;
      }
   }
}
