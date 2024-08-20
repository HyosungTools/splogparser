using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace TCRView
{
   [Export(typeof(IView))]
   public class TransView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      TransView() : base(ParseType.TCR, "TransView") { }

      /// <summary>
      /// Creates an AddKey Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>AddKeyTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         TransTable myTable = new TransTable(ctx, viewName);
         myTable.ReadXmlFile();
         return myTable;
      }
   }
}
