using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CashDispView
{
   [Export(typeof(IView))]
   public class CashDispView : BaseView, IView
   {
      CashDispView() : base(ParseType.AP, "DispView") { }

      /// <summary>
      /// Creates an CD Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CashDispTable ejTable = new CashDispTable(ctx, viewName);
         ejTable.ReadXmlFile();
         return ejTable;
      }
   }
}
