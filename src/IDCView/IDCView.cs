using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace IDCView
{
   [Export(typeof(IView))]
   public class IDCView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      IDCView() : base(ParseType.SP, "IDCView") { }

      /// <summary>
      /// Creates an IDC Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new IDC table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         IDCTable IDCTable = new IDCTable(ctx, viewName);
         IDCTable.ReadXmlFile();
         return IDCTable;
      }
   }
}
