using Contract;
using Impl;
using System.ComponentModel.Composition;

namespace CoinView
{
   /// <summary>
   /// Coin dispenser view. The coin dispenser is a physically separate device from the cash
   /// dispenser, but on this hardware both are exposed through the CDM XFS service class (device 3)
   /// and are distinguished only by the logical name "CoinDispenser" vs "CashDispenser". SPLine.Factory
   /// classifies the CoinDispenser lines with the WFS_*_COIN_* types; this view renders them into their
   /// own worksheets so coin is never mixed into the cash CDM sheets.
   ///
   /// ParseType.SP, so it is produced by the same -s selection as CDM (see Options.RunView).
   /// </summary>
   [Export(typeof(IView))]
   public class CoinView : BaseView, IView
   {
      /// <summary>
      /// Constructor
      /// </summary>
      CoinView() : base(ParseType.SP, "CoinView") { }

      /// <summary>
      /// Creates a Coin Table instance.
      /// </summary>
      /// <param name="ctx">Context for the command.</param>
      /// <returns>new Coin table</returns>
      protected override BaseTable CreateTableInstance(IContext ctx)
      {
         CoinTable coinTable = new CoinTable(ctx, viewName);
         coinTable.ReadXmlFile();
         return coinTable;
      }
   }
}
