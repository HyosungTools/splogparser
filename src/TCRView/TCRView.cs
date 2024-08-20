using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace TCRView
{
   [Export(typeof(IView))]
   public class TCRView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      TCRView() : base(ParseType.TCR, "TCRView") { }

      /// <summary>
      /// Creates an AddKey Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AddKeyTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         TCRTable myTable = new TCRTable(ctx, viewName);
         myTable.ReadXmlFile();
         return myTable;
      }
   }
}
