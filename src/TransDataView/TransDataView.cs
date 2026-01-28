using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace TransDataView
{
   [Export(typeof(IView))]
   public class TransDataView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      TransDataView() : base(ParseType.AP, "TransDataView") { }

      /// <summary>
      /// Creates a TransactionData Table instance. 
      /// </summary>
      /// <param name="ctx">Context for the command. </param>
      /// <returns>new TransactionData table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         TransDataTable transactionDataTable = new TransDataTable(ctx, viewName);
         transactionDataTable.ReadXmlFile();
         return transactionDataTable;
      }
   }
}
