using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CIMView
{
   [Export(typeof(IView))]
   public class CIMView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      CIMView() : base("CIM", "CIMView") { }

      /// <summary>
      /// Creates an CIM Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CIMTable cimTable = new CIMTable(ctx, viewName);
         cimTable.ReadXmlFile();
         return cimTable;
      }
   }
}
