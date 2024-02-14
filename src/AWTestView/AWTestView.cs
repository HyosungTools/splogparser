using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace AWTestView
{
   [Export(typeof(IView))]
   public class AWTestView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      AWTestView() : base(ParseType.AW, "AWTestView") { }

      /// <summary>
      /// Creates an AWTestViewTable instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AWTestViewTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         AWTestViewTable installTable = new AWTestViewTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
