using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace OverView
{
   [Export(typeof(IView))]
   public class OverView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      OverView() : base(ParseType.AP, "OverView") { }

      /// <summary>
      /// Creates an Over Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new overview table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         OverTable overTable = new OverTable(ctx, viewName);
         overTable.ReadXmlFile();
         return overTable;
      }
   }
}
