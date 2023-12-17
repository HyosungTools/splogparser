using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace AddKeyView
{
   [Export(typeof(IView))]
   public class AddKeyView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      AddKeyView() : base(ParseType.AP, "AddKeyView") { }

      /// <summary>
      /// Creates an AddKey Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AddKeyTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         AddKeyTable installTable = new AddKeyTable(ctx, viewName);
         installTable.ReadXmlFile();
         return installTable;
      }
   }
}
