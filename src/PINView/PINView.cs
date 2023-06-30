using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace PINView
{
   [Export(typeof(IView))]
   public class PINView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      PINView() : base("PIN", "PINView") { }

      /// <summary>
      /// Creates an PIN Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new PIN table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         PINTable PINTable = new PINTable(ctx, viewName);
         PINTable.ReadXmlFile();
         return PINTable;
      }
   }
}
