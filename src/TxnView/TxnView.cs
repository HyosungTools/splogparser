using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace TxnView
{
   /// <summary>
   /// TxnView is a cash-in / cash-out reconciliation ledger built from the AP log.
   ///
   /// One row per transaction: requested amount, what physically dispensed (per-cassette
   /// note counts, denominations read from the log at run time), whether the customer took
   /// the cash, teller-assisted flag, outcome and fault. It answers the recurring
   /// "where's my money" question: when requested / dispensed / taken don't agree, the row
   /// stands out.
   ///
   /// It is an AP-parseType view (teller-controlled transactions live in the application log,
   /// not the SP/device trace) and is selected on the command line with:  -a Txn  (or -a *).
   /// </summary>
   [Export(typeof(IView))]
   public class TxnView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      TxnView() : base(ParseType.AP, "TxnView") { }

      /// <summary>
      /// Creates a TxnTable instance.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>TxnTable</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         TxnTable txnTable = new TxnTable(ctx, viewName);
         txnTable.ReadXmlFile();
         return txnTable;
      }
   }
}
