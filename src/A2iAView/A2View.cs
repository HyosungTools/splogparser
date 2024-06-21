using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace A2View
{
   [Export(typeof(IView))]
   public class A2View : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      A2View() : base(ParseType.A2, "A2View") { }

      /// <summary>
      /// Creates an AddKey Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AddKeyTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         A2Table installTable = new A2Table(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
