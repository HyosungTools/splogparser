using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace ExtraView
{
   [Export(typeof(IView))]
   public class ExtraView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      ExtraView() : base("Extra", "ExtraView") { }

      /// <summary>
      /// Creates an Extra Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new Extra table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         ExtraTable ExtraTable = new ExtraTable(ctx, viewName);
         ExtraTable.ReadXmlFile();
         return ExtraTable;
      }
   }
}
