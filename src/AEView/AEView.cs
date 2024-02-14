using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace AEView
{
   [Export(typeof(IView))]
   public class AEView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      AEView() : base(ParseType.AE, "AEView") { }

      /// <summary>
      /// Creates an AETable instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AETable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         AETable installTable = new AETable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
