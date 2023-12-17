using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace EJView
{
   [Export(typeof(IView))]
   public class EJView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      EJView() : base(ParseType.AP, "EJView") { }

      /// <summary>
      /// Creates an CIM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         EJTable ejTable = new EJTable(ctx, viewName);
         ejTable.ReadXmlFile();
         return ejTable;
      }
   }
}
