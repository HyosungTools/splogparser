using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CardView
{
   [Export(typeof(IView))]
   public class CardView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      CardView() : base(ParseType.AP, "CardView") { }

      /// <summary>
      /// Creates an Over Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new overview table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CardTable cardTable = new CardTable(ctx, viewName);
         cardTable.ReadXmlFile();
         return cardTable;
      }
   }
}
