using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace IIView
{
   [Export(typeof(IView))]
   public class IIView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      IIView() : base(ParseType.II, "IIView") { }

      /// <summary>
      /// Creates an AVTable instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AVTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         IITable installTable = new IITable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
