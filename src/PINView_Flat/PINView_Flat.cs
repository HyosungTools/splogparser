using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace PINView_Flat
{
   [Export(typeof(IView))]
   public class PINView_Flat : BaseView, IView
   {
      public PINView_Flat() : base(ParseType.SF, "PINView_Flat") { }

      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         PINTable_Flat table = new PINTable_Flat(ctx, viewName);
         table.ReadXmlFile();
         return table;
      }
   }
}
