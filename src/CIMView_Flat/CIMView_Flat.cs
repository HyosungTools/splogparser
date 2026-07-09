using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CIMView_Flat
{
   [Export(typeof(IView))]
   public class CIMView_Flat : BaseView, IView
   {
      public CIMView_Flat() : base(ParseType.SF, "CIMView_Flat") { }

      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CIMTable_Flat table = new CIMTable_Flat(ctx, viewName);
         table.ReadXmlFile(); // Load XML template/schema
         return table;
      }
   }
}
