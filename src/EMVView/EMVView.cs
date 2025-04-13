using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace EMVView
{
   [Export(typeof(IView))]
   public class EMVView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      EMVView() : base(ParseType.AP, "EmvView") { }

      /// <summary>
      /// Creates an Over Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new overview table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         EMVTable emvTable = new EMVTable(ctx, viewName);
         emvTable.ReadXmlFile();
         return emvTable;
      }
   }
}
