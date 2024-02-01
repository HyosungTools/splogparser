using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace ATView
{
   [Export(typeof(IView))]
   public class ATView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      ATView() : base(ParseType.AT, "ATView") { }

      /// <summary>
      /// Creates an ATViewTable instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>ATViewTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         ATTable installTable = new ATTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
