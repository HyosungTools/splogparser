using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace IPMView_Flat
{
   [Export(typeof(IView))]
   public class IPMView_Flat : BaseView, IView
   {
      public IPMView_Flat() : base(ParseType.SF, "IPMView_Flat") { }

      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         IPMTable_Flat table = new IPMTable_Flat(ctx, viewName);
         table.ReadXmlFile();
         return table;
      }
   }
}
