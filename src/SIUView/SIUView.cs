using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace SIUView
{
   [Export(typeof(IView))]
   public class SIUView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      SIUView() : base("SIU", "SIUView") { }

      /// <summary>
      /// Creates an SIU Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new SIU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         SIUTable SIUTable = new SIUTable(ctx, viewName);
         SIUTable.ReadXmlFile();
         return SIUTable;
      }
   }
}
