using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CashIn
{
   [Export(typeof(IView))]
   public class CashInView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      public CashInView() : base("CIM2", "1303.CashIn") { }

      /// <summary>
      /// Creates an HCDU Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         return new CashInTable(ctx, viewName);
      }
   }
}
