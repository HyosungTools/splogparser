using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace SSView
{
   [Export(typeof(IView))]
   public class SSView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      SSView() : base(ParseType.SS, "SSView") { }

      /// <summary>
      /// Creates an AddKey Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AddKeyTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         SSTable installTable = new SSTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
