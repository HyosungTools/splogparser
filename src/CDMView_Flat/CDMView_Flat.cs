using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CDMView_Flat
{
   [Export(typeof(IView))]
   public class CDMView_Flat : BaseView, IView
   {
      public CDMView_Flat() : base(ParseType.SF, "CDMView_Flat") { }

      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         var table = new CDMTable_Flat(ctx, viewName);
         table.ReadXmlFile(); // Load XML template/schema
         return table;
      }
   }
}
