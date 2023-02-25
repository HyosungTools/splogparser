using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CDUCountsView
{

   [Export(typeof(IView))]
   public class CDUCountsView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      public CDUCountsView() : base("CDM2", "DISP") { }

      /// <summary>
      /// Creates an HCDU Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new HCDU table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CDUCountsTable cduCountsTable = new CDUCountsTable(ctx, viewName);
         cduCountsTable.ReadXmlFile();
         return cduCountsTable;
      }
   }
}
