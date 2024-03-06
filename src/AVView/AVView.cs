using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace AVView
{
   [Export(typeof(IView))]
   public class AVView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      AVView() : base(ParseType.AV, "AVView") { }

      /// <summary>
      /// Creates an AVTable instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AVTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         AVTable installTable = new AVTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
