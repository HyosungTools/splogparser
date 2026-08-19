using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace IDCView_Flat
{
   [Export(typeof(IView))]
   public class IDCView_Flat : BaseView, IView
   {
      public IDCView_Flat() : base(ParseType.SF, "IDCView_Flat") { }

      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         IDCTable_Flat table = new IDCTable_Flat(ctx, viewName);
         table.ReadXmlFile();
         return table;
      }
   }
}
