using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace AWView
{
   [Export(typeof(IView))]
   public class AWView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      AWView() : base(ParseType.AW, "AWView") { }

      /// <summary>
      /// Creates an AWTable instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AETable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         AWTable installTable = new AWTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
