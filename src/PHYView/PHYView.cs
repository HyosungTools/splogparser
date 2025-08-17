using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CDMView
{
   [Export(typeof(IView))]
   public class PHYView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      PHYView() : base(ParseType.SP, "PHYView") { }

      /// <summary>
      /// Creates an CDM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new CDM table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         PHYTable cdmTable = new PHYTable(ctx, viewName);
         cdmTable.ReadXmlFile();
         return cdmTable;
      }
   }
}
